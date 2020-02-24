using System;
using System.Collections.Generic;
using Calliope.Monads;
using Calliope.Validation;

namespace Calliope.Validators
{
    public class NotEmptyStringValidator<TOutput> : Validator<string, TOutput>
    {
        private readonly Option<int> _minimumLength;
        private readonly Option<int> _maximumLength;
        private readonly Func<string?, TOutput> _creator;

        public NotEmptyStringValidator(Option<int> minimumLength, Option<int> maximumLength, Func<string?, TOutput> creator)
        {
            _minimumLength = minimumLength;
            _maximumLength = maximumLength;
            _creator = creator;
        }

        public override TOutput Create(string? input) => _creator(input);

        public override IEnumerable<(Func<string, bool> rule, string error)> Rules() => 
            new (Func<string, bool> rule, string error)[]
            {
                (string.IsNullOrEmpty, "Value cannot be null or empty"),
                (IsLessToMinimumLength, "Value is less than the minimum length"),
                (IsMoreThanMaximumLength, "Value is more than maximum length")
            };

        private bool IsLessToMinimumLength(string? input) =>
            _minimumLength.IsSome() && input?.Length < _minimumLength.ValueOrThrow();

        private bool IsMoreThanMaximumLength(string? input) =>
            _maximumLength.IsSome() && input?.Length > _maximumLength.ValueOrThrow();
    }
}