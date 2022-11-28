using System;
using System.Collections.Generic;
using System.Linq;
using Calliope.Validation;
using static Calliope.ResultExtensions;

namespace Calliope;

public abstract record ValidationRule<T>(Func<T, bool> Check, Func<T, string> ErrorFactory);

public static class Validator
{
    public static Result<bool> Validate<TInput>(TInput source, IEnumerable<ValidationRule<TInput>> rules)
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