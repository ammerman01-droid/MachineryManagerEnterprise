using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Usage.Domain.Events;

namespace Usage.Domain;

/// <summary>
/// Aggregate Root representing a physical Meter Device (an odometer or
/// hour-meter) and its lifecycle (Registered, Installed, Operational,
/// Failed, Removed, Archived). Deliberately independent from
/// <see cref="UsageLedger"/> (BR-010): it never stores readings or
/// computed usage itself — every reading it produces is registered on
/// the owner's <see cref="UsageLedger"/>, which is the sole source of
/// truth for Operational Usage (BR-015).
/// <see cref="Unit"/> is fixed for the lifetime of the device, set once
/// at <see cref="Register"/> time (chat, 2026-09-29) — a device
/// measuring hours can never later measure kilometers, regardless of
/// which owner it is installed on (BR-011/BR-013).
/// </summary>
public sealed class MeterDevice : AggregateRoot<MeterDeviceId>
{
    /// <summary>Gets the identifier of the Organization this device belongs to.</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the unit this device reads in (Hour, Kilometer, or Mile), fixed at <see cref="Register"/> time.</summary>
    public MeterReadingUnit Unit { get; private set; }

    /// <summary>Gets the current lifecycle status of this device.</summary>
    public MeterDeviceStatus Status { get; private set; }

    /// <summary>Gets the kind of the current owner (Asset or Component), or <see langword="null"/> when not installed.</summary>
    public UsageOwnerType? OwnerType { get; private set; }

    /// <summary>Gets the identifier of the current owner, or <see langword="null"/> when not installed.</summary>
    public Guid? OwnerId { get; private set; }

    /// <summary>Gets the owner-specific override for the daily Operational Usage cap (BR-045), or <see langword="null"/> to use the system default.</summary>
    public decimal? DailyCapOverride { get; private set; }

    // Reserved for ORM materialization only. Never used by application code.
    private MeterDevice()
    {
    }

    private MeterDevice(MeterDeviceId id, Guid organizationId, MeterReadingUnit unit, decimal? dailyCapOverride)
        : base(id)
    {
        OrganizationId = organizationId;
        Unit = unit;
        DailyCapOverride = dailyCapOverride;
        Status = MeterDeviceStatus.Registered;
    }

    /// <summary>
    /// Registers a new Meter Device with a fixed reading <paramref name="unit"/>
    /// (chat, 2026-09-29) — the unit can never be changed afterward, on
    /// any owner.
    /// </summary>
    /// <param name="organizationId">The Organization this device belongs to.</param>
    /// <param name="unit">The unit this device reads in; fixed for its lifetime.</param>
    /// <param name="dailyCapOverride">An optional owner-specific override for the daily Operational Usage cap (BR-045).</param>
    public static Result<MeterDevice> Register(Guid organizationId, MeterReadingUnit unit, decimal? dailyCapOverride)
    {
        if (organizationId == Guid.Empty)
        {
            return Result.Failure<MeterDevice>(MeterDeviceErrors.OrganizationRequired());
        }

        if (!Enum.IsDefined(unit))
        {
            return Result.Failure<MeterDevice>(MeterDeviceErrors.InvalidUnit());
        }

        if (dailyCapOverride is <= 0)
        {
            return Result.Failure<MeterDevice>(MeterDeviceErrors.InvalidDailyCapOverride());
        }

        return new MeterDevice(MeterDeviceId.New(), organizationId, unit, dailyCapOverride);
    }

