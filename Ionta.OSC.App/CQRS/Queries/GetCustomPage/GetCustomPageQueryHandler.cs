using Ionta.OSC.App.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries.GetCustomPage
{
    public class GetCustomPageQueryHandler : IRequestHandler<GetCustomPageQuery, CustomPageDto>
    {
        private readonly IOscStorage _storage;
        public GetCustomPageQueryHandler(IOscStorage storage) 
        { 
            _storage = storage;
        }
        public Task<CustomPageDto> Handle(GetCustomPageQuery request, CancellationToken cancellationToken)
        {
            var assemblyPackages = _storage.AssemblyPackages.Include(a => a.customPages)
                .FirstOrDefault(a => a.customPages.Any(c => c.Url.ToLower().Replace(" ", "") == request.url.ToLower()));
            if (assemblyPackages != null && assemblyPackages.IsActive)
            {
                var page = assemblyPackages.customPages.First(c => c.Url.ToLower().Replace(" ", "") == request.url.ToLower());
                return Task.FromResult(new CustomPageDto() { Html = page.Html, Url = page.Url, Name = page.Name });
            }
            
            return Task.FromResult(new CustomPageDto() 
                {Html= "<h1>Страница не найдена!</h1>", Url = request.url, Name="NotFound" });
        }
    }
}
