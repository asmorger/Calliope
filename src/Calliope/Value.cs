using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Calliope.Validation;

namespace Calliope
{
    public abstract class Value<T>
    {
        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            var valueObject = (Value<T>) obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator ==(Value<T> a, Value<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Value<T> a, Value<T> b) => !(a == b);
    }
    
    public abstract class Value<TInput, TOutput, TValidator> : Value<TOutput>
        where TOutput : Value<TInput, TOutput, TValidator>, new()
        where TValidator : IValidator<TInput>, new()
    {
        private static readonly Func<TOutput> Factory;
        
        static Value()
        {
            var ctor = typeof(TOutput).GetTypeInfo().DeclaredConstructors.First();

            var arguments = new Expression[0];
            var creationExpression = Expression.New(ctor, arguments);
            var creationLambda = Expression.Lambda(typeof(Func<TOutput>), creationExpression);
            
            Factory = (Func<TOutput>)creationLambda.Compile();
        }

        /// <summary>
        /// The current value
        /// </summary>
        // default value suppresses the C# 8 nullable warning
        public TInput Current { get; private set; } = default!;

        /// <inheritdoc />
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Current;
        }

        /// <summary>
        /// Factory pattern method that validates the input and either throws a validation exception or returns the wrapped value. 
        /// </summary>
        public static TOutput Create(TInput source)
        {
            var output = Factory();
            var validationResult = Validator.Validate(source);
            
            // this will kick it out if it's not valid
            validationResult.DoRight(err => throw new ValidationFailedException(typeof(TOutput).Name, err));
            
            var result = validationResult.MatchLeft();
            
            
            output.Current = result.Unwrap().GoodValue;

            return output;
        }
        
        /// <summary>
        /// Casts the current Value type to that of it's property
        /// </summary>
        public static implicit operator TInput(Value<TInput, TOutput, TValidator> value) => value.Current;

        /// <summary>
        /// Creates a new <see cref="TValidator"/> instance for this type.
        /// </summary>
        public static TValidator Validator => new TValidator();

        /// Overrides ToString so that we get useful DebuggerDisplay.
        /// Takes this route over a DebuggerDisplayAttribute because they don't follow derrived types.
        /// <inheritdoc />
        public override string ToString() => $"Value: {Current}";
    }
}