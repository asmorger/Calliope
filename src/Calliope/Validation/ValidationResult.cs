using System.Collections.Generic;
using System.Linq;

namespace Calliope.Validation
{
    public class ValidationFailed
    {
        public ValidationFailed(IList<string> validationMessages)
        {
            ValidationMessages = validationMessages.ToList();
        }
        
        public IReadOnlyList<string> ValidationMessages { get; }
    }

    public class ValidationSuccess<T>
    {
        public T Value { get; }

        public ValidationSuccess(T value)
        {
            Value = value;
        }
    }
    
    public class ValidationResult<T> : Either<ValidationSuccess<T>, ValidationFailed>
    {
        public ValidationResult(ValidationSuccess<T> left) : base(left) { }
        public ValidationResult(ValidationFailed right) : base(right) { }

        public ValidationResult(IList<string> validationMessages) :
            base(new ValidationFailed(validationMessages)) { }
        
    }
}