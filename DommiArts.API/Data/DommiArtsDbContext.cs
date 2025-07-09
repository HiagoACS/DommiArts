using DommiArts.API.Models;
using Microsoft.EntityFrameworkCore;


namespace DommiArts.API.Data
{
    public class DommiArtsDbContext : DbContext
    {
        public DommiArtsDbContext(DbContextOptions<DommiArtsDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


    }
}