| ویژگی | مقدار |
|---|---|
| **شناسه سند** | DOC-DEV-007 |
| **عنوان** | قراردادهای نام‌گذاری (Naming Conventions) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند، قراردادهای رسمی نام‌گذاری مورد استفاده در سراسر راهکار **MachineryManagerEnterprise** را تعریف می‌کند.

نام‌گذاری منسجم و یکپارچه موارد زیر را بهبود می‌بخشد:

- خوانایی (Readability)
- قابلیت کشف‌پذیری (Discoverability)
- ناوبری میان کدها (Navigation)
- بازآفرینی و ریفکتورینگ (Refactoring)
- ارتباط و تعامل میان توسعه‌دهندگان (Communication between developers)

نام‌گذاری به عنوان بخشی از معماری نرم‌افزار به شمار می‌رود.

---

# قواعد عمومی (General Rules)

تمامی شناسه‌ها (Identifiers) باید:

- تنها از زبان انگلیسی استفاده کنند.
- از نام‌های معنادار استفاده نمایند.
- از مخفف‌ها پرهیز کنند مگر اینکه به صورت جهانی پذیرفته شده باشند.
- نیت و مقصود را توصیف کنند تا جزئیات پیاده‌سازی را.
- در سراسر راهکار منسجم و یکدست باشند.

---

# قواعد بزرگ‌کوچکی حروف (Casing Rules)

| مورد | قرارداد |
|---|---|
| فضای نام (Namespace) | PascalCase |
| کلاس (Class) | PascalCase |
| رکورد (Record) | PascalCase |
| ساختار (Struct) | PascalCase |
| شمارشگر (Enum) | PascalCase |
| اینترفیس (Interface) | PascalCase همراه با پیشوند `I` |
| متد (Method) | PascalCase |
| ویژگی (Property) | PascalCase |
| متغیر محلی (Local Variable) | camelCase |
| پارامتر (Parameter) | camelCase |
| فیلد خصوصی (Private Field) | `_camelCase` |
| مقدار ثابت (Constant) | PascalCase |
| عضو شمارشگر (Enum Member) | PascalCase |

---

# نام‌گذاری کلاس‌ها (Class Naming)

کلاس‌ها باید نمایانگر اسم (Noun) باشند.

نمونه‌های خوب:

```text
Machine

MaintenancePlan

InventoryService
```

اجتناب شود:

```text
MachineHelper

MachineManager2

DoSomething
```

---

# نام‌گذاری اینترفیس‌ها (Interface Naming)

اینترفیس‌ها باید با حرف `I` آغاز شوند.

نمونه‌ها:

```text
IMachineRepository

IUserService

IClock
```

---

# نام‌گذاری متدها (Method Naming)

متدها باید نمایانگر عمل و اقدام (Actions) باشند.

نمونه‌ها:

```text
CreateMachine()

CalculateAvailability()

GenerateReport()
```

متدهای بولی (Boolean Methods) باید به یک پرسش پاسخ دهند.

نمونه‌ها:

```text
IsActive()

HasPermission()

CanDelete()
```

---

# نام‌گذاری متغیرها (Variable Naming)

متغیرها باید هدف خود را به روشنی توصیف کنند.

ترجیح داده می‌شود:

```text
availableMachines

maintenanceSchedule
```

اجتناب شود:

```text
tmp

obj

data

x
```

مگر برای متغیرهای حلقه‌های کوتاه‌مدت.

---

# مجموعه‌ها (Collections)

مجموعه‌ها باید از نام‌های جمع (Plural) استفاده کنند.

نمونه‌ها:

```text
machines

users

maintenancePlans
```

اشیاء تکی باید از نام‌های مفرد (Singular) استفاده کنند.

---

# متغیرهای بولی (Boolean Variables)

متغیرهای بولی باید با کلماتی نظیر موارد زیر شروع شوند:

- is
- has
- can
- should

نمونه‌ها:

```text
isActive

hasPermission

canEdit

shouldRetry
```

---

# متدهای ناهمگام (Async Methods)

متدهای ناهمگام باید با پسوند زیر پایان یابند:

```text
Async
```

نمونه:

```csharp
LoadMachinesAsync()
```

---

# مدیریت‌کننده‌های رخداد (Event Handlers)

مدیریت‌کننده‌های رخداد باید از الگوهای زیر پیروی کنند:

```text
On<Event>

Handle<Event>
```

نمونه‌ها:

```text
OnMachineCreated

HandleUserDeleted
```

---

# اشیاء انتقال داده (DTOs)

نام‌های DTO باید با پسوند زیر پایان یابند:

```text
Dto
```

نمونه‌ها:

```text
MachineDto

UserDto

InventoryDto
```

---

# فرمان‌ها (Commands)

فرمان‌ها باید با پسوند زیر پایان یابند:

```text
Command
```

نمونه‌ها:

```text
CreateMachineCommand

DeleteMachineCommand
```

---

# کوئری‌ها (Queries)

کوئری‌ها باید با پسوند زیر پایان یابند:

```text
Query
```

نمونه‌ها:

```text
GetMachineQuery

SearchInventoryQuery
```

---

# اعتبارسنج‌ها (Validators)

اعتبارسنج‌ها باید با پسوند زیر پایان یابند:

```text
Validator
```

نمونه‌ها:

```text
CreateMachineValidator

UserValidator
```

---

# استثناها (Exceptions)

استثناها باید با پسوند زیر پایان یابند:

```text
Exception
```

نمونه‌ها:

```text
MachineNotFoundException

InvalidLicenseException
```

---

# شمارشگرها (Enumerations)

نام‌های Enum باید مفرد باشند.

نمونه‌ها:

```text
MachineStatus

MaintenancePriority
```

اعضای Enum از PascalCase استفاده می‌کنند.

---

# جداول پایگاه داده (Database Tables)

نام‌های موجودیت‌ها مفرد باقی می‌مانند.

نمونه‌ها:

```text
Machine

MaintenancePlan

InventoryItem
```

فریم‌ورک Entity Framework در صورت لزوم جمع‌بستن اسامی را مدیریت خواهد کرد.

---

# نام فایل‌ها (File Names)

نام فایل‌ها باید با نوع عمومی (Public Type) مطابقت داشته باشد.

نمونه:

```text
MachineService.cs

MachineDto.cs

CreateMachineCommand.cs
```

---

# یکپارچگی فضای نام (Namespace Consistency)

فضاهای نام همواره باید با نام پوشه‌ها مطابقت داشته باشند.

نمونه:

پوشه:

```text
Features

Inventory

Commands
```

فضای نام:

```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

---

# نام‌های ممنوع (Forbidden Names)

اجتناب شود:

```text
Helper

Util

Misc

CommonStuff

Manager

Data

Info
```

نام‌ها باید به جای اهداف عمومی، مسئولیت را منتقل کنند.

---

# انطباق (Compliance)

هر شناسه جدید واردشده به راهکار باید با این قراردادها مطابقت داشته باشد.

استثناها نیازمند تأییدیه معماری هستند.

---

# اسناد مرتبط (Related Documents)

- DOC-CONVENTIONS
- DOC-DEV-001 (اصول توسعه / Development Principles)
- DOC-DEV-004 (قرارداد فضای نام / Namespace Convention)
- DOC-DEV-006 (استانداردهای کدنویسی / Coding Standards)

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
| 1.0.0 | 2026-07-18 | معمار راهکار | قراردادهای اولیه نام‌گذاری |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
