# Technology Evaluation

| Property | Value |
|----------|-------|
| **Document ID** | TE-0009 |
| **Version** | 3.1.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Title

Technology Evaluation — MediatR

---

# Executive Summary

MediatR has been evaluated as the application's request dispatching framework.

It provides an implementation of the Mediator pattern that supports CQRS,
pipeline behaviors, notifications, and loose coupling between the Presentation
and Application layers.

The library is mature, open source, widely adopted within the .NET ecosystem,
and aligns well with the project's Clean Architecture principles.

---

# Evaluation Scope

This evaluation covers:

- Request / Response dispatching
- CQRS implementation support
- Pipeline Behaviors
- Notifications
- Dependency Injection integration
- Long-term maintainability

---

# Candidate

| Property | Value |
|----------|-------|
| Product | MediatR |
| Vendor | Jimmy Bogard |
| License | MIT |
| Repository | https://github.com/jbogard/MediatR |
| NuGet | https://www.nuget.org/packages/MediatR |

---

# Evaluation Criteria

| Criteria | Result |
|----------|--------|
| Open Source | ✔ |
| MIT License | ✔ |
| Active Maintenance | ✔ |
| Community Adoption | ✔ |
| .NET Integration | Excellent |
| Documentation | Excellent |
| Performance | Excellent |
| Vendor Lock-in | Very Low |

---

# Advantages

- Excellent implementation of the Mediator pattern
- Strong CQRS support
- Pipeline Behaviors
- Clean separation between UI and Application
- High testability
- Mature ecosystem
- Native Dependency Injection integration

---

# Disadvantages

- Additional abstraction layer
- Developers must understand request pipeline concepts

---

# Alternatives Considered

## Direct Service Calls

Rejected because they tightly couple Presentation to Application services.

---

## Custom Mediator

Rejected because it duplicates mature functionality already provided by
MediatR.

---

## Event Bus Only

Rejected because synchronous request/response communication remains necessary
inside the application.

---

# Compatibility

| Area | Status |
|------|--------|
| .NET 10 | ✔ |
| Clean Architecture | ✔ |
| Blazor | ✔ |
| FluentValidation | ✔ |
| Mapster | ✔ |
| Entity Framework Core | ✔ |

---

# Risks

| Risk | Mitigation |
|------|------------|
| Overuse of handlers | Follow CQRS guidelines |
| Excessive pipeline behaviors | Keep behaviors focused and modular |

---

# Recommendation

**Approved**

MediatR should be adopted as the standard request dispatching mechanism within
the Application Layer.

---

# Related ADR

ADR-0011 — Use MediatR

---

# Related POC

Not Required

---

# References

https://github.com/jbogard/MediatR

https://www.nuget.org/packages/MediatR

https://github.com/jbogard/MediatR/wiki

---

# Review

| Role | Name | Date |
|------|------|------|
| Solution Architect | | |

---

# Change History

| Version | Date       | Description |
|---------|------------|--------------------|
| 1.0.0   | 2026-07-18 | Initial evaluation |
| 2.0.0   | 2026-07-18 | Standardized |
| 3.0.0   | 2026-07-18 | Rewritten according to Documentation Standard v3.0 |
| 3.1.0   | 2026-07-28 | New section added (Evaluation Scope) |