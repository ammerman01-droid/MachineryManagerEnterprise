| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | TE-0025            |
| **Title**        | Database Migration Technology Evaluation |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-27         |
| **Last Updated** | 2026-08-08         |

# Purpose

This document evaluates the technology used to manage relational database
schema evolution across the MachineryManagerEnterprise solution.

TE-0004 / ADR-0006 approved Entity Framework Core as the platform's ORM and
noted, in passing, that EF Core provides "mature migration support." This
evaluation examines that specific concern in isolation and at the same
depth as every other Technology Evaluation, asking explicitly whether EF
Core Migrations remains the correct schema evolution mechanism, or whether
a dedicated, ORM-independent migration tool (DbUp, Flyway, or Liquibase)
would better serve the platform — particularly given that TE-0024
introduced Dapper as a governed, read-only complement to EF Core, which
makes "who owns the schema" an explicit architectural question rather than
an implicit one.

The objective of this evaluation is to:

- confirm EF Core Migrations as the schema evolution mechanism for the
  platform, consistent with ADR-0006 and the schema-ownership rule
  established in TE-0024's Hybrid Persistence Strategy;
- evaluate DbUp, Flyway, and Liquibase as dedicated, ORM-independent
  migration tools;
- determine whether any of these alternatives should replace, or run
  alongside, EF Core Migrations.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Relationship with TE-0004 / ADR-0006 and TE-0024

**TE-0004 / ADR-0006** approved EF Core as the ORM and implicitly assumed EF
Core Migrations as its companion schema evolution tool, without evaluating
alternatives directly.

**TE-0024 — Data Access Technology Evaluation** established, as one of its
five Hybrid Persistence Strategy governance rules, that "schema ownership
remains with EF Core Migrations," explicitly deferring the full
justification of that ownership to this evaluation.

This evaluation exists to make that ownership decision explicit and
evaluated, rather than assumed. It reaffirms TE-0024's governance rule if
EF Core Migrations is confirmed as the correct choice.

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0002 — Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- TE-0004 — Entity Framework Core
- TE-0024 — Data Access Technology Evaluation
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md
---

# Scope

This evaluation covers:

- the mechanism used to create, version, apply, and roll back relational
  database schema changes across all environments (local development, CI,
  staging, production);
- the relationship between schema migrations and the EF Core model used for
  both write-side persistence and the default read-side query path.

Out of scope:

- data seeding strategy for reference/lookup data (a related but separate
  concern, not evaluated here);
- the Distributed Workspace local database schema, which follows its own
  synchronization-specific versioning strategy under ADR-0014 / ADR-0015
  and is not in scope for this server-side evaluation.

---

# Functional Requirements

The selected solution shall support:

- versioned, ordered, repeatable application of schema changes across
  environments;
- generating migrations directly from the EF Core model, since the model
  itself is the primary source of truth for entity shape;
- safe rollback or forward-fix strategies for failed deployments;
- integration with the CI/CD pipeline (relevant to the forthcoming TE-0031
  — Build, Packaging and Deployment Technology Evaluation) so that
  migrations apply automatically and safely as part of deployment.

---

# Non-Functional Requirements

The solution should provide:

- a single, unambiguous source of truth for schema state, avoiding the risk
  of two migration systems drifting out of sync;
- low operational complexity;
- auditability: every schema change traceable to a specific migration file
  and, ultimately, a specific commit;
- compatibility with SQL Server, the platform's current target database.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| EF Core Migrations | Code-first migrations generated from the EF Core model | **Incumbent (implicit, via ADR-0006)** |
| DbUp | Script-based, ordered SQL migration runner | Evaluated |
| Flyway | Convention-based, versioned SQL migration tool | Evaluated |
| Liquibase | Changelog-based, database-agnostic schema management tool | Evaluated |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Open Source & License Stability | Critical |
| A2 | Single Source of Truth for Schema | Critical |
| A3 | .NET / EF Core Integration | Critical |
| A4 | Rollback / Forward-Fix Safety | High |
| A5 | CI/CD Integration | High |
| A6 | Developer Experience | High |
| A7 | Auditability | High |
| A8 | Operational Complexity | Medium |
| A9 | Migration Cost from Current State | Medium |

---

# Architecture Principle

