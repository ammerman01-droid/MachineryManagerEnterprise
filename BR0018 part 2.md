Authorization Flow
Request → Authentication → Resolve User → Resolve Organization → Resolve Roles → Resolve Permissions → Evaluate Policy → Execute Handler


### Authorization Failure Rules
- Business state remains unchanged
- NO Domain Event published
- Attempt logged

### Audit Requirements
Every authorization-sensitive operation records: User, Time, Operation, Resource, Result, Source. Audit records are immutable.

### Assignment Revocation (Phase 3, chat 2026-08-23)
A UserProfileAssignment is REVOKED, never deleted. Revocation sets
IsRevoked=true and RevokedAt (soft revocation), preserving the
immutable audit trail. IPermissionEvaluator excludes revoked
assignments from both HasPermissionAsync and GetAuthorizedScopesAsync
immediately (no caching), satisfying BR-017's "Access revocation on
reassignment" rule (Section 10.16).

### Profile Deletion Cascade (chat, 2026-08-25)
  Deleting a Profile is blocked while it has any active (non-revoked)
  UserProfileAssignment. Once deletion is allowed, all remaining
  (necessarily revoked) assignment records for that Profile are
  permanently removed as well — both explicitly in
  DeleteProfileCommandHandler and via a database-level
  ON DELETE CASCADE foreign key (UserProfileAssignment.ProfileId →
  Profile.Id), so no orphaned assignment rows can remain.

---



# 6. DEVELOPMENT RULES & STANDARDS

## 6.1 Solution Structure

### Central Build Configuration
- `Directory.Build.props` — common MSBuild properties (TargetFramework, etc.)
- `Directory.Packages.props` — Central Package Management (CPM); all NuGet versions managed centrally
- Project files must NOT duplicate shared MSBuild properties or package versions
- Project files contain only: SDK selection, OutputType, UserSecretsId, Razor settings, ProjectReference, PackageReference

### Repository Root
├── docs/
├── src/
│   ├── BuildingBlocks/
│   │   ├── MachineryManager.SharedKernel
│   │   ├── MachineryManager.SharedKernel.Contracts
│   │   ├── MachineryManager.SharedKernel.Abstractions
│   │   ├── MachineryManager.SharedKernel.Infrastructure
│   │   └── MachineryManager.UI
│   ├── Modules/
│   │   └── [ModuleName]/
│   │       ├── [ModuleName].Domain
│   │       ├── [ModuleName].Application
│   │       ├── [ModuleName].Infrastructure
│   │       └── [ModuleName].Presentation
│   └── Host/
│       ├── MachineryManager.Server
│       └── MachineryManager.Client
├── tests/
├── tools/
├── global.json
├── Directory.Build.props
├── Directory.Packages.props
├── MachineryManagerEnterprise.slnx
└── README.md


### Dependency Direction (Inward)
Presentation (Web) → Application → Domain → SharedKernel
Infrastructure supports higher layers but NEVER the center


### Layer Responsibilities
- **SharedKernel:** Reusable components shared by all modules. NEVER contains business logic. NEVER references any business module.
- **Domain:** Entities, Value Objects, Aggregates, Domain Services, Domain Events, Business Rules. ZERO infrastructure code.
- **Application:** Use Cases, Commands, Queries, Validators, DTOs, Interfaces, Mapping. Business workflows. Depends only on abstractions.
- **Infrastructure:** EF Core, Repositories, External Services, File Storage, Logging, Caching. Implements abstractions from higher layers.
- **Presentation:** Blazor UI, Components, Pages, View Models. NO business rules, NO persistence logic.

### Module Rules
- Each module = independent bounded context with Clean Architecture internally
- Modules communicate only through contracts and application boundaries
- Future extraction to microservices requires NO architectural restructuring

## 6.1a Checklist — Adding a New Blazor-Enabled Module
Every module that adds Blazor pages must register its assembly in
**both** of the following places, or its pages will render once
during prerender and immediately flip to Not Found once the
interactive circuit connects (the interactive Router uses a fully
separate assembly list from the one used during prerendering):
1. `Program.cs` → `MapRazorComponents<App>().AddAdditionalAssemblies(...)`
2. `Routes.razor` → `<Router AdditionalAssemblies="...">`

## 6.2 Project Internal Structure

### Standard Folders (create only when needed)
Project/
├── Abstractions/
├── Configuration/
│   ├── DependencyInjection/
│   ├── OptionsConfiguration/
│   └── MiddlewareConfiguration/
├── Constants/
├── Contracts/
├── Exceptions/
├── Extensions/
├── Features/
│   └── [FeatureName]/
│       ├── Commands/
│       ├── Queries/
│       ├── DTOs/
│       ├── Validators/
│       ├── Mappings/
│       └── Services/
├── Interfaces/
├── Mapping/
├── Models/
├── Options/
├── Services/
├── Utilities/
└── Validation/


### Folder Creation Policy
- NEVER create empty folders
- Only create when: contains meaningful content, multiple files justify existence, improves organization

## 6.3 Namespace Conventions

### Root Namespace
`MachineryManagerEnterprise`

### Pattern
MachineryManagerEnterprise.[Layer/Module].[Feature].[Subcategory]


### Examples
- `MachineryManagerEnterprise.Application.Features.Inventory.Commands`
- `MachineryManagerEnterprise.Domain.Inventory`
- `MachineryManagerEnterprise.Infrastructure.Persistence`
- `MachineryManagerEnterprise.Web.Components`
- `MachineryManagerEnterprise.Application.Tests.Features.Inventory`

### Rules
- Namespace = physical folder (one-to-one)
- PascalCase, no spaces, no version numbers, no implementation tech leaks
- Max recommended depth: 5 segments (e.g., `.Application.Features.Inventory.Commands`)
- AVOID: `Domain.Entities`, `Domain.Models`, `Domain.Classes`
- New bounded contexts = new namespace roots (e.g., `MachineryManagerEnterprise.Inventory`)

## 6.4 Dependency Rules (Strict)

| Project | May Reference |
|---------|--------------|
| SharedKernel | — (lowest layer) |
| Domain | SharedKernel |
| Application | Domain, SharedKernel |
| Infrastructure | Application, Domain, SharedKernel |
| Web (Presentation) | Application |

### Forbidden
- Domain → Infrastructure, Presentation, UI Frameworks, EF Core, Logging, External Services
- Application → Infrastructure implementations, UI Components, Database Providers
- Presentation → Business Rules, Persistence Logic, Repository Implementations
- Circular dependencies of ANY kind

### DI Rule
- Application defines interfaces
- Infrastructure provides implementations
- Presentation consumes abstractions
- Runtime wiring = DI / Configuration / Composition Root (never direct `new` in business logic)

## 6.5 Coding Standards

### File Organization (Recommended Order)
1. using directives
2. namespace
3. class declaration
4. constants
5. fields
6. constructors
7. public properties
8. public methods
9. private methods

### Method Rules
- One logical task, descriptive name, minimize nesting, return early
- Max ~40 lines unless justified

### Comments
- Explain WHY, not WHAT
- NEVER leave dead code commented

### Magic Numbers
- PROHIBITED. Use named constants.

### Null Handling
- Nullable Reference Types enabled
- Handle null explicitly
- AVOID null-forgiving operator (`!`) unless absolutely necessary

### Async Programming
- Prefer async APIs
- AVOID `.Result`, `.Wait()`, blocking threads
- Use `CancellationToken` where applicable

### DI
- NEVER instantiate infrastructure directly
- Correct: constructor injection of interfaces

### Performance
- Optimize ONLY when evidence exists
- Readability is default priority

### Static Analysis
- Warnings treated as defects
- Roslyn Analyzers + .NET SDK Analyzers
- New warnings NOT introduced

## 6.5a EF Core 10 — Mapping a Collection of a Custom Value Object
Never map a public property whose type is `List<TValueObject>` (e.g.
`List<EngineModelId>`) directly to its backing field as an EF Core
primitive collection. EF Core 10's query-shaping compiler throws a
`NullReferenceException` in this exact configuration (first observed
with `Profile.Permissions`, reproduced again with
`AssetModel.CompatibleEngineModelIds`). Required pattern:

1. The backing field must be a plain, EF-native collection type
   (`List<Guid>`), never a collection of a custom value object.
2. The public property computes the value-object view on read (e.g.
   `_field.Select(EngineModelId.From)`), not a converted wrapper.
3. In `IEntityTypeConfiguration`: call `builder.Ignore(x => x.PublicProperty)`,
   then map the backing field independently as a shadow property:
   `builder.Property<List<Guid>>("_backingField")` — never configure
   it via a lambda expression pointing at the public property.
4. In repositories, for any Search/List query that projects into a
   different DTO type, always call `ToListAsync()` first to fully
   materialize the entities, then `.Select(...)` into the DTO in
   memory. Never project a computed collection property directly
   inside the LINQ query's `.Select()`.

## 6.6 Naming Conventions

| Item | Convention |
|------|-----------|
| Namespace, Class, Record, Struct, Enum | PascalCase |
| Interface | PascalCase with `I` prefix |
| Method, Property | PascalCase |
| Local Variable, Parameter | camelCase |
| Private Field | `_camelCase` |
| Constant, Enum Member | PascalCase |

### Specific Suffixes
- Async methods: `Async` suffix (e.g., `LoadMachinesAsync()`)
- DTOs: `Dto` suffix (e.g., `MachineDto`)
- Commands: `Command` suffix (e.g., `CreateMachineCommand`)
- Queries: `Query` suffix (e.g., `GetMachineQuery`)
- Validators: `Validator` suffix (e.g., `CreateMachineValidator`)
- Exceptions: `Exception` suffix (e.g., `MachineNotFoundException`)
- Event handlers: `On` or `Handle` prefix (e.g., `OnMachineCreated`, `HandleUserDeleted`)
- Boolean methods: question form (e.g., `IsActive()`, `HasPermission()`, `CanDelete()`)
- Collections: plural names (e.g., `machines`, `users`)
- Boolean variables: `is`, `has`, `can`, `should` prefix (e.g., `isActive`, `hasPermission`)

### Forbidden Names
`Helper`, `Util`, `Misc`, `CommonStuff`, `Manager`, `Data`, `Info`

### File Names
Match the public type exactly (e.g., `MachineService.cs`, `CreateMachineCommand.cs`)

### Database Tables
Entity names remain SINGULAR (EF handles pluralization)

## 6.7 Error Handling Strategy

### Error Categories
| Category | Handling |
|----------|--------|
| **Validation Errors** | NEVER throw exceptions. Return to caller with clear messages. FluentValidation primary tool. |
| **Business Errors** | Domain-specific exceptions where appropriate. NOT system failures. |
| **Infrastructure Errors** | Never expose implementation details to higher layers. |
| **Unexpected Errors** | Logged with full diagnostics. Indicate defects. |

### Exception Rules
- Used ONLY for exceptional situations
- NEVER for: validation, normal control flow, expected business outcomes
- Preserve inner exceptions when wrapping
- NEVER swallow silently

### User Messages
- Friendly messages to users
- Internal details NEVER exposed (e.g., never show `SqlException` to user)

### Global Exception Handler
- Centralized: Logging, CorrelationId, User-friendly response, Consistent formatting

### Retry Policy
- Only for transient failures (HTTP timeout, temporary network)
- NEVER applied blindly

### Fail Fast
- Invalid state → fail immediately. Early failure preferred over corrupted state.

## 6.8 Logging Strategy

### Principles
- Structured logging (Serilog)
- Answer: What happened? When? Why?
- NEVER log: passwords, tokens, secrets, connection strings, PII, payment info

### Log Levels
| Level | Usage |
|-------|-------|
| Trace | Detailed execution flow |
| Debug | Development diagnostics |
| Information | Normal business operations, lifecycle events, user actions |
| Warning | Retry performed, missing config, slow response, approaching limits |
| Error | Failed operations (include Exception, CorrelationId, Operation, Context) |
| Critical | App instability (DB unavailable, startup failure, data corruption) |

### Correlation ID
- Every request has CorrelationId
- Appears in every related log entry

### Audit Logging
- Security-sensitive actions: Auth, AuthZ failure, role changes, config changes, user management
- Audit logs NEVER deleted manually
- Immutable

### Log Retention (Suggested)
| Type | Retention |
|------|----------|
| Trace/Debug | Short |
| Information/Warning | Medium |
| Error/Audit | Long |

### Approved Stack
| Responsibility | Technology |
|---------------|-----------|
| Logging Abstraction | Microsoft.Extensions.Logging |
| Structured Provider | Serilog |
| Telemetry Standard | OpenTelemetry |
| Metrics Backend | Prometheus |
| Dashboards | Grafana |
| Distributed Trace Backend | Grafana Tempo |

> Business modules NEVER depend directly on Serilog, OpenTelemetry SDKs, Prometheus, or Grafana Tempo. These remain isolated in Infrastructure layer, consumed through `Microsoft.Extensions.Logging` abstractions.

## 6.9 Testing Strategy

### Testing Pyramid

UI Tests (top, few)
Integration Tests (middle)
Unit Tests (base, most)


### Test Categories
| Category | Purpose |
|----------|---------|
| Unit Tests | Isolated behavior |
| Integration Tests | Collaboration between components (real infra via Testcontainers) |
| Architecture Tests | Verify architectural rules (NetArchTest, ArchUnitNET) |
| UI Tests | User interaction (critical workflows only) |
| Smoke Tests | Deployment health |

### Unit Test Rules
- Deterministic, fast, no external resources, one behavior per test
- Mock ONLY external dependencies (Moq approved)
- NEVER mock: Value Objects, Domain Entities, Pure business logic

### Test Naming
`MethodName_State_ExpectedBehavior`
Example: `CreateMachine_WhenSerialExists_ShouldReturnValidationError`

### AAA Pattern
All tests: Arrange → Act → Assert

### Test Isolation
- NEVER depend on: execution order, shared state, external services, previous test results

### Performance Testing (Separate)
- Governed by ADR-0027
- Tools: k6, NBomber
- Out of scope for regular test suite

### Approved Stack
| Responsibility | Technology |
|---------------|-----------|
| Unit Test Framework | xUnit |
| Mocking | Moq |
| Integration Test Infra | Testcontainers |
| Load/Performance | k6, NBomber |

### Regression Prevention
- Every fixed defect → at least one automated test added

## 6.10 Build Pipeline (CI/CD)

### Approved Stack
| Responsibility | Technology |
|---------------|-----------|
| CI/CD Platform | GitHub Actions |
| Containerization | Docker |
| Local Orchestration | .NET Aspire |

> **Kubernetes is NOT approved.** No ADR authorizes it.

### Branch Strategy
main
└── develop
└── feature/*


### Feature Branch Pipeline
Restore → Build → Static Analysis → Unit Tests → Architecture Tests

### Develop Branch Pipeline
+ Integration Tests → Package Validation → Artifact Generation

### Main Branch Pipeline
+ Full Build → Full Test Suite → Release Artifact → Version Tagging → Deployment Approval

### Build Configuration
- Default: `Release`
- TargetFramework defined centrally in `Directory.Build.props`
- Individual projects must NOT redefine TargetFramework

### Static Analysis
- Execute BEFORE automated tests
- .NET SDK Analyzers + Roslyn Analyzers
- Warnings treated as defects

### Architecture Validation
- Verify: Dependency Rules, Layer Boundaries, Namespace Rules
- Tools: NetArchTest, ArchUnitNET

### Security
- Secrets managed by CI native secret store (pipeline) + HashiCorp Vault / Azure Key Vault (runtime)
- NEVER expose secrets in pipeline

### Versioning
Semantic Versioning: `MAJOR.MINOR.PATCH`

## 6.11 Dependency Catalog Governance

### Open Source First Policy
- Only open-source libraries unless approved ADR exception
- Every dependency requires: Technology Evaluation (TE) → Proof of Concept (optional) → ADR → Approval

### Central Package Management
- Single source of truth: `Directory.Packages.props`
- Project files: `PackageReference` WITHOUT `Version` attributes
- NO package bypasses this process

### Dependency Lifecycle
Need → Technology Evaluation (TE) → Proof of Concept (opt) → ADR → Approved → Directory.Packages.props → Implementation → Maintenance


### Status Definitions
- **Proposed:** Under evaluation
- **Approved:** Official dependency
- **Deprecated:** Planned for removal
- **Rejected:** Not accepted

### Upgrade Policy
- Review release notes → verify compatibility → execute tests → update ADR if behavior changes

### Security Monitoring
- Known vulnerabilities, unsupported versions, license changes, maintenance status
- Critical vulnerabilities → immediate review

### Removal Policy
1. Verify no references remain
2. Remove from implementation
3. Remove from Directory.Packages.props
4. Update catalog
5. Close maintenance task

### Key Approved Packages (Summary)
See Section 3.7 for full tech stack. Key packages:
- Framework: .NET 10, Blazor, MudBlazor, .NET MAUI, MediatR
- Persistence: EF Core 10, Dapper, SQLite, LiteDB
- Validation: FluentValidation
- Mapping: Mapster
- Logging: Serilog, OpenTelemetry
- Messaging: MassTransit + RabbitMQ
- AI: Semantic Kernel, Azure OpenAI SDK, OpenAI SDK, Ollama, Qdrant.Client
- Storage: MinIO / AWSSDK.S3
- Identity: ASP.NET Core Identity, OpenIddict
- Caching: FusionCache, IMemoryCache, StackExchange.Redis
- Scheduling: Quartz.NET, System.Threading.Channels
- Config: Microsoft.Extensions.Configuration/Options, HashiCorp Vault, Azure Key Vault
- API Docs: Scalar.AspNetCore, NSwag
- Testing: xUnit, Moq, Testcontainers, k6, NBomber
- Build: Docker, .NET Aspire, GitHub Actions
- Reporting: QuestPDF
- Security: Microsoft.AspNetCore.DataProtection

### Deprecated (Do NOT use)
- Avalonia UI, FluentAvalonia, CommunityToolkit.Mvvm — superseded by .NET MAUI

---



# 7. ARCHITECTURE DECISION RECORDS (ADR)

## 7.1 ADR Governance
- 37 ADRs total: 36 Approved, 1 Superseded (ADR-0028)
- All ADRs map to a Technology Evaluation (TE)
- No architectural change bypasses ADR process
- Status: Approved = binding, Superseded = do not use, Proposed = pending

## 7.2 Approved ADR Summary

| ADR | Decision | Status | Layer | Key Constraint |
|-----|----------|--------|-------|----------------|
| ADR-0001 | Clean Architecture + Modular Monolith | Approved | All | Domain NEVER references outer layers |
| ADR-0002 | Open Source First Policy | Approved | Governance | Commercial only when no OSS alternative |
| ADR-0003 | .NET 10 SDK | Approved | Runtime | All projects target net10.0 |
| ADR-0004 | Blazor (Server/WebAssembly/Auto) | Approved | Presentation | Blazor ONLY in Presentation |
| ADR-0005 | MudBlazor | Approved | Presentation | MudBlazor ONLY in Presentation |
| ADR-0006 | Entity Framework Core 10 | Approved | Infrastructure | EF Core ONLY in Infrastructure; Domain NEVER references it |
| ADR-0007 | FluentValidation | Approved | Application | FluentValidation ONLY in Application; Domain NEVER references it |
| ADR-0008 | Mapster | Approved | Application | Mapster ONLY in Application; compile-time preferred |
| ADR-0009 | Serilog | Approved | Infrastructure | Serilog ONLY in Infrastructure; use ILogger abstraction |
| ADR-0010 | OpenTelemetry | Approved | Infrastructure | OpenTelemetry ONLY in Infrastructure |
| ADR-0011 | MediatR | Approved | Application | MediatR ONLY in Application; CQRS + Pipeline Behaviors |
| ADR-0012 | Distributed Workspace Architecture | Approved | Platform | 3 workspace levels; offline-first; sync is architectural capability |
| ADR-0013 | .NET MAUI (Desktop + Mobile) | Approved | Client | Supersedes ADR-0028; Workspace Client pattern |
| ADR-0014 | Workspace Data Architecture | Approved | Platform | 3 logical domains: Master, Project, User |
| ADR-0015 | Workspace Synchronization Architecture | Approved | Platform | Bidirectional, incremental, idempotent, transport-independent |
| ADR-0016 | MassTransit + RabbitMQ | Approved | Infrastructure | Messaging abstraction = MassTransit; Broker = RabbitMQ |
| ADR-0017 | Semantic Kernel | Approved | AI | AI orchestration framework; provider selection = ADR-0023 |
| ADR-0018 | MassTransit-based Connector Framework | Approved | Integration | Default: MassTransit connector; Opt-in: Azure Logic Apps |
| ADR-0019 | Hybrid Persistence Strategy | Approved | Infrastructure | EF Core = mandatory for writes; Dapper = opt-in per query for read-only reporting |
| ADR-0020 | S3-Compatible Object Storage (MinIO) | Approved | Infrastructure | Default: MinIO; Portable via AWS SDK; LocalFileStorage for smallest deployments |
| ADR-0021 | Search Strategy | Approved | Infrastructure | Default: SQL Server FTS; Escalation: OpenSearch; Future: Hybrid Search |
| ADR-0022 | Qdrant Vector Database | Approved | AI | Qdrant = semantic retrieval; SQL Server = System of Record; Eventual consistency |
| ADR-0023 | Multi-Provider AI Strategy | Approved | AI | IAIProvider abstraction; Azure OpenAI default; OpenAI secondary; Ollama local |
| ADR-0024 | Enterprise Testing Strategy | Approved | Quality | xUnit v3 + FluentAssertions + NSubstitute + Testcontainers + Playwright |
| ADR-0025 | Build & Deployment Architecture | Approved | DevOps | .NET 10 SDK + Docker + .NET Aspire + GitHub Actions; Azure DevOps = supported alt |
| ADR-0026 | Enterprise Security Strategy | Approved | Security | ASP.NET Core Data Protection + AES-256 + X.509 + OWASP |
| ADR-0027 | Enterprise Performance Testing | Approved | Quality | BenchmarkDotNet (algorithms) + NBomber (workloads) + k6 (APIs) |
| ADR-0028 | Avalonia UI | **SUPERSEDED** | — | Replaced by ADR-0013 (.NET MAUI). DO NOT USE. |
| ADR-0029 | Enterprise Reporting Architecture | Approved | Infrastructure | QuestPDF (PDF) + ClosedXML (Excel); FastReport & RDLC EXCLUDED |
| ADR-0030 | Identity & Access Management | Approved | Platform | ASP.NET Core Identity + OpenIddict + JWT; Identity = platform module (NOT business BC) |
| ADR-0031 | Caching Architecture | Approved | Infrastructure | FusionCache L1 (IMemoryCache) + L2 (Redis); stampede protection |
| ADR-0032 | Background Processing & Job Scheduling | Approved | Infrastructure | Quartz.NET (persistent jobs) + System.Threading.Channels (in-memory queues) |
| ADR-0033 | Enterprise Observability Architecture | Approved | Infrastructure | Serilog + OpenTelemetry + Prometheus + Grafana + Grafana Tempo |
| ADR-0034 | Configuration & Secrets Management | Approved | Infrastructure | Microsoft.Extensions.Configuration/Options + HashiCorp Vault (primary) + Azure Key Vault (alt) |
| ADR-0035 | API Documentation & Client Generation | Approved | Presentation | OpenAPI 3.x + Scalar (primary UI) + NSwag (client gen) + Kiota (future) |
| ADR-0036 | Validation Pipeline Architecture | Approved | Application | FluentValidation + MediatR Pipeline Behavior; automatic before handler |
| ADR-0037 | Database Migration Strategy | Approved | Infrastructure | EF Core Migrations = SOLE schema owner; Dapper = read-only, never DDL |

## 7.3 Key ADR Compliance Rules

### ADR-0001 (Clean Architecture)
1. Domain never references Infrastructure, Presentation, or external frameworks
2. Infrastructure implements Application abstractions
3. Business rules exist ONLY inside Domain
4. DTOs never exist inside Domain
5. EF Core, Serilog, OpenTelemetry ONLY inside Infrastructure

### ADR-0012 (Distributed Workspace)
- 3 workspace levels: Enterprise → Project → User
- Business executes locally; sync propagates validated changes only
- Sync is bidirectional, incremental, resumable, idempotent
- Conflict resolution follows business semantics (NOT timestamp-based)
- Database replication PROHIBITED
- Working Set = minimal operational data per user

### ADR-0019 (Hybrid Persistence)
1. EF Core is the DEFAULT for ALL reads and writes
2. Dapper is OPT-IN per specific query, not per module
3. Dapper repositories are READ-ONLY (SELECT only; NO INSERT/UPDATE/DELETE)
4. Schema ownership = EF Core Migrations exclusively
5. Every Dapper file must document performance justification

### ADR-0022 (AI Knowledge Retrieval)
- SQL Server = System of Record (business entities, documents)
- Qdrant = Vector storage (embeddings, ANN indexes)
- Embeddings are derived artifacts, NOT authoritative
- Event-driven sync: DocumentCreated/Updated/Deleted → regenerate embedding
- Retrieval flow: Qdrant similarity search → SQL Server document retrieval → LLM response

### ADR-0023 (AI Provider Strategy)
- Application depends ONLY on IAIProvider abstraction
- Azure OpenAI = default production
- OpenAI = optional secondary
- Ollama = local/offline inference
- Provider selection = configuration-driven (no code change)
- Prompts must be provider-neutral

### ADR-0030 (Identity)
- Identity is a PLATFORM module, NOT a DDD Bounded Context
- ASP.NET Core Identity (local) + OpenIddict (auth server) + JWT (tokens)
- Optional external providers: Microsoft Entra ID, Google, GitHub
- Identity NEVER owns Organization data

### ADR-0036 (Validation Pipeline)
- Every Command/Query with input MUST have FluentValidation validator
- Validation executes via MediatR Pipeline Behavior BEFORE handler
- Handlers shall NOT perform ad hoc input validation
- Business invariants = Domain Model responsibility

### ADR-0037 (DB Migrations)
- ONLY EF Core Migrations alter schema
- Dapper queries NEVER contain DDL
- Migrations run as explicit, environment-guarded deployment step

---



# 8. API CONVENTIONS

## 8.1 API Principles
- REST, stateless, resource-oriented, explicit contracts
- API exposes business capabilities, NOT database tables or internal architecture
- Technology independent: consumers never know DB schema, ORM, or implementation language
- Decision hierarchy: Business Rules > Domain Principles > Architecture > API Principles > REST Conventions > Implementation Preferences
- API Layer translates HTTP → Commands/Queries; NEVER implements business rules, NEVER accesses persistence directly, NEVER exposes domain objects

## 8.2 Base URL & Versioning
- Base: `/api/v{major}`
- URL-based versioning only (e.g., `/api/v1/assets`)
- Only major version in URL
- Breaking changes = new major version; non-breaking changes within current version
- Version lifecycle: New → Preview → Supported → Deprecated → Sunset → Retired
- Clients must explicitly request version; server never silently redirects
- Each version has independent OpenAPI spec
- `/api/v1/asset-models` — CRUD + search (search requires `holdingId` query parameter) + Engine-compatibility management (`/compatible-engine-models` sub-route)
- `/api/v1/engine-models` — CRUD + search (search requires `holdingId` query parameter)
- `/api/v1/unit-categories` — CRUD + list (Organization-scoped)
- `/api/v1/units-of-measurement` — CRUD + list (Organization-scoped, each unit references a UnitCategory in the same Organization)
- `/api/v1/work-calendars` — Project-scoped WorkCalendar management: CRUD on WorkPatterns, DayOverrides, and day-resolution queries. All endpoints require `projectId` as query parameter or route segment.

## 8.3 URI Design Rules
- Plural nouns, lowercase, hyphen (`-`) separator
- Resource identifiers: `GET /assets/{assetId}`
- Max nesting depth: **2** (e.g., `/assets/{id}/engines`)
- Business actions (non-CRUD): `POST /assets/{id}/retire`, `POST /engines/{id}/install`
- Bulk operations explicit: `POST /assets/bulk-import`
- Search via query params: `GET /assets?serialNumber=...` (avoid `/search` endpoints unless complex)
- NEVER: `/GetAssets`, `/AssetList`, `/CreateAsset`


## 8.4 HTTP Methods

| Method | Purpose | Idempotent |
|--------|---------|------------|
| GET | Read | Yes |
| POST | Create / Business Action | No |
| PUT | Full Replace | Yes |
| PATCH | Partial Update | Usually |
| DELETE | Remove / Retire | Yes |

### CQRS Mapping
| HTTP | CQRS |
|------|------|
| GET | Query |
| POST/PUT/PATCH/DELETE | Command |

## 8.5 Request / Response Model

### Request Rules
- Contains only execution-required info
- NEVER: internal identifiers, DB metadata, audit info, server-generated values

### Response Rules
- DTOs only; Domain Entities NEVER serialized directly
- Empty collections = `[]` (never `null`)
- Dates: ISO-8601, UTC default
- Enums: string values (never numeric)
- Booleans: clear meaning (`isActive`, `hasPermission`)
- Direct resource response; envelope only for: pagination, metadata, async ops, errors

### Collection Response Structure
```json
{
  "items": [],
  "page": 1,
  "pageSize": 25,
  "totalItems": 1543,
  "totalPages": 62,
  "hasNextPage": true,
  "hasPreviousPage": false
}

8.6 Pagination, Filtering, Sorting
Table
Parameter	Default	Max
page	1	—
pageSize	25	200
Multiple filters: combined with AND
Date range: from=YYYY-MM-DD&to=YYYY-MM-DD
Sort: sort=field (asc), sort=-field (desc), multi: sort=status,name
Search: search=query
Empty results: 200 OK with empty items array (NOT 404)
8.7 Error Response Structure (CORRECTED, chat 2026-09-06 — supersedes the original VAL/AUTH/RES/BUS/INF/SYS prefix scheme below, which was never implemented; this section now documents what every module's ResultExtensions.ToProblemResult actually produces)

{
  "errorCode": "Holding.NotAuthorized",
  "title": "Not Authorized",
  "message": "You do not have permission to perform this action.",
  "correlationId": "0HNO2FASCBQG4:00000004",
  "details": []
}

Error Code Format
errorCode is always "{Domain}.{Reason}" — the Domain segment matches the owning aggregate/module (e.g. "Holding", "AssetModel", "EngineModel", "FuelType", "AuditLog"), never a numbered prefix like "BUS-014". Each {Domain}Errors static class (e.g. HoldingErrors, AssetModelErrors) owns the exhaustive list of codes for its aggregate.

Error Categories & HTTP Mapping (ErrorType enum — 4 values, no more)
Table
ErrorType	HTTP Status	Title
Validation	400 Bad Request	Validation Error
NotFound	404 Not Found	Resource Not Found
Conflict	409 Conflict	Business Rule Violation
Failure	403 Forbidden	Not Authorized
(anything unmapped — should not occur in practice; a genuinely unhandled exception is caught by ASP.NET's own exception-handling middleware, not by this mapping)	500 Internal Server Error	Unexpected Error

Authentication vs. Authorization — not distinguished
There is no separate 401 Unauthorized category. Both "the caller could not be identified" (ICurrentUserService.UserId is null) and "the caller is identified but lacks the permission" (IPermissionEvaluator.HasPermissionAsync returns false) return the same Error.Failure(...) and the same 403 Forbidden — deliberately simplified relative to the original 8.7 design (chat, 2026-08-30 bug fix + 2026-09-06 documentation correction).

Infrastructure / System errors — not a separate category
The original design's INF (503) and SYS (500) categories, distinct from each other, were never implemented. A database or other infrastructure failure surfaces as an unhandled exception, not as a Result.Failure with a special ErrorType — it is caught by the host's generic exception-handling middleware and returns 500 Internal Server Error with the generic "Unexpected Error" title, indistinguishable from any other unmapped case.

Implementation
Every module's Presentation project has its own ResultExtensions.ToProblemResult(this Result result, HttpContext httpContext) extension (duplicated per module by convention — not yet extracted to BuildingBlocks, an open follow-up, not a defect) implementing exactly the mapping table above. correlationId is always httpContext.TraceIdentifier. details is currently always an empty array in every module (reserved for future field-level validation detail, not yet populated by any handler).

Error Rules (unchanged from original design)
Consistent, deterministic, machine-readable, traceable
NEVER expose internal implementation details (stack traces, SQL, connection info) in message
Every error contains correlationId (matches logs, audit, traces)
8.8 Authentication & Authorization
Authentication
Bearer Token in Authorization: Bearer <token> header
HTTPS only; plain HTTP prohibited in production
Token lifetime configurable; expired tokens rejected
Authorization
RBAC + Claims-based
Flow: User → Identity → Roles → Permissions → Business Operation
Multi-tenant: every request executes within exactly one Organization (tenant) context via OrganizationId
Cross-tenant access prohibited unless explicitly authorized
Endpoint Protection Matrix
Table
Type	Auth	AuthZ
Public	❌	❌
Authenticated	✅	❌
Protected	✅	✅
Administrative	✅	✅ (Admin Permission)
Audit Logging
Log: successful/failed auth, authZ failures, permission changes, user lockout, admin access
NEVER log: passwords, tokens, secrets, connection strings, PII, payment info
sign-out mechanism
Sign-out is performed via a plain HTML link to /connect/logout (a full server-side navigation), deliberately not a Blazor button routed through the interactive circuit (chat, 2026-08-27).
8.9 OpenAPI Specification
Generation Pipeline
plain
Source Code → XML Comments → OpenAPI Generator → openapi.json → Scalar (primary UI) → NSwag (client gen)
Rules
OpenAPI 3.x, generated automatically from source code
Manual editing of generated docs PROHIBITED
Scalar = primary interactive documentation UI
Swagger UI = backward compatibility ONLY (per ADR-0035)
NEVER expose: Domain Entities, EF Core Entities, Internal Models, Infrastructure Types
Business-oriented tags (Assets, Engines, Maintenance), NOT technical tags
Every endpoint documents: Summary, Description, Parameters, Request Body, Response Model, Error Responses, Auth, Examples
Deprecated endpoints marked with replacement endpoint
Build pipeline validates OpenAPI spec validity
8.10 API Lifecycle
plain
Design → Development → Review → Release → Maintenance → Deprecation → Retirement
No stage skipped
Design approval BEFORE implementation
Release requires: complete docs, passing tests, valid OpenAPI, security review, build success
Breaking changes never in existing version
Deprecation: notice + replacement + planned removal + sunset date
Retirement only after: deprecation period elapsed, consumers notified, replacement exists
8.11 Long-Running & Async Operations
Return 202 Accepted with OperationId, Status, Location header
Clients poll status endpoint for completion
8.12 Content Types
Request/Response: application/json
Future versions may support additional media types
8.13 HATEOAS
Initial API: NOT required
Future versions may introduce hypermedia links if required (backward compatible)
9. RELEASE & DEPLOYMENT
9.1 Versioning Policy
Semantic Versioning: MAJOR.MINOR.PATCH
Application version independent from API version
Git tags for every production release: v{major}.{minor}.{patch}
Build identification: Version + Build Number + Commit SHA + Build Timestamp
Pre-release identifiers: -alpha, -beta, -rc1
Version Change Rules
Table
Change	Version Bump
Breaking API / Domain Contract / Architectural Redesign	Major
New Feature / New Module / New Endpoint	Minor
Bug Fix / Security Fix / Performance / Documentation	Patch
9.2 Release Types
Table
Type	Content	Example
Major	New business capabilities, architectural evolution, breaking API changes	2.0.0
Minor	New features, backward-compatible improvements, new modules	1.4.0
Patch	Bug fixes, security fixes, performance improvements (NO new functionality)	1.4.3
Hotfix	Critical production defects only; minimal unrelated changes	1.4.4
9.3 Release Lifecycle
plain
Planning → Development → Testing → Validation → Release Candidate → Production Release → Maintenance → End of Support
No stage skipped
Incomplete features NEVER released
Every release reproducible from source control
9.4 Release Process
plain
Development → Feature Complete → Code Freeze → Release Candidate → Validation → Approval → Production Deployment → Monitoring
Code Freeze Rules
New features prohibited
Only approved bug fixes, documentation corrections, config adjustments
Version numbers finalized
Release Candidate (RC)
Feature complete, deployable, production-like
Only bug fixes, doc corrections, config adjustments permitted
Example: 2.0.0-rc1
Validation Gates (ALL must pass)
Build verification
Automated testing (Unit + Integration + Functional + Architecture + Regression)
Manual business verification
Documentation review
Security review
Release Approval Required From
Technical Approval
Architecture Approval
Business Approval (when applicable)
Production Deployment Rules
Use approved artifacts ONLY
Use tagged source code
Manual source modifications PROHIBITED
Post-deployment verification: Application startup, API availability, DB connectivity, Background jobs, Critical business workflows
9.5 Deployment Strategy
Deployment Pipeline
plain
Build → Package → Publish Artifact → Deploy → Verify → Monitor
Environments
plain
Development → Testing → Staging → Production
Each environment isolated
Same artifact promoted through environments (never rebuilt)
Configuration Rules
Configuration external to application binaries
NEVER hardcoded: connection strings, API endpoints, storage, cache, logging config
Secrets managed via approved secure storage (HashiCorp Vault / Azure Key Vault)
Secrets NEVER in: Source Control, Build Artifacts, Application Source Code
Database Deployment
ONLY approved migrations (EF Core Migrations per ADR-0037)
Manual production schema changes PROHIBITED
Migration execution version controlled
Backup procedure verified before deployment
Rollback procedure prepared before deployment
Downtime Policy
Minimize service interruption
Strategies: Rolling deployment, Blue-Green deployment, Zero-downtime migration
Selected strategy depends on infrastructure capabilities and service criticality
Rollback Strategy
Every deployment MUST have rollback plan
Rollback includes: previous app version, previous config, DB recovery strategy
Rollback procedures documented, validated, rehearsed BEFORE production deployment
Rollback triggers: critical defects, deployment failure, data integrity threat
Deployment Audit
Every deployment records: Version, Build Number, Git Commit, Deployment Time, Environment, Operator (if manual)
9.6 Support Lifecycle
plain
Released → Supported → Maintenance → Deprecated → End of Support → Archived
Support Matrix
Table
Stage	Bug Fix	Security	Documentation
Supported	✅	✅	✅
Maintenance	✅	✅	✅
Deprecated	❌	✅	✅
End of Support	❌	❌	❌
Archived	❌	❌	❌
Rules
Security updates ONLY for supported versions
Deprecated versions remain functional during support period
End of Support = no further updates; clients must migrate
Archived = historical only, no maintenance
Upgrade Policy
Patch → Patch: no changes required
Minor → Minor: backward compatible
Major → Major: may require migration
9.7 Release Checklist (Mandatory Gates)
Table
Gate	Required
Source Control (committed, PRs merged, tag prepared)	✅
Build Success (zero errors, zero critical warnings, static analysis pass)	✅
Tests Passed (Unit + Integration + Functional + Architecture + Regression)	✅
Documentation Updated (API docs, architecture docs, release notes, migration)	✅
Database Ready (migrations reviewed, scripts validated, backup verified)	✅
Security Verified (secrets configured, certificates valid, auth/authZ verified, security review)	✅
Configuration Reviewed (env config, connection strings, endpoints, logging, monitoring)	✅
Deployment Package Prepared	✅
Rollback Prepared (previous release available, instructions reviewed, backup completed)	✅
Approval Granted (Technical + Architecture + Business)	✅
Health Check Passed (post-deployment)	✅
9.8 Release Notes Template (Required Sections)
Release Information (Version, Date, Type, Git Tag, Build Number)
Summary
New Features
Improvements
Bug Fixes
Breaking Changes (or "None")
Database Changes (or "None")
API Changes (or "None")
Security Updates (or "None")
Migration Notes (or "No migration steps required")
Known Issues (or "None")
Upgrade Recommendation
Support Information
References (Checklist, Deployment Report, Build Number, Git Tag)
10. BUSINESS SPECIFICATIONS (BR)
10.1 BR Governance
18 Business Specifications total, all Approved
Each BR defines one business capability with: Purpose, Scope, Business Definitions, Lifecycle, Rules, Constraints, Acceptance Criteria
BRs are authoritative source for business rules; AI must NEVER invent rules outside BRs
10.2 BR-003 — Asset Relationships
Purpose: Define how Assets relate to other business objects (Projects, Organizations, Components, Documents)
Key Rules:
Every Asset has exactly one owning Organization
Asset-Project relationship is temporary and historical
Asset hierarchy (parent/child) permitted; cycles prohibited
Asset relationships never transfer Asset identity
Historical relationships preserved; never overwritten
Note (chat, 2026-08-25): AssetModel and EngineModel catalogs are
scoped Per-Holding — shared across every Organization under that,
not shared platform-wide. only Asset identity records are per-Organization.
Each Holding> maintains and manages its own model catalog independently.
10.3 BR-004 — Tracked Components
Purpose: Manage components with independent lifecycle (Engine, Transmission, Tire, Battery, Hydraulic Attachment)
Key Rules:
Component is independent business object, not property of Asset
One Component may serve multiple Assets over time
Component may exist without being installed (warehouse, repair)
Component identity unchanged after rebuild; rebuild = maintenance history
Replacement preserves: Removed, Installed, Date, Technician, Maintenance Operation
Installation position belongs to installation event, not component
Current position = derived from latest active installation
10.4 BR-005 — Tire Lifecycle
Purpose: Track tire as a Tracked Component with specific lifecycle rules
Key Rules:
Tire has independent identity and lifecycle
Tire may be installed/removed/transferred/replaced/rebuilt/retired
Tire replacement preserves complete installation history
Tire position (Front Left, Rear Right, etc.) belongs to installation event
Tire lifecycle updated ONLY through Maintenance Operations
10.5 BR-006 — Battery Lifecycle
Purpose: Track battery as a Tracked Component with specific lifecycle rules
Key Rules:
Battery has independent identity and lifecycle
Battery may be installed/removed/transferred/replaced/rebuilt/retired
Battery replacement preserves complete installation history
Battery health/age may generate Condition-Based Forecasts
Battery lifecycle updated ONLY through Maintenance Operations
10.6 BR-007 — Parts Catalog
Purpose: Manage spare parts definitions, specifications, and compatibility
Key Rules:
Part has permanent identity; definitions immutable after approval
Part belongs to exactly one Part Category
Part may have multiple manufacturers and suppliers
Part specifications (dimensions, weight, material) are reference data
Part Catalog does NOT track inventory quantities (that is Inventory Management)
10.7 BR-008 — Part Cross Reference
Purpose: Define equivalent, alternative, and replacement relationships between Parts
Key Rules:
Cross-reference relationships are bidirectional and symmetric
Cross-reference NEVER implies automatic substitution approval
Part A equivalent to Part B = both satisfy same functional requirement
Replacement relationship preserves historical compatibility
Cross-references are reference data; operational substitution requires approval
10.8 BR-009 — Incident Management
Purpose: Record, classify, investigate, and resolve unexpected operational events
Lifecycle: Reported → Validated → Classified → Assigned → Under Investigation → Decision → Resolved → Closed
Alt: Reported/Validated → Rejected; Closed → Reopened → Under Investigation
Forbidden: Skip mandatory states; Closed modified directly
Key Rules:
Incident = any unexpected event affecting Assets, Components, Personnel, Environment
Every Incident has one primary classification (Mechanical, Electrical, Safety, Environmental, Operational, Security)
Severity (business impact) and Priority (response speed) are independent
Investigation produces Corrective Actions; Corrective Actions are business outcomes, NOT maintenance operations
Creating Corrective Action does NOT automatically execute it
Closed Incidents = read-only; reopening creates new lifecycle transition
Investigation history, evidence, root cause preserved permanently
One Incident may generate multiple Corrective Actions (Maintenance, Training, Safety, etc.)
Corrective Action completion does NOT automatically close Incident
10.9 BR-010 — Maintenance Forecast
Purpose: Predict future maintenance needs before operational failures occur
Forecast Types: Preventive (deterministic), Predictive (probabilistic), Condition-Based (threshold), Regulatory, Manufacturer, AI
Lifecycle: Generated → Validated → Approved → Scheduled → Consumed → Completed
Alt: Generated/Validated/Approved/Scheduled → Cancelled
Key Rules:
Forecast is prediction, NOT evidence of current requirement
Forecasts NEVER execute maintenance; they support planning decisions
Only Approved Forecasts may participate in maintenance planning
Multiple Forecasts may exist simultaneously for same object (independent)
Forecast generation NEVER modifies historical operational records
Forecast confidence level included (Very High/High/Medium/Low/Unknown)
Forecast completion preserves original prediction + actual execution for accuracy analysis
Forecasts may be consumed by: Maintenance Planning, Procurement Planning, Shutdown Planning, Fleet Planning
Rejected Forecasts preserve rejection reason
Expired Forecasts remain historical records
10.10 BR-011 — Maintenance Operations
Purpose: Execute approved maintenance work; authoritative source of operational history
Lifecycle: Requested → Planned → Approved → Scheduled → Started → In Progress → Completed → Verified → Closed
Alt: Requested/Planned/Approved/Scheduled → Cancelled
Alt: Started/In Progress → Suspended → In Progress (resumed)
Forbidden: Skip mandatory stages; Completed→InProgress; Closed modified
Key Rules:
Maintenance Operation = controlled execution of ONE approved maintenance activity
Every completed Maintenance Operation has at least one Activity
Activities: Inspection, Cleaning, Adjustment, Calibration, Lubrication, Repair, Replacement, Installation, Removal, etc.
Findings = observed conditions; may generate Recommendations/Forecasts/Additional Work Orders
Measurements preserve historical values; never overwrite previous observations
Component Changes (Install/Remove/Replace/Relocate) owned by Maintenance Operation
Installation records: Component, Asset, Position, Date, Time, Technician, Reason, Maintenance Operation
Replacement = two linked events (Remove Old + Install New), permanently linked
Position belongs to installation event, NOT component
Labor records: Person, Role, Skill, Start/Finish Time, Hours, Cost
Inventory consumption: Item, Quantity, Unit, Warehouse, Cost, linked to Maintenance Operation
External services: Supplier, Type, Invoice, Cost, Duration, Warranty
Downtime: Start/Finish Time, Duration, Category, Planned/Unplanned, Reason
Financial impact: Labor + Parts + External Services + Transportation + Consumables + Miscellaneous
Operational Result: Successfully Repaired, Temporarily Repaired, Replaced, Inspected, Tested, No Fault Found, Deferred
Closed Maintenance Operations are immutable
Component lifecycle updated ONLY through Maintenance Operations (direct modification prohibited)
Maintenance Operation is Aggregate Root; owns: Activities, Findings, Measurements, Labor, Inventory, Downtime, Component Changes
10.11 BR-012 — Notification Center
Purpose: Distribute business events to appropriate recipients through appropriate channels
Notification Types: Informational, Operational, Reminder, Alert, Escalation, Approval, Incident, Forecast, Maintenance, System
Lifecycle: Created → Queued → Delivered → Viewed → Acknowledged → Archived
Alt: Created → Cancelled
Reminder and Escalation cycles create additional history, never modify original
Key Rules:
Notification Center communicates business events; NEVER creates or modifies business events
Every Notification references exactly one originating business event
Same business event may generate multiple Notifications (Technician, Supervisor, Manager)
Recipient Resolution consumes business relationships (never hard-coded, never duplicated)
Delivery Channels: Dashboard, Mobile Push, Email, SMS, Internal Messaging, Voice
Delivery failure NEVER deletes Notification; may trigger Retry or Alternate Channel
Delivery does NOT imply awareness; only Viewed/Acknowledged confirm recipient interaction
Archived Notifications = permanent historical records
Notification lifecycle independent from originating business object lifecycle
Hierarchical propagation: Project User → Project Admin → Enterprise Admin → Super Admin
Escalation appends higher-level recipients, never replaces original
Duplicate prevention governed by business policy
Quiet Hours configurable; Critical may bypass
10.12 BR-013 — Internal Messaging
Purpose: Human-to-human business communication with complete traceability
Conversation Types: Direct, Group, Business Context, Temporary, Permanent, Broadcast
Message Lifecycle: Created → Sent → Delivered → Read → Archived (or Soft Deleted)
Key Rules:
Every Message belongs to exactly one Conversation
Conversation may reference Business Objects (Asset, Work Order, Incident, etc.) for context only
Closing Conversation NEVER closes referenced business object
Messages are immutable historical records; permanent deletion prohibited
Editing creates version history; original preserved
Soft Delete = presentation action only; organizational history preserved
Read status independent per recipient
Attachments belong to Messages; never exist independently
Forwarding creates new Message; original unchanged
Business Context References are read-only navigation
Messages NEVER modify business state
Messages NEVER approve/authorize/execute business operations
Internal Messaging and Notification Center are independent capabilities
Participant resolution consumes Relationship Management (never duplicates hierarchy)
10.13 BR-014 — AI Assistant
Purpose: Provide intelligent advisory assistance across all business domains
Core Principle: Intelligence without Authority. AI observes, reasons, recommends. NEVER approves, rejects, executes, modifies.
AI Capabilities: Knowledge Discovery, Business Q&A, Historical Summarization, Recommendation Generation, Pattern Recognition, Risk Identification, Cross-Capability Analysis, Explanation, Navigation Assistance, Learning Assistance
Key Rules:
AI Assistant is advisory ONLY; business authority always belongs to humans
AI NEVER modifies business state (Assets, Components, Parts, Incidents, Forecasts, Maintenance Operations, Notifications, Messages, Relationships)
AI NEVER approves/rejects/authorizes/executes business operations
AI NEVER creates business events
AI NEVER fabricates business facts; unknown = explicitly stated as unknown
Every recommendation must be explainable with supporting evidence
AI consumes authorization decisions; NEVER defines authorization
AI may recommend notifications but NEVER sends them
AI may participate in conversations when explicitly invoked but NEVER initiates independently
AI may improve forecasting quality but NEVER creates/modifies/approves forecasts
Recommendation language: "Suggested", "Recommended", "Consider", "Possible"
Prohibited language: "Must", "Required", "Approved", "Authorized", "Completed" (unless quoting existing record)
Recommendations may be stored as historical advisory records; never become operational history
Expired Recommendations never influence future decisions
Model improvement affects future recommendations only; never changes historical business truth
Business accountability NEVER assigned to AI
Every approved business action identifies responsible human participant
10.14 BR-015 — Relationship Management
Purpose: Create, manage, validate, and preserve business relationships between entities
Relationship Types: Ownership, Hierarchical, Assignment, Installation, Replacement, Equivalence, Dependency, Reference, Communication, Advisory
Relationship Lifecycle: Draft → Active → Modified → Expired → Historical
Forbidden: Historical → Active/Modified
Key Rules:
Every relationship has its own identity, lifecycle, history, business meaning
Relationships are independent from connected business entities
Relationship ownership NEVER transfers ownership of connected entities
Relationship Management owns relationships only; NEVER owns Assets, Parts, Components, Incidents, Forecasts, Maintenance Operations, Notifications, Conversations
Relationship Management NEVER executes business operations
Relationship propagation occurs ONLY through Domain Events
Historical relationships immutable; never overwritten or deleted
Only Active Relationships participate in authorization, navigation, propagation
Hierarchical relationships: acyclic, single parent per child
Ownership propagation follows hierarchy; never transfers aggregate ownership
Circular hierarchy prohibited
Every relationship validated before activation (structural, ownership, hierarchy, type, authorization, temporal, dependency, consistency)
Rejected relationships remain non-operational
Relationship metadata (Effective Date, Expiration Date, Reason, Status) belongs to relationship only
10.15 BR-016 — Distributed Workspace Synchronization
Purpose: Enable business operations to continue without connectivity while preserving enterprise consistency
Workspace Hierarchy: Enterprise → Project → User (synchronization only between adjacent levels)
Terminology reconciliation (RESOLVED, chat 2026-08-19): "Enterprise" in this hierarchy is the same entity as Organization (BR-017), not Holding. Holding does NOT participate in Workspace Synchronization — Holding is a purely administrative grouping above Organization (see BR-017) and has no sync authority, Working Set, or offline concerns of its own. The synchronization hierarchy remains exactly two hops: Organization ("Enterprise") → Project → User.
Key Rules:
Business execution continues regardless of synchronization availability
Synchronization occurs ONLY after successful business validation
Synchronization is bidirectional: Upstream (User→Project→Enterprise) and Downstream (Enterprise→Project→User)
Direct User→Enterprise synchronization PROHIBITED
Only validated business changes exchanged; incremental (no full history retransmission)
Synchronization timing does NOT alter business behavior
Synchronization Packages: immutable, atomic, traceable, idempotent
Partial package application PROHIBITED
Working Set = minimum info required for user's responsibilities; responsibility-driven, minimal, refreshed after sync
Completed operations may be removed from User Workspace after sync; preserved in higher levels
Conflict Resolution: Business Rules determine outcome; timestamp-based resolution PROHIBITED
Automatic resolution only when deterministic; non-deterministic = manual review
Monotonic values (Hour Meter, Odometer) never decrease
Completed operations never become incomplete
Enterprise = permanent custodian of business history
Project = operational authority for project activities
User = responsible only for personal operational activities
Primary Synchronization Authority per Project; only this authority may sync Project→Enterprise
Anonymous synchronization PROHIBITED
Every sync session generates audit record
10 synchronization scenarios defined (Online, Offline, Package Delivery, Consolidation, Distribution, Long Offline, Simultaneous, Device Replacement, Project Closure, Enterprise Recovery)
10.16 BR-017 — Organization Management (Tenant Hierarchy: Holding → Organization → Project)
Purpose: Define the tenant hierarchy, and the ownership vs. operational-assignment split within it, as the authorization scope boundary
Tenant Hierarchy (RESOLVED, chat 2026-08-19 — replaces the former "sub-organizations" open question):
Holding: optional top-level tenant grouping — a collection of one or more Organizations under common administrative oversight. An Organization MAY exist without belonging to any Holding (standalone tenant).
Organization: business entity (company/operating unit). Distinct from Company = manufacturer brand. Remains THE authorization scope boundary and THE sole owner of Assets, Personnel, and Warehouse Inventory — this did not change.
Project: the operational tier beneath Organization. A Project belongs to exactly one Organization. Projects are where Assets, Personnel, and Warehouse Inventory are currently operationally active — but Projects do NOT own them (ownership stays at Organization level; see "Ownership vs. Current Assignment" below).
Ownership vs. Current Assignment (RESOLVED, chat 2026-08-19):
Every Asset has exactly one owning Organization (unchanged).
Assets, Personnel, and Warehouse Inventory each have (a) a permanent owning Organization, and (b) a CURRENT Project assignment that may change over time.
Reassignment between Projects (of the same Organization) is a normal operational event, not an ownership transfer.
Historical Usage/Maintenance/Activity records remain permanently scoped to whichever Project was current at the time each record was created. This scoping is immutable and is never retroactively updated when the resource is later reassigned to a different Project — consistent with the Historical Entities principle (append-only; see Section 4.2).
The owning Organization has standing access to all historical records across all of its Projects, past and present.
Warehouse facility vs. Inventory (RESOLVED, chat 2026-08-19): a Warehouse (the physical facility) is fixed to a single Project — it does not itself move between Projects. Inventory items stored within a Warehouse are owned by the Organization and MAY be moved between Warehouses, whether within the same Project or across different Projects of the same Organization.
Authorization Scope Rules:
Organization-scoped permissions evaluated within a single resolved Organization.
Project-scoped permissions evaluated within a single resolved Project (RESOLVED, chat 2026-08-19).
Holding-scoped permissions span all Organizations under that Holding (RESOLVED, chat 2026-08-19).
Asset shall not exist without owning Organization.
Authorization checks shall not evaluate across Organization boundaries — the sole exception is a Holding Administrator, who is authorized across all Organizations within their Holding (RESOLVED, chat 2026-08-19).
Access revocation on reassignment (RESOLVED, chat 2026-08-19): when a User's current Project assignment changes, access to the PREVIOUS Project's data is revoked immediately and does not persist. A User promoted to Organization Administrator gains scope across all Projects of that Organization; a User promoted to Holding Administrator gains scope across all Organizations (and their Projects) of that Holding.
Scope resolution (which Organizations/Projects a User currently has access to) is evaluated dynamically against current assignment state at request time — NOT cached or baked into a long-lived token, so that revocation and reassignment take effect immediately (RESOLVED, chat 2026-08-19).
Cross-Organization Asset Transfer (RESOLVED, chat 2026-08-19):
An Asset MAY be transferred from one Organization to a different Organization (e.g. a sale). This is distinct from — and independent of — a Project reassignment within the same Organization.
All historical records (Usage, Maintenance, ownership history, etc.) created BEFORE the transfer date remain permanently visible to BOTH the source (origin) Organization and the destination Organization.
Records created AFTER the transfer date belong exclusively to the destination Organization; the source Organization loses access to these new records.
Re-acquisition case: an Asset MAY later be transferred back to an Organization that previously owned it (e.g. Org A sells to Org B, then later reacquires it from Org B). The transfer mechanism shall be idempotent with respect to previously archived history: re-linking an Asset to a former owning Organization shall NOT duplicate, rewrite, or conflict with that Organization's pre-existing historical records for that Asset — the prior history is already present and is simply reconnected, not recreated.
Each transfer event itself is recorded as an immutable history entry (which Organization → which Organization, when), consistent with the existing "Asset ownership history" rule.
Organization Suspension (RESOLVED, chat 2026-08-19):
When an Organization is suspended, all of its historical records remain intact and are NOT deleted.
If a suspended Organization's Assets are subsequently transferred (sold) to another Organization — whether within the same Holding or not — the Cross-Organization Asset Transfer rule above applies identically: history splits at the transfer date, both Organizations retain access to pre-transfer history, and re-acquisition remains idempotent.
Open questions (not yet decided):
Full Organization lifecycle states beyond Suspension (e.g. permanent closure/dissolution) and what happens to any Assets that are never transferred out of a closed Organization.
10.17 BR-018 — Work Calendar Management
Purpose: Define the working capacity of each Project (shifts, breaks, holidays, and day overrides) as the single authoritative source for asset usage calculation, maintenance planning, and operational reporting.
Scope: Project-scoped. Each Project has exactly one WorkCalendar. No sharing across Projects, Organizations, or Holdings. Future consumers: Asset Usage, Maintenance, Service Scheduling, Forecasting, Reporting.
Business Definitions:
WorkCalendar: The complete calendar configuration for one Project. Created empty when Project is created.
WorkPattern: A weekly recurring pattern with a specific date range [StartDate, EndDate]. Contains exactly 7 DaySchedules (one per DayOfWeek).
DaySchedule: The schedule for one specific day of the week. Contains zero or more Shifts.
Shift: A working time interval within a day (StartTime, EndTime).
Break: A rest interval within a Shift (StartTime, EndTime). Subtracted from gross shift hours to calculate NetWorkingHours.
DayOverride: A one-day exception that overrides the WorkPattern for a specific Date. Types: Holiday (no work), Working (custom shifts on a non-working day), Modified (replacement shifts on a working day).
NetWorkingHours: Gross shift duration minus break duration for a given day.
Lifecycle:
WorkCalendar: Draft → Active → Archived
WorkPattern: Draft → Active → Superseded → Historical
DayOverride: Created → Active → Cancelled → Historical
Key Rules:
BR-018-001: Every Project has exactly one WorkCalendar.
BR-018-002: WorkCalendar is created empty when Project is created; user must configure it.
BR-018-003: WorkPattern date ranges within one WorkCalendar must never overlap; every date falls under exactly one active Pattern.
BR-018-004: Every WorkPattern defines exactly one DaySchedule per DayOfWeek (7 total); each DaySchedule contains zero or more Shifts.
BR-018-005: Each Shift may contain zero or more Breaks; NetWorkingHours = Gross Shift Hours minus Break Hours.
BR-018-006: DayOverride takes absolute precedence over WorkPattern for its specific Date.
BR-018-007: DayOverride types: Holiday (no work), Working (custom shifts on a normally non-working day), Modified (replacement shifts on a working day).
BR-018-008: Changing a WorkPattern or DayOverride triggers recalculation of derived indicators (capacity, utilization denominator, maintenance windows); raw recorded usage data (MeterReading, OperationalUsage) remains immutable.
BR-018-009: WorkCalendar is the sole authoritative source for "available working hours per day" within a Project.
BR-018-010: Editing a WorkPattern creates a new version; previous versions remain accessible. DayOverrides are cancelled, never physically deleted.
BR-018-011: A WorkPattern with a future StartDate may be created in Draft status and activated automatically (or manually) when the date arrives.
BR-018-012: Cancelling a DayOverride returns control to the WorkPattern governing that Date.
Authorization:
WorkCalendar.View — read-only access to WorkCalendar, WorkPatterns, and DayOverrides (Project-scoped, cascades upward per BR-017).
WorkCalendar.Configure — create, update, cancel WorkPatterns and DayOverrides (Project-scoped, cascades upward per BR-017).
API Base: /api/v1/work-calendars — all endpoints require projectId.
11. PROOF OF CONCEPTS
11.1 POC-0001 — Jalali Support for MudBlazor DatePicker
Status: Approved
Objective: Evaluate MudBlazor DatePicker supporting Persian (Jalali) calendar without additional UI library
Hypothesis: MudBlazor remains sole UI framework; Jalali via adapter/conversion layer
Success Criteria: Jalali display correct, RTL support, Persian localization, DateOnly/DateTime conversion, FluentValidation compatibility, keyboard navigation, browser compatibility (Edge/Chrome/Firefox), no noticeable latency
Fallback (if fails): Blazor.PersianDatePicker
Outcome: If PASS → Create ADR → Implement MudBlazor Jalali Adapter; If FAIL → Create ADR → Select Alternative
END OF DOCUMENT
MachineryManagerEnterprise — AI Engineering Reference
Complete: All 11 Sections + BR-018
Source: https://github.com/ammerman01-droid/MachineryManagerEnterprise
Purpose: Single Source of Truth for AI assistants. Contains only final decisions, rules, and structures.
Last Updated: 2026-09-06 (Work Calendar Management — BR-018 added across Sections 1.1, 3.9, 4.5, 4.6, 4.7, 4.8, 4.9, 5.2, 5.3, 5.4, 5.8, 8.2, 10)