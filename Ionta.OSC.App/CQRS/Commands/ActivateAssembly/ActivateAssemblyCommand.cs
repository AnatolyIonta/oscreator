using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.ActivateAssembly
{
    public class ActivateAssemblyCommand : IRequest<bool>
    {
        public int AssemblyId { get; set; } 
        public bool IsActive { get; set; }
    }
}
