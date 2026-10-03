using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.CorrectEntryDetails;

/// <summary>Handles <see cref="CorrectEntryDetailsCommand"/> by loading the ledger, checking permission, and applying the detail-only correction.</summary>
public sealed class CorrectEntryDetailsCommandHandler
    : IRequestHandler<CorrectEntryDetailsCommand, Result>
{
    private const string RequiredPermission = "DailyUsage.Correct";

    private readonly IUsageLedgerRepository _usageLedgerRepository;
    private readonly IUsageUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="CorrectEntryDetailsCommandHandler"/> class.</summary>
    /// <param name="usageLedgerRepository">The Usage Ledger repository.</param>
    /// <param name="unitOfWork">The Usage module's Unit of Work.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to resolve the ledger's Holding.</param>
    public CorrectEntryDetailsCommandHandler(
        IUsageLedgerRepository usageLedgerRepository,
        IUsageUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _usageLedgerRepository = usageLedgerRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the shift reading detail correction use case.</summary>
    public async Task<Result> Handle(CorrectEntryDetailsCommand request, CancellationToken cancellationToken)
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

        var correctResult = ledger.CorrectEntryDetails(
            request.EntryDate, request.ShiftIndex, request.OperatorId, request.ShiftStartTime, request.ShiftEndTime);

        if (correctResult.IsFailure)
        {
            return correctResult;
        }

        _usageLedgerRepository.Update(ledger);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
