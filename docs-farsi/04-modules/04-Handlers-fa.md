# پردازنده‌ها (Handlers)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | APP-004 |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف

این سند مسئولیت‌های پردازنده‌های دستور (Command Handlers) و پردازنده‌های پرس‌وجو (Query Handlers) را تعریف می‌کند.

پردازنده‌ها اجرای درخواست‌های لایه کاربرد را هماهنگ می‌کنند.

پردازنده‌ها شامل قوانین کسب‌وکار نیستند.

---

# فلسفه پردازنده‌ها

پردازنده‌ها هماهنگ‌کننده اجرای عملیات هستند.

آن‌ها جریان کاربردی را به اجرا درمی‌آورند ولی هرگز قوانین کسب‌وکار را پیاده‌سازی نمی‌کنند.

رفتار و قوانین کسب‌وکار همواره متعلق به بخش‌های زیر است:

- مجموعه‌ها (Aggregates)
- خدمات دامنه (Domain Services)

پردازنده‌ها لایه کاربرد را به لایه دامنه متصل می‌کنند.

---

# قوانین طراحی پردازنده

هر پردازنده باید:

- دقیقاً یک درخواست را پردازش کند؛
- بدون وضعیت (Stateless) باشد؛
- تنها به انتزاع‌ها (Abstractions) وابسته باشد؛
- نتایج کاربردی (Application Results) بازگرداند؛
- هرگز موجودیت‌های دامنه را به صورت مستقیم در دسترس بیرون قرار ندهد.

---

# ۲. اصول پردازنده‌ها

هر پردازنده باید اصول زیر را برآورده سازد:

- مسئولیت واحد (Single Responsibility)
- یک پردازنده به ازای هر دستور (One Handler per Command)
- یک پردازنده به ازای هر پرس‌وجو (One Handler per Query)
- مستقل از فناوری (Technology independent)
- بدون وضعیت (Stateless)
- لایه هماهنگ‌کننده سبک (Thin orchestration layer)

---

# ۳. دسته‌بندی پردازنده‌ها

```text
پردازنده‌ها (Handlers)

├── پردازنده‌های دستور (Command Handlers)
└── پردازنده‌های پرس‌وجو (Query Handlers)
```

---

# ۴. مسئولیت‌های پردازنده دستور (Command Handler)

یک پردازنده دستور باید:

- درخواست کاربردی را اعتبارسنجی کند.
- مجاز بودن و دسترسی را بررسی کند (Authorization).
- مجموعه(های) دامنه را بارگذاری کند.
- رفتار مجموعه را فراخوانی نماید.
- در صورت نیاز خدمات دامنه (Domain Services) را فراخوانی کند.
- رویدادهای دامنه (Domain Events) را منتشر سازد.
- تراکنش را تایید (Commit) کند.
- نتیجه اجرای عملیات را بازگرداند.

یک پردازنده دستور هرگز نباید منطق یا قوانین کسب‌وکار را پیاده‌سازی کند.

---

# ۵. مسئولیت‌های پردازنده پرس‌وجو (Query Handler)

یک پردازنده پرس‌وجو باید:

- درخواست را اعتبارسنجی کند.
- مجاز بودن و دسترسی را بررسی کند (Authorization).
- مدل خواندن را بازیابی نماید.
- داده‌ها را نگاشت / پروجکت کند.
- پاسخ را بازگرداند.

یک پردازنده پرس‌وجو هرگز نباید وضعیت کسب‌وکار را تغییر دهد.

---

# ۶. چرخه حیات پردازنده دستور

```text
دریافت دستور (Receive Command)
          │
          ▼
احراز دسترسی (Authorization)
          │
          ▼
اعتبارسنجی کاربردی (Application Validation)
          │
          ▼
بارگذاری مجموعه (Load Aggregate)
          │
          ▼
اجرای رفتار دامنه (Execute Domain Behavior)
          │
          ▼
جمع‌آوری رویدادهای دامنه (Collect Domain Events)
          │
          ▼
تایید تراکنش (Commit Transaction)
          │
          ▼
انتشار رویدادها (Publish Events)
          │
          ▼
بازگرداندن نتیجه (Return Result)
```

---

# ۷. چرخه حیات پردازنده پرس‌وجو

