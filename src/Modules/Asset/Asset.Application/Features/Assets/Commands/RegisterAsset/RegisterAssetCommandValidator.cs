using FluentValidation;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.RegisterAsset;

/// <summary>Validates <see cref="RegisterAssetCommand"/> per ADR-0036.</summary>
public sealed class RegisterAssetCommandValidator : AbstractValidator<RegisterAssetCommand>
{
    /// <summary>Initializes validation rules for the register asset command.</summary>
    public RegisterAssetCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.Asset.MaxCodeLength);

        RuleFor(x => x.AssetModelId).NotEmpty();

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.Asset.MaxColorLength);

        RuleFor(x => x.SerialNumber)
            .MaximumLength(global::Asset.Domain.Asset.MaxSerialNumberLength);

        RuleFor(x => x.LicensePlate)
            .MaximumLength(global::Asset.Domain.Asset.MaxLicensePlateLength);
    }
}