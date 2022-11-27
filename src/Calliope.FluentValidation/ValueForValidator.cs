using System.Collections.Generic;
using Calliope.Validation;
using FluentValidation.Validators;

namespace Calliope.FluentValidation
{
    public class ValueForValidator<TSource> : PropertyValidator
    {
        private readonly IEnumerable<ValidationRule<TSource>> _rules;

        public ValueForValidator(IEnumerable<ValidationRule<TSource>> rules) 
            : base("{ValidationMessage}")
        {
            _rules = rules;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var item = (TSource) context.PropertyValue;
            var validator = new Validator<TSource>();
            var result = validator.Validate(item, _rules);

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