```text
دریافت پرس‌وجو (Receive Query)
          │
          ▼
احراز دسترسی (Authorization)
          │
          ▼
اعتبارسنجی پرس‌وجو (Validate Query)
          │
          ▼
مدل خواندن (Read Model)
          │
          ▼
پروجکشن / نگاشت (Projection)
          │
          ▼
بازگرداندن نتیجه (Return Result)
```

---

# ۸. نگاشت دستورات و پردازنده‌ها (نمونه‌ها)

| دستور | پردازنده |
|----------|---------|
| RegisterAsset | RegisterAssetHandler |
| UpdateAssetInformation | UpdateAssetInformationHandler |
| TransferAsset | TransferAssetHandler |
| RetireAsset | RetireAssetHandler |
| DisposeAsset | DisposeAssetHandler |
| RegisterEngine | RegisterEngineHandler |
| InstallEngine | InstallEngineHandler |
| RemoveEngine | RemoveEngineHandler |
| ReplaceEngine | ReplaceEngineHandler |
| InstallMeter | InstallMeterHandler |
| ReplaceMeter | ReplaceMeterHandler |
| CreateMaintenancePlan | CreateMaintenancePlanHandler |
| CompleteMaintenance | CompleteMaintenanceHandler |
| RegisterAssetPurchase | RegisterAssetPurchaseHandler |

---

# ۹. نگاشت پرس‌وجوها و پردازنده‌ها (نمونه‌ها)

| پرس‌وجو | پردازنده |
|--------|---------|
| GetAsset | GetAssetHandler |
| SearchAssets | SearchAssetsHandler |
| GetAssetHistory | GetAssetHistoryHandler |
| GetEngine | GetEngineHandler |
| GetMaintenanceHistory | GetMaintenanceHistoryHandler |
| GetCurrentAssetValue | GetCurrentAssetValueHandler |
| GetDocumentPackage | GetDocumentPackageHandler |
| GetFuelForecast | GetFuelForecastHandler |

---

# ۱۰. قوانین تعامل با مجموعه‌ها

یک پردازنده می‌تواند:

- یک مجموعه را بارگذاری کند؛
- در صورت نیاز چندین مجموعه را بارگذاری کند؛
- خدمات دامنه را فراخوانی کند؛
- خدمات زیرساختی را از طریق انتزاع‌ها فراخوانی نماید.

یک پردازنده هرگز نباید وضعیت مجموعه را مستقیماً دستکاری یا ویرایش کند (باید حتماً متدهای دامنه مجموعه فراخوانی شوند).

---

# ۱۱. قوانین تراکنش‌ها

به طور معمول:

- یک دستور
- یک تراکنش
- یک تایید (Commit)

اگر چندین مجموعه مشارکت دارند، یکپارچگی باید بر اساس قوانین دامنه مدیریت شود.

---

# ۱۲. قوانین وابستگی

پردازنده‌ها می‌توانند به موارد زیر وابسته باشند:

- رابط‌های مخزن (Repository Interfaces)
- خدمات دامنه (Domain Services)
- واحد کار (Unit of Work)
- ثبت‌کننده لاگ (Logger)
- خدمات کاربردی (Application Services)

پردازنده‌ها هرگز نباید مستقیماً به موارد زیر وابسته باشند:

- Entity Framework یا ORMهای خاص
- کدهای مستقیم SQL
- پیاده‌سازی‌های مستقیم زیرساختی

---

# ۱۳. مدیریت خطاها

پردازنده‌ها باید موارد زیر را ترجمه کرده و به نتایج کاربردی (Application Results) تبدیل کنند:

- خطاهای اعتبارسنجی
- خطاهای عدم دسترسی
- استثناهای دامنه
- استثناهای همزمانی
- استثناهای زیرساختی

---

# ۱۴. قواعد نام‌گذاری

پردازنده دستور:
`<CommandName>Handler` (مانند `RegisterAssetHandler`)

پردازنده پرس‌وجو:
`<QueryName>Handler` (مانند `GetAssetHandler`)

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

- `02-Commands-fa.md`
- `03-Queries-fa.md`
- `05-ApplicationServices-fa.md`
- `05-DomainServices-fa.md`
- `06-DomainEvents-fa.md`
- `ADR-0011 — Adopt CQRS`

---

# تاریخچه تغییرات

| نسخه | تاریخ | شرح |
|----------|------------|----------------------------------------------|
| 1.0.0 | اولیه | معماری اولیه پردازنده‌ها |
| 3.0.0 | 2026-07-18 | استانداردسازی مطابق با استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
