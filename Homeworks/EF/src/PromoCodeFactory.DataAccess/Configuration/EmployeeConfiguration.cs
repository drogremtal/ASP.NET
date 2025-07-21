using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(e => e.FirstName)
                .HasMaxLength(32)
                .IsRequired();
            builder.Property(e => e.LastName)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(e => e.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.HasData(FakeDataFactory.Employees);

        }
    }
}
