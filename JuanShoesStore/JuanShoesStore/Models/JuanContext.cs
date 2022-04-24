using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JuanShoesStore.Models
{
    public class JuanContext : IdentityDbContext
    {
        public JuanContext(DbContextOptions<JuanContext> options) : base(options)
        {

        }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoeColorItem> ShoeColorItems { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoeComment> ShoeComments { get; set;}
        public DbSet<ContactUs> ContactUses { get; set; }
    }
}
