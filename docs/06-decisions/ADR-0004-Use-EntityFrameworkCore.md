# ADR-0004 — Use Entity Framework Core as ORM

**Status:** Accepted

**Date:** 2026-07-18

**Decision Makers**

- Solution Architect
- Development Team

---

# Context

MachineryManagerEnterprise requires a production-ready persistence framework capable of supporting:

- SQL Server
- Clean Architecture
- Domain-Driven Design
- Repository Pattern
- Unit of Work
- LINQ
- Transactions
- Migrations
- Optimistic Concurrency
- High Maintainability

The persistence technology must integrate naturally with .NET 10.

---

# Problem

The application requires an ORM capable of:

- Mapping Aggregate Roots
- Supporting Value Objects
- Supporting Owned Types
- Managing Migrations
- Supporting Transactions
- Handling Optimistic Concurrency
- Supporting Dependency Injection
- Remaining maintainable over many years

---

# Considered Options

## Option 1

### Raw ADO.NET

Advantages

- Maximum performance
- Complete SQL control

Disadvantages

- Very high maintenance cost
- Large amount of boilerplate
- Difficult testing
- Poor productivity

---

## Option 2

### Dapper

Advantages

- Very fast
- Lightweight
- Excellent SQL control

Disadvantages

- No Change Tracking
- No Migrations
- Manual relationship management
- Repository implementation becomes larger

---

## Option 3

### NHibernate

Advantages

- Mature
- Rich feature set

Disadvantages

- Smaller ecosystem
- Less community activity
- Higher learning curve

---

## Option 4

### Entity Framework Core

Advantages

- Official Microsoft ORM
- Native .NET 10 support
- Excellent tooling
- LINQ support
- Migrations
- Transactions
- Owned Types
- Value Converters
- Optimistic Concurrency
- Strong community
- Long-term support

Disadvantages

- Slightly slower than Dapper for micro-queries
- Requires understanding of Change Tracker

---

# Decision

The project adopts **Entity Framework Core** as the primary ORM.

Entity Framework Core shall be responsible only for persistence.

Business rules shall remain inside the Domain Model.

---

# Architectural Rules

## Domain

Domain shall never reference Entity Framework Core.

No Entity Framework attributes shall exist inside Domain Entities.

---

## Infrastructure

DbContext shall exist only inside Infrastructure.

---

## Application

Application shall never depend directly on DbContext.

Persistence shall be accessed through abstractions.

---

## Repositories

Repositories shall expose domain-oriented operations.

Repositories shall not become generic CRUD wrappers.

---

## Migrations

All migrations shall reside in Infrastructure.

Migration history shall be version controlled.

---

## Transactions

Transactions shall be coordinated through Unit of Work.

---

## Lazy Loading

Lazy Loading is prohibited.

Explicit loading or projection shall be preferred.

---

## Tracking

Read-only queries should use:

```
AsNoTracking()
```

when change tracking is unnecessary.

---

## Concurrency

Optimistic concurrency shall be used where required.

---

# Consequences

## Positive

- Official Microsoft support
- Excellent tooling
- Maintainable infrastructure
- Strong DDD support
- Long-term compatibility
- Excellent developer productivity

---

## Negative

- Additional abstraction
- Developers must understand Change Tracker behavior

---

# Constraints

DbContext shall never leak outside Infrastructure.

Presentation shall never reference Entity Framework Core.

---

# Future Considerations

If future performance profiling identifies critical bottlenecks, Dapper may be introduced for selected read-only queries.

Entity Framework Core shall remain the primary persistence mechanism.

---

# Related Decisions

- ADR-0002 — Use FluentValidation
- ADR-0003 — Use MediatR
- ADR-0005 — Use Serilog (Planned)

---

# References

- Microsoft Entity Framework Core Documentation
- Domain-Driven Design
- Clean Architecture