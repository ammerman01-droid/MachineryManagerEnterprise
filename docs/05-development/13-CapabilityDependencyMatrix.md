# Capability Dependency Matrix

| Property | Value |
|----------|-------|
| **Document ID** | DD-013 |
| **Document Name** | Capability Dependency Matrix |
| **Version** | 1.0.0 |
| **Status** | Draft |
| **Owner** | Domain Architect |
| **Created** | 2026-07-20 |
| **Last Updated** | 2026-07-20 |

---

# 1. Purpose

This document defines the dependency relationships between Business Capabilities within MachineryManagerEnterprise.

The matrix determines:

- implementation order;
- architectural dependency;
- business dependency;
- integration sequencing;
- bootstrap priority.

The matrix is the authoritative implementation roadmap for all business capabilities.

---

# 2. Scope

This document covers dependencies between business capabilities only.

It does **not** describe:

- project references;
- source code dependencies;
- NuGet dependencies;
- infrastructure dependencies.

Those concerns are documented elsewhere.

---

# 3. Dependency Philosophy

Business Capabilities are independent.

However,

some capabilities require business information provided by other capabilities.

This creates a dependency graph.

The graph determines implementation sequencing.

Dependencies shall remain acyclic.

Circular capability dependencies are prohibited.

---

# 4. Dependency Types

## Business Definition

Business Capabilities may depend upon one another for different reasons.

Not every dependency has the same meaning.

The dependency type determines:

- implementation order;
- runtime interaction;
- data ownership;
- business responsibility.

Dependencies shall remain explicit.

Implicit dependencies are prohibited.

---

# Dependency Categories

Business Capability dependencies are classified into four categories.

---

## 4.1 Foundational Dependency

A capability requires another capability because it provides the foundational business model.

Example

```text
Incident Management

↓

Asset Management
```

Reason

Incidents cannot exist without Assets.

Characteristics

- mandatory
- structural
- implemented first

---

## 4.2 Context Dependency

A capability consumes business context owned by another capability.

Example

```text
AI Assistant

↓

Notification Center
```

or

```text
AI Assistant

↓

Relationship Management
```

The provider owns the information.

The consumer only reads it.

Characteristics

- read-only
- non-owning
- contextual

---

## 4.3 Operational Dependency

A capability requires another capability to perform business operations.

Example

```text
Maintenance Operations

↓

Maintenance Forecast
```

Reason

Forecast produces planned work.

Maintenance executes work.

Characteristics

- behavioral
- operational
- sequential

---

## 4.4 Integration Dependency

Two capabilities exchange business events.

Example

```text
Relationship Management

↓

Notification Center
```

Relationship changes

↓

Notification Events

↓

Notification Center

Characteristics

- event driven
- asynchronous
- loosely coupled

---

# Dependency Direction

Dependencies are always directional.

```text
Capability A

↓

depends on

↓

Capability B
```

The opposite direction shall not be assumed.

---

# Allowed Dependency

Example

```text
Incident Management

↓

Asset Management
```

Incidents require Assets.

---

# Invalid Dependency

Example

```text
Asset Management

↓

Incident Management
```

Assets do not require Incidents.

This dependency is prohibited.

---

# Dependency Strength

Dependencies have different strengths.

| Strength | Description |
|----------|-------------|
| Required | Capability cannot exist without provider |
| Recommended | Capability benefits from provider |
| Optional | Capability may operate independently |
| Event | Communication only |

---

# Dependency Lifetime

Some dependencies exist permanently.

Example

```text
Maintenance

↓

Asset
```

Permanent.

---

Some dependencies exist only during runtime.

Example

```text
Notification

↓

Relationship Event
```

Temporary.

---

# Ownership Rule

Dependencies never transfer ownership.

Example

```text
Notification Center

↓

depends on

↓

Relationship Management
```

Notification Center never owns Relationships.

Relationship Management never owns Notifications.

---

