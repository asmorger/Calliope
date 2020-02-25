using System;
using System.Collections.Generic;
using System.Diagnostics;
using Calliope.Validation;

namespace Calliope
{
    [DebuggerDisplay("Value")]
    public abstract class PrimitiveValue<TInput, TOutput, TValidator> : Value<TOutput>
        
        where TValidator : IValidator<TInput>, new()
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

        // makes this protected so that we can provide a public wrapper that defines the required validator
        protected static TOutput Create(TInput source, Func<TInput, TOutput> factory)
        {
            var validationResult = GetValidator().Validate(source);
            
            // this will kick it out if it's not valid
            validationResult.DoRight(err => throw new ValidationFailedException(typeof(TOutput).Name, err));
            
            var result = validationResult.MatchLeft();
            return factory(result.ValueOrThrow().GoodValue);
        }

        public static IValidator<TInput> GetValidator() => new TValidator();

    }
}