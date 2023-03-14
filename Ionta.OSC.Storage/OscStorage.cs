using Ionta.OSC.App;
using Ionta.OSC.App.Services.HashingPassword;
using Ionta.OSC.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Storage
{
    public class OscStorage : DbContext, IOscStorage
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AssemblyFile> AssemblyFiles { get; set; }
        public DbSet<AssemblyPackage> AssemblyPackages { get; set; }
        public DbSet<JobInformation> Jobs { get; set; }

        public OscStorage(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id= 1,
                Name = "Admin",
                Password = HashingPasswordService.Hash("Password"),
                Email = "Admin@OSC.ru"
            });
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
        public void ApplyMigrations()
        {
            Database.Migrate();
        }
    }
}
