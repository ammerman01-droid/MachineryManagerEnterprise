# ماتریس وابستگی Aggregateها (Aggregate Dependency Matrix)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | DD-014 |
| **نام سند** | ماتریس وابستگی Aggregateها |
| **نسخه** | 4.0.0 |
| **وضعیت** | پیش‌نویس |
| **مالک** | معمار دامنه (Domain Architect) |
| **تاریخ ایجاد** | 2026-07-20 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف

این سند روابط وابستگی بین Aggregateها را درون **MachineryManagerEnterprise** تعریف می‌کند.

هدف از این سند عبارت است از:

- حفظ استقلال و خودمختاری Aggregateها؛
- تعریف تعاملات بین Aggregateها؛
- شناسایی مالکیت Aggregateها؛
- پشتیبانی از ترتیب پیاده‌سازی؛
- پشتیبانی از طراحی دامنه‌محور (Domain-Driven Design).

این سند مرجع معتبر تعاملات بین Aggregateهاست.

---

# ۲. محدوده

این سند موارد زیر را پوشش می‌دهد:

- وابستگی‌های Aggregateها؛
- مالکیت Aggregateها؛
- تعاملات Aggregateها؛
- وابستگی‌های چرخه حیات Aggregateها.

این سند موارد زیر را **تعریف نمی‌کند**:

- قوانین کسب‌وکار (Business Rules)؛
- سرویس‌های کاربرد (Application Services)؛
- ارجاعات پروژه؛
- زیرساخت‌ها.

---

# ۳. فلسفه وابستگی Aggregateها

Aggregateها مرزهای خودگردان و مستقل یکپارچگی (Consistency Boundaries) هستند.

یک Aggregate هرگز نباید از طریق مالکیت مستقیم به Aggregate دیگری وابسته باشد.

در عوض، Aggregateها از طریق موارد زیر تعامل می‌کنند:

- شناسه و شناساگرها (Identifiers)؛
- رویدادهای دامنه (Domain Events)؛
- سرویس‌های دامنه (Domain Services)؛
- مخازن داده (Repositories)؛
- سرویس‌های کاربرد (Application Services).

استقلال و خودمختاری Aggregateها همواره باید حفظ شود.

---

# ارجاع مستقیم ممنوع

**درست:**
```text
Aggregate A ➔ شناسه (Identifier) ➔ Aggregate B
```

**نادرست:**
```text
Aggregate A ➔ ارجاع شیء (Object Reference) ➔ Aggregate B
```

ارجاع مستقیم بین Aggregateها ممنوع است.

---

# ۴. انواع وابستگی

وابستگی‌های بین Aggregateها به ۵ دسته تقسیم می‌شوند:

۱. **وابستگی شناسه (Identity Dependency)**: یک Aggregate تنها شناسه (ID) Aggregate دیگری را ذخیره می‌کند (روش ترجیحی).
۲. **وابستگی ارجاعی (Reference Dependency)**: یک Aggregate به اطلاعات کسب‌وکار Aggregate دیگر نیاز دارد، اما مالک آن نیست (خواندن-تنها).
۳. **وابستگی رویداد دامنه (Domain Event Dependency)**: Aggregateها از طریق رویدادهای دامنه به صورت ناهمگام ارتباط برقرار می‌کنند.
۴. **وابستگی سرویس (Service Dependency)**: دو Aggregate از طریق یک سرویس دامنه (Domain Service) بدون داشتن مالکیت متقابل همکاری می‌کنند.
۵. **وابستگی تاریخی (Historical Dependency)**: یک Aggregate به اطلاعات تاریخی تغییرناپذیر Aggregate دیگر نیاز دارد.

---

# ۵. ماتریس وابستگی Aggregateها

| Aggregate | وابسته است به | نوع وابستگی | قدرت |
|------------|------------|-----------------|----------|
| Asset (دارایی) | — | — | مستقل |
| TrackedComponent (قطعه ردیابی‌شونده) | Asset | شناسه (Identity) | اجباری (Required) |
| Meter (کنتور) | Asset | شناسه (Identity) | اجباری (Required) |
| ConditionAssessment (پایش وضعیت) | Asset, Meter | ارجاعی (Reference) | اجباری (Required) |
| Part (قطعه کاتالوگ) | — | — | مستقل |
| Inventory (انبار) | Part | شناسه (Identity) | اجباری (Required) |
| InventoryTransaction (تراکنش انبار) | Inventory, Part | شناسه (Identity) | اجباری (Required) |
| Incident (حادثه) | Asset | شناسه (Identity) | اجباری (Required) |
| MaintenanceForecast (پیش‌بینی نت) | Asset, TrackedComponent, Meter, ConditionAssessment, Incident, Part | ارجاعی (Reference) | اجباری (Required) |
| MaintenanceOperation (عملیات نت) | Asset, TrackedComponent, Part, Inventory, MaintenanceForecast | ارجاعی (Reference) | اجباری (Required) |
| Notification (اعلان) | Relationship | رویداد (Event) | اجباری (Required) |
| Conversation (گفتگو) | Relationship | زمینه‌ای (Context) | پیشنهادی (Recommended) |
| Relationship (روابط) | — | — | مستقل |
| AIConversation (گفتگوی هوش مصنوعی) | تمام Aggregateهای قبلی | زمینه‌ای (Context) | اجباری (Required) |

---

# ۶. قوانین تعامل Aggregateها

- **عدم دستکاری مستقیم**: یک Aggregate هرگز نباید متدهای تغییر دهنده وضعیت Aggregate دیگر را مستقیماً فراخوانی کند.
- **مخازن داده اختصاصی (Repositories)**: هر Aggregate دارای Repository اختصاصی خود است. Repositories نباید گراف‌های شامل چندین Aggregate را برگردانند.
- **مرز تراکنش**: هر Aggregate مرز تراکنش اختصاصی خود را تعریف می‌کند. تراکنش‌هایی که شامل چندین Aggregate هستند باید بیرون از آن‌ها (مثلاً در Application Serviceها با یکپارچگی نهایی/Eventual Consistency) مدیریت شوند.
- **الگوی CQRS/Read Models**: گزارش‌گیری و هوش مصنوعی تنها از مدل‌های خواندن (Read Models) و پروژکشن‌ها استفاده می‌کنند، نه از گراف‌های عملیاتی Aggregateها.

---

# ۷. گراف توپولوژی Aggregateها

سه خوشه اصلی (Cluster) از Aggregateها تشکیل می‌شود:

۱. **خوشه عملیاتی**: Asset ➔ TrackedComponent / Meter / Incident ➔ ConditionAssessment ➔ MaintenanceForecast ➔ MaintenanceOperation
۲. **خوشه انبار و قطعات**: Part ➔ Inventory ➔ InventoryTransaction / MaintenanceOperation
۳. **خوشه همکاری**: Relationship ➔ Notification / Conversation ➔ AIConversation

---

# ۸. مالکیت Aggregateها (Aggregate Ownership)

هر Aggregate مالک ویژگی‌ها، متدها، وضعیت و چرخه حیات خود است. هیچ Aggregateی مالک Aggregate دیگری نیست.

مثال:
- **MaintenanceOperation** مالک کارهای نت (Maintenance Tasks) درون خود است، اما مالک **Asset** یا **Inventory** نیست.
- **Asset** مالک خصوصیات خود است، اما مالک **Incidents** یا **Components** نیست.

---

# ۹. وابستگی‌های چرخه حیات (Lifecycle Dependencies)

ترتیب ایجاد Aggregateها باید رعایت شود:
- ابتدا Asset، Part، Relationship ایجاد می‌شوند.
- سپس TrackedComponent، Meter، Inventory، Incident ایجاد می‌گردند.
- سپس ConditionAssessment و MaintenanceForecast.
- سپس MaintenanceOperation.
- حذف فیزیکی (Hard Delete) برای Aggregateهایی که داده‌های تاریخی وابسته دارند ممنوع است و باید از حذف منطقی (Soft Delete) استفاده شود.

---

# ۱۰. قوانین یکپارچه‌سازی (Integration Rules)

- یکپارچه‌سازی بین Aggregateها تنها از طریق شناسه (ID)، رویدادهای دامنه، سرویس‌های دامنه، سرویس‌های کاربرد و مدل‌های خواندن (Read Models) مجاز است.
- اشتراک‌گذاری دیتابیس یا جدول مشترک بین Aggregateها ممنوع است.

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

- 02-architecture/01-Architecture.md
- 02-architecture/09-CapabilityModel.md
- DOC-DEV-001 (اصول توسعه)
- DOC-DEV-002 (ساختار راهکار)
- DOC-DEV-012 (الگوهای دامنه)
- DD-013 (ماتریس وابستگی قابلیت‌ها)

---

# تاریخچه تغییرات

| نسخه | تاریخ | نویسنده / نقش | شرح |
|----------|------------|-------------------|-------------------------------------------|
| 1.0.0 | 2026-07-20 | معمار دامنه | ماتریس اولیه وابستگی Aggregateها |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
