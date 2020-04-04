using System.Linq;
using Calliope.Validation;
using Calliope.Validators;
using Xunit;

namespace Calliope.Tests.Validators
{
    public class NotEmptyStringValidatorTests
    {
        private readonly NotEmptyStringValidator _validator;

        public NotEmptyStringValidatorTests()
        {
            _validator = new NotEmptyStringValidator(1, 100);
        }

        [Fact]
        public void Validator_fails_when_string_is_null() =>
            Assert.IsAssignableFrom<Optional<ValidationFailed>>(_validator.Validate(null).MatchRight());
        
        [Fact]
        public void Validator_fails_when_string_has_too_few_characters() =>
            Assert.IsAssignableFrom<Optional<ValidationFailed>>(_validator.Validate("").MatchRight());
        
        [Fact]
        public void Validator_fails_when_string_has_too_many_characters() =>
            Assert.IsAssignableFrom<Optional<ValidationFailed>>(
                _validator.Validate(new string(Enumerable.Repeat('a', 150).ToArray())).MatchRight());
        
        [Fact]
        public void Validator_succeeds_when_string_is_valid() =>
            Assert.IsAssignableFrom<Optional<ValidationSuccess<string>>>(_validator.Validate("Engage").MatchLeft());
    }
}