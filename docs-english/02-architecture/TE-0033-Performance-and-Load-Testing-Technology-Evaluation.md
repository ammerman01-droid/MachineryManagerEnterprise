| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | TE-0033            |
| **Title**        | Performance and Load Testing Technology Evaluation |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-28         |
| **Last Updated** | 2026-08-08         |

# Purpose

This document evaluates candidate technologies for Performance and Load Testing Technology Evaluation in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

The selected technologies shall support continuous performance validation throughout the software lifecycle.

---

# Evaluation Scope

This Technology Evaluation evaluates:

- BenchmarkDotNet
- NBomber
- k6

This document does **not** define:

- Performance SLA
- Production Monitoring
- Observability Platform
- Capacity Planning Policy
- Auto-Scaling Strategy

These architectural decisions are documented separately within the corresponding ADR.

---

# Relationship with Related ADRs

This Technology Evaluation supports:

- **ADR-0027 — Enterprise Performance Testing Strategy** *(Pending)*

It depends upon:

- ADR-0001 — Clean Architecture
- ADR-0024 — Enterprise Testing Strategy
- ADR-0025 — Build & Deployment Architecture
- ADR-0026 — Enterprise Security Strategy

---

# Architectural References

This evaluation is based upon:

- Microsoft Performance Guidelines
- BenchmarkDotNet Documentation
- NBomber Documentation
- Grafana k6 Documentation
- Enterprise Performance Engineering Best Practices

---

# Scope

The following technologies are evaluated:

- BenchmarkDotNet
- NBomber
- k6

---

# Performance Testing Objectives

MachineryManagerEnterprise shall support validation of:

- Algorithm Performance
- Memory Usage
- CPU Consumption
- Database Throughput
- Messaging Throughput
- Concurrent Users
- Long Running Stability
- Scalability

---

# Functional Requirements

The selected technologies shall support:

- Micro Benchmarking
- Load Testing
- Stress Testing
- Endurance Testing
- Throughput Measurement
- Latency Measurement
- Performance Regression Detection
- CI/CD Integration

---

# Non-Functional Requirements

The performance testing platform shall provide:

- Repeatability
- Deterministic Results
- Automation
- Cross Platform Support
- Enterprise Scalability
- Excellent Reporting
- Low Operational Complexity

---

# Candidate Technologies

| Candidate | Category |
|-----------|----------|
| BenchmarkDotNet | Micro Benchmark Framework |
| NBomber | Load Testing Framework |
| k6 | HTTP/API Load Testing |

---

# Evaluation Criteria

| ID | Criterion | Priority |
|----|-----------|----------|
| PF-01 | Enterprise Readiness | Critical |
| PF-02 | Automation | Critical |
| PF-03 | CI/CD Compatibility | High |
| PF-04 | Cross Platform | High |
| PF-05 | Reporting | High |
| PF-06 | Scalability | High |
| PF-07 | Operational Simplicity | Medium |
| PF-08 | Documentation | Medium |
| PF-09 | Performance Accuracy | Critical |
| PF-10 | Long-Term Viability | High |

---

# Architecture Principle

The evaluated component acts as an isolated infrastructure service, adhering strictly to Clean Architecture layer dependencies and domain isolation rules.

---

# 8. BenchmarkDotNet Evaluation

## Overview

BenchmarkDotNet is the official benchmarking framework for the .NET ecosystem.

It is specifically designed to measure the performance characteristics of small pieces of code with scientific rigor.

Unlike ordinary timing measurements, BenchmarkDotNet automatically controls for:

- JIT compilation
- CPU warmup
- Garbage Collection
- Multiple iterations
- Statistical analysis

Within MachineryManagerEnterprise, BenchmarkDotNet is evaluated as the primary micro-benchmarking framework.

---

# Architectural Role

```text
Application Component

          │

          ▼

BenchmarkDotNet

 ┌────────────────────────────┐

 │ Warmup                     │
 │ Multiple Iterations         │
 │ GC Analysis                 │
 │ Memory Analysis             │
 │ Statistical Evaluation      │

 └────────────────────────────┘

          │

          ▼

 Performance Report
```

BenchmarkDotNet evaluates individual algorithms rather than complete system behavior.

---

# Architectural Strengths

Advantages include:

- Official .NET benchmarking framework
- Statistical accuracy
- Memory diagnostics
- CPU diagnostics
- Repeatable execution
- Excellent reporting
- Continuous performance regression detection
- Native .NET integration

---

# Functional Capabilities

BenchmarkDotNet supports:

- Execution Time Measurement
- Memory Allocation Analysis
- Garbage Collection Metrics
- CPU Performance Analysis
- Parameterized Benchmarks
- Baseline Comparison
- Export Formats
- Statistical Reporting

---

# Typical Benchmark Workflow

```text
Benchmark Method

      │

Warmup

      │

Measurement

      │

Statistical Analysis

      │

Benchmark Report
```

The framework automatically performs warmup and repeated execution to improve measurement accuracy.

---

# Statistical Accuracy

BenchmarkDotNet automatically computes:

- Mean
- Median
- Standard Deviation
- Error
- Confidence Interval

This significantly improves the reliability of performance measurements compared with manual timing.

---

# Memory Diagnostics

The framework reports:

- Allocated Memory
- Garbage Collections
- Allocation Rate

This allows developers to identify unnecessary allocations and optimize memory usage.

---

# Reporting

Supported output formats include:

- Markdown
- HTML
- CSV
- JSON

Reports can be archived as CI/CD artifacts.

---

# CI/CD Compatibility

BenchmarkDotNet integrates well with:

- GitHub Actions
- Azure DevOps
- .NET CLI

Performance benchmarks can be executed automatically as part of continuous validation.

---

# Cross Platform Support

Supported operating systems:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

---

# Enterprise Suitability

Appropriate for benchmarking:

- Algorithms
- Domain Services
- Repository Performance
- Serialization
- Caching
- Parsing
- Mathematical Computations

It is **not** intended for system-wide load testing.

---

# Performance Impact

Benchmark execution is intentionally slower because statistical accuracy is prioritized over execution speed.

This is appropriate because benchmarks execute outside production workloads.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| Performance Accuracy | Excellent |
| Automation | Excellent |
| Cross Platform | Excellent |
| CI/CD Compatibility | Excellent |
| Reporting | Excellent |
| Documentation | Excellent |
| Long-Term Viability | Excellent |

---

# Advantages

- Industry-standard .NET benchmarking framework
- Scientifically accurate measurements
- Rich diagnostics
- Excellent reporting
- Strong Microsoft ecosystem integration

---

# Disadvantages

- Not suitable for load testing
- Benchmark execution time can be relatively long

These limitations are expected because the framework targets micro-performance analysis.

---

# Preliminary Conclusion

BenchmarkDotNet fully satisfies the micro-performance benchmarking requirements of MachineryManagerEnterprise.

It is approved as the standard framework for performance benchmarking and regression detection.

---

# 9. NBomber Evaluation

## Overview

NBomber is an open-source load testing framework designed specifically for .NET applications.

Unlike BenchmarkDotNet, which measures the performance of individual methods, NBomber evaluates the behavior of complete systems under concurrent workloads.

Within MachineryManagerEnterprise, NBomber is evaluated as the primary enterprise load testing framework.

---

# Architectural Role

```text
Concurrent Users

        │

        ▼

     NBomber

 ┌────────────────────────────┐

 │ Virtual Users              │
 │ Scenarios                  │
 │ Throughput                 │
 │ Latency                    │
 │ Error Rate                 │

 └────────────────────────────┘

        │

        ▼

 Performance Report
```

NBomber evaluates complete application behavior under realistic concurrent workloads.

---

# Architectural Strengths

Advantages include:

- Native .NET implementation
- High concurrency support
- Scenario-based testing
- Real-time metrics
- Cluster execution
- Excellent reporting
- Easy automation
- Strong .NET ecosystem integration

---

# Functional Capabilities

NBomber supports:

- Load Testing
- Stress Testing
- Spike Testing
- Endurance Testing
- Concurrent User Simulation
- Throughput Measurement
- Latency Measurement
- Failure Rate Measurement

---

# Scenario-Based Testing

NBomber models realistic business workflows.

Example:

```text
Login

   │

Search

   │

Open Machinery

   │

Create Work Order

   │

Logout
```

Entire business scenarios can be executed simultaneously by hundreds or thousands of virtual users.

---

# Metrics

NBomber measures:

- Requests per Second (RPS)
- Response Time
- Average Latency
- Percentiles (P50 / P95 / P99)
- Error Rate
- Throughput
- Active Users

---

# Distributed Execution

NBomber supports:

- Single Machine Execution
- Multi-Node Cluster Execution

This enables enterprise-scale load testing.

---

# Reporting

Supported report formats include:

- HTML
- Markdown
- JSON
- CSV

Results can be archived automatically by CI/CD pipelines.

---

# CI/CD Compatibility

NBomber integrates with:

- GitHub Actions
- Azure DevOps
- .NET CLI
- Docker

Load tests can execute automatically after successful builds.

---

# Cross Platform Support

Supported operating systems:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

---

# Enterprise Suitability

NBomber is appropriate for validating:

- API scalability
- Database throughput
- Messaging performance
- Background processing
- Repository performance
- Distributed workflows

---

# Performance Characteristics

NBomber is capable of generating high concurrent workloads while maintaining accurate timing statistics.

Performance is considered **Excellent**.

---

# Operational Characteristics

Advantages include:

- Strongly typed scenarios
- Native C# implementation
- Easy integration with existing test projects
- Minimal operational overhead

Operational simplicity is considered **Excellent**.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| Load Testing Capability | Excellent |
| Scalability | Excellent |
| Automation | Excellent |
| CI/CD Compatibility | Excellent |
| Reporting | Excellent |
| Cross Platform | Excellent |
| Long-Term Viability | Excellent |

---

# Advantages

- Native .NET framework
- Excellent concurrency support
- Scenario-driven testing
- Enterprise scalability
- Excellent reporting

---

# Disadvantages

- Smaller ecosystem than k6
- Focused primarily on .NET workloads

These limitations are acceptable because MachineryManagerEnterprise is entirely built on the .NET platform.

---

# Preliminary Conclusion

NBomber fully satisfies the enterprise load testing requirements of MachineryManagerEnterprise.

It is approved as the primary framework for load, stress and endurance testing.

---

# 10. k6 Evaluation

## Overview

k6 is a modern open-source load testing platform focused on HTTP-based workloads and API performance validation.

Unlike BenchmarkDotNet, which measures internal algorithm performance, and unlike NBomber, which is tightly integrated with .NET, k6 focuses on external system behavior through protocol-level testing.

Within MachineryManagerEnterprise, k6 is evaluated as an API and infrastructure load-testing solution.

---

# Architectural Role

```text
Virtual Users

      │

      ▼

      k6

 ┌──────────────────────────┐

 │ HTTP Requests            │
 │ API Scenarios            │
 │ Load Generation          │
 │ Metrics Collection       │

 └──────────────────────────┘

      │

      ▼

 REST APIs / Gateways
```

k6 evaluates externally observable system performance through HTTP traffic.

---

# Architectural Strengths

Advantages include:

- Industry-standard load testing platform
- HTTP-first design
- High scalability
- JavaScript-based scripting
- Cloud execution support
- Excellent dashboards
- Strong Grafana ecosystem integration
- CI/CD friendly

---

# Functional Capabilities

k6 supports:

- Load Testing
- Stress Testing
- Spike Testing
- Soak Testing
- API Performance Testing
- Throughput Measurement
- Latency Measurement
- SLA Validation

---

# API Testing Model

Typical execution flow:

```text
Virtual Users

      │

HTTP Requests

      │

Application API

      │

Performance Metrics
```

The platform validates externally observable behavior rather than internal implementation details.

---

# Metrics

k6 measures:

- Requests per Second
- Response Time
- Latency
- Error Rate
- Percentiles
- Throughput
- Active Virtual Users

---

# Scalability

k6 is capable of generating:

- Thousands of concurrent users
- Millions of HTTP requests
- Distributed execution
- Cloud-based execution

Scalability is considered **Excellent**.

---

# Reporting

Supported outputs include:

- JSON
- CSV
- HTML
- Grafana Cloud
- Prometheus
- InfluxDB

Rich visualization is available through the Grafana ecosystem.

---

# CI/CD Compatibility

k6 integrates directly with:

- GitHub Actions
- Azure DevOps
- Docker
- Kubernetes
- Grafana Cloud

Automated execution is straightforward.

---

# Cross Platform Support

Supported operating systems:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

---

# Enterprise Suitability

k6 is appropriate for validating:

- REST APIs
- Public APIs
- HTTP Gateways
- Reverse Proxies
- Web Services
- Infrastructure Capacity

---

# Performance Characteristics

The engine is optimized for large-scale protocol-level load generation.

Performance is considered **Excellent**.

---

# Operational Characteristics

Advantages include:

- Lightweight runtime
- Simple scripting model
- Excellent documentation
- Strong automation support

Operational simplicity is considered **Excellent**.

---

# Limitations

k6 focuses on protocol-level testing.

It does **not** execute internal .NET business workflows directly and therefore complements rather than replaces BenchmarkDotNet or NBomber.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| API Load Testing | Excellent |
| Scalability | Excellent |
| Automation | Excellent |
| Reporting | Excellent |
| CI/CD Compatibility | Excellent |
| Cross Platform | Excellent |
| Long-Term Viability | Excellent |

---

# Advantages

- Industry standard
- Excellent scalability
- Strong Grafana integration
- Rich reporting
- Excellent cloud support

---

# Disadvantages

- Focused primarily on HTTP workloads
- Not intended for internal .NET component benchmarking

---

# Preliminary Conclusion

k6 fully satisfies the API load testing requirements of MachineryManagerEnterprise.

It is approved as the preferred platform for external API performance validation.

---

# 11. Overall Technology Comparison

## Technology Stack Overview

| Responsibility | Approved Technology |
|---------------|---------------------|
| Micro Benchmarking | BenchmarkDotNet |
| Enterprise Load Testing | NBomber |
| API Load Testing | k6 |

These technologies complement each other rather than competing.

---

# Technology Comparison Matrix

| Criterion | BenchmarkDotNet | NBomber | k6 |
|-----------|:---------------:|:--------:|:--:|
| Enterprise Readiness | Excellent | Excellent | Excellent |
| Micro Benchmarking | Excellent | Poor | Very Poor |
| Load Testing | Very Poor | Excellent | Excellent |
| API Testing | Very Poor | Good | Excellent |
| CI/CD Integration | Excellent | Excellent | Excellent |
| Reporting | Excellent | Excellent | Excellent |
| Cross Platform | Excellent | Excellent | Excellent |
| Scalability | Poor | Excellent | Excellent |

---

# Responsibility Separation

```text
Algorithms

     │

BenchmarkDotNet

     │

Application Workloads

     │

NBomber

     │

HTTP / REST APIs

     │

k6
```

Each technology addresses a distinct performance-testing concern.

---

# Enterprise Coverage

| Capability | Coverage |
|------------|----------|
| Algorithm Performance | Complete |
| Load Testing | Complete |
| Stress Testing | Complete |
| Endurance Testing | Complete |
| API Performance | Complete |
| CI Automation | Complete |

No overlap results in unnecessary duplication.

---

# Architectural Assessment

The combined performance testing platform provides complete coverage across all required performance validation layers.

No additional performance testing technologies are required.

---

# 12. Final Recommendation

Following the evaluation of all candidate technologies, the Architecture Review Board recommends adoption of the following enterprise performance testing stack.

| Category | Approved Technology |
|----------|---------------------|
| Micro Benchmarking | **BenchmarkDotNet** |
| Enterprise Load Testing | **NBomber** |
| API Load Testing | **k6** |

