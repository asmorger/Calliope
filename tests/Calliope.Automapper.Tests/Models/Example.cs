namespace Calliope.Automapper.Tests {
    public class Example
    {
        public Example() => Answer = TheAnswer.Create(42);

        public TheAnswer Answer { get; }
    }
}