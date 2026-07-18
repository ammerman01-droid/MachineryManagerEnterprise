# Technology Evaluation — Object Mapper Selection

**Document ID**

TE-0003

---

# Purpose

This document evaluates object mapping technologies for
MachineryManagerEnterprise.

Object mapping shall support:

- DTO ↔ Domain mapping
- Entity ↔ DTO mapping
- Command ↔ Domain mapping
- High performance
- .NET 10
- Clean Architecture
- Maintainability

---

# Candidate Technologies

| Mapper | Status |
|---------|--------|
| AutoMapper | Evaluated |
| Mapster | Evaluated |
| Mapperly | Evaluated |
| Manual Mapping | Evaluated |

---

# Evaluation Criteria

| Criterion | Weight |
|------------|-------:|
| Performance | High |
| .NET 10 Compatibility | High |
| Compile-time Safety | High |
| Learning Curve | Medium |
| Community | High |
| Maintainability | High |
| Reflection-Free | High |
| NativeAOT Ready | Medium |

---

# Comparison

| Feature | AutoMapper | Mapster | Mapperly | Manual |
|----------|:----------:|:--------:|:---------:|:------:|
| Runtime Mapping | ✅ | ✅ | ❌ | ❌ |
| Source Generator | ❌ | ✅ | ✅ | ❌ |
| Reflection-Free | ❌ | ✅ | ✅ | ✅ |
| Performance | Medium | Excellent | Excellent | Excellent |
| NativeAOT | Weak | Good | Excellent | Excellent |
| Configuration Simplicity | Medium | Good | Excellent | Excellent |
| Community | Very Large | Large | Growing | N/A |
| Learning Curve | Low | Low | Medium | High |

---

# Individual Analysis

## AutoMapper

### Advantages

- Very mature
- Huge community
- Rich documentation

### Disadvantages

- Runtime reflection
- Slower
- More magic
- Less suitable for NativeAOT

---

## Mapster

### Advantages

- Excellent performance
- Source Generator support
- Reflection-free option
- Clean API
- Excellent balance between productivity and performance

### Disadvantages

- Smaller ecosystem than AutoMapper

---

## Mapperly

### Advantages

- Pure Source Generator
- Compile-time validation
- No runtime reflection
- Extremely fast

### Disadvantages

- Smaller ecosystem
- Less mature than Mapster

---

## Manual Mapping

### Advantages

- Maximum control
- No dependency
- Best runtime performance

### Disadvantages

- Boilerplate code
- Maintenance cost grows rapidly
- Error-prone

---

# Risk Analysis

| Mapper | Risk |
|---------|------|
| AutoMapper | Medium |
| Mapster | Low |
| Mapperly | Medium |
| Manual | High |

---

# Evaluation Score

| Mapper | Score |
|---------|------:|
| Mapster | **95 / 100** |
| Mapperly | **92 / 100** |
| AutoMapper | **79 / 100** |
| Manual Mapping | **68 / 100** |

---

# Final Evaluation

Mapster achieves the highest overall score.

Reasons:

- Excellent runtime performance
- Source Generator support
- Clean syntax
- Low maintenance
- Mature enough for enterprise systems
- Compatible with .NET 10

Mapperly is an excellent modern alternative, but its ecosystem is currently smaller than Mapster.

---

# Recommendation

The project adopts **Mapster** as the primary object mapper.

Source Generator mode should be preferred whenever practical.

---

# Future Strategy

If future .NET releases make Mapperly the de facto standard, migration can be evaluated.

The architecture isolates mapping behind the Application Layer, making future replacement feasible.

---

# Related Documents

ADR-0006 — Use Mapster