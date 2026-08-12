| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ADR-0035           |
| **Title**        | API Documentation and Client Generation Architecture |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-08-02         |
| **Last Updated** | 2026-08-08         |

---

# Context

TE-0021 — API Documentation and Client Generation Technology Evaluation
was approved but had no corresponding Architecture Decision Record. The
platform requires a consistent, vendor-neutral approach to describing
its APIs and generating client SDKs for internal and future external
consumers.

---

# Decision

MachineryManagerEnterprise adopts the following API documentation and
client generation architecture, formalizing TE-0021:

| Responsibility | Selected Technology |
|----------------|----------------------|
| API Contract Standard | OpenAPI 3.x |
| Interactive Documentation | Scalar |
| Primary Client SDK Generation | NSwag |
| Multi-language SDK Generation | Kiota (optional, future) |
| Legacy Documentation UI | Swagger UI (compatibility only) |

The canonical OpenAPI document shall be generated natively from
ASP.NET Core. Scalar is the primary interactive documentation UI; Swagger
UI is retained only for backward compatibility.

---

# Decision Drivers

- Vendor neutrality (OpenAPI as industry standard)
- .NET 10 alignment and developer experience (Scalar)
- Mature C# client generation (NSwag)
- Long-term Maintainability

---

# Alternatives Considered

Refer to TE-0021 for the full candidate comparison across documentation
UIs and client generators.

---

# Consequences

**Positive**

- Single canonical, machine-readable API contract (OpenAPI).
- Automated, consistent client SDK generation.

**Negative / Trade-offs**

- Maintaining two documentation UIs (Scalar + legacy Swagger UI) during
  the transition period.

---

# Architecture Impact

- Presentation layer (API project) only. No impact on Domain or
  Application layers.

---

# Implementation Notes

- Enable native OpenAPI generation in ASP.NET Core.
- Configure Scalar as the default documentation route; retain Swagger UI
  only where existing tooling depends on it.
- Generate client SDKs via NSwag as part of the build pipeline.

---

# Compliance Rules

```
All public API endpoints shall be described through the canonical
OpenAPI document. No endpoint shall be exempted from documentation
generation.
```

---

# Related Technology Evaluation

```
TE-0021
```

---

# Related Proof of Concept

```
Not Required
```

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- TE-0021 — API Documentation and Client Generation Technology Evaluation

---

# References

- OpenAPI Specification
- Scalar Documentation
- NSwag Documentation

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial decision, formalizing previously unratified TE-0021 |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes |