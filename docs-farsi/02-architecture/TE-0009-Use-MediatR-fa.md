| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0009 |
| **عنوان** | ارزیابی پیام‌رسانی درون‌پردازشی MediatR (MediatR In-Process Messaging Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند فناوری‌های کاندید را برای ارزیابی پیام‌رسانی درون‌پردازشی MediatR در MachineryManagerEnterprise ارزیابی می‌کند.

هدف، ایجاد یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را ضمن حفظ اصول معماری تمیز (Clean Architecture) برآورده سازد.

---

# دامنه ارزیابی (Evaluation Scope)

انتخاب فناوری پیام‌رسانی درون‌پردازشی (In-Process Messaging) را ارزیابی می‌کند. پیام‌رسانی توزیع‌شده به‌طور جداگانه در TE-0012 ارزیابی شده است.

---

# ارتباط با ارزیابی‌های فناوری پیشین (Relationship with Previous Technology Evaluations)

مکانیزم میانجی‌گری (Mediation) درون‌پردازشی الگوی CQRS را برای لایه کاربرد (Application Layer) برقرار می‌سازد.

---

# مراجع معماری (Architectural References)

- ADR-0001 — معماری تمیز (Clean Architecture)
- ADR-0003 — پیاده‌سازی الگوی CQRS (CQRS Pattern Implementation)

---

# دامنه (Scope)

کتابخانه MediatR را در برابر Wolverine و فراخوانی‌های مستقیم سرویس‌ها (Direct Service Invocations) ارزیابی می‌کند.

---

# نیازمندی‌های کارکردی (Functional Requirements)

ارسال و توزیع درون‌پردازشی دستورات/کوئری‌ها (Command/Query dispatch)، انتشار اعلان‌ها (Notification publishing)، میان‌افزارهای IPipelineBehavior برای اعتبارسنجی، لاگ‌گیری و مدیریت تراکنش‌ها.

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

جفت‌شدگی سست (Loose coupling)، ایزولاسیون تمیز مدیریت‌کننده‌ها (Handlers)، اجرای با سربار اندک.

---

# فناوری‌های کاندید (Candidate Technologies)

| فناوری | هدف | وضعیت |
|---|---|---|
| MediatR | میانجی درون‌پردازشی و پایپ‌لاین | انتخاب‌شده (Selected) |
| Direct Application Services (سرویس‌های مستقیم برنامه) | سرویس‌های با جفت‌شدگی محکم | ارزیابی‌شده (Evaluated) |
| Wolverine | موتور میانجی دستورات | ارزیابی‌شده (Evaluated) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|---|---|---|
| A1 | الگوی تفکیک‌شده Request/Handler | حیاتی (Critical) |
| A2 | پشتیبانی از رفتارهای پایپ‌لاین (Pipeline Behaviors) | حیاتی (Critical) |

---

# اصل معماری (Architecture Principle)

کنترلرها و کامپوننت‌های رابط کاربری، درخواست‌ها و دستورات (Requests/Commands) را از طریق IMediator ارسال می‌کنند؛ و Handlerهای لایه کاربرد منطق کسب‌وکار را اجرا می‌نمایند.

---

# ۵. ارزیابی‌های عمیق کاندیدها (Candidate Deep-Dive Evaluations)

## ارزیابی MediatR (MediatR Evaluation)

### نمای کلی (Overview)
کتابخانه MediatR یک کتابخانه پیام‌رسانی درون‌پردازشی بدون تعصب ساختاری (Unopinionated) است که از الگوهای CQRS پشتیبانی می‌کند.

### نقاط قوت معماری (Architectural Strengths)
- فعال‌سازی دغدغه‌های عرضی (Cross-cutting concerns) نظیر اعتبارسنجی، لاگ‌گیری و کش‌گذاری از طریق رفتارهای پایپ‌لاین (Pipeline behaviors).
- تحمیل اصل تک‌مسئولیتی دقیق (Single Responsibility Principle) برای هر Command/Query Handler.

---

# مقایسه جامع فناوری‌ها (Overall Technology Comparison)

کتابخانه MediatR همچنان استاندارد طلایی صنعت برای برنامه‌های مبتنی بر CQRS در #C است.

---

# پیشنهاد نهایی (Final Recommendation)

کتابخانه MediatR برای تمامی مدیریت‌های درون‌پردازشی دستورات و کوئری‌های CQRS اتخاذ شود.

---

# تصمیم نهایی (Final Decision)

| کامپوننت | تصمیم |
|---|---|
| MediatR | تصویب شد (Approved) |

---

# خلاصه تصمیمات (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.

---

# سوابق تصمیمات معماری مرتبط (Related ADR)

- ADR-0003 — پیاده‌سازی الگوی CQRS (CQRS Pattern Implementation)

---

# اسناد مرتبط (Related Documents)

- TE-0001 — ارزیابی پلتفرم برنامه NET 10.

---

# مراجع (References)

- https://github.com/jbogard/MediatR

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | ارزیابی اولیه |
| 2.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی‌شده |
| 3.0.0 | 2026-07-18 | معمار راهکار | بازنویسی بر اساس استاندارد مستندسازی v3.0 |
| 3.1.0 | 2026-07-28 | معمار راهکار | افزودن بخش جدید (دامنه ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
