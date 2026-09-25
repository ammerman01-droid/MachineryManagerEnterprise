using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain;

/// <summary>Business Errors for the <see cref="LubricantOverflowReport"/> aggregate and its child entities.</summary>
public static class LubricantOverflowReportErrors
{
    /// <summary>Creates an error indicating the report is missing a required Asset reference.</summary>
    public static Error AssetRequired() => Error.Validation("LubricantOverflowReport.AssetRequired", "Asset is required.");

    /// <summary>Creates an error indicating the report is missing a required Project reference.</summary>
    public static Error ProjectRequired() => Error.Validation("LubricantOverflowReport.ProjectRequired", "Project is required.");

    /// <summary>Creates an error indicating the Hour Meter Reading is invalid.</summary>
    public static Error HourMeterReadingInvalid() => Error.Validation("LubricantOverflowReport.HourMeterReadingInvalid", "Hour meter reading must be zero or greater.");

    /// <summary>Creates an error indicating the report date is invalid (e.g. in the future).</summary>
    public static Error ReportDateInvalid() => Error.Validation("LubricantOverflowReport.ReportDateInvalid", "Report date cannot be in the future.");

    /// <summary>Creates an error indicating a report was submitted with no overflow lines.</summary>
    public static Error AtLeastOneLineRequired() => Error.Validation("LubricantOverflowReport.AtLeastOneLineRequired", "At least one overflow line is required.");

    /// <summary>Creates an error indicating a line's Overflow Component was not provided.</summary>
    public static Error OverflowComponentRequired() => Error.Validation("LubricantOverflowReport.OverflowComponentRequired", "Overflowed component is required.");

    /// <summary>Creates an error indicating a line's Lubricant Type was not provided.</summary>
    public static Error LubricantTypeRequired() => Error.Validation("LubricantOverflowReport.LubricantTypeRequired", "Lubricant type is required.");

    /// <summary>Creates an error indicating a line's amount is not a positive number.</summary>
    public static Error AmountMustBePositive() => Error.Validation("LubricantOverflowReport.AmountMustBePositive", "Overflow amount (liters) must be greater than zero.");

    /// <summary>Creates an error indicating a line's reason was not provided.</summary>
    public static Error ReasonRequired() => Error.Validation("LubricantOverflowReport.ReasonRequired", "Overflow reason is required.");

    /// <summary>Creates an error indicating a line's reason exceeds the maximum allowed length.</summary>
    public static Error ReasonTooLong(int maxLength) => Error.Validation("LubricantOverflowReport.ReasonTooLong", $"Overflow reason shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating a personnel entry is missing its Personnel reference.</summary>
    public static Error PersonnelRequired() => Error.Validation("LubricantOverflowReport.PersonnelRequired", "Personnel is required.");

    /// <summary>Creates an error indicating a personnel entry's duration is invalid.</summary>
    public static Error DurationInvalid() => Error.Validation("LubricantOverflowReport.DurationInvalid", "Duration must be greater than zero and no more than 24 hours.");

    /// <summary>Creates an error indicating the referenced line was not found on the report.</summary>
    public static Error LineNotFound() => Error.NotFound("LubricantOverflowReport.LineNotFound", "Overflow line not found on this report.");

    /// <summary>Creates an error indicating the referenced personnel entry was not found on the report.</summary>
    public static Error PersonnelEntryNotFound() => Error.NotFound("LubricantOverflowReport.PersonnelEntryNotFound", "Personnel entry not found on this report.");

    /// <summary>Creates an error indicating the report was not found.</summary>
    public static Error NotFound() => Error.NotFound("LubricantOverflowReport.NotFound", "Lubricant overflow report not found.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure("LubricantOverflowReport.NotAuthorized", "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating the report's date falls on/before the Organization's freeze threshold and can no longer be modified.</summary>
    public static Error Frozen() => Error.Conflict(
        "LubricantOverflowReport.Frozen",
        "This report's date has been frozen by an administrator and can no longer be edited or deleted.");
}
