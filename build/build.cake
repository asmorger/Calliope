var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

var nugetApiKey = HasArgument("NugetApiKey")
    ? Argument<string>("NugetApiKey")
    : EnvironmentVariable("CalliopeNugetKey");

var root = MakeAbsolute(Directory("../"));
var src = root + "/src";
var sln = src + "/Calliope.sln";
var dist = root + "/dist";
var buildProps = src + "/Directory.build.props";

var version = "1.0.0-alpha";

Information("========================================");
Information("Configuration");

Information($"Running target '{target}' in configuration '{configuration}'");
Information($"Building solution '{sln}'");
Information($"Publishing to '{dist}'");
Information($"Source version is '{XmlPeek(buildProps, "//Version")}' with suffix '{XmlPeek(buildProps, "//VersionSuffix")}'");

Information("========================================");

Task("Prerequisites")
    .Does(() => {
        if(string.IsNullOrEmpty(nugetApiKey))
        {
            Information("Unable to publish Nuget package.  Missing Nuget API Key.");
            throw new Exception("Nuget API key missing.");
        }

        Information("All prerequisites have been met.");
    });

// Deletes the contents of the publish folder if it contains previous build artifacts
Task("Clean")
    .Does(() => 
    {
        CleanDirectory(dist);
    });

// Run dotnet restore to restore all package references.
Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore(sln);
    });

Task("Version")
    .Does(() => {
        version = IncrementVersion();

        var fileSuffix = XmlPeek(buildProps, "//VersionSuffix");

        if(!string.IsNullOrEmpty(fileSuffix))
        {
            version = $"{version}-{fileSuffix}";
        }
    });

// Build using the build configuration specified as an argument.  Disables build warning about file versions not being semver.
 Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Version")
    .Does(() =>
    {
        DotNetCoreBuild(sln,
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append($"--no-restore -p:Version={version}")
            });
    });

// Finds all projects, iterates over them, and publishes them to an individual folder
Task("Package")
    .IsDependentOn("Build")
    .Does(() => {
        var settings = new DotNetCorePackSettings
        {
            Configuration = "Release",
            OutputDirectory = dist,
            ArgumentCustomization = args => args.Append($"--no-restore --no-build -p:PackageVersion={version}")
        };
        
        DotNetCorePack(sln, settings);
    });

Task("Publish")
    .IsDependentOn("Package")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings 
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = nugetApiKey
        };

        var packages = GetFiles($"{dist}/**/*.nupkg");
        foreach(var package in packages) 
        {
            Information($"Publishing \"{package}\".");
            DotNetCoreNuGetPush(package.FullPath, settings);
        }    
    });
    
Task("Default")
    .IsDependentOn("Prerequisites")
    .IsDependentOn("Publish");

RunTarget(target);

private string IncrementVersion() => UpdateVersion(1);

private string DecrementVersion() => UpdateVersion(-1);

private string UpdateVersion(int increment)
{
    var fileVersion = XmlPeek(buildProps, "//Version");
    var currentVersion = new Version(fileVersion);

    var newVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build + increment);
    version = newVersion.ToString();

    XmlPoke(buildProps, "//Version", version);

    return version;
}