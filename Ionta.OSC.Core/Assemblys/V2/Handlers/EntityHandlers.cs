using Ionta.OSC.ToolKit.Store;
using System.Reflection;

namespace Ionta.OSC.Core.Assemblys.V2.Handlers
{
    public class EntityHandlers : IGetTypeHandler<Type>
    {
        public Type Type => typeof(BaseEntity);

        public IEnumerable<Type> Handle(Assembly assembly)
        {
            var entityTypes = assembly.GetExportedTypes().Where(c => c.IsClass && !c.IsAbstract && c.IsPublic &&
                                                                       typeof(BaseEntity).IsAssignableFrom(c));
            return entityTypes;
        }
    }
}