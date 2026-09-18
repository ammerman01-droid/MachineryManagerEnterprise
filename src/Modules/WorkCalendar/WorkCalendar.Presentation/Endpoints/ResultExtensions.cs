using MachineryManagerEnterprise.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace MachineryManagerEnterprise.WorkCalendar.Presentation.Endpoints;

/// <summary>
/// Translates a failed <see cref="Result"/> into the error response
/// shape defined in 07-api conventions, Section 8.7. Duplicated per
/// module by existing project convention (see Asset/Organization/
/// Administration.Presentation's identical copies).
/// </summary>
internal static class ResultExtensions
{
    /// <summary>Builds the standard problem response for a failed <paramref name="result"/>.</summary>
    /// <param name="result">The failed result to translate.</param>
    /// <param name="httpContext">The current HTTP context, used for the correlation id.</param>
    /// <returns>An <see cref="IResult"/> producing the standard error JSON body.</returns>
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
