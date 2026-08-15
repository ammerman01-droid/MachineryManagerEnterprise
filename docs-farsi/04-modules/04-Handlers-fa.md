| ویژگی | مقدار |
|---|---|
| **شناسه سند** | MOD-004 |
| **عنوان** | معماری مدیریت‌کننده‌ها (Handlers Architecture) |
| **نسخه** | 4.7.0 |
| **وضعیت** | تصویب‌شده (Approved) |
| **مالک سند** | معمار راهکار (Solution Architect) |
| **تاریخ ایجاد** | 2026-07-18 |
| **آخرین به‌روزرسانی** | 2026-08-08 |

---

# ۱. هدف (Purpose)

این سند، مسئولیت‌های مدیریت‌کننده‌های فرمان (Command Handlers) و مدیریت‌کننده‌های کوئری (Query Handlers) را تعریف می‌کند.

مدیریت‌کننده‌ها اجرای درخواست‌های لایه کاربرد را هماهنگ می‌نمایند.

مدیریت‌کننده‌ها حاوی قواعد تجاری نیستند.

---

# فلسفه مدیریت‌کننده (Handler Philosophy)

مدیریت‌کننده‌ها اجرا را هماهنگ می‌کنند.

مدیریت‌کننده‌ها جریان کاربرد (Application Flow) را ارکستریت و هدایت می‌کنند.

مدیریت‌کننده‌ها هرگز قواعد تجاری را پیاده‌سازی نمی‌کنند.

رفتار تجاری همواره متعلق به بخش‌های زیر است:

- تجمیع‌ها (Aggregates)
- سرویس‌های دامنه (Domain Services)

مدیریت‌کننده‌ها لایه کاربرد را به لایه دامنه متصل می‌کنند.

---

# قواعد طراحی مدیریت‌کننده (Handler Design Rules)

هر مدیریت‌کننده باید:

- دقیقاً یک درخواست را مدیریت کند.
- بدون وضعیت (Stateless) باشد.
- تنها به انتزاع‌ها (Abstractions) وابسته باشد.
- نتایج کاربردی (Application Results) را برگرداند.
- هرگز موجودیت‌های دامنه را مستقیماً افشا نکند.

---

# ۲. اصول مدیریت‌کننده (Handler Principles)

هر مدیریت‌کننده باید اصول زیر را برآورده سازد:

- مسئولیت یگانه (Single Responsibility)
- یک مدیریت‌کننده به ازای هر فرمان (One Handler per Command)
- یک مدیریت‌کننده به ازای هر کوئری (One Handler per Query)
- مستقل از فناوری (Technology independent)
- بدون وضعیت (Stateless)
- لایه هماهنگ‌سازی نازک (Thin orchestration layer)

منطق تجاری متعلق به تجمیع‌ها و سرویس‌های دامنه است.

---

# ۳. دسته‌بندی‌های مدیریت‌کننده‌ها (Handler Categories)

```text
مدیریت‌کننده‌ها (Handlers)

├── مدیریت‌کننده‌های فرمان (Command Handlers)
└── مدیریت‌کننده‌های کوئری (Query Handlers)
```

---

# ۴. مسئولیت‌های مدیریت‌کننده فرمان (Command Handler Responsibilities)

یک مدیریت‌کننده فرمان باید:

- درخواست کاربردی را اعتبارسنجی کند
- مجوز و دسترسی را بررسی کند
- تجمیع(ها) را بارگذاری نماید
- رفتار تجمیع را فراخوانی کند
- در صورت نیاز سرویس‌های دامنه را فراخوانی نماید
- رخدادهای دامنه را منتشر کند
- تراکنش را کامیت کند
- نتیجه اجرا را بازگرداند

یک مدیریت‌کننده فرمان هرگز نباید قواعد تجاری را پیاده‌سازی کند.

---

# ۵. مسئولیت‌های مدیریت‌کننده کوئری (Query Handler Responsibilities)

یک مدیریت‌کننده کوئری باید:

- درخواست را اعتبارسنجی کند
- مجوز و دسترسی را بررسی کند
- مدل خواندن (Read Model) را بازیابی نماید
- داده‌ها را پروجکت و نگاشت کند
- پاسخ را بازگرداند

یک مدیریت‌کننده کوئری هرگز نباید وضعیت تجاری را تغییر دهد.

---

# ۶. چرخه حیات مدیریت‌کننده فرمان (Command Handler Lifecycle)

```text
دریافت فرمان (Receive Command)

↓

اعطای مجوز (Authorization)

↓

اعتبارسنجی کاربرد (Application Validation)

↓

بارگذاری تجمیع (Load Aggregate)

↓

اجرای رفتار دامنه (Execute Domain Behavior)

↓

جمع‌آوری رخدادهای دامنه (Collect Domain Events)

↓

کامیت تراکنش (Commit Transaction)

↓

انتشار رخدادها (Publish Events)

↓

بازگرداندن نتیجه (Return Result)
```

---

# ۷. چرخه حیات مدیریت‌کننده کوئری (Query Handler Lifecycle)

```text
دریافت کوئری (Receive Query)

↓

اعطای مجوز (Authorization)

↓

اعتبارسنجی کوئری (Validate Query)

↓

مدل خواندن (Read Model)

↓

پروجکشن و تصویرسازی داده (Projection)

↓

بازگرداندن نتیجه (Return Result)
```

---

# ۸. مدیریت‌کننده‌های فرمان دارایی (Asset Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| RegisterAsset | RegisterAssetHandler |
| UpdateAssetInformation | UpdateAssetInformationHandler |
| TransferAsset | TransferAssetHandler |
| RetireAsset | RetireAssetHandler |
| DisposeAsset | DisposeAssetHandler |

---

# ۹. مدیریت‌کننده‌های فرمان موتور (Engine Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| RegisterEngine | RegisterEngineHandler |
| InstallEngine | InstallEngineHandler |
| RemoveEngine | RemoveEngineHandler |
| ReplaceEngine | ReplaceEngineHandler |
| SendEngineToWorkshop | SendEngineToWorkshopHandler |
| ReturnEngineFromWorkshop | ReturnEngineFromWorkshopHandler |
| RegisterEngineRebuild | RegisterEngineRebuildHandler |

---

# ۱۰. مدیریت‌کننده‌های فرمان قطعات و اجزا (Component Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| RegisterComponent | RegisterComponentHandler |
| InstallComponent | InstallComponentHandler |
| RemoveComponent | RemoveComponentHandler |
| ReplaceComponent | ReplaceComponentHandler |
| RetireComponent | RetireComponentHandler |

---

# ۱۱. مدیریت‌کننده‌های فرمان کنتور/کارکردسنج (Meter Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| InstallMeter | InstallMeterHandler |
| ReplaceMeter | ReplaceMeterHandler |
| RegisterMeterReading | RegisterMeterReadingHandler |
| RegisterNonOperationalUsage | RegisterNonOperationalUsageHandler |
| CorrectMeterReading | CorrectMeterReadingHandler |
| ArchiveMeter | ArchiveMeterHandler |

---

# ۱۲. مدیریت‌کننده‌های فرمان نگهداری و تعمیرات (Maintenance Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| RequestMaintenance | RequestMaintenanceHandler |
| CreateMaintenancePlan | CreateMaintenancePlanHandler |
| ApproveMaintenancePlan | ApproveMaintenancePlanHandler |
| ScheduleMaintenance | ScheduleMaintenanceHandler |
| StartMaintenance | StartMaintenanceHandler |
| CompleteMaintenance | CompleteMaintenanceHandler |
| VerifyMaintenance | VerifyMaintenanceHandler |
| CloseMaintenance | CloseMaintenanceHandler |
| CancelMaintenance | CancelMaintenanceHandler |
| SuspendMaintenance | SuspendMaintenanceHandler |
| ResumeMaintenance | ResumeMaintenanceHandler |
| RegisterFailure | RegisterFailureHandler |
| RegisterRepair | RegisterRepairHandler |
| RegisterInspection | RegisterInspectionHandler |
| RegisterOverhaul | RegisterOverhaulHandler |
| ReplaceMaintenanceComponent | ReplaceMaintenanceComponentHandler |

---

# ۱۳. مدیریت‌کننده‌های فرمان مالی (Financial Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| RegisterAssetPurchase | RegisterAssetPurchaseHandler |
| RegisterOperatingExpense | RegisterOperatingExpenseHandler |
| RegisterFuelExpense | RegisterFuelExpenseHandler |
| RegisterMaintenanceExpense | RegisterMaintenanceExpenseHandler |
| RegisterInsuranceExpense | RegisterInsuranceExpenseHandler |
| RegisterTaxExpense | RegisterTaxExpenseHandler |
| CalculateDepreciation | CalculateDepreciationHandler |
| RecalculateAssetValue | RecalculateAssetValueHandler |
| RecalculateOwnershipCost | RecalculateOwnershipCostHandler |

---

# ۱۴. مدیریت‌کننده‌های فرمان اسناد (Document Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| RegisterDocument | RegisterDocumentHandler |
| UploadDocumentImage | UploadDocumentImageHandler |
| UploadDocumentFile | UploadDocumentFileHandler |
| ReplaceDocumentVersion | ReplaceDocumentVersionHandler |
| RenewDocument | RenewDocumentHandler |
| ArchiveDocument | ArchiveDocumentHandler |

---

# ۱۵. مدیریت‌کننده‌های فرمان پیش‌بینی (Forecast Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| GenerateFuelForecast | GenerateFuelForecastHandler |
| GenerateLubricantForecast | GenerateLubricantForecastHandler |
| GenerateCoolantForecast | GenerateCoolantForecastHandler |
| GenerateMaintenanceForecast | GenerateMaintenanceForecastHandler |
| GenerateSparePartsForecast | GenerateSparePartsForecastHandler |
| GenerateReplacementForecast | GenerateReplacementForecastHandler |
| RefreshForecastModels | RefreshForecastModelsHandler |
| ValidateForecast | ValidateForecastHandler |
| ApproveForecast | ApproveForecastHandler |
| ScheduleForecast | ScheduleForecastHandler |
| ConsumeForecast | ConsumeForecastHandler |
| CompleteForecast | CompleteForecastHandler |
| CancelForecast | CancelForecastHandler |

---

# 15a. مدیریت‌کننده‌های فرمان سازمان (Organization Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| RegisterOrganization | RegisterOrganizationHandler |
| AssociateUserWithOrganization | AssociateUserWithOrganizationHandler |

---

# 15b. مدیریت‌کننده‌های فرمان اعلان‌ها (Notification Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| AcknowledgeNotification | AcknowledgeNotificationHandler |
| ArchiveNotification | ArchiveNotificationHandler |
| CancelNotification | CancelNotificationHandler |
| UpdateNotificationPreferences | UpdateNotificationPreferencesHandler |

---

# 15c. مدیریت‌کننده‌های فرمان پیام‌رسانی داخلی (Internal Messaging Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| StartConversation | StartConversationHandler |
| AddConversationParticipant | AddConversationParticipantHandler |
| SendMessage | SendMessageHandler |
| AttachFileToMessage | AttachFileToMessageHandler |
| MarkMessageAsRead | MarkMessageAsReadHandler |
| ArchiveMessage | ArchiveMessageHandler |
| DeleteMessage | DeleteMessageHandler |
| CloseConversation | CloseConversationHandler |
| ReopenConversation | ReopenConversationHandler |

---

# 15d. مدیریت‌کننده‌های فرمان دستیار هوش مصنوعی (AI Assistant Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| AskBusinessQuestion | AskBusinessQuestionHandler |
| RequestRecommendation | RequestRecommendationHandler |
| RequestHistoricalSummary | RequestHistoricalSummaryHandler |
| RequestKnowledgeDiscovery | RequestKnowledgeDiscoveryHandler |
| RequestRiskAssessment | RequestRiskAssessmentHandler |