# Circular Dependency

Circular capability dependencies are prohibited.

Invalid

```text
Capability A

↓

Capability B

↓

Capability C

↓

Capability A
```

The dependency graph shall remain acyclic.

---

# Business Rules

### BR-DT-001

Every dependency shall have exactly one dependency type.

---

### BR-DT-002

Dependency direction shall always be explicit.

---

### BR-DT-003

Dependencies shall never transfer business ownership.

---

### BR-DT-004

Circular capability dependencies are prohibited.

---

### BR-DT-005

Dependency strength shall determine implementation order.

---

## Business Outcomes

Dependency Types provide:

- predictable implementation order;
- architectural consistency;
- clear ownership;
- reusable integration model;
- stable enterprise architecture.

---

# 5. Capability Dependency Matrix

## Overview

The following matrix defines business capability dependencies.

The dependency indicates that the capability in the row requires information or services provided by the capability in the column.

The matrix does not represent source-code references.

It represents business dependency only.

---

## Capability Matrix

| Capability | Depends On | Dependency Type | Strength |
|------------|------------|-----------------|----------|
| BR-001 Asset Management | — | — | Independent |
| BR-002 Tracked Components | BR-001 | Foundational | Required |
| BR-003 Meter Management | BR-001 | Foundational | Required |
| BR-004 Condition Monitoring | BR-001, BR-003 | Operational | Required |
| BR-005 Parts Catalog | — | — | Independent |
| BR-006 Inventory Management | BR-005 | Foundational | Required |
| BR-007 Incident Management | BR-001 | Foundational | Required |
| BR-008 Maintenance Forecast | BR-001, BR-002, BR-003, BR-004, BR-005, BR-007 | Operational | Required |
| BR-009 Maintenance Operations | BR-001, BR-002, BR-005, BR-006, BR-008 | Operational | Required |
| BR-010 Notification Center | BR-013 | Integration | Required |
| BR-011 Internal Messaging | BR-013 | Context | Recommended |
| BR-012 AI Assistant | BR-001, BR-002, BR-003, BR-004, BR-005, BR-006, BR-007, BR-008, BR-009, BR-010, BR-011, BR-013 | Context | Required |
| BR-013 Relationship Management | — | — | Independent |

---

## Dependency Summary

### Independent Capabilities

These capabilities have no business dependency.

- Asset Management
- Parts Catalog
- Relationship Management

They may be implemented first.

---

### Foundational Capabilities

These capabilities provide business foundations.

- Asset Management
- Parts Catalog
- Relationship Management

Most other capabilities depend on them.

---

### Operational Capabilities

Operational capabilities consume information from foundational capabilities.

Examples include:

- Maintenance Forecast
- Maintenance Operations

Operational capabilities execute business work.

---

### Context Providers

Some capabilities primarily provide enterprise context.

Examples include:

- Relationship Management
- Notification Center

These capabilities improve enterprise consistency without owning business execution.

---

### Enterprise Consumer

The AI Assistant consumes almost every capability.

It owns no business information.

It produces no business execution.

It is therefore intentionally placed near the end of the dependency graph.

---

## Dependency Principles

Business dependencies follow the following principles.

- Dependencies always point toward foundational capabilities.
- Dependencies never transfer ownership.
- Dependencies remain acyclic.
- Independent capabilities should be implemented before dependent capabilities.

---

# 6. Dependency Graph

## Layered Dependency Model

Business Capabilities are organized into implementation layers.

Higher layers depend only on lower layers.

Lower layers never depend upon upper layers.

```text
Layer 1
Foundation

────────────────────────────────────

BR-001  Asset Management

BR-005  Parts Catalog

BR-013  Relationship Management

────────────────────────────────────

↓

Layer 2
Core Business

────────────────────────────────────

BR-002  Tracked Components

BR-003  Meter Management

BR-006  Inventory Management

BR-007  Incident Management

────────────────────────────────────

↓

Layer 3
Operational Intelligence

────────────────────────────────────

BR-004  Condition Monitoring

BR-008  Maintenance Forecast

────────────────────────────────────

↓

Layer 4
Operations

────────────────────────────────────

BR-009  Maintenance Operations

────────────────────────────────────

↓

Layer 5
Enterprise Services

────────────────────────────────────

BR-010  Notification Center

BR-011  Internal Messaging

────────────────────────────────────

↓

Layer 6
Enterprise Intelligence

────────────────────────────────────

BR-012  AI Assistant

────────────────────────────────────
```

---

## Layer Responsibilities

### Layer 1 — Foundation

Provides the enterprise business foundation.

Capabilities:

- Asset Management
- Parts Catalog
- Relationship Management

Characteristics

- independent;
- reusable;
- no business dependencies.

---

### Layer 2 — Core Business

Defines operational business information.

Capabilities

- Tracked Components
- Meter Management
- Inventory Management
- Incident Management

Characteristics

- consume foundational capabilities;
- provide business information;
- no operational execution.

---

### Layer 3 — Operational Intelligence

Transforms business information into operational planning.

Capabilities

- Condition Monitoring
- Maintenance Forecast

Characteristics

- analytical;
- predictive;
- planning-oriented.

---

### Layer 4 — Operations

Executes business work.

Capability

- Maintenance Operations

Characteristics

- operational;
- transactional;
- business execution.

---

### Layer 5 — Enterprise Services

Provides enterprise communication.

Capabilities

- Notification Center
- Internal Messaging

Characteristics

- reusable;
- event-driven;
- context-aware.

---

### Layer 6 — Enterprise Intelligence

Provides enterprise reasoning.

Capability

- AI Assistant

Characteristics

- advisory;
- read-only;
- explainable;
- consumes every previous layer.

---

## Architectural Principles

Dependencies always point downward.

Example

```text
AI Assistant

↓

Maintenance Operations

↓

Maintenance Forecast

↓

Tracked Components

↓

Asset
```

Never

```text
Asset

↓

AI Assistant
```

---

No capability may depend on another capability from the same or higher layer unless explicitly approved by architecture governance.

---

## Business Outcomes

The layered dependency graph provides:

- predictable implementation order;
- stable architecture;
- loose coupling;
- reusable business capabilities;
- scalable enterprise evolution.

---

# 7. Bootstrap Order

## Purpose

The Bootstrap Order defines the recommended implementation sequence for every Business Capability.

The objective is to:

- minimize dependency conflicts;
- reduce implementation risk;
- preserve architectural integrity;
- maximize incremental delivery.

Capabilities shall be implemented according to dependency order.

---

# Bootstrap Principles

Implementation follows these principles.

1. Independent capabilities first.
2. Foundational capabilities before dependent capabilities.
3. Context providers before context consumers.
4. Operational capabilities after planning capabilities.
5. AI implementation after business capabilities stabilize.

---

# Phase 1 — Foundation

The following capabilities establish the enterprise foundation.

| Order | Capability | Reason |
|--------|------------|--------|
| 1 | BR-001 Asset Management | Core enterprise entity |
| 2 | BR-005 Parts Catalog | Shared enterprise reference |
| 3 | BR-013 Relationship Management | Enterprise relationship infrastructure |

These capabilities have no business dependencies.

They provide the foundation for every remaining capability.

---

# Phase 2 — Core Business

The following capabilities extend the domain model.

| Order | Capability | Depends On |
|--------|------------|------------|
| 4 | BR-002 Tracked Components | BR-001 |
| 5 | BR-003 Meter Management | BR-001 |
| 6 | BR-006 Inventory Management | BR-005 |
| 7 | BR-007 Incident Management | BR-001 |

These capabilities provide operational business information.

---

# Phase 3 — Planning

Planning capabilities consume business information.

