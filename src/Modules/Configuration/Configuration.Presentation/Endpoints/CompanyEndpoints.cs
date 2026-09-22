using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.ActivateCompany;
using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.DeactivateCompany;
using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.RegisterCompany;
using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.UpdateCompany;
using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Queries.GetCompaniesByHolding;
using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Queries.GetCompanyById;
using MachineryManagerEnterprise.Configuration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Configuration.Presentation.Endpoints;

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

        group.MapGet("/{id:guid}", GetCompanyByIdAsync)
            .WithName("GetCompanyById")
            .WithSummary("Retrieves a single Company by its identifier.");

        group.MapPut("/{id:guid}", UpdateCompanyAsync)
            .WithName("UpdateCompany")
            .WithSummary("Updates an existing Company's display name.");

        group.MapPatch("/{id:guid}/deactivate", DeactivateCompanyAsync)
            .WithName("DeactivateCompany")
            .WithSummary("Deactivates (soft-deletes) a Company.");

        group.MapPatch("/{id:guid}/activate", ActivateCompanyAsync)
            .WithName("ActivateCompany")
            .WithSummary("Reactivates a previously deactivated Company.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/companies</c>.</summary>
    /// <param name="request">The registration payload.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RegisterCompanyCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>201 Created</c> with the new company's id on success; otherwise a standard error body.</returns>
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

    /// <summary>Handles <c>GET /api/v1/companies?holdingId=...&amp;includeInactive=...</c>.</summary>
    /// <param name="holdingId">The Holding whose Company catalog should be returned.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetCompaniesByHoldingQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <param name="includeInactive">
    /// When <see langword="true"/>, deactivated companies are included
    /// (used by the admin management page). Defaults to <see langword="false"/>.
    /// </param>
    /// <returns><c>200 OK</c> with the list of companies on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetCompaniesByHoldingAsync(
        Guid holdingId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        bool includeInactive = false)
    {
        var result = await sender.Send(new GetCompaniesByHoldingQuery(holdingId, includeInactive), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/companies/{id}</c>.</summary>
    /// <param name="id">The identifier of the Company to retrieve.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetCompanyByIdQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the Company on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetCompanyByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCompanyByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PUT /api/v1/companies/{id}</c>.</summary>
    /// <param name="id">The identifier of the Company to update.</param>
    /// <param name="request">The update payload (Name).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="UpdateCompanyCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> UpdateCompanyAsync(
        Guid id,
        UpdateCompanyRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateCompanyCommand(id, request.Name), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PATCH /api/v1/companies/{id}/deactivate</c>.</summary>
    /// <param name="id">The identifier of the Company to deactivate.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="DeactivateCompanyCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> DeactivateCompanyAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateCompanyCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>PATCH /api/v1/companies/{id}/activate</c>.</summary>
    /// <param name="id">The identifier of the Company to reactivate.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="ActivateCompanyCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>204 No Content</c> on success; otherwise a standard error body.</returns>
    private static async Task<IResult> ActivateCompanyAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateCompanyCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }
}
