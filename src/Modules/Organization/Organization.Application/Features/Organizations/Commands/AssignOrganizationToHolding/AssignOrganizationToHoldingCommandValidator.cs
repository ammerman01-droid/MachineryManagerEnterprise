using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.AssignOrganizationToHolding;

/// <summary>
/// Validates <see cref="AssignOrganizationToHoldingCommand"/> per ADR-0036.
/// </summary>
public sealed class AssignOrganizationToHoldingCommandValidator : AbstractValidator<AssignOrganizationToHoldingCommand>
{
    /// <summary>
    /// Initializes validation rules for the assign-to-holding command.
    /// </summary>
    public AssignOrganizationToHoldingCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.HoldingId).NotEmpty();
    }
}