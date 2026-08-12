| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | TE-0027            |
| **Title**        | Solution Architect |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-27         |
| **Last Updated** | 2026-08-08         |

# Purpose

This document evaluates the technology used to provide free-text search
across the MachineryManagerEnterprise platform — searching Assets, Work
Orders, Maintenance Records, Inventory items, and similar entities by
name, description, notes, and other free-text fields.

Unlike TE-0026 (File Storage), this capability is not yet recorded as a
gap in `03-TechnologyGapAnalysis.md`; it is introduced here proactively,
since every module described in `02-CapabilityModel.md` exposes list and
detail views that will require free-text search as the platform's data
volume grows beyond what simple `LIKE`-based filtering can serve
acceptably.

The objective of this evaluation is to:

- evaluate the search technology already available at zero additional
  infrastructure cost through the platform's approved database engine
  (SQL Server Full-Text Search), given ADR-0006;
- evaluate PostgreSQL Full-Text Search, Elasticsearch, and OpenSearch as
  originally requested;
- additionally evaluate Azure AI Search, given the platform's .NET/Azure
  affinity, and a Hybrid Search strategy combining lexical (keyword) search
  with the semantic/vector search capability to be selected in the
  forthcoming TE-0028 (Vector Database Technology Evaluation);
- select the search technology and strategy for the platform, or an
  explicit combination, consistent with the platform's cloud-neutral,
  Open Source First (ADR-0002) posture.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Relationship with Previous Technology Evaluations

This evaluation does not supersede any previously approved Technology
Evaluation. It depends on **ADR-0006 — Use Entity Framework Core** (which
established SQL Server as the platform's current target database) as
architectural context for the SQL Server Full-Text Search candidate, and
it establishes the lexical-search half of the Hybrid Search strategy that
will compose with the semantic/vector search technology selected in the
forthcoming **TE-0028 — Vector Database Technology Evaluation**.

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0002 — Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- 02-CapabilityModel.md
- ../05-development/04-DependencyRules.md
---

# Scope

This evaluation covers:

- free-text (keyword-based, lexical) search across entity fields such as
  Asset name/description, Work Order notes, and similar free-text content;
- ranking and relevance scoring of search results;
- the abstraction layer the Application layer uses to issue search queries,
  independent of the underlying search technology.

Out of scope:

- semantic / vector-based similarity search — covered by the forthcoming
  TE-0028 (Vector Database Technology Evaluation); this evaluation only
  defines how a lexical search technology would compose with it under the
  Hybrid Search candidate.
- the AI Provider and embedding-generation technology used to produce
  vectors for semantic search — covered by the forthcoming TE-0029
  (Artificial Intelligence Provider Technology Evaluation).

---

# Functional Requirements

The selected solution shall support:

- free-text search across multiple entity types and fields, with relevance
  ranking rather than simple substring matching;
- filtering combined with free-text search (e.g. search within a specific
  Organization, Asset category, or date range);
- reasonable result latency for interactive list-view search-as-you-type
  scenarios in the Blazor Server UI;
- multi-tenant isolation, consistent with the platform's Organization-scoped
  data model, so that search results never cross organizational boundaries.

---

# Non-Functional Requirements

The solution should provide:

- minimal additional operational complexity for the platform's smaller,
  on-premise customers, consistent with the deployment-flexibility posture
  established in TE-0026;
- cloud neutrality wherever practical;
- Open Source First compliance (ADR-0002);
- a clear upgrade path from the platform's current, database-only search
  approach to a dedicated search technology, without requiring a rewrite of
  Application-layer query logic.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| SQL Server Full-Text Search | Native full-text indexing built into the already-approved database engine | Evaluated (added at Solution Architect's recommendation) |
| PostgreSQL Full-Text Search | Native full-text indexing built into PostgreSQL | Evaluated |
| Elasticsearch | Dedicated, distributed search and analytics engine | Evaluated |
| OpenSearch | Open-source fork of Elasticsearch, AWS-stewarded | Evaluated |
| Azure AI Search | Microsoft's managed cloud search service | Evaluated (added at Solution Architect's recommendation) |
| Hybrid Search | Lexical search (selected candidate above) combined with semantic/vector search (TE-0028) | Evaluated (added at Solution Architect's recommendation) |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Open Source & License Stability | Critical |
| A2 | Clean Architecture Compatibility | Critical |
| A3 | Deployment Flexibility (On-Premise / Cloud) | Critical |
| A4 | Cloud Neutrality | High |
| A5 | Search Quality (Relevance, Ranking) | High |
| A6 | Operational Complexity | High |
| A7 | Cost Predictability | Medium |
| A8 | Scalability | Medium |
| A9 | Multi-Tenant Isolation | Medium |
| A10 | Future Semantic Search Compatibility | Medium |

---

# Architecture Principle

Search shall be accessed exclusively through an Application-layer
abstraction, never directly by any layer above Infrastructure, so that the
underlying search technology remains fully replaceable.

```text
Application Layer
   ISearchService  (SearchAssets / SearchWorkOrders / ...)
        │
        ▼
Infrastructure Layer
   SqlServerFullTextSearchService | ElasticsearchService | ...
        │
        ▼
   SQL Server Full-Text Index | Elasticsearch Cluster | ...
```

Domain entities shall never reference a search technology. SharedKernel
shall never reference a search technology. Search indexes shall always be
derived from, and kept synchronized with, the authoritative data owned by
EF Core (ADR-0006) — no search technology shall become an independent
source of truth for business data.

---

## Future AI Compatibility

Future architecture shall support:

- Semantic Search
- Vector Search
- Retrieval Augmented Generation (RAG)
- AI Assistant Integration
- Hybrid Search

Search technologies shall therefore be evaluated not only for keyword search but also for their long-term capability to integrate with AI-assisted retrieval systems.


---

# 5. SQL Server Full-Text Search Evaluation

## Overview

SQL Server Full-Text Search is a native indexing and querying feature
built directly into SQL Server, the platform's already-approved database
engine (ADR-0006). It provides linguistic, word-based indexing and query
predicates (`CONTAINS`, `FREETEXT`) without requiring any additional
service or infrastructure.

## Architectural Role

```text
Application Layer

   SearchAssetsQueryHandler
          │
          ▼
   ISearchService.SearchAsync("hydraulic pump")
          │
          ▼
Infrastructure Layer
   SqlServerFullTextSearchService
          │
          ▼
   SELECT ... FROM Assets
   WHERE CONTAINS(SearchVector, @query)
          │
          ▼
   SQL Server Full-Text Index (same database as EF Core)
```

## Architectural Strengths

- Zero additional infrastructure: the capability already exists inside the
  database engine already approved under ADR-0006, requiring only enabling
  the Full-Text feature and creating full-text indexes and catalogs.
- Data remains in a single system: no synchronization pipeline is needed
  between the authoritative EF Core-owned data and a separate search
  index, eliminating an entire class of index-staleness risk that every
  external search engine candidate must address.
- Transactionally consistent with writes in most practical scenarios,
  since the full-text index is maintained by the same database engine that
  owns the data (subject to SQL Server's asynchronous population tracking
  for very high-write-volume tables).
- Supports linguistic features out of the box: stemming, thesaurus,
  stopword lists, and ranking via `CONTAINSTABLE`/`FREETEXTTABLE`.
- Composable directly with existing EF Core LINQ queries and the Hybrid
  Persistence Strategy (ADR-0019), since full-text predicates can be
  expressed through raw SQL fragments or Dapper queries against the same
  database.

## Architectural Weaknesses

- Relevance ranking and query capabilities are meaningfully less
  sophisticated than a dedicated search engine: no native faceting,
  weighted multi-field scoring is more limited, and fuzzy/typo-tolerant
  matching is weaker than Elasticsearch or OpenSearch.
- Full-text indexing adds write-path overhead directly on the primary
  transactional database, competing for the same resources as the
  platform's core OLTP workload, unlike an external search engine which
  isolates search load onto separate infrastructure.
- Cross-entity, cross-table search (e.g. a single query spanning Assets,
  Work Orders, and Inventory simultaneously with unified relevance
  ranking) is considerably more awkward to express than in a
  purpose-built search engine.
- No native support for semantic/vector search, meaning the future Hybrid
  Search strategy would need a separate integration point regardless.

## Operational Characteristics

No new service to deploy; full-text catalogs and indexes are managed as
part of the existing SQL Server instance and its existing backup/maintenance
routines.

## Scalability

Adequate for small-to-medium data volumes and query rates; does not scale
independently from the primary database, meaning search load directly
competes with transactional load under high concurrency.

## Deployment Flexibility

Excellent: available in every SQL Server edition the platform already
targets, with identical behavior across on-premise, air-gapped, and cloud
deployments, requiring no additional infrastructure decision per
deployment.

## Cost

Zero additional cost: the feature is included with the already-licensed
SQL Server instance.

## Security

Inherits the database's existing authentication, authorization, and
row-level security model; no additional credential or access-control
surface is introduced.

## Multi-Tenant Isolation

Excellent: because queries run against the same tables already scoped by
Organization through existing EF Core query filters, multi-tenant
isolation is inherited automatically rather than requiring separate
configuration in an external system.

## Future Semantic Search Compatibility

Weak on its own: SQL Server Full-Text Search has no native vector/semantic
search capability, so a future Hybrid Search strategy would need to query
this technology and a separate vector store independently and merge
results at the Application layer.

## Typical Usage

```csharp
public sealed class SqlServerFullTextSearchService : ISearchService
{
    private readonly IDbConnection _connection;

    public async Task<IReadOnlyList<AssetSearchResultDto>> SearchAssetsAsync(
        string query, Guid organizationId, CancellationToken ct)
    {
        const string sql = """
            SELECT a.Id, a.Name, KEY_TBL.RANK
            FROM Assets a
            INNER JOIN CONTAINSTABLE(Assets, (Name, Description), @query) AS KEY_TBL
                ON a.Id = KEY_TBL.[KEY]
            WHERE a.OrganizationId = @organizationId
            ORDER BY KEY_TBL.RANK DESC
            """;

        return (await _connection.QueryAsync<AssetSearchResultDto>(
            sql, new { query, organizationId })).AsList();
    }
}
```

## Comparison with Elasticsearch

| Aspect | SQL Server Full-Text Search | Elasticsearch |
|--------|--------------------------------|----------------|
| Additional infrastructure | None | Dedicated cluster required |
| Index/data consistency | Native (same database) | Requires synchronization pipeline |
| Relevance/ranking sophistication | Basic | Excellent |
| Cross-entity unified search | Awkward | Native |
| Cost | Included in existing license | Additional infrastructure cost |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (abstracted behind `ISearchService`) |
| Deployment Flexibility | Excellent |
| Cloud Neutrality | Excellent (part of the already-neutral SQL Server deployment) |
| Search Quality | Fair |
| Cost Predictability | Excellent (zero additional cost) |
| Migration Cost | None |

## Relationship with the Hybrid Persistence Strategy (ADR-0019)

SQL Server Full-Text Search composes naturally with the read-side Dapper
path already approved under ADR-0019: full-text `CONTAINSTABLE` queries
are themselves exactly the kind of hand-written, performance-justified SQL
that ADR-0019's governance rules already anticipate for read-only reporting
and search scenarios.

## Preliminary Conclusion

SQL Server Full-Text Search is the strongest starting point for the
platform: zero additional infrastructure, zero additional cost, and native
consistency with the authoritative data, at the cost of weaker relevance
ranking and cross-entity search sophistication than a dedicated engine.

---

# 6. PostgreSQL Full-Text Search Evaluation

## Overview

PostgreSQL Full-Text Search is PostgreSQL's native text-search
capability, based on `tsvector`/`tsquery` types, GIN/GiST indexes, and
configurable text-search dictionaries. It is evaluated here as originally
requested, even though the platform's current target database is SQL
Server (ADR-0006), because it represents a credible pattern used by some
enterprise systems: running a dedicated PostgreSQL instance purely for
search, alongside the primary SQL Server transactional database.

## Architectural Role

```text
Application Layer

   SearchAssetsQueryHandler
          │
          ▼
   ISearchService.SearchAsync("hydraulic pump")
          │
          ▼
Infrastructure Layer
   PostgresFullTextSearchService
          │
          ▼
   Synchronization pipeline (change data capture / outbox)
          │
          ▼
   Dedicated PostgreSQL instance (tsvector index, separate from SQL Server)
```

## Architectural Strengths

- Mature, well-documented, and highly capable full-text search feature
  set, generally regarded as stronger than SQL Server's equivalent (more
  flexible ranking functions, richer dictionary/configuration options).
- Fully open source (PostgreSQL License, permissive), strongly aligned
  with ADR-0002.
- If the platform ever needed `pgvector` for semantic search (a candidate
  already listed for the forthcoming TE-0028), the same PostgreSQL
  instance could serve both lexical and vector search from a single
  engine, a notable architectural synergy unique to this candidate.

## Architectural Weaknesses

- **Introduces a second, entirely different relational database engine**
  into a platform whose primary transactional data lives in SQL Server
  (ADR-0006), requiring a dedicated synchronization pipeline (change data
  capture, an outbox pattern, or a scheduled ETL job) to keep the
  PostgreSQL search index consistent with the authoritative SQL Server
  data — a significant new category of operational and consistency risk
  not shared by SQL Server Full-Text Search.
- Doubles the platform's database operational surface: two engines to
  patch, back up, monitor, and provision, which directly conflicts with
  the low-operational-complexity posture that made SQL Server Full-Text
  Search attractive.
- No existing team expertise or tooling investment in PostgreSQL
  administration, unlike SQL Server, where the team already has deep,
  established operational experience from the platform's core database.
- For customers with the smallest, single-server, on-premise deployments
  (the same customer segment motivating TE-0026's Local Storage and MinIO
  discussion), running a second database engine is a materially higher
  operational burden than either SQL Server Full-Text Search or a fully
  managed external search service.

## Operational Characteristics

Requires deploying, configuring, and operating a dedicated PostgreSQL
instance, plus a synchronization mechanism to replicate relevant fields
from SQL Server into PostgreSQL's `tsvector` columns.

## Scalability

Good; PostgreSQL full-text search scales adequately for moderate data
volumes and can be scaled independently of the primary SQL Server database,
which is an advantage over SQL Server Full-Text Search's shared-resource
limitation.

## Deployment Flexibility

Good in principle (PostgreSQL runs on-premise, in containers, or in any
cloud), but practically weaker than SQL Server Full-Text Search for the
platform's smallest customers, who would need to operate a second database
engine solely for search.

## Cost

Additional infrastructure cost (a dedicated PostgreSQL instance) beyond
the already-licensed SQL Server, though the database engine itself carries
no license fee.

## Security

Requires its own authentication and access-control configuration, separate
from SQL Server's existing security model, adding a second security
surface to audit.

## Multi-Tenant Isolation

Requires explicit configuration (e.g. an `organization_id` column indexed
alongside the `tsvector` column) rather than inheriting the isolation SQL
Server Full-Text Search gets automatically from existing EF Core query
filters.

## Future Semantic Search Compatibility

Strong, uniquely among the lexical-only candidates: `pgvector` (a
candidate already identified for TE-0028) runs inside the same PostgreSQL
instance, meaning a PostgreSQL-based search strategy could unify lexical
and semantic search in a single engine rather than requiring the
cross-engine merge a Hybrid Search strategy otherwise implies.

## Typical Usage

```sql
ALTER TABLE assets ADD COLUMN search_vector tsvector
    GENERATED ALWAYS AS (to_tsvector('english', name || ' ' || description)) STORED;

CREATE INDEX assets_search_idx ON assets USING GIN (search_vector);

SELECT id, name, ts_rank(search_vector, query) AS rank
FROM assets, to_tsquery('english', 'hydraulic & pump') query
WHERE search_vector @@ query AND organization_id = $1
ORDER BY rank DESC;
```

## Comparison with SQL Server Full-Text Search

| Aspect | PostgreSQL Full-Text Search | SQL Server Full-Text Search |
|--------|---------------------------------|----------------------------------|
| Additional infrastructure | Yes (second database engine) | None |
| Data consistency | Requires synchronization pipeline | Native |
| Ranking sophistication | Better | Fair |
| Future pgvector synergy | Yes (same engine) | No |
| Operational burden | Higher (second engine) | None (already operated) |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (abstracted behind `ISearchService`) |
| Deployment Flexibility | Good, weaker than SQL Server FTS for smallest deployments |
| Cloud Neutrality | Excellent |
| Search Quality | Good |
| Cost Predictability | Fair (new infrastructure, but no license cost) |
| Migration Cost | High (new engine, new sync pipeline) |

## Relationship with TE-0028 (pgvector)

If the forthcoming TE-0028 selects `pgvector` as the platform's vector
database, this candidate becomes materially more attractive, since a
single PostgreSQL instance could then serve both this evaluation's lexical
search need and TE-0028's semantic search need, reducing the total number
of specialized data stores the platform operates from two (a separate
search engine and a separate vector database) to one.

## Preliminary Conclusion

PostgreSQL Full-Text Search offers real technical strengths and a genuine
architectural synergy with a possible future `pgvector` decision, but
introduces a second relational database engine and a synchronization
pipeline that the platform does not currently need, given SQL Server Full-
Text Search already satisfies the platform's near-term requirements at
zero additional operational cost.

---

# 7. Elasticsearch Evaluation

## Overview

Elasticsearch is a distributed, JSON-document-oriented search and
analytics engine built on Apache Lucene, widely regarded as the industry
standard for dedicated full-text search at scale.

## Architectural Role

```text
Application Layer

   SearchAssetsQueryHandler
          │
          ▼
   ISearchService.SearchAsync("hydraulic pump")
          │
          ▼
Infrastructure Layer
   ElasticsearchSearchService (Elastic.Clients.Elasticsearch)
          │
          ▼
   Synchronization pipeline (outbox / CDC)
          │
          ▼
   Elasticsearch Cluster (separate from SQL Server)
```

## Architectural Strengths

- Best-in-class relevance ranking, fuzzy/typo-tolerant matching, faceting,
  aggregations, and cross-entity unified search — the strongest search
  quality of any candidate evaluated here.
- Scales independently of the primary transactional database, isolating
  search load entirely from the platform's core OLTP workload.
- Mature .NET client (`Elastic.Clients.Elasticsearch`), extensive
  documentation, and a very large ecosystem of operational tooling and
  community knowledge.
- Naturally supports future requirements such as autocomplete,
  "search-as-you-type," and analytics dashboards over search behavior,
  none of which SQL Server or PostgreSQL full-text search handle natively.

## Architectural Weaknesses

- **Licensing**: Elasticsearch's core distribution moved away from a fully
  permissive open-source license (Apache 2.0) to the Server Side Public
  License (SSPL) / Elastic License in 2021, and later relicensed portions
  back under AGPLv3 in 2024 alongside a dual-license model; the practical
  effect is that "free" Elasticsearch today is not the same permissive
  open-source proposition it once was, and several advanced features
  remain behind a commercial subscription (Elastic Stack subscription
  tiers). This is directly analogous to the AutoMapper licensing concern
  raised in TE-0023 and requires the same careful reading against
  ADR-0002.
- Requires operating a genuinely separate, resource-intensive distributed
  system (a JVM-based cluster with its own memory, storage, and
  operational tuning requirements), a significant operational step up from
  either full-text search candidate evaluated above.
- Requires a dedicated synchronization pipeline to keep the Elasticsearch
  index consistent with SQL Server's authoritative data, introducing the
  same index-staleness risk category identified for PostgreSQL Full-Text
  Search, at greater operational complexity.
- Poor fit for the platform's smallest, single-server, on-premise
  customers, for whom operating a JVM-based search cluster is a
  disproportionate operational burden relative to their data volume.

## Operational Characteristics

Requires deploying, sizing, and operating an Elasticsearch cluster
(minimum a single well-resourced node for small deployments, multiple
nodes for production-grade redundancy), plus a synchronization mechanism
from SQL Server.

## Scalability

Excellent; Elasticsearch is specifically designed for horizontal
scalability and is the strongest-scaling candidate evaluated here for
very large data volumes and query rates.

## Deployment Flexibility

Good for larger, cloud-hosted, or well-resourced on-premise deployments;
poor for the platform's smallest customers due to the JVM-based cluster's
resource and operational requirements.

## Cost

Open-source-tier Elasticsearch carries no license fee but requires
dedicated infrastructure (compute, memory, storage) whose cost scales with
data volume; several advanced features require a paid Elastic subscription.

## Security

Strong security features (role-based access control, field-level security)
are available, but predominantly gated behind the paid subscription tiers
in practice for production-grade deployments, another parallel to the
AutoMapper-style licensing concern.

## Multi-Tenant Isolation

Requires explicit index design (e.g. a per-organization field with query
filters, or index-per-tenant for the largest customers), configured and
maintained separately from EF Core's existing query filters.

## Future Semantic Search Compatibility

Good: recent Elasticsearch versions include native dense-vector field
types and approximate nearest-neighbor search, meaning Elasticsearch could
in principle serve both lexical and semantic search — though this
capability is newer and less established than the purpose-built vector
databases evaluated in the forthcoming TE-0028.

## Typical Usage

```csharp
var response = await _client.SearchAsync<AssetDocument>(s => s
    .Index("assets")
    .Query(q => q
        .Bool(b => b
            .Must(m => m.Match(mt => mt.Field(f => f.Name).Query("hydraulic pump")))
            .Filter(f => f.Term(t => t.Field(ff => ff.OrganizationId).Value(organizationId))))));
```

## Comparison with OpenSearch

| Aspect | Elasticsearch | OpenSearch |
|--------|----------------|------------|
| License | SSPL / Elastic License (dual, partially AGPLv3) | Apache 2.0 (fully permissive) |
| Feature parity | Baseline, plus commercial-tier extras | Very close to Elasticsearch's open feature set |
| Ecosystem maturity | Larger, longer track record | Smaller but rapidly growing (AWS-stewarded) |
| ADR-0002 compliance | Partial (core free tier only) | Full |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (abstracted behind `ISearchService`) |
| Deployment Flexibility | Fair (poor for smallest deployments) |
| Cloud Neutrality | Good (self-hostable, also available managed on every major cloud) |
| Search Quality | Excellent |
| Cost Predictability | Fair (infrastructure cost scales with data; advanced features are commercial) |
| License Stability | Fair (licensing changed materially in recent years) |

## Relationship with SQL Server Full-Text Search

Elasticsearch and SQL Server Full-Text Search are not mutually exclusive at
the architecture level: because access happens exclusively through
`ISearchService`, a smaller deployment could use SQL Server Full-Text
Search while a larger, higher-volume deployment uses Elasticsearch,
selected via configuration and Infrastructure-layer implementation choice
per deployment, similar to the pattern already established for file
storage in TE-0026.

## Preliminary Conclusion

Elasticsearch delivers the strongest search quality and scalability of any
candidate, but its licensing trajectory, operational weight, and poor fit
for the platform's smallest customers make it a poor default, better
reserved as an optional, larger-deployment alternative rather than the
platform-wide baseline.

---

# 8. OpenSearch Evaluation

## Overview

OpenSearch is an open-source fork of Elasticsearch, created in 2021 by AWS
and other contributors in direct response to Elasticsearch's licensing
change, and released under the fully permissive Apache 2.0 license. It
aims to track Elasticsearch's open-source feature set closely while
remaining unambiguously open source.

## Architectural Role

```text
Application Layer

   SearchAssetsQueryHandler
          │
          ▼
   ISearchService.SearchAsync("hydraulic pump")
          │
          ▼
Infrastructure Layer
   OpenSearchSearchService (OpenSearch.Client for .NET)
          │
          ▼
   Synchronization pipeline (outbox / CDC)
          │
          ▼
   OpenSearch Cluster (separate from SQL Server)
```

## Architectural Strengths

- Fully Apache 2.0 licensed, with no dual-licensing or commercial-tier
  feature gating — the cleanest Open Source First (ADR-0002) fit among the
  dedicated search engine candidates, directly resolving the licensing
  concern identified for Elasticsearch.
- API and query DSL remain very close to Elasticsearch's, meaning the same
  architectural knowledge, query patterns, and much of the operational
  tooling transfer directly, minimizing the practical cost of choosing
  OpenSearch over Elasticsearch.
- Backed by a broad multi-vendor community (AWS and others) with a
  governance model explicitly structured to prevent a repeat of
  Elasticsearch's relicensing event.
- Available as a genuinely open-source, self-hostable distribution as well
  as a managed offering on AWS and other clouds, giving deployment
  flexibility similar to the MinIO / S3-Compatible pattern established in
  TE-0026.

## Architectural Weaknesses

- Shares Elasticsearch's operational weight: a JVM-based distributed
  system requiring dedicated infrastructure, sizing, and operational
  expertise, still a disproportionate burden for the platform's smallest,
  single-server, on-premise customers.
- Still requires a dedicated synchronization pipeline from SQL Server, the
  same index-staleness risk category shared with Elasticsearch and
  PostgreSQL Full-Text Search.
- Smaller ecosystem and shorter track record than Elasticsearch, though
  this gap has narrowed substantially since 2021 and is much less material
  than it was at OpenSearch's inception.
- .NET client library (`OpenSearch.Client`) is less mature and less widely
  used than Elasticsearch's official .NET client, though it remains fully
  functional and actively maintained.

## Operational Characteristics

Materially identical to Elasticsearch's operational profile: a distributed,
JVM-based cluster requiring dedicated infrastructure and operational
expertise.

## Scalability

Excellent; inherits Elasticsearch's Lucene-based, horizontally scalable
architecture nearly unchanged.

## Deployment Flexibility

Good for larger, cloud-hosted, or well-resourced on-premise deployments;
poor for the platform's smallest customers, identical to Elasticsearch's
profile.

## Cost

No license fee for any feature tier (unlike Elasticsearch's commercial
gating), though infrastructure cost still scales with data volume and
cluster size.

## Security

Full security feature set (role-based access control, field- and
document-level security) available without a commercial subscription,
directly resolving the licensing-gated security concern identified for
Elasticsearch.

## Multi-Tenant Isolation

Equivalent to Elasticsearch: requires explicit index design and query
filters for per-organization isolation.

## Future Semantic Search Compatibility

Good: OpenSearch includes a k-NN plugin supporting approximate
nearest-neighbor vector search, offering similar future semantic-search
potential to Elasticsearch's dense-vector capability.

## Typical Usage

```csharp
var response = await _client.SearchAsync<AssetDocument>(s => s
    .Index("assets")
    .Query(q => q
        .Bool(b => b
            .Must(m => m.Match(mt => mt.Field(f => f.Name).Query("hydraulic pump")))
            .Filter(f => f.Term(t => t.Field(ff => ff.OrganizationId).Value(organizationId))))));
```

## Comparison with Elasticsearch

(See Elasticsearch's Comparison with OpenSearch table above; the
relationship is symmetric.)

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (abstracted behind `ISearchService`) |
| Deployment Flexibility | Fair (poor for smallest deployments, same as Elasticsearch) |
| Cloud Neutrality | Excellent (fully open source, multi-cloud managed offerings) |
| Search Quality | Excellent |
| Cost Predictability | Good (infrastructure cost only, no license gating) |
| License Stability | Excellent (Apache 2.0, governance designed against relicensing risk) |

## Relationship with Elasticsearch

OpenSearch is architecturally and operationally the closest substitute for
Elasticsearch of any candidate in this evaluation; choosing between them is
almost entirely a licensing and governance decision rather than a
technical one, given their near-identical API surface and operational
profile.

## Preliminary Conclusion

OpenSearch delivers Elasticsearch-class search quality and scalability
with full Open Source First compliance, making it the correct choice
**if and when** the platform needs a dedicated search engine beyond what
SQL Server Full-Text Search provides — but it carries the same operational
weight that makes it a poor default for the platform's smallest customers.

---

# 9. Azure AI Search Evaluation

## Overview

Azure AI Search (formerly Azure Cognitive Search) is Microsoft's managed,
cloud-hosted search-as-a-service offering, providing full-text search,
faceting, and native vector/semantic search capabilities without requiring
the platform to operate any search infrastructure directly. It is
evaluated here for the same reason Azure Blob Storage was evaluated in
TE-0026: the platform's .NET-centric stack creates a natural temptation
toward first-party Azure services, and this evaluation would be incomplete
without addressing it directly.

## Architectural Role

```text
Application Layer

   SearchAssetsQueryHandler
          │
          ▼
   ISearchService.SearchAsync("hydraulic pump")
          │
          ▼
Infrastructure Layer
   AzureAiSearchService (Azure.Search.Documents SDK)
          │
          ▼
   Azure AI Search Service (cloud-managed)
```

## Architectural Strengths

- Fully managed: no cluster, no JVM tuning, no infrastructure to operate —
  the lowest operational burden of any dedicated search engine candidate.
- Native, first-party integration of both lexical and vector/semantic
  search in a single managed service, which would otherwise require
  combining a separate lexical engine (this evaluation) with a separate
  vector database (TE-0028) under the Hybrid Search candidate below.
- First-party .NET SDK (`Azure.Search.Documents`), excellent documentation,
  and strong integration with the broader Azure ecosystem.
- Built-in AI enrichment pipelines (e.g. automatic entity extraction),
  though not currently a stated platform requirement.

## Architectural Weaknesses

- **Not cloud-neutral**: identical concern to Azure Blob Storage in
  TE-0026 — Azure AI Search is a proprietary Microsoft service, creating
  vendor lock-in that conflicts with the platform's cloud-neutral posture
  and is entirely unavailable for on-premise or air-gapped deployments,
  which is a hard disqualifier for a meaningful share of the platform's
  target construction-company customers.
- Ongoing usage-based billing (tiered by search unit / replica / partition),
  a materially different and less predictable cost model than either
  full-text search candidate or a self-hosted OpenSearch cluster with
  fixed infrastructure cost.
- Not open source in any respect, in direct tension with ADR-0002, more
  so than any other candidate evaluated in this document.
- Requires an active Azure subscription and internet connectivity from the
  Host, identical to the deployment-flexibility weakness identified for
  Azure Blob Storage.

## Operational Characteristics

No self-hosted infrastructure to operate; operational responsibility shifts
entirely to Azure's SLA, at the cost of the deployment flexibility and
licensing concerns above.

## Scalability

Excellent; scales transparently within the limits of the selected Azure AI
Search pricing tier, with no platform-side operational effort.

## Deployment Flexibility

Poor for on-premise or air-gapped deployments; excellent only for
customers already committed to Azure-hosted deployment of the Host
application itself — the same pattern already assessed for Azure Blob
Storage in TE-0026.

## Cost

Usage-based, tier-billed; the least cost-predictable candidate among the
dedicated search engines evaluated here.

## Security

Strong built-in access control (Azure AD integration, API keys, network
isolation options) and encryption at rest by default.

## Multi-Tenant Isolation

Supported via index design and query filters, similar to Elasticsearch and
OpenSearch, with the added option of Azure AD-based identity integration
for enterprise customers already standardized on Azure AD.

## Future Semantic Search Compatibility

Excellent: native vector search and hybrid lexical/semantic ranking are
first-class, fully supported features, arguably the strongest and most
turnkey semantic-search story of any candidate in this evaluation.

## Typical Usage

```csharp
var response = await _searchClient.SearchAsync<AssetDocument>(
    "hydraulic pump",
    new SearchOptions { Filter = $"organizationId eq '{organizationId}'" });
```

## Comparison with OpenSearch

| Aspect | Azure AI Search | OpenSearch |
|--------|--------------------|------------|
| Hosting model | Managed (Azure only) | Self-hosted or managed (multi-cloud) |
| Operational burden | Lowest | Higher (cluster operation) |
| Native vector/semantic search | Excellent, first-class | Good (k-NN plugin) |
| Cost model | Usage-based billing | Fixed infrastructure cost (self-hosted) |
| Open Source (ADR-0002) | No | Yes (Apache 2.0) |
| On-premise / air-gapped support | None | Excellent |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (abstracted behind `ISearchService`) |
| Deployment Flexibility | Poor for on-premise/air-gapped; excellent for Azure-hosted |
| Cloud Neutrality | Poor |
| Search Quality | Excellent |
| Cost Predictability | Poor (usage-based billing) |
| License Stability | N/A (proprietary managed service, not a licensing concern in the AutoMapper sense, but a cloud-neutrality concern) |

## Relationship with the Other Candidates

As with Azure Blob Storage in TE-0026, Azure AI Search is architecturally
interchangeable with OpenSearch or Elasticsearch under the same
`ISearchService` abstraction; the meaningful difference is deployment
posture (managed-Azure-only versus self-hosted-or-multi-cloud) rather than
code structure.

## Preliminary Conclusion

Azure AI Search offers the lowest operational burden and the strongest
turnkey semantic search story of any candidate, but its exclusively
Azure-hosted, proprietary nature makes it unsuitable as the platform's
default given the same cloud-neutrality and on-premise-friendly
requirements that excluded Azure Blob Storage in TE-0026. It remains a
reasonable optional implementation for specific Azure-committed customers.

---

# 10. Hybrid Search Evaluation

## Overview

Hybrid Search is not a single technology but a strategy: combining a
lexical (keyword-based) search technology — selected from the candidates
above — with a semantic/vector search technology, to be selected in the
forthcoming TE-0028 (Vector Database Technology Evaluation), so that search
results benefit from both exact keyword matching and conceptual/semantic
similarity (e.g. a search for "engine won't start" returning Work Orders
tagged with "ignition failure" even without an exact keyword match).

## Architectural Role

```text
Application Layer
   ISearchService.SearchAsync(query, organizationId)
        │
        ├──► Lexical Search (SQL Server FTS, OpenSearch, or similar)
        │        │
        │        ▼
        │    Keyword-ranked results
        │
        └──► Semantic Search (vector database, selected in TE-0028)
                 │
                 ▼
             Similarity-ranked results
                 │
                 ▼
        Result fusion (e.g. reciprocal rank fusion)
                 │
                 ▼
        Unified, ranked search results
```

## Architectural Strengths

- Combines the precision of exact keyword matching with the recall benefit
  of semantic similarity, directly relevant to the platform's maintenance
  and inspection domain, where field technicians often search using
  informal, non-exact phrasing.
- Composes cleanly with the `ISearchService` abstraction already
  established: result fusion is an Application-layer concern that queries
  two Infrastructure-layer services and merges their output, without
  either underlying technology needing awareness of the other.
- Allows the platform to start with the zero-cost lexical option (SQL
  Server Full-Text Search) and add the semantic half incrementally once
  TE-0028 selects a vector database, rather than requiring both decisions
  simultaneously.

## Architectural Weaknesses

- Depends on a decision — the vector database technology — that has not
  yet been made (deferred to TE-0028); this candidate's full architectural
  fit cannot be finally assessed until that evaluation is complete.
- Result fusion (merging and re-ranking two independently scored result
  sets) is a non-trivial piece of Application-layer logic that neither
  underlying technology provides out of the box, unless the selected
  vector database or search engine happens to support fusion natively
  (as Azure AI Search does).
- Introduces the operational and licensing considerations of *two*
  specialized data stores (a lexical search technology and a vector
  database) unless a single engine capable of both is chosen — a
  consideration that directly favors candidates like PostgreSQL (via
  `pgvector`) or Azure AI Search, which can serve both roles from one
  engine, over a combination of, for example, SQL Server Full-Text Search
  paired with a wholly separate vector database.

## Operational Characteristics

Depends entirely on which lexical and semantic technologies are combined;
ranges from "no additional infrastructure" (SQL Server Full-Text Search
paired with a lightweight embedded vector store) to "two separate managed
or self-hosted services" (OpenSearch paired with a dedicated vector
database), a range that will only be fully resolved once TE-0028 is
complete.

## Scalability

Inherits the scalability characteristics of whichever two technologies are
combined.

## Security

Inherits the combined security posture of both underlying technologies;
no additional security surface is introduced by the fusion logic itself,
since it operates only on already-authorized query results.

## Future Semantic Search Compatibility

By definition, this is the candidate most directly aligned with the
platform's future semantic search needs, since it is explicitly designed
to incorporate the vector database technology selected in TE-0028.

## Typical Usage

```csharp
public sealed class HybridSearchService : ISearchService
{
    public async Task<IReadOnlyList<AssetSearchResultDto>> SearchAsync(
        string query, Guid organizationId, CancellationToken ct)
    {
        var lexicalTask = _lexicalSearch.SearchAsync(query, organizationId, ct);
        var semanticTask = _semanticSearch.SearchAsync(query, organizationId, ct);

        await Task.WhenAll(lexicalTask, semanticTask);

        return ReciprocalRankFusion.Merge(lexicalTask.Result, semanticTask.Result);
    }
}
```

## Comparison with Lexical-Only Search

| Aspect | Hybrid Search | Lexical-Only (e.g. SQL Server FTS alone) |
|--------|-----------------|-----------------------------------------------|
| Handles informal / non-exact phrasing | Yes | No |
| Additional complexity | Higher (fusion logic, second data store) | Lower |
| Dependency on TE-0028 | Yes | No |
| Suitable as an immediate, near-term solution | Not yet (pending TE-0028) | Yes |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent (fusion logic lives in Application layer) |
| Deployment Flexibility | Depends on selected components |
| Cloud Neutrality | Depends on selected components |
| Search Quality | Excellent (best of both approaches) |
| Migration Cost | Low if built incrementally on top of the lexical candidate selected below |

