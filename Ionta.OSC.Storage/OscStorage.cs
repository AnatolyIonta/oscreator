using Ionta.OSC.App;
using Ionta.OSC.App.Services.HashingPasswordService;
using Ionta.OSC.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ionta.OSC.Storage
{
    public class OscStorage : DbContext, IOscStorage
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AssemblyFile> AssemblyFiles { get; set; }

        private readonly IHashingPasswordService _hashingPassword;

        public OscStorage(DbContextOptions options, IHashingPasswordService hashingPassword) : base(options) 
        { 
            _hashingPassword= hashingPassword;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new User()
            {
                Id= 1,
                Name = "Admin",
                Password = _hashingPassword.Hash("Password"),
                Email = "Admin@OSC.ru"
            });
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
