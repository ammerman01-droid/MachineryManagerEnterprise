using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>Business Errors for the <see cref="OverflowComponent"/> aggregate.</summary>
public static class OverflowComponentErrors
{
    /// <summary>Creates an error indicating the overflow component's name was not provided.</summary>
    public static Error NameRequired() => Error.Validation("OverflowComponent.NameRequired", "Overflow component name is required.");

    /// <summary>Creates an error indicating the overflow component's name exceeds the maximum allowed length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation("OverflowComponent.NameTooLong", $"Overflow component name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure("OverflowComponent.NotAuthorized", "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating this Overflow Component is referenced by at least one Lubricant Overflow Report and cannot be deleted.</summary>
    public static Error InUse() => Error.Conflict(
        "OverflowComponent.InUse",
        "This overflow component is used by at least one Lubricant Overflow Report and cannot be deleted.");
}
