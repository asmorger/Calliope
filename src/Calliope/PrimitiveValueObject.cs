using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Calliope.Validation;

namespace Calliope 
{
    public interface IPrimitiveValueObject<out TEntity> : IValueObject
    {
        TEntity Value { get; }
    }
    
    public abstract class PrimitiveValueObject<TInput, TOutput, TValidator> : OldValueObject<TOutput>, IPrimitiveValueObject<TInput>
        where TOutput : PrimitiveValueObject<TInput, TOutput, TValidator>
        where TValidator : IValidator<TInput>, new()
    {
        private static readonly Func<TOutput> Factory;

        static PrimitiveValueObject()
        {
            var ctor = typeof(TOutput).GetTypeInfo().DeclaredConstructors.First();

            var arguments = new Expression[0];
            var creationExpression = Expression.New(ctor, arguments);
            var creationLambda = Expression.Lambda(typeof(Func<TOutput>), creationExpression);

            Factory = (Func<TOutput>) creationLambda.Compile();
        }

        /// <summary>
        /// The current value
        /// </summary>
        // default value suppresses the C# 8 nullable warning
        public TInput Value { get; private set; } = default!;

        /// <summary>
        /// Factory pattern method that validates the input and either throws a validation exception or returns the value. 
        /// </summary>
        public static TOutput Create(TInput source)
        {
            var validationResult = Validator.Validate(source);

            if (validationResult.IsError(out var validationException))
                throw new Exception(validationException.ToErrorMessage());

            var output = Factory();
            output.Value = output.Transform(source);
            return output;
        }

        /// <summary>
        /// Factory method that returns an <see cref="Optional"/> instance based on if the validation was successful.
        /// </summary>
        public static Option<TOutput> Parse(TInput source)
        {
            Option<TOutput> Success(bool _)
            {
                var output = Factory();
                output.Value = output.Transform(source);
                return Optional.Some(output);
            }

            Option<TOutput> Failure(DomainError failure) => Option<TOutput>.None;

            var validationResult = Validator.Validate(source);
            return validationResult.Match(Success, Failure);
        }

        /// <summary>
        /// Casts the current Value type to that of it's property
        /// </summary>
        public static implicit operator TInput(PrimitiveValueObject<TInput, TOutput, TValidator> primitiveValueObject) =>
            primitiveValueObject.Value;

        /// <summary>
        /// Creates a new <see cref="TValidator"/> instance for this type.
        /// </summary>
        public static TValidator Validator => new TValidator();

        /// Overrides ToString so that we get useful DebuggerDisplay.
        /// Takes this route over a DebuggerDisplayAttribute because they don't follow derrived types.
        /// <inheritdoc />
        public override string ToString() => Value.ToString();

        protected virtual TInput Transform(TInput input) => input;
    }
}