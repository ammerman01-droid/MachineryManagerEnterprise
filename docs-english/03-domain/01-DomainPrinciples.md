| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOM-001            |
| **Title**        | Domain Principles  |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the constitutional principles governing the business domain of MachineryManagerEnterprise.
All future domain models, business rules, APIs and database structures shall conform to these principles.

# 2. Scope

These principles apply to every business object managed by the platform, including Assets, Components, Engines, Meters, Documents, Maintenance Records and Financial Records.

# 3. Core Philosophy

The platform is an Enterprise Asset Lifecycle Management System.

The business domain always has priority over technical implementation.

---

# 4. Fundamental Principles

## P-01 Preserve History

Business history is never deleted.
Business events create history.

**Impact**

- Soft delete instead of destructive delete
- Auditable records
- Immutable historical data

---

## P-02 Identity Never Changes

Every business object owns a permanent identity.

Serial numbers, Engine IDs and Asset IDs identify history, not current state.

---

## P-03 Lifecycle Over Current State

Current state is only a snapshot.

Business value comes from the complete lifecycle.

---

## P-04 Components Have Independent Lifecycles

Components are independent business entities.

An Engine may move between multiple Assets during its lifetime.

---

## P-05 Usage Is Business Knowledge

Meter readings are observations.

Operational Usage and Non-operational Usage are different concepts.

Only operational usage contributes to lifecycle calculations.

---

## P-06 Models Are Templates

Model ≠ Instance.

Asset Models, Engine Models and Component Models define shared specifications.

Instances define physical reality.

---

## P-07 Financial Truth Is Preserved

Purchase value is immutable.

Current value is calculated.

Depreciation never overwrites acquisition value.

---

## P-08 Documents Are Business Assets

Documents are first-class business objects.

Expiration creates reminders, not deletion.

---

## P-09 Business Before Technology

Database tables never define the business.

The domain defines the database.

---

## P-10 Documentation Is Part of the Product

Documentation is a deliverable and evolves with the product.

---

## P-11 Asset Relationships Are First-Class Business Concepts

Business Assets may form operational configurations.

Relationships between Assets are business entities with their own lifecycle and history.

Operational usage may propagate through these relationships according to business rules.

---

## P-12 Installation History Must Be Preserved

Installation, removal and relocation of Components and Assets are business events.

The platform shall preserve complete installation history for all movable business objects.

---

## P-13 Forecast Before Execution

Maintenance planning begins with business observations.

Forecasts are independent business objects and shall exist before Work Orders.

A Forecast may result in zero, one or many Work Orders.

---

## P-14 Knowledge Is a Business Asset

Business observations, failures, maintenance history and operational data together create organizational knowledge.

The platform shall preserve and expose this knowledge for future analysis and decision support.

---

## P-15 Business Events Drive Cross-Cutting Behavior

Significant business state changes shall generate domain events.

Notifications, reporting, analytics and integrations should react to business events rather than directly querying business logic.

---

# 5. Architectural Consequences

These principles influence:

- Domain Model
- Aggregates
- Events
- Database
- APIs
- Reporting
- Forecasting

---

# 6. Validation Checklist

Before accepting any design decision verify:

- Does it preserve history?
- Does it preserve identity?
- Does it respect lifecycle?
- Does it separate Model from Instance?
- Does it preserve financial truth?
- Is terminology business-oriented?

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 00-Vision.md
- 01-Architecture.md
- ../02-architecture/02-CapabilityModel.md
- ADR-0001 — Adopt Clean Architecture

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial constitutional principles                     |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Fixed Document ID collision: was DOM-000 (duplicate of 00-Glossary.md), corrected to DOM-001 |