| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ADR-0037           |
| **Title**        | Database Migration Strategy |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-08-02         |
| **Last Updated** | 2026-08-08         |

---

# Context

ADR-0006 approved Entity Framework Core as the platform's ORM (based on
TE-0004), and ADR-0019 established a hybrid persistence strategy adding
Dapper for read-heavy queries (based on TE-0024). Neither ADR addressed
schema evolution and migration ownership. TE-0025 — Database Migration
Technology Evaluation was approved to close this gap but had no
corresponding Architecture Decision Record.

---

# Decision

MachineryManagerEnterprise adopts **EF Core Migrations** as the sole
owner of schema evolution, formalizing TE-0025:

| Responsibility | Selected Technology |
|-----------------|---------------------|
| Schema evolution (sole owner) | EF Core Migrations |

EF Core Migrations shall be generated from the EF Core domain model
(`dotnet ef migrations add`) and applied via `dotnet ef database update`
or `Database.MigrateAsync()`, guarded by environment checks, as an
explicit deployment step. Dapper (per ADR-0019 / TE-0024) remains
strictly read-only and shall never alter schema.

---

# Decision Drivers

- Reaffirms TE-0024's governance rule (Dapper is read-only)
- Model-derived migrations require no additional tooling
- Native rollback support
- Consistency with the already-approved EF Core standard (ADR-0006)

---

# Alternatives Considered

Flyway and Liquibase were evaluated in TE-0025 for cross-database-engine
neutrality, which is not a current platform requirement since the
platform standardizes on SQL Server. Refer to TE-0025 for the full
comparison.

---

# Consequences

**Positive**

- Single, authoritative source of schema truth (the EF Core model).
- No new credential surface; migrations use existing, already-audited
  database credentials.

**Negative / Trade-offs**

- Multi-database-engine portability (a strength of Flyway/Liquibase) is
  not available; the platform is committed to SQL Server for schema
  ownership.

---

# Architecture Impact

- Infrastructure layer only. Dapper-based read queries (ADR-0019) are
  unaffected and remain read-only against the EF Core-owned schema.

---

# Implementation Notes

- `dotnet ef database update` (or `Database.MigrateAsync()`) shall run
  as an explicit, environment-guarded deployment step, to be formalized
  further in the platform's Build and Deployment Architecture
  (ADR-0025).

---

# Compliance Rules

```
Only EF Core Migrations shall alter database schema. Dapper queries
(ADR-0019) shall never contain DDL statements.
```

---

# Related Technology Evaluation

```
TE-0025 (reaffirms TE-0024's governance rule)
```

---

# Related Proof of Concept

```
Not Required
```

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Cloud Neutrality
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0006 — Use Entity Framework Core
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- TE-0025 — Database Migration Technology Evaluation

---

# References

- EF Core Migrations Documentation

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial decision, formalizing previously unratified TE-0025 |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes |