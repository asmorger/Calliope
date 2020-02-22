using System;
using System.Collections.Generic;

namespace Calliope.Monads
{
    public sealed class Some<T> : Option<T>, IEquatable<Some<T>>
    {
        public Some(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public override bool HasValue => true;

        private string ValueToString => Value?.ToString() ?? "<null>";

        public bool Equals(Some<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public static implicit operator T(Some<T> some) => some.Value;

        public static implicit operator Some<T>(T value) => new Some<T>(value);

        public override Option<TResult> Select<TResult>(Func<T, TResult> map) => map(Value);

        public override Option<TResult> SelectOptional<TResult>(Func<T, Option<TResult>> map) => map(Value);

        public override T Where(T whenNone) => Value;

        public override T Where(Func<T> whenNone) => Value;

        public override string ToString() => $"Some({ValueToString})";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Some<T> && Equals((Some<T>) obj);
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

        public static bool operator ==(Some<T> a, Some<T> b) =>
            a is null && b is null ||
            !(a is null) && a.Equals(b);

        public static bool operator !=(Some<T> a, Some<T> b) => !(a == b);
    }
}