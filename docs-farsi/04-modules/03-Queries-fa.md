# پرس‌وجوها (Queries)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | APP-003 |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف

این سند تمامی پرس‌وجوهای (Queries) مورد استفاده در پلتفرم MachineryManagerEnterprise را تعریف می‌کند.

پرس‌وجوها اطلاعات را از سیستم بازخوانی می‌کنند.

پرس‌وجوها هرگز وضعیت کسب‌وکار را تغییر نمی‌دهند و کاملاً فقط‌خواندنی (Read-Only) هستند.

---

# فلسفه پرس‌وجوها

پرس‌وجوها اطلاعات را بدون تغییر وضعیت کسب‌وکار بازیابی می‌کنند.

پرس‌وجوها هرگز قوانین کسب‌وکار را اجرا نمی‌کنند.

پرس‌وجوها در صورت نیاز می‌توانند داده‌ها را از طریق مدل‌های خواندن (Read Models) از چندین ماژول ترکیب کنند.

---

# قوانین طراحی پرس‌وجو

هر پرس‌وجو باید:

- تنها داده بازگرداند؛
- هرگز وضعیت کسب‌وکار را تغییر ندهد؛
- دقیقاً یک پردازنده (Handler) داشته باشد؛
- هرگز رویداد دامنه (Domain Event) منتشر نکند؛
- برای کارایی و سرعت خواندن بهینه‌سازی شده باشد.

---

# ۲. اصول پرس‌وجوها

هر پرس‌وجو باید اصول زیر را برآورده سازد:

- فقط‌خواندنی (Read-only)
- بدون عوارض جانبی (Side-effect free)
- مستقل از فناوری (Technology independent)
- مبتنی بر کسب‌وکار (Business oriented)
- قابل اجرای مستقل (Independently executable)
- بهینه‌شده برای خواندن (Optimized for reading)

---

# ۳. دسته‌بندی پرس‌وجوها

```text
پرس‌وجوها (Queries)

├── پرس‌وجوهای دارایی (Asset Queries)
├── پرس‌وجوهای موتور (Engine Queries)
├── پرس‌وجوهای قطعات (Component Queries)
├── پرس‌وجوهای کارکردسنج (Meter Queries)
├── پرس‌وجوهای نگهداری و تعمیرات (Maintenance Queries)
├── پرس‌وجوهای مالی (Financial Queries)
├── پرس‌وجوهای اسناد (Document Queries)
├── پرس‌وجوهای پیش‌بینی (Forecast Queries)
├── پرس‌وجوهای گزارش‌گیری (Reporting Queries)
└── پرس‌وجوهای مدیریت سیستم (Administration Queries)
```

---

# ۴. ساختار پرس‌وجو

هر پرس‌وجو شامل موارد زیر است:

- `QueryId` (شناسه پرس‌وجو)
- `QueryType` (نوع پرس‌وجو)
- `RequestedAt` (زمان درخواست)
- `RequestedBy` (درخواست‌کننده)
- `Filters` (فیلترها)
- `Paging` (صفحه‌بندی - اختیاری)
- `Sorting` (مرتب‌سازی - اختیاری)

---

# ۵. پرس‌وجوهای دارایی (Asset Queries)

- **QRY-001 — GetAsset**: دریافت یک دارایی مشخص
- **QRY-002 — SearchAssets**: جستجوی دارایی‌ها بر اساس معیارهای فیلتر
- **QRY-003 — GetAssetHistory**: دریافت تاریخچه کامل چرخه حیات دارایی
- **QRY-004 — GetAssetCurrentConfiguration**: دریافت پیکربندی فعلی (موتور، کنتور و قطعات نصب‌شده)
- **QRY-005 — GetAssetTimeline**: دریافت خط زمانی رویدادهای کسب‌وکاری دارایی
- **QRY-006 — GetAssetDashboard**: دریافت خلاصه اطلاعات عملیاتی دارایی

---

# ۶. پرس‌وجوهای موتور (Engine Queries)

- **QRY-101 — GetEngine**: دریافت اطلاعات یک موتور
- **QRY-102 — SearchEngines**: جستجوی موتورها
- **QRY-103 — GetEngineInstallationHistory**: دریافت تاریخچه نصب‌های موتور
- **QRY-104 — GetCurrentInstalledEngine**: دریافت موتور فعلی نصب‌شده روی یک دارایی
- **QRY-105 — GetEngineRepairHistory**: دریافت تاریخچه تعمیرات موتور
- **QRY-106 — GetEngineUsageHistory**: دریافت تاریخچه کارکرد موتور

---

# ۷. پرس‌وجوهای قطعات (Component Queries)

- **QRY-201 — GetComponent**: دریافت اطلاعات یک قطعه
- **QRY-202 — SearchComponents**: جستجوی قطعات
- **QRY-203 — GetComponentHistory**: دریافت تاریخچه قطعه
- **QRY-204 — GetInstalledComponents**: دریافت قطعات نصب‌شده فعلی
- **QRY-205 — GetReplacementHistory**: دریافت تاریخچه تعویض قطعات

---

# ۸. پرس‌وجوهای کارکردسنج (Meter Queries)

- **QRY-301 — GetCurrentMeter**: دریافت کارکردسنج فعلی
- **QRY-302 — GetMeterHistory**: دریافت تاریخچه کارکردسنج
- **QRY-303 — GetMeterReadings**: دریافت سوابق قرائت کارکردسنج
- **QRY-304 — GetOperationalUsage**: دریافت میزان کارکرد عملیاتی
- **QRY-305 — GetNonOperationalUsage**: دریافت میزان کارکرد غیرعملیاتی
- **QRY-306 — GetUsageCorrections**: دریافت سوابق اصلاح کارکرد

---

# ۹. پرس‌وجوهای نگهداری و تعمیرات (Maintenance Queries)

- **QRY-401 — GetMaintenancePlan**: دریافت برنامه نت فعال دارایی
- **QRY-402 — GetScheduledMaintenance**: دریافت فعالیت‌های نت زمان‌بندی‌شده
- **QRY-403 — GetMaintenanceHistory**: دریافت تاریخچه کامل نت
- **QRY-404 — GetInspectionHistory**: دریافت سوابق بازرسی
- **QRY-405 — GetFailureHistory**: دریافت تاریخچه خرابی‌ها
- **QRY-406 — GetRepairHistory**: دریافت تاریخچه تعمیرات
- **QRY-407 — GetOverhaulHistory**: دریافت تاریخچه اورهال‌ها
- **QRY-408 — GetUpcomingMaintenance**: دریافت سرویس‌های نت آتی پیش‌رو

---

# ۱۰. پرس‌وجوهای مالی (Financial Queries)

- **QRY-501 — GetPurchaseInformation**: دریافت اطلاعات خرید
- **QRY-502 — GetOperatingExpenses**: دریافت هزینه‌های عملیاتی
- **QRY-503 — GetFuelConsumptionCost**: دریافت هزینه‌های سوخت
- **QRY-504 — GetMaintenanceCost**: دریافت هزینه‌های نت
- **QRY-505 — GetDepreciation**: دریافت محاسبات استهلاک
- **QRY-506 — GetCurrentAssetValue**: دریافت ارزش تخمینی فعلی دارایی
- **QRY-507 — GetOwnershipCost**: دریافت هزینه کل مالکیت (TCO)
- **QRY-508 — GetFinancialTimeline**: دریافت خط زمانی مالی دارایی

---

# ۱۱. پرس‌وجوهای اسناد (Document Queries)

- **QRY-601 — GetDocument**: دریافت یک سند
- **QRY-602 — GetDocuments**: دریافت تمامی اسناد مرتبط با یک دارایی
- **QRY-603 — GetExpiredDocuments**: دریافت اسناد منقضی‌شده
- **QRY-604 — GetDocumentsExpiringSoon**: دریافت اسنادی که به موعد انقضا نزدیک می‌شوند
- **QRY-605 — GetDocumentVersions**: دریافت تاریخچه نسخه‌های سند
- **QRY-606 — GetDocumentPackage**: دریافت بسته خروجی اسناد

---

# ۱۲. پرس‌وجوهای پیش‌بینی (Forecast Queries)

- **QRY-701 — GetFuelForecast**: دریافت پیش‌بینی مصرف سوخت
- **QRY-702 — GetLubricantForecast**: دریافت پیش‌بینی مصرف روغن
- **QRY-703 — GetMaintenanceForecast**: دریافت پیش‌بینی نت
- **QRY-704 — GetReplacementForecast**: دریافت پیش‌بینی تعویض
- **QRY-705 — CompareForecasts**: مقایسه پیش‌بینی‌های تاریخی با مقادیر واقعی
- **QRY-706 — GetForecastHistory**: دریافت پیش‌بینی‌های تولیدشده قبلی

---

# ۱۳. پرس‌وجوهای گزارش‌گیری (Reporting Queries)

- **QRY-801 — GetExecutiveDashboard**: دریافت اطلاعات داشبورد مدیریتی
- **QRY-802 — GetAssetDashboard**: دریافت داشبورد عملیاتی دارایی‌ها
- **QRY-803 — GetFleetStatistics**: دریافت آمار و شاخص‌های کل ناوگان
- **QRY-804 — GetOperationalKPIs**: دریافت شاخص‌های کلیدی عملکرد عملیاتی
- **QRY-805 — GetFinancialKPIs**: دریافت شاخص‌های کلیدی عملکرد مالی
- **QRY-806 — GetMaintenanceKPIs**: دریافت شاخص‌های کلیدی عملکرد نت
- **QRY-807 — GetForecastKPIs**: دریافت شاخص‌های دقت پیش‌بینی

---

# ۱۴. پرس‌وجوهای مدیریت سیستم (Administration Queries)

- **QRY-901 — GetUsers**: دریافت لیست کاربران
- **QRY-902 — GetRoles**: دریافت لیست نقش‌ها
- **QRY-903 — GetOrganizations**: دریافت لیست سازمان‌ها
- **QRY-904 — GetLocations**: دریافت لیست موقعیت‌ها / سایت‌ها
- **QRY-905 — GetAuditLog**: دریافت لاگ‌های حسابرسی
- **QRY-906 — GetSystemConfiguration**: دریافت تنظیمات و پیکربندی سیستم

---

# ۱۵. پرس‌وجوهای ترکیبی و میان‌ماژولی (Cross-Module Queries)

- **QRY-1001 — GetCompleteAssetProfile**: دریافت شناسنامه کامل دارایی (ترکیب دارایی، موتور، قطعات، نت، اسناد، مالی، پیش‌بینی)
- **QRY-1002 — GetOperationalSummary**: دریافت خلاصه وضعیت عملیاتی (ترکیب کارکرد، نت، مالی)
- **QRY-1003 — GetTechnicalSummary**: دریافت خلاصه اطلاعات فنی (ترکیب مدل دارایی، مدل موتور، کتابخانه فنی)
- **QRY-1004 — GetBusinessTimeline**: دریافت خط زمانی یکپارچه کسب‌وکار (ترکیب قرائت کارکرد، نت، تعمیرات، تراکنش‌های مالی، اسناد، تعویض موتور)

---

# ۱۶. اعتبارسنجی پرس‌وجوها

پرس‌وجوها موارد زیر را اعتبارسنجی می‌کنند:

- احراز دسترسی و صلاحیت (Authorization)
- محدوده درخواست‌شده (Requested scope)
- سازگاری فیلترها (Filter consistency)
- محدودیت‌های صفحه‌بندی (Paging limits)
- قوانین مرتب‌سازی (Sorting rules)

---

# ۱۷. قوانین نام‌گذاری پرس‌وجوها

هر پرس‌وجو باید:

- با کلمات **Get**، **Search**، یا **Compare** شروع شود؛
- نشان‌دهنده یک درخواست اطلاعات کسب‌وکاری باشد؛
- مستقل از فناوری پیاده‌سازی باقی بماند.

از اسامی تکنیکال مانند `ReadTable` یا `ExecuteSQL` اجتناب کنید.

---

# ۱۸. چرخه حیات اجرای پرس‌وجو

```text
پرس‌وجو (Query)
       │
       ▼
احراز دسترسی (Authorization)
       │
       ▼
اعتبارسنجی (Validation)
       │
       ▼
پردازنده پرس‌وجو (Query Handler)
       │
       ▼
مدل خواندن (Read Model)
       │
       ▼
پروجکشن / نمای اشتقاقی (Projection)
       │
       ▼
پاسخ (Response)
```

پرس‌وجوها هرگز وضعیت کسب‌وکار را تغییر نداده و رویدادی منتشر نمی‌کنند.

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

- `00-ApplicationArchitecture-fa.md`
- `02-Commands-fa.md`
- `04-Handlers-fa.md`
- `04-DomainModel-fa.md`
- `ADR-0004 — Adopt CQRS`

---

# تاریخچه تغییرات

| نسخه | تاریخ | شرح |
|----------|------------|----------------------------------------------|
| 1.0.0 | اولیه | کاتالوگ اولیه پرس‌وجوها |
| 3.0.0 | 2026-07-18 | استانداردسازی مطابق با استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
