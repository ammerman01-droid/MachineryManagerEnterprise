| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | ADR-0006 |
| **عنوان** | استفاده از انتیتی فریم‌ورک کور (Use Entity Framework Core) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# زمینه و مسئله (Context)

راهکار MachineryManagerEnterprise نیازمند یک نگاشت‌دهنده شیء-رابطه‌ای (ORM) مدرن با قابلیت پشتیبانی از کاربردهای سازمانی و در عین حال حفظ تفکیک معماری و قابلیت نگهداری بلندمدت است.

ابزار ORM انتخابی باید موارد زیر را فراهم نماید:

- کارایی و عملکرد بالا
- ادغام عمیق و یکپارچه با دات‌نت (.NET integration)
- پشتیبانی کامل از LINQ
- مدیریت مایگریشن‌ها و شمای دیتابیس (Migration management)
- پشتیبانی از تراکنش‌ها (Transaction support)
- توسعه و پشتیبانی فعال
- ابزارهای کمکی فوق‌العاده (Excellent tooling)

همچنین این ORM باید به صورت طبیعی با اصول معماری پاک (Clean Architecture) ادغام شود.

---

# تصمیم (Decision)

لایه زیرساخت (Infrastructure Layer) باید از **Entity Framework Core** به عنوان ORM اصلی استفاده نماید.

کلیه دسترسی‌ها به پایگاه داده رابطه‌ای باید از طریق Entity Framework Core پیاده‌سازی شوند.

لایه‌های Application و Domain باید کاملاً مستقل و بی‌خبر از EF Core باقی بمانند.

---

# پیش‌ران‌های تصمیم (Decision Drivers)

- پشتیبانی بومی دات‌نت (Native .NET support)
- کارایی و عملکرد (Performance)
- قابلیت نگهداری (Maintainability)
- اکوسیستم بالغ و آزموده‌شده (Mature ecosystem)
- پشتیبانی از مایگریشن‌ها (Migration support)
- قابلیت‌های LINQ
- ابزارهای قدرتمند توسعه (Strong tooling)
- جامعه کاربری گسترده (Community support)

---

# گزینه‌های جایگزین بررسی‌شده (Alternatives Considered)

## کتابخانه Dapper

رد شد؛ زیرا گرچه عملکرد فوق‌العاده‌ای ارائه می‌دهد، اما فاقد قابلیت‌های پیش‌فرض ردیابی تغییرات (Change tracking)، مایگریشن‌ها و امکانات سطح بالای ORM مورد نیاز برای این راهکار است.

---

## ابزار NHibernate

رد شد؛ به دلیل پیچیدگی بالاتر و نرخ پذیرش پایین‌تر در برنامه‌های مدرن دات‌نت.

---

## ابزار Linq2Db

رد شد؛ زیرا اکوسیستم و جامعه کاربری آن بسیار کوچک‌تر از EF Core است.

---

# پیامدها (Consequences)

## پیامدهای مثبت (Positive)

- مدل ماندگاری یکپارچه و استاندارد (Unified persistence model)
- ابزارهای توسعه عالی (Excellent tooling)
- پشتیبانی قدرتمند از LINQ
- مایگریشن‌های توکار (Built-in migrations)
- قابلیت نگهداری بسیار بالا (High maintainability)
- مستندات آموزشی بی‌نظیر (Excellent documentation)

## پیامدهای منفی (Negative)

- سربار انتزاعی اندکی بالاتر نسبت به میکرو ORMها.
- توسعه‌دهندگان باید سازوکار Change Tracking و چرخه حیات DbContext را به خوبی درک کنند.

---

# تأثیر بر معماری (Architecture Impact)

فناوری Entity Framework Core فقط و فقط باید درون **لایه زیرساخت (Infrastructure Layer)** وجود داشته باشد.

لایه Domain هرگز نباید EF Core را رفرنس دهد.

لایه Application صرفاً باید از طریق انتزاع‌های ریپازیتوری (Repository) یا واحد کار (Unit of Work) به ماندگاری داده‌ها دسترسی داشته باشد.

لایه زیرساخت (Infrastructure) این انتزاع‌ها را پیاده‌سازی می‌نماید.

---

# نکات پیاده‌سازی (Implementation Notes)

لایه زیرساخت (Infrastructure) باید شامل موارد زیر باشد:

- کلاس DbContext
- پیکربندی موجودیت‌ها (Entity configurations)
- پیاده‌سازی‌های ریپازیتوری (Repository implementations)
- مایگریشن‌ها (Migrations)

لایه Application اینترفیس‌های ریپازیتوری را تعریف می‌کند.

لایه Domain نسبت به نحوه ذخیره‌سازی و دیتابیس کاملاً بی‌خبر (Persistence-ignorant) باقی می‌ماند.

---

# قوانین انطباق (Compliance Rules)

۱. انتیتی فریم‌ورک کور فقط و فقط باید درون Infrastructure وجود داشته باشد.

۲. لایه Domain هرگز نباید پکیج `Microsoft.EntityFrameworkCore` را رفرنس دهد.

۳. لایه Application هرگز نباید پکیج `Microsoft.EntityFrameworkCore` را رفرنس دهد.

۴. شیء `DbContext` هرگز نباید به درون لایه Domain تزریق شود.

۵. مایگریشن‌ها صرفاً باید درون لایه Infrastructure نگهداری و مدیریت شوند.

۶. اینترفیس‌های ریپازیتوری متعلق به لایه Application هستند.

۷. پیاده‌سازی‌های ریپازیتوری متعلق به لایه Infrastructure هستند.

۸. منطق ماندگاری داده‌ها (Persistence logic) هرگز نباید درون لایه Presentation وجود داشته باشد.

---

# ارزیابی فناوری مرتبط (Related Technology Evaluation)

TE-0004 — Entity Framework Core

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
- ADR-0003 — استفاده از دات‌نت ۱۰ (Use .NET 10)
- کاتالوگ وابستگی‌ها (Dependency Catalog)

---

# مراجع (References)

https://learn.microsoft.com/ef/core/

https://github.com/dotnet/efcore

https://www.nuget.org/packages/Microsoft.EntityFrameworkCore

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | تصمیم اولیه |
| 2.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی v3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
