using AutoMapper;
using Xunit;

namespace Calliope.Automapper.Tests
{
    public class FromValueObjectToValueConverterTests
    {
        [Fact]
        public void Configuration_is_valid()
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap(typeof(TheAnswer), typeof(int)).ConvertUsing(typeof(FromValueObjectToValueConverter<TheAnswer,int>));
            });

            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Configuration_maps_properties()
        {
            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap(typeof(TheAnswer), typeof(int)).ConvertUsing(typeof(FromValueObjectToValueConverter<TheAnswer,int>));
                cfg.CreateMap<Example, ExampleDto>();
            });

            var mapper = configuration.CreateMapper();
            var destination = mapper.Map<ExampleDto>(new Example());
            
            Assert.Equal(42, destination.Answer);
        }
    }
}