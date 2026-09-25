using Consumption.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Consumption.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="ConsumptionFreezeSetting"/> aggregate.</summary>
public sealed class ConsumptionFreezeSettingConfiguration : IEntityTypeConfiguration<ConsumptionFreezeSetting>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ConsumptionFreezeSetting> builder)
    {
        builder.ToTable("ConsumptionFreezeSetting");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, value => ConsumptionFreezeSettingId.From(value))
            .ValueGeneratedNever();

        builder.Property(s => s.OrganizationId).IsRequired();

        builder.HasIndex(s => s.OrganizationId)
            .IsUnique();

        builder.Property(s => s.ThresholdDate).IsRequired();
    }
}
