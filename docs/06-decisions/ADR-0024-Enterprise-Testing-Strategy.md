# Enterprise Testing Strategy

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0024 |
| **Version** | 1.1.0 |
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

# Revision History

| Version | Date | Author | Description |
|----------|------|--------|-------------|
| 1.0.0 | 2026-07-28 | Solution Architect | Initial version |
| 1.1.0 | 2026-07-28 | Solution Architect | Header reformatted to comply with the official Standard Document Header in DOCUMENT_CONVENTIONS.md |