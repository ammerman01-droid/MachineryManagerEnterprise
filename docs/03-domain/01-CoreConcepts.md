# Core Concepts

| Property | Value |
|----------|-------|
| **Document ID** | DOM-002 |
| **Version** | 3.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# 1. Purpose

This document defines the fundamental business concepts of MachineryManagerEnterprise.

It establishes the official meaning of every core concept used throughout the project and serves as the primary reference for:

- Domain Modeling
- Entity Design
- Aggregate Design
- Business Rules
- Database Design
- APIs
- User Interface
- Reports
- Forecasting

Every future document inside the Domain section shall conform to the concepts defined in this document.

---

# 2. Scope

This document covers only **business concepts**.

It intentionally avoids implementation details such as:

- Database tables
- Entity Framework
- API contracts
- UI components
- Programming language constructs

Those subjects are documented elsewhere.

---

# 3. Domain Philosophy

MachineryManagerEnterprise is an Enterprise Asset Lifecycle Management System.

The software is not intended to manage machines only.

Its purpose is to manage the complete business lifecycle of physical assets, their components, their history, their documents, their financial information and every business event occurring during their lifetime.

The domain model is built around business meaning rather than technical implementation.

---

# 4. Domain Taxonomy

The following taxonomy represents the highest level classification of the business domain.

```text
Enterprise

├── Organization

├── People

├── Assets
│
│   ├── Asset Models
│   ├── Asset Instances
│   ├── Asset Lifecycle
│
├── Components
│
│   ├── Engine
│   ├── Transmission
│   ├── Hydraulic System
│   ├── Electrical System
│   ├── Attachment
│   ├── Meter Device
│
├── Usage
│
│   ├── Meter Readings
│   ├── Operational Usage
│   ├── Non-operational Usage
│
├── Maintenance
│
│   ├── Inspection
│   ├── Preventive Maintenance
│   ├── Corrective Maintenance
│   ├── Repairs
│   ├── Failures
│
├── Finance
│
│   ├── Purchase
│   ├── Depreciation
│   ├── Operating Costs
│   ├── Ownership Cost
│
├── Documents
│
│   ├── Ownership
│   ├── Insurance
│   ├── Licenses
│   ├── Technical Documents
│
├── Knowledge
│
│   ├── Manuals
│   ├── Parts Catalogs
│   ├── Repair Guides
│
├── Media
│
│   ├── Images
│   ├── Gallery
│
└── Forecast

    ├── Fuel
    ├── Lubricants
    ├── Filters
    ├── Consumables
    ├── Planned Maintenance
```

The taxonomy is intended to classify business concepts only.

It is **not** a database schema.

It is **not** a software architecture.

It is **not** an Entity Relationship Diagram.

---

# 5. Core Business Concepts

The following concepts form the foundation of the entire system.

Every future entity, aggregate and business rule shall be traceable to one or more of these concepts.

## 5.1 Asset

An Asset is a physical piece of equipment owned, leased or managed by an organization.

An Asset has:

- Identity
- Lifecycle
- Operational history
- Financial history
- Maintenance history
- Documentation
- Components

An Asset is the primary business object of the system.

Deleting an Asset is prohibited.

Assets may become:

- Active
- Inactive
- Retired
- Sold
- Scrapped

Their history always remains preserved.

---

## 5.2 Asset Model

An Asset Model defines the common specifications shared by multiple physical assets.

Examples include:

- Manufacturer
- Product Family
- Model Name
- Technical Specifications
- Compatible Engines
- Compatible Attachments
- Default Maintenance Plans

Asset Models are templates.

They are not physical objects.

---

## 5.3 Asset Instance

An Asset Instance is an individual physical machine.

Every Asset Instance possesses:

- Unique Identity
- Serial Number
- Purchase Information
- Current Status
- Current Location
- Current Components
- Complete Historical Record

Business operations are performed on Asset Instances rather than Asset Models.
---

## 5.4 Engine

An Engine is an independent physical business asset that provides power for an Asset.

An Engine is **not permanently bound** to an Asset.

During its lifecycle an Engine may:

- Be installed on a new Asset.
- Be removed for repair.
- Be rebuilt.
- Be stored in inventory.
- Be transferred to another Asset.
- Be retired.
- Be scrapped.

Because of this independent lifecycle, the Engine shall always be modeled as an independent business entity.

### Engine Identity

Every Engine possesses its own permanent identity including:

- Engine Serial Number
- Engine Model
- Manufacturer
- Manufacturing Year
- Current Status

The Engine identity never changes.

Only its relationship with Assets changes.

### Engine Lifecycle

The lifecycle of an Engine is independent from the lifecycle of an Asset.

Examples:

- An Asset may be retired while its Engine continues operating on another Asset.
- A used Engine may be installed on a newly purchased Asset.
- A rebuilt Engine keeps its identity while acquiring a new operational state.

Therefore, Engine history shall never be deleted.

---

## 5.5 Engine Model

An Engine Model represents the technical definition shared by many Engines.

Typical characteristics include:

- Manufacturer
- Model Name
- Fuel Type
- Number of Cylinders
- Engine Configuration
- Rated Power
- Rated Torque
- Cooling System
- Lubrication Capacity
- Standard Maintenance Intervals

Engine Models are reusable templates.

They do not represent physical Engines.

---

## 5.6 Component

A Component is a replaceable physical part belonging to an Asset.

Components may include:

- Engine
- Transmission
- Hydraulic Pump
- Starter Motor
- Alternator
- Battery
- Tires
- Tracks
- Attachments

Some Components possess independent identities.

Others only exist as replaceable parts.

The domain distinguishes between:

### Independent Components

These have their own lifecycle.

Examples:

- Engine
- Transmission

### Dependent Components

These exist only as part of another Asset.

Examples:

- Belts
- Hoses
- Filters
- Bearings

The replacement of a Component creates a historical business event.

The previous Component remains part of the historical record.

---

## 5.7 Meter Device

A Meter Device is the physical instrument responsible for measuring accumulated usage.

Examples include:

- Hour Meter
- Odometer
- Cycle Counter

A Meter Device is **not** the same as Usage.

It only records observations.

Meter Devices themselves have lifecycles.

A Meter Device may:

- Fail.
- Be replaced.
- Be repaired.
- Be recalibrated.

Therefore, Meter Devices are modeled separately from Meter Readings.

---

## 5.8 Meter Reading

A Meter Reading is an observation recorded from a Meter Device at a specific point in time.

A Meter Reading contains:

- Reading Value
- Reading Time
- Reading Source
- Meter Device
- Recorder

Meter Readings are immutable.

Existing readings are never edited.

Corrections are represented by new business events.

---

## 5.9 Operational Usage

Operational Usage represents productive work performed by an Asset.

Examples include:

- Engine Running Hours
- Distance Travelled
- Production Cycles

Operational Usage contributes to:

- Maintenance Planning
- Depreciation
- Forecasting
- Operating Costs
- Remaining Useful Life

---

## 5.10 Non-operational Usage

Non-operational Usage represents recorded usage that shall **not** contribute to business calculations.

Examples include:

- Defective Hour Meter
- Electrical Fault
- Calibration Error
- Test Operation
- Maintenance Operation

The system shall preserve this information for audit purposes while excluding it from maintenance planning and lifecycle calculations.

This distinction is fundamental to MachineryManagerEnterprise.

Business calculations shall always be based on Operational Usage rather than raw Meter Readings.

---

## 5.11 Maintenance

Maintenance is any planned or unplanned activity performed to preserve, restore or improve the operational capability of an Asset or one of its Components.

Maintenance is one of the primary business activities within the system.

Every Maintenance activity becomes part of the permanent lifecycle history.

Maintenance shall never be deleted.

Typical maintenance categories include:

- Preventive Maintenance
- Corrective Maintenance
- Predictive Maintenance
- Emergency Maintenance
- Overhaul
- Inspection

Maintenance may affect:

- Asset
- Engine
- Independent Component
- Multiple Components simultaneously

---

## 5.12 Inspection

Inspection is a business activity performed to determine the current condition of an Asset or Component.

An Inspection does not necessarily modify the Asset.

Its primary purpose is to collect business information.

Inspection results may include:

- Pass
- Pass with Recommendations
- Minor Defects
- Major Defects
- Unsafe for Operation

Inspection may generate:

- Maintenance Requests
- Work Orders
- Failure Reports
- Forecast Updates

Inspection history shall always be preserved.

---

## 5.13 Failure

A Failure is an event indicating that an Asset or Component is no longer capable of performing its intended function.

Failures may be:

- Mechanical
- Hydraulic
- Electrical
- Electronic
- Structural
- Operational

A Failure is an observed business event.

A Failure is not a Repair.

One Failure may generate multiple Repair activities.

---

## 5.14 Repair

Repair is a business activity performed to restore functionality following a Failure or Inspection.

Repair activities may include:

- Replacement
- Adjustment
- Rebuilding
- Calibration
- Welding
- Machining

Every Repair shall maintain complete traceability including:

- Cause
- Technician
- Date
- Labor
- Parts Used
- Cost
- Downtime

Repairs contribute to Total Cost of Ownership.

---

## 5.15 Replacement

Replacement is the business event in which one physical Component is removed and another Component is installed.

Replacement does not destroy historical relationships.

Instead it creates a new lifecycle event.

Example:

Engine A

↓

Installed on Asset X

↓

Removed

↓

Stored

↓

Installed on Asset Y

↓

Rebuilt

↓

Installed on Asset Z

All relationships remain permanently traceable.

---

## 5.16 Financial Record

Financial information represents every monetary event occurring during the lifecycle of an Asset.

Examples include:

- Purchase Price
- Transportation
- Registration
- Insurance
- Taxes
- Maintenance Cost
- Fuel Cost
- Lubricants
- Spare Parts
- External Services

Financial records are immutable business facts.

Corrections are represented by additional financial transactions.

---

## 5.17 Depreciation

Depreciation represents the calculated reduction in the economic value of an Asset over time.

The system shall always preserve:

- Original Purchase Value
- Current Estimated Value
- Depreciation Method
- Depreciation History

Calculated values shall never overwrite historical acquisition values.

---

## 5.18 Document

A Document is any official or technical file associated with a business object.

Examples include:

- Ownership Certificate
- Insurance Policy
- Registration
- Warranty
- Inspection Certificate
- Calibration Certificate
- Purchase Invoice

Documents possess independent metadata including:

- Issue Date
- Expiration Date
- Issuing Authority
- Version
- Status

Expired documents remain available as historical records.

---

## 5.19 Technical Library

The Technical Library stores reusable technical knowledge related to Asset Models rather than individual Assets.

Examples include:

- Operator Manuals
- Workshop Manuals
- Parts Catalogs
- Hydraulic Schematics
- Electrical Schematics
- Service Bulletins

One document may serve hundreds of Assets sharing the same Model.

This separation prevents unnecessary duplication.

---

## 5.20 Gallery

The Gallery stores visual history related to business objects.

Images may document:

- Purchase Condition
- Damage
- Repairs
- Component Replacement
- Failures
- Periodic Inspections
- Restoration

Images are business evidence.

They participate in lifecycle documentation.

---

## 5.21 Forecast

Forecast represents the prediction of future business requirements based on historical observations.

Forecasts may include:

- Fuel Consumption
- Lubricant Consumption
- Coolant Consumption
- Filter Replacement
- Spare Parts Demand
- Planned Maintenance
- Component Replacement
- Expected Operating Cost

Forecasts are derived from business history.

Forecasts never modify historical facts.

---

# 6. Relationship Between Core Concepts

The following conceptual relationships define the foundation of the domain.

```
Asset Model
      │
      ▼
Asset Instance
      │
      ├─────────────┐
      ▼             ▼
 Components     Documents
      │             │
      ▼             ▼
 Engine      Technical Library

      ▼
 Meter Device
      ▼
 Meter Reading
      ▼
 Operational Usage
      ▼
 Forecast

Maintenance
      ▲
      │
Inspection
      │
Failure
      │
Repair
      │
Replacement

Financial Records
      │
Depreciation
```

These relationships describe business meaning rather than implementation.

Future Domain Models shall refine these relationships without contradicting them.


---

# 7. Ubiquitous Language

The project shall use a single consistent business language across:

- Documentation
- Source Code
- Database
- APIs
- User Interface
- Reports

Every technical implementation shall respect the business terminology defined in this document.

Whenever a conflict exists between a technical term and a business term, the business term shall prevail.

---

## 7.1 General Naming Principles

Business names shall be:

- Clear
- Stable
- Unambiguous
- Independent from implementation technology

Names shall describe business meaning rather than software behavior.

---

## 7.2 Business Object Naming

The following names are considered official throughout the project.

| Official Term | Description |
|---------------|-------------|
| Asset | Physical machine or equipment |
| Asset Model | Technical template shared by many Assets |
| Asset Instance | One physical Asset |
| Component | Replaceable physical part |
| Engine | Independent power unit |
| Engine Model | Technical definition of an Engine |
| Meter Device | Physical measuring instrument |
| Meter Reading | Observation collected from a Meter Device |
| Operational Usage | Productive accumulated usage |
| Non-operational Usage | Usage excluded from lifecycle calculations |
| Inspection | Condition assessment activity |
| Failure | Loss of intended functionality |
| Repair | Activity restoring functionality |
| Replacement | Exchange of physical Components |
| Forecast | Prediction derived from historical data |

---

# 8. Preferred Terms

The following terminology should always be preferred.

| Preferred | Avoid |
|-----------|-------|
| Asset | Machine |
| Asset Model | Machine Type |
| Asset Instance | Machine Record |
| Component | Part |
| Meter Device | Counter |
| Meter Reading | Counter Value |
| Operational Usage | Working Hours |
| Non-operational Usage | Invalid Hours |
| Replacement | Swap |
| Lifecycle | Status History |
| Technical Library | Manuals Folder |
| Gallery | Pictures |

These preferred terms shall be used consistently throughout the project.

---

# 9. Forbidden Terms

The following expressions shall not be used because they create ambiguity.

## Delete Asset

Incorrect.

Use:

- Retire Asset
- Archive Asset
- Dispose Asset

---

## Delete Engine

Incorrect.

Engines possess independent lifecycles.

Use:

- Remove Engine
- Retire Engine
- Scrap Engine

---

## Current Hours

Ambiguous.

Use:

- Operational Usage
- Meter Reading

---

## Machine Type

Ambiguous.

Use:

- Asset Model

---

## Counter

Too generic.

Use:

- Meter Device

---

## Manual

Too generic.

Use:

- Technical Library Document

---

## Cost

Ambiguous.

Use one of:

- Purchase Cost
- Maintenance Cost
- Operating Cost
- Ownership Cost

---

# 10. Design Implications

The concepts defined within this document directly influence future architectural decisions.

## Domain Model

Every Aggregate shall originate from one or more Core Concepts.

---

## Database

Tables shall represent business concepts rather than UI requirements.

---

## API

API resources shall follow business terminology.

---

## User Interface

Screens shall expose business language rather than technical language.

---

## Reporting

Reports shall present business concepts consistently with this document.

---

## Forecasting

Prediction engines shall consume Operational Usage rather than raw Meter Readings.

---

# 11. References

This document shall be interpreted together with:

- 00-DomainPrinciples.md
- Glossary.md
- 09-CapabilityModel.md
- PROJECT_CHARTER.md

Future related documents include:

- 02-BoundedContexts.md
- 03-DomainModel.md
- 04-Aggregates.md

---

# Related Documents

- 00-DomainPrinciples.md
- 00-Glossary.md
- 09-CapabilityModel.md
- 01-Architecture.md
- 00-Vision.md

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | Initial Draft | Initial Core Concepts |
| 3.0.0 | 2026-07-18 | Standardized according to Documentation Standard v3.0 |