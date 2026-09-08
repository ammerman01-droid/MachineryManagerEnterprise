using MachineryManager.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace MachineryManager.Administration.Presentation.Endpoints;

/// <summary>
/// Translates a failed <see cref="Result"/> into the standard error response shape.
/// </summary>
internal static class ResultExtensions
{
    /// <summary>Builds the standard problem response for a failed result.</summary>
    public static IResult ToProblemResult(this Result result, HttpContext httpContext)
    {
        var statusCode = result.Error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,

            // Added (chat, 2026-08-30 — bug fix, applied identically
            // across every module's copy of this class): ErrorType.Failure
            // had no explicit case and fell into the catch-all 500 branch,
            // even for legitimate authorization denials (every
            // "*.NotAuthorized" error across the codebase uses
            // Error.Failure). An authorization denial is a client error,
            // not a server fault — it must map to 403, not 500.
            ErrorType.Failure => StatusCodes.Status403Forbidden,

            _ => StatusCodes.Status500InternalServerError,
        };

        var title = result.Error.Type switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Conflict => "Business Rule Violation",
            ErrorType.Failure => "Not Authorized",
            _ => "Unexpected Error",
        };

        return Results.Json(
            new
            {
                errorCode = result.Error.Code,
                title,
                message = result.Error.Message,
                correlationId = httpContext.TraceIdentifier,
                details = Array.Empty<string>(),
            },
            statusCode: statusCode);
    }
}