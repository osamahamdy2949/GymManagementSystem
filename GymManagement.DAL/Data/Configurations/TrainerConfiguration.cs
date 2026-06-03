using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configurations
{
    internal class TrainerConfiguration : GymUserConfiguration<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(t => t.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(t => t.Speciality)
                   .HasConversion<string>();

            base.Configure(builder);
            // Seed trainers
            builder.HasData(Seeder.GetTrainers());

            builder.OwnsOne(m => m.Address).HasData(
                 new
                 {
                     TrainerId = 1,
                     Street = "El Nile St",
                     City = "Cairo",
                     BuildingNumber = 10
                 },

                 new
                 {
                     TrainerId = 2,
                     Street = "Tahrir Ave",
                     City = "Cairo",
                     BuildingNumber = 5
                 }
            );
        }
    }
}