| Order | Capability | Depends On |
|--------|------------|------------|
| 8 | BR-004 Condition Monitoring | BR-001, BR-003 |
| 9 | BR-008 Maintenance Forecast | BR-001, BR-002, BR-003, BR-004, BR-005, BR-007 |

Forecasting shall be implemented before operational execution.

---

# Phase 4 — Operations

Operational execution follows planning.

| Order | Capability | Depends On |
|--------|------------|------------|
| 10 | BR-009 Maintenance Operations | BR-008 |

Maintenance Operations consume Forecasts.

They never generate Forecasts.

---

# Phase 5 — Enterprise Services

Enterprise communication services are implemented after operational capabilities.

| Order | Capability | Depends On |
|--------|------------|------------|
| 11 | BR-010 Notification Center | BR-013 |
| 12 | BR-011 Internal Messaging | BR-013 |

Both capabilities rely upon organizational relationships.

---

# Phase 6 — Enterprise Intelligence

The AI Assistant is implemented last.

| Order | Capability | Depends On |
|--------|------------|------------|
| 13 | BR-012 AI Assistant | All previous capabilities |

The AI Assistant consumes enterprise context.

It owns no operational data.

---

# Bootstrap Summary

```text
Phase 1

Asset
Parts
Relationship

↓

Phase 2

Components
Meters
Inventory
Incidents

↓

Phase 3

Condition Monitoring

↓

Forecast

↓

Phase 4

Maintenance

↓

Phase 5

Notification
Messaging

↓

Phase 6

AI Assistant
```

---

# Bootstrap Rules

### BO-001

Implementation shall begin only from independent capabilities.

---

### BO-002

Capabilities shall never be implemented before their required dependencies.

---

### BO-003

Planning capabilities precede operational capabilities.

---

### BO-004

Enterprise services consume business context.

They never own business entities.

---

### BO-005

The AI Assistant shall be implemented only after business capability stabilization.

---

## Business Outcomes

Bootstrap sequencing provides:

- predictable implementation;
- reduced integration risk;
- stable architecture;
- incremental delivery;
- dependency-safe development.

---

# 8. Implementation Waves

## Purpose

Implementation Waves organize Business Capabilities into executable delivery increments.

Each wave delivers a coherent business milestone while preserving architectural integrity.

Implementation Waves are derived from:

- capability dependencies;
- architectural layering;
- business priorities;
- implementation complexity.

---

# Wave Philosophy

Each implementation wave shall:

- be independently testable;
- provide measurable business value;
- minimize dependency risk;
- preserve aggregate independence;
- enable incremental deployment.

Every wave builds upon the previous wave.

---

# Wave 1 — Enterprise Foundation

## Objective

Establish the foundational business model.

Capabilities

- BR-001 Asset Management
- BR-005 Parts Catalog
- BR-013 Relationship Management

Deliverables

- Enterprise Asset model
- Enterprise Parts model
- Relationship infrastructure
- Organizational hierarchy
- Ownership model

Business Value

The enterprise foundation becomes available for all future capabilities.

---

# Wave 2 — Core Operational Model

## Objective

Model enterprise operational data.

Capabilities

- BR-002 Tracked Components
- BR-003 Meter Management
- BR-006 Inventory Management
- BR-007 Incident Management

Deliverables

- Component lifecycle
- Meter readings
- Inventory model
- Incident lifecycle

Business Value

Operational information becomes available.

---

# Wave 3 — Predictive Maintenance

## Objective

Introduce maintenance intelligence.

Capabilities

- BR-004 Condition Monitoring
- BR-008 Maintenance Forecast

Deliverables

- Condition evaluation
- Maintenance forecasting
- Predictive planning

Business Value

Reactive maintenance evolves into predictive maintenance.

---

# Wave 4 — Maintenance Execution

## Objective

Execute maintenance work.

Capabilities

- BR-009 Maintenance Operations

Deliverables

- Work execution
- Maintenance history
- Operational workflows

Business Value

