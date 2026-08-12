| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | TE-0023            |
| **Title**        | Object Mapping Technology Evaluation |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-26         |
| **Last Updated** | 2026-08-08         |

# Purpose

This document re-evaluates the object mapping technology used across the
MachineryManagerEnterprise solution.

Object mapping is a cross-cutting concern that touches nearly every module
of the platform: Assets, Fleet, Preventive Maintenance, Corrective
Maintenance, Inventory, Procurement, Work Orders and Reporting all depend on
translating Entities and Aggregates into Data Transfer Objects (DTOs), and
translating Commands and Requests back into domain objects. Because this
concern appears in every Application layer handler across every module, the
correctness, performance and long-term stability of the chosen mapping
technology has a multiplicative effect on the maintainability of the entire
codebase.

The objective of this evaluation is to:

- confirm whether Mapster, the currently approved object mapping technology
  (TE-0006 / ADR-0008), remains the correct choice as the platform grows
  toward Distributed Workspace synchronization, Desktop/Mobile clients
  (TE-0010 — .NET MAUI), and a substantially larger number of DTOs than
  existed at the time of the original evaluation;
- evaluate Mapperly, a compile-time source-generated mapper that has matured
  significantly since TE-0006 was written and did not exist as a realistic
  enterprise candidate at that time;
- reassess AutoMapper considering a material licensing change that occurred
  after the original TE-0006 evaluation;
- reconfirm manual mapping as a baseline comparison and as a formally
  recognized escape hatch for complex, business-rule-heavy mappings.

This evaluation either reaffirms ADR-0008 or recommends its supersession.
It does not itself change the approved architecture; any change to the
approved technology requires a new or amended ADR.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Relationship with TE-0006 / ADR-0008

Object mapping was originally evaluated in **TE-0006 — Mapster**, and formally
approved through **ADR-0008 — Use Mapster** (Status: Accepted).

This evaluation does not treat that decision as void, and it does not
represent an unprompted architectural redesign. It exists because the
Solution Architect requested a full, symmetric re-evaluation covering
Mapster as the incumbent alongside every realistic alternative, using the
same evaluation depth applied to every other Technology Evaluation produced
for this project.

Mapster is therefore included below as the **Incumbent** candidate,
evaluated on exactly the same criteria, with exactly the same depth, as
every other candidate. TE-0006 remains the historical record of the
original decision. This document supersedes TE-0006 only if the final
recommendation below selects a different technology than Mapster.

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0002 — Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- ADR-0008 — Use Mapster
- ADR-0011 — Use MediatR
- ADR-0013 — Client Application Architecture (.NET MAUI)
- TE-0006 — Mapster (original evaluation)
- TE-0010 — Desktop & Mobile Framework Evaluation
- ../05-development/01-SolutionStructure.md
- ../05-development/02-ProjectStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md
---

# Scope

The evaluation covers object-to-object mapping used for:

- Entity → DTO projection (read side / queries)
- Command / Request → Entity or Value Object construction (write side)
- DTO → DTO transformation across module boundaries
- Aggregate → Read Model projection for reporting scenarios
- Mapping used inside the Application layer only

The following are explicitly **out of scope** for this evaluation:

- Persistence mapping (EF Core entity configuration, fluent mapping,
  value converters) — covered by TE-0004 / ADR-0006 and the forthcoming
  TE-0024 (Data Access Technology Evaluation).
- Serialization used for Distributed Workspace synchronization packages —
  covered separately under GAP-003 (Synchronization Package Format).
- JSON serialization for API responses (System.Text.Json) — an orthogonal
  concern handled at the Presentation layer boundary, not by the object
  mapping library.

---

# Functional Requirements

The selected solution shall support:

- flattening and unflattening of nested object graphs;
- projection to `IQueryable` so that mapping expressions can be translated
  into SQL by Entity Framework Core rather than executed in memory;
- mapping of collections, including nested collections of Value Objects;
- conditional mapping and custom conversion logic (e.g. enum-to-string,
  computed fields, currency formatting);
- two-way mapping where explicitly required by a module;
- integration with FluentValidation (TE-0005 / ADR-0007) and MediatR
  (TE-0009 / ADR-0011) pipelines without violating layer boundaries;
- graceful handling of nullable reference types under `Nullable enable`.

---

# Non-Functional Requirements

The solution should provide:

- high runtime performance with minimal heap allocations, given the volume
  of mapping operations expected across meter readings, maintenance
  records, and fleet-wide reporting queries;
- low startup overhead, relevant both for the Host application and for
  Desktop/Mobile clients where cold-start time matters more than on a
  server;
- compile-time safety wherever practically achievable, to reduce the class
  of "renamed a property, forgot to update the mapping" defects;
- long-term license stability compatible with the Open Source First policy
  (ADR-0002);
- excellent .NET 10 compatibility, including compatibility with Native AOT
  and trimming for future Desktop/Mobile builds;
- low maintenance burden as the domain model grows across an increasing
  number of bounded contexts.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| Mapster | Reflection + compiled-expression mapper with optional source generation | **Incumbent** (ADR-0008) |
