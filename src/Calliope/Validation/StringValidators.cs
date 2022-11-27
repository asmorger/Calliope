// ReSharper disable NotAccessedPositionalProperty.Global
#nullable enable
namespace Calliope.Validation;

public record Required() : ValidationRule<string?>(s => !string.IsNullOrWhiteSpace(s), s => $"{s} must have a value");public record GreaterThanZero() : ValidationRule<int>(i => i > 0, i => $"{i} must be greater than 0");
public record ShorterThan(int Length) : ValidationRule<string>(s => s.Length < Length, s => $"{s} must be shorter than {Length} characters.");
public record LongerThan(int Length) : ValidationRule<string>(s => s.Length > Length, s => $"{s} must be longer than {Length} characters.");
