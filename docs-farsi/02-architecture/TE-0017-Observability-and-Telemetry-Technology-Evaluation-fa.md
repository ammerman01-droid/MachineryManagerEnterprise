| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | TE-0017 |
| **عنوان** | ارزیابی فناوری مشاهده‌پذیری و تله‌متری (.NET 10) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-26 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند به ارزیابی فناوری‌های کاندید برای «ارزیابی فناوری مشاهده‌پذیری و تله‌متری (.NET 10)» در سامانه MachineryManagerEnterprise می‌پردازد.

هدف، دستیابی به یک انتخاب فناوری یکپارچه است که ضمن رعایت کامل اصول معماری پاک (Clean Architecture)، تمامی نیازمندی‌های کارکردی و معماری را برآورده سازد.

---

# دامنه ارزیابی (Evaluation Scope)

این ارزیابی فناوری صرفاً به انتخاب فناوری می‌پردازد.

جزئیات پیاده‌سازی توسط سوابق تصمیمات معماری (ADRها) متناظر تعریف می‌شوند.

---

# مراجع معماری (Architectural References)

این ارزیابی بر پایه موارد زیر استوار است:

- ADR-0001 — معماری پاک (Clean Architecture)
- ADR-0016 — معماری پیام‌رسانی سازمانی (Enterprise Messaging Architecture)
- ADR-0017 — یکپارچه‌سازی هوش مصنوعی (Artificial Intelligence Integration)
- ADR-0018 — معماری یکپارچه‌سازی خارجی (External Integration Architecture)

پلتفرم مشاهده‌پذیری باید دارای ویژگی‌های زیر باقی بماند:

- مستقل از تامین‌کننده (vendor independent)؛
- مستقل از نحوه استقرار (deployment independent)؛
- خنثی نسبت به محیط ابری (cloud neutral)؛
- توسعه‌پذیر (extensible).

---

# نیازمندی‌های کارکردی (Functional Requirements)

این پلتفرم نیازمند قابلیت‌های زیر است:

- لاگ‌گیری ساختاریافته (structured logging)؛
- ردگیری توزیع‌شده (distributed tracing)؛
- جمع‌آوری متریک‌ها (metrics collection)؛
- بررسی‌های سلامت (health checks)؛
- شناسه‌های همبستگی (correlation identifiers)؛
- عیب‌یابی و پایش درخواست‌ها (request diagnostics)؛
- عیب‌یابی و پایش هوش مصنوعی (AI diagnostics)؛
- عیب‌یابی و پایش پیام‌رسانی (messaging diagnostics)؛
- عیب‌یابی و پایش پایگاه داده (database diagnostics)؛
- عیب‌یابی و پایش حافظه موقت (cache diagnostics)؛
- یکپارچه‌سازی با سامانه هشدارهای هوشمند (alerting integration).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

راهکار مشاهده‌پذیری باید ویژگی‌های زیر را فراهم آورد:

- حداقل بار سربار در زمان اجرا (minimal runtime overhead)؛
- مقیاس‌پذیری سازمانی (enterprise scalability)؛
- قابلیت اطمینان بالا (high reliability)؛
- سادگی عملیاتی (operational simplicity)؛
- قابلیت نگهداری بالا (maintainability)؛
- خنثی بودن نسبت به ابر (cloud neutrality)؛
- توسعه‌پذیری بلندمدت (long-term extensibility).

---

# فناوری‌های کاندید (Candidate Technologies)

## استاندارد تله‌متری (Telemetry Standard)

| فناوری | نقش |
|------------|------|
| OpenTelemetry (.NET 10) | استاندارد یکپارچه تله‌متری (Unified Telemetry Standard) |

---

## لاگ‌گیری ساختاریافته (Structured Logging)

| فناوری | نقش |
|------------|------|
| Serilog | فریم‌ورک لاگ‌گیری ساختاریافته (Structured Logging Framework) |
| Microsoft.Extensions.Logging | لایه انتزاع لاگ‌گیری (Logging Abstraction) |

---

## متریک‌ها (Metrics)

| فناوری | نقش |
|------------|------|
| OpenTelemetry Metrics | جمع‌آوری متریک‌ها (Metrics Collection) |
| Prometheus | ذخیره‌سازی متریک‌ها (Metrics Storage) |

---

## مصورسازی و داشبوردها (Visualization)

| فناوری | نقش |
|------------|------|
| Grafana | داشبوردها (Dashboards) |
| Kibana | مصورسازی لاگ‌ها (Log Visualization) |

---

## ذخیره‌سازی ردگیری (Trace Storage)

| فناوری | نقش |
|------------|------|
| Jaeger | ردگیری توزیع‌شده (Distributed Tracing) |
| Grafana Tempo | ردگیری توزیع‌شده (Distributed Tracing) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|----|-----------|--------|
| O1 | سازگاری با معماری پاک (Clean Architecture Compatibility) | حیاتی (Critical) |
| O2 | سازگاری با OpenTelemetry (OpenTelemetry Compatibility) | حیاتی (Critical) |
| O3 | عملکرد و کارایی (Performance) | بالا (High) |
| O4 | استقلال از تامین‌کننده (Vendor Independence) | حیاتی (Critical) |
| O5 | انعطاف‌پذیری استقرار (Deployment Flexibility) | بالا (High) |
| O6 | عیب‌یابی و پایش هوش مصنوعی (AI Diagnostics) | بالا (High) |
| O7 | آمادگی سازمانی (Enterprise Readiness) | بالا (High) |
| O8 | سادگی عملیاتی (Operational Simplicity) | متوسط (Medium) |
| O9 | قابلیت نگهداری (Maintainability) | بالا (High) |

---

# اصل معماری (Architecture Principle)

مشاهده‌پذیری به عنوان یک قابلیت زیرساختی عرضی (cross-cutting infrastructure capability) در نظر گرفته می‌شود.

ماژول‌های کسب‌وکار هرگز به صورت مستقیم به یک پیاده‌سازی لاگ‌گیری داده نمی‌نویسند.

در عوض:

```text
Business Modules
        │
        ▼
Logging Abstraction
        │
        ▼
OpenTelemetry
        │
 ┌──────────────┬───────────────┬──────────────┐
 ▼              ▼               ▼
Logs         Metrics         Traces
```

این معماری به ارائه‌دهندگان زیرساخت اجازه می‌دهد تا به صورت مستقل از کدهای کاربردی توسعه و تکامل یابند.

---

# 5. OpenTelemetry (.NET 10) Evaluation

## نمای کلی (Overview)

فناوری OpenTelemetry به استاندارد صنعت برای مشاهده‌پذیری سازمانی تبدیل شده است.

بستر .NET 10 یکپارچه‌سازی سطح اولی را با OpenTelemetry در تمامی بخش‌های زیر ارائه می‌دهد:

- ASP.NET Core؛
- HttpClient؛
- Entity Framework Core؛
- gRPC؛
- پیام‌رسانی (Messaging)؛
- پردازش پس‌زمینه (Background Processing).

به جای اینکه OpenTelemetry صرفاً یک فریم‌ورک لاگ‌گیری باشد، یک مدل تله‌متری یکپارچه را تعریف می‌کند.

---

## نقاط قوت معماری (Architectural Strengths)

مزایا عبارتند از:

- استاندارد باز (Open standard)؛
- مستقل از تامین‌کننده (Vendor neutral)؛
- یکپارچه‌سازی عالی با .NET 10؛
- ردگیری‌ها، لاگ‌ها و متریک‌های یکپارچه؛
- اکوسیستم گسترده؛
- بی‌طرفی نسبت به ابر؛
- پشتیبانی عالی از عیب‌یابی و پایش هوش مصنوعی؛
- ردگیری توزیع‌شده (Distributed tracing).

---

## نقاط ضعف معماری (Architectural Weaknesses)

فناوری OpenTelemetry به خودی خود ذخیره‌سازی یا مصورسازی ارائه نمی‌دهد.

این فناوری نیازمند صادرکننده‌ها (exporters) و پلتفرم‌های بک‌اند است.

این امر یک طراحی آگاهانه معماری است و نه یک محدودیت.

---

## مشخصات عملیاتی (Operational Characteristics)

فناوری OpenTelemetry موارد زیر را فراهم می‌کند:

- ردگیری‌های توزیع‌شده (distributed traces)؛
- متریک‌ها (metrics)؛
- لاگ‌ها (logs)؛
- محموله زمینه‌ای (baggage)؛
- انتشار زمینه (context propagation)؛
- همبستگی فعالیت‌ها (activity correlation).

پیچیدگی عملیاتی پایین است.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

فناوری OpenTelemetry از محیط‌های زیر پشتیبانی می‌کند:

- Windows؛
- Linux؛
- کانتینرها (Containers)؛
- کوبرنتیز (Kubernetes)؛
- درون‌سازمانی (On-Premise)؛
- ابری (Cloud)؛
- ترکیبی (Hybrid).

---

## سازگاری با هوش مصنوعی (AI Compatibility)

فناوری OpenTelemetry به صورت طبیعی با موارد زیر یکپارچه می‌شود:

- Semantic Kernel؛
- OpenAI؛
- Background Processing؛
- RabbitMQ؛
- Hangfire.

این فناوری ردگیری سراسری (end-to-end) جریان‌های کاری هوش مصنوعی را فراهم می‌سازد.

---

## تطابق معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| Clean Architecture | عالی (Excellent) |
| Vendor Independence | عالی (Excellent) |
| OpenTelemetry Standard | عالی (Excellent) |
| Enterprise Readiness | عالی (Excellent) |
| AI Compatibility | عالی (Excellent) |
| Maintainability | عالی (Excellent) |

---

## نتیجه‌گیری مقدماتی (Preliminary Conclusion)

فناوری OpenTelemetry باید به استاندارد تله‌متری یکپارچه MachineryManagerEnterprise تبدیل شود.

تمامی داده‌های تله‌متری—شامل لاگ‌ها، ردگیری‌ها و متریک‌ها—باید از ابزار دقیق (instrumentation) پلتفرم OpenTelemetry سرچشمه بگیرند.

---

# 6. Serilog Evaluation

## نمای کلی (Overview)

فناوری Serilog فریم‌ورک عملاً استاندارد (de facto) لاگ‌گیری ساختاریافته در اکوسیستم .NET است.

برخلاف لاگ‌گیری متنی سنتی، Serilog رویدادهای ساختاریافته‌ای صادر می‌کند که اطلاعات معنایی را به صورت جفت‌های کلید/مقدار حفظ می‌نمایند.

این امر موارد زیر را امکان‌پذیر می‌سازد:

- کوئری‌گیری کارآمد؛
- همبستگی لاگ‌ها (log correlation)؛
- پردازش ماشینی؛
- تحلیل‌های عملیاتی (operational analytics).

در سامانه MachineryManagerEnterprise، فریم‌ورک Serilog به عنوان پیاده‌سازی اصلی لاگ‌گیری ساختاریافته در زیر لایه انتزاع لاگ‌گیری مایکروسافت ارزیابی می‌شود.

---

## نقش معماری (Architectural Role)

فریم‌ورک Serilog به عنوان API لاگ‌گیری برنامه **نیست**.

کامپوننت‌های برنامه منحصراً با موارد زیر تعامل دارند:

- `ILogger<T>`

فریم‌ورک Serilog به عنوان ارائه‌دهنده زیرساختی عمل می‌کند.

```text
Business Modules
        │
        ▼
ILogger<T>
        │
        ▼
Serilog
        │
        ▼
OpenTelemetry Exporter
```

این معماری مانع از وابستگی کدهای کسب‌وکار به Serilog می‌شود.

---

## نقاط قوت معماری (Architectural Strengths)

### مزایا

- اکوسیستم بالغ در .NET؛
- لاگ‌گیری ساختاریافته (Structured logging)؛
- عملکرد و کارایی بالا؛
- اکوسیستم غنی از مقاصد خروجی (sinks)؛
- یکپارچه‌سازی عالی با `Microsoft.Extensions.Logging`؛
- یکپارچه‌سازی عالی با OpenTelemetry؛
- لاگ‌گیری مبتنی بر JSON؛
- پشتیبانی از همبستگی رویدادها (Correlation support)؛
- غنی‌سازی زمینه (Context enrichment)؛
- سازگار با ابر (Cloud-native)؛
- سازگار با کانتینر (Container friendly).

---

## نقاط ضعف معماری (Architectural Weaknesses)

فریم‌ورک Serilog به طور آگاهانه صرفاً بر لاگ‌گیری تمرکز دارد.

این فریم‌ورک موارد زیر را ارائه نمی‌دهد:

- متریک‌ها (metrics)؛
- ردگیری‌ها (traces)؛
- داشبوردها (dashboards)؛
- تله‌متری توزیع‌شده (distributed telemetry).

این مسئولیت‌ها متعلق به پلتفرم OpenTelemetry است.

---

## مشخصات عملیاتی (Operational Characteristics)

فریم‌ورک Serilog موارد زیر را پشتیبانی می‌کند:

- رویدادهای ساختاریافته؛
- غنی‌سازها (enrichers)؛
- سینک‌ها و مقاصد لاگ (sinks)؛
- لاگ‌گیری ناهمگام (asynchronous logging)؛
- فایل‌های چرخشی (rolling files)؛
- لاگ‌گیری در کنسول؛
- صدور به OpenTelemetry؛
- صدور به Elasticsearch.

پیچیدگی عملیاتی پایین است.

---

## مقیاس‌پذیری (Scalability)

فریم‌ورک Serilog به صورت طبیعی با موارد زیر مقیاس می‌پذیرد:

- ASP.NET Core؛
- سرویس‌های Worker Services؛
- کانتینرها (Containers)؛
- کوبرنتیز (Kubernetes).

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

## امنیت (Security)

فریم‌ورک Serilog موارد زیر را پشتیبانی می‌کند:

- فیلترسازی؛
- ماسک‌گذاری داده‌های حساس (sensitive data masking)؛
- غنی‌سازهای با قابلیت پیکربندی.

با این وجود، توسعه‌دهندگان همچنان مسئول جلوگیری از درج اطلاعات حساس در رویدادهای لاگ هستند.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده عبارتند از:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- On-Premise
- Hybrid

انعطاف‌پذیری استقرار عالی است.

---

## سازگاری با هوش مصنوعی (AI Compatibility)

فریم‌ورک Serilog به ویژه برای عیب‌یابی و پایش هوش مصنوعی بسیار ارزشمند است.

رویدادهای لاگ متداول شامل موارد زیر هستند:

- اجرای پرامپت (prompt execution)؛
- انتخاب مدل (model selection)؛
- تاخیر استنتاج (inference latency)؛
- مصرف توکن (token consumption)؛
- تولید امبدینگ (embedding generation)؛
- درخواست‌های جستجوی معنایی (semantic search requests)؛
- خطاهای هوش مصنوعی (AI failures).

لاگ‌گیری ساختاریافته به طور چشمگیری عیب‌یابی جریان‌های کاری هوش مصنوعی را بهبود می‌بخشد.

---

## قابلیت نگهداری (Maintainability)

فریم‌ورک Serilog ویژگی‌های زیر را به نمایش می‌گذارد:

- مستندات استثنایی و کامل؛
- اکوسیستم بالغ؛
- پایداری بلندمدت؛
- پشتیبانی گسترده جامعه کاربری.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

## تناسب با MachineryManagerEnterprise (Suitability for MachineryManagerEnterprise)

فریم‌ورک Serilog تمامی نیازمندی‌های لاگ‌گیری ساختاریافته شناسایی‌شده در تحلیل معماری را برآورده می‌سازد.

مسئولیت‌های اصلی آن عبارتند از:

- عیب‌یابی و پایش برنامه (application diagnostics)؛
- لاگ‌گیری عملیاتی (operational logging)؛
- عیب‌یابی هوش مصنوعی (AI diagnostics)؛
- عیب‌یابی زیرساخت (infrastructure diagnostics)؛
- ثبت رویدادهای ساختاریافته (structured event recording).

---

## تطابق معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| Clean Architecture | عالی (Excellent) |
| Structured Logging | عالی (Excellent) |
| OpenTelemetry Integration | عالی (Excellent) |
| Enterprise Readiness | عالی (Excellent) |
| Performance | عالی (Excellent) |
| Maintainability | عالی (Excellent) |

---

## رابطه با OpenTelemetry (Relationship with OpenTelemetry)

فناوری‌های Serilog و OpenTelemetry فناوری‌های مکمل یکدیگر هستند و نه رقیب.

```text
Application
      │
ILogger<T>
      │
      ▼
Serilog
      │
      ▼
OpenTelemetry
      │
      ▼
Telemetry Backend
```

مسئولیت‌ها کاملاً تفکیک‌شده باقی می‌مانند.

| فناوری | مسئولیت |
|------------|----------------|
| Serilog | تولید لاگ‌های ساختاریافته (Structured Log Generation) |
| OpenTelemetry | انتقال یکپارچه تله‌متری (Unified Telemetry Transport) |

---

## نتیجه‌گیری مقدماتی (Preliminary Conclusion)

فریم‌ورک Serilog باید به پیاده‌سازی استاندارد لاگ‌گیری ساختاریافته برای MachineryManagerEnterprise تبدیل شود.

ماژول‌های کسب‌وکار هرگز نباید به طور مستقیم به Serilog ارجاع داشته باشند.

در عوض:

- Business Modules
- → `ILogger<T>`
- → Serilog
- → OpenTelemetry

این امر استقلال زیرساختی را حفظ کرده و در عین حال لاگ‌گیری ساختاریافته در سطح سازمانی را فراهم می‌سازد.

---

# 7. Microsoft.Extensions.Logging Evaluation

## نمای کلی (Overview)

کتابخانه `Microsoft.Extensions.Logging` لایه انتزاع رسمی لاگ‌گیری ارائه‌شده توسط مایکروسافت برای پلتفرم .NET است.

برخلاف Serilog، این کتابخانه به عنوان یک پیاده‌سازی لاگ‌گیری **در نظر گرفته نشده است**.

در عوض، قرارداد استاندارد لاگ‌گیری را تعریف می‌کند که توسط کدهای برنامه مورد استفاده قرار می‌گیرد.

هدف اصلی آن، جداسازی کامپوننت‌های برنامه از هرگونه فریم‌ورک لاگ‌گیری خاص است.

---

## نقش معماری (Architectural Role)

در سامانه MachineryManagerEnterprise، کتابخانه `Microsoft.Extensions.Logging` نشان‌دهنده لایه انتزاع لاگ‌گیری است.

```text
Business Modules
        │
        ▼
ILogger<T>
        │
        ▼
Logging Provider
        │
 ┌──────────────┬───────────────┐
 │ Serilog      │ Future Provider│
 └──────────────┴───────────────┘
```

ماژول‌های کسب‌وکار هرگز از ارائه‌دهنده مورد استفاده مطلع نمی‌شوند.

تنها لایه انتزاع قابل مشاهده است.

---

## نقاط قوت معماری (Architectural Strengths)

### مزایا

- لایه انتزاع رسمی مایکروسافت؛
- پشتیبانی بومی در .NET 10؛
- یکپارچه‌سازی با تزریق وابستگی (Dependency Injection)؛
- استقلال از ارائه‌دهنده (Provider independence)؛
- رابط برنامه‌نویسی (API) فوق‌العاده پایدار؛
- حداقل جفت‌شدگی (Minimal coupling)؛
- قابلیت نگهداری بالا؛
- تست‌پذیری ساده؛
- پشتیبانی بلندمدت پلتفرم.

---

## نقاط ضعف معماری (Architectural Weaknesses)

این لایه انتزاع به طور آگاهانه کارکرد بسیار اندکی در خود دارد.

این لایه موارد زیر را ارائه **نمی‌دهد**:

- لاگ‌گیری ساختاریافته؛
- سینک‌ها و مقاصد خروجی؛
- غنی‌سازها (enrichers)؛
- صادرکننده تله‌متری (telemetry export)؛
- ذخیره‌سازی (storage).

این مسئولیت‌ها متعلق به ارائه‌دهنده لاگ‌گیری پیکربندی‌شده است.

---

## مشخصات عملیاتی (Operational Characteristics)

کتابخانه `Microsoft.Extensions.Logging` موارد زیر را فراهم می‌سازد:

- انتزاع لاگ‌گیری؛
- سطوح لاگ (log levels)؛
- دامنه‌ها (scopes)؛
- پشتیبانی از تزریق وابستگی.

پیچیدگی عملیاتی ناچیز است.

---

## مقیاس‌پذیری (Scalability)

از آنجایی که این صرفاً یک لایه انتزاع است، مقیاس‌پذیری کاملاً به ارائه‌دهنده پیکربندی‌شده وابسته است.

این لایه انتزاع عملاً هیچ سرباری در زمان اجرا ایجاد نمی‌کند.

---

## امنیت (Security)

این لایه انتزاع نه امنیت را ارتقا می‌دهد و نه کاهش می‌دهد.

امنیت به موارد زیر بستگی دارد:

- پیکربندی ارائه‌دهنده؛
- سیاست‌های لاگ‌گیری؛
- کدهای برنامه.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

در هر محیطی که .NET 10 اجرا شود پشتیبانی می‌گردد:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- On-Premise
- Hybrid

انعطاف‌پذیری استقرار عالی است.

---

## سازگاری با هوش مصنوعی (AI Compatibility)

این لایه انتزاع مستقل از ارائه‌دهنده است.

بنابراین، کامپوننت‌های هوش مصنوعی دقیقاً مانند هر کامپوننت دیگر برنامه از طریق `ILogger<T>` لاگ ثبت می‌کنند.

این امر موارد زیر را حاصل می‌کند:

- عیب‌یابی یکپارچه و منسجم؛
- ارائه‌دهندگان قابل تعویض؛
- ابزار دقیق‌سازی ساده‌شده.

---

## قابلیت نگهداری (Maintainability)

قابلیت نگهداری برجسته ارزیابی می‌شود زیرا:

- رابط‌های برنامه‌نویسی پایدار هستند؛
- مایکروسافت سازگاری را تضمین می‌کند؛
- ارائه‌دهنده‌ها می‌توانند بدون تغییر در کدهای کسب‌وکار تغییر یابند.

---

## تناسب با MachineryManagerEnterprise (Suitability for MachineryManagerEnterprise)

این لایه انتزاع کاملاً با اصول زیر هم‌راستا است:

- Clean Architecture؛
- Dependency Inversion؛
- Infrastructure Isolation.

هر کامپوننت کسب‌وکار باید منحصراً به `ILogger<T>` وابسته باشد.

---

## تطابق معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| Clean Architecture | عالی (Excellent) |
| Provider Independence | عالی (Excellent) |
| Maintainability | عالی (Excellent) |
| Enterprise Readiness | عالی (Excellent) |
| Performance | عالی (Excellent) |

---

## رابطه با Serilog (Relationship with Serilog)

کتابخانه `Microsoft.Extensions.Logging` رقیبی برای Serilog **نیست**.

در عوض:

```text
Business Code
        │
ILogger<T>
        │
        ▼
Serilog
```

مسئولیت‌ها تفکیک‌شده باقی می‌مانند.

| فناوری | مسئولیت |
|------------|----------------|
| `ILogger<T>` | لایه انتزاع لاگ‌گیری (Logging Abstraction) |
| Serilog | پیاده‌سازی لاگ‌گیری ساختاریافته (Structured Logging Implementation) |

---

## رابطه با OpenTelemetry (Relationship with OpenTelemetry)

```text
Business Code
        │
ILogger<T>
        │
        ▼
Serilog
        │
        ▼
OpenTelemetry
```

هر کامپوننت یک مسئولیت مشخص را انجام می‌دهد.

---

## نتیجه‌گیری مقدماتی (Preliminary Conclusion)

کتابخانه `Microsoft.Extensions.Logging` باید به تنها لایه انتزاع لاگ‌گیری ارجاع‌شده در سراسر MachineryManagerEnterprise تبدیل شود.

ماژول‌های کسب‌وکار هرگز نباید به موارد زیر ارجاع داشته باشند:

- Serilog؛
- OpenTelemetry؛
- هرگونه ارائه‌دهنده لاگ‌گیری.

در عوض، هر کامپوننت باید منحصراً به `ILogger<T>` وابسته باشد.

این امر حداکثر انعطاف‌پذیری معماری را ضمن حفظ جداسازی دقیق زیرساخت تضمین می‌کند.

---

# 8. Prometheus Evaluation

## نمای کلی (Overview)

فناوری Prometheus پلتفرم متن‌باز استاندارد صنعت برای جمع‌آوری متریک‌ها است که به طور گسترده در محیط‌های ابری و کوبرنتیز استفاده می‌شود.

برخلاف فریم‌ورک‌های لاگ‌گیری، Prometheus به جای رویدادهای لاگ، **متریک‌های سری زمانی (time-series metrics)** را ذخیره می‌کند.

سناریوهای متداول کاربرد سازمانی شامل موارد زیر است:

- پایش زیرساخت (infrastructure monitoring)؛
- متریک‌های برنامه (application metrics)؛
- سلامت سرویس (service health)؛
- پایش عملکرد (performance monitoring)؛
- برنامه‌ریزی ظرفیت (capacity planning)؛
- تولید هشدار (alert generation).

در سامانه MachineryManagerEnterprise، فناوری Prometheus به عنوان بک‌اند اصلی ذخیره‌سازی متریک‌ها برای OpenTelemetry ارزیابی می‌شود.

---

## نقش معماری (Architectural Role)

فناوری Prometheus متعلق به لایه متریک‌ها (Metrics layer) است.

```text
Business Modules
        │
        ▼
OpenTelemetry Metrics
        │
        ▼
Prometheus
(Time-Series Metrics Store)
```

ماژول‌های کسب‌وکار هرگز مستقیماً با Prometheus ارتباط برقرار نمی‌کنند.

متریک‌ها از ابزار دقیق OpenTelemetry سرچشمه می‌گیرند.

---

## نقاط قوت معماری (Architectural Strengths)

### مزایا

- متن‌باز (Open Source)؛
- سازگار با ابر (Cloud native)؛
- یکپارچه‌سازی عالی با کوبرنتیز؛
- بهینه‌سازی‌شده برای داده‌های سری زمانی؛
- عملکرد و کارایی بالا؛
- اکوسیستم قدرتمند؛
- سازگاری بومی با OpenTelemetry؛
- پشتیبانی غنی از هشدارهای هوشمند؛
- زبان کوئری‌گیری بالغ (PromQL)؛
- مقیاس‌پذیری عالی.

---

## نقاط ضعف معماری (Architectural Weaknesses)

فناوری Prometheus به طور آگاهانه بر متریک‌ها تمرکز دارد.

این فناوری موارد زیر را ارائه **نمی‌دهد**:

- لاگ‌گیری (logging)؛
- ردگیری توزیع‌شده (distributed tracing)؛
- داشبوردها (dashboards)؛
- ذخیره‌سازی رویدادهای ساختاریافته.

این مسئولیت‌ها متعلق به کامپوننت‌های مکمل مشاهده‌پذیری است.

---

## مشخصات عملیاتی (Operational Characteristics)

فناوری Prometheus موارد زیر را فراهم می‌کند:

- جمع‌آوری متریک‌ها مبتنی بر کشش (pull-based metrics collection)؛
- پایگاه داده سری زمانی؛
- قواعد هشدار (alert rules)؛
- زبان PromQL؛
- کشف سرویس (service discovery)؛
- تجمیع متریک‌ها (metric aggregation).

پیچیدگی عملیاتی متوسط ارزیابی می‌شود.

---

## مقیاس‌پذیری (Scalability)

فناوری Prometheus به خوبی برای بارهای کاری سازمانی مقیاس می‌پذیرد.

مدل‌های استقرار پشتیبانی‌شده شامل موارد زیر است:

- مستقل (standalone)؛
- فدراسیون (federation)؛
- کوبرنتیز (Kubernetes)؛
- استقرارهای ابری (cloud deployments).

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

## امنیت (Security)

استقرارهای سازمانی باید موارد زیر را پیکربندی کنند:

- TLS؛
- احراز هویت (authentication)؛
- ایزولاسیون شبکه (network isolation)؛
- صادرکننده‌های امن (secure exporters).

قابلیت‌های امنیتی نیازمندی‌های سازمانی را برآورده می‌سازند.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده عبارتند از:

- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

انعطاف‌پذیری استقرار عالی است.

---

## سازگاری با هوش مصنوعی (AI Compatibility)

فناوری Prometheus امکان پایش بارهای کاری هوش مصنوعی شامل موارد زیر را فراهم می‌سازد:

- تاخیر استنتاج (inference latency)؛
- زمان تولید امبدینگ (embedding generation time)؛
- نرخ اصابت حافظه موقت (cache hit ratio)؛
- متریک‌های مصرف توکن (token consumption metrics)؛
- آمارهای اجرای مدل (model execution statistics)؛
- تاخیر بازیابی معنایی (semantic retrieval latency).

این متریک‌ها برای بهینه‌سازی عملکرد هوش مصنوعی ضروری هستند.

---

## قابلیت نگهداری (Maintainability)

فناوری Prometheus ویژگی‌های زیر را به نمایش می‌گذارد:

- مستندات استثنایی؛
- اکوسیستم بالغ؛
- پایداری بلندمدت؛
- پذیرش گسترده در جامعه کاربری.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

## تناسب با MachineryManagerEnterprise (Suitability for MachineryManagerEnterprise)

فناوری Prometheus تمامی نیازمندی‌های شناسایی‌شده برای متریک‌ها را پوشش می‌دهد از جمله:

- عملکرد برنامه؛
- پایش زیرساخت؛
- پایش بارهای کاری هوش مصنوعی؛
- پایش حافظه موقت (cache monitoring)؛
- متریک‌های پیام‌رسانی؛
- متریک‌های پایگاه داده.

---

## تطابق معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| Clean Architecture | عالی (Excellent) |
| Metrics Collection | عالی (Excellent) |
| OpenTelemetry Integration | عالی (Excellent) |
| Enterprise Readiness | عالی (Excellent) |
| Cloud Native | عالی (Excellent) |
| Maintainability | عالی (Excellent) |

---

## رابطه با OpenTelemetry (Relationship with OpenTelemetry)

فناوری Prometheus مکمل OpenTelemetry است.

```text
Business Code
        │
OpenTelemetry Metrics
        │
        ▼
Prometheus
```

مسئولیت‌ها تفکیک‌شده باقی می‌مانند.

| فناوری | مسئولیت |
|------------|----------------|
| OpenTelemetry | ابزار دقیق‌سازی متریک‌ها (Metrics Instrumentation) |
| Prometheus | ذخیره‌سازی متریک‌ها (Metrics Storage) |

---

## رابطه با Serilog (Relationship with Serilog)

```text
Observability
        │
 ┌──────────────┬──────────────┐
 ▼              ▼
Serilog     Prometheus
Logs         Metrics
```

لاگ‌ها و متریک‌ها به عنوان جریان‌های تله‌متری مستقل از یکدیگر باقی می‌مانند.

---

## نتیجه‌گیری مقدماتی (Preliminary Conclusion)

فناوری Prometheus باید به پلتفرم استاندارد ذخیره‌سازی متریک‌ها برای MachineryManagerEnterprise تبدیل شود.

کدهای برنامه هرگز نباید مستقیماً با Prometheus ارتباط برقرار کنند.

تمامی متریک‌ها باید از طریق ابزار دقیق OpenTelemetry صادر شده و به Prometheus ارسال گردند.

این معماری استقلال از تامین‌کننده را حفظ کرده و در عین حال پایش عملیاتی در سطح سازمانی را فراهم می‌سازد.

---

# 9. Grafana Evaluation

## نمای کلی (Overview)

فناوری Grafana پلتفرم مصورسازی استاندارد صنعت برای پایش عملیاتی است.

برخلاف Prometheus که متریک‌ها را ذخیره می‌کند، Grafana داشبوردهای تعاملی را فراهم می‌سازد که داده‌های جمع‌آوری‌شده از چندین بک‌اند تله‌متری مختلف را مصورسازی می‌کنند.

منابع داده (Data Sources) پشتیبانی‌شده عبارتند از:

- Prometheus؛
- OpenTelemetry؛
- Elasticsearch؛
- OpenSearch؛
- PostgreSQL؛
- Loki؛
- Tempo؛
- Jaeger.

در سامانه MachineryManagerEnterprise، فناوری Grafana به عنوان پلتفرم اصلی داشبوردهای مشاهده‌پذیری ارزیابی می‌شود.

---

## نقش معماری (Architectural Role)

فناوری Grafana متعلق به لایه مصورسازی (Visualization layer) است.

```text
Business Modules
        │
        ▼
OpenTelemetry
        │
 ┌───────────────┬───────────────┬──────────────┐
 ▼               ▼               ▼
Metrics         Logs           Traces
        │
        ▼
Prometheus / Loki / Tempo
        │
        ▼
Grafana
```

فناوری Grafana هرگز مستقیماً با کدهای برنامه ارتباط برقرار نمی‌کند.

---

## نقاط قوت معماری (Architectural Strengths)

### مزایا

- متن‌باز (Open Source)؛
- قابلیت‌های استثنایی در ساخت داشبورد؛
- پشتیبانی عالی از کوبرنتیز؛
- سازگاری بومی با OpenTelemetry؛
- پشتیبانی از چندین منبع داده همزمان؛
- مصورسازی هشدارها؛
- اکوسیستم غنی از افزونه‌ها (plugins)؛
- پذیرش گسترده سازمانی؛
- رابط کاربری عالی؛
- داشبوردهای پایش هوش مصنوعی؛
- قابلیت سفارشی‌سازی بسیار بالا.

---

## نقاط ضعف معماری (Architectural Weaknesses)

فناوری Grafana به طور آگاهانه صرفاً مصورسازی ارائه می‌دهد.

این فناوری موارد زیر را انجام نمی‌دهد:

- جمع‌آوری تله‌متری؛
- ذخیره‌سازی متریک‌ها؛
- ذخیره‌سازی لاگ‌ها؛
- ذخیره‌سازی ردگیری‌ها.

این مسئولیت‌ها به بک‌اندهای تله‌متری واگذار شده باقی می‌مانند.

---

## مشخصات عملیاتی (Operational Characteristics)

فناوری Grafana موارد زیر را فراهم می‌آورد:

- داشبوردها؛
- مصورسازی؛
- تحلیل عمیق و ریشه‌ای (drill-down analysis)؛
- داشبوردهای هشدار؛
- متغیرها (variables)؛
- حاشیه‌نویسی‌ها (annotations)؛
- پنل‌های با قابلیت استفاده مجدد.

پیچیدگی عملیاتی پایین است.

---

## مقیاس‌پذیری (Scalability)

فناوری Grafana در محیط‌های زیر به شکل کارآمدی مقیاس می‌پذیرد:

- استقرارهای سازمانی؛
- کلاسترهای کوبرنتیز؛
- محیط‌های ابری؛
- استقرارهای ترکیبی.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

## امنیت (Security)

قابلیت‌های سازمانی شامل موارد زیر است:

- احراز هویت؛
- یکپارچه‌سازی با ورود یکپارچه (SSO)؛
- مجوزدهی مبتنی بر نقش (RBAC)؛
- سطوح دسترسی داشبوردها؛
- قابلیت‌های حسابرسی (Audit).

پشتیبانی امنیتی در سطح سازمانی است.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده عبارتند از:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- On-Premise
- Hybrid

انعطاف‌پذیری استقرار عالی است.

---

## سازگاری با هوش مصنوعی (AI Compatibility)

فناوری Grafana به ویژه برای پایش بارهای کاری هوش مصنوعی بسیار ارزشمند است.

داشبوردهای نمونه شامل موارد زیر هستند:

- تاخیر استنتاج (inference latency)؛
- زمان تولید امبدینگ (embedding generation time)؛
- تاخیر بازیابی معنایی (semantic retrieval latency)؛
- زمان اجرای پرامپت (prompt execution time)؛
- مصرف توکن (token consumption)؛
- میزان استفاده از مدل‌ها (model usage)؛
- دسترسی‌پذیری سرویس هوش مصنوعی.

این داشبوردها بینش عملیاتی دقیقی از رفتار هوش مصنوعی ارائه می‌دهند.

---

## قابلیت نگهداری (Maintainability)

فناوری Grafana ویژگی‌های زیر را به نمایش می‌گذارد:

- اکوسیستم بالغ؛
- مستندات عالی؛
- پذیرش گسترده در جامعه کاربری؛
- پایداری بلندمدت.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

## تناسب با MachineryManagerEnterprise (Suitability for MachineryManagerEnterprise)

فناوری Grafana تمامی نیازمندی‌های مصورسازی را برآورده می‌سازد از جمله:

- داشبوردهای عملیاتی؛
- پایش زیرساخت؛
- پایش هوش مصنوعی؛
- داشبوردهای حافظه موقت؛
- داشبوردهای پیام‌رسانی؛
- داشبوردهای پایگاه داده؛
- شاخص‌های کلیدی عملکرد کسب‌وکار (KPIs).

---

## تطابق معماری (Architectural Fit)

| معیار | ارزیابی |
|-----------|------------|
| Clean Architecture | عالی (Excellent) |
| Visualization | عالی (Excellent) |
| OpenTelemetry Integration | عالی (Excellent) |
| Enterprise Readiness | عالی (Excellent) |
| Cloud Native | عالی (Excellent) |
| Maintainability | عالی (Excellent) |

---

## رابطه با Prometheus (Relationship with Prometheus)

فناوری‌های Prometheus و Grafana فناوری‌های مکمل یکدیگر هستند.

```text
OpenTelemetry
        │
        ▼
Prometheus
        │
        ▼
Grafana
```

مسئولیت‌ها کاملاً تفکیک‌شده باقی می‌مانند.

| فناوری | مسئولیت |
|------------|----------------|
| Prometheus | ذخیره‌سازی متریک‌ها (Metrics Storage) |
| Grafana | مصورسازی متریک‌ها (Metrics Visualization) |

---

## رابطه با OpenTelemetry (Relationship with OpenTelemetry)

فناوری OpenTelemetry همچنان تولیدکننده تله‌متری باقی می‌ماند.

فناوری Grafana هرگز کدهای برنامه را ابزار دقیق‌سازی نمی‌کند.

```text
Application
        │
        ▼
OpenTelemetry
        │
        ▼
Telemetry Backend
        │
        ▼
Grafana
```

این تفکیک، استقلال زیرساختی را حفظ می‌کند.

---

## نتیجه‌گیری مقدماتی (Preliminary Conclusion)

فناوری Grafana باید به پلتفرم استاندارد مصورسازی برای MachineryManagerEnterprise تبدیل شود.

این پلتفرم به صورت طبیعی با OpenTelemetry، Prometheus، Loki و Tempo یکپارچه می‌شود و در عین حال داشبوردهای سازمانی برای دید عملیاتی کامل فراهم می‌آورد.

فناوری Grafana نشان‌دهنده پلتفرم مصورسازی ترجیحی برای هر دو نوع بارهای کاری سنتی سازمانی و سرویس‌های مبتنی بر هوش مصنوعی است.

---

# 10. Jaeger Evaluation

## نمای کلی (Overview)

فناوری Jaeger یک پلتفرم متن‌باز برای ردگیری توزیع‌شده (distributed tracing) است که در ابتدا توسط Uber توسعه یافت و اکنون تحت نظارت بنیاد محاسبات ابری بومی (CNCF) نگهداری می‌شود.

فناوری Jaeger منحصراً بر **ردگیری توزیع‌شده** تمرکز دارد.

برخلاف:

- Serilog → لاگ‌ها
- Prometheus → متریک‌ها
- Grafana → مصورسازی

فناوری Jaeger ردگیری‌های درخواست‌ها را در سراسر سیستم‌های توزیع‌شده ذخیره و مصورسازی می‌کند.

سناریوهای متداول سازمانی شامل موارد زیر است:

- مصورسازی جریان درخواست‌ها (request flow visualization)؛
- عیب‌یابی سرویس‌های توزیع‌شده؛
- تحلیل تاخیر (latency analysis)؛
- تحلیل وابستگی‌ها (dependency analysis)؛
- شناسایی گلوگاه‌ها (bottleneck identification)؛
- بررسی و تحلیل خرابی‌ها (failure investigation).

در سامانه MachineryManagerEnterprise، فناوری Jaeger به عنوان یکی از گزینه‌های کاندید برای بک‌اند ردگیری OpenTelemetry ارزیابی می‌شود.

---

# Architectural Role

فناوری Jaeger متعلق به لایه ذخیره‌سازی ردگیری (Trace Storage layer) است.

```text
Application
        │
        ▼
OpenTelemetry
        │
        ▼
Trace Exporter
        │
        ▼
Jaeger
```

ماژول‌های کسب‌وکار هرگز مستقیماً با Jaeger ارتباط برقرار نمی‌کنند.

---

# Architectural Strengths

## Advantages

- پروژه CNCF؛
- اکوسیستم بالغ؛
- سازگاری عالی با OpenTelemetry؛
- ردگیری توزیع‌شده عالی؛
- مصورسازی خط زمانی درخواست‌ها (Request timeline visualization)؛
- تولید گراف وابستگی‌ها (Dependency graph generation)؛
- سربار پایین؛
- یکپارچه‌سازی عالی با کوبرنتیز؛
- سازگار با ابر (Cloud native).

---

# Architectural Weaknesses

فناوری Jaeger به طور آگاهانه صرفاً ردگیری توزیع‌شده ارائه می‌دهد.

این فناوری موارد زیر را فراهم **نمی‌کند**:

- متریک‌ها؛
- لاگ‌گیری ساختاریافته؛
- داشبوردها؛
- ارسال هشدار (alerting).

استفاده از پلتفرم‌های تکمیلی همچنان ضروری خواهد بود.

---

# Operational Characteristics

فناوری Jaeger موارد زیر را فراهم می‌آورد:

- ردگیری‌های توزیع‌شده؛
- اسپن‌ها (spans)؛
- روابط والد-فرزندی (parent-child relationships)؛
- گراف‌های وابستگی؛
- تحلیل تاخیر؛
- جستجوی ردگیری‌ها (trace search).

پیچیدگی عملیاتی متوسط ارزیابی می‌شود.

---

# Scalability

فناوری Jaeger از موارد زیر پشتیبانی می‌کند:

- استقرار توزیع‌شده؛
- کوبرنتیز؛
- مقیاس‌پذیری بومی ابری.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# Security

استقرارهای سازمانی از موارد زیر پشتیبانی می‌کنند:

- احراز هویت؛
- انتقال رمزنگاری‌شده؛
- جمع‌آورنده‌های امن (secure collectors)؛
- ایزولاسیون نقش‌ها.

قابلیت‌های امنیتی نیازمندی‌های سازمانی را برآورده می‌سازند.

---

# Deployment Flexibility

محیط‌های پشتیبانی‌شده عبارتند از:

- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

انعطاف‌پذیری استقرار عالی است.

---

# AI Compatibility

فناوری Jaeger برای عیب‌یابی و پایش هوش مصنوعی بسیار ارزشمند است.

نمونه‌ها عبارتند از:

- ردگیری اجرای پرامپت‌ها؛
- ردگیری تولید امبدینگ‌ها؛
- ردگیری بازیابی معنایی؛
- ردگیری خط لوله‌های استنتاج (inference pipelines)؛
- ردگیری ارائه‌دهندگان خارجی هوش مصنوعی.

ردگیری توزیع‌شده به طور چشمگیری عیب‌یابی هوش مصنوعی را ساده می‌کند.

---

# Maintainability

فناوری Jaeger ویژگی‌های زیر را به نمایش می‌گذارد:

- پشتیبانی بالغ CNCF؛
- مستندات گسترده؛
- معماری پایدار؛
- پذیرش گسترده سازمانی.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# Suitability for MachineryManagerEnterprise

فناوری Jaeger تمامی نیازمندی‌های ردگیری توزیع‌شده را برآورده می‌سازد.

این فناوری دید عملیاتی فوق‌العاده‌ای را در بخش‌های زیر فراهم می‌کند:

- لایه API؛
- پردازش پس‌زمینه (Background Processing)؛
- پیام‌رسانی (Messaging)؛
- سرویس‌های هوش مصنوعی (AI services)؛
- یکپارچه‌سازی‌های خارجی (External integrations).

---

# Architectural Fit

| معیار | ارزیابی |
|-----------|------------|
| Clean Architecture | عالی (Excellent) |
| Distributed Tracing | عالی (Excellent) |
| OpenTelemetry Integration | عالی (Excellent) |
| Enterprise Readiness | عالی (Excellent) |
| Maintainability | عالی (Excellent) |

---

# Relationship with OpenTelemetry

فناوری OpenTelemetry ردگیری‌ها را تولید می‌کند.

فناوری Jaeger ردگیری‌ها را ذخیره می‌کند.

```text
Business Code
        │
        ▼
OpenTelemetry
        │
        ▼
Jaeger
```

مسئولیت‌ها کاملاً تفکیک‌شده باقی می‌مانند.

---

# Relationship with Grafana

فناوری Jaeger می‌تواند به صورت مستقل عمل کند.

همچنین Grafana می‌تواند ردگیری‌های Jaeger را مصورسازی کند.

```text
OpenTelemetry
        │
        ▼
Jaeger
        │
        ▼
Grafana
```

---

# Preliminary Conclusion

فناوری Jaeger یک پلتفرم عالی برای ردگیری توزیع‌شده به شمار می‌رود.

این فناوری نیازمندی‌های ردگیری MachineryManagerEnterprise را به طور کامل برآورده می‌سازد.

با این حال، پلتفرم‌های جدیدتر مشاهده‌پذیری به طور فزاینده‌ای ردگیری‌ها را همراه با لاگ‌ها و متریک‌ها ادغام می‌کنند که پیچیدگی عملیاتی را کاهش می‌دهد.

این موضوع در بخش بعدی ارزیابی می‌شود.

---

# 11. Grafana Tempo Evaluation

## نمای کلی (Overview)

فناوری Grafana Tempo یک بک‌اند مدرن برای ردگیری توزیع‌شده است که به طور خاص برای OpenTelemetry طراحی شده است.

برخلاف Jaeger، فناوری Tempo به طور آگاهانه برای یکپارچه‌سازی با اکوسیستم مشاهده‌پذیری Grafana بهینه‌سازی شده است.

تمرکز Tempo بر موارد زیر است:

- ذخیره‌سازی مقیاس‌پذیر ردگیری‌ها؛
- سازگاری با OpenTelemetry؛
- استقرارهای سازگار با ابر؛
- ساده‌سازی عملیات.

---

# Architectural Role

فناوری Tempo دقیقاً همان لایه معماری Jaeger را اشغال می‌کند.

```text
Application
        │
        ▼
OpenTelemetry
        │
        ▼
Grafana Tempo
```

---

# Architectural Strengths

## Advantages

- بک‌اند بومی OpenTelemetry؛
- یکپارچه‌سازی عالی با Grafana؛
- سازگار با ابر (Cloud native)؛
- پیچیدگی عملیاتی پایین؛
- مقیاس‌پذیری عالی؛
- پشتیبانی از ذخیره‌سازی شیءمحور (object storage)؛
- مستقل از تامین‌کننده (Vendor-neutral)؛
- سازگار با کوبرنتیز؛
- معماری ذخیره‌سازی کارآمد.

---

# Architectural Weaknesses

فناوری Tempo منحصراً بر ذخیره‌سازی ردگیری تمرکز دارد.

مصورسازی کاملاً وابسته به Grafana است.

این فناوری به طور آگاهانه بسیاری از قابلیت‌های سمت کاربر که مستقیماً در Jaeger وجود دارد را حذف کرده است.

---

# Operational Characteristics

فناوری Tempo موارد زیر را فراهم می‌آورد:

- دریافت ردگیری‌ها (trace ingestion)؛
- ذخیره‌سازی مقیاس‌پذیر ردگیری‌ها؛
- یکپارچه‌سازی با فضای ذخیره‌سازی شیءمحور؛
- سازگاری با OpenTelemetry.

پیچیدگی عملیاتی پایین ارزیابی می‌شود.

---

# Scalability

فناوری Tempo فوق‌العاده عالی مقیاس می‌پذیرد.

استقرارهای پشتیبانی‌شده شامل موارد زیر است:

- کوبرنتیز؛
- فضای ذخیره‌سازی شیءمحور ابری؛
- استقرارهای ترکیبی.

مقیاس‌پذیری عالی است.

---

# Security

فناوری Tempo از موارد زیر پشتیبانی می‌کند:

- انتقال رمزنگاری‌شده؛
- احراز هویت؛
- صادرکننده‌های امن.

استقرار در سطح سازمانی به طور کامل پشتیبانی می‌شود.

---

# AI Compatibility

فناوری Tempo از ردگیری موارد زیر پشتیبانی می‌کند:

- استنتاج هوش مصنوعی (AI inference)؛
- تولید امبدینگ (embedding generation)؛
- بازیابی معنایی (semantic retrieval)؛
- اجرای پرامپت (prompt execution)؛
- سرویس‌های خارجی هوش مصنوعی.

---

# Maintainability

از آنجایی که Tempo مستقیماً در اکوسیستم Grafana ادغام می‌شود:

- تلاش برای نگهداری کمتر است؛
- ابزارهای عملیاتی یکپارچه هستند؛
- داشبوردها به کار یکپارچه‌سازی کمتری نیاز دارند.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# Suitability for MachineryManagerEnterprise

فناوری Tempo به طور طبیعی با موارد زیر یکپارچه می‌شود:

- OpenTelemetry؛
- Prometheus؛
- Grafana؛
- Loki.

این اکوسیستم یکپارچه به طور قابل ملاحظه‌ای مشاهده‌پذیری سازمانی را ساده می‌کند.

---

# Architectural Fit

| معیار | ارزیابی |
|-----------|------------|
| Clean Architecture | عالی (Excellent) |
| Distributed Tracing | عالی (Excellent) |
| OpenTelemetry Integration | عالی (Excellent) |
| Cloud Native | عالی (Excellent) |
| Operational Simplicity | عالی (Excellent) |
| Maintainability | عالی (Excellent) |

---

# Comparison: Jaeger vs Tempo

| قابلیت | Jaeger | Grafana Tempo |
|------------|---------|---------------|
| Distributed Tracing | عالی (Excellent) | عالی (Excellent) |
| OpenTelemetry | عالی (Excellent) | عالی (Excellent) |
| Grafana Integration | بسیار خوب (Very Good) | عالی (Excellent) |
| Operational Simplicity | خوب (Good) | عالی (Excellent) |
| Storage Efficiency | خوب (Good) | عالی (Excellent) |
| Cloud Native | عالی (Excellent) | عالی (Excellent) |
| CNCF Maturity | عالی (Excellent) | عالی (Excellent) |

---

# Preliminary Conclusion

هر دو فناوری نیازمندی‌های ردگیری را برآورده می‌سازند.

با این حال، MachineryManagerEnterprise در حال حاضر موارد زیر را برمی‌گزیند:

- OpenTelemetry؛
- Prometheus؛
- Grafana.

انتخاب Tempo به پلتفرم اجازه می‌دهد تا بر روی یک اکوسیستم مشاهده‌پذیری مبتنی بر Grafana استانداردسازی شود.

در نتیجه، فناوری Tempo نشان‌دهنده بک‌اند ترجیحی برای ردگیری‌های توزیع‌شده است.

---

# 12. Overall Technology Comparison

مشاهده‌پذیری مدرن به جای یک محصول منفرد، از چندین فناوری مکمل یکدیگر تشکیل شده است.

هر فناوری یک مسئولیت با تعریف کاملاً مشخص را بر عهده دارد.

---

## Responsibility Matrix

| قابلیت | فناوری پیشنهادی | جایگزین | مسئولیت |
|------------|------------------------|-------------|----------------|
| Logging Abstraction | Microsoft.Extensions.Logging | — | Provider Independence |
| Structured Logging | Serilog | NLog | Structured Event Generation |
| Telemetry Standard | OpenTelemetry | — | Unified Telemetry |
| Metrics Storage | Prometheus | VictoriaMetrics | Time-Series Metrics |
| Dashboard Platform | Grafana | Kibana | Visualization |
| Trace Storage | Grafana Tempo | Jaeger | Distributed Tracing |

---

## Capability Comparison

| قابلیت | ILogger | Serilog | OpenTelemetry | Prometheus | Grafana | Jaeger | Tempo |
|------------|---------|----------|----------------|------------|----------|---------|--------|
| Logging | لایه انتزاع (Abstraction) | عالی (Excellent) | خوب (Good) | خیر (No) | خیر (No) | خیر (No) | خیر (No) |
| Structured Events | خیر (No) | عالی (Excellent) | خوب (Good) | خیر (No) | خیر (No) | خیر (No) | خیر (No) |
| Metrics | خیر (No) | خیر (No) | عالی (Excellent) | عالی (Excellent) | نمایش (View) | خیر (No) | خیر (No) |
| Distributed Traces | خیر (No) | خیر (No) | عالی (Excellent) | خیر (No) | نمایش (View) | عالی (Excellent) | عالی (Excellent) |
| Dashboards | خیر (No) | خیر (No) | خیر (No) | خیر (No) | عالی (Excellent) | محدود (Limited) | از طریق Grafana |
| AI Diagnostics | خوب (Good) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| Vendor Independence | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| Cloud Native | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| Operational Complexity | بسیار پایین (Very Low) | پایین (Low) | پایین (Low) | متوسط (Medium) | پایین (Low) | متوسط (Medium) | پایین (Low) |

---

# 13. Recommended Observability Architecture

این ارزیابی پذیرش یک معماری لایه‌بندی‌شده مشاهده‌پذیری را پیشنهاد می‌دهد.

```text
                         Application
                              │
                              ▼
                     Microsoft.Extensions.Logging
                              │
                              ▼
                           Serilog
                              │
                              ▼
                        OpenTelemetry SDK
          ┌───────────────────┼───────────────────┐
          ▼                   ▼                   ▼
        Logs               Metrics             Traces
          │                   │                   │
          ▼                   ▼                   ▼
        Serilog          Prometheus         Grafana Tempo
              └──────────────┬──────────────┘
                             ▼
                          Grafana
                    Unified Dashboards
```

این معماری موارد زیر را به وضوح تفکیک می‌کند:

- ابزار دقیق‌سازی (instrumentation)؛
- انتقال و جابجایی (transport)؛
- ذخیره‌سازی (storage)؛
- مصورسازی (visualization).

---

# 14. Architectural Responsibilities

## Microsoft.Extensions.Logging

مسئول موارد زیر است:

- انتزاع لاگ‌گیری (logging abstraction)؛
- وارونگی وابستگی (dependency inversion)؛
- استقلال از ارائه‌دهنده (provider independence).

---

## Serilog

مسئول موارد زیر است:

- تولید لاگ‌های ساختاریافته (structured log generation)؛
- غنی‌سازی (enrichment)؛
- فرمت‌بندی لاگ‌ها (log formatting)؛
- سینک‌ها و مقاصد لاگ (log sinks).

---

## OpenTelemetry

مسئول موارد زیر است:

- ابزار دقیق‌سازی تله‌متری (telemetry instrumentation)؛
- انتشار زمینه (context propagation)؛
- شناسه‌های همبستگی (correlation identifiers)؛
- متریک‌ها (metrics)؛
- ردگیری‌ها (traces)؛
- صدور تله‌متری (telemetry export).

---

## Prometheus

مسئول موارد زیر است:

- ذخیره‌سازی متریک‌ها (metrics storage)؛
- تجمیع متریک‌ها (metric aggregation)؛
- ارزیابی هشدارها (alert evaluation).

---

## Grafana

مسئول موارد زیر است:

- داشبوردها (dashboards)؛
- مصورسازی (visualization)؛
- تحلیل عملیاتی (operational analysis)؛
- داشبوردهای پایش هوش مصنوعی (AI monitoring dashboards).

---

## Grafana Tempo

مسئول موارد زیر است:

- ذخیره‌سازی ردگیری‌های توزیع‌شده (distributed trace storage)؛
- کوئری‌گیری از ردگیری‌ها (trace querying)؛
- تحلیل جریان درخواست‌ها (request flow analysis).

---

# 15. Architectural Principles

پلتفرم پیشنهادی مشاهده‌پذیری تمامی اهداف اصلی معماری را برآورده می‌سازد.

| اصل | ارزیابی |
|-----------|------------|
| Clean Architecture | ✓ |
| Dependency Inversion | ✓ |
| Infrastructure Isolation | ✓ |
| Provider Independence | ✓ |
| Deployment Independence | ✓ |
| Cloud Neutrality | ✓ |
| AI Readiness | ✓ |
| Enterprise Readiness | ✓ |

---

# 16. Operational Flow

```text
Application Request
        │
        ▼
OpenTelemetry
        │
 ┌────────────┬────────────┬─────────────┐
 ▼            ▼            ▼
Logs       Metrics       Traces
 ▼            ▼            ▼
Serilog   Prometheus    Tempo
        └───────────────┬───────────────┘
                        ▼
                     Grafana
```

هر سیگنال تله‌متری یک زمینه همبستگی یکسان را به اشتراک می‌گذارد.

---

# 17. AI Observability

این پلتفرم باید بارهای کاری هوش مصنوعی شامل موارد زیر را پایش کند:

- اجرای پرامپت (prompt execution)؛
- تاخیر استنتاج (inference latency)؛
- تولید امبدینگ (embedding generation)؛
- بازیابی معنایی (semantic retrieval)؛
- تاخیر جستجوی برداری (vector search latency)؛
- مصرف توکن (token usage)؛
- کارایی حافظه موقت (cache efficiency)؛
- خطاهای مدل (model failures).

مشاهده‌پذیری به عنوان یک قابلیت معماری درجه یک در نظر گرفته می‌شود و نه یک موضوع ثانویه عملیاتی.

---

# 18. Risks

| ریسک | راهکار کاهش ریسک |
|------|------------|
| عدم همبستگی تله‌متری (Missing telemetry correlation) | انتشار زمینه در OpenTelemetry (OpenTelemetry Context Propagation) |
| رشد حجم لاگ‌ها (Log growth) | لاگ‌گیری ساختاریافته و سیاست‌های نگهداری (retention policies) |
| انفجار کاردینالیتی متریک‌ها (Metric cardinality explosion) | دستورالعمل‌های طراحی متریک‌ها (Metric design guidelines) |
| رشد ذخیره‌سازی ردگیری‌ها (Trace storage growth) | سیاست‌های نگهداری و فشرده‌سازی در Tempo (Retention policies and Tempo compaction) |
| افت عملکرد هوش مصنوعی (AI performance degradation) | داشبوردهای اختصاصی هوش مصنوعی و متریک‌های تاخیر (Dedicated AI dashboards and latency metrics) |

---

# 19. Final Recommendation

سامانه MachineryManagerEnterprise باید استک مشاهده‌پذیری زیر را استانداردسازی کند:

| مسئولیت | فناوری انتخاب‌شده |
|----------------|---------------------|
| Logging Abstraction | Microsoft.Extensions.Logging |
| Structured Logging | Serilog |
| Telemetry Standard | OpenTelemetry |
| Metrics Backend | Prometheus |
| Dashboard Platform | Grafana |
| Trace Backend | Grafana Tempo |

فناوری Jaeger در جاهایی که استانداردهای سازمانی از قبل آن را الزامی کرده باشد، به عنوان یک جایگزین تصویب‌شده باقی می‌ماند.

---

# 20. Final Decision

**معماری تصویب‌شده (Approved Architecture)**

این پلتفرم باید موارد زیر را اتخاذ کند:

- کتابخانه `Microsoft.Extensions.Logging` به عنوان تنها لایه انتزاع لاگ‌گیری.
- فریم‌ورک `Serilog` به عنوان ارائه‌دهنده لاگ‌گیری ساختاریافته.
- استاندارد `OpenTelemetry` به عنوان استاندارد یکپارچه تله‌متری.
- سامانه `Prometheus` به عنوان بک‌اند متریک‌ها.
- سامانه `Grafana` به عنوان پلتفرم مصورسازی.
- سامانه `Grafana Tempo` به عنوان بک‌اند ردگیری توزیع‌شده.

ماژول‌های کسب‌وکار هرگز نباید به طور مستقیم به هیچ پیاده‌سازی مشاهده‌پذیری وابسته باشند.

تمامی دغدغه‌های مشاهده‌پذیری باید به طور ایزوله در لایه زیرساخت (Infrastructure layer) باقی بمانند.

---

# تصمیم معماری مرتبط (Related Architecture Decision)

- ADR-0033 — معماری مشاهده‌پذیری سازمانی (Enterprise Observability Architecture)

---

# خلاصه تصمیم (Decision Summary)

استک فناوری انتخاب‌شده تمامی نیازمندی‌های معماری را برآورده می‌سازد.

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# ADRهای مرتبط (Related ADR)

- ADR-0001 — معماری پاک (Clean Architecture)
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
| 1.0.0 | 2026-07-26 | معمار راهکار (Solution Architect) | ارزیابی اولیه فناوری برای مشاهده‌پذیری و تله‌متری |
| 1.3.0 | 2026-07-28 | معمار راهکار (Solution Architect) | بخش جدید اضافه شد (دامنه ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار (Solution Architect) | ارتقا به استاندارد مستندسازی نسخه v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار (Solution Architect) | بازبینی و همگام‌سازی با آخرین تغییرات |