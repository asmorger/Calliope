using Xunit;

namespace Calliope.Tests
{
    public class EitherTests
    {
        [Fact]
        public void MatchLeft_correctly_applies_logic_to_left()
        {
            var either = new LeftOrRight(new Left("Frodo"));
            var result = either.MatchLeft(x => x.Name);

            Assert.Equal(Optional.Create("Frodo"), result);
        }
        
        [Fact]
        public void MatchLeft_correctly_returns_none_when_right_is_defined()
        {
            var either = new LeftOrRight(new Right(4));
            var result = either.MatchLeft(x => x.Name);

            Assert.Equal(Optional.None<string>(), result);
        }
        
        [Fact]
        public void MatchOptional_correctly_applies_logic_to_right()
        {
            var either = new LeftOrRight(new Right(4));
            var result = either.MatchRight(x => x.Number * 2);

            Assert.Equal(Optional.Create<int>(8), result);
        }
        
        [Fact]
        public void MatchRight_correctly_returns_none_when_left_is_defined()
        {
            var either = new LeftOrRight(new Left("Samwise"));
            var result = either.MatchRight(x => x.Number);

            Assert.Equal(Optional.None<int>(), result);
        }

        private class Left
        {
            public string Name { get; }

            internal Left(string name)
            {
                Name = name;
            }
        }

        private class Right
        {
            public int Number { get; }

            internal Right(int number)
            {
                Number = number;
            }
        }

        private class LeftOrRight : Either<Left, Right>
        {
            public LeftOrRight(Left left) : base(left) { }
            public LeftOrRight(Right right) : base(right) { }
        }
    }
}