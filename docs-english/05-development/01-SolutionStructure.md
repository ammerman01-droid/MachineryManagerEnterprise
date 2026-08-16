| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOC-DEV-002        |
| **Title**        | Solution Structure |
| **Version**      | 4.2.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-12         |

---

# Purpose

This document defines the overall solution structure of the
**MachineryManagerEnterprise** project.

The purpose of the solution structure is to ensure that every project has a
clear responsibility, predictable dependencies, and long-term maintainability.

The solution structure is the highest organizational level of the source code.

---

# Objectives

The solution shall:

- Separate business concerns
- Isolate infrastructure
- Support modular development
- Enable independent testing
- Reduce coupling
- Improve maintainability

---

## Central Build Configuration

The solution uses centralized MSBuild configuration.

- Directory.Build.props contains common MSBuild properties shared by all projects.
- Directory.Packages.props manages NuGet package versions centrally.
- All project files inherit these settings automatically.
- Project files must not duplicate shared MSBuild properties or package versions.

All common MSBuild properties are inherited from Directory.Build.props.

TargetFramework is defined centrally in Directory.Build.props.

Individual project files must not redefine TargetFramework unless explicitly documented.

Individual project files must only contain project-specific configuration such as:

- SDK selection
- OutputType
- UserSecretsId
- Razor-specific settings
- ProjectReference
- PackageReference

---

# Architectural Style

The solution follows the principles of:

- Domain-Driven Design (DDD)
- Clean Architecture
- Vertical Slice Architecture (where appropriate)
- Dependency Inversion
- SOLID Principles

---

# Solution Organization

The solution is organized into multiple projects with clearly defined responsibilities.

A typical high-level organization is shown below.

```text
MachineryManagerEnterprise.sln

│
├── src
│
├── tests
│
├── docs
│
├── build
│
└── tools
```

---

# Source Projects

The source code is divided into logical layers.

```text
src

BuildingBlocks

Modules

Host
```

Each project has a single well-defined responsibility.

---

## BuildingBlocks

```text
BuildingBlocks

MachineryManager.SharedKernel

MachineryManager.SharedKernel.Contracts

MachineryManager.SharedKernel.Abstractions

MachineryManager.SharedKernel.Infrastructure

MachineryManager.UI
```

The BuildingBlocks layer contains reusable components shared by all modules.

Business logic shall never be implemented in BuildingBlocks.

---

## Modules

Business functionality shall be implemented as independent bounded contexts.

Each module follows Clean Architecture internally.

```text
Modules

AssetManagement

AssetManagement.Domain

AssetManagement.Application

AssetManagement.Infrastructure

AssetManagement.Presentation
```

The same structure shall be followed for every business module.

---

## Host

```text
Host

MachineryManager.Server

MachineryManager.Client
```

The Host layer composes the application and configures dependency injection, middleware and application startup.

---

# Dependency Direction

Dependencies shall always point inward.

Presentation

↓

Application

↓

Domain

↓

BuildingBlocks

Infrastructure supports higher layers but shall not introduce business logic.

---

# BuildingBlocks

The BuildingBlocks layer contains reusable components shared across the entire solution.

It includes:

- SharedKernel
- Contracts
- Abstractions
- Infrastructure (cross-cutting implementations, e.g. shared MediatR pipeline behaviors)
- UI Shared Components

BuildingBlocks shall never depend on any business module.

---

# Domain Layer

The Domain layer contains:

- Entities
- Value Objects
- Aggregates
- Domain Services
- Domain Events
- Business Rules

The Domain layer shall contain no infrastructure code.

---

# Application Layer

The Application layer contains:

- Use Cases
- Commands
- Queries
- Validators
- DTOs
- Interfaces
- Mapping

Business workflows belong here.

---

# Infrastructure Layer

Infrastructure contains technical implementations.

Examples:

- Entity Framework Core
- Repositories
- External Services
- File Storage
- Logging
- Caching

Infrastructure shall implement abstractions defined by higher layers.

---

# Presentation Layer

Presentation contains:

- Blazor UI
- Components
- Pages
- View Models

Presentation shall not contain business rules.

---

# Test Projects

Testing projects shall mirror the production solution.

tests

SharedKernel.Tests

AssetManagement.Tests

Maintenance.Tests

Inventory.Tests

Fleet.Tests

Procurement.Tests

Workshop.Tests

Reporting.Tests

Identity.Tests

---

# Naming Principles

Projects shall:

- Use PascalCase
- Match namespaces
- Reflect responsibilities
- Avoid ambiguous names

---

# Scalability

The solution structure is designed to support future expansion without major restructuring.

New modules should be added through new projects or bounded contexts rather than modifying unrelated components.

Every bounded context shall be implemented as an independent module.

Modules communicate only through contracts and application boundaries.

Future extraction into independent services shall not require architectural restructuring.

---

# Compliance

All new projects introduced into the solution shall comply with this structure.

Architectural deviations require an approved ADR.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- DOC-CONVENTIONS
- DOC-README
- DOC-DEV-001 (Development Principles)
- DOC-DEV-003 (Project Structure)
- DOC-DEV-005 (Dependency Rules)
- ADR-0001
- MOD-000 (Application Architecture)
- DOM-003 (Bounded Contexts)

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial solution structure                            |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 3.1.0   | 2026-07-26 | Solution Architect | AI + Project Team | Updated solution bootstrap for .NET 10.0.302, centralized MSBuild configuration and Central Package Management.|
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-12 | Solution Architect | Fixed malformed references DOC-MOD-001 and DOC-DOM-002 (wrong prefix and wrong number) to the real MOD-000 and DOM-003 |
| 4.2.0   | 2026-08-12 | Solution Architect | Corrected the BuildingBlocks project names to match the actual scaffolded solution (MachineryManager.SharedKernel.Contracts / .Abstractions / .Infrastructure, not the previously documented MachineryManager.Contracts / .Abstractions without the SharedKernel prefix); added the missing Infrastructure sub-project |