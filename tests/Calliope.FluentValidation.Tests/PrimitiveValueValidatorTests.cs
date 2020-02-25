using Xunit;

namespace Calliope.FluentValidation.Tests
{
    public class PrimitiveValueValidatorTests
    {
        [Fact]
        public void Validator_can_be_created() => Assert.NotNull(new PrimitiveValueValidator<int>(TestInteger.GetValidator()));

        [Fact]
        public void Valdiator_succeeds_when_item_is_valid()
        {
            var validator = new PrimitiveValueValidator<int>(TestInteger.GetValidator());
            var result = validator.Validate(42);
            
            Assert.True(result.IsValid);
        }
        
        [Fact]
        public void Valdiator_fails_when_item_is_invalid()
        {
            var validator = new PrimitiveValueValidator<int>(TestInteger.GetValidator());
            var result = validator.Validate(-42);
            
            Assert.False(result.IsValid);
        }
        
        [Fact]
        public void Valdiator_sets_proper_validation_message_when_item_is_invalid()
        {
            var validator = new PrimitiveValueValidator<int>(TestInteger.GetValidator());
            var result = validator.Validate(-42);

            Assert.Equal("{PropertyName} must be above zero", result.Errors[0].ErrorMessage);
        }
    }
}