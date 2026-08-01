| Property | Value |
|----------|-------|
| **Document ID** | ADR-0027 |
| **Title** | Enterprise Performance Testing Strategy |
| **Status** | Accepted |
| **Version** | 4.0.0 |
| **Decision Date** | 2026-07-28 |
| **Owner** | Solution Architect |
| **Related TE** | TE-0033 – Performance and Load Testing Technology Evaluation |

---

# Context

MachineryManagerEnterprise is an enterprise-grade distributed application that includes:

- Desktop Client
- Application Services
- SQL Server
- RabbitMQ
- Redis
- Qdrant
- Background Workers

Performance validation must therefore occur at multiple architectural layers.

No single performance-testing framework adequately covers:

- algorithm performance;
- application workload;
- HTTP/API scalability.

---

# Problem

Using a single testing technology would leave major performance risks untested.

Examples include:

- fast algorithms but slow APIs;
- scalable APIs but inefficient business logic;
- optimized code but poor concurrent behavior.

A layered performance testing strategy is therefore required.

---

# Decision Drivers

The architecture shall support:

- Micro Benchmarking
- Load Testing
- Stress Testing
- Endurance Testing
- API Scalability Testing
- Continuous Performance Regression Detection
- CI/CD Automation
- Long-Term Maintainability

---

# Decision

MachineryManagerEnterprise adopts three complementary performance-testing technologies.

| Responsibility | Technology |
|---------------|------------|
| Micro Benchmarking | BenchmarkDotNet |
| Enterprise Load Testing | NBomber |
| API Load Testing | k6 |

Each technology has an exclusive architectural responsibility.

---

# Enterprise Performance Architecture

```text
                Performance Validation

                         │

        ┌────────────────┼────────────────┐

        ▼                ▼                ▼

 BenchmarkDotNet      NBomber            k6

        │                │                │

 Algorithms     Business Workflows    REST APIs

```

---

# Benchmarking Strategy

BenchmarkDotNet shall be used for:

- algorithms;
- repositories;
- serialization;
- caching;
- parsing;
- computational performance.

BenchmarkDotNet shall **not** be used for system load testing.

---

# Enterprise Load Testing Strategy

NBomber shall validate:

- concurrent users;
- business workflows;
- messaging;
- database throughput;
- long-running execution;
- system scalability.

NBomber becomes the primary enterprise load-testing framework.

---

# API Load Testing Strategy

k6 shall validate:

- REST APIs;
- HTTP endpoints;
- API gateways;
- infrastructure scalability;
- externally observable latency.

k6 shall **not** be used for benchmarking internal .NET algorithms.

---

# Layer Responsibilities

| Layer | Technology |
|--------|------------|
| Code Performance | BenchmarkDotNet |
| Application Performance | NBomber |
| API Performance | k6 |

---

# Continuous Integration

Performance validation shall become part of CI.

```text
Build

   │

Unit Tests

   │

BenchmarkDotNet

   │

NBomber

   │

k6

   │

Publish Reports
```

---

# Performance Regression Policy

Every implementation introducing measurable computational work shall be benchmarked.

Performance regressions shall be treated similarly to functional regressions.

---

# Reporting

Performance reports shall be archived for every CI execution.

Supported report types include:

- Markdown
- HTML
- JSON
- CSV

Historical reports provide trend analysis across releases.

---

# Test Environments

Performance validation shall be executed in:

- Local Development
- Continuous Integration
- Dedicated Performance Environment

Production environments shall not be used for synthetic load testing.

---

# Performance Metrics

The following metrics shall be collected where applicable:

- Execution Time
- Throughput
- Response Time
- Latency
- Memory Allocation
- CPU Usage
- Error Rate
- Concurrent Users

---

# Benefits

The selected strategy provides:

- Complete architectural coverage
- Clear separation of responsibilities
- Automated regression detection
- Enterprise scalability validation
- Long-term maintainability

---

# Consequences

Positive

- Reliable performance validation
- Repeatable measurements
- CI integration
- Enterprise scalability assurance

Negative

- Multiple specialized tools
- Longer CI execution time
- Additional performance report management

These consequences are acceptable considering the architectural benefits.

---

# Alternatives Considered

## BenchmarkDotNet Only

Rejected.

Does not validate concurrent workloads.

---

## NBomber Only

Rejected.

Does not accurately benchmark internal algorithms.

---

## k6 Only

Rejected.

Focuses exclusively on externally visible HTTP behavior.

---

## Single Generic Load Testing Framework

Rejected.

No single framework adequately covers all required performance-validation layers.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Clean Architecture
- ADR-0024 — Enterprise Testing Strategy
- ADR-0025 — Build & Deployment Architecture
- TE-0033 — Performance and Load Testing Technology Evaluation

---

# Decision Outcome

**Accepted**

MachineryManagerEnterprise adopts a layered enterprise performance testing strategy consisting of:

- BenchmarkDotNet
- NBomber
- k6

Each technology is responsible for one distinct architectural layer of performance validation.

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-28 | Solution Architect | Initial version                                       |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |