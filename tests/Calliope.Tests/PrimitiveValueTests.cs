using Calliope.Validators;
using Xunit;

namespace Calliope.Tests
{
    public class PrimitiveValueTests
    {
        [Fact]
        public void Two_value_objects_with_the_same_value_should_be_equal() =>
            Assert.Equal(TestValueObject.Create(42), TestValueObject.Create(42));
        
        [Fact]
        public void Two_value_objects_with_different_values_should_not_be_equal() =>
            Assert.NotEqual(TestValueObject.Create(1024),  TestValueObject.Create(2049));

        [Fact]
        public void Test()
        {
            var value = TestValueObject.Create(42);
            
            Assert.NotNull(value);
        }
        
        private class TestValueObject : ValueObject<int, TestValueObject, PositiveIntegerValidator>
        {
        }
    }
}