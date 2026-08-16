| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | TE-0022 |
| **عنوان** | پایپ‌لاین اعتبارسنجی و ارزیابی معماری اعتبارسنجی (Validation Pipeline and Validation Architecture Evaluation) (.NET 10) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-26 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند فناوری‌های کاندید برای پایپ‌لاین اعتبارسنجی و ارزیابی معماری اعتبارسنجی (.NET 10) را در پلتفرم MachineryManagerEnterprise مورد ارزیابی قرار می‌دهد.

هدف، ایجاد یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را برآورده سازد و هم‌زمان اصول معماری تمیز (Clean Architecture) را حفظ نماید.

---

# محدوده ارزیابی (Evaluation Scope)

این ارزیابی فناوری صرفاً انتخاب فناوری را مورد ارزیابی قرار می‌دهد.

جزئیات پیاده‌سازی توسط سوابق تصمیم‌گیری معماری (ADRs) مربوطه تعریف می‌شوند.

---

# ارتباط با TE-0005 (Relationship with TE-0005)

سند TE-0005 به این پرسش پاسخ می‌دهد:

> **از چه فناوری اعتبارسنجی باید استفاده کنیم؟**

پاسخ:

> **FluentValidation**

این سند به این پرسش پاسخ می‌دهد:

> **معماری اعتبارسنجی در سراسر کل راهکار چگونه باید طراحی شود؟**

بنابراین:

- سند TE-0005 معتبر باقی می‌ماند.
- سند TE-0022 کاربرد معماری فناوری انتخاب‌شده را بسط و گسترش می‌دهد.

```text
TE-0005

انتخاب فناوری (Technology Selection)

        │

        ▼

انتخاب FluentValidation

        │

        ▼

TE-0022

معماری اعتبارسنجی (Validation Architecture)
```

---

# مراجع معماری (Architectural References)

این ارزیابی بر اساس مراجع زیر استوار است:

- ADR-0001 — معماری تمیز (Clean Architecture)
- ADR-0003 — تفکیک مسئولیت کوئری و فرمان (CQRS)
- ADR-0004 — الگوی MediatR
- ADR-0007 — فریم‌ورک FluentValidation
- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md

---

# محدوده (Scope)

این سند موارد زیر را مورد ارزیابی قرار می‌دهد:

- پایپ‌لاین اعتبارسنجی (Validation Pipeline)
- چرخه حیات اعتبارسنجی (Validation Lifecycle)
- سازمان‌دهی اعتبارسنج‌ها (Validator Organization)
- استراتژی ثبت و تزریق وابستگی (Registration Strategy)
- یکپارچگی با CQRS
- یکپارچگی با MediatR
- جریان مدیریت خطا (Error Flow)
- ملاحظات کارایی و عملکرد (Performance Considerations)

قواعد کسب‌وکار به خودی خود **خارج از محدوده** این سند قرار دارند.

---

# نیازمندی‌های کارکردی (Functional Requirements)

معماری باید موارد زیر را پشتیبانی نماید:

- اعتبارسنجی خودکار (automatic validation)؛
- اعتبارسنجی فرمان‌ها (command validation)؛
- اعتبارسنجی کوئری‌ها (query validation)؛
- اعتبارسنجی DTOها (DTO validation)؛
- اعتبارسنجی ناهمگام (asynchronous validation)؛
- بومی‌سازی و چندزبانگی (localization)؛
- پشتیبانی از چندین اعتبارسنج (multiple validators)؛
- اجرای خط لوله‌ای/پایپ‌لاینی (pipeline execution)؛
- تزریق وابستگی (dependency injection).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

معماری اعتبارسنجی باید موارد زیر را فراهم آورد:

- انطباق با معماری تمیز (Clean Architecture compliance)؛
- کارایی و عملکرد بالا (high performance)؛
- قابلیت توسعه و گسترش‌پذیری (extensibility)؛
- قابلیت نگهداری (maintainability)؛
- آزمون‌پذیری (testability)؛
- اجرای قطعی و پیش‌بینی‌پذیر (deterministic execution)؛
- حداقل کد تکراری و قالبی (minimal boilerplate).

---

# فناوری‌های کاندید (Candidate Technologies)

| فناوری | هدف | وضعیت |
|------------|---------|--------|
| Selected Primary Engine | زیرساخت سازمانی (Enterprise Infrastructure) | انتخاب شده (Selected) |
| Alternative Engine | کاندیدای مقایسه (Comparison Candidate) | ارزیابی‌شده (Evaluated) |

---

# رویکردهای معماری کاندید (Candidate Architectural Approaches)

| رویکرد | شرح |
|----------|-------------|
| Controller Validation | اعتبارسنجی درون کنترلرها (Controllers) |
| Endpoint Validation | اعتبارسنجی درون اندپوینت‌ها (Endpoints) |
| MediatR Pipeline Validation | اعتبارسنجی پیش از اجرای Handler |
| Business Layer Validation | اعتبارسنجی درون Handlers |
| Domain Validation | اعتبارسنجی درون مدل دامنه (Domain Model) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | ضریب اهمیت |
|----|-----------|--------|
| V1 | معماری تمیز (Clean Architecture) | حیاتی (Critical) |
| V2 | تفکیک دغدغه‌ها (Separation of Concerns) | حیاتی (Critical) |
| V3 | آزمون‌پذیری (Testability) | حیاتی (Critical) |
| V4 | کارایی و عملکرد (Performance) | بالا (High) |
| V5 | قابلیت نگهداری (Maintainability) | بالا (High) |
| V6 | قابلیت توسعه‌پذیری (Extensibility) | بالا (High) |
| V7 | تجربه توسعه‌دهنده (Developer Experience) | بالا (High) |

---

# اصل معماری (Architecture Principle)

اعتبارسنجی یک **دغدغه لایه Application** است.

منطق کسب‌وکار هرگز نباید پیش از موفقیت کامل اعتبارسنجی اجرا شود.

---

# 6. ارزیابی اعتبارسنجی در کنترلر (Controller Validation Evaluation)

## نمای کلی (Overview)

رویکرد Controller Validation رویکرد سنتی اعتبارسنجی در ASP.NET است که در آن هر کنترلر (یا اندپوینت) صراحتاً پیش از اجرای منطق برنامه، اعتبارسنجی را فراخوانی می‌کند.

پیاده‌سازی معمول:

```csharp
var result = await validator.ValidateAsync(request);

if (!result.IsValid)
{
    return BadRequest(result.Errors);
}

await mediator.Send(request);
```

در این حالت کنترلر مسئول هماهنگی و ارکستراسیون اعتبارسنجی می‌شود.

---

# جریان معماری (Architectural Flow)

```text
HTTP Request

      │

      ▼

Controller

      │

Validation

      │

Business Logic

      │

HTTP Response
```

اعتبارسنجی درون هر اکشن از کنترلر اجرا می‌شود.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- فهم و درک ساده.
- جریان اعتبارسنجی صریح و شفاف.
- آسان برای برنامه‌های کوچک.
- بدون نیاز به زیرساخت پایپ‌لاین اضافی.

---

# نقاط ضعف معماری (Architectural Weaknesses)

رویکرد Controller Validation چندین اصل معماری اتخاذشده توسط MachineryManagerEnterprise را نقض می‌کند.

### تکرار اعتبارسنجی (Validation Duplication)

هر کنترلر موارد زیر را تکرار می‌کند:

- حل وابستگی اعتبارسنج (validator resolution)؛
- اجرای اعتبارسنجی (validation execution)؛
- مدیریت خطا (error handling).

سیستم‌های بزرگ به سرعت دچار انباشت کدهای تکراری می‌شوند.

---

### تفکیک دغدغه‌ها (Separation of Concerns)

کنترلرها مسئول موارد زیر می‌شوند:

- دغدغه‌های مربوط به پروتکل HTTP؛
- اعتبارسنجی؛
- ارکستراسیون و هماهنگی.

این امر باعث اختلاط مسئولیت‌ها می‌شود.

---

### عدم انطباق با CQRS (CQRS Inconsistency)

الگوی CQRS نیازمند جریان زیر است:

```text
Command

↓

Validation

↓

Handler
```

اما Controller Validation این جریان را به شکل زیر تغییر می‌دهد:

```text
Controller

↓

Validation

↓

Mediator

↓

Handler
```

در نتیجه، اعتبارسنجی به لایه Presentation وابسته می‌شود.

---

### دور زدن پایپ‌لاین (Pipeline Bypass)

جاب‌های پس‌زمینه (Background jobs)، رویدادهای یکپارچگی (integration events) و درخواست‌های داخلی برنامه ممکن است کنترلرها را به طور کامل دور بزنند.

در نتیجه:

- اعتبارسنجی ممکن است اجرا نشود؛
- هندلرهای کسب‌وکار ممکن است اشیاء نامعتبر دریافت کنند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

رویکرد Controller Validation نیازمند آن است که هر اندپوینت اعتبارسنجی را به صورت دستی انجام دهد.

پیچیدگی عملیاتی به نسبت تعداد اندپوینت‌ها افزایش می‌یابد.

---

# قابلیت نگهداری (Maintainability)

قابلیت نگهداری کاهش می‌یابد زیرا:

- کد اعتبارسنجی تکراری است؛
- نگاشت خطا (error mapping) تکراری است؛
- لاگ‌گیری تکراری است؛
- مدیریت استثناها تکراری است.

هزینه نگهداری بالا ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

کارایی در زمان اجرا (Runtime performance) قابل قبول است.

با این حال، بازدهی و عملکرد توسعه کاهش می‌یابد زیرا توسعه‌دهندگان مکرراً منطق اعتبارسنجی یکسانی را می‌نویسند.

---

# آزمون‌پذیری (Testability)

اعتبارسنجی را نمی‌توان مستقل از ارکستراسیون کنترلر تست کرد.

تست‌های واحد کنترلر به طور غیرضروری با رفتار اعتبارسنجی جفت (coupled) می‌شوند.

آزمون‌پذیری متوسط ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

با رشد تعداد APIها:

```text
10 Controllers

↓

10 Validation Blocks

↓

100 Controllers

↓

100 Validation Blocks
```

معماری از نظر مقیاس‌پذیری ضعیف عمل می‌کند.

---

# ارتباط با معماری تمیز (Relationship with Clean Architecture)

رویکرد Controller Validation یک وابستگی نامطلوب معرفی می‌کند:

```text
Presentation

↓

Validation

↓

Application
```

اعتبارسنجی باید متعلق به لایه Application باشد نه لایه Presentation.

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | ضعیف (Poor) |
| تفکیک دغدغه‌ها (Separation of Concerns) | ضعیف (Poor) |
| قابلیت نگهداری (Maintainability) | ضعیف (Poor) |
| مقیاس‌پذیری (Scalability) | متوسط (Moderate) |
| آزمون‌پذیری (Testability) | متوسط (Moderate) |
| تجربه توسعه‌دهنده (Developer Experience) | متوسط (Moderate) |

---

# مقایسه با اعتبارسنجی پایپ‌لاینی (Comparison with Pipeline Validation)

| معیار | Controller Validation | Pipeline Validation |
|-----------|----------------------|---------------------|
| تکرار کد (Duplication) | بالا (High) | ندارد (None) |
| اجرای خودکار (Automatic Execution) | خیر (No) | بله (Yes) |
| همسویی با CQRS | ضعیف (Poor) | عالی (Excellent) |
| قابلیت نگهداری | متوسط (Moderate) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

رویکرد Controller Validation تنها برای موارد زیر مناسب است:

- برنامه‌های بسیار کوچک؛
- نمونه‌های اولیه (prototypes)؛
- مثال‌های آموزشی.

این رویکرد برای MachineryManagerEnterprise **مناسب نیست**.

از آنجا که این پروژه موارد زیر را اتخاذ کرده است:

- معماری تمیز (Clean Architecture)؛
- تفکیک مسئولیت کوئری و فرمان (CQRS)؛
- کتابخانه MediatR؛
- معماری ماژولار؛

اعتبارسنجی باید مستقل از لایه Presentation اجرا شود.

بنابراین، رویکرد Controller Validation **رد می‌شود (rejected)**.

---

# 7. ارزیابی اعتبارسنجی در اندپوینت (Endpoint Validation Evaluation)

## نمای کلی (Overview)

رویکرد Endpoint Validation منطق اعتبارسنجی را مستقیماً درون اندپوینت‌های Minimal API (یا اندپوینت هندلرها) قرار می‌دهد، به جای آنکه درون کنترلرهای MVC قرار گیرد.

پیاده‌سازی معمول:

```csharp
app.MapPost("/machines", async (
    CreateMachineCommand command,
    IValidator<CreateMachineCommand> validator,
    ISender sender) =>
{
    var result = await validator.ValidateAsync(command);

    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());

    return await sender.Send(command);
});
```

این رویکرد در برنامه‌های مبتنی بر Minimal API به طور فزاینده‌ای رایج است.

---

# جریان معماری (Architectural Flow)

```text
HTTP Request

      │

      ▼

Minimal Endpoint

      │

Validation

      │

Mediator

      │

Handler

      │

HTTP Response
```

اعتبارسنجی پیش از ارسال درخواست به لایه Application اجرا می‌شود.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- ساده‌تر از اعتبارسنجی مبتنی بر کنترلر.
- بسیار مناسب برای پروژه‌های کوچک Minimal API.
- اعتبارسنجی صریح و شفاف باقی می‌ماند.
- درک آسان.
- عدم نیاز به زیرساخت اضافی.

---

# نقاط ضعف معماری (Architectural Weaknesses)

اگرچه کنترلرها حذف شده‌اند، اما مشکلات معماری همچنان باقی است.

### تکرار اعتبارسنجی (Validation Duplication)

هر اندپوینت موارد زیر را تکرار می‌کند:

- حل وابستگی اعتبارسنج؛
- فراخوانی اعتبارسنج؛
- مدیریت نتیجه اعتبارسنجی؛
- نگاشت خطا.

با رشد تعداد اندپوینت‌ها، تکرار کد به طور چشمگیری افزایش می‌یابد.

---

### تفکیک دغدغه‌ها (Separation of Concerns)

اندپوینت‌ها مسئول موارد زیر می‌شوند:

- مسیریابی HTTP (HTTP routing)؛
- اعتبارسنجی؛
- ارکستراسیون و هماهنگی.

این امر تفکیک مطلوب مسئولیت‌ها در پروژه را نقض می‌کند.

---

### عدم انطباق با CQRS (CQRS Misalignment)

مدل اجرای مورد نظر به صورت زیر است:

```text
Request

↓

Validation

↓

Handler
```

اما Endpoint Validation این مدل را به شکل زیر تغییر می‌دهد:

```text
Endpoint

↓

Validation

↓

Mediator

↓

Handler
```

اعتبارسنجی همچنان به لایه انتقال (transport layer) وابسته باقی می‌ماند.

---

### ناسازگاری پایپ‌لاین (Pipeline Inconsistency)

تنها درخواست‌های HTTP به طور خودکار اعتبارسنجی می‌شوند.

سایر مسیرهای اجرا، مانند:

- جاب‌های پس‌زمینه (background jobs)؛
- تسک‌های زمان‌بندی‌شده (scheduled tasks)؛
- هندلرهای رویدادهای یکپارچگی (integration event handlers)؛
- درخواست‌های داخلی برنامه (internal application requests)؛

می‌توانند اعتبارسنجی اندپوینت را به طور کامل دور بزنند.

این امر منجر به رفتاری ناسازگار می‌شود.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

منطق اعتبارسنجی باید در هر اندپوینت به صورت دستی پیاده‌سازی شود.

پیچیدگی عملیاتی به نسبت تعداد اندپوینت‌ها افزایش می‌یابد.

---

# قابلیت نگهداری (Maintainability)

قابلیت نگهداری تحت تاثیر منفی قرار می‌گیرد زیرا:

- کد اعتبارسنجی تکرار می‌شود؛
- نگاشت خطای اعتبارسنجی تکرار می‌شود؛
- رفتار پایپ‌لاین نمی‌تواند متمرکز شود.

قابلیت نگهداری ضعیف ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

کارایی در زمان اجرا قابل قبول است.

با این حال، بهره‌وری توسعه به دلیل پیاده‌سازی تکراری کاهش می‌یابد.

---

# آزمون‌پذیری (Testability)

اعتبارسنجی را نمی‌توان مستقل از ارکستراسیون اندپوینت تست کرد.

تست واحد پیچیده‌تر می‌شود زیرا دغدغه‌های لایه انتقال با رفتار اعتبارسنجی جفت شده‌اند.

آزمون‌پذیری متوسط ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

رویکرد Endpoint Validation مقیاس‌پذیری ضعیفی دارد.

مثال:

```text
20 Endpoints

↓

20 Validation Blocks

↓

300 Endpoints

↓

300 Validation Blocks
```

بار نگهداری به صورت خطی با سطح API افزایش می‌یابد.

---

# ارتباط با معماری تمیز (Relationship with Clean Architecture)

اعتبارسنجی همچنان به لایه Presentation وابسته می‌ماند:

```text
Presentation

↓

Validation

↓

Application
```

معماری ترجیحی، اعتبارسنجی را درون پایپ‌لاین Application قرار می‌دهد تا هر مسیر اجرا به طور سازگار اعتبارسنجی شود.

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | ضعیف (Poor) |
| تفکیک دغدغه‌ها (Separation of Concerns) | ضعیف (Poor) |
| قابلیت نگهداری (Maintainability) | ضعیف (Poor) |
| مقیاس‌پذیری (Scalability) | متوسط (Moderate) |
| آزمون‌پذیری (Testability) | متوسط (Moderate) |
| تجربه توسعه‌دهنده (Developer Experience) | خوب (Good) |

---

# مقایسه با اعتبارسنجی کنترلر (Comparison with Controller Validation)

| معیار | Controller Validation | Endpoint Validation |
|-----------|----------------------|---------------------|
| کدهای قالبی و تکراری (Boilerplate) | بالا (High) | متوسط (Moderate) |
| تکرار کد (Duplication) | بالا (High) | بالا (High) |
| همسویی با CQRS | ضعیف (Poor) | ضعیف (Poor) |
| سازگاری پایپ‌لاین (Pipeline Consistency) | ضعیف (Poor) | ضعیف (Poor) |

رویکرد Endpoint Validation سینتکس را بهبود می‌بخشد اما مشکلات معماری زیربنایی را حل نمی‌کند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

رویکرد Endpoint Validation برای موارد زیر مناسب است:

- پروژه‌های کوچک Minimal API؛
- نمونه‌های اولیه (prototypes)؛
- میکروسرویس‌های سبک.

این رویکرد برای MachineryManagerEnterprise **مناسب نیست**.

اگرچه سینتکس تمیزتری نسبت به Controller Validation ارائه می‌دهد، اما همچنان اهداف معماری پروژه را نقض می‌کند:

- اعتبارسنجی متمرکز نیست؛
- اعتبارسنجی تکرار می‌شود؛
- مسیرهای اجرای غیر HTTP به طور خودکار محافظت نمی‌شوند.

به این دلایل، رویکرد Endpoint Validation به عنوان معماری اصلی اعتبارسنجی **رد می‌شود (rejected)**.

---

# 8. ارزیابی اعتبارسنجی در پایپ‌لاین MediatR (MediatR Pipeline Validation Evaluation)

## نمای کلی (Overview)

رویکرد MediatR Pipeline Validation اعتبارسنجی را درون پایپ‌لاین درخواست‌های MediatR و پیش از آنکه هر Command یا Query به Handler خود برسد، اجرا می‌کند.

به جای آنکه هر کنترلر یا اندپوینت اعتبارسنج‌ها را به صورت دستی فراخوانی کند، یک Pipeline Behavior واحد، اعتبارسنجی را به طور خودکار برای هر درخواست انجام می‌دهد.

پیاده‌سازی معمول:

```text
HTTP Request

        │

        ▼

Controller / Endpoint

        │

        ▼

Mediator.Send()

        │

        ▼

ValidationBehavior<TRequest,TResponse>

        │

        ▼

FluentValidation

        │

        ▼

Handler
```

این رویکرد رایج‌ترین معماری اعتبارسنجی در سیستم‌های سازمانی مبتنی بر CQRS است.

---

# نقش معماری (Architectural Role)

اعتبارسنجی پایپ‌لاینی کاملاً به **لایه Application** تعلق دارد.

```text
Presentation

      │

      ▼

Mediator

      │

      ▼

Validation Pipeline

      │

      ▼

Command / Query Handler

      │

      ▼

Domain
```

نه کنترلرها و نه اندپوینت‌ها هیچ منطق اعتبارسنجی ندارند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- اعتبارسنجی به صورت خودکار اجرا می‌شود.
- صفر کد اعتبارسنجی تکراری.
- رفتار سازگار و یکنواخت.
- مستقل از پروتکل HTTP.
- مستقل از کنترلرها.
- مستقل از اندپوینت‌ها.
- سازگار با الگوی CQRS.
- سازگار با کتابخانه MediatR.
- سازگار با جاب‌های پس‌زمینه (Background Jobs).
- سازگار با رویدادهای یکپارچگی (Integration Events).
- آزمون‌پذیری عالی.
- قابلیت نگهداری عالی.

---

# تفکیک دغدغه‌ها (Separation of Concerns)

مسئولیت‌ها به طور شفاف تفکیک می‌شوند:

```text
Presentation

↓

Routing

↓

Mediator

↓

Validation

↓

Business Logic

↓

Persistence
```

هر لایه دقیقاً مالک یک مسئولیت است.

---

# همسویی با CQRS (CQRS Alignment)

اعتبارسنجی پایپ‌لاینی کاملاً با جریان اجرای مورد نظر در CQRS مطابقت دارد:

