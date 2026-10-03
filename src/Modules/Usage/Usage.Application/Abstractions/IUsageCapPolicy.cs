using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Usage.Application.Abstractions;

/// <summary>
/// Resolves the system-wide default daily cap on Operational Usage
/// (BR-045) for a given reading unit. A specific
/// <see cref="global::Usage.Domain.MeterDevice"/> may override this
/// via its own <c>DailyCapOverride</c>; this policy supplies the
/// fallback when no override is set. New abstraction (chat,
/// 2026-09-12) — Usage.Infrastructure is expected to back it with a
/// small, Usage-module-owned settings store rather than borrowing the
/// general Configuration module, since these defaults are specific to
/// this module's own business rule.
/// </summary>
public interface IUsageCapPolicy
{
    /// <summary>Resolves the current system-wide default daily cap for the given unit.</summary>
    /// <param name="unit">The reading unit (Hour, Kilometer, or Mile).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The default daily cap.</returns>
    Task<decimal> GetDefaultDailyCapAsync(MeterReadingUnit unit, CancellationToken cancellationToken = default);
}
