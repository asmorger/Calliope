using System;
using System.Collections.Generic;

namespace Calliope.Validation
{
    public class ValidationFailedException : Exception
    {
        public IReadOnlyList<string> Messages { get; }
        public ValidationFailedException(ValidationFailures failures)
        {
            Messages = failures.ValidationMessages;
        }
    }
}