using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Domain
{
    public class CustomPage : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }
    }
}
