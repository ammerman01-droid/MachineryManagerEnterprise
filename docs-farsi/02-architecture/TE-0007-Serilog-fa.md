| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0007 |
| **عنوان** | ارزیابی فریم‌ورک لاگ‌گیری Serilog (Serilog Logging Framework Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند فناوری‌های کاندید را برای ارزیابی فریم‌ورک لاگ‌گیری Serilog در MachineryManagerEnterprise ارزیابی می‌کند.

هدف، ایجاد یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را ضمن حفظ اصول معماری تمیز (Clean Architecture) برآورده سازد.

---

# دامنه ارزیابی (Evaluation Scope)

کتابخانه‌های لاگ‌گیری را ارزیابی می‌کند. معماری گسترده‌تر مشاهده‌پذیری در TE-0017 به‌تفصیل شرح داده شده است.

---

# ارتباط با ارزیابی‌های فناوری پیشین (Relationship with Previous Technology Evaluations)

مبنایی برای TE-0017 (ارزیابی فناوری مشاهده‌پذیری و تله‌متری) است.

---

# مراجع معماری (Architectural References)

- ADR-0001 — معماری تمیز (Clean Architecture)
- TE-0017 — ارزیابی مشاهده‌پذیری و تله‌متری (Observability and Telemetry Evaluation)

---

# دامنه (Scope)

کتابخانه Serilog را در برابر NLog و ارائه‌دهندگان پیش‌فرض Microsoft.Extensions.Logging ارزیابی می‌کند.

---

# نیازمندی‌های کارکردی (Functional Requirements)

لاگ‌گیری ساختاریافته JSON، مقصدهای تشخیصی غنی (Console, File, OpenTelemetry, Seq)، غنی‌سازی متنی لاگ‌ها (CorrelationId, TenantId).

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

دریافت و پردازش ناهمگام و غیرمسدودکننده لاگ‌ها، حداقل سربار بر عملکرد سیستم.

---

# فناوری‌های کاندید (Candidate Technologies)

| فناوری | هدف | وضعیت |
|---|---|---|
| Serilog | فریم‌ورک لاگ‌گیری ساختاریافته | انتخاب‌شده (Selected) |
| NLog | کتابخانه لاگ‌گیری جایگزین | ارزیابی‌شده (Evaluated) |
| Default Console Logger | لاگر پیش‌فرض درون‌ساخت مایکروسافت | ارزیابی‌شده (Evaluated) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|---|---|---|
| A1 | لاگ‌گیری ساختاریافته و غنی‌سازی اطلاعات | حیاتی (Critical) |
| A2 | یکپارچگی مقصد با OpenTelemetry | بالا (High) |

---

# اصل معماری (Architecture Principle)

لایه زیرساخت (Infrastructure) انتزاع‌های لاگ‌گیری را که از طریق Microsoft.Extensions.Logging ارائه می‌شوند پیاده‌سازی می‌کند.

---

# ۵. ارزیابی‌های عمیق کاندیدها (Candidate Deep-Dive Evaluations)

## ارزیابی Serilog (Serilog Evaluation)

### نمای کلی (Overview)
کتابخانه Serilog کتابخانه استاندارد و پیشگام (De-facto) لاگ‌گیری ساختاریافته برای برنامه‌های NET. است.

### نقاط قوت معماری (Architectural Strengths)
- اکوسیستم غنی از مقاصد خروجی (Sinks) شامل Elastic، Seq، OTLP، File و Console.
- غنی‌سازی قدرتمند ویژگی‌ها (Property enrichment) و لاگ‌گیری کانتکست‌محور.

---

# مقایسه جامع فناوری‌ها (Overall Technology Comparison)

کتابخانه Serilog در اکوسیستم NET. در زمینه قابلیت‌های لاگ‌گیری ساختاریافته و یکپارچگی با OpenTelemetry پیشتاز است.

---

# پیشنهاد نهایی (Final Recommendation)

کتابخانه Serilog پیکربندی‌شده از طریق انتزاع‌های ILogger اتخاذ شود.

---

# تصمیم نهایی (Final Decision)

| کامپوننت | تصمیم |
|---|---|
| Serilog | تصویب شد (Approved) |

---

# خلاصه تصمیمات (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.

---

# سوابق تصمیمات معماری مرتبط (Related ADR)

- ADR-0001 — معماری تمیز (Clean Architecture)

---

# اسناد مرتبط (Related Documents)

- TE-0017 — ارزیابی مشاهده‌پذیری و تله‌متری (Observability and Telemetry Evaluation)

---

# مراجع (References)

- https://serilog.net/

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
