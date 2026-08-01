| Property | Value |
|----------|-------|
| **Document ID** | ADR-0025 |
| **Title** | Build and Deployment Architecture |
| **Version** | 4.0.0 |
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


---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

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
| Property | Value |
|----------|-------|
| **Document ID** | ADR-0024 |
| **Title** | Enterprise Testing Strategy |
| **Version** | 4.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-28 |
| **Last Updated** | 2026-07-28 |

---

# Context

MachineryManagerEnterprise is an enterprise-grade system built using:

- Clean Architecture
- CQRS
- Domain-Driven Design
- Event-Driven Architecture

The project contains multiple bounded contexts, independent modules, infrastructure components and user interfaces.

A unified enterprise testing strategy is therefore required to ensure:

- architectural correctness;
- business correctness;
- infrastructure reliability;
- long-term maintainability.

TE-0030 selected the following testing technologies:

- xUnit v3
- FluentAssertions
- NSubstitute
- Testcontainers
- Playwright

This ADR defines how these technologies are used throughout the system.

---

# Problem

Without a unified testing strategy:

- different modules would implement inconsistent testing approaches;
- architectural regressions would become difficult to detect;
- infrastructure behavior would diverge from production;
- long-term maintenance cost would increase.

---

# Decision Drivers

The testing strategy shall support:

- Clean Architecture
- Fast feedback
- High confidence deployments
- CI/CD automation
- Deterministic execution
- Enterprise scalability
- Long-term maintainability

---

# Decision

MachineryManagerEnterprise adopts a layered testing strategy based upon the **Test Pyramid**.

---

# Test Pyramid

```text
                 End-to-End Tests

                      Playwright

                           ▲

                Integration Tests

                   Testcontainers

                           ▲

                    Unit Tests

xUnit + FluentAssertions + NSubstitute
```

The majority of tests shall remain Unit Tests.

End-to-End tests shall be limited to validating complete business workflows.

---

# Testing Layers

## Unit Tests

Purpose

Validate:

- Domain Logic
- Value Objects
- Entities
- Domain Services
- Application Services
- Validation Rules

Characteristics

- No infrastructure
- No database
- No network
- No file system

Execution Time

Milliseconds

---

## Integration Tests

Purpose

Validate interaction with real infrastructure.

Examples

- SQL Server
- RabbitMQ
- Redis
- Blob Storage

Infrastructure shall be provisioned using Testcontainers.

Characteristics

- Real infrastructure
- Disposable environment
- Deterministic execution

---

## End-to-End Tests

Purpose

Validate complete user scenarios.

Examples

- Authentication
- Business workflows
- Reporting
- UI behavior

Browser automation shall use Playwright.

---

# Approved Technologies

| Responsibility | Technology |
|---------------|------------|
| Test Framework | xUnit v3 |
| Assertions | FluentAssertions |
| Mocking | NSubstitute |
| Infrastructure | Testcontainers |
| Browser Automation | Playwright |

---

# Test Project Structure

```text
tests/

    Unit/

    Integration/

    Architecture/

    UI/

    Performance/
```

Each project shall contain exactly one testing responsibility.

---

# Unit Testing Rules

Unit tests shall:

- execute independently;
- execute in parallel;
- avoid shared state;
- avoid external dependencies;
- remain deterministic.

Unit tests shall never require:

- SQL Server
- RabbitMQ
- Redis
- Docker
- Network access

---

# Mocking Strategy

Mocking shall use NSubstitute.

Only external dependencies may be mocked.

Business logic shall never be mocked.

Examples

Allowed

- Repository
- External Service
- Clock
- Email Service

Not Allowed

- Entity
- Value Object
- Domain Logic

---

# Assertion Strategy

Assertions shall use FluentAssertions.

Assertions should emphasize readability.

Example

```csharp
result.Should().Be(expected);
```

rather than

```csharp
Assert.Equal(expected, result);
```

---

# Integration Testing Strategy

Integration tests shall validate:

- Repository implementations
- EF Core mappings
- SQL queries
- Message bus integration
- Infrastructure configuration

All infrastructure shall be created using Testcontainers.

No shared databases shall be used.

---

# Test Isolation

Every integration test execution shall create its own infrastructure.

```text
Test Start

      │

Create Containers

      │

Execute Tests

      │

Dispose Containers

      │

Clean Environment
```

---

# Architecture Tests

Architecture tests shall validate:

- Dependency Rules
- Clean Architecture boundaries
- Layer isolation
- Forbidden references

These tests execute as ordinary xUnit tests.

---

# End-to-End Strategy

Playwright shall validate only:

- complete business workflows;
- browser interactions;
- authentication scenarios;
- regression scenarios.

Business rules shall continue to be verified primarily through Unit Tests.

---

# Code Coverage

Coverage goals

| Layer | Target |
|-------|-------:|
| Domain | ≥95% |
| Application | ≥90% |
| Infrastructure | ≥80% |
| UI | Scenario Based |

Coverage targets are quality indicators rather than absolute acceptance criteria.

---

# Continuous Integration

Every Pull Request shall execute:

1. Unit Tests
2. Architecture Tests
3. Integration Tests

End-to-End tests may execute separately depending on pipeline configuration.

---

# Test Naming

Test names shall follow:

```text
MethodName_StateUnderTest_ExpectedBehavior
```

Example

```text
CreateOrder_InvalidCustomer_ShouldThrowValidationException
```

---

# Performance

Tests shall execute in parallel whenever possible.

Infrastructure shall be reused at fixture level where safe.

Long-running tests shall be minimized.

---

# Benefits

This strategy provides:

- Fast developer feedback
- Reliable deployments
- High confidence refactoring
- Consistent testing approach
- Excellent maintainability
- Enterprise scalability

---

# Consequences

Positive

- Consistent quality assurance
- Predictable testing
- High architectural confidence
- Reduced regression risk

Negative

- Additional integration infrastructure
- Slightly longer CI execution
- Increased initial implementation effort

---

# Alternatives Considered

## Unit Tests Only

Rejected.

Insufficient confidence in infrastructure behavior.

---

## Manual Integration Testing

Rejected.

Not repeatable.

---

## Shared Test Database

Rejected.

Creates nondeterministic behavior and cross-test interference.

---

## Selenium

Rejected.

Playwright provides better performance, reliability and .NET integration.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Clean Architecture
- ADR-0002 — CQRS Architecture
- TE-0030 — Testing Technology Evaluation

---

# Decision Outcome

**Accepted**

MachineryManagerEnterprise adopts an enterprise testing strategy based upon:

- xUnit v3
- FluentAssertions
- NSubstitute
- Testcontainers
- Playwright

organized according to the Test Pyramid and executed through automated CI/CD pipelines.

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-28 | Solution Architect | Initial version                                       |
| 1.1.0   | 2026-07-28 | Solution Architect | Header reformatted to comply with the official Standard Document Header in DOCUMENT_CONVENTIONS.md |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |