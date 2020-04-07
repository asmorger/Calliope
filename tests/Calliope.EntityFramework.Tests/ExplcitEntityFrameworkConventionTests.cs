using Calliope.EntityFramework.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace Calliope.EntityFramework.Tests
{
    public class ExplcitEntityFrameworkConventionTests
    {
        public ExplcitEntityFrameworkConventionTests()
        {
            var conventionSet = ConventionSet.CreateConventionSet(new ExplictTestDbContext("testdb"));
            _builder = new ModelBuilder(conventionSet);
        }

        private readonly ModelBuilder _builder;

        [Fact]
        public void Convert_value_object_properly_sets_converter()
        {
            _builder.Entity<BlogPost>().Property(m => m.Title).ConvertValueObject();

            var model = _builder.Model;
            var modelType = model.FindEntityType(typeof(BlogPost));
            var modelProperty = modelType.FindProperty(nameof(BlogPost.Title));

            Assert.IsType<ValueObjectConverter<Title, string>>(modelProperty.GetValueConverter());
        }

        [Fact]
        public void Convert_value_object_properly_sets_converter_on_owned_item()
        {
            _builder.Entity<BlogPost>()
                .OwnsOne(x => x.Information, b =>
                {
                    b.Property(x => x.Author).ConvertValueObject();
                    b.Property(x => x.Date).ConvertValueObject();
                });
            ;

            var model = _builder.Model;
            var modelType = model.FindEntityType(typeof(PublishInformation));
            var modelProperty = modelType.FindProperty(nameof(PublishInformation.Author));
            
            Assert.IsType<ValueObjectConverter<PublishAuthor, string>>(modelProperty.GetValueConverter());
        }
    }
}