| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | TE-0020 |
| **عنوان** | ارزیابی فناوری احراز هویت و هویت (Authentication and Identity Technology Evaluation) (.NET 10) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-26 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند فناوری‌های کاندید برای ارزیابی فناوری احراز هویت و هویت (.NET 10) را در MachineryManagerEnterprise مورد ارزیابی قرار می‌دهد.

هدف، ایجاد یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را برآورده سازد و هم‌زمان اصول معماری تمیز (Clean Architecture) را حفظ نماید.

---

# محدوده ارزیابی (Evaluation Scope)

این ارزیابی فناوری صرفاً انتخاب فناوری را مورد ارزیابی قرار می‌دهد.

جزئیات پیاده‌سازی توسط سوابق تصمیم‌گیری معماری (ADRs) مربوطه تعریف می‌شوند.

---

# مراجع معماری (Architectural References)

این ارزیابی بر اساس مراجع زیر استوار است:

- ADR-0001 — معماری تمیز (Clean Architecture)
- ADR-0008 — معماری امنیت (Security Architecture)
- ADR-0015 — معماری استقرار (Deployment Architecture)
- ADR-0017 — معماری یکپارچگی خارجی (External Integration Architecture)

احراز هویت باید همواره ویژگی‌های زیر را حفظ کند:

- مستقل از ارائه‌دهنده (provider independent)؛
- منطبق با استانداردها (standards compliant)؛
- امن به صورت پیش‌فرض (secure by default)؛
- بی‌طرف نسبت به ابر (cloud neutral)؛
- سازگار با .NET 10.

---

# نیازمندی‌های کارکردی (Functional Requirements)

پلتفرم نیازمند پشتیبانی از موارد زیر است:

- احراز هویت با نام کاربری/رمز عبور (username/password authentication)؛
- احراز هویت مبتنی بر JWT (JWT authentication)؛
- اعطای مجوز مبتنی بر نقش (role-based authorization)؛
- اعطای مجوز مبتنی بر خط‌مشی (policy-based authorization)؛
- توکن‌های نوسازی (refresh tokens)؛
- ارائه‌دهندگان هویت خارجی (external identity providers)؛
- احراز هویت سرویس به سرویس (service-to-service authentication)؛
- احراز هویت API؛
- احراز هویت کلاینت دسکتاپ (desktop client authentication)؛
- پشتیبانی آتی از ورود یکپارچه (Single Sign-On).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

راهکار انتخاب‌شده باید موارد زیر را فراهم آورد:

- امنیت سازمانی (enterprise security)؛
- قابلیت توسعه و گسترش‌پذیری (extensibility)؛
- انطباق با استانداردها (standards compliance)؛
- مقیاس‌پذیری (scalability)؛
- قابلیت نگهداری (maintainability)؛
- بی‌طرفی نسبت به ابر (cloud neutrality)؛
- یکپارچگی عالی با .NET 10.

---

# فناوری‌های کاندید (Candidate Technologies)

## مدیریت هویت (Identity Management)

| فناوری | نقش |
|------------|------|
| ASP.NET Core Identity | مدیریت هویت محلی (Local Identity Management) |
| OpenIddict | سرور OAuth2 / OpenID Connect |
| Duende IdentityServer | سرور هویت سازمانی (Enterprise Identity Server) |

---

## فرمت توکن (Token Format)

| فناوری | نقش |
|------------|------|
| JWT (RFC 7519) | توکن دسترسی (Access Token) |
| Reference Tokens | استراتژی جایگزین توکن (Alternative Token Strategy) |

---

## ارائه‌دهندگان هویت خارجی (External Identity Providers)

| فناوری | نقش |
|------------|------|
| Microsoft Entra ID | هویت سازمانی (Enterprise Identity) |
| Google Identity | ورود خارجی (External Login) |
| GitHub Identity | ورود توسعه‌دهندگان (Developer Login) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | ضریب اهمیت |
|----|-----------|--------|
| A1 | امنیت (Security) | حیاتی (Critical) |
| A2 | انطباق با استانداردها (Standards Compliance) | حیاتی (Critical) |
| A3 | سازگاری با معماری تمیز (Clean Architecture Compatibility) | حیاتی (Critical) |
| A4 | بی‌طرفی نسبت به ابر (Cloud Neutrality) | بالا (High) |
| A5 | قابلیت توسعه‌پذیری (Extensibility) | بالا (High) |
| A6 | سادگی عملیاتی (Operational Simplicity) | متوسط (Medium) |
| A7 | جامعه کاربری و بلوغ (Community & Maturity) | بالا (High) |
| A8 | سازگاری با .NET 10 | حیاتی (Critical) |

---

# اصل معماری (Architecture Principle)

احراز هویت باید کاملاً از منطق کسب‌وکار ایزوله و تفکیک شده باقی بماند.

```text
Presentation Layer

        │

        ▼

Authentication Abstraction

        │

        ▼

Identity Provider

        │

 ┌──────────────┬──────────────┐

 ▼              ▼

ASP.NET     OpenIddict
Identity
```

ماژول‌های کسب‌وکار هرگز کاربران را مستقیماً احراز هویت نمی‌کنند.

آن‌ها صرفاً هویت‌های احراز هویت‌شده و اطلاعات مجوزها (authorization information) را مصرف می‌کنند.

---

# 5. ارزیابی ASP.NET Core Identity (ASP.NET Core Identity Evaluation)

## نمای کلی (Overview)

فریم‌ورک ASP.NET Core Identity فریم‌ورک رسمی مایکروسافت برای مدیریت هویت در برنامه‌های .NET است.

این فریم‌ورک مدیریت محلی کاربران شامل موارد زیر را فراهم می‌سازد:

- حساب‌های کاربری (user accounts)؛
- هش کردن رمز عبور (password hashing)؛
- نقش‌ها (roles)؛
- ادعاها (claims)؛
- قفل شدن حساب کاربری (lockout)؛
- بازیابی رمز عبور (password reset)؛
- تایید ایمیل (email confirmation)؛
- احراز هویت دومرحله‌ای (two-factor authentication).

فریم‌ورک Identity به صورت بومی با Entity Framework Core و میان‌افزار احراز هویت ASP.NET Core یکپارچه می‌شود.

---


# 5. ارزیابی ASP.NET Core Identity (ASP.NET Core Identity Evaluation)

## نمای کلی (Overview)

فریم‌ورک ASP.NET Core Identity فریم‌ورک رسمی احراز هویت و مدیریت هویت مایکروسافت برای برنامه‌های ASP.NET Core است.

این فریم‌ورک یک راهکار کامل هویت محلی شامل موارد زیر را فراهم می‌آورد:

- مدیریت کاربران (user management)؛
- هش کردن رمز عبور (password hashing)؛
- نقش‌ها (roles)؛
- ادعاها (claims)؛
- قفل شدن حساب کاربری (lockout)؛
- بازیابی رمز عبور (password recovery)؛
- تایید ایمیل (email confirmation)؛
- احراز هویت چندمرحله‌ای (multi-factor authentication).

فریم‌ورک Identity به صورت بومی با موارد زیر یکپارچه می‌شود:

- احراز هویت ASP.NET Core؛
- فریم‌ورک Entity Framework Core؛
- میان‌افزار اعطای مجوز (Authorization Middleware)؛
- تزریق وابستگی (Dependency Injection).

در MachineryManagerEnterprise این فریم‌ورک به عنوان راهکار اصلی مدیریت هویت محلی ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

فریم‌ورک Identity به لایه Infrastructure Security تعلق دارد.

```text
Presentation Layer

        │

Authentication Middleware

        │

        ▼

Authentication Abstraction

        │

        ▼

ASP.NET Core Identity

        │

        ▼

Identity Store
```

ماژول‌های کسب‌وکار هرگز موجودیت‌های Identity را مستقیماً دستکاری نمی‌کنند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- فریم‌ورک رسمی مایکروسافت.
- پشتیبانی بومی از .NET 10.
- یکپارچگی عالی با ASP.NET Core.
- مدیریت نقش‌ها (Role management).
- مدیریت ادعاها (Claims management).
- هش کردن رمز عبور.
- پشتیبانی از MFA.
- محافظت در برابر قفل حساب (Lockout protection).
- اعتبارسنجی مهر امنیتی (Security stamp validation).
- اکوسیستم بالغ.
- جامعه کاربری بزرگ.

---

# نقاط ضعف معماری (Architectural Weaknesses)

فریم‌ورک ASP.NET Core Identity عمداً بر روی **مدیریت هویت** متمرکز است، نه فدراسیون هویت سازمانی.

محدودیت‌ها عبارتند از:

- سرور مجوزدهی OAuth2 (OAuth2 Authorization Server) نیست؛
- ارائه‌دهنده OpenID Connect نیست؛
- به خودی خود فدراسیون سازمانی ارائه نمی‌دهد؛
- قابلیت محدود برای SSO در میان چندین برنامه.

این قابلیت‌ها نیازمند فناوری‌های مکمل نظیر OpenIddict هستند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

فریم‌ورک Identity موارد زیر را فراهم می‌آورد:

- چرخه حیات کاربر (user lifecycle)؛
- مدیریت اطلاعات کاربری و اعتباری (credential management)؛
- مدیریت و راهبری نقش‌ها (role administration)؛
- تخصیص ادعاها (claim assignment)؛
- خط‌مشی‌های رمز عبور (password policies)؛
- امنیت حساب کاربری (account security).

پیچیدگی عملیاتی پایین ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

فریم‌ورک Identity برای موارد زیر به خوبی مقیاس‌پذیر است:

- APIهای وب سازمانی؛
- مونولیت‌های ماژولار (modular monoliths)؛
- استقرارهای متوسط تا بزرگ.

مقیاس‌پذیری افقی در درجه اول به پایگاه داده پشتیبان بستگی دارد تا خود فریم‌ورک.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# امنیت (Security)

امنیت یکی از قوی‌ترین ویژگی‌های Identity است.

قابلیت‌های درون‌ساخته شامل موارد زیر است:

- هش کردن رمز عبور با الگوریتم PBKDF2؛
- خط‌مشی‌های قابل پیکربندی رمز عبور؛
- قفل شدن حساب کاربری؛
- مهرهای امنیتی (security stamps)؛
- احراز هویت دومرحله‌ای؛
- محافظت از کوکی‌ها؛
- اعطای مجوز مبتنی بر ادعا (claims-based authorization).

امنیت عالی ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

انعطاف‌پذیری استقرار عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

فریم‌ورک Identity به خودی خود نسبت به هوش مصنوعی خنثی و بی‌طرف است.

با این حال، سرویس‌های هوش مصنوعی از هویت‌های احراز هویت‌شده بهره‌مند می‌شوند که شامل موارد زیر هستند:

- ادعاهای کاربر (user claims)؛
- مجوزها و دسترسی‌ها (permissions)؛
- اطلاعات مستاجر (tenant information)؛
- متادیتای حسابرسی (auditing metadata).

این امر مجوزدهی امن برای عملیات‌های مبتنی بر هوش مصنوعی را ممکن می‌سازد.

---

# قابلیت نگهداری (Maintainability)

فریم‌ورک Identity موارد زیر را فراهم می‌آورد:

- APIهای پایدار؛
- پشتیبانی بلندمدت مایکروسافت؛
- مستندات جامع و قوی؛
- مسیر ارتقاء قابل پیش‌بینی.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# قابلیت توسعه‌پذیری (Extensibility)

فریم‌ورک Identity از سفارشی‌سازی از طریق موارد زیر پشتیبانی می‌کند:

- موجودیت‌های سفارشی کاربر (custom user entities)؛
- موجودیت‌های سفارشی نقش (custom role entities)؛
- تبدیل ادعاها (claims transformation)؛
- ذخیره‌سازهای سفارشی (custom stores)؛
- ارائه‌دهندگان احراز هویت خارجی.

این انعطاف‌پذیری آن را برای سیستم‌های سازمانی بسیار مناسب می‌سازد.

---

# کاربرد معمول (Typical Usage)

سناریوهای مناسب:

```text
User Accounts

Password Authentication

Roles

Claims

MFA

Local Identity Management
```

سناریوهای نامناسب:

```text
Enterprise SSO

OAuth2 Authorization Server

OpenID Connect Provider

Identity Federation
```

این قابلیت‌ها به فناوری‌های اختصاصی سرور هویت تعلق دارند.

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| امنیت (Security) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| قابلیت توسعه‌پذیری (Extensibility) | عالی (Excellent) |
| انطباق با استانداردها (Standards Compliance) | بسیار خوب (Very Good) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |

---

# ارتباط با OpenIddict (Relationship with OpenIddict)

فریم‌ورک Identity کاربران را مدیریت می‌کند.

کتابخانه OpenIddict توکن‌ها را صادر می‌نماید.

```text
User

        │

        ▼

ASP.NET Core Identity

        │

        ▼

OpenIddict

        │

        ▼

JWT Access Token
```

این دو فناوری به جای رقابت، مکمل یکدیگر هستند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

فریم‌ورک ASP.NET Core Identity قوی‌ترین انتخاب برای مدیریت هویت محلی در MachineryManagerEnterprise است.

این فریم‌ورک باید به سازوکار استاندارد برای موارد زیر تبدیل شود:

- حساب‌های کاربری؛
- مدیریت رمز عبور؛
- نقش‌ها؛
- ادعاها؛
- احراز هویت.

احراز هویت فدراسیونی و صدور توکن به صورت جداگانه توسط OpenIddict مدیریت خواهد شد.

---

# 6. ارزیابی OpenIddict (OpenIddict Evaluation)

## نمای کلی (Overview)

کتابخانه OpenIddict یک پیاده‌سازی مدرن و متن‌باز از مشخصات OAuth 2.1 و OpenID Connect برای ASP.NET Core است.

برخلاف ASP.NET Core Identity که مسئول **مدیریت هویت** است، OpenIddict مسئول **صدور توکن و پروتکل‌های اعطای مجوز** است.

کتابخانه OpenIddict پلتفرم MachineryManagerEnterprise را قادر می‌سازد تا به عنوان سرور مجوزدهی اختصاصی خود عمل کند در حالی که کاملاً درون اکوسیستم .NET یکپارچه باقی می‌ماند.

---

# نقش معماری (Architectural Role)

کتابخانه OpenIddict به لایه Identity Provider تعلق دارد.

```text
Presentation Layer

        │

Authentication Middleware

        │

        ▼

ASP.NET Core Identity

        │

        ▼

OpenIddict

        │

        ▼

OAuth2 / OpenID Connect

        │

        ▼

JWT Access Tokens
```

فریم‌ورک Identity کاربران را احراز هویت می‌کند.

کتابخانه OpenIddict توکن‌های امنیتی منطبق با استانداردها را صادر می‌نماید.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- پیاده‌سازی رسمی با اولویت .NET (.NET-first).
- متن‌باز (Open Source).
- یکپارچگی بومی با ASP.NET Core.
- پشتیبانی از OAuth 2.1.
- پشتیبانی از OpenID Connect.
- صدور توکن JWT.
- پشتیبانی از Refresh Token.
- جریان Authorization Code Flow.
- پشتیبانی از PKCE.
- پشتیبانی از جریان Device Flow.
- سفارشی‌سازی دقیق و منعطف.
- سازگاری عالی با .NET 10.

---

# نقاط ضعف معماری (Architectural Weaknesses)

کتابخانه OpenIddict عمداً بر روی پروتکل‌های اعطای مجوز تمرکز دارد.

این کتابخانه موارد زیر را ارائه نمی‌دهد:

- مدیریت کاربر؛
- خط‌مشی‌های رمز عبور؛
- چرخه حیات حساب کاربری؛
- مدیریت نقش‌ها.

این مسئولیت‌ها درون ASP.NET Core Identity باقی می‌مانند.

پیچیدگی استقرار نسبت به یک میان‌افزار ساده JWT بیشتر است زیرا یک سرور مجوزدهی (Authorization Server) باید پیکربندی شود.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

کتابخانه OpenIddict موارد زیر را فراهم می‌سازد:

- اندپوینت‌های اعطای مجوز (authorization endpoints)؛
- اندپوینت‌های توکن (token endpoints)؛
- اندپوینت‌های کشف و شناسایی (discovery endpoints)؛
- زیرساخت امضا (signing infrastructure)؛
- ثبت کلاینت‌ها (client registration)؛
- چرخه حیات توکن‌های نوسازی (refresh token lifecycle).

پیچیدگی عملیاتی متوسط ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

کتابخانه OpenIddict برای موارد زیر به خوبی مقیاس می‌پذیرد:

- مونولیت‌های ماژولار؛
- APIهای سازمانی؛
- سرویس‌های توزیع‌شده؛
- استقرارهای کوبرنتیز.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# امنیت (Security)

کتابخانه OpenIddict از استانداردهای مدرن احراز هویت شامل موارد زیر پشتیبانی می‌کند:

- OAuth 2.1؛
- OpenID Connect؛
- PKCE؛
- توکن‌های امضاشده JWT؛
- چرخش توکن‌های نوسازی (refresh token rotation)؛
- اعتبارسنجی امن توکن‌ها.

امنیت عالی ارزیابی می‌شود.

---

# انطباق با استانداردها (Standards Compliance)

کتابخانه OpenIddict از استانداردهای مدرن احراز هویت پیروی می‌کند.

مشخصات پشتیبانی‌شده شامل موارد زیر است:

- OAuth 2.x
- OpenID Connect
- PKCE
- JWT
- جریان‌های توکن منطبق با RFC

انطباق با استانداردها عالی ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده شامل موارد زیر است:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

انعطاف‌پذیری استقرار عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

کامپوننت‌های هوش مصنوعی مکرراً APIهای محافظت‌شده را فراخوانی می‌کنند.

کتابخانه OpenIddict موارد زیر را فراهم می‌آورد:

- احراز هویت سرویس‌ها؛
- اعطای مجوز تفویض‌شده (delegated authorization)؛
- دسترسی امن به APIهای هوش مصنوعی؛
- احراز هویت ماشین به ماشین (machine-to-machine authentication).

این ویژگی به خوبی با توسعه آتی هوش مصنوعی همخوانی دارد.

---

# قابلیت نگهداری (Maintainability)

مزایا شامل موارد زیر است:

- جامعه کاربری فعال؛
- معماری مدرن؛
- مستندات قوی؛
- یکپارچگی بومی با اکوسیستم .NET.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# ارتباط با ASP.NET Core Identity (Relationship with ASP.NET Core Identity)

مسئولیت‌ها کاملاً تفکیک شده باقی می‌مانند.

```text
Identity

        │

User Authentication

        │

        ▼

ASP.NET Core Identity

        │

Authenticated Principal

        │

        ▼

OpenIddict

        │

Token Issuance
```

فریم‌ورک Identity کاربران را مدیریت می‌کند.

کتابخانه OpenIddict اعطای مجوز را مدیریت می‌نماید.

---

# مقایسه با میان‌افزار JWT (Comparison with JWT Middleware)

| قابلیت | JWT Middleware | OpenIddict |
|------------|----------------|------------|
| اعتبارسنجی توکن (Token Validation) | عالی (Excellent) | عالی (Excellent) |
| صدور توکن (Token Issuance) | خیر (No) | عالی (Excellent) |
| OAuth2 | خیر (No) | عالی (Excellent) |
| OpenID Connect | خیر (No) | عالی (Excellent) |
| توکن‌های نوسازی (Refresh Tokens) | محدود (Limited) | عالی (Excellent) |
| سرور مجوزدهی (Authorization Server) | خیر (No) | عالی (Excellent) |

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| انطباق با استانداردها (Standards Compliance) | عالی (Excellent) |
| امنیت سازمانی (Enterprise Security) | عالی (Excellent) |
| قابلیت توسعه‌پذیری (Extensibility) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

کتابخانه OpenIddict قوی‌ترین انتخاب برای پیاده‌سازی سرور مجوزدهی OAuth 2.1 / OpenID Connect در MachineryManagerEnterprise است.

این کتابخانه به جای جایگزینی ASP.NET Core Identity، مکمل آن است.

فریم‌ورک Identity مسئولیت مدیریت کاربران را بر عهده دارد، در حالی که OpenIddict پروتکل‌های احراز هویت و اعطای مجوز منطبق با استانداردها را فراهم می‌آورد.

---

# 7. ارزیابی Duende IdentityServer (Duende IdentityServer Evaluation)

## نمای کلی (Overview)

محصول Duende IdentityServer جانشین تجاری IdentityServer4 است و یکی از بالغ‌ترین پیاده‌سازی‌های سرور مجوزدهی OAuth 2.1 / OpenID Connect برای .NET محسوب می‌شود.

این محصول قابلیت‌های فدراسیون هویت در سطح سازمانی را فراهم می‌سازد و به طور گسترده در سیستم‌های توزیع‌شده بزرگ به کار گرفته می‌شود.

برخلاف OpenIddict، محصول Duende یک **محصول تجاری** است که برای اکثر سناریوهای تولیدی نیازمند لایسنس است.

در پلتفرم MachineryManagerEnterprise، این محصول به عنوان یک سرور مجوزدهی سازمانی بالقوه ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

محصول Duende لایه سرور مجوزدهی (Authorization Server) را اشغال می‌کند.

```text
Presentation Layer

        │

Authentication

        │

        ▼

Identity Management

        │

        ▼

Duende IdentityServer

        │

        ▼

OAuth2 / OpenID Connect

        │

        ▼

Access Tokens
```

داده‌های هویت معمولاً توسط ASP.NET Core Identity یا منبع هویت دیگری تامین می‌شود.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- فوق‌العاده بالغ و باسابقه.
- مجموعه ویژگی‌های سازمانی.
- پشتیبانی از OAuth 2.1.
- پشتیبانی از OpenID Connect.
- بازرسی توکن (Token introspection).
- مدیریت پویای کلاینت‌ها (Dynamic client management).
- پشتیبانی از فدراسیون هویت.
- پیکربندی دقیق و منعطف.
- مستندات عالی.
- پذیرش گسترده در سازمان‌های بزرگ.

---

# نقاط ضعف معماری (Architectural Weaknesses)

چندین عامل تناسب آن را برای MachineryManagerEnterprise کاهش می‌دهد.

### لایسنس تجاری (Commercial Licensing)

محصول Duende برای اکثر استقرارهای سازمانی نیازمند لایسنس تجاری است.

این امر موارد زیر را تحمیل می‌کند:

- هزینه دوره‌ای لایسنس؛
- وابستگی در تدارکات و خرید؛
- بررسی‌های حقوقی و قانونی؛
- وابستگی به تامین‌کننده (vendor dependency).

### پیچیدگی معماری (Architecture Complexity)

محصول Duende زیرساخت‌های هویتی بسیار پیشرفته را هدف قرار داده است.

بسیاری از قابلیت‌های آن فراتر از نیازهای MachineryManagerEnterprise است.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

محصول Duende از موارد زیر پشتیبانی می‌کند:

- سرور مجوزدهی؛
- فدراسیون هویت؛
- ثبت کلاینت‌ها؛
- اندپوینت‌های شناسایی؛
- صدور توکن؛
- اعتبارسنجی توکن؛
- بازرسی توکن (introspection)؛
- جریان Device Flow.

پیچیدگی عملیاتی متوسط تا بالا ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

محصول Duende به خوبی در موارد زیر مقیاس می‌پذیرد:

- استقرارهای سازمانی؛
- سرویس‌های توزیع‌شده؛
- کوبرنتیز؛
- محیط‌های ابری.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# امنیت (Security)

قابلیت‌های امنیتی شامل موارد زیر است:

- OAuth 2.1؛
- OpenID Connect؛
- PKCE؛
- چرخش توکن‌های نوسازی؛
- مدیریت کلیدهای امضا؛
- صدور توکن منطبق با استانداردها.

امنیت عالی ارزیابی می‌شود.

---

# انطباق با استانداردها (Standards Compliance)

استانداردهای پشتیبانی‌شده شامل موارد زیر است:

- OAuth 2.x
- OpenID Connect
- PKCE
- JWT
- جریان‌های اعطای مجوز منطبق با RFC

انطباق با استانداردها عالی است.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

انعطاف‌پذیری استقرار عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

محصول Duende از احراز هویت امن برای موارد زیر پشتیبانی می‌کند:

- APIهای هوش مصنوعی؛
- ارتباطات ماشین به ماشین؛
- ارائه‌دهندگان خارجی هوش مصنوعی؛
- اعطای مجوز تفویض‌شده.

سازگاری عالی است.

---

# قابلیت نگهداری (Maintainability)

مزایا شامل موارد زیر است:

- مستندات عالی؛
- معماری بالغ؛
- رفتار قابل پیش‌بینی.

با این حال، لایسنس یک دغدغه نگهداری اضافی برای مالکیت بلندمدت به همراه می‌آورد.

قابلیت نگهداری بسیار خوب ارزیابی می‌شود.

---

# ملاحظات لایسنس (Licensing Considerations)

| جنبه | ارزیابی |
|---------|------------|
| هزینه لایسنس (Licensing Cost) | بالا (High) |
| وابستگی به تامین‌کننده (Vendor Dependency) | متوسط (Moderate) |
| در دسترس بودن متن‌باز (Open Source Availability) | خیر (No) |
| نسخه جامعه کاربری (Community Edition) | محدود (Limited) |

برای سیستم‌های سازمانی با طول عمر بالا، لایسنس به یک ملاحظه استراتژیک در معماری تبدیل می‌شود.

---

# مقایسه با OpenIddict (Comparison with OpenIddict)

| قابلیت | OpenIddict | Duende |
|------------|------------|---------|
| OAuth2 | عالی (Excellent) | عالی (Excellent) |
| OpenID Connect | عالی (Excellent) | عالی (Excellent) |
| ویژگی‌های سازمانی | بسیار خوب (Very Good) | عالی (Excellent) |
| لایسنس (Licensing) | متن‌باز (Open Source) | تجاری (Commercial) |
| یکپارچگی با .NET | عالی (Excellent) | عالی (Excellent) |
| پیچیدگی عملیاتی | متوسط (Moderate) | بالاتر (Higher) |

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| امنیت سازمانی (Enterprise Security) | عالی (Excellent) |
| انطباق با استانداردها (Standards Compliance) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | عالی (Excellent) |
| بهره‌وری هزینه (Cost Efficiency) | متوسط (Moderate) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

محصول Duende IdentityServer یک سرور مجوزدهی سازمانی برجسته است.

با این حال، پلتفرم MachineryManagerEnterprise موارد زیر را در اولویت قرار می‌دهد:

- استقلال از ارائه‌دهنده و تامین‌کننده؛
- بهره‌وری هزینه؛
- اکوسیستم متن‌باز؛
- قابلیت نگهداری.

از آنجا که OpenIddict کارایی و عملکردهای مورد نیاز را بدون تحمیل لایسنس تجاری فراهم می‌آورد، Duende مزیت معماری کافی برای توجیه تبدیل شدن به پلتفرم هویت اصلی را ارائه نمی‌دهد.

محصول Duende می‌تواند به عنوان یک گزینه جایگزین پشتیبانی‌شده برای سازمان‌هایی که از قبل بر روی آن استانداردسازی کرده‌اند باقی بماند، اما نباید به عنوان گزینه پیش‌فرض و پیشنهادی باشد.

---

# 8. ارزیابی توکن وب جیسون (JSON Web Token (JWT) Evaluation)

## نمای کلی (Overview)

توکن وب جیسون (JWT) که توسط استاندارد RFC 7519 تعریف شده است، فرمت استاندارد بالفعل (de facto) برای توکن‌های دسترسی بدون‌حالت (stateless access tokens) در سیستم‌های توزیع‌شده مدرن است.

یک JWT حاوی ادعاهایی است که به صورت دیجیتالی امضا شده‌اند و می‌توانند توسط سرورهای منبع (resource servers) بدون نیاز به دسترسی به وضعیت نشست متمرکز (centralized session state) اعتبارسنجی شوند.

در پلتفرم MachineryManagerEnterprise، توکن JWT به عنوان فرمت اصلی توکن دسترسی ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

توکن JWT به لایه Token تعلق دارد.

```text
User

      │

      ▼

Authentication

      │

      ▼

Authorization Server

      │

      ▼

JWT Access Token

      │

      ▼

Protected API
```

توکن نشان‌دهنده هویت احراز هویت‌شده و ادعاهای اعطای مجوز است.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- استاندارد صنعتی.
- بدون‌حالت (Stateless).
- کارایی و عملکرد بالا.
- خودکفا و مستقل (Self-contained).
- تعامل‌پذیری عالی (interoperability).
- پشتیبانی بومی در .NET.
- بی‌طرف نسبت به ابر.
- مناسب برای میکروسرویس‌ها.
- مناسب برای APIها.
- پشتیبانی عالی در اکوسیستم.

---

# نقاط ضعف معماری (Architectural Weaknesses)

از آنجا که JWT خودکفا و مستقل است، توکن‌های صادرشده تا زمان انقضا معتبر باقی می‌مانند.

در نتیجه:

- ابطال فوری (immediate revocation) دشوار است؛
- دسترسی‌ها و مجوزها برای توکن‌های از قبل صادرشده قابل تغییر نیست؛
- بازه‌های انقضای بسیار طولانی ریسک امنیتی را افزایش می‌دهند.

این محدودیت‌ها معمولاً با روش‌های زیر تعدیل می‌شوند:

- توکن‌های دسترسی کوتاه‌مدت (short-lived access tokens)؛
- توکن‌های نوسازی (refresh tokens)؛
- چرخش کلیدها (key rotation).

---

# ویژگی‌های عملیاتی (Operational Characteristics)

توکن JWT موارد زیر را فراهم می‌سازد:

- هویت احراز هویت‌شده؛
- انتقال ادعاها؛
- اعتبارسنجی بدون‌حالت؛
- یکپارچگی رمزنگاری‌شده (cryptographic integrity).

پیچیدگی عملیاتی بسیار پایین است.

---

# مقیاس‌پذیری (Scalability)

توکن JWT فوق‌العاده مقیاس‌پذیر است.

سرورهای منبع توکن‌ها را به صورت محلی اعتبارسنجی می‌کنند.

هیچ ذخیره‌ساز متمرکز نشست مورد نیاز نیست.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

توکن JWT در طول اعتبارسنجی هیچ وابستگی زمان اجرا به یک سرور احراز هویت ایجاد نمی‌کند.

این امر در دسترس بودن APIهای محافظت‌شده را به طور چشمگیری بهبود می‌بخشد.

قابلیت اطمینان عالی ارزیابی می‌شود.

---

# امنیت (Security)

امنیت به پیاده‌سازی صحیح بستگی دارد.

شیوه‌های توصیه‌شده شامل موارد زیر است:

- توکن‌های امضاشده؛
- انقضای کوتاه‌مدت؛
- صرفاً از طریق HTTPS؛
- چرخش توکن نوسازی؛
- کلیدهای امضای قوی.

امنیت در صورت پیاده‌سازی صحیح، عالی ارزیابی می‌شود.

---

# انطباق با استانداردها (Standards Compliance)

توکن JWT با استانداردهای زیر منطبق است:

- RFC 7519
- OAuth 2.x
- OpenID Connect

انطباق با استانداردها عالی است.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

انعطاف‌پذیری استقرار عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

توکن JWT برای ایمن‌سازی اندپوینت‌های هوش مصنوعی ایده‌آل است.

نمونه‌ها شامل موارد زیر است:

- APIهای دستیار هوش مصنوعی؛
- سرویس‌های تعبیه‌سازی (embedding services)؛
- جستجوی معنایی (semantic search)؛
- اندپوینت‌های استنتاج (inference endpoints)؛
- ارتباطات هوش مصنوعی ماشین به ماشین.

سازگاری با هوش مصنوعی عالی ارزیابی می‌شود.

---

# قابلیت نگهداری (Maintainability)

توکن JWT دارای ویژگی‌های زیر است:

- بالغ و باسابقه؛
- دارای مستندات کامل؛
- دارای پشتیبانی گسترده.

قابلیت نگهداری عالی است.

---

# کاربرد معمول (Typical Usage)

سناریوهای مناسب:

```text
REST APIs

Desktop Clients

Mobile Clients

Microservices

Machine-to-Machine Authentication

AI Services
```

سناریوهای نامناسب:

```text
Immediate Session Revocation

Very Long Sessions

Stateful Web Sessions
```

---

# ارتباط با OpenIddict (Relationship with OpenIddict)

```text
OpenIddict

      │

Issues

      ▼

JWT

      │

Validated by

      ▼

Protected APIs
```

کتابخانه OpenIddict توکن‌های JWT را صادر می‌کند.

برنامه‌ها توکن‌های JWT را اعتبارسنجی می‌کنند.

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| کارایی و عملکرد (Performance) | عالی (Excellent) |
| مقیاس‌پذیری (Scalability) | عالی (Excellent) |
| انطباق با استانداردها (Standards Compliance) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| سازگاری با هوش مصنوعی (AI Compatibility) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

توکن JWT باید به فرمت استاندارد توکن دسترسی برای MachineryManagerEnterprise تبدیل شود.

معماری بدون‌حالت، پذیرش در صنعت و مقیاس‌پذیری عالی آن، JWT را به قوی‌ترین انتخاب برای ایمن‌سازی موارد زیر تبدیل می‌کند:

- APIها؛
- کلاینت‌های دسکتاپ؛
- سرویس‌های هوش مصنوعی؛
- کامپوننت‌های توزیع‌شده آتی.

پیاده‌سازی توصیه‌شده موارد زیر را ترکیب خواهد کرد:

- توکن‌های دسترسی JWT کوتاه‌مدت؛
- چرخش توکن‌های نوسازی (refresh token rotation)؛
- مدیریت امن کلیدهای امضا.

---

# 9. ارزیابی توکن مرجع (Reference Token Evaluation)

## نمای کلی (Overview)

توکن‌های مرجع (Reference Tokens) شناسه‌های کدر و مبهمی (opaque identifiers) هستند که نشان‌دهنده یک نشست احراز هویت‌شده می‌باشند، به جای آنکه ادعاهای کاربر را مستقیماً حمل کنند.

برخلاف JWTها، یک توکن مرجع شامل **هیچ اطلاعات خوانایی** نیست.

هر درخواست API نیازمند بازرسی توکن (token introspection) در برابر سرور مجوزدهی است تا اطلاعات هویت و مجوزها را بازیابی نماید.

توکن‌های مرجع معمولاً در محیط‌های با حساسیت امنیتی بسیار بالا به کار می‌روند که در آن‌ها ابطال فوری توکن یک نیازمندی اصلی است.

---

# نقش معماری (Architectural Role)

توکن‌های مرجع به لایه Token تعلق دارند.

```text
User

      │

      ▼

Authentication

      │

      ▼

Authorization Server

      │

Issues

      ▼

Reference Token

      │

Introspection

      ▼

Authorization Server

      │

Returns Claims

      ▼

Protected API
```

برخلاف JWT، لایه API نمی‌تواند توکن را به طور مستقل اعتبارسنجی کند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- ابطال فوری (Immediate revocation).
- اعطای مجوز متمرکز.
- عدم وجود داده‌های حساس درون توکن.
- حجم کوچکتر توکن.
- کنترل امنیتی عالی.
- پشتیبانی از تغییرات پویای مجوزها.
- مناسب برای محیط‌های اعتماد صفر (zero-trust).

---

# نقاط ضعف معماری (Architectural Weaknesses)

توکن‌های مرجع وابستگی زیرساختی اضافی معرفی می‌کنند.

هر درخواست محافظت‌شده نیازمند موارد زیر است:

- ارتباطات شبکه‌ای؛
- بازرسی توکن (token introspection)؛
- در دسترس بودن سرور مجوزدهی.

در نتیجه:

- افزایش تاخیر (latency)؛
- بار زیرساختی بالاتر؛
- کاهش دسترس‌پذیری در طول قطعی‌های سرور مجوزدهی.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

توکن‌های مرجع نیازمند موارد زیر هستند:

- ذخیره‌ساز توکن (token store)؛
- اندپوینت بازرسی توکن (introspection endpoint)؛
- دسترس‌پذیری سرور مجوزدهی؛
- کش توکن (token cache - توصیه‌شده).

پیچیدگی عملیاتی بالا ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

مقیاس‌پذیری به سرور مجوزدهی بستگی دارد.

بدون کش‌سازی تهاجمی:

- هر درخواست API اقدام به بازرسی توکن می‌کند.

بنابراین، سیستم‌های توزیع‌شده بزرگ نیازمند لایه‌های کش‌سازی اضافی هستند.

مقیاس‌پذیری متوسط ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

از آنجا که APIها به دسترس‌پذیری سرور مجوزدهی وابسته هستند:

- قطعی‌های احراز هویت بر تمامی APIهای محافظت‌شده تاثیر می‌گذارد.

بنابراین قابلیت اطمینان پایین‌تر از JWT است.

---

# امنیت (Security)

توکن‌های مرجع چندین مزیت امنیتی مهم را فراهم می‌سازند:

- ابطال فوری؛
- اعطای مجوز متمرکز؛
- اعمال فوری تغییرات مجوزها؛
- توکن‌های سرقت‌شده هیچ ادعایی را افشا نمی‌کنند.

امنیت برجسته ارزیابی می‌شود.

---

# انطباق با استانداردها (Standards Compliance)

توکن‌های مرجع کاملاً با استانداردهای زیر سازگار هستند:

- OAuth 2.x
- OpenID Connect

آن‌ها به طور گسترده در سیستم‌های اعطای مجوز سازمانی استفاده می‌شوند.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده شامل موارد زیر است:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

انعطاف‌پذیری استقرار عالی است.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

توکن‌های مرجع می‌توانند اندپوینت‌های هوش مصنوعی را ایمن سازند.

با این حال، بارهای کاری هوش مصنوعی اغلب حجم بسیار بالایی از درخواست‌ها را ایجاد می‌کنند.

بازرسی مداوم توکن ممکن است تاخیر غیرضروری برای موارد زیر ایجاد کند:

- جستجوی معنایی؛
- تولید تعبیه (embedding generation)؛
- سرویس‌های استنتاج (inference services).

---

# قابلیت نگهداری (Maintainability)

توکن‌های مرجع نیازمند نگهداری موارد زیر هستند:

- ذخیره‌سازی توکن‌ها؛
- اندپوینت بازرسی توکن؛
- زیرساخت کش‌سازی؛
- سلامت سرور مجوزدهی.

قابلیت نگهداری متوسط ارزیابی می‌شود.

---

# مقایسه با JWT (Comparison with JWT)

| قابلیت | JWT | Reference Token |
|------------|-----|-----------------|
| بدون‌حالت (Stateless) | عالی (Excellent) | ضعیف (Poor) |
| ابطال فوری (Immediate Revocation) | ضعیف (Poor) | عالی (Excellent) |
| کارایی و عملکرد (Performance) | عالی (Excellent) | متوسط (Moderate) |
| وابستگی به زیرساخت (Infrastructure Dependency) | پایین (Low) | بالا (High) |
| مقیاس‌پذیری API | عالی (Excellent) | متوسط (Moderate) |
| امنیت (Security) | عالی (Excellent) | عالی (Excellent) |

---

# کاربرد معمول (Typical Usage)

