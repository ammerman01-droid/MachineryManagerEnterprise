| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOM-000            |
| **Title**        | Glossary           |
| **Version**      | 4.2.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

## Purpose

Defines the official ubiquitous language used throughout the
MachineryManagerEnterprise platform.

All architecture documents, domain models, source code, ADRs, Technology
Evaluations, and business documentation shall consistently use these terms.

# Guiding Principles

The glossary serves as the single source of truth for business terminology.

Every new domain concept shall:

- Use a unique identifier.
- Have a precise definition.
- Be technology independent.
- Avoid ambiguous business language.

## GL-ORG-001 Organization

Type: Aggregate Root

A tenant that owns projects, assets, personnel and users. Organizations
are the multi-tenancy boundary: multiple Organizations (customer
companies) use the platform concurrently from a single web deployment,
each isolated within its own Organization boundary.

## GL-ORG-002 Project

Type: Aggregate Root

Operational unit belonging to an Organization.

## GL-IDN-001 User

Authenticated system user.

## GL-IDN-002 Permission Profile

Collection of permissions assignable to users.

## GL-FLT-001 Asset

Organization-owned equipment managed through its lifecycle.

## GL-FLT-002 Asset Category

Excavator, Loader, Dozer, Grader, Generator and future categories.

## GL-FLT-003 Asset Model

Manufacturer specification shared by many assets.

## GL-FLT-003a Company

A manufacturer brand of equipment or components (e.g. Caterpillar,
Komatsu). Distinct from GL-ORG-001 Organization, which is the platform
tenant boundary. Currently represented as the Manufacturer attribute of
Asset Model and Engine Model; not yet modeled as an independent entity.

## GL-FLT-004 Asset Assignment

Assignment of an Asset to a Project with StartDate and EndDate.

## GL-FLT-005 Meter

Current cumulative operating meter.

## GL-PRS-001 Personnel

Organization employee.

## GL-PRS-002 Personnel Assignment

Project assignment preserving history.

## GL-OPR-001 Shift Calendar

Working calendar for a project.

## GL-OPR-002 Operation Entry

Daily operation record.

## GL-FUE-001 Fuel Entry

Fuel transaction with meter reading.

## GL-FLD-001 Fluid Top-Up

Oil/coolant/hydraulic additions outside scheduled service.

## GL-MNT-001 Work Order

Formal repair instruction.

## GL-SRV-001 Service Plan

Recurring maintenance definition.

## GL-SRV-002 Service Order

Execution of a Service Plan.

## GL-INV-001 Warehouse

Project-owned inventory.

## Shared Concepts

Approval, Attachment, Notification, Audit Log.

## Future Reserved Terms

GPS Tracking, IoT Device, Predictive Maintenance, Vendor, Contractor.


---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- Domain Principles
- Capability Model
- Architecture Overview
- ADR-0001

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-17 | Solution Architect | Initial glossary                                      |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Renamed GL-ORG-001 from "Company" to "Organization" (the multi-tenant platform boundary, confirmed by product owner); added distinct GL-FLT-003a "Company" for equipment manufacturer brands (e.g. Caterpillar, Komatsu), which is a separate concept that had been conflated with the tenant term |
| 4.2.0   | 2026-08-08 | Solution Architect | Fixed Document ID collision: was DOM-001 (duplicate of 01-DomainPrinciples.md), corrected to DOM-000 |