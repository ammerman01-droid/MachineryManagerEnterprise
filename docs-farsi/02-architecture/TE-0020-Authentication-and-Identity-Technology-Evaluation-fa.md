| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0020 |
| **عنوان** | ارزیابی فناوری احراز هویت و هویت (.NET 10) (Authentication and Identity Technology Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-26 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند فناوری‌های کاندید را برای ارزیابی فناوری احراز هویت و هویت (.NET 10) در MachineryManagerEnterprise ارزیابی می‌کند.

هدف، ایجاد یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را ضمن حفظ اصول معماری تمیز (Clean Architecture) برآورده سازد.

---

# دامنه ارزیابی (Evaluation Scope)

این ارزیابی فناوری صرفاً انتخاب فناوری را مورد ارزیابی قرار می‌دهد.

جزئیات پیاده‌سازی توسط سوابق تصمیمات معماری (ADRs) مربوطه تعریف می‌شوند.

---

# مراجع معماری (Architectural References)

این ارزیابی مبتنی بر موارد زیر است:

- ADR-0001 — معماری تمیز (Clean Architecture)
- ADR-0008 — معماری امنیت (Security Architecture)
- ADR-0015 — معماری استقرار (Deployment Architecture)
- ADR-0017 — معماری یکپارچه‌سازی خارجی (External Integration Architecture)

احراز هویت باید شرایط زیر را حفظ کند:

- مستقل از ارائه‌دهنده (Provider independent)؛
- منطبق بر استانداردها (Standards compliant)؛
- امن به‌صورت پیش‌فرض (Secure by default)؛
- بی‌طرف نسبت به محیط ابری (Cloud neutral)؛
- سازگار با .NET 10.

---

# نیازمندی‌های کارکردی (Functional Requirements)

پلتفرم به پشتیبانی از موارد زیر نیاز دارد:

- احراز هویت با نام کاربری/رمز عبور (Username/password authentication)؛
- احراز هویت با JWT (JWT authentication)؛
- مجوزدهی مبتنی بر نقش (Role-based authorization)؛
- مجوزدهی مبتنی بر خط‌مشی (Policy-based authorization)؛
- توکن‌های تازه‌سازی (Refresh tokens)؛
- ارائه‌دهندگان خارجی هویت (External identity providers)؛
- احراز هویت سرویس به سرویس (Service-to-service authentication)؛
- احراز هویت API؛
- احراز هویت کلاینت دسکتاپ (Desktop client authentication)؛
- پشتیبانی آتی از ورود یکپارچه (Single Sign-On).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

راهکار انتخاب‌شده باید موارد زیر را فراهم آورد:

- امنیت سازمانی (Enterprise security)؛
- قابلیت توسعه و بسط‌پذیری (Extensibility)؛
- انطباق با استانداردها (Standards compliance)؛
- مقیاس‌پذیری (Scalability)؛
- قابلیت نگهداری (Maintainability)؛
- بی‌طرفی نسبت به ابر (Cloud neutrality)؛
- یکپارچگی عالی با .NET 10.

---

# فناوری‌های کاندید (Candidate Technologies)

## مدیریت هویت (Identity Management)

| فناوری | نقش |
|---|---|
| ASP.NET Core Identity | مدیریت هویت محلی (Local Identity Management) |
| OpenIddict | سرور OAuth2 / OpenID Connect |
| Duende IdentityServer | سرور هویت سازمانی (Enterprise Identity Server) |

---

## فرمت توکن (Token Format)

| فناوری | نقش |
|---|---|
| JWT (RFC 7519) | توکن دسترسی (Access Token) |
| Reference Tokens | استراتژی جایگزین توکن (Alternative Token Strategy) |

---

## ارائه‌دهندگان خارجی هویت (External Identity Providers)

| فناوری | نقش |
|---|---|
| Microsoft Entra ID | هویت سازمانی (Enterprise Identity) |
| Google Identity | ورود خارجی کاربران (External Login) |
| GitHub Identity | ورود توسعه‌دهندگان (Developer Login) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|---|---|---|
| A1 | امنیت | حیاتی (Critical) |
| A2 | انطباق با استانداردها | حیاتی (Critical) |
| A3 | سازگاری با معماری تمیز | حیاتی (Critical) |
| A4 | بی‌طرفی نسبت به ابر | بالا (High) |
| A5 | قابلیت توسعه‌پذیری | بالا (High) |
| A6 | سادگی عملیاتی | متوسط (Medium) |
| A7 | جامعه کاربری و بلوغ | بالا (High) |
| A8 | سازگاری با .NET 10 | حیاتی (Critical) |

---

# اصل معماری (Architecture Principle)

احراز هویت باید کاملاً از منطق کسب‌وکار ایزوله بماند:

```text
Presentation Layer (لایه نمایش)

        │

        ▼

Authentication Abstraction (انتزاع احراز هویت)

        │

        ▼

Identity Provider (ارائه‌دهنده هویت)

        │

 ┌──────────────┬──────────────┐

 ▼              ▼

ASP.NET     OpenIddict
Identity
```

ماژول‌های کسب‌وکار هرگز مستقیماً کاربران را احراز هویت نمی‌کنند، بلکه هویت احراز شده و اطلاعات مجوزها را مصرف می‌نمایند.

---

# ۵. ارزیابی ASP.NET Core Identity (ASP.NET Core Identity Evaluation)

## نمای کلی (Overview)

فریم‌ورک رسمی احراز هویت و مدیریت کاربران مایکروسافت در ASP.NET Core است که امکاناتی نظیر مدیریت حساب‌ها، هشینگ رمز عبور (PBKDF2)، نقش‌ها، Claimها، قفل شدن حساب، بازیابی رمز عبور، تایید ایمیل و احراز هویت دومرحله‌ای (MFA) را فراهم می‌سازد.

---

## نقاط قوت و ضعف معماری

- **نقاط قوت:** ادغام بومی با EF Core، امنیت بسیار بالا، پایداری فوق‌العاده در .NET 10.
- **نقاط ضعف:** به‌تنهایی یک Authorization Server یا پیاده‌ساز پروتکل‌های OAuth2/OIDC نیست و نیاز به مکمل دارد.

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

سامانه ASP.NET Core Identity انتخاب اصلی برای مدیریت محلی کاربران، نقش‌ها و رمزهای عبور خواهد بود.

---

# ۶. ارزیابی OpenIddict (OpenIddict Evaluation)

## نمای کلی (Overview)

یک پیاده‌سازی مدرن، متن‌باز و استاندارد از پروتکل‌های OAuth 2.1 و OpenID Connect برای ASP.NET Core است که به پلتفرم امکان می‌دهد به‌عنوان سرور صدور توکن عمل کند بدون اینکه وابستگی به سرویس‌های ابری تجاری ایجاد نماید.

---

## تفکیک نقش با ASP.NET Core Identity

- **ASP.NET Core Identity:** احراز هویت کاربر را انجام می‌دهد.
- **OpenIddict:** توکن‌های دسترسی استاندارد (JWT) و توکن‌های تازه‌سازی (Refresh Tokens) را با پشتیبانی از PKCE و Authorization Code Flow صادر می‌کند.

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

سامانه OpenIddict پلتفرم استاندارد صدور توکن‌های OAuth 2.1 و OpenID Connect انتخاب می‌شود.

---

# ۷. ارزیابی Duende IdentityServer (Duende IdentityServer Evaluation)

## نمای کلی (Overview)

جانشین تجاری IdentityServer4 با امکانات سازمانی بسیار پیشرفته است.

---

## ارزیابی و دلایل رد (Evaluation and Rejection)

با وجود بلوغ فنی بالا، نیاز به لایسنس تجاری سالانه (Commercial Licensing) دارد. از آنجا که OpenIddict تمامی نیازمندی‌های لازم را به‌صورت متن‌باز، رایگان و بی‌طرف نسبت به ابر پوشش می‌دهد، استفاده از Duende با اهداف مقرون‌به‌صرفه بودن و استقلال پلتفرم مغایرت دارد و رد می‌شود.

---

# ۸. ارزیابی توکن‌های وب جیسون - JWT (JSON Web Token Evaluation)

## نمای کلی (Overview)

فرمت استاندارد صنعتی توکن‌های دسترسی بدون‌حالت (Stateless) مطابق با RFC 7519 است.

---

## نقاط قوت معماری (Architectural Strengths)

- اعتبارسنجی محلی و مستقل توسط هر سرویس بدون نیاز به مراجعه مداوم به سرور مرکزی؛
- مقیاس‌پذیری و کارایی فوق‌العاده بالا؛
- پشتیبانی بومی در کلاینت‌های وب، دسکتاپ، موبایل و سرویس‌های هوش مصنوعی.

---

## استراتژی امنیتی توکن‌ها (Token Strategy)

- **توکن‌های دسترسی (JWT Access Tokens):** عمر کوتاه (۱۰ الی ۳۰ دقیقه)؛
- **توکن‌های تازه‌سازی (Refresh Tokens):** چرخش خودکار کلیدها و ذخیره در دیتابیس؛
- **کلیدهای امضا:** ذخیره‌سازی امن در مخزن اسرار (Vault).

---

# ۹. ارزیابی توکن‌های مرجع (Reference Token Evaluation)

## نمای کلی و نتیجه‌گیری (Overview and Conclusion)

توکن‌های مات (Opaque) که نیازمند اعتبارسنجی رفت‌وبرگشتی (Introspection) به ازای هر ریکوئست هستند. به دلیل تحمیل تاخیر شبکه و بار اضافی روی سرور احراز هویت، به‌عنوان فرمت پیش‌فرض انتخاب نمی‌شوند، اما برای سناریوهای با امنیت حداکثری (ابطال آنی) در دسترس خواهند بود.

---

# ۱۰. ارزیابی Microsoft Entra ID (Microsoft Entra ID Evaluation)

## نمای کلی (Overview)

سامانه مدیریت هویت سازمانی مایکروسافت در فضای ابری (Azure AD سابق).

---

## نقش در معماری (Architectural Role)

صرفاً به‌عنوان یک ارائه‌دهنده احراز هویت خارجی اختیاری (External IdP) برای ورود یکپارچه سازمانی (SSO) در سازمان‌های مبتنی بر مایکروسافت پشتیبانی می‌شود، اما مدل مجوزدهی داخلی برنامه درون پلتفرم باقی می‌ماند.

---

# ۱۱. ارزیابی Google Identity (Google Identity Evaluation)

ارائه‌دهنده اختیاری احراز هویت خارجی (Social Login) برای سناریوهای کلاینت‌های عمومی و شرکا بر پایه OpenID Connect.

---

# ۱۲. ارزیابی GitHub Identity (GitHub Identity Evaluation)

ارائه‌دهنده اختیاری احراز هویت خارجی برای پورتال‌های توسعه‌دهندگان و مهندسی.

---

# ۱۳. مقایسه جامع فناوری‌ها (Overall Technology Comparison)

## ماتریس مسئولیت‌ها (Responsibility Matrix)

| مسئولیت | فناوری توصیه‌شده | جایگزین | هدف |
|---|---|---|---|
| مدیریت کاربران محلی | ASP.NET Core Identity | مخزن سفارشی | حساب‌های کاربری و رمزها |
| سرور OAuth2 / OIDC | OpenIddict | Duende IdentityServer | صدور توکن و پروتکل‌های امنیتی |
| فرمت توکن دسترسی | JWT | Reference Token | احراز هویت در APIها |
| هویت خارجی سازمانی | Microsoft Entra ID | سایر IdPهای سازمانی | SSO سازمانی (اختیاری) |
| احراز هویت مشتریان | Google Identity | GitHub Identity | ورود با اکانت گوگل (اختیاری) |
| احراز هویت توسعه‌دهندگان | GitHub Identity | Google Identity | ورود توسعه‌دهندگان (اختیاری) |

---

## مقایسه قابلیت‌ها (Capability Comparison)

| قابلیت | Identity | OpenIddict | Duende | JWT | Reference Token | Entra ID | Google | GitHub |
|---|---|---|---|---|---|---|---|---|
| مدیریت کاربر | عالی | خیر | خیر | خیر | خیر | متوسط | خیر | خیر |
| OAuth2 / OIDC | محدود | عالی | عالی | صرفاً فرمت | صرفاً فرمت | عالی | عالی | عالی |
| سرور Authorization | خیر | عالی | عالی | خیر | خیر | عالی | خیر | خیر |
| بدون‌حالت (Stateless) | خیر | بله | بله | عالی | ضعیف | بله | بله | بله |
| ابطال آنی | متوسط | عالی | عالی | ضعیف | عالی | عالی | خوب | خوب |
| ورود یکپارچه سازمانی | محدود | خوب | عالی | خیر | خیر | عالی | محدود | محدود |
| لایسنس | متن‌باز | متن‌باز | تجاری | استاندارد باز | استاندارد باز | لایسنس مایکروسافت | رایگان | رایگان |
| بی‌طرفی نسبت به ابر | عالی | عالی | عالی | عالی | عالی | متوسط | خوب | خوب |

---

# ۱۴. پیشنهاد نهایی (Final Recommendation)

## پشته اصلی احراز هویت (Core Authentication Stack)

| مسئولیت | فناوری انتخاب‌شده | دلیل انتخاب |
|---|---|---|
| مدیریت هویت محلی | ASP.NET Core Identity | بالغ، امن، یکپارچه با .NET 10 |
| سرور Authorization | OpenIddict | متن‌باز، منطبق بر استانداردها، بی‌طرف نسبت به ابر |
| فرمت توکن دسترسی | JWT | کارایی بالا، بدون‌حالت، مقیاس‌پذیر |
| توکن‌های تازه‌سازی | OpenIddict | مدیریت امن چرخه حیات توکن |
| مدل مجوزدهی | Policy + Claims + Roles | مجوزدهی منعطف و دقیق سازمانی |

---

## ارائه‌دهندگان اختیاری هویت خارجی (Optional External Identity Providers)

- **Microsoft Entra ID:** ورود سازمانی (SSO) اختیاری
- **Google Identity:** ورود کاربران خارجی اختیاری
- **GitHub Identity:** ورود توسعه‌دهندگان اختیاری

---

# معماری احراز هویت توصیه‌شده (Recommended Authentication Architecture)

```text
                External Identity Providers
                (ارائه‌دهندگان خارجی هویت)

        ┌────────────┬────────────┬────────────┐
        │            │            │
        ▼            ▼            ▼

  Microsoft Entra  Google      GitHub

                  │
                  ▼
        ASP.NET Core Identity (مدیریت هویت محلی)
                  │
                  ▼
              OpenIddict (سرور احراز هویت و صدور توکن)
                  │
                  ▼
      JWT Access / Refresh Tokens (توکن‌های دسترسی)
                  │
                  ▼
          Authorization Middleware (میان‌افزار مجوزدهی)
                  │
                  ▼
            Business Modules (ماژول‌های کسب‌وکار)
```

---

# تصمیم نهایی (Final Decision)

هیئت بازبینی معماری پشته احراز هویت زیر را به تصویب رساند:
- تصویب ASP.NET Core Identity برای مدیریت حساب‌ها.
- تصویب OpenIddict به‌عنوان سرور صدور توکن OAuth 2.1 / OIDC.
- تصویب JWT به‌عنوان توکن‌های دسترسی کوتاه‌مدت به همراه Refresh Tokens.
- پشتیبانی از Entra ID، Google و GitHub به‌عنوان ارائه‌دهندگان خارجی اختیاری.
- رد Duende IdentityServer به دلیل هزینه‌های لایسنس تجاری غیرضروری.

---

# خلاصه تصمیمات (Decision Summary)

پشته فناوری انتخاب‌شده تمامی نیازمندی‌های معماری را برآورده می‌سازد.

- ✔ معماری تمیز (Clean Architecture)
- ✔ امنیت در سطح سازمانی
- ✔ سازگاری با NET 10.
- ✔ بی‌طرفی نسبت به محیط ابری
- ✔ انطباق با استانداردها
- ✔ مقیاس‌پذیری بالا
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

---

# سوابق تصمیمات معماری مرتبط (Related ADR)

- ADR-0001 — معماری تمیز (Clean Architecture)
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
| 1.0.0 | 2026-07-26 | معمار راهکار | ارزیابی اولیه فناوری برای احراز هویت و هویت |
| 1.3.0 | 2026-07-28 | معمار راهکار | افزودن بخش جدید (دامنه ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
