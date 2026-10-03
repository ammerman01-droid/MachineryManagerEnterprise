using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetDaySchedule;

/// <summary>Handles <see cref="GetDayScheduleQuery"/> by resolving the device's owning Asset's Project and delegating to the Work Calendar lookup.</summary>
public sealed class GetDayScheduleQueryHandler : IRequestHandler<GetDayScheduleQuery, Result<DayScheduleDto>>
{
    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IWorkCalendarLookupService _workCalendarLookupService;

    /// <summary>Initializes a new instance of the <see cref="GetDayScheduleQueryHandler"/> class.</summary>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to resolve the owning Asset's current Project.</param>
    /// <param name="workCalendarLookupService">Cross-module, read-only lookup into the WorkCalendar module.</param>
    public GetDayScheduleQueryHandler(
        IMeterDeviceRepository meterDeviceRepository,
        IAssetLookupService assetLookupService,
        IWorkCalendarLookupService workCalendarLookupService)
    {
        _meterDeviceRepository = meterDeviceRepository;
        _assetLookupService = assetLookupService;
        _workCalendarLookupService = workCalendarLookupService;
    }

    /// <summary>Executes the day schedule lookup.</summary>
    public async Task<Result<DayScheduleDto>> Handle(GetDayScheduleQuery request, CancellationToken cancellationToken)
    {
        var deviceId = global::Usage.Domain.MeterDeviceId.From(request.MeterDeviceId);
        var device = await _meterDeviceRepository.GetByIdAsync(deviceId, cancellationToken);

        if (device is null)
        {
            return Result.Failure<DayScheduleDto>(Error.NotFound(
                "MeterDevice.NotFound", $"Meter Device {request.MeterDeviceId} was not found."));
        }

        if (device.OwnerType is not global::Usage.Domain.UsageOwnerType.Asset || device.OwnerId is not { } ownerAssetId)
        {
            return Result.Failure<DayScheduleDto>(Error.Failure(
                "MeterDevice.OwnerNotSupported",
                "This device is not installed on an Asset; its Work Calendar cannot be resolved."));
        }

        var assetContext = await _assetLookupService.GetUsageContextAsync(ownerAssetId, cancellationToken);

        if (assetContext is null)
        {
            return Result.Failure<DayScheduleDto>(Error.NotFound(
                "Asset.NotFound", $"Asset {ownerAssetId} was not found."));
        }

        var schedule = await _workCalendarLookupService.ResolveDayScheduleAsync(assetContext.ProjectId, request.Date, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure<DayScheduleDto>(Error.Validation(
                "Usage.CalendarNotResolved",
                $"No Work Calendar schedule could be resolved for {request.Date:yyyy-MM-dd}."));
        }

        var shifts = schedule.Shifts
            .OrderBy(s => s.Index)
            .Select(s => new ShiftDto(s.Index, s.StartTime, s.EndTime))
            .ToList();

        return new DayScheduleDto(schedule.IsWorkingDay, shifts);
    }
}
