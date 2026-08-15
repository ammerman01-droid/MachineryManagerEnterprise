| ویژگی | مقدار |
|---|---|
| **شناسه سند** | MOD-006 |
| **عنوان** | کاتالوگ گردش کارها (Workflow catalogue) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند، گردش کارهای تجاری طولانی‌مدت (Long-Running Business Workflows) را تعریف می‌کند.

یک گردش کار (Workflow)، چندین عملیات تجاری را جهت دستیابی به یک هدف تجاری واحد هماهنگ می‌کند.

یک گردش کار ممکن است در چندین تجمیع (Aggregates)، سرویس‌های کاربردی (Application Services) و سامانه‌های خارجی امتداد یابد.

---

# فلسفه گردش کار (Workflow Philosophy)

یک گردش کار، عملیات تجاری که در چندین ماژول گسترده شده‌اند را هماهنگ می‌سازد.

یک گردش کار هرگز مالک قواعد تجاری نیست.

قواعد تجاری منحصراً در درون تجمیع‌ها (Aggregates) و سرویس‌های دامنه (Domain Services) باقی می‌مانند.

یک گردش کار صرفاً اجرای آن‌ها را هماهنگ و هدایت (Orchestrate) می‌کند.

---

# ۲. اصول گردش کار (Workflow Principles)

هر گردش کار باید اصول زیر را برآورده سازد:

- مبتنی بر کسب‌وکار (Business oriented)
- مستقل از فناوری (Technology independent)
- قطعی و تعین‌پذیر (Deterministic)
- قابل ردگیری (Traceable)
- قابل بازیابی (Recoverable)
- قابل حسابرسی (Auditable)

یک گردش کار، قاعده تجاری نیست.

یک گردش کار، قواعد تجاری را هماهنگ می‌کند.

---

# ۳. چرخه حیات عمومی گردش کار (Generic Workflow Lifecycle)

```text
درخواست‌شده (Requested)

↓

اعتبارسنجی‌شده (Validated)

↓

در حال اجرا (Executing)

↓

تکمیل‌شده (Completed)
```

مسیرهای جایگزین:

```text
در حال اجرا (Executing)

↓

شکست‌خورده (Failed)
```

یا

```text
در حال اجرا (Executing)

↓

لغوشده (Cancelled)
```

---

# ۴. گردش کار WF-001 — خرید دارایی کارکرده (Purchase Used Asset)

## هدف (Goal)

ثبت یک ماشین خریداری‌شده که از قبل دارای سابقه و تاریخچه عملیاتی است.

## ماژول‌ها (Modules)

- دارایی (Asset)
- موتور (Engine)
- کنتور و کارکردسنج (Meter)
- مالی (Financial)
- اسناد (Documents)

## جریان اصلی (Main Flow)

۱. ثبت دارایی (Register Asset)
۲. ثبت موتور (Register Engine)
۳. ثبت کنتور (Register Meter)
۴. ثبت اطلاعات خرید (Register Purchase Information)
۵. ثبت اسناد اولیه (Register Initial Documents)
۶. فعال‌سازی دارایی (Activate Asset)

---

# ۵. گردش کار WF-002 — تعویض موتور (Replace Engine)

## هدف (Goal)

تعویض موتوری که در حال حاضر روی دستگاه نصب است.

## ماژول‌ها (Modules)

- دارایی (Asset)
- موتور (Engine)
- نگهداری و تعمیرات (Maintenance)
- مالی (Financial)

## جریان اصلی (Main Flow)

۱. پیاده‌سازی و حذف موتور فعلی (Remove current Engine)
۲. ثبت نگهداری و تعمیرات (Register Maintenance)
۳. نصب موتور جایگزین (Install replacement Engine)
۴. به‌روزرسانی پیکربندی جاری (Update current configuration)
۵. حفظ روابط و پیوندهای تاریخی (Preserve historical relationships)
۶. انتشار رخداد EngineReplaced

---

# ۶. گردش کار WF-003 — تعویض دستگاه کنتور/کارکردسنج (Replace Meter Device)

## هدف (Goal)

تعویض دستگاه فیزیکی کنتور با حفظ سوابق کارکرد عملیاتی (Operational Usage).

## ماژول‌ها (Modules)

- کنتور (Meter)
- دارایی (Asset)
- گزارش‌گیری (Reporting)

## جریان اصلی (Main Flow)

۱. آرشیو کنتور قبلی (Archive previous Meter)
۲. نصب کنتور جدید (Install new Meter)
۳. ثبت عدد قرائت‌شده هنگام نصب (Record installation reading)
۴. حفظ کارکرد عملیاتی تجمیعی (Preserve accumulated Operational Usage)
۵. انتشار رخداد MeterReplaced

---

# ۷. گردش کار WF-004 — تکمیل نگهداری و تعمیرات پیشگیرانه (Complete Preventive Maintenance)

## هدف (Goal)

به پایان رساندن یک فعالیت نگهداری و تعمیرات زمان‌بندی‌شده.

## ماژول‌ها (Modules)

- نگهداری و تعمیرات (Maintenance)
- قطعات و اجزا (Components)
- مالی (Financial)
- پیش‌بینی (Forecast)

## جریان اصلی (Main Flow)

۱. اعتبارسنجی دستور کار نت (Validate maintenance order)
۲. ثبت وظایف تکمیل‌شده (Register completed tasks)
۳. ثبت قطعات تعویض‌شده (Register replaced Components)
۴. ثبت هزینه‌ها (Register expenses)
۵. به‌روزرسانی تاریخچه نگهداری و تعمیرات (Update maintenance history)
۶. محاسبه مجدد موعد نگهداری بعدی (Recalculate next maintenance)
۷. انتشار رخداد MaintenanceCompleted

---

# ۸. گردش کار WF-005 — ثبت خرابی (Register Failure)

## هدف (Goal)

ثبت خرابی غیرمنتظره تجهیزات.

## ماژول‌ها (Modules)

- نگهداری و تعمیرات (Maintenance)
- گزارش‌گیری (Reporting)

## جریان اصلی (Main Flow)

۱. ثبت خرابی (Register Failure)
۲. ایجاد درخواست تعمیر (Create repair request)
۳. اطلاع‌رسانی به پرسنل مسئول (Notify responsible personnel)
۴. به‌روزرسانی وضعیت عملیاتی (Update operational status)

---

# ۹. گردش کار WF-006 — تمدید سند (Renew Document)

## هدف (Goal)

جایگزینی یک سند تجاری در حال انقضا.

## ماژول‌ها (Modules)

- اسناد (Documents)
- اعلان‌ها (Notifications)

## جریان اصلی (Main Flow)

۱. ثبت سند جدید (Register new document)
۲. آرشیو نسخه قبلی (Archive previous version)
۳. به‌روزرسانی وضعیت انقضا (Update expiration status)
۴. زمان‌بندی یادآوری بعدی (Schedule next reminder)

---

# ۱۰. گردش کار WF-007 — اسقاط/واگذاری دارایی (Dispose Asset)

## هدف (Goal)

خروج دائمی یک دارایی از چرخه عملیات فعال.

## ماژول‌ها (Modules)

- دارایی (Asset)
- مالی (Financial)
- اسناد (Documents)
- گزارش‌گیری (Reporting)

## جریان اصلی (Main Flow)

۱. اعتبارسنجی واجد شرایط بودن برای واگذاری/اسقاط (Validate disposal eligibility)
۲. بازنشستگی دارایی (Retire Asset)
۳. ثبت اطلاعات واگذاری/اسقاط (Register disposal information)
۴. آرشیو سوابق عملیاتی (Archive operational records)
۵. انتشار رخداد AssetDisposed

---

# ۱۱. گردش کار WF-008 — تولید پیش‌بینی (Generate Forecast)

## هدف (Goal)

تولید اطلاعات عملیاتی پیش‌بینانه.

## ماژول‌ها (Modules)

- پیش‌بینی (Forecast)
- گزارش‌گیری (Reporting)

## جریان اصلی (Main Flow)

۱. جمع‌آوری داده‌های تاریخی اعتبارسنجی‌شده (Collect validated historical data)
۲. اجرای مدل پیش‌بینی (Execute prediction model)
۳. ذخیره پیش‌بینی (Store Forecast)
۴. انتشار رخداد ForecastGenerated

---

# ۱۲. بازیابی خطا و شکست (Failure Recovery)

هر گردش کار باید رفتار بازیابی از خطا را تعریف کند.

بازیابی ممکن است شامل موارد زیر باشد:

- تلاش مجدد (Retry)
- بازگشت به عقب (Rollback)
- جبران خسارت / تراکنش جبرانی (Compensation)
- مداخله دستی (Manual intervention)

سوابق تاریخی هرگز نباید در حین فرایند بازیابی حذف شوند.

---

# ۱۳. پایش و نظارت (Monitoring)

هر بار اجرای گردش کار باید موارد زیر را ثبت و ضبط کند:

- شناسه گردش کار (Workflow Id)
- زمان شروع (Start Time)
- زمان پایان (End Time)
- مدت زمان اجرا (Duration)
- کاربر آغازگر (Initiating User)
- وضعیت نهایی (Final Status)
- علت خرابی در صورت وقوع (Failure Reason)

---

# قواعد طراحی گردش کار (Workflow Design Rules)

هر گردش کار باید:

- دقیقاً دارای یک آغازگر (Trigger) باشد.
- یک خروجی و نتیجه تجاری واحد تولید کند.
- صفر یا چند رخداد دامنه (Domain Events) منتشر کند.
- هرگز مرزهای ریشه‌های تجمیع (Aggregate boundaries) را نقض نکند.
- از بازیابی خطا پشتیبانی کند.
- به طور کامل قابل ردگیری باشد.

---

# ۱۴. گردش کارهای آینده (Future Workflows)

نسخه‌های آینده ممکن است گردش کارهایی برای موارد زیر معرفی کنند:

- شارژ مجدد موجودی انبار (Inventory replenishment)
- تدارکات و خرید (Procurement)
- زمان‌بندی و برنامه‌ریزی ناوگان (Fleet scheduling)
- عیب‌یابی با هوش مصنوعی (AI diagnostics)
- همگام‌سازی اینترنت اشیاء (IoT synchronization)
- همگام‌سازی آفلاین موبایل (Mobile offline synchronization)

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

---

# اسناد مرتبط (Related Documents)

- فرمان‌ها (Commands)
- کوئری‌ها (Queries)
- مدیریت‌کننده‌ها (Handlers)
- رخدادهای دامنه (Domain Events)
- قواعد تجاری (Business Rules)
- ماشین‌های وضعیت (State Machines)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | کاتالوگ اولیه گردش کارها |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
