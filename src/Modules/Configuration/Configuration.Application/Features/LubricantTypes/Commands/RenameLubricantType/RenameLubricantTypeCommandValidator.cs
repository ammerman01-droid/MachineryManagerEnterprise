using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.RenameLubricantType;

/// <summary>Validates <see cref="RenameLubricantTypeCommand"/>.</summary>
public sealed class RenameLubricantTypeCommandValidator : AbstractValidator<RenameLubricantTypeCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RenameLubricantTypeCommandValidator"/> class.</summary>
    public RenameLubricantTypeCommandValidator()
    {
        RuleFor(x => x.LubricantTypeId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.LubricantType.MaxNameLength);
    }
}
