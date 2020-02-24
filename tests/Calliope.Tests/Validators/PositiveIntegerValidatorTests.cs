using Calliope.Validation;
using Calliope.Validators;
using Xunit;

namespace Calliope.Tests.Validators
{
    public class PositiveIntegerValidatorTests
    {
        private readonly PositiveIntegerValidator<TestValue> _validator;
        public PositiveIntegerValidatorTests()
        {
            _validator = new PositiveIntegerValidator<TestValue>(x => new TestValue(x));
        }

        [Fact]
        public void Validator_throws_exception_when_precondition_is_not_met() =>
            Assert.Throws<ValidationFailedException>(() => TestValue.Create(-1, _validator));

        private class TestValue : PrimitiveValue<int, TestValue>
        {
            public TestValue(int value) : base(value) { }
        }
    }
}