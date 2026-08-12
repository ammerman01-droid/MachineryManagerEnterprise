| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ADR-0016           |
| **Title**        | Enterprise Messaging Architecture |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-26         |
| **Last Updated** | 2026-08-08         |

# Purpose

This Architecture Decision Record defines the Enterprise Messaging Architecture used by the MachineryManagerEnterprise platform.

The purpose of this ADR is to establish the architectural responsibilities, communication boundaries and messaging principles required to enable reliable interaction between autonomous architectural components while preserving loose coupling and Clean Architecture principles.

Selection of messaging technologies, communication protocols and infrastructure components is intentionally outside the scope of this ADR and shall be addressed through future Technical Evaluations.

---

# 1. Context

The MachineryManagerEnterprise platform is composed of multiple autonomous architectural components including Enterprise Services, distributed Workspaces, business modules and external integration points.

ADR-0015 establishes that synchronization is an independent architectural capability.

To support scalable communication between independent components, the platform requires a messaging architecture that avoids direct dependencies while allowing components to exchange information in a controlled and consistent manner.

The messaging architecture must remain independent from specific messaging technologies and infrastructure products.

---

# 2. Problem Statement

The platform requires a messaging architecture capable of answering the following architectural questions:

- How do independent architectural components communicate?
- Which architectural layers are permitted to publish messages?
- Which architectural layers are permitted to consume messages?
- How are communication responsibilities separated from business logic?
- How can communication remain technology independent?
- How can future messaging technologies be adopted without modifying business rules?

Without a common messaging architecture, different modules could introduce incompatible communication mechanisms, increase coupling between components and reduce maintainability.

---

# 3. Decision

The MachineryManagerEnterprise platform adopts an architecture in which messaging is treated as an independent architectural capability responsible for exchanging information between autonomous architectural components while preserving loose coupling.

Messaging is not part of business logic.

Messaging is an infrastructure capability that enables communication without introducing architectural dependencies.

---

## D-001 — Architectural Separation

Business logic shall never communicate directly with other architectural components.

All inter-component communication shall occur through the Messaging Architecture.

---

## D-002 — Loose Coupling

Publishers and consumers shall remain logically independent.

A publishing component shall never depend on the implementation details, execution state or availability of consuming components.

---

## D-003 — Message Ownership

A message represents information that has already become valid within the originating component.

Messages do not transfer ownership.

Ownership of business entities remains governed by ADR-0014.

---

## D-004 — Architectural Message Categories

The architecture recognizes the following logical message categories:

- Domain Events
- Integration Events
- Notifications
- Commands

These categories represent architectural responsibilities only.

Their implementation mechanisms are outside the scope of this ADR.

---

## D-005 — Technology Independence

The messaging architecture shall remain independent from:

- Message Brokers
- Communication Protocols
- Transport Technologies
- Serialization Formats
- Cloud Messaging Platforms

Technology selection shall be documented separately through Technical Evaluations.

---

## D-006 — Reliability

Messaging shall be designed assuming that message delivery may be delayed.

Business logic shall never require immediate message delivery in order to complete a business transaction.

---

## D-007 — Synchronization Relationship

Workspace Synchronization (ADR-0015) may use the Messaging Architecture as one possible communication mechanism.

However:

- Synchronization is not Messaging.
- Messaging is not Synchronization.

Each remains an independent architectural capability.

---

# 4. Architectural Principles

The Enterprise Messaging Architecture is governed by the following principles.

---

## AP-001 — Asynchronous by Default

Architectural communication shall be asynchronous by default.

Synchronous communication shall be introduced only where immediate interaction is an explicit architectural requirement.

---

## AP-002 — Loose Coupling

Messaging shall minimize dependencies between architectural components.

Components communicate through message contracts rather than direct knowledge of one another.

---

## AP-003 — Message Immutability

Published messages represent completed business facts.

Once published, a message shall be treated as immutable.

Corrections shall be communicated through new messages rather than modifying previously published messages.

---

## AP-004 — Business Isolation

Business logic shall remain independent from messaging infrastructure.

Business components publish architectural messages without knowledge of transport mechanisms.

---

## AP-005 — Reliability over Immediacy

Architectural reliability takes precedence over immediate delivery.

Temporary delivery delays are acceptable provided architectural consistency is preserved.

---

## AP-006 — Technology Neutrality

Messaging architecture shall remain independent from implementation technologies.

Selection of brokers, transports and communication frameworks shall not affect business architecture.

---

# 5. Architecture Overview

The Enterprise Messaging Architecture provides an architectural communication layer between autonomous components.

```text
               Enterprise Components
                        │
        ┌───────────────┼───────────────┐
        │               │               │
        ▼               ▼               ▼
   Business Module  Workspace      Integration
        │               │               │
        └───────────────┼───────────────┘
                        │
                        ▼
             Messaging Architecture
                        │
        ┌───────────────┼───────────────┐
        │               │               │
        ▼               ▼               ▼
   Message Routing  Message Delivery  Message Consumption
```

The Messaging Architecture is responsible for transporting architectural messages between components.

Business execution remains independent from message delivery.

The architecture intentionally separates:

- Business Execution
- Message Publication
- Message Transport
- Message Consumption

This separation preserves Clean Architecture while allowing messaging infrastructure to evolve independently.

---

# 6. Architectural Constraints

The following constraints are mandatory.

## AC-001 — Business Isolation

Business components shall never communicate directly with messaging infrastructure.

---

## AC-002 — Transport Independence

Business architecture shall remain independent from:

- Message Brokers
- Queues
- Service Bus technologies
- Communication protocols
- Serialization formats

---

## AC-003 — Authoritative Data

Messages shall never become the authoritative source of business data.

Authoritative ownership remains defined by ADR-0014.

---

## AC-004 — Idempotent Consumption

Architectural message processing shall assume that duplicate delivery may occur.

Consumers shall therefore be capable of idempotent processing.

---

## AC-005 — Independent Evolution

Messaging infrastructure may evolve independently from:

- Domain Layer
- Application Layer
- Business Modules

provided architectural contracts remain unchanged.

---

# 7. Consequences

## Positive Consequences

- Strong architectural decoupling.
- Technology independence.
- Improved scalability.
- Easier module evolution.
- Better fault isolation.
- Improved maintainability.
- Future broker replacement becomes possible.
- Supports distributed execution.

---

## Trade-offs

The messaging architecture introduces:

- additional infrastructure;
- message lifecycle management;
- monitoring requirements;
- eventual delivery characteristics.

These trade-offs are accepted because they significantly improve long-term architectural flexibility.

---

# 8. Relationship with Other ADRs

## Depends On

- ADR-0001 — Clean Architecture
- ADR-0012 — Distributed Workspace Architecture
- ADR-0013 — Client Application Architecture
- ADR-0014 — Workspace Data Architecture
- ADR-0015 — Workspace Synchronization Architecture

## Enables

- Artificial Intelligence Integration
- External Integration
- Notification Architecture
- Internal Messenger
- Distributed Event Processing

---

# 9. Risk Assessment

| Risk | Impact | Mitigation |
|------|--------|------------|
| Message duplication | Medium | Idempotent consumers |
| Delivery delay | Medium | Eventual consistency |
| Infrastructure failure | High | Messaging isolation |
| Broker replacement | Low | Technology-independent contracts |
| Tight coupling | High | Architectural boundaries |

---

# 10. Compliance

This ADR complies with:

- Clean Architecture
- Distributed Workspace Architecture
- Workspace Data Architecture
- Workspace Synchronization Architecture

Every messaging implementation shall comply with this ADR before technology selection.

---

# 11. Future Work

Future work includes:

- Technical evaluation of messaging technologies.
- Definition of message contracts.
- Message versioning strategy.
- Broker selection.
- Delivery guarantees.
- Monitoring strategy.

---

# 11a. Technology Selection (Formalized)

TE-0012 — Enterprise Messaging Technology Evaluation evaluated RabbitMQ,
Apache Kafka, Azure Service Bus, MassTransit, and NServiceBus against the
architectural principles established above.

The platform adopts **MassTransit** as the messaging abstraction framework
and **RabbitMQ** as the underlying message broker.

| Responsibility | Selected Technology |
|-----------------|---------------------|
| Messaging Abstraction / Bus Framework | MassTransit |
| Message Broker | RabbitMQ |

All Domain Events, Integration Events, and Notifications defined in this
ADR shall be published and consumed through MassTransit, which shall be
configured to use RabbitMQ as its transport in all deployment
environments.

This closes the technology selection that was previously deferred to
TE-0012 and corrects the prior "Planned" status recorded below.

---

# 12. Related Documents

## Architecture

- ADR-0001
- ADR-0012
- ADR-0013
- ADR-0014
- ADR-0015

## Technical Evaluation

- TE-0012 — Enterprise Messaging Technology Evaluation *(Approved — MassTransit / RabbitMQ, see Section 11a)*

## Development

- Solution Structure
- Dependency Rules

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-26 | Solution Architect | Initial Architecture Decision Record                  |
| 3.0.0   | 2026-07-26 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Formalized technology selection (MassTransit / RabbitMQ), closing TE-0012 |