using MachineryManager.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace MachineryManager.Organization.Presentation.Endpoints;

/// <summary>
/// Translates a failed <see cref="Result"/> into the error response
/// shape defined in 07-api conventions, Section 8.7 (Error Response
/// Structure), mapping the business <see cref="ErrorType"/> to the
/// corresponding HTTP status and error-code prefix.
/// </summary>
/// <remarks>
/// This mapping logic is duplicated per module until a second module
/// needs it; extracting it into a shared BuildingBlocks project would
/// be a structural change requiring approval it does not yet have
/// (06-development, Forbidden Behaviors) — flagged in the completion
/// report as a follow-up, not applied unilaterally here.
/// </remarks>
internal static class ResultExtensions
{
    /// <summary>
    /// Builds the standard problem response for a failed <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The failed result. Caller must have already checked <see cref="Result.IsFailure"/>.</param>
    /// <param name="httpContext">The current HTTP context, used to correlate the error with request logs.</param>
    /// <returns>An <see cref="IResult"/> carrying the mapped HTTP status and error body.</returns>
    public static IResult ToProblemResult(this Result result, HttpContext httpContext)
    {
        var statusCode = result.Error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        var title = result.Error.Type switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Conflict => "Business Rule Violation",
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
