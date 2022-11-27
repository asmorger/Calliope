using System.Collections.Generic;

namespace Calliope.Validation;

public interface IValidatable<TSource>
{
    static abstract IEnumerable<ValidationRule<TSource>> GetValidationRules();
}