```text
Command

      │

      ▼

Validation

      │

      ▼

Handler

      │

      ▼

Domain
```

هیچ منطق کسب‌وکاری پیش از موفقیت اعتبارسنجی اجرا نمی‌شود.

---

# سازگاری پایپ‌لاین (Pipeline Consistency)

هر مسیر اجرا اعتبارسنجی یکسانی دریافت می‌کند.

نمونه‌ها عبارتند از:

```text
HTTP API

Desktop UI

Background Jobs

Message Bus

Scheduled Tasks

Integration Events
```

تمام آنها جریان زیر را اجرا می‌کنند:

```text
Mediator

↓

ValidationBehavior

↓

Handler
```

هیچ مسیر اجرایی اعتبارسنجی را دور نمی‌زند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

اعتبارسنج‌ها به طور خودکار از طریق تزریق وابستگی (Dependency Injection) اجرا می‌شوند.

توسعه‌دهندگان هرگز اعتبارسنج‌ها را به صورت دستی فراخوانی نمی‌کنند.

پیچیدگی عملیاتی با وجود غنای معماری، پایین است.

---

# کارایی و عملکرد (Performance)

اعتبارسنجی پایپ‌لاینی یک رفتار اضافی MediatR را معرفی می‌کند.

سربار این رفتار ناچیز است.

مزایا عبارتند از:

- حذف اعتبارسنجی تکراری؛
- اجرای متمرکز؛
- کارایی قابل پیش‌بینی.

کارایی و عملکرد عالی ارزیابی می‌شود.

---

# آزمون‌پذیری (Testability)

اعتبارسنج‌ها می‌توانند به طور مستقل تست شوند.

رفتار پایپ‌لاین می‌تواند به طور مستقل تست شود.

هندلرها می‌توانند با فرض معتبر بودن درخواست‌ها تست شوند.

این امر تست واحد را بسیار ساده می‌کند.

آزمون‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت نگهداری (Maintainability)

مزایای نگهداری عبارتند از:

- یک پایپ‌لاین اعتبارسنجی واحد؛
- عدم وجود کد تکراری؛
- مدیریت استثنای متمرکز؛
- استراتژی اعتبارسنجی متمرکز.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

اعتبارسنجی پایپ‌لاینی به طور طبیعی مقیاس می‌پذیرد:

```text
5 Commands

↓

One ValidationBehavior

↓

500 Commands

↓

Still One ValidationBehavior
```

معماری بدون کد ارکستراسیون اضافی مقیاس می‌یابد.

---

# ارتباط با FluentValidation

```text
Mediator

      │

      ▼

ValidationBehavior

      │

      ▼

FluentValidation

      │

      ▼

Handler
```

کتابخانه FluentValidation مسئول قوانین اعتبارسنجی باقی می‌ماند.

پایپ‌لاین مسئول ارکستراسیون و هماهنگی باقی می‌ماند.

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| سازگاری با CQRS | عالی (Excellent) |
| تفکیک دغدغه‌ها (Separation of Concerns) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| مقیاس‌پذیری (Scalability) | عالی (Excellent) |
| آزمون‌پذیری (Testability) | عالی (Excellent) |
| تجربه توسعه‌دهنده (Developer Experience) | عالی (Excellent) |

---

# مقایسه با رویکردهای قبلی (Comparison with Previous Approaches)

| معیار | Controller | Endpoint | MediatR Pipeline |
|-----------|------------|----------|------------------|
| اعتبارسنجی خودکار | خیر (No) | خیر (No) | بله (Yes) |
| تکرار کد | بالا (High) | بالا (High) | ندارد (None) |
| استقلال از HTTP | خیر (No) | خیر (No) | بله (Yes) |
| همسویی با CQRS | ضعیف (Poor) | ضعیف (Poor) | عالی (Excellent) |
| قابلیت نگهداری | متوسط (Moderate) | متوسط (Moderate) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

رویکرد MediatR Pipeline Validation تمامی اهداف معماری MachineryManagerEnterprise را برآورده می‌سازد.

این رویکرد موارد زیر را فراهم می‌آورد:

- اعتبارسنجی متمرکز؛
- اجرای خودکار؛
- استقلال از لایه انتقال؛
- همسویی عالی با CQRS؛
- قابلیت نگهداری عالی.

بنابراین، این رویکرد به عنوان **معماری اصلی اعتبارسنجی** برای راهکار **تصویب می‌شود (approved)**.

---

# 9. ارزیابی اعتبارسنجی در لایه کسب‌وکار (Business Layer Validation Evaluation)

## نمای کلی (Overview)

رویکرد Business Layer Validation اعتبارسنجی را مستقیماً درون هندلرهای Command یا Query اجرا می‌کند.

پیاده‌سازی معمول:

```csharp
public async Task<Result> Handle(CreateMachineCommand request, CancellationToken ct)
{
    if (string.IsNullOrWhiteSpace(request.Name))
        throw new ValidationException(...);

    if (request.Price <= 0)
        throw new ValidationException(...);

    ...
}
```

منطق اعتبارسنجی بخشی از جریان اجرای کسب‌وکار می‌شود.

---

# جریان معماری (Architectural Flow)

```text
Command

      │

      ▼

Handler

      │

Validation

      │

Business Logic

      │

Persistence
```

برخلاف اعتبارسنجی پایپ‌لاینی، اعتبارسنجی پس از رسیدن درخواست به هندلر رخ می‌دهد.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- پیاده‌سازی ساده.
- عدم نیاز به زیرساخت پایپ‌لاین.
- نزدیکی اعتبارسنجی به منطق کسب‌وکار.
- درک آسان در برنامه‌های بسیار کوچک.

---

# نقاط ضعف معماری (Architectural Weaknesses)

این رویکرد چندین اصل معماری اتخاذشده توسط MachineryManagerEnterprise را نقض می‌کند.

---

## اختلاط مسئولیت‌ها (Mixed Responsibilities)

هندلرها مسئول موارد زیر می‌شوند:

- اعتبارسنجی؛
- منطق کسب‌وکار؛
- ارکستراسیون و هماهنگی.

مثال:

```text
Handler

├── Validation

├── Authorization

├── Business Rules

└── Persistence
```

اصل تک‌مسئولیتی (Single Responsibility Principle) نقض می‌شود.

---

## تکرار اعتبارسنجی (Validation Duplication)

هر Handler موارد زیر را تکرار می‌کند:

- بندهای محافظ (guard clauses)؛
- منطق اعتبارسنجی؛
- ایجاد استثناها؛
- قالب‌بندی خطاها.

با افزایش تعداد هندلرها، کدهای تکراری اعتبارسنجی به سرعت رشد می‌کنند.

---

## تنزل کیفیت CQRS (CQRS Degradation)

جریان مورد نظر CQRS عبارت است از:

```text
Command

↓

Validation

↓

Handler
```

اما Business Layer Validation این جریان را به شکل زیر تغییر می‌دهد:

```text
Command

↓

Handler

├── Validation

└── Business Logic
```

هندلرهای کسب‌وکار اکنون باید از نحوه کارکرد اعتبارسنجی مطلع باشند.

---

## قابلیت نگهداری (Maintainability)

قوانین اعتبارسنجی در میان هندلرهای متعدد پراکنده می‌شوند.

تغییر یک خط‌مشی اعتبارسنجی مشترک نیازمند اصلاح در چندین مکان است.

