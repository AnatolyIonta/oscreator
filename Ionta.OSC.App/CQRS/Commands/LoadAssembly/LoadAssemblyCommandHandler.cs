using static Ionta.Utilities.ProcessingZipArchive;
using Ionta.OSC.Domain;
using MediatR;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

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
            var customPages = GetFilesFromFolder(request.Data, "Sections", ".html");
            var package = new AssemblyPackage() { Assembly = new(), customPages = new(), Name = request.Name.Replace(".zip","") };
            foreach (var file in AllFilese)
            {
                var AssemblyFile = new AssemblyFile() { AssemblyName = Guid.NewGuid().ToString(), Data = file };
                _storage.AssemblyFiles.Add(AssemblyFile);
                package.Assembly.Add(AssemblyFile);
            }
            foreach(var customPage in customPages)
            {
                var data = Encoding.Default.GetString(customPage).Split(@"/*&*\",3);
                package.customPages.Add(new CustomPage() { Html = data[2], Name = data[0], Url = Regex.Replace(data[1], @"[\n\r]", "") });
            }
            _storage.AssemblyPackages.Add(package);
            await _storage.SaveChangesAsync();
            return true;
        }
    }
}