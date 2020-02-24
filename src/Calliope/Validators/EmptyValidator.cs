using System;
using System.Collections.Generic;
using Calliope.Validation;

namespace Calliope.Validators
{
    public class EmptyValidator<TInput, TOutput> : Validator<TInput, TOutput>
    {
        private readonly Func<TInput, TOutput> _creator;

        public EmptyValidator(Func<TInput, TOutput> creator)
        {
            _creator = creator;
        }

        public override TOutput Create(TInput input) => _creator(input);

        public override IEnumerable<(Func<TInput, bool> rule, string error)> Rules() => new (Func<TInput, bool> rule, string error)[0];
    }
}