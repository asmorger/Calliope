namespace Calliope.Automapper.Tests.Models 
{
    public class Example
    {
        public Example() => Answer = TheAnswer.Create(42);
        public Example(int value) => TheAnswer.Create(value);

        public TheAnswer Answer { get; }
    }
}