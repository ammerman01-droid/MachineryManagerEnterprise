| Property | Value |
|----------|-------|
| **Document ID** | DD-014 |
| **Title** | Aggregate Dependency Matrix |
| **Version** | 4.0.0 |
| **Status** | Draft |
| **Owner** | Domain Architect |
| **Created** | 2026-07-20 |
| **Last Updated** | 2026-07-28 |

---

# 1. Purpose

This document defines dependency relationships between Aggregates inside MachineryManagerEnterprise.

The objective is to:

- preserve Aggregate independence;
- define Aggregate interaction;
- identify Aggregate ownership;
- support implementation sequencing;
- support Domain Driven Design.

The document becomes the authoritative reference for Aggregate interactions.

---

# 2. Scope

This document covers:

- Aggregate dependencies;
- Aggregate ownership;
- Aggregate interaction;
- Aggregate lifecycle dependencies.

This document does not define:

- Business Rules;
- Application Services;
- Project References;
- Infrastructure.

---

# 3. Aggregate Dependency Philosophy

Aggregates are autonomous consistency boundaries.

An Aggregate shall never depend on another Aggregate through direct ownership.

Instead,

Aggregates interact through:

- identifiers;
- domain events;
- domain services;
- repositories;
- application services.

Aggregate autonomy shall always be preserved.

---

# Aggregate Independence

Correct

```text
Aggregate A

↓

Identifier

↓

Aggregate B
```

Incorrect

```text
Aggregate A

↓

Object Reference

↓

Aggregate B
```

Direct Aggregate references are prohibited.

---

# Aggregate Ownership

Every Aggregate owns only its own consistency boundary.

Ownership never crosses Aggregate boundaries.

Example

```text
MaintenanceOperation

owns

Maintenance Tasks
```

but

```text
MaintenanceOperation

does NOT own

Asset
```

Asset remains independently owned.

---

# Dependency Principles

Aggregate dependencies shall satisfy:

- explicit;
- directional;
- acyclic;
- ownership-safe;
- event-friendly.

Every dependency shall have business meaning.

---

# Business Outcomes

Aggregate Dependency Philosophy ensures:

- loose coupling;
- aggregate autonomy;
- scalable domain model;
- maintainable implementation;
- enterprise consistency.

---

# 4. Dependency Types

## Business Definition

Aggregate dependencies exist for different architectural reasons.

Not every dependency represents ownership.

The dependency type determines:

- consistency boundary;
- interaction model;
- implementation strategy;
- persistence behavior.

Dependencies shall remain explicit.

---

# Dependency Categories

Aggregate dependencies are classified into five categories.

---

## 4.1 Identity Dependency

One Aggregate stores only the identifier of another Aggregate.

Example

```text
MaintenanceOperation

↓

AssetId
```

Characteristics

- no ownership
- loose coupling
- preferred dependency

---

## 4.2 Reference Dependency

An Aggregate requires business information from another Aggregate.

The Aggregate never owns that information.

Example

```text
Incident

↓

Asset
```

Incident needs Asset information.

Incident never owns Asset.

Characteristics

- read-only
- non-owning
- contextual

---

## 4.3 Domain Event Dependency

Aggregates communicate through Domain Events.

Example

```text
ForecastCreated

↓

MaintenanceOperation
```

Characteristics

- asynchronous
- loosely coupled
- event-driven

---

## 4.4 Service Dependency

Two Aggregates cooperate through a Domain Service.

Example

```text
Forecast

↓

ForecastCalculationService

↓

ConditionMonitoring
```

Neither Aggregate owns the other.

Characteristics

- coordination
- stateless
- reusable

---

## 4.5 Historical Dependency

One Aggregate requires historical information from another Aggregate.

Example

```text
MaintenanceHistory

↓

MaintenanceOperation
```

Characteristics

- immutable
- traceable
- versioned

---

# Allowed Dependency

```text
Aggregate

↓

Identifier

↓

Aggregate
```

---

# Prohibited Dependency

```text
Aggregate

↓

owns

↓

Aggregate
```

Aggregate ownership across boundaries is prohibited.

---

# Dependency Direction

Dependencies always point toward the information provider.

```text
Consumer

↓

Provider
```

Never the reverse.

---

# Dependency Lifetime

Some dependencies are permanent.

Example

```text
TrackedComponent

↓

Asset
```

---

Others exist only during execution.

Example

```text
Notification

↓

Relationship Event
```

---

# Dependency Strength

| Strength | Description |
|----------|-------------|
| Required | Aggregate cannot function without provider |
| Context | Aggregate consumes provider information |
| Event | Aggregate reacts to provider events |
| Historical | Aggregate consumes immutable history |

---

# Business Rules

### AD-001

Aggregate dependencies shall never transfer ownership.

---

### AD-002

Aggregate dependencies shall remain directional.

---

### AD-003

Aggregate dependencies shall remain explicit.

---

### AD-004

Identifier Dependency is the preferred dependency.

---

### AD-005

Circular Aggregate dependencies are prohibited.

---

## Architectural Outcomes

Dependency Types provide:

- loose coupling;
- aggregate autonomy;
- explicit interaction;
- scalable persistence;
- DDD compliance.

---

# 5. Aggregate Dependency Matrix

## Overview

The following matrix defines dependencies between Aggregates.

The matrix describes business interaction only.

It does not imply object ownership.

---

## Aggregate Dependency Matrix

| Aggregate | Depends On | Dependency Type | Strength |
|------------|------------|-----------------|----------|
| Asset | — | — | Independent |
| TrackedComponent | Asset | Identity | Required |
| Meter | Asset | Identity | Required |
| ConditionAssessment | Asset, Meter | Reference | Required |
| Part | — | — | Independent |
| Inventory | Part | Identity | Required |
| InventoryTransaction | Inventory, Part | Identity | Required |
| Incident | Asset | Identity | Required |
| MaintenanceForecast | Asset, TrackedComponent, Meter, ConditionAssessment, Incident, Part | Reference | Required |
| MaintenanceOperation | Asset, TrackedComponent, Part, Inventory, MaintenanceForecast | Reference | Required |
| Notification | Relationship | Event | Required |
| Conversation | Relationship | Context | Recommended |
| Relationship | — | — | Independent |
| AIConversation | Asset, TrackedComponent, Meter, Part, Inventory, Incident, MaintenanceForecast, MaintenanceOperation, Notification, Conversation, Relationship | Context | Required |

---

## Independent Aggregates

The following Aggregates have no business dependency.

- Asset
- Part
- Relationship

These Aggregates establish enterprise foundations.

---

## Identity Dependency Examples

```text
TrackedComponent

↓

AssetId
```

TrackedComponent stores only AssetId.

It never owns Asset.

---

```text
Inventory

↓

PartId
```

Inventory never owns Part.

---

## Reference Dependency Examples

```text
MaintenanceForecast

↓

Asset

↓

Meter

↓

ConditionAssessment

↓

Incident
```

Forecast consumes information.

Forecast owns none of these Aggregates.

---

## Event Dependency Examples

```text
RelationshipUpdated

↓

Notification
```

Relationship publishes.

Notification reacts.

---

## Context Dependency Examples

```text
AIConversation

↓

Conversation

↓

Notification

↓

Relationship
```

AI consumes enterprise context.

AI owns nothing.

---

## Aggregate Independence

Every Aggregate remains independently persistent.

Every Aggregate owns:

- its state;
- its lifecycle;
- its invariants.

No Aggregate modifies another Aggregate directly.

---

## Dependency Summary

```text
Asset

↓

TrackedComponent

↓

Forecast

↓

MaintenanceOperation

↓

AIConversation
```

and

```text
Part

↓

Inventory

↓

MaintenanceOperation
```

and

```text
Relationship

↓

Notification

↓

Conversation

↓

AIConversation
```

These represent the three primary Aggregate dependency paths.

---

# 6. Aggregate Interaction Rules

## Business Definition

Aggregates collaborate through well-defined interaction mechanisms.

Interaction shall preserve:

- Aggregate autonomy;
- consistency boundaries;
- business ownership;
- transactional integrity.

Aggregates shall never directly manipulate another Aggregate.

---

# Interaction Principles

Every Aggregate interaction shall satisfy:

- explicit intent;
- clear ownership;
- transactional safety;
- bounded consistency.

---

# Allowed Interaction

Aggregates may interact through:

- Aggregate Identifier
- Domain Event
- Domain Service
- Application Service

These mechanisms preserve Aggregate independence.

---

## Aggregate Identifier

The preferred interaction mechanism.

Example

```text
MaintenanceOperation

↓

AssetId
```

The Aggregate stores only the identifier.

Asset remains independently loaded.

---

## Domain Events

Aggregates notify other Aggregates through Domain Events.

Example

```text
RelationshipUpdated

↓

NotificationCreated
```

The publishing Aggregate has no knowledge of subscribers.

---

## Domain Services

Domain Services coordinate multiple Aggregates.

Example

```text
ForecastCalculationService

↓

Asset

↓

Meter

↓

ConditionAssessment
```

The Domain Service performs coordination.

No Aggregate owns another.

---

## Application Services

Application Services orchestrate business use cases.

Example

```text
CreateMaintenanceOperation

↓

Forecast Repository

↓

Asset Repository

↓

Maintenance Repository
```

Application Services coordinate Aggregates.

Aggregates remain autonomous.

---

# Prohibited Interaction

Aggregates shall never:

- hold object references to another Aggregate;
- modify another Aggregate's state;
- bypass repositories;
- bypass business services.

Example

Invalid

```text
MaintenanceOperation

↓

Asset.Update()
```

---

Correct

```text
MaintenanceOperation

↓

AssetId

↓

Application Service

↓

Asset Repository
```

---

# Repository Rule

Each Aggregate has its own Repository.

Example

```text
AssetRepository

TrackedComponentRepository

ForecastRepository

MaintenanceRepository
```

Repositories shall never return graphs spanning multiple Aggregates.

---

# Transaction Boundary

Every Aggregate defines its own transaction boundary.

Transactions spanning multiple Aggregates shall be coordinated externally.

Never internally.

---

# Consistency Rule

Aggregates guarantee:

- immediate consistency inside themselves;
- eventual consistency across Aggregate boundaries.

Cross-Aggregate immediate consistency is prohibited.

---

# AI Interaction

The AI Assistant never interacts with Aggregates directly.

Example

```text
AI

↓

Application Query

↓

Read Model

↓

Response
```

AI consumes projections.

It does not load Aggregate graphs.

---

# Reporting Interaction

Reports consume Read Models.

Reports never consume Aggregate graphs.

This preserves Aggregate performance.

---

# Notification Interaction

Notification Center consumes Domain Events.

Notification Center never modifies the originating Aggregate.

---

# Internal Messaging Interaction

Internal Messaging consumes propagated context.

Conversation ownership remains local.

---

# Business Rules

### AIR-001

Aggregate interaction shall use Aggregate Identifiers whenever possible.

---

### AIR-002

Cross-Aggregate communication shall use Domain Events.

---

### AIR-003

Application Services coordinate Aggregates.

Aggregates never coordinate each other.

---

### AIR-004

Repositories shall remain Aggregate-specific.

---

### AIR-005

Aggregate graphs shall never cross Aggregate boundaries.

---

### AIR-006

Transactions spanning multiple Aggregates shall be externally coordinated.

---

### AIR-007

Read Models shall replace Aggregate graphs for reporting and AI.

---

## Architectural Outcomes

Aggregate Interaction Rules provide:

- loose coupling;
- transactional safety;
- scalable persistence;
- predictable orchestration;
- DDD compliance;
- future microservice readiness.

---

# 7. Aggregate Dependency Graph

## Purpose

The Aggregate Dependency Graph visualizes the dependency topology of the Domain Model.

Unlike the Capability Dependency Graph, this graph focuses on Aggregate interactions.

The graph represents:

- dependency direction;
- ownership boundaries;
- Aggregate autonomy;
- implementation sequencing.

---

# Enterprise Aggregate Topology

```text
                    Asset
                      │
      ┌───────────────┼────────────────┐
      │               │                │
      ▼               ▼                ▼
TrackedComponent    Meter          Incident
      │               │                │
      └───────────────┼────────────────┘
                      ▼
            ConditionAssessment
                      │
                      ▼
            MaintenanceForecast
                      │
                      ▼
          MaintenanceOperation


Part
 │
 ▼
Inventory
 │
 ▼
MaintenanceOperation


Relationship
 │
 ├──────────────► Notification
 │
 └──────────────► Conversation
                     │
                     ▼
               AIConversation


AIConversation
 │
 ├── Asset
 ├── TrackedComponent
 ├── Meter
 ├── Part
 ├── Inventory
 ├── Incident
 ├── ConditionAssessment
 ├── MaintenanceForecast
 ├── MaintenanceOperation
 ├── Notification
 ├── Conversation
 └── Relationship
```

---

# Interpretation

The graph shall be interpreted as:

```text
Aggregate

↓

Consumes Information From

↓

Aggregate
```

It shall never be interpreted as:

```text
Aggregate

↓

Owns

↓

Aggregate
```

---

# Foundation Aggregates

Foundation Aggregates establish the Domain.

- Asset
- Part
- Relationship

Every remaining Aggregate depends directly or indirectly upon one or more of them.

---

# Operational Aggregates

Operational Aggregates execute enterprise business.

- MaintenanceForecast
- MaintenanceOperation

These Aggregates consume multiple business contexts.

They own only their own consistency boundaries.

---

# Enterprise Service Aggregates

Communication Aggregates depend only upon enterprise context.

```text
Relationship

↓

Notification

↓

Conversation
```

These Aggregates remain isolated from operational business logic.

---

# AI Aggregate

AIConversation consumes enterprise knowledge.

```text
Read Models

↓

AIConversation
```

The AI Aggregate shall never become a business owner.

It shall never participate in operational transactions.

---

# Aggregate Clusters

The graph naturally forms three independent clusters.

## Operational Cluster

```text
Asset

↓

TrackedComponent

↓

Forecast

↓

Maintenance
```

---

## Inventory Cluster

```text
Part

↓

Inventory

↓

Maintenance
```

---

## Collaboration Cluster

```text
Relationship

↓

Notification

↓

Conversation

↓

AI
```

Clusters remain loosely coupled.

---

# Graph Constraints

The Aggregate Dependency Graph shall always satisfy:

- directed edges;
- no ownership transfer;
- no circular dependencies;
- Aggregate autonomy;
- explicit dependency.

---

# Business Outcomes

The Aggregate Dependency Graph provides:

- Domain visibility;
- Aggregate understanding;
- implementation guidance;
- architectural validation;
- scalable domain evolution.

---

# 8. Aggregate Ownership

## Purpose

Aggregate Ownership defines the exact responsibility boundary of every Aggregate.

Every Aggregate owns only its own consistency boundary.

Ownership determines:

- transactional responsibility;
- lifecycle authority;
- invariant enforcement;
- persistence responsibility.

Ownership shall always remain explicit.

---

# Ownership Principle

An Aggregate owns:

- its state;
- its lifecycle;
- its invariants;
- its internal entities;
- its value objects.

An Aggregate never owns another Aggregate.

---

# Ownership Matrix

| Aggregate | Owns | Never Owns |
|------------|------|------------|
| Asset | Asset state, Asset lifecycle | Components, Parts, Incidents |
| TrackedComponent | Component state | Asset |
| Meter | Meter readings | Asset |
| ConditionAssessment | Assessment results | Meter, Asset |
| Part | Part definition | Inventory |
| Inventory | Inventory quantities | Part |
| InventoryTransaction | Transaction history | Inventory |
| Incident | Incident lifecycle | Asset |
| MaintenanceForecast | Forecast records | Asset, Component |
| MaintenanceOperation | Maintenance execution | Forecast, Asset, Inventory |
| Notification | Notification lifecycle | Relationship |
| Conversation | Messages | Relationship |
| Relationship | Relationship graph | Business entities |
| AIConversation | AI conversation history | Business entities |

---

# Ownership Examples

## Asset

Asset owns:

- Asset properties
- Asset lifecycle
- Asset business rules

Asset does not own:

- Components
- Incidents
- Forecasts
- Maintenance

Only their identifiers may be referenced.

---

## Maintenance Forecast

Forecast owns:

- Forecast calculations
- Forecast lifecycle
- Forecast recommendations

Forecast never owns:

- Assets
- Components
- Incidents

Forecast consumes information only.

---

## Maintenance Operation

Maintenance Operation owns:

- Work execution
- Work history
- Operational lifecycle

Maintenance Operation never owns:

- Forecast
- Inventory
- Asset

These Aggregates remain autonomous.

---

## Relationship

Relationship owns:

- Parent-child links
- Organizational hierarchy
- Ownership propagation rules

Relationship never owns:

- Asset
- User
- Notification
- Conversation

It owns only the relationships.

---

## Notification

Notification owns:

- Notification lifecycle
- Delivery status
- Delivery history

Notification never owns:

- Relationship
- User hierarchy
- Conversation

---

## AI Conversation

AIConversation owns:

- Prompt history
- Conversation history
- AI interaction metadata

AIConversation never owns:

- Assets
- Forecasts
- Incidents
- Notifications
- Relationships

It consumes them through read models.

---

# Ownership Boundary

Ownership shall always stop at Aggregate boundaries.

```text
Aggregate

↓

Boundary

↓

Ownership Ends
```

No Aggregate may cross this boundary.

---

# Transaction Boundary

Ownership determines transaction scope.

Every Aggregate commits independently.

Cross-Aggregate transactions are prohibited.

---

# Persistence Boundary

Every Aggregate persists independently.

Example

```text
AssetRepository

ForecastRepository

MaintenanceRepository
```

Repositories shall never persist multiple Aggregates together.

---

# Business Rules

### AO-001

Every Aggregate shall own exactly one consistency boundary.

---

### AO-002

Aggregate ownership shall never overlap.

---

### AO-003

Aggregates shall reference external Aggregates by identifier only.

---

### AO-004

Ownership shall determine transaction scope.

---

### AO-005

Ownership shall determine repository responsibility.

---

### AO-006

Historical ownership shall remain immutable.

Ownership changes produce history.

They never overwrite history.

---

## Architectural Outcomes

Aggregate Ownership provides:

- clear responsibility;
- transaction isolation;
- independent persistence;
- maintainable domain model;
- scalable enterprise architecture.

---

# 9. Aggregate Lifecycle Dependencies

## Purpose

Aggregate Lifecycle Dependencies define the creation, evolution, and retirement order of Aggregates.

Although Aggregates remain autonomous,

their business lifecycles are not independent.

Lifecycle dependencies ensure business consistency while preserving Aggregate autonomy.

---

# Lifecycle Principle

Aggregate lifecycle dependency does **not** imply Aggregate ownership.

It only defines business sequencing.

Example

```text
Asset

↓

TrackedComponent
```

The Component cannot exist before the Asset.

The Asset still does not own the Component Aggregate.

---

# Creation Dependencies

The following creation order shall be respected.

| Aggregate | Requires Existing |
|------------|------------------|
| Asset | — |
| Part | — |
| Relationship | — |
| TrackedComponent | Asset |
| Meter | Asset |
| Inventory | Part |
| InventoryTransaction | Inventory |
| Incident | Asset |
| ConditionAssessment | Asset, Meter |
| MaintenanceForecast | Asset, Component, Incident |
| MaintenanceOperation | Forecast |
| Notification | Relationship |
| Conversation | Relationship |
| AIConversation | Enterprise Context |

---

# Update Dependencies

Some Aggregate updates require information from other Aggregates.

Example

```text
MaintenanceForecast

↓

reads

ConditionAssessment
```

Forecast is recalculated.

ConditionAssessment remains unchanged.

---

Another example

```text
MaintenanceOperation

↓

consumes

Inventory
```

Inventory quantity changes.

MaintenanceOperation remains owner of its own lifecycle.

---

# Deletion Dependencies

Some Aggregates cannot be removed while dependent Aggregates still exist.

Example

```text
Asset

↓

TrackedComponent

↓

MaintenanceHistory
```

Asset retirement shall follow business retirement procedures.

Hard deletion is prohibited.

---

Example

```text
Part

↓

Inventory

↓

InventoryTransaction
```

Historical Inventory Transactions must remain available.

Part retirement shall preserve historical integrity.

---

# Historical Dependencies

Historical Aggregates are immutable.

Example

```text
MaintenanceOperation

↓

MaintenanceHistory
```

History is never rewritten.

Updates produce additional history.

---

# AI Lifecycle

The AIConversation lifecycle depends upon enterprise context.

```text
Relationship

↓

Conversation

↓

AIConversation
```

Removing enterprise context does not remove historical AI conversations.

Historical explainability shall be preserved.

---

# Notification Lifecycle

```text
Relationship

↓

Notification

↓

Delivery
```

Notification ownership remains local.

Relationship changes only influence routing.

---

# Lifecycle Diagram

```text
Asset
   │
   ├────────► TrackedComponent
   │
   ├────────► Meter
   │
   ├────────► Incident
   │
   └────────► ConditionAssessment
                     │
                     ▼
            MaintenanceForecast
                     │
                     ▼
           MaintenanceOperation


Part
 │
 ▼
Inventory
 │
 ▼
InventoryTransaction


Relationship
 │
 ├────────► Notification
 │
 └────────► Conversation
                   │
                   ▼
             AIConversation
```

---

# Lifecycle Rules

### AL-001

Aggregate creation shall respect lifecycle dependencies.

---

### AL-002

Aggregate updates shall never bypass dependency rules.

---

### AL-003

Historical Aggregates shall remain immutable.

---

### AL-004

Aggregate retirement shall preserve historical integrity.

---

### AL-005

Lifecycle dependency shall never violate Aggregate autonomy.

---

### AL-006

Soft deletion shall be preferred over hard deletion whenever dependent historical data exists.

---

## Business Outcomes

Lifecycle Dependencies provide:

- predictable creation order;
- historical integrity;
- consistent evolution;
- safe retirement;
- enterprise traceability.

---

# 10. Aggregate Integration Rules

## Purpose

Aggregate Integration Rules define the permitted mechanisms for communication between Aggregates.

Integration shall preserve:

- Aggregate autonomy;
- transactional consistency;
- bounded contexts;
- long-term scalability.

Aggregates shall collaborate.

Aggregates shall never become coupled.

---

# Integration Principle

Aggregates never communicate directly.

Every interaction shall occur through one of the approved integration mechanisms.

---

# Approved Integration Mechanisms

Aggregates may communicate through:

- Aggregate Identifier
- Domain Event
- Domain Service
- Application Service
- Read Model

No other mechanism is permitted.

---

## Identifier Integration

Preferred interaction.

```text
MaintenanceOperation

↓

AssetId
```

Only the identifier is stored.

The Asset Aggregate is loaded independently when required.

---

## Domain Event Integration

Aggregates publish business events.

Other Aggregates react independently.

Example

```text
MaintenanceForecastCreated

↓

MaintenanceOperation
```

Characteristics

- asynchronous
- loosely coupled
- scalable

---

## Domain Service Integration

Business rules spanning multiple Aggregates are coordinated by Domain Services.

Example

```text
ForecastCalculationService

↓

Asset

↓

Meter

↓

ConditionAssessment

↓

Incident
```

The Domain Service owns coordination.

Aggregates remain autonomous.

---

## Application Service Integration

Application Services orchestrate use cases.

Example

```text
CreateMaintenanceOperation

↓

Forecast Repository

↓

Inventory Repository

↓

Maintenance Repository
```

Application Services never own business rules.

They orchestrate Aggregate collaboration.

---

## Read Model Integration

Read Models provide cross-Aggregate views.

Example

```text
Asset

Component

Forecast

Maintenance

↓

Maintenance Dashboard
```

Read Models never become Aggregates.

They are read-only projections.

---

# AI Integration

The AI Assistant interacts only with Read Models.

Example

```text
Read Models

↓

AI Assistant
```

The AI Assistant shall never:

- load Aggregate graphs;
- execute Aggregate logic;
- modify Aggregate state.

---

# Reporting Integration

Reporting consumes Read Models.

Reporting shall never query Aggregate graphs directly.

This preserves performance and Aggregate independence.

---

# Notification Integration

Relationship Management publishes events.

Notification Center consumes events.

```text
RelationshipUpdated

↓

NotificationCreated
```

Neither Aggregate owns the other.

---

# Messaging Integration

Conversation visibility depends on propagated context.

Relationship Management provides context.

Internal Messaging owns conversations.

---

# Integration Constraints

The following integrations are prohibited.

### Direct Aggregate Calls

Invalid

```text
Aggregate

↓

Aggregate
```

---

### Shared Persistence

Invalid

```text
Repository

↓

Multiple Aggregates
```

---

### Shared Transactions

Invalid

```text
Aggregate A

+

Aggregate B

↓

Single Transaction
```

---

### Cross-Aggregate Ownership

Invalid

```text
Aggregate

↓

owns

↓

Aggregate
```

---

# Integration Rules

### AI-001

Aggregates shall communicate only through approved integration mechanisms.

---

### AI-002

Domain Events are the preferred asynchronous mechanism.

---

### AI-003

Application Services orchestrate business use cases.

---

### AI-004

Read Models replace Aggregate graphs for queries.

---

### AI-005

Aggregate autonomy shall never be violated.

---

### AI-006

Repositories remain Aggregate-specific.

---

### AI-007

Shared persistence is prohibited.

---

## Architectural Outcomes

Aggregate Integration Rules provide:

- scalable collaboration;
- independent Aggregates;
- event-driven architecture;
- reusable orchestration;
- DDD compliance;
- future microservice readiness.

---

# 11. Related Documents

## Architecture

The following architectural documents provide the structural foundation for this specification.

- 02-architecture/01-Architecture.md
- 02-architecture/09-CapabilityModel.md

---

## Development

The following development documents complement this specification.

- 05-development/01-SolutionStructure.md
- 05-development/02-ProjectStructure.md
- 05-development/03-DependencyCatalog.md
- 05-development/04-DependencyRules.md
- 05-development/05-BuildPipeline.md
- 03-development/12-DomainPatterns.md
- 05-development/12-CapabilityDependencyMatrix.md

---

## Business Specifications

Aggregate definitions originate from the Business Specifications.

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

Aggregate interactions assume the reusable behaviors defined by:

- DP-001 Business Operation Pattern
- DP-002 Aggregate Pattern
- DP-003 Lifecycle Pattern
- DP-004 Relationship Pattern
- DP-005 Notification Pattern
- DP-006 Business Traceability Pattern
- DP-009 Hierarchical Relationship Pattern
- DP-010 AI Advisory Pattern

---

# 12. Architectural Position

Aggregate Dependency Matrix occupies the lowest architectural abstraction level before implementation.

The documentation hierarchy is:

```text
Vision

↓

Architecture

↓

Business Specification

↓

Capability Dependency Matrix

↓

Aggregate Dependency Matrix

↓

Implementation
```

This document defines:

- Aggregate boundaries;
- Aggregate interaction;
- Aggregate ownership;
- Aggregate sequencing;
- Aggregate orchestration.

Implementation shall conform to this document.

No implementation may introduce Aggregate relationships that contradict this specification.

Changes to Aggregate dependencies require architectural review.

Changes to Aggregate ownership require domain review.

---

## Architectural Responsibility

This document is the authoritative reference for:

- Aggregate creation;
- Repository boundaries;
- Domain Services;
- Domain Events;
- Aggregate interaction;
- Domain Model implementation.

Every Aggregate implementation shall be traceable to this document.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# 13. Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-20 | Solution Architect | Initial Aggregate Dependency Matrix                   |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
