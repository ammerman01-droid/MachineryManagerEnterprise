# تحلیل شکاف فناوری

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | ARCH-009 |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین بروزرسانی** | 2026-07-28 |

---

# هدف

این سند قابلیت‌های معماری که نیازمند ارزیابی‌های فنی و تصمیم‌گیری‌های رسمی در سطح پلتفرم MachineryManagerEnterprise هستند را شناسایی می‌کند.

هدف اصلی آن اطمینان از این است که هر قابلیت معماری که در طول تکامل پلتفرم معرفی می‌شود، پیش از پیاده‌سازی، به صورت نظام‌مند در برابر چشم‌انداز فناوری موجود ارزیابی گردد.

تحلیل شکاف فناوری (Technology Gap Analysis) به عنوان پلی میان معماری سیستم (`01-Architecture.md`)، قابلیت‌های کسب‌وکار (`02-CapabilityModel.md`)، سوابق تصمیم‌گیری معماری (`../06-decisions/000-ADR-INDEX.md`)، ارزیابی‌های فنی (`TE-0001` تا `TE-0035`) و برنامه‌ریزی پیاده‌سازی عمل می‌کند.

این سند تضمین می‌کند که:

- هر قابلیت معماری توسط فناوری‌های مناسب پشتیبانی می‌شود؛
- پذیرش فناوری همچنان هدفمند و کاملاً قابل ردیابی باقی می‌ماند؛
- فناوری‌های جدید تنها از طریق تصمیمات معماری رسمی معرفی می‌شوند؛
- پشته فناوری مصوب به شیوه‌ای کنترل‌شده و مستند تکامل می‌یابد؛
- ثبات معماری در طول چرخه عمر پلتفرم حفظ می‌شود.

این سند شکاف‌های فناوری را شناسایی می‌کند.

این سند فناوری‌ها را انتخاب نمی‌کند.

انتخاب فناوری همواره باید از طریق اسناد مصوب ADR و ارزیابی فنی (Technical Evaluation) صورت گیرد.

---

# دامنه

این سند موارد زیر را پوشش می‌دهد:

- فناوری‌های زمان اجرا (Runtime) سمت سرور و هسته پلتفرم (.NET 10, C#)
- فناوری‌های ارائه (Presentation) کلاینت، دسکتاپ و موبایل (.NET MAUI, Blazor Server, MudBlazor)
- فناوری‌های پایگاه داده و ماندگاری داده (Entity Framework Core, SQLite, LiteDB, Dapper, PostgreSQL)
- فناوری‌های نگاشت شیء، اعتبارسنجی و خط لوله CQRS (Mapster, FluentValidation, MediatR)
- زیرساخت پیام‌رسانی سازمانی و گذرگاه رویداد (MassTransit, RabbitMQ)
- فناوری‌های هوش مصنوعی، موتور برداری و مسیریاب ارائه‌دهنده (Semantic Kernel, Qdrant, Ollama)
- فناوری‌های جستجوی تمام‌متن سازمانی (Meilisearch, Elasticsearch)
- فناوری‌های ذخیره‌سازی فایل (MinIO / S3 Object Store)
- امنیت، احراز هویت و مدیریت هویت (OpenID Connect, Keycloak)
- تست، تضمین کیفیت و تست عملکرد (xUnit, Testcontainers, K6, NBomber)
- ساخت، بسته‌بندی، استقرار و خط لوله‌های CI/CD (GitHub Actions, Docker, Kubernetes)
- موتورهای گزارش‌گیری و خروجی BI (QuestPDF, FastReport)

قوانین کسب‌وکار عمداً خارج از دامنه این سند قرار دارند.

---

# روش‌شناسی ارزیابی

هر قابلیت معماری با استفاده از توالی تصمیم‌گیری زیر ارزیابی می‌شود.

```text
قابلیت کسب‌وکار جدید
        │
        ▼
تحلیل قابلیت
        │
        ▼
آیا توسط فناوری مصوب پوشش داده شده است؟
   │                   │
  بله                  خیر
   │                   │
   ▼                   ▼
نیازی به اقدام       شکاف فناوری شناسایی شد
نیست                   │
                       ▼
              نیازمند ADR و TE
                       │
                       ▼
              انتخاب فناوری
                       │
                       ▼
              معماری تایید شد
```

هر قابلیت در یکی از دسته‌بندی‌های زیر طبقه‌بندی می‌شود.

| وضعیت | معنا |
|---------|---------|
| تحت پوشش (Covered) | به طور کامل توسط فناوری‌های مصوب موجود پشتیبانی می‌شود |
| جزئی (Partial) | فناوری‌های موجود از قابلیت پشتیبانی می‌کنند، اما نیازمند تصمیمات معماری اضافی است |
| فقدان (Missing) | در حال حاضر هیچ فناوری مصوبی از این قابلیت پشتیبانی نمی‌کند |

# ماتریس پوشش فناوری و تحلیل شکاف

ماتریس زیر تمامی قابلیت‌های کلیدی معماری را در برابر پشته فناوری مصوب، ارزیابی‌های فنی (TE) مربوطه و سوابق تصمیمات معماری (ADR) ارزیابی می‌کند.

| شناسه شکاف | قابلیت معماری | استاندارد فناوری انتخاب‌شده | مرجع ADR | مرجع TE | اولویت | وضعیت |
|--------|---------------------------|------------------------------|---------------|--------------|----------|--------|
| GAP-001 | زمان اجرای پلتفرم اصلی | .NET 10 | ADR-0001, ADR-0002 | `TE-0001-.NET10.md` | بالا | تایید شده ✅ |
| GAP-002 | چارچوب رابط کاربری وب | Blazor Server | ADR-0004 | `TE-0002-Blazor.md` | بالا | تایید شده ✅ |
| GAP-003 | کتابخانه اجزای رابط کاربری | MudBlazor | ADR-0008 | `TE-0003-MudBlazor.md` | بالا | تایید شده ✅ |
| GAP-004 | دسترسی به داده ORM | Entity Framework Core 10 | ADR-0006 | `TE-0004-EntityFrameworkCore.md` | بالا | تایید شده ✅ |
| GAP-005 | اعتبارسنجی روان (Fluent) | FluentValidation | ADR-0007 | `TE-0005-FluentValidation.md` | بالا | تایید شده ✅ |
| GAP-006 | نگاشت شیء DTO | Mapster | ADR-0010 | `TE-0006-Mapster.md` | بالا | تایید شده ✅ |
| GAP-007 | لاگ‌گذاری ساختاریافته | Serilog | ADR-0011 | `TE-0007-Serilog.md` | بالا | تایید شده ✅ |
| GAP-008 | مشاهده‌پذیری و ردیابی | OpenTelemetry | ADR-0011 | `TE-0008-OpenTelemetry.md` | بالا | تایید شده ✅ |
| GAP-009 | دیسپچر CQRS | MediatR | ADR-0003, ADR-0009 | `TE-0009-Use-MediatR.md` | بالا | تایید شده ✅ |
| GAP-010 | کلاینت‌های قابل نصب (دسکتاپ و موبایل) | .NET MAUI | ADR-0013 | `TE-0010-Desktop-Mobile-Framework-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-011 | پایگاه داده فضای کاری آفلاین | SQLite & LiteDB | ADR-0014 | `TE-0011-Embedded-Workspace-Database-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-012 | پیام‌رسانی سازمانی | MassTransit & RabbitMQ | ADR-0016 | `TE-0012-Enterprise-Messaging-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-013 | موتور یکپارچه‌سازی هوش مصنوعی | Semantic Kernel | ADR-0017 | `TE-0013-Artificial-Intelligence-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-014 | پردازش پس‌زمینه | Quartz.NET / Channels | ADR-0015 | `TE-0014-Background-Processing-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-015 | معماری کشینگ | حافظه ترکیبی و کش توزیع‌شده | ADR-0019 | `TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md` | متوسط | تایید شده ✅ |
| GAP-016 | معماری جستجوی سازمانی | Meilisearch Engine | ADR-0021 | `TE-0016-Enterprise-Search-Architecture-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-017 | خط لوله مشاهده‌پذیری سازمانی | Prometheus, Grafana, OpenTelemetry | ADR-0011 | `TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-018 | مدیریت اسرار و پیکربندی | Environment & HashiCorp Vault | ADR-0018 | `TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-019 | استراتژی زمان‌بندی وظایف | Quartz.NET Engine | ADR-0015 | `TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-020 | هویت و امنیت | Keycloak / OpenID Connect | ADR-0026 | `TE-0020-Authentication-and-Identity-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-021 | تولید کلاینت API | OpenAPI & NSwag | ADR-0005 | `TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-022 | خط لوله اعتبارسنجی | MediatR Validation Behavior | ADR-0007 | `TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-023 | نگاشت با کارایی بالا | Mapster Compiler Projections | ADR-0010 | `TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-024 | ماندگاری پرس‌وجوهای سنگین (Read-Heavy) | Dapper & Read Replicas | ADR-0019 | `TE-0024-Data-Access-Architecture-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-025 | مهاجرت‌های پایگاه داده | EF Core Migrations & Respawn | ADR-0014 | `TE-0025-Database-Migration-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-026 | ذخیره‌سازی فایل شیء | S3 / MinIO | ADR-0020 | `TE-0026-File-Storage-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-027 | یکپارچه‌سازی موتور جستجو | Meilisearch / Elasticsearch | ADR-0021 | `TE-0027-Search-Engine-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-028 | جستجوی برداری و RAG | Qdrant Vector Engine | ADR-0022 | `TE-0028-Vector-Database-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-029 | مسیریاب ارائه‌دهنده هوش مصنوعی | موتور چند-ارائه‌دهنده‌ای (Ollama/OpenAI) | ADR-0023 | `TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-030 | اتوماسیون تست سازمانی | xUnit, Moq, Testcontainers | ADR-0024 | `TE-0030-Testing-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-031 | بسته‌بندی و استقرار | Docker & GitHub Actions | ADR-0025 | `TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-032 | مقاوم‌سازی امنیت سازمانی | TLS, Secret Vault, RBAC | ADR-0026 | `TE-0032-Security-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-033 | تست کارایی و بار | K6 & NBomber | ADR-0027 | `TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` | متوسط | تایید شده ✅ |
| GAP-034 | چارچوب ترکیبی رابط کاربری کلاینت | Blazor Hybrid Controls | ADR-0028 | `TE-0034-Client-UI-Technology-Evaluation.md` | بالا | تایید شده ✅ |
| GAP-035 | موتور گزارش‌گیری سازمانی | QuestPDF & FastReport | ADR-0029 | `TE-0035-Reporting-Technology-Evaluation.md` | بالا | تایید شده ✅ |

---

# خلاصه‌ی جزئیات ارزیابی شکاف‌ها (Gap Evaluation)

### GAP-010 — چارچوب توزیع‌شده دسکتاپ و موبایل
- **قابلیت**: برنامه‌ی کاری قابل نصب در تمامی پلتفرم‌ها (دسکتاپ Windows/macOS، موبایل Android/iOS)
- **وضعیت**: تایید شده ✅
- **تصمیم معماری**: ADR-0013 — معماری برنامه کلاینت
- **ارزیابی فناوری**: `TE-0010-Desktop-Mobile-Framework-Evaluation.md`
- **فناوری منتخب**: .NET MAUI & Blazor Hybrid

### GAP-011 — پایگاه داده محلی تعبیه‌شده (Embedded)
- **قابلیت**: پایداری فضای کاری آفلاین و همگام‌سازی محلی بسته‌ها
- **وضعیت**: تایید شده ✅
- **تصمیم معماری**: ADR-0014 — پایگاه داده تعبیه‌شده فضای کاری
- **ارزیابی فناوری**: `TE-0011-Embedded-Workspace-Database-Evaluation.md`
- **فناوری منتخب**: SQLite (رابطه‌ای ساختاریافته) و LiteDB (ذخیره‌ساز اسناد)

### GAP-012 — پیام‌رسانی سازمانی
- **قابلیت**: گذرگاه رویداد ناهمگام بین ماژول‌ها و توزیع پیام
- **وضعیت**: تایید شده ✅
- **تصمیم معماری**: ADR-0016 — معماری پیام‌رسانی سازمانی
- **ارزیابی فناوری**: `TE-0012-Enterprise-Messaging-Technology-Evaluation.md`
- **فناوری منتخب**: MassTransit بر روی RabbitMQ

### GAP-013 — یکپارچه‌سازی هوش مصنوعی
- **قابلیت**: دستیار هوش مصنوعی، هسته تشخیصی و بازیابی دانش
- **وضعیت**: تایید شده ✅
- **تصمیم معماری**: ADR-0017 — یکپارچه‌سازی هوش مصنوعی
- **ارزیابی فناوری**: `TE-0013-Artificial-Intelligence-Technology-Evaluation.md`
- **فناوری منتخب**: Semantic Kernel Engine و Multi-Provider Model Router

---

# نقشه راه پیاده‌سازی با اولویت معماری (Architecture First)

این پروژه از متدولوژی دقیق «اولویت با معماری» پیروی می‌کند.

```text
Business Requirement -> Capability Model -> Technology Gap Analysis -> ADR -> TE -> Implementation
```

تمام ۳۵ شکاف فناوری شناسایی شده در سراسر پلتفرم، ارزیابی‌های فنی مربوطه (`TE-0001` تا `TE-0035`) و اسناد تصمیم معماری (`ADR-0001` تا `ADR-0029`) خود را تکمیل کرده‌اند.

---

# خلاصه تصمیمات

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

- `01-Architecture.md`
- `02-CapabilityModel.md`
- `00-TechnologyEvaluationTemplate.md`
- `../06-decisions/000-ADR-INDEX.md`
- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`
- `TE-0001-.NET10.md` تا `TE-0035-Reporting-Technology-Evaluation.md`

---

# تاریخچه بازنگری

| نسخه | تاریخ | نویسنده | شرح |
|---------|------|--------|-------------|
| 1.0.0 | 2026-07-26 | معمار راهکار (Solution Architect) | تحلیل اولیه شکاف‌های فناوری |
| 4.0.0 | 2026-07-28 | معمار راهکار (Solution Architect) | ارتقا به استاندارد مستندسازی v4.0.0؛ گسترش ماتریس شکاف برای پوشش تمام ۳۵ مورد TE و ۲۹ مورد ADR |