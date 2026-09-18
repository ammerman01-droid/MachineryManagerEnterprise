using MachineryManagerEnterprise.Personnel.Application.Features.Commands.RegisterPersonnel;
using MachineryManagerEnterprise.Personnel.Application.Features.Queries.GetPersonnelByOrganization;
using MachineryManagerEnterprise.Personnel.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using MachineryManagerEnterprise.Personnel.Application.Features.Commands.DeletePersonnel;
using MachineryManagerEnterprise.Personnel.Application.Features.Commands.UpdatePersonnel;
using MachineryManagerEnterprise.Personnel.Application.Features.Queries.GetPersonnelById;

namespace MachineryManagerEnterprise.Personnel.Presentation.Endpoints;

/// <summary>Maps the Personnel module's REST endpoints: base path <c>/api/v1/personnel</c>.</summary>
public static class PersonnelEndpoints
{
    /// <summary>Registers the Personnel endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The route builder to add the endpoints to.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/>, for chaining.</returns>
    public static IEndpointRouteBuilder MapPersonnelEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/personnel")
            .WithTags("Personnel")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterPersonnelAsync)
            .WithName("RegisterPersonnel")
            .WithSummary("Registers a new Personnel record within an Organization.");

        group.MapGet("/", GetPersonnelByOrganizationAsync)
            .WithName("GetPersonnelByOrganization")
            .WithSummary("Retrieves every Personnel record owned by an Organization.");

        group.MapPut("/{id:guid}", UpdatePersonnelAsync)
            .WithName("UpdatePersonnel")
            .WithSummary("Updates an existing Personnel record.");

        group.MapDelete("/{id:guid}", DeletePersonnelAsync)
            .WithName("DeletePersonnel")
            .WithSummary("Deletes a Personnel record.");

        group.MapGet("/{id:guid}", GetPersonnelByIdAsync)
            .WithName("GetPersonnelById")
            .WithSummary("Retrieves a single Personnel record by its identifier.");

        return endpoints;
    }

    /// <summary>Handles <c>POST /api/v1/personnel</c>.</summary>
    /// <param name="request">The registration payload.</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="RegisterPersonnelCommand"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>201 Created</c> with the new Personnel's id on success; otherwise a standard error body.</returns>
    private static async Task<IResult> RegisterPersonnelAsync(
        RegisterPersonnelRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new RegisterPersonnelCommand(
            request.OrganizationId,
            request.ProjectId,
            request.FirstName,
            request.LastName,
            request.PersonnelCode,
            request.JobTitleId,
            request.DrivingLicenses.Select(l => new DrivingLicenseInput(l.DrivingLicenseTypeId, l.ExpiryDate)).ToList());

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/personnel/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    /// <summary>Handles <c>GET /api/v1/personnel?organizationId=...</c>.</summary>
    /// <param name="organizationId">The Organization whose Personnel records should be returned (required query parameter).</param>
    /// <param name="sender">MediatR sender used to dispatch the <see cref="GetPersonnelByOrganizationQuery"/>.</param>
    /// <param name="httpContext">The current request's HTTP context, used for error correlation ids.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation if the client disconnects.</param>
    /// <returns><c>200 OK</c> with the list on success; otherwise a standard error body.</returns>
    private static async Task<IResult> GetPersonnelByOrganizationAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPersonnelByOrganizationQuery(organizationId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> UpdatePersonnelAsync(
        Guid id,
        UpdatePersonnelRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePersonnelCommand(
            id,
            request.FirstName,
            request.LastName,
            request.PersonnelCode,
            request.JobTitleId,
            request.DrivingLicenses.Select(l => (l.DrivingLicenseTypeId, l.ExpiryDate)).ToList());

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> DeletePersonnelAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeletePersonnelCommand(id), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

        /// <summary>Handles <c>GET /api/v1/personnel/{id}</c>.</summary>
    private static async Task<IResult> GetPersonnelByIdAsync(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPersonnelByIdQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }
}