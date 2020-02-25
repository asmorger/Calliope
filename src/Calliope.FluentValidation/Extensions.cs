using FluentValidation;

namespace Calliope.FluentValidation
{
    public static class Extensions
    {
        public static IRuleBuilderOptions<TObject, TProperty> ForDomainValue<TObject, TProperty>(
            this IRuleBuilder<TObject, TProperty> rule, Validation.IValidator<TProperty> validator) 
            => rule.SetValidator(new PrimitiveValueValidator<TProperty>(validator));
    }
}