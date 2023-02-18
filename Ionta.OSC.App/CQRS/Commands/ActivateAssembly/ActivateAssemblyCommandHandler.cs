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
using Ionta.OSC.Core.Assemblys.V2;

namespace Ionta.OSC.App.CQRS.Commands.ActivateAssembly
{
    public class ActivateAssemblyCommandHandler : IRequestHandler<ActivateAssemblyCommand, bool>
    {
        private readonly IOscStorage _storage;
        private readonly IAssemblyStore _assemblyManager;
        public ActivateAssemblyCommandHandler(IOscStorage storage, IAssemblyStore assemblyManager) 
        { 
            _storage= storage; 
            _assemblyManager= assemblyManager;
        }

        public async Task<bool> Handle(ActivateAssemblyCommand request, CancellationToken cancellationToken)
        {
            var assemblyPackage = _storage.AssemblyPackages.Include(e => e.Assembly).Single(a => a.Id == request.AssemblyId);
            if (request.IsActive != assemblyPackage.isActive)
            {
                if (request.IsActive)
                {
                    _assemblyManager.Load(assemblyPackage.Assembly.Select(e => e.Data), assemblyPackage.Id);
                }
                else
                {
                    _assemblyManager.Unload(assemblyPackage.Id);
                }
                foreach (var assembly in assemblyPackage.Assembly)
                {
                    assembly.IsActive = request.IsActive;
                }
            }
            assemblyPackage.isActive = request.IsActive;
            await _storage.SaveChangesAsync();
            return true;
        }
    }
}
