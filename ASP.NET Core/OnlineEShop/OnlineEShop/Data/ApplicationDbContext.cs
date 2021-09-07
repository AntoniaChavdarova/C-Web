namespace OnlineEShop.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OnlineEShop.Data.Models;
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder
                .Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<OrderProduct>()
               .HasOne(c => c.Product)
               .WithMany(c => c.Orders)
               .HasForeignKey(c => c.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
           .Entity<OrderProduct>()
           .HasOne(c => c.Order)
           .WithMany(c => c.Products)
           .HasForeignKey(c => c.OrderId)
           .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Vote>()
                .HasOne(c => c.Product)
                .WithMany(d => d.Votes)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}
