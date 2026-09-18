using Configuration.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="JobTitle"/> aggregate.</summary>
public sealed class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("JobTitle");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Id)
            .HasConversion(id => id.Value, value => JobTitleId.From(value))
            .ValueGeneratedNever();

        builder.Property(j => j.HoldingId).IsRequired();

        builder.Property(j => j.Name)
            .HasMaxLength(JobTitle.MaxNameLength)
            .IsRequired();

        builder.HasIndex(j => j.HoldingId);
    }
}