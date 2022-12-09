using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Domain
{
    public class AssemblyFile : BaseEntity
    {
        public string AssemblyName { get; set; } 
        public byte[] Data { get; set; }
        public bool IsActive { get; set; }
    }
}
