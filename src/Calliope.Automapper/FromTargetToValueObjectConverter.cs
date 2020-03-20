using System.Reflection;
using AutoMapper;

namespace Calliope.Automapper
{
    public class FromTargetToValueObjectConverter<TValue, TValueObject> : ITypeConverter<TValue, TValueObject>
        where TValueObject : IValueObject<TValue>
    {
        public TValueObject Convert(TValue source, TValueObject destination, ResolutionContext context)
        {
            var method = typeof(TValueObject).GetMethod("Create", BindingFlags.Public | BindingFlags.Static);

            if (method is null)
                return default;

            var result = method.Invoke(destination, new object[] { source });
            return (TValueObject) result;
        }
    }
}