using System.Linq;
using Calliope.Validation;
using Calliope.Validators;
using Xunit;

namespace Calliope.Tests.Validators
{
    public class EmptyValidatorTests
    {
        private readonly EmptyValidator<string> _validator;
        public EmptyValidatorTests()
        {
            _validator = new EmptyValidator<string>();
        }

        [Fact]
        public void Validator_passes_null_values() =>
            Assert.True(_validator.Validate(null).IsOk());

        [Fact]
        public void Validator_passes_string_empty_values() =>
            Assert.True(_validator.Validate(string.Empty).IsOk());
        
        [Fact]
        public void Validator_passes_string_large_length_values() =>
            Assert.True(_validator.Validate(new string(Enumerable.Repeat('a', 10000).ToArray())).IsOk());
    }
}