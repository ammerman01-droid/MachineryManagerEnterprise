| Property | Value |
|----------|-------|
| **Document ID** | DOM-005 |
| **Version** | 4.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# 1. Purpose

This document defines the Aggregate design of the MachineryManagerEnterprise
domain according to Domain Driven Design.

Aggregates define transactional consistency boundaries and encapsulate business
rules that must always remain valid.

---

# Aggregate Principles

Every aggregate shall:

- Have exactly one Aggregate Root.
- Enforce business invariants.
- Own transactional consistency.
- Prevent external modification of internal entities.
- Reference other aggregates only by identity.

---

# Aggregate Design Rules

The following rules shall always be respected:

- One transaction modifies only one aggregate.
- Cross-aggregate consistency shall be eventual.
- Aggregate Roots expose behavior, not data.
- Internal entities are never accessed directly.

---

# 2. Aggregate Design Principles

Every Aggregate shall follow these principles.

- Single business responsibility
- Single Aggregate Root
- Strong consistency inside the Aggregate
- Eventual consistency between Aggregates
- Stable identity
- Independent lifecycle
- Explicit invariants

Aggregates are business consistency boundaries.

They are **not** database schemas.

They are **not** object hierarchies.

---

# 3. Aggregate Overview

```text
Enterprise

├── Asset Aggregate
│
├── Engine Aggregate
│
├── Maintenance Aggregate
│
├── Financial Aggregate
│
├── Document Aggregate
│
├── Forecast Aggregate
│
└── Knowledge Aggregate
```

---

# 4. Asset Aggregate

## Aggregate Root

Asset

---

## Purpose

Represents one physical machine together with its current operational state.

The Asset Aggregate is responsible for preserving the integrity of the Asset throughout its lifecycle.

---

## Contains

- Asset
- Current Status
- Current Location
- Current Installed Components (References)
- Current Meter Devices (References)

---

## References

The Asset Aggregate references—but does not own—

- Engine
- Engine Model
- Maintenance History
- Financial Records
- Documents
- Technical Library

---

## Invariants

The following business rules must always hold.

### Asset Identity

Every Asset shall possess exactly one permanent identity.

---

### Active Lifecycle State

An Asset may possess only one current lifecycle state.

Examples:

- Operational
- Inactive
- Retired
- Sold

Multiple simultaneous lifecycle states are prohibited.

---

### Installed Engine

An Asset may have zero or one installed primary Engine.

Multiple primary Engines are not allowed unless explicitly supported by future business rules.

---

### Meter Configuration

Only one active primary Meter Device may exist for each measurement type.

Example:

- One Hour Meter
- One Odometer

Historical Meter Devices remain outside the current operational state.

---

# 5. Aggregate Responsibilities

The Asset Aggregate is responsible for:

- Asset Registration
- Asset Activation
- Asset Retirement
- Engine Installation References
- Meter Installation References
- Current Operational Configuration

The Asset Aggregate is **not** responsible for:

- Engine history
- Meter history
- Repairs
- Financial calculations
- Forecast generation

These belong to other Aggregates.

---

# 6. Asset Aggregate Lifecycle

```text
Created

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

Only valid business transitions are allowed.

Illegal transitions shall be rejected by the Domain Layer.

---

# 7. Engine Aggregate

## Aggregate Root

Engine

---

## Purpose

Represents one physical Engine throughout its complete lifecycle.

Unlike an Asset, an Engine may exist independently and may serve multiple Assets during its lifetime.

---

## Contains

- Engine
- Engine Specifications
- Engine Status
- Current Installation
- Engine Lifecycle
- Engine Installation History

---

## References

- Asset (Current Installation)
- Engine Model
- Maintenance Aggregate

---

## Invariants

### Engine Identity

Every Engine has exactly one immutable identity.

---

### Current Installation

An Engine may be installed on **at most one Asset** at any point in time.

---

### Installation History

Every installation and removal operation shall generate a historical record.

No installation history may be modified or removed.

---

### Manufacturing Information

The following information is immutable:

- Manufacturer
- Engine Serial Number
- Manufacturing Year

Operational information may change.

---

# 8. Maintenance Aggregate

## Aggregate Root

Maintenance Record

---

## Purpose

Represents one maintenance operation performed on an Asset or Component.

---

## Contains

- Maintenance Record
- Inspection
- Failure
- Repair
- Replacement
- Labor Records
- Parts Consumption

---

## References

- Asset
- Engine
- Components
- Financial Transaction

---

## Invariants

### Closed Maintenance

Completed Maintenance Records become immutable.

---

### Failure Relationship

A Repair shall reference its originating Failure whenever applicable.

---

### Replacement Relationship

A Replacement shall identify:

- Removed Component
- Installed Component

Both relationships become permanent historical records.

---

# 9. Financial Aggregate

## Aggregate Root

Financial Account

---

## Purpose

Represents all financial events associated with an Asset.

---

## Contains

- Purchase Information
- Financial Transactions
- Depreciation
- Asset Valuation
- Operating Cost Summary

---

## References

- Asset
- Maintenance
- Documents

---

## Invariants

### Purchase Value

Purchase Value never changes.

---

### Current Value

Current Value is always calculated.

It shall never overwrite Purchase Value.

---

### Financial Transactions

Transactions become immutable after posting.

Corrections generate new transactions.

---

# 10. Document Aggregate

## Aggregate Root

Document

---

## Purpose

Represents one managed business document.

---

## Contains

- Metadata
- Versions
- Expiration Information
- Attachments

---

## References

- Asset
- Engine
- Organization

---

## Invariants

### Version History

Document Versions remain immutable.

---

### Expiration

Expired Documents remain accessible.

Only their operational status changes.

---

# 11. Forecast Aggregate

## Aggregate Root

Forecast

---

## Purpose

Represents predictive business information derived from historical observations.

---

## Contains

- Consumption Forecast
- Maintenance Forecast
- Replacement Forecast
- Cost Forecast

---

## References

- Operational Usage
- Asset
- Maintenance History

---

## Invariants

Forecasts never modify business history.

Forecasts are recalculated whenever required.

Historical Forecasts may optionally be retained for comparison.

---

# 12. Knowledge Aggregate

## Aggregate Root

Technical Library Item

---

## Purpose

Represents reusable technical knowledge associated with Models rather than physical Assets.

---

## Contains

- Manual
- Repair Guide
- Parts Catalogue
- Service Bulletin
- Technical Drawing

---

## References

- Asset Model
- Engine Model
- Component Model

---

## Invariants

Technical documents belong to Models whenever possible.

The same document may serve many Assets.

---

# 13. Aggregate Communication

Aggregates communicate through Domain Events.

Typical interactions include:

```text
Asset Registered

↓

Asset Aggregate

↓

Engine Installed

↓

Engine Aggregate

↓

Meter Reading Recorded

↓

Usage Processing

↓

Forecast Updated

↓

Maintenance Planned

↓

Financial Projection Updated
```

Aggregates should avoid direct modification of each other's internal state.

---

# 14. Transaction Boundaries

A single transaction shall never span multiple Aggregate Roots unless explicitly required by business rules.

Preferred strategy:

- Strong consistency inside an Aggregate.
- Eventual consistency between Aggregates.

---

# 15. Aggregate Size Guidelines

Aggregates should remain intentionally small.

Recommended limits:

- One Aggregate Root
- Few child entities
- Minimal object graph
- Clear business invariants

Large historical collections should be modeled outside the Aggregate Root and accessed separately.

---

# 16. Aggregate Evolution

Future versions may introduce additional Aggregates such as:

- Inventory Aggregate
- Procurement Aggregate
- Human Resources Aggregate
- Fleet Scheduling Aggregate
- IoT Device Aggregate
- AI Diagnostic Aggregate

Each new Aggregate shall define:

- Aggregate Root
- Responsibilities
- Invariants
- Relationships
- Transaction Boundary

before implementation.

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

- 04-DomainModel.md
- 03-BoundedContexts.md
- 00-DomainPrinciples.md
- ADR-0001

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial | Initial aggregate definitions               |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |