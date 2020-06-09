using System;
using System.Collections.Generic;
using Calliope.Validation;

namespace Calliope.Validators
{
    /// <summary>
    /// String validator that will ensure a string is either null or empty or is optionally bounded to a minimum and maximum value.
    /// </summary>
    public class NullableStringValidator : Validator<string>
    {
        private readonly Option<int> _maxLength;

        protected NullableStringValidator(Option<int> maxLength)
        {
            _maxLength = maxLength;
        }
        public override IEnumerable<(Func<string, bool> rule, string error)> Rules() =>
            new (Func<string, bool> rule, string error)[]
            {
                (StringExceedsMaxLength, $"{Placeholder.TypeName} is too long.")
            };

        private bool StringExceedsMaxLength(string input) => input?.Length > _maxLength;
    }
}