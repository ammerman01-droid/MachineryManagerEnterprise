| ویژگی | مقدار |
|---|---|
| **شناسه سند** | TE-0024 |
| **عنوان** | ارزیابی معماری و فناوری دسترسی به داده (Data Access Technology Evaluation) |
| **نسخه** | 4.1.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-27 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

# هدف (Purpose)

این سند به ارزیابی **استراتژی دسترسی به داده (Data Access Strategy)** مورد استفاده در سراسر راهکار MachineryManagerEnterprise می‌پردازد که بر پایه انتخاب قبلی ORM در سند TE-0004 / ADR-0006 بنا شده است.

سند TE-0004 به پرسشی محدودتر پاسخ داد — "پلتفرم باید از کدام ORM استفاده کند؟" — و Entity Framework Core را برگزید. این ارزیابی به پرسش گسترده‌تری پاسخ می‌دهد: "استراتژی کامل دسترسی به داده هم برای سمت تراکنشی (نوشتن / Write) و هم برای سمت کوئری‌ها و گزارش‌های سنگین (خواندن / Read) پلتفرم چیست؟" با رشد پلتفرم و افزوده شدن گزارش‌های سراسری ناوگان، داشبوردهای تاریخچه نگهداری و کوئری‌های همگام‌سازی فضای کاری توزیع‌شده، برخی از مسیرهای خواندن ممکن است از رویکردی سبک‌تر و مبتنی بر SQL صریح نسبت به مدل موجودیت‌های دارای ردیابی تغییرات (Change-tracked) در EF Core بهره‌مند شوند.

هدف از این ارزیابی عبارت است از:
- تایید مجدد Entity Framework Core 10 به عنوان فناوری اصلی دسترسی به داده برای عملیات تراکنشی (سمت نوشتن)، همسو با ADR-0006؛
- ارزیابی Dapper به عنوان مکمل هدفمند برای کوئری‌های سنگین سمت خواندن و گزارش‌گیری که سربار ردیابی تغییرات EF Core در آنها غیرضروری است؛
- ارزیابی **استراتژی ماندگاری ترکیبی (Hybrid Persistence Strategy)** که هر دو فناوری را تحت قوانینی کاملاً شفاف و مشخص ترکیب می‌کند، به جای آنکه این انتخاب را به صورت صفر و یک ببیند؛
- تعریف دقیق مرزهای مجاز استفاده از هر فناوری، به طوری که استراتژی ترکیبی به یک رویه بدون حاکمیت و لجام‌گسیخته تبدیل نشود.

این ارزیابی جایگزین ADR-0006 نمی‌شود، بلکه معماری مصوب دسترسی به داده را با یک استراتژی شفاف و حاکمیت‌یافته برای بهینه‌سازی کوئری‌های سمت خواندن بسط می‌دهد.

---

# محدوده ارزیابی (Evaluation Scope)

این ارزیابی فناوری صرفاً انتخاب فناوری را مورد سنجش قرار می‌دهد.
جزئیات پیاده‌سازی توسط سوابق تصمیمات معماری (ADRs) مربوطه تعریف می‌شوند.

---

# ارتباط با TE-0004 / ADR-0006 (Relationship with TE-0004 / ADR-0006)

نگاشت شیء-رابطه‌ای (ORM) در ابتدا در سند **TE-0004 — Entity Framework Core** ارزیابی شد و به طور رسمی از طریق **ADR-0006 — استفاده از Entity Framework Core** (وضعیت: پذیرفته‌شده) به تصویب رسید. سند TE-0004 فریم‌ورک EF Core را در برابر Dapper، NHibernate و Linq2Db به عنوان گزینه‌های کاندیدای ORM ارزیابی کرد و Dapper را صراحتاً **به عنوان جایگزین کامل ORM** به دلایلی چون سربار نگاشت دستی، فقدان فریم‌ورک مهاجرت طرح‌واره پایگاه داده و کاهش بهره‌وری در دامنه‌های پیچیده رد نمود.

این ارزیابی آن تصمیم را بازنگشایی نمی‌کند. این سند EF Core را به عنوان فناوری **مستقر (Incumbent)** و اجباری برای تمام عملیات پایداری تراکنشی، سمت نوشتن و دارای ردیابی تغییرات در نظر می‌گیرد. این سند Dapper را صرفاً در نقش محدود و مکمل اجرای کوئری‌های سمت خواندن ارزیابی می‌کند — نقشی که در TE-0004 ارزیابی نشده بود زیرا محدوده TE-0004 انتخاب ORM بود نه استراتژی جامع دسترسی به داده.

---

# مراجع معماری (Architectural References)

این ارزیابی بر پایه مراجع زیر استوار است:
- ADR-0001 — معماری پاک (Clean Architecture)
- ADR-0002 — سیاست اولویت متن‌باز (Open Source First Policy)
- ADR-0006 — استفاده از Entity Framework Core
- ADR-0008 — استفاده از Mapster
- TE-0004 — بررسی فریم‌ورک Entity Framework Core (ارزیابی اولیه ORM)
- TE-0023 — ارزیابی فناوری نگاشت اشیاء (Object Mapping Technology Evaluation)
- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md

---

# محدوده (Scope)

این ارزیابی موارد زیر را پوشش می‌دهد:
- پایداری داده در سمت نوشتن (دستورات / Commands) از طریق انتزاع‌های ریپازیتوری تعریف‌شده در لایه کاربرد (Application layer)؛
- بازیابی داده در سمت خواندن (کوئری‌ها / Queries) برای نماهای لیستی، داشبوردها و سناریوهای گزارش‌گیری؛
- قوانین مرزی حاکم بر شرایط مجاز استفاده از Dapper به جای EF Core.

موارد خارج از محدوده:
- خود انتخاب ORM (که قبلاً در ADR-0006 تصمیم‌گیری شده است).
- ابزارهای مهاجرت طرح‌واره پایگاه داده — که به طور جداگانه در سند آتی TE-0025 (ارزیابی فناوری مهاجرت پایگاه داده) بررسی می‌شود.
- زیرساخت‌های کامل گزارش‌گیری/تحلیلی پیشرفته (نظیر پایگاه داده مجزای Read Model یا لایه OLAP) — که در حال حاضر در محدوده پلتفرم قرار ندارد.

---

# نیازمندی‌های کارکردی (Functional Requirements)

استراتژی دسترسی به داده باید موارد زیر را پشتیبانی نماید:
- نوشتن‌های تراکنشی با ردیابی کامل تغییرات، همزمانی خوش‌بینانه (Optimistic concurrency) و معناشناسی واحد کار (Unit of Work) برای ریشه‌های تجمیعی (Aggregates)؛
- پروجکشن‌های کارآمد سمت خواندن برای نماهای لیستی، داشبوردها و گزارش‌ها، از جمله پروجکشن‌های دربرگیرنده چندین جدول متصل (Joined tables)؛
- کوئری‌نویسی مبتنی بر LINQ برای بخش اعظم پایگاه کد، جهت حفظ بهره‌وری و ایمنی نوع داده در زمان کامپایل؛
- یک راهکار خروج اضطراری صریح و محدود برای نگارش دست‌نویس SQL در مواردی که SQL تولیدشده توسط EF Core برای یک کوئری گزارش‌گیری خاص به طور محسوسی ناکارآمد است.

---

# نیازمندی‌های غیرکارکردی (Non-Functional Requirements)

استراتژی باید موارد زیر را فراهم آورد:
- کدهای SQL قابل پیش‌بینی، بهینه‌سازی‌شده و قابل بازبینی برای کوئری‌های گزارش‌گیری پرحجم (مانند گزارش‌های بهره‌وری سراسری ناوگان در بین هزاران دارایی و خوانش‌های کنتور)؛
- حاکمیت شفاف به گونه‌ای که معرفی فناوری دوم دسترسی به داده، مرزهای معماری پاک را مخدوش نکند و دو شیوه ناسازگار در ماژول‌ها ایجاد ننماید؛
- حداقل پیچیدگی عملیاتی افزوده — بدون نیاز به موتور پایگاه داده جدید و بدون مدل مدیریت اتصال جدید فراتر از آنچه EF Core ارائه می‌دهد؛
- سازگاری کامل با الگوی ریپازیتوری لایه زیرساخت (Infrastructure layer).

---

# فناوری‌های کاندید (Candidate Technologies)

| Technology | Purpose | Status |
|------------|---------|--------|
| Entity Framework Core 10 | فریم‌ورک کامل ORM: ردیابی تغییرات، مایگریشن‌ها، LINQ، واحد کار | **مستقر (Incumbent)** (ADR-0006) |
| Dapper | میکرو ORM: اجرای SQL دست‌نویس با نگاشت سبک اشیاء | ارزیابی‌شده (Evaluated) |
| استراتژی ماندگاری ترکیبی (Hybrid Persistence Strategy) | فریم‌ورک EF Core برای نوشتن، Dapper برای کوئری‌های سنگین هدفمند | ارزیابی‌شده (Evaluated) |

---

# معیارهای ارزیابی (Evaluation Criteria)

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | متن‌باز بودن و پایداری مجوز (Open Source & License Stability) | حیاتی (Critical) |
| A2 | سازگاری با .NET 10 | حیاتی (Critical) |
| A3 | سازگاری با معماری پاک (Clean Architecture Compatibility) | حیاتی (Critical) |
| A4 | بهره‌وری سمت نوشتن (ردیابی تغییرات، واحد کار) | حیاتی (Critical) |
| A5 | کارایی کوئری‌های سمت خواندن در مقیاس بالا | بالا (High) |
| A6 | شفافیت حاکمیت و مرزهای لایه‌ها (Governance / Boundary Clarity) | بالا (High) |
| A7 | تجربه توسعه‌دهنده (Developer Experience) | بالا (High) |
| A8 | قابلیت نگهداری (Maintainability) | بالا (High) |
| A9 | هزینه مهاجرت از وضعیت فعلی (Migration Cost from Current State) | متوسط (Medium) |
| A10 | پیچیدگی عملیاتی (Operational Complexity) | متوسط (Medium) |

---

# اصل معماری (Architecture Principle)

دسترسی به داده باید منحصراً در لایه زیرساخت (Infrastructure layer) و در پشت انتزاع‌های ریپازیتوری تعریف‌شده توسط لایه کاربرد محصور بماند. سمت نوشتن و سمت خواندن ممکن است در درون خود از فناوری‌های متفاوتی استفاده کنند، اما هر دو باید برای لایه‌های Application، Domain و Presentation کاملاً نامرئی باقی بمانند.

```text
Application Layer
   ICommandRepository<Asset>   IAssetReadRepository
        │                            │
        ▼                            ▼
Infrastructure Layer
   EF Core (DbContext,           Dapper (raw SQL,
   change tracking,              read-only projections)
   unit of work)
        │                            │
        ▼                            ▼
              SQL Server Database
```

موجودیت‌های دامنه هرگز نباید به EF Core یا Dapper ارجاع داشته باشند. پروژه SharedKernel هرگز نباید به هیچ‌یک ارجاع دهد. ابزار Dapper هرگز نباید برای نوشتن‌هایی که نیازمند ردیابی تغییرات، توکن‌های همزمانی یا ارسال رویدادهای دامنه (Domain Events) هستند استفاده شود — این موارد تحت این معماری منحصراً مسئولیت EF Core باقی می‌مانند.

---

# 5. Entity Framework Core 10 Evaluation (Incumbent)

## Overview

فریم‌ورک Entity Framework Core 10 ابزار ORM رسمی مایکروسافت برای دات‌نت است که پیش‌تر به عنوان فناوری اصلی دسترسی به داده تحت ADR-0006 تصویب شده است. این بخش به ارزیابی مجدد آن به ویژه در نقش پایداری داده در سمت نوشتن و فناوری پیش‌فرض دسترسی به داده در استراتژی کلی می‌پردازد.

## Architectural Role

```text
Application Layer
   UpdateAssetOperatingHoursCommandHandler
          │
          ▼
   IAssetRepository.GetByIdAsync() / SaveChangesAsync()
          │
          ▼
Infrastructure Layer
   AssetRepository : IAssetRepository
          │
          ▼
   AppDbContext (EF Core, change tracking, unit of work)
          │
          ▼
   SQL Server Database
```

## Architectural Strengths

- پشتیبانی کامل از ردیابی تغییرات (Change tracking) و واحد کار (Unit of Work)، که برای ماندگاری صحیح نامتغیرهای ریشه تجمیعی و ارسال رویدادهای دامنه پس از `SaveChangesAsync()` ضروری است.
- فریم‌ورک مهاجرت پایگاه داده رسمی و بالغ، که تکامل طرح‌واره داده‌ها را در تمام ماژول‌ها مدیریت می‌کند.
- کوئری‌نویسی مبتنی بر LINQ که ایمنی نوع زمان کامپایل را برای اکثر کوئری‌های سمت نوشتن و کوئری‌های متعارف سمت خواندن فراهم می‌سازد.
- یکپارچگی عمیق با `ProjectToType<T>()` در Mapster (سند TE-0023 / ADR-0008) که امکان پروجکشن کارآمد سمت خواندن را بدون ترک EF Core برای کوئری‌های گزارش‌گیری متوسط فراهم می‌سازد.
- پشتیبانی از همزمانی خوش‌بینانه از طریق توکن‌های نسخه ردیف (Row-version tokens).
- ابزارهای عالی و پشتیبانی درجه یک در Visual Studio و Rider.

## Architectural Weaknesses

- برای کوئری‌های گزارش‌گیری بسیار بزرگ با اتصالات چندگانه (مانند گزارش بهره‌وری سراسری ناوگان در طول یک سال مالی)، کدهای SQL تولیدشده توسط EF Core می‌تواند کمتر قابل پیش‌بینی بوده و بهینه‌سازی دستی آن دشوار باشد.
- سربار ردیابی تغییرات برای سناریوهای صرفاً خواندنی و پرحجم غیرضروری است؛ حتی با استفاده از `AsNoTracking()`، ترجمه کوئری و مادی‌سازی همچنان سربار بیشتری نسبت به یک میکرو ORM سبک دارد.
- عبارات پیچیده LINQ در صورت عدم بازبینی ممکن است SQL ناکارآمد تولید کنند — خطری که پیش‌تر در TE-0004 شناسایی شده و از طریق شیوه‌های بازبینی کد و آزمون کارایی کاهش یافته است.

## Operational Characteristics

کاملاً در سراسر پروژه عملیاتی است؛ `AppDbContext` در مرز هر ماژول ثبت شده و منحصراً از طریق انتزاع‌های ریپازیتوری مصرف می‌شود، همسو با سند SolutionStructure.md.

## Scalability

برای بارهای کاری تراکنشی در سطوح همزمانی مورد انتظار از میزبان Blazor Server به خوبی مقیاس می‌پذیرد. برای تجمیع‌های بسیار بزرگ سمت خواندن، کارایی کوئری ممکن است نسبت به SQL دست‌نویس افت کند که استراتژی ترکیبی دقیقاً برای پر کردن این شکاف طراحی شده است.

## Security

تولید خودکار کدهای SQL پارامتریزه جهت محافظت کامل در برابر حملات تزریق SQL (SQL Injection). امنیت در نهایت به مجوزدهی صحیح در سطح لایه کاربرد بستگی دارد، همسو با نتیجه‌گیری ارائه‌شده در TE-0004.

## Developer Experience

عالی؛ تمام اعضای تیم با EF Core کاملاً آشنا هستند و تایید مجدد آن به عنوان پیش‌فرض، هیچ منحنی یادگیری جدیدی ایجاد نمی‌کند.

## Maintainability

عالی؛ فریم‌ورک مایگریشن EF Core تکامل طرح‌واره را کاملاً قابل حسابرسی و برگشت‌پذیر نگه می‌دارد؛ ویژگی‌ای که رویکرد صرفاً مبتنی بر SQL دست‌نویس ارائه نمی‌دهد.

## AI Compatibility

ارتباط مستقیمی ندارد؛ دسترسی به داده یک دغدغه داخلی لایه زیرساخت بدون قرارداد بیرونی مصرف‌شونده است.

## Cloud Neutrality

کاملاً چندسکویی و مستقل از ارائه‌دهنده؛ راهکار فعلی SQL Server را هدف قرار داده است اما مدل ارائه‌دهندگان EF Core امکان مهاجرت به سایر پایگاه‌های داده رابطه‌ای را بدون بازنویسی لایه دسترسی به داده حفظ می‌کند (به استثنای کدهای SQL دست‌نویس در Dapper که اختصاصاً برای گویش پایگاه داده هدف نوشته می‌شوند).

## Typical Usage

```csharp
public sealed class AssetRepository : IAssetRepository
{
    private readonly AppDbContext _context;
    public AssetRepository(AppDbContext context) => _context = context;

    public async Task<Asset?> GetByIdAsync(AssetId id, CancellationToken ct)
        => await _context.Assets
            .Include(a => a.Engine)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
}
```

## Comparison with Dapper

| Aspect | EF Core 10 | Dapper |
|--------|------------|--------|
| Change tracking | Yes | No |
| Migrations | Yes (native) | No (requires TE-0025 decision) |
| Query authoring | LINQ (type-safe) | Hand-written SQL |
| Read-heavy large-join performance | Good | Excellent |
| Write-side unit of work | Excellent | Not designed for this |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| .NET 10 Compatibility | Excellent |
| Write-Side Productivity | Excellent |
| Read-Side Performance at Scale | Good |
| License Stability | Excellent (MIT) |
| Migration Cost | None (incumbent) |

## Relationship with Mapster (TE-0023)

قابلیت `IQueryable<T>` در EF Core مستقیماً با `ProjectToType<T>()` در Mapster ترکیب شده و عبارات پروجکشن را برای اکثر کوئری‌های سمت خواندن به SQL ترجمه می‌کند. این ارتباط به عنوان مسیر خواندن پیش‌فرض تحت استراتژی ترکیبی باقی می‌ماند و Dapper تنها برای کوئری‌های گزارش‌گیری بسیار سنگین که این ترکیب در آنها ناکارآمد است رزرو می‌شود.

## Preliminary Conclusion

فریم‌ورک Entity Framework Core 10 فناوری الزامی و قطعی برای تمام پایداری‌های سمت نوشتن و اکثر کوئری‌های سمت خواندن باقی می‌ماند و نقش آن بدون هیچ قید و شرطی تایید مجدد می‌شود.

---

# 6. Dapper Evaluation

## Overview

ابزار Dapper یک میکرو ORM سبک برای دات‌نت است که در ابتدا توسط تیم Stack Overflow توسعه یافته است. این ابزار `IDbConnection` را با متدهای الحاقی گسترش می‌دهد که کدهای SQL دست‌نویس را اجرا کرده و نتایج را با حداقل سربار بر روی اشیاء C# نگاشت می‌کنند. سند TE-0004 ابزار Dapper را به عنوان جایگزین کامل ORM رد کرد؛ این ارزیابی آن را صرفاً در یک نقش محدود و مکمل در نظر می‌گیرد.

## Architectural Role

```text
Application Layer
   GetFleetUtilizationReportQueryHandler
          │
          ▼
   IFleetUtilizationReadRepository.GetReportAsync()
          │
          ▼
Infrastructure Layer
   FleetUtilizationReadRepository (Dapper, hand-written SQL)
          │
          ▼
   SQL Server Database (same connection/database as EF Core)
```

## Architectural Strengths

- حداقل سربار ممکن: کدهای SQL دقیقاً همان‌گونه که نوشته شده‌اند بدون هزینه ردیابی تغییرات یا ترجمه LINQ اجرا می‌شوند و آن را برای گزارش‌های سنگین با اتصالات متعدد ایده‌آل می‌سازد.
- کنترل کامل بر کدهای SQL تولیدشده، از جمله امکان استفاده از Query Hints، ایندکس‌های خاص و بهینه‌سازی‌های اختصاصی ارائه‌دهنده پایگاه داده که در LINQ دشوار است.
- نگاشت مستقیم و فوق‌العاده سبک بر روی DTOها بدون نیاز به مادی‌سازی موجودیت‌های دامنه در سناریوهای فقط-خواندنی.
- بلوغ، پایداری و استفاده گسترده به عنوان مکمل استاندارد EF Core در سیستم‌های سازمانی دات‌نت.

## Architectural Weaknesses

- عدم پشتیبانی از ردیابی تغییرات، واحد کار و توکن‌های همزمانی — کاملاً نامناسب برای پایداری داده در سمت نوشتن، که دقیقاً دلیل رد آن در TE-0004 به عنوان ORM کامل بود.
- عدم وجود فریم‌ورک مهاجرت بومی؛ تغییرات طرح‌واره باید منحصراً توسط مایگریشن‌های EF Core (یا فناوری انتخاب‌شده در TE-0025) مدیریت شوند.
- از دست رفتن ایمنی نوع در زمان کامپایل برای کدهای SQL دست‌نویس؛ تغییر نام ستون توسط کامپایلر تشخیص داده نمی‌شود.
- ریسک حاکمیت معماری در صورت عدم کنترل: استفاده بدون ضابطه از Dapper می‌تواند به ایجاد سبک‌های موازی و ناهماهنگ در پایگاه کد منجر شود.

## Operational Characteristics

نیازمند هیچ زیرساخت مجزایی نیست؛ بر روی همان `IDbConnection` و کانکشن استرینگ مورد استفاده EF Core کار می‌کند، یعنی بدون استخر اتصال جدید، بدون موتور پایگاه داده جدید و بدون سطح عملیاتی اضافه فراتر از متن SQL.

## Scalability

برای کوئری‌های گزارش‌گیری بزرگ و فقط-خواندنی با اتصالات متعدد عملکرد فوق‌العاده‌ای دارد — این دقیقاً سناریویی است که Dapper به دلیل عدم وجود سربار ردیابی تغییرات یا ترجمه درخت عبارات، عملکرد بهتری نسبت به EF Core ارائه می‌دهد.

## Security

پشتیبانی کامل از کوئری‌های پارامتریزه از طریق اشیاء ناشناس یا DTOها، که در صورت استفاده صحیح همان سطح محافظت در برابر تزریق SQL را مشابه EF Core فراهم می‌سازد. ریسک از کتابخانه به توسعه‌دهنده منتقل می‌شود: از آنجا که SQL دست‌نویس است، خطای الحاق رشته‌ای (String concatenation) در صورت عدم دقت توسعه‌دهنده می‌تواند رخ دهد، خطری که در کوئری‌های مبتنی بر LINQ در EF Core وجود ندارد.

## Developer Experience

نیازمند نگارش و نگهداری مستقیم کدهای SQL است که مهارتی متفاوت و بار بازبینی متفاوتی نسبت به LINQ است. برای تیمی که به EF Core مسلط است، این امر اصطکاک جزئی ایجاد می‌کند، هرچند اکثر توسعه‌دهندگان سازمانی دات‌نت مهارت پایه SQL را دارا هستند.

## Maintainability

در صورت رعایت دقیق قوانین حاکمیت و محدود ماندن به نقش تعریف‌شده خوب است، اما در صورت پراکندگی کدهای SQL بدون انضباط در گزارش‌های متعدد، همگام نگه‌داشتن آنها با تغییرات طرح‌واره مایگریشن‌های EF Core دشوار خواهد بود.

## AI Compatibility

ارتباط مستقیمی ندارد.

## Cloud Neutrality

کاملاً چندسکویی است و Dapper به خودی خود وابستگی به ارائه‌دهنده ابری ایجاد نمی‌کند. با این حال به دلیل نگارش مستقیم SQL برای گویش پایگاه داده هدف، کدهای دست‌نویس Dapper ذاتاً نسبت به کوئری‌های ترجمه‌شده توسط EF Core قابلیت حمل کمتری دارند — موازنه‌ای که برای محدوده کوچک و مشخص کوئری‌های Dapper پذیرفته شده است.

## Typical Usage

```csharp
public sealed class FleetUtilizationReadRepository : IFleetUtilizationReadRepository
{
    private readonly IDbConnection _connection;
    public FleetUtilizationReadRepository(IDbConnection connection)
        => _connection = connection;

    public async Task<IReadOnlyList<FleetUtilizationRowDto>> GetReportAsync(
        Guid organizationId, DateOnly from, DateOnly to, CancellationToken ct)
    {
        const string sql = """
            SELECT a.Id, a.Name, SUM(m.HoursDelta) AS TotalHours
            FROM Assets a
            JOIN MeterReadings m ON m.AssetId = a.Id
            WHERE a.OrganizationId = @organizationId
              AND m.ReadingDate BETWEEN @from AND @to
            GROUP BY a.Id, a.Name
            """;

        var result = await _connection.QueryAsync<FleetUtilizationRowDto>(
            sql, new { organizationId, from, to });
        return result.AsList();
    }
}
```

## Comparison with EF Core 10

| Aspect | Dapper | EF Core 10 |
|--------|--------|------------|
| Change tracking | No | Yes |
| Migrations | No (relies on EF Core / TE-0025) | Yes (native) |
| Query authoring | Hand-written SQL | LINQ (type-safe) |
| Read-heavy large-join performance | Excellent | Good |
| Compile-time safety | Low | High |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Good (when strictly confined to read repositories) |
| .NET 10 Compatibility | Excellent |
| Write-Side Productivity | Poor (not intended for this role) |
| Read-Side Performance at Scale | Excellent |
| License Stability | Excellent (MIT) |
| Migration Cost | Low (additive, not a replacement) |

## Relationship with Entity Framework Core

ابزار Dapper هرگز به عنوان جایگزین EF Core مطرح نیست و هرگز در DbContext شریک نشده و در ردیابی تغییرات یا واحد کار مشارکت نمی‌کند. این ابزار صرفاً به عنوان یک مسیر مستقل و فقط-خواندنی برای اجرای کوئری در برابر همان پایگاه داده فیزیکی عمل می‌کند که توسط قوانین مرزی تعریف‌شده در بخش استراتژی ترکیبی زیر هدایت می‌شود.

## Preliminary Conclusion

ابزار Dapper یک مکمل قوی و کم‌ریسک برای مجموعه محدودی از کوئری‌های گزارش‌گیری پرحجم است، اما هرگز نباید به عنوان جایگزین عمومی EF Core اتخاذ شود، همسو با نتیجه‌گیری اولیه TE-0004.

---

# 7. Hybrid Persistence Strategy Evaluation

## Overview

استراتژی ماندگاری ترکیبی یک فناوری سوم نیست بلکه یک الگوی معماری حاکمیت‌یافته است: EF Core 10 به عنوان فناوری اجباری برای تمام پایداری‌های سمت نوشتن و پیش‌فرض کوئری‌های سمت خواندن باقی می‌ماند، در حالی که Dapper تنها برای مجموعه‌ای مشخص و توجیه‌شده از کوئری‌های گزارش‌گیری فقط-خواندنی که سربار EF Core در آنها به طور محسوسی قابل توجه است مجاز شمرده می‌شود.

## Architectural Role

```text
Application Layer
   ICommandRepository<T>              IReadRepository<T>
   (write side, always EF Core)       (read side, EF Core by default)
                                              │
                                   large multi-join
                                   reporting query?
                                     │           │
                                    No           Yes
                                     │           │
                                     ▼           ▼
                              EF Core        Dapper
                          (ProjectToType)   (hand-written SQL,
                                             justified in a
                                             module's ADR-style
                                             README note)
```

## Governance Rules

برای جلوگیری از ریسک شناسایی‌شده در ارزیابی Dapper (یک شیوه دسترسی به داده بدون حاکمیت)، استراتژی ترکیبی تنها تحت ۵ قانون صریح زیر اتخاذ می‌شود:

1. **فریم‌ورک EF Core پیش‌فرض است.** هر کوئری سمت خواندن ابتدا به عنوان پروجکشن `IQueryable<T>` با Mapster در EF Core پیاده‌سازی می‌شود (سند TE-0023). ابزار Dapper تنها پس از شناسایی مشکل کارایی در یک کوئری خاص در نظر گرفته می‌شود.
2. **استفاده از Dapper اختیاری به ازای هر کوئری است نه به ازای هر ماژول.** یک ماژول ممکن است برای ۹۵٪ کوئری‌های خود منحصراً از EF Core و تنها برای یک کوئری گزارش‌گیری از Dapper استفاده کند؛ مفهومی به نام "ماژول Dapper" وجود ندارد.
3. **ریپازیتوری‌های مبتنی بر Dapper منحصراً فقط-خواندنی (Read-Only) هستند.** هیچ دستور `INSERT`، `UPDATE` یا `DELETE` نباید از طریق Dapper در هیچ کجای سیستم اجرا شود. تنها کوئری‌های `SELECT` مجاز هستند.
4. **مالکیت طرح‌واره پایگاه داده منحصراً در اختیار مایگریشن‌های EF Core باقی می‌ماند** (یا فناوری انتخاب‌شده در سند TE-0025). هر زمان که یک مایگریشن ستونی را تغییر می‌دهد که در کدهای SQL دست‌نویس Dapper استفاده شده است، آن کدهای Dapper باید به‌روزرسانی شوند؛ این یک گام اجباری در Definition of Done برای هر مایگریشن مرتبط است.
5. **هر ریپازیتوری خواندن مبتنی بر Dapper باید در یک کامنت در بالای فایل، دلیل مشخص فنی و توجیه کارایی خود برای عدم استفاده از EF Core را مستند نماید** (مانند: "جلوگیری از مادی‌سازی بیش از ۴۰ هزار ردیف در هر درخواست در یک اتصال ۴ جدول؛ بنچمارک نشان داد N میلی‌ثانیه سریع‌تر از معادل EF Core است").

## Architectural Strengths

- بهره‌گیری از مزیت کارایی Dapper برای کوئری‌های خاص نیازمند آن، بدون رها کردن ردیابی تغییرات، مایگریشن‌ها و ایمنی نوع EF Core در بقیه بخش‌های پلتفرم.
- قوانین حاکمیتی فوق مستقیماً بزرگ‌ترین ریسک شناسایی‌شده در ارزیابی Dapper مستقل (چندگانگی شیوه کدنویسی) را با صریح، توجیه‌شده و فقط-خواندنی کردن Dapper مدیریت می‌کنند.
- حفظ تجربه توسعه‌دهندگی پیش‌فرض پلتفرم به صورت دست‌نخورده: توسعه‌دهنده‌ای که ویژگی جدیدی می‌سازد با LINQ و EF Core کار می‌کند مگر آنکه یک مشکل کارایی مستند استفاده از گزینه دیگر را توجیه کند.

## Architectural Weaknesses

- معرفی دو شیوه دسترسی به داده در پایگاه کد که — حتی با وجود حاکمیت — سطح مفاهیمی را که عضو جدید تیم باید بیاموزد افزایش می‌دهد.
- نیازمند انضباط مداوم در بازبینی کد (Code Review) جهت اطمینان از عدم استفاده بی‌رویه از Dapper و حفظ هماهنگی کوئری‌های Dapper با مایگریشن‌های طرح‌واره.
- افزودن حجم اندکی از تست‌های یکپارچگی مجزا برای ریپازیتوری‌های خواندن مبتنی بر Dapper در برابر اتصالات پایگاه داده واقعی.

## Operational Characteristics

بدون زیرساخت عملیاتی جدید: هر دو فناوری EF Core و Dapper کانکشن استرینگ، پایگاه داده فیزیکی و خط لوله استقرار مشترک دارند.

## Scalability

این نقطه قوت اصلی این استراتژی است: به پلتفرم اجازه می‌دهد سنگین‌ترین کوئری‌های گزارش‌گیری خود (مانند بهره‌وری ناوگان یا تجمیع هزینه‌های نگهداری سال مالی) را بدون پرداخت سربار ردیابی تغییرات و مادی‌سازی موجودیت‌ها در هر درخواست مقیاس دهد، در حالی که تمام کوئری‌های دیگر از بهره‌وری EF Core منتفع می‌شوند.

## Security

پروفایل امنیتی هر دو فناوری را به ارث می‌برد. قانون حاکمیتی مبنی بر الزام استفاده فقط-خواندنی از Dapper، پرریسک‌ترین دسته خطاهای SQL دست‌نویس (مسیر نوشتن تصادفی یا مخرب بدون رعایت نامتغیرهای دامنه و ردیابی تغییرات) را کاملاً برطرف می‌سازد.

## Developer Experience

برای حالت پیش‌فرض (EF Core + Mapster) بدون تغییر است. برای تعداد اندکی از توسعه‌دهندگانی که ریپازیتوری‌های خواندن مبتنی بر Dapper را می‌نویسند یا بازبینی می‌کنند کمی بار کاری بیشتری دارد زیرا باید توجیه کارایی را طبق قانون ۵ مستند کنند.

## Maintainability

خوب است، به شرط آنکه قوانین حاکمیتی فوق از طریق فرآیند بازبینی کد اعمال گردند — همان سازوکار اجرایی که برای سایر قوانین معماری در DependencyRules.md استفاده می‌شود.

## AI Compatibility

قابل اعمال نیست.

## Cloud Neutrality

معادل ارزیابی‌های مجزای EF Core و Dapper است؛ تعداد اندک کوئری‌های Dapper تنها بخشی از لایه دسترسی به داده است که به طور خودکار قابلیت حمل میان ارائه‌دهندگان ندارد و این یک موازنه آگاهانه و با محدوده مشخص است.

## Typical Usage

