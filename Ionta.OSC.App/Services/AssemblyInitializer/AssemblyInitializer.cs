using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.Core.Assemblys.V2;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAssemblyStore _assemblyStore;
        public AssemblyInitializer(IOscStorage storage, IAssemblyStore assemblyStore) 
        { 
            _storage = storage;
            _assemblyStore = assemblyStore;
        }
        public void Initialize()
        {
            var activePackages = _storage.AssemblyPackages.Include(e => e.Assembly).Where(e => e.IsActive);
            foreach(var packages in activePackages)
            {
                var data = packages.Assembly.Select(e => e.Data);
                _assemblyStore.Load(data, packages.Id);
            }
        }
    }
}
