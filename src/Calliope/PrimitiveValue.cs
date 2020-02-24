using System.Collections.Generic;
using System.Diagnostics;
using Calliope.Validation;

namespace Calliope
{
    [DebuggerDisplay("Value")]
    public abstract class PrimitiveValue<TInput, TOutput> : Value<TOutput>
    {
        protected PrimitiveValue(TInput value)
        {
            Value = value;
        }

        public TInput Value { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static TOutput Create(TInput source, IValidator<TInput, TOutput> validator)
        {
            var validationResult = validator.Validate(source);
            
            // this will kick it out if it's not valid
            validationResult.DoRight(err => throw new ValidationFailedException(err));
            
            var result = validationResult.MatchLeft();
            return result.ValueOrThrow();
        }
    }
}