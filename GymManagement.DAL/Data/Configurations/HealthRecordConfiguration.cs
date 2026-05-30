using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configurations
{
    internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(h => h.Note).HasMaxLength(500);

            builder.Property(h => h.Height)
                   .HasPrecision(5, 2);


            builder.Property(h => h.Weight)
                   .HasPrecision(5, 2);

            builder.Property(h => h.BloodType).HasConversion<string>();
        }
    }
}
