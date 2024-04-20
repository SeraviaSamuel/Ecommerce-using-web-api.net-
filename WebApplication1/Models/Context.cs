using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Shipment> shipments { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<WishList> wishLists { get; set; }
        public DbSet<Cart> carts { get; set; }
        public Context(DbContextOptions options) : base(options)
        {

        }
    }
}
