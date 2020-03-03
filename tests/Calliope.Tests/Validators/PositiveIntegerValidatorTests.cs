using Calliope.Validation;
using Calliope.Validators;
using Xunit;

namespace Calliope.Tests.Validators
{
    public class PositiveIntegerValidatorTests
    {
        private readonly PositiveIntegerValidator _validator;
        public PositiveIntegerValidatorTests()
        {
            _validator = new PositiveIntegerValidator();
        }

        [Fact]
        public void Validator_throws_exception_when_precondition_is_not_met() =>
            Assert.IsAssignableFrom<Optional<ValidationFailures>>(_validator.Validate(-1).MatchRight());
    }
}