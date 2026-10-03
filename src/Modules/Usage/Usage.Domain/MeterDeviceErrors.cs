using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain;

/// <summary>Business Errors for the <see cref="MeterDevice"/> aggregate.</summary>
public static class MeterDeviceErrors
{
    /// <summary>Creates an error indicating a reading's numeric value was negative (BR-018/BR-046).</summary>
    public static Error NegativeReadingValue() => Error.Validation(
        "MeterDevice.NegativeReadingValue",
        "A meter reading value shall never be negative.");

    /// <summary>Creates an error indicating an invalid (undefined) meter reading unit was supplied.</summary>
    public static Error InvalidUnit() => Error.Validation(
        "MeterDevice.InvalidUnit",
        "The meter reading unit is not a recognized value.");

    /// <summary>
    /// Creates an error indicating this device's fixed reading unit
    /// (chat, 2026-09-29 — set once at <see cref="MeterDevice.Register"/>
    /// and never changed afterward) does not match the unit configured
    /// on the owner it is being installed on — a device measuring
    /// hours cannot be installed on an owner configured for kilometers
    /// (BR-011/BR-013).
    /// </summary>
    public static Error UnitMismatch(global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit establishedUnit, global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit attemptedUnit) => Error.Conflict(
        "MeterDevice.UnitMismatch",
        $"This device was registered with unit '{establishedUnit}' and cannot be installed on an owner configured for '{attemptedUnit}'.");

    /// <summary>Creates an error indicating no Organization reference was supplied.</summary>
    public static Error OrganizationRequired() => Error.Validation(
        "MeterDevice.OrganizationRequired",
        "An Organization is required to register a Meter Device.");

    /// <summary>Creates an error indicating the supplied daily cap override was not a positive number (BR-045).</summary>
    public static Error InvalidDailyCapOverride() => Error.Validation(
        "MeterDevice.InvalidDailyCapOverride",
        "The daily cap override, when provided, shall be a positive number.");

    /// <summary>Creates an error indicating an invalid lifecycle transition was attempted (Section 4.9).</summary>
    public static Error InvalidTransition(MeterDeviceStatus from, MeterDeviceStatus to) => Error.Conflict(
        "MeterDevice.InvalidTransition",
        $"Cannot transition a Meter Device from '{from}' to '{to}'.");

    /// <summary>Creates an error indicating no owner (Asset or Component) was supplied for installation.</summary>
    public static Error OwnerRequired() => Error.Validation(
        "MeterDevice.OwnerRequired",
        "An owner (Asset or Component) is required to install a Meter Device.");

    /// <summary>Creates an error indicating the device is not in a state that accepts readings (must be Installed or Operational).</summary>
    public static Error NotReadyForReading(MeterDeviceStatus currentStatus) => Error.Conflict(
        "MeterDevice.NotReadyForReading",
        $"A Meter Device in status '{currentStatus}' cannot accept a reading.");

    /// <summary>Creates an error indicating the current user is not authorized to perform the requested action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "MeterDevice.NotAuthorized",
        "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating the referenced Asset does not exist.</summary>
    public static Error AssetNotFound(Guid assetId) => Error.NotFound(
        "MeterDevice.AssetNotFound",
        $"Asset {assetId} was not found.");

    /// <summary>Creates an error indicating the owner belongs to a different Organization than the device.</summary>
    public static Error OwnerOrganizationMismatch() => Error.Conflict(
        "MeterDevice.OwnerOrganizationMismatch",
        "The owner does not belong to the same Organization as this Meter Device.");

    /// <summary>
    /// Creates an error indicating a Component-owned Meter Device was
    /// requested, but the Component module does not yet exist (chat,
    /// 2026-09-12) — see BR-004 in the reference document for the
    /// intended design once it does.
    /// </summary>
    public static Error ComponentOwnerNotYetSupported() => Error.Failure(
        "MeterDevice.ComponentOwnerNotYetSupported",
        "Installing a Meter Device on a Tracked Component is not yet supported — the Component module has not been implemented.");

    /// <summary>Creates an error indicating the owner Asset has no meter reading unit configured, so a device cannot be matched to it.</summary>
public static Error AssetHasNoMeterReadingUnit(Guid assetId) => Error.Conflict(
    "MeterDevice.AssetHasNoMeterReadingUnit",
    $"Asset {assetId} has no meter reading unit configured, so a Meter Device cannot be installed on it.");
}
