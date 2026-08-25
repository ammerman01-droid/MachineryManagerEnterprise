namespace MachineryManager.SharedKernel;

/// <summary>
/// Categorizes a business <see cref="Error"/> for translation into a
/// transport-specific response (e.g. HTTP status code), per
/// 07-api/06-ErrorHandling.md, Section 8.7 (Error Response Structure).
/// </summary>
public enum ErrorType
{
    /// <summary>An unexpected failure not otherwise categorized (maps to 500 / SYS).</summary>
    Failure,

    /// <summary>The request failed input or business validation (maps to 400 / VAL).</summary>
    Validation,

    /// <summary>The requested resource does not exist (maps to 404 / RES).</summary>
    NotFound,

    /// <summary>The request conflicts with the current business state (maps to 409 / BUS).</summary>
    Conflict,
}
