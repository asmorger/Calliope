using AutoMapper;

namespace Calliope.Automapper
{
    public class FromValueObjectToValueConverter<TValueObject, TValue> : ITypeConverter<TValueObject, TValue>
        where TValueObject : IValueObject<TValue>
    {
        public TValue Convert(TValueObject source, TValue destination, ResolutionContext context) => source.Value;
    }
}