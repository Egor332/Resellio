﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResellioBackend;

#nullable disable

namespace ResellioBackend.Migrations
{
    [DbContext(typeof(ResellioDbContext))]
    partial class ResellioDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ResellioBackend.EventManagementSystem.Models.Base.Ticket", b =>
                {
                    b.Property<Guid>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("HolderId")
                        .HasColumnType("int");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLock")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PurchaseIntenderId")
                        .HasColumnType("int");

                    b.Property<int>("TicketState")
                        .HasColumnType("int");

                    b.Property<int>("TicketTypeId")
                        .HasColumnType("int");

                    b.HasKey("TicketId");

                    b.HasIndex("HolderId");

                    b.HasIndex("PurchaseIntenderId");

                    b.HasIndex("TicketTypeId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("ResellioBackend.EventManagementSystem.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("OrganiserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.HasKey("EventId");

                    b.HasIndex("OrganiserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("ResellioBackend.EventManagementSystem.Models.TicketType", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TypeId"));

                    b.Property<DateTime>("AvailableFrom")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("MaxCount")
                        .HasColumnType("int");

                    b.HasKey("TypeId");

                    b.HasIndex("EventId");

                    b.ToTable("TicketTypes");
                });

            modelBuilder.Entity("ResellioBackend.UserManagementSystem.Models.Base.UserBase", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("ConnectedSellingAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("UserType").HasValue("UserBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ResellioBackend.UserManagementSystem.Models.Tokens.PasswordResetTokenInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("PasswordResetTokens");
                });

            modelBuilder.Entity("ResellioBackend.UserManagementSystem.Models.Users.Administrator", b =>
                {
                    b.HasBaseType("ResellioBackend.UserManagementSystem.Models.Base.UserBase");

                    b.HasDiscriminator().HasValue("Administrator");
                });

            modelBuilder.Entity("ResellioBackend.UserManagementSystem.Models.Users.Customer", b =>
                {
                    b.HasBaseType("ResellioBackend.UserManagementSystem.Models.Base.UserBase");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("ResellioBackend.UserManagementSystem.Models.Users.Organiser", b =>
                {
                    b.HasBaseType("ResellioBackend.UserManagementSystem.Models.Base.UserBase");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("OrganiserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasDiscriminator().HasValue("Organiser");
                });

            modelBuilder.Entity("ResellioBackend.EventManagementSystem.Models.Base.Ticket", b =>
                {
                    b.HasOne("ResellioBackend.UserManagementSystem.Models.Base.UserBase", "Holder")
                        .WithMany()
                        .HasForeignKey("HolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ResellioBackend.UserManagementSystem.Models.Users.Customer", "PurchaseIntender")
                        .WithMany()
                        .HasForeignKey("PurchaseIntenderId");

                    b.HasOne("ResellioBackend.EventManagementSystem.Models.TicketType", "TicketType")
                        .WithMany("Tickets")
                        .HasForeignKey("TicketTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ResellioBackend.EventManagementSystem.Models.Money", "CurrentPrice", b1 =>
                        {
                            b1.Property<Guid>("TicketId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("PriceAmount");

                            b1.Property<string>("CurrencyCode")
                                .IsRequired()
                                .HasMaxLength(3)
                                .HasColumnType("nvarchar(3)")
                                .HasColumnName("PriceCurrency");

                            b1.HasKey("TicketId");

                            b1.ToTable("Tickets");

                            b1.WithOwner()
                                .HasForeignKey("TicketId");
                        });

                    b.Navigation("CurrentPrice");

                    b.Navigation("Holder");

                    b.Navigation("PurchaseIntender");

                    b.Navigation("TicketType");
                });

            modelBuilder.Entity("ResellioBackend.EventManagementSystem.Models.Event", b =>
                {
                    b.HasOne("ResellioBackend.UserManagementSystem.Models.Users.Organiser", "Organiser")
                        .WithMany()
                        .HasForeignKey("OrganiserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Organiser");
                });

            modelBuilder.Entity("ResellioBackend.EventManagementSystem.Models.TicketType", b =>
                {
                    b.HasOne("ResellioBackend.EventManagementSystem.Models.Event", "Event")
                        .WithMany("TicketTypes")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ResellioBackend.EventManagementSystem.Models.Money", "BasePrice", b1 =>
                        {
                            b1.Property<int>("TicketTypeTypeId")
                                .HasColumnType("int");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("PriceAmount");

                            b1.Property<string>("CurrencyCode")
                                .IsRequired()
                                .HasMaxLength(3)
                                .HasColumnType("nvarchar(3)")
                                .HasColumnName("PriceCurrency");

                            b1.HasKey("TicketTypeTypeId");

                            b1.ToTable("TicketTypes");

                            b1.WithOwner()
                                .HasForeignKey("TicketTypeTypeId");
                        });

                    b.Navigation("BasePrice")
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("ResellioBackend.UserManagementSystem.Models.Tokens.PasswordResetTokenInfo", b =>
                {
                    b.HasOne("ResellioBackend.UserManagementSystem.Models.Base.UserBase", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ResellioBackend.EventManagementSystem.Models.Event", b =>
                {
                    b.Navigation("TicketTypes");
                });

            modelBuilder.Entity("ResellioBackend.EventManagementSystem.Models.TicketType", b =>
                {
                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
