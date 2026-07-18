# Namespace Convention

**Document ID:** MME-DEV-003

**Repository Path:** `docs/05-development/03-NamespaceConvention.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-DevelopmentPrinciples.md
- 01-SolutionStructure.md
- 02-ProjectStructure.md

---

# 1. Purpose

This document defines the namespace conventions used throughout MachineryManagerEnterprise.

A consistent namespace hierarchy improves readability, discoverability, maintainability and long-term scalability.

---

# 2. General Principles

Namespaces shall:

- reflect project structure;
- reflect business responsibilities;
- remain stable over time;
- avoid unnecessary depth;
- never expose implementation technology.

---

# 3. Root Namespace

All production projects shall begin with:

```text
MachineryManagerEnterprise
```

Examples

```text
MachineryManagerEnterprise.Domain

MachineryManagerEnterprise.Application

MachineryManagerEnterprise.Infrastructure

MachineryManagerEnterprise.Api

MachineryManagerEnterprise.Shared
```

---

# 4. Namespace Hierarchy

The namespace hierarchy shall mirror the folder hierarchy.

Example

```text
Application

└── Commands

    └── Asset

        └── RegisterAsset
```

Namespace

```text
MachineryManagerEnterprise.Application.Commands.Asset.RegisterAsset
```

Folder and namespace shall always match.

---

# 5. Domain Namespaces

Examples

```text
MachineryManagerEnterprise.Domain.Aggregates

MachineryManagerEnterprise.Domain.Entities

MachineryManagerEnterprise.Domain.ValueObjects

MachineryManagerEnterprise.Domain.DomainEvents

MachineryManagerEnterprise.Domain.DomainServices

MachineryManagerEnterprise.Domain.Specifications

MachineryManagerEnterprise.Domain.Repositories
```

---

# 6. Application Namespaces

Examples

```text
MachineryManagerEnterprise.Application.Commands

MachineryManagerEnterprise.Application.Queries

MachineryManagerEnterprise.Application.Handlers

MachineryManagerEnterprise.Application.Services

MachineryManagerEnterprise.Application.DTOs

MachineryManagerEnterprise.Application.Workflows

MachineryManagerEnterprise.Application.Authorization
```

---

# 7. Infrastructure Namespaces

Examples

```text
MachineryManagerEnterprise.Infrastructure.Persistence

MachineryManagerEnterprise.Infrastructure.Persistence.Repositories

MachineryManagerEnterprise.Infrastructure.Logging

MachineryManagerEnterprise.Infrastructure.Notifications

MachineryManagerEnterprise.Infrastructure.FileStorage

MachineryManagerEnterprise.Infrastructure.ExternalSystems
```

---

# 8. API Namespaces

Examples

```text
MachineryManagerEnterprise.Api.Controllers

MachineryManagerEnterprise.Api.Authentication

MachineryManagerEnterprise.Api.Authorization

MachineryManagerEnterprise.Api.Filters

MachineryManagerEnterprise.Api.Middleware
```

---

# 9. Test Namespaces

Examples

```text
MachineryManagerEnterprise.Tests.Unit

MachineryManagerEnterprise.Tests.Integration

MachineryManagerEnterprise.Tests.Functional
```

Test namespaces should resemble the production namespaces they verify.

---

# 10. Feature Organization

Features shall be grouped by business capability.

Preferred

```text
Commands.Asset.RegisterAsset

Commands.Asset.TransferAsset

Commands.Asset.DisposeAsset
```

Avoid grouping unrelated features together.

---

# 11. Forbidden Namespaces

The following namespace names shall never appear:

```text
Common

Misc

Helpers

Utils

Temp

NewFolder

General
```

Every namespace must communicate business meaning.

---

# 12. Namespace Length

Namespaces should remain concise.

Recommended maximum depth:

```text
6 levels
```

Example

```text
MachineryManagerEnterprise.Application.Commands.Asset.RegisterAsset
```

Avoid deeply nested namespaces without architectural value.

---

# 13. Refactoring Rules

Namespace changes shall occur only when:

- architectural responsibilities change;
- project structure changes;
- ADR approval exists.

Namespace changes shall never be cosmetic.

---

# 14. Stability

Namespaces are considered part of the public architecture.

Frequent namespace changes increase maintenance cost and shall be avoided.

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Namespace Convention |