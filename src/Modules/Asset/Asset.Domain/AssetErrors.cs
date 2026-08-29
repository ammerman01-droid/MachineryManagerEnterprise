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
    /// Organization already uses this identification code.
    /// </summary>
    public static Error DuplicateCode(string code) => Error.Conflict(
        "Asset.DuplicateCode",
        $"An asset with code '{code}' already exists in this organization.");

    /// <summary>Creates an error indicating the name was not provided (chat, 2026-08-28).</summary>
    public static Error NameRequired() => Error.Validation(
        "Asset.NameRequired",
        "Asset name is required.");

    /// <summary>Creates an error indicating the name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Asset.NameTooLong",
        $"Asset name shall not exceed {maxLength} characters.");

    /// <summary>Creates a generic "field too long" error for the optional identity fields.</summary>
    public static Error FieldTooLong(string fieldName, int maxLength) => Error.Validation(
        $"Asset.{fieldName}TooLong",
        $"{fieldName} shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating an invalid lifecycle transition was attempted.</summary>
    public static Error InvalidTransition(AssetStatus from, AssetStatus to) => Error.Conflict(
        "Asset.InvalidTransition",
        $"Cannot transition an Asset from '{from}' to '{to}'.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "Asset.NotAuthorized",
        "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating the given OrganizationId does not correspond to an existing Organization.</summary>
    public static Error OrganizationNotFound(Guid organizationId) => Error.NotFound(
        "Asset.OrganizationNotFound",
        $"Organization with id {organizationId} was not found.");

    /// <summary>Creates an error indicating the given AssetModelId does not correspond to an existing Asset Model.</summary>
    public static Error AssetModelNotFound(Guid assetModelId) => Error.NotFound(
        "Asset.AssetModelNotFound",
        $"Asset model with id {assetModelId} was not found.");

    /// <summary>Creates an error indicating the selected Asset Model belongs to a different Holding than the target Organization.</summary>
    public static Error AssetModelHoldingMismatch() => Error.Conflict(
        "Asset.AssetModelHoldingMismatch",
        "The selected Asset Model does not belong to the same Holding as the target Organization.");

    /// <summary>Creates an error indicating the given ColorId does not correspond to an existing Color (chat, 2026-08-28).</summary>
    public static Error ColorNotFound(Guid colorId) => Error.NotFound(
        "Asset.ColorNotFound",
        $"Color with id {colorId} was not found.");

    /// <summary>Creates an error indicating the selected Color belongs to a different Organization (chat, 2026-08-28).</summary>
    public static Error ColorOrganizationMismatch() => Error.Conflict(
        "Asset.ColorOrganizationMismatch",
        "The selected Color does not belong to the target Organization.");
}