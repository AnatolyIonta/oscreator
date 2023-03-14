using static Ionta.OSC.App.CQRS.ProcessingZipArchive;
using Ionta.OSC.Core.Assemblys.V2;
using Ionta.OSC.Domain;

using Microsoft.EntityFrameworkCore;
using MediatR;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands 
{
    public class RefreshModulCommandHandler : IRequestHandler<RefreshModulCommand, bool>
    {
        private readonly IOscStorage _storage;
        private readonly IAssemblyStore _assemblyManager;

        public RefreshModulCommandHandler(IOscStorage storage, IAssemblyStore assemblyManager) 
        {
            _storage = storage;
            _assemblyManager = assemblyManager;
        }

        public async Task<bool> Handle(RefreshModulCommand request, CancellationToken cancellationToken)
        {
            #region Удаление файлов модуля
            var module = _storage.AssemblyPackages
                .Include(e => e.Assembly)
                .FirstOrDefault(a => a.Id == request.Id);

            if (module == null) throw new Exception("Невозможно обновить модуль! Модуль не найден!");
            if (module.IsActive)
            {
                _assemblyManager.Unload(module.Id);
                foreach (var assembly in module.Assembly)
                {
                    assembly.IsActive = false;
                }
                module.IsActive = false;
            }
            
            _storage.AssemblyFiles.RemoveRange(module.Assembly);
            #endregion

            var AllFilese = GetZipeFile(request.Data);
            foreach (var file in AllFilese)
            {
                var AssemblyFile = new AssemblyFile() { AssemblyName = Guid.NewGuid().ToString(), Data = file };
                _storage.AssemblyFiles.Add(AssemblyFile);
                module.Assembly.Add(AssemblyFile);
            }

            await _storage.SaveChangesAsync();
            return true;
        }
    }
}