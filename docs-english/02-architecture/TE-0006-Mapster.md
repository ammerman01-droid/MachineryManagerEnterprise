| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | TE-0006            |
| **Title**        | Mapster Object Mapping Technology Evaluation |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

This document evaluates candidate technologies for Mapster Object Mapping Technology Evaluation in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

---

# Evaluation Scope

Evaluates object mapping libraries. Detailed mapping strategy is covered in TE-0023.

---

# Relationship with Previous Technology Evaluations

Complements TE-0023 (Object Mapping Strategy Evaluation).

---

# Architectural References

- ADR-0001 — Clean Architecture
- TE-0023 — Object Mapping Strategy Evaluation

---

# Scope

Evaluates Mapster vs AutoMapper vs Manual Mapping.

---

# Functional Requirements

Compile-time code generation / high performance, projection support for EF Core IQueryable.

---

# Non-Functional Requirements

Zero memory overhead during execution, clean API, full .NET 10 support.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| Mapster | Primary Object Mapping Engine | Selected |
| AutoMapper | Reflection-based Mapper | Evaluated |
| Manual Extension Methods | Explicit Code Mapping | Evaluated |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Execution Speed & Memory Efficiency | Critical |
| A2 | IQueryable Projection Support | Critical |

---

# Architecture Principle

Mappings decouple Domain entities from external DTO contracts.

---

# 5. Candidate Deep-Dive Evaluations

## Mapster Evaluation

### Overview
Mapster is a high-performance object-to-object mapper for .NET.

### Architectural Strengths
- Significantly faster execution speed and lower allocations than AutoMapper.
- Native `.ProjectToType<T>()` for EF Core LINQ queries.

---

# Overall Technology Comparison

Mapster achieves near-native manual code execution speed while reducing boilerplate.

---

# Final Recommendation

Adopt Mapster as the official object mapping engine.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| Mapster | Approved |

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility

---

# Related ADR

- ADR-0001 — Clean Architecture

---

# Related Documents

- TE-0023 — Object Mapping Strategy Evaluation

---

# References

- https://github.com/MapsterMapper/Mapster

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial evaluation                                    |
| 2.0.0   | 2026-07-18 | Solution Architect | Standardized                                          |
| 3.0.0   | 2026-07-18 | Solution Architect | Rewritten according to Technology Evaluation Template |
| 3.0.1   | 2026-07-27 | Solution Architect | Corrected Related ADR reference from ADR-0009 to ADR-0008 (ADR-0009 documents Serilog, not Mapster) |
| 3.1.0   | 2026-07-28 | Solution Architect | New section added (Evaluation Scope)                  |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |