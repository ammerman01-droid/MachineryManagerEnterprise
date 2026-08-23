# MachineryManagerEnterprise — AI Engineering Reference
## Generated: 2026-08-17
## Source: https://github.com/ammerman01-droid/MachineryManagerEnterprise
## Purpose: Single Source of Truth for AI assistants. Contains only final decisions, rules, and structures. Analysis/rationale removed.

---

# 1. PROJECT IDENTITY & AI CONTRACT

## 1.1 Project Identity
- **Name:** MachineryManagerEnterprise
- **Type:** Enterprise Asset Lifecycle Management (EALM) / EAM
- **Status:** Phase 3 — Core Platform Modules. Identity & Access Management (ASP.NET Core Identity + OpenIddict Authorization Server/Client, Authorization Code+PKCE and Client Credentials flows, end-to-end verified) is functionally complete. Organization module's initial vertical slice (Organization aggregate, CQRS, EF Core) is complete; the Holding/Project tenant hierarchy (BR-017) has been added at the Domain layer, with Infrastructure (EF configuration, migrations) and the Administration module (Profiles, scoped Role/Permission assignment per Section 5.8) still pending.
- **Branch:** feature/project-bootstrap
- **License:** Private — All Rights Reserved

## 1.2 Core Principles (Frozen)
- Domain Driven Design (DDD)
- Clean Architecture
- Modular Monolith (future microservice-ready)
- Code First
- Documentation First
- Git First
- Open Source First
- Multi-Tenant by Design
- Security by Design
- Maintainability First
- Long-Term Maintainability over short-term speed

## 1.3 AI Engineering Contract — Mandatory Rules

### Authority & Source of Truth
- Authoritative sources: Approved Documentation, Approved ADRs, Existing Repository, Existing Source Code, Explicit User Instructions.
- Architecture is FROZEN. Do not redesign solution structure, project organization, dependency direction, or architectural style unless explicitly requested or required by an approved ADR.
- Repository structure is approved and must be preserved.
- Never assume file/document contents from names or locations. Read before concluding.

### Code Quality (Non-Negotiable)
- Every implementation must finish with: Successful Restore, Successful Build, Zero Build Errors, Zero Build Warnings.
- Generate production-ready code only. No placeholders, TODOs, temporary code, mock patterns, or fake abstractions unless explicitly requested.
- Never generate code expected to fail compilation or produce warnings.
- Target SDK: .NET SDK 10.0.302. Use latest stable compatible library versions.

### Development Workflow
- Business Specification → Domain Model → Application Layer → Infrastructure → Presentation
- Large tasks must use an explicit Progress Ledger: Artifact | Completed | Current | Remaining
- Before every response: identify current artifact, last completed item, current item, remaining items.
- After every response: provide updated Progress Ledger.
- Never duplicate content, restart completed work, or continue from incorrect items.
- Every logical step concludes with a Git commit (atomic, meaningful, traceable).

### Documentation Synchronization
- Implementation and documentation must remain synchronized.
- If implementation changes architecture, APIs, or workflow, explicitly identify every documentation file requiring update (path, name, reason, affected sections).
- Never silently invalidate approved documentation.

### Code Modification Instructions
- For every code block, specify: Repository path, Project name, File name, Object being modified, Modification type.
- Modification types: Replace entire file/class/method/property/section, Insert before/after/inside, Add new file.
- Prefer deterministic insertion points (existing code anchors) over line numbers.
- When modifying large files, state whether entire file or only specific sections are modified.

### Business Rules
- Never invent business rules. If undocumented, identify the gap, suspend implementation, and request clarification.
- Business implementation order: Business Spec → Domain Model → Application → Infrastructure → Presentation.

### Decision Authority
- AI decides independently: implementation details, internal code organization, naming, project file contents, DI registration, build config.
- AI requests approval before changing: architecture, project structure, repository organization, public APIs, documented standards, ADRs.

### Forbidden Behaviors
- Assume content from filenames/paths.
- Modify approved architecture without ADR authorization.
- Generate pseudo-code or incomplete implementations without explicit statement.
- Create unused projects, placeholder folders, future modules, speculative code.
- Group unrelated changes into a single commit.
- Present assumptions as facts or omit uncertainties.

### Completion Report (Required After Every Step)
- Modified Files list
- Created Files list
- Deleted Files list
- Documentation Updates (or explicit "No documentation updates required.")
- Build Expectation (Restore/Build/Zero Warnings/Zero Errors)
- Validation steps
- Suggested Git commit message
- Next Recommended Step

---

# 2. VISION & SCOPE

## 2.1 Vision Statement
Enterprise-grade, modular, multi-tenant platform managing the complete lifecycle of machinery and industrial assets across multiple organizations.

## 2.2 Target Assets
- Heavy Equipment, Construction Machinery, Mining Equipment, Industrial Machines, Fleet Assets, Replaceable Components, Operational Resources.

## 2.3 Platform Capabilities (Complete Lifecycle)
- Asset Registration, Equipment Classification, Organizational Structure, Projects & Job Sites
- Meter Readings (Hour Meter, Odometer, Distance Meter, Meter Replacement)
- Engine Management (Models define specs; Instances represent physical engines; independent lifecycle)
- Replaceable Components (install, remove, repair, transfer, rebuild, scrap; complete installation history)
- Repairs, Preventive Maintenance, Corrective Maintenance, Breakdown Maintenance, Outsourced Maintenance
- Maintenance Approval Workflow, Service Orders, Repair Orders, Parts Consumption, Labor Tracking
- Fuel Management (Diesel, Gasoline), Lubricant Management (Engine Oil, Hydraulic Oil, Gear Oil, Transmission Oil, Coolant, Grease, Brake Fluid)
- Cost Management, Financial Management, Depreciation, Book Value, Estimated Market Value
- Attachments, Knowledge Library, Technical Documents, Image Gallery
- Notifications, Reporting, Analytics, Forecasting, Decision Support
- Internal Messaging, Relationship Management, Distributed Workspace Synchronization
- AI-Assisted Decision Support

## 2.4 Architectural Philosophy
- Source of truth = Measurements + Transactions + Lifecycle Events.
- Current values are DERIVED from historical operational data, never stored as primary truth.
- Everything with an operational lifecycle is an Asset (first-class domain object with own history).
- Replacing a meter never resets lifetime operational hours.
- Forecasts assist decisions but never replace recorded data.
- Historical values are never overwritten.

## 2.5 Target Users
- Enterprise administrators, Organization administrators, Maintenance managers, Warehouse managers, Procurement officers, Financial departments, Machine operators, Executive management.

## 2.6 Core Product Principles
- Clean Architecture, DDD, Modular Monolith, Open Source First, Multi-Tenant by Design, Security by Design, Documentation First, Maintainability First.

## 2.7 Success Criteria
- Reliable multi-company management, consistent business processes, high-quality documentation, sustainable architecture, low maintenance cost, easy onboarding.
- Architecture remains understandable 10 years from now.

## 2.8 Documentation Governance Process
```
Business Requirement → Technology Evaluation (TE) → Proof of Concept (POC, optional) → Architecture Decision Record (ADR) → Implementation
```

## 2.9 Repository Structure
```
Repository/
├── README.md
├── REPOSITORY_GUIDE.md
├── PROJECT_CHARTER.md
├── PROJECT_PROGRESS.md
├── DOCUMENTATION_REVIEW_CHECKLIST.md
├── AI_ENGINEERING_CONTRACT.md
├── docs-english/
│   ├── 01-vision/
│   ├── 02-architecture/
│   ├── 03-domain/
│   ├── 04-modules/
│   ├── 05-development/
│   ├── 06-decisions/
│   ├── 07-api/
│   ├── 08-releases/
│   └── 09-proof-of-concepts/
├── src/
│   ├── Host/
│   ├── Modules/
│   ├── BuildingBlocks/
│   └── (Shared/)
├── tests/
├── tools/
├── Directory.Build.props
├── Directory.Packages.props
├── MachineryManagerEnterprise.slnx
└── .github/
```

## 2.10 Documentation Naming Conventions
- General Document: `DOC-Name.md`
- Technology Evaluation: `TE-0001-Name.md`
- Architecture Decision: `ADR-0001-Name.md`
- Proof of Concept: `POC-0001-Name.md`
- Business Specification: `BR-001-Name.md`

## 2.11 Development Workflow Order
1. Business Analysis → 2. Documentation → 3. Domain Design → 4. Implementation → 5. Testing → 6. Review → 7. Commit → 8. Push

## 2.12 Naming Principles
- Names represent business concepts.
- Avoid abbreviations whenever possible.
- Business terminology has priority over technical terminology.

## 2.13 Project Memory Principle
- Official memory = repository. Neither conversations nor individuals are source of truth.
- AI suggestions are valid only after documentation and approval.

---

# [SECTIONS PENDING: 3-11 will be populated in subsequent turns]



# 3. ARCHITECTURE & TECHNOLOGY STACK

## 3.1 Architectural Style
- **Pattern:** Modular Monolith + Clean Architecture + DDD + CQRS
- **Multi-Tenancy:** Multi-tenant by design
- **Offline Capability:** Distributed Workspace & Offline-First Client Architecture
- **Future-Ready:** Microservice extraction possible without redesign

## 3.2 Layer Architecture (Dependency Direction: Inward)
```
Presentation (Blazor Server, .NET MAUI Client, Web API)
    ↓
Application (CQRS Commands/Queries, MediatR, FluentValidation, Mapster)
    ↓
Domain (Entities, Aggregates, Domain Events, Value Objects) — ZERO infrastructure deps
    ↓
Infrastructure (EF Core, Serilog, OpenTelemetry, RabbitMQ, S3, Search, Qdrant)
```

### Layer Responsibilities
- **Presentation:** Blazor Server + MudBlazor (web), .NET MAUI (desktop/mobile), REST API + OpenAPI, Auth (ASP.NET Core Identity / OpenIddict)
- **Application:** Use Case Orchestration, CQRS via MediatR, Pipeline Behaviors (Logging, Validation, Performance, Transaction), Input Validation (FluentValidation), DTO Mapping (Mapster)
- **Domain:** Core Business Rules, Aggregates/Entities/Value Objects, Domain Events, Asset Lifecycle Logic — NO infrastructure concerns
- **Infrastructure:** Relational DB (EF Core + Dapper), Embedded Local DB (SQLite + LiteDB), Workspace Sync Engine, Logging (Serilog), Observability (OpenTelemetry), Messaging (RabbitMQ + MassTransit), File Storage (MinIO/S3), Search (Meilisearch/Elasticsearch), AI Kernel (Semantic Kernel + Qdrant)

## 3.3 Modular Monolith Rules
- Each module owns: Domain Logic, Application Commands/Queries, Data Storage Schema, Public Service Contracts
- Modules communicate asynchronously via Domain Events or explicit interfaces
- Module boundaries derived from Capability Model

## 3.4 CQRS & Event Pipeline
- Commands modify state and enforce invariants
- Queries execute read-optimized projections without modifying state
- MediatR behaviors handle cross-cutting concerns

## 3.5 Multi-Tenancy & Workspace Topology
- Enterprise Central Cloud Workspace
- Regional / Field Project Workspaces
- Individual Mobile / Offline User Workspaces
- Workspace sync preserves tenant boundaries and business integrity via synchronized packages

## 3.6 Observability (Built into All Layers)
- Structured Logging: Serilog
- Distributed Tracing & Metrics: OpenTelemetry, Prometheus, Jaeger
- Health Checks & Diagnostic Endpoints

## 3.7 Approved Technology Stack (Final Decisions Only)

| Layer | Technology | ADR | TE |
|-------|-----------|-----|-----|
| Runtime | .NET 10 SDK 10.0.302, C# 14 | ADR-0003 | TE-0001 |
| Web UI | Blazor (Server / WebAssembly / Auto) | ADR-0004 | TE-0002 |
| UI Components | MudBlazor (Material Design) | ADR-0005 | TE-0003 |
| Client (Desktop/Mobile) | .NET MAUI + Blazor Hybrid | ADR-0013 | TE-0010 |
| ORM & Data Access | Entity Framework Core 10 + Dapper | ADR-0006, ADR-0019 | TE-0004, TE-0024 |
| DB Migration | EF Core Migrations + Respawn | ADR-0037 | TE-0025 |
| Embedded DB (Offline) | SQLite (relational) + LiteDB (document) | ADR-0014 | TE-0011 |
| Validation | FluentValidation + MediatR Pipeline Behavior | ADR-0007, ADR-0036 | TE-0005, TE-0022 |
| Object Mapping | Mapster (compile-time code generation, IQueryable projection) | ADR-0008 | TE-0006, TE-0023 |
| CQRS Engine | MediatR (IPipelineBehavior for cross-cutting) | ADR-0011 | TE-0009 |
| API Documentation | OpenAPI, Scalar, NSwag | ADR-0035 | TE-0021 |
| Logging | Serilog (structured JSON, contextual enrichment: CorrelationId, TenantId) | ADR-0009 | TE-0007 |
| Telemetry | OpenTelemetry (OTLP, vendor-neutral) | ADR-0010, ADR-0033 | TE-0008, TE-0017 |
| Messaging | MassTransit over RabbitMQ | ADR-0016 | TE-0012 |
| External Integration | MassTransit-based Connector Framework (+ Azure Logic Apps opt-in) | ADR-0018 | TE-0036 |
| AI Engine | Semantic Kernel + Multi-Provider Router (Azure OpenAI / OpenAI / Ollama) | ADR-0017, ADR-0022, ADR-0023 | TE-0013, TE-0028, TE-0029 |
| File Storage | MinIO / S3 Compatible Object Store | ADR-0020 | TE-0026 |
| Search Engine | SQL Server Full-Text Search (default) + OpenSearch (escalation) | ADR-0021 | TE-0027 |
| Vector Search | Qdrant | ADR-0022 | TE-0028 |
| Config & Secrets | Microsoft.Extensions.Configuration/Options + HashiCorp Vault | ADR-0034 | TE-0018 |
| Caching | FusionCache (IMemoryCache L1 + Redis L2) | ADR-0031 | TE-0015 |
| Background Processing | Quartz.NET + System.Threading.Channels | ADR-0032 | TE-0014, TE-0019 |
| Testing | xUnit, Moq, Testcontainers, k6, NBomber | ADR-0024, ADR-0027 | TE-0030, TE-0033 |
| Security & Identity | ASP.NET Core Identity + OpenIddict (Identity); Data Protection, AES-256, X.509 (Encryption) | ADR-0030, ADR-0026 | TE-0020, TE-0032 |
| Build & Deploy | Docker, GitHub Actions, .NET Aspire | ADR-0025 | TE-0031 |
| Reporting | QuestPDF (FastReport & RDLC EXCLUDED) | ADR-0029 | TE-0035 |

> **Superseded:** Avalonia UI (TE-0034 / ADR-0028) — replaced by .NET MAUI. Not part of active stack.

## 3.8 Technology Placement Rules
- **EF Core:** ONLY inside Infrastructure layer. Domain, Application, Presentation must NEVER reference EF Core packages.
- **FluentValidation:** ONLY inside Application layer. Must NOT access Infrastructure or databases.
- **Mapster:** Decouples Domain entities from DTOs. Use `.ProjectToType()` for EF Core queries.
- **MediatR:** Controllers/UI send Requests via `IMediator`; handlers execute business logic.
- **Serilog:** Configured via `ILogger` abstractions. Infrastructure implements logging.

## 3.9 Capability Model (Business → Module Mapping)
Platform is designed around **Asset** (not Machine). Every maintainable object with independent lifecycle is an Asset.

### Capabilities
- Organization Management
- Asset Management (Registration, Classification, Models, Specs, Lifecycle, Status, Ownership, Assignment, Retirement)
- Component Management (Engine, Transmission, Attachment, Replaceable Components; Install, Remove, Transfer, Rebuild, History)
- Meter Management (Hour Meter, Odometer, Distance Meter, Replacement, Operational/Non-operational Usage, Validation, History)
- Maintenance Management (Preventive, Corrective, Breakdown, Work Orders, Service Scheduling, History, Costs)
- Fuel & Lubrication (Consumption tracking for Diesel, Gasoline, Engine Oil, Hydraulic Oil, Gear Oil, Coolant, Grease, Brake Fluid)
- Spare Parts Management (Catalog, Inventory, Stock Transactions, Suppliers, Purchase/Consumption History)
- Financial Management (Purchase, Initial/Current Value, Depreciation, Operating/Maintenance/Ownership Costs, Cost Analysis)
- Document Management (Ownership, Insurance, Licenses, Contracts, Certificates, Manuals, Parts Books, Technical Docs, Expiration Tracking)
- Media Management (Image Gallery, Videos, Attachments, Event Albums, Export)
- Knowledge Management (Repair Manuals, Parts Catalogs, Technical Bulletins, Best Practices, Shared Documents)
- Forecasting (Fuel, Lubricant, Filter, Spare Parts, Maintenance, Budget)
- Notifications (Maintenance Alerts, Expiring Documents, Warranty, Inspection, Custom)
- Reporting & Analytics (Operational, Financial, Maintenance Reports, KPI Dashboard, Cost/Performance Analysis)
- Administration (Users, Roles, Permissions, Audit Logs, Settings, System Config)

### Out of Scope
Accounting, Payroll, HR, CRM, ERP Replacement (integrate but not replace)

### Future Expansion Ready
Multi-company, Fleet, IoT, Telematics, Predictive Maintenance, AI Diagnostics, GIS, Digital Inspections, External ERP Integration

## 3.10 Architecture Governance
- No architectural change bypasses ADR process
- All 35 technology gaps have completed TEs and ADRs
- Decision sequence: Business Requirement → Capability Model → Technology Gap Analysis → ADR → TE → Implementation

---



# 4. DOMAIN MODEL

## 4.1 Bounded Contexts (9 Contexts)

| Context | Owns | Does NOT Own |
|---------|------|-------------|
| **Organization** | Holding, Organization Identity/Structure, Project, Asset Ownership, current Project assignment of Assets/Personnel/Warehouse Inventory (BR-017) | Asset lifecycle, Financial calc, Auth, Usage/Maintenance history (only references the Project that was current when recorded) |
| **Asset** | Asset, Asset Model, Status, Lifecycle, Classification, Hierarchy | Engine internals, Meter readings, Maint history, Financial calc |
| **Component** | Engine, Transmission, Attachments, Batteries, Hydraulic Components | Asset lifecycle |
| **Usage** | Meter Device, Meter Reading, Operational Usage, Non-operational Usage | Assets |
| **Maintenance** | Maintenance Plans, Work Orders, Inspections, Failures, Repairs, Replacements | Operational Usage |
| **Finance** | Purchase Info, Acquisition Cost, Depreciation, Operating Costs, TCO, Asset Valuation | Maintenance execution, Meter readings, Technical specs, Documents |
| **Document** | Ownership Docs, Insurance, Registration, Warranty, Inspection/Calibration Certs, Expiration Rules | |
| **Knowledge** | Operator/Workshop Manuals, Parts Catalogs, Service Bulletins, Technical Procedures, Repair Guides | |
| **Forecast** | Consumption Forecasts, Maintenance Forecasts, Replacement Forecasts, Cost Forecasts | |
| **Media** (implied) | Images, Galleries, Categories, Metadata | |

### Context Ownership Rules
- Every business object has exactly ONE owning context
- Other contexts may reference but shall NOT redefine business rules
- Contexts communicate via Domain Events (loose coupling)

### Context Map
```
Organization → Asset → Component
                    → Usage → Maintenance → Forecast
                    → Document
                    → Finance
Asset Models / Component Models → Knowledge
```

## 4.2 Entity Classification

### Master Entities (permanent identities, years-long lifecycle)
- Asset, Asset Model, Engine, Engine Model, Component, Component Model, Organization, Supplier, Manufacturer

### Operational Entities (day-to-day activities, continuously grow)
- Meter Device, Meter Reading, Operational Usage, Non-operational Usage
- Maintenance Activity, Inspection, Failure, Repair, Replacement
- Financial Transaction, Forecast

### Historical Entities (append-only, never deleted)
- Ownership History, Engine Installation History, Meter Replacement History
- Maintenance History, Failure History, Repair History, Financial History, Status History, Location History

### Reference Entities (shared reusable info)
- Manufacturer, Supplier, Dealer, Fuel Type, Lubricant Type, Failure Category, Maintenance Category, Document Type, Image Category, Unit of Measure

## 4.3 Value Objects
- **Money:** Amount + Currency
- **Date Range:** Start Date + End Date
- **Meter Value:** Numeric Value + Unit + Reading Time
- **Location:** Site + Region + GPS (optional)
- **Technical Specification:** Power, Torque, Capacity, Pressure, Flow Rate (no independent identity)

## 4.4 Key Ownership Rules
- **Asset owns:** Lifecycle, Current Status, Current Location, Current Installed Components (refs), Current Meter Devices (refs)
- **Asset does NOT own:** Engine history, Meter history, Repairs, Financial calculations, Forecast generation
- **Engine owns:** Its own lifecycle, identity, specs, history. Asset NEVER owns Engine history.
- **Meter Device owns:** Physical meter lifecycle. Does NOT own Usage.
- **Operational Usage owns:** Validated productive usage. Business calculations ALWAYS consume Operational Usage.
- **Maintenance owns:** Plans, Work Orders, Records
- **Finance owns:** Transactions, Cost Calculations, Depreciation, Valuation
- **Documents own:** Expiration, Version History, File Metadata

## 4.5 Aggregate Design (6 Aggregates)

### Rules
- One transaction = one aggregate ONLY (strong consistency inside, eventual consistency between)
- Aggregate Roots expose BEHAVIOR, not data
- Internal entities never accessed directly
- Cross-aggregate consistency = eventual (via Domain Events)
- Aggregates intentionally small: 1 root + few children + clear invariants

### Asset Aggregate
- **Root:** Asset
- **Contains:** Asset, Current Status, Current Location, Current Installed Components (refs), Current Meter Devices (refs)
- **Invariants:**
  - Exactly one permanent identity (never changes)
  - Exactly one current lifecycle state at any time
  - Zero or one installed primary Engine
  - Only one active primary Meter Device per measurement type
- **Lifecycle:** Draft → Registered → Commissioned → Operational → Inactive → Retired → Disposed

### Engine Aggregate
- **Root:** Engine
- **Contains:** Engine, Specs, Status, Current Installation, Lifecycle, Installation History
- **Invariants:**
  - Immutable identity
  - Installed on AT MOST one Asset at any time
  - Every installation/removal generates historical record (never modified/removed)
  - Manufacturer, Serial Number, Manufacturing Year are immutable
- **Lifecycle:** Stored → Installed → Removed → Under Repair → Rebuilt → Stored → ... → Retired

