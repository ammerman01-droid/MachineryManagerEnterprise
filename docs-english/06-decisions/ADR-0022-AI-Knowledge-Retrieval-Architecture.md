| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ADR-0022           |
| **Title**        | AI Knowledge Retrieval Architecture |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-27         |
| **Last Updated** | 2026-08-08         |

---

# Context

MachineryManagerEnterprise introduces enterprise Artificial Intelligence capabilities including:

- Semantic Search
- Enterprise Knowledge Assistant
- Retrieval-Augmented Generation (RAG)
- AI Copilot
- Intelligent Maintenance Assistant
- Context-Aware Recommendations

These capabilities require efficient semantic retrieval over enterprise documentation without impacting the operational transactional database.

The operational relational database has already been approved as:

**Microsoft SQL Server**

TE-0028 approved **Qdrant** as the enterprise Vector Database technology.

The remaining architectural decisions concern:

- ownership of business data;
- ownership of embeddings;
- synchronization strategy;
- retrieval architecture;
- infrastructure boundaries.

---

# Problem

The platform requires semantic retrieval while preserving the existing enterprise persistence architecture.

The architecture must ensure:

- operational business data remains authoritative;
- semantic indexes remain synchronized;
- AI retrieval remains independent from transactional persistence;
- vendor independence is preserved;
- hybrid deployment remains possible.

---

# Decision Drivers

The architecture shall satisfy:

- Clean Architecture
- CQRS
- Separation of Concerns
- Enterprise Scalability
- Vendor Independence
- Hybrid Deployment
- AI Readiness
- Operational Maintainability

---

# Decision

The Architecture Review Board adopts the following architecture.

## Operational Data

Microsoft SQL Server remains the **System of Record**.

Responsibilities include:

- Business Entities
- Transactions
- Audit Data
- Configuration
- Operational Persistence

No semantic vectors are stored in SQL Server.

---

## Vector Storage

Qdrant becomes the dedicated semantic retrieval infrastructure.

Responsibilities include:

- Embedding Storage
- ANN Indexes
- Similarity Search
- Metadata Filtering
- Semantic Retrieval

Qdrant never stores operational business entities.

---

## Embedding Ownership

Embeddings are derived artifacts.

They are **not authoritative business data**.

Ownership remains with the originating business document.

The Vector Database contains only:

- embedding vectors;
- document identifiers;
- retrieval metadata.

---

## Source of Truth

| Data | Source of Truth |
|------|-----------------|
| Business Entity | Microsoft SQL Server |
| Business Document | Microsoft SQL Server |
| Embedding | Generated Artifact |
| Vector Index | Qdrant |

---

# Architectural Model

```text
                     Application Layer

                            │

                            ▼

                 Embedding Generation Service

                            │

        ┌───────────────────┴───────────────────┐

        ▼                                       ▼

Microsoft SQL Server                      Qdrant

(System of Record)                  (Vector Database)

        │                                       │

 Structured Business Data            Semantic Embeddings

        └───────────────────┬───────────────────┘

                            ▼

                  Retrieval-Augmented Generation
```

---

# Embedding Lifecycle

The embedding lifecycle shall be:

```text
Document Created

        │

        ▼

Persist in SQL Server

        │

        ▼

Generate Embedding

        │

        ▼

Store Embedding in Qdrant

        │

        ▼

Ready for Semantic Retrieval
```

---

# Document Update Workflow

```text
Document Updated

        │

        ▼

Update SQL Server

        │

        ▼

Regenerate Embedding

        │

        ▼

Replace Existing Vector

        │

        ▼

Retrieval Updated
```

---

# Document Deletion Workflow

```text
Document Deleted

        │

        ▼

Delete SQL Server Record

        │

        ▼

Delete Vector Entry

        │

        ▼

Consistency Restored
```

---

# Synchronization Strategy

Synchronization shall be **event-driven**.

Events include:

- DocumentCreated
- DocumentUpdated
- DocumentDeleted

Each event triggers embedding synchronization.

---

# Retrieval Workflow

```text
User Question

        │

        ▼

Embedding Generation

        │

        ▼

Qdrant Similarity Search

        │

        ▼

Relevant Document IDs

        │

        ▼

Retrieve Documents from SQL Server

        │

        ▼

Context Assembly

        │

        ▼

Large Language Model

        │

        ▼

AI Response
```

---

# Metadata Strategy

Metadata stored in Qdrant shall include:

- DocumentId
- Module
- Language
- DocumentType
- Security Classification
- Department
- Tags

Operational document content remains stored only in SQL Server.

---

# Consistency Model

The architecture adopts **Eventual Consistency**.

Requirements:

- SQL Server transaction completes first.
- Vector synchronization executes asynchronously.
- Temporary delay between persistence and semantic availability is acceptable.

---

# Failure Handling

If embedding generation fails:

- SQL Server transaction SHALL NOT be rolled back.
- Failure SHALL be logged.
- Retry SHALL be scheduled.
- Operational data remains available.

---

# Security

Security principles:

- SQL Server controls business authorization.
- Qdrant stores no sensitive business state.
- Semantic retrieval uses document identifiers only.
- Authorization checks occur before document retrieval.

---

# Benefits

This architecture provides:

- Clear separation of responsibilities.
- Independent scaling of relational and vector storage.
- Vendor independence.
- AI extensibility.
- Operational simplicity.
- Enterprise maintainability.

---

# Consequences

Positive:

- High-performance semantic retrieval.
- Minimal impact on operational database.
- Clear ownership boundaries.
- Future AI expansion.

Negative:

- Additional infrastructure component.
- Event-driven synchronization required.
- Eventual consistency instead of immediate consistency.

---

# Alternatives Considered

## Store vectors inside SQL Server

Rejected.

Reason:

Operational database should not become responsible for AI retrieval.

---

## Cloud-managed Vector Database

Rejected.

Reason:

Conflicts with vendor independence and hybrid deployment strategy.

---

## Generate embeddings on-demand

Rejected.

Reason:

High latency and unacceptable runtime cost.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Clean Architecture
- ADR-0011 — CQRS Architecture
- ADR-000x — SQL Server Persistence Architecture
- TE-0028 — Vector Database Technology Evaluation

---

# Decision Outcome

**Accepted**

The approved AI Knowledge Retrieval Architecture for MachineryManagerEnterprise consists of:

- Microsoft SQL Server as the operational System of Record.
- Qdrant as the dedicated Vector Database.
- Event-driven embedding synchronization.
- Retrieval-Augmented Generation using semantic retrieval followed by authoritative document retrieval from SQL Server.

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-28 | Solution Architect | Initial version                                       |
| 1.1.0   | 2026-07-28 | Solution Architect | Header reformatted to comply with the official Standard Document Header in DOCUMENT_CONVENTIONS.md |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |