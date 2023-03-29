using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.Client.Registration
{
    class RegistrationContext:DbContext
    {
        string sqlitePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),@"Chel");
        public DbSet<ClientUser> Clients { get; set; } = null!;
        //public DbSet<DriverUser> Drivers { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={sqlitePath}");
        }
        public RegistrationContext()
        {
            Directory.CreateDirectory(sqlitePath);
            sqlitePath = Path.Combine(sqlitePath, "chel.db");
            try
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
                Clients.Load();
            }
            catch
            {

            }
        }
    }
}