The maintenance lifecycle becomes complete.

---

# Wave 5 — Enterprise Collaboration

## Objective

Provide enterprise communication services.

Capabilities

- BR-010 Notification Center
- BR-011 Internal Messaging

Deliverables

- Notification routing
- Internal conversations
- Organizational communication

Business Value

Business participants become connected.

---

# Wave 6 — Enterprise Intelligence

## Objective

Deliver enterprise reasoning capabilities.

Capabilities

- BR-012 AI Assistant

Deliverables

- Context-aware assistant
- Explainable recommendations
- Operational intelligence

Business Value

The enterprise becomes knowledge-assisted.

---

# Wave Dependency

```text
Wave 1

↓

Wave 2

↓

Wave 3

↓

Wave 4

↓

Wave 5

↓

Wave 6
```

No wave shall begin before completion of required predecessor waves.

---

# Wave Completion Criteria

A wave is considered complete when:

- all capabilities are implemented;
- integration tests pass;
- architectural validation succeeds;
- dependency contracts remain satisfied;
- business acceptance criteria are fulfilled.

Partial completion does not close a wave.

---

# Parallel Development

Capabilities inside the same wave may be developed in parallel when they do not directly depend on one another.

Example

Wave 2

```text
Tracked Components

||

Meter Management

||

Inventory

||

Incident Management
```

Parallel implementation reduces delivery time without violating dependencies.

---

# Business Rules

### IW-001

Every implementation wave shall deliver usable business value.

---

### IW-002

Capabilities shall not move to later waves if required by earlier waves.

---

### IW-003

Parallel implementation is allowed only within the same wave.

---

### IW-004

Wave completion requires successful integration validation.

---

### IW-005

Architectural constraints shall always override scheduling preferences.

---

## Business Outcomes

Implementation Waves provide:

- predictable project delivery;
- incremental business value;
- controlled implementation risk;
- dependency-safe development;
- scalable enterprise evolution.

---

# 9. Critical Path

## Purpose

The Critical Path identifies Business Capabilities whose implementation directly determines the implementation schedule of other capabilities.

A Critical Capability blocks one or more dependent capabilities.

The Critical Path therefore defines the minimum implementation sequence required to complete the enterprise.

---

# Critical Path Definition

A capability belongs to the Critical Path when delaying its implementation prevents one or more dependent capabilities from being implemented.

Critical Path analysis is based upon business dependencies.

It is independent from project scheduling.

---

# Critical Capability Chain

The primary Critical Path of MachineryManagerEnterprise is:

```text
BR-001
Asset Management

↓

BR-002
Tracked Components

↓

BR-008
Maintenance Forecast

↓

BR-009
Maintenance Operations

↓

BR-012
AI Assistant
```

This represents the minimum business sequence required to achieve enterprise maintenance intelligence.

---

# Supporting Critical Path

A second foundational path exists.

```text
BR-005
Parts Catalog

↓

BR-006
Inventory Management

↓

BR-009
Maintenance Operations
```

Inventory cannot operate without Parts.

Maintenance cannot execute without Inventory.

---

# Enterprise Governance Path

Enterprise governance follows a separate critical path.

```text
BR-013
Relationship Management

↓

BR-010
Notification Center

↓

BR-011
Internal Messaging

↓

BR-012
AI Assistant
```

Relationship Management establishes:

- hierarchy;
- ownership;
- propagation;
- organizational context.

Without it:

- Notifications cannot resolve recipients.
- Internal Messaging cannot determine visibility.
- AI cannot build organizational context.

---

# Parallel Paths

Some capabilities are intentionally isolated from the primary path.

Example

```text
BR-003
Meter Management
```

Although required by Forecast,

it may be developed independently from:

- Inventory
- Incident Management

This allows parallel implementation.

---

# Bottleneck Analysis

The highest-impact capabilities are:

