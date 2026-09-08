using FluentValidation;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RenameEngineModel;

/// <summary>Validates <see cref="RenameEngineModelCommand"/> per ADR-0036.</summary>
public sealed class RenameEngineModelCommandValidator : AbstractValidator<RenameEngineModelCommand>
{
    /// <summary>Initializes validation rules for the rename engine model command.</summary>
    public RenameEngineModelCommandValidator()
    {
        RuleFor(x => x.EngineModelId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.EngineModel.MaxNameLength);
    }
}