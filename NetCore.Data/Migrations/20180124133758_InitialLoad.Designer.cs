﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using NetCore.Data;
using System;

namespace NetCore.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180124133758_InitialLoad")]
    partial class InitialLoad
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetCore.Data.Entities.Country", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CurrencyId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("NetCore.Data.Entities.Currency", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CurrencySettingsId");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<decimal>("FromUSD");

                    b.Property<decimal>("ToUSD");

                    b.HasKey("Id");

                    b.HasIndex("DisplayName")
                        .IsUnique();

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("NetCore.Data.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("NetCore.Data.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .HasMaxLength(255);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime?>("LastPasswordReset");

                    b.Property<long?>("ParentUserId");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(255);

                    b.Property<string>("PasswordResetCode")
                        .HasMaxLength(255);

                    b.Property<string>("PasswordSalt")
                        .HasMaxLength(255);

                    b.Property<long?>("RoleId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("ParentUserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NetCore.Data.Entities.UserCountry", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("CountryId");

                    b.HasKey("UserId", "CountryId");

                    b.HasIndex("CountryId");

                    b.ToTable("UserCountries");
                });

            modelBuilder.Entity("NetCore.Data.Entities.Country", b =>
                {
                    b.HasOne("NetCore.Data.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");
                });

            modelBuilder.Entity("NetCore.Data.Entities.User", b =>
                {
                    b.HasOne("NetCore.Data.Entities.User", "ParentUser")
                        .WithMany()
                        .HasForeignKey("ParentUserId");

                    b.HasOne("NetCore.Data.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("NetCore.Data.Entities.UserCountry", b =>
                {
                    b.HasOne("NetCore.Data.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetCore.Data.Entities.User", "User")
                        .WithMany("Countries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
