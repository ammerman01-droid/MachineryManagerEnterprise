| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ADR-0031           |
| **Title**        | Enterprise Caching Architecture |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-08-02         |
| **Last Updated** | 2026-08-08         |

---

# Context

To maintain sub-second response times, minimize database load for read-heavy operations, and support multi-tenant offline-first and cloud deployments, the platform requires a cohesive multi-tiered caching architecture.

Key requirements:
- Tier-1 L1 In-Memory Caching (fast, zero-network latency for single node / desktop workspace).
- Tier-2 L2 Distributed Caching (shared cache across clustered application servers).
- Prevention of Cache Stampede (Thundering Herd problem), Cache Throttling, and Cache Invalidation complexity.
- Support for .NET 10 `HybridCache` / `FusionCache` primitives with OpenTelemetry metrics and Serilog instrumentation.
- Alignment with Open Source First Policy (ADR-0002).

---

# Decision

1. **Adopt FusionCache / .NET 10 HybridCache** as the standard L1/L2 multi-tier caching abstraction framework (formalizing **TE-0015**).
2. **Adopt `IMemoryCache` / `FusionCache` L1** for local in-process memory caching in all standalone desktop applications and API services.
3. **Adopt Redis (via StackExchange.Redis / IDistributedCache)** as the standard L2 distributed cache provider for multi-node server cluster environments.
4. Enforce fail-safe caching patterns:
   - Built-in stampede protection via lock synchronization.
   - Fail-safe fallback execution if L2 distributed cache becomes unavailable.
   - Tag-based or key-prefix invalidation strategies across domain aggregates.

---

# Decision Drivers

- **Performance:** Sub-millisecond read access for L1 in-memory tier and high throughput for L2 distributed tier.
- **Resilience:** Stampede protection and fail-safe operation prevent database overload during cache misses or Redis outages.
- **Flexibility:** Seamless operation in standalone single-user desktop workspaces (L1 only) and enterprise cloud clusters (L1 + L2 Redis).
- **Open Source First:** Fully open-source ecosystem without proprietary software constraints.

---

# Alternatives Considered

- **Raw `IMemoryCache` / `IDistributedCache` without abstraction:** Requires writing boilerplate code for lock synchronization, cache stampede mitigation, serialization, and multi-tier coordination across every repository/service.
- **Memcached:** Lacks rich data structures, persistence options, and active pub/sub invalidation capabilities compared to Redis.
- **NCache:** Commercial product with proprietary licensing, violating ADR-0002.

---

# Consequences

### Positive
- Unified, robust multi-tiered caching model across desktop and cloud workloads.
- Elimination of cache stampede issues under heavy concurrent load.
- Seamless fallback and graceful degradation when distributed cache tiers experience latency or disconnects.
- Rich telemetry and hit/miss ratio tracking via OpenTelemetry.

### Negative
- Additional operational component (Redis cluster) required for distributed server deployments.
- Requires careful design of cache key naming conventions and TTL policies.

---

# Related Decisions & Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0003 — Use .NET 10
- ADR-0006 — Use Entity Framework Core
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- TE-0015 — Caching Architecture Technology Evaluation
- Dependency Catalog

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial version                                       |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |