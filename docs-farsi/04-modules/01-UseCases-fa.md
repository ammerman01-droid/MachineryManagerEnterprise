# موارد استفاده ماژول‌ها (Module Use Cases)

| ویژگی | مقدار |
|----------|-------|
| **شناسه سند** | MOD-001 |
| **نسخه** | 4.0.0 |
| **وضعیت** | فعال |
| **مالک** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-07-28 |

---

# ۱. هدف

این سند اصول معماری حاکم بر تمامی ماژول‌های نرم‌افزاری پلتفرم MachineryManagerEnterprise و کاتالوگ جامع موارد استفاده (Use Cases) را تعریف می‌کند.

این اصول اطمینان حاصل می‌کنند که سازگاری، قابلیت نگهداری، مقیاس‌پذیری و تکامل مستقل ماژول‌ها تضمین شده است.

---

# فلسفه ماژول‌ها

ماژول‌ها مرزهای پیاده‌سازی مشتق‌شده از قابلیت‌های کسب‌وکار هستند.

هر ماژول مالک منطق کاربردی، مدل دامنه، زیرساخت و قراردادهای عمومی خود است.

ماژول‌ها به جای جزئیات پیاده‌سازی داخلی، از طریق رابط‌های صریح با یکدیگر ارتباط برقرار می‌کنند.

---

# چک‌لیست طراحی ماژول

قبل از معرفی یک ماژول جدید، موارد زیر را بررسی کنید:

- آیا نشان‌دهنده یک قابلیت کسب‌وکار است؟
- آیا مالک یک محدوده زمینه (Bounded Context) است؟
- آیا قراردادهای صریح در اختیار می‌گذارد؟
- آیا از وابستگی مستقیم به زیرساخت اجتناب می‌کند؟
- آیا می‌تواند به صورت مستقل تکامل یابد؟

---

# ۲. اصول طراحی موارد استفاده

هر مورد استفاده باید اصول زیر را برآورده سازد:

- مبتنی بر کسب‌وکار (Business oriented)
- مستقل از فناوری (Technology independent)
- قابل تست به صورت مستقل (Independently testable)
- نام‌گذاری شفاف و صریح (Clearly named)
- هدف کسب‌وکاری واحد (Single business objective)
- قابل استفاده مجدد (Reusable)

---

# ۳. نمای کلی ماژول‌ها

پلتفرم به ماژول‌های کسب‌وکار زیر تقسیم می‌شود:

```text
ماژول‌ها (Modules)

├── مدیریت دارایی‌ها (Asset Management)
├── مدیریت موتورها (Engine Management)
├── مدیریت قطعات (Component Management)
├── مدیریت نگهداری و تعمیرات (Maintenance Management)
├── مدیریت کارکردسنج‌ها (Meter Management)
├── مدیریت امور مالی (Financial Management)
├── مدیریت اسناد و مدارک (Document Management)
├── کتابخانه فنی (Technical Library)
├── گالری تصاویر (Gallery)
├── پیش‌بینی (Forecasting)
├── گزارش‌گیری (Reporting)
├── مدیریت سیستم و پرسنل (Administration)
└── پیکربندی و داده‌های پایه (Configuration)
```

هر ماژول مالک موارد استفاده خود است.

---

# ۴. ماژول مدیریت دارایی‌ها (Asset Management Module)

## هدف

مدیریت چرخه حیات کامل دارایی‌های فیزیکی.

## موارد استفاده

- **UC-001**: ثبت دارایی جدید (Register New Asset)
- **UC-002**: ویرایش اطلاعات دارایی (Modify Asset Information)
- **UC-003**: از رده خارج کردن دارایی (Retire Asset)
- **UC-004**: انتقال مالکیت / مسئولیت دارایی (Transfer Asset Ownership)
- **UC-005**: مشاهده تاریخچه دارایی (View Asset History)
- **UC-006**: جستجوی دارایی‌ها (Search Assets)
- **UC-007**: خروجی گرفتن از اطلاعات دارایی (Export Asset Information)

---

# ۵. ماژول مدیریت موتورها (Engine Management Module)

## هدف

مدیریت مستقل موتورها جدا از دارایی‌ها.

## موارد استفاده

- **UC-101**: ثبت موتور (Register Engine)
- **UC-102**: نصب موتور (Install Engine)
- **UC-103**: جداسازی موتور (Remove Engine)
- **UC-104**: تعویض موتور (Replace Engine)
- **UC-105**: ارسال موتور به تعمیرگاه (Send Engine to Workshop)
- **UC-106**: بازگشت موتور از تعمیرگاه (Return Engine from Workshop)
- **UC-107**: مشاهده تاریخچه موتور (View Engine History)
- **UC-108**: جستجوی تاریخچه موتور (Search Engine History)

---

# ۶. ماژول مدیریت قطعات (Component Management Module)

## هدف

مدیریت قطعات و اجزای قابل تعویض.

## موارد استفاده

- **UC-201**: ثبت قطعه (Register Component)
- **UC-202**: نصب قطعه (Install Component)
- **UC-203**: جداسازی قطعه (Remove Component)
- **UC-204**: تعویض قطعه (Replace Component)
- **UC-205**: مشاهده چرخه حیات قطعه (View Component Lifecycle)

---

# ۷. ماژول مدیریت نگهداری و تعمیرات (Maintenance Management Module)

## هدف

مدیریت فعالیت‌های نگهداری پیشگیرانه، اصلاحی و پیش‌بینانه.

## موارد استفاده

- **UC-301**: ایجاد برنامه نت (Create Maintenance Plan)
- **UC-302**: زمان‌بندی نت (Schedule Maintenance)
- **UC-303**: ثبت فعالیت نت (Record Maintenance Activity)
- **UC-304**: ثبت بازرسی (Record Inspection)
- **UC-305**: ثبت خرابی (Register Failure)
- **UC-306**: ثبت تعمیر (Record Repair)
- **UC-307**: ثبت اورهال (Record Overhaul)
- **UC-308**: مشاهده تاریخچه نت (View Maintenance History)
- **UC-309**: مشاهده تاریخچه خرابی‌ها (View Failure History)
- **UC-310**: محاسبه موعد نت بعدی (Calculate Next Maintenance)

---

# ۸. ماژول مدیریت کارکردسنج‌ها (Meter Management Module)

## هدف

مدیریت دستگاه‌های فیزیکی کارکردسنج و کارکرد عملیاتی.

## موارد استفاده

- **UC-401**: نصب دستگاه کارکردسنج (Install Meter Device)
- **UC-402**: تعویض دستگاه کارکردسنج (Replace Meter Device)
- **UC-403**: ثبت قرائت کارکردسنج (Register Meter Reading)
- **UC-404**: ثبت کارکرد غیرعملیاتی (Register Non-operational Usage)
- **UC-405**: اصلاح قرائت نادرست کنتور (Correct Invalid Meter Reading)
- **UC-406**: مشاهده تاریخچه کنتور (View Meter History)
- **UC-407**: محاسبه کارکرد عملیاتی (Calculate Operational Usage)
- **UC-408**: مشاهده خط زمانی کارکرد (View Usage Timeline)

---

# ۹. ماژول مدیریت امور مالی (Financial Management Module)

## هدف

مدیریت تمامی اطلاعات مالی مرتبط با دارایی‌ها.

## موارد استفاده

- **UC-501**: ثبت خرید دارایی (Register Asset Purchase)
- **UC-502**: ثبت هزینه‌های عملیاتی (Record Operating Expense)
- **UC-503**: ثبت هزینه سوخت (Record Fuel Expense)
- **UC-504**: ثبت هزینه نگهداری و تعمیرات (Record Maintenance Cost)
- **UC-505**: ثبت بیمه (Record Insurance)
- **UC-506**: ثبت مالیات و عوارض (Record Tax)
- **UC-507**: محاسبه استهلاک (Calculate Depreciation)
- **UC-508**: محاسبه ارزش فعلی دارایی (Calculate Current Asset Value)
- **UC-509**: محاسبه هزینه کل مالکیت (Calculate Total Cost of Ownership)
- **UC-510**: مشاهده تاریخچه مالی (View Financial History)

---

# ۱۰. ماژول مدیریت اسناد و مدارک (Document Management Module)

## هدف

مدیریت اسناد رسمی و چرخه حیات آن‌ها.

## موارد استفاده

- **UC-601**: ثبت سند (Register Document)
- **UC-602**: بارگذاری تصویر سند (Upload Document Image)
- **UC-603**: بارگذاری فایل PDF سند (Upload PDF Document)
- **UC-604**: تعویض نسخه سند (Replace Document Version)
- **UC-605**: پایش انقضا (Monitor Expiration)
- **UC-606**: تولید یادآوری انقضا (Generate Expiration Reminder)
- **UC-607**: خروجی گرفتن از بسته اسناد (Export Document Package)
- **UC-608**: مشاهده تاریخچه اسناد (View Document History)

---

# ۱۱. ماژول کتابخانه فنی (Technical Library Module)

## هدف

مدیریت مستندات فنی قابل استفاده مجدد بین مدل‌های مختلف ماشین‌آلات.

## موارد استفاده

- **UC-701**: ثبت راهنمای فنی (Register Technical Manual)
- **UC-702**: ثبت کاتالوگ قطعات (Register Parts Catalogue)
- **UC-703**: ثبت راهنمای تعمیرات (Register Service Manual)
- **UC-704**: تخصیص راهنما به مدل ماشین (Assign Manual to Machine Model)
- **UC-705**: مشاهده کتابخانه فنی (View Technical Library)
- **UC-706**: دانلود مستندات فنی (Download Technical Documentation)

---

# ۱۲. ماژول گالری (Gallery Module)

## هدف

ذخیره‌سازی تصاویر تاریخی مرتبط با دارایی‌ها.

## موارد استفاده

- **UC-801**: بارگذاری تصویر دارایی (Upload Asset Image)
- **UC-802**: دسته‌بندی تصویر (Categorize Image)
- **UC-803**: مرور گالری (Browse Gallery)
- **UC-804**: فیلتر تصاویر بر اساس تاریخ (Filter Images by Date)
- **UC-805**: خروجی گرفتن از گالری (Export Gallery)
- **UC-806**: تولید گزارش تصویری (Generate Photo Report)

---

# ۱۳. ماژول پیش‌بینی (Forecasting Module)

## هدف

پیش‌بینی الزامات عملیاتی آتی با استفاده از داده‌های تاریخی کسب‌وکار.

## موارد استفاده

- **UC-901**: تولید پیش‌بینی مصرف سوخت (Generate Fuel Consumption Forecast)
- **UC-902**: تولید پیش‌بینی مصرف روغن (Generate Lubricant Consumption Forecast)
- **UC-903**: تولید پیش‌بینی مصرف خنک‌کننده (Generate Coolant Consumption Forecast)
- **UC-904**: تولید پیش‌بینی مصرف گریس (Generate Grease Consumption Forecast)
- **UC-905**: تولید پیش‌بینی مصرف فیلتر (Generate Filter Consumption Forecast)
- **UC-906**: تولید پیش‌بینی قطعات یدکی (Generate Spare Parts Forecast)
- **UC-907**: تولید پیش‌بینی نت (Generate Maintenance Forecast)
- **UC-908**: تولید پیش‌بینی تعویض قطعات (Generate Component Replacement Forecast)
- **UC-909**: مقایسه پیش‌بینی با مصرف واقعی (Compare Forecast With Actual Consumption)
- **UC-910**: خروجی گرفتن از گزارش پیش‌بینی (Export Forecast Report)

---

# ۱۴. ماژول گزارش‌گیری (Reporting Module)

## هدف

ارائه گزارش‌های عملیاتی، فنی و مالی.

## موارد استفاده

- **UC-1001**: تولید گزارش دارایی (Generate Asset Report)
- **UC-1002**: تولید گزارش موتور (Generate Engine Report)
- **UC-1003**: تولید گزارش نت (Generate Maintenance Report)
- **UC-1004**: تولید گزارش خرابی (Generate Failure Report)
- **UC-1005**: تولید گزارش مالی (Generate Financial Report)
- **UC-1006**: تولید گزارش استهلاک (Generate Depreciation Report)
- **UC-1007**: تولید گزارش هزینه‌های عملیاتی (Generate Operating Cost Report)
- **UC-1008**: تولید گزارش بهره‌وری / نرخ استفاده (Generate Utilization Report)
- **UC-1009**: تولید گزارش وضعیت اسناد (Generate Document Status Report)
- **UC-1010**: تولید داشبورد مدیریتی (Generate Executive Dashboard)

---

# ۱۵. ماژول مدیریت سیستم و پرسنل (Administration Module)

## هدف

