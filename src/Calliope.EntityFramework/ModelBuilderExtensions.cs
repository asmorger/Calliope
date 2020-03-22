using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Calliope.EntityFramework
{
    public static class ModelBuilderExtensions
    {
        private static Type GetValueObjectInterfaceType(Type type) => 
            type.GetInterfaces()
                .FirstOrDefault(x => x.Name == "IValueObject`1" && x.GenericTypeArguments.Length == 1);
        
        private static bool IsValueObject(PropertyInfo property)
        {
            var targetInterface = GetValueObjectInterfaceType(property.PropertyType);
            var doesMatchInterface = targetInterface != null;
            return doesMatchInterface;
        }

        private static Optional<Type> GetTargetFromValueObjectType(Type clrType)
        {
            var interfaceType = GetValueObjectInterfaceType(clrType);

            if (interfaceType == null)
                return Optional<Type>.None;

            var targetClrType = interfaceType.GenericTypeArguments[0];
            return Optional<Type>.Some(targetClrType);
        }

        public static void AddValueObjectConversions(this ModelBuilder modelBuilder, bool skipConventionalEntities = true)
        {
            if (modelBuilder is null)
                throw new ArgumentNullException(nameof(modelBuilder));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ProcessEntity(entityType, modelBuilder, skipConventionalEntities);
            }
        }

        private static void ProcessEntity(IMutableEntityType entityType, ModelBuilder modelBuilder, bool skipConventionalEntities)
        {
            var typeBase = typeof(TypeBase);
            if (skipConventionalEntities)
            {
                var typeConfigurationSource = typeBase
                    .GetField("_configurationSource", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.GetValue(entityType)?.ToString();

                if (Enum.TryParse(typeConfigurationSource, out ConfigurationSource typeSource) &&
                    typeSource == ConfigurationSource.Convention)
                    return;
            }

            var ignoredMembers = (Dictionary<string, ConfigurationSource>) typeBase
                .GetField("_ignoredMembers", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(entityType);

            bool IsIgnored(PropertyInfo property) =>
                property != null && ignoredMembers != null && ignoredMembers.ContainsKey(property.Name) &&
                property.CustomAttributes.Any(a => a.AttributeType != typeof(NotMappedAttribute));

            var propertiesToProcess = entityType.ClrType.GetProperties().Where(x => !IsIgnored(x) && IsValueObject(x));

            foreach (var clrProperty in propertiesToProcess)
            {
                ProcessProperty(entityType.ClrType, clrProperty, modelBuilder);
            }
        }

        private static void ProcessProperty(Type clrType, PropertyInfo clrProperty, ModelBuilder modelBuilder)
        {
            var property = modelBuilder.Entity(clrType)
                .Property(clrProperty.PropertyType, clrProperty.Name);
                    
            var valueObjectType = clrProperty.PropertyType;
            var targetType = GetTargetFromValueObjectType(valueObjectType);

            if (targetType.IsNone())
            {
                return;
            }
            
            var converterType = typeof(ValueObjectConverter<,>).MakeGenericType(valueObjectType, targetType.Unwrap());
            var converter = (ValueConverter) Activator.CreateInstance(converterType, new object[] {null});
            property.Metadata.SetValueConverter(converter);
        }
    }
}