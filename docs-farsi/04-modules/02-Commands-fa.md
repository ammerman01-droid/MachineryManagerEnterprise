# دستورات (Commands)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | APP-002 |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف

این سند تمامی دستورات (Commands) مورد استفاده در لایه کاربرد را تعریف می‌کند.

دستورات نشان‌دهنده درخواست‌هایی برای تغییر وضعیت کسب‌وکار هستند.

یک دستور بیانگر قصد و نیت کاربر (User Intention) است.

یک دستور موفقیت اجرا را تضمین نمی‌کند؛ تنها اجرای موفقیت‌آمیز منجر به تولید رویدادهای دامنه (Domain Events) می‌شود.

---

# فلسفه دستورات

دستورات نیت‌های کسب‌وکار را نشان می‌دهند.

یک دستور درخواست انتقال وضعیت را مطرح می‌کند اما هرگز خودش منطق کسب‌وکار را اجرا نمی‌کند.

اعتبارسنجی کسب‌وکار متعلق به دامنه است.

اعتبارسنجی فرمت و ساختار درخواست متعلق به لایه کاربرد است.

---

# ۲. اصول دستورات

هر دستور باید اصول زیر را برآورده سازد:

- نشان‌دهنده یک نیت مشخص کسب‌وکار است.
- وضعیت کسب‌وکار را تغییر می‌دهد.
- تغییرناپذیر (Immutable) است.
- دارای یک پردازنده (Handler) مسئول است.
- دارای یک نتیجه کسب‌وکاری انتظاررفته است.
- شامل تنها داده‌های ورودی مورد نیاز است.

دستورات هرگز شامل منطق کسب‌وکار نیستند.

---

# ۳. دسته‌بندی دستورات

```text
دستورات (Commands)

├── دستورات دارایی (Asset Commands)
├── دستورات موتور (Engine Commands)
├── دستورات قطعات (Component Commands)
├── دستورات کارکردسنج (Meter Commands)
├── دستورات نگهداری و تعمیرات (Maintenance Commands)
├── دستورات مالی (Financial Commands)
├── دستورات اسناد (Document Commands)
├── دستورات پیش‌بینی (Forecast Commands)
├── دستورات مدیریت سیستم (Administration Commands)
└── دستورات پیکربندی (Configuration Commands)
```

---

# ۴. ساختار دستور

هر دستور باید شامل موارد زیر باشد:

- `CommandId` (شناسه دستور)
- `CommandType` (نوع دستور)
- `RequestedAt` (زمان درخواست)
- `RequestedBy` (درخواست‌کننده)
- `CorrelationId` (شناسه همبستگی - اختیاری)

فیلدهای اختصاصی کسب‌وکار توسط هر دستور به طور مجزا تعریف می‌شوند.

---

# ۵. دستورات دارایی (Asset Commands)

- **CMD-001 — RegisterAsset**: ثبت دارایی فیزیکی جدید
- **CMD-002 — UpdateAssetInformation**: به‌روزرسانی اطلاعات قابل ویرایش دارایی
- **CMD-003 — TransferAsset**: انتقال مالکیت یا مسئولیت عملیاتی دارایی
- **CMD-004 — RetireAsset**: از رده خارج کردن دارایی
- **CMD-005 — DisposeAsset**: علامت‌گذاری دارایی به عنوان اسقاط / واگذارشده دائمی

---

# ۶. دستورات موتور (Engine Commands)

- **CMD-101 — RegisterEngine**: ثبت موتور جدید
- **CMD-102 — InstallEngine**: نصب موتور روی دارایی
- **CMD-103 — RemoveEngine**: جداسازی موتور از دارایی
- **CMD-104 — ReplaceEngine**: تعویض موتور
- **CMD-105 — SendEngineToWorkshop**: ارسال موتور به تعمیرگاه
- **CMD-106 — ReturnEngineFromWorkshop**: بازگشت موتور از تعمیرگاه
- **CMD-107 — RegisterEngineRebuild**: ثبت بازسازی / اورهال موتور

---

# ۷. دستورات قطعات (Component Commands)

- **CMD-201 — RegisterComponent**: ثبت قطعه جدید
- **CMD-202 — InstallComponent**: نصب قطعه
- **CMD-203 — RemoveComponent**: جداسازی قطعه
- **CMD-204 — ReplaceComponent**: تعویض قطعه
- **CMD-205 — RetireComponent**: از رده خارج کردن قطعه

---

# ۸. دستورات کارکردسنج (Meter Commands)

- **CMD-301 — InstallMeter**: نصب کنتور / کارکردسنج
- **CMD-302 — ReplaceMeter**: تعویض کنتور
- **CMD-303 — RegisterMeterReading**: ثبت قرائت کارکردسنج
- **CMD-304 — RegisterNonOperationalUsage**: ثبت کارکرد غیرعملیاتی
- **CMD-305 — CorrectMeterReading**: اصلاح قرائت کارکردسنج
- **CMD-306 — ArchiveMeter**: آرشیو کردن کارکردسنج

---

# ۹. دستورات نگهداری و تعمیرات (Maintenance Commands)

- **CMD-401 — CreateMaintenancePlan**: ایجاد برنامه نت
- **CMD-402 — ScheduleMaintenance**: زمان‌بندی نت
- **CMD-403 — StartMaintenance**: شروع عملیات نت
- **CMD-404 — CompleteMaintenance**: تکمیل عملیات نت
- **CMD-405 — CancelMaintenance**: لغو عملیات نت
- **CMD-406 — RegisterInspection**: ثبت بازرسی
- **CMD-407 — RegisterFailure**: ثبت خرابی / پیشامد
- **CMD-408 — RegisterRepair**: ثبت تعمیرات
- **CMD-409 — RegisterOverhaul**: ثبت اورهال
- **CMD-410 — ReplaceMaintenanceComponent**: تعویض قطعه در حین نت

---

# ۱۰. دستورات مالی (Financial Commands)

- **CMD-501 — RegisterAssetPurchase**: ثبت خرید دارایی
- **CMD-502 — RegisterOperatingExpense**: ثبت هزینه‌های عملیاتی
- **CMD-503 — RegisterFuelExpense**: ثبت هزینه سوخت
- **CMD-504 — RegisterMaintenanceExpense**: ثبت هزینه نت
- **CMD-505 — RegisterInsuranceExpense**: ثبت هزینه بیمه
- **CMD-506 — RegisterTaxExpense**: ثبت هزینه مالیات و عوارض
- **CMD-507 — CalculateDepreciation**: محاسبه استهلاک
- **CMD-508 — RecalculateAssetValue**: محاسبه مجدد ارزش دارایی
- **CMD-509 — RecalculateOwnershipCost**: محاسبه مجدد هزینه کل مالکیت (TCO)

---

# ۱۱. دستورات اسناد (Document Commands)

- **CMD-601 — RegisterDocument**: ثبت سند
- **CMD-602 — UploadDocumentImage**: بارگذاری تصویر سند
- **CMD-603 — UploadDocumentFile**: بارگذاری فایل سند
- **CMD-604 — ReplaceDocumentVersion**: جایگزینی نسخه سند
- **CMD-605 — RenewDocument**: تمدید سند
- **CMD-606 — ArchiveDocument**: آرشیو سند
- **CMD-607 — DeleteTemporaryDocument**: حذف سند موقت (تنها اسناد موقت قابل حذف هستند؛ اسناد رسمی کسب‌وکار هرگز حذف نمی‌شوند).

---

# ۱۲. دستورات پیش‌بینی (Forecast Commands)

- **CMD-701 — GenerateFuelForecast**: تولید پیش‌بینی سوخت
- **CMD-702 — GenerateLubricantForecast**: تولید پیش‌بینی روغن و روانکارها
- **CMD-703 — GenerateCoolantForecast**: تولید پیش‌بینی مایع خنک‌کننده
- **CMD-704 — GenerateMaintenanceForecast**: تولید پیش‌بینی نت
- **CMD-705 — GenerateSparePartsForecast**: تولید پیش‌بینی قطعات یدکی
- **CMD-706 — GenerateReplacementForecast**: تولید پیش‌بینی تعویض
- **CMD-707 — RefreshForecastModels**: به‌روزرسانی مدل‌های پیش‌بینی

---

# ۱۳. دستورات مدیریت سیستم (Administration Commands)

- **CMD-801 — CreateUser**: ایجاد کاربر
- **CMD-802 — DeactivateUser**: غیرفعال‌سازی کاربر
- **CMD-803 — AssignRole**: تخصیص نقش
- **CMD-804 — ChangePermissions**: تغییر دسترسی‌ها
- **CMD-805 — CreateOrganization**: ایجاد سازمان
- **CMD-806 — RegisterLocation**: ثبت موقعیت / سایت

---

# ۱۴. دستورات پیکربندی (Configuration Commands)

- **CMD-901 — RegisterAssetModel**: ثبت مدل دارایی
- **CMD-902 — RegisterEngineModel**: ثبت مدل موتور
- **CMD-903 — RegisterComponentModel**: ثبت مدل قطعه
- **CMD-904 — RegisterManufacturer**: ثبت سازنده
- **CMD-905 — RegisterSupplier**: ثبت تامین‌کننده
- **CMD-906 — RegisterMaintenanceTemplate**: ثبت الگوی نت
- **CMD-907 — RegisterDocumentType**: ثبت نوع سند
- **CMD-908 — UpdateForecastParameters**: به‌روزرسانی پارامترهای پیش‌بینی

---

# ۱۵. دستورات ترکیبی و میان‌ماژولی (Cross-Module Commands)

- **CMD-1001 — PurchaseUsedAsset**: خرید دارایی دست‌دوم (شامل ماژول‌های دارایی، موتور، کنتور، مالی)
- **CMD-1002 — ReplaceEngineAndContinueOperation**: تعویض موتور و ادامه عملیات (شامل دارایی، موتور، نت، مالی)
- **CMD-1003 — ReplaceHourMeter**: تعویض ساعت‌سنج (شامل دارایی، کنتور، گزارش‌گیری)
- **CMD-1004 — CompletePreventiveMaintenance**: تکمیل نت پیشگیرانه (شامل نت، مالی، پیش‌بینی)
- **CMD-1005 — DisposeAssetWithDocuments**: اسقاط دارایی همراه با اسناد (شامل دارایی، اسناد، مالی، گزارش‌گیری)

---

# ۱۶. اعتبارسنجی دستورات (Command Validation)

هر دستور قبل از اجرا باید مراحل زیر را طی کند:

1. احراز دسترسی و صلاحیت (Authorization)
2. اعتبارسنجی ورودی‌ها (Input Validation)
3. پیش‌شرط‌های کسب‌وکار (Business Preconditions)
4. دسترس‌پذیری مجموعه (Aggregate Availability)
5. اعتبارسنجی همزمانی (Concurrency Validation)

تنها دستورات معتبر به لایه دامنه راه می‌یابند.

---

# ۱۷. قوانین نام‌گذاری دستورات

هر دستور باید:

- با یک فعل شروع شود؛
- نیت کسب‌وکار را توصیف کند؛
- از اصطلاحات تخصصی کسب‌وکار استفاده کند؛
- از جزئیات فنی پیاده‌سازی اجتناب کند (مانند `SaveAsset` یا `ExecuteSQL` که نامناسب هستند).

---

# ۱۸. چرخه حیات اجرای دستور

```text
دستور (Command)
       │
       ▼
اعتبارسنجی (Validation)
       │
       ▼
احراز دسترسی (Authorization)
       │
       ▼
پردازنده (Handler)
       │
       ▼
مجموعه دامنه (Aggregate)
       │
       ▼
رویدادهای دامنه (Domain Events)
       │
       ▼
تایید تراکنش (Commit)
       │
       ▼
پاسخ (Response)
```

شکست در هر مرحله مانع از تغییر وضعیت می‌شود.

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
- `01-UseCases-fa.md`
- `03-Queries-fa.md`
- `04-Handlers-fa.md`
- `06-DomainEvents-fa.md`
- `ADR-0004 — Adopt CQRS`

---

# تاریخچه تغییرات

| نسخه | تاریخ | شرح |
|----------|------------|----------------------------------------------|
| 1.0.0 | اولیه | کاتالوگ اولیه دستورات |
| 3.0.0 | 2026-07-18 | استانداردسازی مطابق با استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
