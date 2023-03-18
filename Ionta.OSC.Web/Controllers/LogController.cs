using Ionta.OSC.App.CQRS.Queries.LogList;
using Ionta.OSC.App.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ionta.OSC.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogsController
    {
        private readonly IMediator _mediator;

        public LogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(DtosList<LogDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("list")]
        public async Task<DtosList<LogDto>> List(LogListQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
