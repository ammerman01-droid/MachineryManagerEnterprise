# ADR-0007 — Use FluentValidation for Application Validation

**Status:** Accepted

**Date:** 2026-07-18

**Decision Makers**

- Solution Architect
- Development Team

---

# Context

MachineryManagerEnterprise follows:

- Clean Architecture
- Domain-Driven Design
- CQRS
- Vertical Slice Architecture

The application requires a validation framework capable of separating validation logic from business logic while remaining testable and maintainable.

Validation must occur before command execution.

---

# Problem

The project requires validation for:

- Commands
- Queries (where applicable)
- DTOs
- User input
- Business Rules at Application Layer

Validation should not pollute:

- Domain Entities
- Controllers
- UI Components

---

# Considered Options

## Option 1

DataAnnotations

### Advantages

- Built into .NET
- Simple

### Disadvantages

- Poor support for complex business rules
- Validation logic mixed with models
- Limited testability

---

## Option 2

Custom Validation Framework

### Advantages

- Full control

### Disadvantages

- Reinventing existing solutions
- Increased maintenance cost

---

## Option 3

FluentValidation

### Advantages

- Mature ecosystem
- Excellent readability
- Excellent testability
- Strong separation of concerns
- Widely adopted in Clean Architecture
- Pipeline integration support

### Disadvantages

- External dependency

---

# Decision

The project adopts **FluentValidation** as the standard validation framework.

Validation rules shall reside in the Application Layer.

Each Command shall have its own Validator.

Validation shall execute before command handlers.

---

# Consequences

## Positive

- High readability
- Strong testability
- Separation of concerns
- Clean command handlers
- Consistent validation approach

---

## Negative

- Additional dependency
- Validators must be maintained

---

# Constraints

The following package shall **NOT** be used:

```
FluentValidation.AspNetCore
```

This package is considered legacy and is no longer recommended for new projects.

Validation registration shall use the core FluentValidation package and dependency injection.

---

# Architecture Impact

```text
Presentation

↓

Application

↓

FluentValidation

↓

Command Handler

↓

Domain
```

Domain entities remain free from UI and infrastructure validation concerns.

---

# Related Decisions

- ADR-002 — Use MediatR as CQRS Mediator (Planned)

---

# References

- FluentValidation Official Documentation
- Clean Architecture Principles
- Domain-Driven Design