استراتژی ترکیبی به صورت ساختاری بیان می‌شود نه از طریق یک نمونه کد واحد: اینترفیس‌های `IAssetRepository` (قابلیت نوشتن، صرفاً EF Core) و `IFleetUtilizationReadRepository` (فقط-خواندنی، Dapper، همان‌گونه که در ارزیابی Dapper نشان داده شد) به عنوان اینترفیس‌های مجزا ثبت می‌شوند تا تمایز دو شیوه در سطح تزریق وابستگی و اینترفیس کاملاً آشکار باشد و درون یک کلاس واحد پنهان نشود.

## Comparison with EF-Core-Only Strategy

| Aspect | Hybrid Strategy | EF-Core-Only |
|--------|------------------|----------------|
| Write-side integrity | Excellent (EF Core, unchanged) | Excellent |
| Large reporting query performance | Excellent (Dapper where justified) | Good |
| Governance overhead | Low (5 explicit rules) | None |
| Idiom consistency | Two idioms, clearly bounded | One idiom |

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent (both idioms remain confined to Infrastructure layer) |
| .NET 10 Compatibility | Excellent |
| Write-Side Productivity | Excellent (unchanged, EF Core only) |
| Read-Side Performance at Scale | Excellent |
| License Stability | Excellent (both MIT) |
| Migration Cost | Low — additive, opt-in per query |

## Relationship with ADR-0006

این استراتژی به هیچ وجه ADR-0006 را تغییر نمی‌دهد؛ EF Core فناوری انحصاری سمت نوشتن و پیش‌فرض سمت خواندن باقی می‌ماند. این استراتژی یک بسط مشخص و حاکمیت‌یافته برای کوئری‌های گزارش‌گیری فقط-خواندنی اضافه می‌کند.

## Preliminary Conclusion

استراتژی ماندگاری ترکیبی رویکرد توصیه‌شده است: تمام تصمیمات تثبیت‌شده در TE-0004 / ADR-0006 را حفظ می‌کند و در عین حال یک مسیر صریح و حاکمیت‌یافته برای کوئری‌های در مقیاس گزارش‌گیری که EF Core به تنهایی کارایی کمتری در آنها دارد فراهم می‌آورد.

---

# 8. Overall Technology Comparison

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|-----------------|------------------------|--------------|---------|
| All write-side / transactional persistence | EF Core 10 | — (mandatory, no alternative) | Change tracking, unit of work, migrations |
| Default read-side queries | EF Core 10 + Mapster `ProjectToType<T>()` | — | Type-safe, productive, sufficient for most queries |
| Justified high-volume reporting queries | Dapper (read-only) | EF Core (fallback if not yet justified) | Predictable, hand-tuned SQL for large joins |

## Capability Comparison

| Capability | EF Core 10 | Dapper | Hybrid Strategy |
|------------|------------|--------|-------------------|
| Open Source (ADR-0002 compliant) | Yes | Yes | Yes |
| Write-side change tracking | Yes | No | Yes (via EF Core) |
| Migrations | Yes | No | Yes (via EF Core) |
| Large-join read performance | Good | Excellent | Excellent (where justified) |
| Compile-time query safety | High (LINQ) | Low (raw SQL) | High by default, low only where opted-in |
| Governance overhead | None | High if ungoverned | Low (explicit rules defined) |
| Migration cost from current state | None | N/A alone | Low, additive |

## Cloud Neutrality Assessment

هر دو فناوری در اصل چندسکویی و مستقل از ارائه‌دهنده هستند. استراتژی ترکیبی یک افت محدود و آگاهانه در قابلیت حمل ارائه‌دهنده را برای تعداد اندک کوئری‌های گزارش‌گیری Dapper در ازای بهبودهای کارایی ملموس در همان کوئری‌ها می‌پذیرد.

## Enterprise Suitability

| Criterion | EF Core 10 | Dapper (standalone) | Hybrid Strategy |
|-----------|------------|----------------------|--------------------|
| Suitable as platform-wide default | Yes | No | Yes (as the governing strategy) |
| Suitable for write-side persistence | Yes | No | Yes (via EF Core) |
| Suitable for large reporting queries | Acceptable | Excellent | Excellent |

## Clean Architecture Compliance

هر سه گزینه می‌توانند به درستی در لایه زیرساخت محصور شوند. قوانین حاکمیتی استراتژی ترکیبی دقیقاً برای حفظ این انطباق با ورود شیوه دوم تدوین شده‌اند تا از نشت مسئولیت‌های سمت نوشتن که منحصراً متعلق به EF Core است به Dapper جلوگیری کنند.

## Risk Assessment

| Risk | Affected Option | Severity | Mitigation |
|------|--------------------|----------|------------|
| Idiom fragmentation | Hybrid Strategy | Medium | Governance rules 1–5, enforced via code review |
| Reporting queries slow at scale if Hybrid is rejected | EF-Core-Only | Medium | Adopt Hybrid strategy as recommended |
| Accidental write via Dapper bypassing domain invariants | Hybrid Strategy | High if unmitigated | Governance rule 3 (read-only enforcement) |
| Dapper SQL drifting from schema after a migration | Hybrid Strategy | Medium | Governance rule 4 (mandatory update step in Definition of Done) |

## Overall Evaluation

فریم‌ورک EF Core 10 برای تمام پایداری‌های سمت نوشتن اجباری و بدون تغییر باقی می‌ماند و ADR-0006 را کاملاً تایید مجدد می‌کند. پذیرش مستقل Dapper به عنوان جایگزین کامل ORM همچنان به درستی رد می‌شود، همسو با TE-0004. استراتژی ماندگاری ترکیبی که تحت پنج قانون حاکمیتی صریح تعریف‌شده در بالا مدیریت می‌شود، بسط توصیه‌شده است: این استراتژی مزیت کارایی Dapper در سمت خواندن را برای مجموعه محدودی از کوئری‌های گزارش‌گیری توجیه‌شده بدون تضعیف معماری سمت نوشتن یا ایجاد هرج‌ومرج در شیوه کدنویسی فراهم می‌سازد.

---

# 9. Final Recommendation

## Core Technology Stack

| Responsibility | Selected Technology | Rationale |
|-----------------|----------------------|-----------|
| Write-side persistence | EF Core 10 | Reaffirmed incumbent (ADR-0006); mandatory, no exceptions |
| Default read-side queries | EF Core 10 + Mapster | Type-safe, productive, sufficient for the large majority of queries |
| Justified high-volume reporting queries | Dapper (read-only, governed) | Predictable, hand-tuned SQL for large multi-join reports |

## Recommended Architecture

```text
Application Layer
   Write side: ICommandRepository<T>       Read side: IReadRepository<T>
        │                                        │
        ▼                                        ▼
Infrastructure Layer
   EF Core (AppDbContext,             EF Core + Mapster (default)
   change tracking,                          │
   migrations, unit of work)         Dapper (opt-in, read-only,
        │                            justified per governance rules)
        ▼                                        │
              SQL Server Database  ◄──────────────┘
```

## Governance Summary

پنج قانون حاکمیتی تعریف‌شده در بخش ۷ (EF Core پیش‌فرض است؛ Dapper اختیاری به ازای هر کوئری است؛ ریپازیتوری‌های Dapper فقط-خواندنی هستند؛ مالکیت طرح‌واره با مایگریشن‌های EF Core باقی می‌ماند؛ هر کوئری Dapper توجیه کارایی خود را مستند می‌کند) به عنوان رویه الزام‌آور اتخاذ شده و از طریق فرآیند بازبینی کد تحت فرآیند موجود DependencyRules.md اعمال می‌گردند.

## Security Recommendations

فرآیند بازبینی کد باید صراحتاً بررسی و تایید کند که هیچ ریپازیتوری مبتنی بر Dapper دستورات نوشتن صادر نکند، همسو با قانون حاکمیتی ۳.

## Cloud Neutrality

پشته توصیه‌شده قابلیت حمل ارائه‌دهنده را برای کل سمت نوشتن و اکثریت قریب به اتفاق سمت خواندن حفظ می‌کند؛ تعداد اندک کوئری‌های Dapper یک استثنای آگاهانه، با محدوده باریک و مستندشده را تشکیل می‌دهند.

## AI Readiness

برای این ارزیابی قابل اعمال نیست.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| EF Core 10 (Incumbent, write-side) | **Reaffirmed** |
| EF Core 10 + Mapster (default read-side) | **Reaffirmed** |
| Dapper (standalone, full ORM replacement) | Rejected — consistent with TE-0004 |
| Hybrid Persistence Strategy (governed, read-only Dapper) | **Approved** |

---

# Decision Summary

- ✔ حفظ کامل معماری پاک (Clean Architecture preserved)
- ✔ سازگاری با .NET 10 (.NET 10 Compatibility)
- ✔ انطباق با سیاست اولویت متن‌باز (ADR-0002)
- ✔ عدم ایجاد اختلال در پیاده‌سازی موجود سمت نوشتن
- ✔ تعریف قوانین حاکمیتی صریح برای جلوگیری از پراکندگی شیوه کدنویسی
- ✔ تعریف مسیر کارایی سمت خواندن برای کوئری‌های بزرگ گزارش‌گیری

این ارزیابی تصمیم **ADR-0006 — استفاده از Entity Framework Core** را بدون تغییر **مجدداً تایید می‌نماید** و استراتژی ماندگاری ترکیبی را به عنوان یک بسط مصوب و حاکمیت‌یافته رسماً معرفی می‌کند. از آنجا که استراتژی ترکیبی قوانین حاکمیتی الزام‌آوری معرفی می‌نماید که توسعه‌دهندگان آینده باید از آن پیروی کنند، پنج قانون انطباق آن به عنوان یک تصمیم معماری درجه‌یک در **ADR-0019 — استراتژی ماندگاری ترکیبی برای کوئری‌های سنگین سمت خواندن** ثبت شده است، زیرا خود ADR-0006 صرفاً به انتخاب ORM پرداخته بود و نه به این استراتژی جامع‌تر.

---

# Related ADR

```text
ADR-0006 (Reaffirmed — no change)
ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries (new)
```

---

# Related Documents

- TE-0004 — بررسی فریم‌ورک Entity Framework Core (ارزیابی اولیه ORM)
- TE-0023 — ارزیابی فناوری نگاشت اشیاء (Object Mapping Technology Evaluation)
- ADR-0002 — سیاست اولویت متن‌باز (Open Source First Policy)
- ADR-0006 — استفاده از Entity Framework Core
- ADR-0008 — استفاده از Mapster
- ADR-0019 — Hybrid Persistence Strategy for Read-Heavy Queries
- Dependency Catalog
- Dependency Rules

---

# References

https://learn.microsoft.com/ef/core/
https://github.com/dotnet/efcore
https://github.com/DapperLib/Dapper
https://www.learndapper.com/

---

# Revision History

| Version | Date       | Author             | Description                                    |
|---------|------------|--------------------|------------------------------------------------|
| 1.0.0   | 2026-07-27 | Solution Architect | Initial evaluation; reaffirms ADR-0006 (EF Core), evaluates Dapper as a governed read-side complement, recommends Hybrid Persistence Strategy with five explicit governance rules |
| 1.1.0   | 2026-07-27 | Solution Architect | Updated to reference ADR-0019, created to formalize the Hybrid Persistence Strategy's five Compliance Rules |
| 1.1.1   | 2026-07-28 | File name Changed from (Data Access Technology Evaluation)          |
| 1.3.0   | 2026-07-28 | New section added (Evaluation Scope)                                |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0      |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes |