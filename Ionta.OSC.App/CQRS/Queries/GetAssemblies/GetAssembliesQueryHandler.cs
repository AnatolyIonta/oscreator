using Ionta.OSC.App.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries.GetAssemblies
{
    public class GetAssembliesQueryHandler : IRequestHandler<GetAssembliesQuery, DtosList<AssemblyDto>>
    {
        private readonly IOscStorage _storage;
        public GetAssembliesQueryHandler(IOscStorage storage) 
        { 
            _storage = storage;
        }
        public Task<DtosList<AssemblyDto>> Handle(GetAssembliesQuery request, CancellationToken cancellationToken)
        {
            var assemblyCollection = _storage.AssemblyPackages.Select(a => new AssemblyDto() { IsActive= a.IsActive, Name = a.Name, Id = a.Id });

            return Task.FromResult(new DtosList<AssemblyDto>()
            {
                Count = assemblyCollection.Count(),
                Dtos = assemblyCollection.ToArray()
            });
        }
    }
}
