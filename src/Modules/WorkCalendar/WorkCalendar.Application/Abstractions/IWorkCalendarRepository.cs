using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::WorkCalendar.Domain.WorkCalendar"/> aggregate.</summary>
public interface IWorkCalendarRepository
    : IRepository<global::WorkCalendar.Domain.WorkCalendar, global::WorkCalendar.Domain.WorkCalendarId>
{
    /// <summary>
    /// Finds the Work Calendar owned by the given Project, if one has
    /// been registered (BR-018-001: at most one per Project). Added
    /// (chat, 2026-09-13) to back <c>GetWorkCalendarByProjectIdQuery</c>.
    /// </summary>
    /// <param name="projectId">The owning Project's identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The Work Calendar for <paramref name="projectId"/>, or <see langword="null"/> if none exists.</returns>
    Task<global::WorkCalendar.Domain.WorkCalendar?> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}
