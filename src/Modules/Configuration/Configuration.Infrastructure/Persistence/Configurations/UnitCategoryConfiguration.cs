using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="UnitCategory"/> aggregate.</summary>
public sealed class UnitCategoryConfiguration : IEntityTypeConfiguration<UnitCategory>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UnitCategory> builder)
    {
        builder.ToTable("UnitCategory");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => UnitCategoryId.From(value))
            .ValueGeneratedNever();

        builder.Property(c => c.HoldingId).IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(UnitCategory.MaxNameLength)
            .IsRequired();

        builder.HasIndex(c => c.HoldingId);
    }
}