using MachineryManagerEnterprise.Consumption.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Consumption.Domain;

/// <summary>
/// Aggregate Root representing a single fuel-fill event for one fuel
/// slot of one Asset (chat, 2026-09-15/16/20/22). Captures the fuel
/// quantity, a reference to the specific Configuration FuelType used
/// and its price snapshot, together with the Asset's meter reading
/// (odometer/hour-meter) at the moment of fueling, and who
/// delivered/received the fuel. Owned by exactly one Organization
/// (mirroring Asset), with the Asset's current Project recorded
/// alongside it for historical reporting.
/// A record always targets exactly one of the Asset's fuel slots
/// (<see cref="FuelSlot"/>) — refueling a bi-fuel Asset's two tanks in
/// one visit requires two separate records. The fuel kind, fuel unit,
/// and meter-reading unit are all snapshotted from the Asset at the
/// time of recording, because the Asset's configuration can change
/// later and a historical record must not silently reinterpret its
/// own units.
/// <see cref="FuelTypeId"/> is a specific Configuration.FuelType
/// Aggregate chosen by the user (chat, 2026-09-22) — a Holding may
/// register several FuelTypes sharing the same <see cref="FuelKind"/>
/// (e.g. different diesel grades/vendors with different prices);
/// FuelKind is used only to constrain which FuelTypes are valid for
/// this record's slot, never to derive the price. Unlike Meter
/// Reading elsewhere in the spec, records of this aggregate are
/// directly editable and hard-deletable — there is no
/// immutable/correction-record requirement here; the monotonic
/// meter-reading chain is only checked against an Asset's immediately
/// neighboring records, by the caller.
/// </summary>
public sealed class FuelConsumption : AggregateRoot<FuelConsumptionId>
{
    /// <summary>The maximum allowed length for the notes field.</summary>
    public const int MaxNotesLength = 500;

    /// <summary>Gets the identifier of the Asset this record belongs to.</summary>
    public Guid AssetId { get; private set; }

    /// <summary>Gets the identifier of the owning Organization (copied from the Asset).</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Gets the identifier of the Project the Asset was assigned to at
    /// the time of fueling (snapshotted from the Asset's ProjectId).
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>Gets which of the Asset's fuel slots (Primary/Secondary) this record refers to.</summary>
    public FuelSlot FuelSlot { get; private set; }

    /// <summary>
    /// Gets the fuel kind actually filled, snapshotted from the
    /// Asset's PrimaryFuelKind/SecondaryFuelKind (matching
    /// <see cref="FuelSlot"/>) at the time of this record's creation.
    /// Immutable — the selected <see cref="FuelTypeId"/> (on Create or
    /// on Edit) must always match this value.
    /// </summary>
    public FuelKind FuelKind { get; private set; }

    /// <summary>
    /// Gets the unit the fuel quantity is counted in, snapshotted from
    /// the Asset's PrimaryFuelUnit/SecondaryFuelUnit (matching
    /// <see cref="FuelSlot"/>) at the time of this record's creation.
    /// </summary>
    public FuelUnit FuelUnit { get; private set; }

    /// <summary>
    /// Gets the identifier of the specific Configuration.FuelType
    /// Aggregate the user selected for this fill event (chat,
    /// 2026-09-22). Editable — changing it re-derives
    /// <see cref="UnitPriceSnapshot"/> from the newly selected
    /// FuelType (enforced by the caller/Application layer, since the
    /// aggregate cannot query other modules).
    /// </summary>
    public Guid FuelTypeId { get; private set; }

    /// <summary>
    /// Gets the fuel's unit price, snapshotted from the selected
    /// <see cref="FuelTypeId"/> at the moment of Create or the most
    /// recent Edit. Historical records are unaffected by later price
    /// changes on the FuelType itself.
    /// </summary>
    public decimal UnitPriceSnapshot { get; private set; }

    /// <summary>Gets the fuel quantity, in <see cref="FuelUnit"/>.</summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Gets the unit the meter reading is counted in, snapshotted from
    /// the Asset's MeterReadingUnit at the time of this record's
    /// creation.
    /// </summary>
    public MeterReadingUnit MeterReadingUnit { get; private set; }

