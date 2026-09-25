using MachineryManagerEnterprise.Maintenance.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>
/// A Work Order (دستور کار): opened at the start of a failure or
/// stoppage that needs repair (chat, 2026-09-22). Records what is
/// suspected to be wrong and where/by whom it is expected to be fixed.
/// A Work Order results in exactly ONE repair — once converted, any
/// leftover or extra work is recorded on a NEW Work Order, not this
/// one. The actual work performed and parts consumed are recorded by
/// the separate Repair Execution part (a later phase), referencing this
/// Work Order's identifier.
/// </summary>
public sealed class WorkOrder : AggregateRoot<WorkOrderId>
{
    /// <summary>The maximum length allowed for <see cref="ObservationDescription"/>.</summary>
    public const int MaxObservationDescriptionLength = 2000;

    /// <summary>The maximum length allowed for <see cref="CancellationReason"/>.</summary>
    public const int MaxCancellationReasonLength = 1000;

    /// <summary>Gets the identifier of the owning Organization (mirrors Asset's OrganizationId at the moment this Work Order was registered).</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the identifier of the Asset this Work Order was opened for.</summary>
    public Guid AssetId { get; private set; }

    /// <summary>Gets the identifier of the Project the Asset was deployed at when this Work Order was registered (read from the Asset, not chosen separately).</summary>
    public Guid ProjectId { get; private set; }

    /// <summary>Gets the Work Order's human-readable sequential number, unique within the owning Organization, starting at 1.</summary>
    public int Number { get; private set; }

    /// <summary>Gets the exact date and time the stoppage or failure was reported.</summary>
    public DateTimeOffset ReportedAt { get; private set; }

    /// <summary>Gets the Asset's resulting operational status: the failure may just be reported (<see cref="WorkOrderAssetStatus.Active"/>) or it may have stopped the Asset (<see cref="WorkOrderAssetStatus.OutOfService"/>).</summary>
    public WorkOrderAssetStatus ResultingAssetStatus { get; private set; }

    /// <summary>Gets the Asset's counter/odometer reading at the moment of the stoppage.</summary>
    public decimal MeterReading { get; private set; }

    /// <summary>Gets the repair's priority.</summary>
    public RepairPriority Priority { get; private set; }

    /// <summary>Gets whether the anticipated repair is major or minor.</summary>
    public RepairType RepairType { get; private set; }

    /// <summary>Gets the identifier of the Personnel responsible for the repair.</summary>
    public Guid ResponsiblePersonnelId { get; private set; }

    /// <summary>Gets the description of the observations made about the failure.</summary>
    public string ObservationDescription { get; private set; } = string.Empty;

    /// <summary>Gets where the repair is predicted to be carried out.</summary>
    public RepairLocation PredictedRepairLocation { get; private set; }

    /// <summary>Gets the single main Asset part suspected to need repair.</summary>
    public RepairablePart PartNeedingRepair { get; private set; }

    /// <summary>Gets the Work Order's current lifecycle status.</summary>
    public WorkOrderStatus Status { get; private set; }

    /// <summary>Gets the reason given for cancellation, if the Work Order was cancelled; otherwise null.</summary>
    public string? CancellationReason { get; private set; }

    private WorkOrder(WorkOrderId id)
        : base(id)
    {
    }

    // Reserved for ORM materialization only.
    private WorkOrder()
    {
    }

    /// <summary>
    /// Registers a new Work Order in the <see cref="WorkOrderStatus.Open"/>
    /// status.
    /// </summary>
    /// <param name="organizationId">The owning Organization's identifier.</param>
    /// <param name="assetId">The Asset the Work Order is opened for.</param>
    /// <param name="projectId">The Project the Asset is currently deployed at.</param>
    /// <param name="number">The Work Order's sequential number within the Organization, assigned by the caller before this call.</param>
    /// <param name="reportedAt">The exact date and time the stoppage or failure was reported.</param>
    /// <param name="resultingAssetStatus">The Asset's resulting operational status.</param>
    /// <param name="meterReading">The Asset's counter/odometer reading at the moment of the stoppage.</param>
    /// <param name="priority">The repair's priority.</param>
    /// <param name="repairType">Whether the anticipated repair is major or minor.</param>
    /// <param name="responsiblePersonnelId">The identifier of the Personnel responsible for the repair.</param>
    /// <param name="observationDescription">The description of the observations made about the failure.</param>
    /// <param name="predictedRepairLocation">Where the repair is predicted to be carried out.</param>
    /// <param name="partNeedingRepair">The single main Asset part suspected to need repair.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{WorkOrder}"/> containing the new Work Order on success; otherwise a validation error.</returns>
    public static Result<WorkOrder> Register(
        Guid organizationId,
        Guid assetId,
        Guid projectId,
        int number,
        DateTimeOffset reportedAt,
        WorkOrderAssetStatus resultingAssetStatus,
        decimal meterReading,
        RepairPriority priority,
        RepairType repairType,
        Guid responsiblePersonnelId,
        string observationDescription,
        RepairLocation predictedRepairLocation,
        RepairablePart partNeedingRepair,
        IDateTimeProvider dateTimeProvider)
    {
        if (responsiblePersonnelId == Guid.Empty)
        {
            return Result.Failure<WorkOrder>(WorkOrderErrors.ResponsiblePersonnelRequired());
        }

        if (string.IsNullOrWhiteSpace(observationDescription))
        {
            return Result.Failure<WorkOrder>(WorkOrderErrors.ObservationDescriptionRequired());
        }

        if (observationDescription.Trim().Length > MaxObservationDescriptionLength)
        {
            return Result.Failure<WorkOrder>(WorkOrderErrors.ObservationDescriptionTooLong(MaxObservationDescriptionLength));
        }

        if (meterReading < 0)
        {
            return Result.Failure<WorkOrder>(WorkOrderErrors.MeterReadingNegative());
        }

        if (number <= 0)
        {
            return Result.Failure<WorkOrder>(WorkOrderErrors.InvalidNumber());
        }

        var workOrder = new WorkOrder(WorkOrderId.New())
        {
            OrganizationId = organizationId,
            AssetId = assetId,
            ProjectId = projectId,
            Number = number,
            ReportedAt = reportedAt,
            ResultingAssetStatus = resultingAssetStatus,
            MeterReading = meterReading,
            Priority = priority,
            RepairType = repairType,
            ResponsiblePersonnelId = responsiblePersonnelId,
            ObservationDescription = observationDescription.Trim(),
            PredictedRepairLocation = predictedRepairLocation,
            PartNeedingRepair = partNeedingRepair,
            Status = WorkOrderStatus.Open,
        };

        workOrder.RaiseDomainEvent(new WorkOrderRegistered(
            workOrder.Id, organizationId, assetId, dateTimeProvider.UtcNow));

        return workOrder;
    }

    /// <summary>
    /// Edits an Open Work Order's details. The Asset, Project, and
    /// Work Order number are permanent and cannot be edited.
    /// </summary>
    /// <returns>A successful <see cref="Result"/>, or a validation/conflict error.</returns>
    public Result Edit(
        DateTimeOffset reportedAt,
        WorkOrderAssetStatus resultingAssetStatus,
        decimal meterReading,
        RepairPriority priority,
        RepairType repairType,
        Guid responsiblePersonnelId,
        string observationDescription,
        RepairLocation predictedRepairLocation,
        RepairablePart partNeedingRepair,
        IDateTimeProvider dateTimeProvider)
    {
        if (Status != WorkOrderStatus.Open)
        {
            return Result.Failure(WorkOrderErrors.NotEditableInCurrentStatus(Status));
        }

        if (responsiblePersonnelId == Guid.Empty)
        {
            return Result.Failure(WorkOrderErrors.ResponsiblePersonnelRequired());
        }

        if (string.IsNullOrWhiteSpace(observationDescription))
        {
            return Result.Failure(WorkOrderErrors.ObservationDescriptionRequired());
        }

        if (observationDescription.Trim().Length > MaxObservationDescriptionLength)
        {
            return Result.Failure(WorkOrderErrors.ObservationDescriptionTooLong(MaxObservationDescriptionLength));
        }

        if (meterReading < 0)
        {
            return Result.Failure(WorkOrderErrors.MeterReadingNegative());
        }

        ReportedAt = reportedAt;
        ResultingAssetStatus = resultingAssetStatus;
        MeterReading = meterReading;
        Priority = priority;
        RepairType = repairType;
        ResponsiblePersonnelId = responsiblePersonnelId;
        ObservationDescription = observationDescription.Trim();
        PredictedRepairLocation = predictedRepairLocation;
        PartNeedingRepair = partNeedingRepair;

        RaiseDomainEvent(new WorkOrderEdited(Id, OrganizationId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>Cancels an Open Work Order, recording the given reason.</summary>
    /// <returns>A successful <see cref="Result"/>, or a validation/conflict error.</returns>
    public Result Cancel(string reason, IDateTimeProvider dateTimeProvider)
    {
        if (Status != WorkOrderStatus.Open)
        {
            return Result.Failure(WorkOrderErrors.NotCancellableInCurrentStatus(Status));
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            return Result.Failure(WorkOrderErrors.CancellationReasonRequired());
        }

        if (reason.Trim().Length > MaxCancellationReasonLength)
        {
            return Result.Failure(WorkOrderErrors.CancellationReasonTooLong(MaxCancellationReasonLength));
        }

        Status = WorkOrderStatus.Cancelled;
        CancellationReason = reason.Trim();

        RaiseDomainEvent(new WorkOrderCancelled(Id, OrganizationId, CancellationReason, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Converts an Open Work Order to repair. No approval is required —
    /// any user permitted to edit the Work Order may perform this
    /// transition. Terminal: an Open Work Order can no longer be edited
    /// or cancelled afterward.
    /// </summary>
    /// <returns>A successful <see cref="Result"/>, or a conflict error if the Work Order is not Open.</returns>
    public Result ConvertToRepair(IDateTimeProvider dateTimeProvider)
    {
        if (Status != WorkOrderStatus.Open)
        {
            return Result.Failure(WorkOrderErrors.NotConvertibleInCurrentStatus(Status));
        }

        Status = WorkOrderStatus.InRepair;

        RaiseDomainEvent(new WorkOrderConvertedToRepair(Id, OrganizationId, AssetId, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}
