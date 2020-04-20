using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calliope.EntityFramework
{
    public static class ModelBuilderExtensions
    {
        public static PropertyBuilder<TValueObject> HasValueObject<TValueObject, TPrimitive>(
            this PropertyBuilder<TValueObject> builder)
            where TValueObject : IPrimitiveValueObject<TPrimitive>
        {
            var converter = new ValueObjectConverter<TValueObject, TPrimitive>();

            builder.HasConversion(converter);

            return builder;
        }
    }
}