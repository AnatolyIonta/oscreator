using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.DeleteModul
{
    public class DeleteModulCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }
}
