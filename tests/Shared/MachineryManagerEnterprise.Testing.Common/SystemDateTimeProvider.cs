using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Testing.Common;

/// <summary>
/// A plain, real <see cref="IDateTimeProvider"/> for integration/endpoint
/// test hosts that need a working implementation registered in DI (e.g.
/// because <c>AuditSaveChangesInterceptor</c> requires one) but don't
/// care about controlling the exact instant — unlike unit tests, which
/// should keep using <c>NSubstitute</c> to pin an exact value.
/// </summary>
public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
