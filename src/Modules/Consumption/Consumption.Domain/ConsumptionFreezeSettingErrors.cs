using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain;

/// <summary>Business Errors for the <see cref="ConsumptionFreezeSetting"/> aggregate.</summary>
public static class ConsumptionFreezeSettingErrors
{
    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure("ConsumptionFreezeSetting.NotAuthorized", "Only an Organization Administrator may change the consumption freeze threshold.");

    /// <summary>Creates an error indicating no freeze threshold has been set yet for the organization.</summary>
    public static Error NotFound() => Error.NotFound("ConsumptionFreezeSetting.NotFound", "No consumption freeze threshold has been set for this organization yet.");
}
