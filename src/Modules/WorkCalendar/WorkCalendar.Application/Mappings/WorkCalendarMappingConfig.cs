using Mapster;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Mappings;

/// <summary>
/// Registers Mapster mapping configuration for the WorkCalendar module
/// (chat, 2026-09-06 — this module is the pilot for migrating the
/// project's DTO-mapping convention from manual construction to
/// Mapster; other modules still map manually and will be migrated
/// gradually).
/// </summary>
public sealed class WorkCalendarMappingConfig : IRegister
{
    /// <summary>Configures the type-mapping rules used by this module's Query handlers.</summary>
    /// <param name="config">The Mapster configuration to register mappings into.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<global::WorkCalendar.Domain.WorkPattern, WorkPatternDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.WeeklySchedule, src => src.WeeklySchedule);

        config.NewConfig<global::WorkCalendar.Domain.DayOverride, DayOverrideDto>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<global::WorkCalendar.Domain.DaySchedule, DayScheduleDto>();

        config.NewConfig<global::WorkCalendar.Domain.Shift, ShiftDto>();

        config.NewConfig<global::WorkCalendar.Domain.Break, BreakDto>();

        config.NewConfig<global::WorkCalendar.Domain.WorkCalendar, WorkCalendarDto>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<global::WorkCalendar.Domain.DayResolution, DayResolutionDto>();
    }
}