بنابراین هزینه نگهداری بالاست.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

اعتبارسنجی تنها زمانی اجرا می‌شود که Handler شروع به کار کند.

هندلر نمی‌تواند فرض کند که درخواست‌های ورودی معتبر هستند.

این امر پیچیدگی پیاده‌سازی را افزایش می‌دهد.

---

# کارایی و عملکرد (Performance)

کارایی در زمان اجرا قابل قبول است.

با این حال:

- ساخت غیرضروری هندلر؛
- حل وابستگی غیرضروری؛
- کدهای اعتبارسنجی تکراری؛

نسبت به اعتبارسنجی پایپ‌لاینی کارایی را اندکی کاهش می‌دهند.

---

# آزمون‌پذیری (Testability)

تست‌های منطق کسب‌وکار با رفتار اعتبارسنجی جفت می‌شوند.

توسعه‌دهندگان باید:

- قبل از تست منطق کسب‌وکار، تک‌تک قوانین اعتبارسنجی را برآورده سازند؛

یا

- رفتار اعتبارسنجی را ماک (mock) کنند.

این امر تست واحد را پیچیده می‌کند.

آزمون‌پذیری متوسط ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

با افزایش هندلرها:

```text
20 Handlers

↓

20 Validation Implementations

↓

400 Handlers

↓

400 Validation Implementations
```

معماری از نظر مقیاس‌پذیری ضعیف عمل می‌کند.

---

# ارتباط با FluentValidation

استفاده از FluentValidation درون هندلرها معمولاً منجر به فراخوانی زیر در هر هندلر می‌شود:

```csharp
await validator.ValidateAsync(request);
```

اگرچه FluentValidation مجدداً استفاده می‌شود، اما ارکستراسیون همچنان تکراری باقی می‌ماند.

---

# ارتباط با معماری تمیز (Relationship with Clean Architecture)

اعتبارسنجی درون Use Caseهای برنامه تعبیه می‌شود:

```text
Application

↓

Handler

├── Validation

└── Business Logic
```

معماری ترجیحی این دغدغه‌ها را از یکدیگر تفکیک می‌نماید.

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | ضعیف (Poor) |
| تفکیک دغدغه‌ها (Separation of Concerns) | ضعیف (Poor) |
| قابلیت نگهداری (Maintainability) | متوسط (Moderate) |
| مقیاس‌پذیری (Scalability) | متوسط (Moderate) |
| آزمون‌پذیری (Testability) | متوسط (Moderate) |
| همسویی با CQRS | ضعیف (Poor) |

---

# مقایسه با پایپ‌لاین MediatR (Comparison with MediatR Pipeline)

| معیار | Business Layer | Pipeline |
|-----------|----------------|----------|
| اعتبارسنجی خودکار | خیر (No) | بله (Yes) |
| سادگی هندلر | ضعیف (Poor) | عالی (Excellent) |
| تکرار کد | بالا (High) | ندارد (None) |
| آزمون‌پذیری | متوسط (Moderate) | عالی (Excellent) |
| همسویی با CQRS | ضعیف (Poor) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

رویکرد Business Layer Validation تنها برای موارد زیر قابل قبول است:

- برنامه‌های بسیار کوچک؛
- نمونه‌های اولیه؛
- سیستم‌های ساده CRUD.

برای MachineryManagerEnterprise این رویکرد موارد زیر را به همراه دارد:

- منطق اعتبارسنجی تکراری؛
- اختلاط مسئولیت‌ها؛
- کاهش قابلیت نگهداری؛
- همسویی ضعیف‌تر با CQRS.

بنابراین، رویکرد Business Layer Validation **رد می‌شود (rejected)**.

هندلرهای کسب‌وکار همواره باید فرض کنند که درخواست‌های ورودی قبلاً توسط پایپ‌لاین اعتبارسنجی MediatR با موفقیت اعتبارسنجی شده‌اند.

---

# 10. ارزیابی اعتبارسنجی دامنه (Domain Validation Evaluation)

## نمای کلی (Overview)

رویکرد Domain Validation پایداری و تغییرناپذیری‌های ذاتی (invariants) مدل دامنه را اعمال می‌کند.

برخلاف Application Validation که درخواست‌های ورودی را اعتبارسنجی می‌کند، Domain Validation از صحت موجودیت‌های تجاری و اشیاء مقدار (Value Objects) صرف‌نظر از مکان ایجاد آنها محافظت می‌نماید.

نمونه‌های متداول عبارتند از:

- وضعیت نامعتبر موجودیت؛
- ساخت نامعتبر شیء مقدار؛
- نقض ناوردایی‌های تجمیع (aggregate invariants)؛
- تغییرات غیرمجاز وضعیت.

بنابراین، اعتبارسنجی دامنه **جایگزینی** برای اعتبارسنجی برنامه نیست.

این رویکرد آخرین خط دفاعی برای صحت و سلامت دامنه است.

---

# جریان معماری (Architectural Flow)

```text
Request

      │

      ▼

Application Validation

      │

      ▼

Command Handler

      │

      ▼

Domain Entity

      │

Domain Validation

      │

      ▼

Valid Aggregate
```

اعتبارسنجی لایه Application از ورود درخواست‌های نامعتبر جلوگیری می‌کند.

اعتبارسنجی لایه Domain داشتن مدل‌های تجاری معتبر را تضمین می‌نماید.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- محافظت از ناوردایی‌های دامنه (domain invariants).
- مستقل از پروتکل انتقال.
- مستقل از رابط کاربری (UI).
- مستقل از لایه پایداری داده‌ها (persistence).
- جلوگیری از ایجاد موجودیت‌های نامعتبر.
- پشتیبانی از زبان فراگیر (Ubiquitous Language).
- تشویق به مدل‌های غنی دامنه (Rich Domain Models).
- تضمین صحت و انطباق قوانین کسب‌وکار.

---

# ناوردایی‌های دامنه (Domain Invariants)

نمونه‌ها عبارتند از:

```text
شماره سریال دستگاه نمی‌تواند خالی باشد.

بازه تعمیر و نگهداری نمی‌تواند منفی باشد.

تاریخ خرید نمی‌تواند پس از تاریخ اسقاط باشد.

واحد پولی باید همواره وجود داشته باشد.

مبلغ پول نمی‌تواند مقدار منفی داشته باشد (زمانی که قوانین کسب‌وکار اقتضا می‌کند).

ریشه تجمیع (Aggregate Root) هرگز نباید وارد وضعیت نامعتبر شود.
```

این قوانین متعلق به لایه Domain هستند.

---

# تفکیک مسئولیت‌ها (Separation of Responsibilities)

اعتبارسنجی لایه Application به این سوال پاسخ می‌دهد:

```text
آیا این درخواست از نظر ساختاری معتبر و قابل پردازش است؟
```

اعتبارسنجی لایه Domain به این سوال پاسخ می‌دهد:

```text
آیا این شیء کسب‌وکار می‌تواند وجود داشته باشد؟
```

این‌ها دو مسئولیت بنیادین و متفاوت هستند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

اعتبارسنجی دامنه در موارد زیر اجرا می‌شود:

- هنگام ساخت موجودیت؛
- هنگام تغییر وضعیت تجمیع؛
- هنگام عملیات کسب‌وکار.

دور زدن آن غیرممکن است زیرا درون مدل دامنه تعبیه شده است.

---

# قابلیت نگهداری (Maintainability)

قوانین کسب‌وکار متمرکز باقی می‌مانند.

مثال:

```text
Machine

├── Constructor

├── ChangeStatus()

├── RegisterMaintenance()

└── Domain Invariants
```

هیچ تکراری رخ نمی‌دهد.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

اعتبارسنجی دامنه تنها زمانی اجرا می‌شود که اشیاء دامنه تغییر کنند.

هزینه کارایی آن ناچیز است.

کارایی و عملکرد عالی ارزیابی می‌شود.

---

# آزمون‌پذیری (Testability)

قوانین دامنه می‌توانند به طور مستقل تست شوند.

مثال:

```text
MachineTests

↓

CreateInvalidMachine

↓

Expect DomainException
```

هیچ زیرساخت HTTP یا MediatR مورد نیاز نیست.

آزمون‌پذیری عالی ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

هر Aggregate از خود محافظت می‌کند:

```text
Machine

Vehicle

Warehouse

MaintenanceOrder

Inventory

Supplier
```

هر تجمیع مالک ناوردایی‌های خود است.

معماری به طور طبیعی مقیاس‌پذیر است.

---

# ارتباط با معماری تمیز (Relationship with Clean Architecture)

اعتبارسنجی دامنه کاملاً درون لایه Domain قرار دارد:

```text
Domain

↓

Entity

↓

Invariant
```

بدون هیچ وابستگی به:

- ASP.NET Core؛
- MediatR؛
- FluentValidation؛
- HTTP؛
- زیرساخت (Infrastructure).

این امر خالص‌ترین پیاده‌سازی معماری تمیز را به نمایش می‌گذارد.

---

# ارتباط با FluentValidation

این فناوری‌ها مکمل یکدیگر هستند:

```text
Application

↓

FluentValidation

↓

Command

↓

Handler

↓

Domain Entity

↓

Domain Validation
```

کتابخانه FluentValidation هرگز جایگزین ناوردایی‌های دامنه نمی‌شود.

مشابهاً، ناوردایی‌های دامنه هرگز نباید فرمت‌بندی DTOها را اعتبارسنجی کنند.

---

# مقایسه با اعتبارسنجی برنامه (Comparison with Application Validation)

| معیار | Application Validation | Domain Validation |
|-----------|------------------------|-------------------|
| اعتبارسنجی DTO | عالی (Excellent) | خیر (No) |
| ناوردایی‌های کسب‌وکار | خیر (No) | عالی (Excellent) |
| استقلال از لایه انتقال | بله (Yes) | بله (Yes) |
| جلوگیری از درخواست‌های نامعتبر | عالی (Excellent) | متوسط (Moderate) |
| محافظت از مدل دامنه | متوسط (Moderate) | عالی (Excellent) |

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| تفکیک دغدغه‌ها (Separation of Concerns) | عالی (Excellent) |
| صحت کسب‌وکار (Business Correctness) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| مقیاس‌پذیری (Scalability) | عالی (Excellent) |
| آزمون‌پذیری (Testability) | عالی (Excellent) |

---

# استراتژی پیشنهادی اعتبارسنجی دامنه (Recommended Domain Validation Strategy)

پروژه باید مدل اعتبارسنجی چندلایه‌ای زیر را اتخاذ نماید:

```text
Incoming Request

        │

        ▼

FluentValidation

(Application Layer)

        │

        ▼

Command Handler

        │

        ▼

Domain Entity

        │

Domain Invariants

        │

        ▼

Valid Aggregate
```

این امر تضمین می‌کند:

- درخواست‌های نامعتبر هرگز به منطق کسب‌وکار نمی‌رسند؛
- موجودیت‌های نامعتبر هرگز نمی‌توانند وجود داشته باشند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

اعتبارسنجی دامنه الزامی و اجباری است.

این اعتبارسنجی مکمل اعتبارسنجی برنامه است—نه جایگزین آن.

برای MachineryManagerEnterprise معماری اعتبارسنجی پیشنهادی عبارت است از:

- **Application Validation → FluentValidation**
- **Domain Validation → Aggregate Invariants**

هر دو لایه برای دستیابی به یک معماری سازمانی قدرتمند و مقاوم مورد نیاز هستند.

---

# 11. مقایسه جامع معماری (Overall Architecture Comparison)

اعتبارسنجی در MachineryManagerEnterprise به چندین لایه معماری تقسیم می‌شود.

هر لایه یک مسئولیت متمایز دارد.

اعتبارسنجی یک فعالیت منفرد و تکی **نیست**.

بلکه، یک دغدغه معماری چندلایه‌ای است.

---

# لایه‌های اعتبارسنجی (Validation Layers)

```text
Incoming Request

        │

        ▼

Application Validation

(FluentValidation)

        │

        ▼

Mediator Pipeline

        │

        ▼

Command Handler

        │

        ▼

Domain Validation

        │

        ▼

Persistence
```

هر لایه از لایه بعدی محافظت می‌نماید.

---

# ماتریس مسئولیت معماری (Architectural Responsibility Matrix)

| لایه | مسئولیت | فناوری |
|--------|----------------|------------|
| Presentation | دریافت درخواست (Receive Request) | ASP.NET Core |
| Application | اعتبارسنجی درخواست (Validate Request) | FluentValidation |
| Application | ارکستراسیون اعتبارسنجی (Validation Orchestration) | MediatR Pipeline |
| Domain | محافظت از ناوردایی‌های کسب‌وکار (Protect Business Invariants) | Domain Model |
| Infrastructure | پایداری داده‌ها (Persistence) | EF Core |

---

# مقایسه کاندیداها (Candidate Comparison)

| معیار | Controller | Endpoint | Business Layer | MediatR Pipeline | Domain Validation |
|-----------|------------|----------|----------------|------------------|-------------------|
| اعتبارسنجی خودکار | خیر (No) | خیر (No) | خیر (No) | بله (Yes) | نامربوط (N/A) |
| تکرار کد | بالا (High) | بالا (High) | بالا (High) | ندارد (None) | ندارد (None) |
| معماری تمیز | ضعیف (Poor) | ضعیف (Poor) | ضعیف (Poor) | عالی (Excellent) | عالی (Excellent) |
| سازگاری با CQRS | ضعیف (Poor) | ضعیف (Poor) | متوسط (Moderate) | عالی (Excellent) | عالی (Excellent) |
| تفکیک دغدغه‌ها | ضعیف (Poor) | متوسط (Moderate) | ضعیف (Poor) | عالی (Excellent) | عالی (Excellent) |
| آزمون‌پذیری | متوسط (Moderate) | متوسط (Moderate) | متوسط (Moderate) | عالی (Excellent) | عالی (Excellent) |
| مقیاس‌پذیری | متوسط (Moderate) | متوسط (Moderate) | متوسط (Moderate) | عالی (Excellent) | عالی (Excellent) |
| آمادگی سازمانی | پایین (Low) | پایین (Low) | متوسط (Moderate) | عالی (Excellent) | عالی (Excellent) |

---

# تفکیک مسئولیت‌ها (Responsibility Separation)

معماری پیشنهادی به وضوح دو دسته مختلف از اعتبارسنجی را تفکیک می‌کند:

## اعتبارسنجی برنامه (Application Validation)

هدف:

```text
آیا این درخواست قابل پردازش است؟
```

قوانین معمول:

- فیلدهای الزامی (Required fields)
- طول رشته‌ها (Length)
- بازه مقادیر (Range)
- فرمت و قالب داده‌ها (Format)
- سازگاری و یکپارچگی DTO
- بررسی‌های متقابل میان ویژگی‌ها (Cross-property checks)

فناوری:

```text
FluentValidation
```

---

## اعتبارسنجی دامنه (Domain Validation)

هدف:

```text
آیا این شیء کسب‌وکار می‌تواند وجود داشته باشد؟
```

قوانین معمول:

- ناوردایی‌های تجمیع (Aggregate invariants)
- پایداری و یکپارچگی موجودیت (Entity consistency)
- اعتبار شیء مقدار (Value Object validity)
- صحت کسب‌وکار (Business correctness)
- تغییرات غیرمجاز وضعیت (Illegal state transitions)

فناوری:

```text
Domain Model
```

---

# ترتیب اجرای اعتبارسنجی (Validation Execution Order)

هیئت بازنگری معماری (Architecture Review Board) ترتیب اجرای زیر را توصیه می‌نماید:

```text
Request

      │

      ▼

FluentValidation

      │

      ▼

ValidationBehavior

      │

      ▼

Handler

      │

      ▼

Domain Invariants

      │

      ▼

Repository
```

هر لایه تنها دغدغه‌های تحت مالکیت خود را اعتبارسنجی می‌کند.

---

# انتشار و مدیریت خطا (Error Propagation)

شکست در اعتبارسنجی باید بلافاصله پردازش را متوقف سازد:

```text
Request

↓

Validation Failed

↓

Validation Exception

↓

Problem Details

↓

HTTP Response
```

منطق کسب‌وکار هرگز نباید پس از شکست در اعتبارسنجی اجرا شود.

---

# مقایسه کارایی و عملکرد (Performance Comparison)

| معماری | هزینه زمان اجرا (Runtime Cost) | هزینه زمان توسعه (Development Cost) |
|--------------|-------------|------------------|
| Controller Validation | پایین (Low) | بالا (High) |
| Endpoint Validation | پایین (Low) | بالا (High) |
| Business Validation | متوسط (Moderate) | بالا (High) |
| Pipeline Validation | بسیار پایین (Very Low) | بسیار پایین (Very Low) |
| Domain Validation | ناچیز (Negligible) | پایین (Low) |

---

# مقایسه قابلیت نگهداری (Maintainability Comparison)

| معماری | قابلیت نگهداری (Maintainability) |
|--------------|----------------|
| Controller Validation | ضعیف (Poor) |
| Endpoint Validation | ضعیف (Poor) |
| Business Layer Validation | متوسط (Moderate) |
| MediatR Pipeline | عالی (Excellent) |
| Domain Validation | عالی (Excellent) |

---

# ارزیابی معماری تمیز (Clean Architecture Assessment)

معماری انتخاب‌شده جریان وابستگی‌ها را به درستی دنبال می‌کند:

```text
Presentation

      │

      ▼

Application

      │

      ▼

Domain

      │

      ▼

Infrastructure
```

مسئولیت‌های اعتبارسنجی کاملاً با مرزهای معماری همسو هستند.

هیچ لایه‌ای دغدغه‌های متعلق به لایه دیگر را اعتبارسنجی نمی‌کند.

---

# آمادگی برای هوش مصنوعی (AI Readiness)

اعتبارسنجی متمرکز، یکپارچگی آینده با هوش مصنوعی را ارتقاء می‌دهد:

نمونه‌ها:

- فرمان‌های تولیدشده توسط هوش مصنوعی (AI-generated Commands)
- دستیارهای هوش مصنوعی (AI Assistants)
- جاب‌های پس‌زمینه هوش مصنوعی (Background AI Jobs)
- ارتباط عامل با عامل (Agent-to-Agent Communication)

هر درخواست—صرف‌نظر از منبع آن—از همان پایپ‌لاین اعتبارسنجی یکپارچه عبور می‌کند.

---

# آمادگی سازمانی (Enterprise Readiness)

معماری ترکیبی موارد زیر را پشتیبانی می‌کند:

- REST APIs
- Desktop Clients
- Background Workers
- Message Bus
- سرویس‌های آینده هوش مصنوعی (Future AI Services)
- کلاینت‌های آینده موبایل (Future Mobile Clients)

بدون اینکه رفتار اعتبارسنجی تغییر یابد.

---

# خلاصه معماری (Architecture Summary)

بنابراین معماری ترجیحی اعتبارسنجی عبارت است از:

```text
Presentation

↓

FluentValidation

↓

ValidationBehavior

↓

Handler

↓

Domain Invariants

↓

Persistence
```

این معماری موارد زیر را فراهم می‌آورد:

- صفر کد اعتبارسنجی تکراری؛
- اجرای قطعی و پیش‌بینی‌پذیر؛
- قابلیت نگهداری عالی؛
- استقلال از پروتکل و لایه انتقال؛
- مقیاس‌پذیری سازمانی؛
- انطباق کامل با معماری تمیز (Clean Architecture).

---

# 12. توصیه نهایی (Final Recommendation)

پس از ارزیابی تمامی معماری‌های کاندید اعتبارسنجی، هیئت بازنگری معماری اتخاذ یک **معماری اعتبارسنجی چندلایه‌ای** را توصیه می‌نماید.

هر لایه مسئولیتی کاملاً تعریف‌شده دارد.

---

# معماری اعتبارسنجی پیشنهادی (Recommended Validation Architecture)

| لایه | مسئولیت | فناوری انتخاب‌شده |
|--------|----------------|---------------------|
| Request Validation | اعتبارسنجی DTOها و Commandها | FluentValidation |
| Validation Orchestration | اجرای خودکار اعتبارسنجی | MediatR Pipeline Behavior |
| Business Validation | محافظت از ناوردایی‌های تجمیع | Domain Model |
| Persistence Validation | قیود پایگاه داده (Database Constraints) | EF Core / Database |

---

# پایپ‌لاین اعتبارسنجی پیشنهادی (Recommended Validation Pipeline)

```text
HTTP Request

        │

        ▼

Model Binding

        │

        ▼

FluentValidation

        │

        ▼

ValidationBehavior

        │

        ▼

Command / Query Handler

        │

        ▼

Domain Aggregate

        │

        ▼

Domain Invariants

        │

        ▼

Repository

        │

        ▼

Database Constraints
```

هر درخواستی که وارد برنامه می‌شود باید از این ترتیب اجرا پیروی نماید.

---

# مسئولیت‌ها (Responsibilities)

## کتابخانه FluentValidation

مسئول اعتبارسنجی موارد زیر است:

- فیلدهای الزامی؛
- یکپارچگی و سازگاری DTO؛
- فرمت‌ها؛
- بازه‌ها؛
- اعتبارسنجی متقابل ویژگی‌ها؛
- ورودی لایه Application.

این فریم‌ورک **نباید** ناوردایی‌های دامنه را اعتبارسنجی کند.

---

## پایپ‌لاین MediatR (MediatR Pipeline)

مسئول موارد زیر است:

- کشف خودکار اعتبارسنج‌ها (automatic validator discovery)؛
- اجرای تمام اعتبارسنج‌ها؛
- متوقف ساختن اجرا در صورت بروز خطا؛
- تضمین دریافت تنها درخواست‌های معتبر توسط هندلرها.

هیچ Handlerای نباید اعتبارسنج‌ها را به صورت دستی فراخوانی کند.

---

## هندلر فرمان / کوئری (Command / Query Handler)

هندلرها باید فرض نمایند:

```text
درخواست ورودی از قبل معتبر است.
```

هندلرها منحصراً بر موارد زیر تمرکز دارند:

- ارکستراسیون کسب‌وکار؛
- موارد کاربرد برنامه (Application Use Cases)؛
- تعامل با لایه Domain.

---

## مدل دامنه (Domain Model)

مدل دامنه مسئول موارد زیر باقی می‌ماند:

- یکپارچگی تجمیع (aggregate consistency)؛
- اعتبار موجودیت (entity validity)؛
- صحت شیء مقدار (value object correctness)؛
- ناوردایی‌های کسب‌وکار (business invariants).

این اعتبارسنجی‌ها صرف‌نظر از منبع درخواست اجرا می‌شوند.

---

# قوانین معماری (Architectural Rules)

قوانین زیر برای MachineryManagerEnterprise اجباری و الزامی هستند:

## قانون ۱ (Rule 1)

کنترلرها و اندپوینت‌های Minimal API هرگز نباید اعتبارسنجی را به صورت دستی اجرا کنند.

---

## قانون ۲ (Rule 2)

تمامی Commandها و Queryها باید از طریق پایپ‌لاین اعتبارسنجی MediatR اعتبارسنجی شوند.

---

## قانون ۳ (Rule 3)

هندلرها هرگز نباید شامل منطق اعتبارسنجی تکراری باشند.

---

## قانون ۴ (Rule 4)

موجودیت‌های دامنه همواره باید از ناوردایی‌های خود محافظت نمایند.

---

## قانون ۵ (Rule 5)

قواعد کسب‌وکار هرگز نباید درون اعتبارسنج‌های FluentValidation پیاده‌سازی شوند.

اعتبارسنج‌ها ورودی برنامه را اعتبارسنجی می‌کنند.

دامنه، صحت کسب‌وکار را اعتبارسنجی می‌نماید.

---

# مزایا (Benefits)

معماری انتخاب‌شده موارد زیر را فراهم می‌سازد:

- اعتبارسنجی خودکار؛
- صفر کد اعتبارسنجی تکراری؛
- اجرای قطعی و پیش‌بینی‌پذیر؛
- استقلال از لایه انتقال؛
- رفتار سازگار و یکنواخت؛
- قابلیت نگهداری عالی؛
- مقیاس‌پذیری عالی؛
- همسویی کامل با CQRS.

---

# آمادگی سازمانی (Enterprise Readiness)

این معماری از تمامی مسیرهای اجرا پشتیبانی می‌کند:

```text
REST API

Desktop UI

Background Workers

Scheduled Jobs

Message Bus

Future AI Services
```

تمام آنها رفتار اعتبارسنجی کاملاً یکسانی را دریافت می‌نمایند.

---

# آمادگی برای هوش مصنوعی (AI Readiness)

از آنجا که اعتبارسنجی متمرکز است، فرمان‌های تولیدشده در آینده توسط هوش مصنوعی یا عامل‌های خودکار (Autonomous Agents) به طور خودکار همان ضمانت‌های اعتبارسنجی کاربران انسانی را به ارث می‌برند.

هیچ مسیر اعتبارسنجی خاصی برای هوش مصنوعی مورد نیاز نیست.

---

# مقایسه جامع فناوری (Overall Technology Comparison)

فناوری انتخاب‌شده کارایی بهینه، قابلیت نگهداری قوی و همسویی بومی با معماری تمیز .NET 10 را فراهم می‌سازد.

## ماتریس مسئولیت (Responsibility Matrix)

| مسئولیت | فناوری پیشنهادی | گزینه جایگزین |
|-----------------|------------------------|-------------|
| قابلیت معماری (Architectural Capability) | فناوری اصلی (Primary Technology) | گزینه جایگزین موروثی (Legacy Alternative) |

## مقایسه قابلیت‌ها (Capability Comparison)

| قابلیت | فناوری اصلی (Primary Technology) | گزینه جایگزین (Alternative) |
|------------|--------------------|-------------|
| کارایی و عملکرد (Performance) | عالی (Excellent) | خوب (Good) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) | متوسط (Fair) |

---

# توصیه نهایی (Final Recommendation)

اتخاذ فناوری اصلی انتخاب‌شده به عنوان استاندارد رسمی برای MachineryManagerEnterprise.

---

# تصمیم نهایی (Final Decision)

| کاندیدا | تصمیم |
|-----------|----------|
| Controller Validation | رد شد (Rejected) |
| Endpoint Validation | رد شد (Rejected) |
| Business Layer Validation | رد شد (Rejected) |
| MediatR Pipeline Validation | تصویب شد (Approved) |
| Domain Validation | تصویب شد (Approved) |

---

# سوابق تصمیمات معماری مرتبط (Related Architecture Decision)

- ADR-0036 — معماری پایپ‌لاین اعتبارسنجی (Validation Pipeline Architecture)

---

# خلاصه تصمیمات (Decision Summary)

معماری اعتبارسنجی تصویب‌شده متشکل از موارد زیر است:

- **FluentValidation** برای اعتبارسنجی ورودی برنامه (انتخاب‌شده در TE-0005)؛
- **MediatR Validation Pipeline** برای اجرای خودکار؛
- **Domain Invariants** برای تضمین صحت کسب‌وکار.

این معماری موارد زیر را برآورده می‌سازد:

- ✔ Clean Architecture
- ✔ CQRS
- ✔ MediatR
- ✔ .NET 10
- ✔ High Scalability
- ✔ Maintainability
- ✔ Testability
- ✔ Enterprise Readiness
- ✔ AI Readiness

بر این اساس، این مدل اعتبارسنجی چندلایه‌ای به عنوان استاندارد اعتبارسنجی سازمانی برای MachineryManagerEnterprise اتخاذ می‌گردد.

---

# سوابق تصمیم‌گیری معماری مرتبط (Related ADR)

- ADR-0001 — معماری تمیز (Clean Architecture)
- ADR-0015 — معماری استقرار (Deployment Architecture)

---

# اسناد مرتبط (Related Documents)

- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md

---

# ارتباط با TE-0005 (Relationship to TE-0005)

این سند مکمل **TE-0005** است.

| سند | هدف |
|----------|---------|
| **TE-0005** | فناوری اعتبارسنجی (FluentValidation) را انتخاب می‌کند |
| **TE-0022** | نحوه معماری و اجرای اعتبارسنجی را در سراسر راهکار تعریف می‌نماید |

هر دو سند معتبر باقی می‌مانند و باید با یکدیگر مطالعه شوند.

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | شرح |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0 | 2026-07-26 | معمار راهکار (Solution Architect) | ارزیابی اولیه فناوری برای پایپ‌لاین اعتبارسنجی و معماری اعتبارسنجی |
| 1.3.0 | 2026-07-28 | معمار راهکار (Solution Architect) | افزودن بخش جدید محدوده ارزیابی (Evaluation Scope) |
| 4.0.0 | 2026-07-28 | معمار راهکار (Solution Architect) | ارتقاء به استاندارد مستندسازی نگارش v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار (Solution Architect) | بازنگری و همگام‌سازی با آخرین تغییرات |