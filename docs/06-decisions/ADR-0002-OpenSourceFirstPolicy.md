# ADR-0002 — Open Source First Policy

**Status:** Accepted

**Date:** 2026-07-18

**Decision Makers**

- Solution Architect
- Development Team

---

# Context

MachineryManagerEnterprise is designed as a long-term Enterprise software platform.

The project is expected to:

- Remain maintainable for many years.
- Be buildable by every developer without purchasing licenses.
- Avoid vendor lock-in.
- Support CI/CD pipelines without commercial dependencies.
- Remain portable across different environments.

Technology selection therefore requires a clear policy regarding software dependencies.

---

# Problem

Many commercial libraries provide rich functionality but introduce:

- License costs
- Vendor lock-in
- Build restrictions
- Upgrade limitations
- Dependency on external licensing policies

The project requires a sustainable dependency strategy.

---

# Decision

MachineryManagerEnterprise adopts an **Open Source First Policy**.

All third-party dependencies shall be open source unless a documented architectural exception is approved.

---

# Architectural Principles

## Open Source Preferred

When multiple libraries satisfy the same requirement, the preferred choice shall be an actively maintained open source project.

---

## Vendor Lock-in Avoidance

No technology shall be selected if it unnecessarily couples the project to a commercial vendor.

---

## Community Support

Preferred libraries should have:

- Active maintainers
- Public issue tracker
- Frequent releases
- Healthy community adoption

---

## License Compatibility

Accepted licenses include:

- MIT
- Apache 2.0
- BSD
- MPL (when appropriate)

Libraries with restrictive or incompatible licenses shall not be adopted.

---

## Long-Term Maintainability

Preference shall be given to projects that demonstrate:

- Long-term roadmap
- Stable release history
- Modern .NET support

---

## Commercial Components

Commercial UI frameworks and libraries are not permitted as part of the project baseline.

Examples include (but are not limited to):

- Telerik UI
- DevExpress Components
- Infragistics
- Syncfusion Commercial Editions

---

## Approved Technology Categories

Typical approved technologies include:

- Entity Framework Core
- Serilog
- MediatR
- FluentValidation
- Mapster
- MudBlazor
- QuestPDF
- ClosedXML
- OpenTelemetry

Subject to individual Technology Evaluation and ADR approval.

---

# Exception Process

Commercial software may only be introduced when all of the following conditions are met:

1. No suitable open source alternative exists.
2. A formal Technology Evaluation has been completed.
3. A dedicated ADR documents the justification.
4. Approval is granted by the Solution Architect.

---

# Consequences

## Positive

- Zero licensing cost for development.
- Easier onboarding.
- Better portability.
- Transparent dependency management.
- Lower long-term maintenance cost.

---

## Negative

- Some commercial features may not be available.
- Additional evaluation effort may be required when selecting libraries.

---

# Scope

This policy applies to:

- Runtime dependencies
- Development dependencies
- Build tools
- Testing libraries
- UI frameworks
- Infrastructure components

---

# Related Decisions

- ADR-0001 — Clean Architecture

---

# References

- Open Source Initiative (OSI)
- Microsoft OSS Guidelines
- .NET Foundation Projects