The technologies complement one another and collectively provide complete performance validation across all architectural layers.

---

# Recommended Testing Strategy

## BenchmarkDotNet

Primary responsibility:

- Algorithm Benchmarking
- Memory Allocation Analysis
- CPU Performance Measurement
- Performance Regression Detection

Use BenchmarkDotNet whenever code-level performance characteristics must be measured.

---

## NBomber

Primary responsibility:

- Business Workflow Load Testing
- Concurrent User Simulation
- Repository Performance
- Messaging Throughput
- Endurance Testing

NBomber becomes the primary enterprise load testing framework.

---

## k6

Primary responsibility:

- REST API Performance
- HTTP Endpoint Validation
- Infrastructure Capacity
- Gateway Performance
- Public Interface Load Testing

k6 validates externally observable system behaviour.

---

# Combined Testing Architecture

```text
                Performance Testing

                       │

      ┌────────────────┼────────────────┐

      ▼                ▼                ▼

BenchmarkDotNet     NBomber            k6

      │                │                │

 Algorithms      Application      REST APIs

                 Workflows
```

Each technology owns one clearly defined performance testing responsibility.

---

# CI/CD Integration

The approved pipeline shall execute:

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

Performance validation therefore becomes an automated part of the delivery pipeline.

---

# Benefits

The selected stack provides:

- Accurate micro benchmarks
- Enterprise load validation
- API scalability validation
- Continuous performance regression detection
- Cross-platform execution
- Excellent automation support

---

# Long-Term Maintainability

The selected technologies are:

- actively maintained;
- widely adopted;
- enterprise proven;
- fully compatible with .NET 10.

No foreseeable migration risks have been identified.

---

# Overall Technology Comparison

The selected technology provides optimal performance, maintainability, and Clean Architecture compatibility.

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative |
|-----------------|------------------------|-------------|
| System Capability | Primary Selected | Evaluated Option |

---

# Final Recommendation Statement

The Architecture Review Board unanimously recommends adoption of:

- **BenchmarkDotNet**
- **NBomber**
- **k6**

as the official performance testing platform for MachineryManagerEnterprise.

---

# 13. Final Decision

## Approved Architecture

```text
Performance Validation

        │

 ┌──────┼──────────────┐

 ▼      ▼              ▼

BenchmarkDotNet   NBomber   k6

 ▼                ▼         ▼

Code          Application   API

Performance    Scalability  Scalability
```

---

## Technology Decisions

| Technology | Decision | Status |
|------------|----------|--------|
| BenchmarkDotNet | Approved | ✅ |
| NBomber | Approved | ✅ |
| k6 | Approved | ✅ |

---

## Implementation Strategy

### Phase 1

- BenchmarkDotNet
- Performance regression benchmarks

### Phase 2

- NBomber
- Business workflow load testing

### Phase 3

- k6
- API scalability validation

---

## Consequences

### Positive

- Complete performance validation
- Excellent automation
- Enterprise scalability
- Clear separation of responsibilities
- Long-term maintainability

### Negative

- Three specialized tools instead of one
- Additional CI execution time
- Separate reporting artifacts

These trade-offs are acceptable because each tool specializes in a distinct performance validation domain.

---

## Related Architecture Decision

Implementation of this Technology Evaluation requires:

- **ADR-0027 — Enterprise Performance Testing Strategy**

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---


# Related Documents

- ../05-development/01-SolutionStructure.md
- ../05-development/04-DependencyRules.md
- ../05-development/05-CodingStandards.md
---

# 14. Revision History

| Version | Date       | Author             | Description                                    |
|---------|------------|--------------------|------------------------------------------------|
| 1.0.0   | 2026-07-28 | Solution Architect | Initial technology evaluation for Testing      |
| 1.1.0   | 2026-07-28 | Solution Architect | Converted star-rating (⭐) tables to text ratings (Excellent/Good/Fair/Poor/Very Poor) for consistency with the rest of the documentation |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0      |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes |