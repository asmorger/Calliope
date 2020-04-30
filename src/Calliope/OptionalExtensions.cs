using System;

// ReSharper disable InconsistentNaming

namespace Calliope
{
    public static class OptionalExtensions
    {
        public static Optional<T> Some<T>(this T value) => Optional.Some(value);

        public static Optional<T> AsOptional<T>(this T value) where T : class => Optional.Create(value);

        public static Optional<T> AsOptional<T>(this T? value) where T : struct => Optional.Create(value);

        public static U Match<T, U>(this Optional<T> optional, Func<T, U> onIsSome, Func<U> onIsNone)
        {
            if (onIsSome is null) throw new ArgumentNullException(nameof(onIsSome));
            if (onIsNone is null) throw new ArgumentNullException(nameof(onIsNone));

            return optional.IsSome(out var value) ? onIsSome(value) : onIsNone();
        }

        public static bool IsSome<T>(this Optional<T> optional) =>
            optional.Match(
                _ => true,
                () => false);

        public static bool IsNone<T>(this Optional<T> optional) => !optional.IsSome();

        public static Optional<U> Bind<T, U>(this Optional<T> optional, Func<T, Optional<U>> binder)
        {
            if (binder is null) throw new ArgumentNullException(nameof(binder));

            return optional.Match(
                binder,
                () => Optional<U>.None);
        }

        public static Optional<U> Map<T, U>(this Optional<T> optional, Func<T, U> mapper)
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));
            return optional.Bind(value => mapper(value).Some());
        }

        public static Optional<U> MapNullable<T, U>(this Optional<T> optional, Func<T, U> mapper)
            where U : class
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));
            return optional.Bind(value => mapper(value).AsOptional());
        }

        public static Optional<U> MapNullable<T, U>(this Optional<T> optional, Func<T, U?> mapper)
            where U : struct
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));
            return optional.Bind(value => mapper(value).AsOptional());
        }

        public static T Unwrap<T>(this Optional<T> optional, T defaultValue) =>
            optional.Match(
                value => value,
                () => defaultValue);

        public static T Unwrap<T>(this Optional<T> optional, Func<T> defaultValue) =>
            optional.Match(value => value, defaultValue);

        public static Optional<T> Filter<T>(this Optional<T> optional, Predicate<T> predicate) =>
            optional.Bind(value => predicate(value) ? value.Some() : Optional<T>.None);
    }
}