| Property | Value |
|----------|-------|
| **Document ID** | TE-0024 |
| **Title** | Data Access Technology Evaluation |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-27 |
| **Last Updated** | 2026-07-28 |

# Purpose

This document evaluates the **data access strategy** used across the
MachineryManagerEnterprise solution, building on the ORM selection already
made in TE-0004 / ADR-0006.

TE-0004 answered a narrower question — "which ORM should the platform use?"
— and selected Entity Framework Core. This evaluation answers a broader
question: "what is the complete data access strategy for both the
transactional (write) side and the reporting/query-heavy (read) side of the
platform?" As the platform grows to include fleet-wide reporting,
maintenance-history dashboards, and Distributed Workspace synchronization
queries, some read paths may benefit from a lighter-weight, more
SQL-explicit approach than EF Core's change-tracked entity model provides.

The objective of this evaluation is to:

- reaffirm Entity Framework Core 10 as the primary data access technology
  for transactional (write-side) operations, consistent with ADR-0006;
- evaluate Dapper as a targeted complement for read-heavy, reporting-style
  queries where EF Core's overhead is unnecessary;
- evaluate a **Hybrid Persistence Strategy** that combines both
  technologies under clearly defined rules, rather than treating this as an
  either/or choice;
- define precisely where each technology is permitted, so that the Hybrid
  strategy does not become an ungoverned free-for-all.

This evaluation does not replace ADR-0006. It extends the approved data
access architecture with an explicit, governed strategy for read-side
query optimization.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Relationship with TE-0004 / ADR-0006

Object–relational mapping was originally evaluated in
**TE-0004 — Entity Framework Core**, and formally approved through
**ADR-0006 — Use Entity Framework Core** (Status: Accepted). TE-0004
evaluated EF Core against Dapper, NHibernate, and Linq2Db as candidate ORMs
and rejected Dapper specifically **as a full ORM replacement**, citing
manual mapping overhead, absence of a migration framework, and reduced
productivity for complex domains.

This evaluation does not reopen that decision. It treats EF Core as the
**Incumbent** and mandatory technology for all write-side, transactional,
and change-tracked persistence. It evaluates Dapper only in the narrower,
complementary role of read-side query execution — a role TE-0004 did not
evaluate, since TE-0004's scope was ORM selection, not a full data access
strategy.

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0002 — Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- ADR-0008 — Use Mapster
- TE-0004 — Entity Framework Core (original ORM evaluation)
- TE-0023 — Object Mapping Technology Evaluation
- SolutionStructure.md
- DependencyRules.md
- CodingStandards.md

---

# Scope

This evaluation covers:

- write-side (command) persistence through the repository abstractions
  defined by the Application layer;
- read-side (query) data retrieval for list views, dashboards, and
  reporting scenarios;
- the boundary rules governing when Dapper may be used instead of EF Core.

Out of scope:

- ORM selection itself (already decided — ADR-0006).
- Database migration tooling — covered separately by the forthcoming
  TE-0025 (Database Migration Technology Evaluation).
- Full reporting/analytics infrastructure (e.g. a dedicated read model
  store or OLAP layer) — not currently in scope for the platform.

---

# Functional Requirements

The data access strategy shall support:

- transactional writes with full change tracking, optimistic concurrency,
  and unit-of-work semantics for Aggregates;
- efficient read-side projections for list views, dashboards, and reports,
  including projections spanning multiple joined tables;
- LINQ-based querying for the majority of the codebase, to preserve
  productivity and compile-time type safety;
- an explicit, narrow escape hatch for hand-written SQL where EF Core's
  generated SQL is measurably insufficient for a specific reporting query.

---

# Non-Functional Requirements

The strategy should provide:

- predictable, reviewable SQL for high-volume reporting queries (e.g.
  fleet-wide utilization reports across thousands of Assets and Meter
  Readings);
- clear governance so that introducing a second data access technology
  does not erode Clean Architecture boundaries or create two incompatible
  data access idioms across modules;
- minimal additional operational complexity — no new database engine, no
  new connection management model beyond what EF Core already provides;
