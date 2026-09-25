using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="LubricantType"/> aggregate.</summary>
public sealed class LubricantTypeConfiguration : IEntityTypeConfiguration<LubricantType>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<LubricantType> builder)
    {
        builder.ToTable("LubricantType");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(id => id.Value, value => LubricantTypeId.From(value))
            .ValueGeneratedNever();

        builder.Property(l => l.HoldingId)
            .IsRequired();

        builder.Property(l => l.Name)
            .HasMaxLength(LubricantType.MaxNameLength)
            .IsRequired();

        builder.HasIndex(l => new { l.HoldingId, l.Name })
            .IsUnique();
    }
}
