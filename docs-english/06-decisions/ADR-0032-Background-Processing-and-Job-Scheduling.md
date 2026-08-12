| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ADR-0032           |
| **Title**        | Background Processing and Job Scheduling Architecture    |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-08-02         |
| **Last Updated** | 2026-08-08         |

---

# Context

The enterprise platform requires robust mechanisms for asynchronous processing, background task execution, and cron/interval job scheduling across both monolithic runtime nodes and distributed worker environments. Tasks include scheduled data maintenance, asynchronous message processing, report generation, and periodic synchronization.

Key requirements:
- Reliable recurring job execution (CRON expression support, interval triggers).
- Asynchronous light-weight in-memory background processing for decoupled messaging.
- Support for persistent job stores (SQLite for local/standalone workspace nodes, PostgreSQL/SQL Server for central cloud clusters) to guarantee job execution across application restarts.
- Full integration with .NET 10 Hosted Services (`IHostedService` / `BackgroundService`), OpenTelemetry, and Serilog logging.
- Compliance with Open Source First Policy (ADR-0002).

---

# Decision

1. **Adopt Quartz.NET** as the primary enterprise job scheduling engine for scheduled, recurring, and persistent background jobs (formalizing **TE-0014** and **TE-0019**).
2. **Adopt System.Threading.Channels** for high-throughput, in-memory, producer-consumer background queues within individual application instances where persistent scheduling is not required.
3. Configure Quartz.NET to use:
   - **SQLite JobStore / ADO.NET JobStore** in offline/local desktop workspace environments.
   - **PostgreSQL / SQL Server JobStore** in clustered server deployments for distributed lock management and fault-tolerant job dispatching.
4. Integrate Quartz.NET jobs with .NET 10 Dependency Injection via `IJobFactory` and standard OpenTelemetry instrumentation.

---

# Decision Drivers

- **Feature Completeness:** Quartz.NET provides mature CRON scheduling, misfire handling, persistent job state, and clustering.
- **Flexibility:** Supports in-memory storage for lightweight deployments and database-backed JobStore for resilient enterprise operations.
- **Open Source First Policy:** Quartz.NET is fully open-source (Apache 2.0 license) without commercial paywalls (unlike Hangfire Pro features).
- **Native .NET Integration:** Seamless alignment with `IHostedService` and ASP.NET Core lifecycle.

---

# Alternatives Considered

- **Hangfire:** Excellent UI and API, but core enterprise features (e.g., multi-tenancy, batch jobs) require paid commercial licenses (Hangfire Pro), violating ADR-0002.
- **Native `BackgroundService` with `System.Threading.Timer`:** Lightweight but lacks persistent job store, CRON syntax parsing, misfire policies, and cluster coordination.
- **Coravel:** Simple and lightweight, but lacks enterprise-grade job clustering, complex CRON scheduling, and mature persistent storage backends required for enterprise mission-critical workloads.

---

# Consequences

### Positive
- Unified, enterprise-grade job scheduling architecture across local workspaces and server clusters.
- Zero commercial licensing costs (fully open source under Apache 2.0).
- Durable job execution guarantees with persistent JobStores.
- Native integration with platform telemetry and logging.

### Negative
- Requires database table schema creation for ADO.NET JobStore in clustered mode.
- Requires proper handling of job concurrency and state serialization.

---

# Related Decisions & Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0003 — Use .NET 10
- ADR-0009 — Use Serilog
- ADR-0010 — Use OpenTelemetry
- TE-0014 — Background Processing Technology Evaluation
- TE-0019 — Background Processing and Job Scheduling Technology Evaluation
- Dependency Catalog

---

# Revision History

| Version | Date       | Author             | Description                                    |
|---------|------------|--------------------|------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial version                                |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes |