using FluentValidation;

namespace Calliope.FluentValidation
{
    public static class Extensions
    {
        public static IRuleBuilderOptions<T, TProperty> ValidFor<T, TProperty>
            (this IRuleBuilder<T, TProperty> ruleBuilder, Calliope.Validation.IValidator<TProperty> validator) =>
            ruleBuilder.SetValidator(new ValueForValidator<TProperty>(validator));
    }
}