| Property | Value |
|----------|-------|
| **Document ID** | TE-0022 |
| **Title** | Validation Pipeline and Validation Architecture Evaluation (.NET 10) |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-26 |
| **Last Updated** | 2026-07-28 |

---

# Purpose

This document evaluates candidate technologies for Validation Pipeline and Validation Architecture Evaluation (.NET 10) in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Relationship with TE-0005

TE-0005 answers:

> **Which validation technology should we use?**

Answer:

> **FluentValidation**

This document answers:

> **How should validation be architected across the entire solution?**

Therefore:

- TE-0005 remains valid.
- TE-0022 extends the architectural usage of the selected technology.

```text
TE-0005

Technology Selection

        │

        ▼

FluentValidation Selected

        │

        ▼

TE-0022

Validation Architecture
```

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0003 — CQRS
- ADR-0004 — MediatR
- ADR-0007 — FluentValidation
- SolutionStructure.md
- DependencyRules.md

---

# Scope

This document evaluates:

- Validation Pipeline
- Validation Lifecycle
- Validator Organization
- Registration Strategy
- CQRS Integration
- MediatR Integration
- Error Flow
- Performance Considerations

Business rules themselves are **outside the scope** of this document.

---

# Functional Requirements

The architecture shall support:

- automatic validation;
- command validation;
- query validation;
- DTO validation;
- asynchronous validation;
- localization;
- multiple validators;
- pipeline execution;
- dependency injection.

---

# Non-Functional Requirements

The validation architecture shall provide:

- Clean Architecture compliance;
- high performance;
- extensibility;
- maintainability;
- testability;
- deterministic execution;
- minimal boilerplate.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| Selected Primary Engine | Enterprise Infrastructure | Selected |
| Alternative Engine | Comparison Candidate | Evaluated |

---

# Candidate Architectural Approaches

| Approach | Description |
|----------|-------------|
| Controller Validation | Validation inside Controllers |
| Endpoint Validation | Validation inside Endpoints |
| MediatR Pipeline Validation | Validation before Handler execution |
| Business Layer Validation | Validation inside Handlers |
| Domain Validation | Validation inside Domain Model |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| V1 | Clean Architecture | Critical |
| V2 | Separation of Concerns | Critical |
| V3 | Testability | Critical |
| V4 | Performance | High |
| V5 | Maintainability | High |
| V6 | Extensibility | High |
| V7 | Developer Experience | High |

---

# Architecture Principle

Validation is an **Application concern**.

Business logic must never execute before validation succeeds.

---


# 6. Controller Validation Evaluation

## Overview

Controller Validation is the traditional ASP.NET validation approach in which every Controller (or Endpoint) explicitly invokes validation before executing application logic.

Typical implementation:

```csharp
var result = await validator.ValidateAsync(request);

if (!result.IsValid)
{
    return BadRequest(result.Errors);
}

await mediator.Send(request);
```

The controller becomes responsible for orchestrating validation.

---

# Architectural Flow

```text
HTTP Request

      │

      ▼

Controller

      │

Validation

      │

Business Logic

      │

HTTP Response
```

Validation executes inside every controller action.

---

# Architectural Strengths

## Advantages

- Simple to understand.
- Explicit validation flow.
- Easy for small applications.
- No additional pipeline infrastructure.

---

# Architectural Weaknesses

Controller Validation violates several architectural principles adopted by MachineryManagerEnterprise.

### Validation Duplication

Every controller repeats:

- validator resolution;
- validation execution;
- error handling.

Large systems quickly accumulate duplicated code.

---

### Separation of Concerns

Controllers become responsible for:

- HTTP concerns;
- validation;
- orchestration.

This mixes responsibilities.

---

### CQRS Inconsistency

CQRS requires:

```text
Command

↓

Validation

↓

Handler
```

Controller Validation changes this into:

```text
Controller

↓

Validation

↓

Mediator

↓

Handler
```

Validation becomes dependent on the presentation layer.

---

### Pipeline Bypass

Background jobs, integration events and internal application requests may bypass controllers completely.

Consequently:

- validation may not execute;
- business handlers may receive invalid objects.

---

# Operational Characteristics

Controller Validation requires every endpoint to perform validation manually.

Operational complexity increases proportionally with the number of endpoints.

---

# Maintainability

Maintainability decreases because:

- duplicated validation code;
- duplicated error mapping;
- duplicated logging;
- duplicated exception handling.

Maintenance cost is considered high.

---

# Performance

Runtime performance is acceptable.

However, development performance decreases because developers repeatedly write identical validation logic.

---

# Testability

Validation cannot be tested independently from controller orchestration.

Controller unit tests become unnecessarily coupled to validation behavior.

Testability is considered moderate.

---

# Scalability

As the number of APIs grows:

```text
10 Controllers

↓

10 Validation Blocks

↓

100 Controllers

↓

100 Validation Blocks
```

The architecture scales poorly.

---

# Relationship with Clean Architecture

Controller Validation introduces an undesirable dependency.

```text
Presentation

↓

Validation

↓

Application
```

Validation should belong to the Application layer rather than Presentation.

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Poor |
| Separation of Concerns | Poor |
| Maintainability | Poor |
| Scalability | Moderate |
| Testability | Moderate |
| Developer Experience | Moderate |

---

# Comparison with Pipeline Validation

| Criterion | Controller Validation | Pipeline Validation |
|-----------|----------------------|---------------------|
| Duplication | High | None |
| Automatic Execution | No | Yes |
| CQRS Alignment | Poor | Excellent |
| Maintainability | Moderate | Excellent |

---

# Preliminary Conclusion

Controller Validation is suitable only for:

- very small applications;
- prototypes;
- educational examples.

It is **not suitable** for MachineryManagerEnterprise.

Because the project adopts:

- Clean Architecture;
- CQRS;
- MediatR;
- modular architecture;

validation must execute independently of the presentation layer.

Controller Validation is therefore **rejected**.

---


# 7. Endpoint Validation Evaluation

## Overview

Endpoint Validation places validation logic directly inside Minimal API endpoints (or endpoint handlers) rather than inside MVC Controllers.

Typical implementation:

```csharp
app.MapPost("/machines", async (
    CreateMachineCommand command,
    IValidator<CreateMachineCommand> validator,
    ISender sender) =>
{
    var result = await validator.ValidateAsync(command);

    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());

    return await sender.Send(command);
});
```

This approach is increasingly common in Minimal API applications.

---

# Architectural Flow

```text
HTTP Request

      │

      ▼

Minimal Endpoint

      │

Validation

      │

Mediator

      │

Handler

      │

HTTP Response
```

Validation executes before dispatching the request to the Application layer.

---

# Architectural Strengths

## Advantages

- Simpler than Controller-based validation.
- Well suited to small Minimal API projects.
- Validation remains explicit.
- Easy to understand.
- No additional infrastructure is required.

---

# Architectural Weaknesses

Although Controllers are removed, the architectural problems remain.

### Validation Duplication

Every endpoint repeats:

- validator resolution;
- validator invocation;
- validation result handling;
- error mapping.

As the number of endpoints grows, duplication increases significantly.

---

### Separation of Concerns

Endpoints become responsible for:

- HTTP routing;
- validation;
- orchestration.

This violates the project's preferred separation of responsibilities.

---

### CQRS Misalignment

The intended execution model is:

```text
Request

↓

Validation

↓

Handler
```

Endpoint Validation changes this to:

```text
Endpoint

↓

Validation

↓

Mediator

↓

Handler
```

Validation remains dependent on the transport layer.

---

### Pipeline Inconsistency

Only HTTP requests are validated automatically.

Other execution paths, such as:

- background jobs;
- scheduled tasks;
- integration event handlers;
- internal application requests;

can bypass endpoint validation entirely.

This leads to inconsistent behavior.

---

# Operational Characteristics

Validation logic must be implemented manually in every endpoint.

Operational complexity increases proportionally with the number of endpoints.

---

# Maintainability

Maintainability is negatively affected because:

- validation code is duplicated;
- validation error mapping is duplicated;
- pipeline behavior cannot be centralized.

Maintainability is considered poor.

---

# Performance

Runtime performance is acceptable.

However, development productivity decreases due to repetitive implementation.

---

# Testability

Validation cannot be tested independently from endpoint orchestration.

Unit testing becomes more complex because transport concerns are coupled with validation behavior.

Testability is considered moderate.

---

# Scalability

Endpoint Validation scales poorly.

Example:

```text
20 Endpoints

↓

20 Validation Blocks

↓

300 Endpoints

↓

300 Validation Blocks
```

The maintenance burden grows linearly with the API surface.

---

# Relationship with Clean Architecture

Validation remains tied to the Presentation layer.

```text
Presentation

↓

Validation

↓

Application
```

The preferred architecture places validation inside the Application pipeline so that every execution path is validated consistently.

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Poor |
| Separation of Concerns | Poor |
| Maintainability | Poor |
| Scalability | Moderate |
| Testability | Moderate |
| Developer Experience | Good |

---

# Comparison with Controller Validation

| Criterion | Controller Validation | Endpoint Validation |
|-----------|----------------------|---------------------|
| Boilerplate | High | Moderate |
| Duplication | High | High |
| CQRS Alignment | Poor | Poor |
| Pipeline Consistency | Poor | Poor |

Endpoint Validation improves syntax but does not solve the underlying architectural problems.

---

# Preliminary Conclusion

Endpoint Validation is appropriate for:

- small Minimal API projects;
- prototypes;
- lightweight services.

It is **not suitable** for MachineryManagerEnterprise.

Although it offers cleaner syntax than Controller Validation, it still violates the project's architectural goals:

- validation is not centralized;
- validation is duplicated;
- non-HTTP execution paths are not automatically protected.

For these reasons, Endpoint Validation is **rejected** as the primary validation architecture.

---


# 8. MediatR Pipeline Validation Evaluation

## Overview

MediatR Pipeline Validation executes validation inside the MediatR request pipeline before any Command or Query reaches its Handler.

Instead of each Controller or Endpoint invoking validators manually, a single Pipeline Behavior performs validation automatically for every request.

Typical implementation:

```text
HTTP Request

        │

        ▼

Controller / Endpoint

        │

        ▼

Mediator.Send()

        │

        ▼

ValidationBehavior<TRequest,TResponse>

        │

        ▼

FluentValidation

        │

        ▼

Handler
```

This approach is the most common validation architecture in enterprise CQRS systems.

---

# Architectural Role

Pipeline Validation belongs entirely to the **Application Layer**.

```text
Presentation

      │

      ▼

Mediator

      │

      ▼

Validation Pipeline

      │

      ▼

Command / Query Handler

      │

      ▼

Domain
```

Neither Controllers nor Endpoints contain validation logic.

---

# Architectural Strengths

## Advantages

- Validation executes automatically.
- Zero duplicated validation code.
- Consistent behavior.
- Independent from HTTP.
- Independent from Controllers.
- Independent from Endpoints.
- Compatible with CQRS.
- Compatible with MediatR.
- Compatible with Background Jobs.
- Compatible with Integration Events.
- Excellent testability.
- Excellent maintainability.

---

# Separation of Concerns

Responsibilities become clearly separated.

```text
Presentation

↓

Routing

↓

Mediator

↓

Validation

↓

Business Logic

↓

Persistence
```

Each layer owns exactly one responsibility.

---

# CQRS Alignment

Pipeline Validation matches the intended CQRS execution flow.

```text
Command

      │

      ▼

Validation

      │

      ▼

Handler

      │

      ▼

Domain
```

No business logic executes before validation succeeds.

---

# Pipeline Consistency

Every execution path receives identical validation.

Examples include:

```text
HTTP API

Desktop UI

Background Jobs

Message Bus

Scheduled Tasks

Integration Events
```

All of them execute:

```text
Mediator

↓

ValidationBehavior

↓

Handler
```

No execution path bypasses validation.

---

# Operational Characteristics

Validators execute automatically through Dependency Injection.

Developers never invoke validators manually.

Operational complexity is low despite the richer architecture.

---

# Performance

Pipeline Validation introduces one additional MediatR behavior.

The overhead is negligible.

Benefits include:

- elimination of duplicated validation;
- centralized execution;
- predictable performance.

Performance is considered excellent.

---

# Testability

Validators can be tested independently.

Pipeline behavior can be tested independently.

Handlers can be tested assuming valid requests.

This greatly simplifies unit testing.

Testability is considered excellent.

---

# Maintainability

Maintenance benefits include:

- one validation pipeline;
- no duplicated code;
- centralized exception handling;
- centralized validation strategy.

Maintainability is considered excellent.

---

# Scalability

Pipeline Validation scales naturally.

```text
5 Commands

↓

One ValidationBehavior

↓

500 Commands

↓

Still One ValidationBehavior
```

The architecture scales without additional orchestration code.

---

# Relationship with FluentValidation

```text
Mediator

      │

      ▼

ValidationBehavior

      │

      ▼

FluentValidation

      │

      ▼

Handler
```

FluentValidation remains responsible for validation rules.

The pipeline remains responsible for orchestration.

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| CQRS Compatibility | Excellent |
| Separation of Concerns | Excellent |
| Maintainability | Excellent |
| Scalability | Excellent |
| Testability | Excellent |
| Developer Experience | Excellent |

---

# Comparison with Previous Approaches

| Criterion | Controller | Endpoint | MediatR Pipeline |
|-----------|------------|----------|------------------|
| Automatic Validation | No | No | Yes |
| Code Duplication | High | High | None |
| HTTP Independent | No | No | Yes |
| CQRS Alignment | Poor | Poor | Excellent |
| Maintainability | Moderate | Moderate | Excellent |

---

# Preliminary Conclusion

MediatR Pipeline Validation satisfies every architectural objective of MachineryManagerEnterprise.

It provides:

- centralized validation;
- automatic execution;
- transport independence;
- excellent CQRS alignment;
- excellent maintainability.

This approach is therefore **approved as the primary validation architecture** for the solution.

---


# 9. Business Layer Validation Evaluation

## Overview

Business Layer Validation performs validation directly inside Command or Query Handlers.

Typical implementation:

```csharp
public async Task<Result> Handle(CreateMachineCommand request, CancellationToken ct)
{
    if (string.IsNullOrWhiteSpace(request.Name))
        throw new ValidationException(...);

    if (request.Price <= 0)
        throw new ValidationException(...);

    ...
}
```

Validation logic becomes part of the business execution flow.

---

# Architectural Flow

```text
Command

      │

      ▼

Handler

      │

Validation

      │

Business Logic

      │

Persistence
```

Unlike Pipeline Validation, validation occurs after the request reaches the handler.

---

# Architectural Strengths

## Advantages

- Simple implementation.
- No pipeline infrastructure required.
- Validation is close to business logic.
- Easy to understand in very small applications.

---

# Architectural Weaknesses

This approach violates several architectural principles adopted by MachineryManagerEnterprise.

---

## Mixed Responsibilities

Handlers become responsible for:

- validation;
- business logic;
- orchestration.

Example:

```text
Handler

├── Validation

├── Authorization

├── Business Rules

└── Persistence
```

The Single Responsibility Principle is violated.

---

## Validation Duplication

Every Handler repeats:

- guard clauses;
- validation logic;
- exception creation;
- error formatting.

As the number of handlers increases, duplicated validation code grows rapidly.

---

## CQRS Degradation

The intended CQRS flow is:

```text
Command

↓

Validation

↓

Handler
```

Business Layer Validation changes this into:

```text
Command

↓

Handler

├── Validation

└── Business Logic
```

Business handlers must now know how validation works.

---

## Maintainability

Validation rules become scattered across many handlers.

Changing a common validation policy requires modifications in multiple locations.

Maintenance cost is therefore high.

---

# Operational Characteristics

Validation executes only when the Handler starts.

The handler cannot assume that incoming requests are valid.

This increases implementation complexity.

---

# Performance

Runtime performance is acceptable.

However:

- unnecessary handler construction;
- unnecessary dependency resolution;
- repeated validation code;

slightly reduce efficiency compared with Pipeline Validation.

---

# Testability

Business logic tests become coupled with validation behavior.

Developers must either:

- satisfy every validation rule before testing business logic;

or

- mock validation behavior.

This complicates unit testing.

Testability is considered moderate.

---

# Scalability

As handlers increase:

```text
20 Handlers

↓

20 Validation Implementations

↓

400 Handlers

↓

400 Validation Implementations
```

The architecture scales poorly.

---

# Relationship with FluentValidation

Using FluentValidation inside handlers usually results in:

```csharp
await validator.ValidateAsync(request);
```

inside every handler.

Although FluentValidation is reused, orchestration remains duplicated.

---

# Relationship with Clean Architecture

Validation becomes embedded inside application use cases.

```text
Application

↓

Handler

├── Validation

└── Business Logic
```

The preferred architecture separates these concerns.

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Poor |
| Separation of Concerns | Poor |
| Maintainability | Moderate |
| Scalability | Moderate |
| Testability | Moderate |
| CQRS Alignment | Poor |

---

# Comparison with MediatR Pipeline

| Criterion | Business Layer | Pipeline |
|-----------|----------------|----------|
| Automatic Validation | No | Yes |
| Handler Simplicity | Poor | Excellent |
| Code Duplication | High | None |
| Testability | Moderate | Excellent |
| CQRS Alignment | Poor | Excellent |

---

# Preliminary Conclusion

Business Layer Validation is acceptable only for:

- very small applications;
- prototypes;
- simple CRUD systems.

For MachineryManagerEnterprise it introduces:

- duplicated validation logic;
- mixed responsibilities;
- reduced maintainability;
- weaker CQRS alignment.

Business Layer Validation is therefore **rejected**.

Business Handlers should always assume that incoming requests have already been validated successfully by the MediatR Validation Pipeline.

---


# 10. Domain Validation Evaluation

## Overview

Domain Validation enforces the intrinsic invariants of the Domain Model.

Unlike Application Validation, which validates incoming requests, Domain Validation protects the correctness of business entities and value objects regardless of where they are created.

Typical examples include:

- invalid entity state;
- invalid value object construction;
- broken aggregate invariants;
- illegal state transitions.

Domain Validation is therefore **not a replacement** for Application Validation.

It is the final line of defense for domain correctness.

---

# Architectural Flow

```text
Request

      │

      ▼

Application Validation

      │

      ▼

Command Handler

      │

      ▼

Domain Entity

      │

Domain Validation

      │

      ▼

Valid Aggregate
```

Application Validation prevents invalid requests.

Domain Validation guarantees valid business models.

---

# Architectural Strengths

## Advantages

- Protects domain invariants.
- Independent from transport.
- Independent from UI.
- Independent from persistence.
- Prevents invalid entities.
- Supports ubiquitous language.
- Encourages rich domain models.
- Enforces business correctness.

---

# Domain Invariants

Examples include:

```text
Machine Serial Number cannot be empty.

Maintenance Interval cannot be negative.

Purchase Date cannot be after Disposal Date.

Currency must always exist.

Money cannot have negative quantity (when business rules require it).

Aggregate Root must never enter an invalid state.
```

These rules belong to the Domain.

---

# Separation of Responsibilities

Application Validation answers:

```text
Is this request structurally valid?
```

Domain Validation answers:

```text
Can this business object exist?
```

These are fundamentally different responsibilities.

---

# Operational Characteristics

Domain Validation executes:

- during entity construction;
- during aggregate mutation;
- during business operations.

It is impossible to bypass because it is embedded inside the Domain Model.

---

# Maintainability

Business rules remain centralized.

Example:

```text
Machine

├── Constructor

├── ChangeStatus()

├── RegisterMaintenance()

└── Domain Invariants
```

No duplication occurs.

Maintainability is considered excellent.

---

# Performance

Domain Validation executes only when domain objects change.

Its performance cost is negligible.

Performance is considered excellent.

---

# Testability

Domain rules can be tested independently.

Example:

```text
MachineTests

↓

CreateInvalidMachine

↓

Expect DomainException
```

No HTTP or MediatR infrastructure is required.

Testability is considered excellent.

---

# Scalability

Every Aggregate protects itself.

```text
Machine

Vehicle

Warehouse

MaintenanceOrder

Inventory

Supplier
```

Each aggregate owns its own invariants.

The architecture scales naturally.

---

# Relationship with Clean Architecture

Domain Validation resides entirely inside the Domain layer.

```text
Domain

↓

Entity

↓

Invariant
```

No dependency upon:

- ASP.NET Core;
- MediatR;
- FluentValidation;
- HTTP;
- Infrastructure.

This represents the purest implementation of Clean Architecture.

---

# Relationship with FluentValidation

These technologies are complementary.

```text
Application

↓

FluentValidation

↓

Command

↓

Handler

↓

Domain Entity

↓

Domain Validation
```

FluentValidation never replaces domain invariants.

Likewise, domain invariants should never validate DTO formatting.

---

# Comparison with Application Validation

| Criterion | Application Validation | Domain Validation |
|-----------|------------------------|-------------------|
| DTO Validation | Excellent | No |
| Business Invariants | No | Excellent |
| Transport Independent | Yes | Yes |
| Prevent Invalid Requests | Excellent | Moderate |
| Protect Domain Model | Moderate | Excellent |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Separation of Concerns | Excellent |
| Business Correctness | Excellent |
| Maintainability | Excellent |
| Scalability | Excellent |
| Testability | Excellent |

---

# Recommended Domain Validation Strategy

The project should adopt the following layered validation model.

```text
Incoming Request

        │

        ▼

FluentValidation

(Application Layer)

        │

        ▼

Command Handler

        │

        ▼

Domain Entity

        │

Domain Invariants

        │

        ▼

Valid Aggregate
```

This guarantees:

- invalid requests never reach business logic;
- invalid entities can never exist.

---

# Preliminary Conclusion

Domain Validation is mandatory.

It complements—not replaces—Application Validation.

For MachineryManagerEnterprise the recommended validation architecture is:

- **Application Validation → FluentValidation**
- **Domain Validation → Aggregate Invariants**

Both layers are required to achieve a robust enterprise architecture.

---


# 11. Overall Architecture Comparison

Validation in MachineryManagerEnterprise is divided into multiple architectural layers.

Each layer has a distinct responsibility.

Validation is **not** a single activity.

Instead, it is a layered architectural concern.

---

# Validation Layers

```text
Incoming Request

        │

        ▼

Application Validation

(FluentValidation)

        │

        ▼

Mediator Pipeline

        │

        ▼

Command Handler

        │

        ▼

Domain Validation

        │

        ▼

Persistence
```

Each layer protects the next one.

---

# Architectural Responsibility Matrix

| Layer | Responsibility | Technology |
|--------|----------------|------------|
| Presentation | Receive Request | ASP.NET Core |
| Application | Validate Request | FluentValidation |
| Application | Validation Orchestration | MediatR Pipeline |
| Domain | Protect Business Invariants | Domain Model |
| Infrastructure | Persistence | EF Core |

---

# Candidate Comparison

| Criterion | Controller | Endpoint | Business Layer | MediatR Pipeline | Domain Validation |
|-----------|------------|----------|----------------|------------------|-------------------|
| Automatic Validation | No | No | No | Yes | N/A |
| Code Duplication | High | High | High | None | None |
| Clean Architecture | Poor | Poor | Poor | Excellent | Excellent |
| CQRS Compatibility | Poor | Poor | Moderate | Excellent | Excellent |
| Separation of Concerns | Poor | Moderate | Poor | Excellent | Excellent |
| Testability | Moderate | Moderate | Moderate | Excellent | Excellent |
| Scalability | Moderate | Moderate | Moderate | Excellent | Excellent |
| Enterprise Readiness | Low | Low | Moderate | Excellent | Excellent |

