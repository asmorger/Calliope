using System;
using System.Collections.Generic;
using Calliope.Validation;

namespace Calliope.Validators
{
    public class PositiveIntegerValidator<TOutput> : Validator<int, TOutput>
    {
        private readonly Func<int, TOutput> _creator;

        public PositiveIntegerValidator(Func<int, TOutput> creator)
        {
            _creator = creator;
        }

        public override TOutput Create(int input) => _creator(input);

        public override IEnumerable<(Func<int, bool> rule, string error)> Rules() => 
            new (Func<int, bool> rule, string error)[]
            {
                (LessThanOrEqualToZero, "Value must be above zero")
            };

        private bool LessThanOrEqualToZero(int input) => input <= 0;

    }
}