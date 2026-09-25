namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup into the Personnel module (chat,
/// 2026-09-16), used by other modules — currently Consumption, for its
/// fuel-deliverer/fuel-receiver references — that need to validate a
/// Personnel reference without depending on Personnel.Domain/
/// Personnel.Application directly (Modular Monolith boundary — same
/// pattern as <see cref="IOrganizationLookupService"/>).
/// </summary>
public interface IPersonnelLookupService
{
    /// <summary>Determines whether a Personnel with the given identifier exists.</summary>
    Task<bool> ExistsAsync(Guid personnelId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the identifier of the Organization a Personnel belongs
    /// to, or <see langword="null"/> if the Personnel does not exist.
    /// </summary>
    Task<Guid?> GetOrganizationIdAsync(Guid personnelId, CancellationToken cancellationToken = default);

    /// <summary>Gets the Personnel's full display name (first and last name), for display purposes in other modules.</summary>
    /// <param name="personnelId">The Personnel's identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The Personnel's full name, or <see langword="null"/> if the Personnel record does not exist.</returns>
    Task<string?> GetFullNameAsync(Guid personnelId, CancellationToken cancellationToken = default);
}
