using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Core.Store
{
    public interface IMigrationGenerator
    {
        public void ApplayMigrations();
    }
}
