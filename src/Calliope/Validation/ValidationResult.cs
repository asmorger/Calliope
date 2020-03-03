using System.Collections.Generic;
using System.Linq;

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

    public class ValidationSuccess<T>
    {
        public T GoodValue { get; }

        public ValidationSuccess(T goodValue)
        {
            GoodValue = goodValue;
        }
    }
    
    public class ValidationResult<T> : Either<ValidationSuccess<T>, ValidationFailures>
    {
        public ValidationResult(ValidationSuccess<T> left) : base(left) { }
        public ValidationResult(ValidationFailures right) : base(right) { }

        public ValidationResult(IList<string> validationMessages) :
            base(new ValidationFailures(validationMessages)) { }
    }
}