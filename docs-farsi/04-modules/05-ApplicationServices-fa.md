| ویژگی | مقدار |
|---|---|
| **شناسه سند** | MOD-005 |
| **عنوان** | رفتارهای پایپ‌لاین و سرویس‌های کاربردی (Pipeline Behaviors) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند، مسئولیت‌های سرویس‌های کاربردی (Application Services) و رفتارهای خط لوله (Pipeline Behaviors) را تعریف می‌کند.

سرویس‌های کاربردی، گردش کارهای تجاری پیچیده‌ای را که در چندین مورداستفاده، تجمیع یا سیستم خارجی گسترش یافته‌اند، هماهنگ و هدایت می‌کنند.

سرویس‌های کاربردی اجزای هماهنگ‌سازی و ارکستراسیون هستند.

آن‌ها بخشی از لایه دامنه (Domain Layer) نیستند.

---

# فلسفه پایپ‌لاین (Pipeline Philosophy)

رفتارهای پایپ‌لاین (Pipeline Behaviors) دغدغه‌های عرضی فنی (Technical Cross-Cutting Concerns) را پیاده‌سازی می‌کنند.

آن‌ها هرگز حاوی منطق تجاری نیستند.

رفتار تجاری همواره در درون بخش‌های زیر باقی می‌ماند:

- تجمیع‌ها (Aggregates)
- سرویس‌های دامنه (Domain Services)

رفتارهای پایپ‌لاین اجزای زیرساختی با قابلیت استفاده مجدد هستند که قبل یا بعد از مدیریت‌کننده‌های درخواست (Request Handlers) اجرا می‌شوند.

---

# ۲. مسئولیت‌ها (Responsibilities)

سرویس‌های کاربردی می‌توانند:

- چندین مدیریت‌کننده فرمان را هماهنگ کنند
- چندین مدیریت‌کننده کوئری را هماهنگ کنند
- سرویس‌های دامنه را فراخوانی کنند
- چندین تجمیع را هماهنگ کنند
- سرویس‌های زیرساخت را هماهنگ کنند
- گردش کارهای طولانی‌مدت را اجرا نمایند
- رخدادهای یکپارچه‌سازی (Integration Events) را منتشر کنند

سرویس‌های کاربردی هرگز نباید حاوی قواعد تجاری باشند.

---

# ۳. اصول طراحی (Design Principles)

هر سرویس کاربردی باید اصول زیر را برآورده سازد:

- بدون وضعیت (Stateless)
- مستقل از فناوری (Technology independent)
- صرفاً هماهنگ‌سازی (Orchestration only)
- آگاه از تراکنش (Transaction aware)
- مبتنی بر کسب‌وکار (Business oriented)
- قابل آزمون به صورت مستقل (Independently testable)

---

# ۴. چه زمانی یک سرویس کاربردی مورد نیاز است (When an Application Service is Required)

سرویس‌های کاربردی زمانی باید معرفی شوند که:

- چندین تجمیع مشارکت داشته باشند؛
- چند فرمان باید به همراه یکدیگر اجرا شوند؛
- تعاملات با زیرساخت مورد نیاز باشد؛
- یک گردش کار تجاری در چندین ماژول گسترده شده باشد؛
- یک فرایند تجاری طولانی‌مدت وجود داشته باشد.

عملیات ساده باید مستقیماً از طریق یک مدیریت‌کننده فرمان (Command Handler) اجرا شوند.

---

# ۵. ارتباط با مدیریت‌کننده‌های فرمان (Relationship with Command Handlers)

```text
کنترلر (Controller)

↓

فرمان (Command)

↓

مدیریت‌کننده فرمان (Command Handler)

↓

سرویس کاربردی (Application Service - اختیاری)

↓

دامنه (Domain)

↓

زیرساخت (Infrastructure)
```

سرویس‌های کاربردی هرگز جایگزین مدیریت‌کننده‌های فرمان نمی‌شوند.

آن‌ها آن‌ها را هماهنگ می‌کنند.

---

# ترتیب اجرا (Execution Order)

```text
درخواست (Request)

↓

لاگ‌گیری (Logging)

↓

اعتبارسنجی (Validation)

↓

اعطای مجوز (Authorization)

↓

کارایی و عملکرد (Performance)

↓

تراکنش (Transaction)

↓

مدیریت‌کننده (Handler)

↓

کامیت (Commit)

↓

پاسخ (Response)
```

---

# قواعد طراحی رفتار (Behavior Design Rules)

هر رفتار پایپ‌لاین باید:

- قابل استفاده مجدد باشد.
- بدون وضعیت (Stateless) باشد.
- هرگز به رابط کاربری (UI) دسترسی نداشته باشد.
- هرگز مستقیماً به پیاده‌سازی‌های زیرساخت دسترسی پیدا نکند.
- هرگز حاوی قواعد تجاری نباشد.

---

# ۶. ارتباط با سرویس‌های دامنه (Relationship with Domain Services)

سرویس‌های کاربردی به این سوال پاسخ می‌دهند:

«چه اتفاقی باید بیفتد؟» ("?What should happen")

سرویس‌های دامنه به این سوال پاسخ می‌دهند:

«قواعد تجاری چگونه باید اعمال شوند؟» ("?How should business rules be applied")

سرویس‌های کاربردی ممکن است یک یا چند سرویس دامنه را فراخوانی کنند.

---

# ۷. سرویس‌های کاربردی متداول (Typical Application Services)

## AssetApplicationService

گردش کارهای مرتبط با دارایی را هماهنگ می‌کند.

نمونه‌ها:

- خرید دارایی کارکرده (Purchase Used Asset)
- اسقاط و واگذاری دارایی (Dispose Asset)
- انتقال دارایی (Transfer Asset)

---

## EngineApplicationService

چرخه حیات موتور را هماهنگ می‌کند.

نمونه‌ها:

- نصب موتور (Install Engine)
- تعویض موتور (Replace Engine)
- بازگشت موتور از تعمیرگاه (Return Engine from Workshop)

---

## MeterApplicationService

چرخه حیات کنتور/کارکردسنج را هماهنگ می‌کند.

نمونه‌ها:

- تعویض کنتور (Replace Meter)
- اعتبارسنجی قرائت‌های کنتور (Validate Meter Readings)
- محاسبه مجدد کارکرد عملیاتی (Recalculate Operational Usage)

---

## MaintenanceApplicationService

گردش کارهای نگهداری و تعمیرات را هماهنگ می‌کند.

نمونه‌ها:

- تکمیل نگهداری و تعمیرات (Complete Maintenance)
- تعویض قطعه در حین نگهداری و تعمیرات (Replace Component during Maintenance)
- ثبت اورهال و تعمیر اساسی (Register Overhaul)

---

## FinancialApplicationService

محاسبات مالی را هماهنگ می‌کند.

نمونه‌ها:

- محاسبه هزینه مالکیت (Calculate Ownership Cost)
- به‌روزرسانی ارزش دارایی (Update Asset Value)
- محاسبه استهلاک (Calculate Depreciation)

---

## ForecastApplicationService

گردش کارهای پیش‌بینی را هماهنگ می‌کند.

نمونه‌ها:

- تولید پیش‌بینی مصرف (Generate Consumption Forecast)
- تولید پیش‌بینی نگهداری و تعمیرات (Generate Maintenance Forecast)
- مقایسه دقت پیش‌بینی (Compare Forecast Accuracy)

---

## DocumentApplicationService

چرخه حیات اسناد را هماهنگ می‌کند.

نمونه‌ها:

- ثبت سند (Register Document)
- تمدید سند (Renew Document)
- تولید اعلان‌های انقضا (Generate Expiration Notifications)

---

# ۸. مدیریت تراکنش (Transaction Management)

سرویس‌های کاربردی می‌توانند موارد زیر را اجرا کنند:

- یک تراکنش واحد؛
- چندین تراکنش هماهنگ‌شده؛
- تراکنش‌های جبرانی (Compensating Transactions) در صورت نیاز.

مالکیت تراکنش منحصراً در درون لایه کاربرد باقی می‌ماند.

---

# ۹. تعامل با زیرساخت (Infrastructure Interaction)

سرویس‌های کاربردی ممکن است موارد زیر را فراخوانی کنند:

- سرویس اعلان (Notification Service)
- فضای ذخیره‌سازی فایل (File Storage)
- سرویس ایمیل (Email Service)
- سرویس پیامک (SMS Service)
- تولیدکننده گزارش (Report Generator)
- موتور پیش‌بینی هوش مصنوعی (AI Prediction Engine)
- سامانه ERP خارجی (External ERP)
- سامانه‌های حسابداری خارجی (External Accounting Systems)

تمام ارتباطات خارجی باید از طریق اینترفیس‌ها (Interfaces) انجام شود.

---

# ۱۰. انتشار رخدادها (Event Publishing)

سرویس‌های کاربردی ممکن است موارد زیر را منتشر کنند:

- رخدادهای یکپارچه‌سازی (Integration Events)
- رخدادهای اعلان (Notification Events)
- درخواست‌های پردازش پس‌زمینه (Background Processing Requests)

رخدادهای دامنه تجاری (Business Domain Events) همچنان بر عهده لایه دامنه است.

---

# ۱۱. قرارداد نام‌گذاری (Naming Convention)

سرویس‌های کاربردی باید از الگوی زیر پیروی کنند:

```
<BusinessArea>ApplicationService
```

نمونه‌ها:

- AssetApplicationService
- EngineApplicationService
- ForecastApplicationService
- MaintenanceApplicationService

---

# ۱۲. سرویس‌های کاربردی آینده (Future Application Services)

نسخه‌های آینده ممکن است سرویس‌هایی را برای موارد زیر معرفی کنند:

- مدیریت انبار و موجودی کالا (Inventory Management)
- تدارکات و خرید (Procurement)
- زمان‌بندی ناوگان (Fleet Scheduling)
- عیب‌یابی با هوش مصنوعی (AI Diagnostics)
- همگام‌سازی اینترنت اشیاء (IoT Synchronization)
- همگام‌سازی آفلاین موبایل (Mobile Offline Synchronization)

هر سرویس کاربردی در آینده باید از قواعد تعریف‌شده در این سند پیروی کند.

---

# ۱۳. رفتارهای فرمان در برابر رفتارهای کوئری (Command vs Query Behaviors)

الزامی برای فرمان‌ها (Mandatory for Commands):

- اعتبارسنجی (Validation)
- لاگ‌گیری (Logging)
- اعطای مجوز (Authorization)
- تراکنش (Transaction)

اختیاری برای کوئری‌ها (Optional for Queries):

- لاگ‌گیری (Logging)
- اعطای مجوز (Authorization)
- کارایی و عملکرد (Performance)

هرگز برای کوئری‌ها (Never for Queries):

- تراکنش (Transaction)

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط (Related Documents)

- 02-Commands.md
- 03-Queries.md
- 04-Handlers.md
- ../06-decisions/ADR-0007-Use-FluentValidation.md
- ../06-decisions/ADR-0036-Validation-Pipeline-Architecture.md
- ADR-0011 — اتخاذ CQRS

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | رفتارهای اولیه پایپ‌لاین |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
