| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOM-INDEX          |
| **Title**        | DomainDocumentationIndex |
| **Version**      | 4.2.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document provides the master index of all Domain documentation within the
MachineryManagerEnterprise project.

It defines:

- the documentation hierarchy,
- the role of every document,
- current completion status,
- planned documentation,
- traceability between documents.

This document is the primary navigation entry point for the Domain documentation.

All developers, architects and AI assistants shall start from this
document before making any business-related decision.

---

# 1a. Reading Order

The recommended reading order is:

```text
DomainDocumentationIndex

↓

00-Glossary

↓

01-DomainPrinciples

↓

DG-00-DomainGovernance

↓

10-DomainDiscovery

↓

Business Specifications

↓

Domain Model

↓

Implementation
```

If you are new to the project, read the following documents in order:

1. DomainDocumentationIndex.md (this document)
2. 00-Glossary.md
3. 01-DomainPrinciples.md
4. DG-00-DomainGovernance.md
5. 10-DomainDiscovery.md
6. specifications/BR-001-INDEX.md
7. The relevant Business Specification

Only after understanding the business should implementation begin.

---

# 1b. Directory Structure

```text
03-domain/

DomainDocumentationIndex.md

00-Glossary.md

01-DomainPrinciples.md

DG-00-DomainGovernance.md

02-CoreConcepts.md

03-BoundedContexts.md

04-DomainModel.md

05-Aggregates.md

06-DomainServices.md

07-DomainEvents.md

08-BusinessRules.md

09-StateMachines.md

10-DomainDiscovery.md

11-UbiquitousLanguage.md

12-DomainPatterns.md

specifications/
```

---

# 2. Domain Documentation Architecture

The Domain documentation follows the lifecycle below.

```text
Vision

↓

Domain Principles

↓

Domain Governance

↓

Capability Model

↓

Domain Discovery

↓

Business Specifications

↓

Domain Model

↓

Implementation
```

Each layer depends on the previous one.

---

# 3. Documentation Layers

| Layer | Purpose |
|--------|---------|
| Principles | Define constitutional business rules |
| Governance | Define domain engineering process |
| Discovery | Register business capabilities |
| Specification | Describe business behaviour |
| Domain Model | Model the business |
| Implementation | Build the software |

---

# 4. Document Catalog

## Foundation

| Document | Status | Purpose |
|----------|--------|---------|
| 00-Glossary.md | Complete | Ubiquitous language reference |
| 01-DomainPrinciples.md | Complete | Constitutional rules governing the business domain |
| DG-00-DomainGovernance.md | Complete | Domain lifecycle and governance process |

---

## Discovery

| Document | Status | Purpose |
|----------|--------|---------|
| 10-DomainDiscovery.md | Active | Registry of discovered business capabilities |

---

## Specifications

Location

```text
docs/03-domain/specifications/
```

| Document | Status | Purpose |
|----------|--------|---------|
| BR-001-INDEX.md | Active | Registry of all Business Specifications — authoritative source for specification status |
| BR-002-BusinessSpecificationTemplate.md | Complete | Standard template for all Business Specifications |

> **Note:** See `BR-001-INDEX.md` for the current, authoritative list
> and status of every Business Specification; not duplicated here.

---

## Business Specification Directory

All Business Specifications are stored in:

```text
specifications/
```

Current structure:

```text
specifications/

BR-001-INDEX.md

BR-002-BusinessSpecificationTemplate.md

BR-003-BusinessSpecification-AssetRelationships.md

BR-004-BusinessSpecification-TrackedComponents.md

BR-005-BusinessSpecification-TireLifecycle.md

BR-006-BusinessSpecification-BatteryLifecycle.md

BR-007-BusinessSpecification-PartsCatalog.md

BR-008-BusinessSpecification-PartCrossReference.md

BR-009-BusinessSpecification-IncidentManagement.md

BR-010-BusinessSpecification-MaintenanceForecast.md

BR-011-BusinessSpecification-MaintenanceOperations.md

BR-012-BusinessSpecification-NotificationCenter.md

BR-013-BusinessSpecification-InternalMessaging.md

BR-014-BusinessSpecification-AIAssistant.md

BR-015-BusinessSpecification-RelationshipManagement.md

BR-016-BusinessSpecification-DistributedWorkspaceSynchronization.md

BR-017-BusinessSpecification-OrganizationManagement.md
```

Future specifications shall also be placed in this directory. See
BR-001-INDEX.md for the current, authoritative status of each.

---

## Domain Modeling

| Document | Status | Purpose |
|----------|--------|---------|
| 04-DomainModel.md | Existing | Domain structural model |
| 05-Aggregates.md | Existing | Aggregate definitions |
| 06-DomainServices.md | Existing | Domain Services |
| 07-DomainEvents.md | Existing | Domain Events |
| 08-BusinessRules.md | Existing | Atomic business rules |
| 09-StateMachines.md | Existing | Entity lifecycle state machines |
| 12-DomainPatterns.md | Existing | Reusable domain design patterns |

---

# 5. Current Completion Status

| Area | Status |
|------|--------|
| Domain Principles | ✅ Complete |
| Domain Governance | ✅ Complete |
| Domain Discovery | ✅ Active |
| Business Specification Template | ✅ Complete |
| Business Specifications (15 of 15 drafted) | ✅ See BR-001-INDEX.md for per-specification status |
| Domain Documentation Index | ✅ Complete |

---

# 6. Planned Business Specifications

> **Note:** See `BR-001-INDEX.md` for the current list, priorities,
> and status of every Business Specification; not duplicated here.

---

# 7. Traceability

Every implemented feature shall be traceable through the following chain.

```text
Vision

↓

Capability Model

↓

Domain Discovery

↓

Business Specification

↓

Domain Model

↓

Application Layer

↓

Implementation

↓

Testing

↓

Release
```

No implementation shall bypass this chain.

---

# 8. Governance Rules

All Domain documentation shall comply with:

- Domain Principles
- Domain Governance
- AI Engineering Contract
- Documentation Standards

Business Specifications shall always be created from the official template.

No Business Specification may be implemented before approval.

Architectural changes require an approved ADR.

Business changes require an updated Business Specification.

---

# 9. Maintenance Rules

Whenever a new Business Specification is created:

- Update BR-INDEX.md
- Update DomainDocumentationIndex.md
- Update DomainDiscovery.md if necessary

Whenever a Business Specification changes state:

- Planned → Active
- Active → Approved
- Approved → Implemented

the status shall also be updated in this document.

---

# 10. Related Documents

- 01-DomainPrinciples.md
- DG-00-DomainGovernance.md
- 10-DomainDiscovery.md
- specifications/BR-001-INDEX.md
- specifications/BR-002-BusinessSpecificationTemplate.md
- AI_ENGINEERING_CONTRACT.md

---


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

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Domain Documentation Index                    |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Refreshed Document Catalog and Planned Specifications sections, which had gone stale referencing only the original single specification (BR-001-BusinessSpecification-AssetRelationships.md, itself a pre-renumbering filename) despite 15 specifications now existing; both sections now point to BR-001-INDEX.md as the single maintained source of truth to prevent recurrence |
| 4.2.0   | 2026-08-08 | Solution Architect | Merged README.md into this document — the two files had overlapping, independently-maintained content (reading order, directory listing, completion status) that had drifted out of sync with each other in the past. This is now the single entry point for 03-domain documentation; README.md has been deleted and all references to it updated |