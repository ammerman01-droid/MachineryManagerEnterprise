| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | ADR-INDEX |
| **عنوان** | فهرست اصلی اسناد تصمیم‌گیری معماری |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# فهرست اصلی اسناد تصمیم‌گیری معماری (ADR)

این سند به عنوان فهرست رسمی و مرجع تمامی اسناد تصمیم‌گیری معماری (ADR) حاکم بر پلتفرم MachineryManagerEnterprise عمل می‌کند.

---

# هدف

هدف از این فهرست اصلی، ارائه یک کاتالوگ متمرکز، یکنواخت و معتبر از تمامی تصمیمات معماری سازمانی است. این سند قابلیت ردیابی کامل بین نیازمندی‌های کسب‌وکار، مدل‌های قابلیت، ارزیابی‌های فناوری (TE) و قوانین پیاده‌سازی معماری را تضمین می‌کند.

---

# محدوده ارزیابی

محدوده ارزیابی این فهرست، تمامی تصمیمات معماری را در حوزه‌های زیر در بر می‌گیرد:

- زمان اجرای اصلی و پلتفرم سازمانی (.NET 10، Clean Architecture، Modular Monolith)
- لایه ارائه و رابط کاربری (Blazor Server، MudBlazor، .NET MAUI)
- لایه ماندگاری و دسترسی به داده (Entity Framework Core، Dapper، SQLite، LiteDB، PostgreSQL)
- خط لوله اعتبارسنجی و منطق کسب‌وکار (FluentValidation، MediatR، Mapster)
- دورسنجی و قابلیت مشاهده‌پذیری (Serilog، OpenTelemetry، Prometheus، Jaeger)
- زیرساخت پیام‌رسانی و یکپارچه‌سازی (RabbitMQ، MassTransit، SignalR)
- هوش مصنوعی و موتور دانش (Semantic Kernel، Ollama، Vector Search)
- همگام‌سازی فضای کاری توزیع‌شده و ماندگاری آفلاین-محور
- امنیت، احراز هویت و مدیریت اسرار (OpenID Connect، Keycloak، HashiCorp Vault)
- ساخت، پکیج‌بندی، استقرار و آزمون‌های کارایی (Docker، Kubernetes، K6)

---

# روابط اسناد

این فهرست دارای روابط دوطرفه با اسناد زیر است:

- **چشم‌انداز و نقشه راه**: `../01-vision/00-Vision.md`, `../01-vision/01-DocumentationRoadmap.md`
- **معماری سیستم**: `../02-architecture/01-Architecture.md`
- **مدل قابلیت‌ها**: `../02-architecture/02-CapabilityModel.md`
- **تحلیل شکاف فناوری**: `../02-architecture/03-TechnologyGapAnalysis.md`
- **ارزیابی‌های فنی**: از `../02-architecture/TE-0001-.NET10.md` تا `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md`

---

# مراجع معماری

- **استاندارد معماری پاک نسخه 4.0.0**
- **راهنمای سازمانی طراحی دامنه‌محور (DDD)**
- **مشخصات معماری فضای کاری توزیع‌شده**
- **استاندارد مستندسازی نسخه 4.0.0**

---

# محدوده

محدوده این سند شامل تمامی ماژول‌ها، بلوک‌های سازنده، کتابخانه‌های مشترک، کلاینت‌های دسکتاپ/موبایل و توپولوژی‌های میزبانی ابری در MachineryManagerEnterprise می‌شود.

---

# نیازمندی‌های کاربردی

چارچوب حاکمیت ADR باید موارد زیر را پشتیبانی کند:

- عدم وجود ابهام در انتخاب فناوری و اجرای الگوهای معماری؛
- پیگیری شفاف وضعیت تصمیمات (Approved, Proposed, Deprecated, Superseded)؛
- نگاشت کامل بین ADRها و اسناد ارزیابی فنی (TE)؛
- حفظ مرزهای معماری پاک (Clean Architecture) در تمامی ماژول‌ها.

---

# نیازمندی‌های غیرکاربردی

- **قابلیت ردیابی (Traceability)**: ۱۰۰٪ تصمیمات معماری باید به یک سند TE یا قانون معماری زیربنایی مرتبط نگاشت شوند.
- **قابلیت نگهداری (Maintainability)**: نسخه‌گذاری و تاریخچه تجدیدنظر شفاف در تمامی ورودی‌های ADR.
- **انطباق (Compliance)**: رعایت کامل استاندارد مستندسازی نسخه 4.0.0.

---

# فناوری‌های کاندید

تمامی فناوری‌های کاندید ارزیابی‌شده جهت پشتیبانی از این ADRها به تفصیل در اسناد ارزیابی فنی (TE) مربوطه (`TE-0001` تا `TE-0035`) تشریح شده‌اند.

---

# معیارهای ارزیابی

اعتبار و تصویب ADRها بر اساس موارد زیر ارزیابی می‌شود:

- انطباق با معماری پاک (Clean Architecture)
- همراستایی با اکوسیستم .NET 10
- خط‌مشی اولویت با متن‌باز (ADR-0002)
- خنثی بودن نسبت به ابر و عدم وابستگی به تامین‌کننده خاص (Zero vendor lock-in)
- سادگی عملیاتی و آزمون‌پذیری

---

# اصل معماری

تمامی تصمیمات معماری باید جداسازی صریح دغدغه‌ها را اعمال کرده و مدل‌های دامنه (Domain models) را پاک و عاری از وابستگی‌های زیرساختی خارجی نگه دارند.

---

# مقایسه کلی فناوری‌ها

| شناسه ADR | حوزه تصمیم‌گیری | استاندارد تصویب‌شده | جایگزین ارزیابی‌شده |
|--------|-----------------|-------------------|-----------------------|
| ADR-0001 | معماری | Clean Architecture & Modular Monolith | Microservices, Layered Monolith |
| ADR-0002 | حاکمیت | Open Source First Policy | Commercial Proprietary |
| ADR-0003 | الگو | CQRS & MediatR Pipeline | Monolithic Services |
| ADR-0004 | فریم‌ورک رابط کاربری وب | Blazor Server (.NET 10) | React, Angular, Vue |
| ADR-0005 | معماری API | RESTful API & OpenAPI Client Generation | GraphQL, gRPC |
| ADR-0006 | استراتژی دسترسی به داده | Entity Framework Core 10 | NHibernate, Raw ADO.NET |
| ADR-0007 | معماری اعتبارسنجی | FluentValidation | Data Annotations |
| ADR-0008 | کتابخانه کامپوننت UI | MudBlazor | Radzen, Syncfusion |
| ADR-0009 | موتور خط لوله CQRS | MediatR | Custom Mediator |
| ADR-0010 | نگاشت اشیاء | Mapster | AutoMapper |
| ADR-0011 | موتور مشاهده‌پذیری | Serilog & OpenTelemetry | NLog, Log4Net |
| ADR-0012 | معماری همگام‌سازی | Distributed Workspace Offline Sync | Direct DB Sync |
| ADR-0013 | فریم‌ورک کلاینت | .NET MAUI (Desktop & Mobile) | Electron, Flutter |
| ADR-0014 | استراتژی دیتابیس درون‌برنامه‌ای | SQLite & LiteDB | RavenDB Embedded |
| ADR-0015 | معماری استقرار | Docker & Kubernetes | Bare Metal |
| ADR-0016 | معماری پیام‌رسانی | MassTransit & RabbitMQ | Apache Kafka |
| ADR-0017 | معماری هوش مصنوعی | Semantic Kernel | Direct API Integration |
| ADR-0018 | یکپارچه‌سازی خارجی | Modular Connector Engine | Custom Scripts |
| ADR-0019 | ماندگاری خواندن | Hybrid Persistence Strategy | Single DB Engine |
| ADR-0020 | استراتژی ذخیره‌سازی فایل | S3 Compatible Object Store (MinIO) | Local File System |
| ADR-0021 | استراتژی جستجو | Meilisearch / Elasticsearch Engine | SQL LIKE Queries |
| ADR-0022 | معماری دیتابیس برداری | Qdrant Vector Engine | In-Memory Search |
| ADR-0023 | استراتژی ارائه‌دهنده هوش مصنوعی | Multi-Provider Router Engine | Single LLM Vendor |
| ADR-0024 | آزمون‌سازی سازمانی | xUnit, Moq, Testcontainers | Manual Testing |
| ADR-0025 | خط لوله ساخت | GitHub Actions & Nuke Build | Jenkins |
| ADR-0026 | معماری امنیت | OpenID Connect & Keycloak | Custom Auth |
| ADR-0027 | آزمون کارایی | K6 & NBomber Engine | JMeter |
| ADR-0028 | معماری UI کلاینت | Blazor Hybrid & MAUI Controls | Web View Wrapper |
| ADR-0029 | معماری گزارش‌گیری | QuestPDF & FastReport OpenSource | SSRS |

---

# توصیه نهایی

تمامی ADRهای تصویب‌شده باید در میان تمامی تیم‌های توسعه و خطوط لوله اعتبارسنجی خودکار CI/CD حفظ و اعمال شوند.

---

# تصمیم نهایی

تمامی ۲۹ سند تصمیم‌گیری معماری فهرست‌شده در کاتالوگ اصلی زیر رسماً **تصویب‌شده (Approved)** و فعال هستند.

---

# خلاصه تصمیمات

- ✔ ۲۹/۲۹ سند ADR تصویب شده است
- ✔ ۳۵/۳۵ سند TE تکمیل و لینک شده است
- ✔ معماری پاک اعمال شده است
- ✔ انطباق کامل با استاندارد مستندسازی نسخه 4.0.0

---

# کاتالوگ اصلی اسناد ADR

| شناسه ADR | عنوان تصمیم | وضعیت | فناوری اصلی | ارزیابی فنی مرتبط |
|--------|----------------|--------|--------------------|------------------------------|
| ADR-0001 | تصویب معماری پاک و مونوپلیت ماژولار | تصویب‌شده | .NET 10 | `../02-architecture/TE-0001-.NET10.md` |
| ADR-0002 | خط‌مشی اولویت با متن‌باز | تصویب‌شده | Permissive OSS | `../02-architecture/TE-0001-.NET10.md` |
| ADR-0003 | معماری CQRS و رویدادمحور | تصویب‌شده | MediatR | `../02-architecture/TE-0009-Use-MediatR.md` |
| ADR-0004 | استاندارد فریم‌ورک رابط کاربری وب | تصویب‌شده | Blazor Server | `../02-architecture/TE-0002-Blazor.md` |
| ADR-0005 | معماری API و تولید کلاینت | تصویب‌شده | OpenAPI / NSwag | `../02-architecture/TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` |
| ADR-0006 | استراتژی دسترسی به داده | تصویب‌شده | Entity Framework Core 10 | `../02-architecture/TE-0004-EntityFrameworkCore.md`, `../02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md` |
| ADR-0007 | استراتژی خط لوله اعتبارسنجی | تصویب‌شده | FluentValidation | `../02-architecture/TE-0005-FluentValidation.md`, `../02-architecture/TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` |
| ADR-0008 | استراتژی کتابخانه کامپوننت UI | تصویب‌شده | MudBlazor | `../02-architecture/TE-0003-MudBlazor.md`, `../02-architecture/TE-0034-Client-UI-Technology-Evaluation.md` |
| ADR-0009 | هدایت‌گر CQRS و معماری خط لوله | تصویب‌شده | MediatR | `../02-architecture/TE-0009-Use-MediatR.md` |
| ADR-0010 | استراتژی نگاشت اشیاء | تصویب‌شده | Mapster | `../02-architecture/TE-0006-Mapster.md`, `../02-architecture/TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` |
| ADR-0011 | معماری مشاهده‌پذیری و دورسنجی | تصویب‌شده | Serilog & OpenTelemetry | `../02-architecture/TE-0007-Serilog.md`, `../02-architecture/TE-0008-OpenTelemetry.md`, `../02-architecture/TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` |
| ADR-0012 | معماری فضای کاری توزیع‌شده | تصویب‌شده | Sync Package Engine | `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md`, `../02-architecture/TE-0026-File-Storage-Technology-Evaluation.md` |
| ADR-0013 | معماری اپلیکیشن کلاینت | تصویب‌شده | .NET MAUI | `../02-architecture/TE-0010-Desktop-Mobile-Framework-Evaluation.md` |
| ADR-0014 | دیتابیس درون‌برنامه‌ای فضای کاری | تصویب‌شده | SQLite / LiteDB | `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md`, `../02-architecture/TE-0025-Database-Migration-Technology-Evaluation.md` |
| ADR-0015 | معماری استقرار | تصویب‌شده | Docker / Kubernetes | `../02-architecture/TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` |
| ADR-0016 | معماری پیام‌رسانی سازمانی | تصویب‌شده | MassTransit & RabbitMQ | `../02-architecture/TE-0012-Enterprise-Messaging-Technology-Evaluation.md` |
| ADR-0017 | معماری هوش مصنوعی | تصویب‌شده | Semantic Kernel Engine | `../02-architecture/TE-0013-Artificial-Intelligence-Technology-Evaluation.md` |
| ADR-0018 | معماری یکپارچه‌سازی خارجی | تصویب‌شده | Connector Engine | `../02-architecture/TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` |
| ADR-0019 | استراتژی ماندگاری ترکیبی | تصویب‌شده | Dapper / Read Replicas | `../02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md` |
| ADR-0020 | استراتژی ذخیره‌سازی فایل | تصویب‌شده | S3 / MinIO | `../02-architecture/TE-0026-File-Storage-Technology-Evaluation.md` |
| ADR-0021 | استراتژی موتور جستجو | تصویب‌شده | Meilisearch / Elasticsearch | `../02-architecture/TE-0027-Search-Engine-Technology-Evaluation.md` |
| ADR-0022 | معماری دیتابیس برداری | تصویب‌شده | Qdrant Vector Engine | `../02-architecture/TE-0028-Vector-Database-Technology-Evaluation.md` |
| ADR-0023 | استراتژی ارائه‌دهنده هوش مصنوعی | تصویب‌شده | Multi-Provider Model Router | `../02-architecture/TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` |
| ADR-0024 | استراتژی آزمون‌سازی سازمانی | تصویب‌شده | xUnit & Testcontainers | `../02-architecture/TE-0030-Testing-Technology-Evaluation.md` |
| ADR-0025 | معماری ساخت و استقرار | تصویب‌شده | GitHub Actions / Nuke | `../02-architecture/TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` |
| ADR-0026 | استراتژی امنیت سازمانی | تصویب‌شده | OpenID Connect & Keycloak | `../02-architecture/TE-0020-Authentication-and-Identity-Technology-Evaluation.md`, `../02-architecture/TE-0032-Security-Technology-Evaluation.md` |
| ADR-0027 | استراتژی آزمون کارایی سازمانی | تصویب‌شده | K6 & NBomber | `../02-architecture/TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` |
| ADR-0028 | معماری UI کلاینت | تصویب‌شده | Blazor Hybrid & MAUI | `../02-architecture/TE-0034-Client-UI-Technology-Evaluation.md` |
| ADR-0029 | معماری گزارش‌گیری سازمانی | تصویب‌شده | QuestPDF & FastReport | `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md` |

---

# اسناد ADR مرتبط

- ADR-0001 — تصویب معماری پاک و مونوپلیت ماژولار
- ADR-0002 — خط‌مشی اولویت با متن‌باز
- ADR-0012 — معماری فضای کاری توزیع‌شده

---

# اسناد مرتبط

- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`
- `../02-architecture/00-TechnologyEvaluationTemplate.md`
- `../02-architecture/01-Architecture.md`
- `../02-architecture/02-CapabilityModel.md`
- `../02-architecture/03-TechnologyGapAnalysis.md`
- `../02-architecture/TE-0001-.NET10.md`
- `../02-architecture/TE-0002-Blazor.md`
- `../02-architecture/TE-0003-MudBlazor.md`
- `../02-architecture/TE-0004-EntityFrameworkCore.md`
- `../02-architecture/TE-0005-FluentValidation.md`
- `../02-architecture/TE-0006-Mapster.md`
- `../02-architecture/TE-0007-Serilog.md`
- `../02-architecture/TE-0008-OpenTelemetry.md`
- `../02-architecture/TE-0009-Use-MediatR.md`
- `../02-architecture/TE-0010-Desktop-Mobile-Framework-Evaluation.md`
- `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md`
- `../02-architecture/TE-0012-Enterprise-Messaging-Technology-Evaluation.md`
- `../02-architecture/TE-0013-Artificial-Intelligence-Technology-Evaluation.md`
- `../02-architecture/TE-0014-Background-Processing-Technology-Evaluation.md`
- `../02-architecture/TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md`
- `../02-architecture/TE-0016-Enterprise-Search-Architecture-Evaluation.md`
- `../02-architecture/TE-0017-Observability-and-Telemetry-Technology-Evaluation.md`
- `../02-architecture/TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md`
- `../02-architecture/TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md`
- `../02-architecture/TE-0020-Authentication-and-Identity-Technology-Evaluation.md`
- `../02-architecture/TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md`
- `../02-architecture/TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md`
- `../02-architecture/TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md`
- `../02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md`
- `../02-architecture/TE-0025-Database-Migration-Technology-Evaluation.md`
- `../02-architecture/TE-0026-File-Storage-Technology-Evaluation.md`
- `../02-architecture/TE-0027-Search-Engine-Technology-Evaluation.md`
- `../02-architecture/TE-0028-Vector-Database-Technology-Evaluation.md`
- `../02-architecture/TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md`
- `../02-architecture/TE-0030-Testing-Technology-Evaluation.md`
- `../02-architecture/TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md`
- `../02-architecture/TE-0032-Security-Technology-Evaluation.md`
- `../02-architecture/TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md`
- `../02-architecture/TE-0034-Client-UI-Technology-Evaluation.md`
- `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md`

---

# مراجع

- اصول طراحی معماری پاک (روبرت سی. مارتین)
- الگوهای یکپارچه‌سازی سازمانی (گرگور هوپ)
- استانداردهای معماری .NET 10

---

# تاریخچه تغییرات

| نسخه | تاریخ | نویسنده / نقش | شرح |
|---------|------------|--------------------|--------------------------------------------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | فهرست اولیه ADR |
| 3.0.0 | 2026-07-18 | معمار راهکار | فهرست اولیه تصمیمات معماری |
| 3.1.0 | 2026-07-27 | معمار راهکار | افزودن ADR-0019 (استراتژی ماندگاری ترکیبی برای پرس‌وجوهای سنگین خواندن) |
| 3.2.0 | 2026-07-27 | معمار راهکار | افزودن ADR-0020 (استراتژی ذخیره‌سازی فایل) |
| 3.3.0 | 2026-07-27 | معمار راهکار | افزودن ADR-0021 (استراتژی جستجو) |
| 3.4.0 | 2026-07-27 | معمار راهکار | افزودن ADR-0013 تا ADR-0018 به فهرست |
| 3.5.0 | 2026-07-28 | معمار راهکار | افزودن ADR-0022 تا ADR-0025 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه 4.0.0؛ تجمیع تمامی ۲۹ ADR و ۳۵ TE |
