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
}