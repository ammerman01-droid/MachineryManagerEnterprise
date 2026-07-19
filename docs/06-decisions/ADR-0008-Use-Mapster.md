# Architecture Decision Record

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0008 |
| **Version** | 3.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Title

Use Mapster

---

# Status

Accepted

---

# Context

The MachineryManagerEnterprise solution requires a lightweight object mapping
library to transform data between Domain entities, DTOs, ViewModels, Commands,
Queries, and Responses.

The selected mapping solution should:

- Minimize boilerplate code
- Deliver high runtime performance
- Integrate naturally with .NET
- Support compile-time mapping
- Remain easy to maintain
- Align with Clean Architecture

---

# Decision

The Application Layer shall use **Mapster** as the standard object mapping
framework.

Object transformations between architectural boundaries shall be implemented
using Mapster.

---

# Decision Drivers

- High performance
- Low memory allocation
- Simplicity
- Compile-time code generation support
- Open Source
- Maintainability
- Excellent .NET integration

---

# Alternatives Considered

## AutoMapper

Rejected because it introduces additional runtime overhead and is less efficient
than Mapster in common mapping scenarios.

---

## Manual Mapping

Rejected because it increases boilerplate code, duplication, and maintenance
effort.

---

## Mapperly

Rejected because although compile-time mapping is attractive, Mapster currently
offers a broader ecosystem and greater flexibility for the project's needs.

---

# Consequences

## Positive

- Reduced mapping boilerplate
- High runtime performance
- Consistent mapping strategy
- Easy maintenance
- Cleaner application code

## Negative

- Developers must understand mapping configuration.
- Incorrect mappings may introduce subtle runtime bugs if not tested.

---

# Architecture Impact

Mapster shall exist only inside the **Application Layer**.

Presentation shall never contain mapping logic.

Domain entities shall remain unaware of DTOs.

Infrastructure shall not perform business object mapping.

---

# Implementation Notes

Mapping configuration shall be centralized.

Mapping profiles should be organized by module.

Compile-time mapping should be preferred where practical.

Complex mappings shall be covered by unit tests.

---

# Compliance Rules

1. Mapster shall only exist inside Application.

2. Domain shall never reference Mapster.

3. Presentation shall never perform object mapping.

4. Mapping shall occur only across architectural boundaries.

5. Domain entities shall never reference DTOs.

6. Mapping configuration shall be centralized.

---

# Related Technology Evaluation

TE-0006 — Mapster

---

# Related Proof of Concept

Not Required

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0007 — Use FluentValidation
- Dependency Catalog

---

# References

https://github.com/MapsterMapper/Mapster

https://www.nuget.org/packages/Mapster

https://github.com/MapsterMapper/Mapster/wiki

---

# Review

| Role | Name | Date |
|------|------|------|
| Solution Architect | | |

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial decision |
| 2.0.0 | 2026-07-18 | Standardized |
| 3.0.0 | 2026-07-18 | Rewritten according to ADR Template v3.0 |