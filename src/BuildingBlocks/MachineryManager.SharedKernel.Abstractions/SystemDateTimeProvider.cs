using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.SharedKernel.Infrastructure;

/// <summary>
/// Default <see cref="IDateTimeProvider"/> implementation backed by the
/// system clock. Registered for every module so business code never
/// calls <see cref="DateTimeOffset.UtcNow"/> directly (06-development
/// Coding Standards).
/// </summary>
public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
