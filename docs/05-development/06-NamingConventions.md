# Naming Conventions

| Property | Value |
|----------|-------|
| **Document ID** | DOC-DEV-007 |
| **Version** | 2.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Purpose

This document defines the official naming conventions used throughout the
**MachineryManagerEnterprise** solution.

Consistent naming improves:

- Readability
- Discoverability
- Navigation
- Refactoring
- Communication between developers

Naming is considered part of the software architecture.

---

# General Rules

All identifiers shall:

- Use English only.
- Use meaningful names.
- Avoid abbreviations unless universally accepted.
- Describe intent rather than implementation.
- Be consistent across the entire solution.

---

# Casing Rules

| Item | Convention |
|-------|------------|
| Namespace | PascalCase |
| Class | PascalCase |
| Record | PascalCase |
| Struct | PascalCase |
| Enum | PascalCase |
| Interface | PascalCase with `I` prefix |
| Method | PascalCase |
| Property | PascalCase |
| Local Variable | camelCase |
| Parameter | camelCase |
| Private Field | `_camelCase` |
| Constant | PascalCase |
| Enum Member | PascalCase |

---

# Class Naming

Classes should represent nouns.

Good examples:

```text
Machine

MaintenancePlan

InventoryService
```

Avoid:

```text
MachineHelper

MachineManager2

DoSomething
```

---

# Interface Naming

Interfaces shall begin with `I`.

Examples

```text
IMachineRepository

IUserService

IClock
```

---

# Method Naming

Methods should represent actions.

Examples

```text
CreateMachine()

CalculateAvailability()

GenerateReport()
```

Boolean methods should answer a question.

Examples

```text
IsActive()

HasPermission()

CanDelete()
```

---

# Variable Naming

Variables should clearly describe their purpose.

Prefer:

```text
availableMachines

maintenanceSchedule
```

Avoid:

```text
tmp

obj

data

x
```

Except for short-lived loop variables.

---

# Collections

Collections should use plural names.

Examples

```text
machines

users

maintenancePlans
```

Single objects should use singular names.

---

# Boolean Variables

Boolean variables should begin with words such as:

- is
- has
- can
- should

Examples

```text
isActive

hasPermission

canEdit

shouldRetry
```

---

# Async Methods

Asynchronous methods shall end with:

```text
Async
```

Example

```csharp
LoadMachinesAsync()
```

---

# Event Handlers

Event handlers should follow:

```text
On<Event>

Handle<Event>
```

Examples

```text
OnMachineCreated

HandleUserDeleted
```

---

# DTOs

DTO names should end with:

```text
Dto
```

Examples

```text
MachineDto

UserDto

InventoryDto
```

---

# Commands

Commands should end with:

```text
Command
```

Examples

```text
CreateMachineCommand

DeleteMachineCommand
```

---

# Queries

Queries should end with:

```text
Query
```

Examples

```text
GetMachineQuery

SearchInventoryQuery
```

---

# Validators

Validators should end with:

```text
Validator
```

Examples

```text
CreateMachineValidator

UserValidator
```

---

# Exceptions

Exceptions should end with:

```text
Exception
```

Examples

```text
MachineNotFoundException

InvalidLicenseException
```

---

# Enumerations

Enum names should be singular.

Examples

```text
MachineStatus

MaintenancePriority
```

Enum members use PascalCase.

---

# Database Tables

Entity names remain singular.

Examples

```text
Machine

MaintenancePlan

InventoryItem
```

Entity Framework will determine pluralization where applicable.

---

# File Names

File names shall match the public type.

Example

```text
MachineService.cs

MachineDto.cs

CreateMachineCommand.cs
```

---

# Namespace Consistency

Namespaces shall always match folder names.

Example

Folder

```text
Features

Inventory

Commands
```

Namespace

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

---

# Forbidden Names

Avoid:

```text
Helper

Util

Misc

CommonStuff

Manager

Data

Info
```

Names should communicate responsibility rather than generic purpose.

---

# Compliance

Every new identifier introduced into the solution shall comply with these conventions.

Exceptions require architectural approval.

---

# Related Documents

- DOC-CONVENTIONS
- DOC-DEV-001 (Development Principles)
- DOC-DEV-004 (Namespace Convention)
- DOC-DEV-006 (Coding Standards)

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial naming conventions |
| 2.0.0 | 2026-07-18 | Standardized according to Documentation Standard v3.0 |