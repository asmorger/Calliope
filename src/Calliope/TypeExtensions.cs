using System;
using System.Linq;
using System.Reflection;

namespace Calliope
{
    public static class TypeExtensions
    {
        private static readonly Type ValueObjectMarkerInterface = typeof(IValueObject);
        private static readonly Type PrimitiveValueObjectMarkerInterface = typeof(IPrimitiveValueObject<>);
        
        public static bool HasNestedValueObjects(this Type type) => type.GetProperties().Any(IsValueObject);

        public static bool IsValueObject(this PropertyInfo i) => IsValueObject(i.PropertyType);
        
        public static bool IsValueObject(this Type type)
        {
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface == ValueObjectMarkerInterface)
                    return true;
            }

            return false;
        }

        public static bool IsPrimitiveValueObject(this Type type) =>
            ImplementsGenericInterface(type, PrimitiveValueObjectMarkerInterface) && !type.IsAbstract;

        public static bool IsPrimitiveValueObject(this PropertyInfo i) => ImplementsPrimitiveValueObject(i.PropertyType);

        private static bool ImplementsPrimitiveValueObject(Type typeToCheck) => 
            ImplementsGenericInterface(typeToCheck, PrimitiveValueObjectMarkerInterface);
        
        private static bool ImplementsGenericInterface(Type typeToCheck, Type genericInterface)
        {
            foreach (var @interface in typeToCheck.GetInterfaces())
            {
                if (!@interface.IsGenericType)
                {
                    continue;
                }

                if (@interface.GetGenericTypeDefinition() == genericInterface)
                {
                    return true;
                }
            }

            return false;
        }

        private static Optional<Type> GetPrimitiveValueObjectInterfaceType(this Type type)
        {
            var @interface = type.GetInterfaces().FirstOrDefault(t =>
                t.IsGenericType && t.GetGenericTypeDefinition() == PrimitiveValueObjectMarkerInterface);
            
            if(@interface == null)
                return Optional<Type>.None;

            var output = @interface.GetGenericArguments()[0];
            
            return Optional.Some(output);
        }

        public static Optional<Type> GetTargetFromValueObjectType(this Type clrType)
        {
            if(!ImplementsPrimitiveValueObject(clrType))
                return Optional<Type>.None;
            
            var interfaceType = GetPrimitiveValueObjectInterfaceType(clrType);
            return interfaceType;
        }
    }
}