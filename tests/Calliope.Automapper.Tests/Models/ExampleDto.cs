namespace Calliope.Automapper.Tests.Models 
{
    public class ExampleDto
    {
        public ExampleDto() { }
        public ExampleDto(int value) => Answer = value;
        
        public int Answer { get; set; }
    }
}