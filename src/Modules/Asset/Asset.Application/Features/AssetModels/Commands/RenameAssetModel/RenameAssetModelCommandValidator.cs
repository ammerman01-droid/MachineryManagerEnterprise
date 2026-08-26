using FluentValidation;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RenameAssetModel;

/// <summary>Validates <see cref="RenameAssetModelCommand"/> per ADR-0036.</summary>
public sealed class RenameAssetModelCommandValidator : AbstractValidator<RenameAssetModelCommand>
{
    /// <summary>Initializes validation rules for the rename asset model command.</summary>
    public RenameAssetModelCommandValidator()
    {
        RuleFor(x => x.AssetModelId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.AssetModel.MaxNameLength);
    }
}