| ویژگی | مقدار |
|---|---|
| **شناسه سند** | DOC-DEV-012 |
| **عنوان** | کاتالوگ وابستگی‌ها (Dependency Catalog) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند، فرایند رسمی حاکمیت بر وابستگی‌ها را برای راهکار **MachineryManagerEnterprise** تعریف می‌نماید.

این سند به عنوان ثبت رسمی و معتبر تمامی کتابخانه‌های شخص ثالث اتخاذشده توسط پروژه عمل می‌کند.

هر وابستگی که به راهکار وارد می‌شود باید در این کاتالوگ ثبت گردد.

---

# اهداف (Objectives)

کاتالوگ وابستگی‌ها باید:

- از رشد کنترل‌نشده بسته‌ها جلوگیری کند.
- تصمیمات معماری را ثبت نماید.
- قابلیت نگهداری را بهبود بخشد.
- ارتقاء بسته‌ها را ساده سازد.
- از بازبینی‌های امنیتی پشتیبانی کند.
- انطباق با مجوزها (Licenses) را تضمین نماید.

---

# اولویت متن‌باز (Open Source First)

این راهکار خط‌مشی **اولویت متن‌باز** را اتخاذ کرده است.

تنها کتابخانه‌های متن‌باز مجاز به ورود هستند مگر اینکه یک سند ADR تصویب‌شده صریحاً یک استثنا را مستند کرده باشد.

مشاهده کنید:

- ADR-0002 — خط‌مشی اولویت متن‌باز

---

# چرخه حیات وابستگی (Dependency Lifecycle)

هر وابستگی از یک چرخه حیات یکسان پیروی می‌کند:

```text
نیاز (Need)

↓

ارزیابی فناوری (Technology Evaluation - TE)

↓

اثبات مفهوم (Proof of Concept - اختیاری)

↓

سند ثبت تصمیم معماری (Architecture Decision Record - ADR)

↓

تصویب‌شده (Approved)

↓

Directory.Packages.props

↓

پیاده‌سازی (Implementation)

↓

نگهداری (Maintenance)
```

هیچ بسته‌ای مجاز به دور زدن این فرایند نیست.

---

# مدیریت متمرکز بسته‌ها (Central Package Management)

تمامی نسخه‌های بسته‌های NuGet به صورت متمرکز از طریق `Directory.Packages.props` مدیریت می‌شوند.
فایل‌های پروژه شامل عناصر `PackageReference` بدون مشخصه `Version` هستند.

نسخه‌های بسته‌ها به صورت متمرکز از طریق `Directory.Packages.props` مدیریت می‌شوند.

فایل‌های پروژه نباید مشخصه `Version` را در عناصر `PackageReference` تعریف کنند.

تنها مرجع معتبر (Single source of truth) عبارت است از:

```text
Directory.Packages.props
```

فایل‌های پروژه هرگز نباید حاوی نسخه‌های بسته‌ها باشند.

---

# دسته‌بندی‌های وابستگی (Dependency Categories)

وابستگی‌ها در دسته‌های مختلف گروه‌بندی می‌شوند:

نمونه‌ها:

- فریم‌ورک (Framework)
- اعتبارسنجی (Validation)
- پایداری داده‌ها (Persistence)
- نگاشت (Mapping)
- لاگ‌گیری (Logging)
- آزمون (Testing)
- مؤلفه‌های رابط کاربری (UI Components)
- ابزارها (Utilities)

---

# دفتر ثبت وابستگی‌ها (Dependency Register)

| بسته (Package) | دسته | TE | ADR | وضعیت | یادداشت‌ها |
|---|---|---|---|---|---|
| Blazor (Server / WebAssembly) | Framework | TE-0002 | ADR-0004 | تصویب‌شده (Approved) | فریم‌ورک رابط کاربری وب |
| MudBlazor | UI Components | TE-0003 | ADR-0005 | تصویب‌شده (Approved) | کتابخانه مؤلفه‌های Blazor |
| Microsoft.EntityFrameworkCore | Persistence | TE-0004, TE-0024 | ADR-0006 | تصویب‌شده (Approved) | ابزار ORM اصلی / سمت نوشتن |
| FluentValidation | Validation | TE-0005, TE-0022 | ADR-0007, ADR-0036 | تصویب‌شده (Approved) | اعتبارسنجی درخواست‌ها |
| MediatR.Extensions.FluentValidation (یا رفتار معادل خط لوله) | Validation | TE-0022 | ADR-0036 | تصویب‌شده (Approved) | ارکستراسیون خط لوله اعتبارسنجی |
| Mapster | Mapping | TE-0006, TE-0023 | ADR-0008 | تصویب‌شده (Approved) | نگاشت اشیاء |
| Serilog | Logging | TE-0007, TE-0017 | ADR-0009, ADR-0033 | تصویب‌شده (Approved) | ارائه‌دهنده لاگ‌گیری ساختاریافته |
| OpenTelemetry | Observability | TE-0008, TE-0017 | ADR-0010, ADR-0033 | تصویب‌شده (Approved) | استاندارد تله‌متری یکپارچه |
| Prometheus (کلاینت/اکسپورتور) | Observability | TE-0017 | ADR-0033 | تصویب‌شده (Approved) | بک‌اند متریک‌ها |
| Grafana | Observability | TE-0017 | ADR-0033 | تصویب‌شده (Approved) | داشبورد / بصری‌سازی |
| Grafana Tempo | Observability | TE-0017 | ADR-0033 | تصویب‌شده (Approved) | بک‌اند ردیابی توزیع‌شده |
| MediatR | Framework | TE-0009 | ADR-0011 | تصویب‌شده (Approved) | خط لوله CQRS |
| NET MAUI. | UI Framework | TE-0010 | ADR-0013 | تصویب‌شده (Approved) | فریم‌ورک کلاینت دسکتاپ و موبایل |
| SQLite | Persistence | TE-0011 | ADR-0014 | تصویب‌شده (Approved) | پایگاه داده تعبیه‌شده فضای کاری |
| LiteDB | Persistence | TE-0011 | ADR-0014 | تصویب‌شده (Approved) | پایگاه داده تعبیه‌شده فضای کاری (جایگزین) |
| MassTransit | Messaging | TE-0012 | ADR-0016, ADR-0018 | تصویب‌شده (Approved) | انتزاع پیام‌رسانی؛ همچنین پشتیبان فریم‌ورک کانکتور یکپارچه‌سازی خارجی |
| RabbitMQ | Messaging | TE-0012 | ADR-0016 | تصویب‌شده (Approved) | کارگزار پیام (Message broker) |
| Semantic Kernel | AI | TE-0013 | ADR-0017 | تصویب‌شده (Approved) | فریم‌ورک ارکستراسیون هوش مصنوعی |
| Dapper | Persistence | TE-0024 | ADR-0019 | تصویب‌شده (Approved) | فقط کوئری‌های با خواندن سنگین / گزارش‌گیری، هرگز برای DDL |
| MinIO | Storage | TE-0026 | ADR-0020 | تصویب‌شده (Approved) | ذخیره‌ساز شیء سازگار با S3 (پیش‌فرض) |
| AWSSDK.S3 (یا کلاینت معادل سازگار با S3) | Storage | TE-0026 | ADR-0020 | تصویب‌شده (Approved) | کلاینت S3 API |
| Qdrant.Client | AI / Search | TE-0028 | ADR-0022 | تصویب‌شده (Approved) | کلاینت پایگاه داده برداری |
| Azure OpenAI SDK | AI | TE-0029 | ADR-0023 | تصویب‌شده (Approved) | ارائه‌دهنده اصلی هوش مصنوعی |
| OpenAI SDK | AI | TE-0029 | ADR-0023 | تصویب‌شده (Approved) | ارائه‌دهنده ثانویه هوش مصنوعی |
| Ollama (کلاینت) | AI | TE-0029 | ADR-0023 | تصویب‌شده (Approved) | ارائه‌دهنده هوش مصنوعی محلی/آفلاین |
| xUnit | Testing | TE-0030 | ADR-0024 | تصویب‌شده (Approved) | فریم‌ورک آزمون |
| Moq | Testing | TE-0030 | ADR-0024 | تصویب‌شده (Approved) | فریم‌ورک ماک‌سازی |
| Testcontainers | Testing | TE-0030 | ADR-0024 | تصویب‌شده (Approved) | زیرساخت آزمون یکپارچه‌سازی |
| Docker | Build / Deployment | TE-0031 | ADR-0025 | تصویب‌شده (Approved) | کانتینرسازی |
| NET Aspire. | Build / Deployment | TE-0031 | ADR-0025 | تصویب‌شده (Approved) | ارکستراسیون چندسرویسی محلی |
| GitHub Actions | Build / Deployment | TE-0031 | ADR-0025 | تصویب‌شده (Approved) | خط لوله CI/CD |
| Microsoft.AspNetCore.DataProtection | Security | TE-0032 | ADR-0026 | تصویب‌شده (Approved) | حفاظت از داده‌های کاربردی |
| k6 | Testing | TE-0033 | ADR-0027 | تصویب‌شده (Approved) | آزمون بار |
| NBomber | Testing | TE-0033 | ADR-0027 | تصویب‌شده (Approved) | آزمون بار / کارایی بومی NET. |
| QuestPDF | Reporting | TE-0035 | ADR-0029 | تصویب‌شده (Approved) | تولید PDF |
| ASP.NET Core Identity | Identity | TE-0020 | ADR-0030 | تصویب‌شده (Approved) | مدیریت هویت |
| OpenIddict | Identity | TE-0020 | ADR-0030 | تصویب‌شده (Approved) | سرور OAuth2/OIDC |
| FusionCache (یا HybridCache) | Caching | TE-0015 | ADR-0031 | تصویب‌شده (Approved) | کش هیبریدی L1/L2 |
| Microsoft.Extensions.Caching.Memory (IMemoryCache) | Caching | TE-0015 | ADR-0031 | تصویب‌شده (Approved) | کش درون‌فرایندی L1 |
| StackExchange.Redis | Caching | TE-0015 | ADR-0031 | تصویب‌شده (Approved) | کش توزیع‌شده L2 |
| Quartz.NET | Scheduling | TE-0014, TE-0019 | ADR-0032 | تصویب‌شده (Approved) | زمان‌بندی کارها |
| System.Threading.Channels | Scheduling | TE-0014, TE-0019 | ADR-0032 | تصویب‌شده (Approved) | صف‌های پس‌زمینه درون‌فرایندی |
| Microsoft.Extensions.Configuration | Configuration | TE-0018 | ADR-0034 | تصویب‌شده (Approved) | انتزاع پیکربندی |
| Microsoft.Extensions.Options | Configuration | TE-0018 | ADR-0034 | تصویب‌شده (Approved) | پیکربندی با نوع‌بندی قوی |
| Microsoft.FeatureManagement | Configuration | TE-0018 | ADR-0034 | تصویب‌شده (Approved) | فلگ‌های ویژگی (Feature flags) |
| HashiCorp Vault (کلاینت) | Configuration | TE-0018 | ADR-0034 | تصویب‌شده (Approved) | مخزن اسرار سازمانی |
| Azure.Security.KeyVault.Secrets | Configuration | TE-0018 | ADR-0034 | تصویب‌شده (Approved) | جایگزین مخزن اسرار مختص Azure |
| Scalar.AspNetCore | API Documentation | TE-0021 | ADR-0035 | تصویب‌شده (Approved) | مستندسازی تعاملی API |
| NSwag | API Documentation | TE-0021 | ADR-0035 | تصویب‌شده (Approved) | تولید کلاینت SDK سی‌شارپ |
| EF Core Migrations (ابزار) | Persistence | TE-0025 | ADR-0037 | تصویب‌شده (Approved) | مایگریشن‌های شمای پایگاه داده، مالک انحصاری شما |
| Avalonia UI | UI Framework | TE-0034 | ADR-0028 | منسوخ‌شده (Deprecated) | جایگزین‌شده توسط NET MAUI (ADR-0013). — در کدهای جدید وارد نشود |
| FluentAvalonia | UI Components | TE-0034 | ADR-0028 | منسوخ‌شده (Deprecated) | جایگزین‌شده توسط NET MAUI (ADR-0013). |
| CommunityToolkit.Mvvm | MVVM | TE-0034 | ADR-0028 | منسوخ‌شده (Deprecated) | همراه با Avalonia جفت شده بود؛ قبل از استفاده مجدد برای MAUI بازبینی شود |
| Azure Logic Apps | Integration | TE-0036 | ADR-0018 | تصویب‌شده (Approved) | مسیر اختیاری و غیراصلی یکپارچه‌سازی مختص Azure |

---

# تعاریف وضعیت (Status Definitions)

| وضعیت | معنا |
|---|---|
| پیشنهادی (Proposed) | تحت ارزیابی |
| تصویب‌شده (Approved) | وابستگی رسمی |
| منسوخ‌شده (Deprecated) | برنامه‌ریزی‌شده برای حذف |
| ردشده (Rejected) | پذیرفته‌نشده |

---

# خط‌مشی ارتقاء (Upgrade Policy)

وابستگی‌ها باید به طور منظم به‌روزرسانی شوند.

پیش از ارتقاء:

- یادداشت‌های انتشار (Release notes) را بررسی کنید.
- سازگاری را راستی‌آزمایی نمایید.
- آزمون‌های خودکار را اجرا کنید.
- در صورت تغییر در رفتار معماری، سند ADR را به‌روزرسانی نمایید.

---

# امنیت (Security)

وابستگی‌ها باید برای موارد زیر مانیتور شوند:

- آسیب‌پذیری‌های شناخته‌شده
- نسخه‌های پشتیبانی‌نشده
- تغییرات مجوز
- وضعیت نگهداری

آسیب‌پذیری‌های بحرانی نیازمند بازبینی و اقدام فوری هستند.

---

# خط‌مشی حذف (Removal Policy)

وابستگی‌های بلااستفاده باید حذف شوند.

فرایند حذف:

1. اطمینان حاصل کنید هیچ ارجاعی در پروژه‌ها باقی نمانده است.
2. از پیاده‌سازی کد حذف نمایید.
3. از `Directory.Packages.props` حذف کنید.
4. این کاتالوگ را به‌روزرسانی نمایید.
5. وظیفه نگهداری مرتبط را ببندید.

---

# کتابخانه‌های تجربی (Experimental Libraries)

بسته‌های تجربی هرگز نباید مستقیماً به محیط تولید افزوده شوند.

آن‌ها ابتدا باید مراحل زیر را طی کنند:

- ارزیابی فناوری (Technology Evaluation)
- اثبات مفهوم (Proof of Concept)

---

# نسخه‌بندی (Versioning)

نسخه‌های پایدار همواره ترجیح داده می‌شوند.

بسته‌های پیش‌نمایش (Preview) نیازمند تأییدیه صریح معماری هستند.

---

# انطباق (Compliance)

هر وابستگی شخص ثالث واردشده به راهکار باید در این کاتالوگ مستند شود.

وابستگی‌های مستندنشده مجاز نیستند.

---

# اسناد مرتبط (Related Documents)

- DOC-CONVENTIONS
- DOC-README
- ADR-0002 (خط‌مشی اولویت متن‌باز / Open Source First Policy)
- ADR-0007 (استفاده از FluentValidation / Use FluentValidation)
- TE-0005 (ارزیابی FluentValidation / FluentValidation Evaluation)

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | کاتالوگ اولیه وابستگی‌ها |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-08 | معمار راهکار | تکمیل کامل دفتر ثبت وابستگی‌ها (که قبلاً یک سطر نمونه فقط برای FluentValidation بود) با تمام بسته‌های تصویب‌شده در ADR-0003 تا ADR-0037، از جمله ورودی‌های منسوخ‌شده برای Avalonia UI / FluentAvalonia / CommunityToolkit.Mvvm (جایگزین‌شده توسط ADR-0013) |
