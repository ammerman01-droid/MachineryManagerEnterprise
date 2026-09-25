using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Dtos;

namespace MachineryManagerEnterprise.Configuration.Application.Abstractions;

/// <summary>Repository for the <see cref="OverflowComponent"/> aggregate.</summary>
public interface IOverflowComponentRepository
{
    /// <summary>Retrieves a Overflow Component by its identifier, or <c>null</c> if it does not exist.</summary>
    Task<OverflowComponent?> GetByIdAsync(OverflowComponentId id, CancellationToken cancellationToken = default);

    /// <summary>Retrieves the list of Overflow Component options defined for a Holding.</summary>
    Task<IReadOnlyList<OverflowComponentDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>Adds a new Overflow Component.</summary>
    void Add(OverflowComponent overflowComponent);

    /// <summary>Marks an existing Overflow Component as updated.</summary>
    void Update(OverflowComponent overflowComponent);

    /// <summary>Removes a Overflow Component.</summary>
    void Remove(OverflowComponent overflowComponent);
}
