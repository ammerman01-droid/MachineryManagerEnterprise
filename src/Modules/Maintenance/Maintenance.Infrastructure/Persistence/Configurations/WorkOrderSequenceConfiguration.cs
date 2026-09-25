using MachineryManagerEnterprise.Maintenance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Maintenance.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the infrastructure-only <see cref="WorkOrderSequence"/> counter.</summary>
public sealed class WorkOrderSequenceConfiguration : IEntityTypeConfiguration<WorkOrderSequence>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<WorkOrderSequence> builder)
    {
        builder.ToTable("WorkOrderSequence");
        builder.HasKey(s => s.OrganizationId);
        builder.Property(s => s.OrganizationId).ValueGeneratedNever();
        builder.Property(s => s.LastNumber).IsRequired();
    }
}
