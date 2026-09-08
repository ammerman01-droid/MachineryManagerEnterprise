using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>
/// Business Errors for the <see cref="Color"/> aggregate.
/// </summary>
public static class ColorErrors
{
    /// <summary>
    /// Creates an error indicating the color's name was not provided.
    /// </summary>
    /// <returns>A validation <see cref="Error"/>.</returns>
    public static Error NameRequired() => Error.Validation(
        "Color.NameRequired",
        "Color name is required.");

    /// <summary>
    /// Creates an error indicating the color's name exceeds the maximum allowed length.
    /// </summary>
    /// <param name="maxLength">The maximum number of characters allowed.</param>
    /// <returns>A validation <see cref="Error"/>.</returns>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Color.NameTooLong",
        $"Color name shall not exceed {maxLength} characters.");

    /// <summary>
    /// Creates an error indicating the current user lacks permission for this action.
    /// </summary>
    /// <returns>A failure-type <see cref="Error"/>.</returns>
    public static Error NotAuthorized() => Error.Failure(
        "Color.NotAuthorized",
        "You do not have permission to perform this action.");
}
