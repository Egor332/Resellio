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
            
            // Configure Organiser – Event Relationship
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany() 
                .HasForeignKey(e => e.OrganiserId)
                .OnDelete(DeleteBehavior.Restrict);

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

            // Configure Ticket to Holder relation
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Holder)
                .WithMany()
                .HasForeignKey(t => t.HolderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Ticket to PurchaseIntender relation
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.PurchaseIntender)
                .WithMany()
                .HasForeignKey(t => t.PurchaseIntenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Configure price properties of Ticket and TicketType
            modelBuilder.Entity<TicketType>().OwnsOne(tt => tt.BasePrice, basePrice =>
            {
                basePrice.Property(p => p.Amount).HasColumnName("PriceAmount").HasColumnType("decimal(18,2)");
                basePrice.Property(p => p.CurrencyCode).HasColumnName("PriceCurrency").HasMaxLength(3).IsRequired();
            });

            modelBuilder.Entity<Ticket>().OwnsOne(t => t.CurrentPrice, currentPrice =>
            {
                currentPrice.Property(p => p.Amount).HasColumnName("PriceAmount").HasColumnType("decimal(18,2)");
                currentPrice.Property(p => p.CurrencyCode).HasColumnName("PriceCurrency").HasMaxLength(3).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
