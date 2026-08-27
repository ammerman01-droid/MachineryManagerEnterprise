using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Asset.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Asset.Domain.Asset"/> aggregate.</summary>
public interface IAssetRepository : IRepository<global::Asset.Domain.Asset, global::Asset.Domain.AssetId>
{
}