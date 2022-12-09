using Ionta.OSC.App.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.CQRS.Commands.LoadAssembly
{
    public class LoadAssemblyCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
