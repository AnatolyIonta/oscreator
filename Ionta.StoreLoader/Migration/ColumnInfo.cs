using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.StoreLoader.Migration
{
    internal class ColumnInfo
    {
        public string Name { get; set; }
        public ColumnType Type { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
