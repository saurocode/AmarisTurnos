using Amaris.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Amaris.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Turn> Turn => Set<Turn>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Usuario> Users => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "Sucursal Centro", Address = "Calle 10 #5-20", City = "Bogotá", Active = true },
                new Location { Id = 2, Name = "Sucursal Norte", Address = "Av. 19 #120-45", City = "Bogotá", Active = true },
                new Location { Id = 3, Name = "Sucursal Sur", Address = "Calle 45 Sur #80-10", City = "Bogotá", Active = true }
            );
        }
    }
}

