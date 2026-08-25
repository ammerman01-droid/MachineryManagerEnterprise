using MachineryManager.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Identity.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Identity platform module
/// (ADR-0006, ADR-0030). Owns the "identity" schema exclusively; per
/// the Modular Monolith Rules (06-development, Section 6.1) no other
/// module may reference these tables directly.
/// </summary>
/// <remarks>
/// The base type is fully qualified (rather than imported via
/// <c>using</c>) to avoid visual ambiguity between this type's name
/// and its generic base class, which share the same simple name.
/// Also hosts the OpenIddict EF Core stores (Applications,
/// Authorizations, Scopes, Tokens) in the same schema, since both
/// concerns belong to the same Identity platform module (ADR-0030).
/// </remarks>
public sealed class IdentityDbContext
    : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    /// <summary>Initializes a new instance of the <see cref="IdentityDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");

        base.OnModelCreating(modelBuilder);

        // OpenIddict entities keyed by Guid, consistent with
        // ApplicationUser/ApplicationRole (ADR-0030).
        modelBuilder.UseOpenIddict<Guid>();
    }
}