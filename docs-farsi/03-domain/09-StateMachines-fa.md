| ویژگی | مقدار |
|---|---|
| **شناسه سند** | DOM-009 |
| **عنوان** | ماشین‌های وضعیت (State Machines) |
| **نسخه** | 4.5.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند، ماشین‌های وضعیت چرخه حیات موجودیت‌های اصلی تجاری را تعریف می‌نماید.

هر گذار و تغییر وضعیت در چرخه حیات باید قواعد تجاری (Business Rules) تعریف‌شده در این پروژه را برآورده سازد.

یک ماشین وضعیت موارد زیر را تعریف می‌کند:

- وضعیت‌های معتبر (Valid States)
- گذارهای مجاز (Allowed Transitions)
- گذارهای ممنوع (Forbidden Transitions)
- رخدادهای تجاری محرک (Triggering Business Events)

---

# فلسفه ماشین وضعیت (State Machine Philosophy)

ماشین‌های وضعیت چرخه حیات قانونی و معتبر موجودیت‌های تجاری را تعریف می‌کنند.

هر گذار نمایانگر یک تصمیم تجاری است و همواره باید قواعد تجاری، ناورداهای تجمیع و اصول دامنه را ارضا نماید.

گذارهای وضعیت هرگز خارج از مرز تجمیع رخ نمی‌دهند.

---

# ۲. اصول ماشین وضعیت (State Machine Principles)

هر ماشین وضعیت باید اصول زیر را برآورده سازد:

- وضعیت‌ها متناهی و محدود هستند.
- وضعیت‌ها مانعةالجمع هستند (Mutually exclusive).
- هر موجودیت دقیقاً دارای یک وضعیت جاری است.
- وضعیت‌های تاریخی حفظ می‌شوند.
- گذارهای غیرقانونی رد می‌شوند.
- گذارها رخدادهای دامنه تولید می‌کنند.

---

# قواعد مدل‌سازی چرخه حیات (Lifecycle Modeling Rules)

تمامی مدل‌های چرخه حیات باید واجد شرایط زیر باشند:

- دقیقاً یک وضعیت جاری
- گذارهای قانونی صریح
- گذارهای غیرقانونی صریح
- محرک‌های گذار
- انتشار رخداد دامنه
- پایستگی و سازگاری تجمیع

---

# ۳. چرخه حیات دارایی (Asset Lifecycle)

## وضعیت‌ها (States)

```text
Draft (پیش‌نویس)

↓

Registered (ثبت‌شده)

↓

Commissioned (راه‌اندازی‌شده)

↓

Operational (عملیاتی)

↓

Inactive (غیرفعال)

↓

Retired (بازنشسته)

↓

Disposed (اسقاط/واگذارشده)
```

---

## گذارهای مجاز (Allowed Transitions)

| از (From) | به (To) |
|---|---|
| Draft | Registered |
| Registered | Commissioned |
| Commissioned | Operational |
| Operational | Inactive |
| Inactive | Operational |
| Operational | Retired |
| Inactive | Retired |
| Retired | Disposed |

---

## گذارهای ممنوع (Forbidden Transitions)

نمونه‌ها:

- Draft → Operational
- Registered → Retired
- Disposed → Operational
- Retired → Commissioned

---

## رخدادهای محرک (Trigger Events)

- AssetRegistered
- AssetActivated
- AssetRetired
- AssetDisposed

---

# ۴. چرخه حیات موتور (Engine Lifecycle)

## وضعیت‌ها (States)

```text
Stored (در انبار)

↓

Installed (نصب‌شده)

↓

Removed (جداسازی‌شده)

↓

Under Repair (تحت تعمیر)

↓

Rebuilt (بازسازی‌شده/اورهال)

↓

Stored (در انبار)

↓

Installed (نصب‌شده)
```

وضعیت نهایی (Final State):

```text
Retired (بازنشسته)
```

---

## گذارهای مجاز (Allowed Transitions)

| از (From) | به (To) |
|---|---|
| Stored | Installed |
| Installed | Removed |
| Removed | Stored |
| Removed | Under Repair |
| Under Repair | Rebuilt |
| Rebuilt | Stored |
| Installed | Retired |
| Stored | Retired |

---

## محدودیت‌های تجاری (Business Constraints)

تنها یک دارایی (Asset) می‌تواند به موتوری در وضعیت `Installed` ارجاع دهد.

---

## رخدادهای محرک (Trigger Events)

- EngineInstalled
- EngineRemoved
- EngineRebuilt

---

# ۵. چرخه حیات دستگاه کنتور/شمارنده (Meter Device Lifecycle)

## وضعیت‌ها (States)

```text
Registered (ثبت‌شده)

↓

Installed (نصب‌شده)

↓

Operational (عملیاتی)

↓

Failed (خراب/معیوب)

↓

Removed (جداسازی‌شده)

↓

Archived (بایگانی‌شده)
```

---

## قواعد تجاری (Business Rules)

جداسازی یک دستگاه کنتور هرگز تاریخچه آن را حذف نمی‌کند.

تعویض یک دستگاه کنتور هرگز کارکرد عملیاتی (Operational Usage) را بازنشانی یا ریست نمی‌کند.

---

## رخدادهای محرک (Trigger Events)

- MeterInstalled
- MeterRemoved
- MeterFailureDetected

---

# ۶. چرخه حیات نگهداری و تعمیرات (Maintenance Lifecycle)

## وضعیت‌ها (States)

```text
Requested (درخواست‌شده)

↓

Planned (برنامه‌ریزی‌شده)

↓

Approved (تصویب‌شده)

↓

Scheduled (زمان‌بندی‌شده)

↓

Started (شروع‌شده)

↓

In Progress (در حال اجرا)

↓

Completed (تکمیل‌شده)

↓

Verified (راستی‌آزمایی‌شده/تأیید کیفی)

↓

Closed (بسته‌شده)
```

مسیرهای جایگزین:

```text
Requested / Planned / Approved / Scheduled

↓

Cancelled (لغوشده)
```

```text
Started / In Progress

↓

Suspended (معلق‌شده)

↓

In Progress (ازسرگرفته‌شده)
```

> **توجه:** سند BR-011 (مشخصات تجاری — عملیات نگهداری) مرجع معتبر و تفصیلی برای این چرخه حیات است.

---

## گذارهای مجاز (Allowed Transitions)

| از (From) | به (To) |
|---|---|
| Requested | Planned |
| Planned | Approved |
| Approved | Scheduled |
| Scheduled | Started |
| Started | In Progress |
| In Progress | Completed |
| Completed | Verified |
| Verified | Closed |
| Requested | Cancelled |
| Planned | Cancelled |
| Approved | Cancelled |
| Scheduled | Cancelled |
| Started | Suspended |
| In Progress | Suspended |
| Suspended | In Progress |

---

## موارد ممنوع (Forbidden)

- عملیات نگهداری تکمیل‌شده (`Completed`) هرگز نباید به وضعیت در حال اجرا (`In Progress`) بازگردد.
- وضعیت درخواست‌شده (`Requested`) هرگز نباید مستقیماً به تکمیل‌شده (`Completed`) گذار کند (مراحل الزامی نباید نادیده گرفته شوند مگر اینکه صراحتاً توسط پیکربندی تجاری مجاز شده باشد).
- عملیات نگهداری بسته‌شده (`Closed`) تغییرناپذیر هستند؛ تنها اصلاحات اداری ممکن باقی می‌ماند.

---

## رخدادهای محرک (Trigger Events)

- MaintenanceRequested
- MaintenancePlanned
- MaintenanceApproved
- MaintenanceScheduled
- MaintenanceStarted
- MaintenanceCompleted
- MaintenanceVerified
- MaintenanceClosed
- MaintenanceCancelled
- MaintenanceSuspended
- MaintenanceResumed

---

# ۷. چرخه حیات عیب و خرابی (Failure Lifecycle)

## وضعیت‌ها (States)

```text
Detected (شناسایی‌شده)

↓

Diagnosed (عیب‌یابی‌شده)

↓

Repair Planned (تعمیر برنامه‌ریزی‌شده)

↓

Repair In Progress (تعمیر در حال انجام)

↓

Resolved (رفع‌شده)

↓

Closed (بسته‌شده)
```

---

## رخدادهای محرک (Trigger Events)

- FailureDetected
- RepairStarted
- RepairCompleted

---

# ۷-الف. چرخه حیات رخداد و حادثه (Incident Lifecycle)

رسمی‌شده بر اساس BR-009 (مشخصات تجاری — مدیریت رخدادها) که این چرخه حیات را با جزئیات کامل تعریف می‌کند.

## وضعیت‌ها (States)

