using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.RenameOverflowComponent;

/// <summary>Validates <see cref="RenameOverflowComponentCommand"/>.</summary>
public sealed class RenameOverflowComponentCommandValidator : AbstractValidator<RenameOverflowComponentCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RenameOverflowComponentCommandValidator"/> class.</summary>
    public RenameOverflowComponentCommandValidator()
    {
        RuleFor(x => x.OverflowComponentId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.OverflowComponent.MaxNameLength);
    }
}
