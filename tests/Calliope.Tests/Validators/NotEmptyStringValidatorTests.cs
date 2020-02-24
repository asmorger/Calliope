using System.Linq;
using Calliope.Monads;
using Calliope.Validation;
using Calliope.Validators;
using Xunit;

namespace Calliope.Tests.Validators
{
    public class NotEmptyStringValidatorTests
    {
        private readonly NotEmptyStringValidator<TestValue> _validator;

        public NotEmptyStringValidatorTests()
        {
            _validator = new NotEmptyStringValidator<TestValue>(1, 100, x => new TestValue(x));
        }

        [Fact]
        public void Validator_fails_when_string_is_null() =>
            Assert.IsAssignableFrom<Option<ValidationFailures>>(_validator.Validate(null).MatchRight());
        
        [Fact]
        public void Validator_fails_when_string_has_too_few_characters() =>
            Assert.IsAssignableFrom<Option<ValidationFailures>>(_validator.Validate("").MatchRight());
        
        [Fact]
        public void Validator_fails_when_string_has_too_many_characters() =>
            Assert.IsAssignableFrom<Option<ValidationFailures>>(
                _validator.Validate(new string(Enumerable.Repeat('a', 150).ToArray())).MatchRight());
        
        [Fact]
        public void Validator_succeeds_when_string_is_valid() =>
            Assert.IsAssignableFrom<Option<TestValue>>(_validator.Validate("Engage").MatchLeft());

        
        private class TestValue : PrimitiveValue<string, TestValue>
        {
            public TestValue(string value) : base(value) { }
        }
    }
}