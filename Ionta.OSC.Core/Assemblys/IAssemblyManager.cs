using System.Reflection;
using Ionta.OSC.ToolKit.Controllers;

namespace Ionta.OSC.Core.Assemblys
{
    public interface IAssemblyManager
    {
        Assembly[] GetAssemblies();
        event Action<Assembly[]> OnChange;
        event Action<Assembly[]> OnUnloading;
        public IEnumerable<ControllerInfo> GetControllers(params Assembly[] assemblies);
        public IEnumerable<Type> GetEntities(params Assembly[] assemblies);
        public IEnumerable<Type> GetEntities();
    }
}