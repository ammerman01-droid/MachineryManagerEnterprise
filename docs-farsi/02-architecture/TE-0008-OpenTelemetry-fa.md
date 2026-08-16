| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0008 |
| **عنوان** | ارزیابی استانداردهای OpenTelemetry (OpenTelemetry Standards Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند فناوری‌های کاندید را برای ارزیابی استانداردهای OpenTelemetry در MachineryManagerEnterprise ارزیابی می‌کند.

هدف، ایجاد یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را ضمن حفظ اصول معماری تمیز (Clean Architecture) برآورده سازد.

---

# دامنه ارزیابی (Evaluation Scope)

استانداردهای باز را برای تله‌متری ارزیابی می‌کند. پایپ‌لاین کامل مشاهده‌پذیری در TE-0017 به‌تفصیل شرح داده شده است.

---

# ارتباط با ارزیابی‌های فناوری پیشین (Relationship with Previous Technology Evaluations)

استاندارد پایه برای TE-0017 (مشاهده‌پذیری و تله‌متری) است.

---

# مراجع معماری (Architectural References)

- ADR-0001 — معماری تمیز (Clean Architecture)
- TE-0017 — ارزیابی مشاهده‌پذیری و تله‌متری (Observability and Telemetry Evaluation)

---

# دامنه (Scope)

استاندارد OpenTelemetry را در برابر ایجنت‌های انحصاری APM (نظیر Datadog، AppInsights SDK) ارزیابی می‌کند.

---

# نیازمندی‌های کارکردی (Functional Requirements)

جمع‌آوری یکپارچه ردگیری‌های توزیع‌شده (Distributed Traces)، سنجه‌ها (Metrics) و لاگ‌ها در سراسر سرویس‌های NET.

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

پروتکل اکسپورتر بی‌طرف نسبت به ارائه‌دهنده (OTLP)، سربار عملکردی اندک، بی‌طرفی نسبت به محیط ابری.

---

# فناوری‌های کاندید (Candidate Technologies)

| فناوری | هدف | وضعیت |
|---|---|---|
| OpenTelemetry (.NET) | استاندارد جمع‌آوری تله‌متری | انتخاب‌شده (Selected) |
| Proprietary APM SDKs | ایجنت‌های دارای وابستگی انحصاری | ردشده (Rejected) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|---|---|---|
| A1 | بی‌طرفی نسبت به ابر و استاندارد باز | حیاتی (Critical) |
| A2 | پشتیبانی از Activity/Meter در NET 10. | بالا (High) |

---

# اصل معماری (Architecture Principle)

ابزار دقیق و اندازه‌گیری (Instrumentation) از APIهای استاندارد System.Diagnostics که به‌صورت بومی درون NET 10. تعبیه شده‌اند استفاده می‌کند.

---

# ۵. ارزیابی‌های عمیق کاندیدها (Candidate Deep-Dive Evaluations)

## ارزیابی OpenTelemetry (OpenTelemetry Evaluation)

### نمای کلی (Overview)
فریم‌ورک OpenTelemetry (به اختصار OTel) یک فریم‌ورک مشاهده‌پذیری استاندارد و بی‌طرف نسبت به ارائه‌دهنده از بنیاد CNCF است.

### نقاط قوت معماری (Architectural Strengths)
- حذف وابستگی انحصاری به ارائه‌دهنده (Vendor lock-in)؛ امکان تعویض بک‌اند تله‌متری (Grafana Tempo, Prometheus, Datadog) بدون نیاز به تغییر کد.

---

# مقایسه جامع فناوری‌ها (Overall Technology Comparison)

فریم‌ورک OpenTelemetry استاندارد صنعتی برای مشاهده‌پذیری مدرن در بستر ابر (Cloud-Native) است.

---

# پیشنهاد نهایی (Final Recommendation)

فریم‌ورک OpenTelemetry برای تمامی موارد ردگیری توزیع‌شده و جمع‌آوری سنجه‌ها اتخاذ شود.

---

# تصمیم نهایی (Final Decision)

| کامپوننت | تصمیم |
|---|---|
| OpenTelemetry | تصویب شد (Approved) |

---

# خلاصه تصمیمات (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ بی‌طرفی نسبت به محیط ابری

---

# سوابق تصمیمات معماری مرتبط (Related ADR)

- ADR-0001 — معماری تمیز (Clean Architecture)

---

# اسناد مرتبط (Related Documents)

- TE-0017 — ارزیابی مشاهده‌پذیری و تله‌متری (Observability and Telemetry Evaluation)

---

# مراجع (References)

- https://opentelemetry.io/

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | ارزیابی اولیه |
| 2.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی‌شده |
| 3.0.0 | 2026-07-18 | معمار راهکار | بازنویسی بر اساس الگوی ارزیابی فناوری |
| 3.1.0 | 2026-07-28 | معمار راهکار | افزودن بخش جدید (دامنه ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
