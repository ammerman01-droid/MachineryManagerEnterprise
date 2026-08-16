| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0030 |
| **عنوان** | ارزیابی فناوری آزمون (Testing Technology Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-28 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

# هدف (Purpose)

این ارزیابی فناوری پشته فناوری آزمون را برای MachineryManagerEnterprise تعیین می‌کند.

فناوری‌های انتخاب‌شده باید یک استراتژی جامع آزمون را فراهم سازند که موارد زیر را پوشش دهد:

- آزمون واحد (Unit Testing)
- آزمون یکپارچگی (Integration Testing)
- آزمون قرارداد (Contract Testing)
- آزمون زیرساخت (Infrastructure Testing)
- آزمون سرتاسری (End-to-End Testing)
- اتوماسیون رابط کاربری (UI Automation)
- آزمون رگرسیون (Regression Testing)
- اعتبارسنجی در یکپارچه‌سازی مداوم (Continuous Integration Validation)
- راستی‌آزمایی عملکرد (Performance Verification)

پلتفرم آزمون باید از کیفیت نرم‌افزار در سطح سازمانی پشتیبانی کند در حالی که کاملاً با معماری تمیز مصوب سازگار باقی می‌ماند.

---

# دامنه ارزیابی (Evaluation Scope)

این ارزیابی فناوری موارد زیر را ارزیابی می‌کند:

- xUnit v3
- FluentAssertions
- NSubstitute
- Testcontainers
- Playwright

این سند موارد زیر را تعریف **نمی‌کند**:

- استراتژی آزمون (Test Strategy)
- قراردادهای نام‌گذاری آزمون (Test Naming Conventions)
- سازمان‌دهی آزمون‌ها (Test Organization)
- خط لوله CI (CI Pipeline)
- قواعد پوشش کد (Code Coverage Rules)

این تصمیمات معماری به‌صورت جداگانه در ADR مربوطه مستند خواهند شد.

---

# رابطه با ADRهای مرتبط (Relationship with Related ADRs)

این ارزیابی فناوری از موارد زیر پشتیبانی می‌کند:

- ADR-0024 — استراتژی آزمون سازمانی (Enterprise Testing Strategy) *(در انتظار)*

همچنین به موارد زیر وابسته است:

- ADRهای معماری تمیز (Clean Architecture ADRs)
- ADRهای تفکیک مسئولیت فرمان و پرس‌وجو (CQRS ADRs)
- ساختار سلوشن (Solution Structure)
- قواعد وابستگی (Dependency Rules)

---

# مراجع معماری (Architectural References)

این ارزیابی بر پایه موارد زیر استوار است:

- معماری تمیز (Clean Architecture)
- الگوی تفکیک مسئولیت فرمان و پرس‌وجو (CQRS)
- هرم آزمون (Test Pyramid)
- قابلیت نگهداری سازمانی (Enterprise Maintainability)
- اصول یکپارچه‌سازی مداوم (Continuous Integration Principles)

---

# دامنه (Scope)

فناوری‌های زیر مورد ارزیابی قرار می‌گیرند:

- xUnit v3
- FluentAssertions
- NSubstitute
- Testcontainers
- Playwright

---

# معماری فعلی آزمون (Current Testing Architecture)

معماری مصوب نیازمند آزمون در سطوح چندگانه است.

```text
                End-to-End Tests

                       ▲

                   UI Tests

                       ▲

              Integration Tests

                       ▲

                 Unit Tests
```

هر لایه معماری باید به‌صورت مستقل قابل آزمون باشد.

---

# نیازمندی‌های کارکردی (Functional Requirements)

پلتفرم آزمون باید موارد زیر را پشتیبانی کند:

- آزمون‌های واحد سریع (Fast Unit Tests)
- اعتبارسنجی‌های قطعی (Deterministic Assertions)
- شبیه‌سازی وابستگی‌ها (Mocking Dependencies)
- آزمون یکپارچگی با زیرساخت واقعی (Integration Testing with Real Infrastructure)
- اتوماسیون مرورگر (Browser Automation)
- اجرای موازی آزمون‌ها (Parallel Test Execution)
- اجرای خودکار در CI/CD (CI/CD Execution)
- اجرای چندسکویی (Cross-Platform Execution)

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

فناوری‌های آزمون باید موارد زیر را فراهم آورند:

- پایداری (Stability)
- قابلیت نگهداری (Maintainability)
- بهره‌وری توسعه‌دهنده (Developer Productivity)
- خوانایی بالا (High Readability)
- مقیاس‌پذیری سازمانی (Enterprise Scalability)
- پشتیبانی بلندمدت (Long-Term Support)
- مستندات عالی (Excellent Documentation)
- پذیرش و جامعه کاربری قوی (Strong Community Adoption)

---

# فناوری‌های کاندید (Candidate Technologies)

| کاندید | دسته‌بندی |
|-----------|----------|
| xUnit v3 | چارچوب آزمون واحد (Unit Testing Framework) |
| FluentAssertions | کتابخانه اعتبارسنجی (Assertion Library) |
| NSubstitute | چارچوب شبیه‌سازی (Mocking Framework) |
| Testcontainers | زیرساخت آزمون یکپارچگی (Integration Testing Infrastructure) |
| Playwright | چارچوب آزمون سرتاسری (End-to-End Testing Framework) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | اولویت |
|----|-----------|----------|
| TT-01 | آمادگی سازمانی (Enterprise Readiness) | حیاتی (Critical) |
| TT-02 | یکپارچگی با دات‌نت (.NET Integration) | حیاتی (Critical) |
| TT-03 | قابلیت نگهداری (Maintainability) | بالا (High) |
| TT-04 | خوانایی (Readability) | بالا (High) |
| TT-05 | کارایی و عملکرد (Performance) | بالا (High) |
| TT-06 | پشتیبانی جامعه کاربری (Community Support) | متوسط (Medium) |
| TT-07 | مستندات (Documentation) | متوسط (Medium) |
| TT-08 | سازگاری با CI/CD (CI/CD Compatibility) | بالا (High) |
| TT-09 | پشتیبانی چندسکویی (Cross Platform Support) | بالا (High) |
| TT-10 | دوام و پایداری بلندمدت (Long-Term Viability) | بالا (High) |

---

# اصل معماری (Architecture Principle)

مؤلفه ارزیابی‌شده به‌عنوان یک سرویس زیرساختی ایزوله عمل می‌کند و کاملاً از وابستگی‌های لایه‌ای معماری تمیز و قواعد ایزولاسیون دامنه پیروی می‌نماید.

---

# 8. ارزیابی xUnit v3 (xUnit v3 Evaluation)

## نمای کلی (Overview)

فریم‌ورک xUnit استاندارد دو فاکتوی فریم‌ورک‌های آزمون برای برنامه‌های مدرن دات‌نت است.

نسخه ۳ بهبودهایی را در زمینه‌های زیر ارائه می‌دهد:

- عملکرد و کارایی اجرا؛
- توسعه‌پذیری؛
- موازی‌سازی؛
- ابزارهای عیب‌یابی؛
- اجرای ناهمگام آزمون‌ها.

این فریم‌ورک به‌طور خاص برای توسعه مدرن دات‌نت طراحی شده و با شیوه‌های آزمون پیشنهادی مایکروسافت همسو است.

---

# نقش معماری (Architectural Role)

```text
                Test Project

                      │

                      ▼

                 xUnit v3 Runner

                      │

      ┌───────────────┼────────────────┐

      ▼               ▼                ▼

 Unit Tests    Integration Tests   Architecture Tests
```

فریم‌ورک xUnit موتور اجرایی را برای تمام آزمون‌های خودکار فراهم می‌سازد.

---

# نقاط قوت معماری (Architectural Strengths)

مزایا شامل موارد زیر است:

- پشتیبانی بومی از دات‌نت
- کشف آزمون مبتنی بر خصوصیت (Attribute-based test discovery)
- اجرای موازی
- طراحی مبتنی بر رویکرد ناهمگام (Async-first design)
- اجرای چندسکویی
- اکوسیستم بالغ
- یکپارچگی قوی با محیط‌های توسعه (IDE)
- توسعه‌پذیری عالی

---

# قابلیت‌های کارکردی (Functional Capabilities)

فریم‌ورک xUnit از موارد زیر پشتیبانی می‌کند:

- آزمون‌های واحد (Unit Tests)
- آزمون‌های پارامتری (Parameterized Tests)
- آزمون‌های مبتنی بر تئوری (Theory Tests)
- آزمون‌های داده‌محور (Data-Driven Tests)
- آزمون‌های ناهمگام (Async Tests)
- اشتراک‌گذاری فیچرز (Fixture Sharing)
- فیچرهای مجموعه‌ای (Collection Fixtures)
- اجرای موازی (Parallel Execution)
- دسته‌بندی آزمون‌ها (Test Categorization)

---

# سازگاری با معماری تمیز (Clean Architecture Compatibility)

فریم‌ورک xUnit به‌طور طبیعی با معماری تمیز یکپارچه می‌شود.

چیدمان معمول پروژه:

```text
tests/

    Unit/

    Integration/

    Architecture/

    UI/

    Performance/
```

هر لایه معماری می‌تواند به‌طور مستقل مورد آزمون قرار گیرد.

---

# کارایی و عملکرد (Performance)

نسخه xUnit v3 موارد زیر را فراهم می‌کند:

- کشف سریع آزمون‌ها
- اجرای کارآمد
- زمان‌بندی موازی
- مصرف پایین حافظه

عملکرد در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# اجرای موازی (Parallel Execution)

اجرای موازی به‌صورت بومی پشتیبانی می‌شود.

مزایا:

- کاهش زمان اجرای CI
- بهره‌برداری بهتر از پردازنده
- بازخورد سریع‌تر به توسعه‌دهنده

موازی‌سازی می‌تواند در سطوح زیر پیکربندی شود:

- سطح اسمبلی (Assembly level)؛
- سطح مجموعه (Collection level)؛
- فیچرهای مجزا (Individual fixtures).

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- رابط برنامه‌نویسی ساده
- یکپارچگی عالی با Visual Studio
- پشتیبانی از Rider
- سازگاری با VS Code
- یکپارچگی با `dotnet test`

تجربه توسعه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# سازگاری با CI/CD (CI/CD Compatibility)

فریم‌ورک xUnit مستقیماً با موارد زیر یکپارچه می‌شود:

- dotnet test
- GitHub Actions
- Azure DevOps
- TeamCity
- Jenkins

هیچ ابزار اجرایی اضافی مورد نیاز نیست.

---

# توسعه‌پذیری (Extensibility)

فریم‌ورک xUnit نقاط توسعه را برای موارد زیر فراهم می‌کند:

- خصوصیات سفارشی (Custom attributes)؛
- منابع داده سفارشی؛
- فیچرها؛
- تزریق وابستگی؛
- کاشف‌های سفارشی.

---

# پشتیبانی جامعه کاربری (Community Support)

فریم‌ورک xUnit دارای ویژگی‌های زیر است:

- جامعه کاربری بسیار بزرگ؛
- اکوسیستم بالغ؛
- مستندات جامع؛
- نگهداری و توسعه مداوم.

پشتیبانی جامعه کاربری در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# دوام و پایداری بلندمدت (Long-Term Viability)

فریم‌ورک xUnit به استاندارد صنعت برای آزمون دات‌نت تبدیل شده است.

ریسک پشتیبانی بلندمدت در سطح **بسیار پایین (Very Low)** ارزیابی می‌شود.

---

# مقایسه (Comparison)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| یکپارچگی با دات‌نت (.NET Integration) | عالی (Excellent) |
| کارایی و عملکرد (Performance) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| اجرای موازی (Parallel Execution) | عالی (Excellent) |
| یکپارچگی با CI/CD (CI/CD Integration) | عالی (Excellent) |
| مستندات (Documentation) | عالی (Excellent) |
| پشتیبانی جامعه کاربری (Community Support) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) |

---

# مزایا (Advantages)

- همسویی با اکوسیستم رسمی دات‌نت
- چارچوب بالغ
- پشتیبانی عالی از ابزارها
- عملکرد بالای اجرا
- منحنی یادگیری ساده

---

# معایب (Disadvantages)

- فاقد کتابخانه اعتبارسنجی توکار (توسط FluentAssertions به‌صورت جداگانه مدیریت می‌شود)
- فاقد چارچوب شبیه‌سازی توکار (توسط NSubstitute به‌صورت جداگانه مدیریت می‌شود)

این موارد تصمیمات آگاهانه معماری هستند و نه محدودیت‌ها.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

نسخه xUnit v3 نیازمندی‌های چارچوب آزمون MachineryManagerEnterprise را کاملاً برآورده می‌سازد.

این فریم‌ورک به‌عنوان پایه‌ای که سایر فناوری‌های آزمون بر روی آن بنا می‌شوند، تصویب می‌گردد.

---


# 11. ارزیابی Testcontainers (Testcontainers Evaluation)

## نمای کلی (Overview)

پلتفرم Testcontainers یک چارچوب آزمون یکپارچگی است که کانتینرهای یکبار مصرف داکر را در طول اجرای آزمون‌های خودکار فراهم می‌کند.

به‌جای تکیه بر پایگاه‌های داده اشتراکی یا زیرساخت‌های پیکربندی‌شده دستی، هر مجموعه آزمون سرویس‌های کانتینری ایزوله‌ای ایجاد می‌کند که پس از اجرا به‌طور خودکار نابود می‌شوند.

برای MachineryManagerEnterprise، فریم‌ورک Testcontainers به‌عنوان چارچوب استاندارد برای آزمون یکپارچگی مبتنی بر زیرساخت ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
              Integration Test

                    │

                    ▼

            Testcontainers Library

                    │

      ┌─────────────┼─────────────┐

      ▼             ▼             ▼

 SQL Server     RabbitMQ      Redis

  Container      Container    Container

      │             │             │

      └─────────────┴─────────────┘

            Disposable Infrastructure
```

هر آزمون یکپارچگی در برابر یک نمونه زیرساختی تازه و تمیز اجرا می‌شود.

---

# نقاط قوت معماری (Architectural Strengths)

مزایا شامل موارد زیر است:

- محیط‌های یکبار مصرف
- ایزولاسیون زیرساخت
- اجرای تکرارپذیر
- حذف پایگاه‌های داده اشتراکی آزمون
- زیرساخت واقعی به‌جای شبیه‌سازها (Mocks)
- سازگاری چندسکویی
- اجرای بومی در داکر
- سازگار با CI/CD

---

# قابلیت‌های کارکردی (Functional Capabilities)

فریم‌ورک Testcontainers از موارد زیر پشتیبانی می‌کند:

- کانتینرهای SQL Server
- کانتینرهای PostgreSQL
- کانتینرهای RabbitMQ
- کانتینرهای Redis
- کانتینرهای Elasticsearch
- کانتینرهای MinIO
- تصاویر سفارشی داکر (Custom Docker images)
- مدیریت چرخه حیات کانتینر

---

# سازگاری با معماری تمیز (Clean Architecture Compatibility)

فریم‌ورک Testcontainers منحصراً درون لایه Integration Testing استفاده می‌شود.

ساختار معمول:

```text
tests/

    Integration/

        Containers/

        Fixtures/

        Scenarios/
```

هیچ کد عملیاتی (Production Code) به Testcontainers وابسته نیست.

---

# ایزولاسیون زیرساخت (Infrastructure Isolation)

هر اجرای آزمون زیرساخت ایزوله‌ای ایجاد می‌کند.

مثال:

```text
Test Start

     │

     ▼

Start SQL Server Container

     │

Run Tests

     │

Dispose Container

     │

Environment Clean
```

هیچ حالت باقیمانده‌ای پس از اجرا باقی نمی‌ماند.

---

# قابلیت اطمینان (Reliability)

استفاده از زیرساخت واقعی تناقضات میان موارد زیر را حذف می‌کند:

- توسعه محلی؛
- محیط CI؛
- پیکربندی محیط عملیاتی.

قابلیت اطمینان در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

راه‌اندازی کانتینر بار اضافی (Overhead) به همراه دارد.

با این حال:

- راه‌اندازی فقط یک بار به ازای هر فیچر رخ می‌دهد؛
- استفاده مجدد از زیرساخت پشتیبانی می‌شود؛
- زمان اجرا برای آزمون یکپارچگی سازمانی قابل قبول باقی می‌ماند.

عملکرد در سطح **بسیار خوب (Very Good)** ارزیابی می‌شود.

---

# سازگاری با CI/CD (CI/CD Compatibility)

فریم‌ورک Testcontainers با موارد زیر یکپارچه می‌شود:

- GitHub Actions
- Azure DevOps
- Docker Desktop
- کانتینرهای لینوکس
- کانتینرهای ویندوز (در صورت پشتیبانی)

چرخه حیات کانتینر به‌صورت خودکار مدیریت می‌شود.

---

# قابلیت نگهداری (Maintainability)

مزایا شامل موارد زیر است:

- زیرساخت تعریف‌شده به‌عنوان کد (Infrastructure defined as code)
- محیط‌های آزمون تحت کنترل نسخه
- بدون پیکربندی دستی
- اجرای قطعی و بدون نوسان

قابلیت نگهداری در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تناسب سازمانی (Enterprise Suitability)

فریم‌ورک Testcontainers به‌ویژه برای اعتبارسنجی موارد زیر مناسب است:

- پیاده‌سازی‌های مخزن داده (Repository implementations)
- نگاشت‌های EF Core
- مایگریشن‌های SQL
- زیرساخت پیام‌رسانی
- مؤلفه‌های توزیع‌شده
- یکپارچگی زیرساخت

---

# مقایسه (Comparison)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| ایزولاسیون زیرساخت (Infrastructure Isolation) | عالی (Excellent) |
| قابلیت اطمینان (Reliability) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |
| یکپارچگی با داکر (Docker Integration) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) |
| کارایی و عملکرد (Performance) | بسیار خوب (Very Good) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) |

---

# مزایا (Advantages)

- آزمون بر روی زیرساخت واقعی
- محیط‌های یکبار مصرف
- بدون پایگاه‌های داده اشتراکی
- اجرای قطعی
- سازگاری عالی با CI

---

# معایب (Disadvantages)

- وابستگی به داکر
- کندتر از آزمون‌های واحد
- مصرف منابع بالاتر

این مصالحه‌ها برای آزمون‌های یکپارچگی مناسب و قابل قبول هستند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

فریم‌ورک Testcontainers نیازمندی‌های آزمون زیرساخت MachineryManagerEnterprise را کاملاً برآورده می‌سازد.

این ابزار به‌عنوان استاندارد سازمانی برای آزمون‌های یکپارچگی شامل زیرساخت خارجی تصویب می‌شود.

---


# 12. ارزیابی Playwright (Playwright Evaluation)

## نمای کلی (Overview)

پلی‌رایت (Playwright) چارچوب مدرن اتوماسیون مرورگر مایکروسافت است که برای آزمون سرتاسری (E2E) مطمئن برنامه‌های تحت وب طراحی شده است.

اگرچه MachineryManagerEnterprise عمدتاً یک برنامه دسکتاپ مبتنی بر Avalonia UI را هدف قرار می‌دهد، Playwright برای موارد زیر مرتبط باقی می‌ماند:

- پورتال‌های وب آینده؛
- داشبوردهای مدیریتی؛
- سرویس‌های احراز هویت؛
- پورتال‌های گزارش‌گیری؛
- مؤلفه‌های تعبیه‌شده مرورگر (Embedded browser components)؛
- وب APIها با رابط‌های مبتنی بر مرورگر.

پلتفرم Playwright به‌عنوان پلتفرم اتوماسیون مرورگر سازمانی ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

```text
            End-to-End Test

                   │

                   ▼

               Playwright

                   │

      ┌────────────┼────────────┐

      ▼            ▼            ▼

   Chromium      Firefox      WebKit

                   │

                   ▼

         User Interface Validation
```

فریم‌ورک Playwright سناریوهای کامل کاربری را بر روی برنامه‌های در حال اجرا اجرا می‌کند.

---

# نقاط قوت معماری (Architectural Strengths)

مزایا شامل موارد زیر است:

- اجرای در مرورگرهای مختلف (Cross-browser execution)
- انتظار خودکار (Automatic waiting)
- انتخاب‌کننده‌های قابل اعتماد (Reliable selectors)
- اجرای سریع
- آزمون موازی
- پشتیبانی عالی از دات‌نت
- پشتیبانی قدرتمند مایکروسافت
- اکوسیستم بالغ

---

# قابلیت‌های کارکردی (Functional Capabilities)

فریم‌ورک Playwright از موارد زیر پشتیبانی می‌کند:

- اتوماسیون رابط کاربری (UI Automation)
- اتوماسیون مرورگر
- اعتبارسنجی اسکرین‌شات (Screenshot Validation)
- تولید PDF
- سناریوهای احراز هویت
- آپلود فایل
- اعتبارسنجی دانلود
- رهگیری شبکه (Network Interception)
- آزمون دسترسی‌پذیری (Accessibility Testing)

---

# پشتیبانی از مرورگرهای مختلف (Cross-Browser Support)

مرورگرهای پشتیبانی‌شده شامل موارد زیر است:

| مرورگر | پشتیبانی |
|----------|:-------:|
| Chromium | ✅ |
| Firefox | ✅ |
| WebKit | ✅ |

این امر رفتار یکنواخت را در سراسر موتورهای مرورگر پشتیبانی‌شده تضمین می‌کند.

---

# قابلیت اطمینان (Reliability)

فریم‌ورک Playwright به‌طور خودکار موارد زیر را مدیریت می‌کند:

- بارگذاری ناهمگام صفحات؛
- در دسترس بودن عناصر؛
- تأخیرهای رندرینگ؛
- همگام‌سازی ناوبری.

این امر به‌طور قابل توجهی آزمون‌های ناپایدار رابط کاربری (Flaky UI tests) را در مقایسه با فریم‌ورک‌های قدیمی اتوماسیون مرورگر کاهش می‌دهد.

قابلیت اطمینان در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# کارایی و عملکرد (Performance)

فریم‌ورک Playwright ویژگی‌های زیر را ارائه می‌دهد:

- راه‌اندازی سریع مرورگر
- اجرای موازی
- مصرف بهینه منابع
- اجرای بدون واسط گرافیکی (Headless execution)
- موتور اتوماسیون بهینه‌شده

عملکرد در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# سازگاری با CI/CD (CI/CD Compatibility)

فریم‌ورک Playwright مستقیماً با موارد زیر یکپارچه می‌شود:

- GitHub Actions
- Azure DevOps
- Docker
- Linux
- Windows
- macOS

اجرای Headless آن را برای خطوط لوله خودکار بسیار مناسب می‌سازد.

---

# تجربه توسعه‌دهنده (Developer Experience)

مزایا شامل موارد زیر است:

- رابط برنامه‌نویسی روان (Fluent API)
- ابزارهای عیب‌یابی عالی
- نمایشگر ردگیری (Trace Viewer)
- ثبت تصویر صفحه (Screenshot Capture)
- ضبط ویدیو (Video Recording)
- مستندات غنی

تجربه توسعه‌دهنده در سطح **عالی (Excellent)** ارزیابی می‌شود.

---

# تناسب سازمانی (Enterprise Suitability)

فریم‌ورک Playwright برای سناریوهای زیر مناسب است:

- پورتال مدیریت وب
- رابط کاربری احراز هویت
- داشبورد گزارش‌گیری
- جریان‌های کاری کاربری مبتنی بر مرورگر
- اعتبارسنجی سرتاسری
- آزمون رگرسیون

---

# محدودیت‌ها (Limitations)

فریم‌ورک Playwright برای اتوماسیون پنجره‌های محلی دسکتاپ Avalonia در نظر گرفته **نشده** است.

برای اتوماسیون رابط کاربری محلی دسکتاپ، فناوری‌های اختصاصی اتوماسیون دسکتاپ در صورت نیاز در آینده به‌صورت جداگانه ارزیابی خواهند شد.

گنجاندن آن در پشته آزمون مشخصاً برای مؤلفه‌های مبتنی بر مرورگر است.

---

# مقایسه (Comparison)

| معیار | ارزیابی |
|-----------|------------|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| اتوماسیون مرورگر (Browser Automation) | عالی (Excellent) |
| پشتیبانی چندمرورگری (Cross-Browser Support) | عالی (Excellent) |
| قابلیت اطمینان (Reliability) | عالی (Excellent) |
| کارایی و عملکرد (Performance) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) |
| مستندات (Documentation) | عالی (Excellent) |
| تجربه توسعه‌دهنده (Developer Experience) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) |

---

# مزایا (Advantages)

- پشتیبانی‌شده توسط مایکروسافت
- معماری مدرن
- اجرای پایدار
- عیب‌یابی عالی
- یکپارچگی قوی با اکوسیستم دات‌نت

---

# معایب (Disadvantages)

- برای اتوماسیون رابط کاربری محلی دسکتاپ مناسب نیست
- نیازمند محیط اجرای مرورگر است

این محدودیت‌ها بر نقش مورد نظر آن درون MachineryManagerEnterprise تأثیری نمی‌گذارند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

فریم‌ورک Playwright نیازمندی‌های اتوماسیون مرورگر MachineryManagerEnterprise را کاملاً برآورده می‌سازد.

این ابزار به‌عنوان استاندارد سازمانی برای آزمون سرتاسری مبتنی بر مرورگر تصویب می‌شود.

---


# 13. مقایسه کلی فناوری‌ها (Overall Technology Comparison)

به‌دنبال ارزیابی تفصیلی تمامی فناوری‌های کاندید، شورای بازنگری معماری پشته کامل آزمون را در برابر اهداف معماری MachineryManagerEnterprise مقایسه نمود.

---

# نمای کلی پشته فناوری (Technology Stack Overview)

| لایه آزمون | فناوری انتخاب‌شده |
|--------------|---------------------|
| آزمون واحد (Unit Testing) | xUnit v3 |
| اعتبارسنجی‌ها (Assertions) | FluentAssertions |
| شبیه‌سازی (Mocking) | NSubstitute |
| آزمون یکپارچگی (Integration Testing) | Testcontainers |
| آزمون سرتاسری (End-to-End Testing) | Playwright |

این فناوری‌ها در کنار یکدیگر یک اکوسیستم کامل آزمون سازمانی را تشکیل می‌دهند.

---

# ماتریس مقایسه فناوری‌ها (Technology Comparison Matrix)

| معیار | xUnit v3 | FluentAssertions | NSubstitute | Testcontainers | Playwright |
|-----------|:--------:|:----------------:|:-----------:|:--------------:|:----------:|
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| یکپارچگی با دات‌نت (.NET Integration) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| بهره‌وری توسعه‌دهنده (Developer Productivity) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | خوب (Good) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| کارایی و عملکرد (Performance) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | عالی (Excellent) |
| سازگاری با CI/CD (CI/CD Compatibility) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| مستندات (Documentation) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | عالی (Excellent) |
| پشتیبانی جامعه کاربری (Community Support) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | عالی (Excellent) |
| دوام بلندمدت (Long-Term Viability) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |

---

# نگاشت هرم آزمون (Test Pyramid Mapping)

```text
               End-to-End

                Playwright

                     ▲

             Integration Tests

             Testcontainers

                     ▲

               Unit Testing

      xUnit + FluentAssertions

           + NSubstitute
```

فناوری‌های انتخاب‌شده در مجموع هرم آزمون مصوب را پیاده‌سازی می‌کنند.

---

# مسئولیت‌ها (Responsibilities)

| مسئولیت | فناوری |
|---------------|------------|
| اجراکننده آزمون (Test Runner) | xUnit v3 |
| اعتبارسنجی‌ها (Assertions) | FluentAssertions |
| اشیاء شبیه‌سازی (Mock Objects) | NSubstitute |
| اعتبارسنجی زیرساخت (Infrastructure Validation) | Testcontainers |
| اتوماسیون مرورگر (Browser Automation) | Playwright |

هر فناوری مسئولیت کاملاً مشخصی با حداقل هم‌پوشانی دارد.

---

# سازگاری معماری (Architectural Compatibility)

| اصل معماری | نتیجه |
|------------------------|--------|
| معماری تمیز (Clean Architecture) | ✅ |
| وارونگی وابستگی (Dependency Inversion) | ✅ |
| تفکیک مسئولیت فرمان و پرس‌وجو (CQRS) | ✅ |
| ایزولاسیون زیرساخت (Infrastructure Isolation) | ✅ |
| هرم آزمون (Test Pyramid) | ✅ |
| قابلیت نگهداری سازمانی (Enterprise Maintainability) | ✅ |

پشته کامل آزمون کاملاً با معماری مصوب همسو است.

---

# پیچیدگی عملیاتی (Operational Complexity)

```text
Lowest Complexity

xUnit

↓

FluentAssertions

↓

NSubstitute

↓

Playwright

↓

Testcontainers

Highest Complexity
```

اگرچه Testcontainers بیشترین پیچیدگی عملیاتی را به همراه دارد، اما کاربرد آن به آزمون‌های یکپارچگی مبتنی بر زیرساخت محدود شده است.

---

# آمادگی CI/CD (CI/CD Readiness)

پشته انتخاب‌شده موارد زیر را پشتیبانی می‌کند:

- اجرای موازی
- اجرای چندسکویی
- زیرساخت کانتینری
- اجرای مرورگر بدون واسط گرافیکی (Headless)
- گزارش‌دهی خودکار

این پشته کاملاً با خط لوله ساخت مصوب سازگار است.

---

# پوشش سازمانی (Enterprise Coverage)

| دسته‌بندی آزمون | پوشش |
|------------------|----------|
| آزمون واحد (Unit Testing) | کامل (Complete) |
| آزمون یکپارچگی (Integration Testing) | کامل (Complete) |
| آزمون زیرساخت (Infrastructure Testing) | کامل (Complete) |
| آزمون رابط کاربری مرورگر (Browser UI Testing) | کامل (Complete) |
| آزمون رگرسیون (Regression Testing) | کامل (Complete) |
| یکپارچه‌سازی مداوم (Continuous Integration) | کامل (Complete) |

---

# قابلیت نگهداری بلندمدت (Long-Term Maintainability)

فناوری‌های انتخاب‌شده دارای ویژگی‌های زیر هستند:

- به‌طور فعال نگهداری می‌شوند؛
- به‌طور گسترده پذیرفته شده‌اند؛
- به‌شدت با دات‌نت مدرن یکپارچه هستند؛
- برای نرم‌افزارهای سازمانی بلندمدت مناسب می‌باشند.

---

# ارزیابی معماری (Architectural Assessment)

پلتفرم کامل آزمون تمام اهداف معماری مصوب MachineryManagerEnterprise را برآورده می‌سازد:

- قابلیت نگهداری بالا؛
- اجرای قطعی؛
- ایزولاسیون زیرساخت؛
- اعتبارسنجی خودکار؛
- مقیاس‌پذیری سازمانی؛
- توسعه‌پذیری آینده.

هیچ فناوری آزمون اضافی برای معماری فعلی مورد نیاز نیست.

---


# 14. پیشنهاد نهایی (Final Recommendation)

شورای بازنگری معماری اتخاذ پشته آزمون سازمانی زیر را پیشنهاد می‌کند:

| دسته‌بندی | فناوری مصوب |
|----------|---------------------|
| آزمون واحد (Unit Testing) | **xUnit v3** |
| اعتبارسنجی‌ها (Assertions) | **FluentAssertions** |
| شبیه‌سازی (Mocking) | **NSubstitute** |
| آزمون یکپارچگی (Integration Testing) | **Testcontainers** |
| آزمون سرتاسری (End-to-End Testing) | **Playwright** |

این ترکیب موارد زیر را فراهم می‌آورد:

- پوشش کامل هرم آزمون.
- یکپارچگی عالی با دات‌نت.
- قابلیت نگهداری سازمانی.
- بهره‌وری بالای توسعه‌دهنده.
- سازگاری قوی با CI/CD.

---

# مقایسه کلی فناوری‌ها (Overall Technology Comparison)

فناوری انتخاب‌شده عملکرد بهینه، قابلیت نگهداری و سازگاری با معماری تمیز را فراهم می‌سازد.

## ماتریس مسئولیت (Responsibility Matrix)

| مسئولیت | فناوری پیشنهادی | جایگزین |
|-----------------|------------------------|-------------|
| قابلیت سیستم | انتخاب اصلی (Primary Selected) | گزینه ارزیابی‌شده (Evaluated Option) |

---

# بیانیه پیشنهاد نهایی (Final Recommendation Statement)

پشته آزمون پیشنهادی به‌عنوان پلتفرم استاندارد آزمون برای MachineryManagerEnterprise تصویب می‌شود.

---

# 15. تصمیم نهایی (Final Decision)

## پشته مصوب آزمون (Approved Testing Stack)

```text
                    Playwright

                        ▲

                 Testcontainers

                        ▲

        xUnit v3 + FluentAssertions

              + NSubstitute
```

---

## تصمیمات فناوری (Technology Decisions)

| فناوری | تصمیم | وضعیت |
|------------|----------|--------|
| xUnit v3 | تصویب شد (Approved) | ✅ |
| FluentAssertions | تصویب شد (Approved) | ✅ |
| NSubstitute | تصویب شد (Approved) | ✅ |
| Testcontainers | تصویب شد (Approved) | ✅ |
| Playwright | تصویب شد (Approved) | ✅ |

---

## سوابق تصمیمات معماری مرتبط (Related Architecture Decision)

پیاده‌سازی این ارزیابی نیازمند سند زیر است:

- **ADR-0024 — استراتژی آزمون سازمانی (Enterprise Testing Strategy)**

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
|---------|------------|--------------------|------------------------------------------------|
| 1.0.0 | 2026-07-28 | معمار راهکار | ارزیابی اولیه فناوری برای آزمون |
| 1.1.0 | 2026-07-28 | معمار راهکار | حذف خط تکراری عنوان؛ تبدیل جداول رتبه‌بندی ستاره‌ای به رتبه‌بندی متنی برای هماهنگی |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازنگری و همگام‌سازی با آخرین تغییرات |