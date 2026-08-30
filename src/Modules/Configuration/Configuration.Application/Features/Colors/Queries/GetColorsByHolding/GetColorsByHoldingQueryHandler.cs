using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.Colors.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.Colors.Queries.GetColorsByHolding;

/// <summary>Handles <see cref="GetColorsByHoldingQuery"/>.</summary>
public sealed class GetColorsByHoldingQueryHandler
    : IRequestHandler<GetColorsByHoldingQuery, Result<IReadOnlyList<ColorDto>>>
{
    private const string RequiredPermission = "Color.View";

    private readonly IColorRepository _colorRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetColorsByHoldingQueryHandler"/> class.</summary>
    /// <param name="colorRepository">The Color repository.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public GetColorsByHoldingQueryHandler(
        IColorRepository colorRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _colorRepository = colorRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the query.</summary>
    /// <param name="request">The query to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{T}"/> containing the Holding's Color list, or an authorization error.</returns>
    public async Task<Result<IReadOnlyList<ColorDto>>> Handle(GetColorsByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<ColorDto>>(global::Configuration.Domain.ColorErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<ColorDto>>(global::Configuration.Domain.ColorErrors.NotAuthorized());
        }

        var colors = await _colorRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);

        return Result.Success(colors);
    }
}