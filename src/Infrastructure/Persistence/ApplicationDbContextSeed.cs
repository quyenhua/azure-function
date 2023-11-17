using Microsoft.AspNetCore.Identity;

using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Identity;

namespace Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var administratorRole = new IdentityRole("Administrator");

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Administrator1!");
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }
    }

    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.TodoLists.Any())
        {
            context.TodoLists.Add(new ListTodo
            {
                Title = "Shopping",
                Color = Colors.Blue,
                Items =
                {
                    new Todo { Title = "Apples", Done = true },
                    new Todo { Title = "Milk", Done = true },
                    new Todo { Title = "Bread", Done = true },
                    new Todo { Title = "Toilet paper" },
                    new Todo { Title = "Pasta" },
                    new Todo { Title = "Tissues" },
                    new Todo { Title = "Tuna" },
                    new Todo { Title = "Water" }
                }
            });

            await context.SaveChangesAsync();
        }
    }
}
