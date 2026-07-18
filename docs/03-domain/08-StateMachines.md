# State Machines

**Document ID:** MME-DOM-008

**Repository Path:** `docs/03-domain/08-StateMachines.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 03-DomainModel.md
- 04-Aggregates.md
- 05-DomainServices.md
- 06-DomainEvents.md
- 07-BusinessRules.md

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

# 2. State Machine Principles

Every State Machine shall satisfy the following principles.

- States are finite.
- States are mutually exclusive.
- Every entity has exactly one current state.
- Historical states are preserved.
- Illegal transitions are rejected.
- Transitions generate Domain Events.

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

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial State Machine definitions |