namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, narrowly-scoped WRITE access allowing the Maintenance
/// module to move an Asset between its Active and Out-Of-Service
/// statuses when a Work Order is registered or cancelled (chat,
/// 2026-09-22). Deliberately does not expose the full "move to ANY
/// status" capability — Ready and OutOfFleet are fleet-management
/// decisions outside a Work Order's concern, and remain reachable only
/// through Asset's own ChangeAssetStatus command. This is the
/// codebase's first cross-module WRITE contract; every prior
/// cross-module interface here (IProjectLookupService,
/// IOrganizationLookupService, ...) is read-only.
/// </summary>
public interface IAssetStatusUpdateService
{
    /// <summary>
    /// Moves the given Asset to Active or Out-Of-Service. Idempotent:
    /// succeeds without effect if the Asset is already in the requested
    /// status.
    /// </summary>
    /// <param name="assetId">The identifier of the Asset to update.</param>
    /// <param name="outOfService">
    /// <see langword="true"/> to move the Asset to Out-Of-Service;
    /// <see langword="false"/> to move it to Active.
    /// </param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A successful <see cref="Result"/>, or a not-found error if the Asset does not exist.</returns>
    Task<Result> SetOperationalStatusAsync(Guid assetId, bool outOfService, CancellationToken cancellationToken = default);
}
