using System;
using System.Linq;
using System.Reflection;

namespace Calliope
{
    public static class TypeExtensions
    {
        public static bool HasNestedValueObjects(this Type type) => type.GetProperties().Any(IsValueObject);

        public static bool IsValueObject(this PropertyInfo i) => IsValueObject(i.PropertyType);
        
        public static bool IsValueObject(this Type type)
        {
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface == typeof(IValueObject))
                    return true;
            }

            return false;
        }
        
        public static bool IsPrimitiveValueObject(this Type type)
        {
            var genericInterface = typeof(IPrimitiveValueObject<>);
            foreach (var @interface in type.GetInterfaces())
                
            {
                if (@interface == genericInterface)
                    return true;
            }

            return false;
        }

        public static Type? GetValueObjectInterfaceType(this Type type) => 
            type.GetInterfaces()
                .FirstOrDefault(x => x.Name == "IValueObject`1" && x.GenericTypeArguments.Length == 1);

        public static Optional<Type> GetTargetFromValueObjectType(this Type clrType)
        {
            var interfaceType = GetValueObjectInterfaceType(clrType);

            if (interfaceType == null)
                return Optional<Type>.None;

            var targetClrType = interfaceType.GenericTypeArguments[0];
            return Optional<Type>.Some(targetClrType);
        }
    }
}