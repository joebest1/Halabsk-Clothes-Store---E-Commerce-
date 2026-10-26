using Halabsk.Net.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Halabsk.Net.Data;

public static class AppDbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        var configuration = services.GetRequiredService<IConfiguration>();

        await dbContext.Database.MigrateAsync();
        await EnsureRolesAsync(services);

        var adminEmail = configuration["Admin:Email"];
        var adminPassword = configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) && string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException(
                "Configure both Admin:Email and Admin:Password to seed the admin account.");
        }

        await EnsureAdminUserAsync(services, adminEmail, adminPassword);
    }

    private static async Task EnsureRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var roleName in new[] { UserRoles.Admin, UserRoles.Customer })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create the '{roleName}' role: " +
                        string.Join(" ", result.Errors.Select(error => error.Description)));
                }
            }
        }
    }

    private static async Task EnsureAdminUserAsync(
        IServiceProvider services,
        string adminEmail,
        string adminPassword)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "Admin"
            };

            var createResult = await userManager.CreateAsync(admin, adminPassword);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not create the admin user: " +
                    string.Join(" ", createResult.Errors.Select(error => error.Description)));
            }
        }
        else
        {
            var changed = false;

            if (!admin.EmailConfirmed)
            {
                admin.EmailConfirmed = true;
                changed = true;
            }

            if (string.IsNullOrWhiteSpace(admin.FullName))
            {
                admin.FullName = "Admin";
                changed = true;
            }

            if (admin.LockoutEnd is not null || admin.AccessFailedCount > 0)
            {
                admin.LockoutEnd = null;
                admin.AccessFailedCount = 0;
                changed = true;
            }

            if (changed)
            {
                var updateResult = await userManager.UpdateAsync(admin);

                if (!updateResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Could not update the admin user: " +
                        string.Join(" ", updateResult.Errors.Select(error => error.Description)));
                }
            }
        }

        if (!await userManager.IsInRoleAsync(admin, UserRoles.Admin))
        {
            var roleResult = await userManager.AddToRoleAsync(admin, UserRoles.Admin);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not add the admin role: " +
                    string.Join(" ", roleResult.Errors.Select(error => error.Description)));
            }
        }
    }
}
