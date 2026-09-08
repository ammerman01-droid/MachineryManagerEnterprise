using FluentValidation;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.DisposeAsset;

/// <summary>Validates <see cref="DisposeAssetCommand"/> per ADR-0036.</summary>
public sealed class DisposeAssetCommandValidator : AbstractValidator<DisposeAssetCommand>
{
    /// <summary>Initializes validation rules for the dispose asset command.</summary>
    public DisposeAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
    }
}