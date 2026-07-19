# Dependency Rules

| Property | Value |
|----------|-------|
| **Document ID** | DOC-DEV-005 |
| **Version** | 2.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Purpose

This document defines the dependency rules for the
**MachineryManagerEnterprise** solution.

Correct dependency management is one of the most important architectural
constraints of the project.

Every project shall comply with these rules.

---

# Architectural Principle

The solution follows **Dependency Inversion** and **Clean Architecture**.

Dependencies always point toward the business core.

High-level modules never depend on implementation details.

---

# Dependency Direction

The allowed dependency direction is:

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
SharedKernel
```

Infrastructure provides implementations but depends on abstractions defined
by the Application or Domain layers.

---

# Allowed Dependencies

| Project | May Reference |
|----------|---------------|
| SharedKernel | — |
| Domain | SharedKernel |
| Application | Domain, SharedKernel |
| Infrastructure | Application, Domain, SharedKernel |
| Web (Presentation) | Application |

---

# Forbidden Dependencies

The following dependencies are strictly prohibited.

## Domain

Domain shall never reference:

- Infrastructure
- Presentation
- UI Frameworks
- Entity Framework
- Logging Frameworks
- External Services

---

## SharedKernel

SharedKernel shall never reference any other project.

It is the lowest layer of the architecture.

---

## Application

Application shall never depend on:

- Infrastructure implementations
- UI Components
- Database Providers

Application depends only on abstractions.

---

## Presentation

Presentation shall never contain:

- Business Rules
- Persistence Logic
- Repository Implementations

---

# Infrastructure

Infrastructure may implement interfaces defined in:

- Application
- Domain

Infrastructure shall not define business rules.

---

# Dependency Graph

```text
                Web
                 │
                 ▼
          Application
                 │
                 ▼
             Domain
                 │
                 ▼
          SharedKernel

Infrastructure
      │
      └──────────────►
 Application / Domain
```

Infrastructure supports the core but is never the architectural center.

---

# Circular Dependencies

Circular project references are forbidden.

Example (Invalid)

```text
Application

↓

Infrastructure

↓

Application
```

Every dependency graph must remain acyclic.

---

# Dependency Injection

Concrete implementations shall be registered through Dependency Injection.

Application defines interfaces.

Infrastructure provides implementations.

Presentation consumes abstractions.

---

# Third-Party Libraries

Third-party libraries should be isolated whenever practical.

Example

```text
Infrastructure

Entity Framework

Serilog

FluentValidation

Redis
```

Business layers should remain unaware of implementation libraries.

---

# Compile-Time Dependencies

Compile-time dependencies should be minimized.

Projects should expose only the abstractions required by consumers.

---

# Runtime Dependencies

Runtime wiring shall occur through:

- Dependency Injection
- Configuration
- Composition Root

Never through direct object construction inside business logic.

---

# Future Expansion

As new bounded contexts are introduced, the same dependency rules shall apply.

Every module should remain independently maintainable.

---

# Architectural Exceptions

Any exception to these rules requires an approved Architecture Decision Record (ADR).

Undocumented exceptions are not permitted.

---

# Related Documents

- DOC-CONVENTIONS
- DOC-README
- DOC-DEV-001 (Development Principles)
- DOC-DEV-002 (Solution Structure)
- DOC-DEV-003 (Project Structure)
- DOC-DEV-004 (Namespace Convention)
- ADR-0001

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial dependency rules |
| 2.0.0 | 2026-07-18 | Standardized according to Documentation Standard v3.0 |