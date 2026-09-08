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

        builder.Property(u => u.HoldingId).IsRequired();

        builder.Property(u => u.Name)
            .HasMaxLength(UnitOfMeasurement.MaxNameLength)
            .IsRequired();

        builder.Property(u => u.Kind)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(u => u.HoldingId);
        builder.HasIndex(u => new { u.HoldingId, u.Kind });
    }
}