# Architecture Decision Record

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0003 |
| **Version** | 3.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Title

Use .NET 10

---

# Status

Accepted

---

# Context

MachineryManagerEnterprise requires a modern, high-performance application
platform capable of supporting long-term enterprise development.

The selected platform must provide:

- Excellent runtime performance
- Long-term support
- Modern language features
- Cross-platform capability
- Strong tooling
- Active ecosystem
- Native cloud support

The platform must also align with the project's Clean Architecture and Open
Source First principles.

---

# Decision

The project shall use **.NET 10** as the primary development platform.

All solution projects shall target the .NET 10 SDK unless an explicit
architectural exception is approved.

---

# Decision Drivers

- Performance
- Long-Term Support
- Maintainability
- Technology Independence
- Ecosystem
- Tooling
- Cloud Readiness
- Developer Productivity

---

# Alternatives Considered

## .NET 9

Rejected because .NET 10 provides the latest platform improvements and becomes
the long-term foundation of the project.

---

## Java Spring Boot

Rejected because it introduces an additional technology stack while offering no
significant architectural advantage for this solution.

---

## Node.js

Rejected because it is less suitable for the project's enterprise architecture
and strongly typed backend requirements.

---

# Consequences

## Positive

- Unified technology stack
- Excellent runtime performance
- Strong Microsoft support
- Modern C# language features
- Excellent tooling
- High maintainability

## Negative

- Future major-version upgrades require planning.
- SDK updates should be validated before adoption.

---

# Architecture Impact

.NET 10 forms the technical foundation of every architectural layer.

Future technology selections shall remain compatible with the selected runtime.

---

# Implementation Notes

All projects shall target:

```xml
<TargetFramework>net10.0</TargetFramework>
```

SDK selection shall be controlled through:

```
global.json
```

where appropriate.

---

# Compliance Rules

1. All projects shall target .NET 10.

2. Preview SDKs shall not be used in production.

3. Runtime versions shall remain consistent across the solution.

4. New dependencies shall support .NET 10.

---

# Related Technology Evaluation

TE-0001 — .NET 10

---

# Related Proof of Concept

Not Required

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- Dependency Catalog

---

# References

https://dotnet.microsoft.com/

https://learn.microsoft.com/dotnet/

https://github.com/dotnet/runtime

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