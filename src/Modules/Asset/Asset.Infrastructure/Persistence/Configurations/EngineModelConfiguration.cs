using Asset.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Asset.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="EngineModel"/> aggregate.</summary>
public sealed class EngineModelConfiguration : IEntityTypeConfiguration<EngineModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<EngineModel> builder)
    {
        builder.ToTable("EngineModel");

        builder.HasKey(em => em.Id);

        builder.Property(em => em.Id)
            .HasConversion(id => id.Value, value => EngineModelId.From(value))
            .ValueGeneratedNever();

        builder.Property(em => em.HoldingId)
            .IsRequired();

        builder.HasIndex(em => em.HoldingId);

        builder.Property(em => em.Name)
            .HasMaxLength(EngineModel.MaxNameLength)
            .IsRequired();

        // Changed from a free-text Manufacturer string to a reference
        // into the new Company aggregate (chat, 2026-09-01) — plain
        // Guid column, index only, no database-level FK since Company
        // lives in the separate Configuration module/DbContext, same
        // pattern as HoldingId itself.
        builder.Property(em => em.CompanyId)
            .IsRequired();

        builder.HasIndex(em => em.CompanyId);

        // Stored as its string name for readability in the database
        // (chat, 2026-09-02), matching the convention already used for
        // Asset.Status. Required — FuelKind has no "not specified" state.
        builder.Property(em => em.FuelKind)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(em => em.CylinderCount);

        builder.Property(em => em.EngineDisplacementValue)
            .HasPrecision(18, 4);

        // Plain Guid columns, index only — no database-level FK, since
        // UnitOfMeasurement now lives in the separate Configuration
        // module/DbContext (chat, 2026-08-30). Existence and same-Holding
        // membership are enforced at the application layer via
        // IUnitOfMeasurementLookupService, the same pattern already used
        // for HoldingId itself.
        builder.Property(em => em.EngineDisplacementUnitOfMeasurementId);
        builder.HasIndex(em => em.EngineDisplacementUnitOfMeasurementId);

        builder.Property(em => em.EnginePowerValue)
            .HasPrecision(18, 4);
        builder.Property(em => em.EnginePowerUnitOfMeasurementId);
        builder.HasIndex(em => em.EnginePowerUnitOfMeasurementId);

        builder.Property(em => em.WeightValue)
            .HasPrecision(18, 4);
        builder.Property(em => em.WeightUnitOfMeasurementId);
        builder.HasIndex(em => em.WeightUnitOfMeasurementId);
    }
}