### Maintenance Aggregate
- **Root:** Maintenance Record
- **Contains:** Maintenance Record, Inspection, Failure, Repair, Replacement, Labor Records, Parts Consumption
- **Invariants:**
  - Completed records become immutable
  - Repair references originating Failure
  - Replacement identifies Removed + Installed Component (both permanent historical records)

### Financial Aggregate
- **Root:** Financial Account
- **Contains:** Purchase Info, Financial Transactions, Depreciation, Asset Valuation, Operating Cost Summary
- **Invariants:**
  - Purchase Value NEVER changes
  - Current Value ALWAYS calculated (never overwrites Purchase Value)
  - Transactions immutable after posting; corrections = new transactions

### Document Aggregate
- **Root:** Document
- **Contains:** Metadata, Versions, Expiration Info, Attachments
- **Invariants:**
  - Versions immutable
  - Expired documents remain accessible (only operational status changes)

### Forecast Aggregate
- **Root:** Forecast
- **Contains:** Consumption Forecast, Maintenance Forecast, Replacement Forecast, Cost Forecast
- **Invariants:**
  - Forecasts NEVER modify business history
  - Recalculated whenever required
  - Historical forecasts optionally retained for comparison

### Knowledge Aggregate
- **Root:** Technical Library Item
- **Contains:** Manual, Repair Guide, Parts Catalogue, Service Bulletin, Technical Drawing
- **Invariants:** Documents belong to Models whenever possible; same doc may serve many Assets

## 4.6 Domain Services (by Category)

### Asset Lifecycle Services
- **AssetRegistrationService:** Create identity, validate uniqueness, assign model, init lifecycle, create initial history
- **AssetRetirementService:** Validate rules, terminate lifecycle, preserve history, publish event
- **AssetTransferService:** Validate transfer, preserve ownership history, update current ownership

### Component Lifecycle Services
- **EngineInstallationService:** Verify availability, verify compatibility, close previous installation, create history, update refs
- **EngineRemovalService:** Validate removal, preserve history, update Engine status, update Asset config
- **ComponentReplacementService:** Remove existing, install replacement, preserve historical relationships, notify Maintenance

### Usage Services
- **MeterValidationService:** Detect impossible values, duplicates, rollback, abnormal jumps. Only validated readings → Operational Usage.
- **UsageCalculationService:** Consume validated readings, exclude non-operational, calculate accumulated usage, produce usage events. ALWAYS event-derived, NEVER from current Meter value.

### Maintenance Services
- **MaintenancePlanningService:** Evaluate intervals, usage, determine required maintenance, schedule, publish events
- **MaintenanceExecutionService:** Validate request, create record, record labor, record parts, update history
- **FailureAnalysisService:** Classify failures, identify affected components, estimate downtime, recommend actions, notify Forecast
- **ReplacementDecisionService:** Evaluate repair history, operating hours, cost, remaining useful life

### Financial Services
- **DepreciationCalculationService:** Preserve acquisition value, determine method, calculate accumulated depreciation, calculate current estimated value
- **OwnershipCostService:** Calculate TCO (purchase + transportation + taxes + insurance + maintenance + fuel + lubricants + spare parts + external services)
- **AssetValuationService:** Evaluate depreciation, maintenance history, usage, estimate current value. NEVER modifies financial history.

### Document Services
- **DocumentExpirationService:** Detect upcoming expiration, generate reminders, classify expired, publish events
- **DocumentPackageService:** Produce packages (Ownership, Insurance, Technical, Regulatory) → PDF/ZIP/Printable
- **DocumentVersionService:** Register new versions, preserve previous, maintain traceability

### Forecast Services
- **ConsumptionForecastService:** Predict fuel, engine oil, hydraulic oil, gear oil, coolant, grease usage
- **MaintenanceForecastService:** Evaluate intervals, accumulated usage, estimate next dates, estimate workload
- **SparePartsForecastService:** Predict filters, belts, tires, wear parts, batteries demand
- **ComponentReplacementForecastService:** Predict replacement of high-value components (engine, transmission, hydraulic pump)

### Validation Services
- **AssetValidationService:** Identity, serial uniqueness, lifecycle transitions, ownership rules
- **ComponentValidationService:** Compatibility, installation rules, replacement rules
- **UsageValidationService:** Impossible readings, duplicates, abnormal jumps, counter rollback, operational consistency
- **FinancialValidationService:** Transaction consistency, currency rules, depreciation inputs
- **DocumentValidationService:** Mandatory metadata, expiration dates, document type, ownership relationships

## 4.7 Domain Events (Complete Catalog)

### Event Structure (Required Fields)
EventId, EventType, OccurredAt, AggregateId, AggregateType, EventVersion, CorrelationId (opt), CausationId (opt)

### Naming Convention: `BusinessObject + PastTenseVerb` (e.g., AssetRegistered, EngineInstalled)

### Asset Events
AssetRegistered, AssetActivated, AssetTransferred, AssetRetired, AssetDisposed

### Component Events
EngineInstalled, EngineRemoved, EngineRebuilt, ComponentInstalled, ComponentRemoved, ComponentReplaced

### Incident Events (BR-009)
IncidentReported, IncidentValidated, IncidentRejected, IncidentClassified, IncidentAssigned, IncidentInvestigationStarted, IncidentDecisionMade, IncidentResolved, IncidentClosed, IncidentReopened

### Usage Events
MeterInstalled, MeterRemoved, MeterFailureDetected, MeterReadingRecorded, OperationalUsageCalculated, NonOperationalUsageRecorded

### Maintenance Events
MaintenanceRequested, MaintenancePlanned, MaintenanceApproved, MaintenanceScheduled, MaintenanceStarted, MaintenanceCompleted, MaintenanceVerified, MaintenanceClosed, MaintenanceCancelled, MaintenanceSuspended, MaintenanceResumed, InspectionCompleted, FailureDetected, RepairStarted, RepairCompleted

### Financial Events
PurchaseRecorded, FinancialTransactionRecorded, DepreciationCalculated, AssetValuationUpdated, OwnershipCostUpdated

### Relationship Events (BR-015)
RelationshipCreated, RelationshipActivated, RelationshipModified, RelationshipExpired, RelationshipArchived

### Document Events
DocumentRegistered, DocumentUpdated, DocumentExpired, DocumentRenewed

### Forecast Events
ForecastRequested, ForecastGenerated, ForecastValidated, ForecastApproved, ForecastScheduled, ForecastConsumed, ForecastCompleted, ForecastCancelled, ConsumptionForecastGenerated, MaintenanceForecastGenerated, SparePartsForecastGenerated, ReplacementForecastGenerated

### Notification Events (BR-012)
NotificationCreated, NotificationQueued, NotificationDelivered, NotificationViewed, NotificationAcknowledged, NotificationArchived, NotificationCancelled

### Internal Messaging Events (BR-013)
MessageCreated, MessageSent, MessageDelivered, MessageRead, MessageArchived, MessageDeleted

### Event Publishing Rules
- Publish ONLY after business transaction completes successfully
- Aggregate invariants must remain valid
- Business state must have changed
- Events are immutable (never modified after publish)
- Publishers never know consumers
- One event may be consumed by multiple contexts

## 4.8 Business Rules (40 Rules)

### Asset Rules (BR-001–004)
- BR-001: Every Asset has one permanent identity (never changes)
- BR-002: Every Asset belongs to exactly one Asset Model
- BR-003: Every Asset has exactly one current lifecycle state; historical states preserved
- BR-004: Retiring an Asset never removes its business history

### Component Rules (BR-005–009)
- BR-005: Engine is an independent business object, not a property of Asset
- BR-006: One Engine may serve multiple Assets; every installation recorded
- BR-007: Engine may exist without being installed (warehouse, repair, storage)
- BR-008: Engine identity unchanged after rebuilding; rebuilding creates maintenance history
- BR-009: Replacement Component preserves its own lifecycle; removed Component never disappears from history

### Meter Rules (BR-010–014)
- BR-010: Meter Device is independent from Operational Usage
- BR-011: Meter replacement never resets accumulated Operational Usage
- BR-012: Replacement Meter may have previous readings; those belong to Meter Device, not Asset
- BR-013: Every Meter replacement generates historical records; Meter history never lost
- BR-014: Meter Device may fail; failure never invalidates historical business calculations

### Operational Usage Rules (BR-015–019)
- BR-015: Operational Usage calculated from validated business events, NEVER from latest Meter value
- BR-016: Operational Usage = productive work only (machine operating, driving, excavation, etc.)
- BR-017: Non-operational Usage NEVER contributes to Preventive Maintenance, Depreciation, Performance Indicators, Consumption Forecasts
- BR-018: Meter validation rejects impossible readings (negative, impossible jumps, duplicates, physically impossible rates); rejected readings never participate in calculations
- BR-019: Operational Usage immutable after validation; corrections = business adjustment events

### Maintenance Rules (BR-020–024)
- BR-020: Preventive Maintenance always based on validated Operational Usage
- BR-021: Corrective Maintenance always references originating Failure
- BR-022: Every completed Maintenance Activity becomes immutable; errors → new corrective Record
- BR-023: Component replacement preserves: Removed, Installed, Date, Technician; historical relationships never deleted
- BR-024: Engine replacement updates only current operational config; historical installations unchanged

### Financial Rules (BR-025–029)
- BR-025: Purchase Price is immutable
- BR-026: Current Asset Value always calculated; never overwrites Purchase Price
- BR-027: Every financial expense permanently traceable (fuel, lubricants, repairs, parts, insurance, taxes, transportation)
- BR-028: TCO includes every operating expense
- BR-029: Depreciation calculations never modify historical financial transactions

### Document Rules (BR-030–033)
- BR-030: Business Documents never physically deleted
- BR-031: Expired Documents remain valid historical records; only operational status changes
- BR-032: Every document has version history; older versions accessible
- BR-033: Document reminders generated before expiration per configurable rules

### Forecast Rules (BR-034–037)
- BR-034: Forecasts always based on historical validated data
- BR-035: Forecasts never modify business history
- BR-036: Forecasts are advisory; humans remain responsible for decisions
- BR-037: Forecast models may evolve; historical Forecasts remain reproducible

### Historical Integrity Rules (BR-038–040)
- BR-038: Business history append-only; existing records never removed
- BR-039: Every business identity permanent throughout lifecycle
- BR-040: Historical relationships reconstructable at any point in time

## 4.9 State Machines

### Asset Lifecycle
States: Draft → Registered → Commissioned → Operational → Inactive → Retired → Disposed
Allowed: Draft→Registered, Registered→Commissioned, Commissioned→Operational, Operational↔Inactive, Operational→Retired, Inactive→Retired, Retired→Disposed
Forbidden: Draft→Operational, Registered→Retired, Disposed→Operational, Retired→Commissioned

### Engine Lifecycle
States: Stored → Installed → Removed → Under Repair → Rebuilt → Stored → ... → Retired
Allowed: Stored→Installed, Installed→Removed, Removed→Stored, Removed→UnderRepair, UnderRepair→Rebuilt, Rebuilt→Stored, Installed→Retired, Stored→Retired
Constraint: Only one Asset may reference an Engine in Installed state

### Meter Device Lifecycle
States: Registered → Installed → Operational → Failed → Removed → Archived
Rules: Removing never removes history; Replacing never resets Operational Usage

### Maintenance Lifecycle (9 states + branches)
States: Requested → Planned → Approved → Scheduled → Started → In Progress → Completed → Verified → Closed
Alt: Requested/Planned/Approved/Scheduled → Cancelled
Alt: Started/In Progress → Suspended → In Progress (resumed)
Forbidden: Completed→InProgress, Requested→Completed (mandatory stages not skipped), Closed→anything (immutable)

### Failure Lifecycle
States: Detected → Diagnosed → Repair Planned → Repair In Progress → Resolved → Closed

### Incident Lifecycle (8 states + branches)
States: Reported → Validated → Classified → Assigned → Under Investigation → Decision → Resolved → Closed
Alt: Reported/Validated → Rejected
Alt: Closed → Reopened → Under Investigation
Forbidden: Skip mandatory states; Closed never modified directly

### Document Lifecycle
States: Draft → Approved → Active → Expiring → Expired → Replaced → Archived
Rules: Expired remain historical; Archived remain accessible

### Forecast Lifecycle (7 states + branch)
States: Generated → Validated → Approved → Scheduled → Consumed → Completed
Alt: Generated/Validated/Approved/Scheduled → Cancelled
Rules: Forecasts never overwrite previous; Cancelled/Completed may be regenerated as new object

### Financial Record Lifecycle
States: Draft → Recorded → Posted → Closed
Correction: Posted → Adjustment Created
Rules: Posted transactions immutable; corrections = new transactions

### Relationship Lifecycle (5 states)
States: Draft → Active → Modified → Expired → Historical
Allowed: Draft→Active, Active→Modified, Modified→Active, Active→Expired, Expired→Historical
Forbidden: Historical→Active/Modified (immutable); Draft does not participate in operational propagation

### Generic State Machine Rules (SM-001–006)
- SM-001: Exactly one current state
- SM-002: Every transition timestamped
- SM-003: Every transition preserves history
- SM-004: Illegal transitions rejected
- SM-005: Successful transitions publish Domain Events
- SM-006: State transitions never bypass Aggregate invariants

## 4.10 Domain Discovery — Business Capabilities

| ID | Capability | Status | Priority |
|----|-----------|--------|----------|
| DD-001 | Asset Management | Implemented Foundation | High |
| DD-002 | Asset Relationships | Discovered | High |
| DD-003 | Tracked Components | Discovered | High |
| DD-004 | Tire Lifecycle | Discovered | High |
| DD-005 | Battery Lifecycle | Discovered | High |
| DD-006 | Parts Catalog | Discovered | High |
| DD-007 | Part Cross Reference | Discovered | High |
| DD-008 | Incident Management | Discovered | High |
| DD-009 | Maintenance Forecast | Discovered | High |
| DD-010 | Maintenance Operations | Discovered | High |
| DD-011 | Notification Center | Discovered | High |
| DD-012 | Internal Messaging | Discovered | Medium |
| DD-013 | AI Assistant | Discovered | Medium |
| DD-014 | Lifecycle Tracking | Cross-cutting | High |
| DD-015 | Relationship Management | Cross-cutting | High |

### Cross-Cutting Domain Concepts
- **Lifecycle:** Every physical object has independent lifecycle; history always preserved
- **Relationships:** Temporary/permanent relationships between objects; preserve independent ownership + operational linkage
- **Operational Usage:** Traceable to originating asset; supports propagation where required
- **Historical Traceability:** Every important action historically traceable; history never overwritten

### Core Business Invariants (BI-001–010)
- BI-001: Every physical object has independent lifecycle
- BI-002: Physical object may exist independently from asset where installed
- BI-003: Installation never changes ownership; only operational relationships
- BI-004: Operational usage historically traceable; history never rewritten
- BI-005: Business history never destroyed; corrections create additional history
- BI-006: Business identity never changes; only state changes
- BI-007: Business relationships are first-class knowledge with history
- BI-008: Forecasts are not maintenance; become maintenance only after approval
- BI-009: Notifications never create business events; only inform about them
- BI-010: AI never modifies business data directly; produces recommendations only

## 4.11 Ubiquitous Language — Governance Rules
1. **Single Source of Truth:** All entities, value objects, events, commands, queries, code identifiers MUST match this document
2. **Zero Synonyms:** `Asset` is the sole term; `Equipment`/`Machine` BANNED
3. **No Technical Leaks:** No `AssetRowDTO`, `EquipmentTableManager` in business vocabulary
4. **Code Auditing:** Static analysis verifies entity names match ubiquitous language

### Banned Terminology (MUST Replace)
| Banned | Required Replacement | Reason |
|--------|---------------------|--------|
| Equipment / Machine | Asset | Violates Ubiquitous Language |
| FixTicket / Task | WorkOrder | Vague |
| Item / Stuff | Part / Asset | Non-descriptive |
| UserCompany | Organization | Confuses Identity with Organization; Company = manufacturer brand |
| DeviceData | MeterReading / Telemetry | Technical jargon |
| Company (as tenant grouping) | Holding | Company = manufacturer brand ONLY (reserved, see above); the tenant grouping above Organization is Holding (BR-017) |
| Enterprise (in new code) | Organization | BR-016's "Enterprise" is a legacy synonym for Organization (see BR-016 terminology reconciliation); new code/UI should say Organization, not Enterprise |

### Translation Rules
- **Source Code:** ALL C# classes, interfaces, enums, properties, DB columns, API JSON fields, commit messages = ENGLISH using exact Ubiquitous Language identifiers
- **User Interface:** ALL Blazor UI, MudBlazor dialogs, labels, tooltips, exported PDF reports = PERSIAN (official translation from Vocabulary Dictionary)
- **No Direct Literal Translations:** Use industry-accepted Persian domain terminology (e.g., "دستور کار" for WorkOrder, NOT "سفارش کار")

## 4.12 Domain Patterns (DP-001 – DP-015)

| ID | Pattern | Purpose |
|----|---------|---------|
| DP-001 | Business Operation | Every state change through Business Operation → Business Event → Business History → Current State. NEVER direct update. |
| DP-002 | Projection | Current State = projection from Business History. History is source of truth. Projections disposable/rebuildable. |
| DP-003 | Lifecycle | Every entity owns independent lifecycle via historical events. Current stage = projection. |
| DP-004 | Relationship | Relationships are independent business objects with start/end/purpose/rules. NEVER merge identities. |
| DP-005 | Planning vs Execution | Planning (intention) and Execution (reality) are independent. Neither overwrites the other. |
| DP-006 | Master Data | Single authoritative source of reference info. Consumed, not owned, by capabilities. |
| DP-007 | Approval | Separate preparation from authorization. Approval changes status, does not execute. |
| DP-008 | Versioning | Master Data evolves through revisions. Each revision complete + valid for its period. Historical refs preserved. |
| DP-009 | Hierarchical Relationship | Hierarchy = directed, acyclic, traceable Relationship. NEVER hard-coded ParentId. |
| DP-010 | Advisory Intelligence | AI observes/analyzes/recommends. NEVER approves/rejects/executes/modifies state. Human governance mandatory. |
| DP-011 | Working Set | Minimum business info required for Workspace to perform responsibilities. NOT a cache. |
| DP-012 | Synchronization | Controlled propagation of validated business info between adjacent Workspaces. Bi-directional, idempotent, eventually consistent. |
| DP-013 | Synchronization Package | Transport container for validated business info. Immutable, atomic import, transport-independent. |
| DP-014 | Conflict Resolution | Detect/classify/resolve/audit conflicts during sync. Business process, not technical merge. |
| DP-015 | Business Traceability | Every artifact preserves complete chain from origin through every derived artifact. Historical records never deleted. |

### Pattern Dependency Order (for Workspace)
DP-011 → DP-012 → DP-013 → DP-014

---



# 5. APPLICATION LAYER (Modules)

## 5.1 Application Layer Responsibilities
- Thin orchestration layer — NO business logic
- Execute Use Cases, Coordinate Aggregates, Manage Transactions
- Call Domain Services, Publish Domain Events
- Call Infrastructure Services (through abstractions)
- Authorization, Validation of Application Requests
- Owns transaction boundaries
- Translates Domain Exceptions → Application Results

### Architectural Flow
```
User → Controller → Application Command → Command Handler → Domain Aggregate → Domain Events → Infrastructure → Response
```
- NO controller communicates directly with Domain Layer

### Module Dependency Rules
- Modules communicate through Application contracts
- NEVER reference Infrastructure of another module
- Exchange data only through Contracts
- Publish integration events when required

## 5.2 Modules (19 Modules)

| Module | Key Use Cases |
|--------|--------------|
| **Asset Management** | Register, Modify, Retire, Transfer, View History, Search, Export |
| **Engine Management** | Register, Install, Remove, Replace, Send/Return from Workshop, View History |
| **Component Management** | Register, Install, Remove, Replace, View Lifecycle |
| **Maintenance Management** | Create Plan, Schedule, Record Activity/Inspection/Failure/Repair/Overhaul, View History, Calculate Next Maintenance |
| **Meter Management** | Install/Replace Meter, Register Reading/Non-operational Usage, Correct Reading, View History, Calculate Usage |
| **Financial Management** | Register Purchase/Operating/Fuel/Maintenance/Insurance/Tax Expense, Calculate Depreciation/Current Value/TCO, View History |
| **Document Management** | Register, Upload Image/PDF, Replace Version, Monitor Expiration, Generate Reminder, Export Package, View History |
| **Technical Library** | Register Manual/Parts Catalogue/Service Manual, Assign to Model, View, Download |
| **Gallery** | Upload, Categorize, Browse, Filter by Date, Export, Generate Photo Report |
| **Forecasting** | Generate Fuel/Lubricant/Coolant/Grease/Filter/Spare Parts/Maintenance/Component Replacement Forecast, Compare with Actual, Export Report |
| **Reporting** | Asset/Engine/Maintenance/Failure/Financial/Depreciation/Operating Cost/Utilization/Document Status/Executive Dashboard Reports |
| **Administration** | Create/Deactivate User, Assign Role, Manage Permissions/Organizations/Locations, Audit Activity, View System Log |
| **Configuration** | Manage Asset/Engine/Component Models, Manufacturers, Suppliers, Maintenance Templates, Document Types, Forecast Parameters, Units of Measure, Notification Rules |
| **Organization Management** | Register Organization, View, Associate User, View Owned Assets |
| **Notification Center** | View Notifications/Detail, Acknowledge, Archive, Cancel, Manage Preferences |
| **Internal Messaging** | Start Conversation, Add Participant, Send Message, Attach File, Read, Archive, Delete, Close/Reopen Conversation |
| **AI Assistant** | Ask Business Question, Request Recommendation/Summary/Knowledge Discovery/Risk Assessment, Explain Recommendation |
| **Relationship Management** | Create, Activate, Modify, Expire Relationship, View Relationship/History |
| **Distributed Workspace Sync** | Initiate Sync, Create/Validate/Apply Sync Package, Request Working Set, View Sync History/Conflicts, Resolve Conflict |

### Cross-Module Workflows
- WF-001 Purchase Used Asset: Asset → Engine → Meter → Financial → Documents
- WF-002 Replace Engine: Asset → Engine → Maintenance → Financial
- WF-003 Replace Meter: Meter → Asset → Reporting
- WF-004 Complete Preventive Maintenance: Maintenance → Components → Financial → Forecast
- WF-005 Register Failure: Maintenance → Reporting
- WF-006 Renew Document: Documents → Notifications
- WF-007 Dispose Asset: Asset → Financial → Documents → Reporting
- WF-008 Generate Forecast: Forecast → Reporting

## 5.3 Commands

### Command Principles
- Represents one business intention
- Modifies business state
- Immutable; one responsible Handler; one expected outcome
- NEVER contains business logic

### Command Structure
CommandId, CommandType, RequestedAt, RequestedBy, CorrelationId (opt) + business fields

### Command Categories & Key Commands

| Category | Key Commands |
|----------|-------------|
| **Asset** | RegisterAsset, UpdateAssetInformation, TransferAsset, RetireAsset, DisposeAsset |
| **Engine** | RegisterEngine, InstallEngine, RemoveEngine, ReplaceEngine, SendEngineToWorkshop, ReturnEngineFromWorkshop, RegisterEngineRebuild |
| **Component** | RegisterComponent, InstallComponent, RemoveComponent, ReplaceComponent, RetireComponent |
| **Meter** | InstallMeter, ReplaceMeter, RegisterMeterReading, RegisterNonOperationalUsage, CorrectMeterReading, ArchiveMeter |
| **Maintenance** | RequestMaintenance, CreateMaintenancePlan, ApproveMaintenancePlan, ScheduleMaintenance, StartMaintenance, CompleteMaintenance, VerifyMaintenance, CloseMaintenance, CancelMaintenance, SuspendMaintenance, ResumeMaintenance, RegisterInspection, RegisterFailure, RegisterRepair, RegisterOverhaul, ReplaceMaintenanceComponent |
| **Financial** | RegisterAssetPurchase, RegisterOperating/Fuel/Maintenance/Insurance/Tax Expense, CalculateDepreciation, RecalculateAssetValue, RecalculateOwnershipCost |
| **Document** | RegisterDocument, UploadDocumentImage/File, ReplaceDocumentVersion, RenewDocument, ArchiveDocument, DeleteTemporaryDocument |
| **Forecast** | GenerateFuel/Lubricant/Coolant/Maintenance/SpareParts/Replacement Forecast, RefreshForecastModels, Validate/Approve/Schedule/Consume/Complete/CancelForecast |
| **Admin** | Create/DeactivateUser, AssignRole, ChangePermissions, CreateOrganization, RegisterLocation |
| **Config** | RegisterAsset/Engine/ComponentModel, RegisterManufacturer/Supplier, RegisterMaintenanceTemplate/DocumentType, UpdateForecastParameters, RegisterUnitOfMeasure |
| **Organization** | RegisterOrganization, AssociateUserWithOrganization |
| **Notification** | Acknowledge/Archive/CancelNotification, UpdateNotificationPreferences |
| **Messaging** | StartConversation, AddConversationParticipant, SendMessage, AttachFileToMessage, MarkMessageAsRead, Archive/DeleteMessage, Close/ReopenConversation |
| **AI Assistant** | AskBusinessQuestion, RequestRecommendation, RequestHistoricalSummary, RequestKnowledgeDiscovery, RequestRiskAssessment |
| **Relationship** | Create/Activate/Modify/ExpireRelationship |
| **Workspace Sync** | Create/Transfer/Validate/ApplySynchronizationPackage, RequestWorkingSet, ResolveSynchronizationConflict |

### Command Execution Lifecycle
```
Command → Validation → Authorization → Handler → Aggregate → Domain Events → Commit → Response
```
Failure at any stage prevents state modification.

### Command Naming Rules
- Begin with a verb; describe business intention; use business terminology
- Examples: RegisterAsset, InstallEngine, ReplaceMeter, CompleteMaintenance
- AVOID: SaveAsset, UpdateDatabase, ExecuteSQL, CallAPI

## 5.4 Queries

### Query Principles
- Read-only, side-effect free, NEVER modifies business state
- NEVER publishes Domain Events
- Optimized for read performance

### Query Structure
QueryId, QueryType, RequestedAt, RequestedBy, Filters, Paging (opt), Sorting (opt)

### Query Categories & Key Queries

| Category | Key Queries |
|----------|------------|
| **Asset** | GetAsset, SearchAssets, GetAssetHistory, GetAssetCurrentConfiguration, GetAssetTimeline, GetAssetDashboard |
| **Engine** | GetEngine, SearchEngines, GetEngineInstallationHistory, GetCurrentInstalledEngine, GetEngineRepair/UsageHistory |
| **Component** | GetComponent, SearchComponents, GetComponentHistory, GetInstalledComponents, GetReplacementHistory |
| **Meter** | GetCurrentMeter, GetMeterHistory, GetMeterReadings, GetOperational/NonOperationalUsage, GetUsageCorrections |
| **Maintenance** | GetMaintenancePlan, GetScheduledMaintenance, GetMaintenance/Inspection/Failure/Repair/OverhaulHistory, GetUpcomingMaintenance |
| **Financial** | GetPurchaseInformation, GetOperatingExpenses, GetFuelConsumptionCost, GetMaintenanceCost, GetDepreciation, GetCurrentAssetValue, GetOwnershipCost, GetFinancialTimeline |
| **Document** | GetDocument(s), GetExpiredDocuments, GetDocumentsExpiringSoon, GetDocumentVersions, GetDocumentPackage |
| **Forecast** | GetFuel/Lubricant/Maintenance/ReplacementForecast, CompareForecasts, GetForecastHistory |
| **Reporting** | GetExecutive/AssetDashboard, GetFleetStatistics, GetOperational/Financial/Maintenance/ForecastKPIs |
| **Admin** | GetUsers, GetRoles, GetOrganizations, GetLocations, GetAuditLog, GetSystemConfiguration |
| **Organization** | GetOrganization, GetOrganizationAssets |
| **Notification** | GetNotifications, GetNotification, GetNotificationPreferences |
| **Messaging** | GetConversations, GetConversation, GetMessages, GetMessageAttachments |
| **AI Assistant** | GetRecommendations, GetRecommendationExplanation, GetAIInteractionHistory |
| **Relationship** | GetRelationship, GetRelationshipsForEntity, GetRelationshipHistory |
| **Workspace Sync** | GetSynchronizationHistory, GetSynchronizationPackage, GetSynchronizationConflicts, GetWorkingSet |

### Cross-Module Queries
- GetCompleteAssetProfile: Asset + Engine + Components + Maintenance + Documents + Financial + Forecast
- GetOperationalSummary: Usage + Maintenance + Financial
- GetTechnicalSummary: Asset Model + Engine Model + Technical Library
- GetBusinessTimeline: Meter readings + Maintenance + Repairs + Financial + Documents + Engine replacements

### Query Naming Rules
- Begin with Get, Search, or Compare
- AVOID: ReadTable, ExecuteSQL, SelectRows, LoadEntity

## 5.5 Handlers

### Handler Principles
- Single Responsibility; One Handler per Command/Query
- Stateless; Depend only on abstractions
- Return Application Results; NEVER expose domain entities directly
- Business logic belongs to Aggregates and Domain Services

### Command Handler Responsibilities
Validate request → Verify authorization → Load Aggregate(s) → Invoke Aggregate behavior → Invoke Domain Services → Publish Domain Events → Commit transaction → Return result

### Query Handler Responsibilities
Validate request → Verify authorization → Retrieve read model → Project data → Return response

### Handler Naming Convention
- Command: `RegisterAssetHandler`, `InstallEngineHandler`
- Query: `GetAssetHandler`, `SearchAssetsHandler`

### Handler Dependencies (Allowed)
- Repository Interfaces, Domain Services, Unit of Work, Logger, Application Services

### Handler Dependencies (FORBIDDEN)
- Entity Framework, SQL, Infrastructure Implementations (direct)

### Transaction Rules
- Normally: one Command = one transaction = one commit
- Multiple Aggregates: consistency follows Domain rules (eventual)

## 5.6 Pipeline Behaviors (Cross-Cutting Concerns)

### Execution Order
```
Request → Logging → Validation → Authorization → Performance → Transaction → Handler → Commit → Response
```

### Mandatory for Commands
- Validation, Logging, Authorization, Transaction

### Optional for Queries
- Logging, Authorization, Performance

### NEVER for Queries
- Transaction

### Behavior Design Rules
- Reusable, stateless, never access UI
- Never access Infrastructure implementations directly
- NEVER contain business rules

## 5.7 Application Services

### When Required
- Multiple Aggregates participate
- Several Commands must execute together
- Infrastructure interactions required
- Workflow spans multiple modules
- Long-running business process

### Application Service Responsibilities
- Coordinate multiple Command/Query Handlers
- Invoke Domain Services
- Coordinate Infrastructure Services (through interfaces)
- Execute long-running workflows
- Publish integration events
- Transaction management (single/multiple/compensating)

### Application Service Naming
`[Domain]ApplicationService` (e.g., AssetApplicationService, MaintenanceApplicationService)

### Key Application Services
- AssetApplicationService: Purchase Used Asset, Dispose Asset, Transfer Asset
- EngineApplicationService: Install, Replace, Return from Workshop
- MeterApplicationService: Replace Meter, Validate Readings, Recalculate Usage
- MaintenanceApplicationService: Complete Maintenance, Replace Component, Register Overhaul
- FinancialApplicationService: Calculate Ownership Cost, Update Asset Value, Calculate Depreciation
- ForecastApplicationService: Generate Consumption/Maintenance Forecast, Compare Accuracy
- DocumentApplicationService: Register, Renew, Generate Expiration Notifications

## 5.8 Authorization Model

### Principles
- Role Based Access Control (RBAC)
- Least Privilege
- Business-oriented permissions
- Centralized authorization
- Auditable decisions

### Model
```
User → Role → Permission → Business Operation
```
- One User may have multiple Roles
- One Role may contain multiple Permissions

### Scope Hierarchy & SuperUser Model (RESOLVED, chat 2026-08-19)
Four authorization levels exist, corresponding to the tenant hierarchy (BR-017) plus the platform itself:

```
Platform → Holding → Organization → Project
```

- **SuperUser per level:** each level has at least one SuperUser (there MAY be more than one). A SuperUser has unrestricted access to everything within their level's scope (Platform SuperUser = System Administrator, unrestricted across the entire platform; Holding SuperUser = unrestricted across every Organization/Project under that Holding; and so on down to Project).
- **Partial-scope Administrators:** below the SuperUser of a level, there MAY also be Administrators with RESTRICTED access within that same level — e.g. a Holding-level Administrator who can only see a subset of the Holding's Organizations, or who lacks certain feature permissions (Financial, say) that the Holding SuperUser has. Being "an Administrator at a level" does NOT imply full access to everything at that level or below it.
- **Who grants access:** a User's specific permissions and specific scope (which Organizations/Projects, which features) at a given level are configured by the SuperUser of that same level, or by a SuperUser of any level ABOVE it. A User can never grant a scope or permission they do not themselves hold.
- **Project-level Users vs. Admin-tier Users (RESOLVED, chat 2026-08-19):** an ordinary Project-level User has exactly one CURRENT Project assignment at a time (see BR-017). A User who needs simultaneous (concurrent) access to more than one Project is, by definition, no longer a plain Project-level User — they are an Administrator (at Organization or Holding level, with a Project subset or full Project access configured as above), not a User with multiple simultaneous "current Project" values.
- **Bootstrap requirement:** the platform shall always have at least one Platform-level SuperUser (System Administrator) available — this is the Development-only seeded test user today (`sysadmin`); a proper Production bootstrap procedure is a Production-readiness open item (see ADR-0026 open item on certificates — bootstrap strategy should be documented alongside it).

