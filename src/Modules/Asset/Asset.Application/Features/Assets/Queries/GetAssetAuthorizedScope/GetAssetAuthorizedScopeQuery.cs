using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Queries.GetAssetAuthorizedScope;

/// <summary>
/// Resolves the current user's authorized scope for the "Asset.View"
/// permission (chat, 2026-09-14) — used by the Presentation layer to
/// decide whether to show the full Holding→Organization navigation
/// (an unrestricted/Platform-level user) or restrict the Organization
/// picker to the user's own scope, mirroring the admin-vs-scoped
/// pattern already used on the Personnel list page.
/// </summary>
public sealed record GetAssetAuthorizedScopeQuery : IRequest<Result<AuthorizedScopeSet>>;