| ویژگی | مقدار |
|---|---|
| **شناسه سند** | API-007 |
| **عنوان** | احراز هویت و مجوزدهی API (API Authentication & Authorization) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند استانداردهای احراز هویت و مجوزدهی را برای APIهای MachineryManagerEnterprise تعریف می‌کند.

---

# ۲. استاندارد احراز هویت (Authentication Standard)

APIها باید درخواست‌ها را با استفاده از توکن‌های JWT مبتنی بر OAuth 2.0 / OpenID Connect احراز هویت کنند.

فرمت هدر (Header Format):

```
Authorization: Bearer <JWT_TOKEN>
```

تصمیمات ارائه‌دهنده هویت در **ADR-0030 (معماری مدیریت هویت و دسترسی - Identity and Access Management Architecture)** تعریف شده‌اند.

---

# ۳. استاندارد مجوزدهی (Authorization Standard)

مجوزدهی باید با استفاده از روش‌های زیر اعمال شود:

- کنترل دسترسی مبتنی بر نقش (RBAC)
- کنترل دسترسی مبتنی بر دسترسی/مجوز (PBAC)

نمونه ادعای دسترسی (Permission Claim):

```
permissions: ["asset:read", "asset:create"]
```

---

# ۴. کدهای وضعیت HTTP (HTTP Status Codes)

- **401 Unauthorized**: توکن مفقود یا نامعتبر است.
- **403 Forbidden**: توکن معتبر است، اما مجوز رد شده است.

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
- 04-ErrorResponses.md
- ADR-0030 — معماری مدیریت هویت و دسترسی (Identity and Access Management Architecture)
- ADR-0035 — معماری مستندسازی API و تولید کلاینت (API Documentation and Client Generation Architecture)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | احراز هویت و مجوزدهی اولیه |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی طبق استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | جایگزینی استناد نادرست به ADR-0026 با تصمیم حاکم هویت واقعی ADR-0030 (معماری مدیریت هویت و دسترسی)؛ اصلاح ارجاع از سند ناموجود «ADR-0005 — استراتژی API» به ADR-0035 |