### Standard Roles
- System Administrator = Platform SuperUser (unrestricted, across all Holdings/Organizations)
- Holding Administrator: SuperUser or partial-scope Administrator at Holding level (RESOLVED, chat 2026-08-19)
- Organization Administrator: SuperUser or partial-scope Administrator at Organization level (scoped to a single Organization/tenant; MAY be restricted to a subset of that Organization's Projects)
- Project Administrator: SuperUser or partial-scope Administrator at Project level (RESOLVED, chat 2026-08-19)
- Fleet Manager, Maintenance Manager, Maintenance Technician
- Workshop Supervisor, Operator, Financial Officer
- Procurement Officer, Document Controller, Read-Only Auditor
- (Fleet Manager through Read-Only Auditor are ordinary Project-level operational roles, each with exactly one current Project per the model above, unless explicitly elevated to an Administrator role)

### Permission Naming Convention
`[Domain].[Action]`

Examples:
- Asset.Create, Asset.Update, Asset.Transfer, Asset.Retire, Asset.Dispose
- Engine.Install, Engine.Remove, Engine.Replace, Engine.Rebuild
- Maintenance.Plan, Maintenance.Schedule, Maintenance.Start, Maintenance.Complete, Maintenance.Cancel
- Financial.View, Financial.RecordExpense, Financial.CalculateDepreciation
- Document.Upload, Document.Replace, Document.Archive
- Forecast.Generate, Forecast.View, Forecast.Compare
- User.Create, User.Disable, Role.Assign, Permission.Assign, Organization.Manage, Holding.Manage, Project.Manage
- Organization.View, Holding.View, Project.View (Phase 3 — Scope-based Filtering; consumed by GetAuthorizedScopesAsync, see 07-api or ADR referencing IPermissionEvaluator)

### Profiles (RESOLVED, chat 2026-08-19)
A **Profile** is a named, reusable bundle of Permissions (e.g. "Maintenance Technician — Project X") that a SuperUser (or higher-level SuperUser) can define once and assign to multiple Users, rather than assigning individual Permissions one by one. A Profile bundles Permissions only — it does NOT itself carry a scope (which Organizations/Projects); scope is assigned separately per User, so the same Profile can be reused for different Users across different scopes.

### Authorization Flow
```
Request → Authentication → Resolve User → Resolve Organization → Resolve Roles → Resolve Permissions → Evaluate Policy → Execute Handler
```

### Authorization Failure Rules
- Business state remains unchanged
- NO Domain Event published
- Attempt logged

### Audit Requirements
Every authorization-sensitive operation records: User, Time, Operation, Resource, Result, Source. Audit records are immutable.

+ ### Assignment Revocation (Phase 3, chat 2026-08-23)
+ A UserProfileAssignment is REVOKED, never deleted. Revocation sets
+ IsRevoked=true and RevokedAt (soft revocation), preserving the
+ immutable audit trail. IPermissionEvaluator excludes revoked
+ assignments from both HasPermissionAsync and GetAuthorizedScopesAsync
+ immediately (no caching), satisfying BR-017's "Access revocation on
+ reassignment" rule (Section 10.16).

---



# 6. DEVELOPMENT RULES & STANDARDS

## 6.1 Solution Structure

### Central Build Configuration
- `Directory.Build.props` — common MSBuild properties (TargetFramework, etc.)
- `Directory.Packages.props` — Central Package Management (CPM); all NuGet versions managed centrally
- Project files must NOT duplicate shared MSBuild properties or package versions
- Project files contain only: SDK selection, OutputType, UserSecretsId, Razor settings, ProjectReference, PackageReference

### Repository Root
```
/
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
```

### Dependency Direction (Inward)
```
Presentation (Web) → Application → Domain → SharedKernel
Infrastructure supports higher layers but NEVER the center
```

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

## 6.2 Project Internal Structure

### Standard Folders (create only when needed)
```
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
```

### Folder Creation Policy
- NEVER create empty folders
- Only create when: contains meaningful content, multiple files justify existence, improves organization

## 6.3 Namespace Conventions

### Root Namespace
`MachineryManagerEnterprise`

### Pattern
```
MachineryManagerEnterprise.[Layer/Module].[Feature].[Subcategory]
```

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
|----------|----------|
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
```
UI Tests (top, few)
Integration Tests (middle)
Unit Tests (base, most)
```

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
```
main
 └── develop
      └── feature/*
```

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
```
Need → Technology Evaluation (TE) → Proof of Concept (opt) → ADR → Approved → Directory.Packages.props → Implementation → Maintenance
```

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
```

## 8.6 Pagination, Filtering, Sorting

| Parameter | Default | Max |
|-----------|---------|-----|
| page | 1 | — |
| pageSize | 25 | 200 |

- Multiple filters: combined with **AND**
- Date range: `from=YYYY-MM-DD&to=YYYY-MM-DD`
- Sort: `sort=field` (asc), `sort=-field` (desc), multi: `sort=status,name`
- Search: `search=query`
- Empty results: `200 OK` with empty `items` array (NOT 404)

## 8.7 Error Response Structure
```json
{
  "errorCode": "BUS-014",
  "title": "Business Rule Violation",
  "message": "The selected engine is already installed.",
  "correlationId": "7f1f25f2-a8db-4e76-ae4d-4f8c16e4c08d",
  "details": []
}
```

### Error Categories & HTTP Mapping

| Category | Prefix | HTTP Status |
|----------|--------|-------------|
| Validation | VAL | 400 Bad Request |
| Authentication | AUTH | 401 Unauthorized |
| Authorization | AUTH | 403 Forbidden |
| Resource | RES | 404 Not Found |
| Business | BUS | 409 Conflict |
| Infrastructure | INF | 503 Service Unavailable |
| System | SYS | 500 Internal Server Error |

### Error Rules
- Consistent, deterministic, machine-readable, traceable
- NEVER expose internal implementation details
- Every error contains CorrelationId (matches logs, audit, traces)
- Error codes stable across localization

## 8.8 Authentication & Authorization

### Authentication
- Bearer Token in `Authorization: Bearer <token>` header
- HTTPS only; plain HTTP prohibited in production
- Token lifetime configurable; expired tokens rejected

### Authorization
- RBAC + Claims-based
- Flow: User → Identity → Roles → Permissions → Business Operation
- Multi-tenant: every request executes within exactly one Organization (tenant) context via `OrganizationId`
- Cross-tenant access prohibited unless explicitly authorized

### Endpoint Protection Matrix
| Type | Auth | AuthZ |
|------|:----:|:-----:|
| Public | ❌ | ❌ |
| Authenticated | ✅ | ❌ |
| Protected | ✅ | ✅ |
| Administrative | ✅ | ✅ (Admin Permission) |

### Audit Logging
- Log: successful/failed auth, authZ failures, permission changes, user lockout, admin access
- NEVER log: passwords, tokens, secrets, connection strings, PII, payment info

## 8.9 OpenAPI Specification

### Generation Pipeline
```
Source Code → XML Comments → OpenAPI Generator → openapi.json → Scalar (primary UI) → NSwag (client gen)
```

### Rules
- OpenAPI 3.x, generated automatically from source code
- Manual editing of generated docs PROHIBITED
- Scalar = primary interactive documentation UI
- Swagger UI = backward compatibility ONLY (per ADR-0035)
- NEVER expose: Domain Entities, EF Core Entities, Internal Models, Infrastructure Types
- Business-oriented tags (Assets, Engines, Maintenance), NOT technical tags
- Every endpoint documents: Summary, Description, Parameters, Request Body, Response Model, Error Responses, Auth, Examples
- Deprecated endpoints marked with replacement endpoint
- Build pipeline validates OpenAPI spec validity

## 8.10 API Lifecycle

```
Design → Development → Review → Release → Maintenance → Deprecation → Retirement
```

- No stage skipped
- Design approval BEFORE implementation
- Release requires: complete docs, passing tests, valid OpenAPI, security review, build success
- Breaking changes never in existing version
- Deprecation: notice + replacement + planned removal + sunset date
- Retirement only after: deprecation period elapsed, consumers notified, replacement exists

## 8.11 Long-Running & Async Operations
- Return `202 Accepted` with OperationId, Status, Location header
- Clients poll status endpoint for completion

## 8.12 Content Types
- Request/Response: `application/json`
- Future versions may support additional media types

## 8.13 HATEOAS
- Initial API: NOT required
- Future versions may introduce hypermedia links if required (backward compatible)

---



# 9. RELEASE & DEPLOYMENT

## 9.1 Versioning Policy
- **Semantic Versioning:** `MAJOR.MINOR.PATCH`
- Application version independent from API version
- Git tags for every production release: `v{major}.{minor}.{patch}`
- Build identification: Version + Build Number + Commit SHA + Build Timestamp
- Pre-release identifiers: `-alpha`, `-beta`, `-rc1`

### Version Change Rules
| Change | Version Bump |
|--------|-------------|
| Breaking API / Domain Contract / Architectural Redesign | Major |
| New Feature / New Module / New Endpoint | Minor |
| Bug Fix / Security Fix / Performance / Documentation | Patch |

## 9.2 Release Types

| Type | Content | Example |
|------|---------|---------|
| **Major** | New business capabilities, architectural evolution, breaking API changes | 2.0.0 |
| **Minor** | New features, backward-compatible improvements, new modules | 1.4.0 |
| **Patch** | Bug fixes, security fixes, performance improvements (NO new functionality) | 1.4.3 |
| **Hotfix** | Critical production defects only; minimal unrelated changes | 1.4.4 |

## 9.3 Release Lifecycle

```
Planning → Development → Testing → Validation → Release Candidate → Production Release → Maintenance → End of Support
```

- No stage skipped
- Incomplete features NEVER released
- Every release reproducible from source control

## 9.4 Release Process

```
Development → Feature Complete → Code Freeze → Release Candidate → Validation → Approval → Production Deployment → Monitoring
```

### Code Freeze Rules
- New features prohibited
- Only approved bug fixes, documentation corrections, config adjustments
- Version numbers finalized

### Release Candidate (RC)
- Feature complete, deployable, production-like
- Only bug fixes, doc corrections, config adjustments permitted
- Example: `2.0.0-rc1`

### Validation Gates (ALL must pass)
- Build verification
- Automated testing (Unit + Integration + Functional + Architecture + Regression)
- Manual business verification
- Documentation review
- Security review

### Release Approval Required From
- Technical Approval
- Architecture Approval
- Business Approval (when applicable)

### Production Deployment Rules
- Use approved artifacts ONLY
- Use tagged source code
- Manual source modifications PROHIBITED
- Post-deployment verification: Application startup, API availability, DB connectivity, Background jobs, Critical business workflows

## 9.5 Deployment Strategy

### Deployment Pipeline
```
Build → Package → Publish Artifact → Deploy → Verify → Monitor
```

### Environments
```
Development → Testing → Staging → Production
```
- Each environment isolated
- Same artifact promoted through environments (never rebuilt)

### Configuration Rules
- Configuration external to application binaries
- NEVER hardcoded: connection strings, API endpoints, storage, cache, logging config
- Secrets managed via approved secure storage (HashiCorp Vault / Azure Key Vault)
- Secrets NEVER in: Source Control, Build Artifacts, Application Source Code

### Database Deployment
- ONLY approved migrations (EF Core Migrations per ADR-0037)
- Manual production schema changes PROHIBITED
- Migration execution version controlled
- Backup procedure verified before deployment
- Rollback procedure prepared before deployment

### Downtime Policy
- Minimize service interruption
- Strategies: Rolling deployment, Blue-Green deployment, Zero-downtime migration
- Selected strategy depends on infrastructure capabilities and service criticality

### Rollback Strategy
- Every deployment MUST have rollback plan
- Rollback includes: previous app version, previous config, DB recovery strategy
- Rollback procedures documented, validated, rehearsed BEFORE production deployment
- Rollback triggers: critical defects, deployment failure, data integrity threat

### Deployment Audit
Every deployment records: Version, Build Number, Git Commit, Deployment Time, Environment, Operator (if manual)

## 9.6 Support Lifecycle

```
Released → Supported → Maintenance → Deprecated → End of Support → Archived
```

### Support Matrix

| Stage | Bug Fix | Security | Documentation |
|-------|:-------:|:--------:|:-------------:|
| Supported | ✅ | ✅ | ✅ |
| Maintenance | ✅ | ✅ | ✅ |
| Deprecated | ❌ | ✅ | ✅ |
| End of Support | ❌ | ❌ | ❌ |
| Archived | ❌ | ❌ | ❌ |

### Rules
- Security updates ONLY for supported versions
- Deprecated versions remain functional during support period
- End of Support = no further updates; clients must migrate
- Archived = historical only, no maintenance

### Upgrade Policy
- Patch → Patch: no changes required
- Minor → Minor: backward compatible
- Major → Major: may require migration

## 9.7 Release Checklist (Mandatory Gates)

| Gate | Required |
|------|:--------:|
| Source Control (committed, PRs merged, tag prepared) | ✅ |
| Build Success (zero errors, zero critical warnings, static analysis pass) | ✅ |
| Tests Passed (Unit + Integration + Functional + Architecture + Regression) | ✅ |
| Documentation Updated (API docs, architecture docs, release notes, migration) | ✅ |
| Database Ready (migrations reviewed, scripts validated, backup verified) | ✅ |
| Security Verified (secrets configured, certificates valid, auth/authZ verified, security review) | ✅ |
| Configuration Reviewed (env config, connection strings, endpoints, logging, monitoring) | ✅ |
| Deployment Package Prepared | ✅ |
| Rollback Prepared (previous release available, instructions reviewed, backup completed) | ✅ |
| Approval Granted (Technical + Architecture + Business) | ✅ |
| Health Check Passed (post-deployment) | ✅ |

## 9.8 Release Notes Template (Required Sections)
- Release Information (Version, Date, Type, Git Tag, Build Number)
- Summary
- New Features
- Improvements
- Bug Fixes
- Breaking Changes (or "None")
- Database Changes (or "None")
- API Changes (or "None")
- Security Updates (or "None")
- Migration Notes (or "No migration steps required")
- Known Issues (or "None")
- Upgrade Recommendation
- Support Information
- References (Checklist, Deployment Report, Build Number, Git Tag)

---



# 10. BUSINESS SPECIFICATIONS (BR)

## 10.1 BR Governance
- 17 Business Specifications total, all Approved
- Each BR defines one business capability with: Purpose, Scope, Business Definitions, Lifecycle, Rules, Constraints, Acceptance Criteria
- BRs are authoritative source for business rules; AI must NEVER invent rules outside BRs

## 10.2 BR-003 — Asset Relationships
- **Purpose:** Define how Assets relate to other business objects (Projects, Organizations, Components, Documents)
- **Key Rules:**
  - Every Asset has exactly one owning Organization
  - Asset-Project relationship is temporary and historical
  - Asset hierarchy (parent/child) permitted; cycles prohibited
  - Asset relationships never transfer Asset identity
  - Historical relationships preserved; never overwritten

## 10.3 BR-004 — Tracked Components
- **Purpose:** Manage components with independent lifecycle (Engine, Transmission, Tire, Battery, Hydraulic Attachment)
- **Key Rules:**
  - Component is independent business object, not property of Asset
  - One Component may serve multiple Assets over time
  - Component may exist without being installed (warehouse, repair)
  - Component identity unchanged after rebuild; rebuild = maintenance history
  - Replacement preserves: Removed, Installed, Date, Technician, Maintenance Operation
  - Installation position belongs to installation event, not component
  - Current position = derived from latest active installation

## 10.4 BR-005 — Tire Lifecycle
- **Purpose:** Track tire as a Tracked Component with specific lifecycle rules
- **Key Rules:**
  - Tire has independent identity and lifecycle
  - Tire may be installed/removed/transferred/replaced/rebuilt/retired
  - Tire replacement preserves complete installation history
  - Tire position (Front Left, Rear Right, etc.) belongs to installation event
  - Tire lifecycle updated ONLY through Maintenance Operations

## 10.5 BR-006 — Battery Lifecycle
- **Purpose:** Track battery as a Tracked Component with specific lifecycle rules
- **Key Rules:**
  - Battery has independent identity and lifecycle
  - Battery may be installed/removed/transferred/replaced/rebuilt/retired
  - Battery replacement preserves complete installation history
  - Battery health/age may generate Condition-Based Forecasts
  - Battery lifecycle updated ONLY through Maintenance Operations

## 10.6 BR-007 — Parts Catalog
- **Purpose:** Manage spare parts definitions, specifications, and compatibility
- **Key Rules:**
  - Part has permanent identity; definitions immutable after approval
  - Part belongs to exactly one Part Category
  - Part may have multiple manufacturers and suppliers
  - Part specifications (dimensions, weight, material) are reference data
  - Part Catalog does NOT track inventory quantities (that is Inventory Management)

## 10.7 BR-008 — Part Cross Reference
- **Purpose:** Define equivalent, alternative, and replacement relationships between Parts
- **Key Rules:**
  - Cross-reference relationships are bidirectional and symmetric
  - Cross-reference NEVER implies automatic substitution approval
  - Part A equivalent to Part B = both satisfy same functional requirement
  - Replacement relationship preserves historical compatibility
  - Cross-references are reference data; operational substitution requires approval

## 10.8 BR-009 — Incident Management
- **Purpose:** Record, classify, investigate, and resolve unexpected operational events
- **Lifecycle:** Reported → Validated → Classified → Assigned → Under Investigation → Decision → Resolved → Closed
  - Alt: Reported/Validated → Rejected; Closed → Reopened → Under Investigation
  - Forbidden: Skip mandatory states; Closed modified directly
- **Key Rules:**
  - Incident = any unexpected event affecting Assets, Components, Personnel, Environment
  - Every Incident has one primary classification (Mechanical, Electrical, Safety, Environmental, Operational, Security)
  - Severity (business impact) and Priority (response speed) are independent
  - Investigation produces Corrective Actions; Corrective Actions are business outcomes, NOT maintenance operations
  - Creating Corrective Action does NOT automatically execute it
  - Closed Incidents = read-only; reopening creates new lifecycle transition
  - Investigation history, evidence, root cause preserved permanently
  - One Incident may generate multiple Corrective Actions (Maintenance, Training, Safety, etc.)
  - Corrective Action completion does NOT automatically close Incident

## 10.9 BR-010 — Maintenance Forecast
- **Purpose:** Predict future maintenance needs before operational failures occur
- **Forecast Types:** Preventive (deterministic), Predictive (probabilistic), Condition-Based (threshold), Regulatory, Manufacturer, AI
- **Lifecycle:** Generated → Validated → Approved → Scheduled → Consumed → Completed
  - Alt: Generated/Validated/Approved/Scheduled → Cancelled
- **Key Rules:**
  - Forecast is prediction, NOT evidence of current requirement
  - Forecasts NEVER execute maintenance; they support planning decisions
  - Only Approved Forecasts may participate in maintenance planning
  - Multiple Forecasts may exist simultaneously for same object (independent)
  - Forecast generation NEVER modifies historical operational records
  - Forecast confidence level included (Very High/High/Medium/Low/Unknown)
  - Forecast completion preserves original prediction + actual execution for accuracy analysis
  - Forecasts may be consumed by: Maintenance Planning, Procurement Planning, Shutdown Planning, Fleet Planning
  - Rejected Forecasts preserve rejection reason
  - Expired Forecasts remain historical records

## 10.10 BR-011 — Maintenance Operations
- **Purpose:** Execute approved maintenance work; authoritative source of operational history
- **Lifecycle:** Requested → Planned → Approved → Scheduled → Started → In Progress → Completed → Verified → Closed
  - Alt: Requested/Planned/Approved/Scheduled → Cancelled
  - Alt: Started/In Progress → Suspended → In Progress (resumed)
  - Forbidden: Skip mandatory stages; Completed→InProgress; Closed modified
- **Key Rules:**
  - Maintenance Operation = controlled execution of ONE approved maintenance activity
  - Every completed Maintenance Operation has at least one Activity
  - Activities: Inspection, Cleaning, Adjustment, Calibration, Lubrication, Repair, Replacement, Installation, Removal, etc.
  - Findings = observed conditions; may generate Recommendations/Forecasts/Additional Work Orders
  - Measurements preserve historical values; never overwrite previous observations
  - Component Changes (Install/Remove/Replace/Relocate) owned by Maintenance Operation
  - Installation records: Component, Asset, Position, Date, Time, Technician, Reason, Maintenance Operation
  - Replacement = two linked events (Remove Old + Install New), permanently linked
  - Position belongs to installation event, NOT component
  - Labor records: Person, Role, Skill, Start/Finish Time, Hours, Cost
  - Inventory consumption: Item, Quantity, Unit, Warehouse, Cost, linked to Maintenance Operation
  - External services: Supplier, Type, Invoice, Cost, Duration, Warranty
  - Downtime: Start/Finish Time, Duration, Category, Planned/Unplanned, Reason
  - Financial impact: Labor + Parts + External Services + Transportation + Consumables + Miscellaneous
  - Operational Result: Successfully Repaired, Temporarily Repaired, Replaced, Inspected, Tested, No Fault Found, Deferred
  - Closed Maintenance Operations are immutable
  - Component lifecycle updated ONLY through Maintenance Operations (direct modification prohibited)
  - Maintenance Operation is Aggregate Root; owns: Activities, Findings, Measurements, Labor, Inventory, Downtime, Component Changes

## 10.11 BR-012 — Notification Center
- **Purpose:** Distribute business events to appropriate recipients through appropriate channels
- **Notification Types:** Informational, Operational, Reminder, Alert, Escalation, Approval, Incident, Forecast, Maintenance, System
- **Lifecycle:** Created → Queued → Delivered → Viewed → Acknowledged → Archived
  - Alt: Created → Cancelled
  - Reminder and Escalation cycles create additional history, never modify original
- **Key Rules:**
  - Notification Center communicates business events; NEVER creates or modifies business events
  - Every Notification references exactly one originating business event
  - Same business event may generate multiple Notifications (Technician, Supervisor, Manager)
  - Recipient Resolution consumes business relationships (never hard-coded, never duplicated)
  - Delivery Channels: Dashboard, Mobile Push, Email, SMS, Internal Messaging, Voice
  - Delivery failure NEVER deletes Notification; may trigger Retry or Alternate Channel
  - Delivery does NOT imply awareness; only Viewed/Acknowledged confirm recipient interaction
  - Archived Notifications = permanent historical records
  - Notification lifecycle independent from originating business object lifecycle
  - Hierarchical propagation: Project User → Project Admin → Enterprise Admin → Super Admin
  - Escalation appends higher-level recipients, never replaces original
  - Duplicate prevention governed by business policy
  - Quiet Hours configurable; Critical may bypass

## 10.12 BR-013 — Internal Messaging
- **Purpose:** Human-to-human business communication with complete traceability
- **Conversation Types:** Direct, Group, Business Context, Temporary, Permanent, Broadcast
- **Message Lifecycle:** Created → Sent → Delivered → Read → Archived (or Soft Deleted)
- **Key Rules:**
  - Every Message belongs to exactly one Conversation
  - Conversation may reference Business Objects (Asset, Work Order, Incident, etc.) for context only
  - Closing Conversation NEVER closes referenced business object
  - Messages are immutable historical records; permanent deletion prohibited
  - Editing creates version history; original preserved
  - Soft Delete = presentation action only; organizational history preserved
  - Read status independent per recipient
  - Attachments belong to Messages; never exist independently
  - Forwarding creates new Message; original unchanged
  - Business Context References are read-only navigation
  - Messages NEVER modify business state
  - Messages NEVER approve/authorize/execute business operations
  - Internal Messaging and Notification Center are independent capabilities
  - Participant resolution consumes Relationship Management (never duplicates hierarchy)

## 10.13 BR-014 — AI Assistant
- **Purpose:** Provide intelligent advisory assistance across all business domains
- **Core Principle:** Intelligence without Authority. AI observes, reasons, recommends. NEVER approves, rejects, executes, modifies.
- **AI Capabilities:** Knowledge Discovery, Business Q&A, Historical Summarization, Recommendation Generation, Pattern Recognition, Risk Identification, Cross-Capability Analysis, Explanation, Navigation Assistance, Learning Assistance
- **Key Rules:**
  - AI Assistant is advisory ONLY; business authority always belongs to humans
  - AI NEVER modifies business state (Assets, Components, Parts, Incidents, Forecasts, Maintenance Operations, Notifications, Messages, Relationships)
  - AI NEVER approves/rejects/authorizes/executes business operations
  - AI NEVER creates business events
  - AI NEVER fabricates business facts; unknown = explicitly stated as unknown
  - Every recommendation must be explainable with supporting evidence
  - AI consumes authorization decisions; NEVER defines authorization
  - AI may recommend notifications but NEVER sends them
  - AI may participate in conversations when explicitly invoked but NEVER initiates independently
  - AI may improve forecasting quality but NEVER creates/modifies/approves forecasts
  - Recommendation language: "Suggested", "Recommended", "Consider", "Possible"
  - Prohibited language: "Must", "Required", "Approved", "Authorized", "Completed" (unless quoting existing record)
  - Recommendations may be stored as historical advisory records; never become operational history
  - Expired Recommendations never influence future decisions
  - Model improvement affects future recommendations only; never changes historical business truth
  - Business accountability NEVER assigned to AI
  - Every approved business action identifies responsible human participant

## 10.14 BR-015 — Relationship Management
- **Purpose:** Create, manage, validate, and preserve business relationships between entities
- **Relationship Types:** Ownership, Hierarchical, Assignment, Installation, Replacement, Equivalence, Dependency, Reference, Communication, Advisory
- **Relationship Lifecycle:** Draft → Active → Modified → Expired → Historical
  - Forbidden: Historical → Active/Modified
- **Key Rules:**
  - Every relationship has its own identity, lifecycle, history, business meaning
  - Relationships are independent from connected business entities
  - Relationship ownership NEVER transfers ownership of connected entities
  - Relationship Management owns relationships only; NEVER owns Assets, Parts, Components, Incidents, Forecasts, Maintenance Operations, Notifications, Conversations
  - Relationship Management NEVER executes business operations
  - Relationship propagation occurs ONLY through Domain Events
  - Historical relationships immutable; never overwritten or deleted
  - Only Active Relationships participate in authorization, navigation, propagation
  - Hierarchical relationships: acyclic, single parent per child
  - Ownership propagation follows hierarchy; never transfers aggregate ownership
  - Circular hierarchy prohibited
  - Every relationship validated before activation (structural, ownership, hierarchy, type, authorization, temporal, dependency, consistency)
  - Rejected relationships remain non-operational
  - Relationship metadata (Effective Date, Expiration Date, Reason, Status) belongs to relationship only

## 10.15 BR-016 — Distributed Workspace Synchronization
- **Purpose:** Enable business operations to continue without connectivity while preserving enterprise consistency
- **Workspace Hierarchy:** Enterprise → Project → User (synchronization only between adjacent levels)
- **Terminology reconciliation (RESOLVED, chat 2026-08-19):** "Enterprise" in this hierarchy is the same entity as **Organization** (BR-017), not Holding. Holding does NOT participate in Workspace Synchronization — Holding is a purely administrative grouping above Organization (see BR-017) and has no sync authority, Working Set, or offline concerns of its own. The synchronization hierarchy remains exactly two hops: Organization ("Enterprise") → Project → User.
- **Key Rules:**
  - Business execution continues regardless of synchronization availability
  - Synchronization occurs ONLY after successful business validation
  - Synchronization is bidirectional: Upstream (User→Project→Enterprise) and Downstream (Enterprise→Project→User)
  - Direct User→Enterprise synchronization PROHIBITED
  - Only validated business changes exchanged; incremental (no full history retransmission)
  - Synchronization timing does NOT alter business behavior
  - Synchronization Packages: immutable, atomic, traceable, idempotent
  - Partial package application PROHIBITED
  - Working Set = minimum info required for user's responsibilities; responsibility-driven, minimal, refreshed after sync
  - Completed operations may be removed from User Workspace after sync; preserved in higher levels
  - Conflict Resolution: Business Rules determine outcome; timestamp-based resolution PROHIBITED
  - Automatic resolution only when deterministic; non-deterministic = manual review
  - Monotonic values (Hour Meter, Odometer) never decrease
  - Completed operations never become incomplete
  - Enterprise = permanent custodian of business history
  - Project = operational authority for project activities
  - User = responsible only for personal operational activities
  - Primary Synchronization Authority per Project; only this authority may sync Project→Enterprise
  - Anonymous synchronization PROHIBITED
  - Every sync session generates audit record
  - 10 synchronization scenarios defined (Online, Offline, Package Delivery, Consolidation, Distribution, Long Offline, Simultaneous, Device Replacement, Project Closure, Enterprise Recovery)

## 10.16 BR-017 — Organization Management (Tenant Hierarchy: Holding → Organization → Project)
- **Purpose:** Define the tenant hierarchy, and the ownership vs. operational-assignment split within it, as the authorization scope boundary
- **Tenant Hierarchy (RESOLVED, chat 2026-08-19 — replaces the former "sub-organizations" open question):**
  - **Holding:** optional top-level tenant grouping — a collection of one or more Organizations under common administrative oversight. An Organization MAY exist without belonging to any Holding (standalone tenant).
  - **Organization:** business entity (company/operating unit). Distinct from Company = manufacturer brand. Remains THE authorization scope boundary and THE sole owner of Assets, Personnel, and Warehouse Inventory — this did not change.
  - **Project:** the operational tier beneath Organization. A Project belongs to exactly one Organization. Projects are where Assets, Personnel, and Warehouse Inventory are currently operationally active — but Projects do NOT own them (ownership stays at Organization level; see "Ownership vs. Current Assignment" below).
- **Ownership vs. Current Assignment (RESOLVED, chat 2026-08-19):**
  - Every Asset has exactly one owning Organization (unchanged).
  - Assets, Personnel, and Warehouse Inventory each have (a) a permanent owning Organization, and (b) a CURRENT Project assignment that may change over time.
  - Reassignment between Projects (of the same Organization) is a normal operational event, not an ownership transfer.
  - Historical Usage/Maintenance/Activity records remain permanently scoped to whichever Project was current at the time each record was created. This scoping is immutable and is never retroactively updated when the resource is later reassigned to a different Project — consistent with the Historical Entities principle (append-only; see Section 4.2).
  - The owning Organization has standing access to all historical records across all of its Projects, past and present.
  - **Warehouse facility vs. Inventory (RESOLVED, chat 2026-08-19):** a Warehouse (the physical facility) is fixed to a single Project — it does not itself move between Projects. Inventory items stored within a Warehouse are owned by the Organization and MAY be moved between Warehouses, whether within the same Project or across different Projects of the same Organization.
- **Authorization Scope Rules:**
  - Organization-scoped permissions evaluated within a single resolved Organization.
  - Project-scoped permissions evaluated within a single resolved Project (RESOLVED, chat 2026-08-19).
  - Holding-scoped permissions span all Organizations under that Holding (RESOLVED, chat 2026-08-19).
  - Asset shall not exist without owning Organization.
  - Authorization checks shall not evaluate across Organization boundaries — the sole exception is a Holding Administrator, who is authorized across all Organizations within their Holding (RESOLVED, chat 2026-08-19).
  - **Access revocation on reassignment (RESOLVED, chat 2026-08-19):** when a User's current Project assignment changes, access to the PREVIOUS Project's data is revoked immediately and does not persist. A User promoted to Organization Administrator gains scope across all Projects of that Organization; a User promoted to Holding Administrator gains scope across all Organizations (and their Projects) of that Holding.
  - Scope resolution (which Organizations/Projects a User currently has access to) is evaluated dynamically against current assignment state at request time — NOT cached or baked into a long-lived token, so that revocation and reassignment take effect immediately (RESOLVED, chat 2026-08-19).
- **Cross-Organization Asset Transfer (RESOLVED, chat 2026-08-19):**
  - An Asset MAY be transferred from one Organization to a different Organization (e.g. a sale). This is distinct from — and independent of — a Project reassignment within the same Organization.
  - All historical records (Usage, Maintenance, ownership history, etc.) created BEFORE the transfer date remain permanently visible to BOTH the source (origin) Organization and the destination Organization.
  - Records created AFTER the transfer date belong exclusively to the destination Organization; the source Organization loses access to these new records.
  - **Re-acquisition case:** an Asset MAY later be transferred back to an Organization that previously owned it (e.g. Org A sells to Org B, then later reacquires it from Org B). The transfer mechanism shall be idempotent with respect to previously archived history: re-linking an Asset to a former owning Organization shall NOT duplicate, rewrite, or conflict with that Organization's pre-existing historical records for that Asset — the prior history is already present and is simply reconnected, not recreated.
  - Each transfer event itself is recorded as an immutable history entry (which Organization → which Organization, when), consistent with the existing "Asset ownership history" rule.
- **Organization Suspension (RESOLVED, chat 2026-08-19):**
  - When an Organization is suspended, all of its historical records remain intact and are NOT deleted.
  - If a suspended Organization's Assets are subsequently transferred (sold) to another Organization — whether within the same Holding or not — the Cross-Organization Asset Transfer rule above applies identically: history splits at the transfer date, both Organizations retain access to pre-transfer history, and re-acquisition remains idempotent.
- **Open questions (not yet decided):**
  - Full Organization lifecycle states beyond Suspension (e.g. permanent closure/dissolution) and what happens to any Assets that are never transferred out of a closed Organization.

---

# 11. PROOF OF CONCEPTS

## 11.1 POC-0001 — Jalali Support for MudBlazor DatePicker
- **Status:** Approved
- **Objective:** Evaluate MudBlazor DatePicker supporting Persian (Jalali) calendar without additional UI library
- **Hypothesis:** MudBlazor remains sole UI framework; Jalali via adapter/conversion layer
- **Success Criteria:** Jalali display correct, RTL support, Persian localization, DateOnly/DateTime conversion, FluentValidation compatibility, keyboard navigation, browser compatibility (Edge/Chrome/Firefox), no noticeable latency
- **Fallback (if fails):** Blazor.PersianDatePicker
- **Outcome:** If PASS → Create ADR → Implement MudBlazor Jalali Adapter; If FAIL → Create ADR → Select Alternative

---

# END OF DOCUMENT
## MachineryManagerEnterprise — AI Engineering Reference
## Complete: All 11 Sections
## Source: https://github.com/ammerman01-droid/MachineryManagerEnterprise
## Generated: 2026-08-17
## Purpose: Single Source of Truth for AI assistants. Contains only final decisions, rules, and structures.

