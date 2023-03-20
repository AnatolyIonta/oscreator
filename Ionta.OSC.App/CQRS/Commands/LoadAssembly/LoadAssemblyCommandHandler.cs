using static Ionta.Utilities.ProcessingZipArchive;
using Ionta.OSC.Domain;
using MediatR;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Ionta.OSC.App.CQRS.Commands
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
            var AllFilese = GetZipeFile(request.Data);
            var package = new AssemblyPackage() { Assembly = new List<AssemblyFile>(), Name = request.Name.Replace(".zip","") };
            foreach (var file in AllFilese)
            {
                var AssemblyFile = new AssemblyFile() { AssemblyName = Guid.NewGuid().ToString(), Data = file };
                _storage.AssemblyFiles.Add(AssemblyFile);
                package.Assembly.Add(AssemblyFile);
            }
            _storage.AssemblyPackages.Add(package);
            await _storage.SaveChangesAsync();
            return true;
        }
    }
}