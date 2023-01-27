using Ionta.OSC.Core.Assemblys;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.ActivateAssembly
{
    public class ActivateAssemblyCommandHandler : IRequestHandler<ActivateAssemblyCommand, bool>
    {
        private readonly IOscStorage _storage;
        private readonly IAssemblyManager _assemblyManager;
        public ActivateAssemblyCommandHandler(IOscStorage storage, IAssemblyManager assemblyManager) 
        { 
            _storage= storage; 
            _assemblyManager= assemblyManager;
        }

        public async Task<bool> Handle(ActivateAssemblyCommand request, CancellationToken cancellationToken)
        {
            var assemblyPackage = _storage.AssemblyPackages.Include(e => e.Assembly).Single(a => a.Id == request.AssemblyId);
            if (request.IsActive != assemblyPackage.isActive)
            {
                foreach (var assembly in assemblyPackage.Assembly)
                {
                    if (request.IsActive)
                    {
                        var assemblyData = AppDomain.CurrentDomain.Load(assembly.Data);
                        _assemblyManager.InitAssembly(assemblyData);
                    }
                    else
                    {
                        _assemblyManager.UnloadingAssembly(Assembly.Load(assembly.Data));
                    }
                    assembly.IsActive = request.IsActive;
                }
            }
            assemblyPackage.isActive = request.IsActive;
            await _storage.SaveChangesAsync();
            return true;
        }
    }
}
