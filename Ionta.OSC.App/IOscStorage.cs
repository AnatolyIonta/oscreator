using Ionta.OSC.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.App
{
    public interface IOscStorage
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AssemblyFile> AssemblyFiles { get; set; }
        public DbSet<AssemblyPackage> AssemblyPackages { get; set; }

        public Task SaveChangesAsync();
    }
}
