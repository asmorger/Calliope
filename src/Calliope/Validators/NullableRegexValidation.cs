using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Calliope.Validation;

namespace Calliope.Validators
{
    public class NullableRegexValidation: Validator<string>
    {
        protected readonly Regex Regex;
        protected NullableRegexValidation(string regexPattern)
        {
            Regex = new Regex(regexPattern);
        }
        
        public override IEnumerable<(Func<string, bool> rule, string error)> Rules() => 
            new (Func<string, bool> rule, string error)[]
            {
                (DoesNotMatchRegex, $"{Placeholder.TypeName} does not match expected format.")
            };

        private bool DoesNotMatchRegex(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            
            return !Regex.IsMatch(input);
        }
    }
}