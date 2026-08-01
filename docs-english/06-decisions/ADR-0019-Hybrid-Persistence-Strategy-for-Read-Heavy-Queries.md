| Property | Value |
|----------|-------|
| **Document ID** | ADR-0019 |
| **Title** | Hybrid Persistence Strategy for Read-Heavy Queries |
| **Version** | 4.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-27 |
| **Last Updated** | 2026-07-28 |

---

# Context

ADR-0006 approved Entity Framework Core as the platform's Object-Relational
Mapper, and TE-0004 evaluated and rejected Dapper as a full replacement for
EF Core, citing manual mapping overhead, absence of a migration framework,
and reduced productivity for complex domains.

That decision addressed ORM selection only. It did not address the
narrower, distinct question of how the platform should handle large,
multi-join, read-only reporting queries (for example, fleet-wide
utilization reports spanning Assets, Meter Readings, and Maintenance
Records across a full fiscal year), where EF Core's change tracking and
expression-tree translation overhead is unnecessary and measurably costly.

TE-0024 — Data Access Technology Evaluation examined this narrower question
directly, evaluating Entity Framework Core 10, Dapper, and a Hybrid
Persistence Strategy combining both. It recommended the Hybrid strategy,
governed by five explicit rules, and deferred formal recording of those
rules to this ADR.

Without an explicit governing decision, introducing Dapper into the
codebase — even in a narrow, well-intentioned way — risks fragmenting the
platform's data access idiom, creating a second, ungoverned path capable of
diverging from the schema owned by EF Core Migrations.

---

# Decision

The Application and Infrastructure layers shall follow a **Hybrid
Persistence Strategy**:

- **Entity Framework Core 10 remains the mandatory technology for all
  write-side, transactional, change-tracked persistence**, with no
  exception.
- **Entity Framework Core 10, combined with Mapster's `ProjectToType<T>()`
  (ADR-0008), remains the default technology for all read-side queries.**
- **Dapper may be used only for specific, individually justified, read-only
  reporting queries** where EF Core's overhead is a demonstrated
  performance concern, subject to the Compliance Rules below.

---

# Decision Drivers

- Read-side performance for large, multi-join reporting queries
- Preservation of EF Core's write-side integrity and change-tracking model
- Prevention of data access idiom fragmentation
- Preservation of a single, unambiguous schema owner (ADR-0006 / EF Core
  Migrations)
- Open Source Policy compliance (ADR-0002)
- Maintainability at platform scale

---

# Alternatives Considered

## EF-Core-Only Strategy (status quo, unmodified)

Rejected as the sole path forward because it leaves no defined,
architecturally sanctioned option for the specific class of large,
multi-join reporting queries where EF Core's change-tracking and
expression-tree translation overhead is measurably significant, other than
informal, undocumented deviations that this ADR is specifically intended to
prevent.

## Dapper as a Full ORM Replacement

Rejected, reaffirming TE-0004's original conclusion: Dapper lacks change
tracking, a migration framework, and the productivity EF Core provides for
complex, write-heavy domains.

## Ungoverned Dual Usage (EF Core and Dapper without explicit rules)

Rejected because it was assessed in TE-0024 as carrying a High-severity
risk of idiom fragmentation and accidental schema-bypassing writes, with no
mechanism to prevent Dapper usage expanding beyond its intended, narrow
role.

---

# Consequences

## Positive

- Large, multi-join reporting queries can achieve materially better
  performance than the EF-Core-only path, without weakening the write side.
- The platform's default developer experience is unchanged: a developer
  building a new feature writes LINQ against EF Core unless a specific,
  documented performance problem justifies otherwise.
- Schema remains under the sole ownership of EF Core Migrations, preserving
  the model/schema consistency guarantee already relied upon across the
  platform.

- Optimized read performance.
- EF Core remains the single write model.
- CQRS separation becomes clearer.
- Read-side scalability increases.
- Search indexing becomes simpler.

