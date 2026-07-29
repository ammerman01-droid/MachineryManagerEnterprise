| Property | Value |
|----------|-------|
| **Document ID** | TE-0008 |
| **Title** | OpenTelemetry Standards Evaluation |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-26 |
| **Last Updated** | 2026-07-28 |

---

# Purpose

This document evaluates candidate technologies for OpenTelemetry Standards Evaluation in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

---

# Evaluation Scope

Evaluates open standards for telemetry. Full observability pipeline is detailed in TE-0017.

---

# Relationship with Previous Technology Evaluations

Baseline standard for TE-0017 (Observability and Telemetry).

---

# Architectural References

- ADR-0001 — Clean Architecture
- TE-0017 — Observability and Telemetry Evaluation

---

# Scope

Evaluates OpenTelemetry standard vs proprietary APM agents (Datadog, AppInsights SDK).

---

# Functional Requirements

Unified collection of distributed traces, metrics, and logs across .NET services.

---

# Non-Functional Requirements

Vendor-neutral exporter protocol (OTLP), low performance overhead, cloud neutrality.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| OpenTelemetry (.NET) | Telemetry Collection Standard | Selected |
| Proprietary APM SDKs | Vendor Lock-in Agents | Rejected |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Cloud Neutrality & Open Standard | Critical |
| A2 | .NET 10 Activity/Meter Support | High |

---

# Architecture Principle

Instrumentation uses standard System.Diagnostics APIs natively built into .NET 10.

---

# 5. Candidate Deep-Dive Evaluations

## OpenTelemetry Evaluation

### Overview
OpenTelemetry (OTel) is a CNCF vendor-neutral observability framework.

### Architectural Strengths
- Eliminates vendor lock-in; allows swapping telemetry backends (Grafana Tempo, Prometheus, Datadog) without code changes.

---

# Overall Technology Comparison

OpenTelemetry is the industry standard for modern cloud-native observability.

---

# Final Recommendation

Adopt OpenTelemetry for all distributed tracing and metric collection.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| OpenTelemetry | Approved |

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Cloud Neutrality

---

# Related ADR

- ADR-0001 — Clean Architecture

---

# Related Documents

- TE-0017 — Observability and Telemetry Evaluation

---

# References

- https://opentelemetry.io/

---

# Revision History

| Version | Date       | Author             | Description        |
|---------|------------|--------------------|--------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial evaluation |
| 2.0.0   | 2026-07-18 | Solution Architect | Standardized |
| 3.0.0   | 2026-07-18 | Solution Architect | Rewritten according to Technology Evaluation Template |
| 3.1.0   | 2026-07-28 | Solution Architect | New section added (Evaluation Scope) |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0 |