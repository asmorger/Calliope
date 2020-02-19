using Calliope.Monads;
using Xunit;

namespace Calliope.Tests
{
    public class OptionTests
    {
        [Fact]
        public void Some_implements_some_or_none() => Assert.Equal(42, new Some<int>(42).SomeOrValue(0));
        
        [Fact]
        public void None_implements_some_or_none() => Assert.Equal(42, new None<int>().SomeOrValue(42));
    }
}