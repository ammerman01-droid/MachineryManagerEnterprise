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
}