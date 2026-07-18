# Naming Conventions

**Document ID:** MME-DEV-006

**Repository Path:** `docs/05-development/06-NamingConventions.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-DevelopmentPrinciples.md
- 02-ProjectStructure.md
- 03-NamespaceConvention.md
- 05-CodingStandards.md

---

# 1. Purpose

This document defines the naming conventions used throughout MachineryManagerEnterprise.

Consistent naming improves readability, discoverability and long-term maintainability.

---

# 2. General Principles

Names shall be:

- Business oriented
- Explicit
- Descriptive
- Consistent
- Predictable

Names shall communicate intent rather than implementation.

---

# 3. Language

All source code shall be written in English.

Business terminology shall follow the ubiquitous language defined by the Domain documentation.

---

# 4. Casing Rules

| Element | Convention |
|----------|------------|
| Namespace | PascalCase |
| Class | PascalCase |
| Interface | PascalCase with `I` prefix |
| Method | PascalCase |
| Property | PascalCase |
| Enum | PascalCase |
| Enum Member | PascalCase |
| Record | PascalCase |
| Local Variable | camelCase |
| Parameter | camelCase |
| Private Field | `_camelCase` |
| Constant | PascalCase |
| Static Readonly | PascalCase |

---

# 5. Class Naming

Classes shall be named using nouns.

Examples

```
Asset

Engine

MaintenancePlan

MeterReading

ForecastResult
```

Avoid

```
AssetManager

EngineProcessor

DataObject

Helper
```

---

# 6. Interface Naming

Interfaces shall begin with `I`.

Examples

```
IAssetRepository

IUnitOfWork

IEmailSender

IForecastProvider
```

---

# 7. Command Naming

Commands shall begin with a verb.

Examples

```
RegisterAssetCommand

ReplaceEngineCommand

CompleteMaintenanceCommand

GenerateForecastCommand
```

---

# 8. Query Naming

Queries shall begin with:

- Get
- Search
- Compare
- Find

Examples

```
GetAssetQuery

SearchAssetsQuery

CompareForecastsQuery

GetMaintenanceHistoryQuery
```

---

# 9. Handler Naming

Command Handlers

```
RegisterAssetCommandHandler

ReplaceEngineCommandHandler
```

Query Handlers

```
GetAssetQueryHandler

SearchAssetsQueryHandler
```

---

# 10. Event Naming

Domain Events shall describe something that has already happened.

Examples

```
AssetRegistered

EngineInstalled

MaintenanceCompleted

MeterReplaced

ForecastGenerated
```

Avoid imperative names.

Incorrect

```
RegisterAsset

ReplaceMeter
```

---

# 11. Aggregate Naming

Aggregates shall use business nouns.

Examples

```
Asset

Engine

MaintenanceOrder

FinancialRecord
```

---

# 12. Value Object Naming

Value Objects shall describe immutable business concepts.

Examples

```
Money

HourMeter

OperationalUsage

SerialNumber

RegistrationNumber
```

---

# 13. Enumeration Naming

Enumeration names shall be singular.

Examples

```
AssetStatus

MaintenanceType

FuelType

DocumentCategory
```

Enumeration members shall also use PascalCase.

```
Active

Inactive

Retired

Disposed
```

---

# 14. Repository Naming

Repository Interfaces

```
IAssetRepository

IEngineRepository
```

Implementations

```
AssetRepository

EngineRepository
```

---

# 15. Service Naming

Application Services

```
AssetApplicationService

ForecastApplicationService
```

Domain Services

```
MaintenanceScheduler

DepreciationCalculator

ForecastEngine
```

---

# 16. DTO Naming

DTOs shall end with:

```
Dto
```

Examples

```
AssetDto

EngineDto

ForecastDto
```

---

# 17. Validator Naming

Validators shall end with:

```
Validator
```

Examples

```
RegisterAssetCommandValidator

ReplaceEngineCommandValidator
```

---

# 18. Test Naming

Test classes shall mirror production classes.

Examples

```
AssetTests

RegisterAssetCommandHandlerTests

ForecastEngineTests
```

Test methods should follow:

```
MethodName_ShouldExpectedBehavior_WhenCondition
```

Example

```
ReplaceEngine_ShouldCreateHistory_WhenReplacementSucceeds
```

---

# 19. Forbidden Names

The following names shall not appear in production code.

```
Helper

Manager

Processor

Utility

Common

Misc

Temp

Test123
```

Names shall always express business meaning.

---

# 20. Abbreviations

Avoid abbreviations unless universally accepted.

Allowed

```
DTO

API

URL

ID

HTTP

JSON
```

Avoid

```
Cfg

Tmp

Mgr

Proc

Val
```

---

# 21. Stability

Public names become part of the architecture.

Renaming public types requires:

- Architectural review
- Documentation update
- Refactoring of dependent code

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Naming Convention |