using AutoMapper;

namespace Calliope.Automapper
{
    /// <summary>
    /// Creates a converter that, by convention will convert ValueObjects to the target values
    /// </summary>
    /// <typeparam name="TValueObject"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class FromValueObjectToValueConverter<TValueObject, TValue> : ITypeConverter<TValueObject, TValue>
        where TValueObject : IValueObject<TValue>
    {
        public TValue Convert(TValueObject source, TValue destination, ResolutionContext context) => source.Value;
    }
}