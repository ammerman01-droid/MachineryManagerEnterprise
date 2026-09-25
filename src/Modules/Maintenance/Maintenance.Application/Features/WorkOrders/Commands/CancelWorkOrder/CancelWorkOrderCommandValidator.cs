using FluentValidation;
using MachineryManagerEnterprise.Maintenance.Domain;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.CancelWorkOrder;

/// <summary>Validates <see cref="CancelWorkOrderCommand"/> per ADR-0036.</summary>
public sealed class CancelWorkOrderCommandValidator : AbstractValidator<CancelWorkOrderCommand>
{
    /// <summary>Initializes validation rules for the cancel Work Order command.</summary>
    public CancelWorkOrderCommandValidator()
    {
        RuleFor(x => x.WorkOrderId).NotEmpty();

        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(WorkOrder.MaxCancellationReasonLength);
    }
}
