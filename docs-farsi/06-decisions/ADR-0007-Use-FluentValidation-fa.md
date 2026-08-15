| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | ADR-0007 |
| **عنوان** | استفاده از فلوئنت ولیدیشن (Use FluentValidation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# زمینه و مسئله (Context)

راهکار MachineryManagerEnterprise نیازمند یک فریم‌ورک مدرن برای اعتبارسنجی است که قواعد اعتبارسنجی را مستقل از موجودیت‌های تجاری (Business Entities)، منطق لایه نمایش و دغدغه‌های ذخیره‌سازی نگه دارد.

راهکار اعتبارسنجی انتخابی باید:

- از تفکیک پاک دغدغه‌ها (Separation of Concerns) پشتیبانی کند.
- به صورت طبیعی با ASP.NET Core ادغام شود.
- کاملاً آزمون‌پذیر (Testable) باشد.
- قابلیت توسعه و اکستنشن داشته باشد.
- قواعد اعتبارسنجی خوانا و شفاف تولید نماید.
- از بومی‌سازی و چندزبانگی (Localization) پشتیبانی کند.
- با اصول معماری پاک (Clean Architecture) همسو باشد.

---

# تصمیم (Decision)

لایه اپلیکیشن (Application Layer) باید از **FluentValidation** به عنوان فریم‌ورک استاندارد اعتبارسنجی استفاده نماید.

کلیه اعتبارسنجی‌های ورودی داده‌ها باید با استفاده از اعتبارسنج‌های FluentValidation پیاده‌سازی شوند.

موجودیت‌های تجاری (Business entities) نباید حاوی منطق اعتبارسنجی UI باشند.

---

# پیش‌ران‌های تصمیم (Decision Drivers)

- تفکیک دغدغه‌ها (Separation of Concerns)
- خوانایی بالا (Readability)
- آزمون‌پذیری (Testability)
- ادغام با ASP.NET Core
- متن‌باز بودن (Open Source)
- توسعه‌پذیری (Extensibility)
- قابلیت نگهداری (Maintainability)

---

# گزینه‌های جایگزین بررسی‌شده (Alternatives Considered)

## اتریبیوت‌های DataAnnotations

رد شد؛ زیرا قواعد اعتبارسنجی به شدت با DTOها جفت و متصل (Coupled) می‌شوند و این اتریبیوت‌ها برای سناریوهای پیچیده بیانگری کافی ندارند.

---

## فریم‌ورک اعتبارسنجی اختصاصی (Custom Validation Framework)

رد شد؛ زیرا قابلیت‌های موجود و بالغ فعلی را دوباره‌کاری و بازاختراع می‌کرد و در عین حال هزینه‌های نگهداری را افزایش می‌داد.

---

## اعتبارسنجی دستی در کد (Manual Validation)

رد شد؛ زیرا منطق اعتبارسنجی ناهمگون و غیریکنواختی تولید کرده و تکرار کد (Code Duplication) را به شدت افزایش می‌دهد.

---

# پیامدها (Consequences)

## پیامدهای مثبت (Positive)

- لایه اعتبارسنجی پاک و ایزوله (Clean validation layer)
- قواعد اعتبارسنجی با قابلیت استفاده مجدد (Reusable validation rules)
- تست واحد آسان و سریع (Easy unit testing)
- بهبود قابلیت نگهداری (Improved maintainability)
- رویکرد یکپارچه و منسجم در اعتبارسنجی (Consistent validation approach)
- امکانات بسیار غنی و پیشرفته اعتبارسنجی (Rich validation capabilities)

## پیامدهای منفی (Negative)

- توسعه‌دهندگان باید سینتکس و ساختار FluentValidation را فرا بگیرند.
- اعتبارسنج‌ها نیازمند ثبت صریح (Registration) در سیستم هستند.

---

# تأثیر بر معماری (Architecture Impact)

کتابخانه FluentValidation فقط و فقط باید درون **لایه اپلیکیشن (Application Layer)** وجود داشته باشد.

لایه Presentation اعتبارسنجی را از طریق لایه Application فراخوانی می‌نماید.

لایه Domain کاملاً مستقل و بی‌خبر از FluentValidation باقی می‌ماند.

لایه Infrastructure نباید حاوی اعتبارسنج‌های تجاری باشد.

---

# نکات پیاده‌سازی (Implementation Notes)

اعتبارسنج‌ها (Validators) باید به صورت خودکار از طریق تزریق وابستگی (Dependency Injection) رجیستر شوند.

هر شیء DTO درخواست (Request DTO) باید دارای یک Validator متناظر باشد.

اعتبارسنجی باید همواره پیش از اجرای منطق تجاری (Business Logic) اجرا شود.

---

# قوانین انطباق (Compliance Rules)

۱. کتابخانه FluentValidation فقط باید درون Application وجود داشته باشد.

۲. لایه Domain هرگز نباید پکیج FluentValidation را رفرنس دهد.

۳. لایه Presentation هرگز نباید شامل قواعد اعتبارسنجی تجاری باشد.

۴. هر Request DTO باید دارای یک اعتبارسنج (Validator) متناظر باشد.

۵. اعتبارسنجی تجاری نباید با استفاده از DataAnnotations پیاده‌سازی شود.

۶. منطق اعتبارسنجی باید مستقل از لایه ذخیره‌سازی داده‌ها باقی بماند.

---

# ارزیابی فناوری مرتبط (Related Technology Evaluation)

TE-0005 — FluentValidation

---

# اثبات مفهوم مرتبط (Related Proof of Concept)

الزامی نیست (Not Required)

---

---

# خلاصه تصمیمات (Decision Summary)

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی ابری (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط (Related Documents)

- ADR-0001 — تصویب معماری پاک (Adopt Clean Architecture)
- ADR-0002 — تصویب سیاست اولویت متن‌باز (Adopt Open Source First Policy)
- ADR-0006 — استفاده از انتیتی فریم‌ورک کور (Use Entity Framework Core)
- کاتالوگ وابستگی‌ها (Dependency Catalog)

---

# مراجع (References)

https://docs.fluentvalidation.net/

https://github.com/FluentValidation/FluentValidation

https://www.nuget.org/packages/FluentValidation

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | تصمیم اولیه |
| 2.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
