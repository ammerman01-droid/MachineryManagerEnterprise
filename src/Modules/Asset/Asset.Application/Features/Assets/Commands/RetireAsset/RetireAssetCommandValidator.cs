using FluentValidation;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.RetireAsset;

/// <summary>Validates <see cref="RetireAssetCommand"/> per ADR-0036.</summary>
public sealed class RetireAssetCommandValidator : AbstractValidator<RetireAssetCommand>
{
    /// <summary>Initializes validation rules for the retire asset command.</summary>
    public RetireAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
    }
}