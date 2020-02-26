using Calliope.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Calliope.FluentValidation.Tests
{
    public class ValidatorTests
    {
        internal class TestInteger : PrimitiveValue<int, TestInteger, PositiveIntegerValidator>
        {
        }
        
        internal class TestRequestValidator : AbstractValidator<TestRequest>
        {
            internal TestRequestValidator()
            {
                RuleFor(x => x.Value).Targeting(TestInteger.Validator);
            }
        }

        internal class TestRequest
        {
            public int Value { get; set; }
        }
        
        [Fact]
        public void Validator_can_be_created() => Assert.NotNull(new PrimitiveValueValidator<int>(TestInteger.Validator));

        [Fact]
        public void Valdiator_succeeds_when_item_is_valid()
        {
            var validator = new PrimitiveValueValidator<int>(TestInteger.Validator);
            var result = validator.Validate(42);
            
            Assert.True(result.IsValid);
        }
        
        [Fact]
        public void Valdiator_fails_when_item_is_invalid()
        {
            var validator = new PrimitiveValueValidator<int>(TestInteger.Validator);
            var result = validator.Validate(-42);
            
            Assert.False(result.IsValid);
        }
        
        [Fact]
        public void Valdiator_sets_proper_validation_message_when_item_is_invalid()
        {
            var validator = new PrimitiveValueValidator<int>(TestInteger.Validator);
            var result = validator.Validate(-42);

            Assert.Equal("{PropertyName} must be above zero", result.Errors[0].ErrorMessage);
        }
        
        [Fact]
        public void Extension_method_for_value_object_sets_validator() =>
            new TestRequestValidator().ShouldHaveValidationErrorFor(r => r.Value, -42);
    }
}