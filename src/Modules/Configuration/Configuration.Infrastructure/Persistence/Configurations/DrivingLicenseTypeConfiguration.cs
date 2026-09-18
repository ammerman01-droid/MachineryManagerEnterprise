using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="DrivingLicenseType"/> aggregate.</summary>
public sealed class DrivingLicenseTypeConfiguration : IEntityTypeConfiguration<DrivingLicenseType>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<DrivingLicenseType> builder)
    {
        builder.ToTable("DrivingLicenseType");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(id => id.Value, value => DrivingLicenseTypeId.From(value))
            .ValueGeneratedNever();

        builder.Property(d => d.HoldingId).IsRequired();

        builder.Property(d => d.Name)
            .HasMaxLength(DrivingLicenseType.MaxNameLength)
            .IsRequired();

        builder.HasIndex(d => d.HoldingId);
    }
}