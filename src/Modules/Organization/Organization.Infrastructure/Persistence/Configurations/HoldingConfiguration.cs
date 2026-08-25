using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain;

namespace MachineryManager.Organization.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="Holding"/> aggregate.</summary>
public sealed class HoldingConfiguration : IEntityTypeConfiguration<Holding>
{
    /// <summary>Configures the entity mapping.</summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Holding> builder)
    {
        builder.ToTable("Holding");

        builder.HasKey(holding => holding.Id);

        builder.Property(holding => holding.Id)
            .HasConversion(id => id.Value, value => HoldingId.From(value))
            .ValueGeneratedNever();

        builder.Property(holding => holding.Name)
            .HasMaxLength(Holding.MaxNameLength)
            .IsRequired();

        // Domain Events are transient behavior, never persisted state.
        builder.Ignore(holding => holding.DomainEvents);
    }
}