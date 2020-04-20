using System;
using System.Linq;
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
            var conventionSet = ConventionSet.CreateConventionSet(new ExplicitTestDbContext("testdb"));
            _builder = new ModelBuilder(conventionSet);
        }

        private readonly ModelBuilder _builder;

        [Fact]
        public void Can_save_value_object_to_database()
        {
            _builder.Entity<BlogPost>().Property(m => m.Title).ConvertValueObject();
            _builder.Entity<BlogPost>()
                .OwnsOne(x => x.Information, b =>
                    {
                        b.Property(x => x.Author).ConvertValueObject();
                        b.Property(x => x.Date).ConvertValueObject();
                    }
                );

            var date = PublishDate.Create(new DateTime(2020, 1, 1));
            var author = PublishAuthor.Create("Ender Wiggin");
            
            var context = new ExplicitTestDbContext("testdb");
            var blogPost = new BlogPost(Title.Create("Sample"), PublishInformation.Create(date, author));

            context.Posts.Add(blogPost);
            var result = context.SaveChanges();
            
            Assert.Equal(1, result);
        }
        
        [Fact]
        public void Can_read_value_object_from_database()
        {
            _builder.Entity<BlogPost>().Property(m => m.Title).HasConversion(x => x.Value, v => Title.Create(v));
            _builder.Entity<BlogPost>()
                .OwnsOne(x => x.Information, b =>
                    {
                        b.Property(x => x.Author).HasConversion(x => x.Value, v => PublishAuthor.Create(v));
                        b.Property(x => x.Date).HasConversion(x => x.Value, v => PublishDate.Create(v));
                    }
                );

            var date = PublishDate.Create(new DateTime(2020, 1, 1));
            var author = PublishAuthor.Create("Ender Wiggin");
            
            var context = new ExplicitTestDbContext("testdb");
            var blogPost = new BlogPost(Title.Create("Sample"), PublishInformation.Create(date, author));

            context.Posts.Add(blogPost);
            context.SaveChanges();

            var result = context.Posts.FirstOrDefault(x => x.Title == "Sample");
            
            Assert.NotNull(result);
        }

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
                    }
                );

            var model = _builder.Model;
            var modelType = model.FindEntityType(typeof(PublishInformation));
            var modelProperty = modelType.FindProperty(nameof(PublishInformation.Author));

            Assert.IsType<ValueObjectConverter<PublishAuthor, string>>(modelProperty.GetValueConverter());
            
        }
    }
}