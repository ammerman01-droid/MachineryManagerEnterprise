| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0018 |
| **عنوان** | ارزیابی فناوری مدیریت پیکربندی و اسرار (.NET 10) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-26 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند به ارزیابی فناوری‌های کاندید برای مدیریت پیکربندی و اسرار (Configuration and Secrets Management Technology Evaluation (.NET 10)) در سامانه MachineryManagerEnterprise می‌پردازد.

هدف، دستیابی به یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را برآورده ساخته و در عین حال اصول معماری پاک (Clean Architecture) را کاملاً حفظ نماید.

---

# محدوده ارزیابی (Evaluation Scope)

این ارزیابی فناوری صرفاً انتخاب فناوری را مورد سنجش قرار می‌دهد.
جزئیات پیاده‌سازی توسط سوابق تصمیمات معماری (ADRs) مربوطه تعریف می‌شوند.

---

# مراجع معماری (Architectural References)

این ارزیابی بر پایه موارد زیر استوار است:
- ADR-0001 — معماری پاک (Clean Architecture)
- ADR-0015 — معماری استقرار (Deployment Architecture)
- ADR-0018 — معماری یکپارچه‌سازی خارجی (External Integration Architecture)

مدیریت پیکربندی باید همواره به صورت زیر باقی بماند:
- مستقل از ارائه‌دهنده (provider independent)؛
- مستقل از استقرار (deployment independent)؛
- بی‌طرف نسبت به ابر (cloud neutral)؛
- امن به صورت پیش‌فرض (secure by default).

---

# نیازمندی‌های کارکردی (Functional Requirements)

این پلتفرم نیازمند پشتیبانی از موارد زیر است:
- پیکربندی برنامه (application configuration)؛
- پیکربندی مختص به محیط (environment-specific configuration)؛
- بازخوانی مجدد پیکربندی در زمان اجرا (runtime configuration reload)؛
- پیکربندی با نوع‌بندی قوی (strongly typed configuration)؛
- ذخیره‌سازی امن اسرار (secure secret storage)؛
- رشته‌های اتصال (connection strings)؛
- کلیدهای API (مانند API keys)؛
- اعتبارسنجی‌های ارائه‌دهندگان هوش مصنوعی (AI provider credentials)؛
- کلیدهای امضای JWT (مانند JWT signing keys)؛
- پیکربندی گواهی‌نامه‌ها (certificate configuration)؛
- پرچم‌های قابلیت (feature flags).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

راهکار انتخابی باید موارد زیر را فراهم آورد:
- امنیت در سطح سازمانی (enterprise security)؛
- مقیاس‌پذیری (scalability)؛
- انعطاف‌پذیری استقرار (deployment flexibility)؛
- بی‌طرفی نسبت به ابر (cloud neutrality)؛
- قابلیت نگهداری (maintainability)؛
- سادگی عملیاتی (operational simplicity)؛
- سازگاری کامل با .NET 10.

---

# فناوری‌های کاندید (Candidate Technologies)

## انتزاع پیکربندی (Configuration Abstraction)

| فناوری | نقش |
|---|---|
| Microsoft.Extensions.Configuration | انتزاع پیکربندی (Configuration Abstraction) |
| Microsoft.Extensions.Options | پیکربندی با نوع‌بندی قوی (Strongly Typed Configuration) |

---

## منابع پیکربندی (Configuration Sources)

| فناوری | نقش |
|---|---|
| appsettings.json | پیکربندی پیش‌فرض (Default Configuration) |
| appsettings.{Environment}.json | پیکربندی محیطی (Environment Configuration) |
| متغیرهای محیطی (Environment Variables) | پیکربندی استقرار (Deployment Configuration) |
| خط فرمان (Command Line) | بازنویسی در زمان اجرا (Runtime Override) |

---

## مدیریت اسرار (Secrets Management)

| فناوری | نقش |
|---|---|
| .NET User Secrets | اسرار محیط توسعه (Development Secrets) |
| Azure Key Vault | مخزن سازمانی اسرار (Enterprise Secret Store) |
| HashiCorp Vault | مخزن سازمانی اسرار (Enterprise Secret Store) |

---

## مدیریت قابلیت‌ها (Feature Management)

| فناوری | نقش |
|---|---|
| Microsoft.FeatureManagement | پرچم‌های قابلیت (Feature Flags) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|---|---|---|
| C1 | سازگاری با معماری پاک (Clean Architecture Compatibility) | حیاتی (Critical) |
| C2 | امنیت (Security) | حیاتی (Critical) |
| C3 | استقلال از استقرار (Deployment Independence) | حیاتی (Critical) |
| C4 | بی‌طرفی نسبت به ابر (Cloud Neutrality) | بالا (High) |
| C5 | انعطاف‌پذیری زمان اجرا (Runtime Flexibility) | بالا (High) |
| C6 | قابلیت نگهداری (Maintainability) | بالا (High) |
| C7 | آمادگی سازمانی (Enterprise Readiness) | بالا (High) |
| C8 | یکپارچگی با .NET 10 | حیاتی (Critical) |

---

# اصل معماری (Architecture Principle)

پیکربندی به عنوان یک دغدغه زیرساختی (Infrastructure concern) در نظر گرفته می‌شود.
ماژول‌های تجاری هرگز نباید مستقیماً به ارائه‌دهندگان پیکربندی دسترسی داشته باشند.
در عوض:

```text
Business Modules
        │
        ▼
Strongly Typed Options
        │
        ▼
Configuration Abstraction
        │ 
┌──────────────┬──────────────┬──────────────┐ 
▼              ▼              ▼
JSON       Environment      Secret Store
```

این معماری تضمین می‌کند که ارائه‌دهندگان پیکربندی بدون تأثیرگذاری بر منطق تجاری سیستم قابل تکامل و تغییر باشند.

---

# 5. ارزیابی Microsoft.Extensions.Configuration

## نمای کلی (Overview)

فناوری Microsoft.Extensions.Configuration انتزاع رسمی پیکربندی ارائه‌شده توسط مایکروسافت برای دات‌نت است.
این ابزار یک API یکپارچه را بر روی چندین ارائه‌دهنده پیکربندی فراهم می‌سازد.

ارائه‌دهندگان پشتیبانی‌شده عبارتند از:
- پیکربندی JSON؛
- متغیرهای محیطی؛
- آرگومان‌های خط فرمان؛
- ارائه‌دهندگان درون‌حافظه‌ای (in-memory)؛
- مخزن Azure Key Vault؛
- ارائه‌دهندگان سفارشی (custom providers).

این کتابخانه، زیرساخت پیکربندی توصیه‌شده برای برنامه‌های کاربردی .NET 10 است.

---

## نقاط قوت معماری (Architectural Strengths)

### مزایا (Advantages)

- انتزاع رسمی مایکروسافت.
- پشتیبانی بومی از .NET 10.
- استقلال از ارائه‌دهنده (Provider independence).
- یکپارچگی فوق‌العاده با تزریق وابستگی (Dependency Injection).
- پیکربندی سلسله‌مراتبی (Hierarchical configuration).
- پشتیبانی از بازخوانی در زمان اجرا (Runtime reload).
- یکپارچگی عالی با الگوی Options (Options Pattern).
- اکوسیستم بالغ و آزموده‌شده.
- آمادگی کامل سازمانی (Enterprise ready).

---

## نقاط ضعف معماری (Architectural Weaknesses)

این لایه انتزاع به عمد هیچ‌گونه محافظت از اسرار را انجام نمی‌دهد.
مدیریت اسرار همچنان بر عهده ارائه‌دهندگان اختصاصی اسرار باقی می‌ماند.

---

## ویژگی‌های عملیاتی (Operational Characteristics)

قابلیت‌های پشتیبانی‌شده عبارتند از:
- پیکربندی سلسله‌مراتبی؛
- ترکیب ارائه‌دهندگان (provider composition)؛
- بازخوانی به هنگام تغییر (reload-on-change)؛
- اتصال پیکربندی (configuration binding)؛
- اعتبارسنجی (validation).

پیچیدگی عملیاتی آن بسیار پایین است.

---

## مقیاس‌پذیری (Scalability)

از آنجا که این بخش صرفاً یک لایه انتزاع است، مقیاس‌پذیری به ارائه‌دهندگان انتخابی وابسته است.
خود لایه انتزاع، سرریز بار (overhead) ناچیزی را در زمان اجرا تحمیل می‌کند.

---

## امنیت (Security)

امنیت به طور کامل به ارائه‌دهندگان زیرین وابسته است.
این لایه انتزاع نه امنیت را بهبود می‌بخشد و نه آن را تضعیف می‌کند.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

در هر محیطی که .NET 10 اجرا شود پشتیبانی می‌گردد:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- ابر (Cloud)
- درون‌سازمانی (On-Premise)
- ترکیبی (Hybrid)

انعطاف‌پذیری استقرار در سطح عالی است.

---

## قابلیت نگهداری (Maintainability)

واسط‌های برنامه‌نویسی پیکربندی پایدار بوده، به صورت رسمی پشتیبانی می‌شوند و به طور گسترده به کار گرفته شده‌اند.
قابلیت نگهداری عالی ارزیابی می‌شود.

---

## انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| استقلال از ارائه‌دهنده (Provider Independence) | عالی (Excellent) |
| استقلال از استقرار (Deployment Independence) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

فناوری Microsoft.Extensions.Configuration باید به عنوان تنها انتزاع پیکربندی در سرتاسر MachineryManagerEnterprise مورد استفاده قرار گیرد.
کد برنامه هرگز نباید مستقیماً به هیچ ارائه‌دهنده پیکربندی خاصی وابسته باشد.

---

# 6. ارزیابی Microsoft.Extensions.Options

## نمای کلی (Overview)

فناوری Microsoft.Extensions.Options سازوکار رسمی مایکروسافت برای پیکربندی با نوع‌بندی قوی (strongly typed configuration) در دات‌نت است.
به جای اجازه دادن به کدهای تجاری برای دسترسی مستقیم به مقادیر پیکربندی، الگوی Options مقادیر پیکربندی را به اشیاء با نوع‌بندی قوی متصل نموده و از طریق تزریق وابستگی (Dependency Injection) تزریق می‌کند.

در سامانه MachineryManagerEnterprise، الگوی Options به عنوان سازوکار استاندارد برای دسترسی به پیکربندی در سرتاسر برنامه ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

الگوی Options به مرز زیرساخت برنامه (Application Infrastructure boundary) تعلق دارد.

```text
Configuration Providers
        │
        ▼
Microsoft.Extensions.Configuration
        │
        ▼
Options Binding
        │
        ▼
IOptions<T>
        │
        ▼
Business Services
```

ماژول‌های تجاری هرگز پیکربندی را به طور مستقیم نمی‌خوانند.
در عوض، آن‌ها اشیاء پیکربندی تغییرناپذیر (immutable) را دریافت می‌نمایند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- پیکربندی با نوع‌بندی قوی (Strongly typed configuration).
- ایمنی در زمان کامپایل (Compile-time safety).
- یکپارچگی کامل با تزریق وابستگی (Dependency Injection integration).
- پشتیبانی بومی از .NET 10.
- پشتیبانی از اعتبارسنجی (Validation support).
- کاهش استفاده از رشته‌های متنی صریح (string literals).
- بهبود قابلیت نگهداری (Improved maintainability).
- قابلیت تست‌پذیری بهتر (Better testability).
- سازگاری فوق‌العاده با معماری پاک (Clean Architecture compatibility).

---

# نقاط ضعف معماری (Architectural Weaknesses)

الگوی Options به عمد صرفاً نقش انتزاعی را ایفا می‌کند.
این الگو وظایف زیر را انجام نمی‌دهد:
- بارگذاری پیکربندی (load configuration)؛
- مدیریت اسرار (manage secrets)؛
- ماندگارسازی پیکربندی (persist configuration).

این مسئولیت‌ها همچنان به ارائه‌دهندگان پیکربندی واگذار شده است.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

قابلیت‌های پشتیبانی‌شده عبارتند از:
- اتصال پیکربندی (configuration binding)؛
- آپشن‌های نام‌گذاری‌شده (named options)؛
- اعتبارسنجی آپشن‌ها (options validation)؛
- بازخوانی در زمان اجرا (از طریق IOptionsMonitor)؛
- اسنپ‌شات‌های تغییرناپذیر (از طریق IOptionsSnapshot).

پیچیدگی عملیاتی در حداقل ممکن است.

---

# مقیاس‌پذیری (Scalability)

الگوی Options به طور طبیعی در انواع بسترها مقیاس می‌پذیرد:
- وب APIها (Web APIs)؛
- سرویس‌های پس‌زمینه (Background Services)؛
- سرویس‌های میزبان (Hosted Services)؛
- میکروسرویس‌ها (Microservices)؛
- مونولیت‌های ماژولار (Modular Monoliths).

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# امنیت (Security)

پیکربندی‌های حساس هرگز به صورت مستقیم در معرض نمایش قرار نمی‌گیرند.
در عوض:
- ارائه‌دهندگان، اسرار را واکشی می‌کنند؛
- بخش Options فقط مقادیر مورد نیاز را افشا می‌نماید؛
- مصرف‌کنندگان صرفاً پیکربندی مورد نیاز خود را دریافت می‌کنند.

این رویکرد نشت تصادفی اطلاعات حساس را به شکل چشمگیری کاهش می‌دهد.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

در تمام مدل‌های میزبانی .NET 10 پشتیبانی می‌شود:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- ابر (Cloud)
- ترکیبی (Hybrid)
- درون‌سازمانی (On-Premise)

---

# سازگاری با هوش مصنوعی (AI Compatibility)

سرویس‌های هوش مصنوعی غالباً به پیکربندی‌هایی نظیر موارد زیر نیاز دارند:
- شناسه‌های مدل (model identifiers)؛
- آدرس‌های پایانه (endpoint URLs)؛
- مقادیر مهلت زمانی (timeout values)؛
- سیاست‌های تلاش مجدد (retry policies)؛
- پیکربندی امبدینگ (embedding configuration)؛
- محدودیت‌های توکن (token limits).

استفاده از گزینه‌های دارای نوع‌بندی قوی، پیکربندی سرویس‌های AI را بسیار ساده می‌سازد.

---

# قابلیت نگهداری (Maintainability)

کلاس‌های پیکربندی عبارتند از:
- قابل کشف (discoverable)؛
- تست‌پذیر (testable)؛
- سازگار با بازآرایی کد (refactor-friendly)؛
- اعتبارسنجی‌شده (validated).

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| تزریق وابستگی (Dependency Injection) | عالی (Excellent) |
| نوع‌بندی قوی (Strong Typing) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |

---

# مثال (Example)

به جای نوشتن:
```csharp
_configuration["Jwt:Issuer"]
```

کد تجاری باید ساختار زیر را دریافت کند:
```text
JwtOptions
```
از طریق تزریق وابستگی (Dependency Injection).
این رویکرد دسترسی رشته‌محور به پیکربندی را به طور کامل حذف می‌نماید.

---

# ارتباط با Microsoft.Extensions.Configuration (Relationship with Microsoft.Extensions.Configuration)

پیکربندی و آپشن‌ها مکمل یکدیگر هستند.

```text
Configuration Providers
        │
        ▼
Configuration Abstraction
        │
        ▼
Options Binding
        │
        ▼
Business Modules
```

مسئولیت‌ها به وضوح تفکیک شده باقی می‌مانند.

| فناوری | مسئولیت |
|---|---|
| Configuration | بارگذاری پیکربندی (Load configuration) |
| Options | مصرف با نوع‌بندی قوی (Strongly typed consumption) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

فناوری Microsoft.Extensions.Options باید به سازوکار الزامی و اجباری مصرف پیکربندی در سراسر سامانه MachineryManagerEnterprise تبدیل شود.
مؤلفه‌های برنامه هرگز نباید به صورت مستقیم به IConfiguration دسترسی داشته باشند، مگر در لایه زیرساخت (Infrastructure layer).
سرویس‌های تجاری باید منحصراً گزینه‌های با نوع‌بندی قوی (strongly typed options) را مصرف نمایند.

---

# 7. ارزیابی فایل‌های پیکربندی JSON (JSON Configuration Files Evaluation)

## نمای کلی (Overview)

فایل‌های پیکربندی JSON منبع پیکربندی پیش‌فرض برای برنامه‌های مدرن دات‌نت به شمار می‌روند.
در .NET 10، فایل‌های پیکربندی به طور معمول شامل موارد زیر هستند:
- appsettings.json
- appsettings.Development.json
- appsettings.Staging.json
- appsettings.Production.json

آن‌ها پیکربندی خط مبنا (baseline configuration) را برای هر محیط استقرار فراهم می‌سازند.

---

# نقش معماری (Architectural Role)

فایل‌های JSON لایه پیکربندی پیش‌فرض را فراهم می‌آورند.

```text
Configuration Providers
        │
 ┌──────────────────────────────┐
 │ appsettings.json             │
 ├──────────────────────────────┤
 │ appsettings.{Environment}    │
 └──────────────────────────────┘
        │
        ▼
Configuration Abstraction
```

فایل‌های مختص به هر محیط، پیکربندی پیش‌فرض را بازنویسی (override) می‌کنند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- پشتیبانی بومی در .NET 10.
- خوانایی بالا توسط انسان (Human readable).
- ساختار سلسله‌مراتبی (Hierarchical structure).
- سازگاری کامل با سیستم کنترل نسخه (Source control friendly).
- پشتیبانی عالی ابزارها (tooling support).
- تفکیک شفاف محیط‌ها (Environment separation).
- حداقل پیچیدگی عملیاتی.
- یکپارچگی قوی با سازنده پیکربندی (Configuration Builder).

---

# نقاط ضعف معماری (Architectural Weaknesses)

فایل‌های JSON برای اطلاعات حساس **مناسب نیستند**.
آن‌ها هرگز نباید حاوی موارد زیر باشند:
- گذرواژه‌ها (passwords)؛
- کلیدهای API (مانند API keys)؛
- گواهی‌نامه‌ها (certificates)؛
- اسرار اتصال پایگاه داده (connection secrets)؛
- اعتبارسنجی‌های ارائه‌دهنده هوش مصنوعی (AI provider credentials)؛
- کلیدهای امضا (signing keys).

این مقادیر باید در ارائه‌دهندگان اختصاصی اسرار قرار گیرند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

پیکربندی JSON از موارد زیر پشتیبانی می‌کند:
- بخش‌های سلسله‌مراتبی؛
- آرایه‌ها؛
- اشیاء تودرتو؛
- بازنویسی‌های مختص به محیط؛
- بازخوانی به هنگام تغییر (reload-on-change).

پیچیدگی عملیاتی آن بی‌نهایت کم است.

---

# مقیاس‌پذیری (Scalability)

پیکربندی JSON برای موارد زیر به خوبی مقیاس می‌پذیرد:
- توسعه محلی؛
- تست و آزمایش؛
- استقرار در محیط عملیاتی (Production).

استقرارهای سازمانی بزرگ همچنان از JSON به عنوان منبع پیکربندی خط مبنا استفاده می‌کنند و در عین حال مدیریت اسرار را به بسترهای دیگر واگذار می‌نمایند.

---

# امنیت (Security)

فایل‌های JSON صرفاً برای **پیکربندی‌های غیرحساس (non-sensitive configuration)** مناسب هستند.
نمونه‌ها عبارتند از:
- مقادیر پیش‌فرض قابلیت‌ها؛
- مقادیر مهلت زمانی (timeout values)؛
- مدت زمان کش (cache durations)؛
- سطوح لاگ‌گیری (logging levels)؛
- نام پایانه‌ها (endpoint names).

مقادیر حساس هرگز نباید در سیستم کنترل نسخه (source control) ثبت (commit) شوند.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

پشتیبانی‌شده در:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- ابر (Cloud)
- ترکیبی (Hybrid)
- درون‌سازمانی (On-Premise)

انعطاف‌پذیری استقرار عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

فایل‌های JSON برای پیکربندی‌های هوش مصنوعی که محرمانه نیستند مناسب هستند، از جمله:
- نام‌های پیش‌فرض مدل‌ها؛
- مقادیر مهلت زمانی؛
- سیاست‌های تلاش مجدد؛
- ابعاد بردار امبدینگ (embedding dimensions)؛
- فعال‌سازهای قابلیت (feature toggles).

اعتبارسنجی‌ها و کلیدهای ارائه‌دهنده به صورت بیرونی باقی می‌مانند.

---

# قابلیت نگهداری (Maintainability)

پیکربندی JSON موارد زیر را به ارمغان می‌آورد:
- ساختار قابل پیش‌بینی؛
- خوانایی عالی؛
- کنترل نسخه سرراست؛
- بررسی آسان در طول بازبینی کد (code review).

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# کاربرد توصیه‌شده (Recommended Usage)

پیکربندی‌های مناسب:
```text
Logging
Caching
Feature Defaults
Timeouts
Retry Policies
Module Configuration
Application Metadata
```

پیکربندی‌های نامناسب:
```text
Passwords
Connection Secrets
JWT Signing Keys
API Keys
Certificates
OpenAI Keys
Azure Credentials
Redis Passwords
```

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| استقلال از استقرار (Deployment Independence) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| امنیت (Security) | خوب (صرفاً برای داده‌های غیرحساس) |

---

# ارتباط با پیکربندی محیط (Relationship with Environment Configuration)

فایل‌های JSON مختص به محیط، پیکربندی پایه را گسترش می‌دهند.

```text
appsettings.json
        │
        ▼
appsettings.Development.json
        │
        ▼
Configuration Abstraction
```

هر محیط صرفاً تنظیماتی را بازنویسی می‌کند که تفاوت دارند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

فایل‌های پیکربندی JSON باید به عنوان منبع اصلی پیکربندی غیرحساس برای سامانه MachineryManagerEnterprise باقی بمانند.
آن‌ها مقادیر پیش‌فرض پیکربندی را تعیین می‌کنند، در حالی که تمامی مقادیر حساس به ارائه‌دهندگان امن اسرار واگذار می‌گردند.

---

# 8. ارزیابی متغیرهای محیطی (Environment Variables Evaluation)

## نمای کلی (Overview)

متغیرهای محیطی سازوکار استاندارد پیکربندی در زمان استقرار (deployment-time) را برای برنامه‌های ابری نوین (cloud-native) تشکیل می‌دهند.
برخلاف فایل‌های پیکربندی JSON، متغیرهای محیطی به تیم‌های عملیاتی اجازه می‌دهند برنامه‌ها را بدون نیاز به تغییر مصنوعات استقرار (deployment artifacts) پیکربندی نمایند.

در .NET 10، متغیرهای محیطی به طور بومی با Microsoft.Extensions.Configuration یکپارچه شده‌اند.
آن‌ها به طور گسترده در بسترهای زیر به کار می‌روند:
- کانتینرها (Containers)؛
- کوبرنتیز (Kubernetes)؛
- پلتفرم‌های ابری (Cloud Platforms)؛
- خطوط لوله CI/CD؛
- استقرارهای سازمانی (Enterprise Deployments).

---

# نقش معماری (Architectural Role)

متغیرهای محیطی پیکربندی مختص به استقرار را فراهم می‌سازند.
آن‌ها به عنوان پلی میان زیرساخت و لایه انتزاع پیکربندی عمل می‌کنند.

```text
Infrastructure / Orchestrator
        │
        ▼
Environment Variables
        │
        ▼
Microsoft.Extensions.Configuration
        │
        ▼
Strongly Typed Options
```

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- سازگار با استاندارد ۱۲ عاملی (12-Factor App compliant).
- استقلال کامل از مصنوعات استقرار (No artifact modification).
- پشتیبانی بومی در تمامی ارکستریتورهای کانتینری (مانند Docker و Kubernetes).
- یکپارچگی بومی با .NET 10.
- امکان بازنویسی آسان مقادیر پیش‌فرض.
- پشتیبانی از ساختارهای سلسله‌مراتبی (با استفاده از جداکننده `__`).
- مناسب برای خودکارسازی فرآیندهای DevOps و CI/CD.

---

# نقاط ضعف معماری (Architectural Weaknesses)

متغیرهای محیطی به صورت پیش‌فرض:
- فاقد رمزنگاری در حالت استراحت هستند؛
- ممکن است در لاگ‌های سیستمی یا فرآیندهای فرزند نشت کنند؛
- چرخش خودکار (automatic rotation) را ارائه نمی‌دهند؛
- تاریخچه تغییرات و حسابرسی متمرکز ندارند.

بنابراین نباید به عنوان تنها مکان نگهداری اسرار با ریسک بالا تلقی شوند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

پشتیبانی از:
- نگاشت کلیدهای سلسله‌مراتبی دات‌نت؛
- تزریق پویا توسط کوبرنتیز (ConfigMaps و Secret envs)؛
- خواندن در زمان شروع به کار برنامه.

پیچیدگی عملیاتی پایین است.

---

# مقیاس‌پذیری (Scalability)

متغیرهای محیطی در مقیاس بسیار وسیع در بسترهای زیر کارایی دارند:
- کانتینرها (Containers)؛
- کوبرنتیز (Kubernetes)؛
- داکر کامپوز (Docker Compose)؛
- سرویس‌های Azure App Service؛
- سرویس‌های لینوکس (Linux Services)؛
- سرویس‌های ویندوز (Windows Services).

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# امنیت (Security)

متغیرهای محیطی انعطاف‌پذیری عملیاتی را بهبود می‌بخشند، اما نباید به عنوان یک راهکار کامل مدیریت اسرار قلمداد شوند.
در حالی که برای برخی محیط‌های استقرار قابل قبول هستند، مخازن سازمانی اسرار برای مقادیر بسیار حساس ترجیح داده می‌شوند.

نمونه‌هایی از مقادیر مناسب:
- نام‌های استقرار (deployment names)؛
- آدرس‌های پایانه (endpoint URLs)؛
- کلیدهای فعال‌سازی قابلیت‌ها (feature switches)؛
- شناسه‌های نمونه (instance identifiers).

اعتبارسنجی‌های بسیار حساس باید در ارائه‌دهندگان اختصاصی اسرار باقی بمانند.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده عبارتند از:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- ابر (Cloud)
- ترکیبی (Hybrid)
- درون‌سازمانی (On-Premise)

انعطاف‌پذیری استقرار عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

متغیرهای محیطی برای تنظیمات استقرار هوش مصنوعی مناسب هستند، از جمله:
- آدرس‌های پایانه سرویس هوش مصنوعی (AI endpoint URLs)؛
- انتخاب مدل پیش‌فرض؛
- مناطق استقرار (deployment regions)؛
- مقادیر مهلت زمانی (timeout values)؛
- فعال‌سازی قابلیت‌ها.

اعتبارسنجی‌های طولانی‌مدت ارائه‌دهندگان هوش مصنوعی باید در مخازن امن اسرار نگهداری شوند.

---

# قابلیت نگهداری (Maintainability)

متغیرهای محیطی موارد زیر را به ارمغان می‌آورند:
- خودکارسازی زیرساخت (infrastructure automation)؛
- سازگاری با CI/CD؛
- بازتولیدپذیری استقرار (deployment reproducibility)؛
- ساده‌سازی پیکربندی عملیاتی.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# تقدم پیکربندی (Configuration Precedence)

در سامانه MachineryManagerEnterprise اولویت و تقدم توصیه‌شده به صورت زیر است:

```text
appsettings.json
        │
        ▼
appsettings.{Environment}.json
        │
        ▼
Environment Variables
        │
        ▼
Secret Store
        │
        ▼
Command Line
```

ارائه‌دهندگانی که بعداً می‌آیند مقادیر ارائه‌دهندگان قبلی را بازنویسی (override) می‌کنند.

---

# کاربرد توصیه‌شده (Recommended Usage)

مقادیر مناسب:
```text
Environment Name
Service URLs
Logging Level
Cache Duration
Feature Flags
Deployment Region
Application Instance
```

مقادیر غیرتوصیه‌شده:
```text
JWT Signing Keys
Database Passwords
OpenAI Keys
Azure Secrets
Certificates
```

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| استقلال از استقرار (Deployment Independence) | عالی (Excellent) |
| سازگاری با ابر بومی (Cloud Native) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| امنیت (Security) | بسیار خوب (Very Good) |

---

# ارتباط با پیکربندی JSON (Relationship with JSON Configuration)

متغیرهای محیطی، پیکربندی JSON را گسترش می‌دهند و جایگزین آن نمی‌شوند.

```text
JSON Configuration
        │
        ▼
Environment Variables
        │
        ▼
Configuration Abstraction
```

این امر اجازه می‌دهد مقادیر مختص به استقرار به صورت خارجی باقی بمانند در حالی که پیش‌فرض‌های برنامه تحت کنترل نسخه قرار دارند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

متغیرهای محیطی باید به سازوکار استاندارد پیکربندی در زمان استقرار برای MachineryManagerEnterprise تبدیل شوند.
آن‌ها انعطاف‌پذیری استقرار فوق‌العاده‌ای فراهم می‌سازند و در عین حال کاملاً با زیرساخت پیکربندی .NET 10 سازگارند.
آن‌ها مکمل پیکربندی JSON و ارائه‌دهندگان امن اسرار هستند، نه جایگزین آن‌ها.

---

# 9. ارزیابی .NET User Secrets (.NET User Secrets Evaluation)

## نمای کلی (Overview)

فناوری .NET User Secrets سازوکار درونی دات‌نت برای ذخیره‌سازی اسرار در زمان توسعه (development-time) است.
برخلاف فایل‌های پیکربندی JSON، ابزار User Secrets مقادیر حساس را خارج از پوشه پروژه و خارج از سیستم کنترل نسخه ذخیره می‌کند.

هدف اصلی آن پشتیبانی امن از توسعه محلی بدون افشای اطلاعات محرمانه است.
ابزار User Secrets **صرفاً برای محیط‌های توسعه** در نظر گرفته شده است.

---

# نقش معماری (Architectural Role)

ابزار User Secrets به لایه اسرار محیط توسعه تعلق دارد.

```text
Developer Machine
        │
        ▼
.NET User Secrets
        │
        ▼
Configuration Abstraction
        │
        ▼
Strongly Typed Options
```

کد برنامه از منبع اصلی دریافت اسرار بی‌اطلاع باقی می‌ماند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- پشتیبانی بومی در .NET 10.
- راهکار رسمی مایکروسافت.
- بدون نیاز به هیچ‌گونه زیرساخت خارجی (Zero external infrastructure).
- عدم افشا در سیستم کنترل نسخه (No source control exposure).
- تجربه کاربری عالی برای توسعه‌دهندگان (Developer experience).
- یکپارچگی بدون‌درز با Microsoft.Extensions.Configuration.
- سازگاری قوی با Visual Studio و .NET CLI.

---

# نقاط ضعف معماری (Architectural Weaknesses)

ابزار User Secrets به صورت تعمدی محدود طراحی شده است.
برای موارد زیر **مناسب نیست**:
- محیط عملیاتی (Production)؛
- محیط پیش‌تولید (Staging)؛
- استقرار سازمانی؛
- بارهای کاری کانتینری در محیط عملیاتی؛
- کوبرنتیز؛
- زیرساخت‌های مشترک.

اسرار صرفاً به صورت محلی بر روی ایستگاه کاری توسعه‌دهنده ذخیره می‌شوند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

قابلیت‌های پشتیبانی‌شده عبارتند از:
- ذخیره‌سازی محلی اسرار؛
- یکپارچگی خودکار با پیکربندی؛
- مدیریت از طریق CLI؛
- یکپارچگی با Visual Studio.

پیچیدگی عملیاتی ناچیز است.

---

# مقیاس‌پذیری (Scalability)

ابزار User Secrets فراتر از ایستگاه‌های کاری مجزای توسعه‌دهندگان مقیاس نمی‌پذیرد.
هرگز نباید به عنوان پلتفرم مدیریت اسرار سازمانی در نظر گرفته شود.

---

# امنیت (Security)

در مقایسه با ذخیره اسرار در appsettings.json، ابزار User Secrets امنیت محیط توسعه را به شکل چشمگیری ارتقا می‌دهد.
اطلاعات حساس در شرایط زیر نگهداری می‌شوند:
- خارج از مخزن کد (repository)؛
- خارج از پوشه‌های پروژه؛
- خارج از مصنوعات استقرار.

با این وجود:
- اسرار به صورت متمرکز مدیریت نمی‌شوند؛
- امنیت ایستگاه کاری همچنان حائز اهمیت است.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده:
- ویندوز (Windows)
- لینوکس (Linux)
- مک (macOS)

برای موارد زیر طراحی نشده است:
- محیط عملیاتی (Production)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- زمان اجرای ابری (Cloud Runtime)

---

# سازگاری با هوش مصنوعی (AI Compatibility)

اسرار مناسب زمان توسعه شامل موارد زیر است:
- کلیدهای API سرویس OpenAI؛
- کلیدهای Azure OpenAI؛
- پیکربندی دسترسی Ollama؛
- اعتبارسنجی‌های ارائه‌دهندگان امبدینگ؛
- اعتبارسنجی‌های تست هوش مصنوعی.

این مقادیر هرگز نباید درون فایل‌های پیکربندی مخزن کد ظاهر شوند.

---

# قابلیت نگهداری (Maintainability)

فرآیند ورود توسعه‌دهندگان جدید (onboarding) بسیار آسان‌تر می‌شود.
هر توسعه‌دهنده اعتبارسنجی‌های مستقل خود را بدون نیاز به تغییر فایل‌های پروژه نگهداری می‌کند.
قابلیت نگهداری برای محیط‌های توسعه عالی ارزیابی می‌شود.

---

# کاربرد توصیه‌شده (Recommended Usage)

اسرار مناسب:
```text
OpenAI API Key
Azure OpenAI Key
Redis Password
Development Connection String
JWT Signing Key (Development)
SMTP Credentials (Development)
```

موارد نامناسب:
```text
Production Secrets
Shared Organization Secrets
Enterprise Certificates
Production Signing Keys
```

---

# ارتباط با پیکربندی JSON (Relationship with JSON Configuration)

تقدم پیکربندی در محیط توسعه:

```text
appsettings.json
        │
        ▼
appsettings.Development.json
        │
        ▼
.NET User Secrets
        │
        ▼
Environment Variables
```

ابزار User Secrets پیکربندی محلی را بدون تغییر فایل‌های مخزن بازنویسی می‌نماید.

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| امنیت محیط توسعه (Development Security) | عالی (Excellent) |
| استقرار سازمانی (Enterprise Deployment) | ضعیف (Poor) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| تجربه توسعه‌دهنده (Developer Experience) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

ابزار .NET User Secrets باید به سازوکار الزامی و اجباری برای ذخیره اسرار محیط توسعه در سراسر سامانه MachineryManagerEnterprise تبدیل شود.
توسعه‌دهندگان هرگز نباید مقادیر محرمانه را درون موارد زیر قرار دهند:
- appsettings.json؛
- appsettings.Development.json؛
- فایل‌های تحت کنترل نسخه.

ابزار User Secrets یک قابلیت منحصراً مختص به محیط توسعه باقی می‌ماند و در محیط عملیاتی با ارائه‌دهندگان سازمانی اسرار جایگزین خواهد شد.

---

# 10. ارزیابی Azure Key Vault (Azure Key Vault Evaluation)

## نمای کلی (Overview)

سرویس Azure Key Vault پلتفرم ابری مایکروسافت در سطح سازمانی برای مدیریت اسرار است.
این سرویس ذخیره‌سازی امن را برای موارد زیر فراهم می‌آورد:
- اسرار (secrets)؛
- گواهی‌نامه‌ها (certificates)؛
- کلیدهای رمزنگاری (cryptographic keys).

برخلاف .NET User Secrets، سرویس Azure Key Vault برای محیط‌های عملیاتی و حاکمیت متمرکز اسرار طراحی شده است.
در سامانه MachineryManagerEnterprise، این سرویس به عنوان یک راهکار ابری بومی سازمانی ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

سرویس Azure Key Vault به لایه مخزن سازمانی اسرار تعلق دارد.

```text
Application
        │
        ▼
Configuration Abstraction
        │
        ▼
Azure Key Vault Provider
        │
        ▼
Azure Key Vault
```

ماژول‌های تجاری هرگز مستقیماً با Key Vault ارتباط برقرار نمی‌کنند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- امنیت در سطح سازمانی (Enterprise-grade security).
- مدیریت متمرکز اسرار (Centralized secret management).
- پشتیبانی از هویت‌های مدیریت‌شده (Managed Identities).
- مدیریت چرخه حیات گواهی‌نامه‌ها (Certificate management).
- چرخش خودکار اسرار (Automatic secret rotation).
- یکپارچگی بومی با Azure.
- پشتیبانی بومی دات‌نت (.NET native support).
- کنترل دسترسی مبتنی بر نقش (RBAC).
- لاگ‌گیری حسابرسی کامل (Audit logging).
- دسترسی‌پذیری بالا (High availability).

---

# نقاط ضعف معماری (Architectural Weaknesses)

سرویس Azure Key Vault وابستگی به پلتفرم ایجاد می‌کند.
ملاحظات اصلی عبارتند از:
- نیازمندی به اشتراک ابری Azure؛
- وابستگی به ارائه‌دهنده (Vendor dependency)؛
- نیازمندی به اتصال اینترنت؛
- مدل عملیاتی مبتنی بر اولویت ابر (Cloud-first).

این خصوصیات بی‌طرفی استقرار (deployment neutrality) را کاهش می‌دهند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

سرویس Azure Key Vault امکانات زیر را فراهم می‌کند:
- ذخیره‌سازی متمرکز اسرار؛
- مدیریت کلیدها؛
- چرخه حیات گواهی‌نامه‌ها؛
- سیاست‌های دسترسی؛
- حسابرسی و ممیزی؛
- نسخه‌بندی اسرار (secret versioning).

---

# مقیاس‌پذیری (Scalability)

سرویس به عنوان یک پیشنهاد کاملاً مدیریت‌شده (PaaS)، به طور خودکار با بارهای کاری سازمانی مقیاس می‌پذیرد.
مقیاس‌پذیری عالی است.

---

# امنیت (Security)

امنیت در بالاترین استانداردهای ابری پیاده‌سازی شده است:
- ماژول‌های امنیتی سخت‌افزاری (HSM)؛
- رمزنگاری در حالت استراحت و انتقال؛
- احراز هویت مبتنی بر Microsoft Entra ID؛
- سیاست‌های دسترسی تفکیک‌شده.

امنیت عالی ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

استقرار برنامه‌ها به محیط‌های سازگار با Azure یا محیط‌هایی با اتصال امن به Azure محدود می‌شود.
انعطاف‌پذیری استقرار در سطح متوسط به بالا ارزیابی می‌گردد.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

ایده‌آل برای ذخیره کلیدهای اختصاصی Azure OpenAI، کلیدهای شناختی و اسرار مرتبط با سرویس‌های ابری هوش مصنوعی.

---

# قابلیت نگهداری (Maintainability)

به دلیل مدیریت‌شده بودن سرویس توسط مایکروسافت، بار نگهداری زیرساختی بر عهده تیم توسعه نخواهد بود.
قابلیت نگهداری عالی است.

---

# استقلال از ارائه‌دهنده (Vendor Independence)

به دلیل ماهیت انحصاری در بستر ابری مایکروسافت، استقلال از ارائه‌دهنده در سطح متوسط رو به پایین قرار دارد.

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| امنیت سازمانی (Enterprise Security) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | متوسط (Moderate) |
| استقلال از ارائه‌دهنده (Vendor Independence) | متوسط (Moderate) |
| سهولت عملیاتی (Operational Simplicity) | عالی (Excellent) |

---

# ارتباط با پیکربندی (Relationship with Configuration)

سرویس Azure Key Vault به عنوان یک ارائه‌دهنده پیکربندی به Microsoft.Extensions.Configuration متصل می‌شود.

```text
Configuration
        │
        ▼
Azure Key Vault Provider
        │
        ▼
Azure Key Vault
```

تنها مقادیر حساس باید از Key Vault سرچشمه بگیرند.

---

# کاربرد توصیه‌شده (Recommended Usage)

اسرار مناسب:
```text
Database Passwords
JWT Signing Keys
OpenAI Credentials
Azure Credentials
Certificates
SMTP Passwords
Redis Passwords
```

پیکربندی‌های غیرحساس باید خارج از Key Vault باقی بمانند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

سرویس Azure Key Vault یک پلتفرم عالی مدیریت اسرار برای استقرارهای متمرکز بر بستر Azure است.
با این حال، از آنجا که MachineryManagerEnterprise صراحتاً بر موارد زیر تأکید دارد:
- استقلال از ارائه‌دهنده؛
- بی‌طرفی در استقرار؛
- انعطاف‌پذیری زیرساخت؛
سرویس Azure Key Vault باید به عنوان یک ارائه‌دهنده ابری پشتیبانی‌شده در نظر گرفته شود نه استراتژی اصلی و پیش‌فرض مدیریت اسرار در معماری.

---

# 11. ارزیابی HashiCorp Vault (HashiCorp Vault Evaluation)

## نمای کلی (Overview)

نرم‌افزار HashiCorp Vault یک پلتفرم سازمانی برای مدیریت اسرار است که به منظور ذخیره‌سازی امن، مدیریت و توزیع اطلاعات حساس در سراسر زیرساخت‌های ناهمگن طراحی شده است.
برخلاف راهکارهای وابسته به یک ارائه‌دهنده ابری خاص، پلتفرم Vault از نظر زیرساختی بی‌طرف بوده و می‌تواند به طور یکنواخت در محیط‌های زیر فعالیت کند:
- درون‌سازمانی (On-Premise)؛
- ابر خصوصی (Private Cloud)؛
- ابر عمومی (Public Cloud)؛
- ابر ترکیبی (Hybrid Cloud)؛
- چندابری (Multi-Cloud).

در سامانه MachineryManagerEnterprise، نرم‌افزار HashiCorp Vault به عنوان پلتفرم اصلی و بی‌طرف نسبت به ابر برای مدیریت اسرار سازمانی ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

نرم‌افزار Vault در لایه مخزن سازمانی اسرار قرار می‌گیرد.

```text
Application
        │
        ▼
Configuration Abstraction
        │
        ▼
Vault Configuration Provider
        │
        ▼
HashiCorp Vault
```

ماژول‌های تجاری هرگز مستقیماً به Vault دسترسی ندارند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- بی‌طرف نسبت به ارائه‌دهنده (Vendor neutral).
- پشتیبانی از چندابری (Multi-cloud support).
- پشتیبانی از استقرار درون‌سازمانی (On-premise support).
- تولید پویای اسرار (Dynamic secrets).
- اجاره و انقضای اسرار (Secret leasing).
- چرخش خودکار اسرار (Secret rotation).
- مدیریت گواهی‌نامه‌ها (Certificate management / PKI).
- رمزنگاری به عنوان سرویس (Encryption as a Service / Transit).
- سیاست‌های دسترسی ریزدانه (Fine-grained policies).
- لاگ‌گیری حسابرسی جامع و دقیق.
- اکوسیستم سازمانی بالغ و پیشرو.

---

# نقاط ضعف معماری (Architectural Weaknesses)

پلتفرم Vault زیرساخت عملیاتی اضافی تحمیل می‌کند.
ملاحظات معمول عبارتند از:
- سرورهای اختصاصی Vault؛
- استراتژی پشتیبان‌گیری؛
- پیکربندی دسترسی‌پذیری بالا (HA)؛
- مدیریت و راهبری عملیاتی.

این مسئولیت‌ها پیچیدگی عملیاتی را در مقایسه با سرویس‌های کاملاً مدیریت‌شده ابری افزایش می‌دهند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

پلتفرم Vault از موارد زیر پشتیبانی می‌کند:
- موتور اسرار کلید-مقدار (KV secret engine)؛
- اعتبارسنجی‌های پویای پایگاه داده؛
- زیرساخت کلید عمومی (PKI)؛
- رمزنگاری در حال انتقال (Transit encryption)؛
- مدیریت هویت؛
- بخش‌های احراز هویت گوناگون؛
- سیستم اجاره (Leasing)؛
- انقضای خودکار اسرار.

پیچیدگی عملیاتی در سطح متوسط ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

پلتفرم Vault از موارد زیر پشتیبانی می‌کند:
- کلاسترینگ (Clustering)؛
- تکثیر داده‌ها (Replication)؛
- دسترسی‌پذیری بالا (High availability)؛
- استقرار در مقیاس بسیار بزرگ سازمانی.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# امنیت (Security)

پلتفرم Vault یکی از قدرتمندترین بسترهای مدیریت اسرار سازمانی در حال حاضر به شمار می‌رود.
قابلیت‌ها عبارتند از:
- رمزنگاری در حالت استراحت؛
- رمزنگاری در حال انتقال؛
- چرخش خودکار کلیدها؛
- اعتبارسنجی‌های پویا و کوتاه‌مدت؛
- معماری اعتماد صفر (Zero-trust architecture)؛
- اسرار کوتاه‌مدت (Short-lived secrets)؛
- اعمال سیاست‌های امنیتی سخت‌گیرانه؛
- ردپای حسابرسی غیرقابل دستکاری.

امنیت فوق‌العاده و برجسته (Outstanding) ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده عبارتند از:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- آژور (Azure)
- آمازون (AWS)
- گوگل کلود (Google Cloud)
- درون‌سازمانی (On-Premise)
- ترکیبی (Hybrid)
- چندابری (Multi-Cloud)

انعطاف‌پذیری استقرار در سطح عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

پلتفرم Vault به ویژه برای حفاظت از موارد زیر بسیار مناسب است:
- کلیدهای API سرویس OpenAI؛
- اعتبارسنجی‌های Azure OpenAI؛
- احراز هویت با Ollama؛
- اسرار ارائه‌دهندگان امبدینگ؛
- گواهی‌نامه‌های سرویس‌های AI؛
- اعتبارسنجی‌های سرویس‌های استنتاج (Inference).

چرخش پویای اسرار برای سرویس‌های هوش مصنوعی که با چندین ارائه‌دهنده خارجی یکپارچه می‌شوند، فوق‌العاده ارزشمند است.

---

# قابلیت نگهداری (Maintainability)

پلتفرم Vault امکانات زیر را فراهم می‌کند:
- حاکمیت متمرکز؛
- مدیریت چرخه حیات اسرار؛
- چرخش خودکار؛
- نسخه‌بندی؛
- مدیریت سیاست‌ها.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# بی‌طرفی نسبت به ابر (Cloud Neutrality)

برخلاف Azure Key Vault، نرم‌افزار Vault هیچ وابستگی پیش‌فرضی به ارائه‌دهنده ابری ندارد.
این موضوع مستقیماً با اهداف معماری MachineryManagerEnterprise هم‌راستا است:
- استقلال از ارائه‌دهنده (Provider Independence)؛
- استقلال از استقرار (Deployment Independence)؛
- استقلال از زیرساخت (Infrastructure Independence).

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| امنیت سازمانی (Enterprise Security) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | عالی (Excellent) |
| استقلال از استقرار (Deployment Independence) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |

---

# مقایسه با Azure Key Vault (Comparison with Azure Key Vault)

| قابلیت | Azure Key Vault | HashiCorp Vault |
|---|---|---|
| بی‌طرف نسبت به ارائه‌دهنده | متوسط (Moderate) | عالی (Excellent) |
| یکپارچگی با Azure | عالی (Excellent) | بسیار خوب (Very Good) |
| چندابری (Multi-Cloud) | متوسط (Moderate) | عالی (Excellent) |
| اسرار پویا (Dynamic Secrets) | محدود (Limited) | عالی (Excellent) |
| اجاره اسرار (Secret Leasing) | خیر (No) | عالی (Excellent) |
| انعطاف‌پذیری استقرار | خوب (Good) | عالی (Excellent) |
| سادگی عملیاتی | عالی (Excellent) | متوسط (Moderate) |
| حاکمیت سازمانی | عالی (Excellent) | عالی (Excellent) |

---

# ارتباط با پیکربندی (Relationship with Configuration)

پلتفرم Vault از طریق انتزاع پیکربندی یکپارچه می‌شود.

```text
Configuration
        │
        ▼
Vault Provider
        │
        ▼
HashiCorp Vault
```

تنها مقادیر محرمانه پیکربندی از Vault سرچشمه می‌گیرند.

---

# کاربرد توصیه‌شده (Recommended Usage)

اسرار مناسب:
```text
Database Passwords
Connection Strings
JWT Signing Keys
OpenAI API Keys
Azure Credentials
Redis Passwords
SMTP Credentials
Certificates
Encryption Keys
```

پیکربندی‌های عمومی برنامه باید همچنان در خارج از Vault قرار داشته باشند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

نرم‌افزار HashiCorp Vault قدرتمندترین پلتفرم سازمانی و مستقل از ارائه‌دهنده برای مدیریت اسرار است.
اگرچه پیچیدگی عملیاتی آن بیشتر از Azure Key Vault است، اما تطابق بسیار بیشتری با اهداف معماری MachineryManagerEnterprise در حوزه‌های زیر دارد:
- استقلال از ارائه‌دهنده؛
- بی‌طرفی نسبت به ابر؛
- انعطاف‌پذیری بلندمدت زیرساخت.

بنابراین HashiCorp Vault باید به عنوان پلتفرم ترجیحی و اصلی مدیریت اسرار سازمانی در نظر گرفته شود.

---

# 12. ارزیابی Microsoft.FeatureManagement (Microsoft.FeatureManagement Evaluation)

## نمای کلی (Overview)

فناوری Microsoft.FeatureManagement فریم‌ورک رسمی مایکروسافت برای مدیریت پرچم‌های قابلیت (feature flags) در دات‌نت است.
این ابزار فعال‌سازی کنترل‌شده قابلیت‌های برنامه را بدون نیاز به تغییر کد یا استقرار مجدد میسر می‌سازد.

پرچم‌های قابلیت موارد زیر را پشتیبانی می‌کنند:
- انتشار تدریجی (gradual rollout)؛
- آزمایش A/B؛
- قابلیت‌های آزمایشی (experimental functionality)؛
- کلیدهای قطع اضطراری عملیاتی (operational kill switches)؛
- قابلیت‌های مختص به محیط‌های خاص.

در سامانه MachineryManagerEnterprise، فریم‌ورک Microsoft.FeatureManagement به عنوان چارچوب استاندارد پرچم‌های قابلیت ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

مدیریت قابلیت‌ها به لایه پیکربندی تعلق دارد.

```text
Configuration Providers
        │
        ▼
Microsoft.FeatureManagement
        │
        ▼
Feature Filters
        │
        ▼
Business Services
```

ماژول‌های تجاری صرفاً انتزاع قابلیت را مصرف می‌نمایند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- فریم‌ورک رسمی مایکروسافت.
- یکپارچگی بومی با .NET 10.
- پشتیبانی از تزریق وابستگی (Dependency Injection).
- مبتنی بر پیکربندی (Configuration-based).
- ارزیابی در زمان اجرا (Runtime evaluation).
- فیلترهای قابلیت (Feature filters).
- انتشار درصدی (Percentage rollout).
- انتشار هدفمند (Targeted rollout).
- فعال‌سازی مبتنی بر زمان (Time-based activation).
- پشتیبانی عالی از تست‌های نرم‌افزاری.

---

# نقاط ضعف معماری (Architectural Weaknesses)

فریم‌ورک Microsoft.FeatureManagement به عمد بر روی ارزیابی پرچم‌های قابلیت تمرکز دارد.
این ابزار موارد زیر را ارائه نمی‌دهد:
- حاکمیت متمرکز بر قابلیت‌ها؛
- پلتفرم سازمانی آزمایش‌گری؛
- داشبوردهای تحلیلی و تجاری آزمایش‌ها.

سازمان‌های بسیار بزرگ ممکن است در آینده پلتفرم‌های اختصاصی مدیریت قابلیت‌ها را با آن یکپارچه نمایند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

قابلیت‌های پشتیبانی‌شده عبارتند از:
- پرچم‌های قابلیت؛
- فعال‌سازی شرطی؛
- انتشار درصدی؛
- هدف‌گذاری کاربران یا گروه‌ها؛
- پنجره‌های زمانی؛
- ارزیابی در زمان اجرا.

پیچیدگی عملیاتی بسیار پایین است.

---

# مقیاس‌پذیری (Scalability)

ارزیابی قابلیت‌ها کاملاً درون کد برنامه رخ می‌دهد.
سرریز بار پردازشی آن ناچیز است.
این فریم‌ورک به طور طبیعی در بسترهای زیر مقیاس می‌پذیرد:
- وب APIها؛
- سرویس‌های پس‌زمینه؛
- میکروسرویس‌ها؛
- کانتینرها.

---

# امنیت (Security)

پرچم‌های قابلیت مقادیر پیکربندی هستند نه اطلاعات محرمانه و اسرار.
ملاحظات امنیتی شامل موارد زیر است:
- دسترسی‌های مدیریتی؛
- حاکمیت استقرار؛
- فرآیندهای حسابرسی و ممیزی.

اعتبارسنجی‌های حساس هرگز نباید به عنوان پرچم‌های قابلیت ذخیره شوند.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده عبارتند از:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- ابر (Cloud)
- ترکیبی (Hybrid)
- درون‌سازمانی (On-Premise)

انعطاف‌پذیری استقرار در سطح عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

پرچم‌های قابلیت برای تکامل تدریجی هوش مصنوعی فوق‌العاده کاربردی هستند.
نمونه‌ها عبارتند از:
- فعال‌سازی جستجوی معنایی (semantic search)؛
- تغییر ارائه‌دهنده هوش مصنوعی؛
- فعال‌سازی روش RAG؛
- فعال‌سازی تولید امبدینگ؛
- فعال‌سازی دستیارهای آزمایشی (experimental copilots)؛
- عرضه مدل‌های جدید استنتاج (inference models).

این امر ریسک استقرار عملکردهای هوش مصنوعی را به شدت کاهش می‌دهد.

---

# قابلیت نگهداری (Maintainability)

کد فریم‌ورک ساختارمند، تمیز و یکپارچه با ساختار دات‌نت است و نگهداری آن بسیار ساده است.

---

# کاربرد توصیه‌شده (Recommended Usage)

موارد مناسب:
```text
Feature Toggles
Gradual Feature Rollout
Beta User Features
Maintenance Mode Switches
AI Model Selection Switches
Circuit Breakers Configuration
```

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| یکپارچگی با .NET 10 | عالی (Excellent) |
| سهولت استفاده | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | بسیار خوب (Very Good) |

---

# ارتباط با پیکربندی (Relationship with Configuration)

پرچم‌های قابلیت مستقیماً از ارائه‌دهندگان پیکربندی دات‌نت خوانده می‌شوند.

---

# ارتباط با پلتفرم هوش مصنوعی (Relationship with AI Platform)

به تیم‌ها امکان می‌دهد مدل‌ها و ابزارهای مختلف AI را بدون قطعی سیستم آزمایش و به تدریج فعال کنند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

کتابخانه Microsoft.FeatureManagement به عنوان چارچوب استاندارد مدیریت پرچم‌های قابلیت در سامانه تصویب می‌شود.

---

# 13. مقایسه کلی فناوری‌ها (Overall Technology Comparison)

هیچ فناوری واحدی نباید تمام انواع پیکربندی را مدیریت کند.
در عوض، به هر فناوری مسئولیتی کاملاً مشخص واگذار می‌شود.

---

## ماتریس مسئولیت‌ها (Responsibility Matrix)

| قابلیت | فناوری توصیه‌شده | جایگزین | مسئولیت |
|---|---|---|---|
| انتزاع پیکربندی | Microsoft.Extensions.Configuration | — | واسط کاربری یکپارچه پیکربندی (Unified Configuration API) |
| پیکربندی با نوع‌بندی قوی | Microsoft.Extensions.Options | — | مصرف ساختارمند پیکربندی (Configuration Consumption) |
| پیکربندی پیش‌فرض | appsettings.json | XML | پیکربندی ایستا و مقادیر پایه (Static Configuration) |
| پیکربندی استقرار | متغیرهای محیطی (Environment Variables) | خط فرمان | بازنویسی‌های مختص به محیط (Environment Overrides) |
| اسرار محیط توسعه | .NET User Secrets | فایل‌های محلی | ذخیره‌سازی امن اسرار توسعه‌دهنده (Developer Secret Storage) |
| اسرار سازمانی | HashiCorp Vault | Azure Key Vault | مدیریت اسرار محیط عملیاتی (Production Secret Management) |
| پرچم‌های قابلیت | Microsoft.FeatureManagement | LaunchDarkly | کنترل قابلیت‌ها در زمان اجرا (Runtime Feature Control) |

---

## مقایسه قابلیت‌ها (Capability Comparison)

| قابلیت | Configuration | Options | JSON | Environment | User Secrets | Azure Key Vault | HashiCorp Vault | FeatureManagement |
|---|---|---|---|---|---|---|---|---|
| نوع‌بندی قوی | خیر | عالی | خیر | خیر | خیر | خیر | خیر | خیر |
| استقلال از ارائه‌دهنده | عالی | عالی | خوب | خوب | خوب | متوسط | عالی | عالی |
| بازنویسی در زمان اجرا | عالی | عالی | محدود | عالی | خوب | عالی | عالی | عالی |
| ذخیره‌سازی اسرار | خیر | خیر | ضعیف | متوسط | خوب | عالی | عالی | خیر |
| استقرار سازمانی | عالی | عالی | خوب | عالی | ضعیف | عالی | عالی | عالی |
| بی‌طرفی نسبت به ابر | عالی | عالی | عالی | عالی | عالی | متوسط | عالی | عالی |
| پیکربندی هوش مصنوعی | عالی | عالی | خوب | عالی | خوب | عالی | عالی | عالی |
| قابلیت نگهداری | عالی | عالی | عالی | عالی | عالی | عالی | عالی | عالی |

---

# 14. معماری توصیه‌شده پیکربندی (Recommended Configuration Architecture)

این ارزیابی اتخاذ یک معماری پیکربندی لایه‌بندی‌شده را توصیه می‌نماید:

```text
                  Business Modules
                         │
                         ▼
                 Strongly Typed Options
                         │
                         ▼
          Microsoft.Extensions.Configuration
                         │
      ┌──────────────────┼──────────────────────┐
      ▼                  ▼                      ▼
appsettings.json   Environment Variables   Secret Providers
                                               │
                                 ┌─────────────┴─────────────┐
                                 ▼                           ▼
                         HashiCorp Vault              Azure Key Vault
                         (Primary)                     (Alternative)
                         │
                         ▼
                Microsoft.FeatureManagement
```

---

# 15. مسئولیت‌های پیکربندی (Configuration Responsibilities)

## Microsoft.Extensions.Configuration

مسئول برای:
- انتزاع ارائه‌دهندگان؛
- تجمیع پیکربندی‌ها؛
- مدیریت سلسله‌مراتبی پیکربندی.

---

## Microsoft.Extensions.Options

مسئول برای:
- پیکربندی با نوع‌بندی قوی؛
- تزریق وابستگی؛
- اعتبارسنجی مقادیر.

---

## پیکربندی JSON (JSON Configuration)

مسئول برای:
- پیکربندی پیش‌فرض؛
- مقادیر پایه برنامه؛
- تنظیمات غیرحساس.

---

## متغیرهای محیطی (Environment Variables)

مسئول برای:
- بازنویسی‌های زمان استقرار؛
- پیکربندی زمان اجرا؛
- پیکربندی‌های مرتبط با زیرساخت.

---

## .NET User Secrets

مسئول برای:
- اسرار محیط توسعه؛
- اعتبارسنجی‌های محلی توسعه‌دهندگان.

---

## HashiCorp Vault

مسئول برای:
- اسرار محیط عملیاتی (Production)؛
- گواهی‌نامه‌ها؛
- کلیدهای رمزنگاری؛
- اعتبارسنجی‌های سرویس‌های هوش مصنوعی؛
- چرخه حیات کامل اسرار سازمانی.

---

## Azure Key Vault

مسئول برای:
- استقرارهای سازمانی متمرکز بر بسترهای ابری Azure؛
- اسرار مدیریت‌شده در فضای ابری.

---

## Microsoft.FeatureManagement

مسئول برای:
- فعال‌سازی قابلیت‌ها در زمان اجرا؛
- انتشار تدریجی قابلیت‌ها؛
- آزمایش‌های مرتبط با هوش مصنوعی؛
- کلیدهای قطع اضطراری عملیاتی.

---

# 16. تقدم پیکربندی (Configuration Precedence)

اولویت و تقدم توصیه‌شده ارائه‌دهندگان به صورت زیر است:

```text
appsettings.json
        │
        ▼
appsettings.{Environment}.json
        │
        ▼
Environment Variables
        │
        ▼
Secret Store
        │
        ▼
Command Line
```

ارائه‌دهندگانی که دیرتر بارگذاری می‌شوند، مقادیر فراهم‌شده توسط ارائه‌دهندگان پیشین را بازنویسی می‌نمایند.

---

# 17. اصول معماری (Architectural Principles)

معماری توصیه‌شده پیکربندی، تمامی اهداف معماری سامانه را برآورده می‌سازد.

| اصل معماری | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | ✓ |
| وارونگی وابستگی (Dependency Inversion) | ✓ |
| استقلال از ارائه‌دهنده (Provider Independence) | ✓ |
| استقلال از استقرار (Deployment Independence) | ✓ |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | ✓ |
| امنیت سازمانی (Enterprise Security) | ✓ |
| آمادگی برای هوش مصنوعی (AI Readiness) | ✓ |
| قابلیت نگهداری (Maintainability) | ✓ |

---

# 18. استراتژی پیکربندی هوش مصنوعی (AI Configuration Strategy)

پیکربندی‌های حساس هوش مصنوعی منحصراً در مخازن سازمانی اسرار ذخیره خواهند شد.
نمونه‌ها عبارتند از:
- کلیدهای API سرویس OpenAI؛
- اعتبارسنجی‌های Azure OpenAI؛
- کلیدهای ارائه‌دهندگان امبدینگ؛
- اعتبارسنجی‌های جستجوی معنایی.

پیکربندی‌های غیرحساس هوش مصنوعی می‌توانند درون پیکربندی استاندارد باقی بمانند.
نمونه‌ها عبارتند از:
- شناسه‌های مدل پیش‌فرض؛
- مقادیر مهلت زمانی (Timeouts)؛
- سیاست‌های تلاش مجدد (Retry policies)؛
- کلیدهای فعال‌سازی قابلیت‌ها.

---

# 19. ریسک‌ها (Risks)

| ریسک | راهکار کاهش ریسک |
|---|---|
| ثبت تصادفی اسرار در سیستم کنترل نسخه | استفاده الزامی از .NET User Secrets و Vault. |
| وابستگی شدید به یک ارائه‌دهنده (Provider lock-in) | استفاده از انتزاع Configuration Abstraction. |
| پیچیدگی چرخش اسرار | استفاده از چرخش خودکار در HashiCorp Vault. |
| انحراف پیکربندی (Configuration drift) | سلسله‌مراتب پیکربندی مختص به محیط. |
| انباشت پرچم‌های قدیمی قابلیت | تدوین سیاست پاک‌سازی دوره‌ای پرچم‌های قابلیت. |

---

# 20. توصیه نهایی (Final Recommendation)

سامانه MachineryManagerEnterprise باید معماری پیکربندی زیر را استانداردسازی نماید:

| مسئولیت | فناوری انتخابی |
|---|---|
| انتزاع پیکربندی | Microsoft.Extensions.Configuration |
| پیکربندی با نوع‌بندی قوی | Microsoft.Extensions.Options |
| پیکربندی پیش‌فرض | appsettings.json |
| بازنویسی‌های زمان استقرار | متغیرهای محیطی (Environment Variables) |
| اسرار محیط توسعه | .NET User Secrets |
| مخزن سازمانی اسرار | HashiCorp Vault |
| جایگزین اختصاصی Azure | Azure Key Vault |
| پرچم‌های قابلیت | Microsoft.FeatureManagement |

---

# 21. تصمیم نهایی (Final Decision)

معماری مصوب پیکربندی:
- فناوری Microsoft.Extensions.Configuration باید به عنوان انتزاع یکپارچه پیکربندی عمل نماید.
- فناوری Microsoft.Extensions.Options باید به عنوان سازوکار الزامی مصرف پیکربندی به کار گرفته شود.
- فایل‌های JSON صرفاً باید حاوی مقادیر پیش‌فرض غیرحساس باشند.
- متغیرهای محیطی باید بازنویسی‌های زمان استقرار را فراهم آورند.
- استفاده از .NET User Secrets در طول فرآیند توسعه الزامی و اجباری است.
- نرم‌افزار HashiCorp Vault به عنوان پلتفرم ترجیحی و اصلی اسرار سازمانی تعیین می‌شود.
- سرویس Azure Key Vault به عنوان جایگزین مصوب و مختص به بسترهای Azure باقی می‌ماند.
- کتابخانه Microsoft.FeatureManagement کنترل قابلیت‌ها را در زمان اجرا فراهم می‌سازد.

ماژول‌های تجاری هرگز نباید مستقیماً به ارائه‌دهندگان پیکربندی دسترسی داشته باشند.
تنها کلاس‌های Options با نوع‌بندی قوی باید به سرویس‌های برنامه تزریق گردند.

---

# تصمیم معماری مرتبط (Related Architecture Decision)

- ADR-0034 — معماری مدیریت پیکربندی و اسرار (Configuration and Secrets Management Architecture)

---

# خلاصه تصمیم (Decision Summary)

پشته فناوری انتخابی تمامی نیازمندی‌های معماری را برآورده می‌سازد:
- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری کامل با .NET 10
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر
- ✔ آمادگی کامل برای قابلیت‌های هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

---

# ADR مرتبط (Related ADR)

- ADR-0001 — معماری پاک (Clean Architecture)
- ADR-0015 — معماری استقرار (Deployment Architecture)

---

# اسناد مرتبط (Related Documents)

- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-26 | معمار راهکار (Solution Architect) | ارزیابی اولیه فناوری برای مدیریت پیکربندی و اسرار |
| 1.3.0 | 2026-07-28 | معمار راهکار (Solution Architect) | افزودن بخش جدید (محدوده ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار (Solution Architect) | ارتقا به استاندارد مستندسازی نسخه v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار (Solution Architect) | بازبینی و همگام‌سازی با آخرین تغییرات |