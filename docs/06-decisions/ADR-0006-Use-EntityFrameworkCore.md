# Architecture Decision Record

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0006 |
| **Version** | 3.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Title

Use Entity Framework Core

---

# Status

Accepted

---

# Context

The MachineryManagerEnterprise solution requires a modern Object Relational
Mapper (ORM) capable of supporting enterprise-grade applications while
maintaining architectural separation and long-term maintainability.

The selected ORM should provide:

- High performance
- Strong .NET integration
- LINQ support
- Migration management
- Transaction support
- Active development
- Excellent tooling

The ORM must integrate naturally with Clean Architecture.

---

# Decision

The Infrastructure Layer shall use **Entity Framework Core** as the primary ORM.

All relational database access shall be implemented through Entity Framework
Core.

Application and Domain layers shall remain independent from EF Core.

---

# Decision Drivers

- Native .NET support
- Performance
- Maintainability
- Mature ecosystem
- Migration support
- LINQ
- Strong tooling
- Community support

---

# Alternatives Considered

## Dapper

Rejected because although it provides excellent performance, it lacks built-in
change tracking, migrations, and higher-level ORM capabilities required for this
solution.

---

## NHibernate

Rejected because of higher complexity and lower adoption within modern .NET
applications.

---

## Linq2Db

Rejected because the ecosystem and community are smaller than EF Core.

---

# Consequences

## Positive

- Unified persistence model
- Excellent tooling
- Strong LINQ support
- Built-in migrations
- High maintainability
- Excellent documentation

## Negative

- Slightly higher abstraction overhead than micro ORMs.
- Developers must understand change tracking and DbContext lifetime.

---

# Architecture Impact

Entity Framework Core shall exist only inside the **Infrastructure Layer**.

The Domain Layer shall never reference EF Core.

The Application Layer shall access persistence only through repository or unit
of work abstractions.

Infrastructure implements those abstractions.

---

# Implementation Notes

Infrastructure shall contain:

- DbContext
- Entity configurations
- Repository implementations
- Migrations

Application defines repository interfaces.

Domain remains persistence-ignorant.

---

# Compliance Rules

1. Entity Framework Core shall only exist inside Infrastructure.

2. Domain shall never reference Microsoft.EntityFrameworkCore.

3. Application shall never reference Microsoft.EntityFrameworkCore.

4. DbContext shall never be injected into Domain.

5. Migrations shall be maintained only inside Infrastructure.

6. Repository interfaces belong to Application.

7. Repository implementations belong to Infrastructure.

8. Persistence logic shall never exist inside Presentation.

---

# Related Technology Evaluation

TE-0004 — Entity Framework Core

---

# Related Proof of Concept

Not Required

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0003 — Use .NET 10
- Dependency Catalog

---

# References

https://learn.microsoft.com/ef/core/

https://github.com/dotnet/efcore

https://www.nuget.org/packages/Microsoft.EntityFrameworkCore

---

# Review

| Role | Name | Date |
|------|------|------|
| Solution Architect | | |

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial decision |
| 2.0.0 | 2026-07-18 | Standardized |
| 3.0.0 | 2026-07-18 | Rewritten according to ADR Template v3.0 |