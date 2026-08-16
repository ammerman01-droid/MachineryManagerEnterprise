| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0019 |
| **عنوان** | ارزیابی فناوری پردازش پس‌زمینه و زمان‌بندی وظایف (.NET 10) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-26 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند به ارزیابی فناوری‌های کاندید برای پردازش پس‌زمینه و زمان‌بندی وظایف (Background Processing and Job Scheduling Technology Evaluation (.NET 10)) در سامانه MachineryManagerEnterprise می‌پردازد.

هدف، ایجاد یک راهکار یکپارچه برای پردازش کارهای پس‌زمینه، کارهای تکرارشونده و زمان‌بندی در سطح سازمانی است؛ به گونه‌ای که با اصول معماری پاک (Clean Architecture) کاملاً سازگار باشد.

---

# محدوده ارزیابی (Evaluation Scope)

این ارزیابی فناوری صرفاً انتخاب فناوری را مورد سنجش قرار می‌دهد.
جزئیات پیاده‌سازی توسط سوابق تصمیمات معماری (ADRs) مربوطه تعریف می‌شوند.

---

# مراجع معماری (Architectural References)

این ارزیابی بر پایه مراجع زیر استوار است:
- ADR-0001 — معماری پاک (Clean Architecture)
- ADR-0012 — معماری پیام‌رسانی سازمانی (Enterprise Messaging Architecture)
- ADR-0015 — معماری استقرار (Deployment Architecture)

معماری پردازش پس‌زمینه باید موارد زیر را برآورده سازد:
- استقلال کامل از ارائه‌دهنده (provider independent)؛
- استقلال کامل از نحوه استقرار (deployment independent)؛
- قابلیت اطمینان و پایداری بالا؛
- قابلیت ردیابی و نظارت بر وظایف.

---

# نیازمندی‌های کارکردی (Functional Requirements)

این پلتفرم نیازمند پشتیبانی از موارد زیر است:
- وظایف اجرا و رها کن (Fire-and-forget jobs)؛
- وظایف با تأخیر زمانی (Delayed jobs)؛
- وظایف تکرارشونده و دوره‌ای (Recurring jobs / Cron)؛
- زنجیره وظایف متوالی (Continuation jobs)؛
- تلاش مجدد خودکار در صورت خطا (Automatic retries)؛
- ماندگاری و ذخیره وضعیت وظایف (Job persistence)؛
- تاریخچه و لاگ اجرای وظایف (Job execution history)؛
- داشبورد مانیتورینگ و مدیریت وظایف (Monitoring dashboard)؛
- وظایف پس‌زمینه طولانی‌مدت (Long-running background workers)؛
- لغو ایمن وظایف (Graceful cancellation).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

راهکار انتخابی باید موارد زیر را فراهم آورد:
- قابلیت اطمینان در سطح سازمانی (Enterprise reliability)؛
- مقیاس‌پذیری افقی (Horizontal scalability)؛
- سازگاری با معماری پاک (Clean Architecture compatibility)؛
- استقلال از پلتفرم و بی‌طرفی نسبت به ابر (Cloud neutrality)؛
- سادگی عملیاتی (Operational simplicity)؛
- سربار کم در مصرف منابع؛
- سازگاری کامل و بومی با .NET 10.

---

# فناوری‌های کاندید (Candidate Technologies)

## پردازش پس‌زمینه بومی (Native Background Processing)

| فناوری | نقش |
|---|---|
| BackgroundService | ورکر میزبانی‌شده (Hosted Worker) |
| IHostedService | ورکر پس‌زمینه (Background Worker) |

---

## زمان‌بندی وظایف (Job Scheduling)

| فناوری | نقش |
|---|---|
| Hangfire | وظایف پس‌زمینه با ماندگاری بالا (Persistent Background Jobs) |
| Quartz.NET | زمان‌بند سازمانی (Enterprise Scheduler) |

---

## جایگزین‌های ابری (Cloud Alternatives)

| فناوری | نقش |
|---|---|
| Azure Functions | پردازش پس‌زمینه ابری (Cloud Background Processing) |
| Kubernetes CronJobs | زمان‌بند زیرساختی (Infrastructure Scheduler) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|---|---|---|
| B1 | سازگاری با معماری پاک (Clean Architecture Compatibility) | حیاتی (Critical) |
| B2 | قابلیت اطمینان (Reliability) | حیاتی (Critical) |
| B3 | ماندگاری داده‌ها و وضعیت (Persistence) | حیاتی (Critical) |
| B4 | مقیاس‌پذیری (Scalability) | بالا (High) |
| B5 | سادگی عملیاتی (Operational Simplicity) | بالا (High) |
| B6 | پشتیبانی از داشبورد مدیریتی (Dashboard Support) | متوسط (Medium) |
| B7 | سازگاری با هوش مصنوعی (AI Compatibility) | بالا (High) |
| B8 | یکپارچگی با .NET 10 | حیاتی (Critical) |

---

# اصل معماری (Architecture Principle)

ماژول‌های تجاری هرگز نباید مستقیماً وظایف را زمان‌بندی کنند یا به کتابخانه‌های خاص وابسته باشند.
در عوض:

```text
Business Modules
        │
        ▼
Background Processing Abstraction
        │
        ▼
Infrastructure Provider
        │ 
┌──────────────┬──────────────┐ 
▼              ▼
Hangfire     BackgroundService
```

منطق تجاری برنامه به طور کامل از فناوری زمان‌بندی و پردازش پس‌زمینه ایزوله باقی می‌ماند.

---

# 5. ارزیابی BackgroundService / IHostedService

## نمای کلی (Overview)

فناوری BackgroundService انتزاع رسمی مایکروسافت برای پیاده‌سازی ورکرهای پس‌زمینه طولانی‌مدت در دات‌نت است.
این قابلیت بر روی IHostedService ساخته شده و مستقیماً با Generic Host معرفی‌شده در نسخه‌های مدرن دات‌نت یکپارچه می‌شود.
ابزار BackgroundService برای فرآیندهای مداوم در حال اجرا طراحی شده است، نه وظایف زمان‌بندی‌شده و ماندگار.

---

# 5. ارزیابی BackgroundService / IHostedService

## نمای کلی (Overview)

کلاس `BackgroundService` انتزاع رسمی مایکروسافت برای پیاده‌سازی ورکرهای پس‌زمینه به صورت پیوسته در دات‌نت است.
این ویژگی بر پایه `IHostedService` بنا شده و مستقیماً با .NET Generic Host یکپارچه است.
برخلاف زمان‌بندهای سازمانی، BackgroundService برای **اجرای مداوم (continuous execution)** طراحی شده است نه ارکستراسیون وظایف ماندگار.

