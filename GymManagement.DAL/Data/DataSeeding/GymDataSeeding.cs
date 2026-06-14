using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.DataSeeding
{
    public static class GymDataSeeding
    {
        private static List<T> LoadDataFromJsonFile<T>(string folderName , string fileName)
        {
            var filePath = Path.Combine(folderName, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File With Path {filePath} Not Found");

            var data = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
        public static async Task SeedDataAsync(GymDbContext context, string folderPath, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if (!await context.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>(folderPath, "plans.json");

                    if(plans.Any())
                    {
                        context.Plans.AddRange(plans);

                        logger.LogInformation($"Plans Seeded With count {plans.Count}");
                    }

                    if (context.ChangeTracker.HasChanges())
                        await context.SaveChangesAsync(ct);
                    else
                        logger.LogInformation("Plan Already Seeded");
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Gym Data Seeding Failed");
            }
        }
    }
}
