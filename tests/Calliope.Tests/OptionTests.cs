using Calliope.Monads;
using Xunit;

namespace Calliope.Tests
{
    public class OptionTests
    {
        [Fact]
        public void Some_implements_some_or_none() => Assert.Equal(42, new Some<int>(42).ValueOrDefault(() => 0));

        [Fact]
        public void Some_has_value_returns_true() => Assert.True(new Some<int>(42).HasValue); 
        
        [Fact]
        public void None_implements_some_or_none() => Assert.Equal(42, new None<int>().ValueOrDefault(() => 42));
        
        [Fact]
        public void None_has_value_returns_false() => Assert.False(new None<int>().HasValue);
    }
}