| Capability | Reason |
|------------|--------|
| BR-001 Asset Management | Root business entity |
| BR-005 Parts Catalog | Root reference data |
| BR-013 Relationship Management | Enterprise governance |
| BR-008 Maintenance Forecast | Enables Maintenance |
| BR-009 Maintenance Operations | Core business execution |
| BR-012 AI Assistant | Consumes entire enterprise context |

These capabilities form the primary architectural bottlenecks.

---

# Dependency Impact

If Asset Management is delayed:

Blocked capabilities:

- Tracked Components
- Meter Management
- Incident Management
- Condition Monitoring
- Forecast
- Maintenance
- AI

---

If Relationship Management is delayed:

Blocked capabilities:

- Notification Center
- Internal Messaging
- AI Context
- Organizational Propagation

---

If Maintenance Forecast is delayed:

Blocked capabilities:

- Maintenance Operations
- AI Recommendations

---

# Risk Classification

| Risk Level | Capability |
|------------|------------|
| Very High | BR-001 |
| Very High | BR-013 |
| High | BR-005 |
| High | BR-008 |
| High | BR-009 |
| Medium | BR-002 |
| Medium | BR-006 |
| Medium | BR-010 |
| Low | BR-003 |
| Low | BR-004 |
| Low | BR-007 |
| Low | BR-011 |
| Low | BR-012 (implementation only after previous capabilities) |

---

# Critical Path Rules

### CP-001

Critical Capabilities shall always be implemented before dependent capabilities.

---

### CP-002

Critical Capabilities shall receive architectural priority.

---

### CP-003

Parallel development shall never violate Critical Path sequencing.

---

### CP-004

Changes to Critical Capabilities require architectural review.

---

### CP-005

The Critical Path shall remain acyclic.

---

## Business Outcomes

Critical Path analysis provides:

- implementation predictability;
- dependency visibility;
- architectural stability;
- project risk reduction;
- optimized implementation sequencing.

---

# 10. Architectural Constraints

## Purpose

Architectural Constraints define the non-negotiable implementation rules governing Business Capability dependencies.

These constraints preserve:

- aggregate independence;
- bounded context isolation;
- business ownership;
- enterprise scalability;
- long-term maintainability.

Architectural constraints override implementation convenience.

---

# Dependency Constraints

Business Capabilities shall communicate only through approved architectural mechanisms.

Allowed

```text
Capability

↓

Domain Event

↓

Capability
```

Allowed

```text
Capability

↓

Application Service

↓

Capability
```

Prohibited

```text
Capability

↓

Direct Aggregate Modification

↓

Capability
```

---

# Aggregate Constraints

Every Aggregate remains autonomous.

No Business Capability may:

- modify another capability's Aggregate;
- persist another capability's Aggregate;
- own another capability's Aggregate.

Aggregates communicate through contracts only.

---

# Ownership Constraints

Ownership shall never propagate across Aggregate boundaries.

Relationship ownership

≠

Business Entity ownership.

This rule is mandatory.

---

# Layer Constraints

Dependencies shall always point downward.

```text
Foundation

↓

Core Business

↓

Operations

↓

Enterprise Services

↓

Enterprise Intelligence
```

Reverse dependencies are prohibited.

---

# Circular Dependency Constraints

Business Capabilities shall never create circular dependency chains.

Invalid

```text
A

↓

B

↓

C

↓

A
```

The dependency graph shall remain acyclic.

---

# Integration Constraints

Integration shall occur only through:

- Domain Events;
- Application Contracts;
- Approved APIs.

Direct database sharing is prohibited.

Shared persistence is prohibited.

---

# Workspace Constraints

Business Capabilities shall remain independent from Workspace topology.

Business execution may occur inside:

- Enterprise Workspace
- Project Workspace
- User Workspace

without modifying:

- Aggregate behavior;
- Business Rules;
- Capability ownership.

Workspace synchronization is an architectural concern defined by:

ADR-0012 — Distributed Workspace Architecture.

