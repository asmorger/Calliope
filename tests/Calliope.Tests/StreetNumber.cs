using System.Collections.Generic;
using Calliope.Validation;
using Calliope.Validators;

namespace Calliope.Tests;

public record StreetNumber(int Number) : IValidatable<int>
{
    public static IEnumerable<ValidationRule<int>> GetValidationRules() => new[]
    {
        new GreaterThanZero()
    };
}