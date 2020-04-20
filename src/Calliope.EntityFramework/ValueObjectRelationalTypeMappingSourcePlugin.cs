using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;

namespace Calliope.EntityFramework
{
    public class PrimitiveValueObjectMapping<TPrimitiveValueType, TSource> : RelationalTypeMapping
        where TPrimitiveValueType : IPrimitiveValueObject<TSource>
    {
        private static readonly ValueObjectConverter<TPrimitiveValueType, TSource> _converter = 
            new ValueObjectConverter<TPrimitiveValueType, TSource>();

        public PrimitiveValueObjectMapping() : 
            base(new RelationalTypeMappingParameters(
                new CoreTypeMappingParameters(typeof(TPrimitiveValueType), _converter), "text" ))
        {
        }
        
        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => 
            new PrimitiveValueObjectMapping<TPrimitiveValueType, TSource>();
    }
    
    public class ValueObjectMappingFactory
    {
        private readonly IDictionary<Type, RelationalTypeMapping> _cache = new Dictionary<Type, RelationalTypeMapping>();
        
        public ValueObjectMappingFactory()
        {
            var assemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic);

            var primitiveValueObjects = assemblies.SelectMany(x => x.ExportedTypes.Where(t => t.IsPrimitiveValueObject()));

            foreach (var valueObjectType in primitiveValueObjects)
            {
                var interfaceType = valueObjectType.GetTargetFromValueObjectType();

                if (interfaceType.IsNone())
                    continue;

                var valueObjectTargetType = interfaceType.Unwrap();

                var genericType = typeof(PrimitiveValueObjectMapping<,>).MakeGenericType(valueObjectType, valueObjectTargetType);
                var instance = Activator.CreateInstance(genericType);
                
                _cache.Add(valueObjectType, (RelationalTypeMapping) instance);
            }
        }
        
        public RelationalTypeMapping GetMapping(Type clrType)
        {
            if (_cache.ContainsKey(clrType))
                return _cache[clrType];
            
            return null;
        }
    }
    
    public class ValueObjectRelationalTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
    {
        private readonly ValueObjectMappingFactory _factory;
        public ValueObjectRelationalTypeMappingSourcePlugin()
        {
            _factory = new ValueObjectMappingFactory();
        }
        public RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo) =>
            _factory.GetMapping(mappingInfo.ClrType);
    }
}