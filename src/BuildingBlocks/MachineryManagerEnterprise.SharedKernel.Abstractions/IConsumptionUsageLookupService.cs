namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup allowing other modules (Configuration)
/// to check whether one of their catalog entries is referenced by any
/// Consumption record, before allowing that entry to be deleted. Mirrors
/// <see cref="IPersonnelUsageLookupService"/>. Introduced (chat,
/// 2026-09-16) for the Consumption module's Lubricant Overflow Report
/// feature.
/// </summary>
public interface IConsumptionUsageLookupService
{
    /// <summary>Determines whether the given Lubricant Type is referenced by any Lubricant Overflow Report line.</summary>
    Task<bool> IsLubricantTypeInUseAsync(Guid lubricantTypeId, CancellationToken cancellationToken = default);

    /// <summary>Determines whether the given Overflow Component is referenced by any Lubricant Overflow Report line.</summary>
    Task<bool> IsOverflowComponentInUseAsync(Guid overflowComponentId, CancellationToken cancellationToken = default);
}
