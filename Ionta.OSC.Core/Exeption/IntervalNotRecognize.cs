using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Exeption
{
    public class IntervalNotRecognize : Exception
    {
        public IntervalNotRecognize() : base("Interval not recognize") { }
    }
}
