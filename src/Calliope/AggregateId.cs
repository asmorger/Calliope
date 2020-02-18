using System;

namespace Calliope
{
    public abstract class AggregateId<T> : Value
        where T : AggregateRoot
    {
        protected AggregateId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(
                    nameof(value), 
                    "The Id cannot be empty");
            
            Value = value;
        }

        public Guid Value { get; }
        
        public static implicit operator Guid(AggregateId<T> self) => self.Value;

        public override string ToString() => Value.ToString();
    }
}