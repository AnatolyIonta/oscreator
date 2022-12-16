﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.DeleteModul
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
            var module = _storage.AssemblyFiles.First(a => a.Id == request.Id);

            if (module.IsActive) throw new Exception("Модуль активирован, его не возможно удалить! Для удаления деактивируйте модуль");

            _storage.AssemblyFiles.Remove(module);
            await _storage.SaveChangesAsync();

            return true;
        }
    }
}
