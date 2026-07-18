# ADR-0006 — Use Mapster as the Object Mapping Framework

**Status:** Accepted

**Date:** 2026-07-18

**Decision Makers**

- Solution Architect
- Development Team

---

# Context

MachineryManagerEnterprise requires an object mapping solution for transforming data between:

- Domain Entities
- DTOs
- Commands
- Queries
- View Models

The mapping solution must integrate naturally with:

- Clean Architecture
- Domain-Driven Design
- CQRS
- .NET 10

The selected framework must emphasize performance, maintainability, and compile-time safety.

---

# Problem

The application needs a mapping framework that:

- Minimizes boilerplate code
- Supports compile-time generation where possible
- Avoids excessive runtime reflection
- Remains easy to understand and debug
- Supports future scalability

---

# Considered Options

## Option 1

### AutoMapper

**Advantages**

- Mature ecosystem
- Large community
- Extensive documentation

**Disadvantages**

- Heavy reliance on runtime reflection
- Runtime configuration errors
- Lower performance
- Less suitable for modern .NET and NativeAOT scenarios

---

## Option 2

### Mapster

**Advantages**

- High performance
- Supports Source Generator
- Reflection-free mode
- Clean and concise configuration
- Easy migration path
- Good ecosystem maturity

**Disadvantages**

- Smaller community than AutoMapper

---

## Option 3

### Mapperly

**Advantages**

- Compile-time generated mapping
- Excellent performance
- Reflection-free

**Disadvantages**

- Smaller ecosystem
- Fewer production references
- Less mature than Mapster

---

## Option 4

### Manual Mapping

**Advantages**

- Maximum control
- No external dependency

**Disadvantages**

- Large amount of repetitive code
- Higher maintenance cost
- Increased probability of mapping errors

---

# Decision

The project adopts **Mapster** as the primary object mapping framework.

Whenever practical, **Mapster Source Generator** shall be preferred to reduce runtime overhead.

---

# Architectural Rules

## Layering

Mapster configuration belongs to:

```
Infrastructure
```

Application code shall only consume mapping abstractions.

---

## Domain

Domain entities shall never depend on Mapster.

No mapping attributes or mapping logic shall exist inside Domain Entities.

---

## Mapping Direction

Typical mappings include:

- DTO → Command
- Command → Domain Entity
- Domain Entity → DTO
- Domain Entity → View Model

Mappings shall remain explicit and predictable.

---

## Reflection

Runtime reflection-based mapping shall be avoided when a source-generated alternative is available.

---

## Performance

Read-heavy scenarios should use generated mappings to minimize allocation and execution overhead.

---

# Consequences

## Positive

- High performance
- Clean configuration
- Compile-time safety
- Reduced boilerplate
- Good long-term maintainability

---

## Negative

- Additional dependency
- Team members should understand generated mappings

---

# Constraints

Mapping logic must not contain business rules.

Business rules remain inside:

- Domain
- Application

Mapping is responsible only for data transformation.

---

# Future Considerations

If the .NET ecosystem converges on a different source-generated mapper in the future, migration should be evaluated.

The architecture isolates mapping concerns, reducing migration cost.

---

# Related Documents

- TE-0003 — Object Mapper Selection
- ADR-0002 — Use FluentValidation
- ADR-0003 — Use MediatR
- ADR-0004 — Use Entity Framework Core
- ADR-0005 — Use Serilog

---

# References

- Mapster Documentation
- Clean Architecture
- Domain-Driven Design
- Microsoft .NET Performance Guidelines