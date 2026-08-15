| ویژگی | مقدار |
|---|---|
| **شناسه سند** | DOC-DEV-014 |
| **عنوان** | ماتریس وابستگی تجمیع‌ها (Aggregate Dependency Matrix) |
| **نسخه** | 4.2.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-20 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند، روابط وابستگی میان تجمیع‌ها (Aggregates) را در راهکار MachineryManagerEnterprise تعریف می‌کند.

هدف عبارت است از:

- حفظ استقلال تجمیع‌ها؛
- تعریف تعامل تجمیع‌ها؛
- شناسایی مالکیت تجمیع‌ها؛
- پشتیبانی از توالی پیاده‌سازی؛
- پشتیبانی از طراحی دامنه‌محور (Domain Driven Design - DDD).

این سند تبدیل به مرجع معتبر برای تعاملات تجمیع‌ها می‌شود.

---

# ۲. محدوده (Scope)

این سند موارد زیر را پوشش می‌دهد:

- وابستگی‌های تجمیع‌ها؛
- مالکیت تجمیع‌ها؛
- تعامل تجمیع‌ها؛
- وابستگی‌های چرخه حیات تجمیع‌ها.

این سند موارد زیر را تعریف نمی‌کند:

- قواعد تجاری (Business Rules)؛
- سرویس‌های کاربردی (Application Services)؛
- مراجع پروژه (Project References)؛
- زیرساخت (Infrastructure).

---

# ۳. فلسفه وابستگی تجمیع‌ها (Aggregate Dependency Philosophy)

تجمیع‌ها، مرزهای مستقل و خودمختار پایستگی و ثبات داده‌ها (Consistency boundaries) هستند.

یک تجمیع هرگز نباید از طریق مالکیت مستقیم به تجمیع دیگری وابسته باشد.

در عوض،

تجمیع‌ها از طریق موارد زیر با یکدیگر تعامل دارند:

- شناسه‌ها (Identifiers)؛
- رخدادهای دامنه (Domain Events)؛
- سرویس‌های دامنه (Domain Services)؛
- مخازن داده (Repositories)؛
- سرویس‌های کاربردی (Application Services).

خودمختاری و استقلال تجمیع‌ها همواره باید حفظ شود.

---

# استقلال تجمیع‌ها (Aggregate Independence)

صحیح:

```text
تجمیع A (Aggregate A)

↓

شناسه (Identifier)

↓

تجمیع B (Aggregate B)
```

نادرست:

```text
تجمیع A (Aggregate A)

↓

ارجاع مستقیم به شیء (Object Reference)

↓

تجمیع B (Aggregate B)
```

ارجاعات مستقیم میان تجمیع‌ها ممنوع است.

---

# مالکیت تجمیع‌ها (Aggregate Ownership)

هر تجمیع تنها مالک مرز پایستگی و ثبات اختصاصی خود است.

مالکیت هرگز از مرزهای تجمیع فراتر نمی‌رود.

نمونه:

```text
MaintenanceOperation

مالک است بر (owns)

Maintenance Tasks (وظایف نگهداری)
```

اما:

```text
MaintenanceOperation

مالک نیست بر (does NOT own)

Asset (دارایی)
```

دارایی به صورت مستقل تحت مالکیت خود باقی می‌ماند.

---

# اصول وابستگی (Dependency Principles)

وابستگی‌های تجمیع‌ها باید واجد شرایط زیر باشند:

- صریح و شفاف (explicit)؛
- جهت‌دار (directional)؛
- بدون چرخه (acyclic)؛
- ایمن از نظر مالکیت (ownership-safe)؛
- سازگار با رخدادها (event-friendly).

هر وابستگی باید دارای معنا و مفهوم تجاری باشد.

---

# پیامدهای تجاری (Business Outcomes)

فلسفه وابستگی تجمیع‌ها موارد زیر را تضمین می‌کند:

- جفت‌شدگی ضعیف (Loose coupling)؛
- خودمختاری تجمیع‌ها؛
- مدل دامنه مقیاس‌پذیر؛
- پیاده‌سازی قابل نگهداری؛
- یکپارچگی و سازگاری سازمانی.

---

# ۴. انواع وابستگی (Dependency Types)

## تعریف تجاری (Business Definition)

وابستگی‌های تجمیع‌ها به دلایل معماری گوناگونی وجود دارند.

همه وابستگی‌ها نشان‌دهنده مالکیت نیستند.

نوع وابستگی موارد زیر را تعیین می‌کند:

- مرز پایستگی و سازگاری (Consistency boundary)؛
- مدل تعامل؛
- استراتژی پیاده‌سازی؛
- رفتار پایداری و ذخیره‌سازی داده‌ها.

وابستگی‌ها همواره باید صریح و شفاف باشند.

---

# دسته‌بندی‌های وابستگی (Dependency Categories)

وابستگی‌های تجمیع‌ها به پنج دسته تقسیم می‌شوند:

---

## ۴.۱ وابستگی هویتی (Identity Dependency)

یک تجمیع تنها شناسه (Identifier) تجمیع دیگری را ذخیره می‌کند.

نمونه:

```text
MaintenanceOperation

↓

AssetId
```

ویژگی‌ها:

- بدون مالکیت
- جفت‌شدگی ضعیف
- وابستگی ارجح و توصیه‌شده

---

## ۴.۲ وابستگی ارجاعی (Reference Dependency)

یک تجمیع به اطلاعات تجاری تجمیع دیگری نیازمند است.

تجمیع هرگز مالک آن اطلاعات نمی‌شود.

نمونه:

```text
Incident

↓

Asset
```

رخداد (Incident) به اطلاعات دارایی (Asset) نیاز دارد.

رخداد هرگز مالک دارایی نیست.

ویژگی‌ها:

- فقط‌خواندنی (read-only)
- بدون مالکیت (non-owning)
- زمینه‌ای (contextual)

---

## ۴.۳ وابستگی رخداد دامنه (Domain Event Dependency)

تجمیع‌ها از طریق رخدادهای دامنه با یکدیگر ارتباط برقرار می‌کنند.

نمونه:

```text
ForecastCreated

↓

MaintenanceOperation
```

ویژگی‌ها:

- ناهمگام (asynchronous)
- جفت‌شدگی ضعیف (loosely coupled)
- رخدادمحور (event-driven)

---

## ۴.۴ وابستگی سرویسی (Service Dependency)

دو تجمیع از طریق یک سرویس دامنه با یکدیگر همکاری می‌کنند.

نمونه:

```text
Forecast

↓

ForecastCalculationService

↓

ConditionMonitoring
```

هیچ‌یک از تجمیع‌ها مالک دیگری نیست.

ویژگی‌ها:

- هماهنگ‌سازی (coordination)
- بدون وضعیت (stateless)
- با قابلیت استفاده مجدد (reusable)

---

## ۴.۵ وابستگی تاریخی (Historical Dependency)

یک تجمیع نیازمند اطلاعات تاریخی تجمیع دیگری است.

نمونه:

```text
MaintenanceHistory

↓

MaintenanceOperation
```

ویژگی‌ها:

- تغییرناپذیر (immutable)
- قابل ردیابی (traceable)
- نسخه‌بندی‌شده (versioned)

---

# وابستگی مجاز (Allowed Dependency)

```text
تجمیع (Aggregate)

↓

شناسه (Identifier)

↓

تجمیع (Aggregate)
```

---

# وابستگی ممنوع (Prohibited Dependency)

```text
تجمیع (Aggregate)

↓

مالک است بر (owns)

↓

تجمیع (Aggregate)
```

مالکیت تجمیع فراتر از مرزها اکیداً ممنوع است.

---

# جهت وابستگی (Dependency Direction)

وابستگی‌ها همواره به سمت ارائه‌دهنده اطلاعات اشاره می‌کنند:

```text
مصرف‌کننده (Consumer)

↓

ارائه‌دهنده (Provider)
```

هرگز برعکس آن مجاز نیست.

---

# طول عمر وابستگی (Dependency Lifetime)

برخی وابستگی‌ها دائمی هستند.

نمونه:

```text
TrackedComponent

↓

Asset
```

---

سایر وابستگی‌ها تنها در طول اجرا وجود دارند.

نمونه:

```text
Notification

↓

Relationship Event
```

---

# قدرت وابستگی (Dependency Strength)

| قدرت | توصیف |
|---|---|
| الزامی (Required) | تجمیع بدون ارائه‌دهنده نمی‌تواند کار کند |
| زمینه‌ای (Context) | تجمیع اطلاعات ارائه‌دهنده را مصرف می‌کند |
| رخدادی (Event) | تجمیع به رخدادهای ارائه‌دهنده واکنش نشان می‌دهد |
| تاریخی (Historical) | تجمیع تاریخچه تغییرناپذیر را مصرف می‌کند |

---

# قواعد تجاری (Business Rules)

### AD-001

وابستگی‌های تجمیع‌ها هرگز نباید مالکیت را منتقل کنند.

---

### AD-002

وابستگی‌های تجمیع‌ها همواره باید جهت‌دار باشند.

---

### AD-003

وابستگی‌های تجمیع‌ها همواره باید صریح و شفاف باقی بمانند.

---

### AD-004

وابستگی هویتی (Identity Dependency) وابستگی ارجح و مطلوب است.

---

### AD-005

وابستگی‌های چرخه‌ای میان تجمیع‌ها ممنوع است.

---

## پیامدهای معماری (Architectural Outcomes)

انواع وابستگی موارد زیر را فراهم می‌آورند:

- جفت‌شدگی ضعیف؛
- خودمختاری تجمیع‌ها؛
- تعامل صریح و شفاف؛
- پایداری داده‌های مقیاس‌پذیر؛
- انطباق با اصول DDD.

---

# ۵. ماتریس وابستگی تجمیع‌ها (Aggregate Dependency Matrix)

## نمای کلی (Overview)

ماتریس زیر وابستگی‌های میان تجمیع‌ها را تعریف می‌کند.

این ماتریس صرفاً تعامل تجاری را شرح می‌دهد.

این ماتریس دلالت بر مالکیت شیء ندارد.

---

## جدول ماتریس وابستگی تجمیع‌ها (Aggregate Dependency Matrix)

| تجمیع (Aggregate) | وابسته است به (Depends On) | نوع وابستگی | قدرت |
|---|---|---|---|
| Asset | — | — | مستقل (Independent) |
| TrackedComponent | Asset | هویتی (Identity) | الزامی (Required) |
| Meter | Asset | هویتی (Identity) | الزامی (Required) |
| ConditionAssessment | Asset, Meter | ارجاعی (Reference) | الزامی (Required) |
| Part | — | — | مستقل (Independent) |
| Inventory | Part | هویتی (Identity) | الزامی (Required) |
| InventoryTransaction | Inventory, Part | هویتی (Identity) | الزامی (Required) |
| Incident | Asset | هویتی (Identity) | الزامی (Required) |
| MaintenanceForecast | Asset, TrackedComponent, Meter, ConditionAssessment, Incident, Part | ارجاعی (Reference) | الزامی (Required) |
| MaintenanceOperation | Asset, TrackedComponent, Part, Inventory, MaintenanceForecast | ارجاعی (Reference) | الزامی (Required) |
| Notification | Relationship | رخدادی (Event) | الزامی (Required) |
| Conversation | Relationship | زمینه‌ای (Context) | توصیه‌شده (Recommended) |
| Relationship | — | — | مستقل (Independent) |
| AIConversation | Asset, TrackedComponent, Meter, Part, Inventory, Incident, MaintenanceForecast, MaintenanceOperation, Notification, Conversation, Relationship | زمینه‌ای (Context) | الزامی (Required) |

---

## تجمیع‌های مستقل (Independent Aggregates)

تجمیع‌های زیر هیچ وابستگی تجاری ندارند:

- Asset
- Part
- Relationship

این تجمیع‌ها شالوده و بنیان سازمان را برقرار می‌سازند.

---

## نمونه‌های وابستگی هویتی (Identity Dependency Examples)

```text
TrackedComponent

↓

AssetId
```

مؤلفه ردیابی‌شده (TrackedComponent) تنها `AssetId` را ذخیره می‌کند.

هرگز مالک Asset نیست.

---

```text
Inventory

↓

PartId
```

موجودی (Inventory) هرگز مالک Part نیست.

---

## نمونه‌های وابستگی ارجاعی (Reference Dependency Examples)

```text
MaintenanceForecast

↓

Asset

↓

Meter

↓

ConditionAssessment

↓

Incident
```

پیش‌بینی نگهداری (Forecast) اطلاعات را مصرف می‌کند.

پیش‌بینی مالک هیچ‌یک از این تجمیع‌ها نیست.

---

## نمونه‌های وابستگی رخدادی (Event Dependency Examples)

```text
RelationshipUpdated

↓

Notification
```

رابطه منتشر می‌کند.

اعلان واکنش نشان می‌دهد.

---

## نمونه‌های وابستگی زمینه‌ای (Context Dependency Examples)

```text
AIConversation

↓

Conversation

↓

Notification

↓

Relationship
```

هوش مصنوعی زمینه سازمانی را مصرف می‌کند.

هوش مصنوعی مالک هیچ‌چیز نیست.

---

## استقلال تجمیع‌ها (Aggregate Independence)

هر تجمیع به صورت مستقل پایدار و ذخیره می‌شود.

هر تجمیع مالک موارد زیر است:

- وضعیت خود؛
- چرخه حیات خود؛
- ناورداها و شروط تغییرناپذیر خود (invariants).

هیچ تجمیعی تجمیع دیگر را مستقیماً تغییر نمی‌دهد.

---

## خلاصه وابستگی‌ها (Dependency Summary)

```text
Asset

↓

TrackedComponent

↓

Forecast

↓

MaintenanceOperation

↓

AIConversation
```

و

```text
Part

↓

Inventory

↓

MaintenanceOperation
```

و

```text
Relationship

↓

Notification

↓

Conversation

↓

AIConversation
```

این موارد نمایانگر سه مسیر اصلی وابستگی تجمیع‌ها هستند.

---

# ۶. قواعد تعامل تجمیع‌ها (Aggregate Interaction Rules)

## تعریف تجاری (Business Definition)

تجمیع‌ها از طریق سازوکارهای تعاملی کاملاً مشخص با یکدیگر همکاری می‌کنند.

تعامل باید موارد زیر را حفظ کند:

- خودمختاری تجمیع‌ها؛
- مرزهای پایستگی داده‌ها؛
- مالکیت تجاری؛
- یکپارچگی تراکنشی.

تجمیع‌ها هرگز نباید مستقیماً تجمیع دیگری را دستکاری کنند.

---

# اصول تعامل (Interaction Principles)

هر تعامل تجمیع باید واجد شرایط زیر باشد:

- قصد و نیت صریح؛
- مالکیت شفاف؛
- ایمنی تراکنشی؛
- پایستگی مرزبندی‌شده.

---

# تعاملات مجاز (Allowed Interaction)

تجمیع‌ها می‌توانند از طریق موارد زیر تعامل داشته باشند:

- شناسه تجمیع (Aggregate Identifier)
- رخداد دامنه (Domain Event)
- سرویس دامنه (Domain Service)
- سرویس کاربردی (Application Service)

این سازوکارها استقلال تجمیع‌ها را حفظ می‌کنند.

---

## شناسه تجمیع (Aggregate Identifier)

سازوکار تعاملی ارجح و مطلوب.

نمونه:

```text
MaintenanceOperation

↓

AssetId
```

تجمیع صرفاً شناسه را ذخیره می‌کند.

دارایی (Asset) در صورت نیاز به طور مستقل بارگذاری می‌شود.

---

## رخدادهای دامنه (Domain Events)

تجمیع‌ها از طریق رخدادهای دامنه به تجمیع‌های دیگر اطلاع‌رسانی می‌کنند.

نمونه:

```text
RelationshipUpdated

↓

NotificationCreated
```

تجمیع منتشرکننده هیچ اطلاعی از مشترکین و دریافت‌کنندگان ندارد.

---

## سرویس‌های دامنه (Domain Services)

سرویس‌های دامنه چندین تجمیع را هماهنگ می‌کنند.

نمونه:

```text
ForecastCalculationService

↓

Asset

↓

Meter

↓

ConditionAssessment
```

سرویس دامنه عملیات هماهنگ‌سازی را انجام می‌دهد.

هیچ تجمیعی مالک دیگری نیست.

---

## سرویس‌های کاربردی (Application Services)

سرویس‌های کاربردی سناریوهای کاربردی تجاری را ارکستریت و سازمان‌دهی می‌کنند.

نمونه:

```text
CreateMaintenanceOperation

↓

Forecast Repository

↓

Asset Repository

↓

Maintenance Repository
```

سرویس‌های کاربردی تجمیع‌ها را هماهنگ می‌سازند.

تجمیع‌ها خودمختار باقی می‌مانند.

---

# تعاملات ممنوع (Prohibited Interaction)

تجمیع‌ها هرگز نباید:

- ارجاع شیئی به تجمیع دیگری داشته باشند؛
- وضعیت تجمیع دیگری را تغییر دهند؛
- مخازن داده (Repositories) را دور بزنند؛
- سرویس‌های تجاری را دور بزنند.

نمونه نادرست:

```text
MaintenanceOperation

↓

Asset.Update()
```

---

صحیح:

```text
MaintenanceOperation

↓

AssetId

↓

Application Service

↓

Asset Repository
```

---

# قاعده مخازن داده (Repository Rule)

هر تجمیع مخزن داده (Repository) اختصاصی خود را دارد.

نمونه:

```text
AssetRepository

TrackedComponentRepository

ForecastRepository

MaintenanceRepository
```

مخازن داده هرگز نباید گراف‌هایی را که چندین تجمیع را در بر می‌گیرند بازگردانند.

---

# مرز تراکنش (Transaction Boundary)

هر تجمیع مرز تراکنش اختصاصی خود را تعریف می‌کند.

تراکنش‌هایی که چندین تجمیع را در بر می‌گیرند باید به صورت خارجی هماهنگ شوند.

هرگز به صورت داخلی هماهنگ نمی‌شوند.

---

# قاعده پایستگی و سازگاری (Consistency Rule)

تجمیع‌ها موارد زیر را تضمین می‌کنند:

- سازگاری و پایستگی آنی درون خود تجمیع؛
- سازگاری نهایی (Eventual consistency) در سراسر مرزهای تجمیع‌ها.

سازگاری آنی میان چندین تجمیع ممنوع است.

---

# تعامل هوش مصنوعی (AI Interaction)

دستیار هوش مصنوعی هرگز مستقیماً با تجمیع‌ها تعامل ندارد.

نمونه:

```text
AI

↓

Application Query

↓

Read Model

↓

Response
```

هوش مصنوعی پروجکشن‌ها و مدل‌های خواندنی را مصرف می‌کند.

این دستیار گراف‌های تجمیع را بارگذاری نمی‌کند.

---

# تعامل گزارش‌گیری (Reporting Interaction)

گزارش‌ها مدل‌های خواندنی (Read Models) را مصرف می‌کنند.

گزارش‌ها هرگز مستقیماً از گراف‌های تجمیع کوئری نمی‌گیرند.

این کار کارایی و عملکرد تجمیع‌ها را حفظ می‌کند.

---

# تعامل اعلان‌ها (Notification Interaction)

مرکز اعلان‌ها رخدادهای دامنه را مصرف می‌کند.

مرکز اعلان‌ها هرگز تجمیع مبدأ را اصلاح نمی‌کند.

---

# تعامل پیام‌رسانی داخلی (Internal Messaging Interaction)

پیام‌رسانی داخلی زمینه انتشاریافته را مصرف می‌کند.

مالکیت گفتگو به صورت محلی باقی می‌ماند.

---

# قواعد تجاری (Business Rules)

### AIR-001

تعامل میان تجمیع‌ها در تمامی موارد ممکن باید از شناسه‌های تجمیع استفاده کند.

---

### AIR-002

ارتباطات میان تجمیع‌ها باید از رخدادهای دامنه استفاده نماید.

---

### AIR-003

سرویس‌های کاربردی تجمیع‌ها را هماهنگ می‌کنند.

تجمیع‌ها هرگز یکدیگر را هماهنگ نمی‌کنند.

---

### AIR-004

مخازن داده باید به صورت اختصاصی برای هر تجمیع باقی بمانند.

---

### AIR-005

گراف‌های تجمیع هرگز نباید از مرزهای تجمیع عبور کنند.

---

### AIR-006

تراکنش‌هایی که چندین تجمیع را در بر می‌گیرند باید به صورت خارجی هماهنگ شوند.

---

### AIR-007

مدل‌های خواندنی (Read Models) باید برای گزارش‌گیری و هوش مصنوعی جایگزین گراف‌های تجمیع شوند.

---

## پیامدهای معماری (Architectural Outcomes)

قواعد تعامل تجمیع‌ها موارد زیر را فراهم می‌آورند:

- جفت‌شدگی ضعیف؛
- ایمنی تراکنشی؛
- پایداری داده‌های مقیاس‌پذیر؛
- ارکستراسیون قابل پیش‌بینی؛
- انطباق با اصول DDD؛
- آمادگی برای مایکروسرویس‌ها در آینده.

---

# ۷. گراف وابستگی تجمیع‌ها (Aggregate Dependency Graph)

## هدف (Purpose)

گراف وابستگی تجمیع‌ها، توپولوژی وابستگی‌های مدل دامنه را بصری‌سازی می‌نماید.

برخلاف گراف وابستگی قابلیت‌ها، این گراف بر تعاملات تجمیع‌ها تمرکز دارد.

این گراف موارد زیر را نشان می‌دهد:

- جهت وابستگی؛
- مرزهای مالکیت؛
- خودمختاری تجمیع‌ها؛
- توالی پیاده‌سازی.

---

# توپولوژی تجمیع‌های سازمانی (Enterprise Aggregate Topology)

```text
                    Asset
                      │
       ┌───────────────┼────────────────┐
       │               │                │
       ▼               ▼                ▼
TrackedComponent    Meter          Incident
       │               │                │
       └───────────────┼────────────────┘
                       ▼
             ConditionAssessment
                       │
                       ▼
             MaintenanceForecast
                       │
                       ▼
           MaintenanceOperation


Part
 │
 ▼
Inventory
 │
 ▼
MaintenanceOperation


Relationship
 │
 ├──────────────► Notification
 │
 └──────────────► Conversation
                     │
                     ▼
               AIConversation


AIConversation
 │
 ├── Asset
 ├── TrackedComponent
 ├── Meter
 ├── Part
 ├── Inventory
 ├── Incident
 ├── ConditionAssessment
 ├── MaintenanceForecast
 ├── MaintenanceOperation
 ├── Notification
 ├── Conversation
 └── Relationship
```

---

# تفسیر گراف (Interpretation)

این گراف باید به صورت زیر تفسیر شود:

```text
تجمیع (Aggregate)

↓

اطلاعات را مصرف می‌کند از (Consumes Information From)

↓

تجمیع (Aggregate)
```

هرگز نباید به صورت زیر تفسیر گردد:

```text
تجمیع (Aggregate)

↓

مالک است بر (Owns)

↓

تجمیع (Aggregate)
```

---

# تجمیع‌های بنیادی (Foundation Aggregates)

تجمیع‌های بنیادی دامنه را پایه‌گذاری می‌کنند:

- Asset
- Part
- Relationship

تمامی تجمیع‌های باقی‌مانده به طور مستقیم یا غیرمستقیم به یک یا چند مورد از آن‌ها وابسته هستند.

---

# تجمیع‌های عملیاتی (Operational Aggregates)

تجمیع‌های عملیاتی کسب‌وکار سازمان را اجرا می‌کنند:

- MaintenanceForecast
- MaintenanceOperation

این تجمیع‌ها چندین زمینه تجاری را مصرف می‌نمایند.

آن‌ها تنها مالک مرزهای پایستگی داده‌های اختصاصی خود هستند.

---

# تجمیع‌های سرویس سازمانی (Enterprise Service Aggregates)

تجمیع‌های ارتباطی تنها به زمینه سازمانی وابسته هستند:

```text
Relationship

↓

Notification

↓

Conversation
```

این تجمیع‌ها از منطق تجاری عملیاتی ایزوله باقی می‌مانند.

---

# تجمیع هوش مصنوعی (AI Aggregate)

تجمیع `AIConversation` دانش سازمانی را مصرف می‌کند:

```text
Read Models (مدل‌های خواندنی)

↓

AIConversation
```

تجمیع هوش مصنوعی هرگز نباید به یک مالک تجاری تبدیل شود.

این تجمیع هرگز نباید در تراکنش‌های عملیاتی مشارکت نماید.

---

# خوشه‌های تجمیع (Aggregate Clusters)

این گراف به طور طبیعی سه خوشه مستقل تشکیل می‌دهد:

## خوشه عملیاتی (Operational Cluster)

```text
Asset

↓

TrackedComponent

↓

Forecast

↓

Maintenance
```

---

## خوشه موجودی (Inventory Cluster)

```text
Part

↓

Inventory

↓

Maintenance
```

---

## خوشه همکاری (Collaboration Cluster)

```text
Relationship

↓

Notification

↓

Conversation

↓

AI
```

خوشه‌ها با جفت‌شدگی ضعیف در کنار هم قرار دارند.

---

# محدودیت‌های گراف (Graph Constraints)

گراف وابستگی تجمیع‌ها همواره باید شرایط زیر را برآورده سازد:

