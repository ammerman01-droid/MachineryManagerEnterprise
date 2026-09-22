using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.UpdateColor;

/// <summary>Validates <see cref="UpdateColorCommand"/>.</summary>
public sealed class UpdateColorCommandValidator : AbstractValidator<UpdateColorCommand>
{
    /// <summary>Initializes a new instance of the <see cref="UpdateColorCommandValidator"/> class.</summary>
    public UpdateColorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.Color.MaxNameLength);
    }
}
