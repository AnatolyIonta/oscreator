using Ionta.OSC.App.Dtos;
using Ionta.OSC.App.Services.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.Auth
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, JWTDto>
    {
        private readonly IAuthService _authService;
        public LoginCommandHandler(IAuthService authService) 
        {
            _authService = authService;
        }
        public Task<JWTDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _authService.Handle(request.Login, request.Password);
        }
    }
}