- یال‌های جهت‌دار؛
- عدم انتقال مالکیت؛
- عدم وجود وابستگی چرخه‌ای؛
- خودمختاری تجمیع‌ها؛
- وابستگی صریح و شفاف.

---

# پیامدهای تجاری (Business Outcomes)

گراف وابستگی تجمیع‌ها موارد زیر را فراهم می‌آورد:

- شفافیت دامنه؛
- درک ساختار تجمیع‌ها؛
- راهنمای پیاده‌سازی؛
- اعتبارسنجی معماری؛
- تکامل مقیاس‌پذیر دامنه.

---

# ۸. مالکیت تجمیع‌ها (Aggregate Ownership)

## هدف (Purpose)

مالکیت تجمیع‌ها مرز دقیق مسئولیت هر یک از تجمیع‌ها را تعریف می‌کند.

هر تجمیع تنها مالک مرز پایستگی و سازگاری داده‌های اختصاصی خود است.

مالکیت موارد زیر را تعیین می‌نماید:

- مسئولیت تراکنشی؛
- مرجعیت چرخه حیات؛
- اجرای ناورداها و شروط تغییرناپذیر؛
- مسئولیت پایداری و ذخیره‌سازی.

مالکیت همواره باید صریح و شفاف باقی بماند.

---

# اصل مالکیت (Ownership Principle)

یک تجمیع مالک موارد زیر است:

- وضعیت خود؛
- چرخه حیات خود؛
- ناورداها و شروط پایستگی خود؛
- موجودیت‌های داخلی خود؛
- اشیاء مقداری (Value Objects) خود.

یک تجمیع هرگز مالک تجمیع دیگری نیست.

---

# ماتریس مالکیت (Ownership Matrix)

| تجمیع | مالک است بر | هرگز مالک نیست بر |
|---|---|---|
| Asset | وضعیت دارایی، چرخه حیات دارایی | مؤلفه‌ها، قطعات، رخدادها |
| TrackedComponent | وضعیت مؤلفه | Asset |
| Meter | قرائت‌های کنتور | Asset |
| ConditionAssessment | نتایج ارزیابی | Meter, Asset |
| Part | تعریف قطعه | Inventory |
| Inventory | مقادیر موجودی | Part |
| InventoryTransaction | تاریخچه تراکنش‌ها | Inventory |
| Incident | چرخه حیات رخداد | Asset |
| MaintenanceForecast | سوابق پیش‌بینی | Asset, Component |
| MaintenanceOperation | اجرای نگهداری | Forecast, Asset, Inventory |
| Notification | چرخه حیات اعلان | Relationship |
| Conversation | پیام‌ها | Relationship |
| Relationship | گراف روابط | موجودیت‌های تجاری |
| AIConversation | تاریخچه گفتگوهای هوش مصنوعی | موجودیت‌های تجاری |

---

# نمونه‌های مالکیت (Ownership Examples)

## دارایی (Asset)

دارایی مالک موارد زیر است:

- ویژگی‌های دارایی
- چرخه حیات دارایی
- قواعد تجاری دارایی

دارایی مالک موارد زیر نیست:

- مؤلفه‌ها (Components)
- رخدادها (Incidents)
- پیش‌بینی‌ها (Forecasts)
- نگهداری (Maintenance)

تنها شناسه‌های آن‌ها می‌توانند ارجاع داده شوند.

---

## پیش‌بینی نگهداری (Maintenance Forecast)

پیش‌بینی مالک موارد زیر است:

- محاسبات پیش‌بینی
- چرخه حیات پیش‌بینی
- توصیه‌های پیش‌بینی

پیش‌بینی هرگز مالک موارد زیر نیست:

- دارایی‌ها
- مؤلفه‌ها
- رخدادها

پیش‌بینی صرفاً اطلاعات را مصرف می‌کند.

---

## عملیات نگهداری (Maintenance Operation)

عملیات نگهداری مالک موارد زیر است:

- اجرای کار
- تاریخچه کار
- چرخه حیات عملیاتی

عملیات نگهداری هرگز مالک موارد زیر نیست:

- پیش‌بینی (Forecast)
- موجودی (Inventory)
- دارایی (Asset)

این تجمیع‌ها خودمختار باقی می‌مانند.

---

## رابطه (Relationship)

رابطه مالک موارد زیر است:

- پیوندهای والد-فرزند
- سلسله‌مراتب سازمانی
- قواعد انتشار مالکیت

رابطه هرگز مالک موارد زیر نیست:

- Asset
- User
- Notification
- Conversation

رابطه صرفاً مالک روابط است.

---

## اعلان (Notification)

اعلان مالک موارد زیر است:

- چرخه حیات اعلان
- وضعیت تحویل
- تاریخچه تحویل

اعلان هرگز مالک موارد زیر نیست:

- Relationship
- سلسله‌مراتب کاربران
- Conversation

---

## گفتگوی هوش مصنوعی (AI Conversation)

تجمیع `AIConversation` مالک موارد زیر است:

- تاریخچه پرامپت‌ها
- تاریخچه گفتگو
- فراداده‌های تعامل هوش مصنوعی

تجمیع `AIConversation` هرگز مالک موارد زیر نیست:

- دارایی‌ها
- پیش‌بینی‌ها
- رخدادها
- اعلان‌ها
- روابط

این تجمیع آن‌ها را از طریق مدل‌های خواندنی مصرف می‌کند.

---

# مرز مالکیت (Ownership Boundary)

مالکیت همواره باید در مرزهای تجمیع متوقف شود.

```text
تجمیع (Aggregate)

↓

مرز (Boundary)

↓

پایان مالکیت (Ownership Ends)
```

هیچ تجمیعی مجاز به عبور از این مرز نیست.

---

# مرز تراکنش (Transaction Boundary)

مالکیت دامنه تراکنش را تعیین می‌کند.

هر تجمیع به صورت مستقل کامیت می‌شود.

تراکنش‌های میان‌تجمیعی ممنوع هستند.

---

# مرز پایداری داده‌ها (Persistence Boundary)

هر تجمیع به صورت مستقل ذخیره و پایدار می‌شود.

نمونه:

```text
AssetRepository

ForecastRepository

MaintenanceRepository
```

مخازن داده هرگز نباید چندین تجمیع را با هم پایدار سازند.

---

# قواعد تجاری (Business Rules)

### AO-001

هر تجمیع باید دقیقاً مالک یک مرز پایستگی و سازگاری باشد.

---

