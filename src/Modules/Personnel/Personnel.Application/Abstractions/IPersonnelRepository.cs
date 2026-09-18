using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Personnel.Application.Abstractions;

/// <summary>Repository contract for the Personnel aggregate.</summary>
public interface IPersonnelRepository
    : IRepository<global::MachineryManagerEnterprise.Personnel.Domain.Personnel, global::MachineryManagerEnterprise.Personnel.Domain.PersonnelId>
{
    /// <summary>Retrieves every Personnel record owned by the given Organization.</summary>
    Task<IReadOnlyList<Features.Dtos.PersonnelDto>> GetByOrganizationAsync(
        Guid organizationId, CancellationToken cancellationToken = default);

    /// <summary>Determines whether a Personnel code is already registered within the given Organization.</summary>
    Task<bool> ExistsByCodeInOrganizationAsync(
        Guid organizationId, string personnelCode, CancellationToken cancellationToken = default);

    /// <summary>Determines whether a Personnel code is already registered within the given Organization, excluding a specific Personnel record (for edit scenarios).</summary>
    Task<bool> ExistsByCodeInOrganizationAsync(
        Guid organizationId, string personnelCode, Guid excludingPersonnelId, CancellationToken cancellationToken = default);

}