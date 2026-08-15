| ویژگی | مقدار |
|---|---|
| **شناسه سند** | DOM-INDEX |
| **عنوان** | نمایه مستندات دامنه (DomainDocumentationIndex) |
| **نسخه** | 4.2.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند، نمایه جامع و اصلی تمامی مستندات دامنه (Domain) را در پروژه MachineryManagerEnterprise ارائه می‌دهد.

این سند موارد زیر را تعریف می‌نماید:

- سلسله‌مراتب مستندسازی،
- نقش و کارکرد هر سند،
- وضعیت تکمیل جاری،
- مستندات برنامه‌ریزی‌شده،
- قابلیت ردگیری میان مستندات.

این سند، نقطه ورود اصلی برای ناوبری و دسترسی به مستندات دامنه است.

تمامی توسعه‌دهندگان، معماران و دستیاران هوش مصنوعی باید پیش از اتخاذ هرگونه تصمیم مرتبط با کسب‌وکار، از این سند شروع کنند.

---

# ۱-الف. ترتیب مطالعه (Reading Order)

ترتیب پیشنهادی برای مطالعه به شرح زیر است:

```text
DomainDocumentationIndex (نمایه مستندات دامنه)

↓

00-Glossary (واژه‌نامه)

↓

01-DomainPrinciples (اصول دامنه)

↓

DG-00-DomainGovernance (حاکمیت دامنه)

↓

10-DomainDiscovery (اکتشاف دامنه)

↓

Business Specifications (مشخصات تجاری)

↓

Domain Model (مدل دامنه)

↓

Implementation (پیاده‌سازی)
```

اگر به تازگی به پروژه پیوسته‌اید، اسناد زیر را به ترتیب مطالعه کنید:

۱. `DomainDocumentationIndex.md` (همین سند)
۲. `00-Glossary.md`
۳. `01-DomainPrinciples.md`
۴. `DG-00-DomainGovernance.md`
۵. `10-DomainDiscovery.md`
۶. `specifications/BR-001-INDEX.md`
۷. مشخصات تجاری مرتبط (The relevant Business Specification)

تنها پس از درک عمیق کسب‌وکار، فرایند پیاده‌سازی باید آغاز شود.

---

# ۱-ب. ساختار پوشه‌ها (Directory Structure)

```text
03-domain/

DomainDocumentationIndex.md

00-Glossary.md

01-DomainPrinciples.md

DG-00-DomainGovernance.md

02-CoreConcepts.md

03-BoundedContexts.md

04-DomainModel.md

05-Aggregates.md

06-DomainServices.md

07-DomainEvents.md

08-BusinessRules.md

09-StateMachines.md

10-DomainDiscovery.md

11-UbiquitousLanguage.md

12-DomainPatterns.md

specifications/
```

---

# ۲. معماری مستندات دامنه (Domain Documentation Architecture)

مستندات دامنه از چرخه حیات زیر پیروی می‌کنند:

```text
چشم‌انداز (Vision)

↓

اصول دامنه (Domain Principles)

↓

حاکمیت دامنه (Domain Governance)

↓

مدل قابلیت‌ها (Capability Model)

↓

اکتشاف دامنه (Domain Discovery)

↓

مشخصات تجاری (Business Specifications)

↓

مدل دامنه (Domain Model)

↓

پیاده‌سازی (Implementation)
```

هر لایه به لایه پیشین خود وابسته است.

---

# ۳. لایه‌های مستندسازی (Documentation Layers)

| لایه | هدف |
|---|---|
| اصول (Principles) | تعریف قواعد تجاری اساسی و بنیادی |
| حاکمیت (Governance) | تعریف فرایند مهندسی دامنه |
| اکتشاف (Discovery) | ثبت قابلیت‌های تجاری |
| مشخصات (Specification) | توصیف رفتار تجاری |
| مدل دامنه (Domain Model) | مدل‌سازی کسب‌وکار |
| پیاده‌سازی (Implementation) | ساخت و تولید نرم‌افزار |

---

# ۴. کاتالوگ اسناد (Document Catalog)

## اسناد بنیادی (Foundation)

| سند | وضعیت | هدف |
|---|---|---|
| 00-Glossary.md | کامل (Complete) | مرجع زبان فراگیر و مشترک (Ubiquitous language) |
| 01-DomainPrinciples.md | کامل (Complete) | قواعد بنیادی حاکم بر دامنه کسب‌وکار |
| DG-00-DomainGovernance.md | کامل (Complete) | چرخه حیات دامنه و فرایند حاکمیت |

---

## اسناد اکتشاف (Discovery)

| سند | وضعیت | هدف |
|---|---|---|
| 10-DomainDiscovery.md | فعال (Active) | رجیستری قابلیت‌های تجاری کشف‌شده |

---

## مشخصات تجاری (Specifications)

مکان:

```text
docs/03-domain/specifications/
```

| سند | وضعیت | هدف |
|---|---|---|
| BR-001-INDEX.md | فعال (Active) | رجیستری تمامی مشخصات تجاری — منبع معتبر برای وضعیت مشخصات |
| BR-002-BusinessSpecificationTemplate.md | کامل (Complete) | قالب استاندارد برای تمامی مشخصات تجاری |

> **توجه:** برای مشاهده فهرست رسمی، معتبر و جاری و وضعیت هر یک از مشخصات تجاری، به `BR-001-INDEX.md` مراجعه فرمایید؛ اطلاعات در این سند تکرار نشده است.

---

## پوشه مشخصات تجاری (Business Specification Directory)

تمامی مشخصات تجاری در مسیر زیر ذخیره شده‌اند:

```text
specifications/
```

ساختار جاری:

```text
specifications/

BR-001-INDEX.md

BR-002-BusinessSpecificationTemplate.md

BR-003-BusinessSpecification-AssetRelationships.md

BR-004-BusinessSpecification-TrackedComponents.md

BR-005-BusinessSpecification-TireLifecycle.md

BR-006-BusinessSpecification-BatteryLifecycle.md

BR-007-BusinessSpecification-PartsCatalog.md

BR-008-BusinessSpecification-PartCrossReference.md

BR-009-BusinessSpecification-IncidentManagement.md

BR-010-BusinessSpecification-MaintenanceForecast.md

BR-011-BusinessSpecification-MaintenanceOperations.md

BR-012-BusinessSpecification-NotificationCenter.md

BR-013-BusinessSpecification-InternalMessaging.md

BR-014-BusinessSpecification-AIAssistant.md

BR-015-BusinessSpecification-RelationshipManagement.md

BR-016-BusinessSpecification-DistributedWorkspaceSynchronization.md

BR-017-BusinessSpecification-OrganizationManagement.md
```

مشخصات آتی نیز در همین پوشه قرار خواهند گرفت. برای مشاهده وضعیت معتبر هر یک به `BR-001-INDEX.md` مراجعه نمایید.

---

## مدل‌سازی دامنه (Domain Modeling)

| سند | وضعیت | هدف |
|---|---|---|
| 04-DomainModel.md | موجود (Existing) | مدل ساختاری دامنه |
| 05-Aggregates.md | موجود (Existing) | تعاریف تجمیع‌ها (Aggregates) |
| 06-DomainServices.md | موجود (Existing) | سرویس‌های دامنه (Domain Services) |
| 07-DomainEvents.md | موجود (Existing) | رخدادهای دامنه (Domain Events) |
| 08-BusinessRules.md | موجود (Existing) | قواعد تجاری اتمیک (Atomic business rules) |
| 09-StateMachines.md | موجود (Existing) | ماشین‌های وضعیت چرخه حیات موجودیت‌ها |
| 12-DomainPatterns.md | موجود (Existing) | الگوهای طراحی دامنه با قابلیت استفاده مجدد |

---

# ۵. وضعیت جاری تکمیل (Current Completion Status)

| بخش | وضعیت |
|---|---|
| اصول دامنه (Domain Principles) | ✅ کامل (Complete) |
| حاکمیت دامنه (Domain Governance) | ✅ کامل (Complete) |
| اکتشاف دامنه (Domain Discovery) | ✅ فعال (Active) |
| قالب مشخصات تجاری | ✅ کامل (Complete) |
| مشخصات تجاری (۱۵ از ۱۵ پیش‌نویس شده) | ✅ جهت مشاهده وضعیت هر مشخصات به BR-001-INDEX.md مراجعه کنید |
| نمایه مستندات دامنه | ✅ کامل (Complete) |

---

# ۶. مشخصات تجاری برنامه‌ریزی‌شده (Planned Business Specifications)

> **توجه:** برای مشاهده فهرست جاری، اولویت‌ها و وضعیت هر یک از مشخصات تجاری، به `BR-001-INDEX.md` مراجعه فرمایید؛ موارد در این سند تکرار نشده است.

---

# ۷. ردیابی‌پذیری (Traceability)

هر ویژگی پیاده‌سازی‌شده باید از طریق زنجیره زیر قابل ردگیری باشد:

```text
چشم‌انداز (Vision)

↓

مدل قابلیت‌ها (Capability Model)

↓

اکتشاف دامنه (Domain Discovery)

↓

مشخصات تجاری (Business Specification)

↓

مدل دامنه (Domain Model)

↓

لایه کاربردی (Application Layer)

↓

پیاده‌سازی (Implementation)

↓

آزمون (Testing)

↓

انتشار (Release)
```

هیچ پیاده‌سازی نباید این زنجیره را دور بزند.

---

# ۸. قواعد حاکمیتی (Governance Rules)

تمامی مستندات دامنه باید منطبق با موارد زیر باشند:

- اصول دامنه (Domain Principles)
- حاکمیت دامنه (Domain Governance)
- قرارداد مهندسی هوش مصنوعی (AI Engineering Contract)
- استانداردهای مستندسازی (Documentation Standards)

مشخصات تجاری همواره باید از روی قالب رسمی ایجاد شوند.

هیچ مشخصات تجاری پیش از اخذ تصویب نباید پیاده‌سازی شود.

تغییرات معماری نیازمند تصویب سند ADR هستند.

تغییرات تجاری نیازمند مشخصات تجاری به‌روزرسانی‌شده هستند.

---

# ۹. قواعد نگهداری (Maintenance Rules)

هر زمان که مشخصات تجاری جدیدی ایجاد شد:

- سند `BR-INDEX.md` را به‌روزرسانی کنید.
- سند `DomainDocumentationIndex.md` را به‌روزرسانی کنید.
- در صورت لزوم `DomainDiscovery.md` را به‌روزرسانی کنید.

هر زمان که وضعیت یک مشخصات تجاری تغییر کرد:

- برنامه‌ریزی‌شده (Planned) ← فعال (Active)
- فعال (Active) ← تصویب‌شده (Approved)
- تصویب‌شده (Approved) ← پیاده‌سازی‌شده (Implemented)

وضعیت باید در این سند نیز به‌روزرسانی گردد.

---

# ۱۰. اسناد مرتبط (Related Documents)

- 01-DomainPrinciples.md
- DG-00-DomainGovernance.md
- 10-DomainDiscovery.md
- specifications/BR-001-INDEX.md
- specifications/BR-002-BusinessSpecificationTemplate.md
- AI_ENGINEERING_CONTRACT.md

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | نمایه اولیه مستندات دامنه |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-02 | معمار راهکار | بازسازی بخش‌های کاتالوگ اسناد و مشخصات برنامه‌ریزی‌شده که به دلیل ارجاع به تک‌سند اولیه قدیمی شده بودند، ارجاع به BR-001-INDEX.md به عنوان مرجع واحد |
| 4.2.0 | 2026-08-08 | معمار راهکار | ادغام README.md در این سند — دو فایل دارای محتوای هم‌پوشان بودند که در طول زمان ناهماهنگ شده بودند. اکنون این سند تنها نقطه ورود مستندات 03-domain است |
