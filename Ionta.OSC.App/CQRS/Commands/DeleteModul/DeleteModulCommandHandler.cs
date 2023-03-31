using MediatR;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class DeleteModulCommandHandler : IRequestHandler<DeleteModulCommand, bool>
    {
        private readonly IOscStorage _storage;
        public DeleteModulCommandHandler(IOscStorage storage) 
        { 
            _storage = storage;
        }

        public async Task<bool> Handle(DeleteModulCommand request, CancellationToken cancellationToken)
        {
            var module = _storage.AssemblyPackages.Include(e => e.Assembly).Include(a => a.customPages).First(a => a.Id == request.Id);

            if (module.IsActive) throw new Exception("Модуль активирован, его не возможно удалить! Для удаления деактивируйте модуль");

            _storage.AssemblyFiles.RemoveRange(module.Assembly);
            _storage.CustomPages.RemoveRange(module.customPages);
            _storage.AssemblyPackages.Remove(module);
            await _storage.SaveChangesAsync();

            return true;
        }
    }
}