| Property | Value |
|----------|-------|
| **Document ID** | TE-0021 |
| **Title** | API Documentation and Client Generation Technology Evaluation (.NET 10) |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-26 |
| **Last Updated** | 2026-07-28 |

---

# Purpose

This document evaluates candidate technologies for API Documentation and Client Generation Technology Evaluation (.NET 10) in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0005 — API Architecture
- ADR-0015 — Deployment Architecture
- SolutionStructure.md
- DependencyRules.md

---

# Scope

The evaluation covers two independent concerns:

## API Documentation

Technologies responsible for exposing machine-readable API contracts.

Examples:

- OpenAPI
- Scalar
- Swagger compatibility

---

## Client SDK Generation

Technologies responsible for generating strongly typed client libraries.

Examples:

- NSwag
- Kiota

---

# Functional Requirements

The selected solution shall support:

- OpenAPI 3.x generation;
- interactive API exploration;
- endpoint documentation;
- request/response schemas;
- authentication documentation;
- versioned APIs;
- client SDK generation;
- strongly typed clients;
- CI/CD integration.

---

# Non-Functional Requirements

The solution should provide:

- excellent .NET 10 support;
- OpenAPI standards compliance;
- cloud neutrality;
- maintainability;
- automation;
- good developer experience.

---

# Candidate Technologies

## API Documentation

| Technology | Purpose |
|------------|---------|
| OpenAPI | API Contract Standard |
| Scalar | Interactive API UI |
| Swagger UI | Legacy UI Compatibility |

---

## Client Generation

| Technology | Purpose |
|------------|---------|
| NSwag | .NET Client Generation |
| Kiota | Multi-language Client Generation |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Standards Compliance | Critical |
| A2 | .NET 10 Compatibility | Critical |
| A3 | Clean Architecture Compatibility | Critical |
| A4 | Developer Experience | High |
| A5 | Client Generation Quality | High |
| A6 | Automation | High |
| A7 | Community & Maturity | High |
| A8 | Maintainability | High |

---

# Architecture Principle

Documentation shall always be generated from implementation.

```text
Controllers / Endpoints

        │

        ▼

OpenAPI Generation

        │

        ▼

Interactive Documentation

        │

        ▼

Generated Client SDKs
```

The OpenAPI document is the single source of truth.

Manual API documentation is prohibited.

---

# 5. OpenAPI Evaluation

## Overview

OpenAPI is the industry standard specification for describing HTTP APIs.

It defines:

- endpoints;
- parameters;
- request bodies;
- responses;
- authentication;
- schemas;
- metadata.

OpenAPI is not a UI framework.

It is the contract consumed by documentation tools, testing tools, gateways, and SDK generators.

---


# 5. OpenAPI Evaluation

## Overview

OpenAPI (formerly known as Swagger Specification) is the industry standard for describing RESTful HTTP APIs.

It provides a machine-readable contract that fully describes:

- endpoints;
- HTTP methods;
- request models;
- response models;
- authentication requirements;
- error contracts;
- metadata.

The OpenAPI document becomes the canonical description of the public API surface.

---

# Architectural Role

OpenAPI belongs to the API Contract layer.

```text
Minimal APIs / Controllers

          │

          ▼

 ASP.NET Core OpenAPI Generator

          │

          ▼

      OpenAPI Document

          │

 ┌────────┼───────────────┬───────────────┐

 ▼        ▼               ▼

Scalar   NSwag         Kiota

          │

          ▼

 Generated Clients
```

Business modules remain completely unaware of OpenAPI generation.

---

# Architectural Strengths

## Advantages

- Industry standard.
- Vendor neutral.
- Machine readable.
- Human readable.
- Strong ecosystem.
- Excellent tooling.
- Native ASP.NET Core integration.
- Client generation support.
- API gateway support.
- Testing tool integration.

---

# Standards Compliance

OpenAPI supports:

- OpenAPI 3.x
- JSON Schema
- HTTP
- OAuth2
- OpenID Connect

Standards compliance is considered excellent.

---

# Operational Characteristics

OpenAPI provides:

- API contracts;
- endpoint metadata;
- schema generation;
- security descriptions;
- versioning support.

Operational complexity is extremely low because the specification is generated automatically.

---

# Scalability

OpenAPI scales naturally regardless of API size.

Whether the platform contains:

- 20 endpoints;
- 200 endpoints;
- 2,000 endpoints;

the same generation process applies.

Scalability is considered excellent.

---

# Security Documentation

OpenAPI documents authentication without implementing it.

Examples include:

- JWT Bearer;
- OAuth2;
- OpenID Connect;
- API Keys.

This allows client generators and interactive documentation tools to authenticate correctly.

---

# Developer Experience

Developer productivity improves significantly because:

- documentation is always synchronized;
- contracts are strongly typed;
- request/response models remain discoverable;
- clients can be generated automatically.

Developer experience is considered excellent.

---

# Maintainability

OpenAPI documentation is generated from source code.

Consequently:

- no duplicated documentation;
- no manual synchronization;
- lower maintenance cost.

Maintainability is considered excellent.

---

# AI Compatibility

AI tooling increasingly relies on OpenAPI specifications.

Examples include:

- LLM Tool Calling;
- AI Agents;
- API discovery;
- automatic SDK generation;
- semantic API understanding.

OpenAPI therefore significantly improves AI integration capabilities.

---

# Cloud Neutrality

OpenAPI is completely platform independent.

Supported everywhere:

- Windows;
- Linux;
- Containers;
- Kubernetes;
- Cloud;
- Hybrid;
- On-Premise.

Cloud neutrality is excellent.

---

# Typical Usage

Suitable scenarios:

```text
REST APIs

Public APIs

Internal APIs

Microservices

Developer Portals

AI Tool Integration
```

Unsuitable scenarios:

```text
Business Documentation

Architecture Documentation

Requirements Documentation
```

OpenAPI documents API contracts—not business behavior.

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Standards Compliance | Excellent |
| Clean Architecture | Excellent |
| Cloud Neutrality | Excellent |
| Tool Integration | Excellent |
| AI Readiness | Excellent |
| Maintainability | Excellent |

---

# Relationship with ASP.NET Core

ASP.NET Core .NET 10 generates OpenAPI documents natively.

```text
Minimal API

      │

      ▼

Native OpenAPI Generator

      │

      ▼

OpenAPI Document
```

No third-party package is required for basic specification generation.

---

# Preliminary Conclusion

OpenAPI should become the single authoritative API contract format for MachineryManagerEnterprise.

All API documentation, client generation, testing tooling, and future AI integrations shall derive from the generated OpenAPI specification.

Manual API documentation shall not be maintained separately.

---


# 6. Scalar Evaluation

## Overview

Scalar is the modern interactive API documentation interface introduced as the recommended documentation experience for modern ASP.NET applications.

Unlike Swagger UI, which primarily focuses on rendering OpenAPI specifications, Scalar emphasizes:

- developer experience;
- readability;
- modern user interface;
- high-performance rendering;
- OpenAPI-first workflow.

Beginning with modern ASP.NET releases, Scalar is becoming the preferred interactive documentation interface.

Within MachineryManagerEnterprise, Scalar is evaluated as the primary API exploration interface.

---

# Architectural Role

Scalar belongs to the API Documentation Presentation layer.

```text
ASP.NET Core Endpoints

          │

          ▼

 OpenAPI Specification

          │

          ▼

        Scalar UI

          │

          ▼

Developers / Integrators
```

Scalar consumes the OpenAPI specification.

It never generates API contracts itself.

---

# Architectural Strengths

## Advantages

- Modern user interface.
- Excellent developer experience.
- Native OpenAPI integration.
- Fast rendering.
- Minimal configuration.
- Excellent dark/light themes.
- Responsive interface.
- Excellent readability.
- Fully standards based.
- Excellent .NET 10 integration.

---

# Operational Characteristics

Scalar provides:

- endpoint browsing;
- schema visualization;
- request examples;
- response visualization;
- authentication support;
- API exploration.

Operational complexity is extremely low.

---

# Standards Compliance

Scalar consumes standard OpenAPI documents.

Supported standards include:

- OpenAPI 3.x
- OAuth2
- OpenID Connect
- JWT Bearer

Standards compliance is excellent.

---

# Developer Experience

Developer productivity benefits from:

- cleaner navigation;
- improved endpoint discovery;
- modern interface;
- reduced visual complexity;
- excellent usability.

Compared to Swagger UI, Scalar provides a significantly improved experience.

---

# Security

Scalar does not implement authentication.

It visualizes authentication mechanisms described in the OpenAPI document.

Supported examples include:

- JWT Bearer
- OAuth2
- OpenID Connect

Security implementation remains entirely within the application.

---

# Scalability

Scalar performs well regardless of API size.

It supports:

- small APIs;
- enterprise APIs;
- modular monoliths;
- microservice ecosystems.

Scalability is excellent.

---

# Cloud Neutrality

Scalar is completely platform independent.

Supported environments include:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

Cloud neutrality is excellent.

---

# AI Compatibility

Although Scalar is primarily a documentation interface, its reliance on OpenAPI makes it naturally compatible with:

- AI-assisted API exploration;
- LLM tool discovery;
- automated documentation indexing.

Compatibility is considered excellent.

---

# Maintainability

Scalar requires almost no maintenance because:

- it consumes generated OpenAPI documents;
- configuration is minimal;
- upgrades follow the ASP.NET ecosystem.

Maintainability is considered excellent.

---

# Typical Usage

Suitable scenarios:

```text
Developer Portal

Internal API Documentation

Partner Integration

Interactive Testing

API Exploration
```

Unsuitable scenarios:

```text
Business Documentation

Architecture Documentation

Requirements Documentation
```

Scalar is an API exploration tool—not a documentation authoring system.

---

# Comparison with Swagger UI

| Capability | Scalar | Swagger UI |
|------------|---------|------------|
| Modern UI | Excellent | Moderate |
| Performance | Excellent | Good |
| Developer Experience | Excellent | Good |
| OpenAPI Support | Excellent | Excellent |
| ASP.NET 10 Alignment | Excellent | Good |
| Community Maturity | Growing | Excellent |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Standards Compliance | Excellent |
| Developer Experience | Excellent |
| Cloud Neutrality | Excellent |
| Maintainability | Excellent |
| .NET 10 Compatibility | Excellent |

---

# Relationship with OpenAPI

```text
ASP.NET Core

      │

      ▼

OpenAPI Document

      │

      ▼

Scalar
```

OpenAPI remains the authoritative contract.

Scalar provides its interactive visualization.

---

# Preliminary Conclusion

Scalar should become the primary interactive API documentation interface for MachineryManagerEnterprise.

Its modern design, native OpenAPI integration, and alignment with .NET 10 make it the preferred replacement for traditional Swagger UI in day-to-day API exploration.

---


# 7. Swagger UI Compatibility Evaluation

## Overview

Swagger UI has been the de facto interactive OpenAPI visualization tool in the .NET ecosystem for many years.

It provides a browser-based interface for:

- exploring endpoints;
- viewing schemas;
- submitting requests;
- testing APIs interactively.

Although it remains mature and widely adopted, .NET 10 increasingly positions **Scalar** as the preferred interactive documentation experience.

Therefore, Swagger UI is evaluated from the perspective of **compatibility**, not as the primary documentation interface.

---

# Architectural Role

Swagger UI belongs to the API Documentation Presentation layer.

```text
ASP.NET Core Endpoints

          │

          ▼

 OpenAPI Specification

          │

          ▼

      Swagger UI

          │

          ▼

Developers / Integrators
```

Swagger UI consumes an OpenAPI specification.

It does not generate API contracts.

---

# Architectural Strengths

## Advantages

- Extremely mature.
- Very large community.
- Excellent ecosystem support.
- OpenAPI compliant.
- Interactive testing.
- Authentication support.
- Extensive documentation.
- Broad tooling compatibility.

---

# Architectural Weaknesses

Compared with Scalar:

- legacy user interface;
- heavier presentation layer;
- lower visual clarity;
- slower evolution;
- weaker alignment with modern ASP.NET direction.

These are usability concerns rather than functional limitations.

---

# Operational Characteristics

Swagger UI provides:

- endpoint browsing;
- request execution;
- response visualization;
- authentication testing;
- schema inspection.

Operational complexity is very low.

---

# Standards Compliance

Swagger UI fully supports:

- OpenAPI 3.x;
- OAuth2;
- OpenID Connect;
- JWT Bearer authentication.

Standards compliance is excellent.

---

# Developer Experience

Developer experience remains very good.

However, compared with Scalar:

- navigation is less streamlined;
- presentation is more traditional;
- large APIs become harder to browse.

---

# Security

Swagger UI merely visualizes authentication.

Supported mechanisms include:

- Bearer Token;
- OAuth2;
- API Keys.

Application security remains implemented within ASP.NET Core.

---

# Scalability

Swagger UI scales adequately even for large APIs.

Performance remains acceptable for enterprise projects.

Scalability is considered very good.

---

# Cloud Neutrality

Supported everywhere:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

Cloud neutrality is excellent.

---

# AI Compatibility

Swagger UI itself provides no AI-specific capabilities.

Since it consumes OpenAPI specifications, it remains compatible with AI tooling indirectly.

Compatibility is considered good.

---

# Maintainability

Swagger UI benefits from:

- mature ecosystem;
- stable releases;
- predictable behavior.

Maintainability is considered excellent.

---

# Typical Usage

Suitable scenarios:

```text
API Exploration

Interactive Testing

Developer Documentation

Partner APIs
```

Unsuitable scenarios:

```text
Business Documentation

Architecture Documentation

Requirements Documentation
```

---

# Comparison with Scalar

| Criterion | Scalar | Swagger UI |
|-----------|---------|------------|
| Modern UX | Excellent | Good |
| Visual Design | Excellent | Moderate |
| Performance | Excellent | Very Good |
| Community Maturity | Growing | Excellent |
| OpenAPI Compliance | Excellent | Excellent |
| .NET 10 Alignment | Excellent | Good |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Standards Compliance | Excellent |
| Maintainability | Excellent |
| Cloud Neutrality | Excellent |
| Developer Experience | Very Good |
| .NET 10 Alignment | Good |

---

# Relationship with Scalar

Both tools consume the same OpenAPI document.

```text
OpenAPI

      │

 ┌────┴───────────┐

 ▼                ▼

Scalar      Swagger UI
```

Only one documentation UI should be exposed in production.

Maintaining multiple interactive documentation interfaces increases operational complexity without providing additional architectural value.

---

# Preliminary Conclusion

Swagger UI remains an excellent and mature OpenAPI visualization tool.

However, because MachineryManagerEnterprise targets **.NET 10**, the preferred interactive documentation interface should be **Scalar**.

Swagger UI should be retained only when backward compatibility with existing tooling or organizational standards requires it.

---


# 8. NSwag Evaluation

## Overview

NSwag is one of the most mature OpenAPI tooling ecosystems available for .NET.

It provides:

- OpenAPI document generation;
- strongly typed C# client generation;
- TypeScript client generation;
- MSBuild integration;
- CLI tooling;
- automated client generation.

Within MachineryManagerEnterprise, NSwag is evaluated as the primary client SDK generation platform.

---

# Architectural Role

NSwag belongs to the Client Generation layer.

```text
ASP.NET Core APIs

        │

        ▼

 OpenAPI Specification

        │

        ▼

        NSwag

        │

 ┌──────────────┬──────────────┐

 ▼              ▼

C# SDK     TypeScript SDK
```

NSwag consumes OpenAPI.

It does not replace the API contract.

---

# Architectural Strengths

## Advantages

- Mature ecosystem.
- Excellent .NET support.
- Automatic C# client generation.
- TypeScript client generation.
- MSBuild integration.
- CLI tooling.
- CI/CD friendly.
- Strong OpenAPI support.
- Large community.
- Excellent documentation.

---

# Operational Characteristics

NSwag supports:

- code generation;
- contract synchronization;
- automated SDK updates;
- build integration.

Operational complexity is considered low.

---

# Developer Experience

Developer productivity improves significantly because:

- API clients are generated automatically;
- no handwritten HTTP wrappers;
- compile-time type safety;
- synchronized contracts.

Developer experience is considered excellent.

---

# Maintainability

Generated clients:

- remain synchronized with OpenAPI;
- reduce duplicated code;
- eliminate manual HTTP implementation.

Maintainability is considered excellent.

---

# Scalability

NSwag scales naturally regardless of API size.

Whether the API contains:

- 20 endpoints;
- 500 endpoints;
- 2,000 endpoints;

generation remains automated.

Scalability is excellent.

---

# Cloud Neutrality

NSwag is completely deployment independent.

Supported environments include:

- Windows
- Linux
- Containers
- Cloud
- On-Premise
- CI/CD

Cloud neutrality is excellent.

---

# Security

Generated SDKs support:

- JWT Bearer authentication;
- OAuth2;
- OpenID Connect.

Authentication behavior is derived directly from the OpenAPI specification.

---

# AI Compatibility

Generated SDKs improve AI integration because:

- strongly typed contracts;
- consistent API surface;
- machine-readable specifications.

AI compatibility is considered excellent.

---

# CI/CD Integration

NSwag integrates naturally into:

- MSBuild;
- GitHub Actions;
- Azure DevOps;
- command-line pipelines.

Client SDKs can therefore be regenerated automatically during every build.

---

# Typical Usage

Suitable scenarios:

```text
Desktop Client SDK

Internal SDK

Shared API Library

TypeScript SDK

Automated API Client Generation
```

Unsuitable scenarios:

```text
Manual HTTP Wrappers

Handwritten SDKs

Contract Duplication
```

---

# Comparison with Manual Clients

| Criterion | NSwag | Manual Client |
|-----------|--------|---------------|
| Maintainability | Excellent | Poor |
| Synchronization | Excellent | Manual |
| Type Safety | Excellent | Variable |
| Automation | Excellent | None |
| Error Risk | Low | High |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Developer Experience | Excellent |
| Automation | Excellent |
| Cloud Neutrality | Excellent |
| Maintainability | Excellent |
| Enterprise Readiness | Excellent |

---

# Relationship with OpenAPI

```text
OpenAPI

      │

      ▼

    NSwag

      │

      ▼

 Generated SDK
```

OpenAPI remains the authoritative contract.

NSwag automatically produces strongly typed client libraries from that contract.

---

# Preliminary Conclusion

NSwag represents the strongest choice for automatic client SDK generation within MachineryManagerEnterprise.

Its maturity, automation capabilities, and excellent .NET integration make it the recommended solution for generating C# (and optionally TypeScript) client libraries directly from the OpenAPI specification.

---


# 9. Kiota Evaluation

## Overview

Kiota is Microsoft's modern OpenAPI-based client SDK generator.

Unlike NSwag, which primarily targets .NET and TypeScript, Kiota is designed to generate SDKs for multiple programming languages from a single OpenAPI specification.

Supported languages include:

- C#
- TypeScript
- Java
- Go
- Python
- PHP
- Ruby

Kiota was originally developed for Microsoft Graph but has evolved into a general-purpose OpenAPI client generator.

Within MachineryManagerEnterprise, Kiota is evaluated as a potential multi-language SDK generation platform.

---

# Architectural Role

Kiota belongs to the Client SDK Generation layer.

```text
ASP.NET Core APIs

        │

        ▼

 OpenAPI Specification

        │

        ▼

        Kiota

        │

 ┌────────────┬─────────────┬──────────────┐

 ▼            ▼             ▼

 C#        TypeScript     Python

        Generated SDKs
```

Like NSwag, Kiota consumes OpenAPI rather than generating it.

---

# Architectural Strengths

## Advantages

- Microsoft-supported project.
- Multi-language SDK generation.
- Modern architecture.
- OpenAPI-first design.
- Excellent integration with Microsoft tooling.
- Strong long-term roadmap.
- Consistent SDK generation model.

---

# Architectural Weaknesses

Compared with NSwag:

- younger ecosystem;
- smaller community;
- fewer .NET-specific customization capabilities;
- less mature developer tooling;
- more limited ecosystem adoption outside Microsoft environments.

For projects whose primary client is .NET, Kiota currently offers fewer practical advantages.

---

# Operational Characteristics

Kiota supports:

- automated SDK generation;
- strongly typed clients;
- contract synchronization;
- multi-language outputs.

Operational complexity is considered low.

---

# Developer Experience

Developer experience is good.

Strengths:

- consistent SDKs;
- clean generated code;
- language independence.

Weaknesses:

- fewer examples;
- fewer community extensions;
- less mature tooling than NSwag.

---

# Maintainability

Generated SDKs remain synchronized with OpenAPI specifications.

Maintainability is considered excellent.

---

# Scalability

Kiota scales well regardless of API size.

Its architecture is particularly attractive for organizations supporting multiple programming languages.

Scalability is considered excellent.

---

# Cloud Neutrality

Kiota is fully platform independent.

Supported environments include:

- Windows
- Linux
- Containers
- Cloud
- Hybrid
- On-Premise

Cloud neutrality is excellent.

---

# Security

Authentication support derives directly from the OpenAPI specification.

Supported mechanisms include:

- OAuth2;
- OpenID Connect;
- Bearer Tokens.

Security support is excellent.

---

# AI Compatibility

Multi-language SDK generation is valuable for AI ecosystems where services may be implemented in different languages.

Examples include:

- Python AI pipelines;
- Go services;
- C# enterprise services.

AI compatibility is considered excellent.

---

# Typical Usage

Suitable scenarios:

```text
Multi-language SDKs

Cross-platform APIs

Public API Distribution

Microsoft Graph-style APIs
```

Less suitable scenarios:

```text
Single-language Enterprise Systems

Pure .NET Client Ecosystems
```

---

# Comparison with NSwag

| Criterion | NSwag | Kiota |
|-----------|--------|--------|
| C# SDK Quality | Excellent | Very Good |
| TypeScript Support | Excellent | Excellent |
| Multi-language Support | Moderate | Excellent |
| Community Maturity | Excellent | Good |
| .NET Tooling | Excellent | Good |
| Microsoft Alignment | Good | Excellent |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Cloud Neutrality | Excellent |
| Multi-language Support | Excellent |
| Developer Experience | Very Good |
| Enterprise Readiness | Very Good |
| .NET 10 Alignment | Excellent |

---

# Relationship with NSwag

The two tools are complementary rather than mutually exclusive.

```text
OpenAPI

      │

 ┌────┴────────────┐

 ▼                 ▼

NSwag          Kiota

.NET SDK     Multi-language SDKs
```

For organizations primarily developing .NET clients, NSwag remains the stronger default.

Kiota becomes attractive when official SDKs must be distributed across multiple programming languages.

---

# Preliminary Conclusion

Kiota is an excellent modern client SDK generator, particularly for organizations requiring first-class multi-language support.

However, MachineryManagerEnterprise currently targets a predominantly .NET ecosystem with an Avalonia desktop client and internal service consumers.

For this reason:

- **NSwag should be the primary client generation technology.**
- **Kiota may be introduced in the future if public multi-language SDK distribution becomes a project requirement.**

---


# 10. Overall Technology Comparison

API documentation and client generation are two distinct but tightly related concerns.

The recommended architecture separates:

- API contract generation;
- interactive documentation;
- client SDK generation.

All downstream tooling shall consume a single authoritative OpenAPI document.

---

# Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|----------------|------------------------|-------------|---------|
| API Contract | OpenAPI 3.x | — | Canonical API Specification |
| Interactive Documentation | Scalar | Swagger UI | API Exploration |
| Legacy Compatibility | Swagger UI | — | Backward Compatibility |
| .NET Client Generation | NSwag | Kiota | Strongly Typed .NET SDK |
| Multi-language SDK Generation | Kiota (Optional) | NSwag | Public SDK Distribution |

---

# Capability Comparison

| Capability | OpenAPI | Scalar | Swagger UI | NSwag | Kiota |
|------------|---------|--------|------------|--------|--------|
| API Contract | Excellent | No | No | Consumes | Consumes |
| Interactive UI | No | Excellent | Excellent | No | No |
| Native .NET 10 Alignment | Excellent | Excellent | Good | Excellent | Excellent |
| C# SDK Generation | No | No | No | Excellent | Very Good |
| TypeScript SDK | No | No | No | Excellent | Excellent |
| Multi-language SDK | No | No | No | Moderate | Excellent |
| Automation | Excellent | Excellent | Good | Excellent | Excellent |
| Community Maturity | Excellent | Good | Excellent | Excellent | Good |
| Cloud Neutrality | Excellent | Excellent | Excellent | Excellent | Excellent |
| AI Compatibility | Excellent | Good | Good | Excellent | Excellent |

---

# Developer Experience Comparison

| Criterion | Best Choice |
|-----------|-------------|
| API Exploration | Scalar |
| API Compatibility | Swagger UI |
| .NET Client Development | NSwag |
| Cross-language SDKs | Kiota |
| API Contract | OpenAPI |

---

# Build Pipeline Integration

```text
ASP.NET Core API

        │

        ▼

Native OpenAPI Generation

        │

        ▼

OpenAPI Specification

        │

 ┌──────┼───────────────┬───────────────┐

 ▼      ▼               ▼

Scalar Swagger UI    Client Generators

                         │

               ┌─────────┴─────────┐

               ▼                   ▼

            NSwag               Kiota

               │                   │

               ▼                   ▼

      .NET SDKs          Multi-language SDKs
```

This pipeline ensures every artifact derives from the same contract.

---

# Maintainability Assessment

Maintaining a single OpenAPI document as the authoritative contract provides:

- zero duplicated documentation;
- synchronized client SDKs;
- consistent testing;
- easier versioning;
- simplified CI/CD.

Maintainability is considered excellent.

---

# Cloud Neutrality

All evaluated technologies are cloud neutral.

They support:

- Windows;
- Linux;
- Containers;
- Kubernetes;
- Cloud;
- Hybrid;
- On-Premise.

No vendor lock-in is introduced.

---

# AI Readiness

The recommended architecture naturally supports AI-enabled tooling.

Benefits include:

- OpenAPI-based tool discovery;
- automatic SDK generation;
- machine-readable API contracts;
- future LLM tool integration.

AI readiness is excellent.

---

# Technology Selection Summary

| Area | Selected Technology |
|------|---------------------|
| API Contract | OpenAPI |
| Interactive Documentation | Scalar |
| Compatibility UI | Swagger UI (optional) |
| Primary SDK Generator | NSwag |
| Multi-language SDKs | Kiota (optional) |

---

# Architectural Assessment

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Standards Compliance | Excellent |
| Developer Experience | Excellent |
| Automation | Excellent |
| Maintainability | Excellent |
| AI Readiness | Excellent |
| Enterprise Readiness | Excellent |

---

# Overall Conclusion

The evaluated technologies complement each other rather than compete.

OpenAPI becomes the canonical API contract.

Scalar becomes the preferred interactive documentation interface.

NSwag becomes the primary SDK generation platform.

Kiota remains available for future multi-language distribution scenarios.

Swagger UI remains supported only for backward compatibility where organizational requirements demand it.

---


# 11. Final Recommendation

After evaluating the available technologies, the following API documentation and client generation architecture is recommended for MachineryManagerEnterprise.

## Core Technology Stack

| Responsibility | Selected Technology | Rationale |
|----------------|---------------------|-----------|
| API Contract Standard | OpenAPI 3.x | Industry standard, vendor neutral, ecosystem foundation |
| Interactive Documentation | Scalar | Modern UI, excellent .NET 10 alignment, superior developer experience |
| Primary Client SDK Generation | NSwag | Mature ecosystem, outstanding C# support, excellent automation |
| Multi-language SDK Generation | Kiota | Optional future capability for public SDK distribution |
| Legacy Documentation UI | Swagger UI | Compatibility only |

---

# Recommended Documentation Architecture

```text
ASP.NET Core API

        │

        ▼

Native OpenAPI Generation

        │

        ▼

Canonical OpenAPI Document

        │

 ┌──────────────┬────────────────────┬────────────────────┐

 ▼              ▼                    ▼

Scalar      NSwag              Kiota (Optional)

                │

        Generated Client SDKs

                │

        Desktop / Services / Future SDKs
```

---

# API Contract Strategy

The OpenAPI document becomes the **single source of truth** for the public API.

Every downstream artifact shall be generated from that contract.

Generated artifacts include:

- interactive documentation;
- client SDKs;
- integration tests (future);
- AI tool metadata (future).

Manual duplication of endpoint documentation is prohibited.

---

# Documentation Strategy

Interactive documentation shall use:

- **Scalar** as the default UI.

Swagger UI should remain available only when:

- legacy integrations require it;
- existing organizational tooling depends on it.

It shall not be the primary documentation interface.

---

# Client Generation Strategy

The project shall generate strongly typed SDKs rather than maintaining handwritten HTTP clients.

Primary technology:

- NSwag

Future extension:

- Kiota when official multi-language SDKs become a project requirement.

---

# Build Pipeline Integration

The build pipeline should automatically:

1. Build APIs.
2. Generate OpenAPI.
3. Generate client SDKs.
4. Publish documentation.

```text
Build

   │

   ▼

OpenAPI

   │

   ▼

NSwag

   │

   ▼

Generated SDK

   │

   ▼

Application Packaging
```

No manual SDK generation should be performed.

---

# Security Considerations

The documentation platform must accurately describe:

- JWT authentication;
- OAuth 2.1;
- OpenID Connect;
- authorization requirements.

However:

Documentation never replaces security implementation.

Authentication remains enforced by the application itself.

---

# Cloud Neutrality

The selected stack remains completely cloud neutral.

It introduces:

- no vendor lock-in;
- no proprietary runtime dependency;
- no platform restrictions.

Deployment targets include:

- Windows;
- Linux;
- Containers;
- Kubernetes;
- Hybrid;
- On-Premise.

---

# AI Readiness

The selected technologies significantly improve future AI integration.

Examples include:

- LLM Tool Calling via OpenAPI;
- automatic API discovery;
- generated SDKs for AI agents;
- semantic endpoint understanding.

The architecture therefore aligns with future AI expansion.

---

# Final Decision

The Architecture Review Board approves the following technology stack.

| Component | Decision |
|----------|----------|
| OpenAPI | Approved |
| Scalar | Approved |
| NSwag | Approved |
| Kiota | Approved (Optional Future Use) |
| Swagger UI | Approved (Compatibility Only) |

---

# Decision Summary

The selected solution satisfies all architectural goals.

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Excellent Developer Experience
- ✔ Cloud Neutrality
- ✔ Automation
- ✔ AI Readiness
- ✔ Long-term Maintainability

The above technologies are therefore adopted as the enterprise standard for API documentation and client generation within MachineryManagerEnterprise.

---

# Related ADR

- ADR-0001 — Clean Architecture
- ADR-0015 — Deployment Architecture

---

# Related Documents

- SolutionStructure.md
- DependencyRules.md
- CodingStandards.md

---

# Revision History

| Version | Date       | Author             | Description                                                               |
|---------|------------|--------------------|---------------------------------------------------------------------------|
| 1.0.0   | 2026-07-26 | Solution Architect | Initial technology evaluation for API Documentation and Client Generation |
| 1.3.0   | 2026-07-28 | Solution Architect | New section added (Evaluation Scope)                                      |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0                                 |