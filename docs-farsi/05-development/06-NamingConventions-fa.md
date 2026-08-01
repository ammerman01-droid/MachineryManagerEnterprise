# قراردادهای نام‌گذاری (Naming Conventions)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | DOC-DEV-007 |
| **نسخه** | 4.0.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف

این سند قراردادهای رسمی نام‌گذاری مورد استفاده در سراسر راهکار **MachineryManagerEnterprise** را تعریف می‌کند.

نام‌گذاری یکنواخت و سازگار موارد زیر را بهبود می‌بخشد:

- خوانایی (Readability)
- قابلیت کشف (Discoverability)
- قابلیت پیمایش (Navigation)
- بازسازی کد (Refactoring)
- ارتباطات بین توسعه‌دهندگان (Communication)

نام‌گذاری بخشی از معماری نرم‌افزار تلقی می‌شود.

---

# ۲. قوانین عمومی

تمامی شناسه (Identifier)ها باید:

- تنها از زبان انگلیسی استفاده کنند.
- از نام‌های معنادار استفاده کنند.
- از به کار بردن مخفف‌ها اجتناب کنند مگر اینکه به صورت جهانی پذیرفته شده باشند.
- هدف را توضیح دهند نه جزئیات پیاده‌سازی را.
- در سراسر راهکار یکنواخت و سازگار باشند.

---

# ۳. قوانین حروف کوچک و بزرگ (Casing Rules)

| مورد | قرارداد |
|-------|------------|
| فضای نام (Namespace) | PascalCase |
| کلاس (Class) | PascalCase |
| رکورد (Record) | PascalCase |
| ساختار (Struct) | PascalCase |
| شمارشگر (Enum) | PascalCase |
| رابط (Interface) | PascalCase با پیشوند `I` |
| متد (Method) | PascalCase |
| ویژگی (Property) | PascalCase |
| متغیر محلی (Local Variable) | camelCase |
| پارامتر (Parameter) | camelCase |
| فیلد خصوصی (Private Field) | `_camelCase` |
| مقدار ثابت (Constant) | PascalCase |
| عضو Enum | PascalCase |

---

# ۴. نام‌گذاری کلاس‌ها (Class Naming)

کلاس‌ها باید نشان‌دهنده اسم (Noun) باشند.

مثال‌های خوب:

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

# ۵. نام‌گذاری رابط‌ها (Interface Naming)

رابط‌ها باید با `I` آغاز شوند.

مثال‌ها:

```text
IMachineRepository

IUserService

IClock
```

---

# ۶. نام‌گذاری متدها (Method Naming)

متدها باید نشان‌دهنده فعل یا اقدام (Action) باشند.

مثال‌ها:

```text
CreateMachine()

CalculateAvailability()

GenerateReport()
```

متدهای بولین (Boolean) باید به یک سوال پاسخ دهند.

مثال‌ها:

```text
IsActive()

HasPermission()

CanDelete()
```

---

# ۷. نام‌گذاری متغیرها (Variable Naming)

متغیرها باید هدف خود را صریحاً توضیح دهند.

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

مگر برای متغیرهای حلقه کوتاه‌مدت.

---

# ۸. مجموعه‌ها (Collections)

مجموعه‌ها باید از نام‌های جمع استفاده کنند.

مثال‌ها:

```text
machines

users

maintenancePlans
```

اشیاء تکی باید از نام‌های مفرد استفاده کنند.

---

# ۹. متغیرهای بولین (Boolean Variables)

متغیرهای بولین باید با کلماتی مانند موارد زیر آغاز شوند:

- is
- has
- can
- should

مثال‌ها:

```text
isActive

hasPermission

canEdit

shouldRetry
```

---

# ۱۰. متدهای ناهمگام (Async Methods)

متدهای ناهمگام (Asynchronous) باید به `Async` ختم شوند:

مثال:

```csharp
LoadMachinesAsync()
```

---

# ۱۱. پردازنده‌های رویداد (Event Handlers)

پردازنده‌های رویداد باید از الگوی زیر پیروی کنند:

```text
On<Event>

Handle<Event>
```

مثال‌ها:

```text
OnMachineCreated

HandleUserDeleted
```

---

# ۱۲. اشیاء انتقال داده (DTOs)

نام DTOها باید به `Dto` ختم شود:

مثال‌ها:

```text
MachineDto

UserDto

InventoryDto
```

---

# ۱۳. دستورات (Commands)

دستورات باید به `Command` ختم شوند:

مثال‌ها:

```text
CreateMachineCommand

DeleteMachineCommand
```

---

# ۱۴. پرس‌وجوها (Queries)

پرس‌وجوها باید به `Query` ختم شوند:

مثال‌ها:

```text
GetMachineQuery

SearchInventoryQuery
```

---

# ۱۵. اعتبارسنج‌ها (Validators)

اعتبارسنج‌ها باید به `Validator` ختم شوند:

مثال‌ها:

```text
CreateMachineValidator

UserValidator
```

---

# ۱۶. استثناها (Exceptions)

استثناها باید به `Exception` ختم شوند:

مثال‌ها:

```text
MachineNotFoundException

InvalidLicenseException
```

---

# ۱۷. شمارشگرها (Enumerations)

نام‌های Enum باید مفرد باشند.

مثال‌ها:

```text
MachineStatus

MaintenancePriority
```

اعضای Enum از PascalCase استفاده می‌کنند.

---

# ۱۸. جداول دیتابیس (Database Tables)

نام‌های موجودیت‌ها (Entities) مفرد باقی می‌مانند.

مثال‌ها:

```text
Machine

MaintenancePlan

InventoryItem
```

فریم‌ورک Entity Framework در صورت لزوم جمع‌بستن نام‌ها را مدیریت می‌کند.

---

# ۱۹. نام فایل‌ها (File Names)

نام فایل‌ها باید دقیقاً با تایپ عمومی (Public Type) مطابقت داشته باشد.

مثال:

```text
MachineService.cs

MachineDto.cs

CreateMachineCommand.cs
```

---

# ۲۰. انطباق فضای نام (Namespace Consistency)

فضاهای نام باید همواره با نام پوشه‌ها مطابقت داشته باشند.

مثال:

پوشه:
```text
Features/Inventory/Commands
```

فضای نام:
```text
MachineryManagerEnterprise.Application.Features.Inventory.Commands
```

---

# ۲۱. نام‌های ممنوعه (Forbidden Names)

از موارد زیر اجتناب کنید:

```text
Helper

Util

Misc

CommonStuff

Manager

Data

Info
```

نام‌ها باید به جای هدف عمومی، مسئولیت شفاف را برسانند.

---

# ۲۲. انطباق و رعایت

هر شناسه جدید واردشده به راهکار باید با این قراردادها انطباق داشته باشد.

انحرافات نیازمند تاییدیه معماری است.

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
- DOC-DEV-001 (اصول توسعه)
- DOC-DEV-004 (قراردادهای فضای نام)
- DOC-DEV-006 (استانداردهای کدنویسی)

---

# تاریخچه تغییرات

| نسخه | تاریخ | نویسنده / نقش | شرح |
|----------|------------|-------------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | معمار راهکار | قراردادهای اولیه نام‌گذاری |
| 2.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
