# ADR-0005 — Use Serilog as the Primary Logging Framework

**Status:** Accepted

**Date:** 2026-07-18

**Decision Makers**

- Solution Architect
- Development Team

---

# Context

MachineryManagerEnterprise requires an enterprise-grade logging solution.

Logging is considered part of the system observability strategy and must support:

- Structured Logging
- Diagnostics
- Distributed Systems
- Correlation IDs
- Request Tracking
- Exception Analysis
- Future Cloud Deployment
- Future OpenTelemetry Integration

Logging is not limited to exception recording.

---

# Problem

The project requires a logging solution capable of:

- Structured log events
- Multiple output targets
- High performance
- Dependency Injection integration
- ASP.NET Core integration
- Future scalability

---

# Considered Options

## Option 1

Microsoft.Extensions.Logging

### Advantages

- Built into .NET
- Simple
- Native abstraction

### Disadvantages

- Not a complete logging solution
- Limited structured logging
- Requires external providers

---

## Option 2

Serilog

### Advantages

- Structured logging
- Mature ecosystem
- Rich sink collection
- Excellent diagnostics
- Enterprise adoption
- OpenTelemetry ready
- Seq integration
- SQL integration

### Disadvantages

- Additional dependency
- Requires initial configuration

---

## Option 3

NLog

### Advantages

- Mature
- Good performance

### Disadvantages

- Smaller ecosystem
- Less active community

---

## Option 4

log4net

### Advantages

- Stable

### Disadvantages

- Aging ecosystem
- Weak structured logging
- Limited future roadmap

---

# Decision

The project adopts **Serilog** as the primary logging implementation.

Microsoft.Extensions.Logging remains the logging abstraction used by ASP.NET Core.

Serilog becomes the logging provider.

---

# Architectural Rules

## Logging Abstraction

Application and Domain layers shall never reference Serilog directly.

Only the Infrastructure layer configures Serilog.

---

## Domain

Domain shall never perform logging.

Business rules must remain free from logging concerns.

---

## Application

Application may depend only on:

```
ILogger<T>
```

through Microsoft.Extensions.Logging abstractions.

---

## Infrastructure

Infrastructure owns:

- Logger configuration
- Sinks
- Enrichers
- Filters

---

## Structured Logging

All logs shall be structured.

The following style is prohibited:

```csharp
logger.LogInformation("Machine " + machineId + " created");
```

Preferred style:

```csharp
logger.LogInformation(
    "Machine {MachineId} created",
    machineId);
```

---

## Correlation

Every request shall contain a CorrelationId.

Every log entry should include:

- CorrelationId
- RequestId
- UserId (when available)

---

## Future Targets

The architecture shall support adding:

- Seq
- SQL Server
- Elasticsearch
- Azure Monitor
- OpenTelemetry

without changing application code.

---

# Consequences

## Positive

- Enterprise-grade diagnostics
- Structured logging
- Rich ecosystem
- Long-term maintainability
- Excellent tooling

---

## Negative

- Additional dependency
- Initial configuration effort

---

# Constraints

Application code shall never create or configure loggers.

Logger configuration belongs exclusively to Infrastructure.

---

# Related Documents

- TE-0002 — Logging Framework Selection
- ADR-0002 — Use FluentValidation
- ADR-0003 — Use MediatR
- ADR-0004 — Use Entity Framework Core

---

# References

- Serilog Documentation
- Microsoft Logging Documentation
- OpenTelemetry Specification