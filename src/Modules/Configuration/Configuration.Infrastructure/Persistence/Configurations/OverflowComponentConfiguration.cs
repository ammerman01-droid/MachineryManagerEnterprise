using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="OverflowComponent"/> aggregate.</summary>
public sealed class OverflowComponentConfiguration : IEntityTypeConfiguration<OverflowComponent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<OverflowComponent> builder)
    {
        builder.ToTable("OverflowComponent");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(id => id.Value, value => OverflowComponentId.From(value))
            .ValueGeneratedNever();

        builder.Property(l => l.HoldingId)
            .IsRequired();

        builder.Property(l => l.Name)
            .HasMaxLength(OverflowComponent.MaxNameLength)
            .IsRequired();

        builder.HasIndex(l => new { l.HoldingId, l.Name })
            .IsUnique();
    }
}