Schema evolution shall have exactly one owner. Whatever tool is selected
generates or applies the authoritative, ordered set of schema changes;
no other tool or manual process may alter schema outside that mechanism.

```text
EF Core Model (Domain-driven entity shape)
        │
        ▼
Migration Tool (generates/applies ordered SQL scripts)
        │
        ▼
SQL Server Database (single authoritative schema)
        ▲
        │
Dapper (TE-0024) — read-only, never alters schema
```

---

# 5. EF Core Migrations Evaluation (Incumbent)

## Overview

EF Core Migrations is the first-party schema evolution mechanism built into
Entity Framework Core. It generates C# migration classes directly from
changes detected in the EF Core model, and applies them in order via
`dotnet ef database update` or `context.Database.Migrate()`.

## Architectural Role

```text
Domain / Application Layer
   Entity model changes (e.g. new property on Asset)
          │
          ▼
Infrastructure Layer
   dotnet ef migrations add AddAssetWarrantyExpiryDate
          │
          ▼
   Generated C# Migration (Up/Down methods)
          │
          ▼
   dotnet ef database update  →  SQL Server
```

## Architectural Strengths

- Generated directly from the EF Core model, guaranteeing that schema and
  entity mapping can never silently drift apart — the single biggest
  architectural advantage over any ORM-independent tool.
- Migrations are C# code, reviewable through the same pull-request process
  as any other change, with full IDE support (IntelliSense, refactoring
  safety when entity properties are renamed).
- Built-in `Up()` / `Down()` methods provide a native rollback mechanism
  without requiring a second tool or hand-written reverse scripts.
- Zero additional package or infrastructure: already present as part of
  the EF Core dependency already approved under ADR-0006.
- `context.Database.Migrate()` integrates trivially into a startup hosted
  service or CI/CD deployment step, requiring no additional orchestration
  tooling.

## Architectural Weaknesses

- Generated SQL, while generally reliable, is not always as tunable or
  reviewable at the raw-SQL level as hand-written migration scripts for
  advanced scenarios (e.g. complex data backfills combined with schema
  changes, or SQL Server-specific indexed view creation).
- Coupled to the EF Core model by design — this is a strength for
  guaranteeing consistency, but means a schema change that does not
  originate from an EF Core model change (e.g. a DBA-authored performance
  index) must still be expressed as an EF Core migration to remain in the
  single source of truth, which can feel indirect to database
  specialists more accustomed to hand-written SQL tooling.
- Down-migrations are not always safe to run against production data
  (e.g. a column drop cannot un-drop data), a limitation shared by every
  candidate evaluated here, not unique to EF Core.

## Operational Characteristics

Already fully operational; migrations are applied via the existing
`AppDbContext` and standard `dotnet ef` tooling, requiring no new runtime
component.

## Scalability

Scales without concern at the schema-change volume expected for this
platform; migration application time is proportional to the SQL generated,
identical in this respect to any other tool in this evaluation.

## Rollback / Forward-Fix Safety

Native `Down()` methods provide a rollback path for reversible changes.
For irreversible changes (data-destructive operations), the same
forward-fix discipline required by every other candidate applies equally
here — this is a process concern, not a tool-specific one.

## Security

Migrations execute with the same database credentials already governing
the application's connection string; no additional credential or service
account is introduced.

## Developer Experience

Excellent for the team, since migrations are authored in C#, inside the
same codebase and the same pull-request workflow already used for every
other change, with no context-switch to a separate migration DSL or
external tool.

## Maintainability

Excellent: because migrations are derived from the EF Core model, model
and schema can never silently diverge, which removes an entire class of
"the migration script doesn't match the entity" defects that
ORM-independent tools are structurally exposed to.

## CI/CD Integration

Straightforward: `dotnet ef database update` (or `Migrate()` invoked at
startup, guarded by an environment check) integrates directly into the
existing .NET build and deployment pipeline with no additional tool
installation, which is directly relevant to the forthcoming TE-0031.

## AI Compatibility

Not applicable.

## Cloud Neutrality

Fully cross-platform; EF Core Migrations run identically on Windows, Linux,
and containerized CI/CD agents, and are provider-independent to the extent
EF Core's provider model allows.

## Typical Usage

```csharp
// Program.cs — applied automatically in non-production environments,
// and as an explicit deployment step in production
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}
```

```bash
dotnet ef migrations add AddAssetWarrantyExpiryDate --project src/Infrastructure
dotnet ef database update --project src/Infrastructure
```

## Comparison with DbUp

| Aspect | EF Core Migrations | DbUp |
|--------|----------------------|------|
| Source of truth | EF Core model | Ordered SQL script files |
| Model/schema drift risk | None | Possible (manual discipline required) |
| Rollback | Native `Down()` | Manual reverse scripts |
| Authoring language | C# | Raw SQL |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| .NET / EF Core Integration | Excellent (native) |
| Single Source of Truth | Excellent |
| CI/CD Integration | Excellent |
| License Stability | Excellent (MIT) |
| Migration Cost | None (incumbent) |

## Relationship with the Hybrid Persistence Strategy (TE-0024)

EF Core Migrations directly implements TE-0024's governance rule that
"schema ownership remains with EF Core Migrations." Dapper-based read
repositories introduced under that strategy never author or apply schema
changes; they only consume the schema EF Core Migrations produces.

## Preliminary Conclusion

EF Core Migrations remains the correct, model-derived, single source of
truth for schema evolution, and directly satisfies the governance
commitment already made in TE-0024.

---

# 6. DbUp Evaluation

## Overview

DbUp is a lightweight, open-source .NET library that applies a directory of
ordered, versioned SQL scripts against a database, tracking which scripts
have already run in a dedicated journal table.

## Architectural Role

```text
scripts/
  0001_CreateAssetsTable.sql
  0002_AddEngineTable.sql
  0003_AddAssetWarrantyExpiryDate.sql
          │
          ▼
DbUp.Run()  →  applies unrun scripts in order  →  SQL Server
          │
          ▼
   SchemaVersions journal table (tracks applied scripts)
```

## Architectural Strengths

- Full control over raw SQL, appealing for teams or specific changes that
  need hand-tuned, review-friendly SQL rather than generated migration
  code.
- Extremely simple mental model: a folder of ordered `.sql` files and a
  journal table; minimal abstraction to learn.
- ORM-independent: does not require EF Core at all, meaning it would work
  identically even if the project changed ORMs in the future.
- MIT licensed, small, stable, with a long track record in the .NET
  ecosystem.

## Architectural Weaknesses

- Completely decoupled from the EF Core model: nothing prevents a
  developer from changing an entity's shape without writing the
  corresponding SQL script, or vice versa. This reintroduces exactly the
  model/schema drift risk that EF Core Migrations structurally eliminates.
- No native rollback mechanism; reversing a change requires authoring and
  running a new forward script, which is a more manual process than EF
  Core's `Down()` method.
- Introducing DbUp alongside EF Core would mean maintaining two
  overlapping mechanisms capable of altering schema, directly violating
  the "single owner of schema" principle this evaluation establishes.
- No IDE-level compile-time safety; a `.sql` script with a typo is only
  caught when it fails to execute.

## Operational Characteristics

Requires its own journal table and its own invocation step in the
deployment pipeline, run independently of, and in addition to, whatever EF
Core does.

## Scalability

Adequate for any realistic schema-change volume; not a differentiator on
its own.

## Rollback / Forward-Fix Safety

Weaker than EF Core Migrations: no native `Down()` equivalent; every
reversal must be authored as a new forward-only script.

## Security

Equivalent to EF Core Migrations: executes with the same database
credentials as the rest of the application.

## Developer Experience

Appeals to developers who prefer writing raw SQL directly, but removes the
IDE-assisted, refactor-safe migration authoring experience EF Core
provides when an entity property is renamed.

## Maintainability

Weaker than EF Core Migrations specifically because of the drift risk
described above — a schema change and its corresponding entity model
change must both be remembered and kept in sync manually, with no
compiler or tooling enforcement connecting the two.

## CI/CD Integration

Good: DbUp exposes a simple `UpgradeEngine.PerformUpgrade()` API well
suited to a console-based deployment step, but this is a second, separate
integration point in addition to whatever step already applies EF Core
Migrations — increasing pipeline complexity rather than simplifying it.

## AI Compatibility

Not applicable.

## Cloud Neutrality

Fully cross-platform; not a differentiator.

