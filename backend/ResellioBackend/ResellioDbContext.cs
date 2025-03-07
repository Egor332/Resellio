using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        public DbSet<PasswordResetTokenInfo> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TPH Inheritance
            modelBuilder.Entity<UserBase>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Customer>("Customer")
            .HasValue<Organiser>("Organiser")
            .HasValue<Administrator>("Administrator");

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
