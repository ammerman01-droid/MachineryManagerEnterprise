| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0018 |
| **عنوان** | ارزیابی فناوری مدیریت پیکربندی و اسرار (.NET 10) (Configuration and Secrets Management Technology Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-26 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند فناوری‌های کاندید را برای ارزیابی فناوری مدیریت پیکربندی و اسرار (.NET 10) در MachineryManagerEnterprise ارزیابی می‌کند.

هدف، ایجاد یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را ضمن حفظ اصول معماری تمیز (Clean Architecture) برآورده سازد.

---

# دامنه ارزیابی (Evaluation Scope)

این ارزیابی فناوری صرفاً انتخاب فناوری را مورد ارزیابی قرار می‌دهد.

جزئیات پیاده‌سازی توسط سوابق تصمیمات معماری (ADRs) مربوطه تعریف می‌شوند.

---

# مراجع معماری (Architectural References)

این ارزیابی مبتنی بر موارد زیر است:

- ADR-0001 — معماری تمیز (Clean Architecture)
- ADR-0015 — معماری استقرار (Deployment Architecture)
- ADR-0018 — معماری یکپارچه‌سازی خارجی (External Integration Architecture)

مدیریت پیکربندی باید شرایط زیر را حفظ کند:

- مستقل از ارائه‌دهنده (Provider independent)؛
- مستقل از استقرار (Deployment independent)؛
- بی‌طرف نسبت به محیط ابری (Cloud neutral)؛
- امن به‌صورت پیش‌فرض (Secure by default).

---

# نیازمندی‌های کارکردی (Functional Requirements)

پلتفرم به پشتیبانی از موارد زیر نیاز دارد:

- پیکربندی برنامه (Application configuration)؛
- پیکربندی مختص به هر محیط (Environment-specific configuration)؛
- بارگذاری مجدد پیکربندی در زمان اجرا (Runtime configuration reload)؛
- پیکربندی با نوع‌بندی قوی (Strongly typed configuration)؛
- ذخیره‌سازی امن اسرار و اطلاعات حساس (Secure secret storage)؛
- رشته‌های اتصال (Connection strings)؛
- کلیدهای API؛
- اطلاعات احراز هویت ارائه‌دهندگان هوش مصنوعی (AI provider credentials)؛
- کلیدهای امضای JWT؛
- پیکربندی گواهینامه‌ها؛
- پرچم‌های ویژگی (Feature flags).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

راهکار انتخاب‌شده باید موارد زیر را فراهم آورد:

- امنیت در سطح سازمانی؛
- مقیاس‌پذیری؛
- انعطاف‌پذیری استقرار؛
- بی‌طرفی نسبت به ابر؛
- قابلیت نگهداری؛
- سادگی عملیاتی؛
- سازگاری کامل با NET 10.

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
| متغیرهای محیطی (Environment Variables) | پیکربندی زمان استقرار (Deployment Configuration) |
| خط فرمان (Command Line) | بازنویسی در زمان اجرا (Runtime Override) |

---

## مدیریت اسرار (Secrets Management)

| فناوری | نقش |
|---|---|
| .NET User Secrets | اسرار محیط توسعه (Development Secrets) |
| Azure Key Vault | مخزن اسرار سازمانی (Enterprise Secret Store) |
| HashiCorp Vault | مخزن اسرار سازمانی (Enterprise Secret Store) |

---

## مدیریت پرچم‌های ویژگی (Feature Management)

| فناوری | نقش |
|---|---|
| Microsoft.FeatureManagement | پرچم‌های ویژگی (Feature Flags) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|---|---|---|
| C1 | سازگاری با معماری تمیز | حیاتی (Critical) |
| C2 | امنیت | حیاتی (Critical) |
| C3 | استقلال از استقرار | حیاتی (Critical) |
| C4 | بی‌طرفی نسبت به ابر | بالا (High) |
| C5 | انعطاف‌پذیری زمان اجرا | بالا (High) |
| C6 | قابلیت نگهداری | بالا (High) |
| C7 | آمادگی سازمانی | بالا (High) |
| C8 | یکپارچگی با .NET 10 | حیاتی (Critical) |

---

# اصل معماری (Architecture Principle)

پیکربندی به‌عنوان یک دغدغه لایه زیرساخت (Infrastructure concern) در نظر گرفته می‌شود.

ماژول‌های کسب‌وکار هرگز نباید مستقیماً به ارائه‌دهندگان پیکربندی دسترسی داشته باشند، بلکه:

```text
Business Modules (ماژول‌های کسب‌وکار)

        │

        ▼

Strongly Typed Options (گزینه‌های با نوع‌بندی قوی)

        │

        ▼

Configuration Abstraction (انتزاع پیکربندی)

        │

 ┌──────────────┬──────────────┬──────────────┐

 ▼              ▼              ▼

JSON       Environment      Secret Store
          (متغیرهای محیطی)   (مخزن اسرار)
```

این معماری تضمین می‌کند که ارائه‌دهندگان پیکربندی می‌توانند بدون تاثیر بر منطق کسب‌وکار تکامل یابند.

---

# ۵. ارزیابی Microsoft.Extensions.Configuration (Microsoft.Extensions.Configuration Evaluation)

## نمای کلی (Overview)

لایه انتزاع رسمی ارائه‌شده توسط مایکروسافت برای .NET 10 است که یک API یکپارچه روی ارائه‌دهندگان مختلف پیکربندی (JSON، متغیرهای محیطی، خط فرمان و مخازن اسرار) فراهم می‌سازد.

---

## نقاط قوت معماری (Architectural Strengths)

- انتزاع رسمی با پشتیبانی بومی .NET 10؛
- یکپارچگی کامل با تزریق وابستگی و الگوی Options؛
- پیکربندی سلسله‌مراتبی و پشتیبانی از بارگذاری مجدد زمان اجرا؛
- آمادگی سازمانی و پایداری فوق‌العاده API.

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

واسط Microsoft.Extensions.Configuration باید به‌عنوان تنها انتزاع پیکربندی در سراسر MachineryManagerEnterprise مورد استفاده قرار گیرد.

---

# ۶. ارزیابی Microsoft.Extensions.Options (Microsoft.Extensions.Options Evaluation)

## نمای کلی (Overview)

مکانیزم رسمی مایکروسافت برای مقیدسازی پیکربندی به شیء‌های با نوع‌بندی قوی (Strongly typed) و تزریق آن‌ها از طریق Dependency Injection است.

---

## نقش معماری (Architectural Role)

ماژول‌های کسب‌وکار هرگز مستقیماً مقادیر متنی پیکربندی مانند `_configuration["Jwt:Issuer"]` را نمی‌خوانند، بلکه کلاس‌های تنظیمات نظیر `JwtOptions` را دریافت می‌کنند.

---

## نقاط قوت معماری (Architectural Strengths)

- ایمنی در زمان کامپایل (Compile-time safety)؛
- حذف مقادیر متنی هاردکدشده و افزایش خوانایی و تست‌پذیری؛
- پشتیبانی از اعتبارسنجی گزینه‌ها (Options Validation)؛
- پشتیبانی از وضعیت‌های اسنپ‌شات و مانیتورینگ تغییرات (`IOptionsSnapshot` و `IOptionsMonitor`).

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

الگوی Microsoft.Extensions.Options باید مکانیزم اجباری برای مصرف پیکربندی‌ها در سراسر سرویس‌های برنامه باشد.

---

# ۷. ارزیابی فایل‌های پیکربندی JSON (JSON Configuration Files Evaluation)

## نمای کلی (Overview)

فایل‌های JSON (نظیر `appsettings.json` و `appsettings.Development.json`) منبع پایه و پیش‌فرض برای مقادیر غیرحساس پیکربندی در .NET 10 هستند.

---

## نقاط ضعف و الزامات امنیتی (Security Limitations)

فایل‌های JSON هرگز نباید برای داده‌های حساس و محرمانه استفاده شوند:
- **مناسب برای:** سطوح لاگ‌گیری، مدت زمان کش، مقادیر تایم‌اوت، تنظیمات پیش‌فرض ماژول‌ها.
- **اکیداً ممنوع برای:** رمزهای عبور، کلیدهای OpenAI، کلیدهای JWT، رمزهای Redis، گواهینامه‌ها.

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

فایل‌های JSON منبع اصلی پیکربندی‌های غیرحساس و پیش‌فرض باقی می‌مانند و تمام مقادیر حساس به مخازن امن اسرار واگذار می‌شوند.

---

# ۸. ارزیابی متغیرهای محیطی (Environment Variables Evaluation)

## نمای کلی (Overview)

متغیرهای محیطی مکانیزم استاندارد برای پیکربندی زمان استقرار در برنامه‌های Cloud-Native، کانتینرها، پایپ‌لاین‌های CI/CD و کوبرنتیز مطابق با اصول Twelve-Factor App هستند.

---

## اولویت پیکربندی (Configuration Precedence)

```text
appsettings.json (پیکربندی پایه)

        │

        ▼

appsettings.{Environment}.json (پیکربندی محیطی)

        │

        ▼

Environment Variables (متغیرهای محیطی استقرار)

        │

        ▼

Secret Store (مخزن اسرار)

        │

        ▼

Command Line (آرگومان‌های خط فرمان)
```

ارائه‌دهندگانی که دیرتر فراخوانی می‌شوند، مقادیر قبلی را بازنویسی (Override) می‌کنند.

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

متغیرهای محیطی استاندارد اصلی برای تنظیمات زمان استقرار در محیط‌های اجرایی و کانتینری خواهند بود.

---

# ۹. ارزیابی .NET User Secrets (.NET User Secrets Evaluation)

## نمای کلی (Overview)

ابزار تعبیه‌شده مایکروسافت برای ذخیره‌سازی اسرار زمان توسعه در خارج از درخت سورس‌کد و دور از کنترل نسخه (Git) در سیستم توسعه‌دهنده است.

---

## دامنه کاربرد (Scope)

- **صرفاً جهت محیط توسعه محلی:** کلیدهای آزمایشی OpenAI، رمز دیتابیس لوکال و غیره.
- **اکیداً نامناسب برای:** محیط‌های عملیاتی، Staging، سرورها و کانتینرهای پروداکشن.

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

استفاده از .NET User Secrets در محیط توسعه اجباری است تا هیچ رازداری به‌اشتباه وارد مخزن کد نشود.

---

# ۱۰. ارزیابی Azure Key Vault (Azure Key Vault Evaluation)

## نمای کلی (Overview)

سرویس ابری سازمانی مایکروسافت برای مدیریت متمرکز اسرار، کلیدهای رمزنگاری و گواهینامه‌ها در محیط Azure است.

---

## ارزیابی معماری (Architectural Assessment)

امنیت و کارایی در سطح عالی است، اما به دلیل ایجاد وابستگی به بستر Azure و کاهش بی‌طرفی نسبت به ابر (Cloud Neutrality)، به‌عنوان یک گزینه پشتیبانی‌شده مختص محیط‌های Azure در نظر گرفته می‌شود و نه انتخاب اول معماری بی‌طرف.

---

# ۱۱. ارزیابی HashiCorp Vault (HashiCorp Vault Evaluation)

## نمای کلی (Overview)

سامانه HashiCorp Vault یک پلتفرم سازمانی، بی‌طرف نسبت به ارائه‌دهنده و چندابری (Multi-Cloud / On-Premise) برای ذخیره‌سازی، توزیع و چرخش خودکار اسرار، تولید رمزهای داینامیک و مدیریت گواهینامه‌ها است.

---

## نقاط قوت معماری (Architectural Strengths)

- کاملاً بی‌طرف نسبت به ارائه‌دهنده و ابر؛
- پشتیبانی از استقرار در محل (On-Premise) و تمام ابرها؛
- اسرار پویا (Dynamic secrets) و اجاره اسرار (Secret leasing)؛
- چرخش خودکار کلیدها و موتورهای رمزنگاری Transit؛
- ممیزی و کنترل‌های امنیتی جامع Zero-Trust.

---

## مقایسه Azure Key Vault و HashiCorp Vault

| قابلیت | Azure Key Vault | HashiCorp Vault |
|---|---|---|
| بی‌طرفی نسبت به ارائه‌دهنده | متوسط (Moderate) | عالی (Excellent) |
| یکپارچگی با Azure | عالی (Excellent) | بسیار خوب (Very Good) |
| پشتیبانی چندابری | متوسط (Moderate) | عالی (Excellent) |
| اسرار پویا | محدود (Limited) | عالی (Excellent) |
| اجاره اسرار (Leasing) | خیر (No) | عالی (Excellent) |
| انعطاف‌پذیری استقرار | خوب (Good) | عالی (Excellent) |
| سادگی عملیاتی | عالی (Excellent) | متوسط (Moderate) |
| حاکمیت سازمانی | عالی (Excellent) | عالی (Excellent) |

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

سامانه HashiCorp Vault به دلیل انطباق کامل با اصل بی‌طرفی نسبت به ابر و استقلال زیرساخت، پلتفرم ارجح مدیریت اسرار سازمانی انتخاب می‌شود.

---

# ۱۲. ارزیابی Microsoft.FeatureManagement (Microsoft.FeatureManagement Evaluation)

## نمای کلی (Overview)

فریم‌ورک رسمی مایکروسافت برای مدیریت پرچم‌های ویژگی (Feature Flags) در .NET 10 است که امکان فعال‌سازی تدریجی قابلیت‌ها، آزمایش‌های A/B و کلیدهای قطع اضطراری (Kill switches) را بدون نیاز به دیپلوی مجدد فراهم می‌سازد.

---

## کاربرد در هوش مصنوعی (AI Compatibility)

امکان فعال‌سازی تدریجی جستجوی معنایی، جابه‌جایی ارائه‌دهنده مدل، فعال‌سازی RAG و تست مدل‌های جدید استنتاج بدون ایجاد ریسک برای بارهای کاری پروداکشن.

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

فریم‌ورک Microsoft.FeatureManagement به‌عنوان فریم‌ورک استاندارد پرچم‌های ویژگی در پلتفرم انتخاب می‌شود.

---

# ۱۳. مقایسه جامع فناوری‌ها (Overall Technology Comparison)

## ماتریس مسئولیت‌ها (Responsibility Matrix)

| قابلیت | فناوری توصیه‌شده | جایگزین | مسئولیت |
|---|---|---|---|
| انتزاع پیکربندی | Microsoft.Extensions.Configuration | — | API یکپارچه پیکربندی |
| پیکربندی با نوع‌بندی قوی | Microsoft.Extensions.Options | — | مصرف پیکربندی در سرویس‌ها |
| پیکربندی پیش‌فرض | appsettings.json | XML | پیکربندی استاتیک غیرحساس |
| پیکربندی استقرار | Environment Variables | Command Line | بازنویسی متغیرهای محیطی |
| اسرار محیط توسعه | .NET User Secrets | فایل‌های محلی | ذخیره‌سازی اسرار توسعه‌دهنده |
| اسرار سازمانی | HashiCorp Vault | Azure Key Vault | مدیریت اسرار پروداکشن |
| پرچم‌های ویژگی | Microsoft.FeatureManagement | LaunchDarkly | کنترل قابلیت‌ها در زمان اجرا |

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

# ۱۴. معماری پیکربندی توصیه‌شده (Recommended Configuration Architecture)

```text
                  Business Modules (ماژول‌های کسب‌وکار)

                         │

                         ▼

                 Strongly Typed Options (گزینه‌های با نوع‌بندی قوی)

                         │

                         ▼

          Microsoft.Extensions.Configuration

                         │

      ┌──────────────────┼──────────────────────┐

      ▼                  ▼                      ▼

appsettings.json   Environment Variables   Secret Providers
(پیش‌فرض‌ها)       (متغیرهای محیطی)         (ارائه‌دهندگان اسرار)

                                               │

                                 ┌─────────────┴─────────────┐

                                 ▼                           ▼

                         HashiCorp Vault              Azure Key Vault
                         (انتخاب اصلی)                (جایگزین تاییدشده)

                         │

                         ▼

                Microsoft.FeatureManagement
```

---

# ۱۵. مسئولیت‌های پیکربندی (Configuration Responsibilities)

- **Microsoft.Extensions.Configuration:** انتزاع ارائه‌دهندگان و تجمیع داده‌ها.
- **Microsoft.Extensions.Options:** تزریق مقادیر به‌صورت شیء‌های با نوع‌بندی قوی به سرویس‌ها.
- **فایل‌های JSON:** تنظیمات پیش‌فرض و غیرحساس برنامه.
- **متغیرهای محیطی:** بازنویسی مقادیر زمان استقرار در کانتینرها.
- **.NET User Secrets:** محافظت از اسرار محیط توسعه محلی.
- **HashiCorp Vault:** مدیریت چرخه حیات اسرار، کلیدها، گواهینامه‌ها و مدارک هوش مصنوعی در پروداکشن.
- **Azure Key Vault:** گزینه جایگزین برای استقرارهای مختص محیط Azure.
- **Microsoft.FeatureManagement:** مدیریت پرچم‌های ویژگی و رول‌اوت تدریجی.

---

# ۱۶. اولویت اعمال پیکربندی (Configuration Precedence)

```text
appsettings.json
      │
      ▼
appsettings.{Environment}.json
      │
      ▼
متغیرهای محیطی (Environment Variables)
      │
      ▼
مخزن اسرار (Secret Store)
      │
      ▼
خط فرمان (Command Line)
```

---

# ۱۷. اصول معماری (Architectural Principles)

| اصل | ارزیابی |
|---|---|
| معماری تمیز | ✓ |
| وارونگی وابستگی | ✓ |
| استقلال از ارائه‌دهنده | ✓ |
| استقلال از استقرار | ✓ |
| بی‌طرفی نسبت به ابر | ✓ |
| امنیت در سطح سازمانی | ✓ |
| آمادگی برای هوش مصنوعی | ✓ |
| قابلیت نگهداری | ✓ |

---

# ۱۸. استراتژی پیکربندی هوش مصنوعی (AI Configuration Strategy)

کلیدهای محرمانه ارائه‌دهندگان هوش مصنوعی (مانند OpenAI API Key و اطلاعات Azure OpenAI) باید منحصراً در مخزن اسرار نگهداری شوند. تنظیمات عمومی مانند نام مدل پیش‌فرض، تایم‌اوت‌ها و ابعاد امبدینگ در فایل‌های پیکربندی استاندارد قرار می‌گیرند.

---

# ۱۹. ریسک‌ها (Risks)

| ریسک | اقدام کاهنده |
|---|---|
| کامیت شدن اسرار در گیت | استفاده اجباری از .NET User Secrets و Vault |
| وابستگی به ارائه‌دهنده | لایه انتزاع پیکربندی |
| پیچیدگی چرخش اسرار | چرخش خودکار در HashiCorp Vault |
| انباشتگی پرچم‌های ویژگی | خط‌مشی دوره‌ای پاک‌سازی پرچم‌ها |

---

# ۲۰. پیشنهاد نهایی (Final Recommendation)

پلتفرم MachineryManagerEnterprise باید روی معماری پیکربندی زیر استانداردسازی شود:

| مسئولیت | فناوری انتخاب‌شده |
|---|---|
| انتزاع پیکربندی | Microsoft.Extensions.Configuration |
| پیکربندی با نوع‌بندی قوی | Microsoft.Extensions.Options |
| پیکربندی پیش‌فرض | appsettings.json |
| بازنویسی‌های استقرار | متغیرهای محیطی (Environment Variables) |
| اسرار محیط توسعه | .NET User Secrets |
| مخزن اسرار سازمانی | HashiCorp Vault |
| جایگزین محیط Azure | Azure Key Vault |
| پرچم‌های ویژگی | Microsoft.FeatureManagement |

---

# ۲۱. تصمیم نهایی (Final Decision)

معماری پیکربندی زیر به تصویب رسید:
- استفاده از Microsoft.Extensions.Configuration به‌عنوان انتزاع یکپارچه.
- استفاده اجباری از Microsoft.Extensions.Options برای مصرف پیکربندی در کدهای برنامه.
- استفاده از فایل‌های JSON صرفاً برای داده‌های غیرحساس پیش‌فرض.
- استفاده از متغیرهای محیطی برای تنظیمات زمان استقرار.
- استفاده از .NET User Secrets در محیط توسعه.
- استفاده از HashiCorp Vault به‌عنوان پلتفرم ارجح اسرار سازمانی.
- استفاده از Microsoft.FeatureManagement جهت مدیریت پرچم‌های ویژگی.

ماژول‌های کسب‌وکار هرگز مستقیماً به ارائه‌دهندگان پیکربندی دسترسی نخواهند داشت.

---

# سوابق تصمیمات معماری مرتبط (Related ADR)

- ADR-0001 — معماری تمیز (Clean Architecture)
- ADR-0015 — معماری استقرار (Deployment Architecture)
- ADR-0034 — معماری مدیریت پیکربندی و اسرار (Configuration and Secrets Management Architecture)

---

# خلاصه تصمیمات (Decision Summary)

پشته فناوری انتخاب‌شده تمامی نیازمندی‌های معماری را برآورده می‌سازد.

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به محیط ابری
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

---

# اسناد مرتبط (Related Documents)

- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-26 | معمار راهکار | ارزیابی اولیه فناوری برای مدیریت پیکربندی و اسرار |
| 1.3.0 | 2026-07-28 | معمار راهکار | افزودن بخش جدید (دامنه ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
