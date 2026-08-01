| Property | Value |
|----------|-------|
| **Document ID** | ADR-0021 |
| **Title** | Search Strategy |
| **Version** | 4.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-27 |
| **Last Updated** | 2026-07-28 |

---

# Context

Every module described in `02-CapabilityModel.md` exposes list and detail
views (Assets, Work Orders, Maintenance Records, Inventory, and similar
entities) that require free-text search by name, description, notes, and
other free-text fields. This capability was not yet covered by any
approved Technology Evaluation or ADR.

`TE-0027 — Search Engine Technology Evaluation` evaluated SQL Server
Full-Text Search, PostgreSQL Full-Text Search, Elasticsearch, OpenSearch,
Azure AI Search, and a Hybrid Search strategy against the platform's
established cloud-neutral, Open Source First (ADR-0002) posture, its
deployment-flexibility requirements (on-premise, air-gapped, and cloud, as
already established in ADR-0020), and its existing database engine
(SQL Server, per ADR-0006). It recommended SQL Server Full-Text Search as
the platform-wide default, OpenSearch as an optional escalation path for
larger deployments, and Hybrid Search — combining lexical search with the
semantic/vector search technology to be selected by the forthcoming
TE-0028 — as the platform's future direction. It deferred formal recording
of this decision to this ADR.

---

# Decision

The platform shall access all free-text search functionality exclusively
through an Application-layer abstraction, `ISearchService`, implemented in
Infrastructure as follows:

- **SQL Server Full-Text Search** shall be the default lexical search
  technology for all deployments, requiring no additional infrastructure
  beyond the already-approved SQL Server database (ADR-0006).
- **OpenSearch** may be introduced as an additional `ISearchService`
  implementation for specific deployments whose search volume or relevance
  requirements exceed what SQL Server Full-Text Search can serve
  acceptably, selected per deployment without requiring any
  Application-layer code change.
- **Elasticsearch** and **Azure AI Search** are not adopted as platform
  defaults, for licensing and cloud-neutrality reasons respectively.
- **PostgreSQL Full-Text Search** is not adopted now; it shall be
  re-evaluated if the forthcoming TE-0028 (Vector Database Technology
  Evaluation) selects `pgvector`, since that would allow a single
  PostgreSQL instance to serve both lexical and semantic search.
- **Hybrid Search** — combining the lexical technology selected above with
  a semantic/vector search technology — is approved as the platform's
  future direction and shall be implemented once TE-0028 selects a vector
  database, using result fusion (e.g. reciprocal rank fusion) at the
  Application layer.

---

# Decision Drivers

- Zero additional infrastructure/cost for the default deployment path
- Cloud neutrality
- Deployment flexibility (on-premise, air-gapped, and cloud, consistent
  with ADR-0020)
- Open Source Policy compliance (ADR-0002)
- A clear, low-risk escalation path for larger-scale deployments
- Compatibility with a future semantic/hybrid search capability

---

## Search Flow

Application Layer

        │

        ▼

Repository

        │

        ▼

Indexing Service

        │

        ▼

Search Engine

        │

        ▼

Search API

        │

        ▼

Presentation Layer

The search index is not accessed directly by domain objects.

All indexing operations are coordinated by the Application Layer.

The Presentation Layer communicates only through the Search API abstraction.

---

# Alternatives Considered

## Elasticsearch as the Default Search Engine

Rejected because its licensing moved away from a fully permissive
open-source model (SSPL / Elastic License, partially AGPLv3), directly
paralleling the AutoMapper licensing concern raised in TE-0023, and because
operating a JVM-based cluster is disproportionate for the platform's
smallest, single-server, on-premise customers.

## Azure AI Search as the Default Search Engine

Rejected for the same cloud-neutrality reasons already established for
Azure Blob Storage under ADR-0020: it is a proprietary, Azure-only managed
service, unavailable for on-premise or air-gapped deployments.

## PostgreSQL Full-Text Search as the Default Search Engine

Rejected for now because it would introduce a second relational database
engine, and a dedicated synchronization pipeline to keep it consistent with
the authoritative SQL Server data, without sufficient current
justification. Explicitly deferred for re-evaluation if TE-0028 selects
`pgvector`.

## Deciding the Full Hybrid Search Architecture Immediately

Rejected because the semantic/vector half of the strategy depends on a
technology selection (TE-0028) not yet made; deciding it prematurely would
risk contradicting that forthcoming evaluation.

---

# Consequences

## Positive

- The platform gains free-text search at zero additional infrastructure
  cost and identical behavior across every deployment posture it already
  supports.
- A clear, pre-defined escalation path (OpenSearch) exists for
  higher-volume or higher-relevance-requirement deployments, without
  requiring Application-layer changes.
- The eventual Hybrid Search direction is explicitly planned for, so
  TE-0028's semantic/vector selection can be integrated without
  revisiting this decision.

## Negative

- SQL Server Full-Text Search's relevance ranking and cross-entity search
  capabilities are meaningfully weaker than a dedicated search engine;
  this is an accepted trade-off for the default path.
- Deployments that later escalate to OpenSearch take on the operational
  responsibility of running a JVM-based cluster.

## Trade-offs

The platform accepts weaker default search quality in exchange for zero
additional infrastructure and full cloud neutrality; deployments with
genuine scale or relevance requirements have an explicit, pre-approved path
to a stronger technology without an architectural redesign.

## Future Limitations

If a large share of deployments end up requiring OpenSearch, the platform
should revisit whether OpenSearch should become the default rather than an
opt-in escalation.

---

# Architecture Impact

- **Domain** — No impact. Domain entities never reference a search
  technology.
- **Application** — Depends only on the `ISearchService` abstraction.
- **Infrastructure** — Hosts `SqlServerFullTextSearchService` (default) and
  `OpenSearchSearchService` (optional), selected per deployment via
  configuration. A future `HybridSearchService` will compose the selected
  lexical implementation with the vector database selected in TE-0028.
- **Presentation** — No impact; the search technology remains fully
  invisible above the Application layer.

---

# Implementation Notes

- Search indexes shall always be derived from, and kept synchronized with,
  the authoritative data owned by EF Core (ADR-0006); no search technology
  shall become an independent source of truth for business data.
- Multi-tenant isolation (Organization scoping) shall be enforced
  identically regardless of which `ISearchService` implementation is
  active.
- Escalation to OpenSearch for a given deployment is a configuration and
  Infrastructure-layer implementation change only, never an
  Application-layer change.

---

# Compliance Rules

1. All free-text search access shall go through the `ISearchService`
   abstraction; no layer above Infrastructure shall reference a search
   technology's client library directly.

2. SQL Server Full-Text Search shall be the default `ISearchService`
   implementation for all deployments unless explicitly escalated.

3. OpenSearch is the only approved escalation path for higher-volume or
   higher-relevance-requirement deployments unless a new or amended ADR
   approves an alternative.

4. Search indexes shall never become an independent source of truth;
   they shall always be derivable from EF Core-owned authoritative data.

5. Hybrid Search shall not be implemented until TE-0028 selects a vector
   database technology.

---

# Related Technology Evaluation

TE-0027 — Search Engine Technology Evaluation

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
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- ADR-0020 — File Storage Strategy
- TE-0027 — Search Engine Technology Evaluation
- 02-CapabilityModel.md

---

# References

https://learn.microsoft.com/sql/relational-databases/search/full-text-search

https://opensearch.org/docs/latest/

---

#  Revision History

| Version | Date       | Author             | Description                                                              |
|---------|------------|--------------------|--------------------------------------------------------------------------|
| 1.0.0   | 2026-07-27 | Solution Architect | Initial decision, formalizing the Search Strategy recommended by TE-0027 |
| 1.1.0   | 2026-07-28 | Solution Architect | New Section Added (Search Flow)                                          |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0                    |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0                                |