سناریوهای مناسب:

```text
Highly Sensitive APIs

Financial Systems

Government Systems

Zero-Trust Architectures
```

سناریوهای کمتر مناسب:

```text
Public APIs

High-throughput Microservices

AI Inference

Large-scale Distributed APIs
```

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| امنیت (Security) | عالی (Excellent) |
| ابطال (Revocation) | عالی (Excellent) |
| کارایی و عملکرد (Performance) | متوسط (Moderate) |
| مقیاس‌پذیری (Scalability) | متوسط (Moderate) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | عالی (Excellent) |

---

# ارتباط با JWT (Relationship with JWT)

این دو فرمت توکن اولویت‌های متفاوتی را برطرف می‌کنند.

```text
High Performance

        │

       JWT

----------------------------

Immediate Revocation

        │

Reference Token
```

توکن JWT مقیاس‌پذیری را بهینه می‌کند.

توکن‌های مرجع کنترل متمرکز را بهینه می‌سازند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

توکن‌های مرجع قابلیت‌های امنیتی عالی و ابطال فوری را فراهم می‌آورند.

با این حال، پلتفرم MachineryManagerEnterprise موارد زیر را در اولویت قرار می‌دهد:

- کارایی و عملکرد بالا؛
- APIهای مقیاس‌پذیر؛
- یکپارچگی با هوش مصنوعی؛
- معماری توزیع‌شده.

این اولویت‌ها بیشتر با توکن‌های دسترسی JWT کوتاه‌مدت ترکیب‌شده با توکن‌های نوسازی همخوانی دارند.

بنابراین، توکن‌های مرجع باید به عنوان یک استراتژی اختیاری برای استقرارهای تخصصی با امنیت بسیار بالا باقی بمانند تا فرمت پیش‌فرض توکن دسترسی پلتفرم.

---

# 10. ارزیابی Microsoft Entra ID (Microsoft Entra ID Evaluation)

## نمای کلی (Overview)

پلتفرم Microsoft Entra ID (که قبلاً Azure Active Directory نامیده می‌شد) پلتفرم ارائه‌دهنده هویت سازمانی (IdP) و مدیریت هویت و دسترسی (IAM) مایکروسافت است.

این پلتفرم موارد زیر را فراهم می‌آورد:

- احراز هویت سازمانی؛
- ورود یکپارچه (SSO)؛
- OAuth 2.1؛
- OpenID Connect؛
- SAML 2.0؛
- دسترسی مشروط (Conditional Access)؛
- احراز هویت چندمرحله‌ای (MFA)؛
- سرویس‌های دایرکتوری سازمانی.

در MachineryManagerEnterprise این پلتفرم به عنوان یک ارائه‌دهنده هویت سازمانی خارجی ارزیابی می‌شود، نه سیستم هویت اصلی برنامه.

---

# نقش معماری (Architectural Role)

پلتفرم Microsoft Entra ID به لایه External Identity Provider تعلق دارد.

```text
Enterprise User

        │

        ▼

Microsoft Entra ID

        │

OpenID Connect

        │

        ▼

MachineryManagerEnterprise

        │

Authentication Abstraction

        ▼

Business Modules
```

برنامه به جای مدیریت مستقیم اطلاعات کاربری سازمانی، هویت‌های احراز هویت‌شده را مصرف می‌نماید.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- پلتفرم هویتی در سطح سازمانی.
- پشتیبانی بومی از OAuth 2.1.
- پشتیبانی از OpenID Connect.
- ورود یکپارچه (Single Sign-On).
- دسترسی مشروط (Conditional Access).
- احراز هویت چندمرحله‌ای (Multi-Factor Authentication).
- حاکمیت سازمانی (Enterprise governance).
- پذیرش گسترده در سازمان‌های بزرگ.
- یکپارچگی عالی با اکوسیستم مایکروسافت.
- زیرساخت ابری مدیریت‌شده.

---

# نقاط ضعف معماری (Architectural Weaknesses)

محدودیت اصلی وابستگی به پلتفرم است.

استفاده از Microsoft Entra ID موارد زیر را معرفی می‌کند:

- وابستگی به اکوسیستم Azure؛
- نیازمندی‌های مدیریت مستاجر (tenant administration)؛
- جفت‌شدگی با ارائه‌دهنده ابر؛
- ملاحظات لایسنس برای قابلیت‌های پیشرفته.

این ویژگی‌ها بی‌طرفی نسبت به ابر را کاهش می‌دهند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

پلتفرم Microsoft Entra ID موارد زیر را فراهم می‌سازد:

- احراز هویت سازمانی؛
- مدیریت متمرکز هویت؛
- SSO؛
- فدراسیون خارجی؛
- دایرکتوری سازمانی.

پیچیدگی عملیاتی برای برنامه پایین است زیرا زیرساخت هویت به صورت خارجی مدیریت می‌شود.

---

# مقیاس‌پذیری (Scalability)

پلتفرم Microsoft Entra ID برای استقرارهای سازمانی جهانی طراحی شده است.

این پلتفرم از موارد زیر پشتیبانی می‌کند:

- میلیون‌ها کاربر؛
- در دسترس بودن در سراسر جهان؛
- احراز هویت توزیع‌شده؛
- فدراسیون سازمانی.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

مزایای قابلیت اطمینان شامل موارد زیر است:

- زیرساخت توزیع‌شده جهانی مایکروسافت؛
- در دسترس بودن بالا؛
- سرویس‌های احراز هویت تاب‌آور.

قابلیت اطمینان عالی ارزیابی می‌شود.

---

# امنیت (Security)

قابلیت‌های امنیتی شامل موارد زیر است:

- MFA؛
- دسترسی مشروط (Conditional Access)؛
- حفاظت از هویت (Identity Protection)؛
- احراز هویت بدون رمز عبور (Passwordless)؛
- احراز هویت مبتنی بر ریسک؛
- یکپارچگی با انطباق دستگاه‌ها.

امنیت برجسته ارزیابی می‌شود.

---

# انطباق با استانداردها (Standards Compliance)

استانداردهای پشتیبانی‌شده شامل موارد زیر است:

- OAuth 2.x
- OpenID Connect
- SAML 2.0
- SCIM

انطباق با استانداردها عالی است.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده:

- Azure
- Microsoft 365
- Hybrid Enterprise

اگرچه پروتکل‌های مبتنی بر استاندارد اجازه یکپارچگی با برنامه‌های غیر از Azure را می‌دهند، اما خود زیرساخت هویت توسط مایکروسافت میزبانی می‌شود.

بنابراین انعطاف‌پذیری استقرار **متوسط** ارزیابی می‌شود.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

پلتفرم Microsoft Entra ID به طور طبیعی با موارد زیر یکپارچه می‌شود:

- Azure OpenAI؛
- اکوسیستم Microsoft Copilot؛
- سرویس‌های هوش مصنوعی Azure؛
- Microsoft Graph.

سازگاری با اکوسیستم هوش مصنوعی مایکروسافت عالی است.

---

# قابلیت نگهداری (Maintainability)

نگهداری زیرساخت هویت تا حد زیادی به مایکروسافت محول می‌شود.

مزایا شامل موارد زیر است:

- به‌روزرسانی‌های خودکار؛
- امنیت مدیریت‌شده؛
- ابزارهای مدیریت سازمانی.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# کاربرد معمول (Typical Usage)

سناریوهای مناسب:

```text
Corporate Employees

Enterprise SSO

Azure-based Organizations

Microsoft 365 Integration

Hybrid Enterprise Identity
```

سناریوهای کمتر مناسب:

```text
Standalone Commercial Software

Cloud-Neutral Products

Independent Customer Deployments
```

---

# مقایسه با هویت محلی (Comparison with Local Identity)

| قابلیت | ASP.NET Core Identity | Microsoft Entra ID |
|------------|----------------------|--------------------|
| مدیریت محلی کاربران | عالی (Excellent) | خیر (No) |
| ورود یکپارچه سازمانی (SSO) | محدود (Limited) | عالی (Excellent) |
| ارائه‌دهنده OAuth2 | از طریق OpenIddict | عالی (Excellent) |
| بی‌طرفی نسبت به ابر | عالی (Excellent) | متوسط (Moderate) |
| فدراسیون سازمانی | محدود (Limited) | عالی (Excellent) |
| سادگی لایسنس | عالی (Excellent) | متوسط (Moderate) |

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| امنیت سازمانی (Enterprise Security) | عالی (Excellent) |
| انطباق با استانداردها (Standards Compliance) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | متوسط (Moderate) |
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| بهره‌وری هزینه (Cost Efficiency) | خوب (Good) |

---

# ارتباط با OpenIddict (Relationship with OpenIddict)

کتابخانه OpenIddict و Microsoft Entra ID رقیب یکدیگر نیستند.

یکپارچگی معمول:

```text
Microsoft Entra ID

        │

External Login

        ▼

MachineryManagerEnterprise

        │

ASP.NET Core Identity

        │

OpenIddict

        ▼

Application JWT
```

این معماری به برنامه اجازه می‌دهد تا هویت‌های سازمانی را بپذیرد در حالی که مدل اعطای مجوز اختصاصی خود را حفظ می‌نماید.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

پلتفرم Microsoft Entra ID یک ارائه‌دهنده هویت سازمانی عالی برای سازمان‌هایی است که از قبل در اکوسیستم مایکروسافت سرمایه‌گذاری کرده‌اند.

با این حال، MachineryManagerEnterprise صراحتاً موارد زیر را هدف قرار می‌دهد:

- بی‌طرفی نسبت به ابر؛
- استقلال در استقرار؛
- استقلال از ارائه‌دهنده.

بنابراین Microsoft Entra ID باید به عنوان یک **ارائه‌دهنده احراز هویت خارجی اختیاری** در نظر گرفته شود، نه راهکار اصلی هویت پلتفرم.

معماری اصلی و پیشنهادی هویت همچنان بر پایه موارد زیر استوار است:

- ASP.NET Core Identity
- OpenIddict
- JWT

در حالی که Microsoft Entra ID به عنوان یک گزینه فدراسیون سازمانی در دسترس خواهد بود.

---

# 11. ارزیابی Google Identity (Google Identity Evaluation)

## نمای کلی (Overview)

سرویس Google Identity خدمات احراز هویت را برای کاربرانی که دارای حساب کاربری گوگل هستند فراهم می‌کند.

این سرویس از موارد زیر پشتیبانی می‌نماید:

- OpenID Connect؛
- OAuth 2.x؛
- ورود یکپارچه (Single Sign-On)؛
- ورود با شبکه‌های اجتماعی (Social Login)؛
- احراز هویت موبایل؛
- احراز هویت وب.

سرویس Google Identity به عنوان یک ارائه‌دهنده هویت خارجی اختیاری برای MachineryManagerEnterprise ارزیابی می‌شود.

هدف اصلی آن ساده‌سازی احراز هویت برای کاربران خارجی است نه هویت نیروی کار سازمانی.

---

# نقش معماری (Architectural Role)

سرویس Google Identity به لایه External Authentication Provider تعلق دارد.

```text
Google User

      │

      ▼

Google Identity

      │

OpenID Connect

      ▼

MachineryManagerEnterprise

      │

Authentication Abstraction

      ▼

Business Modules
```

منطق کسب‌وکار کاملاً مستقل از APIهای خاص گوگل باقی می‌ماند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- پروتکل‌های استاندارد صنعتی.
- پیاده‌سازی عالی OpenID Connect.
- پذیرش گسترده کاربران.
- آنبوردینگ ساده کاربران.
- اکوسیستم بالغ.
- پشتیبانی بومی از OAuth.
- سازگاری گسترده با پلتفرم‌ها.
- حداقل سربار عملیاتی.

---

# نقاط ضعف معماری (Architectural Weaknesses)

سرویس Google Identity برای احراز هویت مصرف‌کنندگان (consumer authentication) در نظر گرفته شده است.

محدودیت‌ها شامل موارد زیر است:

- فاقد دایرکتوری سازمانی؛
- فاقد حاکمیت سازمانی؛
- فاقد اعطای مجوز در سطح سازمان؛
- نامناسب برای مدیریت نیروی کار داخلی.

این سرویس به جای جایگزینی زیرساخت هویت اصلی پلتفرم، مکمل آن است.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

سرویس Google Identity موارد زیر را فراهم می‌سازد:

- احراز هویت؛
- تایید هویت؛
- ورود تفویض‌شده (delegated login).

برنامه همچنان مالک موارد زیر باقی می‌ماند:

- اعطای مجوز؛
- نقش‌ها؛
- دسترسی‌ها؛
- هویت کسب‌وکار.

پیچیدگی عملیاتی پایین است.

---

# مقیاس‌پذیری (Scalability)

سرویس Google Identity در سراسر جهان توزیع شده است.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

دسترس‌پذیری احراز هویت از زیرساخت جهانی گوگل بهره‌مند می‌شود.

قابلیت اطمینان عالی ارزیابی می‌شود.

---

# امنیت (Security)

قابلیت‌های امنیتی شامل موارد زیر است:

- پشتیبانی از MFA؛
- جریان‌های مدرن OAuth؛
- OpenID Connect؛
- PKCE؛
- امنیت حساب گوگل.

امنیت عالی ارزیابی می‌شود.

---

# انطباق با استانداردها (Standards Compliance)

استانداردهای پشتیبانی‌شده شامل موارد زیر است:

- OAuth 2.x
- OpenID Connect

انطباق با استانداردها عالی است.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

سرویس Google Identity مبتنی بر ابر میزبانی می‌شود.

برنامه‌ها قابل حمل باقی می‌مانند زیرا یکپارچگی بر پروتکل‌های استاندارد تکیه دارد.

انعطاف‌پذیری استقرار خوب ارزیابی می‌شود.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

سرویس Google Identity هیچ مزیت یکپارچگی خاصی با هوش مصنوعی ندارد.

این سرویس صرفاً کاربران را پیش از دسترسی به قابلیت‌های هوش مصنوعی احراز هویت می‌کند.

سازگاری خنثی و بی‌طرف ارزیابی می‌شود.

---

# قابلیت نگهداری (Maintainability)

تلاش برای نگهداری در حداقل مقدار است.

گوگل موارد زیر را مدیریت می‌کند:

- زیرساخت احراز هویت؛
- تکامل پروتکل‌ها؛
- به‌روزرسانی‌های امنیتی.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# کاربرد معمول (Typical Usage)

سناریوهای مناسب:

```text
Customer Login

Public Applications

Partner Access

Consumer Authentication
```

سناریوهای کمتر مناسب:

```text
Internal Enterprise Identity

Corporate Workforce

Organization-wide Authorization
```

---

# مقایسه با ASP.NET Core Identity (Comparison with ASP.NET Core Identity)

| قابلیت | ASP.NET Core Identity | Google Identity |
|------------|----------------------|-----------------|
| مدیریت محلی کاربران | عالی (Excellent) | خیر (No) |
| ورود مصرف‌کنندگان | خوب (Good) | عالی (Excellent) |
| هویت سازمانی | خوب (Good) | محدود (Limited) |
| احراز هویت خارجی | متوسط (Moderate) | عالی (Excellent) |
| اعطای مجوز (Authorization) | عالی (Excellent) | خیر (No) |

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| انطباق با استانداردها (Standards Compliance) | عالی (Excellent) |
| احراز هویت مصرف‌کنندگان | عالی (Excellent) |
| هویت سازمانی | متوسط (Moderate) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | خوب (Good) |

---

# ارتباط با ASP.NET Core Identity (Relationship with ASP.NET Core Identity)

سرویس Google Identity کاربر را احراز هویت می‌کند.

فریم‌ورک ASP.NET Core Identity مالک مدل اعطای مجوز برنامه است.

```text
Google Identity

      │

Authentication

      ▼

ASP.NET Core Identity

      │

Application User

      ▼

Authorization
```

این تفکیک، معماری تمیز را حفظ می‌کند و هم‌زمان از احراز هویت خارجی پشتیبانی می‌نماید.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

سرویس Google Identity یک ارائه‌دهنده احراز هویت اختیاری عالی برای سناریوهای رو به مشتری است.

این سرویس باید به عنوان یک ارائه‌دهنده ورود خارجی پشتیبانی شود.

با این حال، نباید جایگزین زیرساخت هویت اصلی پلتفرم مبتنی بر موارد زیر شود:

- ASP.NET Core Identity؛
- OpenIddict؛
- JWT.

---

# 12. ارزیابی GitHub Identity (GitHub Identity Evaluation)

## نمای کلی (Overview)

سرویس GitHub Identity احراز هویت مبتنی بر OAuth 2.0 و OpenID Connect را برای کاربرانی که دارای حساب کاربری گیت‌هاب هستند فراهم می‌سازد.

این سرویس در درجه اول برای موارد زیر در نظر گرفته شده است:

- توسعه‌دهندگان؛
- جوامع متن‌باز؛
- پلتفرم‌های DevOps؛
- پورتال‌های مهندسی.

در پلتفرم MachineryManagerEnterprise، سرویس GitHub Identity به عنوان یک ارائه‌دهنده احراز هویت خارجی اختیاری برای استقرارهای توسعه‌محور ارزیابی می‌شود.

این سرویس **قرار نیست** به یک ارائه‌دهنده هویت سازمانی اصلی تبدیل شود.

---

# نقش معماری (Architectural Role)

سرویس GitHub Identity به لایه External Authentication Provider تعلق دارد.

```text
GitHub User

      │

      ▼

GitHub Identity

      │

OAuth2 / OIDC

      ▼

MachineryManagerEnterprise

      │

Authentication Abstraction

      ▼

Business Modules
```

ماژول‌های کسب‌وکار کاملاً از احراز هویت خاص گیت‌هاب بی‌خبر و مستقل باقی می‌مانند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا

- یکپارچگی ساده با OAuth.
- پشتیبانی از OpenID Connect.
- پذیرش عالی میان توسعه‌دهندگان.
- APIهای بالغ.
- حداقل زیرساخت مورد نیاز.
- مفید برای محیط‌های مهندسی.
- مستقل از پلتفرم (Cross-platform).

---

# نقاط ضعف معماری (Architectural Weaknesses)

سرویس GitHub Identity برای هویت نیروی کار سازمانی طراحی نشده است.

محدودیت‌ها شامل موارد زیر است:

- فاقد دایرکتوری سازمانی؛
- فاقد حاکمیت هویت سازمانی؛
- فاقد مدل اعطای مجوز سازمانی؛
- نامناسب برای مدیریت کاربران تجاری.

کاربرد آن در درجه اول به برنامه‌های رو به توسعه‌دهندگان محدود می‌شود.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

سرویس GitHub Identity موارد زیر را فراهم می‌آورد:

- احراز هویت؛
- تایید هویت؛
- اعطای مجوز با OAuth.

برنامه همچنان مسئول موارد زیر باقی می‌ماند:

- اعطای مجوز؛
- نقش‌ها؛
- دسترسی‌ها؛
- چرخه حیات کاربر در برنامه.

پیچیدگی عملیاتی بسیار پایین است.

---

# مقیاس‌پذیری (Scalability)

سرویس GitHub Identity از زیرساخت جهانی گیت‌هاب بهره می‌برد.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

دسترس‌پذیری احراز هویت به طور کلی عالی است.

با این حال، دسترس‌پذیری برنامه در زمان ورود، به سرویس‌های احراز هویت گیت‌هاب وابسته است.

قابلیت اطمینان بسیار خوب ارزیابی می‌شود.

---

# امنیت (Security)

قابلیت‌های امنیتی شامل موارد زیر است:

- OAuth 2.x؛
- OpenID Connect؛
- PKCE؛
- حفاظت از حساب کاربری گیت‌هاب؛
- پشتیبانی از MFA.

