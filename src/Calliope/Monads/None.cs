using System;

namespace Calliope.Monads
{
    public sealed class None<T> : Option<T>, IEquatable<None<T>>, IEquatable<None>
    {
        public override bool HasValue => false;
        public bool Equals(None<T> other) => true;

        public bool Equals(None other) => true;
        public override Option<TResult> Select<TResult>(Func<T, TResult> map) => None.Value;

        public override Option<TResult> SelectOptional<TResult>(Func<T, Option<TResult>> map) => None.Value;

        public override T Where(T whenNone) => whenNone;

        public override T Where(Func<T> whenNone) => whenNone();

        public override bool Equals(object obj) => !(obj is null) && (obj is None<T> || obj is None);

        public override int GetHashCode() => 0;

        public static bool operator ==(None<T> a, None<T> b) =>
            a is null && b is null ||
            !(a is null) && a.Equals(b);

        public static bool operator !=(None<T> a, None<T> b) => !(a == b);

        public override string ToString() => "None";
    }

    public sealed class None : IEquatable<None>
    {
        private None() { }
        public static None Value { get; } = new None();

        public bool Equals(None other) => true;

        public override string ToString() => "None";

        public override bool Equals(object obj) =>
            !(obj is null) && (obj is None || IsGenericNone(obj.GetType()));

        private bool IsGenericNone(Type type) =>
            type.GenericTypeArguments.Length == 1 &&
            typeof(None<>).MakeGenericType(type.GenericTypeArguments[0]) == type;

        public override int GetHashCode() => 0;
    }
}