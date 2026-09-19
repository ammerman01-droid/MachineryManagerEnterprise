using FluentValidation;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.UpdateAsset;

/// <summary>Validates <see cref="UpdateAssetCommand"/>.</summary>
public sealed class UpdateAssetCommandValidator : AbstractValidator<UpdateAssetCommand>
{
    /// <summary>Initializes validation rules for the update asset command.</summary>
    public UpdateAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.Asset.MaxCodeLength);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.Asset.MaxNameLength);

        RuleFor(x => x.AssetModelId).NotEmpty();
        RuleFor(x => x.ColorId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();

        RuleFor(x => x.SerialNumber).MaximumLength(global::Asset.Domain.Asset.MaxSerialNumberLength);
        RuleFor(x => x.ChassisNumber).MaximumLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);
        RuleFor(x => x.BodyNumber).MaximumLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);
        RuleFor(x => x.Vin).MaximumLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);
        RuleFor(x => x.LicensePlate).MaximumLength(global::Asset.Domain.Asset.MaxLicensePlateLength);

        RuleFor(x => x.PrimaryFuelKind).IsInEnum().When(x => x.PrimaryFuelKind.HasValue);
        RuleFor(x => x.SecondaryFuelKind).IsInEnum().When(x => x.SecondaryFuelKind.HasValue);

        // Units are fixed enums (chat, 2026-09-19).
        RuleFor(x => x.MeterReadingUnit).IsInEnum().When(x => x.MeterReadingUnit.HasValue);
        RuleFor(x => x.PrimaryFuelUnit).IsInEnum().When(x => x.PrimaryFuelUnit.HasValue);
        RuleFor(x => x.SecondaryFuelUnit).IsInEnum().When(x => x.SecondaryFuelUnit.HasValue);
    }
}
