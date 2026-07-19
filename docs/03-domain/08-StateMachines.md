# State Machines

| Property | Value |
|----------|-------|
| **Document ID** | DOM-008 |
| **Version** | 3.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

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
Planned

↓

Scheduled

↓

In Progress

↓

Completed
```

Alternative path

```text
Scheduled

↓

Cancelled
```

---

## Allowed Transitions

| From | To |
|------|----|
| Planned | Scheduled |
| Scheduled | In Progress |
| In Progress | Completed |
| Scheduled | Cancelled |

---

## Forbidden

Completed Maintenance shall never return to In Progress.

---

## Trigger Events

- MaintenancePlanned
- MaintenanceStarted
- MaintenanceCompleted

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
Requested

↓

Generating

↓

Available

↓

Superseded
```

---

## Business Rules

Forecasts never overwrite previous Forecasts.

Generating a new Forecast produces a new business object.

---

## Trigger Events

- ForecastRequested
- ForecastGenerated

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

# Related Documents

- 07-BusinessRules.md
- 06-DomainEvents.md
- 05-DomainServices.md
- 04-Aggregates.md
- 03-DomainModel.md
- 02-BoundedContexts.md

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | Initial | Initial State Machine definitions |
| 3.0.0 | 2026-07-18 | Standardized according to Documentation Standard v3.0 |