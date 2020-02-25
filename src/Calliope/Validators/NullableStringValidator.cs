using System;
using System.Collections.Generic;
using Calliope.Monads;
using Calliope.Validation;

namespace Calliope.Validators
{
    /// <summary>
    /// String validator that will ensure a string is either null or empty or is optionally bounded to a minimum and maximum value.
    /// </summary>
    public class NullableStringValidator : Validator<string>
    {
        private readonly Option<int> _minimumLength;
        private readonly Option<int> _maximumLength;
        
        public NullableStringValidator(Option<int> minimumLength, Option<int> maximumLength)
        {
            _minimumLength = minimumLength;
            _maximumLength = maximumLength;
        }
        public override IEnumerable<(Func<string, bool> rule, string error)> Rules() => 
            new (Func<string, bool> rule, string error)[]
            {
                (IsLessToMinimumLength, $"{Placeholder.TypeName} is less than the minimum length"),
                (IsMoreThanMaximumLength, $"{Placeholder.TypeName} is more than maximum length")
            };

        private bool IsLessToMinimumLength(string? input) =>
            _minimumLength.IsSome() && input?.Length < _minimumLength.ValueOrThrow();

        private bool IsMoreThanMaximumLength(string? input) =>
            _maximumLength.IsSome() && input?.Length > _maximumLength.ValueOrThrow();
    }
}