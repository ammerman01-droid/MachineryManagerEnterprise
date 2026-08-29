using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>
/// Business Errors for the UnitOfMeasurement aggregate.
/// </summary>
public static class UnitOfMeasurementErrors
{
    /// <summary>
    /// Creates an error indicating that the unit of measurement name was not provided.
    /// </summary>
    public static Error NameRequired() => Error.Validation(
        "UnitOfMeasurement.NameRequired",
        "Unit of measurement name is required.");

    /// <summary>
    /// Creates an error indicating that the unit of measurement name exceeds the maximum length.
    /// </summary>
    /// <param name="maxLength">The maximum allowed length.</param>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "UnitOfMeasurement.NameTooLong",
        $"Unit of measurement name shall not exceed {maxLength} characters.");

    /// <summary>
    /// Creates an error indicating that the unit of measurement category was not provided.
    /// </summary>
    public static Error CategoryRequired() => Error.Validation(
        "UnitOfMeasurement.CategoryRequired",
        "Unit of measurement category is required.");

    /// <summary>
    /// Creates an error indicating that the unit of measurement category exceeds the maximum length.
    /// </summary>
    /// <param name="maxLength">The maximum allowed length.</param>
    public static Error CategoryTooLong(int maxLength) => Error.Validation(
        "UnitOfMeasurement.CategoryTooLong",
        $"Unit of measurement category shall not exceed {maxLength} characters.");

    /// <summary>
    /// Creates an error indicating that the current user is not authorized to perform the operation.
    /// </summary>
    public static Error NotAuthorized() => Error.Failure(
        "UnitOfMeasurement.NotAuthorized",
        "You do not have permission to perform this action.");
}