# ADR-0003 — Use MediatR as CQRS Mediator

**Status:** Accepted

**Date:** 2026-07-18

**Decision Makers**

- Solution Architect
- Development Team

---

# Context

MachineryManagerEnterprise follows the following architectural principles:

- Clean Architecture
- Domain-Driven Design
- CQRS
- Vertical Slice Architecture
- SOLID Principles

The application contains numerous business use cases including:

- Machine Management
- Work Orders
- Inventory
- Preventive Maintenance
- Attachments
- Notifications
- Forecasting
- Reporting

Application logic must remain loosely coupled and easily testable.

---

# Problem

Application requests require a mechanism to:

- Dispatch Commands
- Dispatch Queries
- Execute Pipeline Behaviors
- Support future Domain Events
- Keep Controllers thin
- Prevent direct dependencies between Presentation and Application Services

---

# Considered Options

## Option 1

Direct Service Invocation

### Advantages

- No external dependency
- Simple

### Disadvantages

- Strong coupling
- Difficult to extend
- No pipeline support
- Poor scalability

---

## Option 2

Custom Mediator Implementation

### Advantages

- Full control

### Disadvantages

- Reinventing an existing mature solution
- Higher maintenance cost
- Additional testing effort

---

## Option 3

MediatR

### Advantages

- Mature ecosystem
- Widely adopted
- Excellent CQRS support
- Pipeline Behaviors
- Notifications
- Strong testability
- Clear separation of concerns

### Disadvantages

- External dependency
- Additional abstraction layer

---

## Option 4

Wolverine

### Advantages

- Advanced messaging
- Event-driven architecture
- Distributed processing

### Disadvantages

- More infrastructure than currently required
- Higher learning curve
- Overkill for the first implementation phase

---

# Decision

The project adopts **MediatR** as the application's request mediator.

MediatR shall be responsible only for request dispatching.

Business logic shall remain inside:

- Application Handlers
- Domain Model

MediatR shall **not** become a business layer.

---

# Architectural Rules

The following rules are mandatory.

## Controllers

Controllers shall communicate only with:

```
IMediator
```

---

## Command Handlers

Each Command shall have exactly one Handler.

---

## Query Handlers

Each Query shall have exactly one Handler.

---

## Pipeline Behaviors

The following concerns shall execute through Pipeline Behaviors whenever applicable:

- Validation
- Logging
- Performance Monitoring
- Transactions
- Authorization (if required)

---

## Domain

Domain Entities shall never reference MediatR.

---

## Infrastructure

Infrastructure shall never depend directly on MediatR abstractions.

---

# Consequences

## Positive

- High maintainability
- Loose coupling
- Excellent testability
- Extensible request pipeline
- Consistent CQRS implementation

---

## Negative

- Additional dependency
- Slight increase in complexity

---

# Constraints

Handlers shall not directly expose Entity Framework DbContext.

Persistence access shall occur through:

- Repository
- Unit of Work

or other approved abstractions.

---

# Future Considerations

If future versions require distributed messaging, MediatR may coexist with:

- Wolverine
- MassTransit
- Azure Service Bus

without changing the Application Layer contracts.

---

# Related Decisions

- ADR-0002 — Use FluentValidation
- ADR-0004 — Use Entity Framework Core (Planned)

---

# References

- MediatR Documentation
- CQRS Pattern
- Clean Architecture
- Domain-Driven Design