| Mapperly | Roslyn-based compile-time source-generated mapper | Evaluated |
| AutoMapper | Convention-based reflection mapper | Evaluated |
| Manual Mapping | Hand-written mapping code / extension methods | Evaluated |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Open Source & License Stability | Critical |
| A2 | .NET 10 Compatibility | Critical |
| A3 | Clean Architecture Compatibility | Critical |
| A4 | Performance | High |
| A5 | Compile-Time Safety | High |
| A6 | Developer Experience | High |
| A7 | Maintainability at Scale | High |
| A8 | Community & Maturity | Medium |
| A9 | AI Compatibility | Low |
| A10 | Cloud & Deployment Neutrality | Medium |
| A11 | Migration Cost from Current State | Medium |

---

# Architecture Principle

Object mapping shall remain confined to the Application layer.

```text
Domain Layer (Entities, Aggregates, Value Objects)

        │
        ▼

Application Layer
   Commands / Queries / Handlers
   Mapping happens here only

        │
        ▼

Presentation Layer (DTOs / View Models / API Contracts)
```

Domain entities shall never reference a mapping library.

SharedKernel shall never reference a mapping library.

Infrastructure shall never perform DTO mapping; its responsibility ends at
persistence.

Mapping configuration shall remain centralized per module, in a dedicated
mapping class or generated mapper, rather than being scattered ad hoc
across individual command/query handlers.

---

# 6. Mapster Evaluation (Incumbent)

## Overview

Mapster is a convention-based object mapper for .NET. It supports both
runtime reflection/compiled-expression mapping (the default mode) and
optional Roslyn source generation through `Mapster.Tool` and
`MapsterMapper`, activated via `[AdaptTo]` / `[AdaptFrom]` / `[MapperAttribute]`
annotations. It was originally approved for MachineryManagerEnterprise under
ADR-0008, following TE-0006.

## Architectural Role

```text
Application Layer

   CreateAssetCommandHandler
          │
          ▼
   entity.Adapt<AssetDto>()
          │
          ▼
   Presentation Layer (DTO)
```

For read-side queries, Mapster is used directly against `IQueryable<T>`:

```text
Repository (IQueryable<Asset>)
          │
          ▼
   query.ProjectToType<AssetListItemDto>()
          │
          ▼
   Translated to SQL by EF Core
          │
          ▼
   Executed against SQL Server
```

Mapster is used exclusively inside Application layer handlers and dedicated
mapping extension classes (e.g. `AssetMappingExtensions`). It is never
referenced by Domain or Infrastructure projects.

## Architectural Strengths

- Excellent runtime performance, consistently competitive with hand-written
  mapping code in independent benchmarks.
- Supports both zero-configuration convention mapping (`Adapt<T>()`) for the
  common case and explicit `TypeAdapterConfig` for cases requiring custom
  rules.
- Optional compile-time source generation removes reflection entirely when
  enabled, closing most of the performance and cold-start gap with
  Mapperly.
- Native `ProjectToType<T>()` support for `IQueryable`, translating cleanly
  into SQL through EF Core — critical for reporting and list-view queries
  that must not materialize entire entity graphs into memory.
- Minimal ceremony for the common case: a single DTO property rename or
  addition typically requires no configuration change at all, because
  Mapster maps by convention.
- Supports flattening (`Engine.SerialNumber` → `EngineSerialNumber`) out of
  the box, which is heavily used across Asset and Component DTOs.
- Global configuration can be unit tested via `TypeAdapterConfig.GlobalSettings.Compile()`
  to catch missing mappings before runtime.

## Architectural Weaknesses

- When source generation is not explicitly enabled, mapping still relies on
  compiled expression trees built at startup or first use rather than being
  fully compile-time verified — mapping errors on complex graphs can surface
  at runtime instead of at build time.
- Smaller ecosystem and fewer learning resources than AutoMapper, though
  this gap has narrowed considerably since the original TE-0006 evaluation.
- Global `TypeAdapterConfig` registration can become an implicit, hard to
  trace source of behavior if configuration discipline is not enforced
  across modules.
- Enabling source generation requires an additional build-time package
  (`Mapster.Tool`) and slightly more setup than the default mode, so teams
  often ship without it unless explicitly directed to enable it.

## Operational Characteristics

Mapster requires no dedicated runtime infrastructure. Mapping configuration
is registered once at application startup (typically inside a module's
`DependencyInjection.cs`) and reused for the lifetime of the process. No
external service, cache, or database is involved.

## Scalability

Performance remains stable as the number of mapped types grows. Startup
cost increases marginally with the number of registered `TypeAdapterConfig`
rules, which remains negligible even at the scale of the 20+ modules
described in 02-CapabilityModel.md. Runtime mapping throughput scales
linearly with object graph size and shows no degradation under the
concurrent load expected from a Blazor Server host serving many
simultaneous users.

## Security

Mapster performs in-memory object mapping only. It does not execute
untrusted input as code, does not perform deserialization of external data
by itself, and introduces no meaningful attack surface beyond the standard
reflection-based library risk profile, which is further reduced when source
generation is enabled. No CVEs affecting the currently maintained version
line are known at the time of this evaluation.

## Developer Experience

Mapster's convention-based default mode means most developers rarely write
explicit mapping code at all; `entity.Adapt<Dto>()` is enough for the large
majority of cases. When custom logic is required, `TypeAdapterConfig`
provides a fluent, discoverable API. The learning curve for a team already
familiar with the codebase (as this team is, given Mapster's incumbent
status) is effectively zero.

## Maintainability

Good, provided mapping configuration stays centralized per module rather
than being defined ad hoc inside handlers. Because the team already has an
established convention for where `TypeAdapterConfig` registrations live
(per DOCUMENT_CONVENTIONS and 05-development namespace rules), this risk is
already mitigated in practice.

## AI Compatibility

Not directly relevant to AI tooling; mapping is an internal implementation
detail with no external contract surface consumed by AI agents or LLM tool
calling (unlike, for example, the OpenAPI contracts evaluated in TE-0021).

## Cloud Neutrality

Fully cross-platform: Windows, Linux, containers, and any .NET 10 target,
including the Desktop/Mobile clients planned under TE-0010 (.NET MAUI).
Mapster introduces no cloud-vendor dependency of any kind.

## Typical Usage

```csharp
public sealed class AssetMappingExtensions
{
    public static void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Asset, AssetDto>()
            .Map(dest => dest.EngineSerialNumber, src => src.Engine.SerialNumber)
            .Map(dest => dest.CurrentOperatingHours, src => src.MeterReadings.Latest().Value);
    }
}

// Application layer handler
public async Task<AssetDto> Handle(GetAssetByIdQuery request, CancellationToken ct)
{
    var asset = await _repository.GetByIdAsync(request.AssetId, ct);
    return asset.Adapt<AssetDto>();
}

// IQueryable projection, translated to SQL
public async Task<List<AssetListItemDto>> Handle(ListAssetsQuery request, CancellationToken ct)
{
    return await _dbContext.Assets
        .Where(a => a.OrganizationId == request.OrganizationId)
        .ProjectToType<AssetListItemDto>()
        .ToListAsync(ct);
}
```

## Comparison with Mapperly

| Aspect | Mapster | Mapperly |
|--------|---------|----------|
| Mapping verification | Runtime (unless source-gen mode is used consistently) | Compile-time, always |
| Reflection at runtime | Optional, avoidable | Never |
| `IQueryable` projection maturity | Very mature | Good, less mature |
| Convention-based zero-config mapping | Yes | Partial (requires partial method declaration) |
| Setup effort for the common case | Very low | Low, but requires declaring a mapper class per surface |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| .NET 10 Compatibility | Excellent |
| Performance | Excellent |
| Compile-Time Safety | Good (Excellent if source generation is enabled) |
| License Stability | Excellent (MIT, no known commercialization plans) |
| Migration Cost | None (incumbent) |

## Relationship with Entity Framework Core

Mapster's `ProjectToType<T>()` composes directly with `IQueryable<T>`
exposed by EF Core repositories (ADR-0006), allowing projection expressions
to be translated into SQL rather than requiring full entity materialization.
This relationship is a significant architectural asset: it keeps read-side
queries efficient without requiring hand-written `Select()` projections for
every DTO.

## Preliminary Conclusion

Mapster continues to meet every original functional and non-functional
requirement and introduces no new risk. Its `IQueryable` projection
maturity and zero-friction convention-based defaults remain a strong fit
for a Clean Architecture / CQRS-oriented modular monolith of this size. It
remains a strong incumbent.

---

# 7. Mapperly Evaluation

## Overview

Mapperly (`Riok.Mapperly`) is a Roslyn incremental source generator that
generates fully compile-time mapping code from partial mapper class or
interface declarations. Unlike Mapster's optional source generation,
Mapperly is source-generation-first by design — there is no reflection-based
fallback path, and no runtime configuration step exists at all. It has
matured substantially since the original TE-0006 evaluation and is now a
realistic enterprise candidate.

## Architectural Role

```text
Application Layer

   [Mapper]
   public partial class AssetMapper
   {
       [MapProperty(nameof(Asset.Engine.SerialNumber), nameof(AssetDto.EngineSerialNumber))]
       public partial AssetDto ToDto(Asset entity);

       public partial IQueryable<AssetListItemDto> ProjectToListItem(IQueryable<Asset> query);
   }

          │  (generated entirely at compile time)
          ▼

   Generated mapping method
   (plain C#, no reflection, no expression tree compilation)
```

## Architectural Strengths

- Mapping code is generated entirely at compile time; unmapped properties,
  type mismatches, and unreachable members produce build-time diagnostics
  (configurable as warnings or errors), not runtime failures. This directly
  satisfies Directory.Build.props's `TreatWarningsAsErrors` policy, turning
  a mapping mistake into a build failure rather than a production defect.
- Zero reflection, zero runtime expression compilation — generated mapping
  methods are ordinary C#, giving performance on par with hand-written code
  and near-zero allocation overhead, frequently outperforming Mapster in
  micro-benchmarks for simple-to-moderate object graphs.
- Fully debuggable: generated code can be inspected in the IDE and stepped
  through exactly like any other source file, which significantly improves
  troubleshooting compared to expression-tree-based mappers.
- MIT licensed, actively maintained, no commercial tier, no known
  commercialization roadmap.
- Very small dependency footprint: an analyzer package used only at build
  time, plus a thin attribute library referenced at compile time; no
  runtime reflection engine is shipped with the application.
- Diagnostics can be tuned per-mapping (e.g. requiring every target
  property to be mapped, or explicitly allowing unmapped properties),
  giving fine-grained control that neither Mapster nor AutoMapper offers at
  the same level of build-time enforcement.

## Architectural Weaknesses

- More explicit than Mapster or AutoMapper: each mapping surface requires a
  declared partial method inside a partial class or interface, which is
  more ceremony for very simple, high-churn DTOs than Mapster's
  `Adapt<T>()` one-liner.
- `IQueryable` projection support exists and functions correctly, but is
  less flexible than Mapster's `ProjectToType<T>()` for deeply nested,
  conditional projections combining multiple navigation properties.
- Smaller community and shorter track record in large enterprise .NET
  systems compared to both Mapster and AutoMapper; fewer StackOverflow
  answers and third-party tutorials exist for edge cases.
- Migrating the entire codebase from Mapster to Mapperly is not a drop-in
  change: every `Adapt<T>()` / `ProjectToType<T>()` call site must be
  converted to a generated mapper method, and every module's DI
  registration must change from Mapster's `TypeAdapterConfig` to a
  Mapperly mapper class reference.

## Operational Characteristics

No runtime configuration step is required; all mapping behavior is fixed at
compile time inside the generated partial methods. This eliminates an
entire category of startup-order and configuration-drift issues that can
occur with globally registered runtime configuration.

## Scalability

Scales very well from a runtime perspective: because mapping is generated
per declared mapper, execution has no per-call configuration lookup cost at
all. From a build perspective, compilation time grows linearly and
predictably with the number of declared mapping surfaces, which remains
acceptable even as the number of modules grows toward the full scope
described in 02-CapabilityModel.md.

## Security

Equivalent to Mapster from a data-handling perspective: in-memory mapping
only, no execution of untrusted input as code. Mapperly's security profile
is arguably even smaller than Mapster's, since no reflection or
expression-tree compilation machinery exists in the shipped application at
all — the generated code is indistinguishable from hand-written C# at
runtime.

## Developer Experience

Requires slightly more upfront ceremony than Mapster (declaring a partial
mapper class/interface per mapping surface), but this cost is offset by
immediate, IDE-visible compiler feedback when a mapping is incomplete or
incorrect. Developers unfamiliar with source generators may need a short
ramp-up period to understand the partial-method pattern, though this is a
one-time cost.

## Maintainability

Excellent for long-term maintainability: breaking changes in mapped types
(a renamed property, a removed field, a changed nullability annotation) are
caught by the compiler rather than discovered in production or in
integration tests. This directly reduces regression risk as the domain
model grows across many modules, and is arguably Mapperly's single
strongest architectural argument relative to Mapster.

## AI Compatibility

Not directly relevant; same reasoning as Mapster — mapping is an internal
concern with no externally consumed contract.

## Cloud Neutrality

Fully cross-platform and Native AOT / trimming friendly by construction,
since it produces plain C# with no reflection at all. This is a meaningful
advantage for the future Desktop/Mobile direction of the platform
(.NET MAUI, TE-0010), where Native AOT and trimming materially reduce
application size and startup time on constrained devices.

## Typical Usage

```csharp
[Mapper]
public partial class AssetMapper
{
    [MapProperty(nameof(Asset.Engine) + "." + nameof(Engine.SerialNumber),
                 nameof(AssetDto.EngineSerialNumber))]
    public partial AssetDto ToDto(Asset entity);

    public partial IQueryable<AssetListItemDto> ProjectToListItem(IQueryable<Asset> query);
}

// Application layer handler
public async Task<AssetDto> Handle(GetAssetByIdQuery request, CancellationToken ct)
{
    var asset = await _repository.GetByIdAsync(request.AssetId, ct);
    return _mapper.ToDto(asset);
}
```

## Comparison with Mapster

| Aspect | Mapperly | Mapster |
|--------|----------|---------|
| Mapping verification | Compile-time, always | Runtime (unless source-gen mode is used consistently) |
| Reflection at runtime | Never | Optional, avoidable |
| `IQueryable` projection maturity | Good, less mature | Very mature |
| Convention-based zero-config mapping | Partial (requires partial method declaration) | Yes |
| AOT / trimming friendliness | Excellent | Good |
| Migration cost from current codebase | Moderate | None (incumbent) |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| .NET 10 Compatibility | Excellent |
| Performance | Excellent |
| Compile-Time Safety | Excellent |
| License Stability | Excellent (MIT) |
| Migration Cost | Moderate |

## Relationship with Native AOT / .NET MAUI

Mapperly's reflection-free generated code aligns closely with the goals of
Native AOT publishing, which is directly relevant to the Desktop and Mobile
clients approved under ADR-0013 / TE-0010. Should the Desktop/Mobile
workstream later adopt Native AOT trimming as a hard requirement to reduce
application size or improve cold-start time on mobile devices, Mapperly
would remove a source of trimming warnings that Mapster's
reflection-fallback path can introduce.

## Preliminary Conclusion

Mapperly is a strong, more compile-time-safe alternative, particularly
attractive for the future Desktop/Mobile and Native AOT direction of the
platform. It is not yet clearly superior enough today to justify the
migration cost of replacing an incumbent that already satisfies every
functional requirement, but it is the strongest candidate to revisit if
Native AOT becomes a hard requirement.

---

# 8. AutoMapper Evaluation

## Overview

AutoMapper is the most widely known convention-based object mapper in the
.NET ecosystem, originally evaluated and rejected during TE-0006 on
functional and performance grounds. It is re-evaluated here for
completeness and because a significant licensing event has occurred since
the original evaluation.

## Material Change Since TE-0006: Licensing

Since the original TE-0006 evaluation, AutoMapper's licensing model has
changed materially. As of July 2, 2025, AutoMapper — together with MediatR,
already approved for this project under ADR-0011 — transitioned to a
commercial model under a new maintaining company, Lucky Penny Software,
founded by AutoMapper's original creator. New releases are distributed
under the Reciprocal Public License 1.5 (RPL 1.5), a copyleft license,
rather than the original permissive MIT license. A free community tier
exists, but organizations above a defined revenue threshold are expected to
purchase a commercial license to use current releases and receive ongoing
updates. Previously published MIT-licensed versions remain usable under
their original license in perpetuity, but do not receive new features,
bug fixes, or security patches going forward. The project has also
graduated out of the .NET Foundation as a direct consequence of this
licensing change, since RPL 1.5 no longer meets the Foundation's permissive
open source membership criteria.

This directly conflicts with **ADR-0002 — Open Source First Policy**, which
requires open-source libraries unless an approved ADR explicitly documents
an exception.

## Architectural Role

```text
Application Layer

   CreateAssetCommandHandler
          │
          ▼
   _mapper.Map<AssetDto>(entity)      (IMapper, reflection-based)
          │
          ▼
   Presentation Layer (DTO)
```

## Architectural Strengths

- Very large community, extensive documentation, and the largest body of
  third-party tutorials and StackOverflow answers of any candidate
  evaluated here.
- Mature profile-based configuration model (`Profile` classes), familiar to
  a large share of the .NET developer population.
- Strong `IQueryable` projection support via `ProjectTo<T>()`, comparable in
  maturity to Mapster's `ProjectToType<T>()`.
- Built-in configuration validation (`AssertConfigurationIsValid()`) can
  catch a subset of mapping mistakes at application startup, though not at
  compile time.

## Architectural Weaknesses

- Reflection and runtime expression-tree compilation based; consistently
  slower and more allocation-heavy than Mapster or Mapperly in published
  benchmarks.
- Runtime mapping validation only; misconfiguration surfaces at application
  startup at best, or at the specific call site at worst — never at
  compile time.
- **Licensing risk**: adopting current AutoMapper releases would require
  either a formal exception to ADR-0002 or an ongoing commercial license
  cost, and introduces long-term dependency on a single vendor's future
  pricing decisions.
- Pinning to the last MIT-licensed version avoids licensing cost but
  forfeits all future updates, security fixes, and .NET 10-era
  improvements — effectively adopting an unmaintained dependency by policy,
  which is itself a long-term architectural risk.
- Configuration complexity tends to grow non-linearly in large codebases
  with many `Profile` classes and custom `IValueResolver`/`ITypeConverter`
  implementations, a pattern this project would be exposed to given its
  module count.

## Operational Characteristics

Requires a registered `IMapper` singleton built from one or more `Profile`
classes at startup. Configuration validation, if enabled, adds a one-time
startup cost proportional to the number of registered mappings.

## Scalability

Runtime mapping throughput is lower than Mapster or Mapperly under
equivalent load due to reflection and expression-tree evaluation overhead.
This gap widens on larger object graphs, which is a realistic concern for
Asset aggregates that include Engine, Attachments, and multiple Meter
readings in a single projection.

## Security

No specific vulnerabilities are known in currently maintained releases, but
committing to a frozen legacy MIT version to avoid licensing cost means
forfeiting all future security patches — an unacceptable long-term posture
for an enterprise platform with a multi-year roadmap.

## Developer Experience

Familiar to most .NET developers, which lowers onboarding cost for new team
members. However, this advantage is undermined by the same licensing
uncertainty described above: any new developer researching AutoMapper today
will immediately encounter warnings about its commercial transition.

## Maintainability

Poor from a governance standpoint under current project policy: either the
project pays for a commercial license (an explicit ADR-0002 exception,
requiring separate architectural approval), or it freezes on an
unmaintained legacy version, which is itself a long-term maintainability
risk equivalent to carrying an abandoned dependency.

## AI Compatibility

Not applicable; equivalent to the other candidates.

## Cloud Neutrality

Cross-platform, same as other candidates; not a differentiator on its own.
The licensing model, however, introduces a form of organizational
"vendor lock-in risk" that is architecturally analogous to a cloud vendor
dependency, even though the runtime itself is platform-neutral.

## Typical Usage

```csharp
// Legacy MIT-licensed usage pattern (not recommended for adoption)
public class AssetProfile : Profile
{
    public AssetProfile()
    {
        CreateMap<Asset, AssetDto>()
            .ForMember(dest => dest.EngineSerialNumber,
                       opt => opt.MapFrom(src => src.Engine.SerialNumber));
    }
}

public async Task<AssetDto> Handle(GetAssetByIdQuery request, CancellationToken ct)
{
    var asset = await _repository.GetByIdAsync(request.AssetId, ct);
    return _mapper.Map<AssetDto>(asset);
}
```

## Comparison with Mapster / Mapperly

| Aspect | AutoMapper | Mapster | Mapperly |
|--------|------------|---------|----------|
| License (current releases) | RPL 1.5 / Commercial | MIT | MIT |
| Performance | Good | Excellent | Excellent |
| Compile-time safety | No | Partial | Yes |
| Open Source First compliance | No (current releases) | Yes | Yes |
| Configuration validation timing | Startup (opt-in) | Startup (opt-in) | Compile-time |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good |
| .NET 10 Compatibility | Good |
| Performance | Good |
| Compile-Time Safety | Poor |
| License Stability | Poor (current releases) |
| Migration Cost | N/A — rejected |

## Relationship with ADR-0011 (MediatR)

AutoMapper and MediatR were commercialized together under the same
corporate entity and the same RPL 1.5 licensing model. Because MediatR is
already an approved dependency of this project (ADR-0011), the governance
implications identified here extend beyond object mapping. This is flagged
separately from this evaluation's scope as a distinct governance item,
since altering the status of ADR-0011 is outside the boundaries of a TE
focused on object mapping.

## Preliminary Conclusion

AutoMapper is rejected for the same functional reasons identified in
TE-0006 — lower performance and weaker compile-time safety than the
incumbent — now reinforced by a disqualifying licensing change that
directly conflicts with ADR-0002. Adopting current AutoMapper releases
would require an explicit, separately justified exception to the Open
Source First Policy. No such exception is proposed by this evaluation.

---

# 9. Manual Mapping Evaluation

## Overview

Manual mapping means hand-written mapping code, typically implemented as
static extension methods or dedicated mapper classes, with no third-party
mapping library involved at all.

## Architectural Role

```text
Application Layer

   CreateAssetCommandHandler
          │
          ▼
   AssetMappingExtensions.ToDto(entity)   (plain static method, hand-written)
          │
          ▼
   Presentation Layer (DTO)
```

## Architectural Strengths

- Maximum control and transparency; no "magic," fully debuggable, and
  immediately understandable to any C# developer regardless of familiarity
  with a specific mapping library.
- No third-party dependency, and therefore no licensing risk whatsoever —
  the strongest possible position with respect to ADR-0002.
- Compile-time safety by construction: the compiler enforces every
  property assignment exactly as it would for any other line of C# code.
- Zero learning curve for new team members already familiar with plain C#.
- Trivially unit-testable using ordinary unit tests, with no special
  mapping-library test infrastructure required.

## Architectural Weaknesses

- Significant boilerplate for large or frequently changing object graphs,
  particularly for Asset-related DTOs that flatten nested Engine,
  Attachment, and MeterReading data.
- Higher risk of silently forgetting to map a newly added property, since
  there is no generator or convention engine flagging omissions — unless a
  dedicated unit test explicitly asserts every property is populated,
  which itself becomes additional code to write and maintain.
- Higher long-term maintenance cost as the number of DTOs grows across the
  platform's many modules (Assets, Fleet, Preventive Maintenance,
  Corrective Maintenance, Inventory, Procurement, Work Orders, Reporting).
- No built-in `IQueryable` projection support; projections must be written
  manually for every query, duplicating logic already solved generically
  by Mapster and Mapperly, and increasing the risk of inconsistent
  projection logic between similar queries.

## Operational Characteristics

No runtime or build-time infrastructure beyond ordinary C# compilation; no
registration step, no configuration object, no generator.

## Scalability

Scales poorly in developer effort (though not in runtime performance) as
the number of mapped types grows across the platform's many modules. Given
that 02-CapabilityModel.md and 04-modules describe more than a dozen
distinct capability areas, a fully manual-mapping strategy would require
maintaining dozens of hand-written mapper classes with no shared
infrastructure to keep them consistent.

## Security

No additional risk beyond ordinary hand-written application code; the
strongest possible security posture among the four candidates, since there
is no third-party mapping library in the dependency graph at all.

## Developer Experience

Straightforward for small, stable DTOs; becomes noticeably more tedious for
DTOs with many properties or frequent schema changes, where a one-line
`Adapt<T>()` or a generated mapper method would otherwise suffice.

## Maintainability

Acceptable for a small number of simple, stable DTOs; becomes a genuine
maintenance burden at the scale of this platform, which already spans many
bounded contexts (see 03-domain/03-BoundedContexts.md) and is expected to
grow further as Distributed Workspace and Desktop/Mobile capabilities are
implemented.

## AI Compatibility

Not applicable.

## Cloud Neutrality

Fully neutral by definition — there is no library to depend on, and
therefore no deployment constraint introduced by this choice.

## Typical Usage

```csharp
public static class AssetMappingExtensions
{
    public static AssetDto ToDto(this Asset entity)
    {
        return new AssetDto
        {
            Id = entity.Id,
            Name = entity.Name,
            EngineSerialNumber = entity.Engine?.SerialNumber,
            CurrentOperatingHours = entity.MeterReadings.Latest()?.Value ?? 0
        };
    }
}

public async Task<AssetDto> Handle(GetAssetByIdQuery request, CancellationToken ct)
{
    var asset = await _repository.GetByIdAsync(request.AssetId, ct);
    return asset.ToDto();
}
```

## Comparison with Mapster

| Aspect | Manual Mapping | Mapster |
|--------|------------------|---------|
| Compile-time safety | Excellent | Good |
| Boilerplate | High | Low |
| `IQueryable` projection support | None (hand-written) | Excellent, built-in |
| License risk | None | None (MIT) |
| Effort to add a new DTO | High | Very low |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| .NET 10 Compatibility | Excellent |
| Performance | Excellent |
| Compile-Time Safety | Excellent |
| License Stability | Excellent (no dependency) |
| Migration Cost | N/A — used as a targeted complement, not a replacement |

## Relationship with Mapster / Mapperly

Manual mapping is not proposed as a platform-wide replacement for a mapping
library; it is proposed as a formally recognized, narrowly scoped
complement for the subset of mappings that involve substantial business
logic (e.g. computing depreciation, aggregating multi-source cost figures)
where a declarative mapping configuration would be harder to read than
plain, explicit C#.

## Preliminary Conclusion

Manual mapping remains valuable as a targeted escape hatch for a small
number of complex, business-rule-heavy mappings that do not fit
convention-based tooling well. It is not viable as the platform-wide
default given the number of modules and DTOs already planned across the
Capability Model.

---

# 10. Overall Technology Comparison

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|-----------------|------------------------|--------------|---------|
| Default Entity ↔ DTO mapping | Mapster | Mapperly | Primary mapping mechanism for the majority of modules |
| `IQueryable` read-side projection | Mapster (`ProjectToType<T>()`) | AutoMapper `ProjectTo<T>()` (rejected) | Query translation to SQL for list/reporting views |
| Complex, business-rule-heavy mapping | Manual Mapping | — | Explicit, testable, narrowly scoped escape hatch |
| Future Native AOT / Desktop-Mobile mapping | Mapperly (candidate for future adoption) | Mapster (source-gen mode) | Reflection-free mapping for trimmed/AOT builds |

## Capability Comparison

| Capability | Mapster | Mapperly | AutoMapper | Manual Mapping |
|------------|---------|----------|------------|-----------------|
| Open Source (compatible with ADR-0002) | Yes | Yes | No (current releases) | Yes |
| Performance | Excellent | Excellent | Good | Excellent |
| Compile-Time Safety | Good | Excellent | No | Excellent |
| `IQueryable` Projection | Excellent | Good | Excellent | Manual, per-query |
| Convention-based / low ceremony | Excellent | Moderate | Excellent | None |
| AOT / Trimming Friendliness | Good | Excellent | Moderate | Excellent |
| Debuggability | Good | Excellent | Moderate | Excellent |
| Community Maturity | Good | Moderate | Excellent (legacy) | N/A |
| License Stability | Excellent | Excellent | Poor | Excellent |
| Migration Cost from Current State | None | Moderate | N/A (rejected) | High |

## Developer Experience Comparison

| Criterion | Best Choice |
|-----------|-------------|
| Lowest ceremony for simple DTOs | Mapster |
| Strongest compile-time feedback | Mapperly |
| Familiarity for developers new to .NET | AutoMapper (legacy knowledge only) |
| Maximum transparency / no library "magic" | Manual Mapping |

## Cloud Neutrality Assessment

All four candidates are technically platform-neutral and introduce no
cloud-vendor dependency. AutoMapper is the only candidate where a
non-technical form of "lock-in" risk exists, arising from its commercial
licensing model rather than from its runtime behavior.

## Enterprise Suitability

| Criterion | Mapster | Mapperly | AutoMapper | Manual Mapping |
|-----------|---------|----------|------------|-----------------|
| Suitable as platform-wide default | Yes | Conditionally (future) | No | No |
| Suitable as targeted complement | N/A | N/A | No | Yes |
| Long-term governance risk | Low | Low | High | Low |

## AI Compatibility

None of the four candidates has any meaningful interaction with AI tooling,
LLM agents, or tool-calling contracts; this criterion is a non-differentiator
for this particular evaluation, unlike TE-0021 (API Documentation), where it
was material.

## Clean Architecture Compliance

All four candidates can be confined correctly to the Application layer.
Mapster and Mapperly both already enforce this cleanly in practice. Manual
mapping trivially complies by definition. AutoMapper would comply equally
well from a layering perspective; its disqualification is unrelated to
Clean Architecture fit.

## Cost Comparison

| Candidate | Direct Cost |
|-----------|--------------|
| Mapster | None (MIT) |
| Mapperly | None (MIT) |
| AutoMapper | Commercial license fee above free-tier threshold, or forfeiting updates | 
| Manual Mapping | None (developer time only) |

## Risk Assessment

| Risk | Affected Candidate | Severity |
|------|--------------------|----------|
| Vendor commercial licensing exposure | AutoMapper | High |
| Runtime-only mapping error detection | Mapster (non-source-gen path) | Low–Medium |
| Boilerplate growth at scale | Manual Mapping (if over-applied) | Medium |
| Migration cost with limited immediate benefit | Mapperly (if replacing incumbent now) | Medium |
| Smaller ecosystem for edge-case troubleshooting | Mapperly | Low |

## Overall Evaluation

