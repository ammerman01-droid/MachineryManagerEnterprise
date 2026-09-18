using MachineryManagerEnterprise.Personnel.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Personnel.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the Personnel aggregate and its owned <see cref="PersonnelDrivingLicense"/> child entities.</summary>
public sealed class PersonnelConfiguration : IEntityTypeConfiguration<global::MachineryManagerEnterprise.Personnel.Domain.Personnel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<global::MachineryManagerEnterprise.Personnel.Domain.Personnel> builder)
    {
        builder.ToTable("Personnel");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => PersonnelId.From(value))
            .ValueGeneratedNever();

        builder.Property(p => p.OrganizationId).IsRequired();
        builder.Property(p => p.CurrentProjectId).IsRequired();

        builder.Property(p => p.FirstName)
            .HasMaxLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxFirstNameLength)
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasMaxLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxLastNameLength)
            .IsRequired();

        builder.Property(p => p.PersonnelCode)
            .HasMaxLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxPersonnelCodeLength)
            .IsRequired();

        builder.Property(p => p.JobTitleId).IsRequired();

        builder.HasIndex(p => p.OrganizationId);
        builder.HasIndex(p => p.CurrentProjectId);

        // Enforces the agreed rule: PersonnelCode is unique per Organization.
        builder.HasIndex(p => new { p.OrganizationId, p.PersonnelCode }).IsUnique();

        // Owned child entities: a Personnel may hold zero or more driving
        // licenses, each with its own identity but no independent
        // repository/lifecycle outside the Personnel aggregate.
        builder.OwnsMany(p => p.DrivingLicenses, license =>
        {
            license.ToTable("PersonnelDrivingLicense");
            license.WithOwner().HasForeignKey("PersonnelId");
            license.HasKey(l => l.Id);

            license.Property(l => l.Id)
                .HasConversion(id => id.Value, value => PersonnelDrivingLicenseId.From(value))
                .ValueGeneratedNever();

            license.Property(l => l.DrivingLicenseTypeId).IsRequired();

            license.Property(l => l.ExpiryDate)
                .HasColumnType("date")
                .IsRequired();

            // A Personnel may have at most one license per Driving License Type (enforced in the domain, mirrored here).
            license.HasIndex("PersonnelId", nameof(PersonnelDrivingLicense.DrivingLicenseTypeId)).IsUnique();
        });

        // DrivingLicenses is exposed as a read-only IReadOnlyList<T> backed
        // by a private field with no public setter — EF must read/write
        // through the field, not the property.
        builder.Navigation(p => p.DrivingLicenses).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}