بارهای کاری معمول عبارتند از:
- مصرف‌کنندگان صف‌ها (queue consumers)؛
- نگهداری و پاک‌سازی کش (cache maintenance)؛
- همگام‌سازی سلامت سیستم؛
- تجمیع تله‌متری (telemetry aggregation)؛
- فرآیندهای طولانی‌مدت از نوع دیمن (long-running daemon processes).

---

# نقش معماری (Architectural Role)

کلاس BackgroundService به لایه زیرساخت (Infrastructure layer) تعلق دارد.

```text
Business Modules
        │
        ▼
Background Processing Abstraction
        │
        ▼
BackgroundService
        │
        ▼
Hosted Worker
```

ماژول‌های تجاری هرگز از BackgroundService ارث‌بری نمی‌کنند.
لایه زیرساخت مالک پیاده‌سازی ورکر است.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- پیاده‌سازی رسمی و استاندارد مایکروسافت.
- پشتیبانی بومی از .NET 10.
- یکپارچگی کامل با Generic Host.
- پشتیبانی بومی از تزریق وابستگی (Dependency Injection).
- بسیار سبک و بهینه (Lightweight).
- کارایی و عملکرد فوق‌العاده بالا (High performance).
- ایده‌آل برای ورکرهای دیمن (daemon-style workers).
- سازگاری عالی با محیط‌های کانتینری.
- عدم نیاز به هیچ‌گونه زیرساخت یا پایگاه داده خارجی.

---

# نقاط ضعف معماری (Architectural Weaknesses)

ابزار BackgroundService به عمد صرفاً زیرساخت اجرای فرآیند را فراهم می‌کند.
این ابزار موارد زیر را **ارائه نمی‌دهد**:
- وظایف ماندگار در پایگاه داده (persistent jobs)؛
- زمان‌بندی پیشرفته (scheduling)؛
- مدیریت تلاش مجدد خودکار (automatic retries)؛
- داشبورد مدیریتی و مانیتورینگ؛
- اجرای توزیع‌شده بین چندین سرور؛
- ثبت تاریخچه وظایف؛
- ماندگارسازی وضعیت وظایف.

این مسئولیت‌ها نیازمند فریم‌ورک‌های اختصاصی زمان‌بندی هستند.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

کلاس BackgroundService از موارد زیر پشتیبانی می‌کند:
- اجرای مداوم و پیوسته؛
- توکن‌های لغو عملیات (CancellationToken)؛
- خاموش‌سازی ایمن و کنترل‌شده (graceful shutdown)؛
- تزریق وابستگی؛
- چرخه حیات میزبانی‌شده (hosted lifecycle).

پیچیدگی عملیاتی آن بی‌نهایت ناچیز است.

---

# مقیاس‌پذیری (Scalability)

کلاس BackgroundService به صورت طبیعی با ساختارهای زیر مقیاس می‌پذیرد:
- برنامه‌های ASP.NET Core؛
- سرویس‌های کارگر (.NET Worker Services)؛
- کانتینرها (Containers)؛
- کوبرنتیز (Kubernetes).

مقیاس‌پذیری افقی به توپولوژی استقرار سیستم بستگی دارد.

---

# قابلیت اطمینان (Reliability)

قابلیت اطمینان کاملاً به پیاده‌سازی کد برنامه وابسته است.
خود فریم‌ورک موارد زیر را فراهم می‌سازد:
- شروع به کار کنترل‌شده (graceful startup)؛
- پایان کار ایمن (graceful shutdown)؛
- پشتیبانی از لغو فرآیندها.

این ابزار تلاش مجدد خودکار یا ماندگاری در دیتابیس را ارائه نمی‌دهد.

---

# امنیت (Security)

کلاس BackgroundService هیچ‌گونه نگرانی امنیتی اضافه‌ای تحمیل نمی‌کند.
امنیت همچنان توسط موارد زیر هدایت می‌شود:
- تزریق وابستگی؛
- پیکربندی زیرساخت؛
- منطق خود برنامه کاربردی.

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

کلاس BackgroundService برای بارهای کاری هوش مصنوعی که به صورت مداوم اجرا می‌شوند بسیار مناسب است، مانند:
- صف‌های تولید امبدینگ (embedding generation queues)؛
- ایندکس‌گذاری معنایی مداوم؛
- همگام‌سازی بردارها؛
- تازه‌سازی کش مدل‌های هوش مصنوعی؛
- پردازش پس‌زمینه اسناد ورودی.

با این حال، جریان‌های کاری پایدار هوش مصنوعی که نیازمند تلاش مجدد یا زمان‌بندی تقویمی هستند فراتر از محدوده طراحی این کلاس می‌باشند.

---

# قابلیت نگهداری (Maintainability)

از آنجا که BackgroundService بخشی از پلتفرم رسمی دات‌نت است:
- مستندات آن فوق‌العاده است؛
- بار نگهداری آن در کمترین حد ممکن است؛
- پایداری فریم‌ورک بسیار بالاست.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# کاربرد معمول (Typical Usage)

بارهای کاری مناسب:
```text
Queue Consumers
Telemetry Aggregation
Cache Maintenance
Message Listeners
Synchronization Workers
AI Indexing Daemons
```

بارهای کاری نامناسب:
```text
Recurring Reports
Scheduled Jobs
Persistent Workflows
Retry-Based Processing
Job Dashboard
Distributed Scheduling
```

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| سادگی (Simplicity) | عالی (Excellent) |
| کارایی و سرعت (Performance) | عالی (Excellent) |
| زمان‌بندی (Scheduling) | ضعیف (Poor) |
| ماندگاری (Persistence) | ضعیف (Poor) |
| جریان‌های کاری سازمانی | متوسط (Moderate) |

---

# ارتباط با زمان‌بندهای سازمانی (Relationship with Enterprise Schedulers)

کلاس BackgroundService مکمل پلتفرم‌های زمان‌بندی سازمانی است، نه جایگزین آن‌ها.

```text
Continuous Workers
        │
BackgroundService
----------------------------
Persistent Jobs
        │
Hangfire / Quartz
```

هر فناوری به یک هدف معماری کاملاً متفاوت خدمت می‌کند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

کلاس BackgroundService باید به پیاده‌سازی استاندارد برای ورکرهای زیرساختی با اجرای مداوم در MachineryManagerEnterprise تبدیل شود.
اما این ابزار به عنوان راهکار زمان‌بندی وظایف سازمانی **کافی نیست**.
بارهای کاری ماندگار، زمان‌بندی‌شده، دارای تلاش مجدد و پایدار نیازمند یک زمان‌بند اختصاصی هستند که در بخش‌های بعدی ارزیابی می‌شوند.

---

# 6. ارزیابی Hangfire (Hangfire Evaluation)

## نمای کلی (Overview)

فریم‌ورک Hangfire یکی از پرکاربردترین و محبوب‌ترین ابزارهای پردازش کارهای پس‌زمینه در اکوسیستم دات‌نت است.
برخلاف `BackgroundService`، فریم‌ورک Hangfire مشخصاً برای **اجرای وظایف ماندگار (persistent job execution)** طراحی شده است.

این پلتفرم امکانات زیر را فراهم می‌آورد:
- وظایف پس‌زمینه با ماندگاری بالا؛
- اجرای با تأخیر (delayed execution)؛
- کارهای تکرارشونده و دوره‌ای (recurring jobs)؛
- تلاش‌های مجدد خودکار (automatic retries)؛
- ذخیره‌سازی وضعیت در پایگاه داده؛
- داشبورد نظارتی کامل.

در سامانه MachineryManagerEnterprise، فریم‌ورک Hangfire به عنوان پلتفرم اصلی پردازش وظایف سازمانی ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

فریم‌ورک Hangfire به لایه زمان‌بندی وظایف (Job Scheduling layer) تعلق دارد.

```text
Business Modules
        │
        ▼
Background Job Abstraction
        │
        ▼
Hangfire
        │
        ▼
Persistent Storage
```

ماژول‌های تجاری هرگز مستقیماً با Hangfire ارتباط برقرار نمی‌کنند.
لایه زیرساخت پیاده‌سازی زمان‌بندی را بر عهده دارد.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- اکوسیستم بسیار بالغ و آزموده‌شده.
- یکپارچگی عالی با اکوسیستم دات‌نت.
- وظایف کاملاً ماندگار در دیتابیس (بدون از دست رفتن کارها با ریستارت برنامه).
- تلاش مجدد خودکار با الگوریتم‌های پس‌زدگی نمایی (Exponential backoff).
- پشتیبانی کامل از کارهای تکرارشونده (Cron).
- اجرای وظایف با تأخیر زمانی مشخص.
- زنجیره‌سازی وظایف متوالی (Continuations).
- داشبورد بصری و قدرتمند تحت وب برای پایش و مدیریت وظایف.
- پشتیبانی از مخازن مختلف: SQL Server، PostgreSQL و Redis.
- پذیرش گسترده در جامعه برنامه‌نویسان.
- مستندات غنی و جامع.

---

# نقاط ضعف معماری (Architectural Weaknesses)

فریم‌ورک Hangfire نیازمند یک پایگاه داده ماندگار است.
ملاحظات معمول عبارتند از:
- وابستگی به پایگاه داده ذخیره‌سازی؛
- نیاز به ایمن‌سازی دسترسی به داشبورد؛
- مدیریت و نگهداری داده‌های ذخیره‌شده.

اگرچه استقرار آن ساده است، اما نیازمندی‌های عملیاتی آن بیشتر از BackgroundService ساده است.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

فریم‌ورک Hangfire از قابلیت‌های زیر پشتیبانی می‌کند:
- کارهای اجرا و رها کن (Fire-and-forget)؛
- کارهای دارای تأخیر زمانی (Delayed jobs)؛
- کارهای تکرارشونده و منظم (Recurring jobs)؛
- کارهای زنجیره‌ای (Continuation jobs)؛
- تلاش مجدد هوشمند در صورت بروز استثنا؛
- ورکرهای توزیع‌شده بر روی چند سرور؛
- داشبورد مانیتورینگ لحظه‌ای.

پیچیدگی عملیاتی آن پایین ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

فریم‌ورک Hangfire قابلیت‌های زیر را پشتیبانی می‌کند:
- چند ورکر همزمان بر روی چندین سرور یا کانتینر؛
- پردازش توزیع‌شده؛
- استقرار در بسترهای کوبرنتیز؛
- استقرار در محیط‌های ابری.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

قابلیت اطمینان یکی از قوی‌ترین نقاط قوت Hangfire است.
قابلیت‌ها عبارتند از:
- ذخیره‌سازی ماندگار وضعیت‌ها؛
- تلاش مجدد خودکار؛
- بازیابی از کارافتادگی‌ها و خرابی سرور (Crash recovery)؛
- تضمین اجرای حداقل یک‌بار وظایف؛
- ثبت دقیق تاریخچه اجرا.

قابلیت اطمینان عالی ارزیابی می‌شود.

---

# امنیت (Security)

استقرار سازمانی باید موارد زیر را ایمن سازد:
- داشبورد مدیریتی Hangfire (احراز هویت و مجوزها)؛
- دسترسی به پایگاه داده وضعیت؛
- مجوزهای دسترسی ورکرها.

خود فریم‌ورک نقاط توسعه مناسبی برای پیکربندی‌های امنیتی ارائه می‌دهد.

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

فریم‌ورک Hangfire برای بارهای کاری هوش مصنوعی فوق‌العاده ارزشمند است، از جمله:
- تولید بردارهای امبدینگ (embedding generation)؛
- ایندکس‌گذاری اسناد در پایگاه‌های دانش؛
- همگام‌سازی بردارهای داده در پایگاه‌های وکتور؛
- فراخوانی‌های سنگین مدل‌های زبانی با امکان تلاش مجدد خودکار در صورت قطعی شبکه یا محدودیت نرخ (rate limits)؛
- پاک‌سازی و نگهداری دوره‌ای کش‌های هوش مصنوعی.

---

# قابلیت نگهداری (Maintainability)

به دلیل شفافیت بالا در پایش وضعیت‌ها و وجود داشبورد وب، ردیابی خطاهای وظایف بسیار سریع انجام شده و هزینه نگهداری سیستم کاهش می‌یابد.

---

# داشبورد مدیریتی (Dashboard)

داشبورد تعبیه‌شده Hangfire امکان مشاهده وضعیت، آمار لحظه‌ای، کارهای در صف، کارهای ناموفق و امکان اجرای دستی مجدد (Retry) را با یک کلیک فراهم می‌آورد.

---

# کاربرد معمول (Typical Usage)

بارهای کاری مناسب:
```text
Email Notifications
Document Generation
Report Processing
AI Embedding Generation
Vector Index Sync
Data Import / Export
Retry-Sensitive Workflows
Recurring Data Synchronization
```

بارهای کاری نامناسب:
```text
Low-Latency Streaming
Real-Time Message Bus
Continuous Streaming
```

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| قابلیت اطمینان (Reliability) | عالی (Excellent) |
| ماندگاری داده‌ها (Persistence) | عالی (Excellent) |
| زمان‌بندی (Scheduling) | عالی (Excellent) |
| داشبورد مدیریتی (Dashboard) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |

---

# ارتباط با BackgroundService (Relationship with BackgroundService)

هر دو فناوری مکمل یکدیگر هستند:

```text
Continuous Processing
        │
BackgroundService
----------------------------
Durable Jobs
        │
Hangfire
```

هر فناوری مسئولیت متفاوتی را بر دوش دارد.

---

# ارتباط با سیستم پیام‌رسانی (Relationship with Messaging)

```text
Message Bus
        │
        ▼
Background Job
        │
        ▼
Hangfire
```

سیستم پیام‌رسانی می‌تواند وظایف پس‌زمینه را تحریک کند در حالی که Hangfire اجرای ماندگار و مطمئن را مدیریت می‌نماید.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

فریم‌ورک Hangfire یک پلتفرم فوق‌العاده در سطح سازمانی برای پردازش کارهای پس‌زمینه است.
این فریم‌ورک نیازمندی‌های سیستم برای اجرای ماندگار، تلاش مجدد، زمان‌بندی و دید عملیاتی را کاملاً برآورده می‌سازد.
این گزینه کاندیدای اصلی پلتفرم زمان‌بندی وظایف برای MachineryManagerEnterprise به شمار می‌رود.

---

# 7. ارزیابی Quartz.NET (Quartz.NET Evaluation)

## نمای کلی (Overview)

فریم‌ورک Quartz.NET پورت دات‌نتی فریم‌ورک زمان‌بندی سازمانی Quartz از اکوسیستم جاوا است.
برخلاف Hangfire که تمرکز اصلی آن بر روی پردازش کارهای ماندگار پس‌زمینه است، Quartz.NET در **زمان‌بندی پیشرفته (advanced scheduling)** تخصص دارد.

این فریم‌ورک امکانات زیر را ارائه می‌دهد:
- زمان‌بندی پیشرفته مبتنی بر Cron؛
- تقویم‌های کاری و تعطیلات (Calendars)؛
- ساختارهای سلسله‌مراتبی تریگرها (Trigger hierarchies)؛
- زمان‌بندی در سطح سازمانی؛
- کلاسترهای زمان‌بندی توزیع‌شده؛
- سیاست‌های اجرای به شدت قابل سفارشی‌سازی.

Quartz.NET از دیرباز در سیستم‌های سازمانی که نیازمند قابلیت‌های زمان‌بندی پیچیده هستند به کار رفته است.

---

# نقش معماری (Architectural Role)

فریم‌ورک Quartz.NET به لایه زمان‌بندی سازمانی (Enterprise Scheduling layer) تعلق دارد.

```text
Business Modules
        │
        ▼
Scheduling Abstraction
        │
        ▼
Quartz.NET
        │
        ▼
Scheduler Engine
```

ماژول‌های تجاری از APIهای اختصاصی Quartz ایزوله باقی می‌مانند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- زمان‌بند سازمانی بسیار بالغ و کهنه‌کار.
- پشتیبانی فوق‌العاده از الگوهای پیچیده Cron.
- مدل تریگر بسیار منعطف.
- پشتیبانی از تقویم‌های کاری و استثنائات تقویمی.
- اجرای کلاستربندی‌شده (Clustered execution).
- ذخیره و ماندگارسازی زمان‌بندی‌ها.
- پیکربندی‌پذیری فوق‌العاده بالا.
- سازگاری خوب با دات‌نت.
- متن‌باز با پایداری بلندمدت.

---

# نقاط ضعف معماری (Architectural Weaknesses)

فریم‌ورک Quartz.NET بیشتر بر روی زمان‌بندی تمرکز دارد تا مدیریت جریان‌های کاری پس‌زمینه.
در مقایسه با Hangfire:
- فاقد داشبورد عملیاتی داخلی و آماده است؛
- تجربه توسعه‌دهندگی آن پیچیده‌تر و کمتر شهودی است؛
- منحنی یادگیری تندتری دارد؛
- جریان تلاش مجدد خطای آن یکپارچگی کمتری دارد.

پیکربندی عملیاتی آن به طور کلی پیچیده‌تر است.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

امکانات پشتیبانی‌شده عبارتند از:
- زمان‌بندی کرون (Cron)؛
- زمان‌بندی بازه‌ای (Interval)؛
- مدیریت تقویم‌ها؛
- تریگرهای ماندگار در پایگاه داده؛
- زمان‌بندهای کلاسترشده؛
- اجرای توزیع‌شده.

پیچیدگی عملیاتی در سطح متوسط ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

پلتفرم Quartz.NET از موارد زیر پشتیبانی می‌کند:
- کلاسترینگ؛
- زمان‌بندهای ماندگار؛
- استقرار توزیع‌شده؛
- کوبرنتیز.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

پلتفرم Quartz.NET امکانات زیر را فراهم می‌سازد:
- برنامه‌ریزی‌های ماندگار؛
- اجرای کلاستربندی‌شده؛
- ذخیره‌سازی داده‌ها در پایگاه داده؛
- مدیریت عدم اجرای به موقع (Misfire handling)؛
- بازیابی خودکار.

قابلیت اطمینان عالی ارزیابی می‌شود.

---

# امنیت (Security)

پلتفرم Quartz.NET نگرانی امنیتی غیرمعمولی تحمیل نمی‌کند.
امنیت عملیاتی عمدتاً متمرکز بر موارد زیر است:
- پایگاه داده زمان‌بند؛
- ارتباطات کلاستر؛
- مجوزهای زیرساختی.

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

پلتفرم Quartz.NET برای سناریوهای زمان‌بندی قابل پیش‌بینی هوش مصنوعی مناسب است، از جمله:
- بازسازی شبانه امبدینگ‌ها؛
- بازآموزی برنامه‌ریزی‌شده؛
- پاک‌سازی زمان‌بندی‌شده؛
- همگام‌سازی دوره‌ای.

با این حال، جریان‌های کاری پویای هوش مصنوعی به طور کلی از مدل اجرای وظیفه‌محور Hangfire بهره بیشتری می‌برند.

---

# قابلیت نگهداری (Maintainability)

پلتفرم Quartz.NET امکانات زیر را ارائه می‌دهد:
- معماری بالغ؛
- واسط‌های برنامه‌نویسی پایدار؛
- انعطاف‌پذیری گسترده در زمان‌بندی.

با این حال، تلاش مورد نیاز برای پیکربندی و نگهداری بیشتر از Hangfire است.
قابلیت نگهداری بسیار خوب ارزیابی می‌شود.

---

# کاربرد معمول (Typical Usage)

بارهای کاری مناسب:
```text
Nightly Batch Processing
Cron Scheduling
Periodic Synchronization
Monthly Maintenance
Quarterly Reporting
Enterprise Scheduling
```

بارهای کاری نامناسب:
```text
Interactive Background Jobs
User-triggered Jobs
AI Queue Processing
Retry-heavy Workflows
```

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| انعطاف‌پذیری زمان‌بندی (Scheduling Flexibility) | عالی (Excellent) |
| زمان‌بندی سازمانی (Enterprise Scheduling) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | متوسط (Moderate) |
| پشتیبانی از داشبورد (Dashboard Support) | محدود (Limited) |
| قابلیت نگهداری (Maintainability) | بسیار خوب (Very Good) |

---

# مقایسه با Hangfire (Comparison with Hangfire)

| قابلیت | Hangfire | Quartz.NET |
|---|---|---|
| وظایف ماندگار (Persistent Jobs) | عالی (Excellent) | عالی (Excellent) |
| زمان‌بندی با کرون (Cron Scheduling) | بسیار خوب (Very Good) | عالی (Excellent) |
| داشبورد مدیریتی (Dashboard) | عالی (Excellent) | محدود (Limited) |
| جریان تلاش مجدد (Retry Workflow) | عالی (Excellent) | خوب (Good) |
| تجربه توسعه‌دهنده (Developer Experience) | عالی (Excellent) | خوب (Good) |
| زمان‌بندی سازمانی (Enterprise Scheduling) | بسیار خوب (Very Good) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) | متوسط (Moderate) |

---

# ارتباط با BackgroundService (Relationship with BackgroundService)

```text
Continuous Workers
        │
BackgroundService
----------------------------
Scheduled Jobs
        │
Quartz.NET
```

کتابخانه Quartz.NET ورکرهای مداوم را تکمیل می‌کند نه جایگزین.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

پلتفرم Quartz.NET یک پلتفرم زمان‌بندی سازمانی فوق‌العاده است.
این ابزار به ویژه در سناریوهایی که معناشناسی پیچیده زمان‌بندی بر بار کاری حاکم است کاربرد دارد.
با این حال، سامانه MachineryManagerEnterprise علاوه بر زمان‌بندی، به موارد زیر نیز نیاز دارد:
- پردازش پس‌زمینه ماندگار؛
- تلاش‌های مجدد؛
- داشبوردهای عملیاتی؛
- جریان‌های کاری ناهمگام متمرکز بر هوش مصنوعی.

برای این سناریوها، Hangfire تعادل کلی بهتری از توانمندی‌ها را ارائه می‌دهد.

---

# 8. ارزیابی Azure Functions (Azure Functions Evaluation)

## نمای کلی (Overview)

سرویس Azure Functions پلتفرم رایانش بدون‌سرور (Serverless) شرکت مایکروسافت است.
برخلاف Hangfire و Quartz.NET، سرویس Azure Functions **یک فریم‌ورک زمان‌بندی نیست**؛ بلکه یک پلتفرم اجرای ابری است که توانایی اجرای بارهای کاری مبتنی بر رویداد (event-driven) و مبتنی بر تایمر را دارد.

انواع تریگرهای پشتیبانی‌شده عبارتند از:
- HTTP؛
- Timer؛
- Queue؛
- Event Grid؛
- Service Bus؛
- Blob Storage؛
- Cosmos DB؛
- Event Hub.

در سامانه MachineryManagerEnterprise، سرویس Azure Functions به عنوان یک جایگزین ابری خاص برای اجرای پس‌زمینه ارزیابی می‌شود.

---

# نقش معماری (Architectural Role)

سرویس Azure Functions به لایه اجرای ابری (Cloud Execution layer) تعلق دارد.

```text
Cloud Event
      │
      ▼
Azure Function
      │
      ▼
Business Service
```

ماژول‌های تجاری از Azure Functions بی‌اطلاع باقی می‌مانند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- کاملاً مدیریت‌شده (Fully managed).
- مقیاس‌پذیری خودکار (Automatic scaling).
- یکپارچگی بومی با Azure.
- قیمت‌گذاری بر اساس مصرف (Consumption pricing).
- تریگرهای تایمر (Timer triggers).
- تریگرهای صف (Queue triggers).
- معماری رویدادمحور (Event-driven architecture).
- بدون نیاز به مدیریت زیرساخت (No infrastructure management).
- یکپارچگی عالی با اکوسیستم Azure.

---

# نقاط ضعف معماری (Architectural Weaknesses)

سرویس Azure Functions وابستگی قابل توجهی به پلتفرم ایجاد می‌کند.
محدودیت‌های کلیدی عبارتند از:
- مدل اجرای اختصاصی Azure؛
- قفل شدن به ارائه‌دهنده (Vendor lock-in)؛
- وابستگی عملیاتی به Azure؛
- قابلیت حمل محدود استقرار؛
- رفتار شروع سرد (Cold-start) در پلن مصرفی.

این خصوصیات با اهداف معماری بی‌طرف نسبت به ابر MachineryManagerEnterprise در تضاد است.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

سرویس Azure Functions امکانات زیر را فراهم می‌کند:
- اجرای بدون‌سرور؛
- زمان‌بندی تایمری؛
- پردازش صف؛
- پردازش رویداد؛
- مقیاس‌پذیری خودکار.

پیچیدگی عملیاتی بسیار پایین ارزیابی می‌شود.

---

# مقیاس‌پذیری (Scalability)

مقیاس‌پذیری یکی از قدرتمندترین قابلیت‌های Azure Functions است.
ویژگی‌ها عبارتند از:
- مقیاس‌پذیری افقی خودکار (automatic scale-out)؛
- اجرای مبتنی بر مصرف؛
- زیرساخت کشسان و الاستیک؛
- مقیاس‌پذیری مبتنی بر رویداد.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

سرویس Azure Functions امکانات زیر را ارائه می‌دهد:
- اجرای مدیریت‌شده؛
- تلاش‌های مجدد خودکار (بسته به تریگر)؛
- زیرساخت ابری ارتجاعی؛
- دسترسی‌پذیری بالا.

قابلیت اطمینان عالی ارزیابی می‌شود.

---

# امنیت (Security)

قابلیت‌های امنیتی عبارتند از:
- هویت سازمانی Azure Active Directory / Microsoft Entra ID؛
- هویت مدیریت‌شده (Managed Identity)؛
- یکپارچگی با Azure Key Vault؛
- کنترل دسترسی مبتنی بر نقش (RBAC)؛
- ارتباطات رمزنگاری‌شده.

امنیت عالی ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های استقرار پشتیبانی‌شده:
- آژور (Azure)

پشتیبانی از:
- درون‌سازمانی (On-Premise)
- چندابری (Multi-Cloud)
- ترکیبی (Hybrid)
محدود است یا نیازمند زیرساخت‌های اضافی است.
بنابراین انعطاف‌پذیری استقرار در سطح متوسط ارزیابی می‌شود.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

سرویس Azure Functions برای موارد زیر بسیار مناسب است:
- پایانه‌های استنتاج هوش مصنوعی (AI inference endpoints)؛
- تولید امبدینگ؛
- پردازش برنامه‌ریزی‌شده هوش مصنوعی؛
- خطوط لوله رویدادمحور هوش مصنوعی.

هنگامی که بارهای کاری AI از پیش در Azure اجرا می‌شوند، Functions به صورت طبیعی یکپارچه می‌گردد.

---

# قابلیت نگهداری (Maintainability)

تلاش برای نگهداری در کمترین حد است، زیرا:
- زیرساخت مدیریت‌شده است؛
- مقیاس‌پذیری خودکار است؛
- مانیتورینگ با Azure یکپارچه است.

قابلیت نگهداری عالی ارزیابی می‌شود.

---

# کاربرد معمول (Typical Usage)

بارهای کاری مناسب:
```text
HTTP Endpoints
Queue Processing
Blob Processing
Scheduled Cleanup
Event Grid Processing
Cloud Integrations
```

بارهای کاری نامناسب:
```text
Portable Enterprise Scheduling
Cloud-Neutral Background Jobs
On-Premise Processing
Infrastructure-Independent Workflows
```

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| ابر بومی (Cloud Native) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | ضعیف (Poor) |
| استقلال از استقرار (Deployment Independence) | ضعیف (Poor) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) |

---

# ارتباط با Hangfire (Relationship with Hangfire)

```text
Portable Enterprise Jobs        │     Hangfire
----------------------------
Azure Cloud Execution        │ Azure Functions
```

فریم‌ورک Hangfire مستقل از استقرار باقی می‌ماند.
سرویس Azure Functions اجرای میزبانی‌شده در Azure را بهینه‌سازی می‌کند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

سرویس Azure Functions یک پلتفرم عالی برای اجرای پس‌زمینه بومی Azure به شمار می‌رود.
با این حال، سامانه MachineryManagerEnterprise صراحتاً اولویت‌های زیر را مد نظر دارد:
- استقلال از ارائه‌دهنده؛
- بی‌طرفی نسبت به ابر؛
- انعطاف‌پذیری استقرار.

در نتیجه، Azure Functions باید به عنوان یک مدل استقرار ابری اختیاری و خاص در نظر گرفته شود نه معماری اصلی پردازش پس‌زمینه سامانه.

---

# 9. ارزیابی Kubernetes CronJobs (Kubernetes CronJobs Evaluation)

## نمای کلی (Overview)

ابزار Kubernetes CronJobs زمان‌بندی در سطح زیرساخت را برای بارهای کاری کانتینری فراهم می‌سازد.
برخلاف Hangfire و Quartz.NET، ابزار Kubernetes CronJobs **عناصر پایه‌ای زمان‌بندی پلتفرم** هستند نه فریم‌ورک‌های برنامه‌نویسی.

هر اجرای زمان‌بندی‌شده یک Kubernetes Job ایجاد می‌کند که یک یا چند پاد (Pod) را برای انجام کار مورد نظر اجرا می‌نماید.
بنابراین CronJobs بیشتر برای بارهای کاری زمان‌بندی‌شده متمایل به زیرساخت مناسب هستند تا پردازش‌های پس‌زمینه تحت مدیریت برنامه کاربردی.

---

# نقش معماری (Architectural Role)

ابزار Kubernetes CronJobs به لایه زیرساخت استقرار (Deployment Infrastructure layer) تعلق دارد.

```text
Kubernetes Scheduler
          │
          ▼
     Kubernetes CronJob
          │
          ▼
      Kubernetes Job
          │
          ▼
   Application Container
```

خود برنامه از اینکه کوبرنتیز مسئول زمان‌بندی است بی‌اطلاع باقی می‌ماند.

---

# نقاط قوت معماری (Architectural Strengths)

## مزایا (Advantages)

- قابلیت بومی کوبرنتیز.
- بدون نیاز به فریم‌ورک زمان‌بندی اضافی.
- یکپارچگی عالی با کانتینرها.
- اجرای مدیریت‌شده توسط زیرساخت.
- مقیاس‌پذیری افقی.
- انعطاف‌پذیری و پایداری پلتفرم.
- عملکرد ابری نوین (Cloud-native).
- کانتینرهای کارگر مستقل.
- عالی برای خودکارسازی‌های عملیاتی.

---

# نقاط ضعف معماری (Architectural Weaknesses)

ابزار CronJobs **کانتینرها** را زمان‌بندی می‌کند نه متدها و وظایف درون برنامه را.
محدودیت‌ها عبارتند از:
- وابستگی به کوبرنتیز؛
- فاقد داشبورد برنامه‌ای؛
- فاقد جریان کاری تلاش مجدد در سطح برنامه؛
- فاقد تاریخچه وظایف در سطح برنامه؛
- فاقد ارکستراسیون در سطح منطق تجاری؛
- نامناسب برای وظایف پس‌زمینه تحریک‌شده توسط کاربر.

آن‌ها زمان‌بندی زیرساختی را حل می‌کنند نه مدیریت جریان‌های کاری سازمانی.

---

# ویژگی‌های عملیاتی (Operational Characteristics)

قابلیت‌های پشتیبانی‌شده عبارتند از:
- زمان‌بندی مبتنی بر Cron؛
- تلاش مجدد از طریق سیاست‌های Job؛
- سیاست‌های همزمانی (Concurrency policies)؛
- مهلت‌های زمانی اجرا؛
- محدودیت‌های تاریخچه اجرا.

پیچیدگی عملیاتی در صورتی که زیرساخت کوبرنتیز از پیش موجود باشد پایین است.

---

# مقیاس‌پذیری (Scalability)

ابزار CronJobs مقیاس‌پذیری کوبرنتیز را به ارث می‌برد.
قابلیت‌ها عبارتند از:
- زمان‌بندی در سطح کلاستر؛
- اجرای توزیع‌شده؛
- راه‌اندازی مجدد خودکار؛
- ایزوله‌سازی کانتینر.

مقیاس‌پذیری عالی ارزیابی می‌شود.

---

# قابلیت اطمینان (Reliability)

قابلیت اطمینان به اجرای Job در کوبرنتیز وابسته است.
قابلیت‌ها عبارتند از:
- تلاش مجدد برای جاب‌های ناموفق؛
- سیاست‌های راه‌اندازی مجدد پادها؛
- تاریخچه اجرا؛
- تطبیق توسط کنترلر (Controller reconciliation).

قابلیت اطمینان عالی ارزیابی می‌شود.

---

# امنیت (Security)

امنیت از الگوهای استاندارد کوبرنتیز پیروی می‌کند:
- کنترل دسترسی RBAC؛
- فضاهای نام (Namespaces)؛
- حساب‌های سرویس (Service accounts)؛
- اسرار کوبرنتیز (Secrets)؛
- سیاست‌های شبکه (Network policies).

امنیت عالی ارزیابی می‌شود.

---

# انعطاف‌پذیری استقرار (Deployment Flexibility)

محیط‌های پشتیبانی‌شده:
- Kubernetes
- AKS
- EKS
- GKE
- OpenShift
- On-Prem Kubernetes

خارج از محیط کوبرنتیز مناسب نیست.
بنابراین انعطاف‌پذیری استقرار در سطح متوسط ارزیابی می‌شود.

---

# سازگاری با هوش مصنوعی (AI Compatibility)

ابزار CronJobs برای نگهداری‌های زمان‌بندی‌شده هوش مصنوعی مناسب است، نظیر:
- بازسازی شبانه امبدینگ‌ها؛
- بهینه‌سازی پایگاه داده برداری؛
- همگام‌سازی اسناد؛
- تازه‌سازی دوره‌ای مدل‌ها.

جریان‌های کاری تعاملی هوش مصنوعی همچنان برای وظایف پس‌زمینه تحت مدیریت برنامه مناسب‌تر هستند.

---

# قابلیت نگهداری (Maintainability)

ابزار CronJobs موارد زیر را فراهم می‌سازد:
- زمان‌بندی تعریفی (Declarative scheduling)؛
- یکپارچگی زیرساخت؛
- سازگاری با GitOps؛
- چرخه حیات بومی کوبرنتیز.

قابلیت نگهداری در محیط‌های کوبرنتیز بسیار خوب ارزیابی می‌شود.

---

# کاربرد معمول (Typical Usage)

بارهای کاری مناسب:
```text
Nightly Database Backup
Scheduled Cleanup
Index Rebuild
Embedding Refresh
Vector Optimization
Infrastructure Maintenance
```

بارهای کاری نامناسب:
```text
User-triggered Jobs
Application Workflows
Retry-intensive Business Jobs
Interactive Background Tasks
```

---

# انطباق معماری (Architectural Fit)

| معیار | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | عالی (Excellent) |
| یکپارچگی با کوبرنتیز (Kubernetes Integration) | عالی (Excellent) |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | خوب (Good) |
| استقلال از استقرار (Deployment Independence) | متوسط (Moderate) |
| زمان‌بندی سازمانی (Enterprise Scheduling) | خوب (Good) |
| پشتیبانی از جریان‌های کاری پس‌زمینه | متوسط (Moderate) |

---

# ارتباط با Hangfire (Relationship with Hangfire)

```text
Application Jobs        │     Hangfire
----------------------------
Infrastructure Jobs        │
Kubernetes CronJobs
```

فریم‌ورک Hangfire وظایف تجاری برنامه را زمان‌بندی می‌کند.
ابزار CronJobs وظایف زیرساختی را زمان‌بندی می‌نماید.
هر دو فناوری می‌توانند بدون همپوشانی در کنار هم حضور داشته باشند.

---

# نتیجه‌گیری اولیه (Preliminary Conclusion)

ابزار Kubernetes CronJobs یک سازوکار زمان‌بندی عالی برای بارهای کاری سطح زیرساخت در محیط‌های کوبرنتیز فراهم می‌سازد.
با این حال، این ابزار جایگزینی برای پردازش پس‌زمینه در سطح برنامه‌های سازمانی نیست.
سامانه MachineryManagerEnterprise باید از CronJobs تنها در مواردی استفاده کند که زمان‌بندی به زیرساخت استقرار تعلق دارد نه جریان‌های کاری تجاری.

---

# 10. مقایسه کلی فناوری‌ها (Overall Technology Comparison)

پردازش پس‌زمینه در MachineryManagerEnterprise از چندین مدل اجرای مکمل تشکیل شده است.
هر فناوری به یک دغدغه عملیاتی خاص پاسخ می‌دهد.
هیچ فناوری منفردی برای تمامی انواع بارهای کاری بهینه نیست.

---

## ماتریس مسئولیت‌ها (Responsibility Matrix)

| قابلیت | فناوری توصیه‌شده | جایگزین | مسئولیت |
|---|---|---|---|
| ورکرهای مداوم | BackgroundService | IHostedService | سرویس‌های طولانی‌مدت (Long-running Services) |
| وظایف پس‌زمینه ماندگار | Hangfire | Quartz.NET | اجرای وظایف ماندگار (Persistent Job Execution) |
| زمان‌بندی سازمانی | Quartz.NET | Hangfire | زمان‌بندی پیشرفته (Advanced Scheduling) |
| پردازش بدون‌سرور ابری | Azure Functions | Azure WebJobs | اجرای بومی در بستر Azure |
| زمان‌بندی زیرساختی | Kubernetes CronJobs | زمان‌بند سیستم‌عامل | زمان‌بندی در سطح پلتفرم (Platform Scheduling) |

---

## مقایسه قابلیت‌ها (Capability Comparison)

| قابلیت | BackgroundService | Hangfire | Quartz.NET | Azure Functions | Kubernetes CronJobs |
|---|---|---|---|---|---|
| ورکرهای مداوم | عالی | ضعیف | ضعیف | متوسط | ضعیف |
| وظایف ماندگار | خیر | عالی | عالی | متوسط | متوسط |
| زمان‌بندی با کرون | ضعیف | بسیار خوب | عالی | عالی | عالی |
| تلاش مجدد خودکار | دستی | عالی | خوب | خوب | خوب |
| داشبورد مدیریتی | خیر | عالی | محدود | پرتال آژور | کوبرنتیز |
| اجرای توزیع‌شده | دستی | عالی | عالی | عالی | عالی |
| بی‌طرفی نسبت به ابر | عالی | عالی | عالی | ضعیف | خوب |
| پشتیبانی از کوبرنتیز | عالی | عالی | عالی | صرفاً آژور | عالی |
| سادگی عملیاتی | عالی | عالی | متوسط | عالی | خوب |
| بارهای کاری پس‌زمینه AI | خوب | عالی | بسیار خوب | خوب | خوب |

---

# 11. نگاشت بارهای کاری (Workload Mapping)

دسته‌بندی‌های مختلف بارهای کاری نیازمند فناوری‌های اجرای متفاوتی هستند:

| بار کاری | فناوری توصیه‌شده |
|---|---|
| مصرف‌کننده صف (Queue Consumer) | BackgroundService |
| شنونده پیام (Message Listener) | BackgroundService |
| ارسال ایمیل (Email Delivery) | Hangfire |
| پردازش اعلان‌ها (Notification Processing) | Hangfire |
| تولید گزارش‌ها (Report Generation) | Hangfire |
| گرم کردن کش (Cache Warming) | Hangfire |
| تولید امبدینگ هوش مصنوعی (AI Embedding Generation) | Hangfire |
| به‌روزرسانی ایندکس برداری (Vector Index Updates) | Hangfire |
| نگهداری برنامه‌ریزی‌شده (Scheduled Maintenance) | Quartz.NET / CronJobs |
| پاک‌سازی زیرساخت (Infrastructure Cleanup) | Kubernetes CronJobs |
| پردازش رویدادهای Azure | Azure Functions |

---

# 12. معماری توصیه‌شده پردازش پس‌زمینه (Recommended Background Processing Architecture)

```text
                     Business Modules
                           │
                           ▼
             Background Processing Abstraction
                           │
        ┌──────────────────┼────────────────────┐
        ▼                  ▼                    ▼
Continuous Workers    Durable Jobs      Infrastructure Jobs
        │                  │                    │
BackgroundService      Hangfire         Kubernetes CronJobs
                           │
                           ▼
                 Persistent Job Storage
```

این معماری لایه‌بندی‌شده موارد زیر را تفکیک می‌نماید:
- سرویس‌های پیوسته و مداوم؛
- جریان‌های کاری تجاری ماندگار؛
- زمان‌بندی وظایف زیرساختی.

---

# 13. اصول معماری (Architectural Principles)

معماری توصیه‌شده تمامی اهداف اصلی معماری را برآورده می‌سازد:

| اصل معماری | ارزیابی |
|---|---|
| معماری پاک (Clean Architecture) | ✓ |
| ایزوله‌سازی زیرساخت (Infrastructure Isolation) | ✓ |
| استقلال از استقرار (Deployment Independence) | ✓ |
| استقلال از ارائه‌دهنده (Provider Independence) | ✓ |
| بی‌طرفی نسبت به ابر (Cloud Neutrality) | ✓ |
| قابلیت اطمینان سازمانی (Enterprise Reliability) | ✓ |
| آمادگی برای هوش مصنوعی (AI Readiness) | ✓ |
| قابلیت نگهداری (Maintainability) | ✓ |

---

# 14. استراتژی پردازش پس‌زمینه هوش مصنوعی (AI Background Processing Strategy)

بارهای کاری هوش مصنوعی اغلب به صورت ناهمگام اجرا می‌شوند.
کارهای معمول پس‌زمینه AI عبارتند از:
- تولید امبدینگ (embedding generation)؛
- همگام‌سازی بردارها؛
- ایندکس‌گذاری معنایی؛
- پیش‌پردازش اسناد؛
- نگهداری برنامه‌ریزی‌شده مدل‌های هوش مصنوعی؛
- بازسازی مجدد کش‌ها.

این بارهای کاری از موارد زیر بهره‌مند می‌شوند:
- ماندگاری پایدار وضعیت‌ها؛
- تلاش‌های مجدد خودکار؛
- پایش و مانیتورینگ؛
- شفافیت عملیاتی.

فریم‌ورک Hangfire قوی‌ترین انطباق کلی را فراهم می‌سازد.

---

# 15. ریسک‌ها (Risks)

| ریسک | راهکار کاهش ریسک |
|---|---|
| از دست رفتن کارهای پس‌زمینه بر اثر ریستارت | استفاده از ذخیره‌سازی ماندگار در Hangfire. |
| اجرای تکراری وظایف | طراحی همان‌توان (Idempotent job design). |
| شکست وظایف طولانی‌مدت | اعمال سیاست‌های تلاش مجدد و پایش دقیق. |
| پیچیدگی زمان‌بند | محدود کردن Quartz.NET به سناریوهای زمان‌بندی پیشرفته. |
| قفل شدن به ابر (Cloud lock-in) | ترجیح دادن انتزاع‌های بی‌طرف نسبت به ارائه‌دهنده. |
| وابستگی به زیرساخت | تفکیک وظایف زیرساختی از وظایف تجاری برنامه. |

---

# 16. توصیه نهایی (Final Recommendation)

سامانه MachineryManagerEnterprise باید معماری پردازش پس‌زمینه زیر را اتخاذ نماید:

| مسئولیت | فناوری انتخابی |
|---|---|
| ورکرهای مداوم و پیوسته | BackgroundService |
| وظایف پس‌زمینه ماندگار | Hangfire |
| زمان‌بندی پیشرفته | Quartz.NET (صرفاً در صورت نیاز خاص) |
| پردازش رویدادهای بومی Azure | Azure Functions (اختیاری) |
| زمان‌بندی زیرساختی | Kubernetes CronJobs |

فریم‌ورک Hangfire باید به عنوان پلتفرم اصلی وظایف پس‌زمینه سازمانی تبدیل شود.
کلاس BackgroundService پیاده‌سازی استاندارد برای ورکرهای مداوم باقی می‌ماند.

---

# 17. تصمیم نهایی (Final Decision)

معماری مصوب:
- کلاس BackgroundService باید ورکرهای پیوسته زیرساخت را پیاده‌سازی نماید.
- فریم‌ورک Hangfire باید به عنوان فریم‌ورک اصلی کارهای پس‌زمینه ماندگار تبدیل شود.
- کتابخانه Quartz.NET تنها زمانی معرفی می‌شود که قابلیت‌های زمان‌بندی فراتر از توانایی‌های بومی Hangfire باشد.
- سرویس Azure Functions به عنوان مدل استقرار اختیاری مختص به Azure باقی می‌ماند.
- ابزار Kubernetes CronJobs منحصراً وظایف نگهداری زیرساخت را زمان‌بندی می‌نماید.

ماژول‌های تجاری صرفاً باید به انتزاع پردازش پس‌زمینه وابسته باشند.
لایه زیرساخت مسئولیت انتخاب فناوری اجرا را بر عهده خواهد داشت.

---

# خلاصه تصمیم (Decision Summary)

پشته فناوری انتخابی تمامی نیازمندی‌های معماری را برآورده می‌سازد:
- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر
- ✔ آمادگی برای هوش مصنوعی
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
| 1.0.0 | 2026-07-26 | معمار راهکار (Solution Architect) | ارزیابی اولیه فناوری برای پردازش پس‌زمینه و زمان‌بندی وظایف |
| 1.3.0 | 2026-07-28 | معمار راهکار (Solution Architect) | افزودن بخش جدید (محدوده ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار (Solution Architect) | ارتقا به استاندارد مستندسازی نسخه v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار (Solution Architect) | بازبینی و همگام‌سازی با آخرین تغییرات |