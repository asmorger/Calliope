namespace Calliope.Validation;

public record Required() : ValidationRule<int?>(i => i is not null, i => $"{i} must not be null");
public record GreaterThanZero() : ValidationRule<int>(i => i > 0, i => $"{i} must be greater than 0");
public record LessThanZero() : ValidationRule<int>(i => i < 0, i => $"{i} must be less than 0");