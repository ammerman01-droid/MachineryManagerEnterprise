using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.RegisterJobTitle;

/// <summary>Validates <see cref="RegisterJobTitleCommand"/>.</summary>
public sealed class RegisterJobTitleCommandValidator : AbstractValidator<RegisterJobTitleCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterJobTitleCommandValidator"/> class.</summary>
    public RegisterJobTitleCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.JobTitle.MaxNameLength);
    }
}