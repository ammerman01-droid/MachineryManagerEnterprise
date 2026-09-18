using MachineryManagerEnterprise.SharedKernel;

namespace WorkCalendar.Domain;

/// <summary>Business Errors for the WorkCalendar aggregate and its child entities.</summary>
public static class WorkCalendarErrors
{
    /// <summary>Creates an error indicating the calendar name was not provided.</summary>
    public static Error NameRequired() => Error.Validation(
        "WorkCalendar.NameRequired", "Work calendar name is required.");

    /// <summary>Creates an error indicating the calendar name exceeds the maximum length.</summary>
    public static Error NameTooLong(int maxLength) => Error.Validation(
        "WorkCalendar.NameTooLong", $"Work calendar name shall not exceed {maxLength} characters.");

    /// <summary>Creates an error indicating the current user lacks permission for this action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "WorkCalendar.NotAuthorized", "You do not have permission to perform this action.");

    /// <summary>Creates an error indicating the given ProjectId does not correspond to an existing Project.</summary>
    public static Error ProjectNotFound(Guid projectId) => Error.NotFound(
        "WorkCalendar.ProjectNotFound", $"Project with id {projectId} was not found.");

    /// <summary>Creates an error indicating a new Work Pattern's date range overlaps an existing one.</summary>
    public static Error WorkPatternOverlap() => Error.Conflict(
        "WorkCalendar.WorkPatternOverlap", "This date range overlaps an existing Work Pattern.");

    /// <summary>Creates an error indicating the given Work Pattern was not found on this calendar.</summary>
    public static Error WorkPatternNotFound(Guid workPatternId) => Error.NotFound(
        "WorkCalendar.WorkPatternNotFound", $"Work pattern with id {workPatternId} was not found on this calendar.");

    /// <summary>Creates an error indicating a Work Pattern cannot be removed because it is not in Draft.</summary>
    public static Error WorkPatternNotRemovable() => Error.Conflict(
        "WorkCalendar.WorkPatternNotRemovable", "Only a Work Pattern still in Draft may be removed.");

    /// <summary>Creates an error indicating a Work Pattern cannot be activated because it is not in Draft.</summary>
    public static Error WorkPatternNotDraft() => Error.Conflict(
        "WorkCalendar.WorkPatternNotDraft", "Only a Work Pattern in Draft may be activated.");

    /// <summary>Creates an error indicating a Work Pattern's end date precedes its start date.</summary>
    public static Error WorkPatternEndBeforeStart() => Error.Validation(
        "WorkCalendar.WorkPatternEndBeforeStart", "A Work Pattern's end date must not precede its start date.");

    /// <summary>Creates an error indicating a Work Pattern's weekly schedule is missing one or more days of the week.</summary>
    public static Error WorkPatternMissingDayOfWeek() => Error.Validation(
        "WorkCalendar.WorkPatternMissingDayOfWeek", "A Work Pattern's weekly schedule must specify all seven days of the week.");

    /// <summary>Creates an error indicating another non-cancelled Day Override already exists for this date.</summary>
    public static Error DayOverrideConflict() => Error.Conflict(
        "WorkCalendar.DayOverrideConflict", "An active Day Override already exists for this date.");

    /// <summary>Creates an error indicating the given Day Override was not found on this calendar.</summary>
    public static Error DayOverrideNotFound(Guid dayOverrideId) => Error.NotFound(
        "WorkCalendar.DayOverrideNotFound", $"Day override with id {dayOverrideId} was not found on this calendar.");

    /// <summary>Creates an error indicating the Day Override is already cancelled.</summary>
    public static Error DayOverrideAlreadyCancelled() => Error.Conflict(
        "WorkCalendar.DayOverrideAlreadyCancelled", "This day override has already been cancelled.");

    /// <summary>Creates an error indicating a Holiday override was given a schedule, which is not permitted.</summary>
    public static Error HolidayOverrideMustNotHaveSchedule() => Error.Validation(
        "WorkCalendar.HolidayOverrideMustNotHaveSchedule", "A Holiday day override must not have a custom schedule.");

    /// <summary>Creates an error indicating a non-Holiday override was not given a required schedule.</summary>
    public static Error NonHolidayOverrideRequiresSchedule() => Error.Validation(
        "WorkCalendar.NonHolidayOverrideRequiresSchedule", "A Working or Modified day override requires a custom schedule.");

    /// <summary>Creates an error indicating a day's shifts overlap one another.</summary>
    public static Error OverlappingShifts() => Error.Validation(
        "WorkCalendar.OverlappingShifts", "Shifts within a single day's schedule must not overlap.");

    /// <summary>Creates an error indicating a day schedule was given no shifts.</summary>
    public static Error DayScheduleRequiresAtLeastOneShift() => Error.Validation(
        "WorkCalendar.DayScheduleRequiresAtLeastOneShift", "A working day schedule requires at least one shift.");

    /// <summary>Creates an error indicating a shift's end time precedes its start time.</summary>
    public static Error ShiftEndBeforeStart() => Error.Validation(
        "WorkCalendar.ShiftEndBeforeStart", "A shift's end time must be after its start time.");

    /// <summary>Creates an error indicating a break falls outside its shift's time range.</summary>
    public static Error BreakOutsideShift() => Error.Validation(
        "WorkCalendar.BreakOutsideShift", "A break must fall entirely within its shift's time range.");

    /// <summary>Creates an error indicating two breaks within a shift overlap.</summary>
    public static Error OverlappingBreaks() => Error.Validation(
        "WorkCalendar.OverlappingBreaks", "Breaks within a single shift must not overlap.");

    /// <summary>Creates an error indicating a break's end time precedes its start time.</summary>
    public static Error BreakEndBeforeStart() => Error.Validation(
        "WorkCalendar.BreakEndBeforeStart", "A break's end time must be after its start time.");

    /// <summary>Creates an error indicating a Work Pattern cannot be superseded because it is not currently Active.</summary>
    public static Error WorkPatternNotActive() => Error.Conflict(
        "WorkCalendar.WorkPatternNotActive", "Only a Work Pattern currently Active may be superseded.");

    /// <summary>Creates an error indicating a Work Pattern cannot be marked Historical because it is not currently Superseded.</summary>
    public static Error WorkPatternNotSuperseded() => Error.Conflict(
        "WorkCalendar.WorkPatternNotSuperseded", "Only a Work Pattern currently Superseded may be marked Historical.");

    /// <summary>Only an Active pattern may be manually retired.</summary>
    public static Error WorkPatternNotRetirable() =>
        Error.Conflict("WorkCalendar.WorkPatternNotRetirable", "Only an Active Work Pattern can be retired.");
}