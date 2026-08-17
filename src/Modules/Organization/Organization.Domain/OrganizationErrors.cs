using MachineryManager.SharedKernel;

namespace Organization.Domain;

/// <summary>Business Errors for the Organization module (05-development/07-ErrorHandling.md).</summary>
public static class OrganizationErrors
{
    /// <summary>Creates an error indicating that the organization name is required.</summary>
    /// <returns>An <see cref="Error"/> representing the validation failure.</returns>
    public static Error NameRequired() => Error.Validation(
        "Organization.NameRequired",
        "Organization name is required.");

    /// <summary>Creates an error indicating that the organization name exceeds the maximum length.</summary>
    /// <param name="maxLength">The maximum allowed length for the organization name.</param>
    /// <returns>An <see cref="Error"/> representing the validation failure.</returns>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Organization.NameTooLong",
        $"Organization name shall not exceed {maxLength} characters.");
}