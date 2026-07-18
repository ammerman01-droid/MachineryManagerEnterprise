# Commands

**Document ID:** MME-MOD-002

**Repository Path:** `docs/04-modules/02-Commands.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-ApplicationArchitecture.md
- 01-UseCases.md
- docs/03-domain/03-DomainModel.md
- docs/03-domain/04-Aggregates.md
- docs/03-domain/05-DomainServices.md

---

# 1. Purpose

This document defines every Command used by the Application Layer.

Commands represent requests to change business state.

A Command expresses user intention.

A Command does not guarantee success.

Only successful execution produces Domain Events.

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

## CMD-401

CreateMaintenancePlan

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

## CMD-405

CancelMaintenance

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

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Command Catalogue |