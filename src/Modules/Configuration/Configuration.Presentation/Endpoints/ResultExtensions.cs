using MachineryManager.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace MachineryManager.Configuration.Presentation.Endpoints;

/// <summary>
/// Translates a failed <see cref="Result"/> from a Configuration-module
/// handler into the standard error response shape defined in
/// 07-api-conventions.md, Section 8.7 ("Error Response Structure").
/// </summary>
/// <remarks>
/// This class is intentionally duplicated per module (also present in
/// Organization.Presentation, Administration.Presentation, and
/// Asset.Presentation) rather than extracted into BuildingBlocks —
/// extracting it would be a structural change requiring separate
/// architectural approval per the AI Engineering Contract.
/// </remarks>
internal static class ResultExtensions
{
    /// <summary>
    /// Maps a failed <see cref="Result"/> to an <see cref="IResult"/>
    /// carrying the appropriate HTTP status code and a JSON error body.
    /// </summary>
    /// <param name="result">The failed result to translate. Behavior is undefined if the result represents success.</param>
    /// <param name="httpContext">The current request's HTTP context, used to populate the response's correlation id.</param>
    /// <returns>An <see cref="IResult"/> that, when executed, writes the standard error JSON body with the mapped status code.</returns>
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