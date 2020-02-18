using System;
using System.Collections.Generic;

namespace Calliope.Monads
{
    public sealed class Some<T> : Option<T>, IEquatable<Some<T>>
    {
        public Some(T value)
        {
            Content = value;
        }

        public T Content { get; }

        private string ContentToString => Content?.ToString() ?? "<null>";

        public bool Equals(Some<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Content, other.Content);
        }

        public static implicit operator T(Some<T> some) => some.Content;

        public static implicit operator Some<T>(T value) => new Some<T>(value);

        public override Option<TResult> Select<TResult>(Func<T, TResult> map) => map(Content);

        public override Option<TResult> SelectOptional<TResult>(Func<T, Option<TResult>> map) => map(Content);

        public override T Where(T whenNone) => Content;

        public override T Where(Func<T> whenNone) => Content;

        public override string ToString() => $"Some({ContentToString})";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Some<T> && Equals((Some<T>) obj);
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Content);

        public static bool operator ==(Some<T> a, Some<T> b) =>
            a is null && b is null ||
            !(a is null) && a.Equals(b);

        public static bool operator !=(Some<T> a, Some<T> b) => !(a == b);
    }
}