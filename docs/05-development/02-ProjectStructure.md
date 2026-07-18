# Project Structure

**Document ID:** MME-DEV-002

**Repository Path:** `docs/05-development/02-ProjectStructure.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-DevelopmentPrinciples.md
- 01-SolutionStructure.md
- docs/03-domain/00-DomainPrinciples.md
- docs/04-modules/00-ApplicationArchitecture.md

---

# 1. Purpose

This document defines the internal folder structure of every project within the MachineryManagerEnterprise solution.

The goal is to ensure every project follows the same organizational pattern.

---

# 2. General Principles

Every project shall:

- have one clear responsibility;
- expose a predictable folder structure;
- avoid unnecessary nesting;
- group related concepts together;
- remain understandable without external documentation.

---

# 3. Shared Project

```
Shared

├── Abstractions
├── Common
├── Constants
├── Exceptions
├── Extensions
├── Results
├── Utilities
└── ValueTypes
```

The Shared project contains reusable technical components.

No business logic shall exist here.

---

# 4. Domain Project

```
Domain

├── Aggregates
├── Entities
├── ValueObjects
├── DomainEvents
├── DomainServices
├── Specifications
├── Repositories
├── Enumerations
├── Exceptions
└── Rules
```

Only business concepts belong inside the Domain project.

---

# 5. Application Project

```
Application

├── Commands
├── Queries
├── Handlers
├── Services
├── DTOs
├── Validators
├── Interfaces
├── Workflows
├── Authorization
└── Mappings
```

Application coordinates business execution.

It does not implement business rules.

---

# 6. Infrastructure Project

```
Infrastructure

├── Persistence
│   ├── Context
│   ├── Configurations
│   ├── Repositories
│   └── Migrations
│
├── FileStorage
├── Notifications
├── Logging
├── Email
├── AI
├── ExternalSystems
└── DependencyInjection
```

Infrastructure contains implementation details.

---

# 7. API Project

```
Api

├── Controllers
├── Contracts
├── Filters
├── Middleware
├── Authentication
├── Authorization
├── Swagger
├── DependencyInjection
└── Configuration
```

Controllers should remain thin.

Business execution belongs to the Application Layer.

---

# 8. Test Projects

```
Tests

├── UnitTests
├── IntegrationTests
├── FunctionalTests
├── TestUtilities
└── TestData
```

Every test project shall mirror the production structure whenever practical.

---

# 9. Folder Naming Rules

Folder names shall:

- use PascalCase;
- represent business meaning;
- avoid abbreviations;
- remain singular when representing one concept.

Examples:

- Aggregates
- DomainEvents
- Specifications
- Validators

Avoid:

- Misc
- Utils
- Temp
- NewFolder

---

# 10. Namespace Alignment

Folder hierarchy shall match namespace hierarchy.

Example

```
Application

└── Commands

    └── Asset

        └── RegisterAsset
```

Namespace

```text
MachineryManagerEnterprise.Application.Commands.Asset.RegisterAsset
```

Folder structure and namespace shall never diverge.

---

# 11. File Organization

Each file shall contain one primary type.

Preferred:

```
RegisterAssetCommand.cs

RegisterAssetHandler.cs

RegisterAssetValidator.cs
```

Avoid grouping unrelated classes in the same file.

---

# 12. Growth Strategy

New folders shall only be introduced when:

- they represent a new responsibility;
- existing folders become too large;
- architectural clarity improves.

Folder creation shall never be arbitrary.

---

# 13. Architectural Stability

The project structure is expected to remain stable throughout the lifetime of the system.

Business evolution should primarily add new business types, not reorganize the folder hierarchy.

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Project Structure |