---

# 15e. مدیریت‌کننده‌های فرمان مدیریت روابط (Relationship Management Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| CreateRelationship | CreateRelationshipHandler |
| ActivateRelationship | ActivateRelationshipHandler |
| ModifyRelationship | ModifyRelationshipHandler |
| ExpireRelationship | ExpireRelationshipHandler |

---

# 15f. مدیریت‌کننده‌های فرمان همگام‌سازی فضای کاری توزیع‌شده (Distributed Workspace Synchronization Command Handlers)

| فرمان (Command) | مدیریت‌کننده (Handler) |
|---|---|
| CreateSynchronizationPackage | CreateSynchronizationPackageHandler |
| TransferSynchronizationPackage | TransferSynchronizationPackageHandler |
| ValidateSynchronizationPackage | ValidateSynchronizationPackageHandler |
| ApplySynchronizationPackage | ApplySynchronizationPackageHandler |
| RequestWorkingSet | RequestWorkingSetHandler |
| ResolveSynchronizationConflict | ResolveSynchronizationConflictHandler |

---

# ۱۶. مدیریت‌کننده‌های کوئری (Query Handlers)

هر کوئری دارای دقیقاً یک مدیریت‌کننده کوئری است.

نمونه‌ها:

| کوئری (Query) | مدیریت‌کننده (Handler) |
|---|---|
| GetAsset | GetAssetHandler |
| SearchAssets | SearchAssetsHandler |
| GetAssetHistory | GetAssetHistoryHandler |
| GetEngine | GetEngineHandler |
| GetMaintenanceHistory | GetMaintenanceHistoryHandler |
| GetCurrentAssetValue | GetCurrentAssetValueHandler |
| GetDocumentPackage | GetDocumentPackageHandler |
| GetFuelForecast | GetFuelForecastHandler |

---

# ۱۷. قواعد تعامل با تجمیع (Aggregate Interaction Rules)

یک مدیریت‌کننده می‌تواند:

- یک تجمیع را بارگذاری کند
- در صورت نیاز چندین تجمیع را بارگذاری کند
- سرویس‌های دامنه را فراخوانی کند
- سرویس‌های زیرساخت را از طریق انتزاع‌ها فراخوانی کند

یک مدیریت‌کننده هرگز نباید وضعیت تجمیع را مستقیماً اصلاح کند.

---

# ۱۸. قواعد تراکنش (Transaction Rules)

به طور معمول:

- یک فرمان
- یک تراکنش
- یک کامیت

اگر چندین تجمیع مشارکت داشته باشند، سازگاری و یکپارچگی باید از قواعد دامنه پیروی کند.

---

# قواعد وابستگی (Dependency Rules)

مدیریت‌کننده‌ها می‌توانند به موارد زیر وابسته باشند:

- اینترفیس‌های مخزن (Repository Interfaces)
- سرویس‌های دامنه (Domain Services)
- واحد کار (Unit of Work)
- سیستم لاگ‌گیری (Logger)
- سرویس‌های کاربردی (Application Services)

مدیریت‌کننده‌ها هرگز نباید مستقیماً به موارد زیر وابسته باشند:

- فریم‌ورک Entity Framework
- دستورات SQL
- پیاده‌سازی‌های زیرساخت

---

# ۱۹. مدیریت خطا (Error Handling)

مدیریت‌کننده‌ها باید موارد زیر را به نتایج کاربردی (Application Results) ترجمه کنند:

- خطاهای اعتبارسنجی
- خطاهای اعطای مجوز
- استثناهای دامنه
- استثناهای همروندی (Concurrency Exceptions)
- استثناهای زیرساخت

---

# ۲۰. قرارداد نام‌گذاری (Naming Convention)

مدیریت‌کننده فرمان:

```
<CommandName>Handler
```

نمونه‌ها:

- RegisterAssetHandler
- InstallEngineHandler
- ReplaceMeterHandler

مدیریت‌کننده کوئری:

```
<QueryName>Handler
```

