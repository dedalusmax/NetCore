using Microsoft.EntityFrameworkCore;
using NetCore.Data.Entities;

namespace NetCore.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Alternate keys

            modelBuilder.Entity<Role>()
                .HasAlternateKey(_ => _.Name);

            #endregion

            #region Indexes

            modelBuilder.Entity<User>()
                .HasIndex(_ => _.UserName)
                .IsUnique(true);

            modelBuilder.Entity<Currency>()
               .HasIndex(_ => new { _.DisplayName })
               .IsUnique(true);

            #endregion

            #region CascadeActions

            modelBuilder.Entity<User>()
                .HasOne(_ => _.Role)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(_ => _.Role)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            #endregion

            #region ManyToMany

            modelBuilder.Entity<UserCountry>()
                .ToTable("UserCountries")
                .HasKey(_ => new { _.UserId, _.CountryId });

            #endregion

            #region Keys

            //modelBuilder.Entity<FlexProject>()
            //    .HasOne(_ => _.CalculationResult)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("FK_FlexProject_CalculationResults_CalculationResultId");

            //modelBuilder.Entity<FlexProjectProCare>()
            //    .Property(_ => _.MinimalProCareCoverage)
            //    .HasDefaultValue(ProCareType.None);

            #endregion
        }
    }
}
