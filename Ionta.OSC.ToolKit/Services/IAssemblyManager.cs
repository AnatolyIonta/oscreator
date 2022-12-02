using System;
using System.Collections.Generic;
using System.Reflection;
using Ionta.OSC.ToolKit.Controllers;

namespace Ionta.OSC.ToolKit.Services
{
    public interface IAssemblyManager
    {
        void InitAssembly(params Assembly[] assemblies);
        Assembly[] GetAssemblies();
        event Action<Assembly[]> OnChange;
        public IEnumerable<ControllerInfo> GetControllers(params Assembly[] assemblies);
        public IEnumerable<Type> GetEntities(params Assembly[] assemblies);
    }
}