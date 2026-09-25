using FluentValidation;
using MachineryManagerEnterprise.Maintenance.Domain;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.EditWorkOrder;

/// <summary>Validates <see cref="EditWorkOrderCommand"/> per ADR-0036.</summary>
public sealed class EditWorkOrderCommandValidator : AbstractValidator<EditWorkOrderCommand>
{
    /// <summary>Initializes validation rules for the edit Work Order command.</summary>
    public EditWorkOrderCommandValidator()
    {
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.ReportedAt).NotEmpty();
        RuleFor(x => x.ResultingAssetStatus).IsInEnum();
        RuleFor(x => x.MeterReading).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Priority).IsInEnum();
        RuleFor(x => x.RepairType).IsInEnum();
        RuleFor(x => x.ResponsiblePersonnelId).NotEmpty();

        RuleFor(x => x.ObservationDescription)
            .NotEmpty()
            .MaximumLength(WorkOrder.MaxObservationDescriptionLength);

        RuleFor(x => x.PredictedRepairLocation).IsInEnum();
        RuleFor(x => x.PartNeedingRepair).IsInEnum();
    }
}
