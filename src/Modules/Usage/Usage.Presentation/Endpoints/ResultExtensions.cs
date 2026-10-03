using MachineryManagerEnterprise.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace MachineryManagerEnterprise.Usage.Presentation.Endpoints;

/// <summary>
/// Translates a failed <see cref="Result"/> into the error response
/// shape defined in 07-api conventions, Section 8.7 (Error Response
/// Structure), mapping the business <see cref="ErrorType"/> to the
/// corresponding HTTP status and error-code prefix.
/// </summary>
/// <remarks>
/// Mirrors Asset.Presentation's (and Organization/Administration's)
/// copy of this class — duplicated per module by existing project
/// convention, not extracted into BuildingBlocks (would be a
/// structural change requiring separate approval).
/// </remarks>
internal static class ResultExtensions
{
    /// <summary>Builds the standard problem response for a failed <paramref name="result"/>.</summary>
    public static IResult ToProblemResult(this Result result, HttpContext httpContext)
    {
        var statusCode = result.Error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,

            // Every "*.NotAuthorized" error across the codebase uses
            // Error.Failure — an authorization denial is a client
            // error (403), not a server fault (matches the fix applied
            // identically across every other module's copy, chat 2026-08-30).
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
