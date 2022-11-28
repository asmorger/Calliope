using System.Collections.Generic;
using System.Reflection;
using FluentValidation;

namespace Calliope.FluentValidation;

public static class Extensions
{
    public static IRuleBuilderOptions<T, TProperty> ValidFor<T, TProperty, TValueObject>
        (this IRuleBuilder<T, TProperty> ruleBuilder)
        where TValueObject : IValidatable<TProperty>
    {
        var method = typeof(TValueObject).GetMethod(nameof(IValidatable<TProperty>.GetValidationRules),
            BindingFlags.Public | BindingFlags.Static);
        var rules = (IEnumerable<ValidationRule<TProperty>>) method!.Invoke(null, null)!;

        return ruleBuilder.SetValidator(new ValueForValidator<TProperty>(rules));
    }
}