| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | ADR-INDEX |
| **عنوان** | فهرست سوابق تصمیمات معماری (Architecture Decision Record Index) |
| **نسخه** | 4.4.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# فهرست جامع سوابق تصمیمات معماری (ADR Master Index)

این سند به عنوان فهرست رسمی و مرجع اصلی برای کلیه سوابق تصمیمات معماری (ADR) حاکم بر پلتفرم MachineryManagerEnterprise عمل می‌نماید.

---

# هدف (Purpose)

هدف از فهرست جامع ADR، فراهم آوردن یک کاتالوگ واحد، متمرکز و معتبر از کلیه تصمیمات معماری سازمانی است. این فهرست تضمین‌کننده قابلیت ردگیری و انطباق کامل میان نیازمندی‌های کسب‌وکار، مدل‌های قابلیت (Capability Models)، ارزیابی‌های فنی (TE) و قواعد پیاده‌سازی معماری می‌باشد.

---

# محدوده ارزیابی (Evaluation Scope)

محدوده ارزیابی این فهرست، کلیه تصمیمات معماری را در حوزه‌های زیر در بر می‌گیرد:

- بستر زمان اجرا و پلتفرم سازمانی (.NET 10، معماری پاک، Modular Monolith)
- رابط کاربری و لایه نمایش (Blazor Server، MudBlazor، .NET MAUI)
- لایه ماندگاری و دسترسی به داده (Entity Framework Core، Dapper، SQLite، LiteDB، PostgreSQL)
- پایپ‌لاین اعتبارسنجی و منطق تجاری (FluentValidation، MediatR، Mapster)
- تله‌متری و مشاهده‌پذیری (Serilog، OpenTelemetry، Prometheus، Jaeger)
- زیرساخت پیام‌رسانی و یکپارچه‌سازی (RabbitMQ، MassTransit، SignalR)
- هوش مصنوعی و موتور دانش (Semantic Kernel، Ollama، جستجوی برداری)
- همگام‌سازی فضای کاری توزیع‌شده و ماندگاری با اولویت آفلاین (Offline-First)
- امنیت، احراز هویت و مدیریت اسرار (OpenID Connect، Keycloak، HashiCorp Vault)
- ساخت، بسته‌بندی، استقرار و آزمون کارایی (Docker، Kubernetes، K6)

---

# روابط و وابستگی‌ها (Relationship)

این فهرست روابط دوطرفه با اسناد زیر را حفظ می‌کند:

- **چشم‌انداز و نقشه راه**: `../01-vision/00-Vision.md`، `../01-vision/01-DocumentationRoadmap.md`
- **معماری سیستم**: `../02-architecture/01-Architecture.md`
- **مدل قابلیت**: `../02-architecture/02-CapabilityModel.md`
- **تحلیل شکاف فناوری**: `../02-architecture/03-TechnologyGapAnalysis.md`
- **ارزیابی‌های فنی**: `../02-architecture/TE-0001-.NET10.md` تا `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md`

---

# مراجع معماری (Architectural References)

- **استاندارد معماری پاک نسخه 4.0.0 (Clean Architecture Standard v4.0.0)**
- **راهنمای سازمانی طراحی دامنه-محور (DDD Enterprise Guide)**
- **مشخصات معماری فضای کاری توزیع‌شده (Distributed Workspace Architecture Specification)**
- **استاندارد مستندسازی نسخه 4.0.0 (Documentation Standard v4.0.0)**

---

# محدوده شمول (Scope)

این محدوده بر تمامی ماژول‌ها، بلوک‌های سازنده، کتابخانه‌های مشترک، کلاینت‌های دسکتاپ/موبایل و توپولوژی‌های میزبانی ابری در سامانه MachineryManagerEnterprise اعمال می‌گردد.

---

# نیازمندی‌های کارکردی (Functional Requirements)

چارچوب راهبری ADR باید موارد زیر را پشتیبانی نماید:

- عدم وجود هرگونه ابهام در انتخاب فناوری‌ها و اعمال الگوهای معماری؛
- ردگیری شفاف وضعیت تصمیمات (Approved، Proposed، Deprecated، Superseded)؛
- نگاشت کامل میان اسناد ADR و اسناد ارزیابی فنی (TE)؛
- حفظ مرزهای معماری پاک (Clean Architecture) در سراسر تمامی ماژول‌ها.

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

- **قابلیت ردگیری (Traceability)**: ۱۰۰٪ تصمیمات معماری باید به یک سند TE متناظر یا قاعده بنیادین معماری نگاشت شوند.
- **قابلیت نگهداری (Maintainability)**: نسخه‌بندی شفاف و تاریخچه بازنگری مشخص در تمام مدخل‌های ADR.
- **انطباق (Compliance)**: پیروی دقیق و سخت‌گیرانه از استاندارد مستندسازی نسخه 4.0.0.

---

# فناوری‌های نامزد (Candidate Technologies)

کلیه فناوری‌های نامزد که در پشتیبانی از این ADRها ارزیابی شده‌اند، به طور کامل در اسناد ارزیابی فنی (TE) متناظر (`TE-0001` تا `TE-0035`) با جزئیات تشریح شده‌اند.

---

# معیارهای ارزیابی (Evaluation Criteria)

اعتبار و تصویب ADR بر اساس معیارهای زیر ارزیابی می‌گردد:

- انطباق با Clean Architecture
- همراستایی با اکوسیستم .NET 10
- خط‌مشی اولویت متن‌باز (ADR-0002)
- بی‌طرفی نسبت به ابر و عدم وابستگی انحصاری به فروشنده (Zero Vendor Lock-in)
- سادگی عملیاتی و قابلیت آزمون‌پذیری

---

# اصل معماری (Architecture Principle)

تمامی تصمیمات معماری باید تفکیک دقیق وظایف (Separation of Concerns) را اعمال نموده و مدل‌های لایه دامنه (Domain) را پاک و عاری از هرگونه وابستگی به زیرساخت‌های خارجی نگه دارند.

---

# مقایسه کلی فناوری‌ها (Overall Technology Comparison)

| شناسه ADR | دامنه تصمیم‌گیری | استاندارد تصویب‌شده | جایگزین ارزیابی‌شده |
|-----------|------------------|---------------------|----------------------|
| ADR-0001 | معماری | Clean Architecture & Modular Monolith | Microservices, Layered Monolith |
| ADR-0002 | راهبری | خط‌مشی اولویت متن‌باز (Open Source First Policy) | Commercial Proprietary |
| ADR-0003 | بستر هسته زمان اجرا | استاندارد پلتفرم .NET 10 | .NET 8 / 9 |
| ADR-0004 | فریم‌ورک وب UI | Blazor Server (.NET 10) | React, Angular, Vue |
| ADR-0005 | کتابخانه کامپوننت UI | MudBlazor | Radzen, Syncfusion |
| ADR-0006 | استراتژی دسترسی به داده | Entity Framework Core 10 | NHibernate, Raw ADO.NET |
| ADR-0007 | معماری اعتبارسنجی | FluentValidation | Data Annotations |
| ADR-0008 | نگاشت اشیاء | Mapster | AutoMapper |
| ADR-0009 | لاگ‌گیری ساختاریافته | موتور Serilog | NLog, Log4Net |
| ADR-0010 | موتور مشاهده‌پذیری | ابزار دقیق OpenTelemetry Instrumentation | تله‌متری سفارشی |
| ADR-0011 | موتور پایپ‌لاین CQRS | پایپ‌لاین MediatR | واسط (Mediator) سفارشی |
| ADR-0012 | معماری همگام‌سازی | معماری فضای کاری توزیع‌شده | همگام‌سازی مستقیم دیتابیس |
| ADR-0013 | فریم‌ورک کلاینت | .NET MAUI (دسکتاپ و موبایل) | Electron, Flutter |
| ADR-0014 | استراتژی پایگاه داده تعبیه‌شده | SQLite & LiteDB | RavenDB Embedded |
| ADR-0015 | همگام‌سازی فضای کاری | موتور همگام‌سازی / پروتکل تفاضلی | Bare Metal |
| ADR-0016 | معماری پیام‌رسانی | MassTransit & RabbitMQ | Apache Kafka |
| ADR-0017 | معماری هوش مصنوعی | موتور Semantic Kernel | ادغام مستقیم API |
| ADR-0018 | یکپارچه‌سازی خارجی | چارچوب رابط‌های مبتنی بر MassTransit (+ انتخاب اختیاری Azure Logic Apps) | آداپتورهای NServiceBus |
| ADR-0019 | ماندگاری داده‌های خواندنی | استراتژی ماندگاری ترکیبی (Hybrid Persistence) | موتور پایگاه داده منفرد |
| ADR-0020 | استراتژی ذخیره‌سازی فایل | مخزن شیء سازگار با S3 (MinIO) | سیستم فایل محلی |
| ADR-0021 | استراتژی جستجو | SQL Server FTS (پیش‌فرض) + OpenSearch (در شرایط ارتقای مقیاس) | PostgreSQL FTS (در TE-0016، منسوخ‌شده) |
| ADR-0022 | بازیابی دانش هوش مصنوعی | موتور برداری Qdrant | جستجوی درون‌حافظه‌ای |
| ADR-0023 | استراتژی ارائه‌دهنده هوش مصنوعی | موتور مسیریاب چند ارائه‌دهنده‌ای | فروشنده منفرد LLM |
| ADR-0024 | آزمون سازمانی | xUnit، Moq، Testcontainers | آزمون دستی |
| ADR-0025 | پایپ‌لاین ساخت | Docker، GitHub Actions، .NET Aspire | Jenkins |
| ADR-0026 | حفاظت از داده‌ها و رمزنگاری | ASP.NET Core Data Protection، AES-256، X.509 | رمزنگاری سفارشی |
| ADR-0027 | آزمون کارایی و بار | موتور K6 & NBomber | JMeter |
| ADR-0028 | معماری UI کلاینت | منسوخ‌شده — Avalonia UI (رجوع به ADR-0013 / .NET MAUI) | بسته‌بندی Web View |
| ADR-0029 | معماری گزارش‌گیری سازمانی | QuestPDF (ابزارهای FastReport و RDLC صراحتاً مستثنی شدند) | SSRS |
| ADR-0033 | معماری مشاهده‌پذیری | Serilog، OpenTelemetry، Prometheus، Grafana، Tempo | Jaeger |
| ADR-0034 | مدیریت پیکربندی و اسرار | Microsoft.Extensions.Configuration/Options، HashiCorp Vault | Azure Key Vault (جایگزین) |
| ADR-0035 | مستندسازی API و تولید کلاینت | OpenAPI 3.x، Scalar، NSwag | Kiota (آینده)، Swagger UI (سنتی) |
| ADR-0036 | معماری پایپ‌لاین اعتبارسنجی | FluentValidation + رفتار پایپ‌لاین MediatR | اعتبارسنجی موردی در هندلر |
| ADR-0037 | استراتژی مهاجرت پایگاه داده | مایگریشن‌های EF Core Migrations | Flyway، Liquibase |
| ADR-0030 | مدیریت هویت و دسترسی | ASP.NET Core Identity & OpenIddict | Duende IdentityServer، Keycloak |
| ADR-0031 | معماری کشینگ | FusionCache، IMemoryCache، Redis L2 | IMemoryCache خام |
| ADR-0032 | پردازش پس‌زمینه و زمان‌بندی کارها | Quartz.NET & System.Threading.Channels | Hangfire Pro، Coravel |

---

# یادداشت استقرار و زیرساخت (Deployment & Infrastructure Note)

- **بسته‌بندی و CI/CD مصوب (ADR-0025):** کانتینرسازی داکر (Docker)، پایپ‌لاین‌های GitHub Actions، ارکستراسیون چندسرویسی محلی با .NET Aspire، و یکپارچه‌سازی Azure DevOps.
- **وضعیت کوبرنتیز (Kubernetes - k8s):** حل‌نشده / مورد باز (Open Item). کوبرنتیز رسماً توسط هیچ ADR تصویب نشده است. میزبانی محیط عملیاتی متکی بر محیط‌های کانتینری Docker و سرویس‌های ابری بومی (Cloud Native app services) است.

---

# توصیه نهایی (Final Recommendation)

حفظ و اعمال تمامی ADRهای تصویب‌شده در کلیه تیم‌های توسعه و پایپ‌لاین‌های اعتبارسنجی خودکار CI/CD.

---

# تصمیم نهایی (Final Decision)

هم‌اکنون ۳۷ سند ثبت تصمیم معماری وجود دارد. ۳۶ مورد رسماً **تصویب‌شده (Approved)** و فعال هستند؛ **ADR-0028** **منسوخ‌شده (Superseded)** است (Avalonia UI با تصمیم استاندارد پلتفرم برای .NET MAUI در ADR-0013/TE-0010 در تعارض بود و به نفع آن منسوخ شد).

ابهام موجود در ارزیابی فنی مرتبط با **ADR-0018** (که پیش‌تر به اشتباه ارجاع به `TE-0012` در متن و `TE-0018` در این فهرست بود) برطرف گردید: سند جدید **TE-0036 — ارزیابی فناوری یکپارچه‌سازی خارجی و رابط‌ها** ایجاد و رسماً تصویب شد که چارچوب رابط‌های مبتنی بر MassTransit را به عنوان سازوکار پیش‌فرض انتخاب کرده و Azure Logic Apps را به عنوان مسیر غیرپیش‌فرض و اختیاری تایید نمود.

هیچ مورد بازی باقی نمانده است.

---

# خلاصه تصمیمات (Decision Summary)

- ✔ مجموعاً ۳۷ سند ADR — ۳۶ مورد تصویب‌شده، ۱ مورد منسوخ‌شده (ADR-0028)
- ✔ ۳۶ مورد از ۳۶ ارزیابی فنی تکمیل و لینک شدند — ۲ مورد منسوخ‌شده (TE-0016، TE-0034)
- ✔ اعمال کامل معماری پاک (Clean Architecture)
- ✔ منطبق با استاندارد مستندسازی نسخه 4.0.0
- ✔ ۰ مورد باز — ابهام ADR-0018 از طریق TE-0036 جدید حل شد

---

# فهرست مرجع اسناد ADR (Master ADR Directory)

| شناسه ADR | عنوان تصمیم | وضعیت | فناوری اصلی | ارزیابی فنی مرتبط |
|-----------|-------------|--------|--------------|-------------------|
| ADR-0001 | تصویب معماری پاک و مونولیت ماژولار | تصویب‌شده | .NET 10 | `../02-architecture/TE-0001-.NET10.md` |
| ADR-0002 | خط‌مشی اولویت متن‌باز | تصویب‌شده | Permissive OSS | `../02-architecture/TE-0001-.NET10.md` |
| ADR-0003 | استفاده از .NET 10 | تصویب‌شده | .NET 10 | `../02-architecture/TE-0001-.NET10.md` |
| ADR-0004 | استفاده از Blazor | تصویب‌شده | Blazor Server / WebAssembly | `../02-architecture/TE-0002-Blazor.md` |
| ADR-0005 | استفاده از MudBlazor | تصویب‌شده | MudBlazor | `../02-architecture/TE-0003-MudBlazor.md` |
| ADR-0006 | استفاده از Entity Framework Core | تصویب‌شده | Entity Framework Core 10 | `../02-architecture/TE-0004-EntityFrameworkCore.md`، `../02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md` |
| ADR-0007 | استفاده از FluentValidation | تصویب‌شده | FluentValidation | `../02-architecture/TE-0005-FluentValidation.md`، `../02-architecture/TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` |
| ADR-0008 | استفاده از Mapster | تصویب‌شده | Mapster | `../02-architecture/TE-0006-Mapster.md`، `../02-architecture/TE-0023-Object-Mapping-Strategy-and-Technology-Evaluation.md` |
| ADR-0009 | استفاده از Serilog | تصویب‌شده | Serilog | `../02-architecture/TE-0007-Serilog.md` |
| ADR-0010 | استفاده از OpenTelemetry | تصویب‌شده | OpenTelemetry | `../02-architecture/TE-0008-OpenTelemetry.md`، `../02-architecture/TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` |
| ADR-0011 | استفاده از MediatR | تصویب‌شده | MediatR | `../02-architecture/TE-0009-Use-MediatR.md` |
| ADR-0012 | معماری فضای کاری توزیع‌شده | تصویب‌شده | موتور بسته همگام‌سازی | `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md`، `../02-architecture/TE-0026-File-Storage-Technology-Evaluation.md` |
| ADR-0013 | معماری برنامه‌های کلاینت | تصویب‌شده | .NET MAUI | `../02-architecture/TE-0010-Desktop-Mobile-Framework-Evaluation.md` |
| ADR-0014 | معماری داده‌های فضای کاری | تصویب‌شده | SQLite / LiteDB | `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md`، `../02-architecture/TE-0025-Database-Migration-Technology-Evaluation.md` |
| ADR-0015 | معماری همگام‌سازی فضای کاری | تصویب‌شده | موتور همگام‌سازی / پروتکل تفاضلی | `../02-architecture/TE-0011-Embedded-Workspace-Database-Evaluation.md` |
| ADR-0016 | معماری پیام‌رسانی سازمانی | تصویب‌شده | MassTransit & RabbitMQ | `../02-architecture/TE-0012-Enterprise-Messaging-Technology-Evaluation.md` |
| ADR-0017 | معماری هوش مصنوعی | تصویب‌شده | موتور Semantic Kernel | `../02-architecture/TE-0013-Artificial-Intelligence-Technology-Evaluation.md` |
| ADR-0018 | معماری یکپارچه‌سازی خارجی | تصویب‌شده | چارچوب رابط‌های مبتنی بر MassTransit | `../02-architecture/TE-0036-External-Integration-and-Connector-Technology-Evaluation.md` |
| ADR-0019 | استراتژی ماندگاری ترکیبی | تصویب‌شده | EF Core / Mapster / Dapper | `../02-architecture/TE-0024-Data-Access-Architecture-Evaluation.md` |
| ADR-0020 | استراتژی ذخیره‌سازی فایل | تصویب‌شده | S3 / MinIO | `../02-architecture/TE-0026-File-Storage-Technology-Evaluation.md` |
| ADR-0021 | استراتژی جستجو | تصویب‌شده | SQL Server FTS (پیش‌فرض) + OpenSearch (ارتقای مقیاس) | `../02-architecture/TE-0027-Search-Engine-Technology-Evaluation.md`؛ `../02-architecture/TE-0016-Enterprise-Search-Architecture-Evaluation.md` (منسوخ‌شده) |
| ADR-0022 | معماری بازیابی دانش هوش مصنوعی | تصویب‌شده | موتور برداری Qdrant | `../02-architecture/TE-0028-Vector-Database-Technology-Evaluation.md` |
| ADR-0023 | استراتژی ارائه‌دهنده هوش مصنوعی | تصویب‌شده | مسیریاب چند مدلی | `../02-architecture/TE-0029-Artificial-Intelligence-Provider-Technology-Evaluation.md` |
| ADR-0024 | استراتژی آزمون سازمانی | تصویب‌شده | xUnit & Testcontainers | `../02-architecture/TE-0030-Testing-Technology-Evaluation.md` |
| ADR-0025 | معماری ساخت و استقرار | تصویب‌شده | Docker، GitHub Actions، Aspire | `../02-architecture/TE-0031-Build-Packaging-and-Deployment-Technology-Evaluation.md` |
| ADR-0026 | استراتژی امنیت سازمانی (حفاظت و رمزنگاری داده‌ها) | تصویب‌شده | ASP.NET Core Data Protection، AES-256، X.509 | `../02-architecture/TE-0032-Security-Technology-Evaluation.md` |
| ADR-0027 | استراتژی آزمون کارایی و بار | تصویب‌شده | K6 & NBomber | `../02-architecture/TE-0033-Performance-and-Load-Testing-Technology-Evaluation.md` |
| ADR-0028 | معماری UI کلاینت | منسوخ‌شده | Avalonia UI (منسوخ‌شده — رجوع به ADR-0013 / .NET MAUI) | `../02-architecture/TE-0034-Client-UI-Technology-Evaluation.md` (منسوخ‌شده) |
| ADR-0029 | معماری گزارش‌گیری سازمانی | تصویب‌شده | QuestPDF (ابزارهای FastReport و RDLC صراحتاً مستثنی شدند) | `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md` |
| ADR-0030 | معماری مدیریت هویت و دسترسی | تصویب‌شده | ASP.NET Core Identity & OpenIddict | `../02-architecture/TE-0020-Authentication-and-Identity-Technology-Evaluation.md` |
| ADR-0031 | معماری کشینگ سازمانی | تصویب‌شده | FusionCache، IMemoryCache، Redis L2 | `../02-architecture/TE-0015-Caching-Architecture-Technology-Evaluation-.NET10.md` |
| ADR-0032 | پردازش پس‌زمینه و زمان‌بندی کارها | تصویب‌شده | Quartz.NET & System.Threading.Channels | `../02-architecture/TE-0014-Background-Processing-Technology-Evaluation.md`، `../02-architecture/TE-0019-Background-Processing-and-Job-Scheduling-Technology-Evaluation.md` |
| ADR-0033 | معماری مشاهده‌پذیری سازمانی | تصویب‌شده | Serilog، OpenTelemetry، Prometheus، Grafana، Tempo | `../02-architecture/TE-0017-Observability-and-Telemetry-Technology-Evaluation.md` |
| ADR-0034 | معماری مدیریت پیکربندی و اسرار | تصویب‌شده | Microsoft.Extensions.Configuration/Options، HashiCorp Vault | `../02-architecture/TE-0018-Configuration-and-Secrets-Management-Technology-Evaluation.md` |
| ADR-0035 | معماری مستندسازی API و تولید کلاینت | تصویب‌شده | OpenAPI 3.x، Scalar، NSwag | `../02-architecture/TE-0021-API-Documentation-and-Client-Generation-Technology-Evaluation.md` |
| ADR-0036 | معماری پایپ‌لاین اعتبارسنجی | تصویب‌شده | FluentValidation + رفتار پایپ‌لاین MediatR | `../02-architecture/TE-0022-Validation-Pipeline-and-Validation-Architecture-Evaluation.md` |
| ADR-0037 | استراتژی مهاجرت پایگاه داده | تصویب‌شده | مایگریشن‌های EF Core Migrations | `../02-architecture/TE-0025-Database-Migration-Technology-Evaluation.md` |

---

# اسناد مرتبط ADR (Related ADR)

- ADR-0001 — تصویب معماری پاک و مونولیت ماژولار
- ADR-0002 — خط‌مشی اولویت متن‌باز
- ADR-0012 — معماری فضای کاری توزیع‌شده
- ADR-0030 — معماری پردازش پس‌زمینه و زمان‌بندی کارها
- ADR-0031 — معماری کشینگ سازمانی

---

# اسناد مرتبط (Related Documents)

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
- `../02-architecture/TE-0016-Enterprise-Search-Architecture-Evaluation.md` (منسوخ‌شده — رجوع به ADR-0021)
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
- `../02-architecture/TE-0034-Client-UI-Technology-Evaluation.md` (منسوخ‌شده — رجوع به ADR-0013 / TE-0010)
- `../02-architecture/TE-0035-Reporting-Technology-Evaluation.md`
- `../02-architecture/TE-0036-External-Integration-and-Connector-Technology-Evaluation.md`

---

# مراجع (References)

- اصول طراحی معماری پاک (Robert C. Martin)
- الگوهای یکپارچه‌سازی سازمانی (Gregor Hohpe)
- استانداردهای معماری .NET 10

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | فهرست اولیه ADR |
| 3.0.0 | 2026-07-18 | معمار راهکار | فهرست اولیه تصمیمات معماری |
| 3.1.0 | 2026-07-27 | معمار راهکار | افزودن ADR-0019 (استراتژی ماندگاری ترکیبی برای پرس‌وجوهای خواندنی سنگین)؛ ثبت شکاف‌های موجود برای ADR-0013 تا ADR-0018 |
| 3.2.0 | 2026-07-27 | معمار راهکار | افزودن ADR-0020 (استراتژی ذخیره‌سازی فایل) |
| 3.3.0 | 2026-07-27 | معمار راهکار | افزودن ADR-0021 (استراتژی جستجو) |
| 3.4.0 | 2026-07-27 | معمار راهکار | رفع شکاف قبلی: افزودن ADR-0013 تا ADR-0018 به فهرست (همگی با وضعیت Proposed بدون TE مرتبط)، و افزودن دسته‌بندی جدید "معماری پلتفرم / فراگیر" برای ADR-0012 تا ADR-0018 |
| 3.5.0 | 2026-07-28 | معمار راهکار | افزودن ADR-0022 (معماری بازیابی دانش هوش مصنوعی)، ADR-0023 (استراتژی ارائه‌دهنده هوش مصنوعی)، ADR-0024 (استراتژی آزمون سازمانی)، و ADR-0025 (معماری ساخت و استقرار، ایجادشده برای پوشش TE-0031)؛ افزودن دسته‌بندی‌های جدید "هوش مصنوعی" و "کیفیت و آزمون"؛ انتقال ADR-0017 به دسته‌بندی هوش مصنوعی |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0؛ تجمیع کلیه ۲۹ سند ADR و ۳۵ سند TE |
| 4.1.0 | 2026-08-02 | معمار راهکار | افزودن ADR-0030 (معماری مدیریت هویت و دسترسی)، تصویب توصیه نهایی TE-0020 و دسته‌بندی هویت به عنوان ماژول پلتفرم؛ تصحیح سطرهای ADR-0026 در هر دو جدول که قبلاً اشتباهاً OpenID Connect و Keycloak را ذکر کرده بودند (تصمیم ADR-0026 صرفاً شامل Data Protection/AES-256/X.509 است) |
| 4.2.0 | 2026-08-02 | معمار راهکار | همراستاسازی عناوین ADR-0003 تا ADR-0011 با محتوای واقعی فایل‌ها؛ به‌روزرسانی ADR-0015 به معماری همگام‌سازی فضای کاری؛ افزودن ADR-0030 (پردازش پس‌زمینه و زمان‌بندی کارها) و ADR-0031 (معماری کشینگ)؛ مستندسازی وضعیت باز برای Kubernetes |
| 4.3.0 | 2026-08-02 | معمار راهکار | بازنگری و اصلاح جامع انطباق مستندات: (۱) تصحیح وضعیت ADR-0015 به تصویب‌شده؛ (۲) تثبیت رسمی فناوری پیام‌رسانی ADR-0016 بر روی MassTransit/RabbitMQ؛ (۳) علامت‌گذاری TE-0016 به عنوان منسوخ‌شده به نفع ADR-0021/TE-0027؛ (۴) علامت‌گذاری ADR-0028 و TE-0034 (Avalonia UI) به عنوان منسوخ‌شده به نفع .NET MAUI در ADR-0013/TE-0010؛ (۵) تصحیح ارجاع نادرست TE-0012 در ADR-0017 به TE-0013 و تثبیت تصمیم Semantic Kernel؛ (۶) ایجاد ADR-0033 تا ADR-0037؛ (۷) اصلاح ارجاع خراب 09-CapabilityModel.md به 02-CapabilityModel.md؛ (۸) اصلاح سطرهای جداول برای تطابق دقیق با متن بدنه ADRها؛ (۹) ثبت ارجاع TE برای ADR-0018 به عنوان مورد باز |
| 4.4.0 | 2026-08-08 | معمار راهکار | حل مورد باز ADR-0018: ایجاد TE-0036 (ارزیابی فناوری یکپارچه‌سازی خارجی و رابط‌ها)، ارزیابی جامع گزینه‌ها و تصویب MassTransit-based Connector Framework به عنوان پیش‌فرض و Azure Logic Apps به صورت اختیاری. تصحیح ارجاع در ADR-0018 و این فهرست. صفر مورد باز باقی مانده است. |
