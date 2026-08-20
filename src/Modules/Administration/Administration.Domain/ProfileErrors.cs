using MachineryManager.SharedKernel;

namespace Administration.Domain;

/// <summary>Business Errors for the Administration module's Profile aggregate.</summary>
public static class ProfileErrors
{
    /// <summary>Creates an error indicating that the profile name is required.</summary>
    public static Error NameRequired() => Error.Validation(
        "Profile.NameRequired",
        "Profile name is required.");

    /// <summary>Creates an error indicating that the profile name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Profile.NameTooLong",
        $"Profile name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating that a permission string is required.</summary>
    public static Error PermissionRequired() => Error.Validation(
        "Profile.PermissionRequired",
        "Permission is required.");

    /// <summary>Creates an error indicating that the permission already exists on the profile.</summary>
    public static Error PermissionAlreadyExists(string permission) => Error.Validation(
        "Profile.PermissionAlreadyExists",
        $"Permission '{permission}' already exists on this profile.");

    /// <summary>Creates an error indicating that the permission was not found on the profile.</summary>
    public static Error PermissionNotFound(string permission) => Error.Validation(
        "Profile.PermissionNotFound",
        $"Permission '{permission}' was not found on this profile.");

    /// <summary>Creates an error indicating that a user identifier is required.</summary>
    public static Error UserIdRequired() => Error.Validation(
        "Profile.UserIdRequired",
        "A valid User identifier is required.");

    /// <summary>Creates an error indicating that a profile identifier is required.</summary>
    public static Error ProfileIdRequired() => Error.Validation(
        "Profile.ProfileIdRequired",
        "A valid Profile identifier is required.");

    /// <summary>Creates an error indicating that an authorization scope is required.</summary>
    public static Error ScopeRequired() => Error.Validation(
        "Profile.ScopeRequired",
        "An authorization scope is required.");
}