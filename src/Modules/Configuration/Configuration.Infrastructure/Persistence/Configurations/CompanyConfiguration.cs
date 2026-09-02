using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManager.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core mapping for the <see cref="Company"/> aggregate.
/// </summary>
public sealed class CompanyConfiguration
    : IEntityTypeConfiguration<Company>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Company");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CompanyId.From(value))
            .ValueGeneratedNever();

        builder.Property(c => c.HoldingId)
            .IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(Company.MaxNameLength)
            .IsRequired();

        builder.HasIndex(c => c.HoldingId);

        builder.HasIndex(c => new
        {
            c.HoldingId,
            c.Name
        })
        .IsUnique();
    }
}