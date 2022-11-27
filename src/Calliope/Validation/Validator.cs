using System.Collections.Generic;
using System.Linq;
using static Calliope.ResultExtensions;

namespace Calliope.Validation
{
    public interface IValidator<TInput>
    {
        Result<bool> Validate(TInput source, IEnumerable<ValidationRule<TInput>> rules);
    }

    public class Validator<TInput> : IValidator<TInput>
    {
        public Result<bool> Validate(TInput source, IEnumerable<ValidationRule<TInput>> rules)
        {
            var failures = new List<string>();

            foreach (var rule in rules)
            {
                var test = rule.Check.Invoke(source);

                if (!test)
                {
                    failures.Add(rule.ErrorFactory.Invoke(source));
                }
            }
            
            if(failures.Any()) 
                return Fail<bool>(new ValidationFailed(failures));

            return Ok(true);
        }
    }
}