## Typical Usage

```csharp
var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();
```

## Comparison with EF Core Migrations

| Aspect | DbUp | EF Core Migrations |
|--------|------|----------------------|
| Source of truth | SQL scripts (manual) | EF Core model (generated) |
| Model/schema drift risk | High | None |
| Rollback | Manual forward-fix only | Native `Down()` |
| Introduces a second migration owner | Yes, if used alongside EF Core | N/A |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (Infrastructure-layer concern either way) |
| .NET / EF Core Integration | Poor (no model awareness) |
| Single Source of Truth | Poor (introduces a second schema owner) |
| CI/CD Integration | Good, but additive complexity |
| License Stability | Excellent (MIT) |
| Migration Cost | Moderate — would require re-authoring existing EF Core migrations as scripts |

## Relationship with EF Core Migrations

DbUp and EF Core Migrations are not designed to compose; adopting DbUp
alongside EF Core Migrations would create two independent, uncoordinated
mechanisms capable of altering the same schema, directly violating this
evaluation's Architecture Principle of a single schema owner.

## Preliminary Conclusion

DbUp is a capable, well-established tool, but adopting it would discard EF
Core Migrations' strongest property — guaranteed model/schema consistency —
without a compensating architectural benefit for this platform.

---

# 7. Flyway Evaluation

## Overview

Flyway is a widely used, convention-based database migration tool that
applies versioned SQL (or Java-based) migration files following a strict
naming convention (`V1__Description.sql`), tracking applied versions in a
`flyway_schema_history` table. It is broadly used across polyglot
enterprise environments, including Java and Node.js stacks, not exclusively
.NET.

## Architectural Role

```text
db/migration/
  V1__CreateAssetsTable.sql
  V2__AddEngineTable.sql
  V3__AddAssetWarrantyExpiryDate.sql
          │
          ▼
Flyway CLI / Maven-Gradle-style migrate command
          │
          ▼
   flyway_schema_history table  →  SQL Server
```

## Architectural Strengths

- Extremely mature and widely adopted across the broader software industry,
  with strong documentation and a large body of operational experience
  across many database engines.
- Strict, enforced naming/versioning convention reduces ambiguity about
  migration ordering compared to a loosely structured script folder.
- Database-engine-agnostic in principle, which would matter if the
  platform ever needed to support a database engine other than SQL Server.

## Architectural Weaknesses

- Not a .NET-native tool: Flyway is a Java-based (or, in its Community
  Edition, CLI-based) tool, introducing a non-.NET runtime dependency into
  a project whose stack, tooling, and CI/CD pipeline (Directory.Build.props,
  Directory.Packages.props) are otherwise entirely .NET-centric.
- Like DbUp, completely decoupled from the EF Core model, reintroducing the
  same model/schema drift risk identified above.
- Flyway's advanced features (undo migrations, certain validation
  capabilities) are gated behind Flyway Teams/Enterprise commercial
  editions, creating the same category of licensing governance concern
  already flagged for AutoMapper in TE-0023, and a direct tension with
  ADR-0002 if the Community Edition proves insufficient.
- Introduces a second CI/CD tool and a second runtime (Java, or a
  standalone binary) purely for schema management, adding operational
  complexity disproportionate to the platform's current needs.

## Operational Characteristics

Requires installing and invoking a separate Flyway CLI (or Java runtime) in
every environment where migrations are applied, including local developer
machines, in addition to the .NET SDK already required for the rest of the
solution.

## Scalability

Adequate for any realistic schema-change volume; not a differentiator.

## Rollback / Forward-Fix Safety

Undo migrations exist but are a commercial-tier (Flyway Teams) feature in
practice for most production use cases; the free Community Edition
generally relies on forward-fix migrations, similar to DbUp.

## Security

Requires its own database credentials configuration, separate from the
application's connection string configuration, adding a second credential
surface to manage and audit.

## Developer Experience

Familiar to developers with a polyglot or Java background; introduces a
genuinely new tool, naming convention, and CLI for a team that is otherwise
fully within the .NET/EF Core ecosystem.

## Maintainability

Weaker than EF Core Migrations for the same model/schema drift reason
identified for DbUp, compounded by the operational overhead of maintaining
a non-.NET tool in an otherwise homogeneous .NET toolchain.

## CI/CD Integration

Well documented in general, but requires adding a distinct pipeline step,
a distinct credential, and a distinct tool installation to the CI/CD
pipeline, which is planned for evaluation under the forthcoming TE-0031 and
would be an unnecessary complication given EF Core Migrations already
integrates natively.

## AI Compatibility

Not applicable.

## Cloud Neutrality

Database-engine-neutral, which is a genuine strength in principle, but not
one the platform currently needs, since it targets SQL Server exclusively
and has no stated requirement to support multiple database engines.

## Typical Usage

```sql
-- db/migration/V3__AddAssetWarrantyExpiryDate.sql
ALTER TABLE Assets ADD WarrantyExpiryDate DATE NULL;
```

```bash
flyway -url=jdbc:sqlserver://... -user=... -password=... migrate
```

## Comparison with EF Core Migrations

| Aspect | Flyway | EF Core Migrations |
|--------|--------|----------------------|
| Source of truth | SQL scripts (manual) | EF Core model (generated) |
| Runtime dependency | Java / standalone CLI | Already-present .NET SDK |
| Rollback (free tier) | Forward-fix only | Native `Down()` |
| Commercial tier required for advanced features | Yes (Teams/Enterprise) | No |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (Infrastructure-layer concern either way) |
| .NET / EF Core Integration | Poor (non-.NET tool, no model awareness) |
| Single Source of Truth | Poor |
| CI/CD Integration | Fair, adds a new tool and credential |
| License Stability | Fair (Community Edition free; advanced features commercial) |
| Migration Cost | High — full re-authoring plus new CI/CD tooling |

## Relationship with EF Core Migrations

As with DbUp, Flyway does not compose with EF Core Migrations; adopting it
would mean discarding the model-derived migration guarantee entirely and
introducing a second, non-.NET schema-owning tool.

## Preliminary Conclusion

Flyway is an excellent tool for polyglot organizations managing many
database engines, but it is a poor architectural fit for a homogeneous
.NET/EF Core platform, introducing a non-.NET runtime dependency and
discarding model/schema consistency without a compensating benefit.

---

# 8. Liquibase Evaluation

## Overview

Liquibase is a mature, database-agnostic schema management tool that
describes migrations through a changelog file (XML, YAML, JSON, or SQL),
tracking applied changesets in a `DATABASECHANGELOG` table. Like Flyway, it
is widely used across polyglot enterprise environments.

## Architectural Role

```text
changelog/
  changelog-master.yaml
    - changeset: create-assets-table
    - changeset: add-engine-table
    - changeset: add-asset-warranty-expiry-date
          │
          ▼
Liquibase CLI  →  applies unrun changesets  →  SQL Server
          │
          ▼
   DATABASECHANGELOG table
```

## Architectural Strengths

- Changelog-based format supports genuine, declarative rollback
  definitions per changeset (`<rollback>` blocks), a stronger free-tier
  rollback story than Flyway's Community Edition.
- Database-engine-agnostic, with broad provider support across many
  relational and some non-relational databases.
- Mature, enterprise-proven, with strong auditing and reporting features
  (`liquibase history`, `diff`, `status`).

## Architectural Weaknesses

- Not .NET-native: like Flyway, Liquibase is a Java-based tool (with a
  .NET-hostable variant available, but the ecosystem's center of gravity
  and richest tooling remain Java-first), introducing the same category of
  non-.NET runtime dependency into an otherwise homogeneous .NET toolchain.
- Changelog authoring in XML/YAML is more verbose and less familiar to the
  team than either raw SQL (DbUp/Flyway) or C# (EF Core Migrations).
- Completely decoupled from the EF Core model, reintroducing the same
  model/schema drift risk identified for DbUp and Flyway.
- Liquibase Pro (commercial tier) gates some advanced features (drift
  detection, additional quality checks), again raising the same category
  of licensing governance question already flagged for AutoMapper and
  Flyway, though the free Community Edition is fully functional for core
  migration needs.

## Operational Characteristics

Requires installing and invoking the Liquibase CLI (or a Java runtime) in
every environment, plus authoring changelogs in a markup format distinct
from the rest of the C# codebase.

## Scalability

Adequate for any realistic schema-change volume; not a differentiator.

## Rollback / Forward-Fix Safety

Genuinely strong in its free tier via declarative `<rollback>` blocks per
changeset — the best rollback story among the three ORM-independent
candidates evaluated here, though still authored manually rather than
derived automatically the way EF Core's `Down()` is.

## Security

Requires its own credential configuration, separate from the application's
connection string, similar to Flyway.

## Developer Experience

Verbose changelog authoring (XML/YAML) is a genuinely new skill for the
team relative to C#-based EF Core Migrations, with a steeper learning curve
than either DbUp or Flyway's plain-SQL approach.

## Maintainability

Weaker than EF Core Migrations for the same structural reason as DbUp and
Flyway: nothing enforces that a changelog stays synchronized with the EF
Core model.

## CI/CD Integration

Well documented, but — as with Flyway — requires a distinct pipeline step,
tool installation, and credential separate from the .NET-native EF Core
Migrations integration already available at zero additional cost.

## AI Compatibility

Not applicable.

## Cloud Neutrality

Strongly database-engine-neutral, a genuine strength in principle but,
as with Flyway, not a requirement the platform currently has.

## Typical Usage

```yaml
# changelog/changelog-master.yaml
databaseChangeLog:
  - changeSet:
      id: add-asset-warranty-expiry-date
      author: solution-architect
      changes:
        - addColumn:
            tableName: Assets
            columns:
              - column:
                  name: WarrantyExpiryDate
                  type: date
      rollback:
        - dropColumn:
            tableName: Assets
            columnName: WarrantyExpiryDate
```

## Comparison with EF Core Migrations

| Aspect | Liquibase | EF Core Migrations |
|--------|-----------|----------------------|
| Source of truth | Changelog files (manual) | EF Core model (generated) |
| Runtime dependency | Java / standalone CLI | Already-present .NET SDK |
| Rollback | Declarative, per-changeset | Native `Down()` |
| Authoring format | XML / YAML / JSON | C# |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (Infrastructure-layer concern either way) |
| .NET / EF Core Integration | Poor (non-.NET tool, no model awareness) |
| Single Source of Truth | Poor |
| CI/CD Integration | Fair, adds a new tool and credential |
| License Stability | Good (Community Edition sufficient for core needs) |
| Migration Cost | High — full re-authoring plus new CI/CD tooling |

## Relationship with EF Core Migrations

As with DbUp and Flyway, Liquibase does not compose with EF Core
Migrations and would require discarding the model-derived consistency
guarantee entirely if adopted as the platform's migration mechanism.

## Preliminary Conclusion

Liquibase offers the strongest rollback story among the three
ORM-independent candidates, but shares the same fundamental architectural
mismatch as DbUp and Flyway: it is a non-.NET, model-unaware tool being
considered for a platform whose entity model — and therefore its
authoritative schema source of truth — already lives natively in EF Core.

---

# 9. Overall Technology Comparison

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|-----------------|------------------------|--------------|---------|
| Schema evolution (sole owner) | EF Core Migrations | — (no alternative approved) | Model-derived, versioned, reviewable schema changes |

## Capability Comparison

| Capability | EF Core Migrations | DbUp | Flyway | Liquibase |
|------------|----------------------|------|--------|-----------|
| Open Source (ADR-0002 compliant) | Yes | Yes | Community Edition only | Community Edition only |
| .NET-native, no extra runtime | Yes | Yes | No (Java/CLI) | No (Java/CLI) |
| Derived from EF Core model | Yes | No | No | No |
| Native rollback | Yes (`Down()`) | No | Community: No | Yes (declarative) |
| Authoring language | C# | Raw SQL | Raw SQL | XML/YAML/JSON |
| CI/CD integration effort | None (already present) | Low-Medium | Medium | Medium |

## Cloud Neutrality Assessment

All four candidates are cross-platform. Flyway and Liquibase offer
stronger multi-database-engine neutrality, which is not currently a
platform requirement given the SQL Server-only target defined in the
Architectural References.

## Enterprise Suitability

| Criterion | EF Core Migrations | DbUp | Flyway | Liquibase |
|-----------|----------------------|------|--------|-----------|
| Suitable for a homogeneous .NET/EF Core platform | Yes | Conditionally | No | No |
| Suitable for polyglot, multi-engine organizations | N/A | N/A | Yes | Yes |
| Introduces model/schema drift risk | No | Yes | Yes | Yes |

## Risk Assessment

| Risk | Affected Candidate | Severity |
|------|--------------------|----------|
| Model/schema drift (no compiler-enforced link between entity and schema) | DbUp, Flyway, Liquibase | High |
| Non-.NET runtime dependency in an otherwise .NET-native pipeline | Flyway, Liquibase | Medium |
| Commercial-tier feature gating | Flyway, Liquibase | Low–Medium |
| Two competing schema owners if adopted alongside EF Core | DbUp, Flyway, Liquibase | High |

## Overall Evaluation

EF Core Migrations is the only candidate that derives schema changes
directly from the same model that governs the platform's entities,
eliminating the model/schema drift risk shared by every ORM-independent
alternative. DbUp, Flyway, and Liquibase are all mature, credible tools in
general, but none offers a compensating architectural benefit large enough
to justify introducing a second schema-owning mechanism, additional
runtime dependencies, or (for Flyway and Liquibase) a non-.NET tool into an
otherwise homogeneous .NET/EF Core platform.

---

# 10. Final Recommendation

## Core Technology Stack

| Responsibility | Selected Technology | Rationale |
|-----------------|----------------------|-----------|
| Schema evolution (sole owner) | EF Core Migrations | Model-derived, zero additional tooling, native rollback, reaffirms TE-0024's governance rule |

## Recommended Architecture

```text
EF Core Model (Domain-driven entity shape)
        │
        ▼
EF Core Migrations (dotnet ef migrations add / database update)
        │
        ▼
SQL Server Database (single authoritative schema)
        ▲
        │
Dapper (TE-0024) — read-only, never alters schema
```

## Build Pipeline Integration

`dotnet ef database update` (or `Database.MigrateAsync()` guarded by
environment checks) is recommended as an explicit deployment step, to be
formalized in the forthcoming TE-0031 (Build, Packaging and Deployment
Technology Evaluation), requiring no new tool installation in the CI/CD
pipeline.

## Security Recommendations

Migrations continue to execute using the same, already-audited application
database credentials; no new credential surface is introduced.

## Cloud Neutrality

EF Core Migrations remain fully cross-platform for the .NET/SQL Server
combination already in use; multi-database-engine neutrality (Flyway's and
Liquibase's core strength) is not a current platform requirement.

## AI Readiness

Not applicable to this evaluation.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| EF Core Migrations | **Approved as sole schema owner** |
| DbUp | Rejected — reintroduces model/schema drift risk |
| Flyway | Rejected — non-.NET runtime dependency, model/schema drift risk |
| Liquibase | Rejected — non-.NET runtime dependency, model/schema drift risk |

---

# Related Architecture Decision

- ADR-0037 — Database Migration Strategy

---

# Decision Summary

- ✔ Clean Architecture preserved
- ✔ .NET 10 Compatibility
- ✔ Open Source First Policy (ADR-0002) compliance
- ✔ Single, model-derived source of truth for schema
- ✔ No new tooling, credentials, or runtime dependency introduced
- ✔ Directly fulfills the schema-ownership governance rule established in TE-0024

This evaluation formalizes EF Core Migrations as the platform's sole schema
evolution mechanism. Since ADR-0006 already implicitly assumed this and no
alternative is being adopted, no new ADR is required; this document itself
serves as the explicit record closing the open question deferred from
TE-0024.

---

# Related ADR

```
ADR-0006 (Reaffirmed — schema ownership clause formalized, no new ADR required)
```

---

# Related Documents

- TE-0004 — Entity Framework Core
- TE-0024 — Data Access Technology Evaluation
- ADR-0002 — Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- Dependency Catalog
- Dependency Rules

---

# References

https://learn.microsoft.com/ef/core/managing-schemas/migrations/

https://dbup.readthedocs.io/

https://flywaydb.org/

https://www.liquibase.org/

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-27 | Solution Architect | Initial evaluation; formalizes EF Core Migrations as sole schema owner, evaluates and rejects DbUp, Flyway, and Liquibase, closes the schema-ownership question deferred from TE-0024 |
| 1.3.0   | 2026-07-28 | New section added (Evaluation Scope)                                       |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |