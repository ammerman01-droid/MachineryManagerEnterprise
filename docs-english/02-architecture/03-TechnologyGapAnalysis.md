| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ARCH-003           |
| **Title**        | Technology Gap Analysis |
| **Version**      | 4.2.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-26         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

This document identifies architectural capabilities that require technical evaluations and formal decisions across the MachineryManagerEnterprise platform.

Its primary objective is to ensure that every architectural capability introduced during the evolution of the platform is systematically evaluated against the existing technology landscape before implementation.

The Technology Gap Analysis serves as the bridge between System Architecture (`01-Architecture.md`), Business Capabilities (`02-CapabilityModel.md`), Architecture Decision Records (`../06-decisions/000-ADR-INDEX.md`), Technical Evaluations (`TE-0001` to `TE-0035`), and implementation planning.

This document ensures that:

- every architectural capability is supported by appropriate technologies;
- technology adoption remains intentional and fully traceable;
- new technologies are introduced only through formal architectural decisions;
- the approved technology stack evolves in a controlled and documented manner;
- architectural consistency is preserved throughout the lifetime of the platform.

This document identifies technology gaps.

It does not select technologies.

Technology selection shall always be performed through approved ADR and Technical Evaluation documents.

---

# Scope

This document covers:

- Server-side & core platform runtime technologies (.NET 10, C#)
- Client, Desktop & Mobile presentation technologies (.NET MAUI, Blazor Server, MudBlazor)
- Database & Persistence technologies (Entity Framework Core, SQLite, LiteDB, Dapper, PostgreSQL)
- Object Mapping, Validation & CQRS Pipeline technologies (Mapster, FluentValidation, MediatR)
- Enterprise Messaging & Event Bus infrastructure (MassTransit, RabbitMQ)
- Artificial Intelligence, Vector Engine & Provider Router technologies (Semantic Kernel, Qdrant, Ollama)
- Enterprise Full-Text Search technologies (SQL Server FTS, OpenSearch)
- File Storage technologies (MinIO / S3 Object Store)
- Security, Authentication & Identity Management (ASP.NET Core Identity, OpenIddict)
- Testing, Quality Assurance & Performance Testing (xUnit, Testcontainers, K6, NBomber)
- Build, Packaging, Deployment & CI/CD Pipelines (GitHub Actions, Docker, .NET Aspire)
- Reporting & BI Output engines (QuestPDF, FastReport)

Business rules are intentionally outside the scope of this document.

---

# Assessment Methodology

Every Architectural Capability is evaluated using the following decision sequence.

```text
New Business Capability
        │
        ▼
Capability Analysis
        │
        ▼
Covered by Approved Technology?
   │                   │
  Yes                  No
   │                   │
   ▼                   ▼
No Action     Technology Gap Identified
Required               │
                       ▼
              ADR & TE Required
                       │
                       ▼
              Technology Selection
                       │
                       ▼
              Architecture Approved
```

Each capability is classified into one of the following categories.

| Status | Meaning |
|---------|---------|
| Covered | Fully supported by existing approved technologies |
| Partial | Existing technologies support the capability, but additional architectural decisions are required |
| Missing | No approved technology currently supports the capability |

---

# Technology Coverage & Gap Analysis Matrix

The following matrix evaluates every major architectural capability against the approved technology stack, corresponding Technical Evaluations (TE), and Architecture Decision Records (ADR).

| Gap ID | Architectural Capability | Technology Standard Selected | ADR Reference | TE Reference | Priority | Status |
|--------|---------------------------|------------------------------|---------------|--------------|----------|--------|
| GAP-001 | Core Platform Runtime | .NET 10 | ADR-0001, ADR-0002, ADR-0003 | `TE-0001-.NET10.md` | High | Approved ✅ |
| GAP-002 | Web UI Framework | Blazor Server | ADR-0004 | `TE-0002-Blazor.md` | High | Approved ✅ |
| GAP-003 | UI Component Library | MudBlazor | ADR-0005 | `TE-0003-MudBlazor.md` | High | Approved ✅ |
| GAP-004 | ORM Data Access | Entity Framework Core 10 | ADR-0006 | `TE-0004-EntityFrameworkCore.md` | High | Approved ✅ |
| GAP-005 | Fluent Validation | FluentValidation | ADR-0007 | `TE-0005-FluentValidation.md` | High | Approved ✅ |
| GAP-006 | DTO Object Mapping | Mapster | ADR-0008 | `TE-0006-Mapster.md` | High | Approved ✅ |
| GAP-007 | Structured Logging | Serilog | ADR-0009 | `TE-0007-Serilog.md` | High | Approved ✅ |
| GAP-008 | Observability & Tracing | OpenTelemetry | ADR-0010 | `TE-0008-OpenTelemetry.md` | High | Approved ✅ |
| GAP-009 | CQRS Dispatcher | MediatR | ADR-0011 | `TE-0009-Use-MediatR.md` | High | Approved ✅ |
| GAP-010 | Installable Clients (Desktop & Mobile) | .NET MAUI | ADR-0013 | `TE-0010-Desktop-Mobile-Framework-Evaluation.md` | High | Approved ✅ |
| GAP-011 | Offline Workspace Database | SQLite & LiteDB | ADR-0014 | `TE-0011-Embedded-Workspace-Database-Evaluation.md` | High | Approved ✅ |
| GAP-012 | Enterprise Messaging | MassTransit & RabbitMQ | ADR-0016 | `TE-0012-Enterprise-Messaging-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-013 | AI Integration Engine | Semantic Kernel | ADR-0017 | `TE-0013-Artificial-Intelligence-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-014 | Background Processing | Quartz.NET / Channels | ADR-0032 | `TE-0014-Background-Processing-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-015 | Caching Architecture | Hybrid Memory & Distributed Cache | ADR-0031 | `TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md` | Medium | Approved ✅ |
| GAP-016 | Enterprise Search Architecture | SQL Server FTS + OpenSearch (see GAP-027) | ADR-0021 | `TE-0016-Enterprise-Search-Architecture-Evaluation.md` | Medium | Superseded ⚠️ (TE-0016 superseded by TE-0027; see GAP-027) |
| GAP-017 | Enterprise Observability Pipeline | Prometheus, Grafana, OpenTelemetry, Serilog | ADR-0033 | `TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-018 | Secrets & Config Management | Environment & HashiCorp Vault | ADR-0034 | `TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-019 | Job Scheduling Strategy | Quartz.NET Engine | ADR-0032 | `TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-020 | Identity & Security | ASP.NET Core Identity & OpenIddict | ADR-0030 | `TE-0020-Authentication-and-Identity-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-021 | API Client Generation | OpenAPI, Scalar & NSwag | ADR-0035 | `TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-022 | Validation Pipeline | MediatR Validation Behavior | ADR-0007, ADR-0036 | `TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` | High | Approved ✅ |
| GAP-023 | High-Performance Mapping | Mapster Compiler Projections | ADR-0008 | `TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-024 | Read-Heavy Query Persistence | Dapper & Read Replicas | ADR-0019 | `TE-0024-Data-Access-Architecture-Evaluation.md` | High | Approved ✅ |
| GAP-025 | Database Migrations | EF Core Migrations & Respawn | ADR-0037 | `TE-0025-Database-Migration-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-026 | Object File Storage | S3 / MinIO | ADR-0020 | `TE-0026-File-Storage-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-027 | Search Engine Integration | SQL Server FTS (default) + OpenSearch (escalation) | ADR-0021 | `TE-0027-Search-Engine-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-028 | Vector Search & RAG | Qdrant Vector Engine | ADR-0022 | `TE-0028-Vector-Database-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-029 | AI Provider Router | Multi-Provider Engine (Ollama/OpenAI) | ADR-0023 | `TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-030 | Enterprise Test Automation | xUnit, Moq, Testcontainers | ADR-0024 | `TE-0030-Testing-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-031 | Packaging & Deployment | Docker & GitHub Actions (Kubernetes: Unresolved) | ADR-0025 | `TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-032 | Enterprise Security Hardening | TLS, Secret Vault, RBAC | ADR-0026 | `TE-0032-Security-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-033 | Performance & Load Testing | K6 & NBomber | ADR-0027 | `TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` | Medium | Approved ✅ |
| GAP-034 | Client UI Hybrid Framework | ~~Avalonia UI~~ Superseded — see GAP-010 (.NET MAUI) | ADR-0028 | `TE-0034-Client-UI-Technology-Evaluation.md` | High | Superseded ⚠️ |
| GAP-035 | Enterprise Reporting Engine | QuestPDF (FastReport & RDLC excluded) | ADR-0029 | `TE-0035-Reporting-Technology-Evaluation.md` | High | Approved ✅ |
| GAP-036 | External Integration & Connectors | MassTransit-based Connector Framework (+ Azure Logic Apps opt-in) | ADR-0018 | `TE-0036-External-Integration-and-Connector-Technology-Evaluation.md` | High | Approved ✅ |

---

# Detailed Gap Evaluation Summaries

### GAP-010 — Distributed Desktop & Mobile Framework
- **Capability**: Cross-Platform Installable Workspace Application (Desktop Windows/macOS, Mobile Android/iOS)
- **Status**: Approved ✅
- **Architecture Decision**: ADR-0013 — Client Application Architecture
- **Technology Evaluation**: `TE-0010-Desktop-Mobile-Framework-Evaluation.md`
- **Selected Technology**: .NET MAUI & Blazor Hybrid

### GAP-011 — Embedded Local Database
- **Capability**: Offline Workspace Persistence & Local Package Synchronization
- **Status**: Approved ✅
- **Architecture Decision**: ADR-0014 — Embedded Workspace Database
- **Technology Evaluation**: `TE-0011-Embedded-Workspace-Database-Evaluation.md`
- **Selected Technology**: SQLite (Structured relational) & LiteDB (Document store)

### GAP-012 — Enterprise Messaging
- **Capability**: Asynchronous Inter-Module Event Bus & Message Distribution
- **Status**: Approved ✅
- **Architecture Decision**: ADR-0016 — Enterprise Messaging Architecture
- **Technology Evaluation**: `TE-0012-Enterprise-Messaging-Technology-Evaluation.md`
- **Selected Technology**: MassTransit over RabbitMQ

### GAP-013 — Artificial Intelligence Integration
- **Capability**: AI Assistant, Diagnostic Kernel & Knowledge Retrieval
- **Status**: Approved ✅
- **Architecture Decision**: ADR-0017 — Artificial Intelligence Integration
- **Technology Evaluation**: `TE-0013-Artificial-Intelligence-Technology-Evaluation.md`
- **Selected Technology**: Semantic Kernel Engine & Multi-Provider Model Router

---

# Architecture First Implementation Roadmap

The project follows a strict Architecture First methodology.

```text
Business Requirement -> Capability Model -> Technology Gap Analysis -> ADR -> TE -> Implementation
```

All 35 technology gaps identified across the platform have completed their corresponding Technical Evaluations (`TE-0001` to `TE-0035`) and Architecture Decision Records (`ADR-0001` to `ADR-0029`).

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# Related Documents

- `01-Architecture.md`
- `02-CapabilityModel.md`
- `00-TechnologyEvaluationTemplate.md`
- `../06-decisions/000-ADR-INDEX.md`
- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`
- `TE-0001-.NET10.md` through `TE-0035-Reporting-Technology-Evaluation.md`

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-26 | Solution Architect | Initial Technology Gap Analysis                       |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0; expanded gap matrix to cover all 35 TEs and 29 ADRs |
| 4.1.0   | 2026-08-02 | Solution Architect | Corrected GAP-020: replaced incorrect "Keycloak / OpenID Connect" / ADR-0026 reference with the actual TE-0020 recommendation (ASP.NET Core Identity & OpenIddict) and its ratifying ADR-0030 |
| 4.2.0   | 2026-08-08 | Solution Architect | Note: the 4.1.0 entry above described a GAP-020 fix that had not actually been applied to the table row; corrected for real in this revision. Also fixed 9 further incorrect ADR references (GAP-014, GAP-017, GAP-018, GAP-019, GAP-021, GAP-022, GAP-025 pointed to the wrong ADR number), corrected GAP-016/GAP-027/GAP-035 technology content (Meilisearch/Elasticsearch was never adopted; actual decision is SQL Server FTS + OpenSearch escalation; FastReport is explicitly excluded from Reporting), marked GAP-016 and GAP-034 Superseded (both conflicted with already-approved decisions), and added GAP-036 for External Integration (ADR-0018/TE-0036), which had no row at all |