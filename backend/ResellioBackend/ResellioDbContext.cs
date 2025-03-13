using Microsoft.EntityFrameworkCore;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Tokens;
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

        public DbSet<PasswordResetTokenInfo> PasswordResetTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TPH Inheritance
            modelBuilder.Entity<UserBase>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Customer>("Customer")
            .HasValue<Organiser>("Organiser")
            .HasValue<Administrator>("Administrator");

            // Configure Event – TicketType Relationship
            modelBuilder.Entity<TicketType>()
                .HasOne(t => t.Event)
                .WithMany(e => e.TicketTypes)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TicketType – Ticket Relationship
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.TicketType)
                .WithMany(tt => tt.Tickets)
                .HasForeignKey(t => t.TicketTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add Index on Login Column
            modelBuilder.Entity<UserBase>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure PasswordResetTokens table with Users
            modelBuilder.Entity<PasswordResetTokenInfo>()
                .HasOne(prt => prt.Owner)
                .WithMany()
                .HasForeignKey(prt => prt.OwnerId) 
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
