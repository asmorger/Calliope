using AutoMapper;
using Calliope.Automapper.Tests.Models;
using Xunit;

namespace Calliope.Automapper.Tests
{
    public class ProfileTests
    {
        [Fact]
        public void Profile_loads_all_value_object_mappers()
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<CalliopeProfile>();
                cfg.CreateMap<Example, ExampleDto>();
            });

            configuration.AssertConfigurationIsValid();
        }
    }
}