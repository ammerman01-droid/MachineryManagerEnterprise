| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ARCH-002           |
| **Title**        | Capability Model   |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

This document defines the Business Capability Model of the MachineryManagerEnterprise platform.

A capability describes **what the business is able to do**, independent of technical implementation.

The Capability Model is the foundation for:

- Domain Driven Design
- Module Boundaries
- Navigation Structure
- Permission System
- API Design
- Reporting
- Future Microservice Extraction

This document intentionally focuses on business capabilities rather than software components.

---

# Core Principle

The platform is designed around the concept of **Asset**, not **Machine**.

Every maintainable object with an independent lifecycle is considered an Asset.

Examples include:

- Heavy Machines
- Trucks
- Forklifts
- Engines
- Attachments
- Generators
- Compressors
- Future Equipment Types

This principle guarantees long-term extensibility.

---

# Capability Hierarchy

```text
Enterprise Asset Lifecycle Platform

├── Organization Management
│
├── Asset Management
│   ├── Asset Registration
│   ├── Asset Classification
│   ├── Asset Models
│   ├── Asset Specifications
│   ├── Asset Lifecycle
│   ├── Asset Status
│   ├── Asset Ownership
│   ├── Asset Assignment
│   └── Asset Retirement
│
├── Component Management
│   ├── Engine Management
│   ├── Transmission Management
│   ├── Attachment Management
│   ├── Replaceable Components
│   ├── Component Installation
│   ├── Component Removal
│   ├── Component Transfer
│   ├── Component Rebuild
│   └── Component History
│
├── Meter Management
│   ├── Hour Meter
│   ├── Odometer
│   ├── Meter Replacement
│   ├── Operational Usage
│   ├── Non-operational Usage
│   ├── Meter Validation
│   └── Usage History
│
├── Maintenance Management
│   ├── Preventive Maintenance
│   ├── Corrective Maintenance
│   ├── Breakdown Management
│   ├── Work Orders
│   ├── Service Scheduling
│   ├── Maintenance History
│   └── Service Costs
│
├── Fuel & Lubrication
│   ├── Fuel Consumption
│   ├── Engine Oil
│   ├── Hydraulic Oil
│   ├── Gear Oil
│   ├── Coolant
│   ├── Grease
│   └── Consumption History
│
├── Spare Parts Management
│   ├── Parts Catalog
│   ├── Inventory
│   ├── Stock Transactions
│   ├── Suppliers
│   ├── Purchase History
│   └── Consumption History
│
├── Financial Management
│   ├── Purchase Information
│   ├── Initial Value
│   ├── Current Value
│   ├── Depreciation
│   ├── Operating Costs
│   ├── Maintenance Costs
│   ├── Ownership Cost
│   └── Cost Analysis
│
├── Document Management
│   ├── Ownership Documents
│   ├── Insurance
│   ├── Annual Licenses
│   ├── Contracts
│   ├── Certificates
│   ├── Manuals
│   ├── Parts Books
│   ├── Technical Documents
│   └── Expiration Tracking
│
├── Media Management
│   ├── Image Gallery
│   ├── Videos
│   ├── Attachments
│   ├── Event Albums
│   └── Export
│
├── Knowledge Management
│   ├── Repair Manuals
│   ├── Parts Catalogs
│   ├── Technical Bulletins
│   ├── Best Practices
│   └── Shared Documents
│
├── Forecasting
│   ├── Fuel Forecast
│   ├── Lubricant Forecast
│   ├── Filter Forecast
│   ├── Spare Parts Forecast
│   ├── Maintenance Forecast
│   └── Budget Forecast
│
├── Notifications
│   ├── Maintenance Alerts
│   ├── Expiring Documents
│   ├── Warranty Alerts
│   ├── Inspection Alerts
│   └── Custom Notifications
│
├── Reporting & Analytics
│   ├── Operational Reports
│   ├── Financial Reports
│   ├── Maintenance Reports
│   ├── KPI Dashboard
│   ├── Cost Analysis
│   └── Performance Analysis
│
└── Administration
    ├── Users
    ├── Roles
    ├── Permissions
    ├── Audit Logs
    ├── Settings
    └── System Configuration
```

---

# Capability Design Principles

Every capability shall:

- represent a business responsibility;
- remain independent from implementation technology;
- have clear ownership;
- expose explicit business terminology;
- evolve independently whenever possible.

Capabilities must never be defined based on database tables or UI pages.

---

# Capability Relationships

Capabilities collaborate but remain loosely coupled.

Examples:

- Maintenance consumes Meter data.
- Forecasting consumes historical Maintenance and Fuel data.
- Notifications consume Maintenance, Documents and Warranty data.
- Financial Management consumes Purchase and Maintenance information.
- Reporting consumes data from all business capabilities.

All business capabilities are designed to operate independently of deployment topology.

Business operations may execute inside Enterprise, Project or User Workspaces without changing Business Rules.

Distributed synchronization propagates validated business changes between Workspaces while preserving business consistency.

This behavior is defined by ADR-0012 (Distributed Workspace Architecture).

---

# Future Expansion

The capability model intentionally supports future expansion, including:

- Multi-company environments
- Fleet management
- IoT integration
- Telematics
- Predictive maintenance
- AI-assisted diagnostics
- Mobile applications
- Distributed Workspace synchronization
- Offline-first project execution
- Bidirectional synchronization between Enterprise, Project and User Workspaces
- External ERP integration
- GIS integration
- Digital inspections

---

# Module Mapping

The capability model is expected to evolve into application modules.

Examples:

| Capability | Future Module |
|------------|---------------|
| Organization | Organization |
| Asset Management | Asset |
| Component Management | Components |
| Maintenance | Maintenance |
| Inventory | Inventory |
| Finance | Finance |
| Documents | Documents |
| Knowledge | Knowledge |
| Forecasting | Forecasting |
| Reporting | Reporting |

Module boundaries will be refined during Domain Modeling.

---

# Out of Scope

The following areas are intentionally excluded from the current scope:

- Accounting
- Payroll
- Human Resources
- CRM
- ERP Replacement

The platform may integrate with such systems but will not replace them.

---

# Related Documents

- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`
- `00-TechnologyEvaluationTemplate.md`
- `01-Architecture.md`
- `03-TechnologyGapAnalysis.md`
- `../06-decisions/000-ADR-INDEX.md`
- `TE-0001-.NET10.md` through `TE-0035-Reporting-Technology-Evaluation.md`
- `../03-domain/02-BoundedContexts.md`

# Revision History

---

# Change History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial capability model                              |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0; added links to ADR Master Index and all 35 TEs |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |