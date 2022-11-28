#nullable enable
using System;
using System.Threading.Tasks;

namespace Calliope;

public readonly struct Empty : IEquatable<Empty>, IComparable<Empty>, IComparable
{
    // ReSharper disable once InconsistentNaming
    private static readonly Empty _value = new();

    public static ref readonly Empty Value => ref _value;
    public static Task<Empty> Task { get; } = System.Threading.Tasks.Task.FromResult(_value);

    public int CompareTo(Empty other) => 0;
    int IComparable.CompareTo(object? obj) => 0;

    public override int GetHashCode() => 0;
    public bool Equals(Empty other) => true;
    public override bool Equals(object? obj) => obj is Empty;
    public static bool operator ==(Empty _, Empty __) => true;
    public static bool operator !=(Empty _, Empty __) => false;
    public override string ToString() => "()";
}