    /// <summary>
    /// Gets the Asset's meter reading (in <see cref="MeterReadingUnit"/>)
    /// at the moment of fueling. Must be monotonically non-decreasing
    /// across an Asset's records — enforced by the caller (Application
    /// layer) against this Asset's immediately neighboring records,
    /// since the aggregate itself cannot query other aggregates.
    /// </summary>
    public decimal MeterReading { get; private set; }

    /// <summary>Gets the identifier of the Personnel who delivered the fuel.</summary>
    public Guid DeliveredByPersonnelId { get; private set; }

    /// <summary>Gets the identifier of the Personnel who received the fuel.</summary>
    public Guid ReceivedByPersonnelId { get; private set; }

    /// <summary>Gets the date and time the fueling actually took place.</summary>
    public DateTimeOffset RecordedAtUtc { get; private set; }

    /// <summary>Gets free-form notes about this record, if any.</summary>
    public string? Notes { get; private set; }

    // Reserved for ORM materialization only. Never used by application code.
    private FuelConsumption()
    {
    }

    private FuelConsumption(
        FuelConsumptionId id,
        Guid assetId,
        Guid organizationId,
        Guid projectId,
        FuelSlot fuelSlot,
        FuelKind fuelKind,
        FuelUnit fuelUnit,
        Guid fuelTypeId,
        decimal unitPriceSnapshot,
        decimal quantity,
        MeterReadingUnit meterReadingUnit,
        decimal meterReading,
        Guid deliveredByPersonnelId,
        Guid receivedByPersonnelId,
        DateTimeOffset recordedAtUtc,
        string? notes)
        : base(id)
    {
        AssetId = assetId;
        OrganizationId = organizationId;
        ProjectId = projectId;
        FuelSlot = fuelSlot;
        FuelKind = fuelKind;
        FuelUnit = fuelUnit;
        FuelTypeId = fuelTypeId;
        UnitPriceSnapshot = unitPriceSnapshot;
        Quantity = quantity;
        MeterReadingUnit = meterReadingUnit;
        MeterReading = meterReading;
        DeliveredByPersonnelId = deliveredByPersonnelId;
        ReceivedByPersonnelId = receivedByPersonnelId;
        RecordedAtUtc = recordedAtUtc;
        Notes = notes;
    }

    /// <summary>
    /// Records a new fuel-fill event. Existence of the referenced
    /// Asset/Personnel/FuelType, that the Asset actually has the
    /// requested <paramref name="fuelSlot"/> and a MeterReadingUnit
    /// configured, resolution of <paramref name="fuelKind"/>/
    /// <paramref name="fuelUnit"/>, that the selected FuelType belongs
    /// to the Asset's Holding and matches <paramref name="fuelKind"/>,
    /// and the monotonic meter-reading check against this Asset's
    /// neighboring records are all enforced by the caller (Application
    /// layer) before this method is invoked — the aggregate itself
    /// cannot check other aggregates or modules.
    /// </summary>
    public static Result<FuelConsumption> Record(
        Guid assetId,
        Guid organizationId,
        Guid projectId,
        FuelSlot fuelSlot,
        FuelKind fuelKind,
        FuelUnit fuelUnit,
        Guid fuelTypeId,
        decimal unitPriceSnapshot,
        decimal quantity,
        MeterReadingUnit meterReadingUnit,
        decimal meterReading,
        Guid deliveredByPersonnelId,
        Guid receivedByPersonnelId,
        DateTimeOffset recordedAtUtc,
        string? notes,
        IDateTimeProvider dateTimeProvider)
    {
        var validation = Validate(
            assetId, organizationId, projectId, fuelTypeId, unitPriceSnapshot, quantity,
            meterReading, deliveredByPersonnelId, receivedByPersonnelId, notes);

        if (validation.IsFailure)
        {
            return Result.Failure<FuelConsumption>(validation.Error);
        }

        var fuelConsumption = new FuelConsumption(
            FuelConsumptionId.New(),
            assetId,
            organizationId,
            projectId,
            fuelSlot,
            fuelKind,
            fuelUnit,
            fuelTypeId,
            unitPriceSnapshot,
            quantity,
            meterReadingUnit,
            meterReading,
            deliveredByPersonnelId,
            receivedByPersonnelId,
            recordedAtUtc,
            string.IsNullOrWhiteSpace(notes) ? null : notes.Trim());

        fuelConsumption.RaiseDomainEvent(
            new FuelConsumptionRecorded(fuelConsumption.Id, assetId, dateTimeProvider.UtcNow));

        return fuelConsumption;
    }

