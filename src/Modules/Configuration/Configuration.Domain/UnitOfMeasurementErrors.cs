using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the UnitOfMeasurementErrors type.</summary>
public static class UnitOfMeasurementErrors
{
/// <summary>Executes the NameRequired operation.</summary>
    public static Error NameRequired() => Error.Validation(
        "UnitOfMeasurement.NameRequired", "Unit of measurement name is required.");

/// <summary>Executes the NameTooLong operation.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "UnitOfMeasurement.NameTooLong", $"Unit of measurement name shall not exceed {maxLength} characters.");

/// <summary>Executes the CategoryNotFound operation.</summary>
    public static Error CategoryNotFound(Guid categoryId) => Error.NotFound(
        "UnitOfMeasurement.CategoryNotFound", $"Unit category with id {categoryId} was not found.");

/// <summary>Executes the CategoryHoldingMismatch operation.</summary>
    public static Error CategoryHoldingMismatch() => Error.Conflict(
        "UnitOfMeasurement.CategoryHoldingMismatch",
        "The selected category does not belong to the same Holding.");

/// <summary>Executes the NotAuthorized operation.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "UnitOfMeasurement.NotAuthorized", "You do not have permission to perform this action.");
}