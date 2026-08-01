# مستندات ماژول‌ها و لایه کاربرد (Modules & Application Layer)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | MOD-README |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# هدف

این پوشه شامل مستندات لایه کاربرد (Application Layer)، ماژول‌ها، دستورات (Commands)، پرس‌وجوها (Queries)، پردازنده‌ها (Handlers)، خدمات کاربردی، جریان‌های کاری و مدل احراز دسترسی پروژه **MachineryManagerEnterprise** است.

---

# ترتیب مطالعه اسناد

```text
00-ApplicationArchitecture-fa.md  (معماری لایه کاربرد)
               │
               ▼
01-UseCases-fa.md                 (موارد استفاده ماژول‌ها)
               │
               ▼
02-Commands-fa.md ➔ 03-Queries-fa.md ➔ 04-Handlers-fa.md
               │
               ▼
05-ApplicationServices-fa.md ➔ 06-Workflows-fa.md ➔ 07-Authorization-fa.md
```

---

# فهرست اسناد این پوشه

| سند انگلیسی | نسخه فارسی | شرح |
|---|---|---|
| `00-ApplicationArchitecture.md` | `00-ApplicationArchitecture-fa.md` | معماری لایه کاربرد و ماژول‌ها |
| `01-UseCases.md` | `01-UseCases-fa.md` | کاتالوگ جامع موارد استفاده ماژول‌ها |
| `02-Commands.md` | `02-Commands-fa.md` | کاتالوگ دستورات تغییر وضعیت |
| `03-Queries.md` | `03-Queries-fa.md` | کاتالوگ پرس‌وجوهای خواندن |
| `04-Handlers.md` | `04-Handlers-fa.md` | پردازنده‌های دستور و پرس‌وجو |
| `05-ApplicationServices.md` | `05-ApplicationServices-fa.md` | خدمات کاربردی و رفتارهای پایپ‌لاین |
| `06-Workflows.md` | `06-Workflows-fa.md` | جریان‌های کاری طولانی‌مدت |
| `07-Authorization.md` | `07-Authorization-fa.md` | مدل احراز دسترسی و دسترسی‌ها |

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

- `docs/03-domain/DomainDocumentationIndex-fa.md`
- `docs/02-architecture/01-Architecture-fa.md`

---

# تاریخچه تغییرات

| نسخه | تاریخ | شرح |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | راهنمای اولیه پوشه ماژول‌ها |
| 4.0.0 | 2026-07-28 | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
