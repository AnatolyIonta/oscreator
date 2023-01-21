using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Store.Migration
{
    internal enum ColumnType
    {
        Int,
        Long,
        Float,
        Decimal,
        Double,
        String,
        Boolean,
        Guid,
        None,
        Object,
        Data
    }
}
