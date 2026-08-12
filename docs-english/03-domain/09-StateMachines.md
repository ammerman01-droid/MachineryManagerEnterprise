| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOM-009            |
| **Title**        | State Machines     |
| **Version**      | 4.5.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the lifecycle state machines of the major business entities.

Every lifecycle transition must satisfy the Business Rules defined in this project.

A State Machine defines:

- Valid States
- Allowed Transitions
- Forbidden Transitions
- Triggering Business Events

---

# State Machine Philosophy

State Machines define the legal lifecycle of business entities.

Every transition represents a business decision and shall always satisfy the
Business Rules, Aggregate Invariants, and Domain Principles.

State transitions never occur outside the Aggregate boundary.

---

# 2. State Machine Principles

Every State Machine shall satisfy the following principles.

- States are finite.
- States are mutually exclusive.
- Every entity has exactly one current state.
- Historical states are preserved.
- Illegal transitions are rejected.
- Transitions generate Domain Events.

---

# Lifecycle Modeling Rules

All lifecycle models shall satisfy:

- Exactly one current state
- Explicit legal transitions
- Explicit illegal transitions
- Transition triggers
- Domain Event publication
- Aggregate consistency

---

# 3. Asset Lifecycle

## States

```text
Draft

↓

Registered

↓

Commissioned

↓

Operational

↓

Inactive

↓

Retired

↓

Disposed
```

---

## Allowed Transitions

| From | To |
|------|----|
| Draft | Registered |
| Registered | Commissioned |
| Commissioned | Operational |
| Operational | Inactive |
| Inactive | Operational |
| Operational | Retired |
| Inactive | Retired |
| Retired | Disposed |

---

## Forbidden Transitions

Examples:

- Draft → Operational
- Registered → Retired
- Disposed → Operational
- Retired → Commissioned

---

## Trigger Events

- AssetRegistered
- AssetActivated
- AssetRetired
- AssetDisposed

---

# 4. Engine Lifecycle

## States

```text
Stored

↓

Installed

↓

Removed

↓

Under Repair

↓

Rebuilt

↓

Stored

↓

Installed
```

Final State

```text
Retired
```

---

## Allowed Transitions

| From | To |
|------|----|
| Stored | Installed |
| Installed | Removed |
| Removed | Stored |
| Removed | Under Repair |
| Under Repair | Rebuilt |
| Rebuilt | Stored |
| Installed | Retired |
| Stored | Retired |

---

## Business Constraints

Only one Asset may reference an Engine in Installed state.

---

## Trigger Events

- EngineInstalled
- EngineRemoved
- EngineRebuilt

---

# 5. Meter Device Lifecycle

## States

```text
Registered

↓

Installed

↓

Operational

↓

Failed

↓

Removed

↓

Archived
```

---

## Business Rules

Removing a Meter Device never removes its history.

Replacing a Meter Device never resets Operational Usage.

---

## Trigger Events

- MeterInstalled
- MeterRemoved
- MeterFailureDetected

---

# 6. Maintenance Lifecycle

## States

```text
Requested

↓

Planned

↓

Approved

↓

Scheduled

↓

Started

↓

In Progress

↓

Completed

↓

Verified

↓

Closed
```

Alternative paths

```text
Requested / Planned / Approved / Scheduled

↓

Cancelled
```

```text
Started / In Progress

↓

Suspended

↓

In Progress (resumed)
```

> **Note:** BR-011 (Business Specification — Maintenance Operations)
> is the detailed, authoritative source for this lifecycle.

---

## Allowed Transitions

| From | To |
|------|----|
| Requested | Planned |
| Planned | Approved |
| Approved | Scheduled |
| Scheduled | Started |
| Started | In Progress |
| In Progress | Completed |
| Completed | Verified |
| Verified | Closed |
| Requested | Cancelled |
| Planned | Cancelled |
| Approved | Cancelled |
| Scheduled | Cancelled |
| Started | Suspended |
| In Progress | Suspended |
| Suspended | In Progress |

---

## Forbidden

- Completed Maintenance shall never return to In Progress.
- Requested shall never transition directly to Completed (mandatory stages shall not be skipped unless explicitly authorized by business configuration).
- Closed Maintenance Operations are immutable; only administrative corrections remain possible.

---

## Trigger Events

- MaintenanceRequested
- MaintenancePlanned
- MaintenanceApproved
- MaintenanceScheduled
- MaintenanceStarted
- MaintenanceCompleted
- MaintenanceVerified
- MaintenanceClosed
- MaintenanceCancelled
- MaintenanceSuspended
- MaintenanceResumed

---

# 7. Failure Lifecycle

## States

```text
Detected

↓

Diagnosed

↓

Repair Planned

↓

Repair In Progress

↓

Resolved

↓

Closed
```

---

## Trigger Events

- FailureDetected
- RepairStarted
- RepairCompleted

---

# 7a. Incident Lifecycle

Formalized from BR-009 (Business Specification — Incident Management),
which defines this lifecycle in full detail but had no corresponding
entry in this document.

## States

```text
Reported

↓

Validated

↓

Classified

↓

Assigned

↓

Under Investigation

↓

Decision

↓

Resolved

↓

Closed
```

Alternative paths

```text
Reported / Validated

↓

Rejected
```

```text
Closed

↓

Reopened

↓

Under Investigation
```

---

## Allowed Transitions

| From | To |
|------|----|
| Reported | Validated |
| Validated | Classified |
| Classified | Assigned |
| Assigned | Under Investigation |
| Under Investigation | Decision |
| Decision | Resolved |
| Resolved | Closed |
| Reported | Rejected |
| Validated | Rejected |
| Closed | Reopened |
| Reopened | Under Investigation |

---

## Forbidden

- Skipping mandatory lifecycle states shall not be permitted.
- Closed Incidents shall never be modified directly; reopening always creates a new lifecycle transition, and previous history remains unchanged.

---

## Trigger Events

- IncidentReported
- IncidentValidated
- IncidentRejected
- IncidentClassified
- IncidentAssigned
- IncidentInvestigationStarted
- IncidentDecisionMade
- IncidentResolved
- IncidentClosed
- IncidentReopened

---

# 8. Document Lifecycle

## States

```text
Draft

↓

Approved

↓

Active

↓

Expiring

↓

Expired

↓

Replaced

↓

Archived
```

---

## Business Rules

Expired documents remain part of business history.

Archived documents remain accessible.

---

## Trigger Events

- DocumentRegistered
- DocumentExpired
- DocumentRenewed

---

# 9. Forecast Lifecycle

## States

```text
Generated

↓

Validated

↓

Approved

↓

Scheduled

↓

Consumed

↓

Completed
```

Alternative path

```text
Generated / Validated / Approved / Scheduled

↓

Cancelled
```

> **Note:** BR-010 (Business Specification — Maintenance Forecast) is
> the detailed, authoritative source for this lifecycle.

---

## Allowed Transitions

| From | To |
|------|----|
| Generated | Validated |
| Validated | Approved |
| Approved | Scheduled |
| Scheduled | Consumed |
| Consumed | Completed |
| Generated | Cancelled |
| Validated | Cancelled |
| Approved | Cancelled |
| Scheduled | Cancelled |

---

## Business Rules

Forecasts never overwrite previous Forecasts.

A cancelled or completed Forecast may be regenerated as a new business
object; the new Forecast preserves traceability to the previous one
when applicable.

---

## Trigger Events

- ForecastGenerated
- ForecastValidated
- ForecastApproved
- ForecastScheduled
- ForecastConsumed
- ForecastCompleted
- ForecastCancelled

---

# 10. Financial Record Lifecycle

## States

```text
Draft

↓

Recorded

↓

Posted

↓

Closed
```

Correction path

```text
Posted

↓

Adjustment Created
```

---

## Business Rules

Posted transactions are immutable.

Corrections create new transactions.

---

# 10a. Relationship Lifecycle

Formalized from BR-015 (Business Specification — Relationship
Management), which defines this lifecycle in full detail but had no
corresponding entry in this document.

## States

```text
Draft

↓

Active

↓

Modified

↓

Expired

↓

Historical
```

---

## Allowed Transitions

| From | To |
|------|----|
| Draft | Active |
| Active | Modified |
| Modified | Active |
| Active | Expired |
| Expired | Historical |

---

## Forbidden

- Historical Relationships shall never return to Active or Modified — they are immutable.
- Draft Relationships do not participate in operational propagation, authorization, or navigation; only Active Relationships do.

---

## Trigger Events

- RelationshipCreated
- RelationshipActivated
- RelationshipModified
- RelationshipExpired
- RelationshipArchived

---

# 11. Generic State Machine Rules

The following rules apply to every lifecycle.

## SM-001

Every entity possesses exactly one current state.

---

## SM-002

Every state transition shall be timestamped.

---

## SM-003

Every state transition shall preserve history.

---

## SM-004

Illegal transitions shall be rejected.

---

## SM-005

Successful transitions shall publish Domain Events.

---

## SM-006

State transitions shall never bypass Aggregate invariants.

---

# 12. Future State Machines

Future versions may introduce additional State Machines for:

- Inventory
- Procurement
- Fleet Scheduling
- AI Diagnostics
- IoT Devices
- Compliance

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 08-BusinessRules.md
- 07-DomainEvents.md
- 06-DomainServices.md
- 05-Aggregates.md
- 04-DomainModel.md
- 03-BoundedContexts.md
---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial | Initial State Machine definitions           |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Replaced the simplified 4-state Maintenance Lifecycle with the authoritative 9-state model (plus Cancelled/Suspended branches) from BR-011, per product owner decision; expanded Trigger Events accordingly |
| 4.2.0   | 2026-08-02 | Solution Architect | Replaced the simplified 4-state Forecast Lifecycle with the authoritative 7-state model (plus Cancelled branch) from BR-010, per product owner decision; expanded Trigger Events accordingly |
| 4.3.0   | 2026-08-02 | Solution Architect | Fixed Document ID: was DOM-008 (collided with corrected 08-BusinessRules.md), corrected to DOM-009 |
| 4.4.0   | 2026-08-02 | Solution Architect | Added Section 7a Incident Lifecycle, formalizing BR-009's fully-defined 8-state lifecycle (plus Rejected/Reopened branches), which previously had no entry in this document despite Document, Forecast, and Financial Record lifecycles being present |
| 4.5.0   | 2026-08-08 | Solution Architect | Added Section 10a Relationship Lifecycle, formalizing BR-015's 5-state lifecycle (Draft/Active/Modified/Expired/Historical), which previously had no entry in this document |