using Calliope.Validators;

namespace Calliope.EntityFramework.Tests.Models
{
    public class BlogPost : Entity
    {
        protected BlogPost() { }

        public BlogPost(Title title) : this()
        {
            Title = title;
        }

        public Title Title { get; }
    }

    public class Title : ValueObject<string, Title, EmptyValidator<string>> { }
}