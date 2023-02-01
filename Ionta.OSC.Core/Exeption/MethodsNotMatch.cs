using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Exeption
{
    public class MethodsNotMatch : Exception
    {
        public MethodsNotMatch() : base("Methods do not match") { }
    }
}
