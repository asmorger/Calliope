using System.Collections.Generic;
using Calliope.Validators;

namespace Calliope.Validation
{
    public class ValidationFailedException : DomainException
    {
        public IReadOnlyList<string> Messages { get; }
        public ValidationFailedException(string typeName,IList<string> validationMessages)
        {
            var messages = new List<string>();

            foreach (var failure in validationMessages)
            {
                messages.Add(failure.Replace(Placeholder.TypeName, typeName));
            }
            
            Messages = messages;
        }
    }
}