| ویژگی | مقدار |
|---|---|
| **شناسه سند** | DOC-DEV-003 |
| **عنوان** | ساختار پروژه (Project Structure) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند، ساختار داخلی را که هر پروژه در درون راهکار **MachineryManagerEnterprise** باید از آن پیروی کند، تعریف می‌نماید.

یک ساختار پروژه منسجم و یکپارچه موارد زیر را بهبود می‌بخشد:

- قابلیت نگهداری (Maintainability)
- قابلیت کشف‌پذیری (Discoverability)
- بازبینی‌های کد (Code Reviews)
- مقیاس‌پذیری (Scalability)
- همکاری تیمی (Team Collaboration)

هر پروژه باید از این قراردادها پیروی کند مگر اینکه یک سند تصمیم معماری (ADR) تصویب‌شده صریحاً یک استثنا را تعریف کرده باشد.

---

# اهداف (Objectives)

ساختار پروژه باید:

- کدهای مرتبط را در کنار هم نگه دارد.
- پیچیدگی ناوبری و جابجایی میان کدها را به حداقل برساند.
- انسجام ویژگی‌ها (Feature Cohesion) را تقویت کند.
- وابستگی‌های تصادفی را کاهش دهد.
- از قابلیت نگهداری بلندمدت پشتیبانی نماید.

---

# اصول عمومی (General Principles)

هر پروژه باید:

- دارای یک مسئولیت یگانه باشد.
- تنها حاوی فایل‌های مرتبط با آن مسئولیت باشد.
- از تودرتویی غیرضروری پوشه‌ها پرهیز کند.
- از قراردادهای نام‌گذاری پروژه پیروی نماید.
- به طور منسجم با پروژه‌های مشابه سازمان‌دهی شود.

---

# چیدمان استاندارد پروژه (Standard Project Layout)

یک پروژه متداول باید از ساختار زیر پیروی کند:

```text
Project

│
├── Abstractions
│
├── Configuration
│
├── Constants
│
├── Contracts
│
├── Exceptions
│
├── Extensions
│
├── Features
│
├── Interfaces
│
├── Mapping
│
├── Models
│
├── Options
│
├── Services
│
├── Utilities
│
└── Validation
```

هر پروژه لزوماً نیازمند تمامی این پوشه‌ها نیست.

پوشه‌ها تنها در صورت نیاز باید ایجاد شوند.

---

# ریشه مخزن (Repository Root)

```text
/
├── docs/
├── src/
├── tests/
├── global.json
├── Directory.Build.props
├── Directory.Packages.props
├── MachineryManagerEnterprise.slnx
└── README.md
```

---

# سازمان‌دهی ویژگی‌ها (Feature Organization)

کارکردهای تجاری باید تا جای ممکن بر اساس ویژگی (Feature) گروه‌بندی شوند.

نمونه:

```text
Features

Inventory

Maintenance

Users

Reports

Dashboard
```

هر ویژگی باید تا حد امکان کاربردی، مستقل باقی بماند.

---

# چیدمان ویژگی (Feature Layout)

یک ویژگی می‌تواند شامل موارد زیر باشد:

```text
Inventory

Commands

Queries

DTOs

Validators

Mappings

Services
```

این چیدمان ضمن حفظ سازگاری کامل با معماری تمیز (Clean Architecture)، از معماری برش عمودی (Vertical Slice Architecture) پشتیبانی می‌کند.

---

# پیکربندی (Configuration)

کلاس‌های پیکربندی باید ایزوله باشند:

```text
Configuration

DependencyInjection

OptionsConfiguration

MiddlewareConfiguration
```

---

# متدهای الحاقی (Extensions)

متدهای الحاقی باید بر اساس مسئولیت گروه‌بندی شوند.

نمونه:

```text
Extensions

ServiceCollectionExtensions

ApplicationBuilderExtensions

StringExtensions
```

---

# نگاشت (Mapping)

نگاشت‌های اشیاء باید متمرکز باشند.

کتابخانه‌های نگاشت پشتیبانی‌شده باید از همین سازمان‌دهی پیروی کنند.

نمونه:

```text
Mapping

InventoryProfile

UserProfile

MachineProfile
```

---

# اعتبارسنجی (Validation)

اعتبارسنج‌ها (Validators) باید در کنار یکدیگر گروه‌بندی شوند.

نمونه:

```text
Validation

CreateMachineValidator

UpdateMachineValidator

DeleteMachineValidator
```

---

# سرویس‌ها (Services)

تنها سرویس‌های مرتبط با کسب‌وکار باید در اینجا قرار گیرند.

سرویس‌های زیرساختی متعلق به پروژه Infrastructure هستند.

---

# ابزارها (Utilities)

کلاس‌های ابزاری باید کوچک و بدون وضعیت (Stateless) باقی بمانند.

منطق تجاری هرگز نباید در درون کلاس‌های ابزاری پیاده‌سازی شود.

---

# قواعد نام‌گذاری (Naming Rules)

پوشه‌ها:

- PascalCase

فایل‌ها:

- PascalCase

کلاس‌ها:

- PascalCase

اینترفیس‌ها:

- پیشوند `I`

نمونه‌ها:

```
MachineService

IMachineRepository

MachineProfile

MachineValidator
```

---

# خط‌مشی ایجاد پوشه (Folder Creation Policy)

پوشه‌ها نباید به پیشواز نیازمندی‌های آینده و بدون محتوا ایجاد شوند.

یک پوشه تنها زمانی معرفی می‌شود که:

- حاوی محتوای معنادار باشد.
- وجود چندین فایل ایجاد آن را توجیه کند.
- سازمان‌دهی کد را بهبود بخشد.

از ایجاد پوشه‌های خالی پرهیز کنید.

---

# مقیاس‌پذیری (Scalability)

ساختار پروژه عمداً به نحوی طراحی شده است که از رشد آینده پشتیبانی کند.

با تکامل ماژول‌ها، ویژگی‌های جدید باید بدون نیاز به بازسازی بخش‌های نامرتبط پروژه افزوده شوند.

---

# انطباق (Compliance)

هر پروژه تازه ایجادشده باید از این ساختار پیروی کند مگر اینکه یک سند ADR تصویب‌شده سازمان‌دهی متفاوتی را تعریف کرده باشد.

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
- DOC-DEV-002 (ساختار راهکار / Solution Structure)
- DOC-DEV-004 (قرارداد فضای نام / Namespace Convention)
- DOC-DEV-005 (قواعد وابستگی / Dependency Rules)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | ساختار اولیه پروژه |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
