| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | ADR-0011 |
| **عنوان** | استفاده از مدیاتور (Use MediatR) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# زمینه و مسئله (Context)

راهکار MachineryManagerEnterprise از معماری پاک (Clean Architecture) پیروی می‌کند و نیازمند یک الگوی تعاملی منسجم و یکپارچه در لایه اپلیکیشن است.

لایه اپلیکیشن (Application Layer) باید از موارد زیر پشتیبانی نماید:

- الگوی جداسازی مسئولیت دستور و پرس‌وجو (CQRS)
- پیام‌رسانی درخواست / پاسخ (Request / Response messaging)
- رفتارهای پایپ‌لاین (Pipeline Behaviors)
- دغدغه‌های عرضی و فراگیر (Cross-cutting concerns)
- وابستگی کم و سست (Low coupling)
- قابلیت نگهداری بالا (High maintainability)
- آزمون‌پذیری (Testability)

عملیات تجاری باید همواره مستقل از کنترولرها، کامپوننت‌های رابط کاربری (UI) و پیاده‌سازی‌های زیرساختی باقی بمانند.

---

# تصمیم (Decision)

لایه اپلیکیشن (Application Layer) باید از **MediatR** به عنوان سازوکار توزیع و ارسال درخواست‌ها (Request dispatching mechanism) استفاده نماید.

دستورات (Commands)، پرس‌وجوها (Queries) و اعلان‌ها (Notifications) باید از طریق MediatR پردازش شوند.

رفتارهای پایپ‌لاین (Pipeline Behaviors) باید برای مدیریت دغدغه‌های عرضی نظیر اعتبارسنجی، ثبت وقایع و لاگینگ، پایش کارایی و عملکرد، و تراکنش‌ها مورد استفاده قرار گیرند.

---

# پیش‌ران‌های تصمیم (Decision Drivers)

- پشتیبانی از CQRS
- وابستگی سست (Loose coupling)
- توسعه‌پذیری (Extensibility)
- آزمون‌پذیری (Testability)
- رفتارهای پایپ‌لاین (Pipeline Behaviors)
- تفکیک دغدغه‌ها (Separation of Concerns)
- سازگاری با معماری پاک (Clean Architecture compatibility)

---

# گزینه‌های جایگزین بررسی‌شده (Alternatives Considered)

## فراخوانی مستقیم سرویس‌ها (Direct Service Calls)

رد شد؛ زیرا لایه Presentation را به شدت با سرویس‌های Application جفت و متصل (Coupled) کرده و توسعه‌پذیری را کاهش می‌دهد.

---

## پیاده‌سازی میانجی اختصاصی (Custom Mediator Implementation)

رد شد؛ زیرا MediatR بالغ، به طور گسترده پذیرفته‌شده است و مسئله مورد نظر را از قبل حل کرده است.

---

## استفاده انحصاری از گذرگاه رویداد (Event Bus Only)

رد شد؛ زیرا سناریوهای همگام درخواست/پاسخ (Synchronous request/response) همچنان درون اپلیکیشن ضروری هستند.

---

# پیامدها (Consequences)

## پیامدهای مثبت (Positive)

- مدیریت و پردازش یکپارچه درخواست‌ها (Consistent request handling)
- توسعه‌پذیری فوق‌العاده (Excellent extensibility)
- تست آسان (Easy testing)
- کنترلرهای تمیزتر و خلوت‌تر (Cleaner controllers)
- متمرکزسازی دغدغه‌های عرضی (Centralized cross-cutting concerns)
- تفکیک بهتر میان رابط کاربری و منطق تجاری (Better separation between UI and business logic)

## پیامدهای منفی (Negative)

- یک لایه انتزاعی اضافی (Additional abstraction layer)
- توسعه‌دهندگان باید رفتار پایپ‌لاین درخواست‌ها را به خوبی درک کنند.

---

# تأثیر بر معماری (Architecture Impact)

مدیاتور فقط و فقط باید درون **لایه اپلیکیشن (Application Layer)** وجود داشته باشد.

لایه Presentation با لایه Application صرفاً از طریق ارسال Commands یا Queries ارتباط برقرار می‌کند.

لایه Domain هرگز نباید به MediatR رفرنس داشته باشد.

لایه Infrastructure هرگز نباید هندلرها (Handlers) را مستقیماً فراخوانی کند.

---

# نکات پیاده‌سازی (Implementation Notes)

هر مورد کاربری (Use case) باید توسط موارد زیر نمایندگی شود:

- یک Command یا Query
- یک Handler
- یک Validator (در صورت لزوم)

رفتارهای پایپ‌لاین (Pipeline Behaviors) در صورت تناسب باید موارد زیر را پیاده‌سازی کنند:

- اعتبارسنجی (Validation)
- ثبت وقایع و لاگینگ (Logging)
- پایش کارایی و عملکرد (Performance Monitoring)
- مدیریت تراکنش‌ها (Transaction Management)
- مدیریت خطاها و استثناها (Exception Handling)

---

# قوانین انطباق (Compliance Rules)

۱. کتابخانه MediatR فقط و فقط باید درون Application وجود داشته باشد.

۲. لایه Domain هرگز نباید MediatR را رفرنس دهد.

۳. لایه Presentation هرگز نباید هندلرها را مستقیماً فراخوانی کند.

۴. دستورات (Commands) باید وضعیت را تغییر دهند.

۵. پرس‌وجوها (Queries) هرگز نباید وضعیت را تغییر دهند.

۶. دغدغه‌های عرضی باید با استفاده از Pipeline Behaviors پیاده‌سازی شوند.

۷. کنترولرها و کامپوننت‌های Razor صرفاً باید از طریق `IMediator` ارتباط برقرار نمایند.

---

# ارزیابی فناوری مرتبط (Related Technology Evaluation)

TE-0009 — MediatR *(در آینده ایجاد خواهد شد)*

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
- ADR-0007 — استفاده از فلوئنت ولیدیشن (Use FluentValidation)
- ADR-0008 — استفاده از مپستر (Use Mapster)
- کاتالوگ وابستگی‌ها (Dependency Catalog)

---

# مراجع (References)

https://github.com/jbogard/MediatR

https://www.nuget.org/packages/MediatR

https://github.com/jbogard/MediatR/wiki

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | تصمیم اولیه |
| 2.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
