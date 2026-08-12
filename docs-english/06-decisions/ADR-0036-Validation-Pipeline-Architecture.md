| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ADR-0036           |
| **Title**        | Validation Pipeline Architecture    |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-08-02         |
| **Last Updated** | 2026-08-08         |

---

# Context

ADR-0007 approved FluentValidation as the validation tool (based on
TE-0005), but did not address how validation is orchestrated across the
request lifecycle. TE-0022 — Validation Pipeline and Validation
Architecture Evaluation extends TE-0005 to define a full layered
validation architecture, but had no corresponding Architecture Decision
Record. This ADR closes that gap.

---

# Decision

MachineryManagerEnterprise adopts the following layered validation
architecture, formalizing TE-0022:

| Layer | Responsibility | Selected Technology |
|--------|----------------|---------------------|
| Request Validation | Validate DTOs and Commands | FluentValidation |
| Validation Orchestration | Execute validation automatically | MediatR Pipeline Behavior |
| Business Validation | Protect aggregate invariants | Domain Model |
| Persistence Validation | Database constraints | EF Core / Database |

Request validation shall execute automatically through a MediatR
Pipeline Behavior before a Command or Query reaches its handler.
Business invariants shall remain enforced inside the Domain Model,
independent of request-level validation.

---

# Decision Drivers

- Consistency (validation executes uniformly, not ad hoc per handler)
- Clean Architecture (Domain model remains the ultimate authority over
  invariants)
- Reduces duplicated validation logic across handlers

---

# Alternatives Considered

Refer to TE-0022 for the full comparison of validation orchestration
approaches.

---

# Consequences

**Positive**

- Uniform, automatic request validation across all Commands/Queries.
- Clear separation between input validation and business invariants.

**Negative / Trade-offs**

- Adds a MediatR pipeline behavior to every request, with a small,
  bounded performance cost.

---

# Architecture Impact

- Application layer (MediatR pipeline behavior registration).
- Domain layer remains the owner of business invariants.

---

# Implementation Notes

- Register a generic `ValidationBehavior<TRequest, TResponse>` in the
  MediatR pipeline.
- FluentValidation validators shall be discovered and registered
  automatically by assembly scanning.

---

# Compliance Rules

```
Every Command and Query with input parameters shall have a
corresponding FluentValidation validator executed through the MediatR
pipeline. Handlers shall not perform ad hoc input validation.
```

---

# Related Technology Evaluation

```
TE-0022 (extends TE-0005)
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
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0007 — Use FluentValidation
- ADR-0011 — Use MediatR
- TE-0022 — Validation Pipeline and Validation Architecture Evaluation

---

# References

- FluentValidation Documentation
- MediatR Documentation

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial decision, formalizing previously unratified TE-0022 |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes |