| Property | Value |
|----------|-------|
| **Document ID** | TE-0015 |
| **Title** | Caching Architecture Technology Evaluation (.NET 10) |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-26 |
| **Last Updated** | 2026-07-28 |

---

# Purpose

This document evaluates candidate technologies for Caching Architecture Technology Evaluation (.NET 10) in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0016 — Enterprise Messaging Architecture
- ADR-0017 — Artificial Intelligence Integration
- ADR-0018 — External Integration Architecture

The resulting caching architecture shall remain:

- deployment independent;
- provider independent;
- infrastructure isolated;
- cloud neutral.

---

# Functional Requirements

The platform requires caching support for:

- application data;
- frequently accessed reference data;
- configuration;
- permissions;
- user preferences;
- AI prompt caching;
- AI embedding caching;
- semantic retrieval caching;
- reporting;
- dashboard data;
- session optimization.

---

# Architecture Principle

The evaluated component operates as an isolated infrastructure service in accordance with Clean Architecture principles and domain isolation rules.

---

# Non-Functional Requirements

The caching platform should provide:

- high performance;
- provider independence;
- distributed deployment support;
- local deployment support;
- cloud deployment support;
- hybrid deployment support;
- low operational complexity;
- observability;
- scalability;
- maintainability.

---

# Candidate Technologies

Unlike earlier .NET releases, the candidates are divided by architectural responsibility.

## Cache Abstraction Layer

| Technology | Role |
|------------|------|
| Microsoft HybridCache (.NET 10) | Unified Application Cache API |

---

## Local Cache

| Technology | Role |
|------------|------|
| IMemoryCache | In-Process Cache |

---

## Distributed Cache Providers

| Technology | Role |
|------------|------|
| Redis | Distributed Cache Backend |
| NCache | Enterprise Distributed Cache |

---

## Response Cache

| Technology | Role |
|------------|------|
| ASP.NET Core Output Cache (.NET 10) | HTTP Response Cache |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| C1 | Clean Architecture Compatibility | Critical |
| C2 | Provider Independence | Critical |
| C3 | Performance | High |
| C4 | Deployment Flexibility | High |
| C5 | Scalability | High |
| C6 | Operational Complexity | Medium |
| C7 | Enterprise Readiness | High |
| C8 | Maintainability | High |
| C9 | AI Compatibility | High |
| C10 | Future Extensibility | High |

---

# Architectural Principle

The evaluation follows one fundamental architectural rule:

Application code must never depend directly upon a cache implementation.

Instead, application services interact only with the cache abstraction layer.

The cache abstraction is then free to utilize:

- local memory;
- distributed cache;
- hybrid cache;
- future providers.

This architectural separation preserves provider independence and deployment flexibility.

---

# 5. Microsoft HybridCache (.NET 10)

## Overview

HybridCache is the unified caching abstraction introduced by Microsoft for modern ASP.NET Core applications.

Rather than acting as a cache implementation, HybridCache provides a consistent application-facing API capable of orchestrating multiple cache layers transparently.

Its primary objective is to simplify application code while allowing different cache providers to be combined underneath.

The application therefore interacts with a single cache abstraction while HybridCache coordinates:

- local in-memory cache;
- distributed cache;
- serialization;
- stampede protection;
- cache population.

---

## Architectural Role

HybridCache is **not** a distributed cache.

HybridCache is **not** a replacement for Redis.

HybridCache is an orchestration layer positioned between the application and cache providers.

```text
Application

      │

      ▼

HybridCache

      │

 ┌───────────────┐
 │ Memory Cache  │
 └───────────────┘
      │
      ▼
Distributed Cache
(Redis / NCache / Future Provider)
```

This architecture significantly reduces coupling between business logic and infrastructure.

---

## Architectural Strengths

### Advantages

- Native .NET 10 implementation.
- Official Microsoft support.
- Unified programming model.
- Built-in stampede protection.
- Automatic cache coordination.
- Provider independence.
- Excellent Dependency Injection integration.
- Simple API.
- High developer productivity.
- Excellent maintainability.
- Future-proof architecture.

---

## Architectural Weaknesses

HybridCache intentionally delegates storage responsibilities.

Therefore:

- a distributed provider is still required for distributed deployments;
- infrastructure configuration remains necessary;
- persistence depends entirely on the selected backend.

These characteristics are architectural design decisions rather than limitations.

---

## Operational Characteristics

HybridCache provides:

- asynchronous API;
- cache orchestration;
- cache population coordination;
- automatic concurrency protection;
- serialization pipeline;
- local/distributed coordination.

Operational complexity is considered very low.

---

## Scalability

HybridCache itself introduces negligible scalability limitations.

Overall scalability depends upon the selected distributed cache provider.

This separation aligns well with enterprise architecture principles.

---

## Security

Security responsibilities remain delegated to:

- the hosting environment;
- the distributed cache provider;
- application authorization.

HybridCache introduces no additional attack surface.

---

## Deployment Flexibility

HybridCache supports:

- Desktop
- Mobile
- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- On-Premise
- Hybrid

Deployment behavior adapts automatically to the configured providers.

---

## AI Compatibility

HybridCache is particularly well suited for AI-enabled enterprise systems.

Potential cache targets include:

- prompt templates;
- embedding vectors;
- semantic retrieval results;
- AI configuration;
- model metadata;
- inference metadata.

This minimizes repeated computation and significantly reduces response latency.

---

## Maintainability

HybridCache strongly aligns with:

- Clean Architecture;
- Dependency Inversion;
- Infrastructure Isolation;
- Provider Independence.

Business modules remain completely unaware of:

- Redis;
- NCache;
- future cache technologies.

Only the abstraction is visible.

---

## Suitability for MachineryManagerEnterprise

HybridCache satisfies all architectural objectives established for the platform.

It enables a single implementation capable of supporting:

- standalone desktop deployments;
- mobile clients;
- enterprise servers;
- cloud deployments;
- hybrid installations.

This flexibility directly supports the deployment strategy defined in ADR-0017.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Provider Independence | Excellent |
| Performance | Excellent |
| Deployment Flexibility | Excellent |
| AI Compatibility | Excellent |
| Operational Simplicity | Excellent |
| Maintainability | Excellent |
| Enterprise Readiness | Excellent |

---

## Preliminary Conclusion

HybridCache should become the standard caching abstraction used throughout MachineryManagerEnterprise.

Business modules shall never interact directly with Redis, NCache or any future cache implementation.

Instead, all application services should depend solely upon the HybridCache abstraction.

This approach provides maximum architectural flexibility while minimizing future migration effort.

---

# 6. IMemoryCache Evaluation

## Overview

IMemoryCache is the built-in in-process caching implementation provided by Microsoft for ASP.NET Core.

Unlike distributed cache providers, IMemoryCache stores objects exclusively within the current process memory.

It provides:

- extremely fast access;
- low latency;
- zero network overhead;
- simple configuration.

However, its lifetime is limited to a single application instance.

---

## Architectural Role

IMemoryCache represents the **Level-1 Cache** of the platform.

It should never be treated as the primary enterprise cache.

Instead, under the .NET 10 architecture it operates beneath HybridCache.

```text
Application

      │

      ▼

HybridCache

      │

      ▼

IMemoryCache

(Process Local)
```

When configured together with a distributed provider, HybridCache automatically manages the interaction between the local cache and distributed cache.

---

## Architectural Strengths

### Advantages

- Native .NET implementation.
- Extremely low latency.
- Zero network traffic.
- No external infrastructure.
- Excellent Dependency Injection support.
- Very simple configuration.
- High throughput.
- Excellent object access performance.

---

## Architectural Weaknesses

IMemoryCache is intentionally limited.

Major limitations include:

- no persistence;
- process-local only;
- unsuitable for distributed systems;
- cache lost after restart;
- inconsistent data across multiple application instances.

Consequently, IMemoryCache alone is insufficient for enterprise deployments.

---

## Operational Characteristics

IMemoryCache provides:

- object caching;
- absolute expiration;
- sliding expiration;
- eviction policies;
- memory pressure management.

Operational complexity is extremely low.

---

## Scalability

IMemoryCache scales only vertically.

Each application instance owns an independent cache.

Consequently:

- cache synchronization does not exist;
- horizontal scaling produces multiple independent caches;
- shared enterprise state cannot be maintained.

---

## Security

Since all cached data remains inside the process memory:

- no external attack surface exists;
- security depends entirely on application security.

---

## Deployment Flexibility

IMemoryCache supports:

