# Architecture Decision Record Index

| Property | Value |
|----------|-------|
| **Document ID** | ADR-INDEX |
| **Version** | 3.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Purpose

This document provides the official index of all Architecture Decision Records
(ADR) within the MachineryManagerEnterprise project.

Each ADR documents a significant architectural decision together with its
context, rationale, consequences, and related technology evaluation.

---

# ADR Lifecycle

```text
Technology Evaluation (TE)
            │
            ▼
Architecture Decision Record (ADR)
            │
            ▼
Implementation
            │
            ▼
Architecture Tests (Future)
```

---

# Architecture Decision Records

| ADR | Title | Status | Related TE |
|------|-------------------------------|----------|------------|
| ADR-0001 | Adopt Clean Architecture | Accepted | — |
| ADR-0002 | Adopt Open Source First Policy | Accepted | — |
| ADR-0003 | Use .NET 10 | Accepted | TE-0001 |
| ADR-0004 | Use Blazor | Accepted | TE-0002 |
| ADR-0005 | Use MudBlazor | Accepted | TE-0003 |
| ADR-0006 | Use Entity Framework Core | Accepted | TE-0004 |
| ADR-0007 | Use FluentValidation | Accepted | TE-0005 |
| ADR-0008 | Use Mapster | Accepted | TE-0006 |
| ADR-0009 | Use Serilog | Accepted | TE-0007 |
| ADR-0010 | Use OpenTelemetry | Accepted | TE-0008 |
| ADR-0011 | Use MediatR | Accepted | TE-0009 |

---

# Decision Categories

## Architectural Principles

- ADR-0001
- ADR-0002

---

## Platform

- ADR-0003 (.NET 10)

---

## Presentation

- ADR-0004 (Blazor)
- ADR-0005 (MudBlazor)

---

## Infrastructure

- ADR-0006 (Entity Framework Core)
- ADR-0009 (Serilog)
- ADR-0010 (OpenTelemetry)

---

## Application Layer

- ADR-0007 (FluentValidation)
- ADR-0008 (Mapster)
- ADR-0011 (MediatR)

---

# Traceability Matrix

| Technology Evaluation | Architecture Decision |
|-----------------------|-----------------------|
| TE-0001 | ADR-0003 |
| TE-0002 | ADR-0004 |
| TE-0003 | ADR-0005 |
| TE-0004 | ADR-0006 |
| TE-0005 | ADR-0007 |
| TE-0006 | ADR-0008 |
| TE-0007 | ADR-0009 |
| TE-0008 | ADR-0010 |
| TE-0009 | ADR-0011 |

---

# Governance

Every accepted technology shall satisfy the following lifecycle:

1. Technology Evaluation
2. Architecture Decision Record
3. Implementation
4. Architecture Validation (future)

No production technology shall be adopted without a corresponding ADR.

---

# Related Documents

- DOCUMENT_CONVENTIONS.md
- README.md
- Dependency Catalog
- Technology Evaluations

---

# Change History

| Version | Date | Description |
|----------|------------|-------------------------------------------|
| 3.0.0 | 2026-07-18 | Initial Architecture Decision Index |