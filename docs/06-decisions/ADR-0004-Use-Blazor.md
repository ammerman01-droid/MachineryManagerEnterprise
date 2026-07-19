# Architecture Decision Record

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0004 |
| **Version** | 3.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Title

Use Blazor

---

# Status

Accepted

---

# Context

The MachineryManagerEnterprise solution requires a modern web UI framework that
integrates naturally with the selected .NET platform while minimizing
technological complexity.

The presentation layer should:

- Use the same primary programming language as the backend.
- Support component-based development.
- Integrate seamlessly with ASP.NET Core.
- Be maintainable over many years.
- Avoid unnecessary JavaScript dependencies.
- Align with Microsoft's long-term roadmap.

---

# Decision

The Presentation Layer shall use **Blazor** as the primary UI framework.

Blazor Web Apps (.NET 10) shall be used as the hosting model for the solution.

---

# Decision Drivers

- Single-language development (C#)
- Strong .NET integration
- Component architecture
- Maintainability
- Long-term Microsoft support
- Developer productivity
- Type safety

---

# Alternatives Considered

## React

Rejected because it introduces JavaScript/TypeScript as an additional primary
technology stack.

---

## Angular

Rejected because of higher complexity and steeper learning curve for the project
requirements.

---

## Vue

Rejected because of weaker integration with the .NET ecosystem.

---

# Consequences

## Positive

- Unified technology stack
- Strong compile-time checking
- Excellent ASP.NET Core integration
- Reduced JavaScript complexity
- Improved maintainability
- Shared development skills across backend and frontend

## Negative

- Smaller third-party ecosystem compared to React.
- Some browser features still require JavaScript interoperability.

---

# Architecture Impact

Blazor affects only the **Presentation Layer**.

Application, Domain, Infrastructure, and SharedKernel remain completely
independent of Blazor components.

Presentation communicates with Application only through public contracts.

---

# Implementation Notes

The Presentation project shall use:

- Razor Components
- Dependency Injection
- Component-based UI composition

Interactive rendering mode shall be selected according to application
requirements.

---

# Compliance Rules

1. Blazor shall only exist inside the Presentation layer.

2. Application shall never reference Blazor assemblies.

3. Domain shall never reference Blazor.

4. UI logic shall never contain business rules.

5. Components shall communicate with Application through services or MediatR.

---

# Related Technology Evaluation

TE-0002 — Blazor

---

# Related Proof of Concept

Not Required

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0003 — Use .NET 10
- Dependency Catalog

---

# References

https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor

https://learn.microsoft.com/aspnet/core/blazor/

https://github.com/dotnet/aspnetcore

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