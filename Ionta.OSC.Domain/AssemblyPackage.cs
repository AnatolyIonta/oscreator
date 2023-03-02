using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Domain
{
    public class AssemblyPackage : BaseEntity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public List<AssemblyFile> Assembly { get; set; }
        public bool IsActive { get; set; }
    }
}
