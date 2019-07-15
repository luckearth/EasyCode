using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EasyCode.Core.Data.Extensions
{
    public static class EntityTypeConfigurationExtensions
    {
        private static readonly MethodInfo entityMethod = typeof(ModelBuilder).GetTypeInfo().GetMethods().Single(x => (x.Name == "Entity") && (x.IsGenericMethod == true) && (x.GetParameters().Length == 0));

        private static Type FindEntityType(Type type)
        {
            var interfaceType = type.GetInterfaces().First(x => (x.GetTypeInfo().IsGenericType == true) && (x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
            return interfaceType.GetGenericArguments().First();
        }

        private static readonly Dictionary<Assembly, IEnumerable<Type>> typesPerAssembly = new Dictionary<Assembly, IEnumerable<Type>>();

        public static ModelBuilder ApplyConfiguration<T>(this ModelBuilder modelBuilder, IEntityTypeConfiguration<T> configuration) where T : class
        {
            var entityType = FindEntityType(configuration.GetType());

            dynamic entityTypeBuilder = entityMethod
                .MakeGenericMethod(entityType)
                .Invoke(modelBuilder, new object[0]);

            configuration.Configure(entityTypeBuilder);

            return modelBuilder;
        }

        public static ModelBuilder UseEntityTypeConfiguration(this ModelBuilder modelBuilder, Type type)
        {
            IEnumerable<Type> configurationTypes;
            var asm = type.Assembly;

            if (typesPerAssembly.TryGetValue(asm, out configurationTypes) == false)
            {
                typesPerAssembly[asm] = configurationTypes = asm
                    .GetExportedTypes()
                    .Where(x => (x.GetTypeInfo().IsClass == true) && (x.GetTypeInfo().IsAbstract == false) && (x.GetInterfaces().Any(y => (y.GetTypeInfo().IsGenericType == true) && (y.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))));
            }

            var configurations = configurationTypes.Select(x => Activator.CreateInstance(x));

            foreach (dynamic configuration in configurations)
            {
                ApplyConfiguration(modelBuilder, configuration);
            }

            return modelBuilder;
        }
    }
}
