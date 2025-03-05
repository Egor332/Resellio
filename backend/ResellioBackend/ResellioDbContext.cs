using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend
{
    public class ResellioDbContext : DbContext
    {
        public ResellioDbContext(DbContextOptions<ResellioDbContext> options) : base(options) { }

        public DbSet<UserBase> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Organiser> Organisers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TPH Inheritance
            modelBuilder.Entity<UserBase>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Customer>("Customer")
            .HasValue<Organiser>("Organiser")
            .HasValue<Administrator>("Administrator");
            
            modelBuilder.Entity<Event>()
                .HasMany(e => e.TicketTypes)
                .WithOne(tt => tt.Event)
                .HasForeignKey(tt => tt.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<TicketType>()
                .HasMany(tt => tt.Tickets)
                .WithOne(t => t.TicketType)
                .HasForeignKey(t => t.TicketTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add Index on Login Column
            modelBuilder.Entity<UserBase>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
