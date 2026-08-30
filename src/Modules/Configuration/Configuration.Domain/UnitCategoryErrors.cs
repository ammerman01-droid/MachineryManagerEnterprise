using MachineryManager.SharedKernel;

namespace Configuration.Domain;

/// <summary>Represents the UnitCategoryErrors type.</summary>
public static class UnitCategoryErrors
{
/// <summary>Executes the NameRequired operation.</summary>
    public static Error NameRequired() => Error.Validation(
        "UnitCategory.NameRequired", "Unit category name is required.");

/// <summary>Executes the NameTooLong operation.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "UnitCategory.NameTooLong", $"Unit category name shall not exceed {maxLength} characters.");

/// <summary>Executes the NotAuthorized operation.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "UnitCategory.NotAuthorized", "You do not have permission to perform this action.");
}