- compatibility with the existing Infrastructure-layer repository pattern.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| Entity Framework Core 10 | Full ORM: change tracking, migrations, LINQ, unit of work | **Incumbent** (ADR-0006) |
| Dapper | Micro-ORM: hand-written SQL with lightweight object mapping | Evaluated |
| Hybrid Persistence Strategy | EF Core for writes, Dapper for targeted read-heavy queries | Evaluated |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Open Source & License Stability | Critical |
| A2 | .NET 10 Compatibility | Critical |
| A3 | Clean Architecture Compatibility | Critical |
| A4 | Write-Side Productivity (Change Tracking, Unit of Work) | Critical |
| A5 | Read-Side Query Performance at Scale | High |
| A6 | Governance / Boundary Clarity | High |
| A7 | Developer Experience | High |
| A8 | Maintainability | High |
| A9 | Migration Cost from Current State | Medium |
| A10 | Operational Complexity | Medium |

---

# Architecture Principle

Data access shall remain confined to the Infrastructure layer, behind
repository abstractions defined by the Application layer. The write side
and read side may use different technologies internally, but both must
remain invisible to the Application, Domain, and Presentation layers.

```text
Application Layer
   ICommandRepository<Asset>   IAssetReadRepository
        │                            │
        ▼                            ▼
Infrastructure Layer
   EF Core (DbContext,           Dapper (raw SQL,
   change tracking,              read-only projections)
   unit of work)
        │                            │
        ▼                            ▼
              SQL Server Database
```

Domain entities shall never reference EF Core or Dapper. SharedKernel shall
never reference either. Dapper shall never be used for writes that require
change tracking, concurrency tokens, or domain event dispatch — those
remain EF Core's exclusive responsibility under this architecture.

---

# 5. Entity Framework Core 10 Evaluation (Incumbent)

## Overview

Entity Framework Core 10 is Microsoft's first-party ORM for .NET, already
approved as the platform's primary data access technology under ADR-0006.
This section re-evaluates it specifically in its role as the write-side and
default data access technology within the broader strategy considered here.

## Architectural Role

```text
Application Layer

   UpdateAssetOperatingHoursCommandHandler
          │
          ▼
   IAssetRepository.GetByIdAsync() / SaveChangesAsync()
          │
          ▼
Infrastructure Layer
   AssetRepository : IAssetRepository
          │
          ▼
   AppDbContext (EF Core, change tracking, unit of work)
          │
          ▼
   SQL Server Database
```

## Architectural Strengths

- Full change tracking and unit-of-work support, essential for correctly
  persisting Aggregate invariants and dispatching domain events after
  `SaveChangesAsync()`.
- Mature, first-party migration framework, already governing the schema
  evolution of every module in the solution.
- LINQ-based querying gives compile-time type safety for the majority of
  write-side and moderate-complexity read-side queries.
- Deep integration with Mapster's `ProjectToType<T>()` (TE-0023 / ADR-0008),
  allowing efficient read-side projection without abandoning EF Core for
  simple-to-moderate reporting queries.
- Optimistic concurrency support via row-version tokens, already relied
  upon for Aggregate consistency across modules.
- Excellent tooling: Visual Studio / Rider integration, migration
  scaffolding, and a large first-party support surface from Microsoft.

## Architectural Weaknesses

- For very large, multi-join reporting queries (e.g. fleet-wide utilization
  across Assets, Meter Readings, and Maintenance Records spanning a full
  fiscal year), EF Core's generated SQL can be less predictable and harder
  to hand-tune than explicitly written SQL.
- Change tracking overhead is unnecessary and wasteful for pure read-only,
  high-volume reporting scenarios, even when `AsNoTracking()` is used,
  since query translation and materialization still carry more overhead
  than a lightweight object mapper executing a hand-written query.
- Complex LINQ expressions can occasionally produce inefficient SQL if not
  reviewed — a risk already identified in TE-0004 and mitigated there
  through code review and performance testing practices.

## Operational Characteristics

Already fully operational across the solution; `AppDbContext` is registered
per module boundary and consumed exclusively through repository
abstractions, consistent with SolutionStructure.md.

## Scalability

Scales well for transactional workloads at the concurrency levels expected
from a Blazor Server host. For very large read-side aggregations, query
performance can degrade relative to hand-written SQL, which is precisely
the gap this evaluation's Hybrid strategy is designed to close.

## Security

Parameterized SQL generation by default, protecting against SQL injection.
Security ultimately depends on correct application-level authorization,
consistent with the conclusion already reached in TE-0004.

## Developer Experience

Excellent; the entire team is already fluent in EF Core, and no new
learning curve is introduced by reaffirming it as the default.

## Maintainability

Excellent; EF Core's migration framework keeps schema evolution
auditable and reversible, a property that a hand-written-SQL-only
approach would not provide.

## AI Compatibility

Not directly relevant; data access is an internal Infrastructure-layer
concern with no externally consumed contract.

## Cloud Neutrality

Fully cross-platform and provider-independent; the solution currently
targets SQL Server but EF Core's provider model preserves the option to
target other relational databases without rewriting the data access layer,
aside from the Dapper SQL discussed below, which is written specifically
for the target provider's SQL dialect.

## Typical Usage

```csharp
public sealed class AssetRepository : IAssetRepository
{
    private readonly AppDbContext _context;

    public AssetRepository(AppDbContext context) => _context = context;

    public async Task<Asset?> GetByIdAsync(AssetId id, CancellationToken ct)
        => await _context.Assets
            .Include(a => a.Engine)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
}
```

## Comparison with Dapper

| Aspect | EF Core 10 | Dapper |
|--------|------------|--------|
| Change tracking | Yes | No |
| Migrations | Yes (native) | No (requires TE-0025 decision) |
| Query authoring | LINQ (type-safe) | Hand-written SQL |
| Read-heavy large-join performance | Good | Excellent |
| Write-side unit of work | Excellent | Not designed for this |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| .NET 10 Compatibility | Excellent |
| Write-Side Productivity | Excellent |
| Read-Side Performance at Scale | Good |
| License Stability | Excellent (MIT) |
| Migration Cost | None (incumbent) |

## Relationship with Mapster (TE-0023)

EF Core's `IQueryable<T>` composes directly with Mapster's
`ProjectToType<T>()`, translating projection expressions into SQL for the
majority of read-side queries. This relationship remains the default read
path under the Hybrid strategy recommended below; Dapper is reserved only
for the narrower set of queries where this composition is insufficient.

## Preliminary Conclusion

Entity Framework Core 10 remains the correct, mandatory technology for all
write-side persistence and for the large majority of read-side queries. It
is reaffirmed without qualification for that role.

---

# 6. Dapper Evaluation

## Overview

Dapper is a lightweight micro-ORM for .NET, developed originally by the
Stack Overflow team. It extends `IDbConnection` with extension methods that
execute hand-written SQL and map the results onto plain C# objects with
minimal overhead. TE-0004 evaluated and rejected Dapper as a full ORM
replacement; this evaluation considers it only in a narrower, complementary
role.

## Architectural Role

```text
Application Layer

   GetFleetUtilizationReportQueryHandler
          │
          ▼
   IFleetUtilizationReadRepository.GetReportAsync()
          │
          ▼
Infrastructure Layer
   FleetUtilizationReadRepository (Dapper, hand-written SQL)
          │
          ▼
   SQL Server Database (same connection/database as EF Core)
```

## Architectural Strengths

- Minimal overhead: SQL is executed almost exactly as written, with no
  change-tracking or LINQ-translation cost, making it well suited to
  large, multi-join reporting queries.
- Full control over the generated SQL, including query hints, indexed
  views, and provider-specific optimizations that would be awkward to
  express through LINQ.
- Extremely lightweight object mapping onto DTOs directly, avoiding entity
  materialization entirely for read-only scenarios.
- Widely used, mature, and stable; a de facto standard complement to EF
  Core in many .NET enterprise systems.

## Architectural Weaknesses

- No change tracking, no unit of work, and no concurrency token support —
  entirely unsuitable for write-side persistence, which is precisely why
  TE-0004 rejected it as a full ORM.
- No native migration framework; schema changes must continue to be
  governed entirely by EF Core Migrations (or the technology selected in
  the forthcoming TE-0025), with Dapper never owning schema evolution.
- Hand-written SQL loses compile-time type safety; a column rename in the
  database will not be caught by the compiler the way a LINQ query against
  an EF Core model would be.
- Introducing a second query-authoring idiom creates a governance risk: if
  left unbounded, Dapper usage could gradually expand from "targeted
  reporting queries" into a parallel, ungoverned data access style
  competing with EF Core across the codebase.

## Operational Characteristics

Dapper requires no separate infrastructure; it operates over the same
`IDbConnection` / connection string already used by EF Core, meaning no new
connection pool, no new database engine, and no additional operational
surface beyond the SQL text itself.

## Scalability

Excellent for large, read-heavy, multi-join reporting queries — this is
precisely the scenario where Dapper measurably outperforms EF Core, since
no change tracking or expression-tree translation overhead exists.

## Security

Dapper supports fully parameterized queries via anonymous objects or DTOs
passed as query parameters, providing the same SQL-injection protection as
EF Core when used correctly. The risk shifts from the library to the
developer: because SQL is hand-written, string-concatenation-based SQL
injection vulnerabilities become possible if a developer bypasses
parameterization, a risk that does not exist with LINQ-based EF Core
queries.

## Developer Experience

Requires developers to write and maintain raw SQL directly, which is a
different skill and a different review burden than LINQ. For the team
already fluent in EF Core, this introduces some friction, though most
enterprise .NET developers have at least baseline SQL literacy.

## Maintainability

Good, provided usage is strictly confined to the narrow, targeted role
defined by this evaluation's governance rules below. Poor if allowed to
expand unbounded, since hand-written SQL scattered across many reporting
queries without a shared abstraction becomes difficult to keep consistent
with schema changes made through EF Core Migrations.

## AI Compatibility

Not applicable.

## Cloud Neutrality

Fully cross-platform; Dapper itself introduces no cloud-vendor dependency.
However, because it is written directly against the target database's SQL
dialect, hand-written Dapper queries are inherently less provider-portable
than LINQ queries translated by EF Core's provider abstraction — a
trade-off already accepted for the specific, narrow set of queries where
Dapper is used.

## Typical Usage

```csharp
public sealed class FleetUtilizationReadRepository : IFleetUtilizationReadRepository
{
    private readonly IDbConnection _connection;

    public FleetUtilizationReadRepository(IDbConnection connection)
        => _connection = connection;

    public async Task<IReadOnlyList<FleetUtilizationRowDto>> GetReportAsync(
        Guid organizationId, DateOnly from, DateOnly to, CancellationToken ct)
    {
        const string sql = """
            SELECT a.Id, a.Name, SUM(m.HoursDelta) AS TotalHours
            FROM Assets a
            JOIN MeterReadings m ON m.AssetId = a.Id
            WHERE a.OrganizationId = @organizationId
              AND m.ReadingDate BETWEEN @from AND @to
            GROUP BY a.Id, a.Name
            """;

        var result = await _connection.QueryAsync<FleetUtilizationRowDto>(
            sql, new { organizationId, from, to });

        return result.AsList();
    }
}
```

## Comparison with EF Core 10

| Aspect | Dapper | EF Core 10 |
|--------|--------|------------|
| Change tracking | No | Yes |
| Migrations | No (relies on EF Core / TE-0025) | Yes (native) |
| Query authoring | Hand-written SQL | LINQ (type-safe) |
| Read-heavy large-join performance | Excellent | Good |
| Compile-time safety | Low | High |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (when strictly confined to read repositories) |
| .NET 10 Compatibility | Excellent |
| Write-Side Productivity | Poor (not intended for this role) |
| Read-Side Performance at Scale | Excellent |
| License Stability | Excellent (MIT) |
| Migration Cost | Low (additive, not a replacement) |

## Relationship with Entity Framework Core

Dapper is never proposed as a replacement for EF Core, and never shares a
DbContext or participates in EF Core's change tracking or unit of work. It
operates strictly as an independent, read-only query execution path against
the same physical database, governed by the boundary rules defined in the
Hybrid Persistence Strategy section below.

## Preliminary Conclusion

Dapper is a strong, low-risk complement for a narrow, clearly bounded set
of high-volume reporting queries, but it must never be adopted as a general
replacement for EF Core, consistent with TE-0004's original conclusion.

---

# 7. Hybrid Persistence Strategy Evaluation

## Overview

The Hybrid Persistence Strategy is not a third technology but a governed
architectural pattern: EF Core 10 remains the mandatory technology for all
write-side persistence and the default for read-side queries, while Dapper
is permitted only for a narrowly defined, explicitly justified set of
read-only reporting queries where EF Core's overhead is measurably
significant.

## Architectural Role

```text
Application Layer
   ICommandRepository<T>              IReadRepository<T>
   (write side, always EF Core)       (read side, EF Core by default)
                                              │
                                   large multi-join
                                   reporting query?
                                     │           │
                                    No           Yes
                                     │           │
                                     ▼           ▼
                              EF Core        Dapper
                          (ProjectToType)   (hand-written SQL,
                                             justified in a
                                             module's ADR-style
                                             README note)
```

## Governance Rules

To prevent the risk identified in the Dapper evaluation above — an
ungoverned second data access idiom — the Hybrid strategy is adopted only
under the following explicit rules:

1. **EF Core is the default.** Every read-side query starts as an EF Core
   `IQueryable<T>` projection via Mapster (TE-0023). Dapper is considered
   only after a specific query is identified as a performance concern.
2. **Dapper is opt-in per query, not per module.** A module may use EF Core
   exclusively for 95% of its queries and Dapper for a single reporting
   query; there is no notion of a "Dapper module."
3. **Dapper repositories are read-only.** No `INSERT`, `UPDATE`, or
   `DELETE` statement may be issued through Dapper anywhere in the
   solution. Only `SELECT` queries are permitted.
4. **Schema ownership remains with EF Core Migrations** (or the technology
   selected under the forthcoming TE-0025). Dapper queries must be updated
   whenever a migration changes a column referenced by hand-written SQL;
   this is a mandatory step in the Definition of Done for any migration
   that affects a table read by an existing Dapper query.
5. **Every Dapper-based read repository must document, in a code comment at
   the top of the file, the specific performance justification for
   bypassing EF Core** (e.g. "avoids materializing 40k+ rows per request
   across a 4-table join; benchmarked N ms faster than the EF Core
   equivalent").

## Architectural Strengths

- Captures the performance benefit of Dapper for the specific queries that
  need it, without abandoning EF Core's change tracking, migrations, and
  type safety for the rest of the platform.
- The governance rules above directly address the single biggest risk
  identified in the standalone Dapper evaluation — idiom fragmentation —
  by making Dapper usage explicit, justified, and read-only.
- Keeps the platform's default developer experience unchanged: a developer
  building a new feature writes LINQ against EF Core unless a specific,
  documented performance problem justifies otherwise.

## Architectural Weaknesses

- Introduces two data access idioms into the codebase, which — even when
  governed — increases the conceptual surface a new team member must learn.
- Requires ongoing discipline (code review) to ensure Dapper is not
  quietly used for convenience rather than a genuine, documented
  performance need, and to ensure Dapper queries are kept in sync with
  schema migrations.
- Adds a small amount of additional testing surface: Dapper-based read
  repositories require their own integration tests against a real database
  connection, similar to EF Core repositories, since the SQL itself is
  part of the logic being tested.

## Operational Characteristics

No new operational infrastructure: both EF Core and Dapper share the same
connection string, the same database, and the same deployment pipeline.

## Scalability

This is the strategy's core strength: it allows the platform to scale its
most demanding reporting queries (fleet-wide utilization, maintenance-cost
rollups across a fiscal year) without paying EF Core's materialization and
change-tracking overhead on every request, while every other query
continues to benefit from EF Core's productivity.

## Security

Inherits the security profile of both technologies individually. The
governance rule mandating read-only Dapper usage specifically eliminates
the highest-risk category of hand-written-SQL mistakes (an accidental or
malicious write path bypassing domain invariants and change tracking).

## Developer Experience

Unchanged for the default case (EF Core + Mapster). Slightly more demanding
for the small number of developers authoring or reviewing a Dapper-based
read repository, who must additionally justify and document the
performance rationale per governance rule 5.

## Maintainability

Good, contingent entirely on the governance rules above being enforced
through code review — the same enforcement mechanism already used for
every other architectural rule in DependencyRules.md.

## AI Compatibility

Not applicable.

## Cloud Neutrality

Equivalent to the EF Core and Dapper evaluations individually; the small
number of Dapper queries represent the only portion of the data access
layer that is not automatically provider-portable, which is an accepted,
narrowly scoped trade-off.

## Typical Usage

The Hybrid strategy is expressed structurally, not through a single code
sample: `IAssetRepository` (write-capable, EF Core only) and
`IFleetUtilizationReadRepository` (read-only, Dapper, as shown in the
Dapper evaluation above) are registered as separate interfaces, so that the
distinction between the two idioms is visible at the dependency-injection
and interface level, not hidden inside a single repository class.

## Comparison with EF-Core-Only Strategy

| Aspect | Hybrid Strategy | EF-Core-Only |
|--------|------------------|----------------|
| Write-side integrity | Excellent (EF Core, unchanged) | Excellent |
| Large reporting query performance | Excellent (Dapper where justified) | Good |
| Governance overhead | Low (5 explicit rules) | None |
| Idiom consistency | Two idioms, clearly bounded | One idiom |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent (both idioms remain confined to Infrastructure layer) |
| .NET 10 Compatibility | Excellent |
| Write-Side Productivity | Excellent (unchanged, EF Core only) |
| Read-Side Performance at Scale | Excellent |
| License Stability | Excellent (both MIT) |
| Migration Cost | Low — additive, opt-in per query |

## Relationship with ADR-0006

This strategy does not modify ADR-0006 in any way; EF Core remains the
sole write-side technology and the default read-side technology. It adds a
narrowly scoped, governed extension for read-only reporting queries.

## Preliminary Conclusion

The Hybrid Persistence Strategy is the recommended approach: it preserves
everything TE-0004 / ADR-0006 already established, while giving the
platform an explicit, governed path for the reporting-scale queries that
EF Core alone handles less efficiently.

---

# 8. Overall Technology Comparison

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|-----------------|------------------------|--------------|---------|
| All write-side / transactional persistence | EF Core 10 | — (mandatory, no alternative) | Change tracking, unit of work, migrations |
| Default read-side queries | EF Core 10 + Mapster `ProjectToType<T>()` | — | Type-safe, productive, sufficient for most queries |
| Justified high-volume reporting queries | Dapper (read-only) | EF Core (fallback if not yet justified) | Predictable, hand-tuned SQL for large joins |

## Capability Comparison

| Capability | EF Core 10 | Dapper | Hybrid Strategy |
|------------|------------|--------|-------------------|
| Open Source (ADR-0002 compliant) | Yes | Yes | Yes |
| Write-side change tracking | Yes | No | Yes (via EF Core) |
| Migrations | Yes | No | Yes (via EF Core) |
| Large-join read performance | Good | Excellent | Excellent (where justified) |
| Compile-time query safety | High (LINQ) | Low (raw SQL) | High by default, low only where opted-in |
| Governance overhead | None | High if ungoverned | Low (explicit rules defined) |
| Migration cost from current state | None | N/A alone | Low, additive |

## Cloud Neutrality Assessment

Both technologies are cross-platform and provider-neutral in principle. The
Hybrid strategy accepts a narrow, deliberate loss of provider portability
for the small number of Dapper-based reporting queries, in exchange for
measurable performance gains on exactly those queries.

## Enterprise Suitability

| Criterion | EF Core 10 | Dapper (standalone) | Hybrid Strategy |
|-----------|------------|----------------------|--------------------|
| Suitable as platform-wide default | Yes | No | Yes (as the governing strategy) |
| Suitable for write-side persistence | Yes | No | Yes (via EF Core) |
| Suitable for large reporting queries | Acceptable | Excellent | Excellent |

## Clean Architecture Compliance

All three options can be confined correctly to the Infrastructure layer.
The Hybrid strategy's governance rules exist specifically to preserve this
compliance as a second idiom is introduced, preventing Dapper usage from
leaking write-side responsibilities that belong exclusively to EF Core.

## Risk Assessment

| Risk | Affected Option | Severity | Mitigation |
|------|--------------------|----------|------------|
| Idiom fragmentation | Hybrid Strategy | Medium | Governance rules 1–5, enforced via code review |
| Reporting queries slow at scale if Hybrid is rejected | EF-Core-Only | Medium | Adopt Hybrid strategy as recommended |
| Accidental write via Dapper bypassing domain invariants | Hybrid Strategy | High if unmitigated | Governance rule 3 (read-only enforcement) |
| Dapper SQL drifting from schema after a migration | Hybrid Strategy | Medium | Governance rule 4 (mandatory update step in Definition of Done) |

## Overall Evaluation

EF Core 10 remains mandatory and unchanged for all write-side persistence,
fully reaffirming ADR-0006. Standalone Dapper adoption as a full ORM
replacement remains correctly rejected, consistent with TE-0004. The
Hybrid Persistence Strategy, governed by the five explicit rules defined
above, is the recommended extension: it captures Dapper's read-side
performance benefit for a small, justified set of reporting queries without
weakening the write-side architecture or creating an idiom free-for-all.

---

# 9. Final Recommendation

## Core Technology Stack

| Responsibility | Selected Technology | Rationale |
|-----------------|----------------------|-----------|
| Write-side persistence | EF Core 10 | Reaffirmed incumbent (ADR-0006); mandatory, no exceptions |
| Default read-side queries | EF Core 10 + Mapster | Type-safe, productive, sufficient for the large majority of queries |
| Justified high-volume reporting queries | Dapper (read-only, governed) | Predictable, hand-tuned SQL for large multi-join reports |

## Recommended Architecture

```text
Application Layer

   Write side: ICommandRepository<T>       Read side: IReadRepository<T>
        │                                        │
        ▼                                        ▼
Infrastructure Layer
   EF Core (AppDbContext,             EF Core + Mapster (default)
   change tracking,                          │
   migrations, unit of work)         Dapper (opt-in, read-only,
        │                            justified per governance rules)
        ▼                                        │
              SQL Server Database  ◄──────────────┘
```

## Governance Summary

The five governance rules defined in Section 7 (EF Core is the default;
Dapper is opt-in per query; Dapper repositories are read-only; schema
ownership remains with EF Core Migrations; every Dapper query documents its
performance justification) are adopted as binding practice, enforced
through code review under the existing DependencyRules.md process.

## Security Recommendations

Code review must specifically verify that no Dapper-based repository issues
a write statement, consistent with governance rule 3.

## Cloud Neutrality

The recommended stack preserves provider portability for the entire
write-side and the large majority of the read-side; the small number of
Dapper queries represent a deliberate, narrowly scoped, and documented
exception.

## AI Readiness

Not applicable to this evaluation.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| EF Core 10 (Incumbent, write-side) | **Reaffirmed** |
| EF Core 10 + Mapster (default read-side) | **Reaffirmed** |
| Dapper (standalone, full ORM replacement) | Rejected — consistent with TE-0004 |
| Hybrid Persistence Strategy (governed, read-only Dapper) | **Approved** |

---

# Decision Summary

- ✔ Clean Architecture preserved
- ✔ .NET 10 Compatibility
- ✔ Open Source First Policy (ADR-0002) compliance
- ✔ No disruption to existing write-side implementation
- ✔ Explicit governance rules defined to prevent idiom fragmentation
- ✔ Read-side performance path defined for large reporting queries

This evaluation **reaffirms ADR-0006 — Use Entity Framework Core** without
modification, and formally introduces the Hybrid Persistence Strategy as an
approved, governed extension. Because the Hybrid strategy introduces
binding governance rules that future developers must follow, its five
Compliance Rules have been recorded as a first-class architectural decision
in **ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries**, since
ADR-0006 itself only ever addressed ORM selection, not this broader
strategy.

---

# Related ADR

```
ADR-0006 (Reaffirmed — no change)
ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries (new)
```

---

# Related Documents

- TE-0004 — Entity Framework Core (original ORM evaluation)
- TE-0023 — Object Mapping Technology Evaluation
- ADR-0002 — Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- ADR-0008 — Use Mapster
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- Dependency Catalog
- Dependency Rules

---

# References

https://learn.microsoft.com/ef/core/

https://github.com/dotnet/efcore

https://github.com/DapperLib/Dapper

https://www.learndapper.com/

---

# Revision History

| Version | Date       | Author             | Description                               |
|---------|------------|--------------------|-------------------------------------------|
| 1.0.0   | 2026-07-27 | Solution Architect | Initial evaluation; reaffirms ADR-0006 (EF Core), evaluates Dapper as a governed read-side complement, recommends Hybrid Persistence Strategy with five explicit governance rules |
| 1.1.0   | 2026-07-27 | Solution Architect | Updated to reference ADR-0019, created to formalize the Hybrid Persistence Strategy's five Compliance Rules |
| 1.1.1   | 2026-07-28 | File name Changed from (Data Access Technology Evaluation)     |
| 1.3.0   | 2026-07-28 | New section added (Evaluation Scope) |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0 |