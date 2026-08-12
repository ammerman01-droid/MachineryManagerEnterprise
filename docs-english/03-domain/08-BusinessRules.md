| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOM-008            |
| **Title**        | Business Rules     |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the official business rules governing MachineryManagerEnterprise.

Business Rules describe what is legally or logically permitted within the business domain.

These rules are independent from software implementation.

---

# Rule Philosophy

Business Rules represent the immutable policies of the business domain.

They define what is legally, logically, and operationally valid regardless of
software implementation.

Aggregates, Domain Services, Domain Events, and Application logic shall always
conform to these rules.

---

# 2. Business Rule Principles

Every rule shall be:

- Business driven
- Technology independent
- Testable
- Deterministic
- Explicit
- Traceable

Whenever implementation conflicts with these rules, the rules take precedence.

---

# 3. Rule Classification

Business Rules are grouped into the following categories.

```text
Business Rules

├── Asset Rules
├── Component Rules
├── Usage Rules
├── Maintenance Rules
├── Financial Rules
├── Document Rules
├── Forecast Rules
└── Historical Integrity Rules
```

---

# 4. Asset Rules

| Category | Rules |
|----------|------:|
| Asset | 4 |
| Component | 5 |
| Meter | 5 |
| Operational Usage | 5 |
| Maintenance | 5 |
| Financial | 5 |
| Documents | 4 |
| Forecast | 4 |
| Historical Integrity | 3 |

## BR-001

Every physical Asset shall possess one permanent identity.

The identity shall never change.

---

## BR-002

Every Asset shall belong to exactly one Asset Model.

Changing technical specifications shall normally require changing the Asset Model rather than modifying the Asset.

---

## BR-003

Every Asset shall always have one current lifecycle state.

Historical states remain permanently preserved.

---

## BR-004

Retiring an Asset shall never remove its business history.

---

# 5. Component Rules

## BR-005

An Engine is an independent business object.

It is not a property of an Asset.

---

## BR-006

One Engine may serve multiple Assets throughout its lifetime.

Every installation shall be recorded.

---

## BR-007

An Engine may exist without being installed.

Possible examples:

- Warehouse
- Repair Workshop
- Storage
- Transportation

---

## BR-008

Engine identity shall remain unchanged after rebuilding.

Rebuilding creates maintenance history.

It does not create a new Engine.

---

## BR-009

A replacement Component preserves its own lifecycle.

The removed Component shall never disappear from history.

---

# 6. Meter Rules

## BR-010

A Meter Device is independent from Operational Usage.

---

## BR-011

Meter replacement shall never reset accumulated Operational Usage.

Operational Usage belongs to the Asset.

The Meter only measures it.

---

## BR-012

A replacement Meter may already contain previous readings.

Those historical readings belong to the Meter Device rather than the Asset.

Business calculations shall ignore previous unrelated readings.

---

## BR-013

Every Meter replacement shall generate historical records.

Meter history shall never be lost.

---

## BR-014

A Meter Device may fail.

Failure shall never invalidate historical business calculations.

---

# 7. Operational Usage Rules

## BR-015

Operational Usage shall be calculated from validated business events.

It shall never be calculated directly from the latest Meter value.

---

## BR-016

Operational Usage represents productive work.

Operational Usage includes only productive operation.

Examples include:

- Machine operating
- Productive driving
- Excavation
- Loading
- Transportation
- Agricultural work

---

## BR-017

Non-operational Usage shall never contribute to:

- Preventive Maintenance
- Depreciation
- Performance Indicators
- Consumption Forecasts

Non-operational Usage shall remain part of business history.

---

## BR-018

Meter validation shall reject impossible readings.

Examples include:

- Negative progression
- Impossible jumps
- Duplicate readings
- Physically impossible operating rates

Rejected readings shall never participate in business calculations.

---

## BR-019

Operational Usage is immutable after validation.

Corrections shall be recorded as business adjustment events.

Historical values shall never be overwritten.

---

# 8. Maintenance Rules

## BR-020

Preventive Maintenance shall always be based on validated Operational Usage.

---

## BR-021

Corrective Maintenance shall always reference the originating Failure whenever one exists.

---

## BR-022

Every completed Maintenance Activity shall become immutable.

If an error is discovered, a new corrective Maintenance Record shall be created.

---

## BR-023

Replacing a Component shall preserve:

- Removed Component
- Installed Component
- Replacement Date
- Responsible Technician

Historical relationships shall never be deleted.

---

## BR-024

An Engine replacement shall update only the current operational configuration.

Historical Engine installations remain unchanged.

---

# 9. Financial Rules

## BR-025

Purchase Price is immutable.

The originally paid acquisition value shall never change.

---

## BR-026

Current Asset Value shall always be calculated.

It shall never overwrite Purchase Price.

---

## BR-027

Every financial expense shall remain permanently traceable.

Examples include:

- Fuel
- Lubricants
- Repairs
- Spare Parts
- Insurance
- Taxes
- Transportation

---

## BR-028

Total Cost of Ownership (TCO) shall include every operating expense associated with the Asset.

---

## BR-029

Depreciation calculations shall never modify historical financial transactions.

Depreciation is derived information.

---

# 10. Document Rules

## BR-030

Business Documents shall never be physically deleted.

---

## BR-031

Expired Documents remain valid historical records.

Only their operational status changes.

---

## BR-032

Every document shall possess version history.

Older versions remain accessible.

---

## BR-033

Document reminders shall be generated before expiration according to configurable business rules.

---

# 11. Forecast Rules

## BR-034

Forecasts shall always be based on historical validated data.

---

## BR-035

Forecasts shall never modify business history.

---

## BR-036

Forecasts are advisory.

Human users remain responsible for business decisions.

---

## BR-037

Forecast models may evolve over time.

Historical Forecasts shall remain reproducible whenever possible.

---

# 12. Historical Integrity Rules

## BR-038

Business history shall be append-only.

Existing historical records shall never be removed.

---

## BR-039

Every business identity shall remain permanent throughout its lifecycle.

---

## BR-040

Historical relationships shall remain reconstructable at any point in time.

Examples include:

- Which Engine was installed on a given date.
- Which Meter Device was active.
- Which documents were valid.
- Which Components were installed.
- Which Maintenance Activities had been completed.

---

# 13. Future Rules

Future versions of the platform may introduce additional rules covering:

- Inventory
- Procurement
- Fleet Scheduling
- AI Diagnostics
- IoT Devices
- Regulatory Compliance

All future rules shall follow the principles defined in this document.

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

- 01-DomainPrinciples.md
- 00-Glossary.md
- 02-CoreConcepts.md
- 03-BoundedContexts.md
- 04-DomainModel.md
- 05-Aggregates.md
- 06-DomainServices.md
- 07-DomainEvents.md

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial | Initial Business Rules definition           |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Fixed Document ID: was DOM-007 (collided with corrected 07-DomainEvents.md), corrected to DOM-008 |