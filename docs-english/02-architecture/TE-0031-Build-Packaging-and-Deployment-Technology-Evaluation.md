| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | TE-0031            |
| **Title**        | Build, Packaging and Deployment Technology Evaluation |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-28         |
| **Last Updated** | 2026-08-08         |

# Purpose

This Technology Evaluation determines the build, packaging and deployment technology stack for MachineryManagerEnterprise.

The selected technologies shall support:

- Enterprise Build Automation
- Continuous Integration
- Continuous Delivery
- Containerization
- Local Development
- Cloud Deployment
- Hybrid Deployment
- Infrastructure Consistency
- Long-Term Maintainability

---

# Evaluation Scope

This Technology Evaluation evaluates:

- .NET 10 SDK
- Docker
- .NET Aspire
- GitHub Actions
- Azure DevOps

This document does **not** define:

- Release Strategy
- Environment Topology
- Infrastructure Provisioning
- Versioning Policy
- Branching Strategy

These architectural decisions will be documented separately in the corresponding ADR.

---

# Relationship with Related ADRs

This Technology Evaluation supports:

- ADR-0025 — Build & Deployment Architecture

It depends upon:

- Clean Architecture
- Solution Structure
- Dependency Rules
- Testing Strategy

---

# Architectural References

This evaluation is based upon:

- Clean Architecture
- Hybrid Deployment Strategy
- Enterprise DevOps Principles
- CI/CD Best Practices
- Infrastructure as Code Readiness

---

# Scope

The following technologies are evaluated:

- .NET 10 SDK
- Docker
- .NET Aspire
- GitHub Actions
- Azure DevOps

---

# Current Build Architecture

The approved architecture requires a unified build pipeline capable of supporting:

- Local developer builds
- Automated validation
- Containerized deployment
- Cloud deployment
- Hybrid deployment

```text
Developer

      │

      ▼

Build

      │

      ▼

Test

      │

      ▼

Package

      │

      ▼

Deploy
```

---

# Functional Requirements

The build platform shall support:

- Solution Build
- Incremental Build
- Automated Testing
- Artifact Packaging
- Container Image Generation
- Multi-Environment Deployment
- Pipeline Automation
- Versioned Releases

---

# Non-Functional Requirements

The build technologies shall provide:

- High Reliability
- Reproducible Builds
- Cross Platform Support
- Enterprise Scalability
- Operational Simplicity
- Excellent Tooling
- Long-Term Support
- CI/CD Compatibility

---

# Candidate Technologies

| Candidate | Category |
|-----------|----------|
| .NET 10 SDK | Build Platform |
| Docker | Container Platform |
| .NET Aspire | Distributed Application Orchestration |
| GitHub Actions | CI/CD Platform |
| Azure DevOps | Enterprise DevOps Platform |

---

# Evaluation Criteria

| ID | Criterion | Priority |
|----|-----------|----------|
| BD-01 | Enterprise Readiness | Critical |
| BD-02 | CI/CD Compatibility | Critical |
| BD-03 | Cross Platform | High |
| BD-04 | Developer Productivity | High |
| BD-05 | Operational Simplicity | High |
| BD-06 | Deployment Flexibility | High |
| BD-07 | Long-Term Maintainability | High |
| BD-08 | Microsoft Ecosystem Integration | Medium |
| BD-09 | Community Support | Medium |
| BD-10 | Future Scalability | High |

---

# Architecture Principle

The evaluated component acts as an isolated infrastructure service, adhering strictly to Clean Architecture layer dependencies and domain isolation rules.

---

# 8. .NET 10 SDK Evaluation

## Overview

.NET 10 SDK is the official Microsoft Software Development Kit for building, testing, publishing and packaging .NET applications.

It provides the unified toolchain required for:

- source compilation;
- dependency restoration;
- testing;
- packaging;
- publishing;
- artifact generation.

For MachineryManagerEnterprise, .NET 10 SDK is evaluated as the foundational build platform.

---

# Architectural Role

```text
        Source Code

             │

             ▼

      .NET 10 SDK

 ┌──────────────────────┐

 │ Restore              │
 │ Build                │
 │ Test                 │
 │ Publish              │
 │ Pack                 │

 └──────────────────────┘

             │

             ▼

        Build Artifacts
```

The SDK becomes the single authoritative build tool for every project inside the solution.

---

# Architectural Strengths

Advantages include:

- Official Microsoft platform
- Single build toolchain
- Cross-platform execution
- Unified CLI
- Native MSBuild integration
- SDK-style project support
- Incremental builds
- Long-term Microsoft support

---

# Functional Capabilities

.NET 10 SDK supports:

- Solution Build
- Incremental Build
- Package Restore
- Project References
- Test Execution
- Publish
- NuGet Packaging
- Native AOT (where applicable)
- Cross-platform compilation

---

# Build Pipeline Integration

Typical execution flow:

```text
dotnet restore

        │

        ▼

dotnet build

        │

        ▼

dotnet test

        │

        ▼

dotnet publish
```

The SDK provides every stage required by the approved build pipeline.

---

# Cross-Platform Support

Supported operating systems:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

The same CLI commands execute consistently across all supported environments.

---

# Performance

.NET 10 SDK provides:

- Incremental compilation
- Optimized MSBuild pipeline
- Parallel project compilation
- Efficient dependency restoration

Build performance is considered **Excellent**.

---

# CI/CD Compatibility

The SDK integrates directly with:

- GitHub Actions
- Azure DevOps
- Jenkins
- TeamCity
- Docker
- Local Build Servers

No additional build tools are required.

---

# Developer Experience

Advantages include:

- Unified CLI
- Visual Studio integration
- Rider integration
- VS Code support
- Rich diagnostics
- Excellent documentation

Developer experience is considered **Excellent**.

---

# Packaging Support

Supported outputs include:

- Executables
- Libraries
- NuGet Packages
- Self-contained Deployments
- Single-file Applications
- Ready-to-run Publishing

---

# Enterprise Suitability

.NET 10 SDK is appropriate for:

- Enterprise Applications
- Modular Solutions
- Large Repositories
- Continuous Integration
- Automated Deployment
- Long-Term Maintenance

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| CI/CD Compatibility | Excellent |
| Cross Platform | Excellent |
| Developer Productivity | Excellent |
| Operational Simplicity | Excellent |
| Build Performance | Excellent |
| Documentation | Excellent |
| Long-Term Viability | Excellent |

---

# Advantages

- Official Microsoft build platform
- Mature ecosystem
- Excellent tooling
- Strong automation support
- Native integration with .NET ecosystem

---

# Disadvantages

- Requires SDK installation on build agents
- Version management must remain consistent across all environments

These considerations are operational rather than architectural limitations.

---

# Preliminary Conclusion

.NET 10 SDK completely satisfies the build platform requirements of MachineryManagerEnterprise.

It is approved as the official build toolchain for all development, testing, packaging and publishing activities.

---


# 9. Docker Evaluation

## Overview

Docker is the industry-standard containerization platform for packaging, distributing and executing applications in isolated, reproducible environments.

Within MachineryManagerEnterprise, Docker is evaluated as the primary technology for:

- application packaging;
- deployment consistency;
- integration testing infrastructure;
- CI/CD execution;
- environment standardization.

Docker does **not** replace the application deployment model; it standardizes the execution environment.

---

# Architectural Role

```text
          Application

               │

               ▼

        dotnet publish

               │

               ▼

         Docker Image

               │

      ┌────────┼────────┐

      ▼        ▼        ▼

 Development  Testing  Production
```

Docker provides a consistent runtime across every environment.

---

# Architectural Strengths

Advantages include:

- Environment consistency
- Immutable deployment artifacts
- Infrastructure portability
- Isolation
- Versioned images
- Broad ecosystem support
- Excellent CI/CD integration
- Mature tooling

---

# Functional Capabilities

Docker supports:

- Container Image Creation
- Multi-stage Builds
- Image Versioning
- Runtime Isolation
- Container Networking
- Volume Management
- Environment Configuration
- Registry Integration

---

# Multi-Stage Build Support

Docker enables optimized production images.

Typical workflow:

```dockerfile
SDK Image

      │

Build

      │

Publish

      ▼

Runtime Image
```

Benefits include:

- smaller image size;
- reduced attack surface;
- faster deployments.

---

# Environment Consistency

Docker guarantees that:

- local development;
- automated testing;
- production deployment

execute using the same runtime configuration.

This significantly reduces environment-specific defects.

---

# Cross-Platform Support

Supported hosts include:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

Container images remain portable across supported operating systems.

---

# Performance

Docker provides:

- lightweight isolation;
- fast startup;
- efficient resource utilization;
- minimal runtime overhead.

Performance is considered **Excellent**.

---

# CI/CD Compatibility

Docker integrates directly with:

- GitHub Actions
- Azure DevOps
- Docker Hub
- Azure Container Registry
- GitHub Container Registry

Image creation can be fully automated.

---

# Security

Docker supports:

- image signing;
- image scanning;
- minimal runtime images;
- container isolation;
- non-root execution.

Security is considered **Excellent** when enterprise best practices are followed.

---

# Developer Experience

Advantages include:

- simple CLI;
- Visual Studio integration;
- VS Code integration;
- reproducible local environments;
- extensive documentation.

Developer experience is considered **Excellent**.

---

# Enterprise Suitability

Docker is appropriate for:

- Development
- Integration Testing
- Continuous Integration
- Packaging
- Deployment
- Hybrid Infrastructure

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| CI/CD Compatibility | Excellent |
| Cross Platform | Excellent |
| Developer Productivity | Excellent |
| Operational Simplicity | Excellent |
| Deployment Flexibility | Excellent |
| Security | Excellent |
| Long-Term Viability | Excellent |

---

# Advantages

- Industry standard
- Mature ecosystem
- Portable deployment artifacts
- Strong Microsoft support
- Excellent DevOps integration

---

# Disadvantages

- Additional container layer
- Requires container runtime on deployment hosts

These operational requirements are fully acceptable for the approved deployment architecture.

---

# Preliminary Conclusion

Docker completely satisfies the packaging and deployment requirements of MachineryManagerEnterprise.

It is approved as the standard containerization platform for application packaging, infrastructure consistency and enterprise deployment.

---


# 10. .NET Aspire Evaluation

## Overview

.NET Aspire is Microsoft's opinionated framework for building, orchestrating and operating modern distributed .NET applications.

It provides a unified developer experience for managing multiple services, infrastructure resources and application dependencies during development and deployment.

Within MachineryManagerEnterprise, Aspire is evaluated as a potential orchestration platform rather than as an application framework.

---

# Architectural Role

```text
                Aspire AppHost

                      │

      ┌───────────────┼────────────────┐

      ▼               ▼                ▼

 Application      SQL Server       RabbitMQ

      ▼               ▼                ▼

     Redis         Qdrant         Observability
```

Aspire coordinates distributed resources but does not replace application architecture.

---

# Architectural Strengths

Advantages include:

- Unified local orchestration
- Strong Microsoft ecosystem integration
- Distributed application model
- Built-in service discovery
- Environment configuration
- Resource dependency management
- Developer productivity
- Modern diagnostics

---

# Functional Capabilities

Aspire supports:

- Local orchestration
- Service registration
- Resource provisioning
- Configuration management
- Health monitoring
- Distributed diagnostics
- OpenTelemetry integration
- Dashboard support

---

# Development Experience

Aspire significantly improves the local developer experience.

Typical workflow:

```text
Start AppHost

      │

Automatically Start

      │

SQL Server

RabbitMQ

Redis

Qdrant

Application

      │

Ready
```

Developers no longer need to manually start every dependency.

---

# Distributed Application Support

Aspire is particularly valuable for applications containing:

- Multiple services
- Infrastructure resources
- Distributed communication
- Event-driven components

MachineryManagerEnterprise already contains:

- SQL Server
- RabbitMQ
- Redis
- Qdrant
- Background Services
- Multiple Business Modules

This aligns well with Aspire's intended usage.

---

# Observability

Aspire provides built-in support for:

- OpenTelemetry
- Distributed Tracing
- Structured Logging
- Metrics
- Health Checks
- Central Dashboard

Observability is considered **Excellent**.

---

# Configuration Management

Aspire centralizes configuration for:

- Connection Strings
- Service Discovery
- Environment Variables
- Resource Dependencies

Configuration complexity is significantly reduced.

---

# CI/CD Compatibility

Aspire integrates well with:

- GitHub Actions
- Azure DevOps
- Docker
- Kubernetes (future)
- Azure Container Apps

Pipeline compatibility is considered **Excellent**.

---

# Cross-Platform Support

Supported platforms include:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

---

# Performance

Aspire introduces minimal runtime overhead because its primary responsibilities are orchestration and configuration.

Performance impact is considered **Negligible**.

---

# Operational Characteristics

Aspire primarily targets:

- local development;
- integration environments;
- cloud-native orchestration.

Production deployment remains compatible with standard Docker-based infrastructure.

---

# Enterprise Suitability

Aspire is appropriate for:

- Enterprise distributed applications
- Event-driven architectures
- Hybrid deployments
- Modular systems
- Developer productivity

---

# Limitations

Current limitations include:

- Additional orchestration layer
- Relatively new Microsoft technology
- Best suited to distributed systems
- Requires developer familiarity

These limitations are acceptable given the architecture of MachineryManagerEnterprise.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| Developer Productivity | Excellent |
| Distributed Application Support | Excellent |
| Observability | Excellent |
| CI/CD Compatibility | Excellent |
| Cross Platform | Excellent |
| Performance | Excellent |
| Long-Term Viability | Excellent |

---

# Advantages

- Native Microsoft solution
- Excellent developer experience
- Unified orchestration
- Strong observability
- Simplified local infrastructure management

---

# Disadvantages

- Additional orchestration abstraction
- Learning curve for development teams
- Not required for very small applications

---

# Preliminary Conclusion

MachineryManagerEnterprise is a distributed enterprise application with multiple infrastructure dependencies.

.NET Aspire aligns exceptionally well with this architecture.

It is therefore approved as the preferred orchestration platform for local development and distributed application management.

---


# 11. GitHub Actions Evaluation

## Overview

GitHub Actions is GitHub's native Continuous Integration and Continuous Delivery (CI/CD) platform.

It enables automated execution of:

- Build
- Test
- Packaging
- Security Validation
- Artifact Publishing
- Deployment

directly from the source repository.

Within MachineryManagerEnterprise, GitHub Actions is evaluated as the primary CI platform.

---

# Architectural Role

```text
            Git Push / Pull Request

                     │

                     ▼

              GitHub Actions

        ┌──────────────────────────┐

        │ Restore                  │
        │ Build                    │
        │ Test                     │
        │ Package                  │
        │ Publish                  │
        │ Release                  │
        └──────────────────────────┘

                     │

                     ▼

              Deployment Artifacts
```

GitHub Actions becomes the automation engine responsible for validating every code change before it reaches production.

---

# Architectural Strengths

Advantages include:

- Native GitHub integration
- Event-driven pipelines
- YAML-based workflow definition
- Cross-platform runners
- Marketplace ecosystem
- Secret management
- Matrix builds
- Artifact management

---

# Functional Capabilities

GitHub Actions supports:

- Continuous Integration
- Continuous Delivery
- Pull Request Validation
- Scheduled Jobs
- Manual Workflow Execution
- Artifact Upload
- Package Publishing
- Container Image Build
- Environment Deployment

---

# Workflow Integration

Typical execution flow:

```text
Pull Request

      │

      ▼

Restore

      │

Build

      │

Unit Tests

      │

Integration Tests

      │

Package

      │

Publish Artifact
```

Each workflow executes automatically based on repository events.

---

# Runner Support

Supported execution environments include:

| Runner | Support |
|---------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |
| Self-Hosted | ✅ |

This allows pipelines to execute consistently across development and production environments.

---

# Security

GitHub Actions provides:

- Repository Secrets
- Environment Secrets
- Protected Environments
- Branch Protection Integration
- OIDC Authentication
- Signed Workflow Execution

Security is considered **Excellent**.

---

# CI/CD Compatibility

GitHub Actions integrates directly with:

- Docker
- Azure Container Registry
- GitHub Container Registry
- NuGet
- .NET CLI
- Testcontainers
- Playwright

