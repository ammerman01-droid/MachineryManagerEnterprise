using MachineryManagerEnterprise.Asset.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Asset.Infrastructure;

/// <inheritdoc cref="IAssetStatusUpdateService" />
public sealed class AssetStatusUpdateService : IAssetStatusUpdateService
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>Initializes a new instance of the <see cref="AssetStatusUpdateService"/> class.</summary>
    /// <param name="assetRepository">The Asset repository.</param>
    /// <param name="unitOfWork">The Asset module's Unit of Work, used to commit the status change.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    public AssetStatusUpdateService(
        IAssetRepository assetRepository,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _assetRepository = assetRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public async Task<Result> SetOperationalStatusAsync(Guid assetId, bool outOfService, CancellationToken cancellationToken = default)
    {
        var id = global::Asset.Domain.AssetId.From(assetId);
        var asset = await _assetRepository.GetByIdAsync(id, cancellationToken);

        if (asset is null)
        {
            return Result.Failure(
                Error.NotFound("Asset.NotFound", $"Asset with id {assetId} was not found."));
        }

        var targetStatus = outOfService
            ? global::Asset.Domain.AssetStatus.OutOfService
            : global::Asset.Domain.AssetStatus.Active;

        if (asset.Status == targetStatus)
        {
            // Idempotent by design (chat, 2026-09-22) — the caller
            // (Maintenance) only cares that the Asset ends up in the
            // right status, not whether this call was the one that put
            // it there.
            return Result.Success();
        }

        var result = asset.ChangeStatus(targetStatus, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _assetRepository.Update(asset);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
