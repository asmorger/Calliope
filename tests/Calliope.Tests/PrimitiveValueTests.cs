using Calliope.Validators;
using Xunit;

namespace Calliope.Tests
{
    public class PrimitiveValueTests
    {
        [Fact]
        public void Two_value_objects_with_the_same_value_should_be_equal() =>
            Assert.Equal(TestValue.Create(42), TestValue.Create(42));
        
        [Fact]
        public void Two_value_objects_with_different_values_should_not_be_equal() =>
            Assert.NotEqual(TestValue.Create(1024), TestValue.Create(2049));

        [Fact]
        public void Test()
        {
            var value = TestValue.Create(42);
            
            Assert.NotNull(value);
        }
        
        private class TestValue : Value<int, TestValue, PositiveIntegerValidator>
        {
        }
    }
}