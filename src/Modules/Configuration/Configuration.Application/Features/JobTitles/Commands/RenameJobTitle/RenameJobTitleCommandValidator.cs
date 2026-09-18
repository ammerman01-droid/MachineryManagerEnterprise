using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.RenameJobTitle;

/// <summary>Validates <see cref="RenameJobTitleCommand"/>.</summary>
public sealed class RenameJobTitleCommandValidator : AbstractValidator<RenameJobTitleCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RenameJobTitleCommandValidator"/> class.</summary>
    public RenameJobTitleCommandValidator()
    {
        RuleFor(x => x.JobTitleId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.JobTitle.MaxNameLength);
    }
}