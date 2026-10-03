using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="global::Usage.Domain.MeterDevice"/> aggregate.</summary>
public sealed class MeterDeviceConfiguration : IEntityTypeConfiguration<global::Usage.Domain.MeterDevice>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<global::Usage.Domain.MeterDevice> builder)
    {
        builder.ToTable("MeterDevice");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(id => id.Value, value => global::Usage.Domain.MeterDeviceId.From(value))
            .ValueGeneratedNever();

        builder.Property(d => d.OrganizationId)
            .IsRequired();

        builder.HasIndex(d => d.OrganizationId);

        // Fixed for the device's lifetime as of chat, 2026-09-29 (see
        // MeterDevice.Register remarks) — same string-column convention
        // used throughout Asset (e.g. Asset.Status, Asset.MeterReadingUnit).
        builder.Property(d => d.Unit)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(d => d.OwnerType)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Plain column, no database-level FK: OwnerId is polymorphic
        // (Asset or Tracked Component, BR-004) and each of those lives
        // in its own module's DbContext — same cross-module reference
        // pattern as Asset.ColorId/Asset.ProjectId.
        builder.Property(d => d.OwnerId);

        builder.HasIndex(d => new { d.OwnerType, d.OwnerId });

        builder.Property(d => d.DailyCapOverride)
            .HasPrecision(18, 4);

        // NOTE (chat, 2026-09-29): the owned MeterReading collection and
        // its ordering risk (flagged here previously) are gone — a
        // device no longer stores readings itself; see UsageEntry /
        // UsageLedgerConfiguration, which carries the same risk (and
        // now an explicit Sequence-based fix) for the Ledger's timeline.
    }
}
