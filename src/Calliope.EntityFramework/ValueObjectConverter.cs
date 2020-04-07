using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Calliope.EntityFramework
{
    public class ValueObjectConverter<TValueObject, TTarget> : ValueConverter<TValueObject, TTarget>
        where TValueObject : IPrimitiveValueObject<TTarget>
    {
        public ValueObjectConverter(ConverterMappingHints mappingHints = null)
            : base(
                x => ValueObjectFactory.FromValueObject(x), 
                x => (TValueObject) ValueObjectFactory.FromValue(x, typeof(TValueObject))
            , mappingHints)
        {
        }
    }
}