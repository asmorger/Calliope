using Calliope.Validators;
using Shouldly;
using Xunit;

namespace Calliope.Tests
{
    public class ValueObjectFactoryTests
    {
        [Fact]
        public void The_factory_correctly_identifies_the_value_when_converting_from_a_value_object()
        {
            var valueObject = TestPrimitiveValueObject.Create(42);
            var result = ValueObjectFactory.FromValueObject(valueObject);

            result.ShouldBe(42);
        }

        [Fact]
        public void The_factory_correctly_converts_a_value_to_value_object()
        {
            var value = 42;
            var result = (IPrimitiveValueObject<int>) ValueObjectFactory.FromValue(value, typeof(PrimitiveValueObject<int, TestPrimitiveValueObject, PositiveIntegerValidator>));
            
            result.Value.ShouldBe(42);
        }

        private class TestPrimitiveValueObject : PrimitiveValueObject<int, TestPrimitiveValueObject, PositiveIntegerValidator>
        {
        }
    }
}