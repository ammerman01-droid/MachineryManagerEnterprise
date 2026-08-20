using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Administration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="global::Administration.Domain.UserProfileAssignment"/> aggregate.</summary>
public sealed class UserProfileAssignmentConfiguration : IEntityTypeConfiguration<global::Administration.Domain.UserProfileAssignment>
{
    /// <summary>Configures the entity mapping.</summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<global::Administration.Domain.UserProfileAssignment> builder)
    {
        builder.ToTable("UserProfileAssignment");

        builder.HasKey(assignment => assignment.Id);

        builder.Property(assignment => assignment.Id)
            .HasConversion(id => id.Value, value => global::Administration.Domain.UserProfileAssignmentId.From(value))
            .ValueGeneratedNever();

        builder.Property(assignment => assignment.UserId)
            .IsRequired();

        builder.Property(assignment => assignment.ProfileId)
            .HasConversion(id => id.Value, value => global::Administration.Domain.ProfileId.From(value))
            .IsRequired();

        builder.Property(assignment => assignment.AssignedAt)
            .IsRequired();

        builder.ComplexProperty(assignment => assignment.Scope, scope =>
        {
            scope.Property(s => s.Level)
                .HasColumnName("ScopeLevel")
                .IsRequired();

            scope.Property(s => s.HoldingId)
                .HasColumnName("ScopeHoldingId");

            scope.Property(s => s.OrganizationId)
                .HasColumnName("ScopeOrganizationId");

            scope.Property(s => s.ProjectId)
                .HasColumnName("ScopeProjectId");
        });

        builder.Ignore(assignment => assignment.DomainEvents);
    }
}