using Xunit;

namespace Calliope.Tests
{
    public class PrimitiveValueTests
    {
        [Fact]
        public void Two_value_objects_with_the_same_value_should_be_equal() =>
            Assert.Equal(new TestValue(42), new TestValue(42));
        
        [Fact]
        public void Two_value_objects_with_different_values_should_not_be_equal() =>
            Assert.NotEqual(new TestValue(1024), new TestValue(2049));
        
        private class TestValue : PrimitiveValue<int>
        {
            public TestValue(int value) : base(value)
            {
                
            }
        }
    }
}