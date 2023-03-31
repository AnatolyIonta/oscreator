using System.Collections.Generic;

namespace Ionta.OSC.Domain
{
    public class AssemblyPackage : BaseEntity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public List<AssemblyFile> Assembly { get; set; }
        public List<CustomPage> customPages { get; set; }
        public bool IsActive { get; set; }
    }
}