using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionta.OSC.App.Dtos;
using MediatR;

namespace Ionta.OSC.App.CQRS.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}