Business Capabilities shall never implement synchronization logic directly.

---

# Context Constraints

Every Business Capability owns its own context.

Capabilities consume external context.

Capabilities never own external context.

---

# Historical Constraints

Historical information shall never be overwritten.

Updates produce:

- new versions;
- new events;
- new projections.

Historical integrity is mandatory.

---

# AI Constraints

The AI Assistant shall remain a consumer.

The AI Assistant shall never:

- own business entities;
- execute business operations;
- bypass business rules;
- bypass authorization;
- bypass workflow.

AI produces recommendations only.

---

# Notification Constraints

Notification Center owns notifications.

Relationship Management owns routing.

Authorization owns permissions.

Responsibilities shall remain separated.

---

# Messaging Constraints

Internal Messaging owns conversations.

Relationship Management owns visibility.

The two capabilities remain independent.

---

# Extensibility Constraints

Future Business Capabilities shall:

- reuse existing foundational capabilities;
- avoid duplicate business models;
- follow dependency rules;
- preserve aggregate boundaries.

New capabilities shall never introduce architectural shortcuts.

---

# Architectural Rules

### AC-001

Business Capabilities shall remain independently deployable.

---

### AC-002

Aggregate ownership shall never cross capability boundaries.

---

### AC-003

Dependencies shall remain acyclic.

---

### AC-004

Context propagation shall use Domain Events.

---

### AC-005

Historical data shall remain immutable.

---

### AC-006

AI shall remain advisory.

---

### AC-007

Enterprise Services shall remain infrastructure providers.

They shall never become business owners.

---

## Architectural Outcomes

Architectural Constraints provide:

- long-term maintainability;
- stable bounded contexts;
- scalable implementation;
- independent aggregates;
- predictable dependency evolution;
- sustainable enterprise architecture.

---

# 11. Related Documents

## Architecture

The following architectural documents define the structural foundation of this matrix.

- 02-architecture/01-Architecture.md
- 02-architecture/09-CapabilityModel.md
- 02-architecture/06-decisions/ADR-0012-DistributedWorkspaceArchitecture.md

---

## Development

The following development documents complement this specification.

- 05-development/01-SolutionStructure.md
- 05-development/02-ProjectStructure.md
- 05-development/03-DependencyCatalog.md
- 05-development/04-DependencyRules.md
- 05-development/05-BuildPipeline.md
- 05-development/12-DomainPatterns.md

---

## Business Specifications

Capability Dependency Matrix references the following Business Specifications.

- BR-001 Asset Management
- BR-002 Tracked Components
- BR-003 Meter Management
- BR-004 Condition Monitoring
- BR-005 Parts Catalog
- BR-006 Inventory Management
- BR-007 Incident Management
- BR-008 Maintenance Forecast
- BR-009 Maintenance Operations
- BR-010 Notification Center
- BR-011 Internal Messaging
- BR-012 AI Assistant
- BR-013 Relationship Management

---

## Relationship to Domain Patterns

The implementation sequence defined by this document assumes the reusable architectural behaviors described by:

- DP-001 Business Operation Pattern
- DP-003 Lifecycle Pattern
- DP-004 Relationship Pattern
- DP-006 Business Traceability Pattern
- DP-009 Hierarchical Relationship Pattern

---

## Relationship to Implementation

This document becomes the primary reference for:

- project bootstrap;
- implementation sequencing;
- commit planning;
- sprint planning;
- integration sequencing;
- architectural validation.

No implementation roadmap shall contradict this document.

Distributed implementations shall additionally comply with:

- ADR-0012 — Distributed Workspace Architecture

Synchronization architecture shall not modify capability dependencies defined by this document.

It only defines how validated business changes are propagated between Workspaces.

---

# 12. Revision History

| Version | Date | Description |
|----------|------------|------------------------------------------------|
| 1.0.0 | 2026-07-20 | Initial Capability Dependency Matrix |