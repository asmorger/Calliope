using Calliope.EntityFramework.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace Calliope.EntityFramework.Tests
{
    /* Removing feature (for now).  Will return to it soon.
    public class ConventionalEntityFrameworkConfigurationTests
    {
        private readonly ModelBuilder _builder;
        public ConventionalEntityFrameworkConfigurationTests()
        {
            var conventionSet = ConventionSet.CreateConventionSet(new ConventionBasedTestDbContext("testdb"));
            _builder = new ModelBuilder(conventionSet);
        }

        [Fact]
        public void Entity_configuration_is_valid_when_Calliope_configuration_is_called()
        {
            _builder.Entity<BlogPost>().Property(m => m.Title);
            _builder.AddValueObjectConversions();        

            var model = _builder.Model;
            var modelType = model.FindEntityType(typeof(BlogPost));
            var modelProperty = modelType.FindProperty(nameof(BlogPost.Title));
            
            Assert.IsType<ValueObjectConverter<Title, string>>(modelProperty.GetValueConverter());
        }
    }
    */
}