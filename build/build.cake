#tool "nuget:?package=GitVersion.CommandLine"

// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

var root = MakeAbsolute(Directory("../"));
var src = root + "/src";
var sln = src + "/Calliope.sln";
var dist = root + "/dist";

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";

Information("========================================");
Information("Configuration");

Information($"Running target '{target}' in configuration '{configuration}'");
Information($"Building solution '{sln}'");
Information($"Publishing to '{dist}'");

Information("========================================");

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

// Build using the build configuration specified as an argument.  Disables build warning about file versions not being semver.
 Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetCoreBuild(sln,
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append("--no-restore")
            });
    });

// Finds all projects, iterates over them, and publishes them to an individual folder
Task("Publish")
    .IsDependentOn("Build")
    .Does(() => {
        var settings = new DotNetCorePublishSettings
        {
            Configuration = "Release",
            OutputDirectory = dist
        };
        
        DotNetCorePublish(sln, settings);
    });
    
Task("Default")
    .IsDependentOn("Publish");

RunTarget(target);