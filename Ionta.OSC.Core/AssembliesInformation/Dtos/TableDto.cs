using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.AssembliesInformation.Dtos
{
    public class TableDto
    {
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set;}
    }
}
