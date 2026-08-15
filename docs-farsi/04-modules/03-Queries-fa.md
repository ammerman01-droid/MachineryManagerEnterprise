| ویژگی | مقدار |
|---|---|
| **شناسه سند** | MOD-003 |
| **عنوان** | معماری کوئری‌ها (Query Architecture) |
| **نسخه** | 4.2.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-12 |

---

# هدف

این سند، معماری و قواعد پیاده‌سازی **کوئری‌ها (Queries)** را در تمامی ماژول‌های پلتفرم MachineryManagerEnterprise تعریف می‌کند.

کوئری‌ها نمایانگر عملیات فقط-خواندنی (Read-Only) در الگوی CQRS هستند.

---

# اصول کلیدی (Core Principles)

کوئری‌ها باید:

- هرگز وضعیت سیستم را تغییر ندهند؛
- هرگز رخدادهای دامنه (Domain Events) صادر نکنند؛
- از اعمال منطق تجاری دامنه اجتناب ورزند؛
- برای کارایی و عملکرد خواندن بهینه‌سازی شوند؛
- مستقیماً مدل‌های DTO یا مدل‌های خواندن (Read Models) را برگردانند.

---

# تفکیک CQRS (CQRS Separation)

فرمان‌ها (Commands) و کوئری‌ها (Queries) باید به طور دقیق از یکدیگر تفکیک شوند.

فرمان‌ها (Commands) = جهش و تغییر وضعیت (State Mutation)

کوئری‌ها (Queries) = بازیابی داده‌ها (Data Retrieval)

ترکیب منطق خواندن و نوشتن در یک هندلر واحد اکیداً ممنوع است.

---

# تعریف کوئری (Query Definition)

هر کوئری باید توسط یک کلاس یا رکورد تغییرناپذیر (Immutable Record) نمایش داده شود.

```csharp
public sealed record GetAssetByIdQuery(Guid AssetId) : IRequest<AssetDetailsDto>;
```

کوئری‌ها باید اینترفیس `IRequest<TResponse>` در MediatR را پیاده‌سازی کنند.

---

# مدیریت‌کننده کوئری (Query Handler)

هر کوئری باید یک هندلر متناظر داشته باشد.

```csharp
public sealed class GetAssetByIdQueryHandler 
    : IRequestHandler<GetAssetByIdQuery, AssetDetailsDto>
{
    private readonly IReadDbContext _context;

    public GetAssetByIdQueryHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<AssetDetailsDto> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await _context.Assets
            .AsNoTracking()
            .Where(a => a.Id == request.AssetId)
            .ProjectToType<AssetDetailsDto>()
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
        {
            throw new NotFoundException("Asset", request.AssetId);
        }

        return dto;
    }
}
```

---

# استراتژی ماندگاری خواندن (Read Persistence Strategy)

کوئری‌ها تحت حاکمیت سند **ADR-0019 (استراتژی ماندگاری ترکیبی برای کوئری‌های با بار خواندن سنگین)** اداره می‌شوند.

کوئری‌ها مجاز هستند که مدل دامنه و ریشه‌های تجمیع (Aggregate Roots) را دور بزنند.

فناوری‌های مجاز دسترسی به داده برای کوئری‌ها عبارتند از:

- فریم‌ورک Entity Framework Core (با متد `AsNoTracking()`)
- نگاشت پروجکشن با Mapster (متد `ProjectToType<T>()`)
- کتابخانه Dapper (برای گزارش‌گیری با کارایی بالا)
- نماهای پایگاه داده (Database Views) یا کپی‌های مخصوص خواندن (Read Replicas)

---

# قواعد پروجکشن (Projection Rules)

نگاشت از موجودیت‌های پایگاه داده به DTOها باید در سطح پایگاه داده با استفاده از Mapster یا پروجکشن LINQ انجام شود.

ممنوع:

```csharp
// ممنوع: بارگذاری کامل موجودیت با ردیابی در حافظه قبل از نگاشت
var entity = await _context.Assets.FindAsync(id);
return _mapper.Map<AssetDetailsDto>(entity);
```

الزامی:

```csharp
// الزامی: پروجکشن در سطح پایگاه داده
return await _context.Assets
    .AsNoTracking()
    .Where(x => x.Id == id)
    .ProjectToType<AssetDetailsDto>()
    .FirstOrDefaultAsync(cancellationToken);
```

---

# صفحه‌بندی و فیلترسازی (Pagination & Filtering)

کوئری‌هایی که فهرستی از داده‌ها را برمی‌گردانند باید از صفحه‌بندی، مرتب‌سازی و فیلترسازی با استفاده از مدل‌های استاندارد پشتیبانی کنند.

```csharp
public sealed record GetAssetsPagedQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? SearchTerm = null,
    string? SortBy = null,
    bool SortDescending = false
) : IRequest<PagedResult<AssetSummaryDto>>;
```

---

# قواعد کش‌گذاری (Caching Rules)

پاسخ‌های کوئری می‌توانند در صورت نیاز با استفاده از `FusionCache` یا `IMemoryCache` کش‌گذاری شوند.

باطل‌سازی کش (Cache Invalidation) باید توسط رخدادهای دامنه (Domain Events) صادرشده از فرمان‌ها (Commands) هدایت شود.

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ انطباق با CQRS
- ✔ مدل خواندن با کارایی بالا (High Performance Read Model)
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)

---

# اسناد مرتبط (Related Documents)

- MOD-001 (معماری فرمان‌ها - Command Architecture)
- MOD-002 (معماری رخدادهای دامنه - Domain Events Architecture)
- ADR-0011 (استفاده از MediatR)
- ADR-0019 (استراتژی ماندگاری ترکیبی برای کوئری‌های با بار خواندن سنگین)
- ADR-0008 (استفاده از Mapster)
- ADR-0031 (معماری کش‌گذاری سازمانی)

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | معماری اولیه کوئری‌ها |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-02 | معمار راهکار | به‌روزرسانی بخش استراتژی ماندگاری خواندن جهت ارجاع به ADR-0019 (استراتژی ماندگاری ترکیبی برای کوئری‌های سنگین) |
| 4.2.0 | 2026-08-12 | معمار راهکار | اصلاح ارجاع نادرست از `MOD-001 (Command Pattern)` به `MOD-001 (Command Architecture)` و افزودن ADR-0031 (کش‌گذاری) |
