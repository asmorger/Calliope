using System.Collections.Generic;
using System.Linq;

namespace Calliope.Validation
{
    public class ValidationFailedException : DomainException
    {
        public IReadOnlyList<string> Messages { get; }
        public ValidationFailedException(IList<string> validationMessages)
        {
            Messages = validationMessages.ToList();
        }
    }
}