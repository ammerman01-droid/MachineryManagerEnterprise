# System Architecture

| Property | Value |
|----------|-------|
| **Document ID** | ARCH-001 |
| **Version** | 4.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# Purpose

This document describes the overall software architecture of the MachineryManagerEnterprise platform.

It provides a high-level architectural view and acts as the central entry point for all architecture-related documentation.

Detailed implementation decisions are documented separately within the Architecture Decision Records (ADR) and Technology Evaluations (TE).

---

# Architectural Vision

The system shall be implemented as a:

- Modular Monolith
- Clean Architecture
- Domain Driven Design (DDD)
- CQRS-based application
- Multi-Tenant enterprise platform
- Distributed Workspace & Offline-First Client Architecture

The architecture is designed to maximize maintainability, extensibility, testability, security, and long-term sustainability.

---

# Architecture Principles

The architecture follows these core principles:

- Separation of Concerns
- Dependency Rule (Inward Direction)
- High Cohesion & Low Coupling
- Explicit Module Boundaries
- Documentation First
- Open Source First (ADR-0002)
- Security by Design (ADR-0026)
- Cloud Neutrality & Distributed Offline Operation (ADR-0012)

---

# High-Level Architecture

```text
Presentation (Blazor Server, .NET MAUI Client, Web API)
        │
        ▼
Application (CQRS Commands/Queries, MediatR, FluentValidation, Mapster)
        │
        ▼
Domain (Entities, Aggregates, Domain Events, Value Objects)
        │
        ▼
Infrastructure (EF Core, Serilog, OpenTelemetry, RabbitMQ, S3, Meilisearch, Qdrant)
```

Dependencies always point inward toward the Domain core.

The Domain Layer contains pure business logic and has zero dependencies on infrastructure frameworks.

---

# Architectural Layers

## Presentation

Responsible for:

- Web User Interface (Blazor Server & MudBlazor)
- Desktop & Mobile Client Apps (.NET MAUI & Blazor Hybrid)
- RESTful HTTP API Endpoints & OpenAPI Specifications
- Authentication, Authorization & Identity Management (OpenID Connect / Keycloak)

---

## Application

Responsible for:

- Use Case Orchestration
- CQRS Pattern Implementation via MediatR
- Pipeline Behaviors (Logging, Validation, Performance Monitoring, Transaction Scope)
- Input Validation via FluentValidation
- DTO & Domain Model Mapping via Mapster

---

## Domain

Responsible for:

- Core Business Rules & Asset Lifecycle Policy
- Domain Aggregates, Entities & Value Objects
- Domain Event Dispatching & Handling
- Asset Management, Maintenance, Component & Meter Domain Logic

The Domain Layer contains no infrastructure concerns.

---

## Infrastructure

Responsible for:

- Relational Database Persistence (Entity Framework Core & Dapper)
- Embedded Local Storage for Offline Workspaces (SQLite & LiteDB)
- Distributed Workspace Package Synchronization Engine
- Structured Logging (Serilog) & OpenTelemetry Observability
- Asynchronous Messaging & Background Processing (RabbitMQ, MassTransit, Quartz.NET)
- File Storage (MinIO / S3 Compatible Object Store)
- Enterprise Full-Text Search (Meilisearch / Elasticsearch)
- Artificial Intelligence Kernel & Vector Engine (Semantic Kernel & Qdrant)

---

# Modular Monolith

Business capabilities are implemented as isolated modules.

Each module owns its:

- Domain Logic
- Application Commands & Queries
- Data Storage Schema
- Public Service Contracts

Modules communicate asynchronously via Domain Events or through explicit interfaces.

---

# Domain Driven Design

The architecture adopts Domain Driven Design (DDD).

Primary concepts include:

- Bounded Contexts
- Aggregates & Entities
- Value Objects
- Domain Events
- Domain Services

Detailed domain models are maintained under `docs/03-domain`.

---

# CQRS & Event Pipeline

Commands modify state and enforce invariants.

Queries execute read-optimized projections without modifying state.

Request dispatching and pipeline cross-cutting concerns are executed via MediatR behaviors.

---

# Multi-Tenancy & Distributed Workspace

The platform supports multi-company and multi-workspace deployment topologies:

- Enterprise Central Cloud Workspace
- Regional / Field Project Workspaces
- Individual Mobile / Offline User Workspaces

Workspace data synchronization preserves tenant boundaries and business integrity via synchronized packages.

---

# Observability

Observability is built directly into all application layers:

- Structured Logging (Serilog)
- Distributed Tracing & Metrics (OpenTelemetry, Prometheus, Jaeger)
- Health Checks & Diagnostic Endpoints

---

# Technology Stack

| Layer | Primary Technology | Related ADR / TE |
|-------|--------------------|------------------|
| Runtime | .NET 10 | ADR-0001 / TE-0001 |
| Web UI | Blazor Server | ADR-0004 / TE-0002 |
| UI Components | MudBlazor | ADR-0008 / TE-0003 |
| Client UI | .NET MAUI & Blazor Hybrid | ADR-0013, ADR-0028 / TE-0010, TE-0034 |
| ORM & Data Access | Entity Framework Core 10 & Dapper | ADR-0006, ADR-0019 / TE-0004, TE-0024 |
| Embedded DB | SQLite & LiteDB | ADR-0014 / TE-0011 |
| Validation | FluentValidation | ADR-0007 / TE-0005, TE-0022 |
| Object Mapping | Mapster | ADR-0010 / TE-0006, TE-0023 |
| CQRS Engine | MediatR | ADR-0003, ADR-0009 / TE-0009 |
| API Generation | REST OpenAPI / NSwag | ADR-0005 / TE-0021 |
| Logging & Telemetry | Serilog & OpenTelemetry | ADR-0011 / TE-0007, TE-0008, TE-0017 |
| Messaging Engine | MassTransit & RabbitMQ | ADR-0016 / TE-0012 |
| AI Architecture | Semantic Kernel & Qdrant | ADR-0017, ADR-0022, ADR-0023 / TE-0013, TE-0028, TE-0029 |
| File Storage | MinIO / S3 Object Store | ADR-0020 / TE-0026 |
| Search Engine | Meilisearch / Elasticsearch | ADR-0021 / TE-0027 |
| Testing Engine | xUnit, Testcontainers, K6 | ADR-0024, ADR-0027 / TE-0030, TE-0033 |
| Security & Identity | OpenID Connect & Keycloak | ADR-0026 / TE-0020, TE-0032 |
| Build & Deploy | Docker, Kubernetes, GitHub Actions | ADR-0015, ADR-0025 / TE-0031 |
| Reporting | QuestPDF & FastReport | ADR-0029 / TE-0035 |

---

# Related Documents

## Vision & Roadmap

- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`

---

## Domain & Bounded Contexts

- `../03-domain/02-BoundedContexts.md`

---

## Architecture Foundation & Models

- `00-TechnologyEvaluationTemplate.md`
- `02-CapabilityModel.md`
- `03-TechnologyGapAnalysis.md`

---

## ADR Master Index

- `../06-decisions/000-ADR-INDEX.md`

---

## Technology Evaluation Index (TE-0001 to TE-0035)

| TE ID | Technology Evaluation Name | File Reference |
|-------|----------------------------|----------------|
| TE-0001 | .NET 10 Platform | `TE-0001-.NET10.md` |
| TE-0002 | Blazor Web UI Framework | `TE-0002-Blazor.md` |
| TE-0003 | MudBlazor UI Component Library | `TE-0003-MudBlazor.md` |
| TE-0004 | Entity Framework Core 10 Data Access | `TE-0004-EntityFrameworkCore.md` |
| TE-0005 | FluentValidation Architecture | `TE-0005-FluentValidation.md` |
| TE-0006 | Mapster Object Mapping | `TE-0006-Mapster.md` |
| TE-0007 | Serilog Logging Engine | `TE-0007-Serilog.md` |
| TE-0008 | OpenTelemetry Observability | `TE-0008-OpenTelemetry.md` |
| TE-0009 | MediatR CQRS Pipeline Engine | `TE-0009-Use-MediatR.md` |
| TE-0010 | Desktop & Mobile Client Framework | `TE-0010-Desktop-Mobile-Framework-Evaluation.md` |
| TE-0011 | Embedded Workspace Database | `TE-0011-Embedded-Workspace-Database-Evaluation.md` |
| TE-0012 | Enterprise Messaging Technology | `TE-0012-Enterprise Messaging Technology Evaluation.md` |
| TE-0013 | Artificial Intelligence Integration | `TE-0013-Artificial-Intelligence-Technology-Evaluation.md` |
| TE-0014 | Background Processing Engine | `TE-0014-Background Processing Technology Evaluation.md` |
| TE-0015 | Caching Architecture (.NET 10) | `TE-0015-Caching Architecture Technology Evaluation (.NET 10).md` |
| TE-0016 | Enterprise Search Architecture | `TE-0016-Enterprise-Search-Architecture-Evaluation.md` |
| TE-0017 | Observability and Telemetry Strategy | `TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` |
| TE-0018 | Configuration and Secrets Management | `TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` |
| TE-0019 | Background Processing & Job Scheduling | `TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md` |
| TE-0020 | Authentication and Identity Strategy | `TE-0020-Authentication-and-Identity-Technology-Evaluation.md` |
| TE-0021 | API Documentation & Client Generation | `TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` |
| TE-0022 | Validation Pipeline Architecture | `TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` |
| TE-0023 | Object Mapping Strategy & Technology | `TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` |
| TE-0024 | Data Access Architecture Evaluation | `TE-0024-Data-Access-Architecture-Evaluation.md` |
| TE-0025 | Database Migration Strategy | `TE-0025-Database-Migration-Technology-Evaluation.md` |
| TE-0026 | File Storage Architecture | `TE-0026-File-Storage-Technology-Evaluation.md` |
| TE-0027 | Enterprise Search Engine Strategy | `TE-0027-Search-Engine-Technology-Evaluation.md` |
| TE-0028 | Vector Database Engine Evaluation | `TE-0028-Vector-Database-Technology-Evaluation.md` |
| TE-0029 | AI Provider Strategy & Evaluation | `TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` |
| TE-0030 | Enterprise Testing Strategy | `TE-0030-Testing-Technology-Evaluation.md` |
| TE-0031 | Build Packaging & Deployment Strategy | `TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` |
| TE-0032 | Enterprise Security Strategy | `TE-0032-Security-Technology-Evaluation.md` |
| TE-0033 | Performance & Load Testing | `TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` |
| TE-0034 | Client UI Strategy & Frameworks | `TE-0034-Client-UI-Technology-Evaluation.md` |
| TE-0035 | Enterprise Reporting Architecture | `TE-0035-Reporting-Technology-Evaluation.md` |

---

# Architecture Governance

Architectural decisions are governed by:

- Architecture Decision Records Index (`../06-decisions/000-ADR-INDEX.md`)
- Technical Evaluations Registry (`TE-0001` through `TE-0035`)
- Documentation Standard v4.0.0

No architectural change shall bypass the ADR process.

---

# Revision History

| Version | Date       | Author             | Description |
|---------|------------|--------------------|-------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial architecture notes |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0; fully linked all 35 TE files and ADR Master Index |
