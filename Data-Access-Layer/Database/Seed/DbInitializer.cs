using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Data_Access_Layer.Database.Seed
{
    public static class DbInitializer
    {
        public static readonly string[] Roles = { "Admin", "Organizer", "Attendee" };

        // Default admin (can be overridden through config / env vars: SeedAdmin__Email, SeedAdmin__Password ...)
        private const string DefaultEmail = "olson.giomar@forliion.com";
        private const string DefaultPassword = "Test123#";
        private const string DefaultFullName = "Mohamed Essam";
        private const int DefaultAge = 20;

        /// <summary>
        /// Applies pending EF migrations then seeds roles + default admin. Safe to run on every startup (idempotent).
        /// </summary>
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;
            var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");
            var config = sp.GetRequiredService<IConfiguration>();
            var context = sp.GetRequiredService<EventContext>();

            await MigrateWithRetryAsync(context, logger);
            await SeedRolesAsync(sp.GetRequiredService<RoleManager<IdentityRole>>(), logger);
            await SeedAdminAsync(sp.GetRequiredService<UserManager<ApplicationUser>>(), config, logger);
        }

        private static async Task MigrateWithRetryAsync(EventContext context, ILogger logger)
        {
            const int maxAttempts = 10;
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Database migrations applied.");
                    return;
                }
                catch (Exception ex) when (attempt < maxAttempts)
                {
                    logger.LogWarning(ex, "Database not ready (attempt {Attempt}/{Max}), retrying in 5s...", attempt, maxAttempts);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            foreach (var role in Roles)
            {
                if (await roleManager.RoleExistsAsync(role)) continue;

                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (!result.Succeeded)
                    throw new InvalidOperationException($"Failed to create role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");

                logger.LogInformation("Role '{Role}' created.", role);
            }
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, IConfiguration config, ILogger logger)
        {
            var email = config["SeedAdmin:Email"] ?? DefaultEmail;
            var password = config["SeedAdmin:Password"] ?? DefaultPassword;
            var fullName = config["SeedAdmin:FullName"] ?? DefaultFullName;
            var age = int.TryParse(config["SeedAdmin:Age"], out var a) ? a : DefaultAge;

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    Age = age,
                    EmailConfirmed = true
                };

                var create = await userManager.CreateAsync(user, password);
                if (!create.Succeeded)
                    throw new InvalidOperationException($"Failed to seed admin: {string.Join(", ", create.Errors.Select(e => e.Description))}");

                logger.LogInformation("Default admin '{Email}' created.", email);
            }

            if (!await userManager.IsInRoleAsync(user, "Admin"))
            {
                var add = await userManager.AddToRoleAsync(user, "Admin");
                if (!add.Succeeded)
                    throw new InvalidOperationException($"Failed to assign Admin role: {string.Join(", ", add.Errors.Select(e => e.Description))}");
            }
        }
    }
}
