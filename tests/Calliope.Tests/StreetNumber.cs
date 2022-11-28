using System.Collections.Generic;
using Calliope.Rules;

namespace Calliope.Tests;

public record StreetNumber(int Number) : IValidatable<int>
{
    public static IEnumerable<ValidationRule<int>> GetValidationRules() => new[]
    {
        IntRules.GreaterThanZero
    };
}