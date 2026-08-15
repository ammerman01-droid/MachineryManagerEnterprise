| ویژگی | مقدار |
|---|---|
| **شناسه سند** | MME-GUIDE-001 |
| **عنوان** | راهنمای مخزن پروژه (Repository Guide) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند سازمان‌دهی کلی مخزن، اصول توسعه و دستورالعمل‌های پیمایش و ناوبری را در پروژه تشریح می‌کند.

این سند به عنوان نقطه ورود توسعه‌دهندگان پس از مطالعه فایل اصلی `README` سامانه عمل می‌کند.

---

# ساختار مخزن پروژه (Repository Structure)

```text
Repository (مخزن پروژه)
│
├── README.md / README-fa.md
├── REPOSITORY_GUIDE.md / REPOSITORY_GUIDE-fa.md
├── PROJECT_CHARTER.md / PROJECT_CHARTER-fa.md
├── PROJECT_PROGRESS.md / PROJECT_PROGRESS-fa.md
├── DOCUMENTATION_REVIEW_CHECKLIST.md / DOCUMENTATION_REVIEW_CHECKLIST-fa.md
├── AI_ENGINEERING_CONTRACT.md / AI_ENGINEERING_CONTRACT-fa.md
│
├── docs-english/
├── docs-farsi/
├── src/
├── tests/
│
├── Directory.Build.props
├── Directory.Packages.props
├── MachineryManagerEnterprise.slnx
│
└── .github/
```

---

# مسئولیت پوشه‌ها (Folder Responsibilities)

## docs-english و docs-farsi

حاوی کلیه مستندات تفصیلی پروژه به زبان‌های انگلیسی و فارسی:

- **01-vision**: چشم‌انداز، نیازمندی‌ها و زبان فراگیر (Ubiquitous Language)
- **02-architecture**: معماری کلان و ارزیابی‌های فناوری (TE-0001 تا TE-0036)
- **03-domain**: مدل دامنه، طراحی اگریگیت‌ها و کاتالوگ رویدادها
- **04-modules**: مشخصات ماژول‌های دامنه‌ای و ساختار کاتالوگ ماژول‌ها
- **05-development**: ساختار سلوشن، استانداردهای کدنویسی، پایپ‌لاین ساخت و قواعد وابستگی
- **06-decisions**: سوابق تصمیمات معماری (ADR-0001 تا ADR-0025)
- **07-api**: استانداردهای طراحی و مستندسازی API
- **08-releases**: راهبرد و برنامه‌ریزی انتشار نسخه‌ها
- **09-proof-of-concepts**: مستندات اثبات مفاهیم فنی (PoC)

---

## src

شامل کلیه سورس‌کدهای اصلی سامانه:

- **Host**: برنامه‌های اجرایی و میزبان‌ها (API Server, Web App)
- **Modules**: ماژول‌های تجاری مستقل
- **BuildingBlocks**: بلوک‌های سازنده مشترک، SharedKernel و زیرساخت‌های پایه

---

## tests

شامل تمامی آزمون‌های خودکار نرم‌افزار:

- Unit Tests (تست‌های واحد)
- Integration Tests (تست‌های یکپارچگی)
- Architecture Tests (تست‌های انطباق معماری)
- UI / E2E Tests (تست‌های سرتاسری)

---

## .github

شامل اتوماسیون مخزن، اکشن‌ها و ورک‌فلوهای CI/CD.

---

# اسناد ریشه پروژه (Root Documents)

| سند | موضوع و مسئولیت |
|---|---|
| **README** | معرفی عمومی و کلان پروژه |
| **REPOSITORY_GUIDE** | راهنمای سازمان‌دهی و پیمایش مخزن |
| **PROJECT_CHARTER** | منشور پروژه، اهداف کلان و چشم‌انداز |
| **PROJECT_PROGRESS** | تاریخچه پیشرفت و نقاط عطف پروژه |
| **DOCUMENTATION_REVIEW_CHECKLIST** | چک‌لیست تضمین کیفیت و بازنگری مستندات |
| **AI_ENGINEERING_CONTRACT** | قرارداد مهندسی و الزامات همکاری دستیاران هوش مصنوعی |

---

# اصول راهبردی مخزن (Repository Principles)

- اولویت با مستندات (Documentation First)
- اولویت با معماری (Architecture First)
- معماری تمیز (Clean Architecture)
- یکپارچه ماژولار (Modular Monolith)
- طراحی دامنه-محور (Domain-Driven Design)
- اولویت با راه‌حل‌های متن‌باز (Open Source First)

---

# جریان مطالعه مستندات (Documentation Flow)

```text
README
  ↓
REPOSITORY_GUIDE
  ↓
PROJECT_CHARTER
  ↓
Documentation (docs-farsi / docs-english)
  ↓
Implementation (src)
```

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | نگارش اولیه راهنمای مخزن پروژه |
| 2.0.0 | 2026-07-18 | معمار راهکار | سازمان‌دهی مجدد ساختار مستندات |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
