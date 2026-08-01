# طراحی نقاط پایانی (Endpoint Design)

| ویژگی | مقدار |
|----------|-------|
| **شناسه مستند** | API-002 |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال (Active) |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف (Purpose)

این مستند راهنمای طراحی نقاط پایانی (Endpoints) مورد استفاده در تمامی بخش‌های MachineryManagerEnterprise را تعریف می‌کند.

هر نقطه پایانی باید یک رابط یکنواخت، قابل پیش‌بینی و مبتنی بر کسب‌وکار ارائه دهد.

---

# ۲. فلسفه نقاط پایانی (Endpoint Philosophy)

نقاط پایانی قابلیت‌های کسب‌وکار را افشا می‌کنند.

نقاط پایانی عملیات دیتابیس را افشا نمی‌کنند.

هر نقطه پایانی باید نشان‌دهنده یک اقدام معنی‌دار کسب‌وکار یا یک منبع کسب‌وکار باشد.

---

# چرخه حیات نقطه پایانی (Endpoint Lifecycle)

هر نقطه پایانی باید:

۱. درخواست HTTP را دریافت کند.
۲. محدودیت‌های سطح ترابری (Transport-level) را اعتبارسنجی کند.
۳. دقیقاً یک دستور (Command) یا پرس‌وجو (Query) را فراخوانی کند.
۴. یک قرارداد پایدار بازگرداند.
۵. هرگز جزئیات پیاده‌سازی داخلی را افشا نکند.

---

# قوانین طراحی نقاط پایانی

نقاط پایانی باید:

- نشان‌دهنده قابلیت‌های کسب‌وکار باشند.
- مستقل از فناوری باقی بمانند.
- از شناسه‌های URI پایدار استفاده کنند.
- مدل‌های پاسخ یکنواخت بازگردانند.
- هرگز مفاهیم دیتابیس را افشا نکنند.

---

# ۳. طراحی مبتنی بر منبع (Resource-Oriented Design)

نقاط پایانی باید حول محور منابع (Resources) سازماندهی شوند.

مثال‌ها:

```text
/assets

/engines

/components

/maintenance-orders

/documents

/forecasts
```

منابع نشان‌دهنده موجودیت‌های کسب‌وکار هستند.

---

# ۴. نقاط پایانی مجموعه‌ها (Collection Endpoints)

نقاط پایانی مجموعه‌ها چندین منبع را بازمی‌گردانند.

مثال‌ها:

```text
GET /assets

GET /engines

GET /documents
```

نقاط پایانی مجموعه‌ها باید از موارد زیر پشتیبانی کنند:

- صفحه‌بندی (Pagination)
- فیلترسازی (Filtering)
- مرتب‌سازی (Sorting)

---

# ۵. نقاط پایانی تک‌منبعی (Single Resource Endpoints)

نقاط پایانی تک‌منبعی یک شیء کسب‌وکار مشخص را شناسایی می‌کنند.

مثال‌ها:

```text
GET /assets/{assetId}

GET /engines/{engineId}

GET /documents/{documentId}
```

شناسه‌ها باید درون منبع خود به صورت یکتا در سطح سیستم (Globally Unique) باشند.

---

# ۶. ایجاد منبع (Resource Creation)

نقاط پایانی ایجاد منبع از متد POST استفاده می‌کنند.

مثال‌ها:

```text
POST /assets

POST /maintenance-orders

POST /documents
```

بدنه درخواست (Request body) شامل مدل ایجاد است.

پاسخ، منبع ایجادشده را بازمی‌گرداند.

---

# ۷. به‌روزرسانی منبع (Resource Update)

جایگزینی کامل منبع:

```text
PUT /assets/{assetId}
```

تغییر و به‌روزرسانی جزئی:

```text
PATCH /assets/{assetId}
```

درخواست‌های به‌روزرسانی باید قبل از ثبت در لایه ماندگاری، قوانین کسب‌وکار را اعتبارسنجی کنند.

---

# ۸. حذف منبع (Resource Removal)

نقاط پایانی حذف از متد DELETE استفاده می‌کنند.

مثال:

```text
DELETE /documents/{documentId}
```

حقیقی بودن (Physical) یا منطقی بودن (Logical) حذف به قوانین کسب‌وکار بستگی دارد.

---

# ۹. عملیات‌های کسب‌وکار (Business Operations)

برخی از عملیات‌های کسب‌وکار در قالب CRUD نمی‌گنجند.

مثال‌ها:

```text
POST /assets/{assetId}/retire

POST /engines/{engineId}/install

POST /engines/{engineId}/remove

POST /maintenance-orders/{id}/complete

POST /documents/{id}/renew
```

اقدامات و عملیات‌های کسب‌وکار که نمی‌توان آن‌ها را به صورت طبیعی در قالب CRUD نشان داد، باید از متد POST استفاده کنند.

---

# ۱۰. منابع فرزند (Child Resources)

منابع فرزند نشان‌دهنده مالکیت (Ownership) هستند.

مثال‌ها:

```text
GET /assets/{assetId}/engines

GET /assets/{assetId}/documents

GET /assets/{assetId}/maintenance-history
```

منابع توئیده‌شده باید روابط طبیعی کسب‌وکار را توصیف کنند.

---

# ۱۱. نقاط پایانی جستجو (Search Endpoints)

عملیات‌های جستجو همچنان مبتنی بر منبع باقی می‌مانند.

روش ترجیحی:

```text
GET /assets?serialNumber=...

GET /documents?status=...

GET /engines?manufacturer=...
```

از ایجاد نقاط پایانی اختصاصی `/search` خودداری کنید، مگر اینکه پرس‌وجو برای فیلترسازی استاندارد بیش از حد پیچیده باشد.

---

# ۱۲. نقاط پایانی پیش‌بینی (Forecast Endpoints)

پیش‌بینی‌ها نتایج تولیدشده کسب‌وکار هستند.

مثال‌ها:

```text
POST /forecasts

GET /forecasts/{forecastId}

GET /assets/{assetId}/forecasts
```

تولید پیش‌بینی می‌تواند به صورت همگام (Synchronous) یا ناهمگام (Asynchronous) باشد.

---

# ۱۳. عملیات‌های زمان‌بر (Long Running Operations)

عملیات‌هایی که نیازمند زمان قابل توجهی هستند باید کد زیر را بازگردانند:

```text
202 Accepted
```

مثال:

```text
POST /forecasts
```

پاسخ شامل:

```text
OperationId

Status

Location
```

کلاینت‌ها می‌توانند بعداً وضعیت تکمیل عملیات را استعلام کنند.

---

# ۱۴. عملیات‌های حجمی و گروهی (Bulk Operations)

عملیات‌های حجمی باید به صورت صریح تعریف شوند.

مثال‌ها:

```text
POST /assets/bulk-import

POST /documents/bulk-update

POST /maintenance-orders/bulk-close
```

عملیات‌های حجمی هرگز نباید نقاط پایانی استاندارد CRUD را بارگذاری اضافی (Overload) کنند.

---

# ۱۵. قوانین نام‌گذاری نقاط پایانی

اسامی نقاط پایانی باید:

- از اسم استفاده کنند؛
- از افعال اجتناب کنند؛
- با حروف کوچک نوشته شوند؛
- پایدار بمانند.

صحیح:

```text
/assets

/engines

/components
```

نادرست:

```text
/CreateAsset

/GetAssets

/DeleteEngine
```

---

# ۱۶. پایداری نقاط پایانی

نقاط پایانی عمومی به عنوان رابط‌های قراردادی تلقی می‌شوند.

تغییر یک نقطه پایانی نیازمند موارد زیر است:

- بررسی نسخه API
- به‌روزرسانی مستندات
- ارزیابی سازگاری با نسخه‌های قبلی (Backward compatibility)

---

# ۱۷. توسعه‌های آتی

ماژول‌های آینده باید از همین ساختار نقطه پایانی تبعیت کنند.

مثال‌ها:

```text
/inventory

/procurement

/contracts

/fleet

/iot
```

یکنواختی و انسجام باید در تمامی قابلیت‌های آتی حفظ شود.

---

# نمونه نگاشت نقطه پایانی به لایه Application

| Endpoint                   | Application            |
| -------------------------- | ---------------------- |
| `GET /assets`              | `GetAssetsQuery`       |
| `GET /assets/{id}`         | `GetAssetByIdQuery`    |
| `POST /assets`             | `RegisterAssetCommand` |
| `PATCH /assets/{id}`       | `UpdateAssetCommand`   |
| `POST /assets/{id}/retire` | `RetireAssetCommand`   |

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

- `00-ApiPrinciples.md`
- `01-RestConventions.md`
- `03-ResponseModel.md`
- `04-ErrorResponses.md`
- `05-Versioning.md`
- ADR-0005 — استراتژی API

---

# تاریخچه تغییرات

| نسخه | تاریخ | شرح |
|----------|------------|---------------------------------------------|
| 1.0.0 | اولیه | طراحی اولیه نقاط پایانی |
| 3.0.0 | 2026-07-18 | استانداردسازی طبق استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
