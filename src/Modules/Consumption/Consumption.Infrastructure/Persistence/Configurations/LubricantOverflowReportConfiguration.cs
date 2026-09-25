using Consumption.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Consumption.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core mapping for the <see cref="LubricantOverflowReport"/>
/// aggregate. <see cref="LubricantOverflowLine"/> and
/// <see cref="LubricantOverflowReportPersonnelEntry"/> are mapped as
/// owned collections (<c>OwnsMany</c>) because they have no identity or
/// lifecycle outside their owning report.
/// </summary>
public sealed class LubricantOverflowReportConfiguration : IEntityTypeConfiguration<LubricantOverflowReport>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<LubricantOverflowReport> builder)
    {
        builder.ToTable("LubricantOverflowReport");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(id => id.Value, value => LubricantOverflowReportId.From(value))
            .ValueGeneratedNever();

        builder.Property(r => r.OrganizationId).IsRequired();
        builder.HasIndex(r => r.OrganizationId);

        // Fixed at creation (BR-017) — plain columns, indexed, no
        // database-level FK, since Project and Asset live in other
        // modules' DbContexts (same pattern as Asset.ProjectId/ColorId).
        builder.Property(r => r.ProjectId).IsRequired();
        builder.HasIndex(r => r.ProjectId);

        builder.Property(r => r.AssetId).IsRequired();
        builder.HasIndex(r => r.AssetId);

        builder.Property(r => r.ReportDate).IsRequired();
        builder.HasIndex(r => r.ReportDate);

        builder.Property(r => r.HourMeterReading)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Metadata.FindNavigation(nameof(LubricantOverflowReport.Lines))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(r => r.Lines, lineBuilder =>
        {
            lineBuilder.ToTable("LubricantOverflowLine");
            lineBuilder.WithOwner().HasForeignKey("LubricantOverflowReportId");
            lineBuilder.HasKey(l => l.Id);

            lineBuilder.Property(l => l.OverflowComponentId).IsRequired();
            lineBuilder.Property(l => l.LubricantTypeId).IsRequired();

            lineBuilder.Property(l => l.AmountInLiters)
                .HasPrecision(10, 2)
                .IsRequired();

            lineBuilder.Property(l => l.Reason)
                .HasMaxLength(LubricantOverflowLine.MaxReasonLength)
                .IsRequired();
        });

        builder.Metadata.FindNavigation(nameof(LubricantOverflowReport.PersonnelEntries))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(r => r.PersonnelEntries, entryBuilder =>
        {
            entryBuilder.ToTable("LubricantOverflowReportPersonnelEntry");
            entryBuilder.WithOwner().HasForeignKey("LubricantOverflowReportId");
            entryBuilder.HasKey(e => e.Id);

            entryBuilder.Property(e => e.PersonnelId).IsRequired();
            entryBuilder.Property(e => e.StartTime).IsRequired();
            entryBuilder.Property(e => e.Duration).IsRequired();
        });
    }
}
