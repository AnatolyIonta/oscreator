using Ionta.OSC.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ionta.OSC.App
{
    public interface IOscStorage
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AssemblyFile> AssemblyFiles { get; set; }
        public DbSet<AssemblyPackage> AssemblyPackages { get; set; }
        public DbSet<JobInformation> Jobs { get; set; }

        public Task SaveChangesAsync();
        void ApplyMigrations();
    }
}
