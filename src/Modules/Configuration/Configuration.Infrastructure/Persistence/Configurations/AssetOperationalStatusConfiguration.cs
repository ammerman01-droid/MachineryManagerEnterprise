using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="AssetOperationalStatus"/> aggregate.</summary>
public sealed class AssetOperationalStatusConfiguration : IEntityTypeConfiguration<AssetOperationalStatus>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AssetOperationalStatus> builder)
    {
        builder.ToTable("AssetOperationalStatus");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, value => AssetOperationalStatusId.From(value))
            .ValueGeneratedNever();

        builder.Property(s => s.HoldingId).IsRequired();

        builder.Property(s => s.Name)
            .HasMaxLength(AssetOperationalStatus.MaxNameLength)
            .IsRequired();

        builder.HasIndex(s => s.HoldingId);
    }
}