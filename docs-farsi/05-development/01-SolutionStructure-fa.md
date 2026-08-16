| ویژگی | مقدار |
|---|---|
| **شناسه سند** | DOC-DEV-002 |
| **عنوان** | ساختار راهکار (Solution Structure) |
| **نسخه** | 4.2.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-12 |

---

# هدف (Purpose)

این سند، ساختار کلی راهکار (Solution Structure) را برای پروژه **MachineryManagerEnterprise** تعریف می‌کند.

هدف از ساختار راهکار این است که اطمینان حاصل شود هر پروژه دارای یک مسئولیت شفاف، وابستگی‌های قابل پیش‌بینی و قابلیت نگهداری بلندمدت است.

ساختار راهکار بالاترین سطح سازمانی سورس‌کد به شمار می‌رود.

---

# اهداف (Objectives)

این راهکار باید:

- دغدغه‌های تجاری را تفکیک کند
- زیرساخت را ایزوله سازد
- از توسعه ماژولار پشتیبانی کند
- آزمون مستقل را امکان‌پذیر سازد
- وابستگی و جفت‌شدگی (Coupling) را کاهش دهد
- قابلیت نگهداری را ارتقا بخشد

---

## پیکربندی متمرکز ساخت (Central Build Configuration)

این راهکار از پیکربندی متمرکز MSBuild استفاده می‌کند.

- فایل `Directory.Build.props` شامل مشخصات عمومی MSBuild است که میان تمامی پروژه‌ها به اشتراک گذاشته شده است.
- فایل `Directory.Packages.props` نسخه‌های بسته‌های NuGet را به صورت متمرکز مدیریت می‌کند (Central Package Management).
- تمامی فایل‌های پروژه این تنظیمات را به طور خودکار به ارث می‌برند.
- فایل‌های پروژه نباید مشخصات مشترک MSBuild یا نسخه‌های بسته‌ها را تکرار کنند.

تمامی مشخصات مشترک MSBuild از `Directory.Build.props` به ارث می‌رسند.

چارچوب هدف (`TargetFramework`) به صورت متمرکز در `Directory.Build.props` تعریف شده است.

فایل‌های پروژه مجزا نباید `TargetFramework` را مجدداً تعریف کنند مگر اینکه صریحاً مستند شده باشد.

فایل‌های پروژه مجزا تنها باید شامل پیکربندی‌های مختص همان پروژه باشند، از جمله:

- انتخاب SDK
- نوع خروجی (`OutputType`)
- شناسه اسرار کاربر (`UserSecretsId`)
- تنظیمات مختص Razor
- مراجع پروژه (`ProjectReference`)
- مراجع بسته (`PackageReference`)

---

# سبک معماری (Architectural Style)

این راهکار از اصول زیر پیروی می‌کند:

- طراحی دامنه‌محور (Domain-Driven Design - DDD)
- معماری تمیز (Clean Architecture)
- معماری برشی عمودی (Vertical Slice Architecture - در موارد مناسب)
- وارونگی وابستگی (Dependency Inversion)
- اصول SOLID

---

# سازمان‌دهی راهکار (Solution Organization)

این راهکار در چندین پروژه با مسئولیت‌های شفاف و مشخص سازمان‌دهی شده است.

یک سازمان‌دهی سطح بالای متداول در زیر نشان داده شده است:

```text
MachineryManagerEnterprise.sln

│
├── src
│
├── tests
│
├── docs
│
├── build
│
└── tools
```

---

# پروژه‌های سورس (Source Projects)

سورس‌کد به لایه‌های منطقی تقسیم شده است:

```text
src

BuildingBlocks

Modules

Host
```

هر پروژه دارای یک مسئولیت مشخص و یگانه است.

---

## BuildingBlocks

```text
BuildingBlocks

MachineryManager.SharedKernel

MachineryManager.SharedKernel.Contracts

MachineryManager.SharedKernel.Abstractions

MachineryManager.SharedKernel.Infrastructure

MachineryManager.UI
```

لایه BuildingBlocks شامل مؤلفه‌های با قابلیت استفاده مجدد است که میان تمامی ماژول‌ها به اشتراک گذاشته شده‌اند.

منطق تجاری هرگز نباید در BuildingBlocks پیاده‌سازی شود.

---

## ماژول‌ها (Modules)

کارکردهای تجاری باید به عنوان زمینه‌های مرزبندی‌شده (Bounded Contexts) مستقل پیاده‌سازی شوند.

هر ماژول در ساختار درونی خود از معماری تمیز (Clean Architecture) پیروی می‌کند:

```text
Modules

AssetManagement

AssetManagement.Domain

AssetManagement.Application

AssetManagement.Infrastructure

AssetManagement.Presentation
```

همین ساختار باید برای هر ماژول تجاری تکرار شود.

---

## میزبان (Host)

```text
Host

MachineryManager.Server

MachineryManager.Client
```

لایه Host کل برنامه را ترکیب کرده و تزریق وابستگی، میان‌افزارها (Middleware) و راه‌اندازی برنامه را پیکربندی می‌نماید.

---

# جهت وابستگی (Dependency Direction)

وابستگی‌ها همواره باید به سمت درون (Inward) اشاره کنند:

```text
لایه ارائه (Presentation)

↓

لایه کاربرد (Application)

↓

لایه دامنه (Domain)

↓

BuildingBlocks
```

لایه زیرساخت (Infrastructure) از لایه‌های بالاتر پشتیبانی می‌کند اما نباید منطق تجاری وارد کند.

---

# BuildingBlocks

لایه BuildingBlocks شامل مؤلفه‌های با قابلیت استفاده مجدد است که در سراسر راهکار به اشتراک گذاشته شده‌اند.

این لایه شامل موارد زیر است:

- SharedKernel (هسته مشترک)
- Contracts (قراردادها)
- Abstractions (انتزاع‌ها)
- Infrastructure (پیاده‌سازی‌های دغدغه‌های عرضی، مانند رفتارهای خط لوله مشترک MediatR)
- UI Shared Components (مؤلفه‌های مشترک رابط کاربری)

لایه BuildingBlocks هرگز نباید به هیچ ماژول تجاری وابسته باشد.

---

# لایه دامنه (Domain Layer)

لایه دامنه شامل موارد زیر است:

- موجودیت‌ها (Entities)
- اشیاء مقداری (Value Objects)
- تجمیع‌ها (Aggregates)
- سرویس‌های دامنه (Domain Services)
- رخدادهای دامنه (Domain Events)
- قواعد تجاری (Business Rules)

لایه دامنه نباید شامل هیچ کدی از لایه زیرساخت باشد.

---

# لایه کاربرد (Application Layer)

لایه کاربرد شامل موارد زیر است:

- مورداستفاده‌ها (Use Cases)
- فرمان‌ها (Commands)
- کوئری‌ها (Queries)
- اعتبارسنج‌ها (Validators)
- اشیاء انتقال داده (DTOs)
- اینترفیس‌ها (Interfaces)
- نگاشت‌ها (Mapping)

گردش کارهای تجاری متعلق به این لایه هستند.

---

# لایه زیرساخت (Infrastructure Layer)

زیرساخت شامل پیاده‌سازی‌های فنی است.

نمونه‌ها:

- Entity Framework Core
- مخازن داده (Repositories)
- سرویس‌های خارجی (External Services)
- ذخیره‌سازی فایل (File Storage)
- لاگ‌گیری (Logging)
- کش‌سازی (Caching)

لایه زیرساخت باید انتزاع‌های تعریف‌شده توسط لایه‌های بالاتر را پیاده‌سازی کند.

---

# لایه ارائه (Presentation Layer)

لایه ارائه شامل موارد زیر است:

- رابط کاربری Blazor
- مؤلفه‌ها (Components)
- صفحات (Pages)
- مدل‌های نما (View Models)

لایه ارائه نباید حاوی قواعد تجاری باشد.

---

# پروژه‌های آزمون (Test Projects)

پروژه‌های آزمون باید ساختار راهکار تولیدی را منعکس کنند:

```text
tests

SharedKernel.Tests

AssetManagement.Tests

Maintenance.Tests

Inventory.Tests

Fleet.Tests

Procurement.Tests

Workshop.Tests

Reporting.Tests

Identity.Tests
```

---

# اصول نام‌گذاری (Naming Principles)

پروژه‌ها باید:

- از نگارش PascalCase استفاده کنند
- با فضاهای نام (Namespaces) مطابقت داشته باشند
- مسئولیت‌ها را منعکس سازند
- از نام‌های مبهم پرهیز نمایند

---

# مقیاس‌پذیری (Scalability)

ساختار راهکار به نحوی طراحی شده است که از توسعه‌های آتی بدون نیاز به بازسازی عمده پشتیبانی کند.

ماژول‌های جدید باید از طریق پروژه‌ها یا زمینه‌های مرزبندی‌شده جدید اضافه شوند، نه با اصلاح مؤلفه‌های نامرتبط.

هر زمینه مرزبندی‌شده باید به عنوان یک ماژول مستقل پیاده‌سازی شود.

ماژول‌ها تنها از طریق قراردادها و مرزهای لایه کاربرد ارتباط برقرار می‌کنند.

استخراج آتی به سرویس‌های مستقل نیازمند بازسازی معماری نخواهد بود.

---

# انطباق (Compliance)

تمامی پروژه‌های جدید معرفی‌شده در راهکار باید با این ساختار مطابقت داشته باشند.

انحرافات معماری نیازمند یک سند تصمیم معماری (ADR) تصویب‌شده هستند.

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط (Related Documents)

- DOC-CONVENTIONS
- DOC-README
- DOC-DEV-001 (اصول توسعه / Development Principles)
- DOC-DEV-003 (ساختار پروژه / Project Structure)
- DOC-DEV-005 (قواعد وابستگی / Dependency Rules)
- ADR-0001
- MOD-000 (معماری کاربرد / Application Architecture)
- DOM-003 (زمینه‌های مرزبندی‌شده / Bounded Contexts)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | ساختار اولیه راهکار |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 3.1.0 | 2026-07-26 | معمار راهکار | هوش مصنوعی + تیم پروژه \| به‌روزرسانی راه‌اندازی راهکار برای NET 10.0.302.، پیکربندی متمرکز MSBuild و مدیریت متمرکز بسته‌ها (CPM). |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-12 | معمار راهکار | اصلاح مراجع نادرست DOC-MOD-001 و DOC-DOM-002 به اسناد واقعی MOD-000 و DOM-003 |
| 4.2.0 | 2026-08-12 | معمار راهکار | تصحیح نام‌های پروژه‌های BuildingBlocks برای تطابق با راهکار واقعی (MachineryManager.SharedKernel.Contracts / .Abstractions / .Infrastructure) و افزودن زیرپروژه مفقود Infrastructure |
