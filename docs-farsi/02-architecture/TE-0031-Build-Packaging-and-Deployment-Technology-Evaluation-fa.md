| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0031 |
| **عنوان** | ارزیابی فناوری ساخت، بسته‌بندی و استقرار (Build, Packaging and Deployment Technology Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-28 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

# هدف (Purpose)

این ارزیابی فناوری پشته فناوری ساخت، بسته‌بندی و استقرار را برای MachineryManagerEnterprise تعیین می‌کند.

فناوری‌های انتخاب‌شده باید موارد زیر را پشتیبانی نمایند:

- اتوماسیون ساخت سازمانی (Enterprise Build Automation)
- یکپارچه‌سازی مداوم (Continuous Integration)
- تحویل مداوم (Continuous Delivery)
- کانتینری‌سازی (Containerization)
- توسعه محلی (Local Development)
- استقرار ابری (Cloud Deployment)
- استقرار ترکیبی (Hybrid Deployment)
- یکنواختی و ثبات زیرساخت (Infrastructure Consistency)
- قابلیت نگهداری بلندمدت (Long-Term Maintainability)

---

# دامنه ارزیابی (Evaluation Scope)

این ارزیابی فناوری موارد زیر را ارزیابی می‌کند:

- .NET 10 SDK
- Docker
- .NET Aspire
- GitHub Actions
- Azure DevOps

این سند موارد زیر را تعریف **نمی‌کند**:

- استراتژی انتشار (Release Strategy)
- توپولوژی محیط‌ها (Environment Topology)
- تأمین زیرساخت (Infrastructure Provisioning)
- سیاست نسخه‌گذاری (Versioning Policy)
- استراتژی شاخه‌بندی (Branching Strategy)

این تصمیمات معماری به‌صورت جداگانه در ADR مربوطه مستند خواهند شد.

---

# رابطه با ADRهای مرتبط (Relationship with Related ADRs)

این ارزیابی فناوری از موارد زیر پشتیبانی می‌کند:

- ADR-0025 — معماری ساخت و استقرار (Build & Deployment Architecture)

همچنین به موارد زیر وابسته است:

- معماری تمیز (Clean Architecture)
- ساختار سلوشن (Solution Structure)
- قواعد وابستگی (Dependency Rules)
- استراتژی آزمون (Testing Strategy)

---

# مراجع معماری (Architectural References)

این ارزیابی بر پایه موارد زیر استوار است:

- معماری تمیز (Clean Architecture)
- استراتژی استقرار ترکیبی (Hybrid Deployment Strategy)
- اصول DevOps سازمانی (Enterprise DevOps Principles)
- بهترین شیوه‌های CI/CD (CI/CD Best Practices)
- آمادگی زیرساخت به‌صورت کد (Infrastructure as Code Readiness)

---

# دامنه (Scope)

فناوری‌های زیر مورد ارزیابی قرار می‌گیرند:

- .NET 10 SDK
- Docker
- .NET Aspire
- GitHub Actions
- Azure DevOps

---

# معماری فعلی فرآیند ساخت (Current Build Architecture)

معماری مصوب نیازمند یک خط لوله ساخت یکپارچه است که توانایی پشتیبانی از موارد زیر را داشته باشد:

- ساخت‌های محلی توسعه‌دهنده
- اعتبارسنجی خودکار
- استقرار کانتینری
- استقرار ابری
- استقرار ترکیبی

```text
Developer

      │

      ▼

Build

      │

      ▼

Test

      │

      ▼

Package

      │

      ▼

Deploy
```

---

# نیازمندی‌های کارکردی (Functional Requirements)

پلتفرم ساخت باید موارد زیر را پشتیبانی کند:

- ساخت سلوشن (Solution Build)
- ساخت افزایشی (Incremental Build)
- آزمون خودکار (Automated Testing)
- بسته‌بندی آرتیفکت‌ها (Artifact Packaging)
- تولید ایمیج کانتینر (Container Image Generation)
- استقرار در محیط‌های چندگانه (Multi-Environment Deployment)
- اتوماسیون خط لوله (Pipeline Automation)
- نسخه‌های منتشرشده دارای نسخه (Versioned Releases)

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

فناوری‌های ساخت باید موارد زیر را فراهم آورند:

- قابلیت اطمینان بالا (High Reliability)
- ساخت‌های بازتولیدپذیر (Reproducible Builds)
- پشتیبانی چندسکویی (Cross Platform Support)
- مقیاس‌پذیری سازمانی (Enterprise Scalability)
- سادگی عملیاتی (Operational Simplicity)
- ابزارهای عالی (Excellent Tooling)
- پشتیبانی بلندمدت (Long-Term Support)
- سازگاری با CI/CD (CI/CD Compatibility)

---

# فناوری‌های کاندید (Candidate Technologies)

| کاندید | دسته‌بندی |
|-----------|----------|
| .NET 10 SDK | پلتفرم ساخت (Build Platform) |
| Docker | پلتفرم کانتینر (Container Platform) |
| .NET Aspire | ارکستراسیون برنامه‌های توزیع‌شده (Distributed Application Orchestration) |
| GitHub Actions | پلتفرم CI/CD (CI/CD Platform) |
| Azure DevOps | پلتفرم DevOps سازمانی (Enterprise DevOps Platform) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | اولویت |
|----|-----------|----------|
| BD-01 | آمادگی سازمانی (Enterprise Readiness) | حیاتی (Critical) |
| BD-02 | سازگاری با CI/CD (CI/CD Compatibility) | حیاتی (Critical) |
| BD-03 | چندسکویی (Cross Platform) | بالا (High) |
| BD-04 | بهره‌وری توسعه‌دهنده (Developer Productivity) | بالا (High) |
| BD-05 | سادگی عملیاتی (Operational Simplicity) | بالا (High) |
| BD-06 | انعطاف‌پذیری استقرار (Deployment Flexibility) | بالا (High) |
| BD-07 | قابلیت نگهداری بلندمدت (Long-Term Maintainability) | بالا (High) |
| BD-08 | یکپارچگی با اکوسیستم مایکروسافت (Microsoft Ecosystem Integration) | متوسط (Medium) |
| BD-09 | پشتیبانی جامعه کاربری (Community Support) | متوسط (Medium) |
| BD-10 | مقیاس‌پذیری آینده (Future Scalability) | بالا (High) |

---

# اصل معماری (Architecture Principle)

مؤلفه ارزیابی‌شده به‌عنوان یک سرویس زیرساختی ایزوله عمل می‌کند و کاملاً از وابستگی‌های لایه‌ای معماری تمیز و قواعد ایزولاسیون دامنه پیروی می‌نماید.

---

# 8. ارزیابی .NET 10 SDK (.NET 10 SDK Evaluation)

## نمای کلی (Overview)

کیت توسعه نرم‌افزار .NET 10 SDK کیت رسمی مایکروسافت برای ساخت، آزمون، انتشار و بسته‌بندی برنامه‌های دات‌نت است.

این کیت ابزار یکپارچه‌ای را برای موارد زیر فراهم می‌سازد:

- کامپایل سورس کد؛
- بازیابی وابستگی‌ها (Dependency restoration)؛
- آزمون؛
- بسته‌بندی؛
- انتشار؛
- تولید آرتیفکت‌ها.

برای MachineryManagerEnterprise، کیت .NET 10 SDK به‌عنوان پلتفرم بنیادین ساخت ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
        Source Code

             │

             ▼

       .NET 10 SDK

 ┌──────────────────────┐

 │ Restore              │
 │ Build                │
 │ Test                 │
 │ Publish              │
 │ Pack                 │

 └──────────────────────┘

             │

             ▼

        Build Artifacts
```

کیت SDK به ابزار معتبر و یکتای ساخت برای تمام پروژه‌های درون سلوشن تبدیل می‌شود.

---

# نقاط قوت معماری (Architectural Strengths)

مزایا شامل موارد زیر است:

- پلتفرم رسمی مایکروسافت
- ابزار یکتای زنجیره ساخت
- اجرای چندسکویی
- رابط خط فرمان (CLI) یکپارچه
- یکپارچگی بومی با MSBuild
- پشتیبانی از پروژه‌های سبک SDK
- ساخت‌های افزایشی
- پشتیبانی بلندمدت مایکروسافت

---

# قابلیت‌های کارکردی (Functional Capabilities)

کیت .NET 10 SDK از موارد زیر پشتیبانی می‌کند:

- ساخت سلوشن (Solution Build)
- ساخت افزایشی (Incremental Build)
- بازیابی بسته‌ها (Package Restore)
- مراجع پروژه (Project References)
- اجرای آزمون (Test Execution)
- انتشار (Publish)
- بسته‌بندی NuGet (NuGet Packaging)
- کامپایل AOT محلی (در صورت کاربرد)
- کامپایل چندسکویی

---

# یکپارچگی با خط لوله ساخت (Build Pipeline Integration)

جریان اجرای معمول:

```text
dotnet restore

        │

        ▼

dotnet build

        │

        ▼

dotnet test

        │

        ▼

dotnet publish
```

کیت SDK هر مرحله مورد نیاز توسط خط لوله ساخت مصوب را فراهم می‌سازد.

---

# پشتیبانی چندسکویی (Cross-Platform Support)

سیستم‌عامل‌های پشتیبانی‌شده:

| پلتفرم | پشتیبانی |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

همان دستورات CLI به‌صورت یکنواخت در تمام محیط‌های پشتیبانی‌شده اجرا می‌شوند.

---

# کارایی و عملکرد (Performance)

کیت .NET 10 SDK موارد زیر را فراهم می‌کند:

- کامپایل افزایشی
- خط لوله بهینه‌شده MSBuild
- کامپایل موازی پروژه‌ها
- بازیابی کارآمد وابستگی‌ها

عملکرد ساخت در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# سازگاری با CI/CD (CI/CD Compatibility)

کیت SDK مستقیماً با موارد زیر یکپارچه می‌شود:

- GitHub Actions
- Azure DevOps
- Jenkins
- TeamCity
- Docker
- سرورهای ساخت محلی

هیچ ابزار ساخت اضافی مورد نیاز نیست.

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- رابط CLI یکپارچه
- یکپارچگی با Visual Studio
- یکپارچگی با Rider
- پشتیبانی از VS Code
- ابزارهای عیب‌یابی غنی
- مستندات عالی

تجربه توسعه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# پشتیبانی از بسته‌بندی (Packaging Support)

خروجی‌های پشتیبانی‌شده شامل موارد زیر است:

- فایل‌های اجرایی (Executables)
- کتابخانه‌ها (Libraries)
- بسته‌های NuGet
- استقرارهای مستقل و خودکفا (Self-contained Deployments)
- برنامه‌های تک‌فایلی (Single-file Applications)
- انتشار آماده برای اجرا (Ready-to-run Publishing)

---

# تناسب سازمانی (Enterprise Suitability)

کیت .NET 10 SDK برای موارد زیر مناسب است:

- برنامه‌های سازمانی
- سلوشن‌های ماژولار
- مخازن بزرگ سورس کد
- یکپارچه‌سازی مداوم
- استقرار خودکار
- نگهداری بلندمدت

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) |
| چندسکویی (Cross Platform) | عالی (Excellent) |
| بهره‌وری توسعه‌دهنده (Developer Productivity) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) |
| عملکرد ساخت (Build Performance) | عالی (Excellent) |
| مستندات (Documentation) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) |

---

# مزایا (Advantages)

- پلتفرم رسمی ساخت مایکروسافت
- اکوسیستم بالغ
- ابزارهای عالی
- پشتیبانی قوی از اتوماسیون
- یکپارچگی بومی با اکوسیستم دات‌نت

---

# معایب (Disadvantages)

- نیازمند نصب SDK بر روی عامل‌های ساخت (Build agents)
- مدیریت نسخه باید در تمام محیط‌ها یکنواخت باقی بماند

این ملاحظات عملیاتی هستند و نه محدودیت‌های معماری.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

کیت .NET 10 SDK نیازمندی‌های پلتفرم ساخت MachineryManagerEnterprise را کاملاً برآورده می‌سازد.

این کیت به‌عنوان ابزار رسمی زنجیره ساخت برای تمامی فعالیت‌های توسعه، آزمون، بسته‌بندی و انتشار تصویب می‌شود.

---


# 9. ارزیابی Docker (Docker Evaluation)

## نمای کلی (Overview)

داکر (Docker) پلتفرم استاندارد صنعتی کانتینری‌سازی برای بسته‌بندی، توزیع و اجرای برنامه‌ها در محیط‌های ایزوله و بازتولیدپذیر است.

در MachineryManagerEnterprise، داکر به‌عنوان فناوری اصلی برای موارد زیر ارزیابی می‌شود:

- بسته‌بندی برنامه؛
- یکنواختی استقرار؛
- زیرساخت آزمون یکپارچگی؛
- اجرای CI/CD؛
- استانداردسازی محیط‌ها.

داکر مدل استقرار برنامه را جایگزین **نمی‌کند**؛ بلکه محیط اجرا را استاندارد می‌سازد.

---

# نقش معماری (Architectural Role)

```text
          Application

               │

               ▼

        dotnet publish

               │

               ▼

          Docker Image

               │

       ┌────────┼────────┐

       ▼        ▼        ▼

  Development  Testing  Production
```

داکر یک محیط اجرای یکنواخت را در تمام محیط‌ها فراهم می‌آورد.

---

# نقاط قوت معماری (Architectural Strengths)

مزایا شامل موارد زیر است:

- یکنواختی محیط
- آرتیفکت‌های استقرار تغییرناپذیر (Immutable deployment artifacts)
- قابلیت جابجایی زیرساخت (Infrastructure portability)
- ایزولاسیون
- ایمیج‌های دارای نسخه
- پشتیبانی گسترده در اکوسیستم
- یکپارچگی عالی با CI/CD
- ابزارهای بالغ

---

# قابلیت‌های کارکردی (Functional Capabilities)

داکر از موارد زیر پشتیبانی می‌کند:

- ایجاد ایمیج کانتینر
- ساخت‌های چندمرحله‌ای (Multi-stage Builds)
- نسخه‌گذاری ایمیج‌ها
- ایزولاسیون زمان اجرا
- شبکه‌بندی کانتینرها
- مدیریت Volumeها
- پیکربندی محیط
- یکپارچگی با Registry

---

# پشتیبانی از ساخت چندمرحله‌ای (Multi-Stage Build Support)

داکر ایمیج‌های بهینه‌شده عملیاتی را امکان‌پذیر می‌سازد.

جریان کاری معمول:

```dockerfile
SDK Image

      │

Build

      │

Publish

      ▼

Runtime Image
```

مزایا شامل موارد زیر است:

- اندازه کوچک‌تر ایمیج؛
- کاهش سطح حملات امنیتی؛
- استقرار سریع‌تر.

---

# یکنواختی محیط (Environment Consistency)

داکر تضمین می‌کند که:

- توسعه محلی؛
- آزمون خودکار؛
- استقرار عملیاتی

با استفاده از همان پیکربندی زمان اجرا اجرا می‌شوند.

این امر نقص‌های مختص محیط را به‌طور چشمگیری کاهش می‌دهد.

---

# پشتیبانی چندسکویی (Cross-Platform Support)

میزبان‌های پشتیبانی‌شده:

| پلتفرم | پشتیبانی |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

ایمیج‌های کانتینر در سراسر سیستم‌عامل‌های پشتیبانی‌شده قابل‌انتقال باقی می‌مانند.

---

# کارایی و عملکرد (Performance)

داکر ویژگی‌های زیر را ارائه می‌دهد:

- ایزولاسیون سبک‌وزن؛
- راه‌اندازی سریع؛
- بهره‌برداری کارآمد از منابع؛
- حداقل بار اضافی در زمان اجرا.

عملکرد در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# سازگاری با CI/CD (CI/CD Compatibility)

داکر مستقیماً با موارد زیر یکپارچه می‌شود:

- GitHub Actions
- Azure DevOps
- Docker Hub
- Azure Container Registry
- GitHub Container Registry

ایجاد ایمیج می‌تواند کاملاً خودکار شود.

---

# امنیت (Security)

داکر از موارد زیر پشتیبانی می‌کند:

- امضای ایمیج‌ها (Image signing)؛
- اسکن ایمیج‌ها؛
- ایمیج‌های حداقلی زمان اجرا؛
- ایزولاسیون کانتینرها؛
- اجرای بدون دسترسی ریشه (Non-root execution).

امنیت در صورت رعایت بهترین شیوه‌های سازمانی در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- رابط خط فرمان ساده؛
- یکپارچگی با Visual Studio؛
- یکپارچگی با VS Code؛
- محیط‌های محلی بازتولیدپذیر؛
- مستندات جامع.

تجربه توسعه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تناسب سازمانی (Enterprise Suitability)

داکر برای موارد زیر مناسب است:

- توسعه
- آزمون یکپارچگی
- یکپارچه‌سازی مداوم
- بسته‌بندی
- استقرار
- زیرساخت ترکیبی

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) |
| چندسکویی (Cross Platform) | عالی (Excellent) |
| بهره‌وری توسعه‌دهنده (Developer Productivity) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) |
| انعطاف‌پذیری استقرار (Deployment Flexibility) | عالی (Excellent) |
| امنیت (Security) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) |

---

# مزایا (Advantages)

- استاندارد صنعت
- اکوسیستم بالغ
- آرتیفکت‌های استقرار قابل جابجایی
- پشتیبانی قوی مایکروسافت
- یکپارچگی عالی با DevOps

---

# معایب (Disadvantages)

- لایه کانتینر اضافی
- نیازمند محیط اجرای کانتینر (Container runtime) بر روی میزبان‌های استقرار

این نیازمندی‌های عملیاتی برای معماری مصوب استقرار کاملاً قابل قبول هستند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

داکر نیازمندی‌های بسته‌بندی و استقرار MachineryManagerEnterprise را کاملاً برآورده می‌سازد.

این فناوری به‌عنوان پلتفرم استاندارد کانتینری‌سازی برای بسته‌بندی برنامه، یکنواختی زیرساخت و استقرار سازمانی تصویب می‌شود.

---


# 10. ارزیابی .NET Aspire (.NET Aspire Evaluation)

## نمای کلی (Overview)

پلتفرم .NET Aspire فریم‌ورک اختصاصی و خوش‌ساخت مایکروسافت برای ساخت، ارکستراسیون و بهره‌برداری از برنامه‌های توزیع‌شده مدرن دات‌نت است.

این ابزار یک تجربه توسعه‌دهنده یکپارچه را برای مدیریت چندین سرویس، منابع زیرساختی و وابستگی‌های برنامه در طول توسعه و استقرار فراهم می‌کند.

در MachineryManagerEnterprise، ابزار Aspire به‌عنوان یک پلتفرم بالقوه ارکستراسیون و نه به‌عنوان یک فریم‌ورک برنامه ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
                Aspire AppHost

                      │

      ┌───────────────┼────────────────┐

      ▼               ▼                ▼

 Application      SQL Server       RabbitMQ

      ▼               ▼                ▼

     Redis         Qdrant         Observability
```

ابزار Aspire منابع توزیع‌شده را هماهنگ می‌کند اما جایگزین معماری برنامه نمی‌شود.

---

# نقاط قوت معماری (Architectural Strengths)

مزایا شامل موارد زیر است:

- ارکستراسیون محلی یکپارچه
- یکپارچگی قوی با اکوسیستم مایکروسافت
- مدل برنامه توزیع‌شده
- کشف سرویس توکار (Built-in service discovery)
- پیکربندی محیط
- مدیریت وابستگی منابع
- بهره‌وری توسعه‌دهنده
- ابزارهای عیب‌یابی مدرن

---

# قابلیت‌های کارکردی (Functional Capabilities)

ابزار Aspire از موارد زیر پشتیبانی می‌کند:

- ارکستراسیون محلی
- ثبت سرویس‌ها
- تأمین منابع
- مدیریت پیکربندی
- پایش سلامت سرویس‌ها
- عیب‌یابی توزیع‌شده
- یکپارچگی با OpenTelemetry
- پشتیبانی از داشبورد

---

# تجربه توسعه (Development Experience)

ابزار Aspire تجربه توسعه‌دهنده محلی را به‌طور قابل توجهی بهبود می‌بخشد.

جریان کاری معمول:

```text
Start AppHost

      │

Automatically Start

      │

SQL Server

RabbitMQ

Redis

Qdrant

Application

      │

Ready
```

توسعه‌دهندگان دیگر نیازی به راه‌اندازی دستی تک‌تک وابستگی‌ها ندارند.

---

# پشتیبانی از برنامه‌های توزیع‌شده (Distributed Application Support)

ابزار Aspire به‌ویژه برای برنامه‌هایی ارزشمند است که شامل موارد زیر هستند:

- چندین سرویس
- منابع زیرساختی
- ارتباطات توزیع‌شده
- مؤلفه‌های مبتنی بر رویداد

سامانه MachineryManagerEnterprise در حال حاضر شامل موارد زیر است:

- SQL Server
- RabbitMQ
- Redis
- Qdrant
- سرویس‌های پس‌زمینه (Background Services)
- چندین ماژول تجاری

این موارد به‌خوبی با کاربرد مورد نظر Aspire همخوانی دارد.

---

# مشاهده‌پذیری (Observability)

ابزار Aspire پشتیبانی توکاری برای موارد زیر فراهم می‌کند:

- OpenTelemetry
- ردگیری توزیع‌شده (Distributed Tracing)
- لاگ‌گیری ساخت‌یافته (Structured Logging)
- متریک‌ها (Metrics)
- بررسی‌های سلامت (Health Checks)
- داشبورد مرکزی

مشاهده‌پذیری در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# مدیریت پیکربندی (Configuration Management)

ابزار Aspire پیکربندی موارد زیر را متمرکز می‌سازد:

- رشته‌های اتصال (Connection Strings)
- کشف سرویس (Service Discovery)
- متغیرهای محیطی
- وابستگی‌های منابع

پیچیدگی پیکربندی به‌طور قابل توجهی کاهش می‌یابد.

---

# سازگاری با CI/CD (CI/CD Compatibility)

ابزار Aspire به‌خوبی با موارد زیر یکپارچه می‌شود:

- GitHub Actions
- Azure DevOps
- Docker
- Kubernetes (آینده)
- Azure Container Apps

سازگاری با خط لوله در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# پشتیبانی چندسکویی (Cross-Platform Support)

پلتفرم‌های پشتیبانی‌شده:

| پلتفرم | پشتیبانی |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

---

# کارایی و عملکرد (Performance)

ابزار Aspire بار اضافی حداقلی در زمان اجرا ایجاد می‌کند زیرا مسئولیت‌های اصلی آن ارکستراسیون و پیکربندی است.

تأثیر بر عملکرد **ناچیز (Negligible)** در نظر گرفته می‌شود.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

ابزار Aspire عمدتاً موارد زیر را هدف قرار می‌دهد:

- توسعه محلی؛
- محیط‌های یکپارچگی؛
- ارکستراسیون بومی ابری.

استقرار عملیاتی با زیرساخت استاندارد مبتنی بر داکر سازگار باقی می‌ماند.

---

# تناسب سازمانی (Enterprise Suitability)

ابزار Aspire برای موارد زیر مناسب است:

- برنامه‌های توزیع‌شده سازمانی
- معماری‌های مبتنی بر رویداد
- استقرارهای ترکیبی
- سیستم‌های ماژولار
- بهره‌وری توسعه‌دهنده

---

# محدودیت‌ها (Limitations)

محدودیت‌های فعلی شامل موارد زیر است:

- لایه ارکستراسیون اضافی
- فناوری نسبتاً جدید مایکروسافت
- مناسب‌ترین گزینه برای سیستم‌های توزیع‌شده
- نیازمند آشنایی توسعه‌دهندگان

این محدودیت‌ها با توجه به معماری MachineryManagerEnterprise قابل قبول هستند.

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| بهره‌وری توسعه‌دهنده (Developer Productivity) | عالی (Excellent) |
| پشتیبانی از برنامه توزیع‌شده | عالی (Excellent) |
| مشاهده‌پذیری (Observability) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) |
| چندسکویی (Cross Platform) | عالی (Excellent) |
| عملکرد (Performance) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) |

---

# مزایا (Advantages)

- راهکار بومی مایکروسافت
- تجربه توسعه‌دهنده عالی
- ارکستراسیون یکپارچه
- مشاهده‌پذیری قوی
- مدیریت ساده‌شده زیرساخت محلی

---

# معایب (Disadvantages)

- انتزاع ارکستراسیون اضافی
- منحنی یادگیری برای تیم‌های توسعه
- برای برنامه‌های بسیار کوچک مورد نیاز نیست

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

سامانه MachineryManagerEnterprise یک برنامه سازمانی توزیع‌شده با چندین وابستگی زیرساختی است.

فناوری .NET Aspire فوق‌العاده با این معماری همسو است.

بنابراین به‌عنوان پلتفرم ترجیحی ارکستراسیون برای توسعه محلی و مدیریت برنامه‌های توزیع‌شده تصویب می‌شود.

---


# 11. ارزیابی GitHub Actions (GitHub Actions Evaluation)

## نمای کلی (Overview)

گیت‌هاب اکشنز (GitHub Actions) پلتفرم بومی یکپارچه‌سازی مداوم و تحویل مداوم (CI/CD) گیت‌هاب است.

این ابزار اجرای خودکار موارد زیر را امکان‌پذیر می‌سازد:

- ساخت (Build)
- آزمون (Test)
- بسته‌بندی (Packaging)
- اعتبارسنجی امنیت (Security Validation)
- انتشار آرتیفکت‌ها (Artifact Publishing)
- استقرار (Deployment)

مستقیماً از مخزن سورس کد.

در MachineryManagerEnterprise، پلتفرم GitHub Actions به‌عنوان پلتفرم اصلی CI ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
            Git Push / Pull Request

                     │

                     ▼

               GitHub Actions

         ┌──────────────────────────┐

         │ Restore                  │
         │ Build                    │
         │ Test                     │
         │ Package                  │
         │ Publish                  │
         │ Release                  │
         └──────────────────────────┘

                     │

                     ▼

               Deployment Artifacts
```

ابزار GitHub Actions به موتور اتوماسیون مسئول اعتبارسنجی هر تغییر کد پیش از رسیدن به محیط عملیاتی تبدیل می‌شود.

---

# نقاط قوت معماری (Architectural Strengths)

مزایا شامل موارد زیر است:

- یکپارچگی بومی با GitHub
- خطوط لوله مبتنی بر رویداد (Event-driven pipelines)
- تعریف گردش کار بر مبنای YAML
- رانرهای چندسکویی (Cross-platform runners)
- اکوسیستم Marketplace
- مدیریت اسرار و کلیدها
- ساخت‌های ماتریسی (Matrix builds)
- مدیریت آرتیفکت‌ها

---

# قابلیت‌های کارکردی (Functional Capabilities)

ابزار GitHub Actions از موارد زیر پشتیبانی می‌کند:

- یکپارچه‌سازی مداوم
- تحویل مداوم
- اعتبارسنجی Pull Request
- کارهای زمان‌بندی‌شده (Scheduled Jobs)
- اجرای دستی گردش کار
- آپلود آرتیفکت
- انتشار بسته‌ها
- ساخت ایمیج کانتینر
- استقرار محیط

---

# یکپارچگی گردش کار (Workflow Integration)

جریان اجرای معمول:

```text
Pull Request

      │

      ▼

Restore

      │

      ▼

Build

      │

      ▼

Unit Tests

      │

      ▼

Integration Tests

      │

      ▼

Package

      │

      ▼

Publish Artifact
```

هر گردش کار بر اساس رویدادهای مخزن به‌صورت خودکار اجرا می‌شود.

---

# پشتیبانی از رانرها (Runner Support)

محیط‌های اجرایی پشتیبانی‌شده:

| رانر | پشتیبانی |
|---------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |
| Self-Hosted | ✅ |

این امر امکان می‌دهد خطوط لوله به‌صورت یکنواخت در محیط‌های توسعه و عملیات اجرا شوند.

---

# امنیت (Security)

ابزار GitHub Actions موارد زیر را فراهم می‌کند:

- اسرار مخزن (Repository Secrets)
- اسرار محیط (Environment Secrets)
- محیط‌های حفاظت‌شده
- یکپارچگی با حفاظت از شاخه‌ها
- احراز هویت OIDC
- اجرای امضاشده گردش کار

امنیت در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# سازگاری با CI/CD (CI/CD Compatibility)

ابزار GitHub Actions مستقیماً با موارد زیر یکپارچه می‌شود:

- Docker
- Azure Container Registry
- GitHub Container Registry
- NuGet
- .NET CLI
- Testcontainers
- Playwright

سازگاری با خط لوله در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- گردش کار به‌صورت کد (Workflow as Code)
- رابط کاربری یکپارچه مخزن
- Marketplace غنی
- مستندات عالی
- جامعه کاربری بزرگ

تجربه توسعه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تناسب سازمانی (Enterprise Suitability)

ابزار GitHub Actions برای موارد زیر مناسب است:

- CI سازمانی
- اعتبارسنجی خودکار
- اتوماسیون انتشار
- انتشار بسته‌ها
- ایجاد ایمیج کانتینر
- اتوماسیون زیرساخت

---

# کارایی و عملکرد (Performance)

ابزار GitHub Actions ویژگی‌های زیر را ارائه می‌دهد:

- کارهای موازی (Parallel Jobs)
- ساخت‌های ماتریسی
- اجرای افزایشی
- کش کردن ساخت (Build Caching)
- کش کردن وابستگی‌ها

عملکرد در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

تلاش عملیاتی حداقل است.

گیت‌هاب موارد زیر را مدیریت می‌کند:

- رانرهای میزبانی‌شده (Hosted runners)
- زمان‌بندی گردش کار
- اجرای خط لوله
- ذخیره‌سازی لاگ‌ها
- ذخیره‌سازی آرتیفکت‌ها

پیچیدگی عملیاتی در سطح **بسیار پایین (Very Low)** ارزیابی می‌شود.

---

# مزایا (Advantages)

- یکپارچگی بومی با GitHub
- پشتیبانی عالی از دات‌نت
- اکوسیستم اتوماسیون غنی
- نسخه‌گذاری گردش کار
- پذیرش قوی جامعه کاربری

---

# معایب (Disadvantages)

- محدودیت‌های رانر میزبانی‌شده بر اساس لایسنس
- حاکمیت سازمانی سنگین ممکن است نیازمند رانرهای خودمیزبان باشد

این محدودیت‌ها عملیاتی هستند و نه معماری.

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) |
| بهره‌وری توسعه‌دهنده (Developer Productivity) | عالی (Excellent) |
| چندسکویی (Cross Platform) | عالی (Excellent) |
| عملکرد (Performance) | عالی (Excellent) |
| امنیت (Security) | عالی (Excellent) |
| مستندات (Documentation) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

ابزار GitHub Actions نیازمندی‌های یکپارچه‌سازی مداوم MachineryManagerEnterprise را کاملاً برآورده می‌سازد.

این ابزار به‌عنوان پلتفرم ترجیحی اتوماسیون CI برای اعتبارسنجی ساخت، آزمون خودکار، تولید آرتیفکت‌ها و آماده‌سازی انتشار تصویب می‌شود.

---


# 12. ارزیابی Azure DevOps (Azure DevOps Evaluation)

## نمای کلی (Overview)

پلتفرم Azure DevOps پلتفرم DevOps سازمانی مایکروسافت است که سرویس‌های یکپارچه‌ای را برای موارد زیر ارائه می‌دهد:

- کنترل سورس کد (Source Control)
- یکپارچه‌سازی مداوم (CI)
- تحویل مداوم (CD)
- مدیریت اقلام کاری (Work Item Management)
- مدیریت آزمون (Test Management)
- مدیریت آرتیفکت‌ها
- مدیریت انتشار (Release Management)

برخلاف GitHub Actions، پلتفرم Azure DevOps یک پلتفرم کامل مدیریت چرخه حیات برنامه (ALM) است و نه صرفاً یک موتور CI/CD مستقل.

در MachineryManagerEnterprise، پلتفرم Azure DevOps به‌عنوان یک جایگزین DevOps سازمانی ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
              Azure DevOps

      ┌──────────────────────────┐

      │ Azure Repos              │
      │ Azure Pipelines          │
      │ Azure Artifacts          │
      │ Azure Boards             │
      │ Azure Test Plans         │
      └──────────────────────────┘

                 │

                 ▼

         Build / Test / Deploy
```

پلتفرم Azure DevOps یک پلتفرم تحویل نرم‌افزار سازمانی سرتاسری را فراهم می‌سازد.

---

# نقاط قوت معماری (Architectural Strengths)

مزایا شامل موارد زیر است:

- پلتفرم ALM سازمانی
- قابلیت‌های پیشرفته خط لوله
- مدیریت غنی انتشار
- حاکمیت قوی
- امنیت سازمانی
- مجوزهای دقیق و ریزدانه‌ای
- یکپارچگی با اکوسیستم مایکروسافت
- ابزارهای سازمانی بالغ

---

# قابلیت‌های کارکردی (Functional Capabilities)

پلتفرم Azure DevOps از موارد زیر پشتیبانی می‌کند:

- خطوط لوله CI/CD
- استقرارهای چندمرحله‌ای
- خطوط لوله انتشار
- مدیریت آزمون
- مدیریت آرتیفکت‌ها
- گردش کارهای تأیید (Approval Workflows)
- مدیریت محیط‌ها
- استقرار زیرساخت

---

# قابلیت‌های خط لوله (Pipeline Capabilities)

پلتفرم Azure DevOps از موارد زیر پشتیبانی می‌کند:

```text
Restore

   │

Build

   │

Test

   │

Package

   │

Release

   │

Production
```

این پلتفرم ارکستراسیون پیشرفته استقرار را همراه با تأییدیه‌ها و گیت‌های انتشار ارائه می‌دهد.

---

# حاکمیت سازمانی (Enterprise Governance)

پلتفرم Azure DevOps موارد زیر را فراهم می‌کند:

- سیاست‌های شاخه‌ها (Branch Policies)
- گیت‌های تأیید (Approval Gates)
- حفاظت از محیط‌ها
- ردپای ممیزی (Audit Trails)
- کنترل دسترسی نقش‌محور سازمانی (Enterprise RBAC)
- مجوزهای انتشار

حاکمیت در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# امنیت (Security)

پلتفرم Azure DevOps از موارد زیر پشتیبانی می‌کند:

- Azure Active Directory
- هویت‌های مدیریت‌شده (Managed Identities)
- متغیرهای محرمانه (Secret Variables)
- فایل‌های امن (Secure Files)
- گروه‌های متغیر (Variable Groups)
- ایزولاسیون محیط‌ها

امنیت در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# سازگاری با CI/CD (CI/CD Compatibility)

پلتفرم Azure DevOps مستقیماً با موارد زیر یکپارچه می‌شود:

- .NET SDK
- Docker
- Aspire
- Azure
- Kubernetes
- SQL Server
- GitHub

سازگاری در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- ویرایشگر غنی خط لوله
- خطوط لوله YAML
- خطوط لوله Classic
- داشبوردهای سازمانی
- مستندات جامع مایکروسافت

تجربه توسعه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

پلتفرم Azure DevOps موارد زیر را فراهم می‌کند:

- عامل‌های میزبانی‌شده (Hosted agents)
- عامل‌های خودمیزبان (Self-hosted agents)
- کش کردن خط لوله
- اجرای موازی
- پایش سازمانی

پیچیدگی عملیاتی در سطح **پایین (Low)** ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

پلتفرم Azure DevOps موارد زیر را فراهم می‌کند:

- عامل‌های موازی
- اجرای افزایشی
- کش کردن آرتیفکت‌ها
- اجرای توزیع‌شده

عملکرد در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تناسب سازمانی (Enterprise Suitability)

پلتفرم Azure DevOps برای موارد زیر مناسب است:

- سازمان‌های بزرگ
- محیط‌های تحت مقررات و استانداردها
- فرآیندهای پیچیده انتشار
- تیم‌های بزرگ توسعه
- استقرارهای چندمرحله‌ای

---

# مقایسه با GitHub Actions (Comparison with GitHub Actions)

| معیار | GitHub Actions | Azure DevOps |
|-----------|:--------------:|:------------:|
| CI/CD | عالی (Excellent) | عالی (Excellent) |
| ویژگی‌های ALM | محدود (Limited) | عالی (Excellent) |
| مدیریت اقلام کاری | خیر (No) | بله (Yes) |
| پلن‌های آزمون (Test Plans) | خیر (No) | بله (Yes) |
| مدیریت انتشار | خوب (Good) | عالی (Excellent) |
| یکپارچگی با مخزن | بومی گیت‌هاب | بومی آژور |
| حاکمیت سازمانی | بسیار خوب (Very Good) | عالی (Excellent) |

---

# مزایا (Advantages)

- پلتفرم کامل ALM
- حاکمیت پیشرفته
- مدیریت انتشار سازمانی
- یکپارچگی عالی با مایکروسافت
- قابلیت‌های سازمانی بالغ

---

# معایب (Disadvantages)

- پیچیدگی اداری بالاتر
- پلتفرمی گسترده‌تر از نیازمندی‌های فعلی
- ملاحظات لایسنس اضافی
- منحنی یادگیری تندتر نسبت به GitHub Actions

---

# ارزیابی فناوری (Technology Assessment)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) |
| حاکمیت سازمانی (Enterprise Governance) | عالی (Excellent) |
| امنیت (Security) | عالی (Excellent) |
| عملکرد (Performance) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | بسیار خوب (Very Good) |
| مستندات (Documentation) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) |

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

پلتفرم Azure DevOps یک پلتفرم DevOps سازمانی برجسته است.

با این حال، MachineryManagerEnterprise از GitHub به‌عنوان پلتفرم اصلی کنترل سورس کد خود استفاده می‌کند.

از آنجا که GitHub Actions در حال حاضر قابلیت‌های عالی CI/CD را با پیچیدگی عملیاتی بسیار کمتر فراهم می‌کند، Azure DevOps **به‌عنوان پلتفرم اصلی CI/CD برای این پروژه پیشنهاد نمی‌شود**.

پلتفرم Azure DevOps یک جایگزین سازمانی کاملاً پشتیبانی‌شده برای سازمان‌هایی که از قبل بر روی اکوسیستم Azure DevOps استاندارد شده‌اند، باقی می‌ماند.

---

# 13. مقایسه کلی فناوری‌ها (Overall Technology Comparison)

به‌دنبال ارزیابی تفصیلی تمامی فناوری‌های کاندید، شورای بازنگری معماری پشته کامل ساخت، بسته‌بندی و استقرار را در برابر اهداف معماری MachineryManagerEnterprise مقایسه نمود.

---

# نمای کلی پشته فناوری (Technology Stack Overview)

| مسئولیت | فناوری انتخاب‌شده |
|---------------|---------------------|
| پلتفرم ساخت (Build Platform) | .NET 10 SDK |
| کانتینری‌سازی (Containerization) | Docker |
| ارکستراسیون توزیع‌شده محلی | .NET Aspire |
| یکپارچه‌سازی مداوم (CI) | GitHub Actions |
| جایگزین ALM سازمانی | Azure DevOps |

این فناوری‌ها در کنار یکدیگر یک پلتفرم کامل DevOps سازمانی را فراهم می‌سازند.

---

# ماتریس مقایسه فناوری‌ها (Technology Comparison Matrix)

| معیار | .NET 10 SDK | Docker | Aspire | GitHub Actions | Azure DevOps |
|-----------|:-----------:|:------:|:------:|:--------------:|:------------:|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| قابلیت ساخت (Build Capability) | عالی (Excellent) | خوب (Good) | متوسط (Fair) | عالی (Excellent) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | عالی (Excellent) | عالی (Excellent) |
| چندسکویی (Cross Platform) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| بهره‌وری توسعه‌دهنده (Developer Productivity) | عالی (Excellent) | خوب (Good) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| انعطاف‌پذیری استقرار (Deployment Flexibility) | خوب (Good) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) | خوب (Good) | خوب (Good) | عالی (Excellent) | خوب (Good) |
| مستندات (Documentation) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | عالی (Excellent) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |

---

# تفکیک مسئولیت‌ها (Responsibility Separation)

```text
        Build

      .NET SDK

          │

          ▼

    Containerization

        Docker

          │

          ▼

 Local Orchestration

      .NET Aspire

          │

          ▼

 Continuous Integration

    GitHub Actions

          │

          ▼

  Enterprise ALM

     Azure DevOps
```

هر فناوری مسئولیت کاملاً مشخصی با حداقل هم‌پوشانی دارد.

---

# پوشش استقرار (Deployment Coverage)

| قابلیت | فناوری |
|------------|------------|
| کامپایل | .NET 10 SDK |
| بسته‌بندی | Docker |
| اجرای توزیع‌شده محلی | Aspire |
| اتوماسیون CI | GitHub Actions |
| مدیریت انتشار سازمانی | Azure DevOps |

---

# پشتیبانی چندسکویی (Cross-Platform Support)

تمامی فناوری‌های انتخاب‌شده از موارد زیر پشتیبانی می‌کنند:

- Windows
- Linux
- macOS

این امر نیازمندی‌های توسعه چندسکویی پروژه را برآورده می‌سازد.

---

# ویژگی‌های سازمانی (Enterprise Characteristics)

| نیازمندی | پوشش |
|-------------|----------|
| ساخت‌های بازتولیدپذیر | کامل (Complete) |
| استقرار کانتینری | کامل (Complete) |
| زیرساخت محلی | کامل (Complete) |
| اعتبارسنجی خودکار | کامل (Complete) |
| استقرار ترکیبی | کامل (Complete) |
| حاکمیت سازمانی | کامل (Complete) |

---

# همسویی با اکوسیستم مایکروسافت (Microsoft Ecosystem Alignment)

پشته انتخاب‌شده به‌طور طبیعی با موارد زیر یکپارچه می‌شود:

- .NET 10
- Visual Studio
- Azure
- Docker
- GitHub
- Azure DevOps

این امر پیچیدگی یکپارچه‌سازی را به حداقل رسانده و پشتیبانی بلندمدت را به حداکثر می‌رساند.

---

# پیچیدگی عملیاتی (Operational Complexity)

```text
Lowest Complexity

.NET SDK

↓

GitHub Actions

↓

Docker

↓

Aspire

↓

Azure DevOps

Highest Complexity
```

اگرچه Azure DevOps غنی‌ترین مجموعه ویژگی‌های سازمانی را ارائه می‌دهد، اما بالاترین پیچیدگی اداری را نیز به همراه دارد.

---

# ارزیابی معماری (Architectural Assessment)

پلتفرم ساخت انتخاب‌شده کاملاً از معماری مصوب از طریق ارائه موارد زیر پشتیبانی می‌کند:

- ساخت‌های قطعی؛
- آرتیفکت‌های استقرار قابل جابجایی؛
- اعتبارسنجی خودکار؛
- مقیاس‌پذیری سازمانی؛
- قابلیت نگهداری بلندمدت.

هیچ فناوری ساخت اضافی مورد نیاز نیست.

---

# 14. پیشنهاد نهایی (Final Recommendation)

شورای بازنگری معماری اتخاذ پشته ساخت و استقرار سازمانی زیر را پیشنهاد می‌کند:

| دسته‌بندی | فناوری مصوب |
|----------|---------------------|
| پلتفرم ساخت (Build Platform) | **.NET 10 SDK** |
| کانتینری‌سازی (Containerization) | **Docker** |
| ارکستراسیون محلی (Local Orchestration) | **.NET Aspire** |
| یکپارچه‌سازی مداوم (CI) | **GitHub Actions** |
| مدیریت چرخه حیات سازمانی (ALM) | **Azure DevOps (گزینه جایگزین سازمانی اختیاری)** |

---

# مقایسه کلی فناوری‌ها (Overall Technology Comparison)

فناوری انتخاب‌شده عملکرد بهینه، قابلیت نگهداری و سازگاری با معماری تمیز را فراهم می‌سازد.

## ماتریس مسئولیت (Responsibility Matrix)

| مسئولیت | فناوری پیشنهادی | جایگزین |
|-----------------|------------------------|-------------|
| قابلیت سیستم | انتخاب اصلی (Primary Selected) | گزینه ارزیابی‌شده (Evaluated Option) |

---

# بیانیه پیشنهاد نهایی (Final Recommendation Statement)

استراتژی پیاده‌سازی پیشنهادی به شرح زیر است:

- کیت .NET 10 SDK برای کلیه فعالیت‌های ساخت.
- داکر (Docker) برای بسته‌بندی و یکنواختی استقرار.
- پلتفرم .NET Aspire برای ارکستراسیون برنامه‌های توزیع‌شده محلی.
- سامانه GitHub Actions به‌عنوان پلتفرم اصلی CI.
- سرویس Azure DevOps صرفاً در مواردی پشتیبانی می‌شود که مشتریان سازمانی نیازمند یک پلتفرم یکپارچه ALM باشند.

این ترکیب موارد زیر را فراهم می‌آورد:

- بهره‌وری عالی توسعه‌دهنده؛
- ساخت‌های بازتولیدپذیر؛
- استقرار یکنواخت و پایدار؛
- همسویی قوی با اکوسیستم مایکروسافت؛
- مقیاس‌پذیری سازمانی.

---

# 15. تصمیم نهایی (Final Decision)

## معماری مصوب (Approved Architecture)

```text
Source Code

      │

.NET 10 SDK

      │

Docker

      │

Aspire

      │

GitHub Actions

      │

Deployment
```

---

## تصمیمات فناوری (Technology Decisions)

| فناوری | تصمیم | وضعیت |
|------------|----------|--------|
| .NET 10 SDK | تصویب شد (Approved) | ✅ |
| Docker | تصویب شد (Approved) | ✅ |
| .NET Aspire | تصویب شد (Approved) | ✅ |
| GitHub Actions | تصویب شد (Approved) | ✅ |
| Azure DevOps | جایگزین پشتیبانی‌شده (Supported Alternative) | ✅ |

---

## استراتژی پیاده‌سازی (Implementation Strategy)

فاز ۱ (Phase 1):

- .NET 10 SDK
- Docker
- GitHub Actions

فاز ۲ (Phase 2):

- .NET Aspire

فاز ۳ (Phase 3):

- پشتیبانی اختیاری از Azure DevOps برای مشتریان سازمانی

---

## پیامدها (Consequences)

مثبت:

- ساخت‌های بازتولیدپذیر
- استقرار یکنواخت
- اتوماسیون عالی CI
- آمادگی برای استقرار ترکیبی
- همسویی قوی با مایکروسافت

منفی:

- نیازمندی به محیط اجرای Docker
- لایه ارکستراسیون اضافی (Aspire)
- پیچیدگی عملیاتی اختیاری در صورت اتخاذ Azure DevOps

---

## سوابق تصمیمات معماری مرتبط (Related Architecture Decision)

پیاده‌سازی این ارزیابی فناوری نیازمند سند زیر است:

- **ADR-0025 — معماری ساخت و استقرار (Build & Deployment Architecture)**

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ انطباق با استانداردها (Standards Compliance)
- ✔ بی‌طرفی ابری (Cloud Neutrality)
- ✔ آمادگی هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت (Long-term Maintainability)

---

# اسناد مرتبط (Related Documents)

- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md

---

# 16. تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---------|------------|--------------------|-------------------------------------------------------------------|
| 1.0.0 | 2026-07-28 | معمار راهکار | ارزیابی اولیه فناوری برای ساخت، بسته‌بندی و استقرار |
| 1.1.0 | 2026-07-28 | معمار راهکار | حذف خط تکراری عنوان؛ تبدیل جداول رتبه‌بندی ستاره‌ای به رتبه‌بندی متنی برای هماهنگی |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازنگری و همگام‌سازی با آخرین تغییرات |