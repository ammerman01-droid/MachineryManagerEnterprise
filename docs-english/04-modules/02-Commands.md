| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | MOD-002            |
| **Title**        | Command catalogue  |
| **Version**      | 4.7.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines every Command used by the Application Layer.

Commands represent requests to change business state.

A Command expresses user intention.

A Command does not guarantee success.

Only successful execution produces Domain Events.

---

# Command Philosophy

Commands represent business intentions.

A Command requests a state transition but never performs business logic itself.

Business validation belongs to the Domain.

Application validation belongs to the Application Layer.

---

# 2. Command Principles

Every Command shall satisfy the following principles.

- Represents one business intention.
- Modifies business state.
- Is immutable.
- Has one responsible Handler.
- Has one expected business outcome.
- Contains only required input data.

Commands never contain business logic.

---

# 3. Command Categories

```text
Commands

├── Asset Commands
├── Engine Commands
├── Component Commands
├── Meter Commands
├── Maintenance Commands
├── Financial Commands
├── Document Commands
├── Forecast Commands
├── Administration Commands
└── Configuration Commands
```

---

# 4. Command Structure

Every Command shall contain:

- CommandId
- CommandType
- RequestedAt
- RequestedBy
- CorrelationId (optional)

Business-specific fields are defined by each Command.

---

# 5. Asset Commands

## CMD-001

RegisterAsset

Purpose

Registers a new physical Asset.

---

## CMD-002

UpdateAssetInformation

Purpose

Updates editable Asset information.

---

## CMD-003

TransferAsset

Purpose

Transfers Asset ownership or operational responsibility.

---

## CMD-004

RetireAsset

Purpose

Retires an Asset.

---

## CMD-005

DisposeAsset

Purpose

Marks an Asset as permanently disposed.

---

# 6. Engine Commands

## CMD-101

RegisterEngine

---

## CMD-102

InstallEngine

---

## CMD-103

RemoveEngine

---

## CMD-104

ReplaceEngine

---

## CMD-105

SendEngineToWorkshop

---

## CMD-106

ReturnEngineFromWorkshop

---

## CMD-107

RegisterEngineRebuild

---

# 7. Component Commands

## CMD-201

RegisterComponent

---

## CMD-202

InstallComponent

---

## CMD-203

RemoveComponent

---

## CMD-204

ReplaceComponent

---

## CMD-205

RetireComponent

---

# 8. Meter Commands

## CMD-301

InstallMeter

---

## CMD-302

ReplaceMeter

---

## CMD-303

RegisterMeterReading

---

## CMD-304

RegisterNonOperationalUsage

---

## CMD-305

CorrectMeterReading

---

## CMD-306

ArchiveMeter


---

# 9. Maintenance Commands

## CMD-400

RequestMaintenance

---

## CMD-401

CreateMaintenancePlan

---

## CMD-401a

ApproveMaintenancePlan

---

## CMD-402

ScheduleMaintenance

---

## CMD-403

StartMaintenance

---

## CMD-404

CompleteMaintenance

---

## CMD-404a

VerifyMaintenance

---

## CMD-404b

CloseMaintenance

---

## CMD-405

CancelMaintenance

---

## CMD-405a

SuspendMaintenance

---

## CMD-405b

ResumeMaintenance

---

## CMD-406

RegisterInspection

---

## CMD-407

RegisterFailure

---

## CMD-408

RegisterRepair

---

## CMD-409

RegisterOverhaul

---

## CMD-410

ReplaceMaintenanceComponent

---

# 10. Financial Commands

## CMD-501

RegisterAssetPurchase

---

## CMD-502

RegisterOperatingExpense

---

## CMD-503

RegisterFuelExpense

---

## CMD-504

RegisterMaintenanceExpense

---

## CMD-505

RegisterInsuranceExpense

---

## CMD-506

RegisterTaxExpense

---

## CMD-507

CalculateDepreciation

---

## CMD-508

RecalculateAssetValue

---

## CMD-509

RecalculateOwnershipCost

---

# 11. Document Commands

## CMD-601

RegisterDocument

---

## CMD-602

UploadDocumentImage

---

## CMD-603

UploadDocumentFile

---

## CMD-604

ReplaceDocumentVersion

---

## CMD-605

RenewDocument

---

## CMD-606

ArchiveDocument

---

## CMD-607

DeleteTemporaryDocument

Only temporary documents may be deleted.

Business documents shall never be deleted.

---

# 12. Forecast Commands

## CMD-701

GenerateFuelForecast

---

## CMD-702

GenerateLubricantForecast

---

## CMD-703

GenerateCoolantForecast

---

## CMD-704

GenerateMaintenanceForecast

---

## CMD-705

GenerateSparePartsForecast

---

## CMD-706

GenerateReplacementForecast

---

## CMD-707

RefreshForecastModels

---

## CMD-707a

ValidateForecast

---

## CMD-707b

ApproveForecast

---

## CMD-707c

ScheduleForecast

---

## CMD-707d

ConsumeForecast

---

## CMD-707e

CompleteForecast

---

## CMD-707f

CancelForecast

---

# 13. Administration Commands

## CMD-801

CreateUser

---

## CMD-802

DeactivateUser

---

## CMD-803

AssignRole

---

## CMD-804

ChangePermissions

---

## CMD-805

CreateOrganization

---

## CMD-806

RegisterLocation

---

# 14. Configuration Commands

## CMD-901

RegisterAssetModel

---

## CMD-902

RegisterEngineModel

---

## CMD-903

RegisterComponentModel

---

## CMD-904

RegisterManufacturer

---

## CMD-905

RegisterSupplier

---

## CMD-906

RegisterMaintenanceTemplate

---

## CMD-907

RegisterDocumentType

---

## CMD-908

UpdateForecastParameters

---

# 14a. Organization Commands

Formalized from BR-017 (Business Specification — Organization
Management).

## CMD-950

RegisterOrganization

---

## CMD-951

AssociateUserWithOrganization

---

# 14b. Notification Commands

Formalized from BR-012 (Business Specification — Notification Center).
Notification creation itself is a system-internal side effect of
other modules' business events, not a user-invoked command.

## CMD-960

AcknowledgeNotification

---

## CMD-961

ArchiveNotification

---

## CMD-962

CancelNotification

---

## CMD-963

UpdateNotificationPreferences

---

# 14c. Internal Messaging Commands

Formalized from BR-013 (Business Specification — Internal Messaging).

## CMD-970

StartConversation

---

## CMD-971

AddConversationParticipant

---

## CMD-972

SendMessage

---

## CMD-973

AttachFileToMessage

---

## CMD-974

MarkMessageAsRead

---

## CMD-975

ArchiveMessage

---

## CMD-976

DeleteMessage

---

## CMD-977

CloseConversation

---

## CMD-978

ReopenConversation

---

# 14d. AI Assistant Commands

Formalized from BR-014 (Business Specification — AI Assistant). Every
command below produces an advisory artifact (recommendation, summary,
answer) and never modifies business state in another module,
per BR-AI-003 and BR-AI-006.

## CMD-980

AskBusinessQuestion

---

## CMD-981

RequestRecommendation

---

## CMD-982

RequestHistoricalSummary

---

## CMD-983

RequestKnowledgeDiscovery

---

## CMD-984

RequestRiskAssessment

---

# 14e. Relationship Management Commands

Formalized from BR-015 (Business Specification — Relationship
Management).

## CMD-990

CreateRelationship

---

## CMD-991

ActivateRelationship

---

## CMD-992

ModifyRelationship

---

## CMD-993

ExpireRelationship

---

# 14f. Distributed Workspace Synchronization Commands

Formalized from BR-016 (Business Specification — Distributed Workspace
Synchronization). Package processing is atomic per BR-016's Package
Processing rule — every valid change becomes available, or none does.

## CMD-1000

CreateSynchronizationPackage

---

## CMD-1001

TransferSynchronizationPackage

---

## CMD-1002

ValidateSynchronizationPackage

---

## CMD-1003

ApplySynchronizationPackage

---

## CMD-1004

RequestWorkingSet

---

## CMD-1005

ResolveSynchronizationConflict

---

# 15. Cross-Module Commands

The following Commands coordinate multiple business modules.

---

## CMD-1001

PurchaseUsedAsset

Modules involved:

- Asset
- Engine
- Meter
- Financial

---

## CMD-1002

ReplaceEngineAndContinueOperation

Modules involved:

- Asset
- Engine
- Maintenance
- Financial

---

## CMD-1003

ReplaceHourMeter

Modules involved:

- Asset
- Meter
- Reporting

---

## CMD-1004

CompletePreventiveMaintenance

Modules involved:

- Maintenance
- Financial
- Forecast

---

## CMD-1005

DisposeAssetWithDocuments

Modules involved:

- Asset
- Documents
- Financial
- Reporting

---

# 16. Command Validation

Before execution every Command shall pass:

- Authorization
- Input Validation
- Business Preconditions
- Aggregate Availability
- Concurrency Validation

Only valid Commands reach the Domain Layer.

---

# 17. Command Naming Rules

Every Command shall:

- begin with a verb;
- describe business intention;
- use business terminology;
- avoid technical implementation details.

Examples:

- RegisterAsset
- InstallEngine
- ReplaceMeter
- CompleteMaintenance

Avoid:

- SaveAsset
- UpdateDatabase
- ExecuteSQL
- CallAPI

---

# 18. Command Execution

A successful Command normally follows this lifecycle.

```text
Command

↓

Validation

↓

Authorization

↓

Handler

↓

Aggregate

↓

Domain Events

↓

Commit

↓

Response
```

Failure at any stage prevents state modification.

---

# 19. Future Commands

Future releases may introduce commands for:

- Inventory
- Procurement
- Fleet Scheduling
- AI Diagnostics
- IoT Integration
- Mobile Offline Synchronization

Every future Command shall follow the conventions defined in this document.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 00-ApplicationArchitecture.md
- 01-UseCases.md
- 03-Queries.md
- 04-Handlers.md
- docs/03-domain/07-DomainEvents.md
- ADR-0011 — Adopt CQRS

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Command catalogue                             |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Added Maintenance and Forecast commands for the expanded 9-state and 7-state lifecycles now aligned with BR-011/BR-010 and 09-StateMachines.md |
| 4.2.0   | 2026-08-02 | Solution Architect | Added Section 14a Organization Commands (CMD-950, CMD-951), formalized from BR-017 |
| 4.3.0   | 2026-08-02 | Solution Architect | Added Section 14b Notification Commands (CMD-960 through CMD-963), formalized from BR-012 |
| 4.4.0   | 2026-08-02 | Solution Architect | Added Section 14c Internal Messaging Commands (CMD-970 through CMD-978), formalized from BR-013 |
| 4.5.0   | 2026-08-02 | Solution Architect | Added Section 14d AI Assistant Commands (CMD-980 through CMD-984), formalized from BR-014 |
| 4.6.0   | 2026-08-02 | Solution Architect | Added Section 14e Relationship Management Commands (CMD-990 through CMD-993), formalized from BR-015 |
| 4.7.0   | 2026-08-08 | Solution Architect | Added Section 14f Distributed Workspace Synchronization Commands (CMD-1000 through CMD-1005), formalized from BR-016. This completes all 6 previously-missing module command sets |