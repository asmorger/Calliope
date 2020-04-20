using Calliope.EntityFramework.Tests.Models;
using Xunit;

namespace Calliope.EntityFramework.Tests
{
    public class ValueObjectMappingFactoryTests
    {
        [Fact]
        public void Factory_correctly_loads_mapping_for_known_types()
        {
            var factory = new ValueObjectMappingFactory();
            
            Assert.NotNull(factory.GetMapping(typeof(PublishAuthor)));
        }
    }
}