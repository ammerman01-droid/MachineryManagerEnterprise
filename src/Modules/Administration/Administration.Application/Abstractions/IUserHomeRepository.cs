namespace MachineryManagerEnterprise.Administration.Application.Abstractions;

/// <summary>Persistence port for <see cref="global::Administration.Domain.UserHome"/>.</summary>
public interface IUserHomeRepository
{
    /// <summary>Finds a user's home; <see langword="null"/> means a Platform user.</summary>
    /// <param name="userId">The user.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The home, or <see langword="null"/>.</returns>
    Task<global::Administration.Domain.UserHome?> GetAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Stages a new home for insertion.</summary>
    /// <param name="home">The home to add.</param>
    void Add(global::Administration.Domain.UserHome home);

    /// <summary>
    /// Returns the users whose home lies under an Organization (when given) or otherwise under a Holding.
    /// </summary>
    /// <param name="holdingId">The Holding.</param>
    /// <param name="organizationId">The Organization; takes precedence over <paramref name="holdingId"/>.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The user identifiers (empty when neither id is given).</returns>
    Task<IReadOnlyList<Guid>> GetUserIdsAsync(
        Guid? holdingId,
        Guid? organizationId,
        CancellationToken cancellationToken = default);
}