Pipeline compatibility is considered **Excellent**.

---

# Developer Experience

Advantages include:

- Workflow as Code
- Integrated Repository UI
- Rich Marketplace
- Excellent Documentation
- Large Community

Developer experience is considered **Excellent**.

---

# Enterprise Suitability

GitHub Actions is appropriate for:

- Enterprise CI
- Automated Validation
- Release Automation
- Package Publishing
- Container Image Creation
- Infrastructure Automation

---

# Performance

GitHub Actions provides:

- Parallel Jobs
- Matrix Builds
- Incremental Execution
- Build Caching
- Dependency Caching

Performance is considered **Excellent**.

---

# Operational Characteristics

Operational effort is minimal.

GitHub manages:

- Hosted runners
- Workflow scheduling
- Pipeline execution
- Log storage
- Artifact storage

Operational complexity is considered **Very Low**.

---

# Advantages

- Native GitHub integration
- Excellent .NET support
- Rich automation ecosystem
- Workflow versioning
- Strong community adoption

---

# Disadvantages

- Hosted runner limits depending on licensing
- Heavy enterprise governance may require self-hosted runners

These limitations are operational rather than architectural.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| CI/CD Compatibility | Excellent |
| Developer Productivity | Excellent |
| Cross Platform | Excellent |
| Performance | Excellent |
| Security | Excellent |
| Documentation | Excellent |
| Long-Term Viability | Excellent |

---

# Preliminary Conclusion

GitHub Actions completely satisfies the Continuous Integration requirements of MachineryManagerEnterprise.

It is approved as the preferred CI automation platform for build validation, automated testing, artifact generation and release preparation.

---


# 12. Azure DevOps Evaluation

## Overview

Azure DevOps is Microsoft's enterprise DevOps platform providing integrated services for:

- Source Control
- Continuous Integration
- Continuous Delivery
- Work Item Management
- Test Management
- Artifact Management
- Release Management

Unlike GitHub Actions, Azure DevOps is a complete Application Lifecycle Management (ALM) platform rather than a standalone CI/CD engine.

Within MachineryManagerEnterprise, Azure DevOps is evaluated as an enterprise DevOps alternative.

---

# Architectural Role

```text
              Azure DevOps

      ┌──────────────────────────┐

      │ Azure Repos              │
      │ Azure Pipelines          │
      │ Azure Artifacts          │
      │ Azure Boards             │
      │ Azure Test Plans         │
      └──────────────────────────┘

                 │

                 ▼

         Build / Test / Deploy
```

Azure DevOps provides an end-to-end enterprise software delivery platform.

---

# Architectural Strengths

Advantages include:

- Enterprise ALM platform
- Advanced pipeline capabilities
- Rich release management
- Strong governance
- Enterprise security
- Fine-grained permissions
- Microsoft ecosystem integration
- Mature enterprise tooling

---

# Functional Capabilities

Azure DevOps supports:

- CI/CD Pipelines
- Multi-stage Deployments
- Release Pipelines
- Test Management
- Artifact Management
- Approval Workflows
- Environment Management
- Infrastructure Deployment

---

# Pipeline Capabilities

Azure DevOps supports:

```text
Restore

   │

Build

   │

Test

   │

Package

   │

Release

   │

Production
```

The platform offers advanced deployment orchestration with approvals and release gates.

---

# Enterprise Governance

Azure DevOps provides:

- Branch Policies
- Approval Gates
- Environment Protection
- Audit Trails
- Enterprise RBAC
- Release Permissions

Governance is considered **Excellent**.

---

# Security

Azure DevOps supports:

- Azure Active Directory
- Managed Identities
- Secret Variables
- Secure Files
- Variable Groups
- Environment Isolation

Security is considered **Excellent**.

---

# CI/CD Compatibility

Azure DevOps integrates directly with:

- .NET SDK
- Docker
- Aspire
- Azure
- Kubernetes
- SQL Server
- GitHub

Compatibility is considered **Excellent**.

---

# Developer Experience

Advantages include:

- Rich pipeline editor
- YAML pipelines
- Classic pipelines
- Enterprise dashboards
- Extensive Microsoft documentation

Developer experience is considered **Excellent**.

---

# Operational Characteristics

Azure DevOps provides:

- Hosted agents
- Self-hosted agents
- Pipeline caching
- Parallel execution
- Enterprise monitoring

Operational complexity is considered **Low**.

---

# Performance

Azure DevOps provides:

- Parallel agents
- Incremental execution
- Artifact caching
- Distributed execution

Performance is considered **Excellent**.

---

# Enterprise Suitability

Azure DevOps is appropriate for:

- Large enterprises
- Regulated environments
- Complex release processes
- Large development teams
- Multi-stage deployments

---

# Comparison with GitHub Actions

| Criterion | GitHub Actions | Azure DevOps |
|-----------|:--------------:|:------------:|
| CI/CD | Excellent | Excellent |
| ALM Features | Limited | Excellent |
| Work Item Management | No | Yes |
| Test Plans | No | Yes |
| Release Management | Good | Excellent |
| Repository Integration | GitHub Native | Azure Native |
| Enterprise Governance | Very Good | Excellent |

---

# Advantages

- Complete ALM platform
- Advanced governance
- Enterprise release management
- Excellent Microsoft integration
- Mature enterprise capabilities

---

# Disadvantages

- Higher administrative complexity
- Broader platform than required
- Additional licensing considerations
- Steeper learning curve than GitHub Actions

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| CI/CD Compatibility | Excellent |
| Enterprise Governance | Excellent |
| Security | Excellent |
| Performance | Excellent |
| Operational Simplicity | Very Good |
| Documentation | Excellent |
| Long-Term Viability | Excellent |

---

# Preliminary Conclusion

Azure DevOps is an outstanding enterprise DevOps platform.

However, MachineryManagerEnterprise uses GitHub as its primary source control platform.

Because GitHub Actions already provides excellent CI/CD capabilities with significantly lower operational complexity, Azure DevOps is **not recommended as the primary CI/CD platform** for this project.

Azure DevOps remains a fully supported enterprise alternative for organizations already standardized on the Azure DevOps ecosystem.

---

# 13. Overall Technology Comparison

Following the detailed evaluation of all candidate technologies, the Architecture Review Board compared the complete build, packaging and deployment stack against the architectural objectives of MachineryManagerEnterprise.

---

# Technology Stack Overview

| Responsibility | Selected Technology |
|---------------|---------------------|
| Build Platform | .NET 10 SDK |
| Containerization | Docker |
| Local Distributed Orchestration | .NET Aspire |
| Continuous Integration | GitHub Actions |
| Enterprise ALM Alternative | Azure DevOps |

Together these technologies provide a complete enterprise DevOps platform.

---

# Technology Comparison Matrix

| Criterion | .NET 10 SDK | Docker | Aspire | GitHub Actions | Azure DevOps |
|-----------|:-----------:|:------:|:------:|:--------------:|:------------:|
| Enterprise Readiness | Excellent | Excellent | Excellent | Excellent | Excellent |
| Build Capability | Excellent | Good | Fair | Excellent | Excellent |
| CI/CD Compatibility | Excellent | Excellent | Good | Excellent | Excellent |
| Cross Platform | Excellent | Excellent | Excellent | Excellent | Excellent |
| Developer Productivity | Excellent | Good | Excellent | Excellent | Good |
| Deployment Flexibility | Good | Excellent | Excellent | Good | Excellent |
| Maintainability | Excellent | Excellent | Excellent | Excellent | Excellent |
| Operational Simplicity | Excellent | Good | Good | Excellent | Good |
| Documentation | Excellent | Excellent | Good | Excellent | Excellent |
| Long-Term Viability | Excellent | Excellent | Excellent | Excellent | Excellent |

---

# Responsibility Separation

```text
        Build

      .NET SDK

          │

          ▼

    Containerization

        Docker

          │

          ▼

 Local Orchestration

      .NET Aspire

          │

          ▼

 Continuous Integration

   GitHub Actions

          │

          ▼

 Enterprise ALM

    Azure DevOps
```

Each technology has a clearly defined responsibility with minimal overlap.

---

# Deployment Coverage

