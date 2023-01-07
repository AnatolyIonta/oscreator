using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ionta.OSC.App.CQRS.Commands.Auth;
using Ionta.OSC.App.CQRS.Commands.ChangePassword;
using Ionta.OSC.App.Dtos;
using Ionta.OSC.App.Services.Auth;
using Ionta.OSC.Core.Assemblys;
using Ionta.OSC.ToolKit.Store;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
