using Ionta.OSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Dtos
{
    public class LogDto
    {
        public string Module { get; set; }
        public string Message { get; set; }
        public string StackTace { get; set; }
        public LogType Type { get; set; }
    }
}