    /// <summary>
    /// Installs this device on an owner (Asset or Component). The
    /// owner's configured <paramref name="ownerMeterReadingUnit"/> must
    /// match this device's fixed <see cref="Unit"/> (BR-011/BR-013) —
    /// moving a device to an owner configured for a different unit is
    /// rejected outright rather than silently reinterpreted.
    /// </summary>
    public Result Install(UsageOwnerType ownerType, Guid ownerId, MeterReadingUnit ownerMeterReadingUnit, IDateTimeProvider dateTimeProvider)
    {
        if (Status is not (MeterDeviceStatus.Registered or MeterDeviceStatus.Removed))
        {
            return Result.Failure(MeterDeviceErrors.InvalidTransition(Status, MeterDeviceStatus.Installed));
        }

        if (ownerId == Guid.Empty)
        {
            return Result.Failure(MeterDeviceErrors.OwnerRequired());
        }

        if (Unit != ownerMeterReadingUnit)
        {
            return Result.Failure(MeterDeviceErrors.UnitMismatch(Unit, ownerMeterReadingUnit));
        }

        OwnerType = ownerType;
        OwnerId = ownerId;
        Status = MeterDeviceStatus.Installed;

        RaiseDomainEvent(new MeterInstalled(Id, ownerType, ownerId, Unit, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Marks this device as having produced at least one accepted
    /// reading on its owner's <see cref="UsageLedger"/> (chat,
    /// 2026-09-29). Called by the Application layer immediately after
    /// the Ledger accepts the reading — this aggregate never records
    /// the reading itself.
    /// </summary>
    public Result MarkOperational()
    {
        if (Status is not (MeterDeviceStatus.Installed or MeterDeviceStatus.Operational))
        {
            return Result.Failure(MeterDeviceErrors.NotReadyForReading(Status));
        }

        Status = MeterDeviceStatus.Operational;

        return Result.Success();
    }

    /// <summary>Reports that this device has stopped producing readings (e.g. sensor failure).</summary>
    public Result ReportFailure(IDateTimeProvider dateTimeProvider)
    {
        if (Status is not (MeterDeviceStatus.Installed or MeterDeviceStatus.Operational))
        {
            return Result.Failure(MeterDeviceErrors.InvalidTransition(Status, MeterDeviceStatus.Failed));
        }

        Status = MeterDeviceStatus.Failed;
        RaiseDomainEvent(new MeterFailureDetected(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>Recovers a previously failed device back to Installed, ready to resume producing readings.</summary>
    public Result Recover()
    {
        if (Status != MeterDeviceStatus.Failed)
        {
            return Result.Failure(MeterDeviceErrors.InvalidTransition(Status, MeterDeviceStatus.Installed));
        }

        Status = MeterDeviceStatus.Installed;

        return Result.Success();
    }

    /// <summary>Removes this device from its current owner, making it available for re-installation elsewhere.</summary>
    public Result Remove(IDateTimeProvider dateTimeProvider)
    {
        if (Status is not (MeterDeviceStatus.Installed or MeterDeviceStatus.Operational or MeterDeviceStatus.Failed))
        {
            return Result.Failure(MeterDeviceErrors.InvalidTransition(Status, MeterDeviceStatus.Removed));
        }

        var previousOwnerType = OwnerType!.Value;
        var previousOwnerId = OwnerId!.Value;

        OwnerType = null;
        OwnerId = null;
        Status = MeterDeviceStatus.Removed;

        RaiseDomainEvent(new MeterRemoved(Id, previousOwnerType, previousOwnerId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>Permanently retires a removed device.</summary>
    public Result Archive(IDateTimeProvider dateTimeProvider)
    {
        if (Status != MeterDeviceStatus.Removed)
        {
            return Result.Failure(MeterDeviceErrors.InvalidTransition(Status, MeterDeviceStatus.Archived));
        }

        Status = MeterDeviceStatus.Archived;
        RaiseDomainEvent(new MeterArchived(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>Updates the owner-specific override for the daily Operational Usage cap (BR-045).</summary>
    public Result SetDailyCapOverride(decimal? newValue)
    {
        if (newValue is <= 0)
        {
            return Result.Failure(MeterDeviceErrors.InvalidDailyCapOverride());
        }

        DailyCapOverride = newValue;

        return Result.Success();
    }
}
