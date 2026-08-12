| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOM-010            |
| **Title**        | Domain Discovery   |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document catalogs all discovered business capabilities of MachineryManagerEnterprise.

Its purpose is to ensure that every business idea is formally identified before entering specification, design and implementation.

No business capability shall be implemented unless it has first been registered in this document.

Some discovered capabilities represent **Business Foundations**.

These capabilities are not tied to a single module but define common business behavior reused across multiple business specifications.

Examples include:

- Tracked Components
- Maintenance Operations
- Lifecycle Tracking
- Relationship Management

---

# 2. Capability Lifecycle

Every business capability shall evolve through the following lifecycle.

```text
Idea

↓

Domain Discovery

↓

Business Specification

↓

Domain Model

↓

Application Design

↓

Implementation
```

This document represents the Domain Discovery stage.

---

# 3. Discovery Status

Each capability shall have one of the following states.

| Status | Description |
|----------|-------------|
| Discovered | Business capability identified but not yet specified |
| Specified | Business specification completed |
| Designed | Domain model completed |
| Implemented | Fully implemented |

---

# 4. Priority Levels

| Priority | Description |
|----------|-------------|
| High | Core business capability |
| Medium | Important business capability |
| Low | Optional enhancement |

---

# Discovery Categories

Business capabilities are classified into one of the following categories.

| Category | Description |
|----------|-------------|
| Core Asset Management | Core lifecycle of enterprise assets |
| Maintenance | Preventive, corrective and predictive maintenance |
| Components | Tires, batteries, engines, attachments and replaceable parts |
| Inventory | Warehouses, spare parts and procurement |
| Safety | Incidents, risks and investigations |
| Communication | Messaging, notifications and collaboration |
| Intelligence | Forecasting, AI and analytics |
| Maintenance Lifecycle | Business capabilities responsible for executing and recording maintenance work. |

---

# 5. Capability Categories

To simplify domain analysis, every discovered capability belongs to one primary business category.
|ID     | Business Capability | Status | Priority |
|DD-001 | Asset Management | Asset Lifecycle | Implemented Foundation | High|
|DD-002 | Asset Relationships | Asset Lifecycle | Discovered | High|
|DD-003 | Tracked Components | Discovered | High |
|DD-004 | Tire Lifecycle Management | Component Lifecycle | Discovered | High|
|DD-005 | Battery Lifecycle Management | Component Lifecycle | Discovered | High|
|DD-006 | Parts Catalog | Technical Knowledge | Discovered | High|
|DD-007 | Part Cross Reference | Technical Knowledge | Discovered | High|
|DD-008 | Incident Management | Safety | Discovered | High|
|DD-009 | Maintenance Forecast | Intelligence | Discovered | High|
|DD-010 | Maintenance Operations | Maintenance Lifecycle | Discovered | High |
|DD-011 | Notification Center | Communication | Discovered | High|
|DD-012 | Internal Messaging | Communication | Discovered | Medium|
|DD-013 | AI Assistant | Intelligence | Discovered | Medium|
|DD-014 | Lifecycle Tracking      | Almost all entities have a life cycle.|
|DD-015 | Relationship Management | The relationship between assets, components, and attachments is an independent concept. |


---

# 6. Capability Descriptions

## DD-001 — Asset Management

Purpose

Manage the complete lifecycle of enterprise assets.

Current Status

Architectural foundation completed.

---

## DD-002 — Asset Relationships

Purpose

Support operational relationships between multiple independent assets.

Examples

- Truck + Crane
- Tractor + Trailer
- Excavator + Attachment

Business Value

Operational usage may propagate across related assets while ownership remains independent.

Future Specification

BusinessSpecification-AssetRelationships.md

---

## DD-003 — Tracked Components

Purpose

Provide a unified business model for movable and independently managed physical components.

Definition

A Tracked Component is a physical object that:

- owns a permanent identity;
- can be installed on an Asset;
- can be removed from an Asset;
- may move between multiple Assets during its lifetime;
- owns an independent operational lifecycle;
- owns independent financial history;
- preserves complete installation history.

Typical Examples

- Tire
- Battery
- Engine
- Gearbox
- Hydraulic Hammer (Pikour)
- Bucket
- Generator
- Compressor
- Winch

Business Value

This capability prevents duplication of lifecycle rules across multiple component types.

Future specifications shall specialize this capability rather than redefine it.

Future Specification

BusinessSpecification-TrackedComponents.md

---

## DD-004 — Tire Lifecycle Management

Purpose

Manage tires as independent business assets.

Business Value

Track:

- Serial Number
- Installation History
- Position
- Remaining Life
- Movement between Assets

Future Specification

BusinessSpecification-TireManagement.md

---

## DD-005 — Battery Lifecycle Management

Purpose

Manage batteries independently from the assets where they are installed.

Business Value

Track:

- Serial Number
- Capacity
- Installation History
- Remaining Service Life

Future Specification

BusinessSpecification-BatteryManagement.md

---

## DD-006 — Parts Catalog

Purpose

Maintain a centralized catalog of technical parts.

Business Value

Provide a single source of truth for all technical components used throughout the enterprise.

Future Specification

BusinessSpecification-PartsCatalog.md

---

## DD-007 — Part Cross Reference

Purpose

Maintain equivalent manufacturer part numbers.

Business Value

Allow multiple manufacturers' part numbers to reference the same functional component.

Example

Original Part Number

↓

Equivalent Part Numbers

↓

Alternative Suppliers

Future Specification

BusinessSpecification-PartCrossReference.md

---

## DD-008 — Incident Management

Purpose

Record operational incidents affecting personnel, equipment or facilities.

Business Value

Support:

- Safety Investigation
- Damage Assessment
- Repair Cost Tracking
- Insurance Documentation

Future Specification

BusinessSpecification-IncidentManagement.md

---

## DD-009 — Maintenance Forecast

Purpose

Predict future maintenance requirements before work orders are generated.

Business Value

Support:

Inspection

↓

Observation

↓

Forecast

↓

Approval

↓

Work Order

Future Specification

BusinessSpecification-MaintenanceForecast.md

---

## DD-010 — Maintenance Operations

Purpose

Provide the business process responsible for executing every maintenance activity performed on an Asset.

Definition

A Maintenance Operation is the execution of approved maintenance work that may:

- inspect an Asset;
- repair an Asset;
- replace components;
- install components;
- remove components;
- relocate tracked components;
- consume spare parts;
- consume labor;
- generate operational history;
- generate financial history.

Business Value

Maintenance Operations become the single business source responsible for:

- Asset Maintenance History
- Component Installation History
- Component Removal History
- Operational Cost
- Labor Cost
- Downtime
- Work Completion

All component lifecycle events originate from Maintenance Operations rather than being created independently.

Future Specification

BusinessSpecification-MaintenanceOperations.md

---

## DD-011 — Notification Center

Purpose

Provide centralized notification management.

Business Value

Support:

- Maintenance Alerts
- Inspection Alerts
- Document Expiration
- Inventory Alerts
- User Reminders

Notifications shall be event-driven.

Future Specification

BusinessSpecification-Notifications.md

---

## DD-012 — Internal Messaging

Purpose

Provide communication between operational users and administrators.

Business Value

Exchange:

- Messages
- Files
- Operational Information

The module shall remain optional.

Future Specification

BusinessSpecification-Messaging.md

---

## DD-013 — AI Assistant

### Purpose

Provide an intelligent assistant capable of helping users operate the system, understand asset history, analyze maintenance information, and answer technical questions using enterprise knowledge and approved external references.

### Business Motivation

Enterprise users frequently require assistance that is currently scattered across manuals, historical work orders, technical documentation, and experienced personnel.

The AI Assistant centralizes this knowledge and makes it immediately accessible.

### Primary Capabilities

The AI Assistant may provide:

- Product guidance
- System usage assistance
- Maintenance recommendations
- Technical troubleshooting
- Asset history explanation
- Parts identification
- Forecast assistance
- Notification explanation
- Operational analytics
- Decision support

### Supported Knowledge Sources

The assistant may retrieve information from:

- Internal documentation
- Asset history
- Maintenance history
- Work Orders
- Technical Parts Catalog
- Cross Reference Catalog
- Forecast records
- Incident history
- Enterprise policies

Future versions may also consult approved external technical references.

### Assistant Modes

The assistant may operate in multiple modes.

#### System Assistant

Answers questions about the software itself.

#### Maintenance Assistant

Provides maintenance recommendations.

#### Parts Assistant

Supports part identification and cross-reference lookup.

#### Planning Assistant

Assists with maintenance planning and prioritization.

#### Analytics Assistant

Explains operational data and trends.

### Business Value

The assistant reduces:

- Training time
- Human dependency
- Search effort

while improving:

- Decision quality
- Knowledge sharing
- Operational efficiency

### Architectural Principles

The AI subsystem shall be:

- Modular
- Independently deployable
- Provider-agnostic

No business logic shall reside inside the AI engine.

The AI provides recommendations only.

Critical business decisions remain under user responsibility.

### Future Specification

BusinessSpecification-AIAssistant.md

---

## DD-014 — Lifecycle Tracking

Purpose

Provide a unified business concept for tracking the lifecycle of any managed business entity.

Definition

Many business entities possess an independent lifecycle that shall be preserved throughout their existence.

Examples

- Assets
- Tires
- Batteries
- Engines
- Attachments
- Documents
- Maintenance Forecasts

Business Value

Lifecycle Tracking enables:

- Complete historical traceability
- Accurate operational analysis
- Financial analysis
- Predictive maintenance
- Regulatory compliance

Future Specification

This capability is considered a cross-cutting business concept and may be referenced by multiple Business Specifications.

---

## DD-015 — Relationship Management

Purpose

Define how independent business entities become temporarily or permanently related.

Definition

Relationships between business entities are independent business concepts and shall not be embedded inside individual entities.

Examples

- Asset ↔ Asset
- Asset ↔ Component
- Asset ↔ Attachment
- Component ↔ Component

Business Value

Relationship Management enables:

- Temporary installation
- Operational grouping
- Shared operational usage
- Composite assets
- Historical relationship tracking

Future Specification

BusinessSpecification-RelationshipManagement.md

---

# 7. Cross-Cutting Domain Concepts

The following business concepts span multiple business capabilities.

## Lifecycle

Every physical business object has its own lifecycle.

Examples include:

- Assets
- Engines
- Tires
- Batteries
- Attachments
- Documents

Lifecycle history shall always be preserved.

---

## Relationships

Business objects may participate in temporary or permanent relationships.

Examples include:

- Truck + Trailer
- Truck + Crane
- Excavator + Hydraulic Hammer
- Engine installed in different Assets

Relationships shall preserve independent ownership while enabling operational linkage.

---

## Operational Usage

Usage information may originate from multiple sources.

Operational usage shall always remain traceable to its originating asset while supporting propagation where required by business rules.

---

## Historical Traceability

Every important business action shall remain historically traceable.

History is never overwritten.

History is extended.

---

# 6.x Capability Dependencies

The following diagram illustrates the high-level dependency relationships between discovered business capabilities.

Asset Management
        │
        ├──────────────┐
        │              │
        ▼              ▼
Asset Relationships   Parts Catalog
        │              │
        │              ▼
        │      Part Cross Reference
        │
        ▼
Maintenance Forecast
        │
        ▼
Work Orders
        │
        ▼
Maintenance History

Asset Management
        │
        ├──────────────┐
        ▼              ▼
Tire Management   Battery Management

Asset Management
        │
        ▼
Incident Management
        │
        ▼
Maintenance Forecast

Notification Center
        │
        ├──────────────┐
        ▼              ▼
Internal Messaging   AI Assistant

---

# 6.x Business Priority Roadmap

Business capabilities shall be specified in the following order.

