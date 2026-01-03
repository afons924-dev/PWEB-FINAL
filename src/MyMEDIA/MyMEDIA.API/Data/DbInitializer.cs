using Microsoft.AspNetCore.Identity;
using MyMEDIA.Shared.Data;
using MyMEDIA.Shared.Entities;

namespace MyMEDIA.API.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        context.Database.EnsureCreated();

        // Seed Roles
        string[] roleNames = { "Admin", "Employee", "Supplier", "Client" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed Users
        if (await userManager.FindByEmailAsync("admin@mymedia.com") == null)
        {
            var admin = new ApplicationUser { UserName = "admin@mymedia.com", Email = "admin@mymedia.com", FullName = "Admin User", UserType = "Admin", Status = "Active", EmailConfirmed = true };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (await userManager.FindByEmailAsync("supplier@mymedia.com") == null)
        {
            var supplier = new ApplicationUser { UserName = "supplier@mymedia.com", Email = "supplier@mymedia.com", FullName = "Supplier One", UserType = "Supplier", Status = "Active", EmailConfirmed = true };
            await userManager.CreateAsync(supplier, "Supplier123!");
            await userManager.AddToRoleAsync(supplier, "Supplier");
        }

        // Seed Categories
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Music", Description = "CDs, Vinyls" },
                new Category { Name = "Movies", Description = "DVDs, BluRay" },
                new Category { Name = "Merchandise", Description = "T-Shirts, Mugs" }
            );
            await context.SaveChangesAsync();
        }

        // Seed Products
        if (!context.Products.Any())
        {
            var music = context.Categories.First(c => c.Name == "Music");
            var movies = context.Categories.First(c => c.Name == "Movies");
            var supplier = await userManager.FindByEmailAsync("supplier@mymedia.com");

            context.Products.AddRange(
                new Product {
                    Title = "Rock Classics",
                    Description = "Best of Rock",
                    BasePrice = 10,
                    FinalPrice = 12,
                    IsActive = true,
                    IsForSale = true,
                    CategoryId = music.Id,
                    SupplierId = supplier.Id,
                    ImageUrl = "/images/bananas.jpg"
                },
                new Product {
                    Title = "Action Movie Collection",
                    Description = "Top 10 Action Movies",
                    BasePrice = 20,
                    FinalPrice = 25,
                    IsActive = true,
                    IsForSale = true,
                    CategoryId = movies.Id,
                    SupplierId = supplier.Id,
                    ImageUrl = "/images/mrBean.jpg"
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
