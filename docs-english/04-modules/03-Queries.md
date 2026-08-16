| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | MOD-003            |
| **Title**        | Query catalogue    |
| **Version**      | 4.6.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines all Queries used by MachineryManagerEnterprise.

Queries retrieve information from the system.

Queries never modify business state.

Queries are read-only.

---

# Query Philosophy

Queries retrieve information without modifying business state.

Queries never execute business rules.

Queries may compose data from multiple modules through read models when
necessary.

---

# Query Design Rules

Every Query shall:

- Return data only.
- Never modify business state.
- Have exactly one Handler.
- Never publish Domain Events.
- Be optimized for read performance.

---

# 2. Query Principles

Every Query shall satisfy the following principles.

- Read-only
- Side-effect free
- Technology independent
- Business oriented
- Independently executable
- Optimized for reading

A Query shall never publish Domain Events.

---

# 3. Query Categories

```text
Queries

├── Asset Queries
├── Engine Queries
├── Component Queries
├── Meter Queries
├── Maintenance Queries
├── Financial Queries
├── Document Queries
├── Forecast Queries
├── Reporting Queries
└── Administration Queries
```

---

# 4. Query Structure

Every Query shall contain:

- QueryId
- QueryType
- RequestedAt
- RequestedBy
- Filters
- Paging (optional)
- Sorting (optional)

Business-specific filters are defined by each Query.

---

# 5. Asset Queries

## QRY-001

GetAsset

Returns one Asset.

---

## QRY-002

SearchAssets

Returns Assets matching search criteria.

---

## QRY-003

GetAssetHistory

Returns complete lifecycle history.

---

## QRY-004

GetAssetCurrentConfiguration

Returns current Engine, Meter and Components.

---

## QRY-005

GetAssetTimeline

Returns chronological business history.

---

## QRY-006

GetAssetDashboard

Returns summarized operational information.

---

# 6. Engine Queries

## QRY-101

GetEngine

---

## QRY-102

SearchEngines

---

## QRY-103

GetEngineInstallationHistory

---

## QRY-104

GetCurrentInstalledEngine

---

## QRY-105

GetEngineRepairHistory

---

## QRY-106

GetEngineUsageHistory

---

# 7. Component Queries

## QRY-201

GetComponent

---

## QRY-202

SearchComponents

---

## QRY-203

GetComponentHistory

---

## QRY-204

GetInstalledComponents

---

## QRY-205

GetReplacementHistory

---

# 8. Meter Queries

## QRY-301

GetCurrentMeter

---

## QRY-302

GetMeterHistory

---

## QRY-303

GetMeterReadings

---

## QRY-304

GetOperationalUsage

---

## QRY-305

GetNonOperationalUsage

---

## QRY-306

GetUsageCorrections


---

# 9. Maintenance Queries

## QRY-401

GetMaintenancePlan

Returns the active maintenance plan for an Asset.

---

## QRY-402

GetScheduledMaintenance

Returns scheduled maintenance activities.

---

## QRY-403

GetMaintenanceHistory

Returns complete maintenance history.

---

## QRY-404

GetInspectionHistory

Returns inspection records.

---

## QRY-405

GetFailureHistory

Returns failure history.

---

## QRY-406

GetRepairHistory

Returns repair history.

---

## QRY-407

GetOverhaulHistory

Returns overhaul history.

---

## QRY-408

GetUpcomingMaintenance

Returns future maintenance requirements.

---

# 10. Financial Queries

## QRY-501

GetPurchaseInformation

Returns acquisition information.

---

## QRY-502

GetOperatingExpenses

Returns operating expenses.

---

## QRY-503

GetFuelConsumptionCost

Returns fuel costs.

---

## QRY-504

GetMaintenanceCost

Returns maintenance expenses.

---

## QRY-505

GetDepreciation

Returns depreciation calculations.

---

## QRY-506

GetCurrentAssetValue

Returns current estimated Asset value.

---

## QRY-507

GetOwnershipCost

Returns Total Cost of Ownership.

---

## QRY-508

GetFinancialTimeline

Returns chronological financial history.

---

# 11. Document Queries

## QRY-601

GetDocument

Returns one document.

---

## QRY-602

GetDocuments

Returns all documents associated with an Asset.

---

## QRY-603

GetExpiredDocuments

Returns expired documents.

---

## QRY-604

GetDocumentsExpiringSoon

Returns documents approaching expiration.

---

## QRY-605

GetDocumentVersions

Returns document version history.

---

## QRY-606

GetDocumentPackage

Returns a complete export package.

---

# 12. Forecast Queries

## QRY-701

GetFuelForecast

Returns predicted fuel consumption.

---

## QRY-702

GetLubricantForecast

Returns lubricant forecasts.

---

## QRY-703

GetMaintenanceForecast

Returns maintenance predictions.

---

## QRY-704

GetReplacementForecast

Returns replacement predictions.

---

## QRY-705

CompareForecasts

Compares historical forecasts with actual values.

---

## QRY-706

GetForecastHistory

Returns previously generated forecasts.

---

# 13. Reporting Queries

## QRY-801

GetExecutiveDashboard

Returns executive summary information.

---

## QRY-802

GetAssetDashboard

Returns operational dashboard.

---

## QRY-803

GetFleetStatistics

Returns fleet-wide statistics.

---

## QRY-804

GetOperationalKPIs

Returns operational performance indicators.

---

## QRY-805

GetFinancialKPIs

Returns financial performance indicators.

---

## QRY-806

GetMaintenanceKPIs

Returns maintenance indicators.

---

## QRY-807

GetForecastKPIs

Returns forecast accuracy indicators.

---

# 14. Administration Queries

## QRY-901

GetUsers

---

## QRY-902

GetRoles

---

## QRY-903

GetOrganizations

---

## QRY-904

GetLocations

---

## QRY-905

GetAuditLog

---

## QRY-906

GetSystemConfiguration

---

# 14a. Organization Queries

Formalized from BR-017 (Business Specification — Organization
Management). Extends QRY-903 GetOrganizations (Section 14) with
single-record and ownership queries.

## QRY-950

GetOrganization

---

## QRY-951

GetOrganizationAssets

---

# 14b. Notification Queries

Formalized from BR-012 (Business Specification — Notification Center).

## QRY-960

GetNotifications

---

## QRY-961

GetNotification

---

## QRY-962

GetNotificationPreferences

---

# 14c. Internal Messaging Queries

Formalized from BR-013 (Business Specification — Internal Messaging).

## QRY-970

GetConversations

---

## QRY-971

GetConversation

---

## QRY-972

GetMessages

---

## QRY-973

GetMessageAttachments

---

# 14d. AI Assistant Queries

Formalized from BR-014 (Business Specification — AI Assistant).

## QRY-980

GetRecommendations

---

## QRY-981

GetRecommendationExplanation

---

## QRY-982

GetAIInteractionHistory

---

# 14e. Relationship Management Queries

Formalized from BR-015 (Business Specification — Relationship
Management).

## QRY-990

GetRelationship

---

## QRY-991

GetRelationshipsForEntity

---

## QRY-992

GetRelationshipHistory

---

# 14f. Distributed Workspace Synchronization Queries

Formalized from BR-016 (Business Specification — Distributed Workspace
Synchronization).

## QRY-1000

GetSynchronizationHistory

---

## QRY-1001

GetSynchronizationPackage

---

## QRY-1002

GetSynchronizationConflicts

---

## QRY-1003

GetWorkingSet

---

# 15. Cross-Module Queries

The following Queries combine information from multiple modules.

---

## QRY-1001

GetCompleteAssetProfile

Combines:

- Asset
- Engine
- Components
- Maintenance
- Documents
- Financial
- Forecast

---

## QRY-1002

GetOperationalSummary

Combines:

- Usage
- Maintenance
- Financial

---

## QRY-1003

GetTechnicalSummary

Combines:

- Asset Model
- Engine Model
- Technical Library

---

## QRY-1004

GetBusinessTimeline

Returns a unified chronological history including:

- Meter readings
- Maintenance
- Repairs
- Financial transactions
- Documents
- Engine replacements

---

# 16. Query Validation

Queries shall validate:

- Authorization
- Requested scope
- Filter consistency
- Paging limits
- Sorting rules

Invalid Queries shall never reach the data access layer.

---

# 17. Query Naming Rules

Every Query shall:

- begin with **Get**, **Search**, or **Compare**;
- represent a business information request;
- remain technology independent.

Examples:

- GetAsset
- SearchAssets
- GetMaintenanceHistory
- CompareForecasts

Avoid:

- ReadTable
- ExecuteSQL
- SelectRows
- LoadEntity

---

# 18. Query Execution

Typical Query execution flow:

```text
Query

↓

Authorization

↓

Validation

↓

Query Handler

↓

Read Model

↓

Projection

↓

Response
```

Queries shall never modify business state.

Queries shall never publish Domain Events.

---

# 19. Future Queries

Future releases may introduce queries for:

- Inventory
- Procurement
- Fleet Scheduling
- AI Diagnostics
- IoT Telemetry
- Mobile Synchronization

Every future Query shall follow the conventions defined in this document.

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
- 02-Commands.md
- 04-Handlers.md
- docs/03-domain/04-DomainModel.md
- ADR-0011 — Adopt CQRS

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Query catalogue                               |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Added Section 14a Organization Queries (QRY-950, QRY-951), formalized from BR-017 |
| 4.2.0   | 2026-08-02 | Solution Architect | Added Section 14b Notification Queries (QRY-960 through QRY-962), formalized from BR-012 |
| 4.3.0   | 2026-08-02 | Solution Architect | Added Section 14c Internal Messaging Queries (QRY-970 through QRY-973), formalized from BR-013 |
| 4.4.0   | 2026-08-02 | Solution Architect | Added Section 14d AI Assistant Queries (QRY-980 through QRY-982), formalized from BR-014 |
| 4.5.0   | 2026-08-02 | Solution Architect | Added Section 14e Relationship Management Queries (QRY-990 through QRY-992), formalized from BR-015 |
| 4.6.0   | 2026-08-08 | Solution Architect | Added Section 14f Distributed Workspace Synchronization Queries (QRY-1000 through QRY-1003), formalized from BR-016. This completes all 6 previously-missing module query sets |