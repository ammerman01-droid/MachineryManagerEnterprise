# Architecture Decision Record

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0010 |
| **Version** | 3.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Title

Use OpenTelemetry

---

# Status

Accepted

---

# Context

Modern enterprise applications require comprehensive observability beyond
traditional logging.

The MachineryManagerEnterprise solution must support:

- Distributed tracing
- Metrics
- Correlation
- Performance diagnostics
- Request tracking
- Vendor-neutral telemetry
- Future cloud-native deployment

The selected observability framework must remain independent from monitoring
vendors while integrating naturally with the .NET ecosystem.

---

# Decision

The Infrastructure Layer shall adopt **OpenTelemetry** as the standard
observability framework.

Telemetry collection shall be implemented using OpenTelemetry standards.

OpenTelemetry shall provide tracing and metrics independently of the selected
monitoring backend.

---

# Decision Drivers

- Vendor Neutrality
- CNCF Standard
- Open Source
- Distributed Tracing
- Metrics
- Future-proof Architecture
- Performance
- Ecosystem Support

---

# Alternatives Considered

## Azure Application Insights SDK

Rejected because it introduces vendor lock-in.

---

## Elastic APM

Rejected because OpenTelemetry can export directly to Elastic while remaining
vendor neutral.

---

## Jaeger Native SDK

Rejected because OpenTelemetry already supports Jaeger through standard
exporters.

---

# Consequences

## Positive

- Vendor-independent observability
- Distributed tracing
- Metrics support
- Future compatibility
- Cloud-native architecture
- Easy backend replacement

## Negative

- Requires understanding of observability concepts.
- Incorrect sampling configuration may increase telemetry volume.

---

# Architecture Impact

OpenTelemetry shall exist only inside the **Infrastructure Layer**.

Application, Domain, and Presentation shall never reference OpenTelemetry
packages directly.

Instrumentation shall occur through framework integrations whenever possible.

---

# Implementation Notes

Telemetry exporters shall remain configurable.

Sampling shall be enabled to reduce unnecessary telemetry.

Correlation identifiers shall propagate across requests.

Sensitive information shall never be exported.

---

# Compliance Rules

1. OpenTelemetry shall only exist inside Infrastructure.

2. Domain shall never reference OpenTelemetry.

3. Application shall never reference OpenTelemetry.

4. Presentation shall never configure telemetry.

5. Telemetry shall remain vendor neutral.

6. Sensitive information shall never be exported.

7. Exporters shall be configurable without code changes.

---

# Related Technology Evaluation

TE-0008 — OpenTelemetry

---

# Related Proof of Concept

Not Required

---

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0009 — Use Serilog
- Dependency Catalog

---

# References

https://opentelemetry.io/

https://github.com/open-telemetry

https://learn.microsoft.com/dotnet/core/diagnostics/observability-with-otel

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