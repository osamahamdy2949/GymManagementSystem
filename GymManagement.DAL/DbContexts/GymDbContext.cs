using GymManagement.DaL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DbContexts
{
    public class GymDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<Plan> Plans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new Configurations.PlanConfiguration());
        }
    }
}
