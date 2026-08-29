using Asset.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Asset.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="global::Asset.Domain.Asset"/> aggregate.</summary>
public sealed class AssetConfiguration : IEntityTypeConfiguration<global::Asset.Domain.Asset>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<global::Asset.Domain.Asset> builder)
    {
        builder.ToTable("Asset");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(id => id.Value, value => AssetId.From(value))
            .ValueGeneratedNever();

        builder.Property(a => a.OrganizationId)
            .IsRequired();

        builder.HasIndex(a => a.OrganizationId);

        builder.Property(a => a.Code)
            .HasMaxLength(global::Asset.Domain.Asset.MaxCodeLength)
            .IsRequired();

        // Code is unique within its owning Organization, not
        // globally (chat, 2026-08-28) — a composite unique index gives
        // the database-level guarantee as a second line of defense,
        // in addition to the explicit check in
        // RegisterAssetCommandHandler.
        builder.HasIndex(a => new { a.OrganizationId, a.Code })
            .IsUnique();

        builder.Property(a => a.AssetModelId)
            .HasConversion(id => id.Value, value => AssetModelId.From(value))
            .IsRequired();

        builder.HasIndex(a => a.AssetModelId);

        // Real database-level FK (chat, 2026-08-27): no navigation
        // property on the aggregate itself, preserving the project's
        // "no cross-aggregate navigation" DDD boundary — this only
        // tells EF which column is the FK. Restrict (not Cascade,
        // unlike UserProfileAssignment -> Profile): an Asset is a real
        // physical asset, so deleting its AssetModel must never
        // cascade-delete real Asset records; the AssetModel cannot be
        // removed while any Asset still references it.
        builder.HasOne<AssetModel>()
            .WithMany()
            .HasForeignKey(a => a.AssetModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.SerialNumber)
            .HasMaxLength(global::Asset.Domain.Asset.MaxSerialNumberLength);

        builder.Property(a => a.LicensePlate)
            .HasMaxLength(global::Asset.Domain.Asset.MaxLicensePlateLength);

        builder.Property(a => a.ManufactureYear);

        builder.Property(a => a.Color)
            .HasMaxLength(global::Asset.Domain.Asset.MaxColorLength)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}