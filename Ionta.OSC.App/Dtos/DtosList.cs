using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App.Dtos
{
    public class DtosList<T>
    {
        public int Count { get; set; }
        public T[] Dtos { get; set; }
    }
}
