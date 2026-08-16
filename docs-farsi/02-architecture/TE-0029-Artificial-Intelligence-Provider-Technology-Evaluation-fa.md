| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0029 |
| **عنوان** | ارزیابی فناوری ارائه‌دهنده هوش مصنوعی (Artificial Intelligence Provider Technology Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-28 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

# هدف (Purpose)

این ارزیابی فناوری استراتژی ارائه‌دهنده هوش مصنوعی را برای MachineryManagerEnterprise تعیین می‌کند.

فناوری انتخاب‌شده باید موارد زیر را فراهم آورد:

- تولید امبدینگ (Embedding Generation)
- تکمیل گفتگو (Chat Completion)
- تولید تقویت‌شده با بازیابی (Retrieval-Augmented Generation - RAG)
- دستیار هوش مصنوعی (AI Assistant)
- جستجوی دانش (Knowledge Search)
- توصیه‌های هوشمند تعمیر و نگهداری (Intelligent Maintenance Recommendations)
- توسعه‌های آینده هوش مصنوعی سازمانی

این ارزیابی منحصراً بر **ارائه‌دهندگان هوش مصنوعی (AI Providers)** تمرکز دارد.

انتخاب پایگاه داده برداری پیش‌تر در **TE-0028** تکمیل شده است.

---

# دامنه ارزیابی (Evaluation Scope)

این ارزیابی فناوری موارد زیر را ارزیابی می‌کند:

- ارائه‌دهندگان هوش مصنوعی ابری (Cloud AI Providers)
- ارائه‌دهندگان هوش مصنوعی محلی (Local AI Providers)
- استراتژی هوش مصنوعی ترکیبی (Hybrid AI Strategy)
- یکپارچگی هوش مصنوعی سازمانی
- ملاحظات عملیاتی
- مدل هزینه
- امنیت
- انعطاف‌پذیری استقرار

این سند موارد زیر را تعریف **نمی‌کند**:

- مهندسی پرامپت (Prompt Engineering)
- معماری عامل‌ها (Agent Architecture)
- معماری بازیابی داده‌ها (Retrieval Architecture)
- جریان‌های کاری هوش مصنوعی (AI Workflows)
- قوانین تجاری (Business Rules)

این تصمیمات معماری به‌صورت جداگانه در ADR مربوطه مستند خواهند شد.

---

# رابطه با ADRهای مرتبط (Relationship with Related ADRs)

این ارزیابی فناوری از موارد زیر پشتیبانی می‌کند:

- ADR-0022 — معماری بازیابی دانش هوش مصنوعی (AI Knowledge Retrieval Architecture)
- ADR-0023 — استراتژی ارائه‌دهنده هوش مصنوعی (Artificial Intelligence Provider Strategy) *(در انتظار)*

همچنین به موارد زیر وابسته است:

- TE-0028 — ارزیابی فناوری پایگاه داده برداری (Vector Database Technology Evaluation)
- معماری تمیز مصوب (Approved Clean Architecture)
- معماری امنیت مصوب (Approved Security Architecture)

---

# مراجع معماری (Architectural References)

این ارزیابی بر پایه موارد زیر استوار است:

- معماری تمیز (Clean Architecture)
- الگوی تفکیک مسئولیت فرمان و پرس‌وجو (CQRS)
- استراتژی استقرار ترکیبی (Hybrid Deployment Strategy)
- استانداردهای امنیت سازمانی (Enterprise Security Standards)
- نقشه راه هوش مصنوعی (AI Roadmap)

---

# دامنه (Scope)

فناوری‌های زیر مورد ارزیابی قرار می‌گیرند:

- Azure OpenAI
- OpenAI
- Ollama
- استراتژی هوش مصنوعی ترکیبی (Hybrid AI Strategy)

---

# معماری فعلی هوش مصنوعی (Current AI Architecture)

معماری مصوب هوش مصنوعی در حال حاضر شامل موارد زیر است:

```text
Application

        │

        ▼

Embedding Generation

        │

        ▼

Vector Database (Qdrant)

        │

        ▼

Large Language Model

        │

        ▼

AI Response
```

تصمیم معماری باقی‌مانده، انتخاب ارائه‌دهنده مدل زبانی بزرگ (Large Language Model provider) است.

---

# نیازمندی‌های کارکردی (Functional Requirements)

ارائه‌دهنده منتخب هوش مصنوعی باید موارد زیر را پشتیبانی کند:

- امبدینگ‌های متنی (Text Embeddings)
- تکمیل گفتگو (Chat Completion)
- فراخوانی ابزارها (Tool Calling)
- پاسخ‌های جریانی (Streaming Responses)
- فراخوانی توابع (Function Calling)
- پنجره‌های متنی بزرگ (Long Context Windows)
- احراز هویت سازمانی (Enterprise Authentication)
- رابط‌های برنامه‌نویسی پایدار (Stable APIs)
- پشتیبانی از کیت‌های توسعه (SDK Support)

---

# اصل معماری (Architecture Principle)

مؤلفه ارزیابی‌شده به‌عنوان یک سرویس زیرساختی ایزوله مطابق با اصول معماری تمیز و قواعد ایزولاسیون دامنه عمل می‌کند.

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

ارائه‌دهنده منتخب باید ویژگی‌های زیر را فراهم آورد:

- دسترسی‌پذیری بالا (High Availability)
- امنیت سازمانی (Enterprise Security)
- پیش‌بینی‌پذیری هزینه‌ها (Cost Predictability)
- قابلیت استقرار ترکیبی (Hybrid Deployment Capability)
- انعطاف‌پذیری در انتخاب ارائه‌دهنده (Vendor Flexibility)
- قابلیت نگهداری بلندمدت (Long-Term Maintainability)
- قابلیت اطمینان عملیاتی (Operational Reliability)
- کارایی و عملکرد (Performance)
- توسعه‌پذیری آینده (Future Extensibility)

---

# فناوری‌های کاندید (Candidate Technologies)

| کاندید | دسته‌بندی |
|-----------|----------|
| Azure OpenAI | هوش مصنوعی ابری مدیریت‌شده (Managed Cloud AI) |
| OpenAI | هوش مصنوعی ابری مدیریت‌شده (Managed Cloud AI) |
| Ollama | هوش مصنوعی محلی خودمیزبان (Self-Hosted Local AI) |
| Hybrid AI Strategy | استراتژی معماری (Architectural Strategy) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | اولویت |
|----|-----------|----------|
| AI-01 | آمادگی سازمانی (Enterprise Readiness) | حیاتی (Critical) |
| AI-02 | امنیت (Security) | حیاتی (Critical) |
| AI-03 | کیفیت امبدینگ (Embedding Quality) | حیاتی (Critical) |
| AI-04 | کیفیت تکمیل گفتگو (Chat Completion Quality) | حیاتی (Critical) |
| AI-05 | کارایی و عملکرد (Performance) | بالا (High) |
| AI-06 | هزینه (Cost) | بالا (High) |
| AI-07 | استقرار ترکیبی (Hybrid Deployment) | بالا (High) |
| AI-08 | استقلال از ارائه‌دهنده (Vendor Independence) | بالا (High) |
| AI-09 | سادگی عملیاتی (Operational Simplicity) | متوسط (Medium) |
| AI-10 | قابلیت نگهداری بلندمدت (Long-Term Maintainability) | بالا (High) |

---


# 8. ارزیابی Azure OpenAI (Azure OpenAI Evaluation)

## نمای کلی (Overview)

سرویس Azure OpenAI پیاده‌سازی سازمانی مایکروسافت از مدل‌های پایه OpenAI است که درون اکوسیستم Microsoft Azure میزبانی می‌شود.

به‌جای دسترسی مستقیم به پلتفرم عمومی OpenAI، سرویس Azure OpenAI همان قابلیت‌های هسته‌ای هوش مصنوعی را از طریق سرویس‌های بومی Azure همراه با حاکمیت، امنیت و انطباق سازمانی ارائه می‌دهد.

برای MachineryManagerEnterprise، سرویس Azure OpenAI به‌عنوان ارائه‌دهنده اصلی هوش مصنوعی ابری سازمانی ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
                  Application Layer

                         │

                         ▼

                 AI Orchestration Layer

                         │

                         ▼

                 Azure OpenAI Service

        ┌────────────────────────────────┐

        │ Embedding Models               │
        │ Chat Completion Models         │
        │ Function Calling               │
        │ Streaming                      │
        │ Content Filtering              │
        └────────────────────────────────┘

                         │

                         ▼

                AI Generated Response
```

سرویس Azure OpenAI به ارائه‌دهنده استنتاج ابری سازمانی تبدیل می‌شود در حالی که کلیه داده‌های تجاری درون معماری مصوب برنامه باقی می‌مانند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- میزبانی در سطح سازمانی
- یکپارچگی با Azure Active Directory
- شبکه‌بندی خصوصی (Private Networking)
- پشتیبانی از Managed Identity
- انطباق سازمانی
- دسترسی‌پذیری بالا
- پشتیبانی رسمی مایکروسافت
- یکپارچگی قدرتمند با اکوسیستم Azure
- رابط‌های برنامه‌نویسی پایدار سازمانی

---

# قابلیت‌های کارکردی (Functional Capabilities)

سرویس Azure OpenAI از موارد زیر پشتیبانی می‌کند:

- امبدینگ‌های متنی (Text Embeddings)
- تکمیل گفتگو (Chat Completion)
- فراخوانی توابع (Function Calling)
- فراخوانی ابزارها (Tool Calling)
- پاسخ‌های جریانی (Streaming Responses)
- خروجی ساخت‌یافته (Structured Output)
- حالت JSON (JSON Mode)
- مدل‌های بینایی (Vision Models)
- مدل‌های با پنجره متنی بزرگ (Long Context Models)

---

# امنیت (Security)

سرویس Azure OpenAI قابلیت‌های امنیت سازمانی را فراهم می‌کند از جمله:

- احراز هویت Azure Active Directory
- هویت مدیریت‌شده (Managed Identity)
- نقاط پایانی خصوصی (Private Endpoints)
- یکپارچگی با شبکه مجازی (Virtual Network Integration)
- کلیدهای مدیریت‌شده توسط مشتری (Customer Managed Keys)
- رمزنگاری در حالت سکون (Encryption at Rest)
- رمزنگاری در حال انتقال (Encryption in Transit)
- یکپارچگی با Microsoft Defender

امنیت در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# انطباق‌پذیری (Compliance)

سرویس Azure OpenAI از برنامه‌های انطباق سازمانی مایکروسافت پشتیبانی می‌کند از جمله:

- ISO 27001
- SOC
- GDPR
- HIPAA (منطقه‌ای)
- کنترل‌های هوش مصنوعی مسئولانه مایکروسافت

انطباق‌پذیری در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

سرویس Azure OpenAI موارد زیر را فراهم می‌کند:

- تأخیر استنتاج پایین
- توان عملیاتی بالا
- استقرار منطقه‌ای
- مقیاس‌پذیری خودکار
- توافق‌نامه سطح خدمات سازمانی (Enterprise SLA)

عملکرد در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# مدل هزینه (Cost Model)

قیمت‌گذاری بر مبنای مصرف است.

اجزای هزینه معمول شامل موارد زیر است:

- توکن‌های امبدینگ
- توکن‌های پرامپت (Prompt Tokens)
- توکن‌های تکمیل (Completion Tokens)
- انتخاب مدل

پیش‌بینی‌پذیری هزینه‌ها در سطح **خوب (Good)** ارزیابی می‌شود زیرا مدیریت هزینه Azure می‌تواند در حاکمیت سازمانی یکپارچه گردد.

---

# قابلیت‌های هوش مصنوعی (AI Capability)

سرویس Azure OpenAI موارد زیر را پشتیبانی می‌کند:

- کوپایلوت سازمانی (Enterprise Copilot)
- تولید تقویت‌شده با بازیابی (RAG)
- جستجوی معنایی (Semantic Search)
- دستیار دانش (Knowledge Assistant)
- توصیه‌های هوشمند (Intelligent Recommendations)
- پاسخ‌گویی به سؤالات با آگاهی از زمینه

قابلیت‌های هوش مصنوعی در سطح **عالی (Excellent)** ارزیابی می‌شوند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

تلاش عملیاتی حداقل است.

مایکروسافت موارد زیر را مدیریت می‌کند:

- زیرساخت
- میزبانی مدل‌ها
- مقیاس‌پذیری
- به‌روزرسانی‌ها
- دسترسی‌پذیری

پیچیدگی عملیاتی در سطح **بسیار پایین (Very Low)** ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

مدل‌های استقرار پشتیبانی‌شده:

| محیط | پشتیبانی |
|------------|:-------:|
| ابر Azure (Azure Cloud) | ✅ |
| سازمانی ترکیبی (Hybrid Enterprise) | ✅ |
| استقرار محلی (On-Premise) | ❌ |

اگرچه Azure OpenAI به‌خوبی با برنامه‌های سازمانی ترکیبی یکپارچه می‌شود، استنتاج فی‌نفسه همواره در Azure اجرا می‌گردد.

---

# وابستگی به ارائه‌دهنده (Vendor Lock-In)

سرویس Azure OpenAI وابستگی متوسطی به ارائه‌دهنده ایجاد می‌کند.

وابستگی‌ها شامل موارد زیر است:

- اشتراک Azure
- هویت Azure
- دسترسی‌پذیری منطقه‌ای Azure

با این حال، کد برنامه از طریق انتزاع رابط AI Provider قابل‌انتقال و مستقل باقی می‌ماند.

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- کیت‌های توسعه رسمی مایکروسافت
- رابط‌های برنامه‌نویسی REST APIs
- یکپارچگی با دات‌نت (.NET Integration)
- یکپارچگی با Semantic Kernel
- مستندات قوی
- ابزارهای سازمانی

تجربه توسعه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# آمادگی سازمانی (Enterprise Readiness)

سرویس Azure OpenAI برای سناریوهای زیر کاملاً مناسب است:

- دستیارهای هوش مصنوعی سازمانی
- جستجوی دانش شرکتی
- کوپایلوت داخلی
- سیستم‌های RAG
- اتوماسیون هوش مصنوعی
- هوشمندی اسناد (Document Intelligence)

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| امنیت (Security) | عالی (Excellent) |
| کیفیت امبدینگ (Embedding Quality) | عالی (Excellent) |
| کیفیت تکمیل گفتگو (Chat Completion Quality) | عالی (Excellent) |
| کارایی و عملکرد (Performance) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) |
| انطباق‌پذیری (Compliance) | عالی (Excellent) |
| یکپارچگی با Azure (Azure Integration) | عالی (Excellent) |
| استقلال از ارائه‌دهنده (Vendor Independence) | متوسط (Moderate) |
| قابلیت ترکیبی (Hybrid Capability) | بسیار خوب (Very Good) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

سرویس Azure OpenAI تمام نیازمندی‌های هوش مصنوعی ابری سازمانی MachineryManagerEnterprise را برآورده می‌سازد.

ترکیب آن از:

- امنیت سازمانی،
- یکپارچگی با اکوسیستم مایکروسافت،
- سادگی عملیاتی،
- قابلیت‌های بالغ هوش مصنوعی،
- پشتیبانی بلندمدت،

آن را به قوی‌ترین ارائه‌دهنده هوش مصنوعی ابری مدیریت‌شده در این ارزیابی فناوری تبدیل می‌کند.

---


# 9. ارزیابی OpenAI (OpenAI Evaluation)

## نمای کلی (Overview)

شرکت OpenAI پیاده‌سازی تجاری اولیه خانواده مدل‌های پایه GPT را از طریق پلتفرم ابری عمومی خود ارائه می‌دهد.

برخلاف Azure OpenAI، شرکت OpenAI به‌عنوان یک ارائه‌دهنده نرم‌افزار به‌عنوان سرویس (SaaS) مستقل با دسترسی مستقیم به جدیدترین مدل‌ها و ویژگی‌ها بلافاصله پس از انتشار عمل می‌کند.

در MachineryManagerEnterprise، ارائه‌دهنده OpenAI به‌عنوان ارائه‌دهنده هوش مصنوعی ابری سازمانی مستقل از Microsoft Azure ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
                  Application Layer

                         │

                         ▼

                 AI Orchestration Layer

                         │

                         ▼

                      OpenAI API

        ┌────────────────────────────────┐

        │ Embedding Models               │
        │ Chat Completion Models         │
        │ Function Calling               │
        │ Streaming                      │
        │ Structured Output              │
        └────────────────────────────────┘

                         │

                         ▼

                AI Generated Response
```

پایگاه داده رابطه‌ای عملیاتی و پایگاه داده برداری بدون تغییر باقی می‌مانند.

ارائه‌دهنده OpenAI صرفاً مسئول استنتاج و تولید امبدینگ است.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- دسترسی مستقیم به جدیدترین مدل‌های پایه.
- دسترسی سریع به ویژگی‌های جدید.
- کیفیت عالی مدل‌ها.
- رابط‌های برنامه‌نویسی عمومی بالغ.
- پذیرش گسترده در اکوسیستم.
- پشتیبانی گسترده جامعه کاربری.
- مستندات جامع.
- دسترسی عالی به SDKها.

---

# قابلیت‌های کارکردی (Functional Capabilities)

ارائه‌دهنده OpenAI از موارد زیر پشتیبانی می‌کند:

- امبدینگ‌های متنی (Text Embeddings)
- تکمیل گفتگو (Chat Completion)
- فراخوانی توابع (Function Calling)
- فراخوانی ابزارها (Tool Calling)
- پاسخ‌های جریانی (Streaming Responses)
- خروجی ساخت‌یافته (Structured Output)
- حالت JSON (JSON Mode)
- مدل‌های بینایی (Vision Models)
- مدل‌های با پنجره متنی بزرگ (Long Context Models)

---

# امنیت (Security)

ارائه‌دهنده OpenAI موارد زیر را فراهم می‌کند:

- احراز هویت با کلید API (API Key Authentication)
- رمزنگاری TLS
- رمزنگاری در حالت سکون
- مدیریت در سطح سازمان
- کنترل‌های استفاده و مصرف

در مقایسه با Azure OpenAI، یکپارچگی با هویت سازمانی محدودتر است.

امنیت در سطح **بسیار خوب (Very Good)** ارزیابی می‌شود.

---

# انطباق‌پذیری (Compliance)

ارائه‌دهنده OpenAI پلن‌های سازمانی با قابلیت‌های انطباق ارائه می‌دهد.

پشتیبانی معمول شامل موارد زیر است:

- GDPR
- SOC
- مدیریت سازمانی

انطباق‌پذیری در سطح **بسیار خوب (Very Good)** ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

ارائه‌دهنده OpenAI ویژگی‌های زیر را ارائه می‌دهد:

- استنتاج با کیفیت بالا
- تأخیر پایین
- زیرساخت جهانی
- مقیاس‌پذیری خودکار

عملکرد در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# مدل هزینه (Cost Model)

قیمت‌گذاری مبتنی بر مصرف است.

صورتحساب به موارد زیر بستگی دارد:

- توکن‌های امبدینگ
- توکن‌های پرامپت
- توکن‌های تکمیل
- مدل انتخاب‌شده

پیش‌بینی‌پذیری هزینه‌ها در سطح **خوب (Good)** ارزیابی می‌شود.

---

# قابلیت‌های هوش مصنوعی (AI Capability)

ارائه‌دهنده OpenAI موارد زیر را پشتیبانی می‌کند:

- دستیار دانش سازمانی
- تولید تقویت‌شده با بازیابی (RAG)
- کوپایلوت هوش مصنوعی
- جستجوی معنایی
- توصیه‌های هوشمند
- تعامل با زبان طبیعی

قابلیت‌های هوش مصنوعی در سطح **عالی (Excellent)** ارزیابی می‌شوند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

مدیریت زیرساخت کاملاً توسط OpenAI انجام می‌شود.

تیم توسعه صرفاً مسئول موارد زیر است:

- یکپارچگی API
- مدیریت پرامپت‌ها
- پایش هزینه‌ها

پیچیدگی عملیاتی در سطح **بسیار پایین (Very Low)** ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

مدل‌های استقرار پشتیبانی‌شده:

| محیط | پشتیبانی |
|------------|:-------:|
| ابر OpenAI (OpenAI Cloud) | ✅ |
| سازمانی ترکیبی (Hybrid Enterprise) | ✅ |
| استقرار محلی (On-Premise) | ❌ |

استنتاج همواره درون پلتفرم ابری OpenAI اجرا می‌شود.

---

# وابستگی به ارائه‌دهنده (Vendor Lock-In)

ارائه‌دهنده OpenAI وابستگی به موارد زیر را ایجاد می‌کند:

- ابر OpenAI
- رابط‌های برنامه‌نویسی OpenAI
- مدل قیمت‌گذاری OpenAI

با این حال، انتزاع ارائه‌دهنده در معماری برنامه تلاش‌های مهاجرت بلندمدت را محدود می‌سازد.

استقلال از ارائه‌دهنده در سطح **متوسط (Moderate)** ارزیابی می‌شود.

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- کیت‌های توسعه عالی
- رابط‌های برنامه‌نویسی REST APIs
- مستندات جامع
- جامعه کاربری بزرگ
- دسترسی سریع به ویژگی‌ها

تجربه توسعه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# آمادگی سازمانی (Enterprise Readiness)

ارائه‌دهنده OpenAI برای سناریوهای زیر مناسب است:

- دستیارهای هوش مصنوعی سازمانی
- کوپایلوت داخلی
- جستجوی دانش
- تولید تقویت‌شده با بازیابی
- اتوماسیون هوشمند
- پاسخ‌گویی معنایی به سؤالات

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| امنیت (Security) | بسیار خوب (Very Good) |
| کیفیت امبدینگ (Embedding Quality) | عالی (Excellent) |
| کیفیت تکمیل گفتگو (Chat Completion Quality) | عالی (Excellent) |
| کارایی و عملکرد (Performance) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) |
| انطباق‌پذیری (Compliance) | بسیار خوب (Very Good) |
| استقلال از ارائه‌دهنده (Vendor Independence) | متوسط (Moderate) |
| قابلیت ترکیبی (Hybrid Capability) | خوب (Good) |
| تجربه توسعه‌دهنده (Developer Experience) | عالی (Excellent) |

---

# مقایسه با Azure OpenAI (Comparison with Azure OpenAI)

| معیار | Azure OpenAI | OpenAI |
|-----------|--------------|---------|
| جدیدترین مدل‌ها | دسترسی با تأخیر (Delayed Availability) | دسترسی فوری (Immediate Availability) |
| یکپارچگی با مایکروسافت | عالی (Excellent) | محدود (Limited) |
| Azure AD | بومی (Native) | خیر (No) |
| شبکه‌بندی خصوصی | بومی (Native) | محدود (Limited) |
| حاکمیت سازمانی | عالی (Excellent) | بسیار خوب (Very Good) |
| پیچیدگی عملیاتی | بسیار پایین (Very Low) | بسیار پایین (Very Low) |
| کیفیت مدل | عالی (Excellent) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

ارائه‌دهنده OpenAI قابلیت‌های هوش مصنوعی در سطح جهانی و دسترسی فوری به جدیدترین مدل‌های پایه را ارائه می‌دهد.

با این حال، MachineryManagerEnterprise در حال حاضر فناوری‌های مایکروسافت را در سراسر زیرساخت خود به کار گرفته است.

بنابراین Azure OpenAI از طریق موارد زیر همسویی معماری قوی‌تری فراهم می‌سازد:

- یکپارچگی با هویت سازمانی؛
- امنیت بومی Azure؛
- قابلیت‌های حاکمیت داده؛
- هماهنگی عملیاتی.

ارائه‌دهنده OpenAI یک ارائه‌دهنده برجسته هوش مصنوعی باقی می‌ماند اما **ارائه‌دهنده ابری مدیریت‌شده ترجیحی** برای MachineryManagerEnterprise نیست.

---


# 10. ارزیابی Ollama (Ollama Evaluation)

## نمای کلی (Overview)

پایگاه Ollama یک محیط اجرای محلی و متن‌باز برای مدل‌های زبانی بزرگ است که سازمان‌ها را قادر می‌سازد مدل‌های پایه مدرن را کاملاً درون زیرساخت خود اجرا کنند.

برخلاف Azure OpenAI و OpenAI، اولاما خدمات میزبانی‌شده هوش مصنوعی ارائه نمی‌دهد.

در عوض، به سازمان‌ها اجازه می‌دهد مدل‌های با وزن باز (open-weight models) را به‌صورت محلی مستقر و مدیریت نمایند و کنترل کاملی بر محل نگهداری داده‌ها (data residency)، زیرساخت استنتاج و چرخه حیات مدل داشته باشند.

در MachineryManagerEnterprise، پلتفرم Ollama به‌عنوان **ارائه‌دهنده اصلی هوش مصنوعی خودمیزبان (self-hosted)** ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
                  Application Layer

                         │

                         ▼

                 AI Orchestration Layer

                         │

                         ▼

                   Ollama Runtime

        ┌────────────────────────────────┐

        │ Local Language Models          │
        │ Embedding Models               │
        │ Chat Models                    │
        │ Tool Calling                   │
        └────────────────────────────────┘

                         │

                         ▼

                AI Generated Response
```

پلتفرم Ollama کاملاً درون زیرساخت سازمانی اجرا می‌شود.

هیچ استنتاج ابری خارجی مورد نیاز نیست.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- مالکیت کامل زیرساخت.
- بدون وابستگی به APIهای خارجی.
- حاکمیت کامل بر محل نگهداری داده‌ها (Full data residency).
- عدم وابستگی به ارائه‌دهنده (No vendor lock-in).
- عملکرد کاملاً آفلاین (Offline operation).
- اکوسیستم متن‌باز.
- انتخاب منعطف مدل‌ها.
- حفظ حریم خصوصی سازمانی.
- استقلال از ابر.

---

# قابلیت‌های کارکردی (Functional Capabilities)

پلتفرم Ollama از موارد زیر پشتیبانی می‌کند:

- مدل‌های گفتگوی محلی (Local Chat Models)
- مدل‌های امبدینگ محلی (Local Embedding Models)
- پاسخ‌های جریانی (Streaming Responses)
- فراخوانی ابزارها (Tool Calling)
- رابط کاربری REST API
- مدیریت چندین مدل به‌طور هم‌زمان
- مدل‌های کوانتایز شده (Quantized Models)
- استنتاج آفلاین (Offline Inference)

---

# امنیت (Security)

پلتفرم Ollama قوی‌ترین مدل امنیتی را ارائه می‌دهد زیرا تمام استنتاج درون زیرساخت سازمان باقی می‌ماند.

مزایا شامل موارد زیر است:

- بدون انتقال داده به خارج از سازمان.
- اجرای درون شبکه داخلی.
- حفاظت توسط فایروال سازمانی.
- مالکیت زیرساخت.
- حاکمیت کامل بر داده‌ها.

امنیت در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# انطباق‌پذیری (Compliance)

انطباق‌پذیری کاملاً به زیرساخت سازمان بستگی دارد.

مزایا شامل موارد زیر است:

- انطباق کامل با GDPR از طریق استقرار محلی.
- قابلیت ممیزی داخلی.
- حاکمیت کامل بر داده‌ها.
- کنترل مقرراتی.

انطباق‌پذیری در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

عملکرد به سخت‌افزار در دسترس بستگی دارد.

ملاحظات معمول شامل موارد زیر است:

- کارایی پردازنده مرکزی (CPU)
- در دسترس بودن پردازنده گرافیکی (GPU)
- اندازه مدل
- ظرفیت حافظه رم

عملکرد بسته به استقرار از سطح **خوب (Good)** تا **عالی (Excellent)** متغیر است.

---

# مدل هزینه (Cost Model)

برخلاف ارائه‌دهندگان ابری، Ollama هزینه‌های زیر را به همراه دارد:

هزینه‌های اولیه:

- سخت‌افزار GPU
- زیرساخت محاسباتی
- ذخیره‌سازی
- مدیریت و پیکربندی

هزینه‌های عملیاتی:

- برق و انرژی
- نگهداری سخت‌افزار
- پایش زیرساخت

با این حال:

- بدون قیمت‌گذاری بر مبنای توکن؛
- بدون صورتحساب استنتاج؛
- هزینه‌های عملیاتی بلندمدت و قابل پیش‌بینی.

پیش‌بینی‌پذیری هزینه‌ها در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# قابلیت‌های هوش مصنوعی (AI Capability)

پلتفرم Ollama از مدل‌های متعدد با وزن باز پشتیبانی می‌کند از جمله:

- Llama
- Mistral
- Gemma
- Phi
- DeepSeek
- Qwen
- امبدینگ‌های BGE
- امبدینگ‌های Nomic

قابلیت‌های هوش مصنوعی در سطح **بسیار خوب (Very Good)** ارزیابی می‌شوند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

مسئولیت‌های عملیاتی بر عهده سازمان باقی می‌ماند.

فعالیت‌های مورد نیاز شامل موارد زیر است:

- استقرار مدل‌ها
- پایش زیرساخت
- مدیریت GPU
- برنامه‌ریزی ظرفیت
- ارتقای نسخه‌ها

پیچیدگی عملیاتی در سطح **بالا (High)** ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

مدل‌های استقرار پشتیبانی‌شده:

| محیط | پشتیبانی |
|------------|:-------:|
| استقرار محلی (On-Premise) | ✅ |
| ترکیبی (Hybrid) | ✅ |
| ابر خصوصی (Private Cloud) | ✅ |
| ماشین مجازی در ابر عمومی (Public Cloud VM) | ✅ |
| محیط آفلاین (Offline Environment) | ✅ |

انعطاف‌پذیری استقرار در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# وابستگی به ارائه‌دهنده (Vendor Lock-In)

وابستگی به ارائه‌دهنده عملاً حذف می‌شود.

مزایا:

- محیط اجرای متن‌باز.
- مدل‌های با وزن باز.
- مالکیت زیرساخت.
- مدل‌های قابل تعویض.

استقلال از ارائه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- رابط ساده REST API
- پشتیبانی از Docker
- استقرار چندسکویی
- اکوسیستم رو به رشد

معایب:

- آماده‌سازی سخت‌افزار
- مدیریت مدل‌ها
- بهینه‌سازی GPU

تجربه توسعه‌دهنده در سطح **خوب (Good)** ارزیابی می‌شود.

---

# آمادگی سازمانی (Enterprise Readiness)

پلتفرم Ollama به‌ویژه برای موارد زیر مناسب است:

- محیط‌های ایزوله و بدون اتصال (Air-gapped environments)
- سیستم‌های دولتی
- محیط‌های نظامی
- سازمان‌های با امنیت بسیار بالا
- استقرارهای حساس به حریم خصوصی

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | بسیار خوب (Very Good) |
| امنیت (Security) | عالی (Excellent) |
| کیفیت امبدینگ (Embedding Quality) | بسیار خوب (Very Good) |
| کیفیت تکمیل گفتگو (Chat Completion Quality) | بسیار خوب (Very Good) |
| کارایی و عملکرد (Performance) | خوب تا عالی (Good–Excellent) |
| سادگی عملیاتی (Operational Simplicity) | متوسط (Moderate) |
| انطباق‌پذیری (Compliance) | عالی (Excellent) |
| استقلال از ارائه‌دهنده (Vendor Independence) | عالی (Excellent) |
| قابلیت ترکیبی (Hybrid Capability) | عالی (Excellent) |
| تجربه توسعه‌دهنده (Developer Experience) | خوب (Good) |

---

# مقایسه با ارائه‌دهندگان مدیریت‌شده (Comparison with Managed Providers)

| معیار | Azure OpenAI | OpenAI | Ollama |
|-----------|--------------|---------|---------|
| نیازمندی به ابر | بله (Yes) | بله (Yes) | خیر (No) |
| عملکرد آفلاین | خیر (No) | خیر (No) | بله (Yes) |
| وابستگی به ارائه‌دهنده | متوسط (Moderate) | متوسط (Moderate) | بسیار پایین (Very Low) |
| محل نگهداری داده‌ها | محدود (Limited) | محدود (Limited) | کامل (Complete) |
| پیچیدگی عملیاتی | بسیار پایین (Very Low) | بسیار پایین (Very Low) | بالا (High) |
| مالکیت زیرساخت | خیر (No) | خیر (No) | بله (Yes) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

پلتفرم Ollama بالاترین درجه از مالکیت زیرساخت، امنیت و انعطاف‌پذیری استقرار را فراهم می‌سازد.

با این حال، این مزایا با هزینه‌های زیر همراه است:

- پیچیدگی عملیاتی بالاتر؛
- مدیریت زیرساخت؛
- نیازمندی‌های سخت‌افزاری؛
- مدیریت چرخه حیات مدل.

برای MachineryManagerEnterprise، پلتفرم Ollama به‌عنوان یک **فناوری تکمیلی برجسته** برای استقرارهای آینده هوش مصنوعی ترکیبی در نظر گرفته می‌شود، اما **به‌عنوان ارائه‌دهنده اصلی هوش مصنوعی سازمانی برای فاز اولیه پیاده‌سازی پیشنهاد نمی‌شود**.

---


# 11. ارزیابی استراتژی هوش مصنوعی ترکیبی (Hybrid AI Strategy Evaluation)

## نمای کلی (Overview)

استراتژی هوش مصنوعی ترکیبی، چندین ارائه‌دهنده هوش مصنوعی را تحت یک لایه انتزاعی یکپارچه به جای وابستگی به یک ارائه‌دهنده واحد ترکیب می‌کند.

به‌جای جفت شدن محکم برنامه با یک موتور استنتاج، برنامه با یک رابط داخلی AI Provider تعامل می‌کند در حالی که چندین پیاده‌سازی قابل تعویض باقی می‌مانند.

این استراتژی انعطاف‌پذیری، تاب‌آوری و استقلال بلندمدت از ارائه‌دهندگان را فراهم می‌آورد.

---

# نقش معماری (Architectural Role)

```text
                 Application Layer

                         │

                         ▼

                AI Provider Abstraction

                         │

         ┌────────────────┼────────────────┐

         ▼                ▼                ▼

  Azure OpenAI        OpenAI          Ollama

         │                │                │

         └────────────────┴────────────────┘

                         │

                         ▼

                  AI Response
```

برنامه هرگز مستقیماً با هیچ ارائه‌دهنده خاصی ارتباط برقرار نمی‌کند.

تمام منطق مختص ارائه‌دهندگان درون لایه Infrastructure ایزوله می‌شود.

---

# اصول معماری (Architectural Principles)

استراتژی هوش مصنوعی ترکیبی از اصول معماری مصوب پیروی می‌کند:

- وارونگی وابستگی (Dependency Inversion)
- ایزولاسیون زیرساخت (Infrastructure Isolation)
- استقلال از ارائه‌دهنده (Provider Independence)
- پیاده‌سازی‌های قابل تعویض (Replaceable Implementations)
- معماری تمیز (Clean Architecture)

---

# مزایا (Advantages)

## استقلال از ارائه‌دهنده (Vendor Independence)

هیچ ارائه‌دهنده هوش مصنوعی منفردی به یک وابستگی دائمی معماری تبدیل نمی‌شود.

ارائه‌دهندگان را می‌توان بدون تأثیر بر بخش‌های زیر جایگزین کرد:

- لایه Application
- لایه Domain
- منطق تجاری (Business Logic)

---

## تداوم کسب‌وکار (Business Continuity)

در صورت عدم دسترسی به یک ارائه‌دهنده:

```text
Azure OpenAI

      │

Unavailable

      ▼

Automatic Provider Selection

      ▼

OpenAI

or

Ollama
```

تداوم ارائه سرویس حفظ می‌شود.

---

## بهینه‌سازی هزینه (Cost Optimization)

ارائه‌دهندگان متفاوتی را می‌توان برای بارهای کاری مختلف انتخاب کرد.

نمونه‌ها:

| بار کاری | ارائه‌دهنده |
|----------|----------|
| امبدینگ‌ها (Embeddings) | Azure OpenAI |
| تکمیل گفتگو (Chat Completion) | Azure OpenAI |
| استقرار آفلاین (Offline Deployment) | Ollama |
| بازیابی پس از فاجعه (Disaster Recovery) | OpenAI |

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

این استراتژی از موارد زیر پشتیبانی می‌کند:

- ابر (Cloud)
- ترکیبی (Hybrid)
- محلی (On-Premise)
- آفلاین (Offline)

بدون تغییر در منطق برنامه.

---

# معایب (Disadvantages)

استراتژی ترکیبی پیچیدگی معماری بیشتری را معرفی می‌کند.

مؤلفه‌های مورد نیاز شامل موارد زیر است:

- انتزاع ارائه‌دهنده (Provider Abstraction)
- منطق انتخاب ارائه‌دهنده (Provider Selection Logic)
- مدیریت پیکربندی (Configuration Management)
- پایش سلامت سرویس‌ها (Health Monitoring)
- سیاست‌های تلاش مجدد (Retry Policies)

بنابراین پیچیدگی عملیاتی اندکی افزایش می‌یابد.

---

# انتخاب ارائه‌دهنده (Provider Selection)

این استراتژی از انتخاب ارائه‌دهنده بر مبنای پیکربندی پشتیبانی می‌کند.

مثال:

```text
Embedding Provider

↓

Azure OpenAI

----------------------------

Chat Provider

↓

Azure OpenAI

----------------------------

Offline Mode

↓

Ollama
```

هیچ تغییری در کد برنامه مورد نیاز نیست.

---

# قابلیت Failover (Failover Capability)

قابلیت Failover اختیاری ارائه‌دهنده می‌تواند پیاده‌سازی شود.

مثال:

```text
Primary

Azure OpenAI

      │

Failure

      ▼

Secondary

OpenAI

      │

Failure

      ▼

Local

Ollama
```

این قابلیت دسترسی‌پذیری را افزایش می‌دهد در حالی که کاملاً برای منطق تجاری نامرئی و شفاف باقی می‌ماند.

---

# سازگاری با معماری تمیز (Clean Architecture Compatibility)

استراتژی ترکیبی کاملاً با معماری تمیز سازگار است.

جهت وابستگی‌ها به شرح زیر باقی می‌ماند:

```text
Application

      │

IAIProvider

      │

Infrastructure

      │

Azure OpenAI

OpenAI

Ollama
```

لایه دامنه کاملاً از جزئیات پیاده‌سازی بی‌خبر باقی می‌ماند.

---

# امنیت (Security)

هر ارائه‌دهنده می‌تواند موارد زیر را به‌طور مستقل حفظ کند:

- اطلاعات کاربری و کلیدها؛
- احراز هویت؛
- پیکربندی؛
- سیاست‌های شبکه.

کلیدهای محرمانه همچنان از طریق راهکار مصوب مدیریت اسرار سازمانی مدیریت می‌شوند.

---

# قابلیت نگهداری بلندمدت (Long-Term Maintainability)

استراتژی ترکیبی امکان موارد زیر را فراهم می‌سازد:

- جایگزینی ارائه‌دهنده؛
- ارتقای ارائه‌دهنده؛
- معرفی ارائه‌دهندگان جدید هوش مصنوعی؛
- بازنشستگی ارائه‌دهندگان موجود؛

بدون تأثیر بر منطق تجاری برنامه.

قابلیت نگهداری در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تناسب سازمانی (Enterprise Suitability)

استراتژی ترکیبی به‌ویژه برای موارد زیر ارزشمند است:

- نرم‌افزارهای سازمانی
- چرخه‌های حیات طولانی محصول
- استقلال از ارائه‌دهنده
- انطباق با قوانین و مقررات
- تکامل تدریجی زیرساخت

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| استقلال از ارائه‌دهنده (Vendor Independence) | عالی (Excellent) |
| انعطاف‌پذیری آینده (Future Flexibility) | عالی (Excellent) |
| انعطاف‌پذیری استقرار (Deployment Flexibility) | عالی (Excellent) |
| تداوم کسب‌وکار (Business Continuity) | عالی (Excellent) |
| پیچیدگی عملیاتی (Operational Complexity) | متوسط (Moderate) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

اگرچه پیاده‌سازی اولیه MachineryManagerEnterprise از یک ارائه‌دهنده اصلی هوش مصنوعی استفاده خواهد کرد، اما معماری باید از ابتدا طوری طراحی شود که از چندین ارائه‌دهنده قابل تعویض پشتیبانی کند.

بنابراین استراتژی هوش مصنوعی ترکیبی نمایانگر رویکرد معماری بلندمدت ترجیحی برای پلتفرم است.

پیاده‌سازی اولیه باید سادگی را در اولویت قرار دهد و در عین حال توانایی معرفی ارائه‌دهندگان اضافی را بدون بازنویسی و ریفکتور معماری حفظ نماید.

---


# 12. مقایسه کلی فناوری‌ها (Overall Technology Comparison)

به‌دنبال ارزیابی تفصیلی تمامی فناوری‌های کاندید، شورای بازنگری معماری هر گزینه را در برابر اهداف معماری بلندمدت MachineryManagerEnterprise مقایسه نمود.

---

# ماتریس کلی فناوری‌ها (Overall Technology Matrix)

| معیار ارزیابی | Azure OpenAI | OpenAI | Ollama |
|----------------------|:------------:|:------:|:------:|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| امنیت (Security) | عالی (Excellent) | خوب (Good) | عالی (Excellent) |
| انطباق‌پذیری (Compliance) | عالی (Excellent) | خوب (Good) | عالی (Excellent) |
| کیفیت امبدینگ (Embedding Quality) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| کیفیت تکمیل گفتگو (Chat Completion Quality) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| قابلیت‌های هوش مصنوعی (AI Capability) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| کارایی و عملکرد (Performance) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) | عالی (Excellent) | متوسط (Fair) |
| استقلال از ارائه‌دهنده (Vendor Independence) | متوسط (Fair) | متوسط (Fair) | عالی (Excellent) |
| استقرار ترکیبی (Hybrid Deployment) | خوب (Good) | متوسط (Fair) | عالی (Excellent) |
| پشتیبانی محلی (On-Premise Support) | ❌ | ❌ | ✅ |
| بی‌طرفی ابری (Cloud Neutrality) | متوسط (Fair) | متوسط (Fair) | عالی (Excellent) |
| قابلیت نگهداری بلندمدت (Long-Term Maintainability) | عالی (Excellent) | خوب (Good) | خوب (Good) |

---

# مقایسه مدل استقرار (Deployment Model Comparison)

| قابلیت | Azure OpenAI | OpenAI | Ollama |
|------------|:------------:|:------:|:------:|
| ابر Azure | ✅ | ❌ | اختیاری (Optional) |
| ابر عمومی | ✅ | ✅ | اختیاری (Optional) |
| ترکیبی (Hybrid) | ✅ | محدود (Limited) | ✅ |
| استقرار محلی (On-Premise) | ❌ | ❌ | ✅ |
| عملکرد آفلاین (Offline Operation) | ❌ | ❌ | ✅ |

---

# مالکیت زیرساخت (Infrastructure Ownership)

| فناوری | مالک زیرساخت |
|------------|----------------------|
| Azure OpenAI | Microsoft Azure |
| OpenAI | OpenAI |
| Ollama | سازمان (Organization) |

---

# استقلال از ارائه‌دهنده (Vendor Independence)

```text
Highest Independence

Ollama

↓

Azure OpenAI

↓

OpenAI

Lowest Independence
```

اگرچه Ollama مالکیت کامل زیرساخت را فراهم می‌آورد، اما این مزیت با مسئولیت عملیاتی قابل توجهی همراه است.

---

# پیچیدگی عملیاتی (Operational Complexity)

```text
Lowest Complexity

Azure OpenAI

↓

OpenAI

↓

Ollama

Highest Complexity
```

سرویس‌های Azure OpenAI و OpenAI مدیریت زیرساخت را تقریباً به‌طور کامل حذف می‌کنند.

پلتفرم Ollama نیازمند تأمین سخت‌افزار، پایش، ارتقا و مدیریت چرخه حیات مدل‌ها است.

---

# یکپارچگی سازمانی (Enterprise Integration)

| قابلیت | Azure OpenAI | OpenAI | Ollama |
|------------|:------------:|:------:|:------:|
| هویت مایکروسافت (Microsoft Identity) | ✅ | ❌ | فاقد کاربرد (N/A) |
| حاکمیت سازمانی (Enterprise Governance) | ✅ | خوب (Good) | مدیریت توسط سازمان |
| شبکه‌بندی خصوصی (Private Networking) | ✅ | محدود (Limited) | ✅ |
| استقرار داخلی (Internal Deployment) | ❌ | ❌ | ✅ |

---

# مقایسه قابلیت‌های هوش مصنوعی (AI Capability Comparison)

| قابلیت | Azure OpenAI | OpenAI | Ollama |
|------------|:------------:|:------:|:------:|
| RAG | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| جستجوی معنایی (Semantic Search) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| دستیار هوش مصنوعی (AI Assistant) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| فراخوانی ابزارها (Tool Calling) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| فراخوانی توابع (Function Calling) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| امبدینگ‌ها (Embeddings) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |

---

# ویژگی‌های هزینه (Cost Characteristics)

| معیار | Azure OpenAI | OpenAI | Ollama |
|------------|:------------:|:------:|:------:|
| هزینه اولیه (Initial Cost) | پایین (Low) | پایین (Low) | بالا (High) |
| هزینه عملیاتی (Operational Cost) | مبتنی بر مصرف | مبتنی بر مصرف | مبتنی بر زیرساخت |
| پیش‌بینی‌پذیری (Predictability) | خوب (Good) | خوب (Good) | عالی (Excellent) |
| سرمایه‌گذاری سخت‌افزاری | ندارد (None) | ندارد (None) | مورد نیاز است (Required) |

---

# تناسب معماری بلندمدت (Long-Term Architectural Suitability)

| نیازمندی | بهترین کاندید |
|-------------|----------------|
| حاکمیت سازمانی (Enterprise Governance) | Azure OpenAI |
| اکوسیستم مایکروسافت | Azure OpenAI |
| جدیدترین ویژگی‌های هوش مصنوعی | OpenAI |
| استقرار آفلاین | Ollama |
| استقلال کامل از ارائه‌دهنده | Ollama |
| استراتژی ترکیبی سازمانی | Hybrid AI Strategy |

---

# رتبه‌بندی فناوری‌ها (Technology Ranking)

| رتبه | فناوری |
|------|------------|
| **1** | **Azure OpenAI** |
| **2** | **OpenAI** |
| **3** | **Ollama** |

این رتبه‌بندی اولویت‌های معماری فعلی MachineryManagerEnterprise را بازتاب می‌دهد و نه صرفاً قابلیت خام هوش مصنوعی را.

---

# ارزیابی معماری (Architectural Assessment)

معماری مصوب بر موارد زیر تأکید دارد:

- حاکمیت سازمانی
- یکپارچگی با اکوسیستم مایکروسافت
- امنیت
- قابلیت نگهداری
- آمادگی استقرار ترکیبی
- استقلال آینده از ارائه‌دهندگان

سرویس Azure OpenAI قوی‌ترین همسویی را با این اصول فراهم می‌کند در حالی که دسترسی به پیشرفته‌ترین مدل‌های پایه را حفظ می‌نماید.

ارائه‌دهنده OpenAI یک جایگزین عالی باقی می‌ماند اما یکپارچگی سازمانی ضعیف‌تری ارائه می‌دهد.

پلتفرم Ollama مالکیت زیرساخت و انعطاف‌پذیری استقرار برجسته‌ای را ارائه می‌دهد، اما پیچیدگی عملیاتی آن را برای یک قابلیت تکمیلی نسبت به پلتفرم اولیه هوش مصنوعی سازمانی مناسب‌تر می‌سازد.

---


# 13. پیشنهاد نهایی (Final Recommendation)

پس از ارزیابی تمامی ارائه‌دهندگان کاندید هوش مصنوعی در برابر اصول معماری مصوب MachineryManagerEnterprise، شورای بازنگری معماری اتخاذ **معماری ارائه‌دهنده هوش مصنوعی ترکیبی (Hybrid AI Provider Architecture)** را با **Azure OpenAI** به‌عنوان ارائه‌دهنده اصلی پیشنهاد می‌کند.

---

# مقایسه کلی فناوری‌ها (Overall Technology Comparison)

فناوری انتخاب‌شده عملکرد بهینه، قابلیت نگهداری و سازگاری با معماری تمیز را فراهم می‌سازد.

## ماتریس مسئولیت (Responsibility Matrix)

| مسئولیت | فناوری پیشنهادی | جایگزین |
|-------------------|------------------------|------------------|
| قابلیت سیستم | انتخاب اصلی (Primary Selected) | گزینه ارزیابی‌شده (Evaluated Option) |

---

# خلاصه پیشنهاد (Recommendation Summary)

| فناوری | پیشنهاد |
|------------|----------------|
| **Azure OpenAI** | **ارائه‌دهنده اصلی هوش مصنوعی (Primary AI Provider)** |
| **OpenAI** | ارائه‌دهنده ابری ثانویه (Secondary Cloud Provider) |
| **Ollama** | ارائه‌دهنده محلی / آفلاین (Local / Offline Provider) |
| **Hybrid AI Strategy** | **معماری مصوب (Approved Architecture)** |

---

# پیشنهاد اصلی (Primary Recommendation)

سرویس Azure OpenAI باید به‌عنوان ارائه‌دهنده اصلی هوش مصنوعی برای MachineryManagerEnterprise اتخاذ شود.

این تصمیم بر مبنای ویژگی‌های زیر استوار است:

- امنیت سازمانی
- یکپارچگی با اکوسیستم مایکروسافت
- پشتیبانی از Azure Active Directory
- هویت مدیریت‌شده (Managed identity)
- انطباق سازمانی
- سادگی عملیاتی
- قابلیت نگهداری بلندمدت
- قابلیت‌های بالغ هوش مصنوعی

---

# پیشنهاد ثانویه (Secondary Recommendation)

ارائه‌دهنده OpenAI باید به‌عنوان یک ارائه‌دهنده ابری جایگزین از طریق لایه انتزاعی ارائه‌دهنده پشتیبانی شود.

سناریوهای معمول شامل موارد زیر است:

- ارزیابی مدل‌های جدید منتشرشده؛
- مقایسه ویژگی‌ها؛
- بازیابی پس از فاجعه (Disaster recovery)؛
- مهاجرت آینده.

ارائه‌دهنده OpenAI نباید به وابستگی مستقیم برنامه تبدیل شود.

---

# پیشنهاد هوش مصنوعی محلی (Local AI Recommendation)

پلتفرم Ollama باید به‌عنوان ارائه‌دهنده استنتاج محلی پشتیبانی شود.

سناریوهای معمول شامل موارد زیر است:

- محیط‌های قطع اتصال (Disconnected environments)؛
- نصب‌های محلی مشتریان (On-premise)؛
- محیط‌های توسعه (Development environments)؛
- استقرارهای حساس به حریم خصوصی؛
- نسخه‌های سازمانی آینده.

پلتفرم Ollama نباید به‌عنوان ارائه‌دهنده پیش‌فرض برای نسخه اولیه استفاده شود.

---

# استراتژی ترکیبی مصوب (Approved Hybrid Strategy)

پلتفرم باید سلسله‌مراتب ارائه‌دهندگان زیر را اتخاذ کند:

```text
                    Application

                          │

                          ▼

                  IAIProvider Interface

                          │

        ┌─────────────────┼─────────────────┐

        ▼                 ▼                 ▼

 Azure OpenAI        OpenAI            Ollama

 Primary          Secondary          Local

```

برنامه هرگز نباید مستقیماً به یک پیاده‌سازی ارائه‌دهنده خاص وابسته باشد.

---

# مزایای معماری (Architectural Benefits)

استراتژی مصوب موارد زیر را فراهم می‌آورد:

- استقلال از ارائه‌دهنده
- پیاده‌سازی‌های قابل تعویض
- انطباق با معماری تمیز
- قابلیت توسعه‌پذیری آینده
- پشتیبانی از استقرار ترکیبی
- انعطاف‌پذیری عملیاتی

---

# استراتژی هزینه (Cost Strategy)

مدل عملیاتی پیشنهادی:

| قابلیت | ارائه‌دهنده ترجیحی |
|------------|-------------------|
| امبدینگ‌ها (Embeddings) | Azure OpenAI |
| تکمیل گفتگو (Chat Completion) | Azure OpenAI |
| کوپایلوت سازمانی (Enterprise Copilot) | Azure OpenAI |
| هوش مصنوعی آفلاین (Offline AI) | Ollama |
| مدل‌های تجربی (Experimental Models) | OpenAI |

این استراتژی تعادلی میان موارد زیر برقرار می‌کند:

- حاکمیت سازمانی؛
- سادگی عملیاتی؛
- انعطاف‌پذیری بلندمدت؛
- هزینه‌های عملیاتی قابل پیش‌بینی.

---

# راهنمای سازمانی (Enterprise Guidance)

اصول زیر توسعه آینده هوش مصنوعی را هدایت خواهند کرد:

- منطق تجاری هرگز نباید به یک ارائه‌دهنده مشخص هوش مصنوعی وابسته باشد.
- ارائه‌دهندگان هوش مصنوعی باید از طریق پیکربندی قابل تعویض باشند.
- مدیریت پرامپت‌ها باید مستقل از ارائه‌دهنده باقی بماند.
- تولید تقویت‌شده با بازیابی (RAG) باید مستقل از ارائه‌دهنده باقی بماند.
- تولید امبدینگ باید مستقل از ارائه‌دهنده باقی بماند.

---

# بیانیه پیشنهاد (Recommendation Statement)

بنابراین شورای بازنگری معماری موارد زیر را پیشنهاد می‌کند:

1. Azure OpenAI به‌عنوان ارائه‌دهنده اصلی هوش مصنوعی سازمانی.
2. معماری ارائه‌دهنده هوش مصنوعی ترکیبی (Hybrid AI Provider Architecture) به‌عنوان استراتژی معماری مصوب.
3. OpenAI به‌عنوان ارائه‌دهنده ابری ثانویه و اختیاری.
4. Ollama به‌عنوان ارائه‌دهنده مصوب استنتاج محلی.

این پیشنهاد موارد زیر را به حداکثر می‌رساند:

- انسجام معماری؛
- امنیت سازمانی؛
- قابلیت نگهداری بلندمدت؛
- انعطاف‌پذیری ارائه‌دهندگان؛
- تکامل آینده هوش مصنوعی.

---

# 14. تصمیم نهایی (Final Decision)

## معماری مصوب (Approved Architecture)

معماری زیر تصویب شده است:

```text
                     Application Layer

                            │

                            ▼

                     IAIProvider

                            │

        ┌───────────────────┼───────────────────┐

        ▼                   ▼                   ▼

 Azure OpenAI          OpenAI              Ollama

 Primary             Secondary            Local

```

---

## تصمیمات فناوری (Technology Decisions)

| فناوری | تصمیم | وضعیت |
|------------|----------|--------|
| Azure OpenAI | تصویب شد (Approved) | ✅ |
| OpenAI | پشتیبانی می‌شود (Supported) | ✅ |
| Ollama | پشتیبانی می‌شود (Supported) | ✅ |
| Hybrid AI Strategy | تصویب شد (Approved) | ✅ |

---

## استراتژی پیاده‌سازی (Implementation Strategy)

فاز ۱ (Phase 1):

- Azure OpenAI

فاز ۲ (Phase 2):

- Azure OpenAI
- OpenAI

فاز ۳ (Phase 3):

- Azure OpenAI
- OpenAI
- Ollama

انتزاع ارائه‌دهنده (Provider abstraction) باید از اولین پیاده‌سازی وجود داشته باشد حتی اگر در ابتدا تنها یک ارائه‌دهنده پیکربندی شود.

---

## پیامدها (Consequences)

مثبت:

- استقلال از ارائه‌دهنده
- امنیت سازمانی
- قابلیت استقرار ترکیبی
- توسعه‌پذیری آینده
- انطباق با معماری تمیز

منفی:

- پیچیدگی پیاده‌سازی اندکی بالاتر
- لایه انتزاعی اضافی
- نیازمندی‌های آزمون برای چندین ارائه‌دهنده

---

# خلاصه تصمیم (Decision Summary)

- ✔ تصمیمات معماری تمیز (Clean Architecture Decisions)
- ✔ سازگاری با .NET 10
- ✔ انطباق با استانداردها (Standards Compliance)
- ✔ بی‌طرفی ابری (Cloud Neutrality)
- ✔ آمادگی هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت (Long-term Maintainability)

---

# سوابق تصمیمات معماری مرتبط (Related ADR)

پیاده‌سازی این ارزیابی فناوری نیازمند سند زیر است:

- ADR-0023 — استراتژی ارائه‌دهنده هوش مصنوعی (Artificial Intelligence Provider Strategy)

---

# اسناد مرتبط (Related Documents)

- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md

---

# 15. تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---------|------------|--------------------|------------------------------------------------|
| 1.0.0 | 2026-07-28 | معمار راهکار | نسخه اولیه |
| 1.1.0 | 2026-07-28 | معمار راهکار | تبدیل جداول رتبه‌بندی ستاره‌ای به رتبه‌بندی متنی جهت هماهنگی |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازنگری و همگام‌سازی با آخرین تغییرات |