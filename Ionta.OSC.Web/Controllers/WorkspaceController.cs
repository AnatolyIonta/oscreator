using Ionta.OSC.App.CQRS.Queries.GetCustomPage;
using Ionta.OSC.App.CQRS.Queries.GetCustomPageList;
using Ionta.OSC.App.Dtos;
using Ionta.OSC.ToolKit.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ionta.OSC.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class WorkspaceController : ControllerBase
    {
        private readonly IMediator _mediator;
        //private readonly IAuthService _authService;
        public WorkspaceController(IMediator mediator)
        {
            _mediator = mediator;
            //_authService = authService;
        }
        [HttpPost("getpage")]
        public Task<CustomPageDto> GetCustomPage(GetCustomPageQuery query)
        {
            return _mediator.Send(query);
        }
        [HttpPost("getpages")]
        public Task<DtosList<CustomPageDto>> GetPages(GetCustomPageListQuery query)
        {
            return _mediator.Send(query);
        }
    }
}
