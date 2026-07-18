# Dependency Rules

**Document ID:** MME-DEV-004

**Repository Path:** `docs/05-development/04-DependencyRules.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-DevelopmentPrinciples.md
- 01-SolutionStructure.md
- 02-ProjectStructure.md
- 03-NamespaceConvention.md
- docs/02-architecture/01-Architecture.md

---

# 1. Purpose

This document defines the dependency rules between projects, layers and components of MachineryManagerEnterprise.

Correct dependency management is one of the primary mechanisms used to preserve Clean Architecture.

---

# 2. Guiding Principle

Dependencies shall always point toward the business core.

Outer layers depend on inner layers.

Inner layers shall never depend on outer layers.

---

# 3. Dependency Direction

```text
           Presentation

                 │

                 ▼

            Application

                 │

                 ▼

               Domain

                 ▲

                 │

          Infrastructure
```

Infrastructure implements interfaces defined by Application or Domain.

---

# 4. Allowed Project References

## Domain

May reference:

- Shared

May NOT reference:

- Application
- Infrastructure
- Api
- Tests

---

## Application

May reference:

- Domain
- Shared

May NOT reference:

- Api
- Infrastructure (implementation)

---

## Infrastructure

May reference:

- Application
- Domain
- Shared

May NOT reference:

- Api

---

## Api

May reference:

- Application
- Shared

Infrastructure shall be injected through Dependency Injection.

---

## Tests

Test projects may reference production projects.

Production projects shall never reference test projects.

---

# 5. Dependency Matrix

| From | Domain | Application | Infrastructure | Api | Shared |
|------|:------:|:-----------:|:--------------:|:---:|:------:|
| Domain | — | ❌ | ❌ | ❌ | ✅ |
| Application | ✅ | — | ❌ | ❌ | ✅ |
| Infrastructure | ✅ | ✅ | — | ❌ | ✅ |
| Api | ❌ | ✅ | *(runtime only)* | — | ✅ |
| Shared | ❌ | ❌ | ❌ | ❌ | — |

---

# 6. Interface Ownership

Interfaces belong to the consumer.

Example

Repository Interfaces

```
Domain

└── Repositories

    └── IAssetRepository
```

Implementation

```
Infrastructure

└── Persistence

    └── AssetRepository
```

---

# 7. Dependency Injection

Dependency Injection shall be the only mechanism used to connect implementations.

Business code shall never instantiate infrastructure services directly.

Example

```
Application

↓

IEmailSender

↓

Infrastructure

↓

SmtpEmailSender
```

---

# 8. Forbidden Dependencies

The following dependencies are prohibited.

Domain → Entity Framework

Domain → ASP.NET Core

Domain → SQL Server

Domain → File System

Application → EF Core DbContext

Application → HttpContext

Application → IConfiguration

Business logic shall remain infrastructure independent.

---

# 9. External Libraries

External packages shall remain isolated.

Examples

- Entity Framework Core
- Serilog
- MediatR
- AutoMapper
- FluentValidation

Business objects shall not depend directly upon external packages unless explicitly approved.

---

# 10. Circular Dependencies

Circular dependencies are strictly prohibited.

Example

```
Project A

↓

Project B

↓

Project C

↓

Project A ❌
```

The solution shall compile without cyclic references.

---

# 11. Runtime Dependencies

Runtime dependencies are acceptable when configured through Dependency Injection.

Compile-time dependencies shall still respect architectural boundaries.

---

# 12. Cross-Cutting Concerns

Cross-cutting services include:

- Logging
- Caching
- Notifications
- File Storage
- Email
- AI Providers

These services shall be accessed only through abstractions.

---

# 13. Future Expansion

New projects shall follow the same dependency model.

No new project may violate these dependency rules.

Architectural exceptions require an ADR.

---

# 14. Architectural Validation

Dependency rules shall be verified during:

- Architecture Review
- Code Review
- Build Pipeline
- Automated Architecture Tests (future)

Violations shall be corrected before merge.

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Dependency Rules |