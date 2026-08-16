| ویژگی | مقدار |
|---|---|
| **شناسه سند** | API-003 |
| **عنوان** | مدل‌های درخواست و پاسخ (Request/Response Models) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند استانداردهای مدل‌های درخواست و پاسخ را در سراسر APIهای MachineryManagerEnterprise تعریف می‌کند.

مدل‌های استانداردشده، سازگاری و قابلیت پیش‌بینی API را تضمین می‌نمایند.

---

# ۲. اصول عمومی (General Principles)

مدل‌ها باید:

- دارای نوع‌بندی قوی (Strongly Typed) باشند.
- از رکوردهای #C استفاده کنند (`record`).
- با استفاده از الگوی camelCase به JSON سریال‌سازی شوند.
- از افشای موجودیت‌های دامنه (Domain Entities) اجتناب کنند.
- از ارجاعات دایره‌ای (Circular References) اجتناب کنند.

---

# ۳. قراردادهای نام‌گذاری (Naming Conventions)

درخواست‌ها (Requests):

```
CreateAssetRequest

UpdateAssetRequest

GetAssetsQueryRequest
```

پاسخ‌ها (Responses):

```
AssetResponse

AssetListResponse

AssetSummaryResponse
```

---

# ۴. نمونه مدل درخواست (Request Model Example)

```csharp
public sealed record CreateAssetRequest(
    string Name,
    string SerialNumber,
    Guid CategoryId
);
```

درخواست‌ها تنها باید شامل داده‌های ضروری برای درخواست باشند.

---

# ۵. نمونه مدل پاسخ (Response Model Example)

```csharp
public sealed record AssetResponse(
    Guid Id,
    string Name,
    string SerialNumber,
    string Status,
    DateTime CreatedAt
);
```

پاسخ‌ها باید تمامی فیلدهای بازگردانده‌شده را به‌طور صریح تعریف کنند.

---

# ۶. مدل پاسخ مجموعه (Collection Response Model)

```csharp
public sealed record PagedResponse<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    long TotalCount
);
```

---

# ۷. تغییرناپذیری (Immutability)

مدل‌های درخواست و پاسخ باید تا حد امکان تغییرناپذیر (Immutable) باشند.

از رکوردهای مکانی (Positional Records) زبان #C استفاده کنید.

---

# ۸. مدیریت مقادیر تهی (Null Handling)

ویژگی‌های اختیاری باید قابلیت تهی‌پذیری (Nullable) داشته باشند.

سریال‌سازی JSON باید فیلدهای تهی (null) را در صورت نیازِ پیکربندی، حذف کند.

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
- 04-ErrorResponses.md
- ADR-0035 — معماری مستندسازی API و تولید کلاینت (API Documentation and Client Generation Architecture)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | مدل‌های اولیه درخواست و پاسخ |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی طبق استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | اصلاح ارجاع از سند ناموجود «ADR-0005 — استراتژی API» به ADR-0035 حاکم اصلی (معماری مستندسازی API و تولید کلاینت) |
