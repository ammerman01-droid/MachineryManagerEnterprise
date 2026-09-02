using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="FuelType"/> aggregate.</summary>
public sealed class FuelTypeConfiguration : IEntityTypeConfiguration<FuelType>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<FuelType> builder)
    {
        builder.ToTable("FuelType");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasConversion(id => id.Value, value => FuelTypeId.From(value))
            .ValueGeneratedNever();

        builder.Property(f => f.HoldingId).IsRequired();

        builder.Property(f => f.Name)
            .HasMaxLength(FuelType.MaxNameLength)
            .IsRequired();

        builder.Property(f => f.Price).IsRequired();

        // Stored as string, not int, so the raw database value stays
        // readable and resilient to future enum-member reordering —
        // same rationale as UnitCategory.Kind (chat, 2026-09-02).
        builder.Property(f => f.Kind)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(f => f.HoldingId);
    }
}