## Relationship with TE-0028

This candidate is intentionally left as a **future-state target** rather
than a candidate selected for immediate implementation, because its
semantic half is not yet decided. It is recorded here so that the lexical
search technology selected by this evaluation is chosen with Hybrid
Search's future requirements explicitly in mind (see Final Recommendation).

## Preliminary Conclusion

Hybrid Search is the platform's correct long-term direction, but it is a
composition of two decisions, only one of which (the lexical half) this
evaluation can finalize today. It should be adopted incrementally: begin
with the lexical technology selected below, then extend to Hybrid Search
once TE-0028 selects a semantic/vector technology.

---

# 11. Overall Technology Comparison

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|-----------------|------------------------|--------------|---------|
| Default lexical search (all deployments) | SQL Server Full-Text Search | OpenSearch (larger deployments) | Zero-cost, zero-infrastructure keyword search |
| Optional dedicated search engine (large-scale deployments) | OpenSearch | Elasticsearch (if commercial tier already licensed) | Independently scalable, higher search quality |
| Future semantic/hybrid layer | Deferred to TE-0028 | Azure AI Search (Azure-committed customers only) | Conceptual similarity search combined with lexical results |

## Capability Comparison

| Capability | SQL Server FTS | PostgreSQL FTS | Elasticsearch | OpenSearch | Azure AI Search |
|------------|-------------------|--------------------|-----------------|------------|----------------------|
| Open Source (ADR-0002 compliant) | Yes (part of existing DB) | Yes | Partial (dual-licensed) | Yes | No |
| Additional infrastructure required | None | Yes (second DB engine) | Yes (cluster) | Yes (cluster) | No (managed) |
| Search quality / relevance | Fair | Good | Excellent | Excellent | Excellent |
| Native vector/semantic support | No | Yes (via pgvector) | Good | Good (k-NN plugin) | Excellent |
| Cost model | Included | Infrastructure only | Infrastructure + commercial tiers | Infrastructure only | Usage-based billing |
| On-premise / air-gapped support | Excellent | Good | Fair | Fair | None |
| Cloud neutrality | Excellent | Excellent | Good | Excellent | Poor |

## Cloud Neutrality Assessment

SQL Server Full-Text Search, PostgreSQL Full-Text Search, and OpenSearch
all score highest for cloud neutrality. Elasticsearch scores lower due to
its partial licensing shift. Azure AI Search scores lowest, mirroring the
conclusion already reached for Azure Blob Storage in TE-0026.

## Enterprise Suitability

| Criterion | SQL Server FTS | PostgreSQL FTS | Elasticsearch | OpenSearch | Azure AI Search |
|-----------|-------------------|--------------------|-----------------|------------|----------------------|
| Suitable as platform-wide default | Yes | No (adds a second DB engine without sufficient justification today) | No (operational weight, licensing) | Conditionally (larger deployments) | Conditionally (Azure-committed customers) |
| Suitable for smallest single-server customers | Yes | No | No | No | No |
| Suitable for large, high-search-volume deployments | Fair | Good | Excellent | Excellent | Excellent |

## Risk Assessment

| Risk | Affected Candidate | Severity |
|------|--------------------|----------|
| Weak relevance ranking limits future UX for complex search scenarios | SQL Server Full-Text Search | Low–Medium |
| Second database engine increases operational and consistency risk | PostgreSQL Full-Text Search | Medium |
| Licensing trajectory / commercial-tier feature gating | Elasticsearch | Medium |
| Vendor lock-in to Azure | Azure AI Search (if adopted as default) | High |
| Fusion logic complexity and dependency on an undecided vector technology | Hybrid Search | Medium (time-bound, resolves once TE-0028 completes) |

