using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Configuration.Infrastructure.Persistence.Configurations;

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

        builder.Property(u => u.HoldingId)
            .IsRequired();

        builder.Property(u => u.Name)
            .HasMaxLength(UnitOfMeasurement.MaxNameLength)
            .IsRequired();

        builder.Property(u => u.CategoryId)
            .HasConversion(id => id.Value, value => UnitCategoryId.From(value))
            .IsRequired();

        // Real database-level FK to UnitCategory — Restrict (not
        // Cascade): a UnitCategory cannot be removed while any Unit of
        // Measurement still references it, same reasoning as the
        // AssetModel/Color FKs on the Asset aggregate.
        builder.HasOne<UnitCategory>()
            .WithMany()
            .HasForeignKey(u => u.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(u => u.HoldingId);
        builder.HasIndex(u => new { u.HoldingId, u.CategoryId });
    }
}