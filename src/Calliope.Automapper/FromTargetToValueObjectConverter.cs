using AutoMapper;

namespace Calliope.Automapper
{
    public class FromTargetToValueObjectConverter<TValue, TValueObject> : ITypeConverter<TValue, TValueObject>
        where TValueObject : IValueObject<TValue>
    {
        public TValueObject Convert(TValue source, TValueObject destination, ResolutionContext context) => 
            (TValueObject) ValueObjectFactory.FromValue(source, typeof(TValueObject));
    }
}