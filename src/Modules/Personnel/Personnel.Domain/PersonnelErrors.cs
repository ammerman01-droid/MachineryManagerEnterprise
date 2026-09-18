using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Personnel.Domain;

/// <summary>Business Errors for the <see cref="Personnel"/> aggregate.</summary>
public static class PersonnelErrors
{
    /// <summary>Creates an error indicating the first name was not provided.</summary>
    public static Error FirstNameRequired() => Error.Validation("Personnel.FirstNameRequired", "First name is required.");

    /// <summary>Creates an error indicating the first name exceeds the maximum allowed length.</summary>
    public static Error FirstNameTooLong(int maxLength) => Error.Validation("Personnel.FirstNameTooLong", $"First name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the last name was not provided.</summary>
    public static Error LastNameRequired() => Error.Validation("Personnel.LastNameRequired", "Last name is required.");

    /// <summary>Creates an error indicating the last name exceeds the maximum allowed length.</summary>
    public static Error LastNameTooLong(int maxLength) => Error.Validation("Personnel.LastNameTooLong", $"Last name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the personnel code was not provided.</summary>
    public static Error PersonnelCodeRequired() => Error.Validation("Personnel.PersonnelCodeRequired", "Personnel code is required.");

    /// <summary>Creates an error indicating the personnel code exceeds the maximum allowed length.</summary>
    public static Error PersonnelCodeTooLong(int maxLength) => Error.Validation("Personnel.PersonnelCodeTooLong", $"Personnel code shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating this personnel code is already registered within the Organization.</summary>
    public static Error PersonnelCodeAlreadyExists() => Error.Conflict("Personnel.PersonnelCodeAlreadyExists", "This personnel code is already registered within this Organization.");

    /// <summary>Creates an error indicating the Job Title identifier was not provided.</summary>
    public static Error JobTitleIdRequired() => Error.Validation("Personnel.JobTitleIdRequired", "Job title is required.");

    /// <summary>Creates an error indicating a Driving License Type identifier was not provided.</summary>
    public static Error DrivingLicenseTypeIdRequired() => Error.Validation("Personnel.DrivingLicenseTypeIdRequired", "Driving license type is required.");

    /// <summary>Creates an error indicating a driving license expiry date was not provided.</summary>
    public static Error DrivingLicenseExpiryDateRequired() => Error.Validation("Personnel.DrivingLicenseExpiryDateRequired", "Driving license expiry date is required.");

    /// <summary>Creates an error indicating this Personnel already holds a license of the given Driving License Type.</summary>
    public static Error DuplicateDrivingLicenseType() => Error.Validation("Personnel.DuplicateDrivingLicenseType", "This Personnel already has a driving license of this type.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure("Personnel.NotAuthorized", "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating the given Personnel record was not found.</summary>
    public static Error NotFound() => Error.NotFound("Personnel.NotFound", "Personnel record was not found.");

}