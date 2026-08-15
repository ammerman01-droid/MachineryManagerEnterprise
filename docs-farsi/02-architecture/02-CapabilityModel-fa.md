| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | ARCH-002 |
| **عنوان** | مدل قابلیت‌ها (Capability Model) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند مدل قابلیت‌های کسب‌وکار پلتفرم MachineryManagerEnterprise را تعریف می‌کند.

یک قابلیت توصیف می‌کند که **کسب‌وکار قادر به انجام چه کارهایی است**، مستقل از پیاده‌سازی فنی.

مدل قابلیت‌ها شالوده و اساسی است برای:

- طراحی دامنه‌محور (Domain Driven Design)
- مرزهای ماژول‌ها (Module Boundaries)
- ساختار ناوبری (Navigation Structure)
- سیستم مجوزها و دسترسی‌ها (Permission System)
- طراحی رابط برنامه‌نویسی (API Design)
- گزارش‌گیری (Reporting)
- استخراج میکروسرویس‌ها در آینده (Future Microservice Extraction)

این سند به صورت عمدی به جای مولفه‌های نرم‌افزاری بر قابلیت‌های کسب‌وکار تمرکز دارد.

---

# اصل هسته‌ای (Core Principle)

پلتفرم حول مفهوم **دارایی (Asset)** طراحی شده است، نه **ماشین (Machine)**.

هر شیء قابل نگهداری با چرخه حیات مستقل یک دارایی در نظر گرفته می‌شود.

مثال‌ها عبارتند از:

- ماشین‌آلات سنگین
- کامیون‌ها
- لیفتراک‌ها
- موتورها
- ادوات و تجهیزات جانبی الحاقی (Attachments)
- ژنراتورها
- کمپرسورها
- انواع تجهیزات در آینده

این اصل گسترش‌پذیری بلندمدت را تضمین می‌نماید.

---

# سلسله‌مراتب قابلیت‌ها (Capability Hierarchy)

