using System.Linq;
using Calliope.Monads;
using Calliope.Validators;
using Xunit;

namespace Calliope.Tests.Validators
{
    public class EmptyValidatorTests
    {
        private EmptyValidator<string, TestValue> _validator;
        public EmptyValidatorTests()
        {
            _validator = new EmptyValidator<string, TestValue>(x => new TestValue(x));
        }
        
        [Fact]
        public void Validator_passes_null_values() =>
            Assert.IsAssignableFrom<Option<TestValue>>(_validator.Validate(null).MatchLeft());

        [Fact]
        public void Validator_passes_string_empty_values() =>
            Assert.IsAssignableFrom<Option<TestValue>>(_validator.Validate(string.Empty).MatchLeft());
        
        [Fact]
        public void Validator_passes_string_large_length_values() =>
            Assert.IsAssignableFrom<Option<TestValue>>(_validator.Validate(new string(Enumerable.Repeat('a', 10000).ToArray())).MatchLeft());
        
        private class TestValue : PrimitiveValue<string, TestValue>
        {
            public TestValue(string value) : base(value) { }
        }
    }
}