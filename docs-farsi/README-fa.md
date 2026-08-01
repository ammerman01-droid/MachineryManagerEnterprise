| ویژگی | مقدار |
|----------|-------|
| **شناسه مستند** | DOC-README |
| **عنوان** | فهرست مستندات فنی و حاکمیت معماری (Technical Documentation Index & Architecture Governance) |
| **نسخه** | 4.0.0 |
| **وضعیت** | تاییدشده (Approved) |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف (Purpose)

این پوشه حاوی مستندات فنی رسمی **MachineryManagerEnterprise** است.

مستندات بر اساس **استاندارد مستندسازی نسخه 4.0.0** ساختار یافته‌اند تا از تمامی مراحل چرخه حیات توسعه نرم‌افزار پشتیبانی کنند؛ از چشم‌انداز محصول تا ارزیابی‌های معماری، مدل‌سازی دامنه، پیاده‌سازی، تست و مدیریت انتشار.

هر تصمیم معماری، انتخاب فناوری، مشخصات قوانین کسب‌وکار و استراتژی پیاده‌سازی مستند شده و در سراسر پلتفرم قابل ردگیری است.

---

# ۲. ساختار مستندات (Documentation Structure)

```text
docs/
├── 01-vision/              چشم‌انداز محصول و نقشه راه مستندات
├── 02-architecture/        معماری سیستم، مدل قابلیت‌ها و ارزیابی‌های فنی (TE-0001 تا TE-0035)
├── 03-domain/              مدل دامنه، مفاهیم اصلی، اگريگیت‌های DDD، قوانین کسب‌وکار و مشخصات کسب‌وکار (BR-001 تا BR-016)
├── 04-modules/             معماری ماژول‌ها، سرویس‌های برنامه، دستورات (Commands)، پرس‌وجوها (Queries) و پردازش‌گرها (Handlers)
├── 05-development/         اصول توسعه، ساختار راهکار، استانداردهای کدنویسی و کاتالوگ وابستگی‌ها
├── 06-decisions/           فهرست مرجع تصمیمات معماری (ADR-0001 تا ADR-0029)
├── 07-api/                 اصول API، مشخصات OpenAPI، کنوانسیون‌های REST و طراحی نقاط پایانی
├── 08-releases/            استراتژی انتشار، پایپ‌لاین‌های استقرار، چک‌لیست و سیاست نسخه‌گذاری
└── 09-proof-of-concepts/   اثبات مفاهیم فنی (POC-0001 تقویم جلالی)
```

---

# ۳. کاتالوگ جامع مستندات (Master Documentation Catalog)

## ۱. چشم‌انداز و نقشه راه (`01-vision/`)

- [00-Vision.md](01-vision/00-Vision.md)
- [01-DocumentationRoadmap.md](01-vision/01-DocumentationRoadmap.md)

---

## ۲. معماری و ارزیابی‌های فنی (`02-architecture/`)

- [00-TechnologyEvaluationTemplate.md](02-architecture/00-TechnologyEvaluationTemplate.md)
- [01-Architecture.md](02-architecture/01-Architecture.md)
- [02-CapabilityModel.md](02-architecture/02-CapabilityModel.md)
- [03-TechnologyGapAnalysis.md](02-architecture/03-TechnologyGapAnalysis.md)

### ثبت ارزیابی‌های فنی (TE-0001 تا TE-0035)

- [TE-0001-.NET10.md](02-architecture/TE-0001-.NET10.md)
- [TE-0002-Blazor.md](02-architecture/TE-0002-Blazor.md)
- [TE-0003-MudBlazor.md](02-architecture/TE-0003-MudBlazor.md)
- [TE-0004-EntityFrameworkCore.md](02-architecture/TE-0004-EntityFrameworkCore.md)
- [TE-0005-FluentValidation.md](02-architecture/TE-0005-FluentValidation.md)
- [TE-0006-Mapster.md](02-architecture/TE-0006-Mapster.md)
- [TE-0007-Serilog.md](02-architecture/TE-0007-Serilog.md)
- [TE-0008-OpenTelemetry.md](02-architecture/TE-0008-OpenTelemetry.md)
- [TE-0009-Use-MediatR.md](02-architecture/TE-0009-Use-MediatR.md)
- [TE-0010-Desktop-Mobile-Framework-Evaluation.md](02-architecture/TE-0010-Desktop-Mobile-Framework-Evaluation.md)
- [TE-0011-Embedded-Workspace-Database-Evaluation.md](02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md)
- [TE-0012-Enterprise-Messaging-Technology-Evaluation.md](02-architecture/TE-0012-Enterprise-Messaging-Technology-Evaluation.md)
- [TE-0013-Artificial-Intelligence-Technology-Evaluation.md](02-architecture/TE-0013-Artificial-Intelligence-Technology-Evaluation.md)
- [TE-0014-Background-Processing-Technology-Evaluation.md](02-architecture/TE-0014-Background-Processing-Technology-Evaluation.md)
- [TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md](02-architecture/TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md)
- [TE-0016-Enterprise-Search-Architecture-Evaluation.md](02-architecture/TE-0016-Enterprise-Search-Architecture-Evaluation.md)
- [TE-0017-Observability-and-Telemetry-Technology-Evaluation.md](02-architecture/TE-0017-Observability-and-Telemetry-Technology-Evaluation.md)
- [TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md](02-architecture/TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md)
- [TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md](02-architecture/TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md)
- [TE-0020-Authentication-and-Identity-Technology-Evaluation.md](02-architecture/TE-0020-Authentication-and-Identity-Technology-Evaluation.md)
- [TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md](02-architecture/TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md)
- [TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md](02-architecture/TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md)
- [TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md](02-architecture/TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md)
- [TE-0024-Data-Access-Architecture-Evaluation.md](02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md)
- [TE-0025-Database-Migration-Technology-Evaluation.md](02-architecture/TE-0025-Database-Migration-Technology-Evaluation.md)
- [TE-0026-File-Storage-Technology-Evaluation.md](02-architecture/TE-0026-File-Storage-Technology-Evaluation.md)
- [TE-0027-Search-Engine-Technology-Evaluation.md](02-architecture/TE-0027-Search-Engine-Technology-Evaluation.md)
- [TE-0028-Vector-Database-Technology-Evaluation.md](02-architecture/TE-0028-Vector-Database-Technology-Evaluation.md)
- [TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md](02-architecture/TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md)
- [TE-0030-Testing-Technology-Evaluation.md](02-architecture/TE-0030-Testing-Technology-Evaluation.md)
- [TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md](02-architecture/TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md)
- [TE-0032-Security-Technology-Evaluation.md](02-architecture/TE-0032-Security-Technology-Evaluation.md)
- [TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md](02-architecture/TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md)
- [TE-0034-Client-UI-Technology-Evaluation.md](02-architecture/TE-0034-Client-UI-Technology-Evaluation.md)
- [TE-0035-Reporting-Technology-Evaluation.md](02-architecture/TE-0035-Reporting-Technology-Evaluation.md)

---

## ۳. مدل دامنه و مشخصات کسب‌وکار (`03-domain/`)

- [00-Glossary.md](03-domain/00-Glossary.md)
- [01-DomainPrinciples.md](03-domain/01-DomainPrinciples.md)
- [02-CoreConcepts.md](03-domain/02-CoreConcepts.md)
- [03-BoundedContexts.md](03-domain/03-BoundedContexts.md)
- [04-DomainModel.md](03-domain/04-DomainModel.md)
- [05-Aggregates.md](03-domain/05-Aggregates.md)
- [06-DomainServices.md](03-domain/06-DomainServices.md)
- [07-DomainEvents.md](03-domain/07-DomainEvents.md)
- [08-BusinessRules.md](03-domain/08-BusinessRules.md)
- [09-StateMachines.md](03-domain/09-StateMachines.md)
- [10-DomainDiscovery.md](03-domain/10-DomainDiscovery.md)
- [11-UbiquitousLanguage.md](03-domain/11-UbiquitousLanguage.md)
- [12-DomainPatterns.md](03-domain/12-DomainPatterns.md)
- [DG-00-DomainGovernance.md](03-domain/DG-00-DomainGovernance.md)
- [DomainDocumentationIndex.md](03-domain/DomainDocumentationIndex.md)
- [README.md](03-domain/README.md)

### مشخصات کسب‌وکار (`03-domain/specifications/`)

- [BR-001-INDEX.md](03-domain/specifications/BR-001-INDEX.md)
- [BR-002-BusinessSpecificationTemplate.md](03-domain/specifications/BR-002-BusinessSpecificationTemplate.md)
- [BR-003-BusinessSpecification-AssetRelationships.md](03-domain/specifications/BR-003-BusinessSpecification-AssetRelationships.md)
- [BR-004-BusinessSpecification-TrackedComponents.md](03-domain/specifications/BR-004-BusinessSpecification-TrackedComponents.md)
- [BR-005-BusinessSpecification-TireLifecycle.md](03-domain/specifications/BR-005-BusinessSpecification-TireLifecycle.md)
- [BR-006-BusinessSpecification-BatteryLifecycle.md](03-domain/specifications/BR-006-BusinessSpecification-BatteryLifecycle.md)
- [BR-007-BusinessSpecification-PartsCatalog.md](03-domain/specifications/BR-007-BusinessSpecification-PartsCatalog.md)
- [BR-008-BusinessSpecification-PartCrossReference.md](03-domain/specifications/BR-008-BusinessSpecification-PartCrossReference.md)
- [BR-009-BusinessSpecification-IncidentManagement.md](03-domain/specifications/BR-009-BusinessSpecification-IncidentManagement.md)
- [BR-010-BusinessSpecification-MaintenanceForecast.md](03-domain/specifications/BR-010-BusinessSpecification-MaintenanceForecast.md)
- [BR-011-BusinessSpecification-MaintenanceOperations.md](03-domain/specifications/BR-011-BusinessSpecification-MaintenanceOperations.md)
- [BR-012-BusinessSpecification-NotificationCenter.md](03-domain/specifications/BR-012-BusinessSpecification-NotificationCenter.md)
- [BR-013-BusinessSpecification-InternalMessaging.md](03-domain/specifications/BR-013-BusinessSpecification-InternalMessaging.md)
- [BR-014-BusinessSpecification-AIAssistant.md](03-domain/specifications/BR-014-BusinessSpecification-AIAssistant.md)
- [BR-015-BusinessSpecification-RelationshipManagement.md](03-domain/specifications/BR-015-BusinessSpecification-RelationshipManagement.md)
- [BR-016-BusinessSpecification-DistributedWorkspaceSynchronization.md](03-domain/specifications/BR-016-BusinessSpecification-DistributedWorkspaceSynchronization.md)

---

## ۴. معماری برنامه و ماژول‌ها (`04-modules/`)

- [00-ApplicationArchitecture.md](04-modules/00-ApplicationArchitecture.md)
- [01-UseCases.md](04-modules/01-UseCases.md)
- [02-Commands.md](04-modules/02-Commands.md)
- [03-Queries.md](04-modules/03-Queries.md)
- [04-Handlers.md](04-modules/04-Handlers.md)
- [05-ApplicationServices.md](04-modules/05-ApplicationServices.md)
- [06-Workflows.md](04-modules/06-Workflows.md)
- [07-Authorization.md](04-modules/07-Authorization.md)

---

## ۵. استانداردهای توسعه (`05-development/`)

