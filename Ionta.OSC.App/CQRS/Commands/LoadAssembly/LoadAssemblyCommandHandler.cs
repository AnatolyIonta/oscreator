using Ionta.OSC.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.LoadAssembly
{
    internal class LoadAssemblyCommandHandler : IRequestHandler<LoadAssemblyCommand, bool>
    {
        private readonly IOscStorage _storage;
        public LoadAssemblyCommandHandler(IOscStorage storage) 
        {
            _storage = storage;
        }
        public async Task<bool> Handle(LoadAssemblyCommand request, CancellationToken cancellationToken)
        {
            var AssemblyFile = new AssemblyFile() {AssemblyName = request.Name, Data = request.Data };
            _storage.AssemblyFiles.Add(AssemblyFile);
            await _storage.SaveChangesAsync();
            return true;
        }
    }
}
