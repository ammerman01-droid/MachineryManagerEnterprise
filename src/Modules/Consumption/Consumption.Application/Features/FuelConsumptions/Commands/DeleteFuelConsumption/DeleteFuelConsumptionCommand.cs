using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.DeleteFuelConsumption;

/// <summary>
/// Hard-deletes a fuel-consumption record (chat, 2026-09-15 — direct
/// delete is allowed, unlike Meter Reading elsewhere in the spec). No
/// re-validation of the resulting neighbor pair is performed (chat,
/// 2026-09-16 — checks happen only at the moment of Create/Edit/Delete
/// for the record itself, not a full chain re-validation).
/// </summary>
public sealed record DeleteFuelConsumptionCommand(Guid Id) : IRequest<Result>;
