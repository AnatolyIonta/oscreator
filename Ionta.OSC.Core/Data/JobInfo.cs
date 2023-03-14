using Ionta.OSC.ToolKit.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Data
{
    public class JobInfo
    {
        public required Type Job { get; set; }
        public required JobInterval IntervalType { get; set; }
        public required int Interval { get; set; }
    }
}
