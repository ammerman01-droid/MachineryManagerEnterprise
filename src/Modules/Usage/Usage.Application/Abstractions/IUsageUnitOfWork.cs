using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Usage.Application.Abstractions;

/// <summary>
/// Unit of work for the Usage module. Distinct from every other
/// module's UoW so that each module commits its own aggregates
/// (ADR-0001, Modular Monolith Rules) — mirrors <c>IAssetUnitOfWork</c>.
/// Both <see cref="global::Usage.Domain.MeterDevice"/> and
/// <see cref="global::Usage.Domain.UsageLedger"/> share this single
/// unit of work, since they belong to the same module and the same
/// <c>RegisterDailyUsage</c> use case commits both together in one
/// transaction (chat, 2026-09-12).
/// </summary>
public interface IUsageUnitOfWork : IUnitOfWork
{
}
