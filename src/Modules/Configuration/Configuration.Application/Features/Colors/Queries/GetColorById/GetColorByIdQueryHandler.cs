using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Queries.GetColorById;

/// <summary>Handles <see cref="GetColorByIdQuery"/>.</summary>
public sealed class GetColorByIdQueryHandler : IRequestHandler<GetColorByIdQuery, Result<ColorDto>>
{
    private const string RequiredPermission = "Color.View";

    private readonly IColorRepository _colorRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetColorByIdQueryHandler"/> class.</summary>
    /// <param name="colorRepository">The Color repository.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetColorByIdQueryHandler(
        IColorRepository colorRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IMapper mapper)
    {
        _colorRepository = colorRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _mapper = mapper;
    }

    /// <summary>Executes the query.</summary>
    /// <param name="request">The query to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{ColorDto}"/> containing the Color, or a not-found/authorization error.</returns>
    public async Task<Result<ColorDto>> Handle(GetColorByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<ColorDto>(global::Configuration.Domain.ColorErrors.NotAuthorized());
        }

        var color = await _colorRepository.GetByIdAsync(
            global::Configuration.Domain.ColorId.From(request.Id), cancellationToken);

        if (color is null)
        {
            return Result.Failure<ColorDto>(global::Configuration.Domain.ColorErrors.NotFound(request.Id));
        }

        var scope = new ResourceScope(color.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<ColorDto>(global::Configuration.Domain.ColorErrors.NotAuthorized());
        }

        return Result.Success(_mapper.Map<ColorDto>(color));
    }
}
