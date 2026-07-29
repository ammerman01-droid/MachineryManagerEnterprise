| Property | Value |
|----------|-------|
| **Document ID** | TE-0026 |
| **Title** | File Storage Technology Evaluation |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-27 |
| **Last Updated** | 2026-07-28 |

# Purpose

This document evaluates the technology used to store binary file content —
photographs, inspection documents, manuals, and similar attachments —
across the MachineryManagerEnterprise solution.

`03-TechnologyGapAnalysis.md` already identifies **Embedded File Storage**
as a documented gap (Priority: Medium), requiring a **File Synchronization
Strategy** as its capability driver. `ADR-0012 — Distributed Workspace
Architecture` further establishes that business attachments (photographs,
inspection documents, manuals) are transported separately from business
metadata, with attachment metadata remaining inside the synchronization
package while binary content remains independently transferable. This
evaluation selects the concrete storage technology that satisfies that
already-established architectural requirement.

The objective of this evaluation is to:

- select the storage technology for binary attachments across the Host
  (server) deployment;
- ensure the selected technology composes correctly with the
  Distributed Workspace synchronization model already approved under
  ADR-0012;
- preserve cloud neutrality and Open Source First compliance wherever
  possible, consistent with the architectural posture established across
  every prior Technology Evaluation.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Relationship with Previous Technology Evaluations

This evaluation does not supersede or depend on any previously approved
Technology Evaluation. It resolves the "File Synchronization Strategy"
capability driver identified as a dependency of the Embedded File Storage
gap in `03-TechnologyGapAnalysis.md`, and implements the storage side of
the attachment transport model already defined at the architecture level
by ADR-0012.

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0002 — Open Source First Policy
- ADR-0012 — Distributed Workspace Architecture
- 03-TechnologyGapAnalysis.md (Embedded File Storage gap)
- 02-CapabilityModel.md
- DependencyRules.md

---

# Scope

This evaluation covers:

- storage and retrieval of binary attachment content (photographs,
  inspection documents, manuals, and similar files) on the Host deployment;
- the abstraction layer the Application layer uses to read and write file
  content, independent of the underlying storage provider;
- the transport of binary content for Distributed Workspace synchronization
  packages, consistent with ADR-0012's requirement that binary content
  remain independently transferable from business metadata.

Out of scope:

- attachment **metadata** persistence (filename, size, content type,
  owning Aggregate) — this remains ordinary EF Core-persisted entity data,
  already covered by ADR-0006 / TE-0024.
- full-text indexing or search over document content — covered separately
  by the forthcoming TE-0027 (Search Engine Technology Evaluation).
- image/document processing (thumbnailing, OCR) — not currently a defined
  platform requirement.

---

# Functional Requirements

The selected solution shall support:

- upload, download, and deletion of binary file content, addressed by a
  stable, opaque identifier rather than a filesystem path;
- streaming reads and writes, to avoid materializing large files (e.g.
  high-resolution inspection photographs, multi-page PDF manuals) entirely
  in memory;
- an abstraction (`IFileStorage` or equivalent) that the Application layer
  depends on, so the concrete storage provider is fully replaceable without
  touching Application or Domain code;
- correct behavior under the Distributed Workspace synchronization model,
  where binary content must be independently transferable and content
  addressable for integrity verification (ADR-0012).

---

# Non-Functional Requirements

The solution should provide:

- deployment flexibility across on-premise, single-server, and cloud
  hosting, since the platform's customer base (construction companies)
  spans organizations of very different IT maturity and hosting
  preference;
- cloud neutrality wherever practical, consistent with the posture already
  established across prior evaluations (e.g. TE-0021, TE-0023);
- reasonable cost predictability for on-premise deployments, where ongoing
  cloud storage billing may be undesirable for some customers;
- adequate throughput for concurrent uploads from field technicians using
  the Desktop/Mobile clients (ADR-0013 / TE-0010) during maintenance
  inspections.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| Local Storage | File content stored directly on the Host's local/attached disk | Evaluated |
| Azure Blob Storage | Microsoft's managed cloud object storage service | Evaluated |
| MinIO | Self-hosted, open-source, S3-API-compatible object storage server | Evaluated |
| S3-Compatible Storage (Abstraction) | A vendor-neutral storage abstraction built against the S3 API, portable across AWS S3, MinIO, Cloudflare R2, Backblaze B2, and Wasabi | Evaluated |


| Capability | Local Storage | MinIO | Azure Blob | Amazon S3 |
|------------|---------------|--------|------------|-----------|
| On-Premise Support | Excellent | Excellent | Limited | Limited |
| Cloud Ready | Poor | Excellent | Excellent | Excellent |
| S3 Compatible | No | Yes | No | Native |
| Object Versioning | No | Yes | Yes | Yes |
| Scalability | Moderate | Excellent | Excellent | Excellent |
| Vendor Lock-In | None | None | High | High |
| Kubernetes Friendly | Moderate | Excellent | Excellent | Excellent |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Open Source & License Stability | Critical |
| A2 | Clean Architecture Compatibility | Critical |
| A3 | Deployment Flexibility (On-Premise / Cloud) | Critical |
| A4 | Cloud Neutrality | High |
| A5 | Distributed Workspace Sync Compatibility (ADR-0012) | High |
| A6 | Cost Predictability | High |
| A7 | Operational Complexity | Medium |
| A8 | Scalability | Medium |
| A9 | Developer Experience | Medium |

---

# Architecture Principle

File storage shall be accessed exclusively through an Application-layer
abstraction, never directly by any layer above Infrastructure.

```text
Application Layer
   IFileStorage  (Upload / Download / Delete / GetUri)
        │
        ▼
Infrastructure Layer
   LocalFileStorage | AzureBlobFileStorage | S3CompatibleFileStorage
        │
        ▼
   Physical storage (Disk / Azure Blob / MinIO / S3-compatible endpoint)
```

Domain entities shall never reference a storage technology; they shall
reference only a stable attachment identifier. SharedKernel shall never
reference a storage technology.

---

# 5. Local Storage Evaluation

## Overview

Local Storage means writing binary file content directly to the Host
server's local or network-attached filesystem, addressed through a
directory structure keyed by attachment identifier.

## Architectural Role

```text
Application Layer

   UploadInspectionPhotoCommandHandler
          │
          ▼
   IFileStorage.UploadAsync(attachmentId, stream)
          │
          ▼
Infrastructure Layer
   LocalFileStorage
          │
          ▼
   /var/machinerymanager/attachments/{orgId}/{attachmentId}
```

## Architectural Strengths

- Zero external dependency and zero additional infrastructure cost — the
  simplest possible deployment model, appealing for the smaller, less
  IT-mature construction companies described in the platform's target
  market.
- No network latency between the application and storage; reads and writes
  are as fast as the underlying disk.
- No licensing concern whatsoever, trivially compliant with ADR-0002.
- Straightforward backup story for a single-server deployment: the
  attachments directory can be backed up alongside the database using
  existing on-premise backup tooling.

## Architectural Weaknesses

- Does not scale horizontally: if the Host application is later deployed
  across multiple server instances (e.g. behind a load balancer), local
  disk storage on one instance is not visible to the others without a
  shared network filesystem, which reintroduces most of the complexity
  this option is meant to avoid.
- No built-in redundancy or durability guarantee beyond whatever the
  underlying disk and backup strategy provide; a single-disk failure can
  mean permanent data loss without a separate backup discipline.
- Weakest fit among the four candidates for the Distributed Workspace
  synchronization model (ADR-0012), since exposing local disk content for
  independent binary transfer to synchronizing clients requires
  additional custom infrastructure (e.g. a dedicated download endpoint)
  that the other candidates provide natively through pre-signed URLs.
- No native content-addressable or integrity-verification primitives;
  integrity verification (required by ADR-0012) must be implemented
  entirely in application code (e.g. computing and storing a hash
  alongside the file).

## Operational Characteristics

Requires only a configured directory path and standard filesystem
permissions; no additional service to install, configure, or monitor.

## Scalability

Poor beyond a single-server deployment; adequate for the platform's
smallest customer deployments but a genuine limitation for larger,
multi-instance hosting scenarios.

## Deployment Flexibility

Excellent for single-server, on-premise deployments — precisely the
scenario many of the platform's target construction companies are expected
to use, given the varied IT maturity described in the platform's target
market. Poor for horizontally scaled or fully managed cloud deployments.

## Cost

The lowest direct cost of any candidate: no additional service or
storage-tier billing, only the cost of local disk capacity already
provisioned for the Host server.

## Security

Relies entirely on filesystem-level permissions and the Host application's
own authorization checks; no additional access-control primitive is
provided by the storage layer itself, unlike the presigned-URL model
available to the cloud/object-storage candidates.

## Distributed Workspace Sync Compatibility

Weakest fit: ADR-0012 requires that binary content remain "independently
transferable," which object storage naturally supports through
presigned/temporary URLs. Local Storage requires building this capability
manually as a custom download endpoint, adding implementation and
maintenance burden not required by the other three candidates.

## Developer Experience

Simple and immediately familiar; no SDK, no client library, just standard
`System.IO` stream operations behind the `IFileStorage` abstraction.

## Maintainability

Good for the simple case; becomes a maintenance burden if the platform
later needs to scale horizontally, since migrating from local disk storage
to a shared/object storage backend would require a one-time, non-trivial
data migration.

## Typical Usage

```csharp
public sealed class LocalFileStorage : IFileStorage
{
    private readonly string _rootPath;

    public async Task UploadAsync(AttachmentId id, Stream content, CancellationToken ct)
    {
        var path = Path.Combine(_rootPath, id.OrganizationId.ToString(), id.Value.ToString());
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var fileStream = File.Create(path);
        await content.CopyToAsync(fileStream, ct);
    }
}
```

## Comparison with MinIO

| Aspect | Local Storage | MinIO |
|--------|-----------------|-------|
| Additional infrastructure | None | Self-hosted MinIO server required |
| Horizontal scalability | Poor | Good |
| Presigned URL support | No (custom implementation needed) | Native |
| Deployment complexity | Lowest | Low-Medium |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (fully abstracted behind `IFileStorage`) |
| Deployment Flexibility (Single-Server) | Excellent |
| Deployment Flexibility (Multi-Instance / Cloud) | Poor |
| Distributed Workspace Sync Compatibility | Weak (requires custom implementation) |
| Cost Predictability | Excellent |

## Relationship with the Other Candidates

Local Storage and the object-storage candidates (Azure Blob, MinIO,
S3-Compatible Storage) are not mutually exclusive at the architecture
level: because access happens exclusively through `IFileStorage`, Local
Storage can serve as the implementation for smaller, single-server
on-premise deployments while an object-storage implementation serves
larger or cloud-hosted deployments, selected per deployment via
configuration rather than a code change.

## Preliminary Conclusion

Local Storage remains a valid, low-cost option for small, single-server,
on-premise deployments, but is architecturally weaker than the
object-storage candidates for Distributed Workspace synchronization and
does not scale to multi-instance hosting without additional engineering.

---

# 6. Azure Blob Storage Evaluation

## Overview

Azure Blob Storage is Microsoft's managed, cloud-hosted object storage
service, tightly integrated with the broader Azure ecosystem and with
first-party .NET SDK support.

## Architectural Role

```text
Application Layer

   UploadInspectionPhotoCommandHandler
          │
          ▼
   IFileStorage.UploadAsync(attachmentId, stream)
          │
          ▼
Infrastructure Layer
   AzureBlobFileStorage (Azure.Storage.Blobs SDK)
          │
          ▼
   Azure Blob Storage container (cloud-managed)
```

## Architectural Strengths

- Fully managed: no server to install, patch, or operate; Microsoft
  handles durability, replication, and availability.
- Native presigned URL support (Shared Access Signatures), directly
  satisfying ADR-0012's requirement that binary content remain
  independently transferable, without custom engineering.
- Strong durability guarantees (geo-redundant storage tiers available) far
  exceeding what a single-server Local Storage deployment can offer.
- First-party .NET SDK (`Azure.Storage.Blobs`), with excellent
  documentation and long-term Microsoft support, aligning naturally with
  the platform's existing .NET 10 / Blazor stack.
- Scales horizontally without any additional engineering effort on the
  platform's part.

## Architectural Weaknesses

- **Not cloud-neutral**: Azure Blob Storage is a proprietary Microsoft
  service; adopting it as the platform's exclusive storage technology
  creates a direct dependency on Azure as a hosting provider, in tension
  with the cloud-neutral posture the platform has otherwise maintained
  across prior evaluations (e.g. TE-0023's Deployment Flexibility
  criterion, TE-0024's provider-independence discussion).
  While the `IFileStorage` abstraction keeps this dependency confined to
  Infrastructure, migrating away from Azure later would still require
  a full data migration of stored attachments.
- Ongoing usage-based billing (storage volume plus egress/transaction
  costs), which is a materially different cost model than Local Storage's
  fixed, already-provisioned disk cost — a real concern for
  cost-conscious, smaller construction companies in the platform's target
  market.
- Requires an active Azure subscription and internet connectivity from
  the Host, which is a poor fit for fully on-premise, disconnected, or
  air-gapped deployments some construction companies may require.
- Not open source; while the SDK itself is open source, the underlying
  service is a proprietary managed offering, which is a partial (though
  common, industry-accepted) tension with the spirit of ADR-0002 for a
  platform that otherwise favors open-source infrastructure components.

## Operational Characteristics

No self-hosted infrastructure to operate; operational responsibility shifts
entirely to Azure's SLA. Requires managing a connection string / SAS token
lifecycle as a configuration and secrets-management concern.

## Scalability

Excellent; Azure Blob Storage scales to essentially unlimited volume and
throughput without any platform-side engineering effort.

## Deployment Flexibility

Poor for fully on-premise or air-gapped deployments; excellent for
customers already committed to Azure-hosted deployment of the Host
application itself.

## Cost

Ongoing, usage-based billing; the least cost-predictable of the four
candidates for organizations that prefer fixed, on-premise infrastructure
costs.

## Security

Strong built-in access control via Shared Access Signatures (time-limited,
scope-limited tokens), Azure AD integration, and encryption at rest by
default — a stronger native security posture than Local Storage's reliance
on filesystem permissions alone.

## Distributed Workspace Sync Compatibility

Excellent: Shared Access Signatures provide exactly the "independently
transferable" binary content transport ADR-0012 requires, with built-in
expiration and scope control, requiring no custom engineering.

## Developer Experience

Excellent for a team already working in the .NET ecosystem; the
`Azure.Storage.Blobs` SDK is idiomatic, well documented, and requires
minimal boilerplate.

## Maintainability

Good; Microsoft's long-term support commitment to the SDK and service
reduces long-term maintenance risk, at the cost of the vendor-lock-in
concern noted above.

## Typical Usage

```csharp
public sealed class AzureBlobFileStorage : IFileStorage
{
    private readonly BlobContainerClient _container;

    public async Task UploadAsync(AttachmentId id, Stream content, CancellationToken ct)
    {
        var blob = _container.GetBlobClient(id.Value.ToString());
        await blob.UploadAsync(content, overwrite: true, ct);
    }

    public Uri GetTemporaryDownloadUri(AttachmentId id, TimeSpan validFor)
    {
        var blob = _container.GetBlobClient(id.Value.ToString());
        return blob.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(validFor));
    }
}
```

## Comparison with S3-Compatible Storage

| Aspect | Azure Blob Storage | S3-Compatible Storage |
|--------|----------------------|--------------------------|
| Cloud neutrality | Poor (Azure-proprietary) | Excellent (portable across providers) |
| Managed / self-hosted | Managed only | Both (AWS S3 managed, or self-hosted via MinIO) |
| Presigned URL support | Native (SAS tokens) | Native (S3 presigned URLs) |
| .NET SDK maturity | Excellent (first-party Microsoft) | Excellent (AWS SDK for .NET) |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (fully abstracted behind `IFileStorage`) |
| Deployment Flexibility | Poor for on-premise/air-gapped; excellent for Azure-hosted |
| Cloud Neutrality | Poor |
| Distributed Workspace Sync Compatibility | Excellent |
| Cost Predictability | Poor (usage-based billing) |

## Relationship with the Other Candidates

Azure Blob Storage is architecturally interchangeable with MinIO and
S3-Compatible Storage under the same `IFileStorage` abstraction; the
meaningful difference between them is not code structure but deployment
posture — managed-Azure versus self-hosted-or-multi-cloud.

## Preliminary Conclusion

Azure Blob Storage is an excellent technical fit for customers already
committed to Azure-hosted deployment, with the strongest native security
and sync-compatibility story of any candidate, but its proprietary,
Azure-specific nature is a poor fit as the platform's *exclusive* storage
technology given the platform's cloud-neutral and on-premise-friendly
target market.

---

# 7. MinIO Evaluation

## Overview

MinIO is a self-hosted, open-source, high-performance object storage
server that implements the Amazon S3 API. It can be deployed on-premise, in
a private data center, or in any cloud environment as a containerized
service, giving organizations full control over their storage
infrastructure while retaining S3 API compatibility.

## Architectural Role

```text
Application Layer

   UploadInspectionPhotoCommandHandler
          │
          ▼
   IFileStorage.UploadAsync(attachmentId, stream)
          │
          ▼
Infrastructure Layer
   S3CompatibleFileStorage (AWS SDK for .NET, pointed at MinIO endpoint)
          │
          ▼
   Self-hosted MinIO server (container, on-premise or cloud VM)
```

## Architectural Strengths

- Fully open source (AGPLv3 for the server; client SDKs are Apache 2.0),
  giving strong alignment with ADR-0002's Open Source First policy, though
  the AGPLv3 server license itself should be reviewed against the
  platform's distribution model (see Licensing note below).
- S3 API compatibility means the same `IFileStorage` implementation and
  the same client SDK code work identically whether pointed at a
  self-hosted MinIO instance or, later, a genuine AWS S3 bucket — a direct
  architectural benefit shared with the S3-Compatible Storage candidate
  below.
- Excellent fit for on-premise and air-gapped deployments, since the
  entire storage stack runs inside the customer's own infrastructure with
  no external network dependency, directly addressing the deployment
  flexibility gap identified in the Azure Blob Storage evaluation.
- Native presigned URL support (via the S3 API), satisfying ADR-0012's
  independently-transferable binary content requirement without custom
  engineering, identical in this respect to Azure Blob Storage.
- Predictable, fixed infrastructure cost for self-hosted deployments — no
  usage-based cloud billing.

## Architectural Weaknesses

- Requires operating a self-hosted service: container orchestration,
  disk provisioning, patching, and monitoring become the platform
  operator's responsibility, unlike Azure Blob Storage's fully managed
  model.
- **Licensing note**: MinIO's server is licensed under AGPLv3, a strong
  copyleft license. Unlike the RPL 1.5 concern raised for AutoMapper in
  TE-0023, this does not affect the platform's own source code (MinIO is
  used as an external service accessed over the network, not linked into
  the application), but it does mean MinIO itself cannot be forked,
  modified, and redistributed as part of a closed-source product without
  triggering AGPLv3's obligations — a consideration only relevant if the
  platform ever intended to redistribute a modified MinIO binary, which is
  not the case here.
- Redundancy and durability are the operator's responsibility to configure
  correctly (erasure coding, multi-node clusters), unlike Azure Blob
  Storage's built-in geo-redundancy options.
- Smaller ecosystem of managed hosting offerings than AWS S3 itself,
  though this is mitigated by the fact that the same S3-compatible code
  can point at a genuine AWS S3 bucket if a customer later prefers a fully
  managed option.

## Operational Characteristics

Requires deploying and operating a MinIO container (or cluster, for
production-grade redundancy) as part of the Host deployment, most likely
alongside the SQL Server database in the same on-premise or cloud
environment.

## Scalability

Good: MinIO supports distributed, multi-node deployments with erasure
coding for both capacity and redundancy, though this requires deliberate
operational setup, unlike Azure Blob Storage's transparent scaling.

## Deployment Flexibility

Excellent: MinIO can run on-premise, in a private data center, or in any
public cloud, directly addressing the platform's need to serve customers
across a wide range of IT maturity and hosting preference, including fully
disconnected or air-gapped environments.

## Cost

Fixed infrastructure cost (compute and disk for the MinIO service itself);
no usage-based billing, making cost highly predictable for on-premise
deployments, an advantage over Azure Blob Storage's ongoing billing model.

## Security

S3-compatible access policies, presigned URLs, and encryption at rest
(server-side, when configured) provide a security posture broadly
comparable to Azure Blob Storage's, though correct configuration is the
platform operator's responsibility rather than a managed default.

## Distributed Workspace Sync Compatibility

Excellent: MinIO's native S3 presigned URL support satisfies ADR-0012's
independently-transferable binary content requirement identically to
Azure Blob Storage, without custom engineering.

## Developer Experience

Good: the same AWS SDK for .NET used against genuine AWS S3 works
unmodified against MinIO, so developers work against a single, well
documented, widely known API regardless of the deployment target.

## Maintainability

Good, provided the platform operator has the operational capability to run
a containerized service — a reasonable assumption given the platform
already requires operating a SQL Server instance and a .NET host.

## Typical Usage

```csharp
public sealed class S3CompatibleFileStorage : IFileStorage
{
    private readonly IAmazonS3 _client;
    private readonly string _bucket;

    public async Task UploadAsync(AttachmentId id, Stream content, CancellationToken ct)
    {
        await _client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _bucket,
            Key = id.Value.ToString(),
            InputStream = content
        }, ct);
    }
}
```

```json
// appsettings.json — pointed at a self-hosted MinIO endpoint
{
  "FileStorage": {
    "ServiceUrl": "https://minio.internal.customer.local:9000",
    "Bucket": "attachments"
  }
}
```

## Comparison with Azure Blob Storage

| Aspect | MinIO | Azure Blob Storage |
|--------|-------|-----------------------|
| Hosting model | Self-hosted (any environment) | Managed (Azure only) |
| Operational burden | Higher (operator-managed) | Lower (fully managed) |
| Cost model | Fixed infrastructure cost | Usage-based billing |
| Air-gapped / on-premise support | Excellent | Poor |
| Open Source (ADR-0002) | Yes (AGPLv3 server) | No (proprietary managed service) |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (fully abstracted behind `IFileStorage`) |
| Deployment Flexibility | Excellent (on-premise, air-gapped, or cloud) |
| Cloud Neutrality | Excellent |
| Distributed Workspace Sync Compatibility | Excellent |
| Cost Predictability | Excellent (fixed infrastructure cost) |

## Relationship with S3-Compatible Storage (Abstraction)

MinIO is a concrete, self-hostable implementation of the S3 API. The
S3-Compatible Storage candidate evaluated next is the architectural pattern
of building against that same API generically, so that MinIO becomes one
of several interchangeable *endpoints* the platform can target, rather than
a hard-coded dependency.

## Preliminary Conclusion

MinIO is the strongest single candidate for the platform's stated need for
deployment flexibility across on-premise, air-gapped, and cloud
environments, combined with fixed, predictable cost and full ADR-0002
alignment for the platform's own codebase.

---

# 8. S3-Compatible Storage Evaluation (Abstraction Standard)

## Overview

This candidate is not a specific vendor but an **architectural pattern**:
building the `IFileStorage` implementation against the Amazon S3 API as a
de facto industry-standard object storage interface, using the AWS SDK for
.NET configured with a custom service endpoint. Because MinIO, AWS S3,
Cloudflare R2, Backblaze B2, and Wasabi all implement the same S3 API, this
single implementation becomes portable across all of them through
configuration alone.

## Architectural Role

```text
Application Layer

   IFileStorage
          │
          ▼
Infrastructure Layer
   S3CompatibleFileStorage (single implementation, AWS SDK for .NET)
          │
          ├──► MinIO (self-hosted, on-premise)
          ├──► AWS S3 (managed, cloud)
          ├──► Cloudflare R2 (managed, cloud)
          └──► Any other S3-API-compatible endpoint
```

## Architectural Strengths

- Maximum deployment flexibility: the *same code* serves on-premise
  (pointed at MinIO), fully managed cloud (pointed at AWS S3 or Cloudflare
  R2), or hybrid deployments, selected entirely through configuration —
  the strongest cloud-neutrality result of any candidate evaluated.
- Avoids a second, Azure-specific code path: rather than maintaining both
  an `AzureBlobFileStorage` implementation and a separate S3-compatible
  implementation, the platform maintains a single, well-tested
  implementation against a single, extremely widely adopted API standard.
- Future-proof against changing customer hosting preferences: a customer
  who starts on-premise with MinIO can migrate to a managed cloud
  S3-compatible provider later (or vice versa) without any application
  code change, only a configuration and data-migration exercise.
- The AWS SDK for .NET is mature, actively maintained, MIT/Apache-2.0
  licensed, and fully compliant with ADR-0002.

## Architectural Weaknesses

- Does not, by itself, provide a managed storage service — it is an
  abstraction pattern, not an infrastructure decision; the platform must
  still choose a concrete endpoint (MinIO for self-hosted, or a managed
  S3-compatible cloud provider) for any given deployment, meaning this
  candidate is best understood as *complementary to* MinIO rather than a
  fully independent alternative to it.
- Subtle behavioral differences exist across S3-API implementations
  (e.g. multipart upload limits, some advanced storage-class or lifecycle
  features present in AWS S3 but not in MinIO or vice versa); the platform
  must restrict itself to the common subset of S3 API features to preserve
  true portability, which slightly constrains the feature set relative to
  using a single provider's SDK natively (as the Azure Blob Storage
  candidate would).
- Slightly less idiomatic for a team already working primarily in the
  Microsoft/.NET ecosystem than the first-party `Azure.Storage.Blobs` SDK,
  though the AWS SDK for .NET is itself well documented and widely used.

## Operational Characteristics

Identical to whichever concrete S3-compatible endpoint is selected for a
given deployment (MinIO's operational characteristics for self-hosted
deployments, or a managed provider's operational characteristics for cloud
deployments); the abstraction itself introduces no additional runtime
component.

## Scalability

Inherits the scalability characteristics of the selected endpoint: good
(MinIO, self-managed) to excellent (AWS S3 or another fully managed
S3-compatible provider), without requiring any change to the platform's
own code either way.

## Deployment Flexibility

The strongest of any candidate evaluated: the same `IFileStorage`
implementation supports every deployment posture — on-premise, air-gapped,
private cloud, and public cloud — by configuration alone.

## Cost

Depends entirely on the selected endpoint: fixed and predictable when
pointed at self-hosted MinIO, usage-based when pointed at a managed
provider such as AWS S3 — giving each customer deployment the cost model
that suits their preference, rather than locking the platform into one
model architecture-wide.

## Security

Inherits the S3 API's presigned URL and bucket-policy access-control model
uniformly across every endpoint, giving a single, consistent security
implementation to review and test regardless of which concrete provider a
given deployment uses.

## Distributed Workspace Sync Compatibility

Excellent, and uniformly so across every possible endpoint: S3 presigned
URLs satisfy ADR-0012's independently-transferable binary content
requirement identically whether the underlying endpoint is MinIO, AWS S3,
or any other S3-compatible provider.

## Developer Experience

Good: developers learn and test against a single, well documented API
(the AWS SDK for .NET) regardless of deployment target, and can run
integration tests locally against a MinIO container without needing any
cloud account.

## Maintainability

Excellent: a single implementation to test and maintain, rather than
separate Azure- and S3-specific code paths, directly reducing the
long-term maintenance surface of the Infrastructure layer's storage code.

## Typical Usage

Identical implementation to the MinIO example above (`S3CompatibleFileStorage`
using `IAmazonS3`); the only difference between a MinIO-backed deployment
and an AWS-S3-backed deployment is the `ServiceUrl` and credential
configuration, not the code.

## Comparison with Azure Blob Storage

| Aspect | S3-Compatible Storage | Azure Blob Storage |
|--------|--------------------------|------------------------|
| Portability across providers | Excellent | None (Azure only) |
| Managed cloud option available | Yes (AWS S3, Cloudflare R2, etc.) | Yes (Azure only) |
| Self-hosted option available | Yes (MinIO) | No |
| First-party .NET SDK | Good (AWS SDK, widely used but not Microsoft-authored) | Excellent (Microsoft-authored) |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent (single abstraction, provider-agnostic) |
| Deployment Flexibility | Excellent (broadest of any candidate) |
| Cloud Neutrality | Excellent (strongest of any candidate) |
| Distributed Workspace Sync Compatibility | Excellent |
| Cost Predictability | Depends on selected endpoint; flexible by design |

## Relationship with MinIO

The S3-Compatible Storage abstraction and MinIO are not competing choices;
they are complementary. Adopting the S3-Compatible Storage pattern as the
platform's `IFileStorage` implementation and deploying MinIO as the default
self-hosted endpoint gives the platform both architectural portability and
a concrete, ready-to-deploy open-source storage backend in one decision.

## Preliminary Conclusion

The S3-Compatible Storage abstraction pattern is the strongest architectural
choice for the `IFileStorage` implementation itself, since it delivers
maximum deployment flexibility and cloud neutrality without foreclosing any
future hosting option, and composes directly with MinIO as the default
concrete endpoint.

---

# 9. Overall Technology Comparison

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|-----------------|------------------------|--------------|---------|
| `IFileStorage` implementation (all deployments) | S3-Compatible Storage (AWS SDK for .NET) | Azure Blob Storage (Azure-committed customers only) | Single, portable storage abstraction |
| Default self-hosted / on-premise endpoint | MinIO | Local Storage (smallest single-server deployments only) | Open-source, cloud-neutral object storage |
| Managed cloud endpoint (optional, per customer) | AWS S3 or another S3-compatible managed provider | Azure Blob Storage (if customer is Azure-committed) | Fully managed durability and scaling |

## Capability Comparison

| Capability | Local Storage | Azure Blob Storage | MinIO | S3-Compatible (Abstraction) |
|------------|-----------------|------------------------|-------|--------------------------------|
| Open Source (ADR-0002 compliant) | Yes | No | Yes (AGPLv3 server) | Yes (SDK) |
| Cloud neutrality | N/A (no cloud) | Poor | Excellent | Excellent |
| Native presigned URL support | No | Yes | Yes | Yes |
| Distributed Workspace sync fit (ADR-0012) | Weak | Excellent | Excellent | Excellent |
| On-premise / air-gapped support | Excellent | Poor | Excellent | Excellent (via MinIO endpoint) |
| Managed cloud option | No | Yes | No (self-hosted only) | Yes (via AWS S3 / other providers) |
| Cost predictability | Excellent | Poor | Excellent | Depends on endpoint |
| Horizontal scalability | Poor | Excellent | Good | Good–Excellent (endpoint dependent) |

## Cloud Neutrality Assessment

S3-Compatible Storage and MinIO both score highest for cloud neutrality,
since the S3 API is implemented by nearly every major storage provider.
Azure Blob Storage scores lowest, since it locks the platform's storage
layer to a single proprietary vendor. Local Storage is cloud-neutral only
in the trivial sense of having no cloud dependency at all, at the cost of
scalability and native sync support.

## Enterprise Suitability

| Criterion | Local Storage | Azure Blob Storage | MinIO | S3-Compatible (Abstraction) |
|-----------|-----------------|------------------------|-------|--------------------------------|
| Suitable as platform-wide default | No | Conditionally (Azure-committed customers) | Yes | Yes (as the governing abstraction) |
| Suitable for smallest single-server customers | Yes | No | Conditionally | Conditionally |
| Suitable for large, multi-instance deployments | No | Yes | Yes | Yes |

## Risk Assessment

| Risk | Affected Candidate | Severity |
|------|--------------------|----------|
| Vendor lock-in to a single cloud provider | Azure Blob Storage (if adopted exclusively) | High |
| Manual integrity-verification and sync engineering burden | Local Storage | Medium |
| Operational burden of self-hosting | MinIO | Medium |
| Subtle S3-API behavioral differences across providers | S3-Compatible Storage (Abstraction) | Low |
| Data loss without a disciplined backup strategy | Local Storage | Medium |

## Overall Evaluation

Local Storage remains viable only for the platform's smallest,
single-server, on-premise deployments, and is architecturally weaker for
Distributed Workspace synchronization. Azure Blob Storage offers the
strongest fully managed experience but conflicts with the platform's
cloud-neutral posture if adopted as the exclusive storage technology.
MinIO and the S3-Compatible Storage abstraction pattern together offer the
best combination of open-source alignment, deployment flexibility, cost
predictability, and Distributed Workspace synchronization compatibility,
without foreclosing a fully managed cloud option (including Azure, via a
compatible endpoint, or AWS S3 directly) for customers who prefer one.

---

# 10. Final Recommendation

## Core Technology Stack

| Responsibility | Selected Technology | Rationale |
|-----------------|----------------------|-----------|
| `IFileStorage` implementation | S3-Compatible Storage (AWS SDK for .NET) | Single, portable implementation; maximum cloud neutrality and deployment flexibility |
| Default self-hosted endpoint | MinIO | Open source, on-premise/air-gapped friendly, fixed cost, native presigned URL support |
| Optional managed cloud endpoint | AWS S3 or another S3-compatible managed provider | Available per customer without any application code change |
| Local Storage | Retained as a secondary implementation for the smallest single-server deployments only | Zero-infrastructure option where object storage is not justified |

## Recommended Architecture

```text
Application Layer
   IFileStorage
        │
        ▼
Infrastructure Layer
   S3CompatibleFileStorage   (default, configuration-selected endpoint)
   LocalFileStorage          (fallback, smallest deployments only)
        │
        ├──► MinIO (self-hosted, default for on-premise)
        ├──► AWS S3 / other managed S3-compatible provider (optional)
        └──► Local disk (smallest deployments only)
```

## Security Recommendations

Presigned URLs shall always be issued with the shortest practical
expiration window, and access shall be scoped per-organization at the
bucket/prefix level to prevent cross-tenant attachment access.

## Cloud Neutrality

The recommended stack achieves the platform's strongest possible cloud
neutrality result: the same code and the same `IFileStorage` contract
serve on-premise, air-gapped, and any public cloud deployment without
modification.

## AI Readiness

Not directly applicable, though a content-addressable, S3-compatible
storage layer is a reasonable long-term foundation should the platform
later need to store embeddings-adjacent binary artifacts for the AI
capabilities referenced in `02-CapabilityModel.md`.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| S3-Compatible Storage (Abstraction, AWS SDK for .NET) | **Approved** as the primary `IFileStorage` implementation |
| MinIO | **Approved** as the default self-hosted endpoint |
| Local Storage | Approved, restricted to smallest single-server deployments |
| Azure Blob Storage | Not adopted as the platform default; may be supported later via an Azure-specific `IFileStorage` implementation if a specific customer requires it |

---

# Decision Summary

- ✔ Clean Architecture preserved
- ✔ .NET 10 Compatibility
- ✔ Open Source First Policy (ADR-0002) compliance
- ✔ Strong cloud neutrality (S3 API portable across providers)
- ✔ Distributed Workspace synchronization compatibility (ADR-0012)
- ✔ Deployment flexibility across on-premise, air-gapped, and cloud

Given that this evaluation resolves a previously undecided architectural
gap (Embedded File Storage, per `03-TechnologyGapAnalysis.md`) rather than
modifying an existing ADR, this decision has been formally recorded in
**ADR-0020 — File Storage Strategy**, consistent with the same governance
pattern followed for TE-0024 / ADR-0019.

---

# Related ADR

```
ADR-0020 — File Storage Strategy (new)
```

---

# Related Documents

- ADR-0002 — Open Source First Policy
- ADR-0012 — Distributed Workspace Architecture
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- ADR-0020 — File Storage Strategy
- 03-TechnologyGapAnalysis.md
- 02-CapabilityModel.md

---

# References

https://learn.microsoft.com/azure/storage/blobs/

https://min.io/docs/minio/linux/index.html

https://aws.amazon.com/sdk-for-net/

https://docs.aws.amazon.com/AmazonS3/latest/API/Welcome.html

---

# Revision History
| Version | Date       | Author             | Description                                                                   |
|---------|------------|--------------------|-------------------------------------------------------------------------------|
| 1.0.0   | 2026-07-27 | Solution Architect | Initial evaluation; recommends S3-Compatible Storage abstraction with MinIO as default self-hosted endpoint, resolving the Embedded File Storage gap from the Technology Gap Analysis |
| 1.1.0   | 2026-07-27 | Solution Architect | Updated to reference ADR-0020, created to formalize the File Storage Strategy |
| 1.2.0   | 2026-07-28 | Solution Architect | Table Added to Candidate Technologies                                         |
| 1.3.0   | 2026-07-28 | New section added (Evaluation Scope)                                                               |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0                                     |