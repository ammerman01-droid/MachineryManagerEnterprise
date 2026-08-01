# قراردادهای فضای نام (Namespace Conventions)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | DOC-DEV-004 |
| **نسخه** | 4.0.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف

این سند قراردادهای رسمی فضای نام (Namespace) را برای راهکار **MachineryManagerEnterprise** تعریف می‌کند.

یک سلسله‌مراتب فضای نام یکنواخت و سازگار موارد زیر را بهبود می‌بخشد:

- خوانایی (Readability)
- قابلیت پیمایش (Navigation)
- بازسازی کد (Refactoring)
- قابلیت کشف (Discoverability)
- قابلیت نگهداری بلندمدت (Long-term maintainability)

هر پروژه باید از این قراردادها پیروی کند.

---

# ۲. اصول عمومی

فضاهای نام باید:

- منعکس‌کننده ساختار فیزیکی پروژه باشند.
- قطعی و تعیین‌شده باشند.
- از تو در تویی غیرضروری اجتناب کنند.
- مرزهای پروژه را دنبال کنند.
- هرگز جزئیات پیاده‌سازی را افشا نکنند.

فضاهای نام بخشی از معماری هستند و نباید تصادفی یا سلیقه‌ای تلقی شوند.

---

# ۳. فضای نام ریشه (Root Namespace)

هر پروژه با یک فضای نام ریشه یکسان آغاز می‌شود:

```text
MachineryManagerEnterprise
```

---

# ۴. فضاهای نام پروژه (Project Namespaces)

هر پروژه فضای نام ریشه را گسترش می‌دهد.

مثال‌ها:

```text
MachineryManagerEnterprise.SharedKernel

MachineryManagerEnterprise.Domain

MachineryManagerEnterprise.Application

MachineryManagerEnterprise.Infrastructure

MachineryManagerEnterprise.Web
```

---

# ۵. فضاهای نام ویژگی (Feature Namespaces)

قابلیت‌های کسب‌وکار باید بر اساس ویژگی (Feature) گروه‌بندی شوند.

مثال:

```text
MachineryManagerEnterprise.Application.Features.Inventory

MachineryManagerEnterprise.Application.Features.Users

MachineryManagerEnterprise.Application.Features.Maintenance
```

---

# ۶. دستورات (Commands)

دستورات باید ذیل ویژگی مربوطه قرار گیرند.

مثال:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

---

# ۷. پرس‌وجوها (Queries)

پرس‌وجوها از همان قرارداد پیروی می‌کنند:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Queries
```

---

# ۸. اعتبارسنج‌ها (Validators)

اعتبارسنج‌ها متعلق به کنار دستورات یا پرس‌وجوهای مربوط به خود هستند.

مثال:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Validation
```

---

# ۹. اشیاء انتقال داده (DTOs)

DTOها درون ویژگی مالک خود باقی می‌مانند:

```text
MachineryManagerEnterprise.Application.Features.Inventory.DTOs
```

---

# ۱۰. نگاشت (Mapping)

پروفایل‌های نگاشت بر اساس ویژگی گروه‌بندی می‌مانند:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Mapping
```

---

# ۱۱. دامنه (Domain)

فضاهای نام دامنه باید به جای لایه‌های فنی، مفاهیم کسب‌وکار را منعکس کنند.

مثال:

```text
MachineryManagerEnterprise.Domain.Inventory

MachineryManagerEnterprise.Domain.Users

MachineryManagerEnterprise.Domain.Maintenance
```

از فضاهای نام مانند موارد زیر اجتناب کنید:

```text
Domain.Entities

Domain.Models

Domain.Classes
```

مفهوم کسب‌وکار مهم‌تر از مصنوع فنی است.

---

# ۱۲. زیرساخت (Infrastructure)

فضاهای نام زیرساخت، جزئیات پیاده‌سازی را منعکس می‌کنند.

مثال‌ها:

```text
MachineryManagerEnterprise.Infrastructure.Persistence

MachineryManagerEnterprise.Infrastructure.Identity

MachineryManagerEnterprise.Infrastructure.Logging

MachineryManagerEnterprise.Infrastructure.Caching
```

---

# ۱۳. وب / لایه ارائه (Web)

فضاهای نام لایه ارائه باید از سازماندهی رابط کاربری پیروی کنند.

مثال‌ها:

```text
MachineryManagerEnterprise.Web.Components

MachineryManagerEnterprise.Web.Pages

MachineryManagerEnterprise.Web.Layout

MachineryManagerEnterprise.Web.Shared
```

---

# ۱۴. آزمون‌ها (Tests)

پروژه‌های آزمون، فضاهای نام محیط تولید را منعکس می‌کنند.

مثال:

```text
MachineryManagerEnterprise.Application.Tests.Features.Inventory
```

این امر یافتن کد مربوطه در محیط تولید را مستقیم و آسان می‌سازد.

---

# ۱۵. قوانین نام‌گذاری

فضاهای نام باید:

- از PascalCase استفاده کنند.
- هرگز شامل فاصله (Space) نباشند.
- هرگز از حروف اختصاری استفاده نکنند مگر اینکه به صورت جهانی پذیرفته شده باشند.
- هرگز شامل شماره نسخه نباشند.
- هرگز فناوری پیاده‌سازی را افشا نکنند.

---

# ۱۶. حداکثر عمق فضای نام (Maximum Namespace Depth)

فضاهای نام بیش از حد عمیق، خوانایی را کاهش می‌دهند.

عمق پیشنهادی:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

از ساختارهایی مانند زیر اجتناب کنید:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands.Create.Internal.Models
```

---

# ۱۷. انطباق فضای نام با پوشه (Namespace Equals Folder)

هر فضای نام باید با پوشه فیزیکی خود مطابقت داشته باشد.

مثال:

مسیر فیزیکی:
```text
Features/Inventory/Commands/CreateMachineCommand.cs
```

فضای نام:
```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

این رابطه یک-به-یک، پیمایش و بازسازی کد را ساده می‌سازد.

---

# ۱۸. ماژول‌های آتی (Future Modules)

محیط‌های متناظر (Bounded Contexts) جدید باید به ریشه‌های جدید فضای نام تبدیل شوند.

مثال:

```text
MachineryManagerEnterprise.Inventory

MachineryManagerEnterprise.Finance

MachineryManagerEnterprise.HumanResources
```

این امر با رشد راهکار، ماژول‌ها را مستقل نگه می‌دارد.

---

# ۱۹. انطباق و رعایت

هر فضای نام جدید ایجادشده باید با این سند منطبق باشد.

انحرافات فضای نام نیازمند تاییدیه معماری از طریق ADR است.

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

- DOC-CONVENTIONS
- DOC-README
- DOC-DEV-001 (اصول توسعه)
- DOC-DEV-002 (ساختار راهکار)
- DOC-DEV-003 (ساختار پروژه)
- DOC-DEV-005 (قوانین وابستگی)

---

# تاریخچه تغییرات

| نسخه | تاریخ | نویسنده / نقش | شرح |
|----------|------------|-------------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | قراردادهای اولیه فضای نام |
| 2.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
