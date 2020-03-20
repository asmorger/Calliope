using AutoMapper;
using Calliope.Automapper.Tests.Models;
using Xunit;

namespace Calliope.Automapper.Tests
{
    public class FromTargetToValueObjectConverterTests
    {
        [Fact]
        public void Configuration_is_valid()
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap(typeof(int), typeof(TheAnswer)).ConvertUsing(typeof(FromTargetToValueObjectConverter<int, TheAnswer>));
            });

            configuration.AssertConfigurationIsValid();
        }
        
        [Fact]
        public void Configuration_maps_properties()
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap(typeof(int), typeof(TheAnswer)).ConvertUsing(typeof(FromTargetToValueObjectConverter<int, TheAnswer>));
                cfg.CreateMap<ExampleDto, Example>();
            });

            var mapper = configuration.CreateMapper();
            var destination = mapper.Map<Example>(new ExampleDto(42));
            
            Assert.Equal(TheAnswer.Create(42), destination.Answer);
        }
    }
}