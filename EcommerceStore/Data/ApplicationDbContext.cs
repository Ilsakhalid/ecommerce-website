using Microsoft.EntityFrameworkCore;
using EcommerceStore.Models;

namespace EcommerceStore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<BusinessNatureType> BusinessNatureTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GRNHeader> GRNHeaders { get; set; }
        public DbSet<GRNDetails> GRNDetails { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderDetails> SalesOrderDetails { get; set; }
    }
}