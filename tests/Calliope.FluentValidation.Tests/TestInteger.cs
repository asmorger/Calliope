using Calliope.Validators;

namespace Calliope.FluentValidation.Tests {
    internal class TestInteger : PrimitiveValue<int, TestInteger, PositiveIntegerValidator>
    {
        private TestInteger(int value) : base(value) { }

        public static TestInteger Create(int value) => Create(value, x => new TestInteger(value));
        public static implicit operator int(TestInteger t) => t.Value;
    }
}