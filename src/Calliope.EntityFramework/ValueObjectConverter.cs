using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Calliope.EntityFramework
{
    public class ValueObjectConverter<TValueObject, TPrimitive> : ValueConverter<TValueObject, TPrimitive>
        where TValueObject : IPrimitiveValueObject<TPrimitive>
    {
        public ValueObjectConverter(ConverterMappingHints mappingHints = null)
            : base(x => x.Value, x => DynamicCreate(x), mappingHints)
        {
        }

        private static TValueObject DynamicCreate(TPrimitive primitive)
        {
            var method = typeof(TValueObject).GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
            return (TValueObject) method?.Invoke(null, new object[] {primitive});
        }
    }
}