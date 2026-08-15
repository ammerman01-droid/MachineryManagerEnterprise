| ویژگی | مقدار |
|---|---|
| **شناسه سند** | MOD-002 |
| **عنوان** | کاتالوگ فرمان‌ها (Command catalogue) |
| **نسخه** | 4.7.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند، تمامی فرمان‌های (Commands) مورد استفاده در لایه کاربرد (Application Layer) را تعریف می‌کند.

فرمان‌ها نمایانگر درخواست‌هایی برای تغییر وضعیت تجاری سیستم هستند.

یک فرمان بیانگر قصد و نیت کاربر (User Intention) است.

یک فرمان تضمین‌کننده موفقیت نیست.

تنها اجرای موفقیت‌آمیز، رخدادهای دامنه (Domain Events) را تولید می‌کند.

---

# فلسفه فرمان (Command Philosophy)

فرمان‌ها نمایانگر مقاصد تجاری هستند.

یک فرمان درخواست انتقال وضعیت را دارد اما هرگز خود منطق تجاری را انجام نمی‌دهد.

اعتبارسنجی تجاری متعلق به لایه دامنه (Domain) است.

اعتبارسنجی کاربردی متعلق به لایه کاربرد (Application Layer) است.

---

# ۲. اصول فرمان (Command Principles)

هر فرمان باید اصول زیر را برآورده سازد:

- نمایانگر یک قصد تجاری واحد است.
- وضعیت تجاری را تغییر می‌دهد.
- تغییرناپذیر (Immutable) است.
- دارای یک مدیریت‌کننده (Handler) مسئول است.
- دارای یک خروجی تجاری مورد انتظار است.
- تنها حاوی داده‌های ورودی مورد نیاز است.

فرمان‌ها هرگز حاوی منطق تجاری نیستند.

---

# ۳. دسته‌بندی‌های فرمان‌ها (Command Categories)

```text
فرمان‌ها (Commands)

├── فرمان‌های دارایی (Asset Commands)
├── فرمان‌های موتور (Engine Commands)
├── فرمان‌های قطعات و اجزا (Component Commands)
├── فرمان‌های کنتور و کارکردسنج (Meter Commands)
├── فرمان‌های نگهداری و تعمیرات (Maintenance Commands)
├── فرمان‌های مالی (Financial Commands)
├── فرمان‌های اسناد (Document Commands)
├── فرمان‌های پیش‌بینی (Forecast Commands)
├── فرمان‌های مدیریتی (Administration Commands)
└── فرمان‌های پیکربندی (Configuration Commands)
```

---

# ۴. ساختار فرمان (Command Structure)

هر فرمان باید شامل موارد زیر باشد:

- شناسه فرمان (CommandId)
- نوع فرمان (CommandType)
- زمان درخواست (RequestedAt)
- درخواست‌کننده (RequestedBy)
- شناسه همبستگی (CorrelationId - اختیاری)

فیلدهای اختصاصی تجاری توسط هر فرمان تعریف می‌شوند.

---

# ۵. فرمان‌های دارایی (Asset Commands)

## CMD-001

RegisterAsset

**هدف:** ثبت یک دارایی فیزیکی جدید.

---

## CMD-002

UpdateAssetInformation

**هدف:** به‌روزرسانی اطلاعات قابل ویرایش دارایی.

---

## CMD-003

TransferAsset

**هدف:** انتقال مالکیت یا مسئولیت عملیاتی دارایی.

---

## CMD-004

RetireAsset

**هدف:** بازنشستگی دارایی.

---

## CMD-005

DisposeAsset

**هدف:** علامت‌گذاری دارایی به عنوان اسقاط یا واگذارشده دائمی.

---

# ۶. فرمان‌های موتور (Engine Commands)

## CMD-101

RegisterEngine

---

## CMD-102

InstallEngine

---

## CMD-103

RemoveEngine

---

## CMD-104

ReplaceEngine

---

## CMD-105

SendEngineToWorkshop

---

## CMD-106

ReturnEngineFromWorkshop

---

## CMD-107

RegisterEngineRebuild

---

# ۷. فرمان‌های قطعات و اجزا (Component Commands)

## CMD-201

RegisterComponent

---

## CMD-202

InstallComponent

---

## CMD-203

RemoveComponent

---

## CMD-204

ReplaceComponent

---

## CMD-205

RetireComponent

---

# ۸. فرمان‌های کنتور/کارکردسنج (Meter Commands)

## CMD-301

InstallMeter

---

## CMD-302

ReplaceMeter

---

## CMD-303

RegisterMeterReading

---

## CMD-304

RegisterNonOperationalUsage

---

## CMD-305

CorrectMeterReading

---

## CMD-306

ArchiveMeter

---

# ۹. فرمان‌های نگهداری و تعمیرات (Maintenance Commands)

## CMD-400

RequestMaintenance

---

## CMD-401

CreateMaintenancePlan

---

## CMD-401a

ApproveMaintenancePlan

---

## CMD-402

ScheduleMaintenance

---

## CMD-403

StartMaintenance

---

## CMD-404

CompleteMaintenance

---

## CMD-404a

VerifyMaintenance

---

## CMD-404b

CloseMaintenance

---

## CMD-405

CancelMaintenance

---

## CMD-405a

SuspendMaintenance

---

## CMD-405b

ResumeMaintenance

---

## CMD-406

RegisterInspection

---

## CMD-407

RegisterFailure

---

## CMD-408

RegisterRepair

---

## CMD-409

RegisterOverhaul

---

## CMD-410

ReplaceMaintenanceComponent

---

# ۱۰. فرمان‌های مالی (Financial Commands)

## CMD-501

RegisterAssetPurchase

---

## CMD-502

RegisterOperatingExpense

---

## CMD-503

RegisterFuelExpense

---

## CMD-504

RegisterMaintenanceExpense

---

## CMD-505

RegisterInsuranceExpense

---

## CMD-506

RegisterTaxExpense

---

## CMD-507

CalculateDepreciation

---

## CMD-508

RecalculateAssetValue

---

## CMD-509

RecalculateOwnershipCost

---

# ۱۱. فرمان‌های اسناد (Document Commands)

## CMD-601

RegisterDocument

---

## CMD-602

UploadDocumentImage

---

## CMD-603

UploadDocumentFile

---

## CMD-604

ReplaceDocumentVersion

---

## CMD-605

RenewDocument

---

## CMD-606

ArchiveDocument

---

## CMD-607

DeleteTemporaryDocument

تنها اسناد موقت ممکن است حذف شوند.

اسناد تجاری هرگز نباید حذف گردند.

---

# ۱۲. فرمان‌های پیش‌بینی (Forecast Commands)

## CMD-701

GenerateFuelForecast

---

## CMD-702

GenerateLubricantForecast

---

## CMD-703

GenerateCoolantForecast

---

## CMD-704

GenerateMaintenanceForecast

---

## CMD-705

GenerateSparePartsForecast

---

## CMD-706

GenerateReplacementForecast

---

## CMD-707

RefreshForecastModels

---

## CMD-707a

ValidateForecast

---

## CMD-707b

ApproveForecast

---

## CMD-707c

ScheduleForecast

---

## CMD-707d

ConsumeForecast

---

## CMD-707e

CompleteForecast

---

## CMD-707f

CancelForecast

---

# ۱۳. فرمان‌های مدیریتی (Administration Commands)

## CMD-801

CreateUser

---

## CMD-802

DeactivateUser

---

## CMD-803

AssignRole

---

## CMD-804

ChangePermissions

---

## CMD-805

CreateOrganization

---

## CMD-806

RegisterLocation

---

# ۱۴. فرمان‌های پیکربندی (Configuration Commands)

## CMD-901

RegisterAssetModel

---

## CMD-902

RegisterEngineModel

---

## CMD-903

RegisterComponentModel

---

## CMD-904

RegisterManufacturer

---

## CMD-905

RegisterSupplier

---

## CMD-906

RegisterMaintenanceTemplate

---

## CMD-907

RegisterDocumentType

---

## CMD-908

UpdateForecastParameters

---

# 14a. فرمان‌های سازمان (Organization Commands)

رسمی‌شده بر اساس سند BR-017 (مشخصات تجاری — مدیریت سازمان).

## CMD-950

RegisterOrganization

---

## CMD-951

AssociateUserWithOrganization

---

# 14b. فرمان‌های اعلان‌ها (Notification Commands)

رسمی‌شده بر اساس سند BR-012 (مشخصات تجاری — مرکز اعلان‌ها).
ایجاد اعلان به خودی خود یک اثر جانبی درون‌سیستمی از رخدادهای تجاری سایر ماژول‌ها است، نه فرمانی که توسط کاربر فراخوانی شود.

## CMD-960

AcknowledgeNotification

---

## CMD-961

ArchiveNotification

---

## CMD-962

CancelNotification

---

## CMD-963

UpdateNotificationPreferences

---

# 14c. فرمان‌های پیام‌رسانی داخلی (Internal Messaging Commands)

رسمی‌شده بر اساس سند BR-013 (مشخصات تجاری — پیام‌رسانی داخلی).

## CMD-970

StartConversation

---

## CMD-971

AddConversationParticipant

---

## CMD-972

SendMessage

---

## CMD-973

AttachFileToMessage

---

## CMD-974

MarkMessageAsRead

---

## CMD-975

ArchiveMessage

---

## CMD-976

DeleteMessage

---

## CMD-977

CloseConversation

---

## CMD-978

ReopenConversation

---

# 14d. فرمان‌های دستیار هوش مصنوعی (AI Assistant Commands)

رسمی‌شده بر اساس سند BR-014 (مشخصات تجاری — دستیار هوش مصنوعی). هر فرمان زیر یک خروجی مشاوره‌ای (توصیه، خلاصه، پاسخ) تولید می‌کند و مطابق قواعد BR-AI-003 و BR-AI-006 هرگز وضعیت تجاری را در ماژول دیگری تغییر نمی‌دهد.

## CMD-980

AskBusinessQuestion

---

## CMD-981

RequestRecommendation

---

## CMD-982

RequestHistoricalSummary

---

## CMD-983

RequestKnowledgeDiscovery

---

## CMD-984

RequestRiskAssessment

---

# 14e. فرمان‌های مدیریت روابط (Relationship Management Commands)

رسمی‌شده بر اساس سند BR-015 (مشخصات تجاری — مدیریت روابط).

## CMD-990

CreateRelationship

---

## CMD-991

ActivateRelationship

---

## CMD-992

ModifyRelationship

---

## CMD-993

ExpireRelationship

---

# 14f. فرمان‌های همگام‌سازی فضای کاری توزیع‌شده (Distributed Workspace Synchronization Commands)

رسمی‌شده بر اساس سند BR-016 (مشخصات تجاری — همگام‌سازی فضای کاری توزیع‌شده). پردازش پکیج بر اساس قاعده پردازش پکیج در BR-016 اتمیک است — یا تمام تغییرات معتبر در دسترس قرار می‌گیرند یا هیچ‌کدام اعمال نمی‌شوند.

## CMD-1000

CreateSynchronizationPackage

---

## CMD-1001

TransferSynchronizationPackage

---

## CMD-1002

ValidateSynchronizationPackage

---

## CMD-1003

ApplySynchronizationPackage

---

## CMD-1004

RequestWorkingSet

---

## CMD-1005

ResolveSynchronizationConflict

---

# ۱۵. فرمان‌های بین‌ماژولی (Cross-Module Commands)

فرمان‌های زیر چندین ماژول تجاری را هماهنگ می‌کنند.

---

## CMD-1001

PurchaseUsedAsset

ماژول‌های درگیر:

- دارایی (Asset)
- موتور (Engine)
- کنتور (Meter)
- مالی (Financial)

---

## CMD-1002

ReplaceEngineAndContinueOperation

ماژول‌های درگیر:

- دارایی (Asset)
- موتور (Engine)
- نگهداری و تعمیرات (Maintenance)
- مالی (Financial)

---

## CMD-1003

ReplaceHourMeter

ماژول‌های درگیر:

- دارایی (Asset)
- کنتور (Meter)
- گزارش‌گیری (Reporting)

---

## CMD-1004

CompletePreventiveMaintenance

ماژول‌های درگیر:

- نگهداری و تعمیرات (Maintenance)
- مالی (Financial)
- پیش‌بینی (Forecast)

---

## CMD-1005

DisposeAssetWithDocuments

ماژول‌های درگیر:

- دارایی (Asset)
- اسناد (Documents)
- مالی (Financial)
- گزارش‌گیری (Reporting)

---

# ۱۶. اعتبارسنجی فرمان (Command Validation)

قبل از اجرا، هر فرمان باید مراحل زیر را با موفقیت سپری کند:

- اعطای مجوز و دسترسی (Authorization)
- اعتبارسنجی ورودی‌ها (Input Validation)
- پیش‌شرط‌های تجاری (Business Preconditions)
- در دسترس بودن تجمیع (Aggregate Availability)
- اعتبارسنجی همروندی (Concurrency Validation)

تنها فرمان‌های معتبر به لایه دامنه می‌رسند.

---

# ۱۷. قواعد نام‌گذاری فرمان (Command Naming Rules)

هر فرمان باید:

- با یک فعل شروع شود؛
- قصد تجاری را توصیف کند؛
- از اصطلاحات تجاری استفاده کند؛
- از جزئیات پیاده‌سازی فنی پرهیز نماید.

نمونه‌ها:

- RegisterAsset
- InstallEngine
- ReplaceMeter
- CompleteMaintenance

پرهیز از:

- SaveAsset
- UpdateDatabase
- ExecuteSQL
- CallAPI

---

# ۱۸. اجرای فرمان (Command Execution)

یک فرمان موفق معمولاً از چرخه حیات زیر پیروی می‌کند:

```text
فرمان (Command)

↓

اعتبارسنجی (Validation)

↓

اعطای مجوز (Authorization)

↓

مدیریت‌کننده (Handler)

↓

تجمیع (Aggregate)

↓

رخدادهای دامنه (Domain Events)

↓

کامیت (Commit)

↓

پاسخ (Response)
```

شکست در هر مرحله مانع از تغییر وضعیت می‌شود.

---

# ۱۹. فرمان‌های آینده (Future Commands)

نسخه‌های آینده ممکن است فرمان‌هایی برای موارد زیر معرفی کنند:

- موجودی و انبار (Inventory)
- تدارکات و خرید (Procurement)
- زمان‌بندی ناوگان (Fleet Scheduling)
- عیب‌یابی با هوش مصنوعی (AI Diagnostics)
- یکپارچه‌سازی اینترنت اشیاء (IoT Integration)
- همگام‌سازی آفلاین موبایل (Mobile Offline Synchronization)

هر فرمان در آینده باید از قراردادهای تعریف‌شده در این سند پیروی کند.

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط (Related Documents)

- 00-ApplicationArchitecture.md
- 01-UseCases.md
- 03-Queries.md
- 04-Handlers.md
- docs/03-domain/07-DomainEvents.md
- ADR-0011 — اتخاذ CQRS

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | کاتالوگ اولیه فرمان‌ها |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-02 | معمار راهکار | افزودن فرمان‌های نگهداری و تعمیرات و پیش‌بینی برای چرخه‌های حیات گسترش‌یافته ۹ وضعیتی و ۷ وضعیتی همگام با BR-011/BR-010 و 09-StateMachines.md |
| 4.2.0 | 2026-08-02 | معمار راهکار | افزودن بخش 14a فرمان‌های سازمان (CMD-950 و CMD-951)، رسمی‌شده از BR-017 |
| 4.3.0 | 2026-08-02 | معمار راهکار | افزودن بخش 14b فرمان‌های اعلان‌ها (CMD-960 تا CMD-963)، رسمی‌شده از BR-012 |
| 4.4.0 | 2026-08-02 | معمار راهکار | افزودن بخش 14c فرمان‌های پیام‌رسانی داخلی (CMD-970 تا CMD-978)، رسمی‌شده از BR-013 |
| 4.5.0 | 2026-08-02 | معمار راهکار | افزودن بخش 14d فرمان‌های دستیار هوش مصنوعی (CMD-980 تا CMD-984)، رسمی‌شده از BR-014 |
| 4.6.0 | 2026-08-02 | معمار راهکار | افزودن بخش 14e فرمان‌های مدیریت روابط (CMD-990 تا CMD-993)، رسمی‌شده از BR-015 |
| 4.7.0 | 2026-08-08 | معمار راهکار | افزودن بخش 14f فرمان‌های همگام‌سازی فضای کاری توزیع‌شده (CMD-1000 تا CMD-1005)، رسمی‌شده از BR-016. این کار تمام ۶ مجموعه فرمان‌های قبلاً مفقود ماژول‌ها را کامل می‌کند |
