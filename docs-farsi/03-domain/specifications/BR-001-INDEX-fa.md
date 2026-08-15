| ویژگی | مقدار |
|---|---|
| **شناسه سند** | BR-INDEX |
| **عنوان** | نمایه مشخصات کسب‌وکار (Business Specifications INDEX) |
| **نسخه** | 4.4.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-20 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند نمایه اصلی تمامی مشخصات کسب‌وکار (Business Specifications) در راهکار MachineryManagerEnterprise است.

هر قابلیت کسب‌وکار شناسایی‌شده در فاز اکتشاف دامنه (Domain Discovery) در نهایت توسط یک سند مشخصه کسب‌وکار بازنمایی خواهد شد.

این نمایه مکان واحدی را برای پیگیری موارد زیر فراهم می‌کند:

- قابلیت‌های کسب‌وکار
- پیشرفت تحلیل
- وضعیت مشخصات
- آمادگی برای پیاده‌سازی

---

# ۲. چرخه حیات (Lifecycle)

هر قابلیت کسب‌وکار باید از چرخه حیات زیر پیروی نماید:

```text
Domain Discovery (اکتشاف دامنه)

↓

Business Specification (مشخصات کسب‌وکار)

↓

Domain Model (مدل دامنه)

↓

Application Design (طراحی کاربردی)

↓

Implementation (پیاده‌سازی)

↓

Testing (آزمون)

↓

Production (محیط عملیاتی)
```

هیچ قابلیتی نباید از هیچ مرحله‌ای عبور کند یا آن را نادیده بگیرد.

---

# ۳. تعاریف وضعیت (Status Definitions)

| وضعیت (Status) | توصیف (Description) |
|---|---|
| Planned (برنامه‌ریزی‌شده) | شناسایی شده اما نگارش مشخصات هنوز آغاز نشده است |
| Draft (پیش‌نویس) | مشخصات کسب‌وکار در حال نگارش است |
| Under Review (تحت بررسی) | در انتظار بازبینی کسب‌وکار یا معماری |
| Approved (تصویب‌شده) | مشخصات کسب‌وکار تصویب شده است |
| Modeled (مدل‌سازی‌شده) | مدل دامنه تکمیل شده است |
| Implemented (پیاده‌سازی‌شده) | قابلیت به طور کامل پیاده‌سازی شده است |

---

# ۴. کاتالوگ مشخصات کسب‌وکار (Business Specification Catalog)

| شناسه | قابلیت کسب‌وکار | شناسه کشف | سند مشخصه | وابسته به | اولویت | وضعیت |
|---|---|---|---|---|---|---|
| BR-017 | Organization Management (مدیریت سازمان) | — | BR-017-BusinessSpecification-OrganizationManagement.md | — | بالا | Draft |
| BR-003 | Asset Relationships (روابط دارایی‌ها) | DD-002 | BR-003-BusinessSpecification-AssetRelationships.md | — | بالا | Planned |
| BR-004 | Tracked Components (مؤلفه‌های دارای قابلیت ردیابی) | DD-003 | BR-004-BusinessSpecification-TrackedComponents.md | BR-003 | بالا | Draft |
| BR-005 | Tire Lifecycle Management (مدیریت چرخه حیات لاستیک) | DD-004 | BR-005-BusinessSpecification-TireLifecycle.md | BR-004 | بالا | Planned |
| BR-006 | Battery Lifecycle Management (مدیریت چرخه حیات باتری) | DD-005 | BR-006-BusinessSpecification-BatteryLifecycle.md | BR-004 | بالا | Planned |
| BR-007 | Parts Catalog (کاتالوگ قطعات) | DD-006 | BR-007-BusinessSpecification-PartsCatalog.md | — | بالا | Planned |
| BR-008 | Part Cross Reference (ارجاع متقابل قطعات) | DD-007 | BR-008-BusinessSpecification-PartCrossReference.md | BR-007 | بالا | Planned |
| BR-009 | Incident Management (مدیریت حوادث و وقایع) | DD-008 | BR-009-BusinessSpecification-IncidentManagement.md | BR-012 | بالا | Planned |
| BR-010 | Maintenance Forecast (پیش‌بینی نگهداری) | DD-009 | BR-010-BusinessSpecification-MaintenanceForecast.md | BR-012 | بالا | Planned |
| BR-011 | Maintenance Operations (عملیات نگهداری و تعمیرات) | DD-010 | BR-011-BusinessSpecification-MaintenanceOperations.md | BR-003, BR-004 | بالا | Planned |
| BR-012 | Notification Center (مرکز اعلان‌ها) | DD-011 | BR-012-BusinessSpecification-NotificationCenter.md | — | بالا | Planned |
| BR-013 | Internal Messaging (پیام‌رسانی داخلی) | DD-012 | BR-013-BusinessSpecification-InternalMessaging.md | — | متوسط | Planned |
| BR-014 | AI Assistant (دستیار هوش مصنوعی) | DD-013 | BR-014-BusinessSpecification-AIAssistant.md | — | متوسط | Planned |
| BR-015 | Relationship Management (مدیریت روابط) | DD-015 | BR-015-BusinessSpecification-RelationshipManagement.md | BR-003 | بالا | Planned |
| BR-016 | DistributedWorkspaceSynchronization (همگام‌سازی توزیع‌شده فضای کاری) | — | BR-016-BusinessSpecification-DistributedWorkspaceSynchronization.md | BR-003 | بالا | Planned |

