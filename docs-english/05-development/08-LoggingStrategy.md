| Property | Value |
|----------|-------|
| **Document ID** | DOC-DEV-009 |
| **Title** | Logging Strategy |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# Purpose

This document defines the official logging strategy for the
**MachineryManagerEnterprise** solution.

A consistent logging strategy improves:

- Monitoring
- Diagnostics
- Production Support
- Incident Investigation
- Security Auditing

Logging shall provide operational visibility without exposing sensitive information.

---

# Objectives

The logging system shall:

- Produce structured logs.
- Support centralized log aggregation.
- Minimize noise.
- Preserve diagnostic context.
- Protect confidential information.

---

# Logging Principles

Logs should answer three questions:

1. What happened?
2. When did it happen?
3. Why did it happen?

Every log entry should provide useful operational information.

---

# Structured Logging

The solution shall use structured logging.

Example

```csharp
logger.LogInformation(
    "Machine {MachineId} created by {UserId}",
    machineId,
    userId);
```

Avoid string concatenation.

Incorrect

```csharp
logger.LogInformation(
    "Machine " + machineId + " created");
```

---

# Log Levels

| Level | Usage |
|---------|--------------------------------|
| Trace | Detailed execution flow |
| Debug | Development diagnostics |
| Information | Normal business operations |
| Warning | Recoverable abnormal conditions |
| Error | Operation failed |
| Critical | Application instability |

---

# Information Logs

Information logs should record:

- Successful business operations
- Important lifecycle events
- User actions
- Background jobs

Information logs should not become excessively verbose.

---

# Warning Logs

Warnings indicate abnormal but recoverable situations.

Examples

- Retry performed
- Missing optional configuration
- Slow response
- Business constraint approaching limits

---

# Error Logs

Errors indicate failed operations.

Each error log should include:

- Exception
- Correlation Id
- Operation
- Context

---

# Critical Logs

Critical logs indicate that the application may become unavailable.

Examples

- Database unavailable
- Startup failure
- Data corruption
- Unhandled fatal exception

---

# Sensitive Information

Never log:

- Passwords
- Access Tokens
- Refresh Tokens
- Secrets
- Connection Strings
- Personal Identification Numbers
- Payment Information

---

# Correlation ID

Every request should have a Correlation Id.

The Correlation Id shall appear in every related log entry.

This enables end-to-end request tracing.

---

# Exception Logging

Unexpected exceptions should always include:

- Exception type
- Message
- Stack trace
- Inner exception
- Correlation Id

---

# Performance Logging

Long-running operations should record execution duration.

Example

```text
GenerateMonthlyReport completed in 2143 ms.
```

---

# Business Logging

Business events may be logged independently from technical logs.

Examples

- Machine Registered
- Maintenance Scheduled
- User Logged In

These events support auditing and reporting.

---

# Audit Logging

Security-sensitive actions should always be logged.

Examples

- Authentication
- Authorization failure
- Role changes
- Configuration changes
- User management

Audit logs should never be deleted manually.

---

# Log Retention

Retention policies should be configurable.

Suggested defaults

| Log Type | Retention |
|-----------|-----------|
| Trace | Short |
| Debug | Short |
| Information | Medium |
| Warning | Medium |
| Error | Long |
| Audit | Long |

---

# Log Destinations

The logging infrastructure should support multiple targets.

Examples

- Console
- File
- Seq
- OpenTelemetry
- Elasticsearch
- Azure Monitor

The logging abstraction shall remain independent from any specific provider.

---

# Open Source Policy

Only approved open-source logging providers may be used.

Technology selection requires:

- Technology Evaluation (TE)
- Approved ADR

---

# Compliance

Every project shall follow this logging strategy.

Architectural deviations require an approved ADR.

---

# Related Documents

- DOC-CONVENTIONS
- DOC-DEV-001 (Development Principles)
- DOC-DEV-006 (Coding Standards)
- DOC-DEV-008 (Error Handling)
- DOC-DEV-010 (Testing Strategy)
- ADR-0002

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial logging strategy                              |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |