| ویژگی | مقدار |
|---|---|
| **شناسه سند** | MOD-007 |
| **عنوان** | مدل اعطای مجوز و دسترسی (Authorization model) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند، مدل اعطای مجوز (Authorization Model) پلتفرم MachineryManagerEnterprise را تعریف می‌کند.

اعطای مجوز تعیین می‌کند که **چه کسی مجاز است کدام عملیات تجاری را انجام دهد**.

احراز هویت (Authentication)، هویت کاربر را شناسایی می‌کند.

اعطای مجوز (Authorization)، سطوح دسترسی و مجوزها را تعیین می‌نماید.

---

# فلسفه اعطای مجوز (Authorization Philosophy)

اعطای مجوز از عملیات تجاری محافظت می‌کند.

اعطای مجوز هرگز حاوی منطق تجاری نیست.

قواعد تجاری (Business Rules) در درون لایه دامنه (Domain) باقی می‌مانند.

اعطای مجوز تعیین می‌کند که آیا اجرای عملیات قبل از اینکه لایه کاربرد (Application Layer) اقدام به فراخوانی مدیریت‌کننده (Handler) کند مجاز است یا خیر.

---

# ۲. اصول اعطای مجوز (Authorization Principles)

سیستم اعطای مجوز باید اصول زیر را برآورده سازد:

- کنترل دسترسی مبتنی بر نقش (RBAC - Role Based Access Control)
- اصل حداقل دسترسی (Least Privilege)
- مجوزهای مبتنی بر کسب‌وکار (Business oriented permissions)
- اعطای مجوز متمرکز (Centralized authorization)
- تصمیمات قابل حسابرسی (Auditable decisions)
- مستقل از فناوری (Technology independent)

اعطای مجوز هرگز نباید به پیاده‌سازی رابط کاربری وابسته باشد.

---

# ۳. مدل اعطای مجوز (Authorization Model)

```text
کاربر (User)

↓

نقش (Role)

↓

مجوز (Permission)

↓

عملیات تجاری (Business Operation)
```

یک کاربر می‌تواند دارای چندین نقش باشد.

یک نقش می‌تواند شامل چندین مجوز باشد.

---

# ۴. دسته‌بندی‌های مجوزها (Permission Categories)

مجوزها بر اساس دامنه‌های تجاری گروه‌بندی می‌شوند:

```text
مجوزها (Permissions)

├── دارایی (Asset)
├── موتور (Engine)
├── قطعات و اجزا (Components)
├── کنتور و کارکردسنج (Meter)
├── نگهداری و تعمیرات (Maintenance)
├── مالی (Financial)
├── اسناد (Documents)
├── پیش‌بینی (Forecast)
├── گزارش‌گیری (Reporting)
├── مدیریت سیستم (Administration)
└── پیکربندی (Configuration)
```

---

# ۵. نقش‌های استاندارد (Standard Roles)

نقش‌های زیر به عنوان بخشی از هسته اصلی پلتفرم در نظر گرفته می‌شوند:

- مدیر سیستم (System Administrator)
- مدیر سازمان (Organization Administrator)
- مدیر ناوگان (Fleet Manager)
- مدیر نگهداری و تعمیرات (Maintenance Manager)
- تکنسین نگهداری و تعمیرات (Maintenance Technician)
- سرپرست کارگاه (Workshop Supervisor)
- اپراتور (Operator)
- مسئول مالی (Financial Officer)
- مسئول تدارکات و خرید (Procurement Officer)
- کنترل‌کننده اسناد (Document Controller)
- حسابرس فقط-خواندنی (Read-Only Auditor)

نقش «مدیر سیستم» در سطح کل پلتفرم و در سراسر سازمان‌ها عمل می‌کند. نقش «مدیر سازمان» محدود به یک سازمان (مستاجر / Tenant) واحد است و دارای مجوزهای مدیریتی در محدوده سازمان از جمله `Organization.Manage` (به بخش ۱۴ مراجعه فرمایید) تنها در داخل همان مرز سازمانی است.

سازمان‌ها می‌توانند نقش‌های بیشتری را تعریف نمایند.

---

# ۶. قرارداد نام‌گذاری مجوزها (Permission Naming Convention)

مجوزها باید از الگوی زیر پیروی کنند:

```
<Module>.<Operation>
```

نمونه‌ها:

```
Asset.Create
Asset.Update
Asset.Delete

Engine.Install
Engine.Remove

Maintenance.Create
Maintenance.Complete

Document.Upload
Document.Renew

Forecast.Generate
```

---

# ۷. مجوزهای دارایی (Asset Permissions)

نمونه‌ها:

- Asset.View
- Asset.Create
- Asset.Update
- Asset.Transfer
- Asset.Retire
- Asset.Dispose
- Asset.Export

---

# ۸. مجوزهای موتور (Engine Permissions)

نمونه‌ها:

- Engine.View
- Engine.Register
- Engine.Install
- Engine.Remove
- Engine.Replace
- Engine.Rebuild

---

# ۹. مجوزهای نگهداری و تعمیرات (Maintenance Permissions)

نمونه‌ها:

- Maintenance.Plan
- Maintenance.Schedule
- Maintenance.Start
- Maintenance.Complete
- Maintenance.Cancel
- Failure.Register
- Inspection.Register

---

# ۱۰. مجوزهای مالی (Financial Permissions)

نمونه‌ها:

- Financial.View
- Financial.RecordExpense
- Financial.CalculateDepreciation
- Financial.ViewOwnershipCost

مجوزهای مالی باید با دقت و احتیاط اعطا شوند.

---

# ۱۱. مجوزهای اسناد (Document Permissions)

نمونه‌ها:

- Document.View
- Document.Upload
- Document.Replace
- Document.Archive
- Document.Export

---

# ۱۲. مجوزهای پیش‌بینی (Forecast Permissions)

نمونه‌ها:

- Forecast.View
- Forecast.Generate
- Forecast.Compare

تولید پیش‌بینی ممکن است نیازمند سطوح دسترسی بالاتری باشد.

---

# ۱۳. مجوزهای گزارش‌گیری (Reporting Permissions)

نمونه‌ها:

- Report.View
- Report.Generate
- Report.Export

---

# ۱۴. مجوزهای مدیریتی (Administrative Permissions)

نمونه‌ها:

- User.Create
- User.Disable
- Role.Assign
- Permission.Assign
- Organization.Manage
- Configuration.Manage

مجوزهای مدیریتی باید محدود شوند.

---

# ۱۵. جریان اعطای مجوز (Authorization Flow)

جریان معمول اعطای مجوز:

```text
درخواست (Request)

↓

احراز هویت (Authentication)

↓

تشخیص و حل کاربر (Resolve User)

↓

تشخیص نقش‌ها (Resolve Roles)

↓

تشخیص مجوزها (Resolve Permissions)

↓

اعطای مجوز (Authorize)

↓

اجرای مدیریت‌کننده (Execute Handler)
```

اعطای مجوز باید قبل از اجرای منطق تجاری انجام شود.

---

# ۱۶. خطاهای اعطای مجوز (Authorization Failures)

هنگامی که اعطای مجوز با شکست مواجه می‌شود:

- وضعیت تجاری باید بدون تغییر باقی بماند؛
- هیچ رخداد دامنه‌ای (Domain Event) نباید منتشر شود؛
- این تلاش باید لاگ و ثبت شود.

---

# ۱۷. الزامات حسابرسی (Audit Requirements)

هر عملیات حساس به اعطای مجوز باید موارد زیر را ثبت و ضبط کند:

- کاربر (User)
- زمان (Time)
- عملیات (Operation)
- منبع (Resource)
- نتیجه (Result)
- مبدا (Source)

سوابق حسابرسی تغییرناپذیر (Immutable) هستند.

---

# ۱۸. قابلیت‌های آینده اعطای مجوز (Future Authorization Features)

نسخه‌های آینده ممکن است از موارد زیر پشتیبانی کنند:

- اعطای مجوز مبتنی بر منبع (Resource-based authorization)
- مجوزهای سطح سازمان (Organization-level permissions)
- مجوزهای موقت (Temporary permissions)
- تفویض اختیار (Delegation)
- گردش کارهای تاییدیه (Approval workflows)
- احراز هویت چندعاملی برای عملیات حیاتی (Multi-factor authorization for critical operations)

---

# ۱۹. حل و بررسی مجوز (Permission Resolution)

اعطای مجوز باید به ترتیب زیر انجام شود:

۱. احراز هویت کاربر (Authenticate User)
۲. تشخیص سازمان (Resolve Organization)
۳. تشخیص نقش‌ها (Resolve Roles)
۴. تشخیص مجوزها (Resolve Permissions)
۵. ارزیابی خط‌مشی (Evaluate Policy)
۶. اجرای مدیریت‌کننده (Execute Handler)

---

| مجوز (Permission) | فرمان (Command) |
|---|---|
| Asset.Create | RegisterAssetCommand |
| Engine.Install | InstallEngineCommand |
| Maintenance.Complete | CompleteMaintenanceCommand |

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط (Related Documents)

- 06-Workflows.md
- 04-Handlers.md
- 02-Commands.md
- 03-Queries.md
- docs/03-domain/08-BusinessRules.md
- ADR-0030-Identity and Access Management Architecture

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | مدل اولیه اعطای مجوز |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-02 | معمار راهکار | اصلاح ارجاع از ADR-0026 (حفاظت داده و رمزنگاری، نامرتبط) به ADR-0030 (معماری مدیریت هویت و دسترسی) |
| 4.2.0 | 2026-08-08 | معمار راهکار | افزودن مدیر سازمان به نقش‌های استاندارد — سند 01-vision/00-Vision.md آن را به عنوان یک کاربر هدف اصلی نام می‌برد، و این سند از قبل به مجوزهای در محدوده سازمان (Organization.Manage) بدون داشتن نقشی که آنها را داشته باشد ارجاع داده بود |
