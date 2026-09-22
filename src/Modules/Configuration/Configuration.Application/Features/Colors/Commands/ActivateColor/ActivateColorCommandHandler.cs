using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.ActivateColor;

/// <summary>Handles <see cref="ActivateColorCommand"/>.</summary>
public sealed class ActivateColorCommandHandler : IRequestHandler<ActivateColorCommand, Result>
{
    private const string RequiredPermission = "Color.Edit";

    private readonly IColorRepository _colorRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="ActivateColorCommandHandler"/> class.</summary>
    /// <param name="colorRepository">The Color repository.</param>
    /// <param name="unitOfWork">The Configuration module's unit of work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public ActivateColorCommandHandler(
        IColorRepository colorRepository,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _colorRepository = colorRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the reactivation use case.</summary>
    /// <param name="request">The command to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A successful <see cref="Result"/>, or a not-found/authorization/conflict error.</returns>
    public async Task<Result> Handle(ActivateColorCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Configuration.Domain.ColorErrors.NotAuthorized());
        }

        var color = await _colorRepository.GetByIdAsync(
            global::Configuration.Domain.ColorId.From(request.Id), cancellationToken);

        if (color is null)
        {
            return Result.Failure(global::Configuration.Domain.ColorErrors.NotFound(request.Id));
        }

        var scope = new ResourceScope(color.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Configuration.Domain.ColorErrors.NotAuthorized());
        }

        var result = color.Activate(_dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _colorRepository.Update(color);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
