using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Calliope.ResultExtensions;

namespace Calliope;

public record ValidationRule<T>(Func<T, bool> Check, Func<T, string> ErrorFactory);

public interface IValidatable<TSource>
{
    static abstract IEnumerable<ValidationRule<TSource>> GetValidationRules();
}

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

public static class Placeholder
{
    public static readonly string TypeName = "{TypeName}";
}

public class ValidationFailed : DomainError
{
    public ValidationFailed(IEnumerable<string> validationMessages)
    {
        Messages.AddRange(validationMessages);
    }

    public List<string> Messages { get; } = new();

    public string ToErrorMessage()
    {
        var builder = new StringBuilder();
        builder.AppendLine("Validation Failed!");

        foreach(var message in Messages)
        {
            builder.Append("  ");
            builder.AppendLine(message);
        }

        return builder.ToString();
    }
}