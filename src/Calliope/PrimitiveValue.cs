using System.Collections.Generic;

namespace Calliope
{
    public abstract class PrimitiveValue<T> : Value
    {
        protected PrimitiveValue(T value)
        {
            Value = value;
        }

        public T Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            if (Value is null) yield return default(T)!;
            else yield return Value!;
        }
    }
}