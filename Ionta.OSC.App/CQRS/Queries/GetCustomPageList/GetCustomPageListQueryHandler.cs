using Ionta.OSC.App.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries.GetCustomPageList
{
    internal class GetCustomPageListQueryHandler : IRequestHandler<GetCustomPageListQuery, DtosList<CustomPageDto>>
    {
        private readonly IOscStorage _storage;
        public GetCustomPageListQueryHandler(IOscStorage storage)
        {
            _storage = storage;
        }
        public async Task<DtosList<CustomPageDto>> Handle(GetCustomPageListQuery request, CancellationToken cancellationToken)
        {
            var customPages = _storage.AssemblyPackages.Include(a => a.customPages)
                .First(a => a.Id == request.Id).customPages;
            return new DtosList<CustomPageDto>()
            {
                Count = customPages.Count(),
                Dtos = customPages.Select(a => new CustomPageDto() 
                { Html = "", Name = a.Name, Url = a.Url }).ToArray()
            };
        }
    }
}
