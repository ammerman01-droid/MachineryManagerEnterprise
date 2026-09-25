using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain;

/// <summary>
/// Child Entity of <see cref="LubricantOverflowReport"/>: one independent
/// overflow occurrence within a report (e.g. the Engine overflowed 2L of
/// Engine Oil 15W40 because of a worn gasket). A single report may
/// contain several Lines, each fully independent of the others except
/// for sharing the report's Date and Hour Meter Reading (chat, 2026-09-16).
/// Modeled as a plain-<see cref="Guid"/>-keyed child entity because it has
/// no existence or identity outside its owning aggregate.
/// </summary>
public sealed class LubricantOverflowLine
{
    /// <summary>Gets the maximum allowed length of <see cref="Reason"/>.</summary>
    public const int MaxReasonLength = 500;

    /// <summary>Gets the identifier of this line, unique within its owning report.</summary>
    public Guid Id { get; private set; }

    /// <summary>Gets the identifier of the Overflow Component (Configuration module) this line describes, e.g. Engine, Gearbox, Hydraulic Tank.</summary>
    public Guid OverflowComponentId { get; private set; }

    /// <summary>Gets the identifier of the Lubricant Type (Configuration module) that overflowed.</summary>
    public Guid LubricantTypeId { get; private set; }

    /// <summary>Gets the amount of fluid that overflowed, in liters.</summary>
    public decimal AmountInLiters { get; private set; }

    /// <summary>Gets the free-text reason given for this overflow.</summary>
    public string Reason { get; private set; } = string.Empty;

    private LubricantOverflowLine()
    {
    }

    private LubricantOverflowLine(Guid id, Guid overflowComponentId, Guid lubricantTypeId, decimal amountInLiters, string reason)
    {
        Id = id;
        OverflowComponentId = overflowComponentId;
        LubricantTypeId = lubricantTypeId;
        AmountInLiters = amountInLiters;
        Reason = reason;
    }

    internal static Result<LubricantOverflowLine> Create(Guid overflowComponentId, Guid lubricantTypeId, decimal amountInLiters, string reason)
    {
        if (overflowComponentId == Guid.Empty)
        {
            return Result.Failure<LubricantOverflowLine>(LubricantOverflowReportErrors.OverflowComponentRequired());
        }

        if (lubricantTypeId == Guid.Empty)
        {
            return Result.Failure<LubricantOverflowLine>(LubricantOverflowReportErrors.LubricantTypeRequired());
        }

        if (amountInLiters <= 0)
        {
            return Result.Failure<LubricantOverflowLine>(LubricantOverflowReportErrors.AmountMustBePositive());
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            return Result.Failure<LubricantOverflowLine>(LubricantOverflowReportErrors.ReasonRequired());
        }

        if (reason.Length > MaxReasonLength)
        {
            return Result.Failure<LubricantOverflowLine>(LubricantOverflowReportErrors.ReasonTooLong(MaxReasonLength));
        }

        return new LubricantOverflowLine(Guid.NewGuid(), overflowComponentId, lubricantTypeId, amountInLiters, reason.Trim());
    }
}
