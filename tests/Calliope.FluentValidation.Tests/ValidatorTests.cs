using System.Collections.Generic;
using System.Linq;
using Calliope.Validation;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.TestHelper;
using FluentValidation.Validators;
using Xunit;

namespace Calliope.FluentValidation.Tests
{
    public class ValidatorTests
    {
        private record TestInteger(int Value) : IValidatable<int>
        {
            public static IEnumerable<ValidationRule<int>> GetValidationRules() => new []
            {
                new IntValidators.GreaterThanZero()
            };
        }

        private class TestRequestValidator : AbstractValidator<TestRequest>
        {
            internal TestRequestValidator()
            {
                RuleFor(x => x.Value).ValidFor<TestRequest, int, TestInteger>();
            }
        }

        internal class TestRequest
        {
            public int Value { get; set; }
        }
        
        [Fact]
        public void Validator_can_be_created() => Assert.NotNull(new ValueForValidator<int>(TestInteger.GetValidationRules()));

        [Fact]
        public void Valdiator_succeeds_when_item_is_valid()
        {
            var instance = new TestRequest {Value = 42};
            var validator = new ValueForValidator<int>(TestInteger.GetValidationRules());
            
            var selector = ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory();
            var context = new ValidationContext(instance, new PropertyChain(), selector);
            var propertyValidatorContext = new PropertyValidatorContext(context, PropertyRule.Create<TestRequest,int>(t => t.Value), nameof(TestRequest.Value));
            
            var result = validator.Validate(propertyValidatorContext).ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void Valdiator_sets_proper_validation_message_when_item_is_invalid()
        {
            var instance = new TestRequest {Value = -42};
            var validator = new ValueForValidator<int>(TestInteger.GetValidationRules());
            
            var selector = ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory();
            var context = new ValidationContext(instance, new PropertyChain(), selector);
            var propertyValidatorContext = new PropertyValidatorContext(context, PropertyRule.Create<TestRequest,int>(t => t.Value), nameof(TestRequest.Value));
            
            var result = validator.Validate(propertyValidatorContext).ToList();

            Assert.NotEmpty(result);
            Assert.Equal("-42 must be greater than 0", result.First().ErrorMessage);
        }
        
        [Fact]
        public void Extension_method_for_value_object_sets_validator() =>
            new TestRequestValidator().ShouldHaveValidationErrorFor(r => r.Value, -42);
    }
}