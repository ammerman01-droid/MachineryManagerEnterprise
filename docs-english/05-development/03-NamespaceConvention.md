| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOC-DEV-004        |
| **Title**        | Namespace Convention |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

This document defines the official namespace conventions for the
**MachineryManagerEnterprise** solution.

A consistent namespace hierarchy improves:

- Readability
- Navigation
- Refactoring
- Discoverability
- Long-term maintainability

Every project shall follow these conventions.

---

# General Principles

Namespaces shall:

- Reflect the physical project structure.
- Be deterministic.
- Avoid unnecessary nesting.
- Follow project boundaries.
- Never expose implementation details.

Namespaces are part of the architecture and shall not be considered arbitrary.

---

# Root Namespace

Every project begins with the same root namespace.

```text
MachineryManagerEnterprise
```

---

# Project Namespaces

Each project extends the root namespace.

Examples

```text
MachineryManagerEnterprise.SharedKernel

MachineryManagerEnterprise.Domain

MachineryManagerEnterprise.Application

MachineryManagerEnterprise.Infrastructure

MachineryManagerEnterprise.Web
```

---

# Feature Namespaces

Business functionality should be grouped by feature.

Example

```text
MachineryManagerEnterprise.Application.Features.Inventory

MachineryManagerEnterprise.Application.Features.Users

MachineryManagerEnterprise.Application.Features.Maintenance
```

---

# Commands

Commands shall be located under the corresponding feature.

Example

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

---

# Queries

Queries follow the same convention.

```text
MachineryManagerEnterprise.Application.Features.Inventory.Queries
```

---

# Validators

Validators belong beside their corresponding Commands or Queries.

Example

```text
MachineryManagerEnterprise.Application.Features.Inventory.Validation
```

---

# DTOs

DTOs remain inside the owning feature.

```text
MachineryManagerEnterprise.Application.Features.Inventory.DTOs
```

---

# Mapping

Mapping profiles remain grouped by feature.

```text
MachineryManagerEnterprise.Application.Features.Inventory.Mapping
```

---

# Domain

Domain namespaces should reflect business concepts rather than technical layers.

Example

```text
MachineryManagerEnterprise.Domain.Inventory

MachineryManagerEnterprise.Domain.Users

MachineryManagerEnterprise.Domain.Maintenance
```

Avoid namespaces such as:

```text
Domain.Entities

Domain.Models

Domain.Classes
```

The business concept is more important than the technical artifact.

---

# Infrastructure

Infrastructure namespaces reflect implementation details.

Examples

```text
MachineryManagerEnterprise.Infrastructure.Persistence

MachineryManagerEnterprise.Infrastructure.Identity

MachineryManagerEnterprise.Infrastructure.Logging

MachineryManagerEnterprise.Infrastructure.Caching
```

---

# Web

Presentation namespaces should follow UI organization.

Examples

```text
MachineryManagerEnterprise.Web.Components

MachineryManagerEnterprise.Web.Pages

MachineryManagerEnterprise.Web.Layout

MachineryManagerEnterprise.Web.Shared
```

---

# Tests

Test projects mirror production namespaces.

Example

```text
MachineryManagerEnterprise.Application.Tests.Features.Inventory
```

This makes locating corresponding production code straightforward.

---

# Naming Rules

Namespaces shall:

- Use PascalCase.
- Never contain spaces.
- Never use abbreviations unless universally accepted.
- Never contain version numbers.
- Never expose implementation technology.

---

# Maximum Namespace Depth

Excessively deep namespaces reduce readability.

Recommended depth:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

Avoid structures such as:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands.Create.Internal.Models
```

---

# Namespace Equals Folder

Every namespace should match its physical folder.

Example

```text
Features

Inventory

Commands

CreateMachineCommand.cs
```

Namespace

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

This one-to-one relationship simplifies navigation and refactoring.

---

# Future Modules

New bounded contexts should become new namespace roots.

Example

```text
MachineryManagerEnterprise.Inventory

MachineryManagerEnterprise.Finance

MachineryManagerEnterprise.HumanResources
```

This keeps modules independent as the solution grows.

---

# Compliance

Every newly created namespace shall comply with this document.

Namespace deviations require architectural approval through an ADR.

---

# Related Documents

- DOC-CONVENTIONS
- DOC-README
- DOC-DEV-001 (Development Principles)
- DOC-DEV-002 (Solution Structure)
- DOC-DEV-003 (Project Structure)
- DOC-DEV-005 (Dependency Rules)

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | Solution Architect | 2026-07-18 | Initial namespace conventions                         |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |