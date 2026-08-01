| Property | Value |
|----------|-------|
| **Document ID** | ADR-0007 |
| **Title** | Use FluentValidation |
| **Version** | 4.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# Context

The MachineryManagerEnterprise solution requires a modern validation framework
that keeps validation rules independent from business entities, presentation
logic, and persistence concerns.

The selected validation solution should:

- Support clean separation of concerns
- Integrate naturally with ASP.NET Core
- Be fully testable
- Be extensible
- Produce readable validation rules
- Support localization
- Align with Clean Architecture

---

# Decision

The Application Layer shall use **FluentValidation** as the standard validation
framework.

All input validation shall be implemented using FluentValidation validators.

Business entities shall not contain UI validation logic.

---

# Decision Drivers

- Separation of Concerns
- Readability
- Testability
- ASP.NET Core Integration
- Open Source
- Extensibility
- Maintainability

---

# Alternatives Considered

## DataAnnotations

Rejected because validation rules become tightly coupled to DTOs and attributes
are less expressive for complex scenarios.

---

## Custom Validation Framework

Rejected because it would duplicate existing, mature functionality while
increasing maintenance cost.

---

## Manual Validation

Rejected because it produces inconsistent validation logic and increases code
duplication.

---

# Consequences

## Positive

- Clean validation layer
- Reusable validation rules
- Easy unit testing
- Improved maintainability
- Consistent validation approach
- Rich validation capabilities

## Negative

- Developers must learn FluentValidation syntax.
- Validators require explicit registration.

---

# Architecture Impact

FluentValidation shall exist only inside the **Application Layer**.

Presentation invokes validation through the Application layer.

Domain remains independent from FluentValidation.

Infrastructure shall not contain business validators.

---

# Implementation Notes

Validators shall be registered automatically through Dependency Injection.

Each Request DTO should have a corresponding validator.

Validation shall execute before business logic.

---

# Compliance Rules

1. FluentValidation shall only exist inside Application.

2. Domain shall never reference FluentValidation.

3. Presentation shall never contain business validation rules.

4. Every Request DTO shall have a corresponding validator.

5. Business validation shall not be implemented using DataAnnotations.

6. Validation logic shall remain independent from persistence.

---

# Related Technology Evaluation

TE-0005 — FluentValidation

---

# Related Proof of Concept

Not Required

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- ADR-0006 — Use Entity Framework Core
- Dependency Catalog

---

# References

https://docs.fluentvalidation.net/

https://github.com/FluentValidation/FluentValidation

https://www.nuget.org/packages/FluentValidation

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial decision                                      |
| 2.0.0   | 2026-07-18 | Solution Architect | Standardized                                          |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |