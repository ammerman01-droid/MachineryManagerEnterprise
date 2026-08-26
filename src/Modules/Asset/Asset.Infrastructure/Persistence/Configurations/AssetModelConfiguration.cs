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

        builder.Property(am => am.OrganizationId)
            .IsRequired();

        builder.Property(am => am.Name)
            .HasMaxLength(AssetModel.MaxNameLength)
            .IsRequired();

        builder.Property(am => am.Manufacturer)
            .HasMaxLength(AssetModel.MaxManufacturerLength)
            .IsRequired();

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