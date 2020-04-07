using Calliope.Validators;
using Xunit;

namespace Calliope.Tests
{
    public class PrimitiveValueTests
    {
        [Fact]
        public void Two_value_objects_with_the_same_value_should_be_equal() =>
            Assert.Equal(TestPrimitiveValueObject.Create(42), TestPrimitiveValueObject.Create(42));
        
        [Fact]
        public void Two_value_objects_with_different_values_should_not_be_equal() =>
            Assert.NotEqual(TestPrimitiveValueObject.Create(1024),  TestPrimitiveValueObject.Create(2049));

        [Fact]
        public void Test()
        {
            var value = TestPrimitiveValueObject.Create(42);
            
            Assert.NotNull(value);
        }
        
        private class TestPrimitiveValueObject : PrimitiveValueObject<int, TestPrimitiveValueObject, PositiveIntegerValidator>
        {
        }
    }
}