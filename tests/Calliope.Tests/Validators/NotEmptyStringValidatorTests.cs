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
            Assert.True(_validator.Validate(null).IsError());
        
        [Fact]
        public void Validator_fails_when_string_has_too_few_characters() =>
            Assert.True(_validator.Validate("").IsError());
        
        [Fact]
        public void Validator_fails_when_string_has_too_many_characters() =>
            Assert.True(_validator.Validate(new string(Enumerable.Repeat('a', 150).ToArray())).IsError());
        
        [Fact]
        public void Validator_succeeds_when_string_is_valid() =>
            Assert.True(_validator.Validate("Engage").IsOk());
    }
}