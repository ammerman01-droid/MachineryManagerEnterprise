# معماری سیستم

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | ARCH-001 |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# هدف

این سند به توصیف معماری کلی نرم‌افزار پلتفرم MachineryManagerEnterprise می‌پردازد.

این سند یک نمای کلی از معماری ارائه می‌دهد و به عنوان نقطه ورود مرکزی برای تمامی مستندات مرتبط با معماری عمل می‌کند.

تصمیمات اجرایی دقیق به صورت جداگانه در بخش «سوابق تصمیمات معماری» (ADR) و «ارزیابی‌های فناوری» (TE) مستند شده‌اند.

---

# چشم‌انداز معماری

این سیستم باید به صورت زیر پیاده‌سازی شود:

- یکپارچه ماژولار (Modular Monolith)
- معماری تمیز (Clean Architecture)
- طراحی دامنه-محور (DDD)
- برنامه مبتنی بر CQRS
- پلتفرم سازمانی چند-مستأجری (Multi-Tenant)
- معماری فضای کاری توزیع‌شده و کلاینت با اولویت آفلاین (Offline-First)

این معماری برای به حداکثر رساندن قابلیت نگهداری، توسعه‌پذیری، قابلیت تست، امنیت و پایداری بلندمدت طراحی شده است.

---

# اصول معماری

این معماری از اصول اصلی زیر پیروی می‌کند:

- تفکیک دغدغه‌ها (Separation of Concerns)
- قاعده وابستگی (جهت درونی)
- انسجام بالا و وابستگی پایین (High Cohesion & Low Coupling)
- مرزهای صریح ماژول
- اولویت با مستندسازی (Documentation First)
- اولویت با متن‌باز (ADR-0002)
- امنیت در طراحی (ADR-0026)
- خنثی بودن نسبت به ابر و عملیات آفلاین توزیع‌شده (ADR-0012)

---

# معماری کلان (High-Level)

```text
Presentation (Blazor Server, .NET MAUI Client, Web API)
        │
        ▼
Application (CQRS Commands/Queries, MediatR, FluentValidation, Mapster)
        │
        ▼
Domain (Entities, Aggregates, Domain Events, Value Objects)
        │
        ▼
Infrastructure (EF Core, Serilog, OpenTelemetry, RabbitMQ, S3, Meilisearch, Qdrant)
```

وابستگی‌ها همواره به سمت هسته دامنه (Domain) جهت‌گیری دارند.

لایه دامنه شامل منطق خالص کسب‌وکار است و هیچ وابستگی به فریم‌ورک‌های زیرساختی ندارد.

---

# لایه‌های معماری

## لایه ارائه (Presentation)

مسئولیت‌ها:

- رابط کاربری وب (Blazor Server و MudBlazor)
- اپلیکیشن‌های کلاینت دسکتاپ و موبایل (.NET MAUI و Blazor Hybrid)
- نقاط پایانی (Endpoints) وب‌سرویس RESTful و مشخصات OpenAPI
- احراز هویت، تعیین سطح دسترسی و مدیریت هویت (OpenID Connect / Keycloak)

---

## لایه کاربرد (Application)

مسئولیت‌ها:

- هماهنگ‌سازی موارد استفاده (Use Case Orchestration)
- پیاده‌سازی الگوی CQRS از طریق MediatR
- رفتارهای خط لوله (Logging، اعتبارسنجی، پایش عملکرد، محدوده تراکنش)
- اعتبارسنجی ورودی از طریق FluentValidation
- نگاشت DTO به مدل دامنه از طریق Mapster

---

## لایه دامنه (Domain)

مسئولیت‌ها:

- قوانین اصلی کسب‌وکار و سیاست‌های چرخه حیات دارایی‌ها
- مجموعه‌ها (Aggregates)، موجودیت‌ها (Entities) و اشیاء مقداری (Value Objects) دامنه
- ارسال و مدیریت رویدادهای دامنه (Domain Events)
- منطق دامنه در حوزه مدیریت دارایی‌ها، نگهداری و تعمیرات، قطعات و تجهیزات اندازه‌گیری

لایه دامنه هیچ‌گونه دغدغه زیرساختی ندارد.

---

## لایه زیرساخت (Infrastructure)

مسئولیت‌ها:

- پایداری داده‌های رابطه‌ای (Entity Framework Core و Dapper)
- ذخیره‌سازی محلی تعبیه‌شده برای فضاهای کاری آفلاین (SQLite و LiteDB)
- موتور همگام‌سازی بسته (Package) فضاهای کاری توزیع‌شده
- لاگ‌گذاری ساختاریافته (Serilog) و قابلیت مشاهده (Observability) با OpenTelemetry
- پیام‌رسانی ناهمگام و پردازش پس‌زمینه (RabbitMQ، MassTransit، Quartz.NET)
- ذخیره‌سازی فایل (MinIO / ذخیره‌ساز شیء سازگار با S3)
- جستجوی تمام‌متن سازمانی (Meilisearch / Elasticsearch)
- هسته هوش مصنوعی و موتور برداری (Semantic Kernel و Qdrant)

---

# معماری یکپارچه ماژولار (Modular Monolith)

قابلیت‌های کسب‌وکار به عنوان ماژول‌های مجزا پیاده‌سازی می‌شوند.

هر ماژول مالک موارد زیر است:

- منطق دامنه
- دستورات (Commands) و پرس‌وجوهای (Queries) کاربرد
- طرحواره ذخیره‌سازی داده
- قراردادهای سرویس عمومی

ماژول‌ها از طریق رویدادهای دامنه (Domain Events) یا واسط‌های صریح، به صورت ناهمگام با یکدیگر ارتباط برقرار می‌کنند.

---

# طراحی مبتنی بر دامنه (Domain Driven Design)

این معماری از «طراحی مبتنی بر دامنه» (DDD) استفاده می‌کند.

مفاهیم اصلی شامل موارد زیر است:

- محدوده‌های زمینه (Bounded Contexts)
- مجموعه‌ها و موجودیت‌ها (Aggregates & Entities)
- اشیاء مقدار (Value Objects)
- رویدادهای دامنه (Domain Events)
- خدمات دامنه (Domain Services)

مدل‌های جامع دامنه در مسیر `docs/03-domain` نگهداری می‌شوند.

---

# الگوی CQRS و خط لوله رویداد

فرمان‌ها (Commands) وضعیت سیستم را تغییر داده و ناورداها (Invariants) را اعمال می‌کنند.

پرس‌وجوها (Queries) پروجکشن‌های بهینه‌سازی‌شده برای خواندن را بدون تغییر در وضعیت سیستم اجرا می‌کنند.

ارسال درخواست‌ها و دغدغه‌های میان‌بر (Cross-cutting concerns) در خط لوله، از طریق رفتارهای (Behaviors) کتابخانه MediatR انجام می‌شود.

---

# چند مستاجری (Multi-Tenancy) و فضای کاری توزیع‌شده

این پلتفرم از توپولوژی‌های استقرار برای چندین شرکت و فضای کاری مختلف پشتیبانی می‌کند:

- فضای کاری ابری مرکزی سازمانی (Enterprise Central Cloud Workspace)
- فضاهای کاری پروژه‌ای منطقه‌ای / میدانی
- فضاهای کاری کاربری موبایل / آفلاین اختصاصی

همگام‌سازی داده‌های فضای کاری، مرزهای مستاجران و یکپارچگی کسب‌وکار را از طریق بسته‌های همگام‌سازی‌شده (Synchronized Packages) حفظ می‌کند.

---

# مشاهده‌پذیری (Observability)

مشاهده‌پذیری مستقیماً در تمامی لایه‌های برنامه پیاده‌سازی شده است:

- لاگ‌گذاری ساختاریافته (Serilog)
- رهگیری توزیع‌شده و متریک‌ها (OpenTelemetry, Prometheus, Jaeger)
- بررسی‌های سلامت و نقاط پایانی تشخیصی (Health Checks & Diagnostic Endpoints)

---

# پشته تکنولوژی (Technology Stack)

| لایه | تکنولوژی اصلی | ADR / TE مربوطه |
|-------|--------------------|------------------|
| زمان اجرا (Runtime) | .NET 10 | ADR-0001 / TE-0001 |
| رابط کاربری وب | Blazor Server | ADR-0004 / TE-0002 |
| کامپوننت‌های رابط کاربری | MudBlazor | ADR-0008 / TE-0003 |
| رابط کاربری کلاینت | .NET MAUI & Blazor Hybrid | ADR-0013, ADR-0028 / TE-0010, TE-0034 |
| ORM و دسترسی به داده | Entity Framework Core 10 & Dapper | ADR-0006, ADR-0019 / TE-0004, TE-0024 |
| پایگاه داده تعبیه شده | SQLite & LiteDB | ADR-0014 / TE-0011 |
| اعتبارسنجی | FluentValidation | ADR-0007 / TE-0005, TE-0022 |
| نگاشت اشیاء | Mapster | ADR-0010 / TE-0006, TE-0023 |
| موتور CQRS | MediatR | ADR-0003, ADR-0009 / TE-0009 |
| تولید API | REST OpenAPI / NSwag | ADR-0005 / TE-0021 |
| لاگ‌گذاری و تله‌متری | Serilog & OpenTelemetry | ADR-0011 / TE-0007, TE-0008, TE-0017 |
| موتور پیام‌رسانی | MassTransit & RabbitMQ | ADR-0016 / TE-0012 |
| معماری هوش مصنوعی | Semantic Kernel & Qdrant | ADR-0017, ADR-0022, ADR-0023 / TE-0013, TE-0028, TE-0029 |
| ذخیره‌سازی فایل | MinIO / S3 Object Store | ADR-0020 / TE-0026 |
| موتور جستجو | Meilisearch / Elasticsearch | ADR-0021 / TE-0027 |
| موتور تست | xUnit, Testcontainers, K6 | ADR-0024, ADR-0027 / TE-0030, TE-0033 |
| امنیت و هویت | OpenID Connect & Keycloak | ADR-0026 / TE-0020, TE-0032 |
| ساخت و استقرار | Docker, Kubernetes, GitHub Actions | ADR-0015, ADR-0025 / TE-0031 |
| گزارش‌گیری | QuestPDF & FastReport | ADR-0029 / TE-0035 |

---

---

# خلاصه تصمیمات

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

## چشم‌انداز و نقشه راه

- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`

---

## دامنه‌ها و محدوده‌های زمینه

- `../03-domain/02-BoundedContexts.md`

---

## بنیاد معماری و مدل‌ها

- `00-TechnologyEvaluationTemplate.md`
- `02-CapabilityModel.md`
- `03-TechnologyGapAnalysis.md`

---

## فهرست جامع ADR

- `../06-decisions/000-ADR-INDEX.md`

---

## فهرست ارزیابی تکنولوژی (TE-0001 تا TE-0035)

| شناسه TE | نام ارزیابی تکنولوژی | مرجع فایل |
|-------|----------------------------|----------------|
| TE-0001 | پلتفرم .NET 10 | `TE-0001-.NET10.md` |
| TE-0002 | فریم‌ورک رابط کاربری وب Blazor | `TE-0002-Blazor.md` |
| TE-0003 | کتابخانه کامپوننت MudBlazor | `TE-0003-MudBlazor.md` |
| TE-0004 | دسترسی به داده با Entity Framework Core 10 | `TE-0004-EntityFrameworkCore.md` |
| TE-0005 | معماری FluentValidation | `TE-0005-FluentValidation.md` |
| TE-0006 | نگاشت اشیاء با Mapster | `TE-0006-Mapster.md` |
| TE-0007 | موتور لاگ‌گذاری Serilog | `TE-0007-Serilog.md` |
| TE-0008 | مشاهده‌پذیری با OpenTelemetry | `TE-0008-OpenTelemetry.md` |
| TE-0009 | موتور خط لوله CQRS با MediatR | `TE-0009-Use-MediatR.md` |
| TE-0010 | فریم‌ورک کلاینت دسکتاپ و موبایل | `TE-0010-Desktop-Mobile-Framework-Evaluation.md` |
| TE-0011 | پایگاه داده فضای کاری تعبیه شده | `TE-0011-Embedded-Workspace-Database-Evaluation.md` |
| TE-0012 | تکنولوژی پیام‌رسانی سازمانی | `TE-0012-Enterprise-Messaging-Technology-Evaluation.md` |
| TE-0013 | یکپارچه‌سازی هوش مصنوعی | `TE-0013-Artificial-Intelligence-Technology-Evaluation.md` |
| TE-0014 | موتور پردازش پس‌زمینه | `TE-0014-Background-Processing-Technology-Evaluation.md` |
| TE-0015 | معماری کشینگ (.NET 10) | `TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md` |
| TE-0016 | معماری جستجوی سازمانی | `TE-0016-Enterprise-Search-Architecture-Evaluation.md` |
| TE-0017 | استراتژی مشاهده‌پذیری و تله‌متری | `TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` |
| TE-0018 | مدیریت پیکربندی و اسرار (Secrets) | `TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` |
| TE-0019 | پردازش پس‌زمینه و زمان‌بندی وظایف | `TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md` |
| TE-0020 | استراتژی احراز هویت و هویت | `TE-0020-Authentication-and-Identity-Technology-Evaluation.md` |
| TE-0021 | مستندسازی API و تولید کلاینت | `TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` |
| TE-0022 | معماری خط لوله اعتبارسنجی | `TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` |
| TE-0023 | استراتژی و تکنولوژی نگاشت اشیاء | `TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` |
| TE-0024 | ارزیابی معماری دسترسی به داده | `TE-0024-Data-Access-Architecture-Evaluation.md` |
| TE-0025 | استراتژی مهاجرت پایگاه داده | `TE-0025-Database-Migration-Technology-Evaluation.md` |
| TE-0026 | معماری ذخیره‌سازی فایل | `TE-0026-File-Storage-Technology-Evaluation.md` |
| TE-0027 | استراتژی موتور جستجوی سازمانی | `TE-0027-Search-Engine-Technology-Evaluation.md` |
| TE-0028 | ارزیابی موتور پایگاه داده برداری | `TE-0028-Vector-Database-Technology-Evaluation.md` |
| TE-0029 | استراتژی و ارزیابی ارائه‌دهنده هوش مصنوعی | `TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` |
| TE-0030 | استراتژی تست سازمانی | `TE-0030-Testing-Technology-Evaluation.md` |
| TE-0031 | استراتژی بسته‌بندی ساخت و استقرار | `TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` |
| TE-0032 | استراتژی امنیت سازمانی | `TE-0032-Security-Technology-Evaluation.md` |
| TE-0033 | تست کارایی و بار | `TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` |
| TE-0034 | استراتژی و فریم‌ورک‌های رابط کاربری کلاینت | `TE-0034-Client-UI-Technology-Evaluation.md` |
| TE-0035 | معماری گزارش‌گیری سازمانی | `TE-0035-Reporting-Technology-Evaluation.md` |

---

# حاکمیت معماری

تصمیمات معماری توسط موارد زیر کنترل می‌شوند:

- فهرست سوابق تصمیمات معماری (ADR) (`../06-decisions/000-ADR-INDEX.md`)
- ثبت ارزیابی‌های فنی (`TE-0001` تا `TE-0035`)
- استاندارد مستندسازی نسخه 4.0.0

هیچ تغییر معماری نباید فرآیند ADR را دور بزند.

---

# تاریخچه بازنگری

| نسخه | تاریخ | نویسنده | شرح |
|---------|------------|--------------------|-------------|
| 1.0.0   | 2026-07-18 | معمار راهکار | یادداشت‌های اولیه معماری |
| 3.0.0   | 2026-07-18 | معمار راهکار | استانداردسازی مطابق با استاندارد مستندسازی نسخه 3.0 |
| 4.0.0   | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه 4.0.0؛ پیوند کامل تمام 35 فایل TE و فهرست اصلی ADR |