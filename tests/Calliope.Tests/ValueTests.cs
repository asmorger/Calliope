using System.Collections.Generic;
using Xunit;

namespace Calliope.Tests
{
    public class ValueTests
    {
        [Fact]
        public void Two_value_objects_with_the_same_value_should_be_equal() =>
            Assert.Equal(new TestValueObject(42), new TestValueObject(42));
        
        [Fact]
        public void Two_value_objects_with_different_values_should_not_be_equal() =>
            Assert.NotEqual(new TestValueObject(1024), new TestValueObject(2049));

        private class TestValueObject : ValueObject<TestValueObject>
        {
            public TestValueObject(int number)
            {
                Number = number;
            }
            
            private int Number { get; }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Number;
            }
        }
    }
}