using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Holdings.Commands.RenameHolding;

/// <summary>Validates <see cref="RenameHoldingCommand"/> per ADR-0036.</summary>
public sealed class RenameHoldingCommandValidator : AbstractValidator<RenameHoldingCommand>
{
    /// <summary>Initializes validation rules for the rename holding command.</summary>
    public RenameHoldingCommandValidator()
    {
        int maxLength = global::Organization.Domain.Holding.MaxNameLength;

        RuleFor(x => x.HoldingId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Holding name is required.")
            .MaximumLength(maxLength)
            .WithMessage($"Holding name shall not exceed {maxLength} characters.");
    }
}