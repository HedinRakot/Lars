using LarsProjekt.Domain;
using Microsoft.EntityFrameworkCore;

namespace LarsProjekt.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
           .HasMany(o => o.Items)
           .WithOne(o => o.Product)
           .HasForeignKey(o => o.ProductId)
           .HasPrincipalKey(o => o.Id);
    }
}
