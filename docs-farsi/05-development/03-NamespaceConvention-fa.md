| ویژگی | مقدار |
|---|---|
| **شناسه سند** | DOC-DEV-004 |
| **عنوان** | قرارداد فضای نام (Namespace Convention) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند، قراردادهای رسمی فضای نام (Namespace Conventions) را برای راهکار **MachineryManagerEnterprise** تعریف می‌کند.

یک سلسله‌مراتب فضای نام منسجم موارد زیر را ارتقا می‌دهد:

- خوانایی (Readability)
- ناوبری و جابجایی میان کدها (Navigation)
- بازآفرینی و ریفکتورینگ (Refactoring)
- قابلیت کشف‌پذیری (Discoverability)
- قابلیت نگهداری بلندمدت (Long-term maintainability)

هر پروژه باید از این قراردادها پیروی کند.

---

# اصول عمومی (General Principles)

فضاهای نام باید:

- ساختار فیزیکی پروژه را منعکس کنند.
- قطعی و مشخص (Deterministic) باشند.
- از تودرتویی غیرضروری پرهیز کنند.
- از مرزهای پروژه پیروی نمایند.
- هرگز جزئیات پیاده‌سازی را افشا نکنند.

فضاهای نام بخشی از معماری به شمار می‌روند و نباید اختیاری یا سلیقه‌ای در نظر گرفته شوند.

---

# فضای نام ریشه (Root Namespace)

هر پروژه با یک فضای نام ریشه یکسان آغاز می‌شود:

```text
MachineryManagerEnterprise
```

---

# فضاهای نام پروژه (Project Namespaces)

هر پروژه فضای نام ریشه را گسترش می‌دهد.

نمونه‌ها:

```text
MachineryManagerEnterprise.SharedKernel

MachineryManagerEnterprise.Domain

MachineryManagerEnterprise.Application

MachineryManagerEnterprise.Infrastructure

MachineryManagerEnterprise.Web
```

---

# فضاهای نام ویژگی‌ها (Feature Namespaces)

کارکردهای تجاری باید بر اساس ویژگی (Feature) گروه‌بندی شوند.

نمونه:

```text
MachineryManagerEnterprise.Application.Features.Inventory

MachineryManagerEnterprise.Application.Features.Users

MachineryManagerEnterprise.Application.Features.Maintenance
```

---

# فرمان‌ها (Commands)

فرمان‌ها باید در زیر ویژگی متناظر خود قرار گیرند.

نمونه:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

---

# کوئری‌ها (Queries)

کوئری‌ها از همین قرارداد پیروی می‌کنند:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Queries
```

---

# اعتبارسنج‌ها (Validators)

اعتبارسنج‌ها متعلق به کنار فرمان‌ها یا کوئری‌های متناظر خود هستند.

نمونه:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Validation
```

---

# اشیاء انتقال داده (DTOs)

اشیاء DTO در درون ویژگی مالک خود باقی می‌مانند:

```text
MachineryManagerEnterprise.Application.Features.Inventory.DTOs
```

---

# نگاشت (Mapping)

پروفایل‌های نگاشت به صورت گروه‌بندی‌شده بر اساس ویژگی باقی می‌مانند:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Mapping
```

---

# دامنه (Domain)

فضاهای نام دامنه باید مفاهیم تجاری را منعکس کنند نه لایه‌های فنی را.

نمونه:

```text
MachineryManagerEnterprise.Domain.Inventory

MachineryManagerEnterprise.Domain.Users

MachineryManagerEnterprise.Domain.Maintenance
```

از فضاهای نام مانند موارد زیر پرهیز کنید:

```text
Domain.Entities

Domain.Models

Domain.Classes
```

مفهوم تجاری بسیار بااهمیت‌تر از مصنوعات و ابزارهای فنی است.

---

# زیرساخت (Infrastructure)

فضاهای نام زیرساخت جزئیات پیاده‌سازی را منعکس می‌کنند.

نمونه‌ها:

```text
MachineryManagerEnterprise.Infrastructure.Persistence

MachineryManagerEnterprise.Infrastructure.Identity

MachineryManagerEnterprise.Infrastructure.Logging

MachineryManagerEnterprise.Infrastructure.Caching
```

---

# وب و لایه ارائه (Web)

فضاهای نام لایه ارائه باید از سازمان‌دهی رابط کاربری پیروی نمایند.

نمونه‌ها:

```text
MachineryManagerEnterprise.Web.Components

MachineryManagerEnterprise.Web.Pages

MachineryManagerEnterprise.Web.Layout

MachineryManagerEnterprise.Web.Shared
```

---

# آزمون‌ها (Tests)

پروژه‌های آزمون، فضاهای نام بخش تولیدی را منعکس می‌کنند.

نمونه:

```text
MachineryManagerEnterprise.Application.Tests.Features.Inventory
```

این تطابق، یافتن کدهای متناظر در بخش تولیدی را بسیار ساده می‌سازد.

---

# قواعد نام‌گذاری (Naming Rules)

فضاهای نام باید:

- از نگارش PascalCase استفاده کنند.
- هرگز شامل فاصله (Space) نباشند.
- هرگز از مخفف‌ها استفاده نکنند مگر اینکه به صورت جهانی پذیرفته شده باشند.
- هرگز حاوی شماره نسخه نباشند.
- هرگز فناوری پیاده‌سازی را افشا نکنند.

---

# حداکثر عمق فضای نام (Maximum Namespace Depth)

فضاهای نام بیش از حد عمیق موجب کاهش خوانایی می‌شوند.

عمق توصیه‌شده:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

از ساختارهایی مانند نمونه زیر پرهیز کنید:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands.Create.Internal.Models
```

---

# برابری فضای نام با پوشه (Namespace Equals Folder)

هر فضای نام باید با پوشه فیزیکی خود مطابقت داشته باشد.

نمونه:

```text
Features

Inventory

Commands

CreateMachineCommand.cs
```

فضای نام:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

این رابطه یک‌به‌یک، ناوبری و ریفکتورینگ را ساده می‌کند.

---

# ماژول‌های آینده (Future Modules)

زمینه‌های مرزبندی‌شده جدید باید تبدیل به ریشه‌های جدید فضای نام شوند.

نمونه:

```text
MachineryManagerEnterprise.Inventory

MachineryManagerEnterprise.Finance

MachineryManagerEnterprise.HumanResources
```

این امر ماژول‌ها را با رشد راهکار مستقل نگه می‌دارد.

---

# انطباق (Compliance)

هر فضای نام تازه ایجادشده باید با این سند مطابقت داشته باشد.

انحرافات فضای نام نیازمند تأییدیه معماری از طریق یک سند ADR است.

---

# اسناد مرتبط (Related Documents)

- DOC-CONVENTIONS
- DOC-README
- DOC-DEV-001 (اصول توسعه / Development Principles)
- DOC-DEV-002 (ساختار راهکار / Solution Structure)
- DOC-DEV-003 (ساختار پروژه / Project Structure)
- DOC-DEV-005 (قواعد وابستگی / Dependency Rules)

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
| 1.0.0 | 2026-07-18 | معمار راهکار | قراردادهای اولیه فضای نام |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
