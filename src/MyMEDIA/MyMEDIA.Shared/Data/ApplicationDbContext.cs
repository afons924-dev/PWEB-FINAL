using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.Shared.Data;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string UserType { get; set; } = "Client"; // Client, Supplier, Employee, Admin
    public string Status { get; set; } = "Pending"; // Pending, Active

    // Additional Profile Data
    public string NIF { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<DeliveryMode> DeliveryModes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