| Phase | Capabilities |
|--------|--------------|
| Phase 1 | Asset Management |
| Phase 2 | Asset Relationships |
| Phase 3 | Tire Lifecycle Management |
| Phase 4 | Battery Lifecycle Management |
| Phase 5 | Parts Catalog |
| Phase 6 | Part Cross Reference |
| Phase 7 | Incident Management |
| Phase 8 | Maintenance Forecast |
| Phase 9 | Notification Center |
| Phase 10 | Internal Messaging |
| Phase 11 | AI Assistant |

This roadmap represents the recommended order of business specification and implementation.

Implementation may only begin after the corresponding Business Specification has been approved.

---

# 6.x Specification Queue

The following Business Specification documents shall be produced in order.

| Order | Specification |
|--------|---------------|
| 1 | BR-003-BusinessSpecification-AssetRelationships.md |
| 2 | BR-005-BusinessSpecification-TireLifecycle.md |
| 3 | BR-006-BusinessSpecification-BatteryLifecycle.md |
| 4 | BR-007-BusinessSpecification-PartsCatalog.md |
| 5 | BR-008-BusinessSpecification-PartCrossReference.md |
| 6 | BR-009-BusinessSpecification-IncidentManagement.md |
| 7 | BR-010-BusinessSpecification-MaintenanceForecast.md |
| 8 | BR-012-BusinessSpecification-NotificationCenter.md |
| 9 | BR-013-BusinessSpecification-InternalMessaging.md |
| 10 | BR-014-BusinessSpecification-AIAssistant.md |



Each specification shall be completed and approved before its implementation begins.

---

# 8. Discovery Rules

Every newly identified business capability shall first be added to this document.

Capabilities shall not proceed directly to implementation.

The required progression is:

Domain Discovery

↓

Business Specification

↓

Domain Model

↓

Implementation

---

# 8. Core Business Invariants

The following business invariants have been discovered during domain analysis.

These invariants are expected to remain valid throughout the lifetime of the system.

---

## BI-001

Every physical object has an independent lifecycle.

Examples include:

- Asset
- Engine
- Tire
- Battery
- Attachment

---

## BI-002

A physical object may exist independently from the asset on which it is currently installed.

---

## BI-003

Installation never changes ownership.

Installation only changes operational relationships.

---

## BI-004

Operational usage shall remain historically traceable.

Usage history is never rewritten.

---

## BI-005

Business history shall never be destroyed.

Corrections create additional history.

---

## BI-006

Business identity never changes.

Only business state changes.

---

## BI-007

Business relationships are first-class business knowledge.

Relationships themselves have history.

---

## BI-008

Forecasts are not maintenance.

Forecasts become maintenance only after approval.

---

## BI-009

Notifications never create business events.

They only inform users about business events.

---

## BI-010

Artificial Intelligence never modifies business data directly.

AI produces recommendations.

Business users remain responsible for decisions.

---

# 9. Future Domain Discovery Candidates

The following business areas have been identified as possible future discovery activities.

These areas are intentionally postponed until business priorities require them.

- Fuel Management
- Lubricant Management
- Workshop Resource Planning
- Personnel Certification
- Warranty Management
- Supplier Performance Evaluation
- Machine Telemetry
- IoT Integration
- Mobile Offline Operations
- GIS Integration
- Cost Center Analytics
- Machine Availability Analytics
- Reliability Engineering
- Failure Mode Analysis
- Condition Monitoring
- Predictive Maintenance using AI

---

# 8. Related Documents

- 01-DomainPrinciples.md
- 02-CoreConcepts.md
- 03-BoundedContexts.md
- 04-DomainModel.md
- ../02-architecture/02-CapabilityModel.md

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-20 | Solution Architect | Initial Domain Discovery catalog                      |
| 1.1.0   | 2026-07-20 | Solution Architect | Added Tracked Components as a new root business capability and renumbered subsequent discoveries |
| 1.2.0   | 2026-07-20 | Solution Architect | Added Maintenance Operations as a core discovered business capability and renumbered subsequent capabilities. |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Fixed Document ID: was DOM-009 (collided with corrected 09-StateMachines.md), corrected to DOM-010, filling a gap that had left DOM-010 unused |