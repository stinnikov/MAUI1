using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Dispatcher
{
    public class LocationModel
    {

        public int Id { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public LocationModel(string address, double longitude, double latitude)
        {
            Address = address;
            Longitude = longitude;
            Latitude = latitude;
        }
    }

    public class TDispatcherContext : DbContext
    {
        string sqlitePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"MAUI//MAUI1//Dispatcher");
        public DbSet<LocationModel> Locations { get; set; } = null!;
        //public DbSet<DriverUser> Drivers { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={sqlitePath}");
        }
        public TDispatcherContext()
        {
            sqlitePath = Path.Combine(sqlitePath, "locations.db");
            Directory.CreateDirectory(sqlitePath);
            try
            {
                Database.EnsureCreated();
                Locations.Load();
            }
            catch
            {

            }
        }
    }
}
