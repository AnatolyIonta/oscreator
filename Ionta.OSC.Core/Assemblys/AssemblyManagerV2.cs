﻿using Ionta.OSC.Core.Assemblys.V2;
using Ionta.OSC.ToolKit.Controllers;
using Ionta.OSC.ToolKit.Store;

using System.Reflection;

namespace Ionta.OSC.Core.Assemblys
{
    
    public class AssemblyManagerV2 : IAssemblyManager
    {
        public event Action<Assembly[]> OnChange;
        public event Action<Assembly[]> OnUnloading;

        private readonly IAssemblyStore _assemblyStore;

        public AssemblyManagerV2(IAssemblyStore assemblyStore) 
        {
            _assemblyStore = assemblyStore;
            _assemblyStore.OnUnloading += (Assembly[] assemblies) => OnUnloading!.Invoke(assemblies);
            _assemblyStore.OnLoad += (Assembly[] assemblies) => OnChange!.Invoke(assemblies);
        }

        public Assembly[] GetAssemblies()
        {
            return _assemblyStore.GetAllAssembly().ToArray();
        }

        public IEnumerable<ControllerInfo> GetControllers(params Assembly[] assemblies)
        {
            return _assemblyStore.Get<ControllerInfo>();
        }

        public IEnumerable<Type> GetEntities(params Assembly[] assemblies)
        {
            return GetEntities();
        }

        public IEnumerable<Type> GetEntities()
        {
            return _assemblyStore.GetWithType<BaseEntity,Type>();
        }
    }
}
