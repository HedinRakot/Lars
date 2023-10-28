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
    public DbSet<Address> Address { get; set; }
    public DbSet<Coupon> Coupons { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()            
            .HasKey(o => o.Id);        

        modelBuilder.Entity<OrderDetail>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<User>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<Product>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<Address>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<Coupon>()
            .HasKey(o => o.Id);

        modelBuilder.Entity<User>()
            .HasMany(o => o.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .HasPrincipalKey(o => o.Id);
            
    }
}
