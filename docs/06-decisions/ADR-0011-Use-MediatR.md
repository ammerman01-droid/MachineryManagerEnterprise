# Architecture Decision Record

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0011 |
| **Version** | 3.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Title

Use MediatR

---

# Status

Accepted

---

# Context

The MachineryManagerEnterprise solution follows Clean Architecture and requires a
consistent application interaction model.

The Application Layer must support:

- CQRS
- Request / Response messaging
- Pipeline Behaviors
- Cross-cutting concerns
- Low coupling
- High maintainability
- Testability

Business operations should remain independent from controllers, UI components,
and infrastructure implementations.

---

# Decision

The Application Layer shall use **MediatR** as the request dispatching mechanism.

Commands, Queries, and Notifications shall be processed through MediatR.

Pipeline Behaviors shall be used for cross-cutting concerns such as validation,
logging, performance monitoring, and transactions.

---

# Decision Drivers

- CQRS support
- Loose coupling
- Extensibility
- Testability
- Pipeline Behaviors
- Separation of Concerns
- Clean Architecture compatibility

---

# Alternatives Considered

## Direct Service Calls

Rejected because it tightly couples Presentation to Application services and
reduces extensibility.

---

## Custom Mediator Implementation

Rejected because MediatR is mature, widely adopted, and already solves the
required problem.

---

## Event Bus Only

Rejected because synchronous request/response scenarios remain necessary inside
the application.

---

# Consequences

## Positive

- Consistent request handling
- Excellent extensibility
- Easy testing
- Cleaner controllers
- Centralized cross-cutting concerns
- Better separation between UI and business logic

## Negative

- Additional abstraction layer
- Developers must understand request pipeline behavior

---

# Architecture Impact

MediatR shall exist only inside the **Application Layer**.

Presentation communicates with Application only by sending Commands or Queries.

Domain shall never reference MediatR.

Infrastructure shall never invoke handlers directly.

---

# Implementation Notes

Each use case shall be represented by:

- Command or Query
- Handler
- Validator (when required)

Pipeline Behaviors should implement:

- Validation
- Logging
- Performance Monitoring
- Transaction Management
- Exception Handling

where appropriate.

---

# Compliance Rules

1. MediatR shall only exist inside Application.

2. Domain shall never reference MediatR.

3. Presentation shall never invoke handlers directly.

4. Commands shall modify state.

5. Queries shall never modify state.

6. Cross-cutting concerns shall be implemented using Pipeline Behaviors.

7. Controllers and Razor Components shall communicate only through IMediator.

---

# Related Technology Evaluation

TE-0009 — MediatR *(to be created)*

---

# Related Proof of Concept

Not Required

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0007 — Use FluentValidation
- ADR-0008 — Use Mapster
- Dependency Catalog

---

# References

https://github.com/jbogard/MediatR

https://www.nuget.org/packages/MediatR

https://github.com/jbogard/MediatR/wiki

---

# Review

| Role | Name | Date |
|------|------|------|
| Solution Architect | | |

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial decision |
| 2.0.0 | 2026-07-18 | Standardized |
| 3.0.0 | 2026-07-18 | Rewritten according to ADR Template v3.0 |