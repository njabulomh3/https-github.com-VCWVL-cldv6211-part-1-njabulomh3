using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EventEase.Models
{
    public class EventEaseDbContext : IdentityDbContext<IdentityUser>
    {
        public EventEaseDbContext(DbContextOptions<EventEaseDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Required for Identity

            // Ignore file upload properties
            modelBuilder.Entity<Venue>().Ignore(v => v.ImageFile);
            modelBuilder.Entity<Event>().Ignore(e => e.ImageFile);

            // Configure relationships with proper delete behaviors
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add unique constraint to prevent duplicate bookings
            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.EventId, b.UserId })
                .IsUnique();
        }
    }
}


