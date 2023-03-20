using Ionta.OSC.App.Services.Scheduler;
using Ionta.OSC.Core.Assemblys.V2;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Reflection;

namespace Ionta.OSC.App.Services.AssemblyInitializer
{
    public class AssemblyInitializer : IAssemblyInitializer
    {
        private readonly IOscStorage _storage;
        private readonly IAssemblyStore _assemblyStore;
        public AssemblyInitializer(IOscStorage storage, IAssemblyStore assemblyStore, IScheduler scheduler) 
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
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AppDomain domain = (AppDomain)sender;
            foreach (Assembly asm in domain.GetAssemblies())
            {
                if (asm.FullName == args.Name)
                {
                    return asm;
                }
            }
            throw new ApplicationException($"Can't find assembly {args.Name}");
        }

    }
}
