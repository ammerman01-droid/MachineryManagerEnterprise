using Administration.Domain;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Administration.Application.Authorization;

/// <inheritdoc cref="IScopeChainResolver" />
public sealed class ScopeChainResolver : IScopeChainResolver
{
    private readonly IHoldingLookupService _holdingLookup;
    private readonly IOrganizationLookupService _organizationLookup;
    private readonly IProjectLookupService _projectLookup;

    /// <summary>Initializes a new instance of the <see cref="ScopeChainResolver"/> class.</summary>
    /// <param name="holdingLookup">Verifies Holdings.</param>
    /// <param name="organizationLookup">Resolves an Organization's Holding.</param>
    /// <param name="projectLookup">Resolves a Project's Organization.</param>
    public ScopeChainResolver(
        IHoldingLookupService holdingLookup,
        IOrganizationLookupService organizationLookup,
        IProjectLookupService projectLookup)
    {
        _holdingLookup = holdingLookup;
        _organizationLookup = organizationLookup;
        _projectLookup = projectLookup;
    }

    /// <inheritdoc />
    public async Task<Result<ScopeChain>> ResolveAsync(
        AuthorizationScope scope,
        CancellationToken cancellationToken = default)
    {
        switch (scope.Level)
        {
            case AuthorizationScopeLevel.Platform:
                return ScopeChain.Platform;

            case AuthorizationScopeLevel.Holding:
            {
                if (scope.HoldingId is not { } holdingId)
                {
                    return Result.Failure<ScopeChain>(Error.Validation("Scope.HoldingRequired", "A Holding identifier is required."));
                }

                return await _holdingLookup.ExistsAsync(holdingId, cancellationToken)
                    ? new ScopeChain(ScopeLevel.Holding, holdingId, null, null)
                    : Result.Failure<ScopeChain>(Error.NotFound("Holding.NotFound", $"Holding with id {holdingId} was not found."));
            }

            case AuthorizationScopeLevel.Organization:
            {
                if (scope.OrganizationId is not { } organizationId)
                {
                    return Result.Failure<ScopeChain>(Error.Validation("Scope.OrganizationRequired", "An Organization identifier is required."));
                }

                var holdingId = await _organizationLookup.GetHoldingIdAsync(organizationId, cancellationToken);

                return holdingId is null
                    ? Result.Failure<ScopeChain>(Error.NotFound("Organization.NotFound", $"Organization with id {organizationId} was not found."))
                    : new ScopeChain(ScopeLevel.Organization, holdingId, organizationId, null);
            }

            case AuthorizationScopeLevel.Project:
            {
                if (scope.ProjectId is not { } projectId)
                {
                    return Result.Failure<ScopeChain>(Error.Validation("Scope.ProjectRequired", "A Project identifier is required."));
                }

                var organizationId = await _projectLookup.GetOrganizationIdAsync(projectId, cancellationToken);

                if (organizationId is null)
                {
                    return Result.Failure<ScopeChain>(Error.NotFound("Project.NotFound", $"Project with id {projectId} was not found."));
                }

                var holdingId = await _organizationLookup.GetHoldingIdAsync(organizationId.Value, cancellationToken);

                return holdingId is null
                    ? Result.Failure<ScopeChain>(Error.NotFound("Organization.NotFound", $"Organization with id {organizationId} was not found."))
                    : new ScopeChain(ScopeLevel.Project, holdingId, organizationId, projectId);
            }

            default:
                throw new ArgumentOutOfRangeException(nameof(scope));
        }
    }
}
