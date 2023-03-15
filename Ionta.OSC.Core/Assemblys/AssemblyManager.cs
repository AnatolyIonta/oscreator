using System.Reflection;
using Ionta.OSC.ToolKit.Controllers;
using Ionta.OSC.ToolKit.Store;
using Ionta.OSC.ToolKit.Auth;
using Microsoft.Extensions.Caching.Memory;

namespace Ionta.OSC.Core.Assemblys
{
    public class AssemblyManager : IAssemblyManager
    {
        private readonly List<Assembly> assemblies = new List<Assembly>();
        private readonly IMemoryCache _cache;

        public AssemblyManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Assembly[] GetAssemblies()
        {
            return assemblies.ToArray();
        }

        public event Action<Assembly[]> OnChange;
        public event Action<Assembly[]> OnUnloading;

        public void InitAssembly(params Assembly[] assemblies)
        {
            ResetCache();
            foreach (var assembly in assemblies)
            {
                if (!this.assemblies.Any(e => e.FullName == assembly.FullName))
                {
                    this.assemblies.Add(assembly);
                }
            }
            try
            {
                OnChange?.Invoke(assemblies);
            }
            catch(Exception ex)
            {

            }
        }

        public void ResetCache()
        {
            _cache.Remove("controllers");
        }

        public void UnloadAssembli(params Assembly[] assemblies)
        {
            ResetCache();
            foreach (var assembly in assemblies)
            {
                this.assemblies.Remove(assembly);
            }
            OnUnloading?.Invoke(assemblies);
        }

        public void UnloadingAssembly(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var target = this.assemblies.FirstOrDefault(a => a.FullName == assembly.FullName);
                if (target == null) return;
                this.assemblies.RemoveAll(e => e.FullName == target.FullName);
            }
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
                        .Where(m => m.GetCustomAttribute(typeof(PostAttribute)) != null || m.GetCustomAttribute(typeof(GetAttribute)) != null);
                    yield return new ControllerInfo()
                    {
                        Handlers = methods.Select(m => { 
                            var attribute = m.GetCustomAttribute(typeof(PostAttribute)) ?? m.GetCustomAttribute(typeof(GetAttribute));
                            return new HandlerInfo()
                            {
                                Handler = m,
                                Path = ((IMethodAttribute)attribute).Path,
                                Method = ((IMethodAttribute)attribute).Method
                            };
                            }
                        ),
                        Path = ((ControllerAttribute)controller.GetCustomAttribute(typeof(ControllerAttribute)))?.Prefix,
                        Type = controller,
                        Authorize = controller.GetCustomAttribute(typeof(AuthorizeAttribute)) != null
                    };
                    
                }
            }
        }
    }
}