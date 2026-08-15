| ویژگی | مقدار |
|---|---|
| **شناسه سند** | API-002 |
| **عنوان** | طراحی اندپوینت (Endpoint Design) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند استانداردهای طراحی اندپوینت را برای MachineryManagerEnterprise تعریف می‌کند.

تمامی اندپوینت‌های HTTP باید با این استانداردها مطابقت داشته باشند.

---

# ۲. ساختار پایه اندپوینت (Base Endpoint Structure)

اندپوینت‌های عمومی باید از ساختار زیر استفاده کنند:

```
/api/v1/{resource}
```

مثال‌ها:

```
/api/v1/assets

/api/v1/engines

/api/v1/maintenance-orders
```

---

# ۳. اندپوینت‌های استاندارد (Standard Endpoints)

هر موجودیت اصلی کسب‌وکار می‌تواند اندپوینت‌های زیر را ارائه دهد:

| کنش | متد HTTP | اندپوینت | کد پاسخ |
|---|---|---|---|
| فهرست (List) | GET | `/api/v1/assets` | 200 OK |
| دریافت با شناسه (Get by ID) | GET | `/api/v1/assets/{id}` | 200 OK |
| ایجاد (Create) | POST | `/api/v1/assets` | 201 Created |
| جایگزینی (Replace) | PUT | `/api/v1/assets/{id}` | 200 OK |
| به‌روزرسانی (Update) | PATCH | `/api/v1/assets/{id}` | 200 OK |
| حذف (Delete) | DELETE | `/api/v1/assets/{id}` | 204 No Content |

---

# ۴. اندپوینت‌های کنش (Action Endpoints)

هنگامی که یک عملیات به‌جای جهش منبع (Resource Mutation)، نمایانگر یک گردش کار کسب‌وکار باشد:

مثال:

```
POST /api/v1/assets/{id}/retire
```

مثال:

```
POST /api/v1/engines/{id}/rebuild
```

اندپوینت‌های کنش باید از متد POST استفاده کنند.

---

# ۵. اندپوینت‌های پرس‌وجو (Query Endpoints)

پرس‌وجوهای جستجو یا گزارش‌گیری با پارامترهای پیچیده می‌توانند از متد POST استفاده کنند.

مثال:

```
POST /api/v1/assets/search
```

این امر از ایجاد رشته‌های پرس‌وجوی (Query Strings) طولانی و ناخوانا در URI جلوگیری می‌کند.

---

# ۶. اندپوینت‌های دسته‌ای (Bulk Endpoints)

عملیات دسته‌ای باید از اندپوینت‌های اختصاصی استفاده کنند.

مثال‌ها:

```
POST /api/v1/assets/bulk-create

POST /api/v1/assets/bulk-delete
```

درخواست‌های دسته‌ای باید پیش از پردازش، تمام اقلام را اعتبارسنجی کنند.

---

# ۷. فرمت‌های پاسخ (Response Formats)

خواندن تکی (Read Single):

```json
{
  "id": "guid",
  "name": "Engine ABC",
  "status": "Active"
}
```

خواندن فهرست (Read List):

```json
{
  "items": [],
  "totalCount": 100,
  "page": 1,
  "pageSize": 25
}
```

پاسخ ایجاد (Creation Response):

```json
{
  "id": "guid"
}
```

کد وضعیت HTTP برای ایجاد:

```
201 Created
```

هدر (Header):

```
Location: /api/v1/assets/{id}
```

---

# ۸. پاسخ‌های خطا (Error Responses)

خطاها باید از اشیای استاندارد خطا استفاده کنند.

مثال:

```json
{
  "type": "https://errors.machinerymanager.com/validation-error",
  "title": "Validation Failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "errors": {
    "Name": ["Name is required."]
  }
}
```

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
- 03-RequestResponseModel.md
- 04-ErrorResponses.md
- ADR-0035 — معماری مستندسازی API و تولید کلاینت (API Documentation and Client Generation Architecture)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | طراحی اولیه اندپوینت |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی طبق استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | اصلاح ارجاع از سند ناموجود «ADR-0005 — استراتژی API» به ADR-0035 حاکم اصلی (معماری مستندسازی API و تولید کلاینت) |
