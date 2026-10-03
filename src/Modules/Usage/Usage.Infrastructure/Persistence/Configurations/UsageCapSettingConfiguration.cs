using MachineryManagerEnterprise.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="UsageCapSetting"/> settings row, plus its seed data.</summary>
public sealed class UsageCapSettingConfiguration : IEntityTypeConfiguration<UsageCapSetting>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UsageCapSetting> builder)
    {
        builder.ToTable("UsageCapSetting");

        builder.HasKey(s => s.Unit);

        builder.Property(s => s.Unit)
            .HasConversion<string>()
            .HasMaxLength(20)
            .ValueGeneratedNever();

        builder.Property(s => s.DefaultDailyCap)
            .HasPrecision(18, 4)
            .IsRequired();

        // Seed values, per the examples given for BR-045 (chat,
        // 2026-09-12): 24 hours a day, 1000 km/mi a day. An
        // administrator can change these later through
        // IUsageCapPolicy/UsageCapPolicy — this seed only establishes
        // the starting default so the table is never empty.
        builder.HasData(
            new UsageCapSetting(MeterReadingUnit.Hour, 24m),
            new UsageCapSetting(MeterReadingUnit.Kilometer, 1000m),
            new UsageCapSetting(MeterReadingUnit.Mile, 1000m));
    }
}
