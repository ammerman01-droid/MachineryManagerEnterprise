| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOM-007            |
| **Title**        | Domain Events      |
| **Version**      | 4.9.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-16         |

---

# 1. Purpose

This document defines the official Domain Events of MachineryManagerEnterprise.

Domain Events represent business facts that have already occurred.

A Domain Event is immutable.

Once published, it shall never be modified.

---

# Event Philosophy

Domain Events represent completed business facts.

They are immutable records of business history.

Events enable communication between bounded contexts while preserving loose
coupling and protecting aggregate boundaries.

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

# 4a. Organization Events

Formalized from BR-017 (Business Specification — Organization
Management).

## OrganizationRegistered

Raised when a new Organization is registered on the platform.

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

# 6a. Incident Events

Formalized from BR-009 (Business Specification — Incident Management),
which defines this lifecycle in full detail but had no corresponding
events in this catalog.

## IncidentReported

Raised when an Incident is first recorded. Initial information may be
incomplete.

---

## IncidentValidated

Raised when an Incident is confirmed as a legitimate operational
event.

---

## IncidentRejected

Raised when a reported Incident is determined to be a false report,
before or during validation. A reason is mandatory.

---

## IncidentClassified

Raised when an Incident receives its Primary Classification, Severity,
and Priority.

---

## IncidentAssigned

Raised when responsibility for an Incident is assigned to a Person,
Team, Department, or External Contractor. Assignment history is
preserved.

---

## IncidentInvestigationStarted

Raised when investigation activities begin.

---

## IncidentDecisionMade

Raised when the organization determines the appropriate response
(Maintenance Required, Observation Only, Operational Adjustment,
Supplier Claim, Safety Escalation, Insurance Process, or No Action
Required). Not every Incident results in Maintenance.

---

## IncidentResolved

Raised when the immediate business objectives of an Incident have been
completed.

---

## IncidentClosed

Raised when an Incident is formally completed. Closed Incidents become
permanent business history.

---

## IncidentReopened

Raised when a Closed Incident requires further action. Reopening
creates a new lifecycle transition; previous history remains
unchanged.

---

# 7. Usage Events

## MeterInstalled

Raised when a Meter Device becomes active.

---

## MeterRemoved

Raised when a Meter Device is removed.

---

## MeterFailureDetected

Raised when a Meter Device is confirmed to be malfunctioning (e.g.
producing impossible values, rollback, or abnormal jumps repeatedly
flagged by MeterValidationService).

Triggers the Meter Device Lifecycle transition from Operational to
Failed.

Readings recorded prior to this event remain part of history; readings
recorded after this event are treated as Non-operational Usage until
the Meter Device is repaired or replaced.

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

## MaintenanceRequested

Raised when a maintenance need is first identified, before review.

Typical sources:

- Forecast
- Inspection
- Incident
- Manual Request

---

## MaintenancePlanned

Raised when a future maintenance activity has been scheduled.

Consumers may include:

- Forecast Context
- Notification Services
- Reporting

---

## MaintenanceApproved

Raised when a planned Maintenance Operation has been approved for
execution by the configured approval policy (maintenance supervisor,
workshop manager, asset owner, or project manager).

---

## MaintenanceScheduled

Raised when execution of an approved Maintenance Operation has been
scheduled, recording planned start/finish, assigned workshop, and
assigned technicians.

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

## MaintenanceVerified

Raised when inspection confirms the maintenance achieved the expected
result (quality inspection, operational testing, commissioning, or
customer acceptance). Verification does not modify historical work.

---

## MaintenanceClosed

Raised when a Maintenance Operation becomes immutable. Only
administrative corrections remain possible afterward.

---

## MaintenanceCancelled

Raised when a Maintenance Operation is cancelled before execution
begins. Cancellation preserves business history; the operation is
never deleted. A reason is mandatory.

---

## MaintenanceSuspended

Raised when execution of a Maintenance Operation is temporarily
suspended (e.g. missing parts, weather, unavailable technicians,
awaiting an external vendor or customer approval).

---

## MaintenanceResumed

Raised when a suspended Maintenance Operation resumes execution. This
continues the same Maintenance Operation; a new one shall not be
created.

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

## RepairStarted

Raised when repair work begins following a diagnosed Failure.

Triggers the Failure Lifecycle transition from Repair Planned to Repair
In Progress.

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

# 9a. Relationship Events

Formalized from BR-015 (Business Specification — Relationship
Management), which had no corresponding events in this catalog.

## RelationshipCreated

Raised when a Relationship is created in Draft state.

---

## RelationshipActivated

Raised when a Relationship becomes the current business truth (Draft
to Active, or Modified back to Active).

---

## RelationshipModified

Raised when an Active Relationship changes while preserving historical
continuity.

---

## RelationshipExpired

Raised when a Relationship's business validity ends. Expired
Relationships no longer influence operational behavior but remain
historically available.

---

## RelationshipArchived

Raised when an Expired Relationship becomes Historical and therefore
immutable.

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

## ForecastRequested

Raised when a Forecast is explicitly requested (on demand or as part of
a scheduled recalculation), before generation begins.

Triggers the Forecast Lifecycle transition from Requested to
Generating.

---

## ForecastGenerated

Raised after a forecast has been produced.

---

## ForecastValidated

Raised when a Forecast has been reviewed and confirmed to be
technically correct and based on reliable business information.
Validation does not authorize execution.

---

## ForecastApproved

Raised when the organization accepts a Forecast as a legitimate
maintenance recommendation. Only Approved Forecasts may participate in
maintenance planning.

---

## ForecastScheduled

Raised when an Approved Forecast has been incorporated into future
maintenance planning (expected execution period, priority, sequence).

---

## ForecastConsumed

Raised when a Forecast has been used to initiate operational work
(e.g. Work Order creation, procurement planning). Preserves the
relationship between the Forecast and the resulting operational
activity.

---

## ForecastCompleted

Raised when the maintenance activity recommended by a Forecast has
been successfully executed. The Forecast becomes historical evidence
of a successful prediction; it is not deleted.

---

## ForecastCancelled

Raised when a Forecast is no longer applicable (incorrect prediction,
changed conditions, component replacement, asset retirement, or
duplicate recommendation). The business reason is mandatory.

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

# 11a. Notification Events

## NotificationCreated

Raised when a Business Notification has been generated after a
business event satisfied notification rules. Creation does not imply
delivery.

---

## NotificationQueued

Raised when a Notification has been accepted for delivery: recipient
resolution has completed and a delivery channel has been selected.

---

## NotificationDelivered

Raised when a Notification has successfully reached its delivery
channel (Inbox, Mobile Application, Email, SMS, Dashboard). Delivery
confirms transmission only, not that the recipient has seen it.

---

## NotificationViewed

Raised when the recipient opens or views the Notification. Viewing
does not imply business acceptance.

---

## NotificationAcknowledged

Raised when the recipient explicitly confirms awareness. Satisfies
business communication requirements.

---

## NotificationArchived

Raised when a Notification has completed its communication purpose.
Archived Notifications remain available for historical reporting.

---

## NotificationCancelled

Raised when a Notification becomes unnecessary before completion
(duplicate, originating event withdrawn, or business decision
changed).

---

# 11b. Internal Messaging Events

## MessageCreated

Raised when a sender creates a Message within a Conversation. The
Message exists but has not yet been transmitted.

---

## MessageSent

Raised when a Message has been accepted for transmission and
recipients have been resolved. Confirms intent, not delivery.

---

## MessageDelivered

Raised when a Message reaches a recipient's client (Dashboard, Mobile,
Desktop). Confirms transmission only, not that it has been opened.

---

## MessageRead

Raised when a recipient opens a Message. Read status is recorded
independently per recipient.

---

## MessageArchived

Raised when a Message is moved to historical communication while
remaining part of the Conversation record.

---

## MessageDeleted

Raised when a Message is soft-deleted. The message is hidden from
active views but communication history is never physically destroyed.

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


Aggregate

↓

Business Rule

↓

State Changed

↓

Domain Event

↓

Application Layer

↓

Infrastructure
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


---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 06-DomainServices.md
- 05-Aggregates.md
- 04-DomainModel.md
- 03-BoundedContexts.md
- 02-CoreConcepts.md
- 01-DomainPrinciples.md
- ADR-0001

---

# Revision History

| Version | Date       | Author             | Description                                           |
| 1.0.0   | 2026-07-18 | Solution Architect | Initial | Initial Domain Events definition            |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Added MeterFailureDetected, RepairStarted, and ForecastRequested — three events referenced as Trigger Events in 09-StateMachines.md but never previously defined in this catalog |
| 4.2.0   | 2026-08-02 | Solution Architect | Added MaintenanceRequested, MaintenanceApproved, MaintenanceScheduled, MaintenanceVerified, MaintenanceClosed, MaintenanceCancelled, MaintenanceSuspended, and MaintenanceResumed, supporting the expanded 9-state Maintenance Lifecycle now aligned with BR-011 |
| 4.3.0   | 2026-08-02 | Solution Architect | Added ForecastValidated, ForecastApproved, ForecastScheduled, ForecastConsumed, ForecastCompleted, and ForecastCancelled, supporting the expanded 7-state Forecast Lifecycle now aligned with BR-010 |
| 4.4.0   | 2026-08-02 | Solution Architect | Added a new Notification Events section (NotificationCreated, Queued, Delivered, Viewed, Acknowledged, Archived, Cancelled) covering BR-012's Notification Lifecycle, which had no corresponding events in this catalog at all |
| 4.5.0   | 2026-08-02 | Solution Architect | Added a new Internal Messaging Events section (MessageCreated, Sent, Delivered, Read, Archived, Deleted) covering BR-013's Message Lifecycle, which had no corresponding events in this catalog at all |
| 4.6.0   | 2026-08-02 | Solution Architect | Fixed Document ID: was DOM-006 (collided with corrected 06-DomainServices.md), corrected to DOM-007 |
| 4.7.0   | 2026-08-02 | Solution Architect | Added a new Section 6a Incident Events (IncidentReported through IncidentReopened, 10 events) covering BR-009's Incident Lifecycle, which had no corresponding events in this catalog at all |
| 4.8.0   | 2026-08-02 | Solution Architect | Added a new Section 9a Relationship Events (RelationshipCreated through RelationshipArchived, 5 events) covering BR-015's Relationship Lifecycle, which had no corresponding events in this catalog at all |
| 4.9.0   | 2026-08-02 | Solution Architect | Added Section 4a Organization Events (OrganizationRegistered), needed while implementing the Organization module |