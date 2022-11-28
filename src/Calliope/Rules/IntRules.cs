namespace Calliope.Rules;

public static class IntRules
{
    public static ValidationRule<int?> Required =>
        new(i => i is not null, _ => $"{Placeholder.TypeName} must not be null");

    public static ValidationRule<int> GreaterThanZero =>
        new(i => i > 0, _ => $"{Placeholder.TypeName} must be greater than 0");

    public static ValidationRule<int> LessThanZero =>
        new(i => i < 0, _ => $"{Placeholder.TypeName} must be less than 0");
}