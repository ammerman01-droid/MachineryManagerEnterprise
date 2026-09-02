using MachineryManager.Configuration.Application.Features.Companies.Commands.RegisterCompany;
using MachineryManager.Configuration.Application.Features.Companies.Queries.GetCompaniesByHolding;
using MachineryManager.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Configuration.Presentation.Endpoints;

/// <summary>Maps the Configuration module's Company REST endpoints: base path <c>/api/v1/companies</c>.</summary>
public static class CompanyEndpoints
{
    /// <summary>Registers the Company endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapCompanyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/companies")
            .WithTags("Companies")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterCompanyAsync)
            .WithName("RegisterCompany")
            .WithSummary("Registers a new Company (manufacturer) within a Holding.");

        group.MapGet("/", GetCompaniesByHoldingAsync)
            .WithName("GetCompaniesByHolding")
            .WithSummary("Retrieves every Company registered for a Holding.");

        return endpoints;
    }

    private static async Task<IResult> RegisterCompanyAsync(
        RegisterCompanyRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterCompanyCommand(request.HoldingId, request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/companies/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetCompaniesByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCompaniesByHoldingQuery(holdingId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }
}