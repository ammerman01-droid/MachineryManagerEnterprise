using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>Business Errors for the AssetOperationalStatus aggregate.</summary>
public static class AssetOperationalStatusErrors
{
    /// <summary>Creates an error indicating the name was not provided.</summary>
    public static Error NameRequired() => Error.Validation(
        "AssetOperationalStatus.NameRequired",
        "Operational status name is required.");

    /// <summary>Creates an error indicating the name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "AssetOperationalStatus.NameTooLong",
        $"Operational status name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "AssetOperationalStatus.NotAuthorized",
        "You do not have permission to perform this action.");
}