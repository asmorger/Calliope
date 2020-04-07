using System;
using System.Collections.Generic;
using Calliope.Validators;

namespace Calliope.EntityFramework.Tests.Models
{
    public class PublishDate : PrimitiveValueObject<DateTime, PublishDate, EmptyValidator<DateTime>> { }
    public class PublishAuthor : PrimitiveValueObject<string, PublishAuthor, EmptyValidator<string>> { }
    
    public class PublishInformation : ValueObject<PublishInformation>
    {
        protected PublishInformation() { }
        
        public PublishDate Date { get; private set; }
        public PublishAuthor Author { get; private set; }
        
        public static PublishInformation Create(PublishDate date, PublishAuthor author) =>
            new PublishInformation
            {
                Date =  date,
                Author = author
            };

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Date.Value;
            yield return Author.Value;
        }
    }
}