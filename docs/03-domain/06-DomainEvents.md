# Domain Events

**Document ID:** MME-DOM-006

**Repository Path:** `docs/03-domain/06-DomainEvents.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 01-CoreConcepts.md
- 02-BoundedContexts.md
- 03-DomainModel.md
- 04-Aggregates.md
- 05-DomainServices.md

---

# 1. Purpose

This document defines the official Domain Events of MachineryManagerEnterprise.

Domain Events represent business facts that have already occurred.

A Domain Event is immutable.

Once published, it shall never be modified.

---

# 2. Design Principles

Every Domain Event shall satisfy the following principles.

- Represents a completed business fact.
- Uses business terminology.
- Is immutable.
- Has a unique identifier.
- Has an occurrence time.
- Can be audited.
- May be consumed by multiple bounded contexts.

Domain Events are business concepts.

They are not implementation details.

---

# 3. Event Categories

```text
Domain Events

├── Asset Events
├── Component Events
├── Usage Events
├── Maintenance Events
├── Financial Events
├── Document Events
├── Forecast Events
└── System Events
```

---

# 4. Event Structure

Every Domain Event shall contain at least:

- EventId
- EventType
- OccurredAt
- AggregateId
- AggregateType
- EventVersion
- CorrelationId (optional)
- CausationId (optional)

Additional business data is event-specific.

---

# 5. Asset Events

## AssetRegistered

Raised when a new Asset is successfully registered.

Typical consumers:

- Finance Context
- Document Context
- Reporting

---

## AssetActivated

Raised when an Asset enters operational service.

---

## AssetTransferred

Raised when ownership or operational responsibility changes.

---

## AssetRetired

Raised when an Asset permanently leaves operational service.

---

## AssetDisposed

Raised when an Asset is sold or scrapped.

Asset history remains preserved.

---

# 6. Component Events

## EngineInstalled

Raised after an Engine has been installed on an Asset.

Consumers may include:

- Usage Context
- Forecast Context
- Reporting

---

## EngineRemoved

Raised after an Engine has been removed.

---

## EngineRebuilt

Raised after an Engine overhaul has been completed.

---

## ComponentInstalled

Raised when any Component is installed.

---

## ComponentRemoved

Raised when any Component is removed.

---

## ComponentReplaced

Raised after a replacement operation has completed successfully.

Replacement always preserves historical traceability.

---

# 7. Usage Events

## MeterInstalled

Raised when a Meter Device becomes active.

---

## MeterRemoved

Raised when a Meter Device is removed.

---

## MeterReadingRecorded

Raised after a validated Meter Reading has been stored.

This event shall only be raised after successful validation.

---

## OperationalUsageCalculated

Raised after Operational Usage has been recalculated.

---

## NonOperationalUsageRecorded

Raised when invalid or excluded usage has been registered.

This event does not affect maintenance planning.


---

# 8. Maintenance Events

## MaintenancePlanned

Raised when a future maintenance activity has been scheduled.

Consumers may include:

- Forecast Context
- Notification Services
- Reporting

---

## MaintenanceStarted

Raised when maintenance work officially begins.

---

## MaintenanceCompleted

Raised when all maintenance activities have been completed successfully.

Typical consumers:

- Finance Context
- Forecast Context
- Reporting

---

## InspectionCompleted

Raised after an inspection has been finalized.

The event includes:

- Inspection Result
- Severity
- Recommendations

---

## FailureDetected

Raised immediately after a Failure has been confirmed.

Typical consumers:

- Maintenance Planning
- Forecast
- Reporting

---

## RepairCompleted

Raised after a successful repair.

Repairs may generate additional business events such as:

- ComponentReplaced
- EngineInstalled

---

# 9. Financial Events

## PurchaseRecorded

Raised after the initial purchase has been registered.

---

## FinancialTransactionRecorded

Raised whenever a financial transaction becomes permanent.

Examples include:

- Fuel Purchase
- Insurance
- Maintenance Cost
- Spare Parts
- Transportation

---

## DepreciationCalculated

Raised after asset depreciation has been recalculated.

---

## AssetValuationUpdated

Raised whenever the estimated business value changes.

The original acquisition value remains unchanged.

---

## OwnershipCostUpdated

Raised after Total Cost of Ownership has been recalculated.

---

# 10. Document Events

## DocumentRegistered

Raised when a new document enters the system.

---

## DocumentUpdated

Raised after a new document version has been created.

Existing versions remain immutable.

---

## DocumentExpired

Raised when a document reaches its expiration date.

Consumers may include:

- Notification Services
- Compliance Reports
- Dashboard

---

## DocumentRenewed

Raised after a replacement document has been registered.

---

# 11. Forecast Events

## ForecastGenerated

Raised after a forecast has been produced.

---

## ConsumptionForecastGenerated

Raised after predicting future consumption.

Examples:

- Fuel
- Lubricants
- Coolant
- Grease

---

## MaintenanceForecastGenerated

Raised after estimating future maintenance activities.

---

## SparePartsForecastGenerated

Raised after estimating future spare part demand.

---

## ReplacementForecastGenerated

Raised after estimating future replacement of major Components.

---

# 12. Event Relationships

Typical business event flow:

```text
AssetRegistered

↓

EngineInstalled

↓

MeterInstalled

↓

MeterReadingRecorded

↓

OperationalUsageCalculated

↓

MaintenancePlanned

↓

MaintenanceCompleted

↓

OwnershipCostUpdated

↓

ForecastGenerated
```

Each event represents a completed business fact.

Events never represent intentions.

---

# 13. Event Publishing Rules

A Domain Event shall only be published when:

- the business transaction has completed successfully;
- Aggregate invariants remain valid;
- business state has changed.

Events shall never be published before business completion.

---

# 14. Event Naming Convention

Every Domain Event name shall follow the pattern:

```
BusinessObject + PastTenseVerb
```

Examples:

- AssetRegistered
- EngineInstalled
- MeterReadingRecorded
- FailureDetected
- RepairCompleted
- ForecastGenerated

Avoid names that describe commands or intentions.

Incorrect examples:

- RegisterAsset
- InstallEngine
- UpdateForecast

These represent Commands rather than Events.

---

# 15. Event Consumers

One event may be consumed by multiple bounded contexts.

Example:

```
MaintenanceCompleted

        │

 ┌──────┼──────────────┐

 ▼      ▼              ▼

Finance Forecast   Reporting
```

Publishers shall never know their consumers.

This preserves loose coupling.

---

# 16. Event Versioning

Business events are permanent business records.

If an event schema changes:

- create a new event version;
- never modify historical events;
- preserve backward compatibility whenever possible.

---

# 17. Future Event Categories

The following event groups are expected in future releases:

- Inventory Events
- Procurement Events
- AI Diagnostic Events
- Fleet Scheduling Events
- IoT Telemetry Events
- Compliance Events

Each future event shall follow the rules defined in this document.

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Domain Events definition |