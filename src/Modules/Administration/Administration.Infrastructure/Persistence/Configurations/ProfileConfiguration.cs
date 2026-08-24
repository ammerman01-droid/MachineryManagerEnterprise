using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Administration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="global::Administration.Domain.Profile"/> aggregate.</summary>
public sealed class ProfileConfiguration : IEntityTypeConfiguration<global::Administration.Domain.Profile>
{
    /// <summary>Configures the entity mapping.</summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<global::Administration.Domain.Profile> builder)
    {
        builder.ToTable("Profile");

        builder.HasKey(profile => profile.Id);

        builder.Property(profile => profile.Id)
            .HasConversion(id => id.Value, value => global::Administration.Domain.ProfileId.From(value))
            .ValueGeneratedNever();

        builder.Property(profile => profile.Name)
            .HasMaxLength(global::Administration.Domain.Profile.MaxNameLength)
            .IsRequired();

        builder.Property(profile => profile.IsActive)
            .IsRequired();

        builder.Property(profile => profile.CreatedAt)
            .IsRequired();

                builder.Property(profile => profile.Permissions)
            .HasField("_permissions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(profile => profile.DomainEvents);
    }
}