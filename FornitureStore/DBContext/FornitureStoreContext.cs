
using FornitureStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FornitureStore.DBContext
{
    public class FornitureStoreContext : DbContext
    {


        public FornitureStoreContext(DbContextOptions<FornitureStoreContext> options)
                : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrdersDetails { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);

        }
    }
}
