using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Calliope.FluentValidation.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        public void Extension_method_for_value_object_sets_validator() =>
            new TestRequestValidator().ShouldHaveValidationErrorFor(r => r.Value, -42);

        internal class TestRequestValidator : AbstractValidator<TestRequest>
        {
            internal TestRequestValidator()
            {
                RuleFor(x => x.Value).ForDomainValue(TestInteger.GetValidator());
            }
        }

        internal class TestRequest
        {
            public int Value { get; set; }
        }
    }
}