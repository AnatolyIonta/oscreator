using MediatR;
using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands
{
    public class ChangeNameAssemblyCommandHandler : IRequestHandler<ChangeNameAssemblyCommand, bool>
    {
        private readonly IOscStorage _storage;

        public ChangeNameAssemblyCommandHandler (IOscStorage storage) 
        { 
            _storage = storage;
        }

        public async Task<bool> Handle(ChangeNameAssemblyCommand request, CancellationToken cancellationToken)
        {
            #region Проверка входящего значения
                if (request.Name == "") throw new Exception("Название модуля не может быть пустым!");

                var module = await _storage.AssemblyPackages
                    .FirstOrDefaultAsync(a => a.Name == request.Name, cancellationToken);
                if (module != null) throw new Exception("Невозможно изменить название модуля, поскольку модуль с таким именем уже существует!");

                    module = await _storage.AssemblyPackages
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
                if (module == null) throw new Exception("Модуль не найден! Невозможно изменить название модуля!");
            #endregion

            module.Name = request.Name;
            await _storage.SaveChangesAsync();

            return true;
        }
    }
}