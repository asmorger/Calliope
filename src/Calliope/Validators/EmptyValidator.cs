using System;
using System.Collections.Generic;
using Calliope.Validation;

namespace Calliope.Validators
{
    /// <summary>
    /// Default validator that will pass any value as being successful.
    /// </summary>
    public class EmptyValidator<TInput> : Validator<TInput>
    {
        public override IEnumerable<(Func<TInput, bool> rule, string error)> Rules() => new (Func<TInput, bool> rule, string error)[0];
    }
}