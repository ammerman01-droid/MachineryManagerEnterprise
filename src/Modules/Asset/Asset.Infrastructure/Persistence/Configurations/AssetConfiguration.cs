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

        builder.HasIndex(a => new { a.OrganizationId, a.Code })
            .IsUnique();

        builder.Property(a => a.Name)
            .HasMaxLength(global::Asset.Domain.Asset.MaxNameLength)
            .IsRequired();

        builder.Property(a => a.AssetModelId)
            .HasConversion(id => id.Value, value => AssetModelId.From(value))
            .IsRequired();

        builder.HasIndex(a => a.AssetModelId);

        builder.HasOne<AssetModel>()
            .WithMany()
            .HasForeignKey(a => a.AssetModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.ColorId)
            .HasConversion(id => id.Value, value => ColorId.From(value))
            .IsRequired();

        builder.HasIndex(a => a.ColorId);

        // Real database-level FK to Color (chat, 2026-08-28) — Restrict,
        // same reasoning as the AssetModel FK: a Color cannot be
        // removed while any Asset still references it.
        builder.HasOne<Color>()
            .WithMany()
            .HasForeignKey(a => a.ColorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.SerialNumber)
            .HasMaxLength(global::Asset.Domain.Asset.MaxSerialNumberLength);

        builder.Property(a => a.ChassisNumber)
            .HasMaxLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);

        builder.Property(a => a.BodyNumber)
            .HasMaxLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);

        builder.Property(a => a.Vin)
            .HasMaxLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);

        builder.Property(a => a.LicensePlate)
            .HasMaxLength(global::Asset.Domain.Asset.MaxLicensePlateLength);

        builder.Property(a => a.ManufactureYear);

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}