### AO-002

مالکیت تجمیع‌ها هرگز نباید هم‌پوشانی داشته باشد.

---

### AO-003

تجمیع‌ها باید به تجمیع‌های خارجی صرفاً از طریق شناسه ارجاع دهند.

---

### AO-004

مالکیت باید دامنه تراکنش را تعیین کند.

---

### AO-005

مالکیت باید مسئولیت مخزن داده را تعیین کند.

---

### AO-006

مالکیت تاریخی باید تغییرناپذیر باقی بماند.

تغییرات مالکیت تاریخچه تولید می‌کنند.

آن‌ها هرگز تاریخچه را بازنویسی نمی‌کنند.

---

## پیامدهای معماری (Architectural Outcomes)

مالکیت تجمیع‌ها موارد زیر را فراهم می‌آورد:

- مسئولیت شفاف؛
- ایزوله‌سازی تراکنش‌ها؛
- پایداری مستقل داده‌ها؛
- مدل دامنه قابل نگهداری؛
- معماری سازمانی مقیاس‌پذیر.

---

# ۹. وابستگی‌های چرخه حیات تجمیع‌ها (Aggregate Lifecycle Dependencies)

## هدف (Purpose)

وابستگی‌های چرخه حیات تجمیع‌ها، ترتیب ایجاد، تکامل و بازنشستگی تجمیع‌ها را تعریف می‌نمایند.

اگرچه تجمیع‌ها خودمختار باقی می‌مانند،

اما چرخه‌های حیات تجاری آن‌ها از یکدیگر مستقل نیست.

وابستگی‌های چرخه حیات، پایستگی تجاری را ضمن حفظ خودمختاری تجمیع‌ها تضمین می‌کنند.

---

# اصل چرخه حیات (Lifecycle Principle)

وابستگی چرخه حیات تجمیع دلالت بر مالکیت تجمیع **ندارد**.

این وابستگی صرفاً توالی تجاری را تعریف می‌نماید.

نمونه:

```text
Asset

↓

TrackedComponent
```

مؤلفه نمی‌تواند پیش از دارایی وجود داشته باشد.

دارایی همچنان مالک تجمیع مؤلفه نیست.

---

# وابستگی‌های ایجاد (Creation Dependencies)

ترتیب ایجاد زیر باید رعایت شود:

| تجمیع | نیازمند وجود پیشین |
|---|---|
| Asset | — |
| Part | — |
| Relationship | — |
| TrackedComponent | Asset |
| Meter | Asset |
| Inventory | Part |
| InventoryTransaction | Inventory |
| Incident | Asset |
| ConditionAssessment | Asset, Meter |
| MaintenanceForecast | Asset, Component, Incident |
| MaintenanceOperation | Forecast |
| Notification | Relationship |
| Conversation | Relationship |
| AIConversation | بافت و زمینه سازمانی |

---

# وابستگی‌های به‌روزرسانی (Update Dependencies)

به‌روزرسانی برخی از تجمیع‌ها نیازمند اطلاعات از سایر تجمیع‌ها است.

نمونه:

```text
MaintenanceForecast

↓

می‌خواند (reads)

ConditionAssessment
```

پیش‌بینی مجدداً محاسبه می‌شود.

ارزیابی وضعیت (ConditionAssessment) بدون تغییر باقی می‌ماند.

---

نمونه دیگر:

```text
MaintenanceOperation

↓

مصرف می‌کند (consumes)

Inventory
```

مقدار موجودی تغییر می‌کند.

عملیات نگهداری همچنان مالک چرخه حیات خود باقی می‌ماند.

---

# وابستگی‌های حذف (Deletion Dependencies)

برخی تجمیع‌ها تا زمانی که تجمیع‌های وابسته وجود دارند قابل حذف نیستند.

نمونه:

```text
Asset

↓

TrackedComponent

↓

MaintenanceHistory
```

بازنشستگی دارایی باید از فرایندهای بازنشستگی تجاری پیروی کند.

حذف فیزیکی و سخت (Hard deletion) ممنوع است.

---

نمونه دیگر:

```text
Part

↓

Inventory

↓

InventoryTransaction
```

تراکنش‌های تاریخی موجودی باید در دسترس باقی بمانند.

بازنشستگی قطعه باید یکپارچگی تاریخی را حفظ کند.

---

# وابستگی‌های تاریخی (Historical Dependencies)

تجمیع‌های تاریخی تغییرناپذیر هستند.

نمونه:

```text
MaintenanceOperation

↓

MaintenanceHistory
```

تاریخچه هرگز بازنویسی نمی‌شود.

به‌روزرسانی‌ها تاریخچه تکمیلی تولید می‌کنند.

---

# چرخه حیات هوش مصنوعی (AI Lifecycle)

چرخه حیات `AIConversation` به زمینه و بافت سازمانی وابسته است:

```text
Relationship

↓

Conversation

↓

AIConversation
```

حذف زمینه سازمانی موجب حذف گفتگوهای تاریخی هوش مصنوعی نمی‌شود.

قابلیت توضیح تاریخی باید حفظ گردد.

---

# چرخه حیات اعلان‌ها (Notification Lifecycle)

```text
Relationship

↓

Notification

↓

Delivery
```

مالکیت اعلان به صورت محلی باقی می‌ماند.

تغییرات روابط صرفاً بر مسیریابی تأثیر می‌گذارند.

---

# دیاگرام چرخه حیات (Lifecycle Diagram)

```text
Asset
   │
   ├────────► TrackedComponent
   │
   ├────────► Meter
   │
   ├────────► Incident
   │
   └────────► ConditionAssessment
                     │
                     ▼
            MaintenanceForecast
                     │
                     ▼
           MaintenanceOperation


Part
 │
 ▼
Inventory
 │
 ▼
InventoryTransaction


Relationship
 │
 ├────────► Notification
 │
 └────────► Conversation
                   │
                   ▼
             AIConversation
```

---

# قواعد چرخه حیات (Lifecycle Rules)

### AL-001

ایجاد تجمیع‌ها باید به وابستگی‌های چرخه حیات احترام بگذارد.

---

### AL-002

به‌روزرسانی‌های تجمیع هرگز نباید قواعد وابستگی را دور بزنند.

---

### AL-003

تجمیع‌های تاریخی باید تغییرناپذیر باقی بمانند.

---

### AL-004

بازنشستگی تجمیع باید یکپارچگی تاریخی را حفظ نماید.

---

### AL-005

وابستگی چرخه حیات هرگز نباید خودمختاری تجمیع‌ها را نقض کند.

---

### AL-006

حذف نرم (Soft deletion) در تمامی مواردی که داده‌های تاریخی وابسته وجود دارند باید بر حذف فیزیکی ترجیح داده شود.

---

## پیامدهای تجاری (Business Outcomes)

وابستگی‌های چرخه حیات موارد زیر را فراهم می‌آورند:

- ترتیب ایجاد قابل پیش‌بینی؛
- یکپارچگی تاریخی؛
- تکامل سازگار و منسجم؛
- بازنشستگی ایمن؛
- ردیابی‌پذیری سازمانی.

---

# ۱۰. قواعد یکپارچه‌سازی تجمیع‌ها (Aggregate Integration Rules)

## هدف (Purpose)

قواعد یکپارچه‌سازی تجمیع‌ها، سازوکارهای مجاز برای برقراری ارتباط میان تجمیع‌ها را تعریف می‌کنند.

یکپارچه‌سازی باید موارد زیر را حفظ کند:

- خودمختاری تجمیع‌ها؛
- پایستگی تراکنشی؛
- زمینه‌های مرزبندی‌شده؛
- مقیاس‌پذیری بلندمدت.

تجمیع‌ها باید همکاری کنند.

تجمیع‌ها هرگز نباید دچار جفت‌شدگی شدید شوند.

---

# اصل یکپارچه‌سازی (Integration Principle)

تجمیع‌ها هرگز مستقیماً با یکدیگر ارتباط برقرار نمی‌کنند.

هر تعامل باید از طریق یکی از سازوکارهای یکپارچه‌سازی تصویب‌شده انجام شود.

---

# سازوکارهای یکپارچه‌سازی تصویب‌شده (Approved Integration Mechanisms)

تجمیع‌ها می‌توانند از طریق موارد زیر ارتباط برقرار کنند:

- شناسه تجمیع (Aggregate Identifier)
- رخداد دامنه (Domain Event)
- سرویس دامنه (Domain Service)
- سرویس کاربردی (Application Service)
- مدل خواندنی (Read Model)

هیچ سازوکار دیگری مجاز نیست.

---

## یکپارچه‌سازی از طریق شناسه (Identifier Integration)

تعامل ارجح و مطلوب.

```text
MaintenanceOperation

↓

AssetId
```

تنها شناسه ذخیره می‌شود.

تجمیع دارایی (Asset) در صورت نیاز به طور مستقل بارگذاری می‌شود.

---

## یکپارچه‌سازی از طریق رخداد دامنه (Domain Event Integration)

تجمیع‌ها رخدادهای تجاری را منتشر می‌کنند.

سایر تجمیع‌ها به صورت مستقل واکنش نشان می‌دهند.

نمونه:

```text
MaintenanceForecastCreated

↓

MaintenanceOperation
```

ویژگی‌ها:

- ناهمگام
- جفت‌شدگی ضعیف
- مقیاس‌پذیر

---

## یکپارچه‌سازی از طریق سرویس دامنه (Domain Service Integration)

قواعد تجاری که چندین تجمیع را در بر می‌گیرند توسط سرویس‌های دامنه هماهنگ می‌شوند.

نمونه:

```text
ForecastCalculationService

↓

Asset

↓

Meter

↓

ConditionAssessment

↓

Incident
```

سرویس دامنه مالک هماهنگ‌سازی است.

تجمیع‌ها خودمختار باقی می‌مانند.

---

## یکپارچه‌سازی از طریق سرویس کاربردی (Application Service Integration)

سرویس‌های کاربردی سناریوهای استفاده را ارکستریت می‌کنند.

نمونه:

```text
CreateMaintenanceOperation

↓

Forecast Repository

↓

Inventory Repository

↓

Maintenance Repository
```

سرویس‌های کاربردی هرگز مالک قواعد تجاری نیستند.

آن‌ها همکاری میان تجمیع‌ها را سازمان‌دهی می‌کنند.

---

## یکپارچه‌سازی از طریق مدل خواندنی (Read Model Integration)

مدل‌های خواندنی، نماهای چندتجمیعی را فراهم می‌سازند.

نمونه:

```text
Asset

Component

Forecast

Maintenance

↓

Maintenance Dashboard
```

مدل‌های خواندنی هرگز به تجمیع تبدیل نمی‌شوند.

آن‌ها پروجکشن‌های فقط‌خواندنی هستند.

---

# یکپارچه‌سازی هوش مصنوعی (AI Integration)

دستیار هوش مصنوعی صرفاً با مدل‌های خواندنی تعامل دارد.

نمونه:

```text
Read Models

↓

AI Assistant
```

دستیار هوش مصنوعی هرگز نباید:

- گراف‌های تجمیع را بارگذاری کند؛
- منطق تجمیع را اجرا نماید؛
- وضعیت تجمیع را تغییر دهد.

---

# یکپارچه‌سازی گزارش‌گیری (Reporting Integration)

گزارش‌گیری مدل‌های خواندنی را مصرف می‌کند.

گزارش‌گیری هرگز نباید مستقیماً گراف‌های تجمیع را کوئری بگیرد.

این امر کارایی و استقلال تجمیع‌ها را حفظ می‌کند.

---

# یکپارچه‌سازی اعلان‌ها (Notification Integration)

مدیریت روابط رخدادها را منتشر می‌کند.

مرکز اعلان‌ها رخدادها را مصرف می‌کند.

```text
RelationshipUpdated

↓

NotificationCreated
```

هیچ‌یک از تجمیع‌ها مالک دیگری نیست.

---

# یکپارچه‌سازی پیام‌رسانی (Messaging Integration)

میزان دید گفتگوها به بافت و زمینه انتشاریافته بستگی دارد.

مدیریت روابط زمینه را فراهم می‌سازد.

پیام‌رسانی داخلی مالک گفتگوها است.

---

# محدودیت‌های یکپارچه‌سازی (Integration Constraints)

یکپارچه‌سازی‌های زیر اکیداً ممنوع هستند:

### فراخوانی‌های مستقیم تجمیع‌ها (Direct Aggregate Calls)

نامعتبر:

```text
Aggregate

↓

Aggregate
```

---

### پایداری مشترک داده‌ها (Shared Persistence)

نامعتبر:

```text
Repository

↓

Multiple Aggregates
```

---

### تراکنش‌های مشترک (Shared Transactions)

نامعتبر:

```text
Aggregate A

+

Aggregate B

↓

Single Transaction
```

---