    /// <summary>
    /// Edits the factual details of an existing record in place (chat,
    /// 2026-09-15 — this aggregate is directly editable, unlike Meter
    /// Reading). Structural/snapshotted fields — <see cref="AssetId"/>,
    /// <see cref="OrganizationId"/>, <see cref="ProjectId"/>,
    /// <see cref="FuelSlot"/>, <see cref="FuelKind"/>,
    /// <see cref="FuelUnit"/>, <see cref="MeterReadingUnit"/> — are NOT
    /// editable; correcting which Asset/slot/fuel a record refers to
    /// requires deleting and re-recording. <see cref="FuelTypeId"/> and
    /// <see cref="UnitPriceSnapshot"/> ARE editable (chat, 2026-09-22 —
    /// supersedes the 2026-09-16 assumption that price is never
    /// re-derived on edit): the caller re-resolves and re-validates the
    /// (possibly new) FuelType exactly as on Create and passes its
    /// current price in here. The monotonic meter-reading check against
    /// this Asset's neighboring records is enforced by the caller
    /// (Application layer).
    /// </summary>
    public Result Edit(
        Guid fuelTypeId,
        decimal unitPriceSnapshot,
        decimal quantity,
        decimal meterReading,
        Guid deliveredByPersonnelId,
        Guid receivedByPersonnelId,
        DateTimeOffset recordedAtUtc,
        string? notes,
        IDateTimeProvider dateTimeProvider)
    {
        var validation = Validate(
            AssetId, OrganizationId, ProjectId, fuelTypeId, unitPriceSnapshot, quantity,
            meterReading, deliveredByPersonnelId, receivedByPersonnelId, notes);

        if (validation.IsFailure)
        {
            return validation;
        }

        FuelTypeId = fuelTypeId;
        UnitPriceSnapshot = unitPriceSnapshot;
        Quantity = quantity;
        MeterReading = meterReading;
        DeliveredByPersonnelId = deliveredByPersonnelId;
        ReceivedByPersonnelId = receivedByPersonnelId;
        RecordedAtUtc = recordedAtUtc;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();

        RaiseDomainEvent(new FuelConsumptionUpdated(Id, AssetId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    private static Result Validate(
        Guid assetId,
        Guid organizationId,
        Guid projectId,
        Guid fuelTypeId,
        decimal unitPriceSnapshot,
        decimal quantity,
        decimal meterReading,
        Guid deliveredByPersonnelId,
        Guid receivedByPersonnelId,
        string? notes)
    {
        if (assetId == Guid.Empty)
        {
            return Result.Failure(FuelConsumptionErrors.AssetRequired());
        }

        if (organizationId == Guid.Empty)
        {
            return Result.Failure(FuelConsumptionErrors.OrganizationRequired());
        }

        if (projectId == Guid.Empty)
        {
            return Result.Failure(FuelConsumptionErrors.ProjectRequired());
        }

        if (fuelTypeId == Guid.Empty)
        {
            return Result.Failure(FuelConsumptionErrors.FuelTypeRequired());
        }

        if (quantity <= 0)
        {
            return Result.Failure(FuelConsumptionErrors.QuantityMustBePositive());
        }

        if (unitPriceSnapshot < 0)
        {
            return Result.Failure(FuelConsumptionErrors.UnitPriceMustNotBeNegative());
        }

        if (meterReading < 0)
        {
            return Result.Failure(FuelConsumptionErrors.MeterReadingMustNotBeNegative());
        }

        if (deliveredByPersonnelId == Guid.Empty)
        {
            return Result.Failure(FuelConsumptionErrors.DeliveredByPersonnelRequired());
        }

        if (receivedByPersonnelId == Guid.Empty)
        {
            return Result.Failure(FuelConsumptionErrors.ReceivedByPersonnelRequired());
        }

        if (notes is { Length: > 0 } && notes.Length > MaxNotesLength)
        {
            return Result.Failure(FuelConsumptionErrors.NotesTooLong(MaxNotesLength));
        }

        return Result.Success();
    }
}
