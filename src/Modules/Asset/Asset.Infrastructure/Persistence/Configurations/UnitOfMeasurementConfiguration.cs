using Asset.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Asset.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="UnitOfMeasurement"/> aggregate.</summary>
public sealed class UnitOfMeasurementConfiguration : IEntityTypeConfiguration<UnitOfMeasurement>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UnitOfMeasurement> builder)
    {
        builder.ToTable("UnitOfMeasurement");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => UnitOfMeasurementId.From(value))
            .ValueGeneratedNever();

        builder.Property(u => u.OrganizationId)
            .IsRequired();

        builder.Property(u => u.Name)
            .HasMaxLength(UnitOfMeasurement.MaxNameLength)
            .IsRequired();

        builder.Property(u => u.Category)
            .HasMaxLength(UnitOfMeasurement.MaxCategoryLength)
            .IsRequired();

        builder.HasIndex(u => u.OrganizationId);
        builder.HasIndex(u => new { u.OrganizationId, u.Category });
    }
}