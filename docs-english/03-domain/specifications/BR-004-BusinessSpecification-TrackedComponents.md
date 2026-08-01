| Property | Value |
|----------|-------|
| **Document ID** | BR-002 |
| **Version** | 4.0.0 |
| **Status** | Draft |
| **Owner** | Solution Architect |
| **Created** | 2026-07-20 |
| **Last Updated** | 2026-07-28 |

---

# 1. Purpose

This specification defines the common business behavior shared by every Tracked Component.

All specialized tracked component specifications shall inherit the business rules defined in this document and shall only define component-specific behavior.

A Tracked Component is a reusable physical component that possesses its own independent identity and lifecycle regardless of the Asset on which it is installed.

This document establishes the common business behavior shared by all tracked components.

It intentionally avoids component-specific rules, which are defined in specialized Business Specifications.

---

# 2. Business Problem

In heavy equipment operations, many expensive physical components are repeatedly transferred between different Assets.

Examples include:

- Tires
- Batteries
- Engines
- Gearboxes
- Hydraulic Attachments
- Buckets
- Winches
- Compressors

Although installed on Assets, these components represent valuable business assets whose operational history must be preserved independently.

Without lifecycle tracking:

- maintenance history becomes fragmented;
- operating cost becomes inaccurate;
- ownership becomes unclear;
- usage cannot be calculated correctly;
- future planning becomes unreliable.

---

# 3. Business Objectives

The system shall:

- preserve complete lifecycle history;
- preserve component identity;
- allow installation on multiple Assets throughout its lifetime;
- preserve installation history;
- preserve operational history;
- preserve financial history;
- support forecasting and reporting.

The objectives defined in this specification apply to every Tracked Component regardless of its physical type.

---

# 4. Business Definition

A **Tracked Component** is defined as:

> A physical component that has its own permanent identity and lifecycle and may be installed on, removed from, and transferred between multiple Assets during its operational life.

A Tracked Component remains the same business object regardless of where it is installed.

---

# 5. Examples

This specification applies to every business object classified as a Tracked Component.

Examples include (non-exhaustive):

- Tire
- Battery
- Engine
- Gearbox
- Hydraulic Hammer (Pikour)
- Bucket
- Generator
- Compressor
- Winch

Additional tracked component types may be introduced without modifying this specification.

---

# 6. Fundamental Characteristics

Every Tracked Component shall possess:

- Permanent Identity
- Independent Lifecycle
- Operational History
- Installation History
- Removal History
- Transfer History
- Financial History
- Current Operational State

---

# 7. Identity Rules

Every tracked component shall have a permanent identity.

Identity never changes.

Examples:

- Manufacturer Serial Number
- Internal Tracking Number
- RFID
- QR Code

Business history shall always follow the component identity.

---

# 8. Lifecycle Rules

Tracked Components progress through business states.

Typical lifecycle:

```text
Purchased

↓

Warehouse

↓

Installed

↓

Operating

↓

Removed

↓

Warehouse

↓

Installed Again

↓

Retired

↓

Disposed
```

Historical lifecycle transitions shall never be deleted.

---

# 9. Installation Rules

A tracked component may be:

- Installed
- Removed
- Reinstalled
- Relocated

Every installation event shall preserve:

- Date
- Time
- Asset
- Position (if applicable)
- Installer
- Reason

---

# 10. Transfer Rules

Transfer between Assets creates business history.

Transfer shall never overwrite previous installation records.

The system shall preserve:

Source Asset

↓

Destination Asset

↓

Transfer Date

↓

Transfer Reason

---

# 11. Operational Usage

Operational usage belongs to the component.

Usage may be:

- inherited from the host Asset;
- calculated independently;
- manually recorded.

The calculation strategy depends on the component type.

Specialized specifications define these rules.

---

# 12. Financial Rules

Every tracked component owns independent financial information.

Examples:

- Purchase Cost
- Supplier
- Warranty
- Remaining Value
- Repair Cost
- Replacement Cost

Financial history shall never be overwritten.

---

# 13. Historical Preservation

The following records are immutable:

- Installation History
- Removal History
- Transfer History
- Maintenance History
- Financial History
- Operational History

Current state represents only the latest snapshot.

---

# 14. Business Constraints

The system shall prevent:

- duplicate identities;
- installation conflicts;
- inconsistent lifecycle states;
- orphan historical records.

---

---

# 15. Common Reporting Requirements

Every Tracked Component shall support reporting for:

- Component Inventory
- Current Location
- Installation History
- Operational History
- Maintenance History
- Lifecycle Distribution
- Retirement Statistics

Specialized specifications may extend these reports.

---

# 16. Common Business Analytics

Every Tracked Component shall expose historical operational knowledge suitable for analytical processing.

Typical analytical capabilities include:

- Lifetime Analysis
- Failure Distribution
- Cost Analysis
- Operational Trend Analysis
- Replacement Analysis

Specialized specifications may introduce additional analytical capabilities.

---

# 17. Derived Specifications

This specification serves as the foundation for:

- BR-003 — Tire Lifecycle Management
- BR-004 — Battery Lifecycle Management
- BR-005 — Engine Lifecycle Management
- BR-006 — Gearbox Lifecycle Management
- BR-007 — Attachment Lifecycle Management
- Future tracked component specifications

Derived specifications shall extend this document rather than duplicate its rules.

---

# 18. Related Documents

- BR-001 Business Specification — Asset Relationships
- DOM-009 Domain Discovery
- DOM-000 Domain Principles
- DG-00 Domain Governance

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# Revision History

| Version | Date       | Author             | Description                                                       |
|---------|------------|--------------------|-------------------------------------------------------------------|
| 1.0.0   | 2026-07-20 | Solution Architect | Initial specification for common tracked component business rules |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0             |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0                         |