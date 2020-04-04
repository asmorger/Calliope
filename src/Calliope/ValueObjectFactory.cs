using System;
using System.Reflection;

namespace Calliope
{
    public static class ValueObjectFactory
    {
        public static TOutput FromValueObject<TOutput>(IValueObject<TOutput> valueObject) => valueObject.Value;

        public static object FromValue<TOutput>(TOutput target, Type type) =>
            type.InvokeMember("Create", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null,
                null, new object[] { target });
    }
}