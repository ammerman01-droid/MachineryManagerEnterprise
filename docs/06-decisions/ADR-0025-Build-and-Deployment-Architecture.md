# Build and Deployment Architecture

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0025 |
| **Version** | 1.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-28 |
| **Last Updated** | 2026-07-28 |

---

# Context

MachineryManagerEnterprise requires a unified, enterprise-grade build,
packaging, and deployment pipeline capable of supporting local development,
automated validation, containerized deployment, and both cloud and
on-premise/hybrid hosting, consistent with the deployment-flexibility
posture already established for data access, file storage, and search
(ADR-0019, ADR-0020, ADR-0021).

`TE-0031 — Build, Packaging and Deployment Technology Evaluation` evaluated
the .NET 10 SDK, Docker, .NET Aspire, GitHub Actions, and Azure DevOps
against this requirement and recommended a combined stack, deferring
formal recording of that decision to this ADR.

---

# Problem

Without a single, formally approved build and deployment stack, individual
modules or environments could adopt inconsistent build tooling, CI/CD
platforms, or containerization approaches, undermining reproducibility and
increasing long-term operational and onboarding cost.

---

# Decision Drivers

The build and deployment architecture shall satisfy:

- Enterprise build automation
- Continuous Integration / Continuous Delivery
- Containerization
- Reproducible, deterministic builds
- Cross-platform support (Windows, Linux, macOS)
- Hybrid deployment readiness (on-premise and cloud)
- Long-term maintainability

---

# Decision

MachineryManagerEnterprise adopts the following build, packaging, and
deployment stack:

| Responsibility | Approved Technology |
|-----------------|------------------------|
| Build Platform | **.NET 10 SDK** |
| Containerization | **Docker** |
| Local Distributed Orchestration | **.NET Aspire** |
| Continuous Integration | **GitHub Actions** |
| Enterprise ALM Alternative | **Azure DevOps** (supported, optional) |

The **.NET 10 SDK** is the single authoritative build toolchain for every
project in the solution (restore, build, test, publish, pack). **Docker**
is the standard containerization technology for packaging and deployment
consistency across environments. **.NET Aspire** is adopted for local
distributed-application orchestration during development. **GitHub
Actions** is the primary Continuous Integration platform, consistent with
the project's use of GitHub as its source control platform. **Azure
DevOps** is retained as a supported, optional enterprise ALM alternative
for customers or teams already standardized on the Azure DevOps ecosystem,
but it is not the default.

---

# Build Pipeline

```text
Developer
      │
      ▼
   Build (.NET 10 SDK)
      │
      ▼
   Test
      │
      ▼
   Package (Docker)
      │
      ▼
   Deploy
```

---

# Implementation Strategy

**Phase 1**
- .NET 10 SDK
- Docker
- GitHub Actions

**Phase 2**
- .NET Aspire

**Phase 3**
- Optional Azure DevOps support for enterprise customers who require it

---

# Approved Technologies

| Technology | Decision | Status |
|------------|----------|--------|
| .NET 10 SDK | Approved | ✅ |
| Docker | Approved | ✅ |
| .NET Aspire | Approved | ✅ |
| GitHub Actions | Approved | ✅ |
| Azure DevOps | Supported Alternative | ✅ |

---

# Consequences

## Positive

- Reproducible, deterministic builds across every environment
- Consistent, container-based deployment
- Excellent CI automation with minimal additional tooling
- Hybrid deployment readiness (on-premise and cloud)
- Strong Microsoft ecosystem alignment, consistent with the platform's
  .NET 10 / Blazor stack

## Negative

- Introduces a Docker runtime requirement across build and deployment
  environments
- .NET Aspire adds an additional orchestration layer for local development
- Optional operational complexity if Azure DevOps is adopted by a specific
  customer or team

---

# Alternatives Considered

## Azure DevOps as the Primary CI/CD Platform

Rejected as the default because the project uses GitHub as its primary
source control platform; GitHub Actions provides excellent CI/CD
capabilities with significantly lower administrative complexity. Azure
DevOps remains a fully supported enterprise alternative for organizations
already standardized on it.

---

# Related Technology Evaluation

TE-0031 — Build, Packaging and Deployment Technology Evaluation

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0003 — Use .NET 10
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- ADR-0020 — File Storage Strategy
- ADR-0021 — Search Strategy
- ADR-0024 — Enterprise Testing Strategy
- TE-0031 — Build, Packaging and Deployment Technology Evaluation

---

# Decision Outcome

Implementation of TE-0031 requires this ADR. The approved stack
(.NET 10 SDK, Docker, .NET Aspire, GitHub Actions, with Azure DevOps as a
supported alternative) is binding for all modules and environments; no
module may adopt an alternative build or CI/CD toolchain without a new or
amended ADR.

---

# Revision History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-28 | Initial decision, formalizing the Build and Deployment Architecture recommended by TE-0031 |
