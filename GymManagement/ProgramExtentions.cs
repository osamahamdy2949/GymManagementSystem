using GymManagement.DAL.Data.DataSeeding;
using GymManagement.DAL.Data.DbContexts;
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

            var pendingMigrations = dbcontext.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Appling {pendingMigrations.Count()} Pending Migartion");
                await dbcontext.Database.MigrateAsync();
            }

            var folderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");

            await GymDataSeeding.SeedDataAsync(dbcontext, folderPath, logger);
        }
    }
}
