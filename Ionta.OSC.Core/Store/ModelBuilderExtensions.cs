using System.Reflection;

using Ionta.OSC.Core.Encrypt.EncryptColumn;
using Ionta.OSC.ToolKit.Encrypt;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ionta.OSC.Core.Store
{
    public static class ModelBuilderExtensions {
        public static void RegisterAllEntities<BaseModel>(this ModelBuilder modelBuilder, Assembly[] assemblies, IConfiguration configuration) {
            IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes()).Where(c => c.IsClass && !c.IsAbstract && c.IsPublic &&
                typeof (BaseModel).IsAssignableFrom(c));
            var encryptConverter = new EncryptionConverter(configuration);
            foreach (Type type in types)
            {
                SetEncrypte(modelBuilder, type, encryptConverter);
                modelBuilder.Entity(type).ToTable(type.Name.ToLower()).HasKey("Id");
            }
        }

        private static void SetEncrypte(ModelBuilder modelBuilder, Type type, EncryptionConverter converter)
        {
            foreach (var property in type.GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(typeof(EncryptColumn), false);
                if (attributes.Any())
                {
                    modelBuilder
                        .Entity(type)
                        .Property(property.Name)
                        .HasConversion(converter);
                }
            }
        }
    }
}