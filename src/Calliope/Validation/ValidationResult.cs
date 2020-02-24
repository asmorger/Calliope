using System.Collections.Generic;
using System.Linq;
using Calliope.Monads;

namespace Calliope.Validation
{
    public class ValidationFailures
    {
        public ValidationFailures(IList<string> validationMessages)
        {
            ValidationMessages = validationMessages.ToList();
        }
        
        public IReadOnlyList<string> ValidationMessages { get; }
    }
    
    public class ValidationResult<T> : Either<T, ValidationFailures>
    {
        public ValidationResult(T left) : base(left) { }
        public ValidationResult(ValidationFailures right) : base(right) { }

        public ValidationResult(IList<string> validationMessages) :
            base(new ValidationFailures(validationMessages)) { }
    }
}