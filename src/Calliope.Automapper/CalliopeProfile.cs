using System;
using System.Linq;
using AutoMapper;

namespace Calliope.Automapper
{
    public class CalliopeProfile : Profile
    {
        /// <summary>
        /// This profile loads all <see cref="ValueObject{T}"/> references and registers standard Automapper converters for them. 
        /// </summary>
        public CalliopeProfile()
        {
            var valueObjectOpenGeneric = typeof(IValueObject<>);

            var valueObjectTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.DefinedTypes)
                .Where(t => t.FindInterfacesThatClose(valueObjectOpenGeneric).Any())
                .Where(type => type.IsConcrete())
                .ToList();

            foreach (var valueObjectType in valueObjectTypes)
            {
                var genericInterfaceDefinition = valueObjectType
                        .ImplementedInterfaces
                        .FirstOrDefault(x => x.GenericTypeArguments.Length > 0);
                
                if(genericInterfaceDefinition is null)
                    continue;
                
                var targetingType = genericInterfaceDefinition.GenericTypeArguments.First();

                RegisterFromValueObjectToTarget(valueObjectType, targetingType);
                RegisterFromTargetToValueObject(valueObjectType, targetingType);
            }
        }

        private void RegisterFromValueObjectToTarget(Type valueObjectType, Type targetingType)
        {
            var converterType = typeof(FromValueObjectToValueConverter<,>);
            var genericConverter = converterType.MakeGenericType(valueObjectType, targetingType);
                
            CreateMap(valueObjectType, targetingType).ConvertUsing(genericConverter);
        }

        private void RegisterFromTargetToValueObject(Type valueObjectType, Type targetingType)
        {
            var converterType = typeof(FromTargetToValueObjectConverter<,>);
            var genericConverter = converterType.MakeGenericType(targetingType, valueObjectType);
                
            CreateMap(targetingType, valueObjectType).ConvertUsing(genericConverter);
        }
    }
}