using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.DeleteLatestUsageEntry;

/// <summary>Handles <see cref="DeleteLatestUsageEntryCommand"/> by loading the ledger, checking permission, and invoking domain deletion.</summary>
public sealed class DeleteLatestUsageEntryCommandHandler
    : IRequestHandler<DeleteLatestUsageEntryCommand, Result>
{
    private const string RequiredPermission = "DailyUsage.Delete";

    private readonly IUsageLedgerRepository _usageLedgerRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="DeleteLatestUsageEntryCommandHandler"/> class.</summary>
    /// <param name="usageLedgerRepository">The Usage Ledger repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the ledger's Holding.</param>
    public DeleteLatestUsageEntryCommandHandler(
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

    /// <summary>Executes the deletion use case.</summary>
    public async Task<Result> Handle(DeleteLatestUsageEntryCommand request, CancellationToken cancellationToken)
    {
        var ledger = await _usageLedgerRepository.GetByOwnerAsync(request.OwnerType, request.OwnerId, request.Unit, cancellationToken);

        if (ledger is null)
        {
            return Result.Failure(Error.NotFound(
                "UsageLedger.NotFound", $"No Usage Ledger was found for owner {request.OwnerId}."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(ledger.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, ledger.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Usage.Domain.UsageLedgerErrors.NotAuthorized());
        }

        var result = ledger.DeleteLatestEntry(request.EntryDate, request.ShiftIndex, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _usageLedgerRepository.Update(ledger);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
