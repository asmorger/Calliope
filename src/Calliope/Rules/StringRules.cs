namespace Calliope.Rules;

public static class StringRules
{
    public static ValidationRule<string?> Required => new(s => !string.IsNullOrWhiteSpace(s), _ => $"{Placeholder.TypeName} must have a value");
    public static ValidationRule<string> ShorterThan(int length) => new(s => s.Length < length, _ => $"{Placeholder.TypeName} must be shorter than {length} characters.");
    public static ValidationRule<string> LongerThan(int length) => new(s => s.Length > length, _ => $"{Placeholder.TypeName} must be longer than {length} characters.");
}