using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.FreezeOrganizationUsage;

/// <summary>
/// Handles <see cref="FreezeOrganizationUsageCommand"/>: checks the freeze permission once for the
/// Organization (and Project, when given), then moves the freeze date of every matching Ledger
/// forward in a single transaction. Ledgers already frozen to the same or a later date are skipped
/// rather than treated as an error, since a freeze never moves backward.
/// </summary>
public sealed class FreezeOrganizationUsageCommandHandler
    : IRequestHandler<FreezeOrganizationUsageCommand, Result<FreezeOrganizationUsageResult>>
{
    private const string RequiredPermission = "DailyUsage.Freeze";

    private readonly IUsageLedgerRepository _usageLedgerRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="FreezeOrganizationUsageCommandHandler"/> class.</summary>
    /// <param name="usageLedgerRepository">The Usage Ledger repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work, used to commit every frozen Ledger together.</param>
    /// <param name="dateTimeProvider">Provides the current time for the raised events and today's date for the future-date check.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the Organization's Holding.</param>
    public FreezeOrganizationUsageCommandHandler(
        IUsageLedgerRepository usageLedgerRepository,
        IUsageUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _usageLedgerRepository = usageLedgerRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the bulk freeze.</summary>
    public async Task<Result<FreezeOrganizationUsageResult>> Handle(
        FreezeOrganizationUsageCommand request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<FreezeOrganizationUsageResult>(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, request.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<FreezeOrganizationUsageResult>(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var today = DateOnly.FromDateTime(_dateTimeProvider.UtcNow.UtcDateTime);

        if (request.FrozenUpToDate > today)
        {
            return Result.Failure<FreezeOrganizationUsageResult>(Error.Validation(
                "Usage.FreezeDateInFuture",
                "The freeze date cannot be in the future."));
        }

        var ledgers = await _usageLedgerRepository.ListByOrganizationAsync(
            request.OrganizationId, request.ProjectId, cancellationToken);

        var frozen = 0;
        var alreadyFrozen = 0;

        foreach (var ledger in ledgers)
        {
            if (ledger.FrozenUpToDate is { } current && current >= request.FrozenUpToDate)
            {
                alreadyFrozen++;
                continue;
            }

            var result = ledger.FreezeUpTo(request.FrozenUpToDate, _dateTimeProvider);

            if (result.IsFailure)
            {
                return Result.Failure<FreezeOrganizationUsageResult>(result.Error);
            }

            _usageLedgerRepository.Update(ledger);
            frozen++;
        }

        if (frozen > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new FreezeOrganizationUsageResult(frozen, alreadyFrozen);
    }
}