- Desktop
- Mobile
- Windows
- Linux
- Containers
- Kubernetes

However, distributed deployments require an additional cache provider.

---

## AI Compatibility

IMemoryCache is highly suitable for temporary AI artifacts including:

- prompt templates;
- model metadata;
- temporary embedding results;
- inference metadata;
- frequently reused configuration.

Long-term semantic data should remain inside the distributed cache.

---

## Maintainability

IMemoryCache has:

- stable APIs;
- excellent Microsoft support;
- outstanding documentation;
- minimal maintenance effort.

Maintainability is considered excellent.

---

## Suitability for MachineryManagerEnterprise

IMemoryCache is an essential component of the caching architecture.

However, it should never be used directly by business modules.

Instead:

Business Modules

↓

HybridCache

↓

IMemoryCache

This preserves provider independence while allowing future replacement without affecting application code.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Provider Independence | Excellent (through HybridCache) |
| Performance | Excellent |
| Distributed Deployment | Poor |
| Operational Simplicity | Excellent |
| Maintainability | Excellent |
| Enterprise Readiness | Moderate |

---

## Relationship with HybridCache

IMemoryCache is not an architectural alternative to HybridCache.

Instead:

HybridCache orchestrates

↓

IMemoryCache

↓

Distributed Cache

This layered design provides:

- fastest possible reads;
- reduced network traffic;
- transparent cache population;
- simplified application code.

---

## Preliminary Conclusion

IMemoryCache should be adopted as the Level-1 local cache of MachineryManagerEnterprise.

Application code shall not depend directly upon IMemoryCache.

All cache operations should be performed through HybridCache, allowing the runtime to coordinate local and distributed caching transparently.

The combination of HybridCache and IMemoryCache provides the optimal foundation for high-performance local caching while preserving enterprise architectural principles.

---

# 7. Redis Evaluation

## Overview

Redis is an in-memory data platform widely used as the industry standard for distributed caching.

Within the .NET 10 caching architecture Redis is **not** used directly by business modules.

Instead, Redis operates as the primary **Level-2 Distributed Cache Provider** beneath HybridCache.

Typical enterprise responsibilities include:

- distributed cache;
- shared cache state;
- high-speed key/value storage;
- cache synchronization across application instances;
- AI semantic cache;
- distributed session state.

---

## Architectural Role

Redis occupies the distributed cache layer of the architecture.

```text
Business Modules

        │

        ▼

HybridCache

        │

 ┌───────────────┐
 │ IMemoryCache  │
 └───────────────┘

        │

        ▼

Redis

(Level-2 Distributed Cache)
```

Business modules never communicate directly with Redis.

Infrastructure remains fully isolated behind HybridCache.

---

## Architectural Strengths

### Advantages

- Extremely high performance.
- Mature distributed cache platform.
- Proven enterprise adoption.
- Excellent horizontal scalability.
- Cross-platform.
- Cloud neutral.
- Rich .NET ecosystem.
- Supports clustering.
- Supports replication.
- Supports high availability.
- Excellent Docker support.
- Excellent Kubernetes support.

---

## Architectural Weaknesses

Redis introduces additional infrastructure requiring:

- deployment;
- monitoring;
- backup strategy;
- operational maintenance.

Being memory-first, persistence should be configured appropriately depending on deployment requirements.

Redis is infrastructure rather than an application framework.

---

## Operational Characteristics

Redis provides:

- distributed key/value cache;
- replication;
- clustering;
- expiration policies;
- eviction policies;
- persistence options;
- high throughput.

Operational complexity is considered moderate.

---

## Scalability

Redis demonstrates excellent scalability.

Supported deployment models include:

- standalone;
- replication;
- sentinel;
- cluster;
- managed cloud services.

Redis comfortably supports enterprise-scale workloads.

---

## Security

Enterprise deployments should enable:

- authentication;
- TLS encryption;
- network isolation;
- access control.

Redis itself provides the necessary mechanisms.

---

## Deployment Flexibility

Redis supports:

- Windows (development scenarios)
- Linux
- Containers
- Kubernetes
- Cloud
- On-Premise
- Hybrid

Redis therefore satisfies all deployment objectives established by the architecture.

---

## AI Compatibility

Redis is well suited for AI-enabled systems.

Typical cached artifacts include:

- embedding vectors;
- semantic search results;
- prompt templates;
- inference metadata;
- model configuration;
- retrieval results.

Reducing repeated AI computation significantly lowers latency and cloud cost.

---

## Maintainability

Redis has:

- exceptional community adoption;
- long-term stability;
- mature operational tooling;
- extensive documentation.

Maintainability is considered excellent.

---

## Suitability for MachineryManagerEnterprise

Redis satisfies every distributed caching requirement identified during architectural analysis.

Its role includes:

- shared application cache;
- distributed AI cache;
- cross-node synchronization;
- high-performance read optimization;
- scalable distributed execution.

Redis integrates naturally with:

- HybridCache;
- Hangfire;
- RabbitMQ;
- Semantic Kernel.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Provider Independence | Excellent (through HybridCache) |
| Performance | Excellent |
| Distributed Deployment | Excellent |
| Operational Simplicity | Very Good |
| Enterprise Readiness | Excellent |
| Maintainability | Excellent |

---

## Relationship with HybridCache

Redis is **not** an alternative to HybridCache.

Instead:

HybridCache

↓

Redis

HybridCache determines:

- cache population;
- local/distributed coordination;
- concurrency protection.

Redis provides:

- distributed storage;
- shared cache state;
- scalable retrieval.

This separation produces a significantly cleaner architecture than direct Redis access.

---

## Relationship with IMemoryCache

Redis complements IMemoryCache.

```text
HybridCache

        │

 ┌───────────────┐
 │ IMemoryCache  │
 └───────────────┘

        │

        ▼

Redis
```

Level-1

↓

Fast process-local access.

Level-2

↓

Shared enterprise cache.

This architecture minimizes network traffic while preserving consistency across distributed deployments.

---

## Preliminary Conclusion

Redis should be adopted as the standard distributed cache provider for MachineryManagerEnterprise.

However, Redis should remain an infrastructure component rather than an application dependency.

Application code shall access caching exclusively through HybridCache.

This approach preserves:

- Clean Architecture;
- provider independence;
- deployment flexibility;
- future extensibility.

Redis therefore represents the preferred distributed cache backend for the platform.

---

# 8. NCache Evaluation

## Overview

NCache is a commercial distributed caching platform developed specifically for enterprise .NET applications.

Unlike Redis, which is a general-purpose in-memory data platform, NCache focuses entirely on distributed caching for .NET workloads.

Typical enterprise scenarios include:

- distributed application cache;
- distributed sessions;
- ASP.NET Core applications;
- cloud deployments;
- enterprise data grids.

---

## Architectural Role

Within MachineryManagerEnterprise, NCache occupies exactly the same architectural layer as Redis.

```text
Business Modules

        │

        ▼

HybridCache

        │

 ┌───────────────┐
 │ IMemoryCache  │
 └───────────────┘

        │

        ▼

NCache

(Level-2 Distributed Cache)
```

Business modules remain completely unaware of NCache.

---

## Architectural Strengths

### Advantages

- Native .NET implementation.
- Enterprise-focused architecture.
- Distributed cache.
- High availability.
- Cache clustering.
- Replication.
- Good ASP.NET Core integration.
- Rich monitoring capabilities.
- Commercial support available.
- Good management tools.

---

## Architectural Weaknesses

Compared with Redis:

- Smaller community.
- Commercial licensing for enterprise editions.
- Smaller ecosystem.
- Fewer third-party integrations.
- Less adoption outside the Microsoft ecosystem.

---

## Operational Characteristics

NCache provides:

- distributed cache;
- replication;
- clustering;
- partitioning;
- monitoring;
- cache synchronization.

Operational complexity is considered moderate.

---

## Scalability

NCache scales well across multiple servers.

It supports:

- replicated cache;
- partitioned cache;
- mirrored cache;
- clustered deployments.

Its scalability is appropriate for enterprise applications.

---

## Security

NCache supports:

- authentication;
- encrypted communication;
- role-based administration;
- secure clustering.

Enterprise security capabilities are strong.

---

## Deployment Flexibility

Supported environments include:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- On-Premise

Deployment flexibility is excellent.

---

## AI Compatibility

From an AI perspective, NCache provides no significant functional advantage over Redis.

Typical AI cache objects include:

- embedding vectors;
- retrieval results;
- semantic metadata;
- prompt templates.

Both Redis and NCache satisfy these requirements equally well.

---

## Maintainability

NCache demonstrates:

- stable APIs;
- enterprise support;
- long product history.

However:

- community documentation is smaller than Redis;
- external ecosystem is more limited.

Maintainability is considered very good.

---

## Suitability for MachineryManagerEnterprise

NCache satisfies all technical requirements.

However, no architectural capability required by MachineryManagerEnterprise is uniquely provided by NCache.

Every identified use case can also be implemented using Redis.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Provider Independence | Excellent (through HybridCache) |
| Performance | Excellent |
| Distributed Deployment | Excellent |
| Enterprise Readiness | Excellent |
| Operational Simplicity | Very Good |
| Maintainability | Very Good |

---

## Comparison with Redis

| Capability | Redis | NCache |
|------------|-------|---------|
| Performance | Excellent | Excellent |
| Enterprise Features | Excellent | Excellent |
| Community | Excellent | Good |
| Ecosystem | Excellent | Moderate |
| .NET Integration | Very Good | Excellent |
| Licensing | Open Source | Commercial (Enterprise) |
| Cloud Adoption | Excellent | Good |

---

## Architectural Assessment

The architectural abstraction defined by HybridCache makes Redis and NCache interchangeable.

```text
Business

      │

      ▼

HybridCache

      │

 ┌──────────────┐
 │ Redis        │
 │ or           │
 │ NCache       │
 └──────────────┘
```

Therefore, changing providers requires only infrastructure configuration changes rather than application changes.

---

## Preliminary Conclusion

NCache is an excellent enterprise distributed cache.

Nevertheless, Redis offers:

- broader ecosystem;
- wider community adoption;
- larger operational knowledge base;
- lower vendor dependency.

Consequently, Redis remains the preferred distributed cache backend for MachineryManagerEnterprise.

NCache remains a fully supported alternative when enterprise licensing or organizational standards require its adoption.

---

# 9. ASP.NET Core Output Cache (.NET 10)

## Overview

Output Cache is the modern HTTP response caching framework introduced for ASP.NET Core and further enhanced in .NET 10.

Unlike HybridCache, IMemoryCache, Redis or NCache, Output Cache does **not** cache application objects.

Instead, it caches fully generated HTTP responses.

Its purpose is to reduce repeated endpoint execution and significantly improve response latency for cacheable resources.

Typical use cases include:

- read-only APIs;
- lookup endpoints;
- dashboard queries;
- catalog data;
- public resources;
- reference information.

---

## Architectural Role

Output Cache belongs to the Presentation Layer.

It is **not** part of the application caching architecture.

```text
                HTTP Request

                     │

                     ▼

             ASP.NET Core Endpoint

                     │

              Output Cache Layer

                     │

          Cached HTTP Response

                     │

                     ▼

                 Client
```

Output Cache should therefore be viewed as an optimization mechanism for API responses rather than a general-purpose cache.

---

## Architectural Strengths

### Advantages

- Native .NET 10 implementation.
- Extremely high performance.
- Minimal configuration.
- Response-level caching.
- Built-in cache policies.
- Excellent ASP.NET Core integration.
- Dependency Injection support.
- Endpoint-level configuration.
- Reduced CPU utilization.
- Reduced database load.
- Reduced AI inference load.

---

## Architectural Weaknesses

Output Cache intentionally caches only HTTP responses.

It cannot replace:

- HybridCache;
- IMemoryCache;
- Redis;
- NCache.

It is unsuitable for:

- business object caching;
- distributed application state;
- AI embedding storage;
- semantic retrieval cache.

---

## Operational Characteristics

Output Cache provides:

- endpoint caching;
- cache invalidation;
- cache policies;
- expiration;
- vary-by-query;
- vary-by-route;
- vary-by-header;
- vary-by-user.

Operational complexity is extremely low.

---

## Scalability

Output Cache scales naturally together with ASP.NET Core.

When used together with distributed caching infrastructure, cached responses remain highly efficient across multiple application instances.

---

## Security

Output Cache must never cache responses containing:

- user-specific data;
- authorization-dependent resources;
- confidential information;
- security tokens.

Appropriate cache policies must therefore be defined.

---

## Deployment Flexibility

Output Cache supports:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- On-Premise
- Hybrid

No deployment restrictions exist.

---

## AI Compatibility

Output Cache is particularly valuable for AI-enabled endpoints.

Examples include:

- AI recommendations;
- semantic search results;
- document summaries;
- report generation;
- AI-assisted lookup APIs.

Caching complete responses significantly reduces repeated LLM inference costs.

---

## Maintainability

Output Cache demonstrates:

- excellent Microsoft support;
- native framework integration;
- low maintenance effort;
- long-term platform stability.

Maintainability is considered excellent.

---

## Suitability for MachineryManagerEnterprise

Output Cache should be adopted for read-oriented endpoints where:

- results change infrequently;
- response generation is expensive;
- AI inference cost should be minimized.

Typical candidates include:

- dashboard APIs;
- lookup APIs;
- reporting endpoints;
- AI-assisted read operations.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Performance | Excellent |
| Deployment Flexibility | Excellent |
| Operational Simplicity | Excellent |
| Enterprise Readiness | Excellent |
| AI Optimization | Excellent |
| Maintainability | Excellent |

---

## Relationship with the Caching Architecture

Output Cache complements—not replaces—the application caching stack.

```text
Presentation Layer

        │

        ▼

Output Cache

        │

        ▼

Application Layer

        │

        ▼

HybridCache

        │

 ┌───────────────┐
 │ IMemoryCache  │
 └───────────────┘

        │

        ▼

Redis
```

Each layer serves a different responsibility:

| Layer | Responsibility |
|--------|----------------|
| Output Cache | HTTP Response Caching |
| HybridCache | Application Cache Abstraction |
| IMemoryCache | Local Object Cache |
| Redis | Distributed Object Cache |

This separation follows the Single Responsibility Principle and results in a highly maintainable architecture.

---

## Preliminary Conclusion

ASP.NET Core Output Cache should be adopted as the standard response caching mechanism for MachineryManagerEnterprise.

It should complement the HybridCache architecture rather than compete with it.

The combination of Output Cache, HybridCache, IMemoryCache and Redis provides a complete multi-layer caching strategy covering:

- HTTP responses;
- application objects;
- distributed state;
- AI-generated responses.

This layered architecture fully aligns with the architectural principles defined by the approved ADRs.

---

# 10. Overall Technology Comparison

Unlike previous .NET releases, .NET 10 clearly separates caching responsibilities into multiple architectural layers.

Therefore, the evaluated technologies are **not direct competitors**. Each technology occupies a different architectural responsibility.

---

## Layered Comparison

| Layer | Recommended Technology | Alternative | Responsibility |
|------|-------------------------|-------------|----------------|
| Response Cache | ASP.NET Core Output Cache (.NET 10) | None | HTTP Response Caching |
| Cache Abstraction | HybridCache (.NET 10) | None | Unified Application Cache API |
| Local Cache | IMemoryCache | None | Level-1 Process Cache |
| Distributed Cache | Redis | NCache | Level-2 Shared Cache |

---

## Enterprise Comparison

| Criterion | HybridCache | Redis | NCache | Output Cache |
|-----------|-------------|-------|---------|--------------|
| Clean Architecture | Excellent | Excellent (via HybridCache) | Excellent (via HybridCache) | Excellent |
| Performance | Excellent | Excellent | Excellent | Excellent |
| Distributed Support | Excellent | Excellent | Excellent | N/A |
| AI Compatibility | Excellent | Excellent | Excellent | Very Good |
| Operational Complexity | Low | Moderate | Moderate | Very Low |
| Enterprise Readiness | Excellent | Excellent | Excellent | Excellent |
| Long-Term Maintainability | Excellent | Excellent | Very Good | Excellent |

---

# 11. Recommended Caching Architecture

The evaluation concludes that the optimal caching architecture for MachineryManagerEnterprise is a layered model rather than a single technology.

```text
                    Client

                      │

                      ▼

      ASP.NET Core Output Cache (.NET 10)

                      │

                      ▼

              Application Services

                      │

                      ▼

         Microsoft HybridCache (.NET 10)

                      │

          ┌────────────────────────┐
          │                        │
          ▼                        ▼

   IMemoryCache             Redis (Primary)
 (Level-1 Local)         (Level-2 Distributed)

                               │

                               ▼

                     NCache (Alternative)
```

---

