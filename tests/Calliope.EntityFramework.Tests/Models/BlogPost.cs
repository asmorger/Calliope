using Calliope.Validators;

namespace Calliope.EntityFramework.Tests.Models
{
    public class BlogPost : Entity
    {
        protected BlogPost() { }

        public BlogPost(Title title, PublishInformation information) : this()
        {
            Title = title;
            Information = information;
        }

        public Title Title { get; }
        public PublishInformation Information { get; }
    }

    public class Title : PrimitiveValueObject<string, Title, EmptyValidator<string>> { }
}