| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | ARCH-001 |
| **عنوان** | معماری سیستم (System Architecture) |
| **نسخه** | 4.2.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند معماری کلی نرم‌افزار پلتفرم MachineryManagerEnterprise را توصیف می‌نماید.

این سند دیدگاه کلان معماری را ارائه داده و به عنوان نقطه ورود مرکزی برای تمامی مستندات مرتبط با معماری عمل می‌کند.

تصمیمات تفصیلی پیاده‌سازی به صورت جداگانه در اسناد ثبت تصمیمات معماری (ADR) و ارزیابی‌های فناوری (TE) مستند شده‌اند.

---

# چشم‌انداز معماری (Architectural Vision)

سیستم به صورت زیر پیاده‌سازی خواهد شد:

- مونولیت ماژولار (Modular Monolith)
- معماری پاک (Clean Architecture)
- طراحی دامنه‌محور (Domain Driven Design - DDD)
- برنامه مبتنی بر CQRS
- پلتفرم سازمانی چندمستاجره (Multi-Tenant Enterprise Platform)
- معماری کلاینت با ورک‌اسپیس توزیع‌شده و اولویت آفلاین (Distributed Workspace & Offline-First Client Architecture)

معماری برای بیشینه‌سازی قابلیت نگهداری، گسترش‌پذیری، آزمون‌پذیری، امنیت، و پایداری بلندمدت طراحی شده است.

---

# اصول معماری (Architecture Principles)

معماری از این اصول هسته‌ای پیروی می‌کند:

- تفکیک وظایف (Separation of Concerns)
- قاعده وابستگی (جهت به سمت داخل) (Dependency Rule - Inward Direction)
- انسجام بالا و جفت‌شدگی پایین (High Cohesion & Low Coupling)
- مرزهای صریح ماژول‌ها (Explicit Module Boundaries)
- مستندسازی-نخست (Documentation First)
- اولویت متن‌باز (ADR-0002)
- امنیت بر پایه طراحی (Security by Design - ADR-0026)
- بی‌طرفی ابری و عملیات توزیع‌شده آفلاین (Cloud Neutrality & Distributed Offline Operation - ADR-0012)

---

# معماری سطح بالا (High-Level Architecture)

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

وابستگی‌ها همواره به سمت داخل و به طرف هسته Domain اشاره دارند.

لایه Domain شامل منطق تجاری خالص بوده و هیچ وابستگی به فریم‌ورک‌های زیرساختی ندارد.

---

# لایه‌های معماری (Architectural Layers)

## لایه نمایش (Presentation)

مسئول موارد زیر است:

- رابط کاربری وب (Blazor Server & MudBlazor)
- برنامه‌های کلاینت دسکتاپ و موبایل (.NET MAUI & Blazor Hybrid)
- اندپوینت‌های RESTful HTTP API و مشخصات OpenAPI
- احراز هویت، صدور مجوز و مدیریت هویت (ASP.NET Core Identity / OpenIddict)

---

## لایه کاربرد (Application)

مسئول موارد زیر است:

- ارکستراسیون موارد استفاده (Use Case Orchestration)
- پیاده‌سازی الگوی CQRS از طریق MediatR
- رفتارهای خط لوله (لاگ‌گیری، اعتبارسنجی، پایش عملکرد، محدوده تراکنش)
- اعتبارسنجی ورودی‌ها از طریق FluentValidation
- نگاشت DTO و مدل دامنه از طریق Mapster

---

## لایه دامنه (Domain)

مسئول موارد زیر است:

- قواعد اصلی کسب‌وکار و خط‌مشی چرخه حیات دارایی‌ها
- مجتمع‌ها (Aggregates)، موجودیت‌ها (Entities) و اشیاء مقدار (Value Objects) دامنه
- اعزام و مدیریت رویدادهای دامنه (Domain Event Dispatching & Handling)
- منطق دامنه مدیریت دارایی‌ها، نگهداری و تعمیرات، قطعات و سنجه‌ها/کنتورها

لایه دامنه فاقد هرگونه دغدغه زیرساختی است.

---

## لایه زیرساخت (Infrastructure)

مسئول موارد زیر است:

- ماندگاری پایگاه داده رابطه‌ای (Entity Framework Core & Dapper)
- ذخیره‌سازی محلی توکار برای فضاهای کاری آفلاین (SQLite & LiteDB)
- موتور همگام‌سازی بسته‌های فضای کاری توزیع‌شده
- لاگ‌گیری ساختاریافته (Serilog) و مشاهده‌پذیری OpenTelemetry
- پیام‌رسانی ناهمگام و پردازش پس‌زمینه (RabbitMQ, MassTransit, Quartz.NET)
- ذخیره‌سازی فایل (مخزن شیء سازگار با MinIO / S3)
- جستجوی تمام‌متن سازمانی (Meilisearch / Elasticsearch)
- کرنل هوش مصنوعی و موتور برداری (Semantic Kernel & Qdrant)

---

# مونولیت ماژولار (Modular Monolith)

قابلیت‌های تجاری به عنوان ماژول‌های ایزوله پیاده‌سازی می‌شوند.

هر ماژول مالک موارد زیر برای خود است:

- منطق دامنه
- دستورات و پرس‌وجوهای کاربردی (Application Commands & Queries)
- اسکیمای ذخیره‌سازی داده‌ها
- قراردادهای خدمات عمومی (Public Service Contracts)

ماژول‌ها به صورت ناهمگام از طریق رویدادهای دامنه یا اینترفیس‌های صریح ارتباط برقرار می‌کنند.

---

# طراحی دامنه‌محور (Domain Driven Design)

معماری از رویکرد طراحی دامنه‌محور (DDD) استفاده می‌کند.

مفاهیم اصلی عبارتند از:

- محدوده‌های زمینه‌ای (Bounded Contexts)
- مجتمع‌ها و موجودیت‌ها (Aggregates & Entities)
- اشیاء مقدار (Value Objects)
- رویدادهای دامنه (Domain Events)
- سرویس‌های دامنه (Domain Services)

مدل‌های تفصیلی دامنه تحت مسیر `docs/03-domain` نگهداری می‌شوند.

---

# الگوی CQRS و خط لوله رویداد (CQRS & Event Pipeline)

دستورات (Commands) وضعیت را تغییر داده و ناورداری‌ها را اعمال می‌کنند.

پرس‌وجوها (Queries) پروجکشن‌های بهینه‌شده برای خواندن را بدون تغییر در وضعیت اجرا می‌نمایند.

اعزام درخواست‌ها و دغدغه‌های متقاطع خط لوله از طریق رفتارهای MediatR اجرا می‌شوند.

---

# چندمستاجری و فضای کاری توزیع‌شده (Multi-Tenancy & Distributed Workspace)

پلتفرم از توپولوژی‌های استقرار چندشرکتی و چند فضاکاری پشتیبانی می‌کند:

- فضای کاری ابری مرکزی سازمانی
- فضاهای کاری پروژه‌های منطقه‌ای / میدانی
- فضاهای کاری انفرادی موبایل / آفلاین کاربران

همگام‌سازی داده‌های فضای کاری، مرزهای مستاجران و یکپارچگی تجاری را از طریق بسته‌های همگام‌سازی‌شده حفظ می‌نماید.

---

# مشاهده‌پذیری (Observability)

مشاهده‌پذیری مستقیماً در تمامی لایه‌های برنامه تعبیه شده است:

- لاگ‌گیری ساختاریافته (Serilog)
- ردگیری توزیع‌شده و معیارها (OpenTelemetry, Prometheus, Jaeger)
- بررسی‌های سلامت و اندپوینت‌های تشخیصی

---

# پشته فناوری‌ها (Technology Stack)

| لایه | فناوری اصلی | ADR / TE مرتبط |
|-------|--------------------|------------------|
| محیط اجرا (Runtime) | .NET 10 | ADR-0003 / TE-0001 |
| رابط وب (Web UI) | Blazor (Server / WebAssembly) | ADR-0004 / TE-0002 |
| مولفه‌های UI | MudBlazor | ADR-0005 / TE-0003 |
| رابط کاربری کلاینت (دسکتاپ و موبایل) | .NET MAUI | ADR-0013 / TE-0010 |
| نگاشت شیء-رابطه‌ای و دسترسی به داده | Entity Framework Core 10 & Dapper | ADR-0006, ADR-0019 / TE-0004, TE-0024 |
| مهاجرت پایگاه داده | EF Core Migrations | ADR-0037 / TE-0025 |
| پایگاه داده توکار | SQLite & LiteDB | ADR-0014 / TE-0011 |
| اعتبارسنجی | FluentValidation & MediatR Pipeline Behavior | ADR-0007, ADR-0036 / TE-0005, TE-0022 |
| نگاشت اشیاء | Mapster | ADR-0008 / TE-0006, TE-0023 |
| موتور CQRS | MediatR | ADR-0011 / TE-0009 |
| مستندسازی API و تولید کلاینت | OpenAPI, Scalar, NSwag | ADR-0035 / TE-0021 |
| لاگ‌گیری و تله‌متری | Serilog, OpenTelemetry, Prometheus, Grafana | ADR-0009, ADR-0010, ADR-0033 / TE-0007, TE-0008, TE-0017 |
| موتور پیام‌رسانی | MassTransit & RabbitMQ | ADR-0016 / TE-0012 |
| یکپارچه‌سازی خارجی | فریم‌ورک کانکتور مبتنی بر MassTransit (+ Azure Logic Apps، اختیاری) | ADR-0018 / TE-0036 |
| معماری هوش مصنوعی | Semantic Kernel, Qdrant, Azure OpenAI / OpenAI / Ollama | ADR-0017, ADR-0022, ADR-0023 / TE-0013, TE-0028, TE-0029 |
| ذخیره‌سازی فایل | مخزن شیء MinIO / S3 | ADR-0020 / TE-0026 |
| موتور جستجو | SQL Server FTS (پیش‌فرض) + OpenSearch (افزایش مقیاس) | ADR-0021 / TE-0027 |
| پیکربندی و اسرار | Microsoft.Extensions.Configuration/Options & HashiCorp Vault | ADR-0034 / TE-0018 |
| کش‌گذاری | FusionCache, IMemoryCache, Redis (L2) | ADR-0031 / TE-0015 |
| پردازش پس‌زمینه و زمان‌بندی | Quartz.NET & System.Threading.Channels | ADR-0032 / TE-0014, TE-0019 |
| موتور آزمون | xUnit, Moq, Testcontainers, k6, NBomber | ADR-0024, ADR-0027 / TE-0030, TE-0033 |
| امنیت و هویت | ASP.NET Core Identity & OpenIddict (هویت)؛ Data Protection, AES-256, X.509 (رمزنگاری) | ADR-0030 / TE-0020؛ ADR-0026 / TE-0032 |
| ساخت و استقرار | Docker, GitHub Actions, .NET Aspire | ADR-0025 / TE-0031 |
| گزارش‌گیری | QuestPDF (کنار گذاشتن FastReport و RDLC) | ADR-0029 / TE-0035 |

> **نکته:** فریم‌ورک Avalonia UI (سند TE-0034 / ADR-0028) به طور مستقل برای لایه رابط کاربری کلاینت ارزیابی شد و به نفع .NET MAUI در بالا **جایگزین‌شده (Superseded)** است. این فریم‌ورک بخشی از پشته فعال سیستم نیست.

---

# اسناد مرتبط (Related Documents)

## چشم‌انداز و نقشه راه (Vision & Roadmap)

- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`

---

## دامنه و محدوده‌های زمینه‌ای (Domain & Bounded Contexts)

- `../03-domain/02-BoundedContexts.md`

---

## شالوده و مدل‌های معماری (Architecture Foundation & Models)

- `00-TechnologyEvaluationTemplate.md`
- `02-CapabilityModel.md`
- `03-TechnologyGapAnalysis.md`

---

## نمایه اصلی ADRها (ADR Master Index)

- `../06-decisions/000-ADR-INDEX.md`

---

## نمایه ارزیابی‌های فناوری (TE-0001 تا TE-0035)

| شناسه TE | نام ارزیابی فناوری | ارجاع به فایل |
|-------|----------------------------|----------------|
| TE-0001 | پلتفرم .NET 10 | `TE-0001-.NET10.md` |
| TE-0002 | فریم‌ورک رابط وب Blazor | `TE-0002-Blazor.md` |
| TE-0003 | کتابخانه مولفه‌های رابط کاربری MudBlazor | `TE-0003-MudBlazor.md` |
| TE-0004 | دسترسی داده با Entity Framework Core 10 | `TE-0004-EntityFrameworkCore.md` |
| TE-0005 | معماری FluentValidation | `TE-0005-FluentValidation.md` |
| TE-0006 | نگاشت اشیاء Mapster | `TE-0006-Mapster.md` |
| TE-0007 | موتور لاگ‌گیری Serilog | `TE-0007-Serilog.md` |
| TE-0008 | مشاهده‌پذیری OpenTelemetry | `TE-0008-OpenTelemetry.md` |
| TE-0009 | موتور خط لوله CQRS با MediatR | `TE-0009-Use-MediatR.md` |
| TE-0010 | فریم‌ورک کلاینت دسکتاپ و موبایل | `TE-0010-Desktop-Mobile-Framework-Evaluation.md` |
| TE-0011 | پایگاه داده فضای کاری توکار | `TE-0011-Embedded-Workspace-Database-Evaluation.md` |
| TE-0012 | فناوری پیام‌رسانی سازمانی | `TE-0012-Enterprise Messaging Technology Evaluation.md` |
| TE-0013 | یکپارچه‌سازی هوش مصنوعی | `TE-0013-Artificial-Intelligence-Technology-Evaluation.md` |
| TE-0014 | موتور پردازش پس‌زمینه | `TE-0014-Background Processing Technology Evaluation.md` |
| TE-0015 | معماری کش‌گذاری (.NET 10) | `TE-0015-Caching Architecture Technology Evaluation (.NET 10).md` |
| TE-0016 | معماری جستجوی سازمانی | `TE-0016-Enterprise-Search-Architecture-Evaluation.md` |
| TE-0017 | استراتژی مشاهده‌پذیری و تله‌متری | `TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` |
| TE-0018 | مدیریت پیکربندی و اسرار | `TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` |
| TE-0019 | پردازش پس‌زمینه و زمان‌بندی وظایف | `TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md` |
| TE-0020 | استراتژی احراز هویت و شناسایی | `TE-0020-Authentication-and-Identity-Technology-Evaluation.md` |
| TE-0021 | مستندسازی API و تولید کلاینت | `TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` |
| TE-0022 | معماری خط لوله اعتبارسنجی | `TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` |
| TE-0023 | استراتژی و فناوری نگاشت اشیاء | `TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` |
| TE-0024 | ارزیابی معماری دسترسی به داده | `TE-0024-Data-Access-Architecture-Evaluation.md` |
| TE-0025 | استراتژی مهاجرت پایگاه داده | `TE-0025-Database-Migration-Technology-Evaluation.md` |
| TE-0026 | معماری ذخیره‌سازی فایل | `TE-0026-File-Storage-Technology-Evaluation.md` |
| TE-0027 | استراتژی موتور جستجوی سازمانی | `TE-0027-Search-Engine-Technology-Evaluation.md` |
| TE-0028 | ارزیابی موتور پایگاه داده برداری | `TE-0028-Vector-Database-Technology-Evaluation.md` |
| TE-0029 | ارزیابی و استراتژی ارائه‌دهنده هوش مصنوعی | `TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` |
| TE-0030 | استراتژی آزمون سازمانی | `TE-0030-Testing-Technology-Evaluation.md` |
| TE-0031 | استراتژی ساخت، بسته‌بندی و استقرار | `TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` |
| TE-0032 | استراتژی امنیت سازمانی | `TE-0032-Security-Technology-Evaluation.md` |
| TE-0033 | آزمون کارایی و بار | `TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` |
| TE-0034 | فریم‌ورک‌ها و استراتژی رابط کاربری کلاینت | `TE-0034-Client-UI-Technology-Evaluation.md` |
| TE-0035 | معماری گزارش‌گیری سازمانی | `TE-0035-Reporting-Technology-Evaluation.md` |

---

# حاکمیت معماری (Architecture Governance)

تصمیمات معماری توسط مراجع زیر اداره می‌شوند:

- نمایه اسناد ثبت تصمیمات معماری (`../06-decisions/000-ADR-INDEX.md`)
- ثبت ارزیابی‌های فنی (`TE-0001` تا `TE-0035`)
- استاندارد مستندسازی v4.0.0

هیچ تغییر معماری نباید بدون طی فرایند ADR صورت پذیرد.

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---------|------------|--------------------|-------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | یادداشت‌های معماری اولیه |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0؛ اتصال و پیوند کامل تمامی ۳۵ فایل TE و نمایه اصلی ADRها |
| 4.1.0 | 2026-08-02 | معمار راهکار | اصلاح ارجاعات امنیت و هویت: جایگزینی ارجاع نادرست "OpenID Connect & Keycloak" / ADR-0026 با توصیه واقعی TE-0020 (ASP.NET Core Identity & OpenIddict) و ADR-0030 تصویب‌کننده آن |
| 4.2.0 | 2026-08-08 | معمار راهکار | بازسازی کامل جدول پشته فناوری‌ها: اکثر ارجاعات شناسه ADR نادرست بودند (مثلاً MediatR ذیل ADR-0003/0009 به جای ADR-0011 ذکر شده بود؛ Mapster ذیل ADR-0010 به جای ADR-0008؛ .NET 10 ذیل ADR-0001 به جای ADR-0003)؛ موتور جستجو از "Meilisearch/Elasticsearch" به تصمیم واقعی (SQL Server FTS + OpenSearch برای افزایش مقیاس) اصلاح شد؛ گزارش‌گیری برای کنار گذاشتن FastReport اصلاح شد؛ ساخت و استقرار برای حذف Kubernetes (تصویب‌نشده) و ADR-0015 نامرتبط اصلاح شد؛ رابط کاربری کلاینت صرفاً به .NET MAUI اصلاح شد (Avalonia/ADR-0028/TE-0034 به عنوان جایگزین‌شده قید گردید)؛ سطرهای مفقود برای یکپارچه‌سازی خارجی (ADR-0018/TE-0036)، پیکربندی و اسرار (ADR-0034)، کش‌گذاری (ADR-0031)، پردازش پس‌زمینه (ADR-0032)، و مهاجرت پایگاه داده (ADR-0037) اضافه شدند |
