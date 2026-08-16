| ویژگی (Property) | مقدار (Value) |
|---|---|
| **شناسه سند (Document ID)** | TE-0014 |
| **عنوان (Title)** | ارزیابی فناوری پردازش پس‌زمینه (Background Processing Technology Evaluation) |
| **نسخه (Version)** | 4.1.0 |
| **وضعیت (Status)** | تصویب‌شده (Approved) |
| **مالک (Owner)** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد (Created)** | 2026-07-26 |
| **آخرین به‌روزرسانی (Last Updated)** | 2026-08-08 |

---

# هدف (Purpose)

این سند به ارزیابی فناوری‌های کاندید برای پردازش پس‌زمینه (Background Processing Technology Evaluation) در سامانه MachineryManagerEnterprise می‌پردازد.

هدف، دستیابی به یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را برآورده ساخته و در عین حال اصول معماری تمیز (Clean Architecture) را حفظ نماید.

---

# ارتباط با ارزیابی‌های فناوری پیشین (Relationship with Previous Technology Evaluations)

این ارزیابی فناوری بر پایه پایه‌های ایجاد شده در TE-0001 (پلتفرم NET 10.) بنا شده و با قوانین معماری سازمانی تعریف‌شده در سراسر راهکار هم‌راستا است.

---

# دامنه ارزیابی (Evaluation Scope)

این ارزیابی فناوری تنها به ارزیابی و انتخاب فناوری می‌پردازد.

جزئیات پیاده‌سازی توسط سوابق تصمیمات معماری (ADRs) مربوطه تعریف می‌شوند.

---

# مراجع معماری (Architectural Reference)

این ارزیابی بر پایه موارد زیر استوار است:

- ADR-0001 — معماری تمیز (Clean Architecture)
- ADR-0016 — معماری پیام‌رسانی سازمانی (Enterprise Messaging Architecture)
- ADR-0017 — یکپارچه‌سازی هوش مصنوعی (Artificial Intelligence Integration)
- ADR-0018 — معماری یکپارچه‌سازی خارجی (External Integration Architecture)

فناوری انتخاب‌شده باید به صورت طبیعی با زیرساخت پیام‌رسانی ادغام شده و مستقل از توپولوژی استقرار باقی بماند.

---

# مراجع معماری (Architectural References)

این ارزیابی بر پایه اسناد زیر استوار است:

- ADR-0001 — معماری تمیز (Clean Architecture)
- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md

---

# نیازمندی‌های کارکردی (Functional Requirements)

پلتفرم پردازش پس‌زمینه باید از موارد زیر پشتیبانی نماید:

- کارهای تکرارشونده و دوره‌ای (recurring jobs)؛
- اجرای با تأخیر (delayed execution)؛
- کارهای شلیک و فراموشی (fire-and-forget tasks)؛
- جریان‌های کاری طولانی‌مدت (long-running workflows)؛
- اجرای توزیع‌شده (distributed execution)؛
- سیاست‌های تلاش مجدد (retry policies)؛
- مدیریت خطا و شکست (failure handling)؛
- زمان‌بندی (scheduling)؛
- پایش و مانیتورینگ (monitoring)؛
- داشبورد مدیریتی (dashboarding)؛
- تزریق وابستگی (dependency injection)؛
- پشتیبانی از لغو عملیات (cancellation support).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

فناوری‌های کاندید باید ویژگی‌های زیر را فراهم آورند:

- قابلیت اطمینان در سطح سازمانی (enterprise reliability)؛
- دسترسی‌پذیری بالا (high availability)؛
- انعطاف‌پذیری استقرار (deployment flexibility)؛
- مقیاس‌پذیری (scalability)؛
- سادگی عملیاتی (operational simplicity)؛
- مشاهده‌پذیری (observability)؛
- توسعه‌پذیری (extensibility)؛
- استقلال از ارائه‌دهنده (provider independence)؛
- بار عملیاتی پایین (low operational overhead).

---

# فناوری‌های کاندید (Candidate Technologies)

| فناوری (Technology) | دسته‌بندی (Category) |
|---|---|
| Hangfire | زمان‌بند پایدار کارهای پس‌زمینه (Persistent Background Job Scheduler) |
| Quartz.NET | زمان‌بند سازمانی (Enterprise Scheduler) |
| Coravel | زمان‌بند سبک‌وزن درون‌فرآیندی (Lightweight In-Process Scheduler) |
| Azure Functions | پردازش پس‌زمینه ابری (Cloud Background Processing) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه (ID) | معیار (Criterion) | وزن (Weight) |
|---|---|---|
| B1 | سازگاری با معماری تمیز (Clean Architecture Compatibility) | حیاتی (Critical) |
| B2 | اجرای توزیع‌شده (Distributed Execution) | حیاتی (Critical) |
| B3 | قابلیت‌های زمان‌بندی (Scheduling Capabilities) | بالا (High) |
| B4 | قابلیت اطمینان (Reliability) | بالا (High) |
| B5 | پشتیبانی از تلاش مجدد (Retry Support) | بالا (High) |
| B6 | پایش و مانیتورینگ (Monitoring) | متوسط (Medium) |
| B7 | انعطاف‌پذیری استقرار (Deployment Flexibility) | بالا (High) |
| B8 | پیچیدگی عملیاتی (Operational Complexity) | متوسط (Medium) |
| B9 | جامعه کاربری و اکوسیستم (Community & Ecosystem) | متوسط (Medium) |
| B10 | قابلیت نگهداری بلندمدت (Long-Term Maintainability) | بالا (High) |

---

# اصل معماری (Architecture Principle)

کامپوننت مورد ارزیابی به عنوان یک سرویس زیرساختی مجزا عمل می‌کند و کاملاً به وابستگی‌های لایه‌ای معماری تمیز و قوانین ایزوله‌سازی دامنه پایبند است.

---

# ۵. ارزیابی Hangfire (Hangfire Evaluation)

## نمای کلی (Overview)

فریم‌ورک Hangfire یک چارچوب متن‌باز برای کارهای پس‌زمینه در NET. است که پردازش پس‌زمینه پایدار را با استفاده از ذخیره‌سازی بادوام (durable storage) فراهم می‌کند.

برخلاف زمان‌بندهای ساده درون‌فرآیندی، Hangfire متادیتای کارها را در فضای ذخیره‌سازی پایدار ذخیره کرده و از اجرای قابل اعتماد در هنگام راه‌اندازی مجدد برنامه پشتیبانی می‌نماید.

ارائه‌دهندگان ذخیره‌سازی پشتیبانی‌شده عبارتند از:
- SQL Server
- PostgreSQL
- MySQL
- Redis
- سایر ارائه‌دهندگان ارائه‌شده توسط جامعه کاربری

فریم‌ورک Hangfire به طور گسترده در اکوسیستم NET. پذیرفته شده و معمولاً برای برنامه‌های سازمانی که نیازمند پردازش زمان‌بندی‌شده و ناهمگام هستند استفاده می‌شود.

---

## نقاط قوت معماری (Architectural Strengths)

### مزایا (Advantages)

- یکپارچگی عالی با NET.
- کارهای پس‌زمینه پایدار (Persistent background jobs).
- اجرای قابل اعتماد (Reliable execution).
- تلاش‌های مجدد خودکار (Automatic retries).
- کارهای با تأخیر (Delayed jobs).
- کارهای تکرارشونده و دوره‌ای (Recurring jobs).
- داشبورد برای پایش و مانیتورینگ (Dashboard for monitoring).
- پشتیبانی از تزریق وابستگی (Dependency Injection support).
- اکوسیستم بالغ (Mature ecosystem).
- جامعه کاربری بزرگ (Large community).
- مستندات عالی (Excellent documentation).
- اثبات‌شده در محیط‌های عملیاتی واقعی (Production proven).

---

## نقاط ضعف معماری (Architectural Weaknesses)

فریم‌ورک Hangfire اساساً پیرامون اجرای کارهای پس‌زمینه طراحی شده است.

این فریم‌ورک برای ارکستراسیون جریان‌های کاری توزیع‌شده طولانی‌مدت به همان روش پلتفرم‌های پیام‌رسانی سازمانی در نظر گرفته نشده است.

فرایندهای کسب‌وکار توزیع‌شده پیچیده عموماً نیازمند زیرساخت پیام‌رسانی در ترکیب با Hangfire هستند.

---

## ویژگی‌های عملیاتی (Operational Characteristics)

فریم‌ورک Hangfire موارد زیر را فراهم می‌آورد:
- ذخیره‌سازی پایدار (persistent storage)؛
- تلاش‌های مجدد خودکار (automatic retries)؛
- زمان‌بندی تکرارشونده (recurring scheduling)؛
- اجرای با تأخیر (delayed execution)؛
- داشبورد مانیتورینگ (monitoring dashboard)؛
- استخرهای کارگر (worker pools)؛
- اولویت‌بندی صف‌ها (queue prioritization).

پیچیدگی عملیاتی پایین ارزیابی می‌شود.

---

## مقیاس‌پذیری (Scalability)

فریم‌ورک Hangfire از چندین کارگر (workers) و چندین سرور پشتیبانی می‌کند.

مقیاس‌پذیری به طور کلی به صورت افقی (horizontal) بوده و به ارائه‌دهنده ذخیره‌سازی انتخاب‌شده بستگی دارد.

برای برنامه‌های سازمانی متوسط و بزرگ، Hangfire مقیاس‌پذیری کافی را فراهم می‌سازد.

---

## امنیت (Security)

امنیت اساساً به موارد زیر بستگی دارد:
- امنیت فضای ذخیره‌سازی (storage security)؛
- احراز هویت داشبورد (dashboard authentication)؛
- امنیت انتقال داده (transport security).

داشبورد هرگز نباید بدون احراز هویت در معرض دید قرار گیرد.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

فریم‌ورک Hangfire از موارد زیر پشتیبانی می‌کند:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- درون‌سازمانی (On-Premise)
- ابری (Cloud)

هیچ‌گونه وابستگی به ابر وجود ندارد.

---

## قابلیت نگهداری (Maintainability)

این فریم‌ورک نشان‌دهنده موارد زیر است:
- رابط‌های برنامه‌نویسی پایدار (stable APIs)؛
- پشتیبانی بلندمدت اکوسیستم (long-term ecosystem support)؛
- نگهداری فعال (active maintenance)؛
- مستندات جامع (extensive documentation).

قابلیت نگهداری عالی ارزیابی می‌شود.

---

## تناسب با MachineryManagerEnterprise

فریم‌ورک Hangfire تقریباً تمامی نیازمندی‌های معماری تعریف‌شده توسط اسناد زیر را برآورده می‌سازد:
- ADR-0001
- ADR-0016
- ADR-0017
- ADR-0018

بارهای کاری معمول شامل موارد زیر است:
- نگهداری و تعمیرات زمان‌بندی‌شده (scheduled maintenance)؛
- تحویل اعلان‌ها (notification delivery)؛
- همگام‌سازی (synchronization)؛
- پیش‌پردازش هوش مصنوعی (AI preprocessing)؛
- پس‌پردازش هوش مصنوعی (AI postprocessing)؛
- نوسازی حافظه پنهان (cache refresh)؛
- عملیات پاک‌سازی داده‌ها (cleanup operations)؛
- کارهای گزارش‌گیری (reporting jobs).

فریم‌ورک Hangfire به جای جایگزینی معماری پیام‌رسانی، آن را تکمیل می‌نماید.

---

## تناسب معماری (Architectural Fit)

| معیار (Criterion) | ارزیابی (Assessment) |
|---|---|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| اجرای توزیع‌شده (Distributed Execution) | بسیار خوب (Very Good) |
| زمان‌بندی (Scheduling) | عالی (Excellent) |
| قابلیت اطمینان (Reliability) | عالی (Excellent) |
| مکانیزم‌های تلاش مجدد (Retry Mechanisms) | عالی (Excellent) |
| مانیتورینگ (Monitoring) | عالی (Excellent) |
| انعطاف‌پذیری استقرار (Deployment Flexibility) | عالی (Excellent) |
| پیچیدگی عملیاتی (Operational Complexity) | پایین (Low) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

فریم‌ورک Hangfire یک پلتفرم پردازش پس‌زمینه بسیار بالغ را ارائه می‌دهد.

مدل پایداری، قابلیت‌های زمان‌بندی و بلوغ عملیاتی آن، این فریم‌ورک را به یک گزینه کاندید عالی برای MachineryManagerEnterprise تبدیل کرده است.

رویکرد معماری توصیه‌شده، ترکیب Hangfire با پلتفرم پیام‌رسانی سازمانی انتخاب‌شده (RabbitMQ + MassTransit) است، به جای اینکه تلاش شود از Hangfire به عنوان یک موتور ارکستراسیون جریان کاری استفاده گردد.

---

# ۶. ارزیابی Quartz.NET (Quartz.NET Evaluation)

## نمای کلی (Overview)

فریم‌ورک Quartz.NET یک فریم‌ورک زمان‌بندی کارهای سازمانی متن‌باز برای NET. است که بر پایه Java Quartz Scheduler توسعه یافته است.

برخلاف زمان‌بندهای سبک‌وزن، Quartz.NET بر سناریوهای پیچیده زمان‌بندی، تقویم‌های کاری سازمانی، زمان‌بندی کلاسترشده و کنترل اجرای سطح سازمانی تمرکز دارد.

فریم‌ورک Quartz.NET اساساً یک زمان‌بند (scheduler) است تا یک فریم‌ورک پردازش کارهای پس‌زمینه.

موارد کاربرد معمول شامل موارد زیر است:
- زمان‌بندی‌های تکرارشونده پیچیده (complex recurring schedules)؛
- مدیریت تقویم سازمانی (enterprise calendar management)؛
- زمان‌بندهای خوشه‌ای و کلاسترشده (clustered schedulers)؛
- اجرای مبتنی بر کرون (cron-based execution)؛
- فرآیندهای زمان‌بندی‌شده بلندمدت (long-term scheduled processes).

---

## نقاط قوت معماری (Architectural Strengths)

### مزایا (Advantages)

- زمان‌بند در سطح سازمانی (Enterprise-grade scheduler).
- پشتیبانی بسیار قدرتمند از Cron (Very powerful Cron support).
- قابلیت‌های زمان‌بندی غنی (Rich scheduling capabilities).
- استثنائات تقویمی (Calendar exceptions).
- مدیریت عدم شلیک کارها (Misfire handling).
- زمان‌بندی پایدار (Persistent scheduling).
- پشتیبانی از کلاسترینگ و خوشه‌بندی (Cluster support).
- قابلیت اطمینان بالا (High reliability).
- اکوسیستم بالغ (Mature ecosystem).
- مستندات عالی (Excellent documentation).
- پایداری بلندمدت (Long-term stability).

---

## نقاط ضعف معماری (Architectural Weaknesses)

فریم‌ورک Quartz.NET روی زمان‌بندی تمرکز دارد.

این فریم‌ورک در مقایسه با Hangfire امکانات به مراتب کمتری را در زمینه‌های زیر فراهم می‌آورد:
- پایش و مانیتورینگ کارها (job monitoring)؛
- داشبوردهای عملیاتی (operational dashboards)؛
- ارگونومی پردازش پس‌زمینه (background processing ergonomics)؛
- ادغام با تزریق وابستگی (dependency injection integration)؛
- بهره‌وری توسعه‌دهنده (developer productivity).

بنابراین، اکثر سیستم‌های سازمانی نیازمند زیرساخت‌های جانبی اضافه در اطراف Quartz.NET هستند.

---

## ویژگی‌های عملیاتی (Operational Characteristics)

فریم‌ورک Quartz.NET موارد زیر را فراهم می‌آورد:
- زمان‌بندی با Cron (Cron scheduling)؛
- زمان‌بندی تقویمی (Calendar scheduling)؛
- ذخیره‌سازی پایدار کارها (Persistent job store)؛
- هماهنگی کلاستر (Cluster coordination)؛
- مدیریت تریگرها (Trigger management)؛
- سیاست‌های عدم شلیک کار (Misfire policies)؛
- معماری شنونده‌ها (Listener architecture).

پیچیدگی عملیاتی متوسط ارزیابی می‌شود.

---

## مقیاس‌پذیری (Scalability)

فریم‌ورک Quartz.NET در استقرارهای کلاسترشده به خوبی مقیاس می‌پذیرد.

محیط‌های سازمانی بزرگ سال‌هاست که با موفقیت از Quartz.NET استفاده می‌کنند.

با این حال، مقیاس‌پذیری دید عملیاتی (operational visibility) اغلب نیازمند زیرساخت‌های مانیتورینگ اضافی است.

---

## امنیت (Security)

فریم‌ورک Quartz.NET امنیت را اساساً به موارد زیر واگذار می‌کند:
- برنامه میزبان (host application)؛
- ارائه‌دهنده ذخیره‌سازی (storage provider)؛
- محیط استقرار (deployment environment).

خود فریم‌ورک نگرانی امنیتی قابل توجهی ایجاد نمی‌کند.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

فریم‌ورک Quartz.NET از موارد زیر پشتیبانی می‌کند:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)
- درون‌سازمانی (On-Premise)
- ابری (Cloud)

هیچ وابستگی به فروشنده خاصی وجود ندارد.

---

## قابلیت نگهداری (Maintainability)

فریم‌ورک Quartz.NET ویژگی‌های زیر را نشان می‌دهد:
- رابط‌های برنامه‌نویسی بالغ (mature APIs)؛
- انتشارهای پایدار (stable releases)؛
- نگهداری فعال (active maintenance)؛
- پشتیبانی قوی جامعه کاربری (strong community support).

قابلیت نگهداری عالی ارزیابی می‌شود.

---

## تناسب با MachineryManagerEnterprise

فریم‌ورک Quartz.NET تمامی نیازمندی‌های معماری مربوط به زمان‌بندی را برآورده می‌سازد.

با این حال، MachineryManagerEnterprise به چیزی بسیار فراتر از صرفاً زمان‌بندی نیاز دارد.

قابلیت‌های مورد نیاز عبارتند از:
- کارهای پس‌زمینه توزیع‌شده (distributed background jobs)؛
- سیاست‌های تلاش مجدد (retry policies)؛
- پایش عملیاتی (operational monitoring)؛
- اجرای وظایف هوش مصنوعی (AI task execution)؛
- کارهای همگام‌سازی (synchronization jobs)؛
- پردازش اعلان‌ها (notification processing).

بنابراین، Quartz.NET برای دستیابی به همان تجربه عملیاتی که به صورت بومی توسط Hangfire ارائه می‌شود، نیازمند زیرساخت‌های مکمل خواهد بود.

---

## تناسب معماری (Architectural Fit)

| معیار (Criterion) | ارزیابی (Assessment) |
|---|---|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| اجرای توزیع‌شده (Distributed Execution) | عالی (Excellent) |
| زمان‌بندی (Scheduling) | عالی (Excellent) |
| قابلیت اطمینان (Reliability) | عالی (Excellent) |
| مکانیزم‌های تلاش مجدد (Retry Mechanisms) | خوب (Good) |
| مانیتورینگ (Monitoring) | متوسط (Moderate) |
| انعطاف‌پذیری استقرار (Deployment Flexibility) | عالی (Excellent) |
| پیچیدگی عملیاتی (Operational Complexity) | متوسط (Moderate) |
| قابلیت نگهداری (Maintainability) | عالی (Excellent) |

---

## مقایسه با Hangfire (Comparison with Hangfire)

| قابلیت (Capability) | Hangfire | Quartz.NET |
|---|---|---|
| زمان‌بندی (Scheduling) | عالی (Excellent) | عالی (Excellent) |
| کارهای پس‌زمینه (Background Jobs) | عالی (Excellent) | خوب (Good) |
| داشبورد (Dashboard) | عالی (Excellent) | محدود (Limited) |
| بهره‌وری توسعه‌دهنده (Developer Productivity) | عالی (Excellent) | خوب (Good) |
| پشتیبانی از Cron (Cron Support) | خوب (Good) | عالی (Excellent) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) | متوسط (Moderate) |
| زمان‌بندی سازمانی (Enterprise Scheduling) | خوب (Good) | عالی (Excellent) |

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

فریم‌ورک Quartz.NET یکی از قوی‌ترین فریم‌ورک‌های زمان‌بندی موجود برای NET. است.

اگر MachineryManagerEnterprise تنها به زمان‌بندی سازمانی نیاز داشت، Quartz.NET یک انتخاب برجسته می‌بود.

با این حال، پلتفرم به یک اکوسیستم کامل پردازش پس‌زمینه نیاز دارد و نه صرفاً زمان‌بندی.

تحت این نیازمندی‌ها، Hangfire توازن بهتری بین قابلیت‌های زمان‌بندی، بلوغ عملیاتی، امکانات مانیتورینگ و بهره‌وری توسعه‌دهنده برقرار می‌کند.

بنابراین Quartz.NET به عنوان یک زمان‌بند تخصصی توصیه می‌شود اما به عنوان پلتفرم اصلی پردازش پس‌زمینه پیشنهاد نمی‌گردد.

---

# ۷. ارزیابی Coravel (Coravel Evaluation)

## نمای کلی (Overview)

فریم‌ورک Coravel یک چارچوب متن‌باز سبک‌وزن برای زمان‌بندی و وظایف پس‌زمینه در ASP.NET Core است.

برخلاف Hangfire و Quartz.NET، فریم‌ورک Coravel عمداً برای سادگی و حداقل زیرساخت طراحی شده است.

تمرکز Coravel بر موارد زیر است:
- زمان‌بندی درون‌فرآیندی (in-process scheduling)؛
- کارهای پس‌زمینه صف‌بندی‌شده (queued background tasks)؛
- ادغام با تزریق وابستگی (dependency injection integration)؛
- بار عملیاتی پایین (low operational overhead).

این فریم‌ورک اساساً برای برنامه‌های کوچک تا متوسط در نظر گرفته شده که در آن‌ها سادگی بر ارکستراسیون سازمانی ترجیح داده می‌شود.

---

## نقاط قوت معماری (Architectural Strengths)

### مزایا (Advantages)

- بسیار سبک‌وزن (Very lightweight).
- پیکربندی فوق‌العاده ساده (Extremely simple configuration).
- ادغام بومی با ASP.NET Core (Native ASP.NET Core integration).
- پشتیبانی عالی از تزریق وابستگی (Excellent Dependency Injection support).
- حداقل زیرساخت مورد نیاز (Minimal infrastructure).
- عدم نیاز به ذخیره‌سازی خارجی (No external storage required).
- بار عملیاتی بسیار پایین (Very low operational overhead).
- منحنی یادگیری آسان (Easy learning curve).

---

## نقاط ضعف معماری (Architectural Weaknesses)

فریم‌ورک Coravel عمداً از بسیاری از قابلیت‌های سازمانی صرف‌نظر کرده است.

قابلیت‌های ناموجود یا محدود عبارتند از:
- اجرای توزیع‌شده (distributed execution)؛
- ذخیره‌سازی پایدار کارها (persistent job storage)؛
- کارگران خوشه‌ای (clustered workers)؛
- مانیتورینگ سازمانی (enterprise monitoring)؛
- داشبورد اجرا (execution dashboard)؛
- مکانیزم‌های پایدار تلاش مجدد (persistent retry mechanisms)؛
- ارکستراسیون جریان‌های کاری طولانی‌مدت (long-running workflow orchestration).

از آنجا که کارها به صورت درون‌فرآیندی اجرا می‌شوند، راه‌اندازی مجدد برنامه کارهای در حال انتظار را متوقف می‌سازد.

---

## ویژگی‌های عملیاتی (Operational Characteristics)

فریم‌ورک Coravel موارد زیر را فراهم می‌آورد:
- کارهای زمان‌بندی‌شده (scheduled jobs)؛
- کارهای صف‌بندی‌شده (queued tasks)؛
- زنجیره‌سازی وظایف (task chaining)؛
- پشتیبانی از تزریق وابستگی (dependency injection support).

پیچیدگی عملیاتی بسیار پایین است.

---

## مقیاس‌پذیری (Scalability)

فریم‌ورک Coravel اساساً برای برنامه‌های تک‌نمونه‌ای (single-instance) در نظر گرفته شده است.

مقیاس‌پذیری افقی نیازمند هماهنگی سفارشی است زیرا هیچ زمان‌بند پایدار مشترکی وجود ندارد.

استقرارهای سازمانی بزرگ خارج از اهداف اصلی طراحی این فریم‌ورک قرار دارند.

---

## امنیت (Security)