| Capability | Technology |
|------------|------------|
| Compilation | .NET 10 SDK |
| Packaging | Docker |
| Local Distributed Execution | Aspire |
| CI Automation | GitHub Actions |
| Enterprise Release Management | Azure DevOps |

---

# Cross-Platform Support

All selected technologies support:

- Windows
- Linux
- macOS

This satisfies the project's cross-platform development requirements.

---

# Enterprise Characteristics

| Requirement | Coverage |
|-------------|----------|
| Reproducible Builds | Complete |
| Container Deployment | Complete |
| Local Infrastructure | Complete |
| Automated Validation | Complete |
| Hybrid Deployment | Complete |
| Enterprise Governance | Complete |

---

# Microsoft Ecosystem Alignment

The selected stack integrates naturally with:

- .NET 10
- Visual Studio
- Azure
- Docker
- GitHub
- Azure DevOps

This minimizes integration complexity while maximizing long-term support.

---

# Operational Complexity

```text
Lowest Complexity

.NET SDK

↓

GitHub Actions

↓

Docker

↓

Aspire

↓

Azure DevOps

Highest Complexity
```

Although Azure DevOps offers the richest enterprise feature set, it also introduces the highest administrative complexity.

---

# Architectural Assessment

The selected build platform fully supports the approved architecture by providing:

- deterministic builds;
- portable deployment artifacts;
- automated validation;
- enterprise scalability;
- long-term maintainability.

No additional build technologies are required.

---

# 14. Final Recommendation

The Architecture Review Board recommends adoption of the following enterprise build and deployment stack.

| Category | Approved Technology |
|----------|---------------------|
| Build Platform | **.NET 10 SDK** |
| Containerization | **Docker** |
| Local Orchestration | **.NET Aspire** |
| Continuous Integration | **GitHub Actions** |
| Enterprise ALM | **Azure DevOps (Optional Enterprise Alternative)** |

---

# Overall Technology Comparison

The selected technology provides optimal performance, maintainability, and Clean Architecture compatibility.

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative |
|-----------------|------------------------|-------------|
| System Capability | Primary Selected | Evaluated Option |

---

# Final Recommendation Statement

The recommended implementation strategy is:

- .NET 10 SDK for all build activities.
- Docker for packaging and deployment consistency.
- .NET Aspire for local distributed application orchestration.
- GitHub Actions as the primary CI platform.
- Azure DevOps supported only where enterprise customers require an integrated ALM platform.

This combination provides:

- excellent developer productivity;
- reproducible builds;
- consistent deployment;
- strong Microsoft ecosystem alignment;
- enterprise scalability.

---

# 15. Final Decision

## Approved Architecture

```text
Source Code

      │

.NET 10 SDK

      │

Docker

      │

Aspire

      │

GitHub Actions

      │

Deployment
```

---

## Technology Decisions

| Technology | Decision | Status |
|------------|----------|--------|
| .NET 10 SDK | Approved | ✅ |
| Docker | Approved | ✅ |
| .NET Aspire | Approved | ✅ |
| GitHub Actions | Approved | ✅ |
| Azure DevOps | Supported Alternative | ✅ |

---

## Implementation Strategy

Phase 1

- .NET 10 SDK
- Docker
- GitHub Actions

Phase 2

- .NET Aspire

Phase 3

- Optional Azure DevOps support for enterprise customers

---

## Consequences

Positive

- Reproducible builds
- Consistent deployment
- Excellent CI automation
- Hybrid deployment readiness
- Strong Microsoft alignment

Negative

- Docker runtime requirement
- Additional orchestration layer (Aspire)
- Optional operational complexity if Azure DevOps is adopted

---

## Related Architecture Decision

Implementation of this Technology Evaluation requires:

- **ADR-0025 — Build & Deployment Architecture**

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

- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md
---

# 16. Revision History

| Version | Date       | Author             | Description                                                       |
|---------|------------|--------------------|-------------------------------------------------------------------|
| 1.0.0   | 2026-07-28 | Solution Architect | Initial technology evaluation for Build, Packaging and Deployment |
| 1.1.0   | 2026-07-28 | Solution Architect | Removed stray duplicate title line; converted star-rating tables to text ratings for consistency |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0                         |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes                    |