| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | MOD-004            |
| **Title**        | Handlers Architecture |
| **Version**      | 4.7.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the responsibilities of Command Handlers and Query Handlers.

Handlers coordinate execution of Application requests.

Handlers do not contain business rules.

---

# Handler Philosophy

Handlers coordinate execution.

Handlers orchestrate application flow.

Handlers never implement business rules.

Business behavior always belongs to:

- Aggregates
- Domain Services

Handlers connect the Application Layer to the Domain Layer.

---

# Handler Design Rules

Every Handler shall:

- Handle exactly one request.
- Be stateless.
- Depend only on abstractions.
- Return Application Results.
- Never expose domain entities directly.

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
| RequestMaintenance | RequestMaintenanceHandler |
| CreateMaintenancePlan | CreateMaintenancePlanHandler |
| ApproveMaintenancePlan | ApproveMaintenancePlanHandler |
| ScheduleMaintenance | ScheduleMaintenanceHandler |
| StartMaintenance | StartMaintenanceHandler |
| CompleteMaintenance | CompleteMaintenanceHandler |
| VerifyMaintenance | VerifyMaintenanceHandler |
| CloseMaintenance | CloseMaintenanceHandler |
| CancelMaintenance | CancelMaintenanceHandler |
| SuspendMaintenance | SuspendMaintenanceHandler |
| ResumeMaintenance | ResumeMaintenanceHandler |
| RegisterFailure | RegisterFailureHandler |
| RegisterRepair | RegisterRepairHandler |
| RegisterInspection | RegisterInspectionHandler |
| RegisterOverhaul | RegisterOverhaulHandler |
| ReplaceMaintenanceComponent | ReplaceMaintenanceComponentHandler |

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
| GenerateLubricantForecast | GenerateLubricantForecastHandler |
| GenerateCoolantForecast | GenerateCoolantForecastHandler |
| GenerateMaintenanceForecast | GenerateMaintenanceForecastHandler |
| GenerateSparePartsForecast | GenerateSparePartsForecastHandler |
| GenerateReplacementForecast | GenerateReplacementForecastHandler |
| RefreshForecastModels | RefreshForecastModelsHandler |
| ValidateForecast | ValidateForecastHandler |
| ApproveForecast | ApproveForecastHandler |
| ScheduleForecast | ScheduleForecastHandler |
| ConsumeForecast | ConsumeForecastHandler |
| CompleteForecast | CompleteForecastHandler |
| CancelForecast | CancelForecastHandler |

---

# 15a. Organization Command Handlers

| Command | Handler |
|----------|---------|
| RegisterOrganization | RegisterOrganizationHandler |
| AssociateUserWithOrganization | AssociateUserWithOrganizationHandler |

---

# 15b. Notification Command Handlers

| Command | Handler |
|----------|---------|
| AcknowledgeNotification | AcknowledgeNotificationHandler |
| ArchiveNotification | ArchiveNotificationHandler |
| CancelNotification | CancelNotificationHandler |
| UpdateNotificationPreferences | UpdateNotificationPreferencesHandler |

---

# 15c. Internal Messaging Command Handlers

| Command | Handler |
|----------|---------|
| StartConversation | StartConversationHandler |
| AddConversationParticipant | AddConversationParticipantHandler |
| SendMessage | SendMessageHandler |
| AttachFileToMessage | AttachFileToMessageHandler |
| MarkMessageAsRead | MarkMessageAsReadHandler |
| ArchiveMessage | ArchiveMessageHandler |
| DeleteMessage | DeleteMessageHandler |
| CloseConversation | CloseConversationHandler |
| ReopenConversation | ReopenConversationHandler |

---

# 15d. AI Assistant Command Handlers

| Command | Handler |
|----------|---------|
| AskBusinessQuestion | AskBusinessQuestionHandler |
| RequestRecommendation | RequestRecommendationHandler |
| RequestHistoricalSummary | RequestHistoricalSummaryHandler |
| RequestKnowledgeDiscovery | RequestKnowledgeDiscoveryHandler |
| RequestRiskAssessment | RequestRiskAssessmentHandler |

---

# 15e. Relationship Management Command Handlers

| Command | Handler |
|----------|---------|
| CreateRelationship | CreateRelationshipHandler |
| ActivateRelationship | ActivateRelationshipHandler |
| ModifyRelationship | ModifyRelationshipHandler |
| ExpireRelationship | ExpireRelationshipHandler |

---

# 15f. Distributed Workspace Synchronization Command Handlers

| Command | Handler |
|----------|---------|
| CreateSynchronizationPackage | CreateSynchronizationPackageHandler |
| TransferSynchronizationPackage | TransferSynchronizationPackageHandler |
| ValidateSynchronizationPackage | ValidateSynchronizationPackageHandler |
| ApplySynchronizationPackage | ApplySynchronizationPackageHandler |
| RequestWorkingSet | RequestWorkingSetHandler |
| ResolveSynchronizationConflict | ResolveSynchronizationConflictHandler |

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

# Dependency Rules

Handlers may depend on:

- Repository Interfaces
- Domain Services
- Unit of Work
- Logger
- Application Services

Handlers shall never depend directly on:

- Entity Framework
- SQL
- Infrastructure Implementations

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


---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 02-Commands.md
- 03-Queries.md
- ../06-decisions/ADR-0036-Validation-Pipeline-Architecture.md
- docs/03-domain/06-DomainServices.md
- docs/03-domain/07-DomainEvents.md
- ADR-0011 — Adopt CQRS

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Handler Architecture                          |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Completed Maintenance and Forecast Command Handler tables to match all commands in 02-Commands.md (several were already missing before the recent lifecycle expansion, e.g. CancelMaintenance, ReplaceMaintenanceComponent, and 2 of 6 Forecast Generate commands) |
| 4.2.0   | 2026-08-02 | Solution Architect | Added Section 15a Organization Command Handlers, matching the new commands in 02-Commands.md |
| 4.3.0   | 2026-08-02 | Solution Architect | Added Section 15b Notification Command Handlers, matching the new commands in 02-Commands.md |
| 4.4.0   | 2026-08-02 | Solution Architect | Added Section 15c Internal Messaging Command Handlers, matching the new commands in 02-Commands.md |
| 4.5.0   | 2026-08-02 | Solution Architect | Added Section 15d AI Assistant Command Handlers, matching the new commands in 02-Commands.md |
| 4.6.0   | 2026-08-02 | Solution Architect | Added Section 15e Relationship Management Command Handlers, matching the new commands in 02-Commands.md |
| 4.7.0   | 2026-08-08 | Solution Architect | Added Section 15f Distributed Workspace Synchronization Command Handlers, matching the new commands in 02-Commands.md. This completes all 6 previously-missing module handler sets |