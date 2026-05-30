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
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(g => g.Name)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(g => g.Email)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(g => g.Gender)
                   .HasConversion<string>();

            builder.HasIndex(g => g.Email).IsUnique();
            builder.HasIndex(g => g.PhoneNumber).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_GymUser_Email", "Email LIKE '_%@%._%'");
                tb.HasCheckConstraint("CK_GymUser_Phone", "PhoneNumber LIKE '010[0-9]%' or PhoneNumber LIKE '011[0-9]%' " +
                                      "or PhoneNumber LIKE '012[0-9]%' or PhoneNumber LIKE '015[0-9]%' AND LEN(PhoneNumber) >= 10 AND LEN(PhoneNumber) <= 15");
            });

            builder.OwnsOne(g => g.Address, a =>
            {
                a.Property(ad => ad.Street)
                 .HasColumnType("nvarchar")
                 .HasMaxLength(30)
                 .IsRequired();

                a.Property(ad => ad.City)
                 .HasColumnType("nvarchar")
                 .HasMaxLength(30)
                 .IsRequired();
            });
        }
    }
}
