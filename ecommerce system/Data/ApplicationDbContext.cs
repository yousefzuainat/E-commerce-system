using ecommerce_system.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_system.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Proudect> proudects {  get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<order> orders { get; set; }

        public DbSet<WishList> wishList { get; set; }

        public DbSet<payment> payment { get; set; }

        public DbSet<Review> review { get; set; }

        public DbSet<Cart> cart { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. This is MANDATORY. It configures the Identity tables (AspNetUsers, etc.)
            base.OnModelCreating(modelBuilder);

            // 2. Fix the Decimal precision warnings for SQL Server
            modelBuilder.Entity<Proudect>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<order>()
                .Property(o => o.tootal_amount)
                .HasColumnType("decimal(18,2)");

            // 3. Resolve the Discriminator conflict
            // If you are seeing "Discriminator" errors, it's often because EF 
            // is trying to manage multiple user types. This line ensures 
            // it treats the base IdentityUser correctly.
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUser>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue("IdentityUser");
        }


    }
}
