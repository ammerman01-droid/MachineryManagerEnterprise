using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Configuration.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Configuration.Domain.DrivingLicenseType"/> aggregate.</summary>
public interface IDrivingLicenseTypeRepository
    : IRepository<global::Configuration.Domain.DrivingLicenseType, global::Configuration.Domain.DrivingLicenseTypeId>
{
    /// <summary>Retrieves every Driving License Type registered for the given Holding.</summary>
    Task<IReadOnlyList<Features.DrivingLicenseTypes.Dtos.DrivingLicenseTypeDto>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default);
}