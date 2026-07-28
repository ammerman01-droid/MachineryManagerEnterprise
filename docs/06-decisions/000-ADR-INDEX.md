# Architecture Decision Record Index

| Property | Value |
|----------|-------|
| **Document ID** | ADR-INDEX |
| **Version** | 3.5.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

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
| ADR-0012 | Distributed Workspace Architecture | Accepted | — |
| ADR-0013 | Client Application Architecture | Proposed | — |
| ADR-0014 | Workspace Data Architecture | Proposed | — |
| ADR-0015 | Workspace Synchronization Architecture | Proposed | — |
| ADR-0016 | Enterprise Messaging Architecture | Proposed | — |
| ADR-0017 | Artificial Intelligence Integration Architecture | Proposed | — |
| ADR-0018 | External Integration Architecture | Proposed | — |
| ADR-0019 | Hybrid Persistence Strategy for Read-Heavy Queries | Accepted | TE-0024 |
| ADR-0020 | File Storage Strategy | Accepted | TE-0026 |
| ADR-0021 | Search Strategy | Accepted | TE-0027 |
| ADR-0022 | AI Knowledge Retrieval Architecture | Accepted | TE-0028 |
| ADR-0023 | Artificial Intelligence Provider Strategy | Accepted | TE-0029 |
| ADR-0024 | Enterprise Testing Strategy | Accepted | TE-0030 |
| ADR-0025 | Build and Deployment Architecture | Accepted | TE-0031 |

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
- ADR-0019 (Hybrid Persistence Strategy for Read-Heavy Queries)
- ADR-0020 (File Storage Strategy)
- ADR-0021 (Search Strategy)
- ADR-0025 (Build and Deployment Architecture)

---

## Application Layer

- ADR-0007 (FluentValidation)
- ADR-0008 (Mapster)
- ADR-0011 (MediatR)

---

## Artificial Intelligence

- ADR-0017 (Artificial Intelligence Integration Architecture)
- ADR-0022 (AI Knowledge Retrieval Architecture)
- ADR-0023 (Artificial Intelligence Provider Strategy)

---

## Quality & Testing

- ADR-0024 (Enterprise Testing Strategy)

---

## Cross-Cutting / Platform Architecture

- ADR-0012 (Distributed Workspace Architecture)
- ADR-0013 (Client Application Architecture)
- ADR-0014 (Workspace Data Architecture)
- ADR-0015 (Workspace Synchronization Architecture)
- ADR-0016 (Enterprise Messaging Architecture)
- ADR-0018 (External Integration Architecture)

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
| TE-0024 | ADR-0019 |
| TE-0026 | ADR-0020 |
| TE-0027 | ADR-0021 |
| TE-0028 | ADR-0022 |
| TE-0029 | ADR-0023 |
| TE-0030 | ADR-0024 |
| TE-0031 | ADR-0025 |

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

| Version | Date       | Description                         |
|---------|------------|-------------------------------------|
| 3.0.0   | 2026-07-18 | Initial Architecture Decision Index |
| 3.1.0   | 2026-07-27 | Added ADR-0019 (Hybrid Persistence Strategy for Read-Heavy Queries); flagged pre-existing gap for ADR-0013 through ADR-0018 |
| 3.2.0 | 2026-07-27 | Added ADR-0020 (File Storage Strategy) |
| 3.3.0 | 2026-07-27 | Added ADR-0021 (Search Strategy) |
| 3.4.0 | 2026-07-27 | Closed the previously flagged gap: added ADR-0013 through ADR-0018 to the index (all Status: Proposed, no Related TE), and added a new "Cross-Cutting / Platform Architecture" category for ADR-0012 through ADR-0018 |
| 3.5.0 | 2026-07-28 | Added ADR-0022 (AI Knowledge Retrieval Architecture), ADR-0023 (Artificial Intelligence Provider Strategy), ADR-0024 (Enterprise Testing Strategy), and ADR-0025 (Build and Deployment Architecture, created to close the gap for TE-0031); added new "Artificial Intelligence" and "Quality & Testing" categories; moved ADR-0017 from Cross-Cutting into the new Artificial Intelligence category |