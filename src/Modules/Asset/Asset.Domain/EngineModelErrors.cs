using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>Business Errors for the EngineModel aggregate.</summary>
public static class EngineModelErrors
{
    /// <summary>Creates an error indicating the name was not provided.</summary>
    public static Error NameRequired() => Error.Validation(
        "EngineModel.NameRequired",
        "Engine model name is required.");

    /// <summary>Creates an error indicating the name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "EngineModel.NameTooLong",
        $"Engine model name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "EngineModel.NotAuthorized",
        "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating the given HoldingId does not correspond to an existing Holding.</summary>
    public static Error HoldingNotFound(Guid holdingId) => Error.NotFound(
        "EngineModel.HoldingNotFound",
        $"Holding with id {holdingId} was not found.");

    /// <summary>Creates an error indicating the given CompanyId does not correspond to an existing Company in the Holding.</summary>
    public static Error CompanyNotFound(Guid companyId) => Error.NotFound(
        "EngineModel.CompanyNotFound",
        $"Company with id {companyId} was not found in this holding.");

    /// <summary>Creates an error indicating the cylinder count is not a positive number.</summary>
    public static Error InvalidCylinderCount() => Error.Validation(
        "EngineModel.InvalidCylinderCount",
        "Cylinder count must be greater than zero.");

    /// <summary>Creates an error indicating a technical specification value is not a positive number.</summary>
    public static Error InvalidSpecificationValue(string fieldName) => Error.Validation(
        "EngineModel.InvalidSpecificationValue",
        $"{fieldName} must be greater than zero.");

    /// <summary>
    /// Creates an error indicating a technical specification's value was
    /// supplied without its corresponding unit of measurement, or vice
    /// versa — both must be present together, or both absent.
    /// </summary>
    public static Error SpecificationValueUnitMismatch(string fieldName) => Error.Validation(
        "EngineModel.SpecificationValueUnitMismatch",
        $"{fieldName} requires both a value and a unit of measurement — provide both or neither.");

    /// <summary>
    /// Creates an error indicating the referenced Unit of Measurement
    /// does not exist, or does not belong to this Engine Model's Holding.
    /// </summary>
    public static Error UnitOfMeasurementNotFound(Guid unitOfMeasurementId) => Error.NotFound(
        "EngineModel.UnitOfMeasurementNotFound",
        $"Unit of measurement with id {unitOfMeasurementId} was not found in this holding.");

    /// <summary>
    /// Creates an error indicating the Unit of Measurement selected for
    /// a technical specification field belongs to the wrong physical
    /// quantity category.
    /// </summary>
    public static Error UnitOfMeasurementKindMismatch(string fieldName, global::MachineryManager.SharedKernel.PhysicalQuantityKind expectedKind) =>
        Error.Conflict(
            "EngineModel.UnitOfMeasurementKindMismatch",
            $"{fieldName} requires a unit of measurement of kind '{expectedKind}'.");
}