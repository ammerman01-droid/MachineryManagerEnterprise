using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>
/// Business Errors for the <see cref="DrivingLicenseType"/> aggregate.
/// </summary>
public static class DrivingLicenseTypeErrors
{
    /// <summary>Creates an error indicating the type's name was not provided.</summary>
    public static Error NameRequired() => Error.Validation(
        "DrivingLicenseType.NameRequired",
        "Driving license type name is required.");

    /// <summary>Creates an error indicating the type's name exceeds the maximum allowed length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "DrivingLicenseType.NameTooLong",
        $"Driving license type name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "DrivingLicenseType.NotAuthorized",
        "You do not have permission to perform this action.");

/// <summary>Creates an error indicating this Driving License Type is referenced by at least one Personnel record and cannot be deleted.</summary>
    public static Error InUse() => Error.Conflict(
        "DrivingLicenseType.InUse",
        "This driving license type is used by at least one Personnel record and cannot be deleted.");

}