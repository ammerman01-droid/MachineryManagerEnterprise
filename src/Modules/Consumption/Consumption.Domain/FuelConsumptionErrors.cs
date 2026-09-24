using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Consumption.Domain;

/// <summary>Business Errors for the FuelConsumption aggregate.</summary>
public static class FuelConsumptionErrors
{
    /// <summary>Creates an error indicating no Asset was specified.</summary>
    public static Error AssetRequired() => Error.Validation(
        "FuelConsumption.AssetRequired", "An Asset must be specified.");

    /// <summary>Creates an error indicating no Organization was specified.</summary>
    public static Error OrganizationRequired() => Error.Validation(
        "FuelConsumption.OrganizationRequired", "An Organization must be specified.");

    /// <summary>Creates an error indicating no Project was specified.</summary>
    public static Error ProjectRequired() => Error.Validation(
        "FuelConsumption.ProjectRequired", "A Project must be specified.");

    /// <summary>Creates an error indicating the fuel quantity was not positive.</summary>
    public static Error QuantityMustBePositive() => Error.Validation(
        "FuelConsumption.QuantityMustBePositive", "Fuel quantity must be greater than zero.");

    /// <summary>Creates an error indicating the snapshotted unit price was negative.</summary>
    public static Error UnitPriceMustNotBeNegative() => Error.Validation(
        "FuelConsumption.UnitPriceMustNotBeNegative", "Fuel unit price shall not be negative.");

    /// <summary>Creates an error indicating the meter reading was negative.</summary>
    public static Error MeterReadingMustNotBeNegative() => Error.Validation(
        "FuelConsumption.MeterReadingMustNotBeNegative", "Meter reading shall not be negative.");

    /// <summary>Creates an error indicating the fuel deliverer was not specified.</summary>
    public static Error DeliveredByPersonnelRequired() => Error.Validation(
        "FuelConsumption.DeliveredByPersonnelRequired", "The Personnel who delivered the fuel must be specified.");

    /// <summary>Creates an error indicating the fuel receiver was not specified.</summary>
    public static Error ReceivedByPersonnelRequired() => Error.Validation(
        "FuelConsumption.ReceivedByPersonnelRequired", "The Personnel who received the fuel must be specified.");

    /// <summary>Creates an error indicating the notes field exceeds the maximum length.</summary>
    public static Error NotesTooLong(int maxLength) => Error.Validation(
        "FuelConsumption.NotesTooLong", $"Notes shall not exceed {maxLength} characters.");

    /// <summary>Creates a Not Found error for a missing record.</summary>
    public static Error NotFound(Guid id) => Error.NotFound(
        "FuelConsumption.NotFound", $"Fuel consumption record with id {id} was not found.");

    /// <summary>Creates a Not Found error for a missing referenced Asset.</summary>
    public static Error AssetNotFound(Guid assetId) => Error.NotFound(
        "FuelConsumption.AssetNotFound", $"Asset with id {assetId} was not found.");

    /// <summary>
    /// Creates an error indicating the requested fuel slot (Primary or
    /// Secondary) is not configured on the Asset — e.g. Secondary was
    /// requested but the Asset has no SecondaryFuelKind.
    /// </summary>
    public static Error FuelSlotNotConfiguredOnAsset(FuelSlot fuelSlot) => Error.Validation(
        "FuelConsumption.FuelSlotNotConfiguredOnAsset",
        $"The Asset does not have a {fuelSlot} fuel configured.");

    /// <summary>
    /// Creates an error indicating the Asset has no MeterReadingUnit
    /// configured, so a meter reading cannot be recorded against it
    /// (chat, 2026-09-20).
    /// </summary>
    public static Error MeterUnitNotConfiguredOnAsset() => Error.Validation(
        "FuelConsumption.MeterUnitNotConfiguredOnAsset",
        "The Asset does not have a meter reading unit configured.");

    /// <summary>Creates an error indicating no FuelType was specified.</summary>
    public static Error FuelTypeRequired() => Error.Validation(
        "FuelConsumption.FuelTypeRequired", "A Fuel Type must be specified.");

    /// <summary>Creates a Not Found error for a missing referenced FuelType.</summary>
    public static Error FuelTypeNotFound(Guid fuelTypeId) => Error.NotFound(
        "FuelConsumption.FuelTypeNotFound", $"Fuel type with id {fuelTypeId} was not found.");

    /// <summary>Creates an error indicating the selected FuelType does not belong to the Asset's Holding.</summary>
    public static Error FuelTypeHoldingMismatch() => Error.Conflict(
        "FuelConsumption.FuelTypeHoldingMismatch",
        "The selected Fuel Type does not belong to the Asset's Holding.");

    /// <summary>Creates an error indicating the selected FuelType's Kind does not match the Asset's configured fuel kind for this slot.</summary>
    public static Error FuelTypeKindMismatch(FuelKind expectedKind, FuelKind actualKind) => Error.Validation(
        "FuelConsumption.FuelTypeKindMismatch",
        $"The selected Fuel Type is '{actualKind}', but the Asset requires '{expectedKind}' for this fuel slot.");

    /// <summary>Creates a Not Found error for a missing referenced fuel-deliverer Personnel.</summary>
    public static Error DeliveredByPersonnelNotFound(Guid personnelId) => Error.NotFound(
        "FuelConsumption.DeliveredByPersonnelNotFound", $"Personnel with id {personnelId} was not found.");

    /// <summary>Creates a Not Found error for a missing referenced fuel-receiver Personnel.</summary>
    public static Error ReceivedByPersonnelNotFound(Guid personnelId) => Error.NotFound(
        "FuelConsumption.ReceivedByPersonnelNotFound", $"Personnel with id {personnelId} was not found.");

    /// <summary>Creates an error indicating a referenced Personnel does not belong to the Asset's Organization.</summary>
    public static Error PersonnelOrganizationMismatch() => Error.Conflict(
        "FuelConsumption.PersonnelOrganizationMismatch",
        "The delivering/receiving Personnel does not belong to the Asset's Organization.");

    /// <summary>Creates an error indicating the meter reading is below the Asset's previously recorded reading.</summary>
    public static Error MeterReadingBelowPrevious(decimal previousReading) => Error.Conflict(
        "FuelConsumption.MeterReadingBelowPrevious",
        $"Meter reading must be greater than or equal to the previous recorded reading ({previousReading}).");

    /// <summary>Creates an error indicating the meter reading is above the Asset's next recorded reading.</summary>
    public static Error MeterReadingAboveNext(decimal nextReading) => Error.Conflict(
        "FuelConsumption.MeterReadingAboveNext",
        $"Meter reading must be less than or equal to the next recorded reading ({nextReading}).");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "FuelConsumption.NotAuthorized", "You do not have permission to perform this action.");
}
