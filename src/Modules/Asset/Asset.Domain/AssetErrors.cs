using MachineryManagerEnterprise.SharedKernel;

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

    /// <summary>Creates an error indicating the given value is not a valid Asset status (chat, 2026-09-20).</summary>
    public static Error InvalidStatus(string value) => Error.Validation(
        "Asset.InvalidStatus",
        $"'{value}' is not a valid Asset status. Valid values: Active, Ready, OutOfService, OutOfFleet.");

    /// <summary>Creates an error indicating the Asset already has the requested status (chat, 2026-09-20).</summary>
    public static Error AlreadyInStatus(AssetStatus status) => Error.Conflict(
        "Asset.AlreadyInStatus",
        $"The Asset is already in status '{status}'.");

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

    /// <summary>
    /// Creates an error indicating the selected Color does not exist,
    /// or does not belong to the target Organization's Holding.
    /// </summary>
    /// <param name="colorId">The Color identifier that failed the check.</param>
    /// <returns>A <see cref="Error"/> of type NotFound.</returns>
    public static Error ColorNotFoundInHolding(Guid colorId) => Error.NotFound(
        "Asset.ColorNotFoundInHolding",
        $"Color with id {colorId} was not found in the target Organization's Holding.");

    /// <summary>Creates an error indicating the given ProjectId does not correspond to an existing Project (chat, 2026-09-14).</summary>
    public static Error ProjectNotFound(Guid projectId) => Error.NotFound(
        "Asset.ProjectNotFound",
        $"Project with id {projectId} was not found.");

    /// <summary>Creates an error indicating the selected Project belongs to a different Organization (chat, 2026-09-14).</summary>
    public static Error ProjectOrganizationMismatch() => Error.Conflict(
        "Asset.ProjectOrganizationMismatch",
        "The selected Project does not belong to the target Organization.");

    /// <summary>
    /// Creates an error indicating a fuel Kind was specified without its
    /// matching fuel unit, or vice versa (chat, 2026-09-14).
    /// </summary>
    public static Error FuelSpecificationMismatch(string fieldName) => Error.Validation(
        "Asset.FuelSpecificationMismatch",
        $"{fieldName} requires both a fuel kind and a fuel unit — provide both or neither.");

    /// <summary>Creates an error indicating a secondary fuel was specified without a primary one (chat, 2026-09-14).</summary>
    public static Error SecondaryFuelRequiresPrimary() => Error.Validation(
        "Asset.SecondaryFuelRequiresPrimary",
        "A secondary fuel kind cannot be set without a primary fuel kind.");

    /// <summary>Creates an error indicating the primary and secondary fuel kinds are the same (chat, 2026-09-14).</summary>
    public static Error DuplicateFuelKind() => Error.Validation(
        "Asset.DuplicateFuelKind",
        "Primary and secondary fuel kinds must be different.");

    /// <summary>Gets an error indicating that a project is required for the asset.</summary>
    public static Error ProjectRequired() => Error.Validation(
    "Asset.ProjectRequired",
    "Project assignment is required for every Asset.");

/// <summary>Creates an error indicating no operational status was provided (chat, 2026-09-18).</summary>
public static Error OperationalStatusRequired() => Error.Validation(
    "Asset.OperationalStatusRequired",
    "Operational status is required for every Asset.");

/// <summary>
/// Creates an error indicating the selected operational status does
/// not exist, or does not belong to the target Organization's Holding
/// (chat, 2026-09-18).
/// </summary>
public static Error OperationalStatusNotFoundInHolding(Guid operationalStatusId) => Error.NotFound(
    "Asset.OperationalStatusNotFoundInHolding",
    $"Operational status with id {operationalStatusId} was not found in the target Organization's Holding.");
}
