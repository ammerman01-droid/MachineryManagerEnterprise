# Technology Evaluation — Entity Framework Core

| Property | Value |
|----------|-------|
| **Document ID** | TE-0004 |
| **Version** | 3.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Executive Summary

This document evaluates **Entity Framework Core** as the primary Object–Relational Mapper (ORM) for the MachineryManagerEnterprise solution.

Entity Framework Core was selected because it provides excellent integration with .NET 10, strong tooling, mature migration support, high maintainability, and an active open-source ecosystem while remaining flexible enough for enterprise applications.

---

# Problem Statement

The project requires an ORM that provides:

- High productivity
- Strong .NET integration
- Reliable migrations
- LINQ support
- Provider independence
- Excellent tooling
- Long-term maintainability

---

# Evaluation Scope

Object–Relational Mapping

Database Provider Support

Migration System

Query Translation

Change Tracking

---

# Candidate Technologies

| Technology | Status |
|------------|--------|
| Entity Framework Core | Selected |
| Dapper | Evaluated |
| NHibernate | Evaluated |
| Linq2Db | Evaluated |

---

# Evaluation Criteria

The evaluation considered:

- Open Source
- Performance
- Productivity
- Maintainability
- Documentation
- Community
- Migration Support
- Provider Support
- Tooling
- Testability

---

# Comparison Matrix

| Criteria | EF Core | Dapper | NHibernate | Linq2Db |
|----------|---------|---------|-------------|----------|
| Open Source | Excellent | Excellent | Excellent | Excellent |
| Productivity | Excellent | Moderate | Moderate | Good |
| Performance | Very Good | Excellent | Good | Excellent |
| LINQ Support | Excellent | Limited | Good | Excellent |
| Migrations | Excellent | Manual | Good | Limited |
| Tooling | Excellent | Minimal | Moderate | Moderate |
| Community | Excellent | Excellent | Good | Good |

---

# Advantages

- Native .NET integration
- Excellent LINQ support
- Automatic change tracking
- Mature migration framework
- Provider independence
- Excellent Visual Studio tooling
- Strong documentation
- Large community
- First-party Microsoft support

---

# Disadvantages

- Slightly slower than micro ORMs for simple queries.
- Complex LINQ expressions may generate inefficient SQL if not reviewed.

---

# Risks

Potential risks include:

- Performance degradation caused by incorrect query design.
- Excessive eager loading.
- N+1 query problems.

These risks can be mitigated through code reviews and performance testing.

---

# Performance Considerations

Entity Framework Core delivers very good performance for enterprise applications.

Recommended practices:

- Use projections.
- Disable tracking when appropriate.
- Use compiled queries when justified.
- Avoid unnecessary Includes.
- Review generated SQL.

---

# Security Considerations

Entity Framework Core provides:

- Parameterized SQL generation
- SQL Injection protection
- Secure database provider model

Security ultimately depends on application design.

---

# Licensing

License

MIT License

Commercial usage is permitted.

---

# Community & Ecosystem

Entity Framework Core has:

- Active Microsoft development
- Large community
- Excellent documentation
- Frequent releases
- Wide provider support

---

# Proof of Concept

No dedicated proof of concept was required.

The framework is mature and extensively proven in enterprise environments.

---

# Architecture Impact

Entity Framework Core shall exist **only inside the Infrastructure layer**.

The following layers must never reference EF Core packages:

- SharedKernel
- Domain
- Application
- Presentation

Repositories expose abstractions defined by the Application layer.

---

# Migration Complexity

**Difficulty:** Medium

Entity Framework Core can be replaced if:

- Repository interfaces remain stable.
- Business logic remains persistence-agnostic.
- Infrastructure remains isolated.

Migration effort is considered manageable under the current architecture.

---

# Alternatives Considered

## Dapper

Excellent performance.

Rejected because:

- Manual mapping increases maintenance cost.
- No migration framework.
- Less productive for complex domains.

---

## NHibernate

Very mature ORM.

Rejected because:

- Higher learning curve.
- Smaller ecosystem in modern .NET.
- Less alignment with current Microsoft stack.

---

## Linq2Db

Excellent performance.

Rejected because:

- Smaller community.
- Limited migration ecosystem.

---

# Decision

Approved

---

# Decision Rationale

Entity Framework Core provides the best balance of:

- Productivity
- Maintainability
- Tooling
- Ecosystem
- Integration
- Long-term support

It aligns perfectly with the project's Clean Architecture and Open Source First policy.

---

# Related ADR

- ADR-0002 — Open Source First Policy
- ADR-0004 — Use Entity Framework Core

---

# Related Documents

- TE-0001 — .NET 10
- Dependency Catalog
- Dependency Rules

---

# References

https://learn.microsoft.com/ef/core/

https://github.com/dotnet/efcore

https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial evaluation |
| 2.0.0 | 2026-07-18 | Standardized |
| 3.0.0 | 2026-07-18 | Rewritten according to Technology Evaluation Template |