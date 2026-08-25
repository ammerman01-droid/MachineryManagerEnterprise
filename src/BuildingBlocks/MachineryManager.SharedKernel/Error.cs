namespace MachineryManager.SharedKernel;

/// <summary>
/// Represents a Business Error per 05-development/07-ErrorHandling.md:
/// a violation of a business rule, returned as data rather than thrown
/// as an exception.
/// </summary>
/// <param name="Code">A stable, machine-readable error code (e.g. "Asset.AlreadyRetired").</param>
/// <param name="Message">A human-readable description of the violation.</param>
/// <param name="Type">
/// The category of the error, used by Presentation layers to translate
/// it into a transport-specific response (07-api/06-ErrorHandling.md,
/// Section 8.7). Defaults to <see cref="ErrorType.Failure"/>.
/// </param>
public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    /// <summary>Represents the absence of an error, used only by a successful <see cref="Result"/>.</summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>Creates a Not Found business error.</summary>
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    /// <summary>Creates a Validation business error.</summary>
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    /// <summary>Creates a Conflict business error.</summary>
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    /// <summary>Creates an unexpected-failure business error.</summary>
    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
}
