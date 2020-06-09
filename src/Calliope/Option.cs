using System;
using System.Collections;
using System.Collections.Generic;

namespace Calliope
{
    public readonly struct Option<TValue> : 
        IEquatable<Option<TValue>>,
        IComparable<Option<TValue>>,
        IStructuralEquatable,
        IComparable, 
        IStructuralComparable
    {
        public static Option<TValue> None => default;

        public static Option<TValue> Some(TValue value) => new Option<TValue>(value);

        private readonly bool _isSome;
        private readonly TValue _value;

        private Option(TValue value)
        {
            _isSome = (_value = value) is { };
        }

        //public bool IsSome([MaybeNullWhen(false)]out TValue value)
        public bool IsSome(out TValue value)
        {
            value = _value;
            return _isSome;
        }

        public bool IsSome() => _isSome;

        public static implicit operator Option<TValue>(TValue source) => new Option<TValue>(source);

        public TValue Unwrap() => _value;

        public override bool Equals(object obj) => obj is Option<TValue> other && Equals(other);

        public bool Equals(Option<TValue> other) => Equals(other, EqualityComparer<TValue>.Default);

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) =>
            other is Option<TValue> otherOption && Equals(otherOption, (x, y) => comparer.Equals(x, y));

        public bool Equals(Option<TValue> other, IEqualityComparer<TValue> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return Equals(other, comparer.Equals);
        }

        private bool Equals(Option<TValue> other, Func<TValue, TValue, bool> equals) =>
            AreBothNone(other) || AreBothSome(other) && equals(_value, other._value);

        private bool AreBothNone(Option<TValue> other) => !(_isSome || other._isSome);

        private bool AreBothSome(Option<TValue> other) => _isSome && other._isSome;

        public override int GetHashCode() => GetHashCode(EqualityComparer<TValue>.Default);

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) => GetHashCode(obj => comparer.GetHashCode(obj));

        public int GetHashCode(IEqualityComparer<TValue> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return GetHashCode(comparer.GetHashCode);
        }

        private int GetHashCode(Func<TValue, int> getHashCode) => _isSome ? getHashCode(_value) : int.MinValue;

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is Option<TValue> other)) throw new ArgumentException(nameof(obj));
            return CompareTo(other);
        }

        public int CompareTo(Option<TValue> other) => CompareTo(other, Comparer<TValue>.Default);

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (!(other is Option<TValue> otherOption)) throw new ArgumentException(nameof(other));
            return CompareTo(otherOption, (x, y) => comparer.Compare(x, y));
        }

        public int CompareTo(Option<TValue> other, IComparer<TValue> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return CompareTo(other, comparer.Compare);
        }

        private int CompareTo(Option<TValue> other, Func<TValue, TValue, int> compare)
        {
            if (!_isSome) return other._isSome ? -1 : 0;
            return !other._isSome ? 1 : compare(_value, other._value);
        }

        public static bool operator ==(Option<TValue> a, Option<TValue> b) => a.Equals(b);
        public static bool operator !=(Option<TValue> a, Option<TValue> b) => !(a == b);
        public static bool operator <(Option<TValue> a, Option<TValue> b) => a.CompareTo(b) < 0;
        public static bool operator >(Option<TValue> a, Option<TValue> b) => a.CompareTo(b) > 0;
        public static bool operator <=(Option<TValue> a, Option<TValue> b) => a.CompareTo(b) <= 0;
        public static bool operator >=(Option<TValue> a, Option<TValue> b) => a.CompareTo(b) >= 0;
        
        public override string ToString() => _isSome ? $"Some({_value})" : "None";
    }

    public static class Optional
    {
        public static Option<TValue> None<TValue>() =>
            Option<TValue>.None;

        public static Option<TValue> Some<TValue>(TValue value) =>
            Option<TValue>.Some(value);

        public static Option<TValue> Create<TValue>(TValue value) where TValue : class =>
            value is { } some ? Some(some) : None<TValue>();

        public static Option<TValue> Create<TValue>(TValue? value) where TValue : struct =>
            value.HasValue ? Some(value.Value) : None<TValue>();
    }
}