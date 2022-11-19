using Calliope.Validation;
using Calliope.Validators;
using FluentValidation.Validators;

namespace Calliope.FluentValidation
{
    public class ValueForValidator<TSource> : PropertyValidator
    {
        private readonly IValidator<TSource> _validator;

        public ValueForValidator(IValidator<TSource> validator) : base("{ValidationMessage}")
        {
            _validator = validator;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var item = (TSource) context.PropertyValue;
            var result = _validator.Validate(item);

            if (result.IsError(out var domainException))
            {
                if (domainException is ValidationFailed validationFailed)
                    foreach (var message in validationFailed.Messages)
                        context.MessageFormatter.AppendArgument("ValidationMessage",
                            message.Replace(Placeholder.TypeName, "{PropertyName}"));

                return false;
            }

            return true;
        }
    }
}