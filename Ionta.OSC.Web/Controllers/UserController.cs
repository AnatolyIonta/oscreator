using Ionta.OSC.App.CQRS.Commands;
using Ionta.OSC.App.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenServiceCreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        //private readonly IAuthService _authService;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
            //_authService = authService;
        }

        [ProducesResponseType(typeof(JWTDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("login")]
        public async Task<JWTDto> Login([FromBody] LoginCommand query)
        {
            return await _mediator.Send(query);
        }

        [Authorize]
        [ProducesResponseType(typeof(JWTDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("сhangepassword")]
        public async Task ChangePassword([FromBody] ChangePasswordCommand command)
        {
            await _mediator.Send(command);
        }

        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("status")]
        public string Health()
        {
            return "Health";
        }
    }
}