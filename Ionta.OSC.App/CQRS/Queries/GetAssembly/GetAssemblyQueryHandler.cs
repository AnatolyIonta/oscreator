﻿using Ionta.OSC.App;
using Ionta.OSC.App.Dtos;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ionta.OSC.Core.AssembliesInformation
{
    public class GetAssemblyQueryHandler : IRequestHandler<GetAssemblyQuery, AssemblyDto>
    {
        private readonly IOscStorage _storage;

        public GetAssemblyQueryHandler(IOscStorage storage)
        {
            _storage = storage;
        }

        public async Task<AssemblyDto> Handle(GetAssemblyQuery request, CancellationToken cancellationToken)
        {
            var assembly = await _storage.AssemblyPackages
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if(assembly == null) throw new Exception("Модуль не найден!");
            
            var result = new AssemblyDto
            { 
                Id = assembly.Id,
                Name = assembly.Name,
                IsActive = assembly.IsActive
            };

            return result;
        }
    }
}