using MachineryManagerEnterprise.SharedKernel;

namespace Configuration.Domain;

/// <summary>Business Errors for the <see cref="JobTitle"/> aggregate.</summary>
public static class JobTitleErrors
{
    /// <summary>Creates an error indicating the job title's name was not provided.</summary>
    public static Error NameRequired() => Error.Validation("JobTitle.NameRequired", "Job title name is required.");

    /// <summary>Creates an error indicating the job title's name exceeds the maximum allowed length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation("JobTitle.NameTooLong", $"Job title name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure("JobTitle.NotAuthorized", "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating this Job Title is referenced by at least one Personnel record and cannot be deleted.</summary>
    public static Error InUse() => Error.Conflict(
        "JobTitle.InUse",
        "This job title is used by at least one Personnel record and cannot be deleted.");

}