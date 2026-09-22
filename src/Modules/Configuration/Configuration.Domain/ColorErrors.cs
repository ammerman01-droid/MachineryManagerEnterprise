using MachineryManagerEnterprise.SharedKernel;

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

    /// <summary>
    /// Creates an error indicating the requested Color does not exist.
    /// </summary>
    /// <param name="colorId">The requested Color identifier.</param>
    /// <returns>A not-found <see cref="Error"/>.</returns>
    public static Error NotFound(Guid colorId) => Error.NotFound(
        "Color.NotFound",
        $"Color with id {colorId} was not found.");

    /// <summary>
    /// Creates an error indicating the Color is already deactivated.
    /// </summary>
    /// <returns>A conflict <see cref="Error"/>.</returns>
    public static Error AlreadyInactive() => Error.Conflict(
        "Color.AlreadyInactive",
        "This color is already deactivated.");

    /// <summary>
    /// Creates an error indicating the Color is already active.
    /// </summary>
    /// <returns>A conflict <see cref="Error"/>.</returns>
    public static Error AlreadyActive() => Error.Conflict(
        "Color.AlreadyActive",
        "This color is already active.");
}
