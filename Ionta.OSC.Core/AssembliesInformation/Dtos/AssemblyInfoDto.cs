using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.AssembliesInformation.Dtos
{
    public class AssemblyInfoDto
    {
        public ControllerDto[] Controllers { get; set; }
        public TableDto[] Tables { get; set; }
    }
}
