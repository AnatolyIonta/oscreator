using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ionta.OSC.ToolKit.Controllers;
using Ionta.OSC.ToolKit.Services;
using Ionta.OSC.ToolKit.Store;

namespace AssemblyLoader.Loader
{
    public class AssemblyManager : IAssemblyManager
    {
        private readonly List<Assembly> assemblies = new List<Assembly>();

        public Assembly[] GetAssemblies()
        {
            return assemblies.ToArray();
        }

        public event Action<Assembly[]> OnChange;
        public event Action<Assembly[]> OnUnloading;

        public void InitAssembly(params Assembly[] assemblies)
        {
            this.assemblies.AddRange(assemblies);
            try
            {
                OnChange?.Invoke(assemblies);
            }
            catch(Exception ex)
            {

            }
            finally{

            }
        }

        public void UnloadingAssembly(params Assembly[] assemblies)
        {
            this.assemblies.RemoveAll(a => assemblies.Contains(a));
            OnUnloading?.Invoke(assemblies);
        }

        public IEnumerable<Type> GetEntities(params Assembly[] assemblies)
        {
            var entityTypes = assemblies.SelectMany(a => a.GetExportedTypes()).Where(c => c.IsClass && !c.IsAbstract && c.IsPublic &&
                                                                        typeof(BaseEntity).IsAssignableFrom(c));
            return entityTypes;
        }

        public IEnumerable<Type> GetEntities()
        {
            var entityTypes = this.assemblies.SelectMany(a => a.GetExportedTypes()).Where(c => c.IsClass && !c.IsAbstract && c.IsPublic &&
                                                                        typeof(BaseEntity).IsAssignableFrom(c));
            return entityTypes;
        }

        public IEnumerable<ControllerInfo> GetControllers(params Assembly[] assemblies)
        {
            foreach (var assemble in assemblies)
            {
                var controllers = assemble.GetTypes()
                    .Where(a => a.GetCustomAttribute(typeof(ControllerAttribute)) != null);
                
                foreach (var controller in controllers)
                {
                    var methods = controller
                        .GetMethods()
                        .Where(m => m.GetCustomAttribute(typeof(PostAttribute)) != null);
                    yield return new ControllerInfo()
                    {
                        Handlers = methods.Select(m => new HandlerInfo()
                        {
                            Handler = m,
                            Path = ((PostAttribute)m.GetCustomAttribute(typeof(PostAttribute)))?.Path
                        }),
                        Path = ((ControllerAttribute)controller.GetCustomAttribute(typeof(ControllerAttribute)))?.Prefix,
                        Type = controller
                    };
                    
                }
            }
        }
    }
}