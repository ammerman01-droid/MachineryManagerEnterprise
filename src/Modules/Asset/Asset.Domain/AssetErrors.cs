using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>Business Errors for the Asset aggregate.</summary>
public static class AssetErrors
{
    /// <summary>Creates an error indicating the identification code was not provided.</summary>
    public static Error CodeRequired() => Error.Validation(
        "Asset.CodeRequired",
        "Asset identification code is required.");

    /// <summary>Creates an error indicating the identification code exceeds the maximum length.</summary>
    public static Error CodeTooLong(int maxLength) => Error.Validation(
        "Asset.CodeTooLong",
        $"Asset identification code shall not exceed {maxLength} characters.");

    /// <summary>
    /// Creates an error indicating another Asset within the same
    /// Organization already uses this identification code
    /// (chat, 2026-08-28 — Code is unique per Organization).
    /// </summary>
    public static Error DuplicateCode(string code) => Error.Conflict(
        "Asset.DuplicateCode",
        $"An asset with code '{code}' already exists in this organization.");

    /// <summary>Creates an error indicating the color was not provided.</summary>
    public static Error ColorRequired() => Error.Validation(
        "Asset.ColorRequired",
        "Asset color is required.");

    /// <summary>Creates an error indicating an invalid lifecycle transition was attempted.</summary>
    public static Error InvalidTransition(AssetStatus from, AssetStatus to) => Error.Conflict(
        "Asset.InvalidTransition",
        $"Cannot transition an Asset from '{from}' to '{to}'.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "Asset.NotAuthorized",
        "You do not have permission to perform this action.");

    /// <summary>
    /// Creates an error indicating the given OrganizationId does not
    /// correspond to an existing Organization (chat, 2026-08-27).
    /// </summary>
    public static Error OrganizationNotFound(Guid organizationId) => Error.NotFound(
        "Asset.OrganizationNotFound",
        $"Organization with id {organizationId} was not found.");

    /// <summary>
    /// Creates an error indicating the given AssetModelId does not
    /// correspond to an existing Asset Model (chat, 2026-08-27).
    /// </summary>
    public static Error AssetModelNotFound(Guid assetModelId) => Error.NotFound(
        "Asset.AssetModelNotFound",
        $"Asset model with id {assetModelId} was not found.");

    /// <summary>
    /// Creates an error indicating the selected Asset Model belongs to
    /// a different Holding than the target Organization (chat, 2026-08-27).
    /// </summary>
    public static Error AssetModelHoldingMismatch() => Error.Conflict(
        "Asset.AssetModelHoldingMismatch",
        "The selected Asset Model does not belong to the same Holding as the target Organization.");
}