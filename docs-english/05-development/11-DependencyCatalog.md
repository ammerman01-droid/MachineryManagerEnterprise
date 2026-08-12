| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOC-DEV-012        |
| **Title**        | Dependency Catalog |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

This document defines the official dependency governance process for the
MachineryManagerEnterprise solution.

It serves as the authoritative register of all third-party libraries adopted by
the project.

Every dependency introduced into the solution shall appear in this catalog.

---

# Objectives

The dependency catalog shall:

- Prevent uncontrolled package growth.
- Record architectural decisions.
- Improve maintainability.
- Simplify upgrades.
- Support security reviews.
- Ensure license compliance.

---

# Open Source First

The solution adopts an **Open Source First** policy.

Only open-source libraries may be introduced unless an approved ADR explicitly
documents an exception.

See:

- ADR-0002 – Open Source First Policy

---

# Dependency Lifecycle

Every dependency follows the same lifecycle.

```text
Need

↓

Technology Evaluation (TE)

↓

Proof of Concept (Optional)

↓

Architecture Decision Record (ADR)

↓

Approved

↓

Directory.Packages.props

↓

Implementation

↓

Maintenance
```

No package may bypass this process.

---

# Central Package Management

All NuGet package versions are managed centrally through Directory.Packages.props.
Project files contain PackageReference elements without Version attributes.

Package versions are managed centrally through Directory.Packages.props.

Project files must not define Version attributes in PackageReference elements.

The single source of truth is:

```text
Directory.Packages.props
```

Project files must never contain package versions.

---

# Dependency Categories

Dependencies are grouped into categories.

Examples:

- Framework
- Validation
- Persistence
- Mapping
- Logging
- Testing
- UI Components
- Utilities

---

# Dependency Register

| Package | Category | TE | ADR | Status | Notes |
|----------|----------|----|-----|--------|-------|
| Blazor (Server / WebAssembly) | Framework | TE-0002 | ADR-0004 | Approved | Web UI framework |
| MudBlazor | UI Components | TE-0003 | ADR-0005 | Approved | Blazor component library |
| Microsoft.EntityFrameworkCore | Persistence | TE-0004, TE-0024 | ADR-0006 | Approved | Primary ORM / write-side |
| FluentValidation | Validation | TE-0005, TE-0022 | ADR-0007, ADR-0036 | Approved | Request validation |
| MediatR.Extensions.FluentValidation (or equivalent pipeline behavior) | Validation | TE-0022 | ADR-0036 | Approved | Validation pipeline orchestration |
| Mapster | Mapping | TE-0006, TE-0023 | ADR-0008 | Approved | Object mapping |
| Serilog | Logging | TE-0007, TE-0017 | ADR-0009, ADR-0033 | Approved | Structured logging provider |
| OpenTelemetry | Observability | TE-0008, TE-0017 | ADR-0010, ADR-0033 | Approved | Unified telemetry standard |
| Prometheus (client/exporter) | Observability | TE-0017 | ADR-0033 | Approved | Metrics backend |
| Grafana | Observability | TE-0017 | ADR-0033 | Approved | Dashboard / visualization |
| Grafana Tempo | Observability | TE-0017 | ADR-0033 | Approved | Distributed trace backend |
| MediatR | Framework | TE-0009 | ADR-0011 | Approved | CQRS pipeline |
| .NET MAUI | UI Framework | TE-0010 | ADR-0013 | Approved | Desktop & mobile client framework |
| SQLite | Persistence | TE-0011 | ADR-0014 | Approved | Embedded workspace database |
| LiteDB | Persistence | TE-0011 | ADR-0014 | Approved | Embedded workspace database (alternative) |
| MassTransit | Messaging | TE-0012 | ADR-0016, ADR-0018 | Approved | Messaging abstraction; also backs External Integration Connector Framework |
| RabbitMQ | Messaging | TE-0012 | ADR-0016 | Approved | Message broker |
| Semantic Kernel | AI | TE-0013 | ADR-0017 | Approved | AI orchestration framework |
| Dapper | Persistence | TE-0024 | ADR-0019 | Approved | Read-heavy / reporting queries only, never DDL |
| MinIO | Storage | TE-0026 | ADR-0020 | Approved | S3-compatible object store (default) |
| AWSSDK.S3 (or equivalent S3-compatible client) | Storage | TE-0026 | ADR-0020 | Approved | S3 API client |
| Qdrant.Client | AI / Search | TE-0028 | ADR-0022 | Approved | Vector database client |
| Azure OpenAI SDK | AI | TE-0029 | ADR-0023 | Approved | Primary AI provider |
| OpenAI SDK | AI | TE-0029 | ADR-0023 | Approved | Secondary AI provider |
| Ollama (client) | AI | TE-0029 | ADR-0023 | Approved | Local/offline AI provider |
| xUnit | Testing | TE-0030 | ADR-0024 | Approved | Test framework |
| Moq | Testing | TE-0030 | ADR-0024 | Approved | Mocking framework |
| Testcontainers | Testing | TE-0030 | ADR-0024 | Approved | Integration test infrastructure |
| Docker | Build / Deployment | TE-0031 | ADR-0025 | Approved | Containerization |
| .NET Aspire | Build / Deployment | TE-0031 | ADR-0025 | Approved | Local multi-service orchestration |
| GitHub Actions | Build / Deployment | TE-0031 | ADR-0025 | Approved | CI/CD pipeline |
| Microsoft.AspNetCore.DataProtection | Security | TE-0032 | ADR-0026 | Approved | Application data protection |
| k6 | Testing | TE-0033 | ADR-0027 | Approved | Load testing |
| NBomber | Testing | TE-0033 | ADR-0027 | Approved | .NET-native load/performance testing |
| QuestPDF | Reporting | TE-0035 | ADR-0029 | Approved | PDF generation |
| ASP.NET Core Identity | Identity | TE-0020 | ADR-0030 | Approved | Identity management |
| OpenIddict | Identity | TE-0020 | ADR-0030 | Approved | OAuth2/OIDC server |
| FusionCache (or HybridCache) | Caching | TE-0015 | ADR-0031 | Approved | L1/L2 hybrid cache |
| Microsoft.Extensions.Caching.Memory (IMemoryCache) | Caching | TE-0015 | ADR-0031 | Approved | In-process L1 cache |
| StackExchange.Redis | Caching | TE-0015 | ADR-0031 | Approved | Distributed L2 cache |
| Quartz.NET | Scheduling | TE-0014, TE-0019 | ADR-0032 | Approved | Job scheduling |
| System.Threading.Channels | Scheduling | TE-0014, TE-0019 | ADR-0032 | Approved | In-process background queues |
| Microsoft.Extensions.Configuration | Configuration | TE-0018 | ADR-0034 | Approved | Configuration abstraction |
| Microsoft.Extensions.Options | Configuration | TE-0018 | ADR-0034 | Approved | Strongly typed configuration |
| Microsoft.FeatureManagement | Configuration | TE-0018 | ADR-0034 | Approved | Feature flags |
| HashiCorp Vault (client) | Configuration | TE-0018 | ADR-0034 | Approved | Enterprise secret store |
| Azure.Security.KeyVault.Secrets | Configuration | TE-0018 | ADR-0034 | Approved | Azure-specific secret store alternative |
| Scalar.AspNetCore | API Documentation | TE-0021 | ADR-0035 | Approved | Interactive API documentation |
| NSwag | API Documentation | TE-0021 | ADR-0035 | Approved | C# client SDK generation |
| EF Core Migrations (tooling) | Persistence | TE-0025 | ADR-0037 | Approved | Schema migrations, sole schema owner |
| Avalonia UI | UI Framework | TE-0034 | ADR-0028 | Deprecated | Superseded by .NET MAUI (ADR-0013) — do not introduce into new code |
| FluentAvalonia | UI Components | TE-0034 | ADR-0028 | Deprecated | Superseded by .NET MAUI (ADR-0013) |
| CommunityToolkit.Mvvm | MVVM | TE-0034 | ADR-0028 | Deprecated | Was paired with Avalonia; re-evaluate for MAUI before reuse |
| Azure Logic Apps | Integration | TE-0036 | ADR-0018 | Approved | Optional, non-default Azure-specific integration path |

---

# Status Definitions

| Status | Meaning |
|---------|---------|
| Proposed | Under evaluation |
| Approved | Official dependency |
| Deprecated | Planned for removal |
| Rejected | Not accepted |

---

# Upgrade Policy

Dependencies should be updated regularly.

Before upgrading:

- Review release notes.
- Verify compatibility.
- Execute automated tests.
- Update ADR if architectural behavior changes.

---

# Security

Dependencies should be monitored for:

- Known vulnerabilities
- Unsupported versions
- License changes
- Maintenance status

Critical vulnerabilities require immediate review.

---

# Removal Policy

Unused dependencies shall be removed.

Removal process:

1. Verify no project references remain.
2. Remove from implementation.
3. Remove from Directory.Packages.props.
4. Update this catalog.
5. Close associated maintenance task.

---

# Experimental Libraries

Experimental packages shall never be added directly to production.

They must first pass through:

- Technology Evaluation
- Proof of Concept

---

# Versioning

Stable versions are preferred.

Preview packages require explicit architectural approval.

---

# Compliance

Every third-party dependency introduced into the solution shall be documented in
this catalog.

Undocumented dependencies are not permitted.

---

# Related Documents

- DOC-CONVENTIONS
- DOC-README
- ADR-0002 (Open Source First Policy)
- ADR-0007 (Use FluentValidation)
- TE-0005 (FluentValidation Evaluation)

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial dependency catalog                            |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Populated the full Dependency Register (was a single-row stub covering only FluentValidation) with every package approved across ADR-0003–ADR-0037, including Deprecated entries for Avalonia UI / FluentAvalonia / CommunityToolkit.Mvvm (superseded by ADR-0013) |