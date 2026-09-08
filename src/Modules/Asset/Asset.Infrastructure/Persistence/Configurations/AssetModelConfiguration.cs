using Asset.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Asset.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="AssetModel"/> aggregate.</summary>
public sealed class AssetModelConfiguration : IEntityTypeConfiguration<AssetModel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AssetModel> builder)
    {
        builder.ToTable("AssetModel");

        builder.HasKey(am => am.Id);

        builder.Property(am => am.Id)
            .HasConversion(id => id.Value, value => AssetModelId.From(value))
            .ValueGeneratedNever();

        builder.Property(am => am.HoldingId)
            .IsRequired();

        builder.HasIndex(am => am.HoldingId);

        builder.Property(am => am.Name)
            .HasMaxLength(AssetModel.MaxNameLength)
            .IsRequired();

        // Changed from a free-text Manufacturer string to a reference
        // into the new Company aggregate (chat, 2026-09-01) — plain
        // Guid column, index only, no database-level FK since Company
        // lives in the separate Configuration module/DbContext, same
        // pattern as HoldingId itself.
        builder.Property(am => am.CompanyId)
            .IsRequired();

        builder.HasIndex(am => am.CompanyId);

        // Dimensional / weight / working-capacity specifications
        // (chat, 2026-09-04) — all nullable, each value paired with a
        // Guid unit-of-measurement column. Plain columns + index only,
        // no database-level FK, since UnitOfMeasurement lives in the
        // separate Configuration module/DbContext (same pattern as
        // CompanyId and HoldingId above).
        builder.Property(am => am.LengthValue).HasPrecision(18, 4);
        builder.Property(am => am.LengthUnitOfMeasurementId);
        builder.HasIndex(am => am.LengthUnitOfMeasurementId);

        builder.Property(am => am.WidthValue).HasPrecision(18, 4);
        builder.Property(am => am.WidthUnitOfMeasurementId);
        builder.HasIndex(am => am.WidthUnitOfMeasurementId);

        builder.Property(am => am.HeightValue).HasPrecision(18, 4);
        builder.Property(am => am.HeightUnitOfMeasurementId);
        builder.HasIndex(am => am.HeightUnitOfMeasurementId);

        builder.Property(am => am.WeightValue).HasPrecision(18, 4);
        builder.Property(am => am.WeightUnitOfMeasurementId);
        builder.HasIndex(am => am.WeightUnitOfMeasurementId);

        builder.Property(am => am.WorkingCapacityVolumeValue).HasPrecision(18, 4);
        builder.Property(am => am.WorkingCapacityVolumeUnitOfMeasurementId);
        builder.HasIndex(am => am.WorkingCapacityVolumeUnitOfMeasurementId);

        builder.Property(am => am.WorkingCapacityWeightValue).HasPrecision(18, 4);
        builder.Property(am => am.WorkingCapacityWeightUnitOfMeasurementId);
        builder.HasIndex(am => am.WorkingCapacityWeightUnitOfMeasurementId);

        // CompatibleEngineModelIds is a computed public property (maps
        // Guid -> EngineModelId), so EF must never see it directly —
        // tell it to ignore that property entirely, and instead map the
        // real backing field (plain List<Guid>) as a shadow property by
        // name. This sidesteps the EF Core 10 translation issue hit
        // earlier with Profile.Permissions (chat, 2026-08-25).
        builder.Ignore(am => am.CompatibleEngineModelIds);

        builder.Property<List<Guid>>("_compatibleEngineModelIds")
            .HasColumnName("CompatibleEngineModelIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}