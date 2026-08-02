| Property | Value |
|----------|-------|
| **Document ID** | ADR-INDEX |
| **Title** | Architecture Decision Record Index |
| **Version** | 4.1.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-08-02 |

---

# Architecture Decision Records (ADR) Master Index

This document serves as the official master index for all Architecture Decision Records (ADR) governing the MachineryManagerEnterprise platform.

---

# Purpose

The Purpose of the ADR Master Index is to provide a single, centralized, authoritative catalog of all enterprise architecture decisions. It ensures full traceability between business requirements, capability models, technology evaluations (TE), and architectural implementation rules.

---

# Evaluation Scope

The Evaluation Scope of this index encompasses all architectural decisions across:

- Core Runtime & Enterprise Platform (.NET 10, Clean Architecture, Modular Monolith)
- User Interface & Presentation Layer (Blazor Server, MudBlazor, .NET MAUI)
- Persistence & Data Access Layer (Entity Framework Core, Dapper, SQLite, LiteDB, PostgreSQL)
- Validation & Business Logic Pipeline (FluentValidation, MediatR, Mapster)
- Telemetry & Observability (Serilog, OpenTelemetry, Prometheus, Jaeger)
- Messaging & Integration Infrastructure (RabbitMQ, MassTransit, SignalR)
- Artificial Intelligence & Knowledge Engine (Semantic Kernel, Ollama, Vector Search)
- Distributed Workspace Synchronization & Offline-First Persistence
- Security, Authentication & Secret Management (OpenID Connect, Keycloak, HashiCorp Vault)
- Build, Packaging, Deployment & Performance Testing (Docker, Kubernetes, K6)

---

# Relationship

This index maintains bidirectional relationships with:

- **Vision & Roadmap**: `../01-vision/00-Vision.md`, `../01-vision/01-DocumentationRoadmap.md`
- **System Architecture**: `../02-architecture/01-Architecture.md`
- **Capability Model**: `../02-architecture/02-CapabilityModel.md`
- **Technology Gap Analysis**: `../02-architecture/03-TechnologyGapAnalysis.md`
- **Technical Evaluations**: `../02-architecture/TE-0001-.NET10.md` through `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md`

---

# Architectural References

- **Clean Architecture Standard v4.0.0**
- **Domain Driven Design (DDD) Enterprise Guide**
- **Distributed Workspace Architecture Specification**
- **Documentation Standard v4.0.0**

---

# Scope

The scope applies to all modules, building blocks, shared libraries, desktop/mobile clients, and cloud hosting topologies within MachineryManagerEnterprise.

---

# Functional Requirements

The ADR governance framework shall support:

- zero ambiguity in technology selection and architectural pattern enforcement;
- clear decision status tracking (Approved, Proposed, Deprecated, Superseded);
- complete mapping between ADRs and Technical Evaluation (TE) documents;
- Clean Architecture boundary preservation across all modules.

---

# Non-Functional Requirements

- **Traceability**: 100% of architectural decisions must map to a corresponding TE document or foundational architecture rule.
- **Maintainability**: Clear versioning and revision history across all ADR entries.
- **Compliance**: Strict adherence to Documentation Standard v4.0.0.

---

# Candidate Technologies

All candidate technologies evaluated in support of these ADRs are fully detailed in the corresponding Technical Evaluation (TE) documents (`TE-0001` to `TE-0035`).

---

# Evaluation Criteria

ADR validity and approval are evaluated based on:

- Clean Architecture compliance
- .NET 10 ecosystem alignment
- Open Source First policy (ADR-0002)
- Cloud neutrality & zero vendor lock-in
- Operational simplicity and testability

---

# Architecture Principle

All Architecture Decisions must enforce strict separation of concerns, keeping Domain models clean and free from external infrastructure dependencies.

---

# Overall Technology Comparison

| ADR ID | Decision Domain | Approved Standard | Alternative Evaluated |
|--------|-----------------|-------------------|-----------------------|
| ADR-0001 | Architecture | Clean Architecture & Modular Monolith | Microservices, Layered Monolith |
| ADR-0002 | Governance | Open Source First Policy | Commercial Proprietary |
| ADR-0003 | Pattern | CQRS & MediatR Pipeline | Monolithic Services |
| ADR-0004 | Web UI Framework | Blazor Server (.NET 10) | React, Angular, Vue |
| ADR-0005 | API Architecture | RESTful API & OpenAPI Client Generation | GraphQL, gRPC |
| ADR-0006 | Data Access Strategy | Entity Framework Core 10 | NHibernate, Raw ADO.NET |
| ADR-0007 | Validation Architecture | FluentValidation | Data Annotations |
| ADR-0008 | UI Component Library | MudBlazor | Radzen, Syncfusion |
| ADR-0009 | CQRS Pipeline Engine | MediatR | Custom Mediator |
| ADR-0010 | Object Mapping | Mapster | AutoMapper |
| ADR-0011 | Observability Engine | Serilog & OpenTelemetry | NLog, Log4Net |
| ADR-0012 | Sync Architecture | Distributed Workspace Offline Sync | Direct DB Sync |
| ADR-0013 | Client Framework | .NET MAUI (Desktop & Mobile) | Electron, Flutter |
| ADR-0014 | Embedded DB Strategy | SQLite & LiteDB | RavenDB Embedded |
| ADR-0015 | Deployment Architecture | Docker & Kubernetes | Bare Metal |
| ADR-0016 | Messaging Architecture | MassTransit & RabbitMQ | Apache Kafka |
| ADR-0017 | AI Architecture | Semantic Kernel | Direct API Integration |
| ADR-0018 | External Integration | Modular Connector Engine | Custom Scripts |
| ADR-0019 | Read Persistence | Hybrid Persistence Strategy | Single DB Engine |
| ADR-0020 | File Storage Strategy | S3 Compatible Object Store (MinIO) | Local File System |
| ADR-0021 | Search Strategy | Meilisearch / Elasticsearch Engine | SQL LIKE Queries |
| ADR-0022 | Vector DB Architecture | Qdrant Vector Engine | In-Memory Search |
| ADR-0023 | AI Provider Strategy | Multi-Provider Router Engine | Single LLM Vendor |
| ADR-0024 | Enterprise Testing | xUnit, Moq, Testcontainers | Manual Testing |
| ADR-0025 | Build Pipeline | GitHub Actions & Nuke Build | Jenkins |
| ADR-0026 | Security Architecture | Data Protection, AES-256 & X.509 (Transport/Encryption) | Custom Crypto Implementation |
| ADR-0027 | Performance Testing | K6 & NBomber Engine | JMeter |
| ADR-0028 | Client UI Architecture | Blazor Hybrid & MAUI Controls | Web View Wrapper |
| ADR-0029 | Reporting Architecture | QuestPDF & FastReport OpenSource | SSRS |
| ADR-0030 | Identity & Access Management | ASP.NET Core Identity & OpenIddict | Duende IdentityServer, Keycloak |

---

# Final Recommendation

Maintain and enforce all approved ADRs across all development teams and automated CI/CD validation pipelines.

---

# Final Decision

All 30 Architecture Decision Records listed in the Master Directory below are formally **Approved** and active.

---

# Decision Summary

- ✔ 30/30 ADRs Approved
- ✔ 35/35 TEs Completed & Linked
- ✔ Clean Architecture Enforced
- ✔ Documentation Standard v4.0.0 Compliant

---

# Master ADR Directory

| ADR ID | Decision Title | Status | Primary Technology | Related Technical Evaluation |
|--------|----------------|--------|--------------------|------------------------------|
| ADR-0001 | Adopt Clean Architecture & Modular Monolith | Approved | .NET 10 | `../02-architecture/TE-0001-.NET10.md` |
| ADR-0002 | Open Source First Policy | Approved | Permissive OSS | `../02-architecture/TE-0001-.NET10.md` |
| ADR-0003 | CQRS & Event-Driven Architecture | Approved | MediatR | `../02-architecture/TE-0009-Use-MediatR.md` |
| ADR-0004 | Web UI Framework Standard | Approved | Blazor Server | `../02-architecture/TE-0002-Blazor.md` |
| ADR-0005 | API Architecture & Client Generation | Approved | OpenAPI / NSwag | `../02-architecture/TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` |
| ADR-0006 | Data Access Strategy | Approved | Entity Framework Core 10 | `../02-architecture/TE-0004-EntityFrameworkCore.md`, `../02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md` |
| ADR-0007 | Validation Pipeline Strategy | Approved | FluentValidation | `../02-architecture/TE-0005-FluentValidation.md`, `../02-architecture/TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` |
| ADR-0008 | UI Component Library Strategy | Approved | MudBlazor | `../02-architecture/TE-0003-MudBlazor.md`, `../02-architecture/TE-0034-Client-UI-Technology-Evaluation.md` |
| ADR-0009 | CQRS Dispatcher & Pipeline Architecture | Approved | MediatR | `../02-architecture/TE-0009-Use-MediatR.md` |
| ADR-0010 | Object Mapping Strategy | Approved | Mapster | `../02-architecture/TE-0006-Mapster.md`, `../02-architecture/TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` |
| ADR-0011 | Observability & Telemetry Architecture | Approved | Serilog & OpenTelemetry | `../02-architecture/TE-0007-Serilog.md`, `../02-architecture/TE-0008-OpenTelemetry.md`, `../02-architecture/TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` |
| ADR-0012 | Distributed Workspace Architecture | Approved | Sync Package Engine | `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md`, `../02-architecture/TE-0026-File-Storage-Technology-Evaluation.md` |
| ADR-0013 | Client Application Architecture | Approved | .NET MAUI | `../02-architecture/TE-0010-Desktop-Mobile-Framework-Evaluation.md` |
| ADR-0014 | Embedded Workspace Database | Approved | SQLite / LiteDB | `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md`, `../02-architecture/TE-0025-Database-Migration-Technology-Evaluation.md` |
| ADR-0015 | Deployment Architecture | Approved | Docker / Kubernetes | `../02-architecture/TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` |
| ADR-0016 | Enterprise Messaging Architecture | Approved | MassTransit & RabbitMQ | `../02-architecture/TE-0012-Enterprise-Messaging-Technology-Evaluation.md` |
| ADR-0017 | Artificial Intelligence Architecture | Approved | Semantic Kernel Engine | `../02-architecture/TE-0013-Artificial-Intelligence-Technology-Evaluation.md` |
| ADR-0018 | External Integration Architecture | Approved | Connector Engine | `../02-architecture/TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` |
| ADR-0019 | Hybrid Persistence Strategy | Approved | Dapper / Read Replicas | `../02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md` |
| ADR-0020 | File Storage Strategy | Approved | S3 / MinIO | `../02-architecture/TE-0026-File-Storage-Technology-Evaluation.md` |
| ADR-0021 | Search Engine Strategy | Approved | Meilisearch / Elasticsearch | `../02-architecture/TE-0027-Search-Engine-Technology-Evaluation.md` |
| ADR-0022 | Vector Database Architecture | Approved | Qdrant Vector Engine | `../02-architecture/TE-0028-Vector-Database-Technology-Evaluation.md` |
| ADR-0023 | AI Provider Strategy | Approved | Multi-Provider Model Router | `../02-architecture/TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` |
| ADR-0024 | Enterprise Testing Strategy | Approved | xUnit & Testcontainers | `../02-architecture/TE-0030-Testing-Technology-Evaluation.md` |
| ADR-0025 | Build & Deployment Architecture | Approved | GitHub Actions / Nuke | `../02-architecture/TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` |
| ADR-0026 | Enterprise Security Strategy | Approved | Data Protection, AES-256 & X.509 | `../02-architecture/TE-0032-Security-Technology-Evaluation.md` |
| ADR-0027 | Enterprise Performance Testing Strategy | Approved | K6 & NBomber | `../02-architecture/TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` |
| ADR-0028 | Client UI Architecture | Approved | Blazor Hybrid & MAUI | `../02-architecture/TE-0034-Client-UI-Technology-Evaluation.md` |
| ADR-0029 | Enterprise Reporting Architecture | Approved | QuestPDF & FastReport | `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md` |
| ADR-0030 | Identity and Access Management Architecture | Approved | ASP.NET Core Identity & OpenIddict | `../02-architecture/TE-0020-Authentication-and-Identity-Technology-Evaluation.md` |

---

# Related ADR

- ADR-0001 — Adopt Clean Architecture & Modular Monolith
- ADR-0002 — Open Source First Policy
- ADR-0012 — Distributed Workspace Architecture

---

# Related Documents

- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`
- `../02-architecture/00-TechnologyEvaluationTemplate.md`
- `../02-architecture/01-Architecture.md`
- `../02-architecture/02-CapabilityModel.md`
- `../02-architecture/03-TechnologyGapAnalysis.md`
- `../02-architecture/TE-0001-.NET10.md`
- `../02-architecture/TE-0002-Blazor.md`
- `../02-architecture/TE-0003-MudBlazor.md`
- `../02-architecture/TE-0004-EntityFrameworkCore.md`
- `../02-architecture/TE-0005-FluentValidation.md`
- `../02-architecture/TE-0006-Mapster.md`
- `../02-architecture/TE-0007-Serilog.md`
- `../02-architecture/TE-0008-OpenTelemetry.md`
- `../02-architecture/TE-0009-Use-MediatR.md`
- `../02-architecture/TE-0010-Desktop-Mobile-Framework-Evaluation.md`
- `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md`
- `../02-architecture/TE-0012-Enterprise-Messaging-Technology-Evaluation.md`
- `../02-architecture/TE-0013-Artificial-Intelligence-Technology-Evaluation.md`
- `../02-architecture/TE-0014-Background-Processing-Technology-Evaluation.md`
- `../02-architecture/TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md`
- `../02-architecture/TE-0016-Enterprise-Search-Architecture-Evaluation.md`
- `../02-architecture/TE-0017-Observability-and-Telemetry-Technology-Evaluation.md`
- `../02-architecture/TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md`
- `../02-architecture/TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md`
- `../02-architecture/TE-0020-Authentication-and-Identity-Technology-Evaluation.md`
- `../02-architecture/TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md`
- `../02-architecture/TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md`
- `../02-architecture/TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md`
- `../02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md`
- `../02-architecture/TE-0025-Database-Migration-Technology-Evaluation.md`
- `../02-architecture/TE-0026-File-Storage-Technology-Evaluation.md`
- `../02-architecture/TE-0027-Search-Engine-Technology-Evaluation.md`
- `../02-architecture/TE-0028-Vector-Database-Technology-Evaluation.md`
- `../02-architecture/TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md`
- `../02-architecture/TE-0030-Testing-Technology-Evaluation.md`
- `../02-architecture/TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md`
- `../02-architecture/TE-0032-Security-Technology-Evaluation.md`
- `../02-architecture/TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md`
- `../02-architecture/TE-0034-Client-UI-Technology-Evaluation.md`
- `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md`

---

# References

- Clean Architecture Design Principles (Robert C. Martin)
- Enterprise Integration Patterns (Gregor Hohpe)
- .NET 10 Architecture Standards

---

# Revision History

| Version | Date       | Author             | Description                                                                    |
|---------|------------|--------------------|--------------------------------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial ADR index                                                              |
| 3.0.0   | 2026-07-18 | Solution Architect | Initial Architecture Decision Index                                            |
| 3.1.0   | 2026-07-27 | Solution Architect | Added ADR-0019 (Hybrid Persistence Strategy for Read-Heavy Queries); flagged pre-existing gap for ADR-0013 through ADR-0018 |
| 3.2.0   | 2026-07-27 | Solution Architect | Added ADR-0020 (File Storage Strategy)                                         |
| 3.3.0   | 2026-07-27 | Solution Architect | Added ADR-0021 (Search Strategy)                                               |
| 3.4.0   | 2026-07-27 | Solution Architect | Closed the previously flagged gap: added ADR-0013 through ADR-0018 to the index (all Status: Proposed, no Related TE), and added a new "Cross-Cutting / Platform Architecture" category for ADR-0012 through ADR-0018 |
| 3.5.0   | 2026-07-28 | Solution Architect | Added ADR-0022 (AI Knowledge Retrieval Architecture), ADR-0023 (Artificial Intelligence Provider Strategy), ADR-0024 (Enterprise Testing Strategy), and ADR-0025 (Build and Deployment Architecture, created to close the gap for TE-0031); added new "Artificial Intelligence" and "Quality & Testing" categories; moved ADR-0017 from Cross-Cutting into the new Artificial Intelligence category |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0; consolidated all 29 ADRs and 35 TEs |
| 4.1.0   | 2026-08-02 | Solution Architect | Added ADR-0030 (Identity and Access Management Architecture), ratifying TE-0020's Final Recommendation and classifying Identity as a platform module; corrected ADR-0026's rows in both tables, which had incorrectly listed "OpenID Connect & Keycloak" and TE-0020 — ADR-0026's actual Decision section covers Data Protection/AES-256/X.509 only and explicitly defers authentication to a separate document; that gap is now closed by ADR-0030 |