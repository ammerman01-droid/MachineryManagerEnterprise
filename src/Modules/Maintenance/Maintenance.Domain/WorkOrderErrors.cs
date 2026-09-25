using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>Business Errors for the WorkOrder aggregate.</summary>
public static class WorkOrderErrors
{
    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "WorkOrder.NotAuthorized",
        "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating the given OrganizationId does not correspond to an existing Organization.</summary>
    public static Error OrganizationNotFound(Guid organizationId) => Error.NotFound(
        "WorkOrder.OrganizationNotFound",
        $"Organization with id {organizationId} was not found.");

    /// <summary>Creates an error indicating the given AssetId does not correspond to an existing Asset.</summary>
    public static Error AssetNotFound(Guid assetId) => Error.NotFound(
        "WorkOrder.AssetNotFound",
        $"Asset with id {assetId} was not found.");

    /// <summary>Creates an error indicating the Asset belongs to a different Organization than requested.</summary>
    public static Error AssetOrganizationMismatch() => Error.Conflict(
        "WorkOrder.AssetOrganizationMismatch",
        "The selected Asset does not belong to the target Organization.");

    /// <summary>Creates an error indicating the given ResponsiblePersonnelId does not correspond to an existing Personnel record.</summary>
    public static Error ResponsiblePersonnelNotFound(Guid personnelId) => Error.NotFound(
        "WorkOrder.ResponsiblePersonnelNotFound",
        $"Personnel with id {personnelId} was not found.");

    /// <summary>Creates an error indicating the responsible Personnel belongs to a different Organization than requested.</summary>
    public static Error ResponsiblePersonnelOrganizationMismatch() => Error.Conflict(
        "WorkOrder.ResponsiblePersonnelOrganizationMismatch",
        "The selected responsible Personnel does not belong to the target Organization.");

    /// <summary>Creates an error indicating no responsible Personnel was provided.</summary>
    public static Error ResponsiblePersonnelRequired() => Error.Validation(
        "WorkOrder.ResponsiblePersonnelRequired",
        "A repair-responsible Personnel is required.");

    /// <summary>Creates an error indicating no observation description was provided.</summary>
    public static Error ObservationDescriptionRequired() => Error.Validation(
        "WorkOrder.ObservationDescriptionRequired",
        "A description of the observations is required.");

    /// <summary>Creates an error indicating the observation description exceeds the maximum length.</summary>
    public static Error ObservationDescriptionTooLong(int maxLength) => Error.Validation(
        "WorkOrder.ObservationDescriptionTooLong",
        $"The observation description shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the meter reading is negative.</summary>
    public static Error MeterReadingNegative() => Error.Validation(
        "WorkOrder.MeterReadingNegative",
        "The meter reading cannot be negative.");

    /// <summary>Creates an error indicating the generated Work Order number is invalid.</summary>
    public static Error InvalidNumber() => Error.Failure(
        "WorkOrder.InvalidNumber",
        "The generated Work Order number is invalid.");

    /// <summary>Creates an error indicating the Work Order cannot be edited in its current status.</summary>
    public static Error NotEditableInCurrentStatus(WorkOrderStatus status) => Error.Conflict(
        "WorkOrder.NotEditableInCurrentStatus",
        $"A Work Order in status '{status}' cannot be edited. Only an Open Work Order can be edited.");

    /// <summary>Creates an error indicating the Work Order cannot be cancelled in its current status.</summary>
    public static Error NotCancellableInCurrentStatus(WorkOrderStatus status) => Error.Conflict(
        "WorkOrder.NotCancellableInCurrentStatus",
        $"A Work Order in status '{status}' cannot be cancelled. Only an Open Work Order can be cancelled.");

    /// <summary>Creates an error indicating the Work Order cannot be converted to repair in its current status.</summary>
    public static Error NotConvertibleInCurrentStatus(WorkOrderStatus status) => Error.Conflict(
        "WorkOrder.NotConvertibleInCurrentStatus",
        $"A Work Order in status '{status}' cannot be converted to repair. Only an Open Work Order can be converted.");

    /// <summary>Creates an error indicating no cancellation reason was provided.</summary>
    public static Error CancellationReasonRequired() => Error.Validation(
        "WorkOrder.CancellationReasonRequired",
        "A reason is required to cancel a Work Order.");

    /// <summary>Creates an error indicating the cancellation reason exceeds the maximum length.</summary>
    public static Error CancellationReasonTooLong(int maxLength) => Error.Validation(
        "WorkOrder.CancellationReasonTooLong",
        $"The cancellation reason shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the given value is not a valid Work Order asset-status impact.</summary>
    public static Error InvalidResultingAssetStatus(string value) => Error.Validation(
        "WorkOrder.InvalidResultingAssetStatus",
        $"'{value}' is not a valid resulting Asset status. Valid values: Active, OutOfService.");

    /// <summary>Creates an error indicating the given Work Order was not found.</summary>
    public static Error NotFound(Guid workOrderId) => Error.NotFound(
        "WorkOrder.NotFound",
        $"Work Order with id {workOrderId} was not found.");
}
