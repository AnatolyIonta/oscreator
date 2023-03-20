using System;

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
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public string StackTace { get; set; }
        public LogType Type { get; set; }

    }
}