| ویژگی | مقدار |
|---|---|
| **شناسه سند** | API-004 |
| **عنوان** | پاسخ‌های خطا (Error Responses) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند پاسخ‌های خطای استاندارد را برای MachineryManagerEnterprise تعریف می‌کند.

تمامی پاسخ‌های خطای HTTP باید از استاندارد RFC 7807 Problem Details پیروی کنند.

---

# ۲. استاندارد جزئیات مشکل (Problem Details Standard)

پاسخ‌های خطا باید از نوع محتوای (Content-Type) زیر استفاده کنند:

```
application/problem+json
```

ساختار:

```json
{
  "type": "https://errors.machinerymanager.com/validation-error",
  "title": "Validation Failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/api/v1/assets",
  "errors": {
    "Name": ["Name is required."]
  }
}
```

---

# ۳. کدهای خطای HTTP (HTTP Error Codes)

| کد | عنوان | کاربرد |
|---|---|---|
| 400 | درخواست نامعتبر (Bad Request) | خطاهای اعتبارسنجی، بدنه نامعتبر درخواست |
| 401 | احراز هویت‌نشده (Unauthorized) | توکن احراز هویت مفقود یا نامعتبر |
| 403 | ممنوع (Forbidden) | کاربر فاقد مجوز لازم است |
| 404 | یافت نشد (Not Found) | منبع وجود ندارد |
| 409 | تعارض (Conflict) | نقض قوانین کسب‌وکار یا خطای هم‌زمانی |
| 500 | خطای داخلی (Internal Error) | استثنای مدیریت‌نشده سرور |

---

# ۴. پاسخ خطای اعتبارسنجی (Validation Error Response - 400)

```json
{
  "type": "https://errors.machinerymanager.com/validation-error",
  "title": "Validation Error",
  "status": 400,
  "detail": "Input validation failed.",
  "errors": {
    "serialNumber": [
      "Serial number already exists."
    ]
  }
}
```

---

# ۵. پاسخ یافت نشد (Not Found Response - 404)

```json
{
  "type": "https://errors.machinerymanager.com/not-found",
  "title": "Resource Not Found",
  "status": 404,
  "detail": "Asset with ID '123' was not found."
}
```

---

# ۶. پاسخ تعارض (Conflict Response - 409)

```json
{
  "type": "https://errors.machinerymanager.com/conflict",
  "title": "Business Rule Violation",
  "status": 409,
  "detail": "Cannot delete asset because it is currently assigned to a project."
}
```

---

# ۷. خطای داخلی سرور (Internal Server Error - 500)

استثناهای داخلی سرور هرگز نباید ردپای پشته (Stack Traces) را در محیط‌های غیر از محیط توسعه افشا کنند.

```json
{
  "type": "https://errors.machinerymanager.com/internal-error",
  "title": "Internal Server Error",
  "status": 500,
  "detail": "An unexpected error occurred. Reference ID: 9A8B7C"
}
```

---

# ۸. پیاده‌سازی (Implementation)

مدیریت خطا باید به‌صورت متمرکز با استفاده از میان‌افزار (Middleware) یا هندلرهای استثنا در ASP.NET Core پیاده‌سازی شود.

---

# خلاصه تصمیمات (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به محیط ابری (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط (Related Documents)

- 01-RestConventions.md
- 02-EndpointDesign.md
- 03-RequestResponseModel.md
- ADR-0035 — معماری مستندسازی API و تولید کلاینت (API Documentation and Client Generation Architecture)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | پاسخ‌های اولیه خطا |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی طبق استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | اصلاح ارجاع از سند ناموجود «ADR-0005 — استراتژی API» به ADR-0035 حاکم اصلی (معماری مستندسازی API و تولید کلاینت) |
