# Project Structure

| Property | Value |
|----------|-------|
| **Document ID** | DOC-DEV-003 |
| **Version** | 2.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Purpose

This document defines the internal structure that every project within the
MachineryManagerEnterprise solution shall follow.

A consistent project structure improves:

- Maintainability
- Discoverability
- Code Reviews
- Scalability
- Team Collaboration

Every project shall follow these conventions unless an approved Architecture
Decision Record explicitly defines an exception.

---

# Objectives

The project structure shall:

- Keep related code together.
- Minimize navigation complexity.
- Encourage feature cohesion.
- Reduce accidental dependencies.
- Support long-term maintainability.

---

# General Principles

Each project should:

- Have a single responsibility.
- Contain only files relevant to that responsibility.
- Avoid unnecessary folder nesting.
- Follow the project's naming conventions.
- Be organized consistently with similar projects.

---

# Standard Project Layout

A typical project should follow this structure.

```text
Project

│
├── Abstractions
│
├── Configuration
│
├── Constants
│
├── Contracts
│
├── Exceptions
│
├── Extensions
│
├── Features
│
├── Interfaces
│
├── Mapping
│
├── Models
│
├── Options
│
├── Services
│
├── Utilities
│
└── Validation
```

Not every project requires every folder.

Folders shall only exist when needed.

---

# Repository Root

/
├── docs/
├── src/
├── tests/
├── global.json
├── Directory.Build.props
├── Directory.Packages.props
├── MachineryManagerEnterprise.slnx
└── README.md

---

# Feature Organization

Business functionality should be grouped by feature whenever possible.

Example

```text
Features

Inventory

Maintenance

Users

Reports

Dashboard
```

Each feature should remain as independent as practical.

---

# Feature Layout

A feature may contain:

```text
Inventory

Commands

Queries

DTOs

Validators

Mappings

Services
```

This layout supports Vertical Slice Architecture while remaining compatible
with Clean Architecture.

---

# Configuration

Configuration classes should be isolated.

```text
Configuration

DependencyInjection

OptionsConfiguration

MiddlewareConfiguration
```

---

# Extensions

Extension methods should be grouped by responsibility.

Example

```text
Extensions

ServiceCollectionExtensions

ApplicationBuilderExtensions

StringExtensions
```

---

# Mapping

Object mappings should be centralized.

Supported mapping libraries should follow the same organization.

Example

```text
Mapping

InventoryProfile

UserProfile

MachineProfile
```

---

# Validation

Validators shall be grouped together.

Example

```text
Validation

CreateMachineValidator

UpdateMachineValidator

DeleteMachineValidator
```

---

# Services

Only business-related services should be placed here.

Infrastructure services belong inside the Infrastructure project.

---

# Utilities

Utility classes should remain small and stateless.

Business logic must never be implemented inside utility classes.

---

# Naming Rules

Folders:

- PascalCase

Files:

- PascalCase

Classes:

- PascalCase

Interfaces:

- Prefix `I`

Examples

```
MachineService

IMachineRepository

MachineProfile

MachineValidator
```

---

# Folder Creation Policy

Folders shall not be created in anticipation of future requirements.

Only introduce a folder when:

- It contains meaningful content.
- Multiple files justify its existence.
- It improves organization.

Avoid empty folders.

---

# Scalability

The project structure is intentionally designed to support future growth.

As modules evolve, new features should be added without restructuring unrelated
parts of the project.

---

# Compliance

Every newly created project shall follow this structure unless an approved ADR
defines an alternative organization.

---

# Related Documents

- DOC-CONVENTIONS
- DOC-README
- DOC-DEV-001 (Development Principles)
- DOC-DEV-002 (Solution Structure)
- DOC-DEV-004 (Namespace Convention)
- DOC-DEV-005 (Dependency Rules)

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial project structure |
| 2.0.0 | 2026-07-18 | Standardized according to Documentation Standard v3.0 |