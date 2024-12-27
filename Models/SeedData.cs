using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            context.Database.Migrate();

            // Rolleri oluştur
            string[] roleNames = { "Admin", "Teacher", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole 
                    { 
                        Name = roleName,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }

            // Admin kullanıcısını oluştur
            var adminEmail = "admin@example.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Örnek kategorileri oluştur
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category 
                    { 
                        Name = "Matematik", 
                        Description = "Matematik ödevleri",
                        CreatedDate = DateTime.UtcNow
                    },
                    new Category 
                    { 
                        Name = "Fizik", 
                        Description = "Fizik ödevleri",
                        CreatedDate = DateTime.UtcNow
                    },
                    new Category 
                    { 
                        Name = "Kimya", 
                        Description = "Kimya ödevleri",
                        CreatedDate = DateTime.UtcNow
                    }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }
    }
}
