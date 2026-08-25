using MachineryManager.SharedKernel;

namespace Organization.Domain;

/// <summary>Business Errors for the Project aggregate.</summary>
public static class ProjectErrors
{
    /// <summary>Creates an error indicating that a Project must have an owning Organization.</summary>
    public static Error OrganizationRequired() => Error.Validation(
        "Project.OrganizationRequired",
        "A Project must belong to an Organization.");

    /// <summary>Creates an error indicating that the project name is required.</summary>
    public static Error NameRequired() => Error.Validation(
        "Project.NameRequired",
        "Project name is required.");

    /// <summary>Creates an error indicating that the project name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Project.NameTooLong",
        $"Project name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "Project.NotAuthorized",
        "You do not have permission to perform this action.");
}