using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>Business Errors for the AssetModel aggregate.</summary>
public static class AssetModelErrors
{
    /// <summary>Creates an error indicating the name was not provided.</summary>
    public static Error NameRequired() => Error.Validation(
        "AssetModel.NameRequired",
        "Asset model name is required.");

    /// <summary>Creates an error indicating the name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "AssetModel.NameTooLong",
        $"Asset model name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the manufacturer was not provided.</summary>
    public static Error ManufacturerRequired() => Error.Validation(
        "AssetModel.ManufacturerRequired",
        "Asset model manufacturer is required.");

    /// <summary>Creates an error indicating the engine model is already marked compatible.</summary>
    public static Error EngineModelAlreadyCompatible() => Error.Conflict(
        "AssetModel.EngineModelAlreadyCompatible",
        "This engine model is already marked compatible with this asset model.");

    /// <summary>Creates an error indicating the engine model is not currently marked compatible.</summary>
    public static Error EngineModelNotCompatible() => Error.Conflict(
        "AssetModel.EngineModelNotCompatible",
        "This engine model is not currently marked compatible with this asset model.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "AssetModel.NotAuthorized",
        "You do not have permission to perform this action.");

    /// <summary>
    /// Creates an error indicating the given HoldingId does not
    /// correspond to an existing Holding (chat, 2026-08-26).
    /// </summary>
    public static Error HoldingNotFound(Guid holdingId) => Error.NotFound(
        "AssetModel.HoldingNotFound",
        $"Holding with id {holdingId} was not found.");

    /// <summary>
    /// Creates an error indicating that the referenced Engine Model does
    /// not exist (chat, 2026-08-26).
    /// </summary>
    public static Error EngineModelNotFound(Guid engineModelId) => Error.NotFound(
        "AssetModel.EngineModelNotFound",
        $"Engine model with id {engineModelId} was not found.");

    /// <summary>
    /// Creates an error indicating that the referenced Engine Model
    /// belongs to a different Holding than this Asset Model — catalog
    /// data is scoped Per-Holding and cannot cross that boundary
    /// (chat, 2026-08-26).
    /// </summary>
    public static Error EngineModelBelongsToDifferentHolding() => Error.Conflict(
        "AssetModel.EngineModelBelongsToDifferentHolding",
        "This engine model belongs to a different Holding and cannot be marked compatible with this asset model.");

    /// <summary>
    /// Creates an error indicating that the given CompanyId does not
    /// correspond to an existing Company within this Asset Model's
    /// Holding (chat, 2026-09-01).
    /// </summary>
    public static Error CompanyNotFound(Guid companyId) => Error.NotFound(
        "AssetModel.CompanyNotFound",
        $"Company with id {companyId} was not found.");

    /// <summary>
    /// Creates an error indicating a dimension/technical specification
    /// value is not a positive number (chat, 2026-09-04).
    /// </summary>
    public static Error InvalidSpecificationValue(string fieldName) => Error.Validation(
        "AssetModel.InvalidSpecificationValue",
        $"{fieldName} must be greater than zero.");

    /// <summary>
    /// Creates an error indicating a technical specification's value was
    /// supplied without its corresponding unit of measurement, or vice
    /// versa — both must be present together, or both absent
    /// (chat, 2026-09-04).
    /// </summary>
    public static Error SpecificationValueUnitMismatch(string fieldName) => Error.Validation(
        "AssetModel.SpecificationValueUnitMismatch",
        $"{fieldName} requires both a value and a unit of measurement — provide both or neither.");

    /// <summary>
    /// Creates an error indicating the referenced Unit of Measurement
    /// does not exist, or does not belong to this Asset Model's
    /// Holding (chat, 2026-09-04).
    /// </summary>
    public static Error UnitOfMeasurementNotFound(Guid unitOfMeasurementId) => Error.NotFound(
        "AssetModel.UnitOfMeasurementNotFound",
        $"Unit of measurement with id {unitOfMeasurementId} was not found in this holding.");

    /// <summary>
    /// Creates an error indicating the Unit of Measurement selected for
    /// a technical specification field belongs to the wrong physical
    /// quantity category (chat, 2026-09-04).
    /// </summary>
    public static Error UnitOfMeasurementKindMismatch(string fieldName, global::MachineryManager.SharedKernel.PhysicalQuantityKind expectedKind) =>
        Error.Conflict(
            "AssetModel.UnitOfMeasurementKindMismatch",
            $"{fieldName} requires a unit of measurement of kind '{expectedKind}'.");
}