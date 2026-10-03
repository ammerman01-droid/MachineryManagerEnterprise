using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="global::Usage.Domain.UsageLedger"/> aggregate.</summary>
public sealed class UsageLedgerConfiguration : IEntityTypeConfiguration<global::Usage.Domain.UsageLedger>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<global::Usage.Domain.UsageLedger> builder)
    {
        builder.ToTable("UsageLedger");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(id => id.Value, value => global::Usage.Domain.UsageLedgerId.From(value))
            .ValueGeneratedNever();

        builder.Property(l => l.OwnerType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Plain column, no database-level FK — polymorphic owner
        // (Asset or Tracked Component, BR-004), same cross-module
        // reference pattern as MeterDevice.OwnerId.
        builder.Property(l => l.OwnerId)
            .IsRequired();

        builder.Property(l => l.OrganizationId)
            .IsRequired();

        builder.HasIndex(l => l.OrganizationId);

        builder.Property(l => l.Unit)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // One Ledger per (Owner, Unit) — the aggregate's natural key,
        // enforced at the database level (see IUsageLedgerRepository.GetByOwnerAsync).
        builder.HasIndex(l => new { l.OwnerType, l.OwnerId, l.Unit })
            .IsUnique();

        builder.Property(l => l.FrozenUpToDate);

        // The full append-only timeline (chat, 2026-09-29 redesign).
        // Each entry keeps its own identity (UsageEntryId) and is
        // individually mutable (ApplyReadingCorrection etc.), so it is
        // an owned entity collection rather than a value-object one.
        builder.OwnsMany(l => l.Entries, entryBuilder =>
        {
            entryBuilder.ToTable("UsageEntry");

            entryBuilder.WithOwner().HasForeignKey("UsageLedgerId");
            entryBuilder.HasKey(e => e.Id);

            entryBuilder.Property(e => e.Id)
                .HasConversion(id => id.Value, value => global::Usage.Domain.UsageEntryId.From(value))
                .ValueGeneratedNever();

            // The sole ordering key (chat, 2026-09-29) — see the
            // ⚠ ordering-risk remark below and UsageLedgerRepository:
            // owned-collection materialization order is not otherwise
            // guaranteed by EF Core, and every one of UsageLedger's
            // cursor-based operations (RegisterShiftReading,
            // CorrectEntryReading, DeleteLatestEntry, ...) depends on
            // Entries being in ascending Sequence order in memory.
            entryBuilder.Property(e => e.Sequence)
                .IsRequired();

            entryBuilder.HasIndex("UsageLedgerId", nameof(global::Usage.Domain.UsageEntry.Sequence))
                .IsUnique();

            entryBuilder.Property(e => e.Kind)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entryBuilder.Property(e => e.Origin)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entryBuilder.Property(e => e.EntryDate)
                .IsRequired();

            entryBuilder.Property(e => e.ShiftIndex);

            // One reading per (Ledger, Date, Shift) — filtered so the
            // constraint only applies to shift-reading rows (a Rebase
            // row has no ShiftIndex and there may be several rebases,
            // even same-day ones with different dates only in theory —
            // in practice at most one per date, but nothing here needs
            // to enforce that beyond the Domain's own ordering check).
            entryBuilder.HasIndex("UsageLedgerId", nameof(global::Usage.Domain.UsageEntry.EntryDate), nameof(global::Usage.Domain.UsageEntry.ShiftIndex))
                .IsUnique()
                .HasFilter("[ShiftIndex] IS NOT NULL");

            entryBuilder.Property(e => e.ShiftStartTime);
            entryBuilder.Property(e => e.ShiftEndTime);

            entryBuilder.Property(e => e.RawReadingValue)
                .HasPrecision(18, 4)
                .IsRequired();

            entryBuilder.Property(e => e.SourceMeterDeviceId)
                .HasConversion(id => id.Value, value => global::Usage.Domain.MeterDeviceId.From(value))
                .IsRequired();

            entryBuilder.HasIndex(e => e.SourceMeterDeviceId);

            entryBuilder.Property(e => e.OperationalUsageAmount)
                .HasPrecision(18, 4)
                .IsRequired();

            entryBuilder.Property(e => e.OperatorId);

            // Project snapshot (BR-054). Plain column, no database-level
            // FK: Project lives in the Organization module's own DbContext.
            entryBuilder.Property(e => e.ProjectId)
                .IsRequired();

            entryBuilder.HasIndex(e => e.ProjectId);
        });
    }
}
