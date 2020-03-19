using Calliope.Validators;
using Xunit;

namespace Calliope.Tests
{
    public class PrimitiveValueTests
    {
        [Fact]
        public void Two_value_objects_with_the_same_value_should_be_equal() =>
            Assert.Equal(new TestValueObject(42), new TestValueObject(42));
        
        [Fact]
        public void Two_value_objects_with_different_values_should_not_be_equal() =>
            Assert.NotEqual(new TestValueObject(1024),  new TestValueObject(2049));

        [Fact]
        public void Test()
        {
            var value = new TestValueObject(42);
            
            Assert.NotNull(value);
        }
        
        private class TestValueObject : ValueObject<int, TestValueObject, PositiveIntegerValidator>
        {
            public TestValueObject(int source) : base(source) { }
        }
    }
}