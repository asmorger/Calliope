using System;
using System.Reflection;

namespace Calliope
{
    public static class ValueObjectFactory
    {
        public static TOutput FromValueObject<TOutput>(IPrimitiveValueObject<TOutput> primitiveValueObject) => primitiveValueObject.Value;


        public static object FromValue<TSource>(TSource source, Type type)
        {
            var method = type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
            return method?.Invoke(null, new object[] {source});
        }
    }
}