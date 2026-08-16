| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | ARCH-003 |
| **عنوان** | تحلیل شکاف‌های فناوری (Technology Gap Analysis) |
| **نسخه** | 4.2.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار |
| **تاریخ ایجاد** | 2026-07-26 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند قابلیت‌های معماری را که نیازمند ارزیابی‌های فنی و تصمیمات رسمی در سراسر پلتفرم MachineryManagerEnterprise هستند، شناسایی می‌نماید.

هدف اصلی آن اطمینان از این است که هر قابلیت معماری که در طول تکامل پلتفرم معرفی می‌شود، قبل از پیاده‌سازی به صورت روش‌مند در برابر چشم‌انداز فناوری موجود ارزیابی گردد.

تحلیل شکاف‌های فناوری به عنوان پلی میان معماری سیستم (`01-Architecture.md`)، قابلیت‌های کسب‌وکار (`02-CapabilityModel.md`)، اسناد ثبت تصمیمات معماری (`../06-decisions/000-ADR-INDEX.md`)، ارزیابی‌های فنی (`TE-0001` تا `TE-0035`)، و برنامه‌ریزی پیاده‌سازی عمل می‌کند.

این سند تضمین می‌کند که:

- هر قابلیت معماری توسط فناوری‌های مناسب پشتیبانی شود؛
- به‌کارگیری فناوری همواره هدفمند و کاملاً قابل ردگیری باقی بماند؛
- فناوری‌های جدید صرفاً از طریق تصمیمات رسمی معماری معرفی شوند؛
- پشته فناوری‌های تصویب‌شده به شیوه‌ای کنترل‌شده و مستند تکامل یابد؛
- یکپارچگی و سازگاری معماری در سرتاسر چرخه عمر پلتفرم حفظ گردد.

این سند شکاف‌های فناوری را شناسایی می‌کند.

این سند فناوری‌ها را انتخاب نمی‌کند.

انتخاب فناوری همواره باید از طریق اسناد تصویب‌شده ADR و ارزیابی فنی (TE) انجام پذیرد.

---

# دامنه (Scope)

این سند موارد زیر را پوشش می‌دهد:

- فناوری‌های محیط اجرای سمت سرور و پلتفرم هسته (.NET 10, C#)
- فناوری‌های لایه نمایش کلاینت، دسکتاپ و موبایل (.NET MAUI, Blazor Server, MudBlazor)
- فناوری‌های پایگاه داده و ماندگاری (Entity Framework Core, SQLite, LiteDB, Dapper, PostgreSQL)
- فناوری‌های نگاشت اشیاء، اعتبارسنجی و خط لوله CQRS (Mapster, FluentValidation, MediatR)
- زیرساخت پیام‌رسانی سازمانی و گذرگاه رویداد (MassTransit, RabbitMQ)
- فناوری‌های هوش مصنوعی، موتور برداری و مسیریاب ارائه‌دهنده (Semantic Kernel, Qdrant, Ollama)
- فناوری‌های جستجوی تمام‌متن سازمانی (SQL Server FTS, OpenSearch)
- فناوری‌های ذخیره‌سازی فایل (MinIO / S3 Object Store)
- امنیت، احراز هویت و مدیریت هویت (ASP.NET Core Identity, OpenIddict)
- آزمون، تضمین کیفیت و آزمون کارایی (xUnit, Testcontainers, K6, NBomber)
- خطوط لوله ساخت، بسته‌بندی، استقرار و CI/CD (GitHub Actions, Docker, .NET Aspire)
- موتورهای خروجی گزارش‌گیری و هوش تجاری (QuestPDF, FastReport)

قواعد کسب‌وکار به صورت عمدی خارج از دامنه این سند قرار دارند.

---

# متدولوژی ارزیابی (Assessment Methodology)

هر قابلیت معماری با استفاده از توالی تصمیم‌گیری زیر ارزیابی می‌شود.

```text
قابلیت جدید کسب‌وکار (New Business Capability)
        │
        ▼
تحلیل قابلیت (Capability Analysis)
        │
        ▼
آیا توسط فناوری تصویب‌شده پوشش داده شده است؟ (Covered by Approved Technology?)
   │                   │
  بله                  خیر
   │                   │
   ▼                   ▼
اقدامی لازم نیست    شکاف فناوری شناسایی شد (Technology Gap Identified)
(No Action Required)    │
                        ▼
               نیازمند ADR و TE (ADR & TE Required)
                        │
                        ▼
               انتخاب فناوری (Technology Selection)
                        │
                        ▼
               معماری تصویب شد (Architecture Approved)
```

هر قابلیت در یکی از دسته‌بندی‌های زیر طبقه‌بندی می‌شود:

| وضعیت | مفهوم |
|---------|---------|
| Covered (پوشش‌داده‌شده) | به طور کامل توسط فناوری‌های تصویب‌شده موجود پشتیبانی می‌شود |
| Partial (جزئی) | فناوری‌های موجود از قابلیت پشتیبانی می‌کنند، اما تصمیمات معماری تکمیلی الزامی است |
| Missing (فاقد پوشش) | در حال حاضر هیچ فناوری تصویب‌شده‌ای از قابلیت پشتیبانی نمی‌کند |

---

# ماتریس پوشش فناوری و تحلیل شکاف‌ها (Technology Coverage & Gap Analysis Matrix)

ماتریس زیر تمامی قابلیت‌های اصلی معماری را در برابر پشته فناوری‌های تصویب‌شده، ارزیابی‌های فنی متناظر (TE)، و اسناد ثبت تصمیمات معماری (ADR) ارزیابی می‌نماید.

| شناسه شکاف | قابلیت معماری | استاندارد فناوری انتخاب‌شده | مرجع ADR | مرجع TE | اولویت | وضعیت |
|--------|---------------------------|------------------------------|---------------|--------------|----------|--------|
| GAP-001 | محیط اجرای پلتفرم اصلی | .NET 10 | ADR-0001, ADR-0002, ADR-0003 | `TE-0001-.NET10.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-002 | فریم‌ورک رابط کاربری وب | Blazor Server | ADR-0004 | `TE-0002-Blazor.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-003 | کتابخانه مولفه‌های رابط کاربری | MudBlazor | ADR-0005 | `TE-0003-MudBlazor.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-004 | دسترسی به داده با ORM | Entity Framework Core 10 | ADR-0006 | `TE-0004-EntityFrameworkCore.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-005 | اعتبارسنجی روان (Fluent) | FluentValidation | ADR-0007 | `TE-0005-FluentValidation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-006 | نگاشت اشیاء DTO | Mapster | ADR-0008 | `TE-0006-Mapster.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-007 | لاگ‌گیری ساختاریافته | Serilog | ADR-0009 | `TE-0007-Serilog.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-008 | مشاهده‌پذیری و ردگیری | OpenTelemetry | ADR-0010 | `TE-0008-OpenTelemetry.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-009 | دیسپچر و موتور CQRS | MediatR | ADR-0011 | `TE-0009-Use-MediatR.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-010 | کلاینت‌های قابل نصب (دسکتاپ و موبایل) | .NET MAUI | ADR-0013 | `TE-0010-Desktop-Mobile-Framework-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-011 | پایگاه داده فضای کاری آفلاین | SQLite & LiteDB | ADR-0014 | `TE-0011-Embedded-Workspace-Database-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-012 | پیام‌رسانی سازمانی | MassTransit & RabbitMQ | ADR-0016 | `TE-0012-Enterprise-Messaging-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-013 | موتور یکپارچه‌سازی هوش مصنوعی | Semantic Kernel | ADR-0017 | `TE-0013-Artificial-Intelligence-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-014 | پردازش پس‌زمینه | Quartz.NET / Channels | ADR-0032 | `TE-0014-Background-Processing-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-015 | معماری کش‌گذاری | حافظه هیبریدی و کش توزیع‌شده | ADR-0031 | `TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-016 | معماری جستجوی سازمانی | SQL Server FTS + OpenSearch (GAP-027 را ببینید) | ADR-0021 | `TE-0016-Enterprise-Search-Architecture-Evaluation.md` | متوسط (Medium) | جایگزین‌شده ⚠️ (TE-0016 توسط TE-0027 جایگزین شد؛ GAP-027 را ببینید) |
| GAP-017 | خط لوله مشاهده‌پذیری سازمانی | Prometheus, Grafana, OpenTelemetry, Serilog | ADR-0033 | `TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-018 | مدیریت تنظیمات و اسرار | متغیرهای محیطی و HashiCorp Vault | ADR-0034 | `TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-019 | استراتژی زمان‌بندی وظایف | موتور Quartz.NET | ADR-0032 | `TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-020 | امنیت و هویت | ASP.NET Core Identity & OpenIddict | ADR-0030 | `TE-0020-Authentication-and-Identity-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-021 | تولید کلاینت API | OpenAPI, Scalar & NSwag | ADR-0035 | `TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-022 | خط لوله اعتبارسنجی | رفتار اعتبارسنجی MediatR | ADR-0007, ADR-0036 | `TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-023 | نگاشت با عملکرد بالا | پروجکشن‌های کامپایلری Mapster | ADR-0008 | `TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-024 | ماندگاری پرس‌وجوهای خواندنی سنگین | Dapper و رپلیکاهای خواندنی | ADR-0019 | `TE-0024-Data-Access-Architecture-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-025 | مهاجرت‌های پایگاه داده | EF Core Migrations & Respawn | ADR-0037 | `TE-0025-Database-Migration-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-026 | ذخیره‌سازی شیء و فایل | S3 / MinIO | ADR-0020 | `TE-0026-File-Storage-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-027 | یکپارچه‌سازی موتور جستجو | SQL Server FTS (پیش‌فرض) + OpenSearch (افزایش مقیاس) | ADR-0021 | `TE-0027-Search-Engine-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-028 | جستجوی برداری و RAG | موتور برداری Qdrant | ADR-0022 | `TE-0028-Vector-Database-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-029 | مسیریاب ارائه‌دهندگان هوش مصنوعی | موتور چندارائه‌دهنده‌ای (Ollama/OpenAI) | ADR-0023 | `TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-030 | اتوماسیون آزمون‌های سازمانی | xUnit, Moq, Testcontainers | ADR-0024 | `TE-0030-Testing-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-031 | بسته‌بندی و استقرار | Docker & GitHub Actions (کوبرنتیز: حل‌نشده/تعیین‌تکلیف‌نشده) | ADR-0025 | `TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-032 | مقاوم‌سازی امنیتی سازمانی | TLS, Secret Vault, RBAC | ADR-0026 | `TE-0032-Security-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-033 | آزمون کارایی و بار | K6 & NBomber | ADR-0027 | `TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` | متوسط (Medium) | تصویب‌شده ✅ |
| GAP-034 | فریم‌ورک هیبریدی رابط کاربری کلاینت | ~~Avalonia UI~~ جایگزین‌شده — GAP-010 (.NET MAUI) را ببینید | ADR-0028 | `TE-0034-Client-UI-Technology-Evaluation.md` | بالا (High) | جایگزین‌شده ⚠️ |
| GAP-035 | موتور گزارش‌گیری سازمانی | QuestPDF (کنار گذاشتن FastReport و RDLC) | ADR-0029 | `TE-0035-Reporting-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |
| GAP-036 | یکپارچه‌سازی خارجی و کانکتورها | فریم‌ورک کانکتور مبتنی بر MassTransit (+ Azure Logic Apps اختیاری) | ADR-0018 | `TE-0036-External-Integration-and-Connector-Technology-Evaluation.md` | بالا (High) | تصویب‌شده ✅ |

---

# خلاصه تفصیلی ارزیابی شکاف‌ها (Detailed Gap Evaluation Summaries)

### GAP-010 — فریم‌ورک توزیع‌شده دسکتاپ و موبایل (Distributed Desktop & Mobile Framework)
- **قابلیت**: برنامه کاربردی فضای کاری قابل نصب چندپلتفرمی (دسکتاپ ویندوز/مک‌او‌اس، موبایل اندروید/آی‌او‌اس)
- **وضعیت**: تصویب‌شده ✅
- **تصمیم معماری**: ADR-0013 — معماری برنامه کلاینت (Client Application Architecture)
- **ارزیابی فناوری**: `TE-0010-Desktop-Mobile-Framework-Evaluation.md`
- **فناوری انتخاب‌شده**: .NET MAUI & Blazor Hybrid

### GAP-011 — پایگاه داده محلی توکار (Embedded Local Database)
- **قابلیت**: ماندگاری فضای کاری آفلاین و همگام‌سازی محلی بسته‌ها
- **وضعیت**: تصویب‌شده ✅
- **تصمیم معماری**: ADR-0014 — پایگاه داده فضای کاری توکار (Embedded Workspace Database)
- **ارزیابی فناوری**: `TE-0011-Embedded-Workspace-Database-Evaluation.md`
- **فناوری انتخاب‌شده**: SQLite (رابطه‌ای ساختاریافته) و LiteDB (مخزن اسناد)

### GAP-012 — پیام‌رسانی سازمانی (Enterprise Messaging)
- **قابلیت**: گذرگاه رویداد ناهمگام میان‌ماژولی و توزیع پیام‌ها
- **وضعیت**: تصویب‌شده ✅
- **تصمیم معماری**: ADR-0016 — معماری پیام‌رسانی سازمانی (Enterprise Messaging Architecture)
- **ارزیابی فناوری**: `TE-0012-Enterprise-Messaging-Technology-Evaluation.md`
- **فناوری انتخاب‌شده**: MassTransit بر بستر RabbitMQ

### GAP-013 — یکپارچه‌سازی هوش مصنوعی (Artificial Intelligence Integration)
- **قابلیت**: دستیار هوش مصنوعی، کرنل عیب‌یابی و بازیابی دانش
- **وضعیت**: تصویب‌شده ✅
- **تصمیم معماری**: ADR-0017 — یکپارچه‌سازی هوش مصنوعی (Artificial Intelligence Integration)
- **ارزیابی فناوری**: `TE-0013-Artificial-Intelligence-Technology-Evaluation.md`
- **فناوری انتخاب‌شده**: موتور Semantic Kernel و مسیریاب مدل چندارائه‌دهنده‌ای

---

# نقشه راه پیاده‌سازی مبتنی بر معماری-نخست (Architecture First Implementation Roadmap)

پروژه از متدولوژی دقیق معماری-نخست (Architecture First) پیروی می‌کند.

```text
Business Requirement -> Capability Model -> Technology Gap Analysis -> ADR -> TE -> Implementation
(نیازمندی کسب‌وکار -> مدل قابلیت‌ها -> تحلیل شکاف فناوری -> سند ADR -> سند TE -> پیاده‌سازی)
```

کلیه ۳۵ شکاف فناوری شناسایی‌شده در سرتاسر پلتفرم، ارزیابی‌های فنی متناظر خود (`TE-0001` تا `TE-0035`) و اسناد ثبت تصمیمات معماری (`ADR-0001` تا `ADR-0029`) را تکمیل نموده‌اند.

---

# خلاصه تصمیمات (Decision Summary)

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی ابری (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

---

# اسناد مرتبط (Related Documents)

- `01-Architecture.md`
- `02-CapabilityModel.md`
- `00-TechnologyEvaluationTemplate.md`
- `../06-decisions/000-ADR-INDEX.md`
- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`
- `TE-0001-.NET10.md` تا `TE-0035-Reporting-Technology-Evaluation.md`

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0 | 2026-07-26 | معمار راهکار | تحلیل اولیه شکاف‌های فناوری |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0؛ گسترش ماتریس شکاف‌ها برای پوشش تمامی ۳۵ سند TE و ۲۹ سند ADR |
| 4.1.0 | 2026-08-02 | معمار راهکار | اصلاح GAP-020: جایگزینی ارجاع نادرست "Keycloak / OpenID Connect" / ADR-0026 با توصیه واقعی TE-0020 (ASP.NET Core Identity & OpenIddict) و ADR-0030 تصویب‌کننده آن |
| 4.2.0 | 2026-08-08 | معمار راهکار | نکته: مدخل 4.1.0 بالا اصلاح GAP-020 را شرح داده بود که در عمل بر سطر جدول اعمال نشده بود؛ در این بازنگری به طور واقعی اصلاح شد. همچنین اصلاح ۹ ارجاع نادرست دیگر به ADR (موارد GAP-014، GAP-017، GAP-018، GAP-019، GAP-021، GAP-022، GAP-025 به شماره ADR نادرست اشاره داشتند)، اصلاح محتوای فناوری GAP-016/GAP-027/GAP-035 (مورد Meilisearch/Elasticsearch هرگز تصویب نشد؛ تصمیم واقعی SQL Server FTS + OpenSearch برای افزایش مقیاس است؛ FastReport صراحتاً از گزارش‌گیری مستثنی شد)، علامت‌گذاری GAP-016 و GAP-034 به عنوان جایگزین‌شده (Superseded) (هر دو با تصمیمات قبلاً تصویب‌شده در تعارض بودند)، و افزودن GAP-036 برای یکپارچه‌سازی خارجی (ADR-0018/TE-0036) که کلاً فاقد سطر بود |
