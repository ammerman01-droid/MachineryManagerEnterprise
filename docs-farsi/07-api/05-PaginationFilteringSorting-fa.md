| ویژگی | مقدار |
|---|---|
| **شناسه سند** | API-005 |
| **عنوان** | صفحه‌بندی، فیلترسازی و مرتب‌سازی (Pagination, Filtering, and Sorting) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند قراردادهای صفحه‌بندی، فیلترسازی و مرتب‌سازی را در سراسر APIهای MachineryManagerEnterprise تعریف می‌کند.

---

# ۲. صفحه‌بندی (Pagination)

اندپوینت‌هایی که فهرست‌ها را بازمی‌گردانند باید از صفحه‌بندی پشتیبانی کنند.

پارامترهای پرس‌وجو (Query Parameters):

```
page=1
pageSize=20
```

مقادیر پیش‌فرض:

- page = 1
- pageSize = 20
- maxPageSize = 100

---

# ۳. مدل پاسخ صفحه‌بندی (Pagination Response Model)

```json
{
  "items": [],
  "pageNumber": 1,
  "pageSize": 20,
  "totalPages": 5,
  "totalCount": 100
}
```

---

# ۴. مرتب‌سازی (Sorting)

مرتب‌سازی باید از پارامتر زیر استفاده کند:

```
sort
```

مثال:

```
GET /api/v1/assets?sort=name
```

ترتیب نزولی از علامت خط تیره (`-`) استفاده می‌کند:

```
GET /api/v1/assets?sort=-createdAt
```

چندین فیلد:

```
GET /api/v1/assets?sort=status,-createdAt
```

---

# ۵. فیلترسازی (Filtering)

فیلترسازی ساده از پارامترهای فیلد استفاده می‌کند.

مثال:

```
GET /api/v1/assets?status=Active&categoryId=10
```

---

# ۶. جستجو (Search)

جستجوی متن آزاد از پارامتر زیر استفاده می‌کند:

```
q
```

مثال:

```
GET /api/v1/assets?q=Caterpillar
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
- 02-EndpointDesign.md
- 03-RequestResponseModel.md
- ADR-0035 — معماری مستندسازی API و تولید کلاینت (API Documentation and Client Generation Architecture)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | صفحه‌بندی/فیلترسازی/مرتب‌سازی اولیه |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی طبق استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | اصلاح ارجاع از سند ناموجود «ADR-0005 — استراتژی API» به ADR-0035 حاکم اصلی (معماری مستندسازی API و تولید کلاینت) |
