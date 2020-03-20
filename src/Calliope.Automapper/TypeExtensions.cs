using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Calliope.Automapper
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType) => 
            FindInterfacesThatClosesCore(pluggedType, templateType).Distinct();

        private static IEnumerable<Type> FindInterfacesThatClosesCore(Type pluggedType, Type templateType)
        {
            if (pluggedType == null) yield break;

            if (!pluggedType.IsConcrete()) yield break;

            if (templateType.GetTypeInfo().IsInterface)
            {
                foreach (
                    var interfaceType in
                    pluggedType.GetInterfaces()
                        .Where(type => type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
                {
                    yield return interfaceType;
                }
            }
            else if (pluggedType.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType &&
                     (pluggedType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == templateType))
            {
                yield return pluggedType.GetTypeInfo().BaseType;
            }

            if (pluggedType.GetTypeInfo().BaseType == typeof(object)) yield break;

            foreach (var interfaceType in FindInterfacesThatClosesCore(pluggedType.GetTypeInfo().BaseType, templateType))
            {
                yield return interfaceType;
            }
        }

        public static bool IsConcrete(this Type type) => 
            !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;

        public static bool IsOpenGeneric(this Type type) => 
            type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;
    }
}