## Overall Evaluation

SQL Server Full-Text Search satisfies the platform's near-term search
requirements at zero additional cost and zero additional infrastructure,
and is available identically across every deployment posture the platform
already supports. OpenSearch is the correct escalation path for
larger-scale or higher-search-quality deployments, without the licensing
concerns identified for Elasticsearch or the cloud-neutrality concerns
identified for Azure AI Search. PostgreSQL Full-Text Search is a
technically credible option, primarily interesting for its future synergy
with `pgvector` should TE-0028 select it, but is not justified as an
immediate addition given the operational cost of a second database engine.
Hybrid Search remains the platform's correct long-term direction, to be
completed once TE-0028 resolves the semantic half of the strategy.

---

# 12. Final Recommendation

## Core Technology Stack

| Responsibility | Selected Technology | Rationale |
|-----------------|----------------------|-----------|
| Default lexical search (all deployments) | SQL Server Full-Text Search | Zero additional cost/infrastructure; native consistency with authoritative data |
| Optional dedicated search engine (large-scale deployments) | OpenSearch | Elasticsearch-class quality with full ADR-0002 compliance, when justified |
| Future semantic/hybrid layer | Deferred to TE-0028 | Vector database selection required before Hybrid Search can be finalized |
| Azure AI Search | Not adopted as default | Same cloud-neutrality concern as Azure Blob Storage (TE-0026); available later for specific Azure-committed customers |
| PostgreSQL Full-Text Search | Not adopted now; revisit if TE-0028 selects pgvector | Would introduce a second database engine without sufficient current justification |

## Recommended Architecture

```text
Application Layer
   ISearchService
        │
        ▼
Infrastructure Layer
   SqlServerFullTextSearchService   (default, all deployments)
   OpenSearchSearchService          (optional, larger deployments)
        │
        ▼
   SQL Server Full-Text Index (default) | OpenSearch Cluster (optional)
```

## Decision Criteria for Future Escalation to OpenSearch

```text
IF search query volume or relevance requirements
   exceed what SQL Server Full-Text Search can serve
   acceptably (as measured by search latency or
   user-reported relevance issues)

THEN introduce OpenSearch as an additional
   ISearchService implementation for the affected
   deployment, without changing Application-layer code.
```

## Decision Criteria for Future Hybrid Search Adoption

```text
ONCE TE-0028 selects a vector database technology

THEN implement HybridSearchService, combining the
   lexical technology already selected here with the
   selected vector database, using reciprocal rank
   fusion at the Application layer.

IF TE-0028 selects pgvector specifically

THEN re-evaluate PostgreSQL Full-Text Search as a
   single-engine alternative to combining SQL Server
   Full-Text Search with a separate vector store.
```

**Resolution:** TE-0028 selected Qdrant, not pgvector. This condition is
therefore not triggered, and PostgreSQL Full-Text Search remains rejected
per the Final Decision below; no re-evaluation of this evaluation is
required.

