# Coding Standards

**Document ID:** MME-DEV-005

**Repository Path:** `docs/05-development/05-CodingStandards.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-DevelopmentPrinciples.md
- 01-SolutionStructure.md
- 02-ProjectStructure.md
- 03-NamespaceConvention.md
- 04-DependencyRules.md

---

# 1. Purpose

This document defines the coding standards used throughout MachineryManagerEnterprise.

Coding standards ensure that the entire codebase remains consistent, readable and maintainable regardless of the number of contributors.

---

# 2. General Principles

Source code shall be:

- Readable
- Predictable
- Consistent
- Maintainable
- Self-documenting

Code is written primarily for humans.

Compilers are secondary readers.

---

# 3. SOLID Principles

All production code shall follow SOLID.

- Single Responsibility Principle
- Open / Closed Principle
- Liskov Substitution Principle
- Interface Segregation Principle
- Dependency Inversion Principle

Violations require architectural justification.

---

# 4. DRY

Duplicate logic shall be eliminated.

Business logic shall exist in exactly one place.

Shared behavior shall be extracted only when it improves clarity.

---

# 5. KISS

Prefer the simplest solution that correctly solves the problem.

Avoid unnecessary abstraction.

Avoid speculative architecture.

---

# 6. YAGNI

Features that are not currently required shall not be implemented.

Future extensibility is achieved through architecture, not premature implementation.

---

# 7. Class Design

Classes shall:

- have one responsibility;
- remain cohesive;
- expose minimal public surface;
- hide implementation details.

Large classes shall be decomposed.

---

# 8. Method Design

Methods shall:

- perform one logical task;
- remain short;
- have descriptive names;
- avoid side effects whenever practical.

Deep nesting should be avoided.

---

# 9. Constructors

Constructors shall only initialize dependencies and required state.

Constructors shall never execute business logic.

---

# 10. Dependency Injection

Dependencies shall be injected.

Direct object creation using `new` shall be avoided except for:

- Value Objects
- DTOs
- Primitive helper types

---

# 11. Exceptions

Exceptions represent exceptional situations.

Business validation shall not rely on exceptions.

Expected failures should be represented using explicit Result objects.

---

# 12. Asynchronous Programming

I/O operations shall be asynchronous.

CPU-bound operations should remain synchronous unless parallel execution is justified.

Avoid blocking asynchronous code.

---

# 13. Null Handling

Nullable references shall be minimized.

Guard clauses shall validate required arguments.

Null shall never represent business state when a Value Object or Enumeration is more appropriate.

---

# 14. Comments

Comments shall explain:

- why;
- business intent;
- architectural decisions.

Comments shall not explain obvious code.

Bad example

```csharp
// Increment i
i++;
```

Good example

```csharp
// Preserve historical meter continuity during replacement.
```

---

# 15. Regions

`#region` shall not be used in production code except when explicitly approved.

Proper class decomposition is preferred.

---

# 16. Magic Values

Magic numbers and magic strings are prohibited.

Use:

- Constants
- Enumerations
- Value Objects
- Configuration

---

# 17. Formatting

Formatting shall remain consistent across the solution.

Automatic formatting shall be enabled.

Manual formatting differences shall not appear in commits.

---

# 18. Code Reviews

Every Pull Request shall verify:

- Architecture
- Readability
- Naming
- Dependency rules
- Business correctness
- Test coverage

Style issues should be resolved before merge.

---

# 19. Static Analysis

Static analysis tools should remain enabled.

Warnings shall be resolved whenever practical.

Suppressions require justification.

---

# 20. Future Standards

Future versions may define standards for:

- Performance
- Security
- Multi-threading
- Memory allocation
- Native AOT
- Distributed processing

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Coding Standards |