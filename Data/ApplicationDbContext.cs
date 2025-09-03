using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using online_event_booking.Data.Entities;

namespace online_event_booking.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<EventPrice> EventPrices { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<LoyaltyPoint> LoyaltyPoints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map Event entity to lowercase 'events' table
        modelBuilder.Entity<Event>().ToTable("events");

        // If needed, you can map other entities similarly:
        // modelBuilder.Entity<Venue>().ToTable("venues");
        // modelBuilder.Entity<EventPrice>().ToTable("eventprices");
        // ...
    }
}
