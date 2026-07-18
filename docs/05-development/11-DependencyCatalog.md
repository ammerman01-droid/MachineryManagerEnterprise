| Layer          | Package                                 | Purpose                      | Required | Status          | ADR          |
| -------------- | --------------------------------------- | ---------------------------- | -------- | --------------- | ------------ |
| SharedKernel   | —                                       | بدون وابستگی خارجی           | ✔        | Approved        | —            |
| Domain         | —                                       | Domain باید کاملاً Pure باشد | ✔        | Approved        | —            |
| Application    | FluentValidation                        | Validation Pipeline          | ✔        | Pending Version | ADR Required |
| Application    | MediatR                                 | CQRS / Messaging             | ✔        | Pending Version | ADR Required |
| Infrastructure | Microsoft.EntityFrameworkCore           | ORM                          | ✔        | Pending Version | ADR Required |
| Infrastructure | Microsoft.EntityFrameworkCore.SqlServer | SQL Server Provider          | ✔        | Pending Version | ADR Required |
| Infrastructure | Serilog.AspNetCore                      | Logging                      | ✔        | Pending Version | ADR Required |
| Infrastructure | AspNetCore.HealthChecks.SqlServer       | Health Monitoring            | ✔        | Pending Version | ADR Required |
| Infrastructure | Mapster                                 | Object Mapping               | ✔        | Pending Version | ADR Required |
| Presentation   | MudBlazor                               | UI Framework                 | ✔        | Pending Version | ADR Required |
| Presentation   | Blazor.PersianDatePicker                | Persian Calendar             | ✔        | Pending Version | ADR Required |
| Presentation   | MD.PersianDateTime.Standard             | Persian DateTime             | ✔        | Pending Version | ADR Required |
| Presentation   | Blazored.LocalStorage                   | Browser Storage              | Optional | Pending Version | ADR Required |
| Presentation   | Blazored.Toast                          | Toast Notifications          | Optional | Pending Version | ADR Required |
| Testing        | xUnit                                   | Unit Testing                 | ✔        | Pending Version | —            |
| Testing        | FluentAssertions                        | Assertions                   | ✔        | Pending Version | —            |
| Testing        | NSubstitute                             | Mocking                      | ✔        | Pending Version | —            |
| Testing        | coverlet.collector                      | Code Coverage                | ✔        | Pending Version | —            |