نمونه‌ها:

- GetAssetHandler
- SearchAssetsHandler
- GetMaintenanceHistoryHandler

---

# ۲۱. مدیریت‌کننده‌های آینده (Future Handlers)

نسخه‌های آینده ممکن است مدیریت‌کننده‌هایی برای موارد زیر معرفی کنند:

- انبار و موجودی کالا (Inventory)
- تدارکات و خرید (Procurement)
- زمان‌بندی ناوگان (Fleet Scheduling)
- عیب‌یابی با هوش مصنوعی (AI Diagnostics)
- همگام‌سازی اینترنت اشیاء (IoT Synchronization)
- کارهای پس‌زمینه (Background Jobs)

تمامی مدیریت‌کننده‌های آینده باید از اصول تعریف‌شده در این سند پیروی کنند.

---

# خلاصه تصمیم (Decision Summary)

- ✔ معماری تمیز (Clean Architecture)
- ✔ سازگاری با NET 10.
- ✔ انطباق با استانداردها
- ✔ بی‌طرفی نسبت به ابر (Cloud Neutrality)
- ✔ آمادگی برای هوش مصنوعی (AI Readiness)
- ✔ قابلیت نگهداری بلندمدت

# اسناد مرتبط (Related Documents)

- 02-Commands.md
- 03-Queries.md
- ../06-decisions/ADR-0036-Validation-Pipeline-Architecture.md
- docs/03-domain/06-DomainServices.md
- docs/03-domain/07-DomainEvents.md
- ADR-0011 — اتخاذ CQRS

---

# تاریخچه بازنگری (Revision History)

| نسخه | تاریخ | نویسنده | توصیف |
|---|---|---|---|
| 1.0.0 | 2026-07-18 | معمار راهکار | معماری اولیه مدیریت‌کننده‌ها |
| 3.0.0 | 2026-07-18 | معمار راهکار | استانداردسازی بر اساس استاندارد مستندسازی نسخه ۳.۰ |
| 4.0.0 | 2026-07-28 | معمار راهکار | ارتقا به استاندارد مستندسازی نسخه ۴.۰.۰ |
| 4.1.0 | 2026-08-02 | معمار راهکار | تکمیل جداول مدیریت‌کننده‌های فرمان نت و پیش‌بینی برای تطابق با تمام فرمان‌های سند 02-Commands.md (شامل CancelMaintenance، ReplaceMaintenanceComponent و ۲ مورد از ۶ فرمان تولید پیش‌بینی) |
| 4.2.0 | 2026-08-02 | معمار راهکار | افزودن بخش 15a مدیریت‌کننده‌های فرمان سازمان، منطبق با فرمان‌های جدید در 02-Commands.md |
| 4.3.0 | 2026-08-02 | معمار راهکار | افزودن بخش 15b مدیریت‌کننده‌های فرمان اعلان‌ها، منطبق با فرمان‌های جدید در 02-Commands.md |
| 4.4.0 | 2026-08-02 | معمار راهکار | افزودن بخش 15c مدیریت‌کننده‌های فرمان پیام‌رسانی داخلی، منطبق با فرمان‌های جدید در 02-Commands.md |
| 4.5.0 | 2026-08-02 | معمار راهکار | افزودن بخش 15d مدیریت‌کننده‌های فرمان دستیار هوش مصنوعی، منطبق با فرمان‌های جدید در 02-Commands.md |
| 4.6.0 | 2026-08-02 | معمار راهکار | افزودن بخش 15e مدیریت‌کننده‌های فرمان مدیریت روابط، منطبق با فرمان‌های جدید در 02-Commands.md |
| 4.7.0 | 2026-08-08 | معمار راهکار | افزودن بخش 15f مدیریت‌کننده‌های فرمان همگام‌سازی فضای کاری توزیع‌شده، منطبق با فرمان‌های جدید در 02-Commands.md. این کار تمام ۶ مجموعه مدیریت‌کننده‌های قبلاً مفقود ماژول‌ها را کامل می‌کند |
