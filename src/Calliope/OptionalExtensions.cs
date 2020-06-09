using System;

// ReSharper disable InconsistentNaming

namespace Calliope
{
    public static class OptionalExtensions
    {
        public static Option<T> Some<T>(this T value) => Optional.Some(value);

        public static Option<T> AsOptional<T>(this T value) where T : class => Optional.Create(value);

        public static Option<T> AsOptional<T>(this T? value) where T : struct => Optional.Create(value);

        public static U Match<T, U>(this Option<T> option, Func<T, U> onIsSome, Func<U> onIsNone)
        {
            if (onIsSome is null) throw new ArgumentNullException(nameof(onIsSome));
            if (onIsNone is null) throw new ArgumentNullException(nameof(onIsNone));

            return option.IsSome(out var value) ? onIsSome(value) : onIsNone();
        }

        public static bool IsSome<T>(this Option<T> option) =>
            option.Match(
                _ => true,
                () => false);

        public static bool IsNone<T>(this Option<T> option) => !option.IsSome();

        public static Option<U> Bind<T, U>(this Option<T> option, Func<T, Option<U>> binder)
        {
            if (binder is null) throw new ArgumentNullException(nameof(binder));

            return option.Match(
                binder,
                () => Option<U>.None);
        }

        public static Option<U> Map<T, U>(this Option<T> option, Func<T, U> mapper)
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));
            return option.Bind(value => mapper(value).Some());
        }

        public static Option<U> MapNullable<T, U>(this Option<T> option, Func<T, U> mapper)
            where U : class
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));
            return option.Bind(value => mapper(value).AsOptional());
        }

        public static Option<U> MapNullable<T, U>(this Option<T> option, Func<T, U?> mapper)
            where U : struct
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));
            return option.Bind(value => mapper(value).AsOptional());
        }

        public static T Unwrap<T>(this Option<T> option, T defaultValue) =>
            option.Match(
                value => value,
                () => defaultValue);

        public static T Unwrap<T>(this Option<T> option, Func<T> defaultValue) =>
            option.Match(value => value, defaultValue);

        public static Option<T> Filter<T>(this Option<T> option, Predicate<T> predicate) =>
            option.Bind(value => predicate(value) ? value.Some() : Option<T>.None);
    }
}