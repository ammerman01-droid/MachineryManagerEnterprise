# Architecture Decision Record

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0009 |
| **Version** | 3.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Title

Use Serilog

---

# Status

Accepted

---

# Context

The MachineryManagerEnterprise solution requires a structured logging framework
capable of supporting enterprise diagnostics, monitoring, troubleshooting, and
future observability requirements.

The selected logging framework should:

- Support structured logging
- Integrate with ASP.NET Core
- Support multiple log sinks
- Deliver high performance
- Be vendor neutral
- Be fully open source
- Integrate with OpenTelemetry

---

# Decision

The Infrastructure Layer shall use **Serilog** as the primary logging framework.

All application logging shall be performed through the Microsoft.Extensions.Logging
abstraction while Serilog acts as the logging provider.

---

# Decision Drivers

- Structured logging
- Open Source
- Excellent .NET integration
- Performance
- Rich ecosystem
- Vendor neutrality
- Extensibility
- OpenTelemetry compatibility

---

# Alternatives Considered

## Microsoft Default Logger

Rejected because it lacks the structured logging capabilities and sink ecosystem
required for enterprise applications.

---

## NLog

Rejected because Serilog provides stronger structured logging support and a
larger ecosystem for modern .NET applications.

---

## log4net

Rejected because it represents an older logging model and does not align well
with modern structured logging practices.

---

# Consequences

## Positive

- Structured log events
- Multiple output sinks
- Excellent diagnostics
- Easy integration with monitoring platforms
- Consistent logging strategy
- Future OpenTelemetry compatibility

## Negative

- Additional package dependencies
- Requires careful configuration of sinks and enrichers

---

# Architecture Impact

Serilog shall exist only inside the **Infrastructure Layer**.

Application, Domain, and Presentation shall never reference Serilog directly.

Logging requests shall be made only through the Microsoft logging abstraction.

---

# Implementation Notes

Serilog configuration shall be centralized.

Configuration shall be loaded from application configuration files.

Sensitive information shall never be written to log output.

Logging enrichers should provide:

- Correlation Id
- Machine Name
- Environment
- Thread Id
- Request Id

where appropriate.

---

# Compliance Rules

1. Serilog shall only exist inside Infrastructure.

2. Domain shall never reference Serilog.

3. Application shall never reference Serilog.

4. Presentation shall never configure logging.

5. Logging shall occur through ILogger<T>.

6. Sensitive information shall never be logged.

7. Logging configuration shall remain centralized.

---

# Related Technology Evaluation

TE-0007 — Serilog

---

# Related Proof of Concept

Not Required

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0010 — Use OpenTelemetry
- Dependency Catalog

---

# References

https://serilog.net/

https://github.com/serilog/serilog

https://github.com/serilog/serilog-aspnetcore

https://www.nuget.org/packages/Serilog

---

# Review

| Role | Name | Date |
|------|------|------|
| Solution Architect | | |

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial decision |
| 2.0.0 | 2026-07-18 | Standardized |
| 3.0.0 | 2026-07-18 | Rewritten according to ADR Template v3.0 |