### مالکیت فرامرزی تجمیع‌ها (Cross-Aggregate Ownership)

نامعتبر:

```text
Aggregate

↓

owns

↓

Aggregate
```

---

# قواعد یکپارچه‌سازی (Integration Rules)

### AI-001

تجمیع‌ها تنها باید از طریق سازوکارهای یکپارچه‌سازی تصویب‌شده ارتباط برقرار کنند.

---

### AI-002

رخدادهای دامنه سازوکار ناهمگام مطلوب و ارجح هستند.

---

### AI-003

سرویس‌های کاربردی سناریوهای کاربردی تجاری را ارکستریت می‌کنند.

---

### AI-004

مدل‌های خواندنی برای کوئری‌ها جایگزین گراف‌های تجمیع می‌شوند.

---

### AI-005

خودمختاری تجمیع‌ها هرگز نباید نقض شود.

---

### AI-006

مخازن داده به صورت اختصاصی برای هر تجمیع باقی می‌مانند.

---

### AI-007

پایداری مشترک داده‌ها ممنوع است.

---

## پیامدهای معماری (Architectural Outcomes)

قواعد یکپارچه‌سازی تجمیع‌ها موارد زیر را فراهم می‌آورند:

- همکاری مقیاس‌پذیر؛
- تجمیع‌های مستقل؛
- معماری رخدادمحور؛
- ارکستراسیون با قابلیت استفاده مجدد؛
- انطباق با اصول DDD؛
- آمادگی برای مایکروسرویس‌ها در آینده.

---

# ۱۱. اسناد مرتبط (Related Documents)

## معماری (Architecture)

اسناد معماری زیر شالوده ساختاری این مشخصات را فراهم می‌سازند:

- ../02-architecture/01-Architecture.md
- ../02-architecture/02-CapabilityModel.md

---

## توسعه (Development)

اسناد توسعه زیر مکمل این مشخصات هستند:

- 01-SolutionStructure.md
- 02-ProjectStructure.md
- 11-DependencyCatalog.md
- 04-DependencyRules.md
- 10-BuildPipeline.md
- ../03-domain/12-DomainPatterns.md
- 12-CapabilityDependencyMatrix.md

---

## مشخصات تجاری (Business Specifications)

تعاریف تجمیع‌ها از مشخصات تجاری سرچشمه می‌گیرند:

- Asset Management (DD-001؛ بدون سند مشخصات تجاری اختصاصی)
- BR-004 Tracked Components
- Meter Management (بدون سند مشخصات تجاری اختصاصی)
- Condition Monitoring (بدون سند مشخصات تجاری اختصاصی)
- BR-007 Parts Catalog
- Inventory Management (آتی — هنوز ساخته نشده است)
- BR-009 Incident Management
- BR-010 Maintenance Forecast
- BR-011 Maintenance Operations
- BR-012 Notification Center
- BR-013 Internal Messaging
- BR-014 AI Assistant
- BR-015 Relationship Management

---

## رابطه با الگوهای دامنه (Relationship to Domain Patterns)

تعاملات تجمیع‌ها رفتارهای با قابلیت استفاده مجدد تعریف‌شده توسط موارد زیر را مفروض می‌دانند:

- DP-001 الگوی عملیات تجاری (Business Operation Pattern)
- DP-003 الگوی چرخه حیات (Lifecycle Pattern)
- DP-004 الگوی روابط (Relationship Pattern)
- DP-009 الگوی روابط سلسله‌مراتبی (Hierarchical Relationship Pattern)
- DP-010 الگوی هوش مشورتی (Advisory Intelligence Pattern)
- DP-015 الگوی ردیابی‌پذیری تجاری (Business Traceability Pattern)

---

# ۱۲. جایگاه معماری (Architectural Position)

ماتریس وابستگی تجمیع‌ها پایین‌ترین سطح انتزاع معماری را پیش از پیاده‌سازی اشغال می‌کند.

سلسله‌مراتب مستندات عبارت است از:

```text
چشم‌انداز (Vision)

↓

معماری (Architecture)

↓

مشخصات تجاری (Business Specification)

↓

ماتریس وابستگی قابلیت‌ها (Capability Dependency Matrix)

↓

ماتریس وابستگی تجمیع‌ها (Aggregate Dependency Matrix)

↓

پیاده‌سازی (Implementation)
```

این سند موارد زیر را تعریف می‌کند:

- مرزهای تجمیع‌ها؛
- تعامل تجمیع‌ها؛
- مالکیت تجمیع‌ها؛
- توالی تجمیع‌ها؛
- ارکستراسیون تجمیع‌ها.

پیاده‌سازی باید با این سند مطابقت کامل داشته باشد.

هیچ پیاده‌سازی مجاز به معرفی روابط تجمیعی مغایر با این مشخصات نیست.

تغییر در وابستگی‌های تجمیع‌ها نیازمند بازبینی معماری است.

تغییر در مالکیت تجمیع‌ها نیازمند بازبینی دامنه است.

---

## مسئولیت معماری (Architectural Responsibility)

این سند مرجع معتبر برای موارد زیر است:

- ایجاد تجمیع‌ها؛
- مرزهای مخازن داده؛
- سرویس‌های دامنه؛
- رخدادهای دامنه؛
- تعامل تجمیع‌ها؛
- پیاده‌سازی مدل دامنه.

هرگونه پیاده‌سازی تجمیع باید به این سند قابل ردگیری باشد.

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

---

# ۱۳. تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-20 | معمار راهکار | ماتریس اولیه وابستگی تجمیع‌ها |
| 3.0.0 | 2026-07-20 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-02 | معمار راهکار | اصلاح مراجع BR پیش از شماره‌گذاری مجدد (علامت‌گذاری Asset Management، Meter Management، Condition Monitoring و Inventory Management به عنوان فاقد سند مشخصات اختصاصی بر اساس تصمیم مالک محصول)؛ تصحیح ۲ نام اشتباه الگوی دامنه (DP-002 و DP-005 با نام اشتباه ذکر شده بودند) و اصلاح مسیرهای نسبی خراب به 02-architecture/ و 03-domain/12-DomainPatterns.md |
| 4.2.0 | 2026-08-08 | معمار راهکار | اصلاح نسخه 4.1.0: الگوی "DP-006 Business Traceability Pattern" به اشتباه حذف شده بود، اما خود الگو واقعی است و رسماً با شماره DP-015 در 12-DomainPatterns.md ثبت شده است. مرجع با شماره تصحیح‌شده بازگردانده شد |
