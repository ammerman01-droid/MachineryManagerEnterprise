using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MachineryManagerEnterprise.WorkCalendar.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for the <see cref="global::WorkCalendar.Domain.WorkCalendar"/> aggregate.</summary>
public sealed class WorkCalendarConfiguration : IEntityTypeConfiguration<global::WorkCalendar.Domain.WorkCalendar>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<global::WorkCalendar.Domain.WorkCalendar> builder)
    {
        builder.ToTable("WorkCalendar");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => global::WorkCalendar.Domain.WorkCalendarId.From(value))
            .ValueGeneratedNever();

        builder.Property(c => c.ProjectId)
            .IsRequired();

        // BR-018-001: exactly one Work Calendar per Project.
        builder.HasIndex(c => c.ProjectId).IsUnique();

        builder.Property(c => c.Name)
            .HasMaxLength(global::WorkCalendar.Domain.WorkCalendar.MaxNameLength)
            .IsRequired();

        builder.OwnsMany(c => c.WorkPatterns, patternBuilder =>
        {
            patternBuilder.ToTable("WorkPattern");

            patternBuilder.WithOwner().HasForeignKey("WorkCalendarId");

            patternBuilder.HasKey(p => p.Id);

            patternBuilder.Property(p => p.Id)
                .HasConversion(id => id.Value, value => global::WorkCalendar.Domain.WorkPatternId.From(value))
                .ValueGeneratedNever();

            patternBuilder.Property(p => p.StartDate).IsRequired();
            patternBuilder.Property(p => p.EndDate).IsRequired();

            patternBuilder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            patternBuilder.Ignore(p => p.WeeklySchedule);

            // Fix (chat, 2026-09-14): serializing/deserializing the
            // domain DaySchedule/Shift/Break objects directly failed on
            // read — they have no parameterless constructor by design
            // (only static Create(...) factories, to keep invariants
            // enforced). Now round-trips through the plain JSON-shaped
            // WeeklyScheduleJson model below and rebuilds the domain
            // objects via their own Create/DayOff factories. Property
            // names are unchanged, so already-stored rows still read
            // correctly — no data migration needed.
            patternBuilder.Property<Dictionary<DayOfWeek, global::WorkCalendar.Domain.DaySchedule>>("_weeklySchedule")
                .HasColumnName("WeeklySchedule")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                    schedule => JsonSerializer.Serialize(WeeklyScheduleJson.ToJson(schedule), JsonOptions),
                    json => WeeklyScheduleJson.FromJson(JsonSerializer.Deserialize<Dictionary<DayOfWeek, DayScheduleJson>>(json, JsonOptions)!));
        });

        builder.OwnsMany(c => c.DayOverrides, overrideBuilder =>
        {
            overrideBuilder.ToTable("DayOverride");

            overrideBuilder.WithOwner().HasForeignKey("WorkCalendarId");

            overrideBuilder.HasKey(o => o.Id);

            overrideBuilder.Property(o => o.Id)
                .HasConversion(id => id.Value, value => global::WorkCalendar.Domain.DayOverrideId.From(value))
                .ValueGeneratedNever();

            overrideBuilder.Property(o => o.Date).IsRequired();

            overrideBuilder.Property(o => o.Type)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            overrideBuilder.Property(o => o.IsCancelled).IsRequired();

            overrideBuilder.Property(o => o.CustomSchedule)
                .HasColumnName("CustomSchedule")
                .HasConversion(
                    schedule => schedule == null ? null : JsonSerializer.Serialize(DayScheduleJson.FromDomain(schedule), JsonOptions),
                    json => json == null ? null : DayScheduleJson.ToDomain(JsonSerializer.Deserialize<DayScheduleJson>(json, JsonOptions)!));
        });
    }

    private static readonly JsonSerializerOptions JsonOptions = new();

    /// <summary>Plain, JSON-friendly shape of a <see cref="global::WorkCalendar.Domain.Break"/> (parameterless-constructible, unlike the domain type).</summary>
    private sealed class BreakJson
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    /// <summary>Plain, JSON-friendly shape of a <see cref="global::WorkCalendar.Domain.Shift"/>.</summary>
    private sealed class ShiftJson
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public List<BreakJson> Breaks { get; set; } = [];
    }

    /// <summary>Plain, JSON-friendly shape of a <see cref="global::WorkCalendar.Domain.DaySchedule"/>.</summary>
    private sealed class DayScheduleJson
    {
        public bool IsWorkingDay { get; set; }
        public List<ShiftJson> Shifts { get; set; } = [];

        /// <summary>Converts a domain <see cref="global::WorkCalendar.Domain.DaySchedule"/> to its JSON-friendly shape.</summary>
        public static DayScheduleJson FromDomain(global::WorkCalendar.Domain.DaySchedule schedule) => new()
        {
            IsWorkingDay = schedule.IsWorkingDay,
            Shifts = schedule.Shifts
                .Select(s => new ShiftJson
                {
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Breaks = s.Breaks.Select(b => new BreakJson { StartTime = b.StartTime, EndTime = b.EndTime }).ToList(),
                })
                .ToList(),
        };

        /// <summary>Rebuilds the domain <see cref="global::WorkCalendar.Domain.DaySchedule"/> via its own factories, re-validating on the way.</summary>
        public static global::WorkCalendar.Domain.DaySchedule ToDomain(DayScheduleJson json)
        {
            if (!json.IsWorkingDay || json.Shifts.Count == 0)
            {
                return global::WorkCalendar.Domain.DaySchedule.DayOff();
            }

            var shifts = json.Shifts.Select(s =>
            {
                var breaks = s.Breaks.Select(b => global::WorkCalendar.Domain.Break.Create(b.StartTime, b.EndTime).Value).ToList();
                return global::WorkCalendar.Domain.Shift.Create(s.StartTime, s.EndTime, breaks).Value;
            }).ToList();

            return global::WorkCalendar.Domain.DaySchedule.Create(shifts).Value;
        }
    }

    /// <summary>Converts the 7-day weekly template between its domain and JSON-friendly shapes.</summary>
    private static class WeeklyScheduleJson
    {
        /// <summary>Converts the domain weekly schedule dictionary to its JSON-friendly form.</summary>
        public static Dictionary<DayOfWeek, DayScheduleJson> ToJson(Dictionary<DayOfWeek, global::WorkCalendar.Domain.DaySchedule> schedule) =>
            schedule.ToDictionary(kvp => kvp.Key, kvp => DayScheduleJson.FromDomain(kvp.Value));

        /// <summary>Rebuilds the domain weekly schedule dictionary from its JSON-friendly form.</summary>
        public static Dictionary<DayOfWeek, global::WorkCalendar.Domain.DaySchedule> FromJson(Dictionary<DayOfWeek, DayScheduleJson> json) =>
            json.ToDictionary(kvp => kvp.Key, kvp => DayScheduleJson.ToDomain(kvp.Value));
    }
}