## Negative

- Introduces a second, though narrowly bounded, data access idiom that new
  team members must learn to recognize.
- Requires ongoing code-review discipline to prevent Dapper usage from
  expanding beyond its documented, justified, read-only role.
- Adds a small amount of additional testing surface: Dapper-based read
  repositories require their own integration tests against a real database
  connection.

- Additional synchronization complexity.
- Separate read projections must be maintained.
- More infrastructure services are required.

## Trade-offs

Some provider portability is deliberately sacrificed for the small number
of Dapper-based queries, since hand-written SQL is written against the
target provider's dialect, whereas LINQ queries translated by EF Core
remain provider-independent.

## Future Limitations

If the volume of justified Dapper queries grows substantially, this
decision should be revisited to assess whether a dedicated read-model /
reporting store would serve the platform better than an expanding set of
individually justified Dapper repositories.

---

# Architecture Impact

- **Domain** — No impact. Domain entities shall never reference EF Core or
  Dapper.
- **Application** — Defines separate repository abstractions for
  write-capable (`ICommandRepository<T>`, always backed by EF Core) and
  read-only (`IReadRepository<T>`, EF Core by default, Dapper where
  justified) concerns.
- **Infrastructure** — Hosts both the EF Core `AppDbContext` and any
  Dapper-based read repositories. Dapper repositories connect through the
  same `IDbConnection` / connection string already used by EF Core.
- **Presentation** — No impact; the choice of read-side technology remains
  fully invisible above the Application layer.

---

# Implementation Notes

- Dapper-based read repositories shall be named and registered distinctly
  from EF Core repositories (e.g. `IFleetUtilizationReadRepository`), so
  that the distinction between the two idioms is visible at the interface
  and dependency-injection level, not hidden inside a shared repository
  class.
- Every Dapper-based read repository shall include a code comment at the
  top of the file stating the specific performance justification for
  bypassing EF Core (see Compliance Rule 5).
- Any EF Core Migration that changes a column referenced by an existing
  Dapper query shall include, as part of its Definition of Done, an update
  to the corresponding Dapper SQL.

---

# Compliance Rules

1. **EF Core is the default.** Every read-side query starts as an EF Core
   `IQueryable<T>` projection via Mapster (ADR-0008). Dapper is considered
   only after a specific query is identified as a performance concern.

2. **Dapper is opt-in per query, not per module.** A module may use EF Core
   exclusively for the large majority of its queries and Dapper for a
   single reporting query; there is no notion of a "Dapper module."

3. **Dapper repositories are read-only.** No `INSERT`, `UPDATE`, or
   `DELETE` statement may be issued through Dapper anywhere in the
   solution. Only `SELECT` queries are permitted.

4. **Schema ownership remains exclusively with EF Core Migrations.** Dapper
   queries must be updated whenever a migration changes a column they
   reference; this is a mandatory step in the Definition of Done for any
   such migration.

5. **Every Dapper-based read repository must document its performance
   justification** in a code comment at the top of the file (e.g. "avoids
   materializing 40k+ rows per request across a 4-table join; benchmarked
   N ms faster than the EF Core equivalent").

---

# Related Technology Evaluation

TE-0024 — Data Access Technology Evaluation

---

# Related Proof of Concept

Not Required

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
- ADR-0006 — Use Entity Framework Core
- ADR-0008 — Use Mapster
- TE-0004 — Entity Framework Core
- TE-0023 — Object Mapping Technology Evaluation
- TE-0024 — Data Access Technology Evaluation
- Dependency Catalog

---

# References

https://learn.microsoft.com/ef/core/

https://github.com/DapperLib/Dapper

https://www.learndapper.com/

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-27 | Solution Architect | Initial decision, formalizing the Hybrid Persistence Strategy and its five Compliance Rules as recommended by TE-0024 |
| 1.1.0   | 2026-07-28 | Solution Architect | Items added to  Consequences                          |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |