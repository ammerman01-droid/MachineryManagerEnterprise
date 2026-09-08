using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the UnitOfMeasurementErrors type.</summary>
public static class UnitOfMeasurementErrors
{
    /// <summary>Executes the NameRequired operation.</summary>
    public static Error NameRequired() => Error.Validation(
        "UnitOfMeasurement.NameRequired",
        "Unit of measurement name is required.");

/// <summary>Executes the NameTooLong operation.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "UnitOfMeasurement.NameTooLong",
        $"Unit of measurement name shall not exceed {maxLength} characters.");

/// <summary>Executes the InvalidKind operation.</summary>
    public static Error InvalidKind() => Error.Validation(
        "UnitOfMeasurement.InvalidKind",
        "The provided physical quantity kind is not valid.");

/// <summary>Executes the NotAuthorized operation.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "UnitOfMeasurement.NotAuthorized",
        "You do not have permission to perform this action.");
}