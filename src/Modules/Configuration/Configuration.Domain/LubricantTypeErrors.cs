using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>Business Errors for the <see cref="LubricantType"/> aggregate.</summary>
public static class LubricantTypeErrors
{
    /// <summary>Creates an error indicating the lubricant type's name was not provided.</summary>
    public static Error NameRequired() => Error.Validation("LubricantType.NameRequired", "Lubricant type name is required.");

    /// <summary>Creates an error indicating the lubricant type's name exceeds the maximum allowed length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation("LubricantType.NameTooLong", $"Lubricant type name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure("LubricantType.NotAuthorized", "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating this Lubricant Type is referenced by at least one Lubricant Overflow Report and cannot be deleted.</summary>
    public static Error InUse() => Error.Conflict(
        "LubricantType.InUse",
        "This lubricant type is used by at least one Lubricant Overflow Report and cannot be deleted.");
}