مدیریت کاربران، دسترسی‌ها و پیکربندی‌های سازمانی.

## موارد استفاده

- **UC-1101**: ایجاد کاربر (Create User)
- **UC-1102**: غیرفعال‌سازی کاربر (Deactivate User)
- **UC-1103**: تخصیص نقش (Assign Role)
- **UC-1104**: مدیریت دسترسی‌ها (Manage Permissions)
- **UC-1105**: مدیریت سازمان‌ها (Manage Organizations)
- **UC-1106**: مدیریت موقعیت‌ها و سایت‌ها (Manage Locations)
- **UC-1107**: حسابرسی فعالیت‌های کاربر (Audit User Activity)
- **UC-1108**: مشاهده لاگ‌های سیستم (View System Log)

---

# ۱۶. ماژول پیکربندی و داده‌های پایه (Configuration Module)

## هدف

نگهداری اطلاعات مرجع و داده‌های پایه قابل استفاده مجدد.

## موارد استفاده

- **UC-1201**: مدیریت مدل‌های دارایی (Manage Asset Models)
- **UC-1202**: مدیریت مدل‌های موتور (Manage Engine Models)
- **UC-1203**: مدیریت مدل‌های قطعات (Manage Component Models)
- **UC-1204**: مدیریت سازندگان (Manage Manufacturers)
- **UC-1205**: مدیریت تامین‌کنندگان (Manage Suppliers)
- **UC-1206**: مدیریت الگوهای نت (Manage Maintenance Templates)
- **UC-1207**: مدیریت انواع اسناد (Manage Document Types)
- **UC-1208**: مدیریت پارامترهای پیش‌بینی (Manage Forecast Parameters)
- **UC-1209**: مدیریت واحدهای اندازه‌گیری (Manage Units of Measure)
- **UC-1210**: مدیریت قوانین اعلان‌ها (Manage Notification Rules)

---

# ۱۷. موارد استفاده میان‌ماژولی (Cross-Module Use Cases)

فرآیندهای کسب‌وکاری زیر به صورت همزمان چندین ماژول را در بر می‌گیرند:

- **UC-2001**: خرید دارایی دست‌دوم (Purchase Used Asset) ➔ ماژول‌ها: دارایی، مالی، موتور، کارکردسنج
- **UC-2002**: تعویض موتور (Replace Engine) ➔ ماژول‌ها: دارایی، موتور، نت، مالی
- **UC-2003**: تعویض ساعت‌سنج (Replace Hour Meter) ➔ ماژول‌ها: دارایی، کارکردسنج، کارکرد، گزارش‌گیری
- **UC-2004**: تکمیل نگهداری پیشگیرانه (Complete Preventive Maintenance) ➔ ماژول‌ها: نت، مالی، پیش‌بینی
- **UC-2005**: تمدید بیمه‌نامه (Renew Insurance) ➔ ماژول‌ها: اسناد، مالی، اعلان‌ها
- **UC-2006**: اسقاط / فروش دارایی (Dispose Asset) ➔ ماژول‌ها: دارایی، مالی، اسناد، گزارش‌گیری

---

# ۱۸. قوانین نام‌گذاری موارد استفاده

هر مورد استفاده باید:

- نشان‌دهنده یک هدف مشخص کسب‌وکار باشد؛
- با یک فعل شروع شود؛
- برای کاربران کسب‌وکار قابل فهم باشد؛
- مستقل از فناوری باقی بماند؛
- دارای شناسه منحصربه‌فرد باشد.

---

# خلاصه تصمیمات

- ✔ معماری پاک (Clean Architecture)
- ✔ سازگاری با .NET 10
- ✔ رعایت استانداردها
- ✔ خنثی بودن نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط

- `00-ApplicationArchitecture-fa.md`
- `01-Architecture.md`
- `03-BoundedContexts-fa.md`
- `04-DomainModel-fa.md`

---

# تاریخچه تغییرات

| نسخه | تاریخ | شرح |
|----------|------------|----------------------------------------------|
| 1.0.0 | اولیه | تعاریف اولیه موارد استفاده ماژول‌ها |
| 3.0.0 | 2026-07-18 | استانداردسازی مطابق با استاندارد مستندسازی نسخه 3.0 |
| 4.0.0 | 2026-07-28 | ارتقا به استاندارد مستندسازی نسخه 4.0.0 |