## Security Recommendations

Multi-tenant isolation (Organization scoping) shall be enforced identically
regardless of which `ISearchService` implementation is active, verified
through the same code-review discipline already applied to EF Core query
filters.

## Cloud Neutrality

The recommended default (SQL Server Full-Text Search, escalating to
OpenSearch) preserves full cloud neutrality and on-premise/air-gapped
deployment support; Azure AI Search remains available as a
deployment-specific option without becoming the platform-wide default.

## AI Readiness

This evaluation directly prepares the platform for future AI-driven search
capabilities by explicitly defining the Hybrid Search integration point,
to be completed once TE-0028 and TE-0029 (AI Provider) are evaluated.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| SQL Server Full-Text Search | **Approved** as the default lexical search technology |
| OpenSearch | **Approved** as the optional escalation path for larger deployments |
| Elasticsearch | Not adopted — licensing trajectory less favorable than OpenSearch for equivalent capability |
| PostgreSQL Full-Text Search | Not adopted — resolved: TE-0028 selected Qdrant, not pgvector, so this condition was never triggered |
| Azure AI Search | Not adopted as default — cloud-neutrality concern, same conclusion as Azure Blob Storage (TE-0026) |
| Hybrid Search | Approved as the platform's future direction; finalized once TE-0028 is complete |

---

# Decision Summary

- ✔ Clean Architecture preserved
- ✔ .NET 10 Compatibility
- ✔ Open Source First Policy (ADR-0002) compliance
- ✔ Zero additional cost/infrastructure for the default path
- ✔ Clear, explicit escalation path to OpenSearch and to Hybrid Search
- ✔ Multi-tenant isolation preserved across every candidate

Because this evaluation introduces a capability not previously covered by
any ADR, this decision has been formally recorded in **ADR-0021 — Search
Strategy**, consistent with the governance pattern already followed for
TE-0024 / ADR-0019 and TE-0026 / ADR-0020.

---

# Related ADR

```
ADR-0021 — Search Strategy (new)
```

---

# Related Documents

- ADR-0002 — Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- ADR-0020 — File Storage Strategy
- ADR-0021 — Search Strategy
- TE-0026 — File Storage Technology Evaluation
- 02-CapabilityModel.md

---

# References

https://learn.microsoft.com/sql/relational-databases/search/full-text-search

https://www.postgresql.org/docs/current/textsearch.html

https://www.elastic.co/elasticsearch

https://opensearch.org/docs/latest/

https://learn.microsoft.com/azure/search/

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-27 | Solution Architect | Initial evaluation; recommends SQL Server Full-Text Search as default with OpenSearch as escalation path; adds SQL Server FTS and Azure AI Search candidates beyond the original list; defines Hybrid Search as the future direction pending TE-0028 |
| 1.1.0   | 2026-07-27 | Solution Architect | Updated to reference ADR-0021, created to formalize the Search Strategy |
| 1.2.0   | 2026-07-28 | New section added (Future AI Compatibility)                                |
| 1.3.0   | 2026-07-28 | New section added (Evaluation Scope)                                       |
| 1.4.0   | 2026-07-28 | Solution Architect | Closed the pgvector dependency on TE-0028: TE-0028 selected Qdrant, not pgvector, so PostgreSQL Full-Text Search remains rejected and no re-evaluation is required |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |