using ecommerce_system.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ecommerce_system.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppliactionUser>
    {
        // 1. CONSTRUCTOR (Keep this to fix the parameterless constructor error)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 2. DB SETS
        public DbSet<Proudect> proudects { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<order> orders { get; set; }
        public DbSet<WishList> wishList { get; set; }
        public DbSet<payment> payment { get; set; }
        public DbSet<Review> review { get; set; }
        public DbSet<Cart> cart { get; set; }

        // 3. MODEL CONFIGURATION
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This is MANDATORY. It configures the Identity tables.
            base.OnModelCreating(modelBuilder);

            // Fix the Decimal precision warnings
            modelBuilder.Entity<Proudect>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<order>()
                .Property(o => o.tootal_amount)
                .HasColumnType("decimal(18,2)");

            // Resolve the Discriminator conflict for Identity
            modelBuilder.Entity<IdentityUser>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue("IdentityUser");

            modelBuilder.Entity<AppliactionUser>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue("AppliactionUser");

            // Fix for SQL Server versions (DateTime compatibility)
            modelBuilder.Entity<AppliactionUser>()
                .Property(u => u.LockoutEnd)
                .HasConversion(
                    v => v.HasValue ? v.Value.DateTime : (DateTime?)null,
                    v => v.HasValue ? new DateTimeOffset(v.Value) : (DateTimeOffset?)null)
                .HasColumnType("datetime");

            // Map all other DateTime properties to 'datetime'
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));
                foreach (var property in properties)
                {
                    property.SetColumnType("datetime");
                }
            }
        }
    }
}