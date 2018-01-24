using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace NetCore.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);


            modelBuilder.Entity("FlexFinancial.Data.Entities.Country", b =>
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

                b.ToTable("Countries");
            });

            modelBuilder.Entity("FlexFinancial.Data.Entities.Currency", b =>
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

                b.HasIndex("CurrencySettingsId");

                b.HasIndex("DisplayName")
                    .IsUnique();

                b.ToTable("Currencies");
            });


            modelBuilder.Entity("FlexFinancial.Data.Entities.Role", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int>("CountrySpecific");

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(255);

                b.Property<int>("DivisionSpecific");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(255);

                b.Property<long?>("ParentRoleId");

                b.HasKey("Id");

                b.HasAlternateKey("Name");

                b.HasIndex("ParentRoleId");

                b.ToTable("Roles");
            });

            modelBuilder.Entity("FlexFinancial.Data.Entities.User", b =>
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

            modelBuilder.Entity("FlexFinancial.Data.Entities.UserCountry", b =>
            {
                b.Property<long>("UserId");

                b.Property<long>("CountryId");

                b.HasKey("UserId", "CountryId");

                b.HasIndex("CountryId");

                b.ToTable("UserCountries");
            });

            modelBuilder.Entity("FlexFinancial.Data.Entities.Country", b =>
            {
                b.HasOne("FlexFinancial.Data.Entities.Currency", "Currency")
                    .WithMany()
                    .HasForeignKey("CurrencyId");
            });

            modelBuilder.Entity("FlexFinancial.Data.Entities.User", b =>
            {
                b.HasOne("FlexFinancial.Data.Entities.Role", "Role")
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity("FlexFinancial.Data.Entities.UserCountry", b =>
            {
                b.HasOne("FlexFinancial.Data.Entities.Country", "Country")
                    .WithMany()
                    .HasForeignKey("CountryId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("FlexFinancial.Data.Entities.User", "User")
                    .WithMany("Countries")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}