---

# Responsibility Separation

The recommended architecture clearly separates two different validation categories.

## Application Validation

Purpose:

```text
Can this request be processed?
```

Typical rules:

- Required fields
- Length
- Range
- Format
- DTO consistency
- Cross-property checks

Technology:

```text
FluentValidation
```

---

## Domain Validation

Purpose:

```text
Can this business object exist?
```

Typical rules:

- Aggregate invariants
- Entity consistency
- Value Object validity
- Business correctness
- Illegal state transitions

Technology:

```text
Domain Model
```

---

# Validation Execution Order

The Architecture Review Board recommends the following execution order.

```text
Request

      │

      ▼

FluentValidation

      │

      ▼

ValidationBehavior

      │

      ▼

Handler

      │

      ▼

Domain Invariants

      │

      ▼

Repository
```

Every layer validates only the concerns it owns.

---

# Error Propagation

Validation failures should terminate processing immediately.

```text
Request

↓

Validation Failed

↓

Validation Exception

↓

Problem Details

↓

HTTP Response
```

Business logic should never execute after a validation failure.

---

# Performance Comparison

| Architecture | Runtime Cost | Development Cost |
|--------------|-------------|------------------|
| Controller Validation | Low | High |
| Endpoint Validation | Low | High |
| Business Validation | Moderate | High |
| Pipeline Validation | Very Low | Very Low |
| Domain Validation | Negligible | Low |

---

# Maintainability Comparison

| Architecture | Maintainability |
|--------------|----------------|
| Controller Validation | Poor |
| Endpoint Validation | Poor |
| Business Layer Validation | Moderate |
| MediatR Pipeline | Excellent |
| Domain Validation | Excellent |

---

# Clean Architecture Assessment

The selected architecture follows dependency flow correctly.

```text
Presentation

      │

      ▼

Application

      │

      ▼

Domain

      │

      ▼

Infrastructure
```

Validation responsibilities align with architectural boundaries.

No layer validates concerns belonging to another layer.

---

# AI Readiness

Centralized validation improves future AI integration.

Examples:

- AI-generated Commands
- AI Assistants
- Background AI Jobs
- Agent-to-Agent Communication

Every request—regardless of origin—passes through the same validation pipeline.

---

# Enterprise Readiness

The combined architecture supports:

- REST APIs
- Desktop Clients
- Background Workers
- Message Bus
- Future AI Services
- Future Mobile Clients

without changing validation behavior.

---

# Architecture Summary

The preferred validation architecture is therefore:

```text
Presentation

↓

FluentValidation

↓

ValidationBehavior

↓

Handler

↓

Domain Invariants

↓

Persistence
```

This architecture provides:

- zero duplicated validation code;
- deterministic execution;
- excellent maintainability;
- transport independence;
- enterprise scalability;
- complete Clean Architecture compliance.

---


# 12. Final Recommendation

After evaluating all candidate validation architectures, the Architecture Review Board recommends adopting a **layered validation architecture**.

Each layer has a clearly defined responsibility.

---

# Recommended Validation Architecture

| Layer | Responsibility | Selected Technology |
|--------|----------------|---------------------|
| Request Validation | Validate DTOs and Commands | FluentValidation |
| Validation Orchestration | Execute validation automatically | MediatR Pipeline Behavior |
| Business Validation | Protect aggregate invariants | Domain Model |
| Persistence Validation | Database Constraints | EF Core / Database |

---

# Recommended Validation Pipeline

```text
HTTP Request

        │

        ▼

Model Binding

        │

        ▼

FluentValidation

        │

        ▼

ValidationBehavior

        │

        ▼

Command / Query Handler

        │

        ▼

Domain Aggregate

        │

        ▼

Domain Invariants

        │

        ▼

Repository

        │

        ▼

Database Constraints
```

Every request entering the application shall follow this execution order.

---

# Responsibilities

## FluentValidation

Responsible for validating:

- required fields;
- DTO consistency;
- formats;
- ranges;
- cross-property validation;
- application input.

It **must not** validate domain invariants.

---

## MediatR Pipeline

Responsible for:

- automatic validator discovery;
- executing all validators;
- stopping execution upon failure;
- ensuring handlers receive only valid requests.

No Handler shall invoke validators manually.

---

## Command / Query Handler

Handlers shall assume:

```text
Incoming request is already valid.
```

Handlers focus exclusively on:

- business orchestration;
- application use cases;
- interaction with the Domain.

---

## Domain Model

The Domain Model remains responsible for:

- aggregate consistency;
- entity validity;
- value object correctness;
- business invariants.

These validations execute regardless of request origin.

---

# Architectural Rules

The following rules become mandatory for MachineryManagerEnterprise.

## Rule 1

Controllers and Minimal API Endpoints shall never execute validation manually.

---

## Rule 2

All Commands and Queries shall be validated through the MediatR Validation Pipeline.

---

## Rule 3

Handlers shall never contain duplicated validation logic.

---

## Rule 4

Domain entities shall always protect their own invariants.

---

## Rule 5

Business rules shall never be implemented inside FluentValidation validators.

Validators validate application input.

The Domain validates business correctness.

---

# Benefits

The selected architecture provides:

- automatic validation;
- zero duplicated validation code;
- deterministic execution;
- transport independence;
- consistent behavior;
- excellent maintainability;
- excellent scalability;
- complete CQRS alignment.

---

# Enterprise Readiness

The architecture supports every execution path:

```text
REST API

Desktop UI

Background Workers

Scheduled Jobs

Message Bus

Future AI Services
```

All of them receive identical validation behavior.

---

# AI Readiness

Because validation is centralized, future AI-generated Commands or autonomous Agents automatically inherit the same validation guarantees as human users.

No AI-specific validation path is required.

---


# Overall Technology Comparison

The selected technology provides optimal performance, strong maintainability, and native alignment with .NET 10 Clean Architecture.

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative |
|-----------------|------------------------|-------------|
| Architectural Capability | Primary Technology | Legacy Alternative |

## Capability Comparison

| Capability | Primary Technology | Alternative |
|------------|--------------------|-------------|
| Performance | Excellent | Good |
| Maintainability | Excellent | Fair |

---


# Final Recommendation

Adopt the selected primary technology as the official standard for MachineryManagerEnterprise.

---

# Final Decision

| Candidate | Decision |
|-----------|----------|
| Controller Validation | Rejected |
| Endpoint Validation | Rejected |
| Business Layer Validation | Rejected |
| MediatR Pipeline Validation | Approved |
| Domain Validation | Approved |

---

# Decision Summary

The approved validation architecture consists of:

- **FluentValidation** for application input validation (selected in TE-0005);
- **MediatR Validation Pipeline** for automatic execution;
- **Domain Invariants** for business correctness.

This architecture satisfies:

- ✔ Clean Architecture
- ✔ CQRS
- ✔ MediatR
- ✔ .NET 10
- ✔ High Scalability
- ✔ Maintainability
- ✔ Testability
- ✔ Enterprise Readiness
- ✔ AI Readiness

Accordingly, this layered validation model is adopted as the enterprise validation standard for MachineryManagerEnterprise.

---

# Related ADR

- ADR-0001 — Clean Architecture
- ADR-0015 — Deployment Architecture

---

# Related Documents

- SolutionStructure.md
- DependencyRules.md
- CodingStandards.md

---

# Relationship to TE-0005

This document complements **TE-0005**.

| Document | Purpose |
|----------|---------|
| **TE-0005** | Selects the validation technology (FluentValidation) |
| **TE-0022** | Defines how validation is architected and executed across the solution |

Both documents remain valid and should be read together.

---

# Revision History

| Version | Date       | Author             | Description                                                                       |
|---------|------------|--------------------|-----------------------------------------------------------------------------------|
| 1.0.0   | 2026-07-26 | Solution Architect | Initial technology evaluation for Validation Pipeline and Validation Architecture |
| 1.3.0   | 2026-07-28 | Solution Architect | New section added (Evaluation Scope)                                              |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0                                         |