using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Calliope.Validation;

namespace Calliope
{
    [DebuggerDisplay("Value")]
    public abstract class PrimitiveValue<TInput, TOutput, TValidator> : Value<TOutput>
        where TOutput : PrimitiveValue<TInput, TOutput, TValidator>, new()
        where TValidator : IValidator<TInput>, new()
    {
        private static readonly Func<TOutput> Factory;
        
        static PrimitiveValue()
        {
            var ctor = typeof(TOutput)
                .GetTypeInfo()
                .DeclaredConstructors
                .First();

            var argsExp = new Expression[0];
            var newExp = Expression.New(ctor, argsExp);
            var lambda = Expression.Lambda(typeof(Func<TOutput>), newExp);
            
            Factory = (Func<TOutput>)lambda.Compile();
        }

        // default value suppresses the C# 8 nullable warning
        public TInput Value { get; private set; } = default!;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }


        // makes this protected so that we can provide a public wrapper that defines the required validator
        public static TOutput Create(TInput source)
        {
            var output = Factory();
            var validationResult = Validator.Validate(source);
            
            // this will kick it out if it's not valid
            validationResult.DoRight(err => throw new ValidationFailedException(typeof(TOutput).Name, err));
            
            var result = validationResult.MatchLeft();
            
            
            output.Value = result.ValueOrThrow().GoodValue;

            return output;
        }
        
        public static implicit operator TInput(PrimitiveValue<TInput, TOutput, TValidator> value) => value.Value;

        public static TValidator Validator => new TValidator();
        
    }
}