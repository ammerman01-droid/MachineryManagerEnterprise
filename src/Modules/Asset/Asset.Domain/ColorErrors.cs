using MachineryManager.SharedKernel;

namespace Asset.Domain;

/// <summary>Business Errors for the Color aggregate.</summary>
public static class ColorErrors
{
    /// <summary>Creates an error indicating the color name was not provided.</summary>
    public static Error NameRequired() => Error.Validation(
        "Color.NameRequired",
        "Color name is required.");

    /// <summary>Creates an error indicating the color name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Color.NameTooLong",
        $"Color name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "Color.NotAuthorized",
        "You do not have permission to perform this action.");
}