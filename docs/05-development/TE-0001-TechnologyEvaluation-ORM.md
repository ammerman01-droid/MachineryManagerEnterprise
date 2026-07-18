# Technology Evaluation — ORM Selection

**Document ID**

TE-0001

---

# Purpose

This document evaluates the available ORM technologies for
MachineryManagerEnterprise.

The goal is selecting an ORM that best satisfies:

- Clean Architecture
- Domain Driven Design
- Long-term Maintainability
- SQL Server
- .NET 10
- Enterprise Scalability

---

# Candidate Technologies

| ORM | Status |
|------|---------|
| Entity Framework Core | Evaluated |
| Dapper | Evaluated |
| NHibernate | Evaluated |
| Linq2Db | Evaluated |

---

# Evaluation Criteria

The following criteria were used.

| Criterion | Weight |
|------------|-------:|
| .NET 10 Compatibility | High |
| Community | High |
| Documentation | High |
| Enterprise Adoption | High |
| Performance | Medium |
| DDD Support | High |
| Migrations | High |
| LINQ Support | Medium |
| Tooling | High |
| Learning Curve | Medium |
| Future Outlook | High |

---

# Comparison

| Feature | EF Core | Dapper | NHibernate | Linq2Db |
|----------|:------:|:------:|:-----------:|:--------:|
| Microsoft Supported | ✅ | ❌ | ❌ | ❌ |
| .NET 10 Ready | ✅ | ✅ | ⚠ | ✅ |
| SQL Server | ✅ | ✅ | ✅ | ✅ |
| LINQ | ✅ | ❌ | ✅ | ✅ |
| Change Tracking | ✅ | ❌ | ✅ | ❌ |
| Migrations | ✅ | ❌ | ✅ | ❌ |
| DDD Support | ✅ | ◐ | ✅ | ◐ |
| Transactions | ✅ | ✅ | ✅ | ✅ |
| Dependency Injection | ✅ | ✅ | ◐ | ✅ |
| Performance | Good | Excellent | Good | Excellent |
| Tooling | Excellent | Minimal | Medium | Medium |
| Community | Very Large | Very Large | Medium | Medium |
| Learning Curve | Medium | Low | High | Medium |
| Future Outlook | Excellent | Excellent | Moderate | Good |

---

# Individual Analysis

## Entity Framework Core

### Advantages

- Official Microsoft ORM
- Native .NET support
- Excellent tooling
- Strong SQL Server support
- LINQ
- Migrations
- Value Objects
- Owned Types
- Optimistic Concurrency
- Long-term support

### Disadvantages

- Slightly slower than Dapper for micro queries
- Requires understanding Change Tracker

---

## Dapper

### Advantages

- Extremely fast
- Very lightweight
- Excellent raw SQL support

### Disadvantages

- No Migrations
- No Change Tracking
- Manual mapping
- Larger Repository implementations

---

## NHibernate

### Advantages

- Mature
- Powerful ORM
- Rich mapping capabilities

### Disadvantages

- Smaller ecosystem
- Higher complexity
- Reduced community momentum

---

## Linq2Db

### Advantages

- Lightweight
- Fast
- Strong LINQ support

### Disadvantages

- No migration framework
- Smaller ecosystem
- Less enterprise adoption

---

# Risk Analysis

| ORM | Risk |
|------|------|
| EF Core | Low |
| Dapper | Medium |
| NHibernate | Medium |
| Linq2Db | Medium |

---

# Final Decision

Entity Framework Core achieves the highest overall score.

Reasons:

- Official Microsoft support
- Best tooling
- Lowest long-term maintenance cost
- Strong DDD compatibility
- Excellent SQL Server integration
- Long-term roadmap

---

# Future Strategy

If future performance analysis identifies bottlenecks,
Dapper may be introduced only for specialized read-only queries.

Entity Framework Core remains the primary ORM.

---

# Related Documents

ADR-0004 — Use EntityFrameworkCore