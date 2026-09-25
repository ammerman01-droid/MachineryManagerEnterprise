using Consumption.Domain;

namespace MachineryManagerEnterprise.Consumption.Application.Abstractions;

/// <summary>Repository for the <see cref="LubricantOverflowReport"/> aggregate.</summary>
public interface ILubricantOverflowReportRepository
{
    /// <summary>Retrieves a Lubricant Overflow Report by its identifier, or <c>null</c> if it does not exist.</summary>
    Task<LubricantOverflowReport?> GetByIdAsync(LubricantOverflowReportId id, CancellationToken cancellationToken = default);

    /// <summary>Retrieves the Lubricant Overflow Reports recorded for a given Asset, most recent first.</summary>
    Task<IReadOnlyList<LubricantOverflowReport>> GetByAssetAsync(Guid assetId, CancellationToken cancellationToken = default);

    /// <summary>Determines whether the given Lubricant Type is referenced by any report line.</summary>
    Task<bool> IsLubricantTypeInUseAsync(Guid lubricantTypeId, CancellationToken cancellationToken = default);

    /// <summary>Determines whether the given Overflow Component is referenced by any report line.</summary>
    Task<bool> IsOverflowComponentInUseAsync(Guid overflowComponentId, CancellationToken cancellationToken = default);

    /// <summary>Adds a new Lubricant Overflow Report.</summary>
    void Add(LubricantOverflowReport report);

    /// <summary>Marks an existing Lubricant Overflow Report as updated.</summary>
    void Update(LubricantOverflowReport report);

    /// <summary>Removes a Lubricant Overflow Report.</summary>
    void Remove(LubricantOverflowReport report);
}
