﻿using Ionta.OSC.App.Dtos;
using Ionta.OSC.Core.AssembliesInformation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries.LogList
{
    public class LogListQueryHandler : IRequestHandler<LogListQuery, DtosList<LogDto>>
    {
        public IOscStorage _storage;
        public LogListQueryHandler(IOscStorage storage) 
        { 
            _storage = storage;
        }
        public async Task<DtosList<LogDto>> Handle(LogListQuery request, CancellationToken cancellationToken)
        {
            var data = _storage.Logs.Select(l => 
            new LogDto { Message = l.Message, Module = l.Module, Type = l.Type, StackTace = l.StackTace });

            var result = new DtosList<LogDto>()
            {
                Dtos = data.ToArray(),
                Count = data.Count()
            };

            return result;
        }
    }
}
