using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Configuration.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Configuration.Domain.JobTitle"/> aggregate.</summary>
public interface IJobTitleRepository : IRepository<global::Configuration.Domain.JobTitle, global::Configuration.Domain.JobTitleId>
{
    /// <summary>Retrieves every Job Title registered for the given Holding.</summary>
    Task<IReadOnlyList<Features.JobTitles.Dtos.JobTitleDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default);
}