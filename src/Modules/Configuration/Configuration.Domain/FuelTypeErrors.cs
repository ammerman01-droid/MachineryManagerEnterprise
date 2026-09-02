using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>Business Errors for the <see cref="FuelType"/> aggregate.</summary>
public static class FuelTypeErrors
{
    /// <summary>Creates an error indicating the fuel type's name was not provided.</summary>
    /// <returns>A validation <see cref="Error"/>.</returns>
    public static Error NameRequired() => Error.Validation(
        "FuelType.NameRequired", "Fuel type name is required.");

    /// <summary>Creates an error indicating the fuel type's name exceeds the maximum allowed length.</summary>
    /// <param name="maxLength">The maximum number of characters allowed.</param>
    /// <returns>A validation <see cref="Error"/>.</returns>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "FuelType.NameTooLong", $"Fuel type name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the price was not a positive value.</summary>
    /// <returns>A validation <see cref="Error"/>.</returns>
    public static Error PriceMustBePositive() => Error.Validation(
        "FuelType.PriceMustBePositive", "Fuel type price must be greater than zero.");

    /// <summary>Creates an error indicating the supplied <see cref="FuelKind"/> value is not one of the defined enum members.</summary>
    /// <returns>A validation <see cref="Error"/>.</returns>
    public static Error InvalidKind() => Error.Validation(
        "FuelType.InvalidKind", "The supplied fuel kind is not valid.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    /// <returns>A failure-type <see cref="Error"/>.</returns>
    public static Error NotAuthorized() => Error.Failure(
        "FuelType.NotAuthorized", "You do not have permission to perform this action.");
}