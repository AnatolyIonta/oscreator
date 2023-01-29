using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Exeption
{
    public class MethodNotFound : Exception
    {
        public MethodNotFound() : base("Method not found") { }
    }
}
