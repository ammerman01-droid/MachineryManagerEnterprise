| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOM-011            |
| **Title**        | Ubiquitous Language |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the official **Ubiquitous Language** (زبان یکپارچه) for MachineryManagerEnterprise according to Domain-Driven Design (DDD) principles.

Its purpose is to establish a shared, unambiguous, and precise vocabulary used consistently across domain experts, solution architects, software engineers, source code, database schemas, API contracts, and user interface labels.

---

# 2. Objectives

The Ubiquitous Language provides:

- **Unambiguous Communication**: Eliminates misunderstandings between business stakeholders and software engineers.
- **Direct Code Alignment**: Guarantees that classes, methods, database tables, and API endpoints use the exact business terminology.
- **Contextual Precision**: Clarifies term definitions within specific Bounded Contexts.
- **Bilingual Consistency**: Ensures a 1-to-1 mapping between English domain terms and Persian (فارسی) user interface terms.

---

# 3. Core Governance Rules

1. **Single Source of Truth**: All domain model entities, value objects, domain events, commands, queries, and code identifiers shall strictly match the terms defined in this document.
2. **Zero Synonyms**: Multiple terms for the same concept (e.g., using both `Device` and `Asset` interchangeably) are strictly forbidden. `Asset` is the sole term.
3. **No Technical Leaks into Business Terms**: Technical implementation details (e.g., `AssetRowDTO`, `EquipmentTableManager`) shall not pollute the business domain vocabulary.
4. **Code Auditing**: Static analysis and code reviews shall verify that entity names and ubiquitous language terms match this document.

---

# 4. Domain Vocabulary Dictionary

| English Term | Persian Term (عنوان فارسی) | Bounded Context | Code Identifier | Definition |
|--------------|---------------------------|-----------------|-----------------|------------|
| **Asset** | ماشین‌آلات / دارایی | Asset Management | `Asset` | Any heavy machinery, equipment, or vehicle managed by the system. |
| **Asset Category** | دسته‌بندی ماشین‌آلات | Asset Management | `AssetCategory` | Classification of equipment (e.g., Excavator, Loader, Crane, Dozer). |
| **Tracked Component** | قطعه تحت‌ردیابی | Tracked Components | `TrackedComponent` | High-value serialized component tracked throughout its lifecycle (e.g., Engine, Transmission). |
| **Meter Reading** | کارکرد کارکردسنج | Fleet Operations | `MeterReading` | Recorded operating hours or mileage for an asset. |
| **Hour Meter** | ساعت‌کار | Fleet Operations | `HourMeter` | Counter measuring cumulative operational hours. |
| **Tire** | تایـر / لاستیک | Tire Lifecycle | `Tire` | Serialized tire asset tracked by position, tread depth, and pressure. |
| **Tread Depth** | عمق عاج | Tire Lifecycle | `TreadDepth` | Remaining tread thickness measurement in millimeters. |
| **Battery** | باطری | Battery Lifecycle | `Battery` | Serialized battery asset tracked by voltage, health state, and installation history. |
| **State of Health (SoH)** | شاخص سلامت باطری | Battery Lifecycle | `StateOfHealth` | Percentage representing battery degradation state. |
| **Part** | قطعه / یدکی | Parts Catalog | `Part` | Catalog item representing spare parts and consumables. |
| **Cross Reference** | مرجع متقابل قطعه | Parts Catalog | `PartCrossReference` | Mapping between OEM part numbers and equivalent aftermarket parts. |
| **Work Order** | دستور کار | Maintenance | `WorkOrder` | Formal authorization to perform maintenance activities. |
| **Maintenance Forecast** | پیش‌بینی سرویس | Forecast | `MaintenanceForecast` | Predictive schedule for upcoming maintenance based on meter trends. |
| **Incident** | حادثه / رخداد | Incident Management | `Incident` | Unplanned breakdown, defect, or safety event reported for an asset. |
| **Fault Code** | کد خطا | Diagnostics | `FaultCode` | Standard diagnostic trouble code (DTC) logged by asset telemetry. |
| **Relationship** | رابطه / بستگی | Relationships | `Relationship` | Business relationship between companies, contractors, or suppliers. |
| **Workspace** | فضای کاری | Workspace Sync | `Workspace` | Local or cloud execution context containing tenant operational data. |
| **Workspace Sync** | همگام‌سازی فضای کاری | Workspace Sync | `WorkspaceSync` | Synchronization process between offline node workspace and enterprise cloud. |
| **AI Assistant** | دستیار هوشمند | AI Assistant | `AIAssistant` | Embedded AI service providing predictive insights, voice queries, and recommendations. |

---

# 5. Context-Specific Variations

Certain terms have specific meanings depending on the Bounded Context:

```text
               ┌──────────────────────────────┐
               │         Asset Context        │
               │  Status = Active / Retired   │
               └──────────────┬───────────────┘
                              │
                              ▼
               ┌──────────────────────────────┐
               │      Maintenance Context     │
               │  Status = Pending / Closed   │
               └──────────────────────────────┘
```

- **Status in Asset Context**: Represents operational state (`Active`, `UnderMaintenance`, `Decommissioned`, `Sold`).
- **Status in Work Order Context**: Represents workflow state (`Draft`, `Approved`, `InProgress`, `Completed`, `Cancelled`).
- **Status in Tire Context**: Represents physical assignment state (`InStock`, `Mounted`, `Scrapped`, `Retreaded`).

---

# 6. Anti-Patterns & Banned Terminology

The following terms are strictly **BANNED** in source code, documentation, and discussions to avoid ambiguity:

| Banned / Deprecated Term | Required Replacement | Reason for Ban |
|--------------------------|----------------------|----------------|
| `Equipment` / `Machine` | `Asset` | Violates Ubiquitous Language standardizing on `Asset`. |
| `FixTicket` / `Task` | `WorkOrder` | Vague terminology; `WorkOrder` is the domain standard. |
| `Item` / `Stuff` | `Part` / `Asset` | Non-descriptive and generic. |
| `UserCompany` | `Organization` | Confuses Identity with Organization hierarchy; `Company` is a distinct concept (equipment manufacturer brand), not the tenant. |
| `DeviceData` | `MeterReading` / `Telemetry` | Technical jargon; lacks business meaning. |

---

# 7. Translation & Localization Rules

1. **Source Code**: All C# classes, interfaces, enums, properties, database schema columns, API JSON fields, and commit messages MUST be written in **English** using the exact Ubiquitous Language identifiers.
2. **User Interface**: All Blazor UI components, MudBlazor dialogs, labels, tooltips, and exported PDF reports displayed to users MUST use the official **Persian Translation** from the Vocabulary Dictionary.
3. **No Direct Literal Translations**: Translations must follow industry-accepted Persian domain terminology (e.g., use "دستور کار" for `WorkOrder`, not "سفارش کار").

---

# 8. Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- `docs/03-domain/00-Glossary.md`
- `docs/03-domain/01-DomainPrinciples.md`
- `docs/03-domain/02-CoreConcepts.md`
- `docs/03-domain/03-BoundedContexts.md`
- `docs/03-domain/04-DomainModel.md`

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Ubiquitous Language Specification             |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Corrected UserCompany forbidden-term mapping to Organization, consistent with the Glossary's Company/Organization split |