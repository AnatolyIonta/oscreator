using Ionta.OSC.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

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
            var AllFilese = GetZipeFile(request.Data);
            var package = new AssemblyPackage() { Assembly = new List<AssemblyFile>(), Name = request.Name};
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

        public IEnumerable<byte[]> GetZipeFile(byte[] zip)
        {
            using (Stream data = new MemoryStream(zip)) // The original data
            { 
                Stream unzippedEntryStream; // Unzipped data from a file in the archive


                ZipArchive archive = new ZipArchive(data);
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return ReadFully(entry.Open()); // .Open will return a stream                                                       // Process entry data here
                    }
                }
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