---

# ۵. ترتیب اولویت اجرایی (Priority Order)

توالی پیشنهادی پیاده‌سازی عبارت است از:

0. مدیریت سازمان (Organization Management)
1. روابط دارایی‌ها (Asset Relationships)
2. مؤلفه‌های دارای قابلیت ردیابی (Tracked Components)
3. مدیریت چرخه حیات لاستیک (Tire Lifecycle Management)
4. مدیریت چرخه حیات باتری (Battery Lifecycle Management)
5. کاتالوگ قطعات (Parts Catalog)
6. ارجاع متقابل قطعات (Part Cross Reference)
7. عملیات نگهداری و تعمیرات (Maintenance Operations)
8. پیش‌بینی نگهداری (Maintenance Forecast)
9. مدیریت حوادث و وقایع (Incident Management)
10. مرکز اعلان‌ها (Notification Center)
11. پیام‌رسانی داخلی (Internal Messaging)
12. دستیار هوش مصنوعی (AI Assistant)
13. مدیریت روابط (Relationship Management)

این ترتیب منعکس‌کننده وابستگی‌های معماری است و نیاز به بازطراحی‌های آینده را به حداقل می‌رساند.

مدیریت سازمان پیش از روابط دارایی‌ها قرار گرفته است زیرا سند `04-DomainModel.md` سازمان (Organization) را مالک تجاری دارایی‌ها تعریف می‌کند (`Organization → Owns → Assets`)؛ بنابراین انتساب مالکیت دارایی به وجود داشتن سازمان به عنوان یک قابلیت مدل‌سازی‌شده وابسته است.

---

# ۶. قواعد نگهداری مستندات (Maintenance Rules)

- هر قابلیت کشف‌شده باید به این نمایه افزوده شود.
- هر مشخصه کسب‌وکار باید دارای یک شناسه یکتای BR باشد.
- وضعیت هر قابلیت باید همگام با پیشرفت آن به‌روزرسانی گردد.
- هرگونه تغییر در اولویت یا توالی پیاده‌سازی نیازمند بازبینی معماری است.
- هر مشخصه کسب‌وکار باید مشخصات پیش‌نیاز خود را صراحتاً اعلام نماید.
- مفاهیم فرابخشی دامنه باید در اصول دامنه یا حاکمیت دامنه مستند شوند مگر اینکه نیازمند رفتارهای تجاری مستقل باشند.

---

# ۷. اسناد مرتبط (Related Documents)

- ../10-DomainDiscovery.md
- BR-002-BusinessSpecificationTemplate.md
- ../01-DomainPrinciples.md
- ../02-CoreConcepts.md
- ../03-BoundedContexts.md
- 01-Architecture.md
- CapabilityModel
- AI_ENGINEERING_CONTRACT.md
- REPOSITORY_GUIDE.md

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-20 | معمار راهکار | نمایه اولیه مشخصات کسب‌وکار |
| 1.1.0 | 2026-07-20 | معمار راهکار | افزودن BR-002 مؤلفه‌های دارای قابلیت ردیابی و بازشماری مشخصات بعدی |
| 1.2.0 | 2026-07-20 | معمار راهکار | افزودن ردیابی وابستگی‌ها و معرفی عملیات نگهداری به عنوان مشخصه ریشه‌ای کسب‌وکار |
| 3.0.0 | 2026-07-20 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-02 | معمار راهکار | افزودن BR-017 مدیریت سازمان (وضعیت: پیش‌نویس) و قرارگیری در اولویت ۰ پیش از روابط دارایی‌ها |
| 4.2.0 | 2026-08-02 | معمار راهکار | اصلاح فیلد متادیتای شناسه سند در تمام ۱۵ سند مشخصه (BR-003 تا BR-017) به گونه‌ای که با نام فایل همخوانی داشته باشد |
| 4.3.0 | 2026-08-02 | معمار راهکار | اصلاح شناسه تکراری DD-015 برای BR-015 و اصلاح مقادیر جدول وابستگی‌ها |
| 4.4.0 | 2026-08-08 | معمار راهکار | تطبیق و اصلاح شناسه‌های کشف بر اساس مرجع 10-DomainDiscovery.md |
