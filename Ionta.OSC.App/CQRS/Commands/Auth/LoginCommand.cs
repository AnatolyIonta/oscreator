using Ionta.OSC.App.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.Auth
{
    public class LoginCommand : IRequest<JWTDto>
    {
        public string Login { get; set; }
        public string Password { get; set; }

    }
}
