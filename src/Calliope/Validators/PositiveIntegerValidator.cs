using System;
using System.Collections.Generic;
using Calliope.Validation;

namespace Calliope.Validators
{
    public class PositiveIntegerValidator : Validator<int>
    {
        public override IEnumerable<(Func<int, bool> rule, string error)> Rules() => 
            new (Func<int, bool> rule, string error)[]
            {
                (LessThanOrEqualToZero, "Value must be above zero")
            };

        private bool LessThanOrEqualToZero(int input) => input <= 0;

    }
}