using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Calliope.EntityFramework
{
    public class ValueObjectConverter<TValueObject, TTarget> : ValueConverter<TValueObject, TTarget>
        where TValueObject : IValueObject<TTarget>
    {
        public ValueObjectConverter(ConverterMappingHints mappingHints = null)
            : base(
                x => ValueObjectFactory.FromValueObject(x), 
                x => (TValueObject) ValueObjectFactory.FromTarget(x, typeof(TValueObject))
            , mappingHints)
        {
        }
    }
}