```text
Enterprise Asset Lifecycle Platform (پلتفرم سازمانی چرخه حیات دارایی‌ها)

├── Organization Management (مدیریت سازمان‌ها)
│
├── Asset Management (مدیریت دارایی‌ها)
│   ├── Asset Registration (ثبت دارایی)
│   ├── Asset Classification (طبقه‌بندی دارایی)
│   ├── Asset Models (مدل‌های دارایی)
│   ├── Asset Specifications (مشخصات فنی دارایی)
│   ├── Asset Lifecycle (چرخه حیات دارایی)
│   ├── Asset Status (وضعیت دارایی)
│   ├── Asset Ownership (مالکیت دارایی)
│   ├── Asset Assignment (تخصیص دارایی)
│   └── Asset Retirement (اسقاط / خروج دارایی)
│
├── Component Management (مدیریت قطعات و اجزا)
│   ├── Engine Management (مدیریت موتور)
│   ├── Transmission Management (مدیریت گیربکس / سیستم انتقال قدرت)
│   ├── Attachment Management (مدیریت ادوات الحاقی)
│   ├── Replaceable Components (قطعات قابل تعویض)
│   ├── Component Installation (نصب قطعه)
│   ├── Component Removal (جداسازی / پیاده‌سازی قطعه)
│   ├── Component Transfer (انتقال قطعه)
│   ├── Component Rebuild (بازسازی قطعه)
│   └── Component History (تاریخچه قطعه)
│
├── Meter Management (مدیریت سنجه‌ها و کنتورها)
│   ├── Hour Meter (ساعت‌کار)
│   ├── Odometer (کیلومترشمار)
│   ├── Meter Replacement (تعویض کنتور)
│   ├── Operational Usage (کارکرد عملیاتی)
│   ├── Non-operational Usage (کارکرد غیرعملیاتی)
│   ├── Meter Validation (اعتبارسنجی کنتور)
│   └── Usage History (تاریخچه کارکرد)
│
├── Maintenance Management (مدیریت نگهداری و تعمیرات)
│   ├── Preventive Maintenance (نگهداری و تعمیرات پیشگیرانه)
│   ├── Corrective Maintenance (نگهداری و تعمیرات اصلاحی)
│   ├── Breakdown Management (مدیریت خرابی‌ها و توقفات اضطراری)
│   ├── Work Orders (دستورات کاری)
│   ├── Service Scheduling (زمان‌بندی سرویس‌ها)
│   ├── Maintenance History (تاریخچه نگهداری و تعمیرات)
│   └── Service Costs (هزینه‌های سرویس)
│
├── Fuel & Lubrication (سوخت و روانکارها)
│   ├── Fuel Consumption (مصرف سوخت)
│   ├── Engine Oil (روغن موتور)
│   ├── Hydraulic Oil (روغن هیدرولیک)
│   ├── Gear Oil (واسکازین / روغن دنده)
│   ├── Coolant (مایع خنک‌کننده / ضدیخ)
│   ├── Grease (گریس)
│   └── Consumption History (تاریخچه مصرف)
│
├── Spare Parts Management (مدیریت قطعات یدکی)
│   ├── Parts Catalog (کاتالوگ قطعات)
│   ├── Inventory (موجودی انبار)
│   ├── Stock Transactions (تراکنش‌های انبار / گردش کالا)
│   ├── Suppliers (تامین‌کنندگان)
│   ├── Purchase History (تاریخچه خرید)
│   └── Consumption History (تاریخچه مصرف)
│
├── Financial Management (مدیریت مالی)
│   ├── Purchase Information (اطلاعات خرید)
│   ├── Initial Value (ارزش اولیه)
│   ├── Current Value (ارزش فعلی / دفتری)
│   ├── Depreciation (استهلاک)
│   ├── Operating Costs (هزینه‌های عملیاتی)
│   ├── Maintenance Costs (هزینه‌های نگهداری و تعمیرات)
│   ├── Ownership Cost (هزینه مالکیت / TCO)
│   └── Cost Analysis (تحلیل هزینه‌ها)
│
├── Document Management (مدیریت اسناد و مدارک)
│   ├── Ownership Documents (اسناد مالکیت)
│   ├── Insurance (بیمه‌نامه‌ها)
│   ├── Annual Licenses (مجوزهای سالانه / معاینه فنی)
│   ├── Contracts (قراردادها)
│   ├── Certificates (گواهینامه‌ها / سرتیفیکیت‌ها)
│   ├── Manuals (کتابچه‌های راهنما)
│   ├── Parts Books (کتاب‌های قطعات)
│   ├── Technical Documents (اسناد فنی)
│   └── Expiration Tracking (ردگیری انقضا)
│
├── Media Management (مدیریت چندرسانه‌ای)
│   ├── Image Gallery (گالری تصاویر)
│   ├── Videos (ویدیوها)
│   ├── Attachments (ضمایم)
│   ├── Event Albums (آلبوم‌های وقایع / حوادث)
│   └── Export (خروجی‌گرفتن)
│
├── Knowledge Management (مدیریت دانش)
│   ├── Repair Manuals (راهنماهای تعمیراتی)
│   ├── Parts Catalogs (کاتالوگ‌های قطعات)
│   ├── Technical Bulletins (بولتن‌های فنی)
│   ├── Best Practices (تجارب برتر)
│   └── Shared Documents (اسناد به اشتراک گذاشته‌شده)
│
├── Forecasting (پیش‌بینی)
│   ├── Fuel Forecast (پیش‌بینی سوخت)
│   ├── Lubricant Forecast (پیش‌بینی روانکارها)
│   ├── Filter Forecast (پیش‌بینی فیلترها)
│   ├── Spare Parts Forecast (پیش‌بینی قطعات یدکی)
│   ├── Maintenance Forecast (پیش‌بینی نگهداری و تعمیرات)
│   └── Budget Forecast (پیش‌بینی بودجه)
│
├── Notifications (اعلان‌ها و هشدارها)
│   ├── Maintenance Alerts (هشدارهای نگهداری و سرویس)
│   ├── Expiring Documents (اسناد در حال انقضا)
│   ├── Warranty Alerts (هشدارهای گارانتی)
│   ├── Inspection Alerts (هشدارهای بازرسی)
│   └── Custom Notifications (اعلان‌های سفارشی)
│
├── Reporting & Analytics (گزارش‌گیری و تحلیل)
│   ├── Operational Reports (گزارش‌های عملیاتی)
│   ├── Financial Reports (گزارش‌های مالی)
│   ├── Maintenance Reports (گزارش‌های نگهداری و تعمیرات)
│   ├── KPI Dashboard (داشبورد شاخص‌های کلیدی عملکرد)
│   ├── Cost Analysis (تحلیل هزینه‌ها)
│   └── Performance Analysis (تحلیل عملکرد)
│
└── Administration (مدیریت سیستم)
    ├── Users (کاربران)
    ├── Roles (نقش‌ها)
    ├── Permissions (مجوزها و دسترسی‌ها)
    ├── Audit Logs (لاگ‌های حسابرسی و ممیزی)
    ├── Settings (تنظیمات)
    └── System Configuration (پیکربندی سیستم)
```

---

# اصول طراحی قابلیت‌ها (Capability Design Principles)

هر قابلیت باید:

- نمایانگر یک مسئولیت تجاری باشد؛
- مستقل از فناوری پیاده‌سازی باقی بماند؛
- دارای مالکیت شفاف باشد؛
- اصطلاحات صریح کسب‌وکار را نمایان سازد؛
- در صورت امکان به صورت مستقل تکامل یابد.

قابلیت‌ها هرگز نباید بر اساس جداول پایگاه داده یا صفحات رابط کاربری تعریف گردند.

---

# روابط میان قابلیت‌ها (Capability Relationships)

قابلیت‌ها با یکدیگر همکاری می‌کنند اما با جفت‌شدگی ضعیف (Loosely Coupled) باقی می‌مانند.

مثال‌ها:

- بخش نگهداری و تعمیرات (Maintenance) داده‌های کنتورها (Meter) را مصرف می‌کند.
- بخش پیش‌بینی (Forecasting) داده‌های تاریخی نگهداری و سوخت را مصرف می‌نماید.
- بخش اعلان‌ها (Notifications) داده‌های نگهداری، اسناد و گارانتی را مصرف می‌کند.
- بخش مدیریت مالی (Financial Management) اطلاعات خرید و نگهداری را مصرف می‌نماید.
- بخش گزارش‌گیری (Reporting) داده‌ها را از تمامی قابلیت‌های تجاری مصرف می‌کند.

تمامی قابلیت‌های کسب‌وکار به گونه‌ای طراحی شده‌اند که مستقل از توپولوژی استقرار عمل نمایند.

عملیات تجاری ممکن است درون فضاهای کاری سازمانی، پروژه‌ای یا کاربری بدون تغییر در قواعد کسب‌وکار اجرا شوند.

همگام‌سازی توزیع‌شده تغییرات تجاری اعتبارسنجی‌شده را بین فضاهای کاری منتقل نموده و همزمان یکپارچگی تجاری را حفظ می‌نماید.

این رفتار توسط ADR-0012 (معماری فضای کاری توزیع‌شده) تعریف شده است.

---

# گسترش‌پذیری در آینده (Future Expansion)

مدل قابلیت‌ها به صورت عمدی از گسترش‌های آینده پشتیبانی می‌کند، از جمله:

- محیط‌های چندشرکتی
- مدیریت ناوگان (Fleet management)
- یکپارچه‌سازی اینترنت اشیاء (IoT integration)
- تله‌ماتیک (Telematics)
- نگهداری و تعمیرات پیش‌بینانه (Predictive maintenance)
- عیب‌یابی به کمک هوش مصنوعی (AI-assisted diagnostics)
- برنامه‌های کاربردی موبایل
- همگام‌سازی فضاهای کاری توزیع‌شده
- اجرای پروژه‌ها با اولویت آفلاین (Offline-first)
- همگام‌سازی دوطرفه میان فضاهای کاری سازمانی، پروژه‌ای و کاربری
- یکپارچه‌سازی با ERPهای خارجی
- یکپارچه‌سازی با سیستم‌های اطلاعات جغرافیایی (GIS)
- بازرسی‌های دیجیتال

---

# نگاشت به ماژول‌ها (Module Mapping)

انتظار می‌رود مدل قابلیت‌ها به ماژول‌های برنامه تکامل یابد.

مثال‌ها:

| قابلیت | ماژول آینده |
|------------|---------------|
| Organization | Organization |
| Asset Management | Asset |
| Component Management | Components |
| Maintenance | Maintenance |
| Inventory | Inventory |
| Finance | Finance |
| Documents | Documents |
| Knowledge | Knowledge |
| Forecasting | Forecasting |
| Reporting | Reporting |

مرزهای ماژول‌ها در طول مدل‌سازی دامنه (Domain Modeling) دقیق‌تر خواهند شد.

---

# خارج از محدوده سیستم (Out of Scope)

حوزه‌های زیر به صورت عمدی از محدوده فعلی کنار گذاشته شده‌اند:

- حسابداری عمومی
- حقوق و دستمزد
- منابع انسانی
- مدیریت ارتباط با مشتری (CRM)
- جایگزینی سیستم‌های جامع برنامه‌ریزی منابع سازمان (ERP)

پلتفرم ممکن است با چنین سیستم‌هایی یکپارچه شود اما جایگزین آن‌ها نخواهد شد.

---

# اسناد مرتبط (Related Documents)

- `../01-vision/00-Vision.md`
- `../01-vision/01-DocumentationRoadmap.md`
- `00-TechnologyEvaluationTemplate.md`
- `01-Architecture.md`
- `03-TechnologyGapAnalysis.md`
- `../06-decisions/000-ADR-INDEX.md`
- `TE-0001-.NET10.md` تا `TE-0035-Reporting-Technology-Evaluation.md`
- `../03-domain/02-BoundedContexts.md`

---

# تاریخچه تغییرات (Change History)

| نسخه | تاریخ | نویسنده | توصیف |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | مدل قابلیت‌های اولیه |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0؛ افزودن پیوندها به نمایه اصلی ADRها و تمامی ۳۵ سند TE |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
