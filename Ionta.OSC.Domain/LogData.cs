using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Domain
{
    public enum LogType
    {
        Secsses,
        Error,
        Warrning
    }
    public class LogData : BaseEntity
    {
        public string Module { get; set; }
        public string Message { get; set; }
        public string StackTace { get; set; }
        public LogType Type { get; set; }

    }
}
