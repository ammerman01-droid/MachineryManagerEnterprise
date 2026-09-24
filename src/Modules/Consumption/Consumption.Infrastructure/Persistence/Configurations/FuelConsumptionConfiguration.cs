using MachineryManagerEnterprise.Consumption.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Consumption.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="FuelConsumption"/> aggregate.</summary>
public sealed class FuelConsumptionConfiguration : IEntityTypeConfiguration<FuelConsumption>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<FuelConsumption> builder)
    {
        builder.ToTable("FuelConsumption");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasConversion(id => id.Value, value => FuelConsumptionId.From(value))
            .ValueGeneratedNever();

        // Cross-module references (Asset, Organization, Project, Personnel, FuelType) are
        // plain Guid columns, index only, no database-level FK — same pattern
        // as Asset.ProjectId/ColorId, since each module owns its own DbContext.
        builder.Property(f => f.AssetId).IsRequired();
        builder.HasIndex(f => f.AssetId);

        builder.Property(f => f.OrganizationId).IsRequired();
        builder.HasIndex(f => f.OrganizationId);

        builder.Property(f => f.ProjectId).IsRequired();
        builder.HasIndex(f => f.ProjectId);

        builder.Property(f => f.DeliveredByPersonnelId).IsRequired();
        builder.Property(f => f.ReceivedByPersonnelId).IsRequired();

        // Reference to the specific Configuration.FuelType Aggregate the
        // user selected (chat, 2026-09-22) — editable, unlike the other
        // cross-module references above.
        builder.Property(f => f.FuelTypeId).IsRequired();
        builder.HasIndex(f => f.FuelTypeId);

        // Fuel slot/kind/unit and meter unit are all fixed enums, snapshotted
        // from the Asset at record time — stored as their string names,
        // matching the convention used for Asset.MeterReadingUnit/
        // PrimaryFuelKind/PrimaryFuelUnit/Status (chat, 2026-09-19/20).
        builder.Property(f => f.FuelSlot)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(f => f.FuelKind)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(f => f.FuelUnit)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(f => f.MeterReadingUnit)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Same precision convention as AssetModel's dimension/capacity values.
        builder.Property(f => f.UnitPriceSnapshot).HasPrecision(18, 4);
        builder.Property(f => f.Quantity).HasPrecision(18, 4);
        builder.Property(f => f.MeterReading).HasPrecision(18, 4);

        builder.Property(f => f.RecordedAtUtc).IsRequired();

        // Enforce the monotonic meter-reading chain query efficiently
        // (GetPreviousAsync/GetNextAsync filter by AssetId + order by
        // RecordedAtUtc).
        builder.HasIndex(f => new { f.AssetId, f.RecordedAtUtc });

        builder.Property(f => f.Notes)
            .HasMaxLength(FuelConsumption.MaxNotesLength);
    }
}
