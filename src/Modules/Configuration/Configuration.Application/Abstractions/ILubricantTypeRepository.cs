using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Dtos;

namespace MachineryManagerEnterprise.Configuration.Application.Abstractions;

/// <summary>Repository for the <see cref="LubricantType"/> aggregate.</summary>
public interface ILubricantTypeRepository
{
    /// <summary>Retrieves a Lubricant Type by its identifier, or <c>null</c> if it does not exist.</summary>
    Task<LubricantType?> GetByIdAsync(LubricantTypeId id, CancellationToken cancellationToken = default);

    /// <summary>Retrieves the list of Lubricant Type options defined for a Holding.</summary>
    Task<IReadOnlyList<LubricantTypeDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>Adds a new Lubricant Type.</summary>
    void Add(LubricantType lubricantType);

    /// <summary>Marks an existing Lubricant Type as updated.</summary>
    void Update(LubricantType lubricantType);

    /// <summary>Removes a Lubricant Type.</summary>
    void Remove(LubricantType lubricantType);
}
