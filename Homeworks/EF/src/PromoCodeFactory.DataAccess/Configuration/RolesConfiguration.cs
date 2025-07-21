using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess.Configuration;
public class RolesConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(128)
            .IsRequired();

        builder.HasData(FakeDataFactory.Roles);
    }
}
