using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="Color"/> aggregate.</summary>
public sealed class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable("Color");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => ColorId.From(value))
            .ValueGeneratedNever();

        builder.Property(c => c.HoldingId).IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(Color.MaxNameLength)
            .IsRequired();

        builder.HasIndex(c => c.HoldingId);
    }
}