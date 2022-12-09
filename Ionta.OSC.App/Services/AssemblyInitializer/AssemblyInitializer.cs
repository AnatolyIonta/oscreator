﻿using Ionta.OSC.ToolKit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Services.AssemblyInitializer
{
    public class AssemblyInitializer : IAssemblyInitializer
    {
        private readonly IOscStorage _storage;
        private readonly IAssemblyManager _assemblyManager;
        public AssemblyInitializer(IOscStorage storage, IAssemblyManager assemblyManager) 
        { 
            _storage = storage;
            _assemblyManager = assemblyManager;
        }
        public void Initialize()
        {
            var activeAssemlies = _storage.AssemblyFiles.Where(a => a.IsActive);
            foreach(var file in activeAssemlies)
            {
                var assembly = Assembly.Load(file.Data);
                _assemblyManager.InitAssembly(assembly);
            }
        }
    }
}
