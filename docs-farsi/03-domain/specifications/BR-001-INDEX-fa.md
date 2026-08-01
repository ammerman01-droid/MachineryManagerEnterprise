# شاخص مشخصات کسب‌وکار (Business Specification Index)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | BR-INDEX |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-20 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف

این سند شاخص اصلی تمامی مشخصات کسب‌وکار (Business Specifications) در پلتفرم MachineryManagerEnterprise است.

هر قابلیت کسب‌وکاری کشف‌شده در فاز کشف دامنه (Domain Discovery) در نهایت توسط یک سند مشخصات کسب‌وکار بازنمایی می‌شود.

این شاخص مرجع واحدی برای پیگیری موارد زیر ارائه می‌دهد:

- قابلیت‌های کسب‌وکار
- پیشرفت تحلیل
- وضعیت مشخصات
- آمادگی برای پیاده‌سازی

---

# ۲. چرخه حیات

هر قابلیت کسب‌وکاری از چرخه حیات زیر پیروی می‌کند:

```text
کشف دامنه (Domain Discovery)

↓

مشخصات کسب‌وکار (Business Specification)

↓

مدل دامنه (Domain Model)

↓

طراحی برنامه (Application Design)

↓

پیاده‌سازی (Implementation)

↓

تست (Testing)

↓

محیط عملیاتی (Production)
```

هیچ قابلیت کسب‌وکاری نباید هیچ مرحله‌ای را حذف کند.

---

# ۳. تعاریف وضعیت‌ها

| وضعیت | شرح |
|----------|-------------|
| برنامه‌ریزی‌شده (Planned) | شناسایی‌شده اما نوشتن مشخصات هنوز آغاز نشده است |
| پیش‌نویس (Draft) | مشخصات کسب‌وکار در حال نگارش است |
| در حال بررسی (Under Review) | در انتظار بررسی کسب‌وکاری یا معماری |
| تاییدشده (Approved) | مشخصات کسب‌وکار تایید شده است |
| مدل‌سازی‌شده (Modeled) | مدل دامنه تکمیل شده است |
| پیاده‌سازی‌شده (Implemented) | قابلیت به طور کامل پیاده‌سازی شده است |

---

# ۴. کاتالوگ مشخصات کسب‌وکار

| شناسه | قابلیت کسب‌وکار | کشف دامنه | سند مشخصات | پیش‌نیازها | اولویت | وضعیت |
| ---------- | ----------------------------------- | ---------- | --------------------------------------------------------------------| ---------------| -------- | --------|
| BR-003 | روابط دارایی‌ها (Asset Relationships) | DD-002 | BR-003-BusinessSpecification-AssetRelationships.md | — | بالا | برنامه‌ریزی‌شده |
| BR-004 | قطعات دارای کدردیابی (Tracked Components) | DD-003 | BR-004-BusinessSpecification-TrackedComponents.md | BR-001 | بالا | پیش‌نویس |
| BR-005 | مدیریت چرخه حیات تایرها (Tire Lifecycle Management) | DD-004 | BR-005-BusinessSpecification-TireLifecycle.md | BR-002 | بالا | برنامه‌ریزی‌شده |
| BR-006 | مدیریت چرخه حیات باتری (Battery Lifecycle Management) | DD-005 | BR-006-BusinessSpecification-BatteryLifecycle.md | BR-002 | بالا | برنامه‌ریزی‌شده |
| BR-007 | کاتالوگ قطعات (Parts Catalog) | DD-006 | BR-007-BusinessSpecification-PartsCatalog.md | — | بالا | برنامه‌ریزی‌شده |
| BR-008 | ارجاع متقابل قطعات (Part Cross Reference) | DD-007 | BR-008-BusinessSpecification-PartCrossReference.md | BR-005 | بالا | برنامه‌ریزی‌شده |
| BR-009 | مدیریت حوادث و ایمنی (Incident Management) | DD-008 | BR-009-BusinessSpecification-IncidentManagement.md | BR-010 | بالا | برنامه‌ریزی‌شده |
| BR-010 | پیش‌بینی نت (Maintenance Forecast) | DD-009 | BR-010-BusinessSpecification-MaintenanceForecast.md | BR-010 | بالا | برنامه‌ریزی‌شده |
| BR-011 | عملیات نت (Maintenance Operations) | DD-010 | BR-011-BusinessSpecification-MaintenanceOperations.md | BR-001, BR-002 | بالا | برنامه‌ریزی‌شده |
| BR-012 | مرکز اعلان‌ها (Notification Center) | DD-011 | BR-012-BusinessSpecification-NotificationCenter.md | — | بالا | برنامه‌ریزی‌شده |
| BR-013 | پیام‌رسانی داخلی (Internal Messaging) | DD-012 | BR-013-BusinessSpecification-InternalMessaging.md | — | متوسط | برنامه‌ریزی‌شده |
| BR-014 | دستیار هوش مصنوعی (AI Assistant) | DD-013 | BR-014-BusinessSpecification-AIAssistant.md | — | متوسط | برنامه‌ریزی‌شده |
| BR-015 | مدیریت روابط (Relationship Management) | DD-015 | BR-015-BusinessSpecification-RelationshipManagement.md | BR-001 | بالا | برنامه‌ریزی‌شده |
| BR-016 | همگام‌سازی توزیع‌شده محیط کاری (Distributed Workspace Sync) | DD-015 | BR-016-BusinessSpecification-DistributedWorkspaceSynchronization.md | BR-001 | بالا | برنامه‌ریزی‌شده |

---

# ۵. ترتیب اولویت پیاده‌سازی

ترتیب پیشنهادی پیاده‌سازی به شرح زیر است:

1. روابط دارایی‌ها (Asset Relationships)
2. قطعات دارای کدردیابی (Tracked Components)
3. مدیریت چرخه حیات تایرها (Tire Lifecycle Management)
4. مدیریت چرخه حیات باتری (Battery Lifecycle Management)
5. کاتالوگ قطعات (Parts Catalog)
6. ارجاع متقابل قطعات (Part Cross Reference)
7. عملیات نت (Maintenance Operations)
8. پیش‌بینی نت (Maintenance Forecast)
9. مدیریت حوادث (Incident Management)
10. مرکز اعلان‌ها (Notification Center)
11. پیام‌رسانی داخلی (Internal Messaging)
12. دستیار هوش مصنوعی (AI Assistant)
13. مدیریت روابط (Relationship Management)

این ترتیب منعکس‌کننده وابستگی‌های معماری است و بازطراحی‌های آتی را به حداقل می‌رساند.

---

# ۶. قوانین نگهداری

- هر قابلیت کشف‌شده باید به این شاخص اضافه شود.
- هر مشخصات کسب‌وکاری باید دارای یک شناسه BR منحصربه‌فرد باشد.
- وضعیت هر قابلیت باید با پیشرفت آن به‌روزرسانی شود.
- تغییر در اولویت یا ترتیب پیاده‌سازی مستلزم بررسی معماری است.
- هر مشخصات کسب‌وکاری باید صریحاً مشخصات پیش‌نیاز خود را اعلام کند.
- مفاهیم عرضی دامنه باید در اسناد اصول دامنه یا حاکمیت دامنه مستند شوند مگر اینکه نیازمند رفتار کسب‌وکاری مستقل باشند.

---

# ۷. اسناد مرتبط

- `10-DomainDiscovery-fa.md`
- `BR-002-BusinessSpecificationTemplate-fa.md`
- `01-DomainPrinciples-fa.md`
- `02-CoreConcepts-fa.md`
- `03-BoundedContexts-fa.md`
- `01-Architecture-fa.md`

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# تاریخچه تغییرات

| نسخه | تاریخ | نویسنده / نقش | شرح |
|---------|------------|---------------------|--------------------------------------|
| 1.0.0 | 2026-07-20 | معمار راهکار | شاخص مشخصات کسب‌وکار اولیه |
| 1.1.0 | 2026-07-20 | معمار راهکار | افزودن BR-002 قطعات دارای کدردیابی و شماره‌گذاری مجدد مشخصات بعدی |
| 1.2.0 | 2026-07-20 | معمار راهکار | افزودن پیگیری وابستگی‌ها و معرفی عملیات نت به عنوان مشخصات کسب‌وکار پایه |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
