using Ionta.OSC.ToolKit.Scheduler;

namespace Ionta.OSC.Core.Data
{
    public class JobInfo
    {
        public required Type Job { get; set; }
        public required JobInterval IntervalType { get; set; }
        public required int Interval { get; set; }
    }
}