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

    /// <summary>Creates an error indicating the manufacturer was not provided.</summary>
    public static Error ManufacturerRequired() => Error.Validation(
        "EngineModel.ManufacturerRequired",
        "Engine model manufacturer is required.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "EngineModel.NotAuthorized",
        "You do not have permission to perform this action.");

    /// <summary>
    /// Creates an error indicating the given HoldingId does not
    /// correspond to an existing Holding (chat, 2026-08-26).
    /// </summary>
    public static Error HoldingNotFound(Guid holdingId) => Error.NotFound(
        "EngineModel.HoldingNotFound",
        $"Holding with id {holdingId} was not found.");
}
