using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.AssembliesInformation.Dtos
{
    public class ControllerDto
    {
        public List<MethodDto> Methods { get; set; }
        public bool IsAuth { get; set; }
    }
}
