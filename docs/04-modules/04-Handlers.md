# Handlers

**Document ID:** MME-MOD-004

**Repository Path:** `docs/04-modules/04-Handlers.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-ApplicationArchitecture.md
- 01-UseCases.md
- 02-Commands.md
- 03-Queries.md
- docs/03-domain/05-DomainServices.md
- docs/03-domain/06-DomainEvents.md

---

# 1. Purpose

This document defines the responsibilities of Command Handlers and Query Handlers.

Handlers coordinate execution of Application requests.

Handlers do not contain business rules.

---

# 2. Handler Principles

Every Handler shall satisfy the following principles.

- Single Responsibility
- One Handler per Command
- One Handler per Query
- Technology independent
- Stateless
- Thin orchestration layer

Business logic belongs to Aggregates and Domain Services.

---

# 3. Handler Categories

```text
Handlers

├── Command Handlers
└── Query Handlers
```

---

# 4. Command Handler Responsibilities

A Command Handler shall:

- validate application request
- verify authorization
- load Aggregate(s)
- invoke Aggregate behavior
- invoke Domain Services when required
- publish Domain Events
- commit transaction
- return execution result

A Command Handler shall never implement business rules.

---

# 5. Query Handler Responsibilities

A Query Handler shall:

- validate request
- verify authorization
- retrieve read model
- project data
- return response

A Query Handler shall never modify business state.

---

# 6. Command Handler Lifecycle

```text
Receive Command

↓

Authorization

↓

Application Validation

↓

Load Aggregate

↓

Execute Domain Behavior

↓

Collect Domain Events

↓

Commit Transaction

↓

Publish Events

↓

Return Result
```

---

# 7. Query Handler Lifecycle

```text
Receive Query

↓

Authorization

↓

Validate Query

↓

Read Model

↓

Projection

↓

Return Result
```

---

# 8. Asset Command Handlers

| Command | Handler |
|----------|---------|
| RegisterAsset | RegisterAssetHandler |
| UpdateAssetInformation | UpdateAssetInformationHandler |
| TransferAsset | TransferAssetHandler |
| RetireAsset | RetireAssetHandler |
| DisposeAsset | DisposeAssetHandler |

---

# 9. Engine Command Handlers

| Command | Handler |
|----------|---------|
| RegisterEngine | RegisterEngineHandler |
| InstallEngine | InstallEngineHandler |
| RemoveEngine | RemoveEngineHandler |
| ReplaceEngine | ReplaceEngineHandler |
| SendEngineToWorkshop | SendEngineToWorkshopHandler |
| ReturnEngineFromWorkshop | ReturnEngineFromWorkshopHandler |
| RegisterEngineRebuild | RegisterEngineRebuildHandler |

---

# 10. Component Command Handlers

| Command | Handler |
|----------|---------|
| RegisterComponent | RegisterComponentHandler |
| InstallComponent | InstallComponentHandler |
| RemoveComponent | RemoveComponentHandler |
| ReplaceComponent | ReplaceComponentHandler |
| RetireComponent | RetireComponentHandler |

---

# 11. Meter Command Handlers

| Command | Handler |
|----------|---------|
| InstallMeter | InstallMeterHandler |
| ReplaceMeter | ReplaceMeterHandler |
| RegisterMeterReading | RegisterMeterReadingHandler |
| RegisterNonOperationalUsage | RegisterNonOperationalUsageHandler |
| CorrectMeterReading | CorrectMeterReadingHandler |
| ArchiveMeter | ArchiveMeterHandler |

---

# 12. Maintenance Command Handlers

| Command | Handler |
|----------|---------|
| CreateMaintenancePlan | CreateMaintenancePlanHandler |
| ScheduleMaintenance | ScheduleMaintenanceHandler |
| StartMaintenance | StartMaintenanceHandler |
| CompleteMaintenance | CompleteMaintenanceHandler |
| RegisterFailure | RegisterFailureHandler |
| RegisterRepair | RegisterRepairHandler |
| RegisterInspection | RegisterInspectionHandler |
| RegisterOverhaul | RegisterOverhaulHandler |

---

# 13. Financial Command Handlers

| Command | Handler |
|----------|---------|
| RegisterAssetPurchase | RegisterAssetPurchaseHandler |
| RegisterOperatingExpense | RegisterOperatingExpenseHandler |
| RegisterFuelExpense | RegisterFuelExpenseHandler |
| RegisterMaintenanceExpense | RegisterMaintenanceExpenseHandler |
| RegisterInsuranceExpense | RegisterInsuranceExpenseHandler |
| RegisterTaxExpense | RegisterTaxExpenseHandler |
| CalculateDepreciation | CalculateDepreciationHandler |
| RecalculateAssetValue | RecalculateAssetValueHandler |
| RecalculateOwnershipCost | RecalculateOwnershipCostHandler |

---

# 14. Document Command Handlers

| Command | Handler |
|----------|---------|
| RegisterDocument | RegisterDocumentHandler |
| UploadDocumentImage | UploadDocumentImageHandler |
| UploadDocumentFile | UploadDocumentFileHandler |
| ReplaceDocumentVersion | ReplaceDocumentVersionHandler |
| RenewDocument | RenewDocumentHandler |
| ArchiveDocument | ArchiveDocumentHandler |

---

# 15. Forecast Command Handlers

| Command | Handler |
|----------|---------|
| GenerateFuelForecast | GenerateFuelForecastHandler |
| GenerateMaintenanceForecast | GenerateMaintenanceForecastHandler |
| GenerateReplacementForecast | GenerateReplacementForecastHandler |
| RefreshForecastModels | RefreshForecastModelsHandler |

---

# 16. Query Handlers

Each Query has exactly one Query Handler.

Examples:

| Query | Handler |
|--------|---------|
| GetAsset | GetAssetHandler |
| SearchAssets | SearchAssetsHandler |
| GetAssetHistory | GetAssetHistoryHandler |
| GetEngine | GetEngineHandler |
| GetMaintenanceHistory | GetMaintenanceHistoryHandler |
| GetCurrentAssetValue | GetCurrentAssetValueHandler |
| GetDocumentPackage | GetDocumentPackageHandler |
| GetFuelForecast | GetFuelForecastHandler |

---

# 17. Aggregate Interaction Rules

A Handler may:

- load one Aggregate
- load multiple Aggregates when required
- invoke Domain Services
- invoke Infrastructure Services through abstractions

A Handler shall never modify Aggregate state directly.

---

# 18. Transaction Rules

Normally:

- one Command
- one transaction
- one commit

If multiple Aggregates participate, consistency shall follow Domain rules.

---

# 19. Error Handling

Handlers shall translate:

- Validation failures
- Authorization failures
- Domain exceptions
- Concurrency exceptions
- Infrastructure exceptions

into Application Results.

---

# 20. Naming Convention

Command Handler

```
<CommandName>Handler
```

Examples

- RegisterAssetHandler
- InstallEngineHandler
- ReplaceMeterHandler

Query Handler

```
<QueryName>Handler
```

Examples

- GetAssetHandler
- SearchAssetsHandler
- GetMaintenanceHistoryHandler

---

# 21. Future Handlers

Future releases may introduce handlers for:

- Inventory
- Procurement
- Fleet Scheduling
- AI Diagnostics
- IoT Synchronization
- Background Jobs

All future handlers shall follow the principles defined in this document.

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Handler Architecture |