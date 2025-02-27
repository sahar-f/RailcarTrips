using Microsoft.EntityFrameworkCore;
using RailcarTrips.Shared.Models;

namespace RailcarTrips.Data;

public class RailcarTripsContext(DbContextOptions<RailcarTripsContext> options) : DbContext(options)
{
    public DbSet<Trip> Trips { get; set; }
    public DbSet<EquipmentEvent> EquipmentEvents { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TO solve issue of cascading deletes since ref to two cities
        // Have to remove trip first to delete city!
        modelBuilder.Entity<Trip>()
            .HasOne(t => t.OriginCity)
            .WithMany()
            .HasForeignKey(t => t.OriginCityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Trip>()
            .HasOne(t => t.DestinationCity)
            .WithMany()
            .HasForeignKey(t => t.DestinationCityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}