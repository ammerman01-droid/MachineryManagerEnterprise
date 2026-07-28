# Architecture Decision Record

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0020 |
| **Version** | 1.1.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-27 |
| **Last Updated** | 2026-07-28 |

---

# Title

File Storage Strategy

---

# Status

Accepted

---

# Context

`03-TechnologyGapAnalysis.md` identifies **Embedded File Storage** as a
documented architectural gap (Priority: Medium), depending on a "File
Synchronization Strategy" capability that had not yet been resolved by any
approved ADR.

`ADR-0012 — Distributed Workspace Architecture` establishes that business
attachments — photographs, inspection documents, and manuals — shall be
transported separately from business metadata during synchronization, with
attachment metadata remaining inside the synchronization package while
binary content remains independently transferable. That decision defined
the transport *model* but did not select a concrete storage *technology*.

`TE-0026 — File Storage Technology Evaluation` evaluated Local Storage,
Azure Blob Storage, MinIO, and an S3-Compatible Storage abstraction pattern
against this requirement, together with the platform's established
cloud-neutral, Open Source First (ADR-0002) posture and its need to serve
customers across a wide range of hosting preferences, including on-premise
and air-gapped deployments. It recommended an S3-Compatible Storage
abstraction as the platform's `IFileStorage` implementation, with MinIO as
the default self-hosted endpoint, and deferred formal recording of that
decision to this ADR.

---

# Decision

The platform shall access all binary attachment content exclusively
through an Application-layer abstraction, `IFileStorage`, implemented in
Infrastructure as follows:

- The primary `IFileStorage` implementation shall be built against the
  **Amazon S3 API** using the AWS SDK for .NET, making the implementation
  portable across any S3-API-compatible endpoint through configuration
  alone.
- **MinIO** shall be the default, open-source, self-hosted object storage
  endpoint for on-premise and air-gapped deployments.
- A fully managed S3-compatible provider (for example, AWS S3) may be
  configured as the endpoint for cloud-hosted deployments, without any
  application code change.
- A secondary **Local Storage** implementation of `IFileStorage` is
  retained for the smallest, single-server, on-premise deployments where
  object storage is not justified.
- **Azure Blob Storage** is not adopted as the platform default. A
  dedicated Azure Blob Storage implementation of `IFileStorage` may be
  added later for specific customers already committed to Azure-hosted
  deployment, without affecting this decision.

---

# Decision Drivers

- Cloud neutrality
- Deployment flexibility (on-premise, air-gapped, and cloud)
- Distributed Workspace synchronization compatibility (ADR-0012)
- Open Source Policy compliance (ADR-0002)
- Cost predictability across varied customer hosting preferences

---

# Alternatives Considered

## Azure Blob Storage as the Exclusive Storage Technology

Rejected as the platform default because it is a proprietary,
Azure-specific managed service, creating vendor lock-in that conflicts with
the platform's cloud-neutral posture and is unsuitable for on-premise or
air-gapped customer deployments.

## Local Storage as the Exclusive Storage Technology

Rejected as the platform-wide default because it does not scale beyond a
single server, lacks native presigned-URL support, and requires custom
engineering to satisfy ADR-0012's independently-transferable binary content
requirement. It is retained only as a secondary implementation for the
smallest single-server deployments.

## A Provider-Specific SDK per Cloud (Azure and AWS Maintained in Parallel)

Rejected because maintaining two separate, provider-specific
`IFileStorage` implementations increases long-term maintenance burden
without a compensating architectural benefit over a single S3-compatible
implementation that already serves both self-hosted and most managed cloud
scenarios.

---

# Migration Strategy

The storage implementation shall evolve through the following stages.

Local File Storage

        │

        ▼

Hybrid Storage

        │

        ▼

Object Storage (MinIO)

        │

        ▼

Cloud Storage

This staged migration minimizes operational risk while preserving the Storage Abstraction defined by the architecture.

---

# Consequences

## Positive

- The same `IFileStorage` implementation and code path serves on-premise,
  air-gapped, and public cloud deployments, selected entirely through
  configuration.
- MinIO gives every deployment, including the smallest customers, access to
  presigned URLs and native Distributed Workspace synchronization
  compatibility without a cloud subscription.
- Customers preferring a managed cloud experience can point the same
  implementation at AWS S3 or another S3-compatible managed provider
  without any code change.

## Negative

- Deployments that choose MinIO take on the operational responsibility of
  running a self-hosted service (container orchestration, disk
  provisioning, patching), rather than relying on a fully managed offering.
- The platform does not benefit from Azure Blob Storage's first-party,
  Microsoft-authored SDK ergonomics by default; an Azure-specific
  implementation, if added later for specific customers, is additional
  Infrastructure-layer code to maintain.

## Trade-offs

Some advanced, provider-specific storage features (for example, certain
Azure-only capabilities) are deliberately not used, in favor of the common
subset of the S3 API that remains portable across MinIO, AWS S3, and other
S3-compatible providers.

## Future Limitations

If a specific customer requires Azure Blob Storage for organizational or
compliance reasons, a dedicated `AzureBlobFileStorage` implementation of
the existing `IFileStorage` interface may be added without affecting this
decision or any other deployment's configuration.

---

# Architecture Impact

- **Domain** — No impact. Domain entities reference only a stable
  attachment identifier, never a storage technology.
- **Application** — Depends only on the `IFileStorage` abstraction
  (Upload / Download / Delete / GetTemporaryDownloadUri).
- **Infrastructure** — Hosts the `S3CompatibleFileStorage` implementation
  (AWS SDK for .NET) as the default, and the `LocalFileStorage`
  implementation for the smallest single-server deployments. Both are
  selected per deployment via configuration.
- **Presentation** — No impact; the storage technology remains fully
  invisible above the Application layer.

---

# Implementation Notes

- The concrete storage endpoint (MinIO, AWS S3, or another S3-compatible
  provider) shall be selected per deployment through configuration
  (service URL, bucket name, credentials), never through a code branch.
- Presigned URLs used to satisfy ADR-0012's synchronization requirement
  shall always be issued with the shortest practical expiration window.
- Access shall be scoped per-organization at the bucket/prefix level to
  prevent cross-tenant attachment access.
- Attachment metadata (filename, size, content type, owning Aggregate)
  continues to be persisted as ordinary EF Core entity data, per ADR-0006
  and ADR-0019; this ADR governs binary content only.

---

# Compliance Rules

1. All access to binary attachment content shall go through the
   `IFileStorage` abstraction; no layer above Infrastructure shall
   reference a storage SDK directly.

2. The default `IFileStorage` implementation shall be built against the S3
   API (AWS SDK for .NET), portable across MinIO and any other
   S3-compatible endpoint through configuration alone.

3. MinIO shall be the default self-hosted endpoint for on-premise and
   air-gapped deployments.

4. The `LocalFileStorage` implementation shall be used only for the
   smallest, single-server deployments where object storage is not
   justified, and shall never be assumed to support horizontal scaling.

5. Any Azure Blob Storage implementation added for a specific customer
   shall implement the same `IFileStorage` interface and shall not become
   the platform-wide default without a new or amended ADR.

---

# Related Technology Evaluation

TE-0026 — File Storage Technology Evaluation

---

# Related Proof of Concept

Not Required

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0012 — Distributed Workspace Architecture
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- TE-0026 — File Storage Technology Evaluation
- 03-TechnologyGapAnalysis.md

---

# References

https://min.io/docs/minio/linux/index.html

https://aws.amazon.com/sdk-for-net/

https://docs.aws.amazon.com/AmazonS3/latest/API/Welcome.html

---

# Review

| Role | Name | Date |
|------|------|------|
| Solution Architect | | |

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-27 | Initial decision, formalizing the File Storage Strategy recommended by TE-0026 |
| 1.1.0 | 2026-07-28 | Migration Strategy added |