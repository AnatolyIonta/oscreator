using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Exeption
{
    public class ManyParameters : Exception
    {
        public ManyParameters() : base("Too many parameters") { }
    }
}
