using Ionta.OSC.Domain;
using System;

namespace Ionta.OSC.App.Dtos
{
    public class LogDto
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string StackTace { get; set; }
        public LogType Type { get; set; }
    }
}