```text
Reported (گزارش‌شده)

↓

Validated (اعتبارسنجی‌شده)

↓

Classified (طبقه‌بندی‌شده)

↓

Assigned (تخصیص‌یافته)

↓

Under Investigation (تحت بررسی)

↓

Decision (تصمیم‌گیری)

↓

Resolved (رفع‌شده)

↓

Closed (بسته‌شده)
```

مسیرهای جایگزین:

```text
Reported / Validated

↓

Rejected (ردشده)
```

```text
Closed

↓

Reopened (بازگشایی مجدد)

↓

Under Investigation
```

---

## گذارهای مجاز (Allowed Transitions)

| از (From) | به (To) |
|---|---|
| Reported | Validated |
| Validated | Classified |
| Classified | Assigned |
| Assigned | Under Investigation |
| Under Investigation | Decision |
| Decision | Resolved |
| Resolved | Closed |
| Reported | Rejected |
| Validated | Rejected |
| Closed | Reopened |
| Reopened | Under Investigation |

---

## موارد ممنوع (Forbidden)

- نادیده گرفتن وضعیت‌های اجباری چرخه حیات مجاز نیست.
- رخدادهای بسته‌شده (`Closed`) هرگز نباید مستقیماً تغییر داده شوند؛ بازگشایی مجدد همواره یک گذار چرخه حیات جدید ایجاد می‌کند و تاریخچه قبلی بدون تغییر باقی می‌ماند.

---

## رخدادهای محرک (Trigger Events)

- IncidentReported
- IncidentValidated
- IncidentRejected
- IncidentClassified
- IncidentAssigned
- IncidentInvestigationStarted
- IncidentDecisionMade
- IncidentResolved
- IncidentClosed
- IncidentReopened

---

# ۸. چرخه حیات سند (Document Lifecycle)

## وضعیت‌ها (States)

```text
Draft (پیش‌نویس)

↓

Approved (تصویب‌شده)

↓

Active (فعال)

↓

Expiring (در شرف انقضا)

↓

Expired (منقضی‌شده)

↓

Replaced (جایگزین‌شده)

↓

Archived (بایگانی‌شده)
```

---

## قواعد تجاری (Business Rules)

اسناد منقضی‌شده بخشی از تاریخچه تجاری باقی می‌مانند.

اسناد بایگانی‌شده همچنان قابل دسترسی هستند.

---

## رخدادهای محرک (Trigger Events)

- DocumentRegistered
- DocumentExpired
- DocumentRenewed

---

# ۹. چرخه حیات پیش‌بینی (Forecast Lifecycle)

## وضعیت‌ها (States)

```text
Generated (تولیدشده)

↓

Validated (اعتبارسنجی‌شده)

↓

Approved (تصویب‌شده)

↓

Scheduled (زمان‌بندی‌شده)

↓

Consumed (مصرف‌شده)

↓

Completed (تکمیل‌شده)
```

مسیر جایگزین:

```text
Generated / Validated / Approved / Scheduled

↓

Cancelled (لغوشده)
```

> **توجه:** سند BR-010 (مشخصات تجاری — پیش‌بینی نگهداری) مرجع تفصیلی و معتبر برای این چرخه حیات است.

---

## گذارهای مجاز (Allowed Transitions)

| از (From) | به (To) |
|---|---|
| Generated | Validated |
| Validated | Approved |
| Approved | Scheduled |
| Scheduled | Consumed |
| Consumed | Completed |
| Generated | Cancelled |
| Validated | Cancelled |
| Approved | Cancelled |
| Scheduled | Cancelled |

---

## قواعد تجاری (Business Rules)

پیش‌بینی‌ها هرگز پیش‌بینی‌های قبلی را بازنویسی نمی‌کنند.

یک پیش‌بینی لغوشده یا تکمیل‌شده می‌تواند به عنوان یک شیء تجاری جدید مجدداً تولید شود؛ پیش‌بینی جدید قابلیت ردگیری به پیش‌بینی قبلی را در صورت کاربرد حفظ می‌کند.

---

## رخدادهای محرک (Trigger Events)

- ForecastGenerated
- ForecastValidated
- ForecastApproved
- ForecastScheduled
- ForecastConsumed
- ForecastCompleted
- ForecastCancelled

---

# ۱۰. چرخه حیات سوابق مالی (Financial Record Lifecycle)

## وضعیت‌ها (States)

```text
Draft (پیش‌نویس)

↓

Recorded (ثبت‌شده)

↓

Posted (قطعی‌شده/سندخورده)

↓

Closed (بسته‌شده)
```

