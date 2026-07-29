# Domain Documentation

| Property | Value |
|----------|-------|
| **Document ID** | DOM-README |
| **Version** | 1.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-20 |
| **Last Updated** | 2026-07-20 |

---

# Purpose

This directory contains the complete business domain documentation of the
**MachineryManagerEnterprise** project.

The Domain documentation defines the business before any implementation begins.

All developers, architects and AI assistants shall start from this directory
before making any business-related decision.

---

# Reading Order

The recommended reading order is:

```text
README

↓

DomainDocumentationIndex

↓

00-DomainPrinciples

↓

DG-00-DomainGovernance

↓

09-DomainDiscovery

↓

Business Specifications

↓

Domain Model

↓

Implementation
```

---

# Directory Structure

```text
03-domain/

README.md

DomainDocumentationIndex.md

00-DomainPrinciples.md

DG-00-DomainGovernance.md

01-CoreConcepts.md

02-BoundedContexts.md

03-DomainModel.md

04-Aggregates.md

05-DomainServices.md

06-DomainEvents.md

07-BusinessRules.md

08-StateMachines.md

09-DomainDiscovery.md

specifications/
```

---

# Business Specification Directory

All Business Specifications are stored in:

```text
specifications/
```

Current structure:

```text
specifications/

BR-INDEX.md

BR-00-BusinessSpecificationTemplate.md

BR-001-BusinessSpecification-AssetRelationships.md
```

Future specifications shall also be placed in this directory.

---

# Documentation Workflow

Every business capability shall follow the lifecycle below.

```text
Business Idea

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

↓

Testing

↓

Release
```

No implementation shall bypass this workflow.

---

# Governance

The business domain is governed by:

- Domain Principles
- Domain Governance
- AI Engineering Contract
- Approved ADRs

Architectural changes require an approved ADR.

Business changes require an updated Business Specification.

---

# Current Documentation Status

Completed

- Domain Principles
- Domain Governance
- Domain Discovery
- Domain Documentation Index
- Business Specification Template
- BR-001 Asset Relationships

Planned

- Tire Management
- Battery Management
- Parts Catalog
- Part Cross Reference
- Incident Management
- Maintenance Forecast
- Notification Center
- Internal Messaging
- AI Assistant

---

# Entry Point for New Contributors

If you are new to the project, read the following documents in order:

1. README.md
2. DomainDocumentationIndex.md
3. 00-DomainPrinciples.md
4. DG-00-DomainGovernance.md
5. 09-DomainDiscovery.md
6. specifications/BR-INDEX.md
7. The relevant Business Specification

Only after understanding the business should implementation begin.

---

# Related Documents

- DomainDocumentationIndex.md
- 00-DomainPrinciples.md
- DG-00-DomainGovernance.md
- AI_ENGINEERING_CONTRACT.md

---

# Revision History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-20 | Initial Domain README |