using System;
using System.Collections.Generic;
using Calliope.Monads;
using Calliope.Validation;

namespace Calliope.Validators
{
    public class NotEmptyStringValidator : Validator<string>
    {
        private readonly Option<int> _minimumLength;
        private readonly Option<int> _maximumLength;

        public NotEmptyStringValidator(Option<int> minimumLength, Option<int> maximumLength)
        {
            _minimumLength = minimumLength;
            _maximumLength = maximumLength;
        }

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