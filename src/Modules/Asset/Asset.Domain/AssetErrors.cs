using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>Business Errors for the Asset aggregate.</summary>
public static class AssetErrors
{
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
}