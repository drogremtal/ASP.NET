using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Configuration;
public class CustomerPreferenceConfiguration:IEntityTypeConfiguration<CustomerPreference>
{

    /// <summary>
    /// CustomerPreference (Many-to-Many между Customer и Preference)
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<CustomerPreference> builder)
    {
        builder.Property(cp => new { cp.CustomerId, cp.PreferenceId });

        builder
            .HasOne(cp => cp.Customer)
            .WithMany(c => c.PreferencesLink)
            .HasForeignKey(cp => cp.CustomerId);

        builder.HasOne(cp => cp.Preference)
            .WithMany(c => c.CustomersLink)
            .HasForeignKey(cp => cp.PreferenceId);

    }
}
