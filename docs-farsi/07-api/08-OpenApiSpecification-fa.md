| ویژگی | مقدار |
|---|---|
| **شناسه سند** | API-008 |
| **عنوان** | مشخصات OpenAPI (OpenAPI Specification) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند الزامات مشخصات OpenAPI (یا Swagger) را برای APIهای MachineryManagerEnterprise تعریف می‌کند.

---

# ۲. استاندارد (Standard)

تمامی APIهای REST باید یک سند مشخصات OpenAPI نسخه 3.x ارائه دهند.

محل دسترسی (Location):

```
/swagger/v1/swagger.json
```

رابط کاربری مستندات (Documentation UI):

```
/swagger
```

---

# ۳. الزامات (Requirements)

هر اندپوینت در OpenAPI باید شامل موارد زیر باشد:

- خلاصه و توضیحات (Summary & Description)
- پارامترهای درخواست و اسکیمای بدنه (Body Schema)
- تمامی کدهای پاسخ احتمالی HTTP
- الزامات احراز هویت

---

# ۴. ابزارها (Tooling)

- تولید مستندات: Swashbuckle / NSwag
- رابط کاربری: Scalar / Swagger UI

حاکمیت تحت سند **ADR-0035 (معماری مستندسازی API و تولید کلاینت - API Documentation and Client Generation Architecture)**.

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
- ADR-0035 — معماری مستندسازی API و تولید کلاینت (API Documentation and Client Generation Architecture)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | مشخصات اولیه OpenAPI |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی طبق استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | اصلاح ارجاع از سند ناموجود «ADR-0005 — استراتژی API» به ADR-0035 حاکم اصلی (معماری مستندسازی API و تولید کلاینت) |
