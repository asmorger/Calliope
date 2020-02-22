using System;
using System.Diagnostics.Contracts;

namespace Calliope.Monads
{
    public abstract class Option<T>
    {
        public abstract bool HasValue { get; }
        public static implicit operator Option<T>(T value) => new Some<T>(value);
        
        public static implicit operator Option<T>(None none) => new None<T>();
        
        public abstract Option<TResult> Select<TResult>(Func<T, TResult> map);
        
        public abstract Option<TResult> SelectOptional<TResult>(Func<T, Option<TResult>> map);
        
        public abstract T Where(T whenNone);
        
        public abstract T Where(Func<T> whenNone);

        public Option<TNew> OfType<TNew>() 
            where TNew : class
        {
            if (this is Some<T> some && typeof(TNew).IsAssignableFrom(typeof(T)))
            {
                var typedValue = some.Value as TNew;
                return new Some<TNew>(typedValue!);
            }

            return None.Value;
        }

        public void Match(Action<T> onSome, Action onNone)
        {
            if (this is Some<T> some)
            {
                onSome(some.Value);
            }
            else
            {
                onNone();
            }
        }
        
        [Pure]
        public T Match(Func<T, T> onSome, Func<T> onNone)
        {
            if (this is Some<T> some)
            {
                return onSome(some.Value);
            }
            
            return onNone();
        }

        // I want the lazy initialization of the value so we don't waste execution cycles if we don't need them
        [Pure]
        public T SomeOrValue(Func<T> value)
        {
            if (this is Some<T> some)
            {
                return some.Value;
            }

            return value();
        }
    }
}