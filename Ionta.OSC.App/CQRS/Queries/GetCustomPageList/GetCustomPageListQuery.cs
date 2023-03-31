using Ionta.OSC.App.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Queries.GetCustomPageList
{
    public class GetCustomPageListQuery : IRequest<DtosList<CustomPageDto>>
    {
        public long Id { get; set; }
    }
}