Mapster continues to satisfy every functional and non-functional
requirement without introducing new risk, and remains the only candidate
with zero migration cost. Mapperly is architecturally attractive,
particularly for the platform's future Native AOT / Desktop-Mobile
direction, but does not present a strong enough advantage today to justify
migrating away from an incumbent that already performs well and is already
deeply embedded across the codebase. AutoMapper is disqualified both on the
functional grounds established in TE-0006 and, now more decisively, by its
post-TE-0006 licensing change. Manual mapping remains a valid, narrowly
scoped complement rather than a platform-wide strategy.

---

# 11. Final Recommendation

## Core Technology Stack

| Responsibility | Selected Technology | Rationale |
|-----------------|----------------------|-----------|
| Default object mapping | Mapster | Reaffirmed incumbent; excellent performance, mature `IQueryable` support, MIT license, zero migration cost |
| Complex/business-rule mapping | Manual Mapping | Formally recognized escape hatch for cases that do not fit convention-based mapping |
| Future Native AOT mapping | Mapperly (deferred) | Strongest candidate to revisit if/when Native AOT becomes a hard requirement for Desktop/Mobile clients |
| AutoMapper | Not adopted | Disqualified by RPL 1.5 commercial licensing, conflicts with ADR-0002 |

## Recommended Architecture

```text
Domain Layer  (no mapping dependency)

        │
        ▼

Application Layer
   Mapster.Adapt<T>() / ProjectToType<T>()   — default path
   Manual mapping                            — complex/business-rule path

        │
        ▼

Presentation Layer (DTOs / View Models / API Contracts)
```

## Decision Criteria for Future Mapperly Adoption

This evaluation explicitly defines the trigger condition for revisiting
Mapperly, so that this decision does not need to be re-litigated from
scratch in the future:

```text
IF Native AOT / trimming becomes a hard requirement
   for Desktop or Mobile clients (ADR-0013 / TE-0010)

THEN re-open this evaluation and evaluate a phased
   migration from Mapster to Mapperly for the affected
   client-side mapping surfaces only.
```

## Security Recommendations

Continue monitoring Mapster's release cadence and license status as part of
the standard Dependency Catalog review process (11-DependencyCatalog.md).
No action is required at this time.

## Cloud Neutrality

The recommended stack (Mapster + targeted Manual Mapping) introduces no
cloud-vendor dependency and remains fully deployable across Windows, Linux,
containers, and the Desktop/Mobile targets planned under ADR-0013.

## AI Readiness

Not applicable to this evaluation.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| Mapster (Incumbent) | **Reaffirmed** |
| Manual Mapping (targeted use) | Approved — formalized here as a named, scoped escape hatch |
| Mapperly | Rejected for now — revisit if Native AOT becomes a hard requirement |
| AutoMapper | Rejected — licensing conflict with ADR-0002 |

---

# Decision Summary

- ✔ Clean Architecture preserved
- ✔ .NET 10 Compatibility
- ✔ Open Source First Policy (ADR-0002) compliance
- ✔ No migration cost / no disruption to existing implementation
- ✔ Performance requirements met
- ✔ Long-term license stability
- ✔ Explicit future-review trigger defined (Native AOT) so the decision does
  not need to be re-litigated without a concrete reason

This evaluation **reaffirms ADR-0008 — Use Mapster** without modification.
It does not require a new ADR, since the decision and its rationale are
unchanged; it extends the recorded rationale to explicitly address
Mapperly and the AutoMapper licensing change, and formally documents Manual
Mapping as an approved, scoped complement.

It is recommended that **ADR-0008** be updated with a short addendum noting
that it was reconfirmed via TE-0023 on 2026-07-27, referencing the
AutoMapper licensing disqualification for future readers who might
otherwise ask "why not AutoMapper."

---

# Related ADR

```
ADR-0008 (Reaffirmed — no new ADR required)
```

---

# Related Documents

- TE-0006 — Mapster (original evaluation)
- TE-0010 — Desktop & Mobile Framework Evaluation
- ADR-0002 — Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- ADR-0008 — Use Mapster
- ADR-0011 — Use MediatR (Note: MediatR is affected by the same AutoMapper/
  MediatR commercialization event described above; flagged separately as a
  governance item outside the scope of this evaluation)
- ADR-0013 — Client Application Architecture
- Dependency Catalog

---

# References

https://github.com/MapsterMapper/Mapster

https://mapperly.riok.app/

https://github.com/riok/mapperly

https://automapper.io

https://github.com/LuckyPennySoftware/AutoMapper

https://dotnetfoundation.org/news-events/detail/automapper-graduates-from-the-.net-foundation

---

# Revision History

| Version | Date       | Author             | Description                                    |
|---------|------------|--------------------|------------------------------------------------|
| 1.0.0   | 2026-07-27 | Solution Architect | Initial re-evaluation, rewritten to full project-standard depth; reaffirms ADR-0008 (Mapster), adds Mapperly, updates AutoMapper rejection rationale for licensing change, formalizes Manual Mapping as a scoped complement |
| 1.0.1   | 2026-07-28 | File name Changed from (Object Mapping Technology Evaluation)       |
| 1.3.0   | 2026-07-28 | New section added (Evaluation Scope)                                |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0      |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes |