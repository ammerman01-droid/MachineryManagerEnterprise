using Administration.Domain;
using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Administration.Application.Abstractions;

/// <summary>Resolves an <see cref="AuthorizationScope"/> to its full ancestor chain, verifying that it exists.</summary>
public interface IScopeChainResolver
{
    /// <summary>Resolves the chain of <paramref name="scope"/>.</summary>
    /// <param name="scope">The scope (level plus the id of its own node).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The chain, or a validation/NotFound error when the node does not exist.</returns>
    Task<Result<ScopeChain>> ResolveAsync(AuthorizationScope scope, CancellationToken cancellationToken = default);
}
