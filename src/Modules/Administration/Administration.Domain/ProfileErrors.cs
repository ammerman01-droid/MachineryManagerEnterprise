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

    /// <summary>Creates an error indicating that the assignment was not found.</summary>
    public static Error AssignmentNotFound(Guid assignmentId) => Error.NotFound(
        "Profile.AssignmentNotFound",
        $"UserProfileAssignment with id {assignmentId} was not found.");

    /// <summary>Creates an error indicating that the assignment is already revoked.</summary>
    public static Error AssignmentAlreadyRevoked() => Error.Conflict(
        "Profile.AssignmentAlreadyRevoked",
        "This assignment has already been revoked.");

    /// <summary>
    /// Creates an error indicating that the profile was not found (used
    /// by DeleteProfile, distinct code from AssignmentNotFound above so
    /// callers can distinguish "no such Profile" from "no such
    /// assignment").
    /// </summary>
    public static Error ProfileNotFound(Guid profileId) => Error.NotFound(
        "Profile.NotFound",
        $"Profile with id {profileId} was not found.");

    /// <summary>
    /// Creates an error indicating the user already has an active
    /// (non-revoked) Profile assignment, so a new assignment cannot be
    /// created until the existing one is revoked (chat, 2026-08-25:
    /// each user may hold at most one active Profile at a time).
    /// </summary>
    public static Error UserAlreadyHasActiveAssignment(Guid existingProfileId) => Error.Conflict(
        "Profile.UserAlreadyHasActiveAssignment",
        $"This user already has an active profile (id {existingProfileId}). " +
        "Revoke the existing assignment before assigning a new one.");

    /// <summary>
    /// Creates an error indicating that a Profile cannot be deleted
    /// because it still has one or more active (non-revoked)
    /// UserProfileAssignment records (chat, 2026-08-25).
    /// </summary>
    public static Error ProfileHasActiveAssignments() => Error.Conflict(
        "Profile.HasActiveAssignments",
        "This profile has already been assigned to one or more users. " +
        "Remove it from every user's assignment list before deleting it.");
}
