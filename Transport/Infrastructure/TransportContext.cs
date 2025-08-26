using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Transports;

namespace Infrastructure
{
    public class TransportContext : DbContext
    {
        public TransportContext(DbContextOptions opts) : base(opts)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().OwnsOne(car => car.Coordinates);
            modelBuilder.Entity<Motocycle>().OwnsOne(moto => moto.Coordinates);
            modelBuilder.Entity<Bicycle>().OwnsOne(bicycle => bicycle.Coordinates);
            modelBuilder.Entity<Scooter>().OwnsOne(scooter => scooter.Coordinates);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Motocycle> Motocycles { get; set; }
        public DbSet<Bicycle> Bicycles { get; set; }
        public DbSet<Scooter> Scooters { get; set; }
    }
}