# 12. Architectural Responsibilities

## Output Cache

Responsible for:

- HTTP response caching;
- endpoint optimization;
- reducing repeated endpoint execution.

---

## HybridCache

Responsible for:

- application cache abstraction;
- provider orchestration;
- cache stampede protection;
- unified programming model.

---

## IMemoryCache

Responsible for:

- process-local object cache;
- hot object storage;
- lowest-latency reads.

---

## Redis

Responsible for:

- distributed shared cache;
- enterprise cache consistency;
- cross-node synchronization;
- AI semantic cache.

---

## NCache

Optional replacement for Redis when organizational standards require a commercial .NET-native distributed cache.

---

# 13. Architectural Principles

The selected architecture satisfies all major architectural principles.

| Principle | Assessment |
|-----------|------------|
| Clean Architecture | ✓ |
| Dependency Inversion | ✓ |
| Provider Independence | ✓ |
| Deployment Independence | ✓ |
| Enterprise Scalability | ✓ |
| AI Readiness | ✓ |
| Maintainability | ✓ |

---

# 14. Decision

The following decisions are adopted.

| Decision | Selected Technology |
|----------|---------------------|
| HTTP Response Cache | ASP.NET Core Output Cache (.NET 10) |
| Cache Abstraction | HybridCache (.NET 10) |
| Local Cache | IMemoryCache |
| Distributed Cache | Redis |
| Enterprise Alternative | NCache |

---

# 15. Decision Rationale

The selected stack provides the strongest balance between:

- architectural quality;
- enterprise maintainability;
- deployment flexibility;
- operational simplicity;
- cloud neutrality;
- AI readiness;
- long-term evolution.

Most importantly, the architecture prevents any application component from depending upon a specific cache implementation.

Future migration from Redis to another distributed cache (or vice versa) requires only infrastructure configuration changes.

---

# 16. Risks

| Risk | Mitigation |
|------|------------|
| Distributed cache outage | Local cache continues serving hot objects where appropriate; implement graceful degradation and retry policies. |
| Cache inconsistency | HybridCache coordination, appropriate expiration policies, and explicit invalidation strategies. |
| Excessive memory consumption | Configure size limits, expiration, and eviction policies. |
| Cache stampede | HybridCache built-in stampede protection. |
| Vendor migration | Provider abstraction through HybridCache. |

---

# 17. Decision Impact

The selected architecture enables future capabilities without architectural redesign, including:

- Retrieval-Augmented Generation (RAG);
- AI response caching;
- semantic search optimization;
- distributed application scaling;
- high-performance dashboard rendering;
- intelligent reporting;
- offline deployments;
- hybrid cloud deployments.

---

# 18. Final Recommendation

The platform should standardize on the following caching architecture:

- ASP.NET Core Output Cache (.NET 10) for HTTP response caching.
- HybridCache (.NET 10) as the single application-facing cache abstraction.
- IMemoryCache as the Level-1 in-process cache.
- Redis as the default distributed Level-2 cache.
- NCache as an approved enterprise alternative where organizational standards or licensing requirements justify its use.

No application service shall communicate directly with Redis, NCache or IMemoryCache.

All caching operations shall pass through HybridCache.

This decision preserves complete infrastructure isolation and aligns with the architectural principles established for MachineryManagerEnterprise.

---





# Overall Technology Comparison

The selected technology provides optimal performance, maintainability, and Clean Architecture compatibility.

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative |
|-----------------|------------------------|-------------|
| System Capability | Primary Selected | Evaluated Option |

---

# Final Recommendation

Adopt the selected technology as the official platform standard for MachineryManagerEnterprise.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| Primary Selected Technology | Approved |

---

# Decision Summary

The selected technology stack satisfies all architectural requirements.

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---


# Related ADR

- ADR-0001 — Clean Architecture
- ADR-0015 — Deployment Architecture

---


# Related Documents

- SolutionStructure.md
- DependencyRules.md
- CodingStandards.md

---

# Revision History

| Version | Date       | Author             | Description                                            |
|---------|------------|--------------------|--------------------------------------------------------|
| 1.0.0   | 2026-07-26 | Solution Architect | Initial technology evaluation for Caching Architecture |
| 1.3.0   | 2026-07-28 | Solution Architect | New section added (Evaluation Scope)                   |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0              |