فریم‌ورک Coravel سطح امنیتی بسیار کمی ایجاد می‌کند زیرا هیچ داشبورد مدیریتی یا سرویس خارجی در آن وجود ندارد.

امنیت تقریباً به طور کامل از برنامه میزبان به ارث برده می‌شود.

---

## انعطاف‌پذیری استقرار (Deployment Flexibility)

فریم‌ورک Coravel از موارد زیر پشتیبانی می‌کند:
- ویندوز (Windows)
- لینوکس (Linux)
- کانتینرها (Containers)
- کوبرنتیز (Kubernetes)

با این حال، استقرارهای توزیع‌شده نیازمند کارهای معماری اضافی هستند.

---

## قابلیت نگهداری (Maintainability)

فریم‌ورک Coravel دارای ویژگی‌های زیر است:
- رابط برنامه‌نویسی تمیز (clean API)؛
- مدل برنامه‌نویسی سرراست (straightforward programming model)؛
- مستندات خوب (good documentation)؛
- نگهداری فعال (active maintenance).

قابلیت نگهداری بسیار خوب ارزیابی می‌شود.

---

## تناسب با MachineryManagerEnterprise

سامانه MachineryManagerEnterprise نیازمند موارد زیر است:
- اجرای توزیع‌شده (distributed execution)؛
- زمان‌بندی پایدار (persistent scheduling)؛
- مدیریت تلاش مجدد (retry management)؛
- مانیتورینگ (monitoring)؛
- دید عملیاتی سازمانی (enterprise operational visibility)؛
- پردازش پس‌زمینه تاب‌آور (resilient background processing).

فریم‌ورک Coravel تنها بخشی از این نیازمندی‌ها را برآورده می‌سازد.

اگرچه از نظر معماری تمیز است، فلسفه طراحی آن سیستم‌های بسیار کوچک‌تری را نسبت به MachineryManagerEnterprise هدف قرار داده است.

---

## تناسب معماری (Architectural Fit)

| معیار (Criterion) | ارزیابی (Assessment) |
|---|---|
| معماری تمیز (Clean Architecture) | عالی (Excellent) |
| اجرای توزیع‌شده (Distributed Execution) | ضعیف (Poor) |
| زمان‌بندی (Scheduling) | خوب (Good) |
| قابلیت اطمینان (Reliability) | متوسط (Moderate) |
| مکانیزم‌های تلاش مجدد (Retry Mechanisms) | محدود (Limited) |
| مانیتورینگ (Monitoring) | ضعیف (Poor) |
| انعطاف‌پذیری استقرار (Deployment Flexibility) | خوب (Good) |
| پیچیدگی عملیاتی (Operational Complexity) | عالی (Excellent) |
| قابلیت نگهداری (Maintainability) | بسیار خوب (Very Good) |

---

## مقایسه (Comparison)

| قابلیت (Capability) | Hangfire | Quartz.NET | Coravel |
|---|---|---|---|
| کارهای پس‌زمینه سازمانی (Enterprise Background Jobs) | عالی (Excellent) | خوب (Good) | متوسط (Moderate) |
| زمان‌بندی سازمانی (Enterprise Scheduling) | خوب (Good) | عالی (Excellent) | خوب (Good) |
| ذخیره‌سازی پایدار (Persistent Storage) | بله (Yes) | بله (Yes) | خیر (No) |
| داشبورد (Dashboard) | بله (Yes) | خیر (No) | خیر (No) |
| کارگران توزیع‌شده (Distributed Workers) | بله (Yes) | بله (Yes) | خیر (No) |
| سادگی عملیاتی (Operational Simplicity) | عالی (Excellent) | متوسط (Moderate) | عالی (Excellent) |

---

## نتیجه‌گیری اولیه (Preliminary Conclusion)

فریم‌ورک Coravel یک چارچوب زمان‌بندی سبک‌وزن عالی برای برنامه‌های کوچک ASP.NET Core است.

با این حال، انتظار می‌رود MachineryManagerEnterprise به یک پلتفرم سازمانی بزرگ تبدیل شود که نیازمند پردازش پس‌زمینه توزیع‌شده پایدار است.

در نتیجه، Coravel نباید به عنوان فریم‌ورک اصلی پردازش پس‌زمینه انتخاب شود.

سادگی آن جذاب است، اما عدم وجود پایداری، کلاسترینگ و قابلیت‌های عملیاتی سازمانی، آن را برای اهداف معماری بلندمدت این پروژه نامناسب می‌سازد.

---

# ۹. مقایسه جامع فناوری‌ها (Overall Technology Comparison)

## ماتریس مقایسه فناوری‌ها (Technology Comparison Matrix)

| قابلیت (Capability) | Hangfire | Quartz.NET | Coravel | Azure Functions |
|---|---|---|---|---|
| معماری تمیز (Clean Architecture) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | خوب (Good) |
| زمان‌بندی سازمانی (Enterprise Scheduling) | خوب (Good) | عالی (Excellent) | خوب (Good) | خوب (Good) |
| کارهای پس‌زمینه پایدار (Persistent Background Jobs) | عالی (Excellent) | بسیار خوب (Very Good) | ضعیف (Poor) | عالی (Excellent) |
| اجرای توزیع‌شده (Distributed Execution) | عالی (Excellent) | عالی (Excellent) | ضعیف (Poor) | عالی (Excellent) |
| سیاست‌های تلاش مجدد (Retry Policies) | عالی (Excellent) | خوب (Good) | محدود (Limited) | عالی (Excellent) |
| مانیتورینگ (Monitoring) | عالی (Excellent) | متوسط (Moderate) | ضعیف (Poor) | عالی (Excellent) |
| داشبورد (Dashboard) | عالی (Excellent) | خیر (No) | خیر (No) | پرتال آژور (Azure Portal) |
| تزریق وابستگی (Dependency Injection) | عالی (Excellent) | خوب (Good) | عالی (Excellent) | عالی (Excellent) |
| استقرار درون‌سازمانی (On-Premise Deployment) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | ضعیف (Poor) |
| استقرار ابری (Cloud Deployment) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) | عالی (Excellent) |
| استقرار هیبریدی (Hybrid Deployment) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | متوسط (Moderate) |
| پیچیدگی عملیاتی (Operational Complexity) | پایین (Low) | متوسط (Medium) | بسیار پایین (Very Low) | متوسط (Medium) |
| بلوغ جامعه کاربری (Community Maturity) | عالی (Excellent) | عالی (Excellent) | خوب (Good) | عالی (Excellent) |
| آمادگی سازمانی (Enterprise Readiness) | عالی (Excellent) | عالی (Excellent) | متوسط (Moderate) | بسیار خوب (Very Good) |

---

# ۱۰. ارزیابی معماری (Architecture Assessment)

فناوری‌های ارزیابی‌شده در برابر اصول معماری تعریف‌شده توسط اسناد زیر سنجیده شدند:
- ADR-0001
- ADR-0016
- ADR-0017
- ADR-0018

اهداف اصلی معماری عبارت بودند از:
- استقلال در استقرار (deployment independence)؛
- استقلال از ارائه‌دهنده (provider independence)؛
- قابلیت اطمینان در سطح سازمانی (enterprise reliability)؛
- سادگی عملیاتی (operational simplicity)؛
- قابلیت نگهداری (maintainability)؛
- مقیاس‌پذیری بلندمدت (long-term scalability).

---

# ۱۱. معماری توصیه‌شده پردازش پس‌زمینه (Recommended Background Processing Architecture)

## پلتفرم اصلی (Primary Platform)

Hangfire

مسئولیت‌های توصیه‌شده:
- کارهای تکرارشونده و دوره‌ای (recurring jobs)؛
- کارهای با تأخیر (delayed jobs)؛
- کارگران پس‌زمینه (background workers)؛
- پردازش اعلان‌ها (notification processing)؛
- پیش‌پردازش هوش مصنوعی (AI preprocessing)؛
- پس‌پردازش هوش مصنوعی (AI postprocessing)؛
- همگام‌سازی (synchronization)؛
- وظایف نگهداری و پشتیبانی (maintenance tasks)؛
- کارهای گزارش‌گیری (reporting jobs).

---

## زمان‌بند سازمانی (Enterprise Scheduler)

Quartz.NET

تنها زمانی توصیه می‌شود که ویژگی‌های پیشرفته زمان‌بندی مورد نیاز باشد، از جمله:
- تقویم‌های سازمانی (enterprise calendars)؛
- عبارات پیچیده کرون (sophisticated Cron expressions)؛
- استثنائات زمان‌بندی (scheduling exceptions)؛
- تقویم‌های پیچیده اجرایی (complex execution calendars).

فریم‌ورک Quartz.NET نباید جایگزین Hangfire به عنوان پلتفرم اصلی اجرا شود.

---

## زمان‌بند سبک‌وزن (Lightweight Scheduler)

Coravel

تنها برای موارد زیر توصیه می‌شود:
- نمونه‌های اولیه (prototypes)؛
- سرویس‌های سبک‌وزن (lightweight services)؛
- ابزارهای داخلی (internal utilities).

برای MachineryManagerEnterprise توصیه نمی‌شود.

---

## جایگزین بومی در ابر (Cloud Native Alternative)

Azure Functions

تنها زمانی توصیه می‌شود که:
- استقرار صرفاً مبتنی بر آژور باشد (deployment is Azure-only)؛
- معماری بدون سرور (Serverless) یک نیازمندی کسب‌وکار باشد.

سرویس Azure Functions نباید به شالوده معماری پلتفرم تبدیل شود زیرا وابستگی به ارائه‌دهنده ابری ایجاد می‌کند که با اهداف انعطاف‌پذیری استقرار تعریف‌شده توسط ADR-0001 در تضاد است.

---

# ۱۲. معماری سازمانی توصیه‌شده (Recommended Enterprise Architecture)

```text
Business Module
        │
        ▼
Application Layer
        │
        ▼
Background Processing Abstraction
        │
        ▼
Hangfire
        │
        ▼
RabbitMQ + MassTransit
        │
        ▼
Infrastructure Services
```

پلتفرم پردازش پس‌زمینه، معماری پیام‌رسانی را تکمیل می‌نماید.

پیام‌رسانی مسئولیت ارتباطات را بر عهده دارد.

فریم‌ورک Hangfire مسئولیت اجرا را بر عهده دارد.

---

# ۱۳. پیشنهاد نهایی (Final Recommendation)

استراتژی پیاده‌سازی توصیه‌شده به شرح زیر است:

1. فریم‌ورک Hangfire را به عنوان پلتفرم اصلی پردازش پس‌زمینه بپذیرید.
2. فریم‌ورک Hangfire را با زیرساخت پیام‌رسانی انتخاب‌شده یکپارچه نمایید.
3. از RabbitMQ + MassTransit برای ارتباطات استفاده کنید.
4. فریم‌ورک Quartz.NET را برای سناریوهای استثنایی زمان‌بندی سازمانی رزرو کنید.
5. به دلیل وابستگی به ارائه‌دهنده ابری، بر روی Azure Functions استانداردسازی نکنید.
6. به دلیل ناکافی بودن قابلیت‌های سازمانی، بر روی Coravel استانداردسازی نکنید.

---

# ۱۴. خلاصه تصمیمات (Decision Summary)

| لایه (Layer) | فناوری انتخاب‌شده (Selected Technology) |
|---|---|
| پردازش پس‌زمینه (Background Processing) | Hangfire |
| زمان‌بند سازمانی (Enterprise Scheduler) | Quartz.NET (اختیاری / Optional) |
| زمان‌بند سبک‌وزن (Lightweight Scheduler) | Coravel (انتخاب نشده / Not Selected) |
| سرورلس ابری (Cloud Serverless) | Azure Functions (انتخاب نشده / Not Selected) |

---

# ۱۵. ریسک‌ها (Risks)

| ریسک (Risk) | راهکار کاهش ریسک (Mitigation) |
|---|---|
| کارهای طولانی‌مدت (Long-running jobs) | انتقال بار به زیرساخت پیام‌رسانی (Offload to messaging infrastructure) |
| از کار افتادن کارگر (Worker failure) | ذخیره‌سازی پایدار Hangfire (Persistent Hangfire storage) |
| طوفان‌های تلاش مجدد (Retry storms) | سیاست‌های تلاش مجدد قابل پیکربندی (Configurable retry policies) |
| بار اضافی صف‌ها (Queue overload) | صف‌های اختصاصی و استخرهای کارگر (Dedicated queues and worker pools) |
| دید عملیاتی (Operational visibility) | داشبورد Hangfire به همراه لاگینگ متمرکز (Hangfire Dashboard + centralized logging) |

---

# ۱۶. تاثیر تصمیم (Decision Impact)

معماری انتخاب‌شده موارد زیر را امکان‌پذیر می‌سازد:
- زمان‌بندی سازمانی (enterprise scheduling)؛
- اجرای مقاوم و تاب‌آور کارهای پس‌زمینه (resilient background execution)؛
- جریان‌های کاری کسب‌وکار ناهمگام (asynchronous business workflows)؛
- ارکستراسیون هوش مصنوعی (AI orchestration)؛
- سرویس‌های همگام‌سازی (synchronization services)؛
- گزارش‌گیری (reporting)؛
- تحویل اعلان‌ها (notification delivery)؛

در عین حالی که کاملاً مستقل از ارائه‌دهندگان ابری و توپولوژی استقرار باقی می‌ماند.

---

# تصمیم نهایی (Final Decision)

| کامپوننت (Component) | تصمیم (Decision) |
|---|---|
| فناوری اصلی انتخاب‌شده (Primary Selected Technology) | تصویب شد (Approved) |

---

# خلاصه تصمیمات (Decision Summary)

پشته فناوری انتخاب‌شده تمامی نیازمندی‌های معماری را برآورده می‌سازد:

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10. (.NET 10 Compatibility)
- ✔ انطباق با استانداردها (Standards Compliance)
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت (Long-term Maintainability)

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

| نسخه (Version) | تاریخ (Date) | نویسنده (Author) | توضیحات (Description) |
|---|---|---|---|
| 1.0.0 | 2026-07-26 | معمار راهکار (Solution Architect) | ارزیابی اولیه فناوری برای پردازش پس‌زمینه (Initial technology evaluation for Background Processing) |
| 1.3.0 | 2026-07-28 | معمار راهکار (Solution Architect) | افزودن بخش جدید: دامنه ارزیابی (New section added (Evaluation Scope)) |
| 4.0.0 | 2026-07-28 | معمار راهکار (Solution Architect) | ارتقا به استاندارد مستندسازی v4.0.0 (Upgraded to Documentation Standard v4.0.0) |
| 4.1.0 | 2026-08-08 | معمار راهکار (Solution Architect) | بازبینی و همگام‌سازی با آخرین تغییرات (Review and synchronize with the latest changes) |