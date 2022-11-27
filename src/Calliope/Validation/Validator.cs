using System;
using System.Collections.Generic;
using System.Linq;
using static Calliope.ResultExtensions;

namespace Calliope.Validation
{
    public interface IValidator<TInput>
    {
        Result<bool> Validate(TInput source);
        IEnumerable<(Func<TInput, bool> rule, string error)> Rules();
    }

    public abstract class Validator<TInput> : IValidator<TInput>
    {
        public Result<bool> Validate(TInput source)
        {
            var failures = new List<string>();

            foreach (var rule in Rules())
            {
                var test = rule.rule(source);

                if (test)
                {
                    failures.Add(rule.error);
                }
            }
            
            if(failures.Any()) 
                return Fail<bool>(new ValidationFailed(failures));

            return Ok(true);
        }

        public abstract IEnumerable<(Func<TInput, bool> rule, string error)> Rules();
    }
}