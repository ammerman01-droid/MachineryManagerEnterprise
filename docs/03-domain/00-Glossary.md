# Glossary

| Property | Value |
|----------|-------|
| **Document ID** | DOM-001 |
| **Version** | 3.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

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

## GL-ORG-001 Company

Type: Aggregate Root

A legal tenant that owns projects, assets, personnel and users.

## GL-ORG-002 Project

Type: Aggregate Root

Operational unit belonging to a Company.

## GL-IDN-001 User

Authenticated system user.

## GL-IDN-002 Permission Profile

Collection of permissions assignable to users.

## GL-FLT-001 Asset

Company-owned equipment managed through its lifecycle.

## GL-FLT-002 Asset Category

Excavator, Loader, Dozer, Grader, Generator and future categories.

## GL-FLT-003 Asset Model

Manufacturer specification shared by many assets.

## GL-FLT-004 Asset Assignment

Assignment of an Asset to a Project with StartDate and EndDate.

## GL-FLT-005 Meter

Current cumulative operating meter.

## GL-PRS-001 Personnel

Company employee.

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

# Related Documents

- Domain Principles
- Capability Model
- Architecture Overview
- ADR-0001

# Change History

| Version | Date | Description |
|----------|------------|-------------------------------------------|
| 1.0.0 | 2026-07-17 | Initial glossary |
| 3.0.0 | 2026-07-18 | Standardized according to Documentation Standard v3.0 |