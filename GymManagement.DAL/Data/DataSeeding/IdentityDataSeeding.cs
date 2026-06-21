using GymManagement.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager,ILogger logger, 
            CancellationToken ct = default)
        {
            try
            {
                bool hasUsers = await userManager.Users.AnyAsync(ct);
                bool hasRoles = await roleManager.Roles.AnyAsync(ct);
                if (hasUsers && hasRoles) return;


                var roles = new List<IdentityRole>
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Adminr")
            };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var roleResult = await roleManager.CreateAsync(role);

                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Create Role {role.Name} : {string.Join("-", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                if (!hasUsers)
                {
                    var mainAdmin = new ApplicationUser()
                    {
                        FirstName = "Osama",
                        LastName = "El-Naqeeb",
                        Email = "osamaelnaqeeb@gym.com",
                        UserName = "OsamaEl-Naqeeb",
                        PhoneNumber = "01234567895"
                    };

                    var createMainAdminResult = await userManager.CreateAsync(mainAdmin, "P@ssw0rd");
                    if (!createMainAdminResult.Succeeded)
                    {
                        logger.LogError($"Failed To Create Main Admin : {string.Join("-", createMainAdminResult.Errors.Select(e => e.Description))}");
                    }

                    var addMainAdminRoleResult = await userManager.AddToRoleAsync(mainAdmin, "SuperAdmin");
                    if (!addMainAdminRoleResult.Succeeded)
                    {
                        logger.LogError($"Failed To Create Main Admin Role : {string.Join("-", addMainAdminRoleResult.Errors.Select(e => e.Description))}");
                    }

                    var admin = new ApplicationUser()
                    {
                        FirstName = "Omar",
                        LastName = "El-Naqeeb",
                        Email = "omarelnaqeeb@gym.com",
                        UserName = "OmarEl-Naqeeb",
                        PhoneNumber = "01234567894"
                    };

                    await userManager.CreateAsync(admin, "P@ssw0rd");
                    var createAdminResult = await userManager.CreateAsync(mainAdmin, "P@ssw0rd");
                    if (!createAdminResult.Succeeded)
                    {
                        logger.LogError($"Failed To Create Admin : {string.Join("-", createAdminResult.Errors.Select(e => e.Description))}");
                    }
                    var addAdminRoleResult = await userManager.AddToRoleAsync(admin, "Admin");
                    if (!addAdminRoleResult.Succeeded)
                    {
                        logger.LogError($"Failed To Create Admin Role : {string.Join("-", addAdminRoleResult.Errors.Select(e => e.Description))}");
                    }

                    logger.LogInformation("Identity Data Seeded");
                }

                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity Seeding Failed");
                return;
            }
        }
    }
}