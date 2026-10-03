using FluentValidation;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.RegisterAsset;

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

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.Asset.MaxNameLength);

        RuleFor(x => x.AssetModelId).NotEmpty();
        RuleFor(x => x.ColorId).NotEmpty();

        RuleFor(x => x.SerialNumber)
            .MaximumLength(global::Asset.Domain.Asset.MaxSerialNumberLength);

        RuleFor(x => x.ChassisNumber)
            .MaximumLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);

        RuleFor(x => x.BodyNumber)
            .MaximumLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);

        RuleFor(x => x.Vin)
            .MaximumLength(global::Asset.Domain.Asset.MaxChassisBodyVinLength);

        RuleFor(x => x.LicensePlate)
            .MaximumLength(global::Asset.Domain.Asset.MaxLicensePlateLength);

        RuleFor(x => x.PrimaryFuelKind)
            .IsInEnum()
            .When(x => x.PrimaryFuelKind.HasValue);

        RuleFor(x => x.SecondaryFuelKind)
            .IsInEnum()
            .When(x => x.SecondaryFuelKind.HasValue);

        // Required as of chat, 2026-09-29 (previously optional, chat 2026-09-19) —
        // the Usage module's MeterDevice.Unit is fixed for a device's
        // lifetime, and auto-provisioning a device for a new Asset needs
        // a unit to register it with.
        RuleFor(x => x.MeterReadingUnit).IsInEnum();

        RuleFor(x => x.PrimaryFuelUnit)
            .IsInEnum()
            .When(x => x.PrimaryFuelUnit.HasValue);

        RuleFor(x => x.SecondaryFuelUnit)
            .IsInEnum()
            .When(x => x.SecondaryFuelUnit.HasValue);

        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
