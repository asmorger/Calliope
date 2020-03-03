using System;
using System.Collections;
using System.Collections.Generic;

namespace Calliope
{
    public struct Optional<TValue> : IEquatable<Optional<TValue>>, IStructuralEquatable, IComparable<Optional<TValue>>,
        IComparable, IStructuralComparable
        where TValue : notnull
    {
        public static Optional<TValue> None => default;

        public static Optional<TValue> Some(TValue value) => new Optional<TValue>(value);

        private readonly bool _isSome;
        private readonly TValue _value;

        private Optional(TValue value)
        {
            _isSome = (_value = value) is { };
        }


        //public bool IsSome([MaybeNullWhen(false)]out TValue value)
        public readonly bool IsSome(out TValue value)
        {
            value = _value;
            return _isSome;
        }

        public static implicit operator Optional<TValue>(TValue source) => new Optional<TValue>(source);

        public readonly TValue Unwrap() => _value;

        public override bool Equals(object obj) =>
            obj is Optional<TValue> other && Equals(other);

        public bool Equals(Optional<TValue> other) =>
            Equals(other, EqualityComparer<TValue>.Default);

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) =>
            other is Optional<TValue> otherOption && Equals(otherOption, (x, y) => comparer.Equals(x, y));

        public bool Equals(Optional<TValue> other, IEqualityComparer<TValue> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return Equals(other, comparer.Equals);
        }

        private bool Equals(Optional<TValue> other, Func<TValue, TValue, bool> equals) =>
            AreBothNone(other) ||
            AreBothSome(other) && equals(_value, other._value);

        private bool AreBothNone(Optional<TValue> other) =>
            !(_isSome || other._isSome);

        private bool AreBothSome(Optional<TValue> other) =>
            _isSome && other._isSome;

        public override int GetHashCode() =>
            GetHashCode(EqualityComparer<TValue>.Default);

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) =>
            GetHashCode(obj => comparer.GetHashCode(obj));

        public int GetHashCode(IEqualityComparer<TValue> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return GetHashCode(comparer.GetHashCode);
        }

        private int GetHashCode(Func<TValue, int> getHashCode) =>
            _isSome ? getHashCode(_value) : int.MinValue;

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is Optional<TValue> other)) throw new ArgumentException(nameof(obj));

            return CompareTo(other);
        }

        public int CompareTo(Optional<TValue> other) =>
            CompareTo(other, Comparer<TValue>.Default);

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (!(other is Optional<TValue> otherOption)) throw new ArgumentException(nameof(other));

            return CompareTo(otherOption, (x, y) => comparer.Compare(x, y));
        }

        public int CompareTo(Optional<TValue> other, IComparer<TValue> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            return CompareTo(other, comparer.Compare);
        }

        private int CompareTo(Optional<TValue> other, Func<TValue, TValue, int> compare)
        {
            if (!_isSome) return other._isSome ? -1 : 0;
            return !other._isSome ? 1 : compare(_value, other._value);
        }

        public static bool operator ==(Optional<TValue> a, Optional<TValue> b) =>
            a.Equals(b);

        public static bool operator !=(Optional<TValue> a, Optional<TValue> b) =>
            !(a == b);

        public static bool operator <(Optional<TValue> a, Optional<TValue> b) =>
            a.CompareTo(b) < 0;

        public static bool operator >(Optional<TValue> a, Optional<TValue> b) =>
            a.CompareTo(b) > 0;

        public static bool operator <=(Optional<TValue> a, Optional<TValue> b) =>
            a.CompareTo(b) <= 0;

        public static bool operator >=(Optional<TValue> a, Optional<TValue> b) =>
            a.CompareTo(b) >= 0;

        public override string ToString() => _isSome ? $"Some({_value})" : "None";
    }


    public static class Optional
    {
        public static Optional<TValue> None<TValue>() where TValue : notnull =>
            Optional<TValue>.None;

        public static Optional<TValue> Some<TValue>(TValue value) where TValue : notnull =>
            Optional<TValue>.Some(value);

        public static Optional<TValue> Create<TValue>(TValue? value) where TValue : class =>
            value is { } some ? Some(some) : None<TValue>();

        public static Optional<TValue> Create<TValue>(TValue? value) where TValue : struct =>
            value.HasValue ? Some(value.Value) : None<TValue>();
    }
}