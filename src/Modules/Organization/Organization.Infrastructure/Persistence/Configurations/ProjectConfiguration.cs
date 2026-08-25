using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Organization.Domain;

namespace MachineryManager.Organization.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core mapping for the <see cref="Project"/> aggregate. Per DDD
/// aggregate boundary discipline, this only stores the owning
/// <see cref="Project.OrganizationId"/> as a value (indexed for
/// query performance) — no cross-aggregate navigation/FK constraint
/// is configured, consistent with "one transaction = one aggregate
/// only" (Section 4.5).
/// </summary>
public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    /// <summary>Configures the entity mapping.</summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Project");

        builder.HasKey(project => project.Id);

        builder.Property(project => project.Id)
            .HasConversion(id => id.Value, value => ProjectId.From(value))
            .ValueGeneratedNever();

        builder.Property(project => project.OrganizationId)
            .HasConversion(id => id.Value, value => OrganizationId.From(value))
            .IsRequired();

        builder.HasIndex(project => project.OrganizationId);

        builder.Property(project => project.Name)
            .HasMaxLength(Project.MaxNameLength)
            .IsRequired();

        // Domain Events are transient behavior, never persisted state.
        builder.Ignore(project => project.DomainEvents);
    }
}