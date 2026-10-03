using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetAccumulatedOperationalUsage;

/// <summary>
/// Handles <see cref="GetAccumulatedOperationalUsageQuery"/> by loading
/// the owner's Usage Ledger through the EF Core aggregate repository —
/// this query reads a single, already-loaded aggregate rather than the
/// cross-ledger join <see cref="IUsageReadService"/> exists for, so it
/// has no need of that abstraction — and summing
/// <c>OperationalUsageAmount</c> (BR-048) across the requested date
/// range.
/// </summary>
public sealed class GetAccumulatedOperationalUsageQueryHandler
    : IRequestHandler<GetAccumulatedOperationalUsageQuery, Result<decimal>>
{
    private const string RequiredPermission = "DailyUsage.View";

    private readonly IUsageLedgerRepository _usageLedgerRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="GetAccumulatedOperationalUsageQueryHandler"/> class.</summary>
    /// <param name="usageLedgerRepository">The Usage Ledger repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the ledger's Holding.</param>
    public GetAccumulatedOperationalUsageQueryHandler(
        IUsageLedgerRepository usageLedgerRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _usageLedgerRepository = usageLedgerRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the accumulated-usage lookup.</summary>
    public async Task<Result<decimal>> Handle(GetAccumulatedOperationalUsageQuery request, CancellationToken cancellationToken)
    {
        var ledger = await _usageLedgerRepository.GetByOwnerAsync(request.OwnerType, request.OwnerId, request.Unit, cancellationToken);

        if (ledger is null)
        {
            return Result.Failure<decimal>(Error.NotFound(
                "UsageLedger.NotFound", $"No Usage Ledger was found for owner {request.OwnerId}."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<decimal>(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(ledger.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, ledger.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<decimal>(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var total = ledger.Entries
            .Where(e => e.EntryDate >= request.SinceDate && (request.ThroughDate is null || e.EntryDate <= request.ThroughDate))
            .Sum(e => e.OperationalUsageAmount);

        return Result.Success(total);
    }
}