مسیر اصلاح:

```text
Posted

↓

Adjustment Created (سند اصلاحی ایجادشده)
```

---

## قواعد تجاری (Business Rules)

تراکنش‌های قطعی‌شده (`Posted`) تغییرناپذیر هستند.

اصلاحات، تراکنش‌های جدیدی ایجاد می‌کنند.

---

# ۱۰-الف. چرخه حیات رابطه (Relationship Lifecycle)

رسمی‌شده بر اساس BR-015 (مشخصات تجاری — مدیریت روابط) که این چرخه حیات را به تفصیل تعریف می‌نماید.

## وضعیت‌ها (States)

```text
Draft (پیش‌نویس)

↓

Active (فعال)

↓

Modified (اصلاح‌شده)

↓

Expired (منقضی‌شده)

↓

Historical (تاریخی)
```

---

## گذارهای مجاز (Allowed Transitions)

| از (From) | به (To) |
|---|---|
| Draft | Active |
| Active | Modified |
| Modified | Active |
| Active | Expired |
| Expired | Historical |

---

## موارد ممنوع (Forbidden)

- روابط تاریخی (`Historical`) هرگز نباید به وضعیت فعال (`Active`) یا اصلاح‌شده (`Modified`) بازگردند — آن‌ها تغییرناپذیر هستند.
- روابط پیش‌نویس (`Draft`) در انتشار عملیاتی، اعطای مجوز یا ناوبری مشارکتی ندارند؛ تنها روابط فعال (`Active`) در این فرایندها شرکت می‌کنند.

---

## رخدادهای محرک (Trigger Events)

- RelationshipCreated
- RelationshipActivated
- RelationshipModified
- RelationshipExpired
- RelationshipArchived

---

# ۱۱. قواعد عمومی ماشین‌های وضعیت (Generic State Machine Rules)

قواعد زیر برای تمامی چرخه‌های حیات اعمال می‌گردند:

## SM-001

هر موجودیت دقیقاً دارای یک وضعیت جاری است.

---

## SM-002

هر گذار وضعیت باید دارای برچسب زمانی (Timestamp) باشد.

---

## SM-003

هر گذار وضعیت باید تاریخچه را حفظ نماید.

---

## SM-004

گذارهای غیرقانونی باید رد شوند.

---

## SM-005

گذارهای موفق باید رخدادهای دامنه را منتشر سازند.

---

## SM-006

گذارهای وضعیت هرگز نباید ناورداهای تجمیع را دور بزنند.

---

# ۱۲. ماشین‌های وضعیت آینده (Future State Machines)

نسخه‌های آتی ممکن است ماشین‌های وضعیت تکمیلی زیر را معرفی نمایند:

- موجودی انبار (Inventory)
- تدارکات و خرید (Procurement)
- زمان‌بندی ناوگان (Fleet Scheduling)
- عیب‌یابی با هوش مصنوعی (AI Diagnostics)
- دستگاه‌های اینترنت اشیاء (IoT Devices)
- انطباق و الزامات قانونی (Compliance)

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط (Related Documents)

- 08-BusinessRules.md
- 07-DomainEvents.md
- 06-DomainServices.md
- 05-Aggregates.md
- 04-DomainModel.md
- 03-BoundedContexts.md

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | تعاریف اولیه ماشین وضعیت |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-02 | معمار راهکار | جایگزینی مدل ساده ۴ وضعیتی نگهداری با مدل رسمی ۹ وضعیتی (به همراه شاخه‌های لغو/تعلیق) از سند BR-011 و گسترش رخدادهای محرک بر همین اساس |
| 4.2.0 | 2026-08-02 | معمار راهکار | جایگزینی مدل ساده ۴ وضعیتی پیش‌بینی با مدل رسمی ۷ وضعیتی (به همراه شاخه لغو) از سند BR-010 و گسترش رخدادهای محرک |
| 4.3.0 | 2026-08-02 | معمار راهکار | تصحیح شناسه سند از DOM-008 به DOM-009 |
| 4.4.0 | 2026-08-02 | معمار راهکار | افزودن بخش ۷-الف چرخه حیات رخدادها، بر اساس مدل ۸ وضعیتی سند BR-009 |
| 4.5.0 | 2026-08-08 | معمار راهکار | افزودن بخش ۱۰-الف چرخه حیات رابطه، بر اساس مدل ۵ وضعیتی سند BR-015 |
