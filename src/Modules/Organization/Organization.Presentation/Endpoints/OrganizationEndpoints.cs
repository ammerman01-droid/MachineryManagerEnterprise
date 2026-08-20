using MachineryManager.Organization.Application.Features.Organizations.Commands.RegisterOrganization;
using MachineryManager.Organization.Application.Features.Organizations.Queries.GetOrganizationById;
using MachineryManager.Organization.Application.Features.Organizations.Queries.SearchOrganizations;
using MachineryManager.Organization.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManager.Organization.Presentation.Endpoints;

/// <summary>
/// Maps the Organization module's REST endpoints per 07-api conventions
/// (Section 8): base path <c>/api/v1/organizations</c>.
/// </summary>
public static class OrganizationEndpoints
{
    /// <summary>Registers the Organization module's endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapOrganizationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/organizations")
            .WithTags("Organizations")
            // ADR-0030: every Organization endpoint requires a valid
            // Bearer access token issued by this app's own OpenIddict
            // server. Role/permission-scoped restrictions per action
            // are NOT yet applied here — that requires the
            // Administration module's Role→Permission mapping, which
            // does not exist yet (open item, not a silent gap).
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterOrganizationAsync)
            .WithName("RegisterOrganization")
            .WithSummary("Registers a new Organization.");

        group.MapGet("/{organizationId:guid}", GetOrganizationByIdAsync)
            .WithName("GetOrganizationById")
            .WithSummary("Retrieves a single Organization by its identifier.");

        group.MapGet("/", SearchOrganizationsAsync)
            .WithName("SearchOrganizations")
            .WithSummary("Searches Organizations with optional text filtering and pagination.");

        return endpoints;
    }

    private static async Task<IResult> RegisterOrganizationAsync(
        RegisterOrganizationRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterOrganizationCommand(request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/organizations/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetOrganizationByIdAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetOrganizationByIdQuery(organizationId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchOrganizationsAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchOrganizationsQuery(search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }
}