امنیت عالی ارزیابی می‌شود.

---

# انطباق با استانداردها (Standards Compliance)

استانداردهای پشتیبانی‌شده شامل موارد زیر است:

- OAuth 2.x
- OpenID Connect

انطباق با استانداردها عالی است.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

سرویس GitHub Identity مبتنی بر ابر میزبانی می‌شود.

برنامه‌ها قابل حمل باقی می‌مانند زیرا یکپارچگی از پروتکل‌های استاندارد استفاده می‌کند.

انعطاف‌پذیری استقرار خوب ارزیابی می‌شود.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

سرویس GitHub Identity هیچ قابلیت خاصی در رابطه با هوش مصنوعی ارائه نمی‌دهد.

این سرویس صرفاً کاربران را پیش از دسترسی به سرویس‌های هوش مصنوعی احراز هویت می‌نماید.

سازگاری خنثی و بی‌طرف ارزیابی می‌شود.

---

# قابلیت نگهداری (Maintainability)

تلاش برای نگهداری در حداقل مقدار است.

گیت‌هاب موارد زیر را نگهداری و مدیریت می‌کند:

- زیرساخت احراز هویت؛
- پیاده‌سازی OAuth؛
- تکامل پروتکل‌ها.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# کاربرد معمول (Typical Usage)

سناریوهای مناسب:

```text
Developer Portals

Engineering Tools

Internal DevOps Systems

Open Source Communities
```

سناریوهای کمتر مناسب:

```text
Enterprise Workforce

Customer Identity

Corporate SSO
```

---

# مقایسه با Google Identity (Comparison with Google Identity)

| قابلیت | Google Identity | GitHub Identity |
|------------|----------------|-----------------|
| پذیرش مصرف‌کنندگان | عالی (Excellent) | خوب (Good) |
| پذیرش توسعه‌دهندگان | خوب (Good) | عالی (Excellent) |
| هویت سازمانی | متوسط (Moderate) | محدود (Limited) |
| پشتیبانی از OAuth | عالی (Excellent) | عالی (Excellent) |
| OpenID Connect | عالی (Excellent) | عالی (Excellent) |

---

# تناسب معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| انطباق با استانداردها (Standards Compliance) | عالی (Excellent) |
| احراز هویت توسعه‌دهندگان | عالی (Excellent) |
| هویت سازمانی | محدود (Limited) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | متوسط (Moderate) |

---

# ارتباط با ASP.NET Core Identity (Relationship with ASP.NET Core Identity)

سرویس GitHub Identity توسعه‌دهنده را احراز هویت می‌کند.

فریم‌ورک ASP.NET Core Identity مالک مدل اعطای مجوز برنامه است.

```text
GitHub Identity

      │

Authentication

      ▼

ASP.NET Core Identity

      │

Application User

      ▼

Authorization
```

این تفکیک، معماری تمیز را حفظ می‌کند در حالی که از احراز هویت خارجی توسعه‌دهندگان پشتیبانی می‌نماید.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

سرویس GitHub Identity یک ارائه‌دهنده احراز هویت اختیاری عالی برای سناریوهای توسعه‌محور است.

این سرویس تنها باید در مواردی پشتیبانی شود که احراز هویت مبتنی بر گیت‌هاب ارزش تجاری ایجاد کند.

این سرویس **نباید** جایگزین معماری اصلی احراز هویت مبتنی بر موارد زیر شود:

- ASP.NET Core Identity؛
- OpenIddict؛
- JWT.

---

# 13. مقایسه کلی فناوری‌ها (Overall Technology Comparison)

احراز هویت در MachineryManagerEnterprise شامل چندین مسئولیت مکمل است.

هیچ فناوری واحدی تمامی نیازمندی‌های معماری را برآورده نمی‌سازد.

معماری پیشنهادی موارد زیر را از یکدیگر تفکیک می‌نماید:

- مدیریت هویت (identity management)؛
- صدور توکن (token issuance)؛
- فرمت توکن دسترسی (access token format)؛
- ارائه‌دهندگان احراز هویت خارجی (external authentication providers).

---

# ماتریس مسئولیت‌ها (Responsibility Matrix)

| مسئولیت | فناوری پیشنهادی | گزینه جایگزین | هدف |
|----------------|------------------------|-------------|---------|
| مدیریت محلی کاربران | ASP.NET Core Identity | ذخیره‌ساز سفارشی هویت | حساب‌های کاربری |
| سرور OAuth2 / OIDC | OpenIddict | Duende IdentityServer | سرور مجوزدهی |
| فرمت توکن دسترسی | JWT | Reference Token | احراز هویت API |
| هویت خارجی سازمانی | Microsoft Entra ID | سایر IdPهای سازمانی | SSO شرکتی و سازمانی |
| احراز هویت مصرف‌کنندگان | Google Identity | GitHub Identity | ورود خارجی |
| احراز هویت توسعه‌دهندگان | GitHub Identity | Google Identity | ورود توسعه‌دهندگان |

---

# مقایسه قابلیت‌ها (Capability Comparison)

| قابلیت | ASP.NET Core Identity | OpenIddict | Duende | JWT | Reference Token | Entra ID | Google | GitHub |
|------------|----------------------|------------|---------|-----|-----------------|-----------|---------|---------|
| مدیریت کاربر | عالی (Excellent) | خیر (No) | خیر (No) | خیر (No) | خیر (No) | متوسط (Moderate) | خیر (No) | خیر (No) |
| OAuth2 / OIDC | محدود (Limited) | عالی (Excellent) | عالی (Excellent) | صرفاً فرمت | صرفاً فرمت | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| سرور مجوزدهی | خیر (No) | عالی (Excellent) | عالی (Excellent) | خیر (No) | خیر (No) | عالی (Excellent) | خیر (No) | خیر (No) |
| احراز هویت بدون‌حالت | خیر (No) | بله (Yes) | بله (Yes) | عالی (Excellent) | ضعیف (Poor) | بله (Yes) | بله (Yes) | بله (Yes) |
| ابطال فوری | متوسط (Moderate) | عالی (Excellent) | عالی (Excellent) | ضعیف (Poor) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | خوب (Good) |
| ورود یکپارچه سازمانی | محدود (Limited) | خوب (Good) | عالی (Excellent) | خیر (No) | خیر (No) | عالی (Excellent) | محدود (Limited) | محدود (Limited) |
| ورود مصرف‌کنندگان | متوسط (Moderate) | متوسط (Moderate) | متوسط (Moderate) | خیر (No) | خیر (No) | متوسط (Moderate) | عالی (Excellent) | خوب (Good) |
| ورود توسعه‌دهندگان | متوسط (Moderate) | متوسط (Moderate) | متوسط (Moderate) | خیر (No) | خیر (No) | متوسط (Moderate) | خوب (Good) | عالی (Excellent) |
| لایسنس (Licensing) | متن‌باز (Open Source) | متن‌باز (Open Source) | تجاری (Commercial) | استاندارد باز | استاندارد باز | لایسنس مایکروسافت | رایگان | رایگان |
| بی‌طرفی نسبت به ابر | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | متوسط (Moderate) | خوب (Good) | خوب (Good) |

---

# ارزیابی بی‌طرفی نسبت به ابر (Cloud Neutrality Assessment)

| فناوری | بی‌طرفی نسبت به ابر |
|------------|-----------------|
| ASP.NET Core Identity | عالی (Excellent) |
| OpenIddict | عالی (Excellent) |
| JWT | عالی (Excellent) |
| Reference Tokens | عالی (Excellent) |
| Duende IdentityServer | عالی (Excellent) |
| Microsoft Entra ID | متوسط (Moderate) |
| Google Identity | خوب (Good) |
| GitHub Identity | خوب (Good) |

---

# تناسب سازمانی (Enterprise Suitability)

| فناوری | آمادگی سازمانی |
|------------|---------------------|
| ASP.NET Core Identity | عالی (Excellent) |
| OpenIddict | عالی (Excellent) |
| JWT | عالی (Excellent) |
| Reference Tokens | عالی (Excellent) |
| Duende IdentityServer | عالی (Excellent) |
| Microsoft Entra ID | عالی (Excellent) |
| Google Identity | خوب (Good) |
| GitHub Identity | متوسط (Moderate) |

---

# سازگاری با هوش مصنوعی (AI Compatibility)

| فناوری | سازگاری با هوش مصنوعی |
|------------|------------------|
| ASP.NET Core Identity | عالی (Excellent) |
| OpenIddict | عالی (Excellent) |
| JWT | عالی (Excellent) |
| Reference Tokens | خوب (Good) |
| Microsoft Entra ID | عالی (اکوسیستم Azure) |
| Google Identity | خنثی (Neutral) |
| GitHub Identity | خنثی (Neutral) |

---

# انطباق با معماری تمیز (Clean Architecture Compliance)

معماری ارجح، تفکیک دقیق مسئولیت‌ها را حفظ می‌کند.

```text
                External Identity Providers

      ┌──────────────┬──────────────┬──────────────┐
      │              │              │
      ▼              ▼              ▼

 Microsoft Entra   Google      GitHub Identity

                 │

                 ▼

        ASP.NET Core Identity

                 │

                 ▼

             OpenIddict

                 │

                 ▼

          JWT Access Tokens

                 │

                 ▼

          Protected Resources

                 │

                 ▼

            Business Modules
```

این لایه‌بندی تضمین می‌نماید:

- استقلال از ارائه‌دهنده؛
- انطباق با استانداردها؛
- ایزولاسیون زیرساخت؛
- قابلیت توسعه‌پذیری در آینده.

---

# مقایسه هزینه‌ها (Cost Comparison)

| فناوری | هزینه |
|------------|------|
| ASP.NET Core Identity | رایگان |
| OpenIddict | رایگان |
| JWT | رایگان |
| Reference Tokens | رایگان |
| Microsoft Entra ID | وابسته به لایسنس |
| Google Identity | رایگان |
| GitHub Identity | رایگان |
| Duende IdentityServer | لایسنس تجاری |

---

# ارزیابی ریسک (Risk Assessment)

| فناوری | ریسک اصلی |
|------------|--------------|
| ASP.NET Core Identity | نگهداری هویت محلی |
| OpenIddict | پیکربندی سرور مجوزدهی |
| JWT | استراتژی ابطال توکن |
| Reference Tokens | سربار کارایی و عملکرد |
| Microsoft Entra ID | وابستگی به تامین‌کننده |
| Google Identity | هویت صرفاً مصرف‌کننده |
| GitHub Identity | هویت صرفاً توسعه‌دهنده |
| Duende IdentityServer | لایسنس تجاری |

---

# ارزیابی نهایی (Overall Evaluation)

| معیار | انتخاب پیشنهادی |
|-----------|--------------------|
| هویت محلی (Local Identity) | ASP.NET Core Identity |
| سرور مجوزدهی (Authorization Server) | OpenIddict |
| توکن‌های دسترسی (Access Tokens) | JWT |
| فدراسیون سازمانی (Enterprise Federation) | Microsoft Entra ID (اختیاری) |
| ورود مصرف‌کنندگان (Consumer Login) | Google Identity (اختیاری) |
| ورود توسعه‌دهندگان (Developer Login) | GitHub Identity (اختیاری) |

فناوری‌ها به جای رقابت با یکدیگر، مکمل یکدیگرند. آن‌ها در کنار هم یک معماری احراز هویت کامل و مبتنی بر استانداردها را تشکیل می‌دهند که با اهداف MachineryManagerEnterprise شامل معماری تمیز، بی‌طرفی نسبت به ابر، قابلیت نگهداری و مقیاس‌پذیری در آینده کاملاً همسو است.

---

# 14. توصیه نهایی (Final Recommendation)

پس از ارزیابی تمامی فناوری‌های کاندید، معماری احراز هویت زیر برای MachineryManagerEnterprise پیشنهاد می‌شود.

## پشته احراز هویت اصلی (Core Authentication Stack)

| مسئولیت | فناوری انتخاب‌شده | دلیل منطقی |
|----------------|---------------------|-----------|
| مدیریت هویت محلی | ASP.NET Core Identity | بالغ، امن، کاملاً یکپارچه با .NET 10 |
| سرور مجوزدهی | OpenIddict | متن‌باز، منطبق بر استانداردها، بی‌طرف نسبت به ابر |
| فرمت توکن دسترسی | JWT | کارایی بالا، بدون‌حالت، مقیاس‌پذیر |
| توکن‌های نوسازی | OpenIddict | مدیریت امن چرخه حیات توکن |
| مدل اعطای مجوز | خط‌مشی + ادعاها + نقش‌ها | اعطای مجوز منعطف سازمانی |

---

## ارائه‌دهندگان هویت خارجی اختیاری (Optional External Identity Providers)

ارائه‌دهندگان زیر باید به عنوان منابع احراز هویت اختیاری پشتیبانی شوند.

| ارائه‌دهنده | هدف | وضعیت |
|----------|---------|--------|
| Microsoft Entra ID | ورود یکپارچه سازمانی (SSO) | اختیاری (Optional) |
| Google Identity | احراز هویت مشتریان | اختیاری (Optional) |
| GitHub Identity | احراز هویت توسعه‌دهندگان | اختیاری (Optional) |

این ارائه‌دهندگان کاربران را احراز هویت می‌کنند اما **جایگزین** مدل اعطای مجوز داخلی برنامه نمی‌شوند.

---

# معماری احراز هویت پیشنهادی (Recommended Authentication Architecture)

```text
                External Identity Providers

        ┌────────────┬────────────┬────────────┐
        │            │            │
        ▼            ▼            ▼

 Microsoft Entra   Google      GitHub

                  │

                  ▼

        ASP.NET Core Identity

                  │

                  ▼

              OpenIddict

                  │

                  ▼

      JWT Access / Refresh Tokens

                  │

                  ▼

          Authorization Middleware

                  │

                  ▼

            Business Modules
```

---

# استراتژی پیشنهادی اعطای مجوز (Recommended Authorization Strategy)

احراز هویت (Authentication) تعیین می‌کند **چه کسی** فراخواننده است.

اعطای مجوز (Authorization) تعیین می‌کند فراخواننده مجاز به انجام **چه کاری** است.

پلتفرم باید اعطای مجوز را با استفاده از موارد زیر پیاده‌سازی کند:

- ادعاها (Claims)
- نقش‌ها (Roles)
- خط‌مشی‌ها (Policies)
- اعطای مجوز مبتنی بر منبع (Resource-based Authorization - در صورت نیاز)

منطق کسب‌وکار **هرگز** نباید مستقیماً توکن‌های JWT را بازرسی و بررسی کند.

در عوض، سرویس‌های کسب‌وکار صرفاً `ClaimsPrincipal` احراز هویت‌شده را مصرف می‌کنند که توسط انتزاع احراز هویت در دسترس قرار می‌گیرد.

---

# استراتژی توکن (Token Strategy)

| نوع توکن | طول عمر | هدف |
|------------|----------|---------|
| توکن دسترسی (JWT) | کوتاه‌مدت (مثلاً ۱۰ الی ۳۰ دقیقه) | اعطای مجوز API |
| توکن نوسازی (Refresh Token) | بلندمدت | تداوم نشست |
| توکن مرجع (Reference Token) | به صورت پیش‌فرض استفاده نمی‌شود | رزروشده برای استقرارهای تخصصی |

این رویکرد موارد زیر را فراهم می‌سازد:

- مقیاس‌پذیری عالی؛
- حداقل وابستگی به زیرساخت؛
- امنیت قوی؛
- انطباق با استانداردها.

---

# توصیه‌های امنیتی (Security Recommendations)

شیوه‌های امنیتی زیر الزامی و اجباری هستند:

- استفاده از HTTPS در همه جا؛
- توکن‌های امضاشده JWT؛
- توکن‌های دسترسی کوتاه‌مدت؛
- چرخش توکن‌های نوسازی (refresh token rotation)؛
- ذخیره‌سازی امن کلیدهای امضا؛
- چرخش دوره‌ای کلیدهای امضا؛
- اعطای مجوز مبتنی بر خط‌مشی (policy-based authorization)؛
- اصل حداقل دسترسی (least-privilege principle)؛
- پشتیبانی از MFA (در صورت نیاز)؛
- ایزولاسیون ارائه‌دهندگان خارجی.

---

# بی‌طرفی نسبت به ابر (Cloud Neutrality)

معماری پیشنهادی عمداً از وابستگی و قفل شدن به ابر (cloud lock-in) جلوگیری می‌کند.

کامپوننت‌های اصلی به صورت زیر باقی می‌مانند:

- متن‌باز؛
- مستقل از ارائه‌دهنده؛
- قابل استقرار در محیط‌های محلی (on-premises)؛
- قابل استقرار در هر ارائه‌دهنده ابری؛
- سازگار با کوبرنتیز.

ارائه‌دهندگان خاص ابری (نظیر Microsoft Entra ID) به عنوان یکپارچگی‌های اختیاری در نظر گرفته می‌شوند.

---

# آمادگی برای هوش مصنوعی (AI Readiness)

معماری انتخاب‌شده کاملاً از قابلیت‌های آتی هوش مصنوعی پشتیبانی می‌نماید.

نمونه‌ها عبارتند از:

- دستیاران هوش مصنوعی احراز هویت‌شده؛
- سرویس‌های امن تعبیه‌سازی (embedding)؛
- APIهای محافظت‌شده جستجوی معنایی؛
- ارتباطات هوش مصنوعی ماشین به ماشین؛
- پردازش‌های پس‌زمینه هوش مصنوعی.

احراز هویت مبتنی بر JWT تاخیر را برای بارهای کاری هوش مصنوعی به حداقل می‌رساند در حالی که منطبق بر استانداردها باقی می‌ماند.

---

# تصمیم نهایی (Final Decision)

شورای بازبینی معماری (Architecture Review Board) پشته احراز هویت زیر را به عنوان معماری استاندارد پلتفرم تصویب می‌نماید.

| کامپوننت | تصمیم |
|----------|----------|
| ASP.NET Core Identity | تصویب شد (Approved) |
| OpenIddict | تصویب شد (Approved) |
| JWT Access Tokens | تصویب شد (Approved) |
| Refresh Tokens | تصویب شد (Approved) |
| Microsoft Entra ID | اختیاری (Optional) |
| Google Identity | اختیاری (Optional) |
| GitHub Identity | اختیاری (Optional) |
| Duende IdentityServer | رد شد (لایسنس تجاری توجیه‌پذیر نیست) |
| Reference Tokens | به عنوان پیش‌فرض انتخاب نشد |

---

# خلاصه تصمیم (Decision Summary)

راهکار انتخاب‌شده تمامی اهداف معماری را برآورده می‌سازد:

- ✔ معماری تمیز (Clean Architecture)
- ✔ امنیت سازمانی (Enterprise Security)
- ✔ سازگاری با .NET 10
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ انطباق با استانداردها (Standards Compliance)
- ✔ مقیاس‌پذیری بالا (High Scalability)
- ✔ پیچیدگی عملیاتی پایین (Low Operational Complexity)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت (Long-term Maintainability)

بنابراین، این معماری احراز هویت به عنوان استاندارد سازمانی برای MachineryManagerEnterprise اتخاذ می‌شود.

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

# تاریخچه بازبینی (Revision History)

| نسخه | تاریخ | نویسنده | شرح |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0 | 2026-07-26 | معمار راهکار (Solution Architect) | ارزیابی اولیه فناوری برای احراز هویت و هویت |
| 1.3.0 | 2026-07-28 | معمار راهکار (Solution Architect) | افزودن بخش جدید (محدوده ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار (Solution Architect) | ارتقاء به استاندارد مستندسازی نسخه 4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار (Solution Architect) | بازبینی و همگام‌سازی با آخرین تغییرات |