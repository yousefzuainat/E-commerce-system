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


    }
}
