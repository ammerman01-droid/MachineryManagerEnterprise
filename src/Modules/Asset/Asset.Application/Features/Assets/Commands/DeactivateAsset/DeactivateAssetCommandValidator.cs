using FluentValidation;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.DeactivateAsset;

/// <summary>Validates <see cref="DeactivateAssetCommand"/> per ADR-0036.</summary>
public sealed class DeactivateAssetCommandValidator : AbstractValidator<DeactivateAssetCommand>
{
    /// <summary>Initializes validation rules for the deactivate asset command.</summary>
    public DeactivateAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
    }
}