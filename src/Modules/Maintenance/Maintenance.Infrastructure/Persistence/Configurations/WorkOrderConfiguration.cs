using MachineryManagerEnterprise.Maintenance.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Maintenance.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="WorkOrder"/> aggregate.</summary>
public sealed class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.ToTable("WorkOrder");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasConversion(id => id.Value, value => WorkOrderId.From(value))
            .ValueGeneratedNever();

        builder.Property(w => w.OrganizationId).IsRequired();
        builder.HasIndex(w => w.OrganizationId);

        // Asset, Project, and responsible Personnel are plain columns,
        // index only, no database-level FK — they live in other
        // modules' DbContexts (same pattern as Asset.ProjectId/ColorId).
        builder.Property(w => w.AssetId).IsRequired();
        builder.HasIndex(w => w.AssetId);

        builder.Property(w => w.ProjectId).IsRequired();
        builder.HasIndex(w => w.ProjectId);

        builder.Property(w => w.ResponsiblePersonnelId).IsRequired();
        builder.HasIndex(w => w.ResponsiblePersonnelId);

        builder.Property(w => w.Number).IsRequired();
        builder.HasIndex(w => new { w.OrganizationId, w.Number }).IsUnique();

        builder.Property(w => w.ReportedAt).IsRequired();

        builder.Property(w => w.ResultingAssetStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(w => w.MeterReading)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(w => w.Priority)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(w => w.RepairType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(w => w.ObservationDescription)
            .HasMaxLength(WorkOrder.MaxObservationDescriptionLength)
            .IsRequired();

        builder.Property(w => w.PredictedRepairLocation)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(w => w.PartNeedingRepair)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(w => w.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(w => w.CancellationReason)
            .HasMaxLength(WorkOrder.MaxCancellationReasonLength);
    }
}
