| Property | Value |
|----------|-------|
| **Document ID** | TE-0009 |
| **Title** | MediatR In-Process Messaging Evaluation |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-26 |
| **Last Updated** | 2026-07-28 |

---

# Purpose

This document evaluates candidate technologies for MediatR In-Process Messaging Evaluation in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

---

# Evaluation Scope

Evaluates in-process messaging technology selection. Distributed messaging is evaluated separately in TE-0012.

---

# Relationship with Previous Technology Evaluations

Establishes the in-process CQRS mediation mechanism for the Application layer.

---

# Architectural References

- ADR-0001 — Clean Architecture
- ADR-0003 — CQRS Pattern Implementation

---

# Scope

Evaluates MediatR vs Wolverine vs Direct Service Invocations.

---

# Functional Requirements

In-process Command/Query dispatch, notification publishing, IPipelineBehavior middleware for validation/logging/transaction management.

---

# Non-Functional Requirements

Loose coupling, clean handler isolation, low overhead execution.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| MediatR | In-Process Mediator & Pipeline | Selected |
| Direct Application Services | Tightly Coupled Services | Evaluated |
| Wolverine | Command Mediator Engine | Evaluated |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Decoupled Request/Handler Pattern | Critical |
| A2 | Pipeline Behavior Support | Critical |

---

# Architecture Principle

Controllers and UI components send Requests/Commands through IMediator; Application handlers execute business logic.

---

# 5. Candidate Deep-Dive Evaluations

## MediatR Evaluation

### Overview
MediatR is an unopinionated in-process messaging library supporting CQRS patterns.

### Architectural Strengths
- Enables cross-cutting concerns (Validation, Logging, Caching) via pipeline behaviors.
- Enforces strict single-responsibility principle per command/query handler.

---

# Overall Technology Comparison

MediatR remains the industry gold standard for C# CQRS applications.

---

# Final Recommendation

Adopt MediatR for all in-process CQRS command and query handling.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| MediatR | Approved |

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility

---

# Related ADR

- ADR-0003 — CQRS Pattern Implementation

---

# Related Documents

- TE-0001 — .NET 10 Application Platform Evaluation

---

# References

- https://github.com/jbogard/MediatR

---

# Revision History

| Version | Date       | Author             | Description        |
|---------|------------|--------------------|--------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial evaluation |
| 2.0.0   | 2026-07-18 | Solution Architect | Standardized       |
| 3.0.0   | 2026-07-18 | Solution Architect | Rewritten according to Documentation Standard v3.0 |
| 3.1.0   | 2026-07-28 | Solution Architect | New section added (Evaluation Scope) |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0 |