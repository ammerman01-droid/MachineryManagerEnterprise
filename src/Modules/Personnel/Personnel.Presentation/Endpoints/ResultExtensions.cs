using MachineryManagerEnterprise.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace MachineryManagerEnterprise.Personnel.Presentation.Endpoints;

/// <summary>
/// Translates a failed <see cref="Result"/> into the standard error response
/// shape used by the Personnel module's REST endpoints.
/// </summary>
/// <remarks>
/// This mapping logic is intentionally kept at module level, matching the
/// current API conventions used by the Organization module.
/// </remarks>
internal static class ResultExtensions
{
    /// <summary>
    /// Builds the standard problem response for a failed <paramref name="result"/>.
    /// </summary>
    /// <param name="result">
    /// The failed result. Caller must have already checked
    /// <see cref="Result.IsFailure"/>.
    /// </param>
    /// <param name="httpContext">
    /// The current HTTP context, used to correlate the error with request logs.
    /// </param>
    /// <returns>
    /// An <see cref="IResult"/> carrying the mapped HTTP status and error body.
    /// </returns>
    public static IResult ToProblemResult(
        this Result result,
        HttpContext httpContext)
    {
        var statusCode = result.Error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
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