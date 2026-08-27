using FluentValidation;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.ActivateAsset;

/// <summary>Validates <see cref="ActivateAssetCommand"/> per ADR-0036.</summary>
public sealed class ActivateAssetCommandValidator : AbstractValidator<ActivateAssetCommand>
{
    /// <summary>Initializes validation rules for the activate asset command.</summary>
    public ActivateAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
    }
}