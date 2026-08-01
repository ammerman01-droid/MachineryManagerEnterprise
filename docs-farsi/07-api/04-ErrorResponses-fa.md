# پاسخ‌های خطا (Error Responses)

| ویژگی | مقدار |
|----------|-------|
| **شناسه مستند** | API-004 |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال (Active) |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف (Purpose)

این مستند مدل استاندارد پاسخ به خطاها را برای تمامی APIهای HTTP ارائه شده توسط MachineryManagerEnterprise تعریف می‌کند.

هر درخواست ناموفق باید یک پاسخ یکنواخت، قابل پیش‌بینی و قابل خواندن توسط ماشین بازگرداند.

---

# فلسفه خطاها (Error Philosophy)

خطاها نشان‌دهنده شکست‌های فنی یا کسب‌وکاری در یک فرمت استانداردشده هستند.

پاسخ‌های خطا باید دارای ویژگی‌های زیر باشند:

- پایدار (Stable)
- قابل پیش‌بینی (Predictable)
- قابل خواندن توسط ماشین (Machine readable)
- امن (Safe)

خطاها هرگز نباید جزئیات پیاده‌سازی داخلی را افشا کنند.

---

# ۲. اصول (Principles)

پاسخ‌های خطا باید:

- یکنواخت (Consistent)
- معین و مشخص (Deterministic)
- قابل خواندن توسط ماشین (Machine-readable)
- قابل فهم برای انسان (Human-readable)
- قابل ردگیری (Traceable) باشند.

یک نوع از شکست باید همیشه همان فرمت پاسخ را تولید کند.

---

# ۳. ساختار استاندارد خطا (Standard Error Structure)

هر پاسخ خطا باید از ساختار زیر پیروی کند:

```json
{
  "errorCode": "BUS-014",
  "title": "Business Rule Violation",
  "message": "The selected engine is already installed.",
  "correlationId": "7f1f25f2-a8db-4e76-ae4d-4f8c16e4c08d",
  "details": []
}
```

---

# ۴. فیلدها (Fields)

| فیلد | الزامی | شرح |
|--------|----------|-------------|
| errorCode | بله | کد خطای پایدار برنامه |
| title | بله | دسته‌بندی کوتاه خطا |
| message | بله | توصیف قابل فهم برای انسان |
| correlationId | بله | شناسه همبستگی درخواست |
| details | خیر | اعتبارسنجی یا اطلاعات تکمیلی |

---

# دسته‌بندی خطاها (Error Classification)

| دسته‌بندی | پیشوند (Prefix) |
| -------------- | ------ |
| اعتبارسنجی (Validation) | VAL |
| کسب‌وکار (Business) | BUS |
| احراز هویت (Authentication) | AUTH |
| منبع (Resource) | RES |
| زیرساخت (Infrastructure) | INF |
| سیستم (System) | SYS |

---

# ۵. خطای اعتبارسنجی (Validation Error)

شکست در اعتبارسنجی موارد زیر را بازمی‌گرداند:

کد وضعیت HTTP:

```text
400 Bad Request
```

مثال:

```json
{
  "errorCode": "VAL-001",
  "title": "Validation Failed",
  "message": "Request validation failed.",
  "correlationId": "4af21f0c-37cb-45f3-bc90-0185d0fb5d74",
  "details": [
    {
      "field": "assetNumber",
      "message": "Asset Number is required."
    },
    {
      "field": "engineId",
      "message": "Engine does not exist."
    }
  ]
}
```

---

# ۶. خطای قوانین کسب‌وکار (Business Rule Error)

نقض قوانین کسب‌وکار موارد زیر را بازمی‌گرداند:

```text
409 Conflict
```

مثال:

```json
{
  "errorCode": "BUS-014",
  "title": "Business Rule Violation",
  "message": "The selected engine is already installed.",
  "correlationId": "93db2b73-1b0f-4528-9a87-79d1b8c26bb4"
}
```

---

# ۷. خطای احراز هویت (Authentication Error)

عدم موفقیت در احراز هویت موارد زیر را بازمی‌گرداند:

```text
401 Unauthorized
```

مثال:

```json
{
  "errorCode": "AUTH-001",
  "title": "Authentication Failed",
  "message": "Authentication is required.",
  "correlationId": "dbe6e9db-cdb7-42b0-a92b-6a7efbb78dd3"
}
```

