using MachineryManager.SharedKernel;

namespace Organization.Domain;

/// <summary>Business Errors for the Holding aggregate.</summary>
public static class HoldingErrors
{
    /// <summary>Creates an error indicating that the holding name is required.</summary>
    public static Error NameRequired() => Error.Validation(
        "Holding.NameRequired",
        "Holding name is required.");

    /// <summary>Creates an error indicating that the holding name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Holding.NameTooLong",
        $"Holding name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "Holding.NotAuthorized",
        "You do not have permission to perform this action.");
}