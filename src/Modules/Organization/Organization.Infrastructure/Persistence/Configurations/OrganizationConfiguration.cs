using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain;

namespace MachineryManager.Organization.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core mapping for the <see cref="global::Organization.Domain.Organization"/> aggregate.
/// </summary>
public sealed class OrganizationConfiguration : IEntityTypeConfiguration<global::Organization.Domain.Organization>
{
    /// <summary>Configures the entity mapping.</summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<global::Organization.Domain.Organization> builder)
    {
        builder.ToTable("Organization");

        builder.HasKey(organization => organization.Id);

        builder.Property(organization => organization.Id)
            .HasConversion(id => id.Value, value => OrganizationId.From(value))
            .ValueGeneratedNever();

        builder.Property(organization => organization.Name)
            .HasMaxLength(global::Organization.Domain.Organization.MaxNameLength)
            .IsRequired();

        // Optional parent Holding (BR-017, chat 2026-08-19).
        builder.Property(organization => organization.HoldingId)
            .HasConversion(
                id => id == null ? (Guid?)null : id.Value,
                value => value == null ? null : HoldingId.From(value.Value));

        builder.HasIndex(organization => organization.HoldingId);

        builder.Property(organization => organization.IsSuspended)
    .IsRequired()
    .HasDefaultValue(false);

        builder.Property(organization => organization.SuspendedAt);

        // Domain Events are transient behavior, never persisted state.
        builder.Ignore(organization => organization.DomainEvents);
    }
}
