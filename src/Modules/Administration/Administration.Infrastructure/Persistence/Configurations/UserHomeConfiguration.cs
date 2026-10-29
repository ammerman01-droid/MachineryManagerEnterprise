using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Administration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping of <see cref="global::Administration.Domain.UserHome"/>.</summary>
public sealed class UserHomeConfiguration : IEntityTypeConfiguration<global::Administration.Domain.UserHome>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<global::Administration.Domain.UserHome> builder)
    {
        builder.ToTable("UserHome");

        builder.HasKey(home => home.UserId);

        builder.Property(home => home.UserId)
            .ValueGeneratedNever();

        builder.Property(home => home.HoldingId);
        builder.Property(home => home.OrganizationId);
        builder.Property(home => home.ProjectId);

        builder.Property(home => home.CreatedAt)
            .IsRequired();

        builder.Ignore(home => home.Level);

        builder.HasIndex(home => home.HoldingId).HasDatabaseName("IX_UserHome_HoldingId");
        builder.HasIndex(home => home.OrganizationId).HasDatabaseName("IX_UserHome_OrganizationId");
    }
}
