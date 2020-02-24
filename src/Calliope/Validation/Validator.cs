using System;
using System.Collections.Generic;
using System.Linq;

namespace Calliope.Validation
{
    public interface IValidator<TInput, TOutput>
    {
        ValidationResult<TOutput> Validate(TInput source);

        TOutput Create(TInput input);

        IEnumerable<(Func<TInput, bool> rule, string error)> Rules();
    }

    public abstract class Validator<TInput, TOutput> : IValidator<TInput, TOutput>
    {
        public abstract TOutput Create(TInput input);
        
        public ValidationResult<TOutput> Validate(TInput source)
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
            
            if(failures.Any()) return new ValidationResult<TOutput>(failures);
            
            return new ValidationResult<TOutput>(Create(source));
        }

        public abstract IEnumerable<(Func<TInput, bool> rule, string error)> Rules();
    }
}