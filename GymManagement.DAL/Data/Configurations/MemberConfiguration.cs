using GymManagement.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Configurations
{
    internal class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(m => m.CreatedAt)
                  .HasColumnName("JoinDate")
                  .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);

            builder.HasData(Seeder.GetMembers());

            builder.OwnsOne(m => m.Address).HasData(
                 new
                 {
                     MemberId = 1,
                     Street = "El Nile St",
                     City = "Cairo",
                     BuildingNumber = 10
                 },

                 new
                 {
                     MemberId = 2,
                     Street = "Tahrir Ave",
                     City = "Cairo",
                     BuildingNumber = 5
                 }
            );
        }
    }
}
