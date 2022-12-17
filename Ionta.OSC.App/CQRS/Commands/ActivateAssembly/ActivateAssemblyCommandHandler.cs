using Ionta.OSC.ToolKit.Services;
using MediatR;
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
            var assembly = _storage.AssemblyFiles.Single(a => a.Id == request.AssemblyId);
            if(request.IsActive != assembly.IsActive)
            {
                var assemblyData = AppDomain.CurrentDomain.Load(assembly.Data);
                if (request.IsActive)
                {
                    _assemblyManager.InitAssembly(assemblyData);
                }
                else
                {
                    _assemblyManager.UnloadingAssembly(assemblyData);
                }
                assembly.IsActive = request.IsActive;
                await _storage.SaveChangesAsync();
            }
            return true;
        }
    }
}
