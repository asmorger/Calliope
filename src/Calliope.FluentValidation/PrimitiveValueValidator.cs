using Calliope.Validation;
using Calliope.Validators;
using FluentValidation;
using FluentValidation.Validators;

namespace Calliope.FluentValidation
{
    public class PrimitiveValueValidator<T> : AbstractValidator<T>
    {
        public PrimitiveValueValidator(Validation.IValidator<T> validator)
        {
            RuleFor(x => x).Custom((item, context) =>
            {
                var result = validator.Validate(item);

                var errors = result.MatchRight();

                if (errors.IsSome())
                {
                    HandleErrorState(errors.ValueOrThrow(), context);
                }
            });
        }

        private static void HandleErrorState(ValidationFailures result, CustomContext context)
        {
            foreach (var message in result.ValidationMessages)
            {
                // match FluentValidation placeholder standards
                context.AddFailure(message.Replace(Placeholder.TypeName, "{PropertyName}"));
            }
        }
    }
}