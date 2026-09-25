using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.RegisterLubricantType;

/// <summary>Validates <see cref="RegisterLubricantTypeCommand"/>.</summary>
public sealed class RegisterLubricantTypeCommandValidator : AbstractValidator<RegisterLubricantTypeCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterLubricantTypeCommandValidator"/> class.</summary>
    public RegisterLubricantTypeCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.LubricantType.MaxNameLength);
    }
}
