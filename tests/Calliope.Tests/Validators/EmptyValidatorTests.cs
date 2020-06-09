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
            Assert.IsAssignableFrom<Option<Result<string>>>(_validator.Validate(null).MatchLeft());

        [Fact]
        public void Validator_passes_string_empty_values() =>
            Assert.IsAssignableFrom<Option<Result<string>>>(_validator.Validate(string.Empty).MatchLeft());
        
        [Fact]
        public void Validator_passes_string_large_length_values() =>
            Assert.IsAssignableFrom<Option<Result<string>>>(_validator.Validate(new string(Enumerable.Repeat('a', 10000).ToArray())).MatchLeft());
    }
}