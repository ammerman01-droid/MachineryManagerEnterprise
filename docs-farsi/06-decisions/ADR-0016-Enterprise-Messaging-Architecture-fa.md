| ویژگی | مقدار |
|------------------|--------------------|
| **شناسه سند** | ADR-0016 |
| **عنوان** | معماری پیام‌رسانی سازمانی (Enterprise Messaging Architecture) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده |
| **مالک** | معمار راهکار |
| **تاریخ ایجاد** | 2026-07-27 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# سند ثبت تصمیم معماری (ADR)

# ADR-0016 — معماری پیام‌رسانی سازمانی (Enterprise Messaging Architecture)

---

# هدف (Purpose)

این سند ثبت تصمیم معماری (ADR)، معماری پیام‌رسانی سازمانی را برای ارتباطات ناهمگام و رویداد-محور (Event-Driven) در سراسر زمینه‌های محدود (Bounded Contexts)، ماژول‌ها و یکپارچه‌سازی‌های خارجی در سامانه MachineryManagerEnterprise مستقر می‌سازد.

این پلتفرم نیازمند یک سازوکار پیام‌رسانی ناهمگام و قدرتمند به منظور توزیع رویدادهای دامنه، اجرای وظایف پس‌زمینه طولانی‌مدت، یکپارچه‌سازی بین ماژولی و صف‌بندی همگام‌سازی فضای کاری است، بدون آنکه ماژول‌ها در زمان اجرا به طور مستقیم به یکدیگر وابسته شوند.

---

# محدوده ارزیابی (Evaluation Scope)

محدوده این ارزیابی موارد زیر را در بر می‌گیرد:

- زیرساخت کارگزار پیام یا Message Broker (مانند RabbitMQ، Azure Service Bus، Apache Kafka).
- فریم‌ورک‌های انتزاعی گذرگاه پیام یا Message Bus (شامل .NET MassTransit، NServiceBus، کتابخانه‌های کلاینت خام).
- پشتیبانی از الگوی صندوق خروجی تراکنشی (Transactional Outbox Pattern) برای انتشار مطمئن رویدادها.
- مدیریت صف نامه‌های مرده (Dead-Letter Queue - DLQ) و خط‌مشی‌های تلاش مجدد خودکار (Automated Retry Policies).
- استانداردهای سریال‌سازی پیام‌ها (System.Text.Json / باینری فشرده).

---

# روابط و وابستگی‌ها (Relationship)

این سند با اسناد زیر همراستاست:

- **TE-0012 — ارزیابی فناوری پیام‌رسانی سازمانی (Enterprise Messaging Technology Evaluation)**: ارائه ارزیابی فنی کامل پشتیبان این تصمیم.
- **ADR-0001 — تصویب معماری پاک و مونولیت ماژولار (Adopt Clean Architecture & Modular Monolith)**: حاکم بر قواعد ایزوله‌سازی بین ماژولی.
- **ADR-0011 — استفاده از MediatR (Use MediatR)**: حاکم بر پیام‌رسانی درون‌فرآیندی CQRS (متمایز از پیام‌رسانی برون‌فرآیندی تحت نظارت این سند).
- **ADR-0015 — معماری همگام‌سازی فضای کاری (Workspace Synchronization Architecture)**: استفاده از صف‌های پیام برای انتقال پس‌زمینه همگام‌سازی.

---

# مراجع معماری (Architectural References)

- الگوهای یکپارچه‌سازی سازمانی (Hohpe & Woolf - Enterprise Integration Patterns)
- استاندارد معماری پاک نسخه 4.0.0 (Clean Architecture Standard v4.0.0)
- مستندات معماری MassTransit

---

# محدوده شمول (Scope)

بر تمامی سرویس‌های بک‌اند سازمانی، ورکرها و پردازش‌های پس‌زمینه، و مصرف‌کنندگان پیام‌های یکپارچه‌سازی ماژول‌ها در پلتفرم MachineryManagerEnterprise اعمال می‌گردد.

---

# نیازمندی‌های کارکردی (Functional Requirements)

معماری پیام‌رسانی سازمانی باید قابلیت‌های زیر را فراهم نماید:

- توزیع رویدادها به روش انتشار/اشتراک (Publish/Subscribe) ناهمگام.
- صف‌بندی دستورات به صورت نقطه به نقطه (Point-to-Point).
- تحویل تضمین‌شده پیام از طریق الگوی Transactional Outbox.
- تلاش مجدد خودکار (Retry)، قطع‌کننده مدار (Circuit Breaker) و صف‌بندی پیام‌های خطا (Dead-Letter Queueing).
- انتقال و انتشار سرآیندهای پیام در محیط‌های چندمستاجری (Multi-tenant Header Propagation).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

- **قابلیت اطمینان (Reliability)**: تضمین تحویل حداقل یک‌بار (At-least-once delivery) در سراسر تمامی رویدادهای یکپارچه‌سازی منتشرشده.
- **جداسازی و عدم وابستگی (Decoupling)**: ماژول‌ها باید قراردادهای تعریف‌شده در اسمبلی‌های مشترک قراردادها (`*.Contracts`) را منتشر کنند بدون آنکه به پیاده‌سازی‌های مشترکین ارجاع دهند.
- **قابلیت نگهداری (Maintainability)**: تعاریف ساده و با حداقل کد زائد (Low boilerplate) برای مصرف‌کنندگان پیام (Message Consumers).

---

# فناوری‌های نامزد (Candidate Technologies)

موارد زیر با جزئیات کامل در سند `TE-0012` ارزیابی شده‌اند:

۱. **MassTransit + RabbitMQ**: انتزاع متن‌باز و بسیار غنی پیام‌رسانی در دات‌نت به همراه Outbox توکار، ماشین‌های حالت Saga و پشتیبانی از چندین کارگزار پیام.
۲. **NServiceBus**: مجموعه امکانات سازمانی سطح بالا، اما هزینه مجوز تجاری آن با خط‌مشی اولویت متن‌باز (ADR-0002) در تعارض است.
۳. **Apache Kafka**: جریان رویداد با پهنای باند و توان عملیاتی بسیار بالا، اما پیچیدگی عملیاتی آن برای نیازمندی‌های یک مونولیت ماژولار و صف‌های پس‌زمینه نامتناسب است.

---

# معیارهای ارزیابی (Evaluation Criteria)

- مجوز متن‌باز مجاز (Permissive Open-Source License بر اساس ADR-0002)
- پشتیبانی توکار از الگوی Transactional Outbox برای EF Core
- پشتیبانی بومی از .NET 10 و Microsoft.Extensions.DependencyInjection
- پشتیبانی از ردگیری توزیع‌شده (Distributed Tracing) از طریق OpenTelemetry (بر اساس ADR-0010)

---

# اصل معماری (Architecture Principle)

ارتباطات درون‌فرآیندی (In-process) باید از MediatR (`IMediator`) استفاده نمایند. یکپارچه‌سازی ناهمگام برون‌فرآیندی (Cross-process) یا بین ماژول‌ها باید از MassTransit (`IPublishEndpoint` / `ISendEndpoint`) استفاده کند.

---

# مقایسه کلی فناوری‌ها (Overall Technology Comparison)

ترکیب MassTransit با RabbitMQ تعادل ایده‌آلی میان ماهیت متن‌باز، سهولت توسعه نرم‌افزار، ادغام آماده با الگوی EF Core Outbox، و انعطاف‌پذیری استقرار مستقل از ارائه‌دهندگان ابری را فراهم می‌سازد.

---

# توصیه نهایی (Final Recommendation)

تصویب **MassTransit** به عنوان فریم‌ورک پیام‌رسانی و **RabbitMQ** به عنوان موتور اصلی کارگزار پیام.

---

# تصمیم (Decision)

سامانه MachineryManagerEnterprise بر روی موارد زیر استانداردسازی می‌گردد:

۱. **فریم‌ورک پیام‌رسانی (Messaging Framework)**: استفاده از **MassTransit** در بستر .NET 10.
۲. **کارگزار پیام (Message Broker)**: استفاده از **RabbitMQ** (بر پایه AMQP 0-9-1 / 1.0) برای استقرارهای محلی (Self-hosted) یا ابری؛ پشتیبانی از Azure Service Bus از طریق لایه انتزاع MassTransit در صورت استقرار در مایکروسافت آژور فعال خواهد بود.
۳. **الگوی قابلیت اطمینان (Reliability Pattern)**: پیاده‌سازی اجباری **الگوی صندوق خروجی تراکنشی (Transactional Outbox Pattern)** با استفاده از یکپارچه‌سازی EF Core Outbox در MassTransit به منظور جلوگیری از انتشار رویدادهای فانتوم در زمان بازگشت تراکنش‌ها (Rollback).
۴. **ایزوله‌سازی قراردادها (Contract Isolation)**: کلیه قراردادهای پیام باید به عنوان انواع تغییرناپذیر `record` در پروژه‌های اختصاصی `*.Contracts` تعریف شوند.
۵. **مشاهده‌پذیری (Observability)**: ابزار دقیق توکار OpenTelemetry در MassTransit باید در پایپ‌لاین سراسری OpenTelemetry ثبت گردد (مطابق با ADR-0010).

---

# خلاصه تصمیمات (Decision Summary)

- ✔ تصویب MassTransit و RabbitMQ
- ✔ اجباری شدن الگوی Transactional Outbox
- ✔ انطباق با خط‌مشی اولویت متن‌باز (ADR-0002)
- ✔ یکپارچگی کامل با OpenTelemetry (بر اساس ADR-0010)
- ✔ اعمال تفکیک دقیق قراردادهای پیام

---

# اسناد مرتبط (Related Documents)

- TE-0012 — ارزیابی فناوری پیام‌رسانی سازمانی (Enterprise Messaging Technology Evaluation)
- ADR-0001 — تصویب معماری پاک و مونولیت ماژولار (Adopt Clean Architecture & Modular Monolith)
- ADR-0010 — استفاده از OpenTelemetry (Use OpenTelemetry)
- ADR-0011 — استفاده از MediatR (Use MediatR)
- ADR-0015 — معماری همگام‌سازی فضای کاری (Workspace Synchronization Architecture)

---

# مراجع (References)

- مستندات رسمی MassTransit: https://masstransit.io/
- راهنمای معماری RabbitMQ

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---------|------------|--------------------|-------------|
| 1.0.0 | 2026-07-27 | معمار راهکار | پیش‌نویس اولیه (وضعیت: پیشنهادی) |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | تصویب رسمی؛ تثبیت انتخاب MassTransit + RabbitMQ همراه با الگوی EF Core Outbox |
