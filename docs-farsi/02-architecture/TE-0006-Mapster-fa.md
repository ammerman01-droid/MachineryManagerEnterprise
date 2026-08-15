| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0006 |
| **عنوان** | ارزیابی فناوری نگاشت اشیاء با Mapster (Mapster Object Mapping Technology Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# هدف (Purpose)

این سند فناوری‌های کاندید را برای ارزیابی فناوری نگاشت اشیاء با Mapster در MachineryManagerEnterprise ارزیابی می‌کند.

هدف، ایجاد یک انتخاب فناوری یکپارچه است که تمامی نیازمندی‌های کارکردی و معماری را ضمن حفظ اصول معماری تمیز (Clean Architecture) برآورده سازد.

---

# دامنه ارزیابی (Evaluation Scope)

کتابخانه‌های نگاشت اشیاء را ارزیابی می‌کند. استراتژی تفصیلی نگاشت در TE-0023 پوشش داده شده است.

---

# ارتباط با ارزیابی‌های فناوری پیشین (Relationship with Previous Technology Evaluations)

مکمل TE-0023 (ارزیابی استراتژی نگاشت اشیاء) است.

---

# مراجع معماری (Architectural References)

- ADR-0001 — معماری تمیز (Clean Architecture)
- TE-0023 — ارزیابی استراتژی نگاشت اشیاء (Object Mapping Strategy Evaluation)

---

# دامنه (Scope)

کتابخانه Mapster را در برابر AutoMapper و نگاشت دستی (Manual Mapping) ارزیابی می‌کند.

---

# نیازمندی‌های کارکردی (Functional Requirements)

تولید کد در زمان کامپایل / عملکرد بالا، پشتیبانی از پروژکشن برای IQueryable در EF Core.

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

سربار حافظه صفر در زمان اجرا، API تمیز، پشتیبانی کامل از NET 10.

---

# فناوری‌های کاندید (Candidate Technologies)

| فناوری | هدف | وضعیت |
|---|---|---|
| Mapster | موتور اصلی نگاشت اشیاء | انتخاب‌شده (Selected) |
| AutoMapper | نگاشت‌کننده مبتنی بر Reflection | ارزیابی‌شده (Evaluated) |
| Manual Extension Methods (متدهای توسعه دستی) | نگاشت کد صریح | ارزیابی‌شده (Evaluated) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| شناسه | معیار | وزن |
|---|---|---|
| A1 | سرعت اجرا و بهره‌وری حافظه | حیاتی (Critical) |
| A2 | پشتیبانی از پروژکشن IQueryable | حیاتی (Critical) |

---

# اصل معماری (Architecture Principle)

نگاشت‌ها موجودیت‌های دامنه (Domain entities) را از قراردادهای DTO خارجی جدا می‌سازند (Decouple).

---

# ۵. ارزیابی‌های عمیق کاندیدها (Candidate Deep-Dive Evaluations)

## ارزیابی Mapster (Mapster Evaluation)

### نمای کلی (Overview)
کتابخانه Mapster یک نگاشت‌کننده شیء-به-شیء با کارایی بالا برای NET. است.

### نقاط قوت معماری (Architectural Strengths)
- سرعت اجرای به مراتب بالاتر و تخصیص حافظه کمتر نسبت به AutoMapper.
- متد بومی `ProjectToType<T>()` برای کوئری‌های LINQ در EF Core.

---

# مقایسه جامع فناوری‌ها (Overall Technology Comparison)

کتابخانه Mapster به سرعت اجرای کد دستی و نزدیک به بومی دست می‌یابد در حالی که کدهای تکراری (Boilerplate) را کاهش می‌دهد.

---

# پیشنهاد نهایی (Final Recommendation)

کتابخانه Mapster به‌عنوان موتور رسمی نگاشت اشیاء اتخاذ شود.

---

# تصمیم نهایی (Final Decision)

| کامپوننت | تصمیم |
|---|---|
| Mapster | تصویب شد (Approved) |

---

# خلاصه تصمیمات (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.

---

# سوابق تصمیمات معماری مرتبط (Related ADR)

- ADR-0001 — معماری تمیز (Clean Architecture)

---

# اسناد مرتبط (Related Documents)

- TE-0023 — ارزیابی استراتژی نگاشت اشیاء (Object Mapping Strategy Evaluation)

---

# مراجع (References)

- https://github.com/MapsterMapper/Mapster

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توضیحات |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | ارزیابی اولیه |
| 2.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی‌شده |
| 3.0.0 | 2026-07-18 | معمار راهکار | بازنویسی بر اساس الگوی ارزیابی فناوری |
| 3.0.1 | 2026-07-27 | معمار راهکار | اصلاح ارجاع ADR مرتبط از ADR-0009 به ADR-0008 (ADR-0009 مستندساز Serilog است نه Mapster) |
| 3.1.0 | 2026-07-28 | معمار راهکار | افزودن بخش جدید (دامنه ارزیابی) |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی v4.0.0 |
| 4.1.0 | 2026-08-08 | معمار راهکار | بازبینی و همگام‌سازی با آخرین تغییرات |