- [00-DevelopmentPrinciples.md](05-development/00-DevelopmentPrinciples.md)
- [01-SolutionStructure.md](05-development/01-SolutionStructure.md)
- [02-ProjectStructure.md](05-development/02-ProjectStructure.md)
- [03-NamespaceConvention.md](05-development/03-NamespaceConvention.md)
- [04-DependencyRules.md](05-development/04-DependencyRules.md)
- [05-CodingStandards.md](05-development/05-CodingStandards.md)
- [06-NamingConventions.md](05-development/06-NamingConventions.md)
- [07-ErrorHandling.md](05-development/07-ErrorHandling.md)
- [08-LoggingStrategy.md](05-development/08-LoggingStrategy.md)
- [09-TestingStrategy.md](05-development/09-TestingStrategy.md)
- [10-BuildPipeline.md](05-development/10-BuildPipeline.md)
- [11-DependencyCatalog.md](05-development/11-DependencyCatalog.md)
- [12-CapabilityDependencyMatrix.md](05-development/12-CapabilityDependencyMatrix.md)
- [13-AggregateDependencyMatrix.md](05-development/13-AggregateDependencyMatrix.md)
- [DOCUMENT_CONVENTIONS.md](05-development/DOCUMENT_CONVENTIONS.md)

---

## ۶. اسناد تصمیمات معماری (`06-decisions/`)

- [000-ADR-INDEX.md](06-decisions/000-ADR-INDEX.md)
- [00-ArchitectureDecisionRecordTemplate.md](06-decisions/00-ArchitectureDecisionRecordTemplate.md)
- [ADR-0001-CleanArchitecture.md](06-decisions/ADR-0001-CleanArchitecture.md)
- [ADR-0002-Adopt-Open-Source-First-Policy.md](06-decisions/ADR-0002-Adopt-Open-Source-First-Policy.md)
- [ADR-0003-Use-NET-10.md](06-decisions/ADR-0003-Use-NET-10.md)
- [ADR-0004-Use-Blazor.md](06-decisions/ADR-0004-Use-Blazor.md)
- [ADR-0005-Use-MudBlazor.md](06-decisions/ADR-0005-Use-MudBlazor.md)
- [ADR-0006-Use-EntityFrameworkCore.md](06-decisions/ADR-0006-Use-EntityFrameworkCore.md)
- [ADR-0007-Use-FluentValidation.md](06-decisions/ADR-0007-Use-FluentValidation.md)
- [ADR-0008-Use-Mapster.md](06-decisions/ADR-0008-Use-Mapster.md)
- [ADR-0009-Use-Serilog.md](06-decisions/ADR-0009-Use-Serilog.md)
- [ADR-0010-Use-OpenTelemetry.md](06-decisions/ADR-0010-Use-OpenTelemetry.md)
- [ADR-0011-Use-MediatR.md](06-decisions/ADR-0011-Use-MediatR.md)
- [ADR-0012-DistributedWorkspaceArchitecture.md](06-decisions/ADR-0012-DistributedWorkspaceArchitecture.md)
- [ADR-0013-Client-Application-Architecture.md](06-decisions/ADR-0013-Client-Application-Architecture.md)
- [ADR-0014-Workspace-Data-Architecture.md](06-decisions/ADR-0014-Workspace-Data-Architecture.md)
- [ADR-0015-Workspace-Synchronization-Architecture.md](06-decisions/ADR-0015-Workspace-Synchronization-Architecture.md)
- [ADR-0016-Enterprise-Messaging-Architecture.md](06-decisions/ADR-0016-Enterprise-Messaging-Architecture.md)
- [ADR-0017-Artificial-Intelligence-Integration-Architecture.md](06-decisions/ADR-0017-Artificial-Intelligence-Integration-Architecture.md)
- [ADR-0018-External-Integration-Architecture.md](06-decisions/ADR-0018-External-Integration-Architecture.md)
- [ADR-0019-Hybrid-Persistence-Strategy-for-Read-Heavy-Queries.md](06-decisions/ADR-0019-Hybrid-Persistence-Strategy-for-Read-Heavy-Queries.md)
- [ADR-0020-File-Storage-Strategy.md](06-decisions/ADR-0020-File-Storage-Strategy.md)
- [ADR-0021-Search-Strategy.md](06-decisions/ADR-0021-Search-Strategy.md)
- [ADR-0022-AI-Knowledge-Retrieval-Architecture.md](06-decisions/ADR-0022-AI-Knowledge-Retrieval-Architecture.md)
- [ADR-0023-Artificial-Intelligence-Provider-Strategy.md](06-decisions/ADR-0023-Artificial-Intelligence-Provider-Strategy.md)
- [ADR-0024-Enterprise-Testing-Strategy.md](06-decisions/ADR-0024-Enterprise-Testing-Strategy.md)
- [ADR-0025-Build-and-Deployment-Architecture.md](06-decisions/ADR-0025-Build-and-Deployment-Architecture.md)
- [ADR-0026-Enterprise-Security-Strategy.md](06-decisions/ADR-0026-Enterprise-Security-Strategy.md)
- [ADR-0027-Enterprise-Performance-Testing-Strategy.md](06-decisions/ADR-0027-Enterprise-Performance-Testing-Strategy.md)
- [ADR-0028-Client-UI-Architecture.md](06-decisions/ADR-0028-Client-UI-Architecture.md)
- [ADR-0029-Enterprise-Reporting-Architecture.md](06-decisions/ADR-0029-Enterprise-Reporting-Architecture.md)

---

## ۷. مشخصات API (`07-api/`)

- [00-ApiPrinciples.md](07-api/00-ApiPrinciples.md)
- [01-RestConventions.md](07-api/01-RestConventions.md)
- [02-EndpointDesign.md](07-api/02-EndpointDesign.md)
- [03-RequestResponseModel.md](07-api/03-RequestResponseModel.md)
- [04-ErrorResponses.md](07-api/04-ErrorResponses.md)
- [05-PaginationFilteringSorting.md](07-api/05-PaginationFilteringSorting.md)
- [06-Versioning.md](07-api/06-Versioning.md)
- [07-AuthenticationAuthorization.md](07-api/07-AuthenticationAuthorization.md)
- [08-OpenApiSpecification.md](07-api/08-OpenApiSpecification.md)
- [09-ApiLifecycle.md](07-api/09-ApiLifecycle.md)

---

## ۸. مستندات انتشار (`08-releases/`)

- [00-ReleaseStrategy.md](08-releases/00-ReleaseStrategy.md)
- [01-VersioningPolicy.md](08-releases/01-VersioningPolicy.md)
- [02-ReleaseProcess.md](08-releases/02-ReleaseProcess.md)
- [03-DeploymentStrategy.md](08-releases/03-DeploymentStrategy.md)
- [04-SupportLifecycle.md](08-releases/04-SupportLifecycle.md)
- [05-ReleaseChecklist.md](08-releases/05-ReleaseChecklist.md)
- [06-ReleaseNotesTemplate.md](08-releases/06-ReleaseNotesTemplate.md)

---

## ۹. اثبات مفاهیم (`09-proof-of-concepts/`)

- [POC-0001-JalaliMudDatePicker.md](09-proof-of-concepts/POC-0001-JalaliMudDatePicker.md)

---

# فرآیند حاکمیت معماری (Architecture Governance Process)

تمامی تصمیمات فنی مهم از یک چرخه حیات استاندارد تبعیت می‌کنند:

```text
نیازمندی کسب‌وکار / مشخصات BR
         │
         ▼
ارزیابی فنی (TE)
         │
         ▼
اثبات مفهوم (POC)  (اختیاری برای وظایف فنی با عدم‌قطعیت بالا)
         │
         ▼
سند تصمیم معماری (ADR)
         │
         ▼
پیاده‌سازی و اعتبارسنجی
```

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

---

# اسناد مرتبط

- `../README.md`
- `../PROJECT_CHARTER.md`
- `../PROJECT_PROGRESS.md`
- `../REPOSITORY_GUIDE.md`

---

# تاریخچه تغییرات

| نسخه | تاریخ | نویسنده | شرح |
|---------|------------|--------------------|----------------------------------------------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | ساختار اولیه مستندات |
| 2.0.0 | 2026-07-18 | معمار راهکار | تجدید سازمان معماری مستندات |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی طبق استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه 4.0.0؛ کاتالوگ‌بندی کامل تمامی 145 فایل مستندات |
