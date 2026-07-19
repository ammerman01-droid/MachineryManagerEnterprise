# Technology Evaluation — Mapster

| Property | Value |
|----------|-------|
| **Document ID** | TE-0006 |
| **Version** | 3.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Executive Summary

This document evaluates **Mapster** as the object mapping library for the
MachineryManagerEnterprise solution.

Mapster was selected because it provides high performance, minimal runtime
overhead, excellent compile-time capabilities, and clean integration with
modern .NET applications.

---

# Problem Statement

The solution requires an object mapping framework that:

- Reduces repetitive mapping code
- Improves maintainability
- Supports compile-time mapping
- Delivers high performance
- Integrates naturally with .NET 10

---

# Evaluation Scope

Object Mapping

DTO Mapping

Entity Mapping

Projection Support

---

# Candidate Technologies

| Technology | Status |
|------------|--------|
| Mapster | Selected |
| AutoMapper | Evaluated |
| Manual Mapping | Evaluated |

---

# Evaluation Criteria

The evaluation considered:

- Open Source
- Performance
- Simplicity
- Configuration
- Compile-Time Support
- Testability
- Community
- Documentation

---

# Comparison Matrix

| Criteria | Mapster | AutoMapper | Manual Mapping |
|----------|----------|------------|----------------|
| Performance | Excellent | Good | Excellent |
| Compile-Time Generation | Excellent | Limited | Excellent |
| Configuration | Simple | Moderate | None |
| Maintainability | Excellent | Good | Poor (large projects) |
| Learning Curve | Low | Moderate | None |
| Community | Good | Excellent | N/A |

---

# Advantages

- Excellent runtime performance
- Supports source generation
- Minimal configuration
- Simple API
- Strong typing
- Low memory allocation
- Easy integration with ASP.NET Core

---

# Disadvantages

- Smaller community than AutoMapper.
- Fewer third-party learning resources.

---

# Risks

Potential risks include:

- Smaller ecosystem
- Fewer StackOverflow examples

These risks are considered acceptable.

---

# Performance Considerations

Mapster is significantly faster than reflection-based mapping libraries.

Source generation further improves:

- Startup time
- Runtime performance
- Memory usage

---

# Security Considerations

Mapster performs in-memory object mapping.

No significant security concerns have been identified.

---

# Licensing

License

MIT License

Commercial usage is permitted.

---

# Community & Ecosystem

Mapster has:

- Active GitHub repository
- Good documentation
- Regular releases
- Growing adoption within modern .NET applications

---

# Proof of Concept

No dedicated Proof of Concept was required.

The framework is mature and fully compatible with .NET 10.

---

# Architecture Impact

Mapster shall only be used inside the **Application Layer**.

Domain entities shall never depend on Mapster.

Mapping configuration should remain centralized.

---

# Migration Complexity

**Difficulty:** Low

Mapping libraries can be replaced with moderate effort provided that mapping
configuration remains isolated.

---

# Alternatives Considered

## AutoMapper

Very mature and widely adopted.

Rejected because:

- More runtime reflection
- Higher configuration complexity
- Lower performance

---

## Manual Mapping

Provides maximum control.

Rejected because:

- Large amount of repetitive code
- Higher maintenance cost
- Increased risk of mapping inconsistencies

---

# Decision

Approved

---

# Decision Rationale

Mapster provides the best balance of:

- Performance
- Simplicity
- Maintainability
- Modern .NET compatibility

It aligns with the architectural goals of the project.

---

# Related ADR

- ADR-0002 — Open Source First Policy
- ADR-0009 — Use Mapster

---

# Related Documents

- TE-0001 — .NET 10
- Dependency Catalog
- Coding Standards

---

# References

https://github.com/MapsterMapper/Mapster

https://github.com/MapsterMapper/Mapster/wiki

https://www.nuget.org/packages/Mapster

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial evaluation |
| 2.0.0 | 2026-07-18 | Standardized |
| 3.0.0 | 2026-07-18 | Rewritten according to Technology Evaluation Template |