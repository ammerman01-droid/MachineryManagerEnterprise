using MachineryManagerEnterprise.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace MachineryManagerEnterprise.Consumption.Presentation.Endpoints;

/// <summary>
/// Translates a failed <see cref="Result"/> into the standard error response shape.
/// </summary>
/// <remarks>
/// Mirrors Organization.Presentation's, Administration.Presentation's,
/// Asset.Presentation's, and Configuration.Presentation's copies of
/// this class — duplicated per module by existing project convention.
/// </remarks>
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
