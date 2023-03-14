using System;

namespace Ionta.OSC.Domain
{
    public class JobInformation : BaseEntity
    {
        public string Name { get; set; }
        public DateTime NextExecute { get; set; }
    }
}