---

# ۸. خطای مجوزدهی (Authorization Error)

عدم دسترسی و عدم کفایت مجوزها موارد زیر را بازمی‌گرداند:

```text
403 Forbidden
```

مثال:

```json
{
  "errorCode": "AUTH-003",
  "title": "Access Denied",
  "message": "You do not have permission to perform this operation.",
  "correlationId": "c17d18d9-2930-4df2-a5e4-84fc6d9dc33d"
}
```

---

# ۹. منبع یافت نشد (Resource Not Found)

عدم وجود منبع درخواستی موارد زیر را بازمی‌گرداند:

```text
404 Not Found
```

مثال:

```json
{
  "errorCode": "RES-001",
  "title": "Resource Not Found",
  "message": "The requested asset does not exist.",
  "correlationId": "bd2fb65e-f948-4cb8-8f54-5cb1dc8ff6b4"
}
```

---

# ۱۰. خطای زیرساخت (Infrastructure Error)

شکست موقت زیرساخت موارد زیر را بازمی‌گرداند:

```text
503 Service Unavailable
```

مثال:

```json
{
  "errorCode": "INF-008",
  "title": "Service Unavailable",
  "message": "The requested service is temporarily unavailable.",
  "correlationId": "6dc8b5d8-7d8d-43c0-b74d-88d4fc50d68f"
}
```

---

# ۱۱. خطای غیرمنتظره (Unexpected Error)

خطاها و شکست‌های غیرمنتظره موارد زیر را بازمی‌گردانند:

```text
500 Internal Server Error
```

مثال:

```json
{
  "errorCode": "SYS-001",
  "title": "Unexpected Error",
  "message": "An unexpected error occurred.",
  "correlationId": "5d773fc3-84bc-4214-a72c-a19a3b3d92ff"
}
```

جزئیات پیاده‌سازی داخلی هرگز نباید افشا شوند.

---

# ۱۲. شناسه همبستگی (Correlation Identifier)

هر پاسخ خطا باید شامل یک شناسه همبستگی (Correlation Id) باشد.

همین شناسه باید در موارد زیر نیز درج شود:

- لاگ‌های برنامه (Application logs)
- رکوردهای ممیزی (Audit records)
- ردگیری‌های توزیع‌شده (Distributed traces)
- کارهای پس‌زمینه (Background jobs)

این امر ردگیری کامل درخواست‌ها را امکان‌پذیر می‌سازد.

---

# ۱۳. بومی‌سازی (Localization)

نسخه اولیه API پیام‌ها را به یک زبان بازمی‌گرداند.

نسخه‌های آتی ممکن است موارد زیر را بومی‌سازی کنند:

- عنوان (title)
- پیام (message)
- جزئیات اعتبارسنجی (validation details)

کدهای خطا هرگز نباید به دلیل بومی‌سازی تغییر کنند.

---

# ۱۴. سازگاری با نسخه‌های قبلی (Backward Compatibility)

فیلدهای موجود در پاسخ خطا باید پایدار بمانند.

نسخه‌های آینده ممکن است فیلدهای اختیاری جدیدی اضافه کنند.

فیلدهای موجود هرگز نباید مفهوم معنایی خود را تغییر دهند.

---

# جدول نگاشت HTTP به دسته‌بندی خطاها

| کد HTTP | دسته‌بندی |
| ---- | -------------------------------- |
| 400 | اعتبارسنجی (Validation) |
| 401 | احراز هویت (Authentication) |
| 403 | مجوزدهی (Authorization) |
| 404 | منبع (Resource) |
| 409 | کسب‌وکار (Business) |
| 422 | اعتبارسنجی معنایی *(رزرو شده)* |
| 500 | سیستم (System) |
| 503 | زیرساخت (Infrastructure) |

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
- `03-RequestResponseModel.md`
- `docs/05-development/07-ErrorHandling.md`
- ADR-0005 — استراتژی API

---

# تاریخچه تغییرات

| نسخه | تاریخ | شرح |
|----------|------------|----------------------------------------------|
| 1.0.0 | اولیه | مشخصات اولیه پاسخ‌های خطا |
| 3.0.0 | 2026-07-18 | استانداردسازی طبق استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
