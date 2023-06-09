﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Registration
{
    class RegistrationContext:DbContext
    {
        string sqlitePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),@"MAUI//MAUI1");
        public DbSet<MAUI1.User.Client.ClientModel> Clients { get; set; } = null!;
        //public DbSet<DriverUser> Drivers { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={sqlitePath}");
        }
        public RegistrationContext()
        {
            sqlitePath = Path.Combine(sqlitePath, "chel.db");
            Directory.CreateDirectory(sqlitePath);
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
