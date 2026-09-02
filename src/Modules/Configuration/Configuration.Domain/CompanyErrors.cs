using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>
/// Defines business errors related to the Company aggregate.
/// </summary>
public static class CompanyErrors
{
    /// <summary>Creates an error indicating that the company name is required.</summary>
    public static Error NameRequired() => Error.Validation(
        "Company.NameRequired",
        "Company name is required.");

    /// <summary>Creates an error indicating that the company name is too long.</summary>
    /// <param name="maxLength">The maximum allowed length.</param>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Company.NameTooLong",
        $"Company name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating that the company already exists in the Holding.</summary>
    public static Error AlreadyExists() => Error.Conflict(
        "Company.AlreadyExists",
        "A company with the same name already exists in this Holding.");

    /// <summary>Creates an error indicating that the requested Company does not exist in the Holding.</summary>
    /// <param name="companyId">The requested Company identifier.</param>
    public static Error NotFound(Guid companyId) => Error.NotFound(
        "Company.NotFound",
        $"Company with id {companyId} was not found in this Holding.");

    /// <summary>Creates an authorization error.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "Company.NotAuthorized",
        "You are not authorized to access companies in this Holding.");
}