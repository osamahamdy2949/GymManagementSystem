using GymManagement.DAL.Data.DataSeeding;
using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL
{
    public static class ProgramExtentions
    {
        public static async Task MigrateAndSeedDarabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var pendingMigrations = dbcontext.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Appling {pendingMigrations.Count()} Pending Migartion");
                await dbcontext.Database.MigrateAsync();
            }

            var folderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");

            await GymDataSeeding.SeedDataAsync(dbcontext, folderPath, logger);

            await IdentityDataSeeding.SeedIdentityDataAsync(roleManager, userManager, logger);
        }
    }
}
