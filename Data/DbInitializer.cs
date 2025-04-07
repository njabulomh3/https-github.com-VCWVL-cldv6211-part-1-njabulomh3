using EventEase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            EventEaseDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            try
            {
                // 1. Ensure database is created using migrations
                await context.Database.MigrateAsync();

                // 2. Seed roles with error handling
                await SeedRolesAsync(roleManager);

                // 3. Seed admin user with enhanced validation
                await SeedAdminUserAsync(userManager);

                // 4. Seed venues with null checks
                await SeedVenuesAsync(context);

                // 5. Seed events with relationship validation
                await SeedEventsAsync(context);
            }
            catch (Exception ex)
            {
                throw new Exception("Database initialization failed", ex);
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role '{role}': {string.Join(", ", result.Errors)}");
                    }
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
        {
            const string adminEmail = "admin@eventease.com";
            const string adminPassword = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var creationResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (!creationResult.Succeeded)
                {
                    throw new Exception($"Admin user creation failed: {string.Join(", ", creationResult.Errors)}");
                }

                var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Role assignment failed: {string.Join(", ", roleResult.Errors)}");
                }
            }
        }

        private static async Task SeedVenuesAsync(EventEaseDbContext context)
        {
            if (!context.Venues.Any())
            {
                var venues = new Venue[]
                {
                    new Venue
                    {
                        VenueName = "Grand Ballroom",
                        Location = "123 Main St",
                        Capacity = 500,
                        ImageUrl = "/images/Venue/Venue1.jpg" // Correct path
                    },
                    new Venue
                    {
                        VenueName = "Convention Center",
                        Location = "456 Oak Ave",
                        Capacity = 1000,
                        ImageUrl = "/images/Venue/Venue2.jpg" // Correct path
                    }
                };

                await context.Venues.AddRangeAsync(venues);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedEventsAsync(EventEaseDbContext context)
        {
            if (!context.Events.Any() && context.Venues.Any())
            {
                var venue1 = await context.Venues.FirstOrDefaultAsync(v => v.VenueName == "Grand Ballroom");
                var venue2 = await context.Venues.FirstOrDefaultAsync(v => v.VenueName == "Convention Center");

                if (venue1 == null || venue2 == null)
                {
                    throw new Exception("Required venues not found for event seeding");
                }

                var events = new Event[]
                {
                    new Event
                    {
                        EventName = "Birthday",
                        EventDate = DateTime.Now.AddDays(30),
                        Description = "A client's birthday luncheon.",
                        VenueId = venue1.VenueId
                    },
                    new Event
                    {
                        EventName = "Wedding",
                        EventDate = DateTime.Now.AddDays(45),
                        Description = "Photo of Mr & Mrs Smith outside the venue.",
                        VenueId = venue2.VenueId
                    }
                };

                await context.Events.AddRangeAsync(events);
                await context.SaveChangesAsync();
            }
        }
    }
}

       