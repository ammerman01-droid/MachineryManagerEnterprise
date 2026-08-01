| Property | Value |
|----------|-------|
| **Document ID** | ADR-0001 |
| **Title** | Adopt Clean Architecture |
| **Version** | 4.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# Context

MachineryManagerEnterprise is intended to become a long-lived enterprise platform
that must remain maintainable, testable, modular, and technology-independent.

The project is expected to evolve over many years while supporting multiple
modules, independent development, and changing infrastructure technologies.

Without a clear architectural boundary, business logic would gradually become
coupled to infrastructure concerns, increasing maintenance costs and reducing
testability.

An architectural foundation was therefore required before implementation
began.

---

# Decision

The solution shall adopt **Clean Architecture** as the primary architectural
style.

The architecture shall separate concerns into independent layers with explicit
dependency direction.

Business rules shall remain independent from frameworks, databases, UI
technologies, and external services.

---

# Decision Drivers

- Maintainability
- Testability
- Separation of Concerns
- Technology Independence
- Scalability
- Modular Development
- Long-Term Sustainability
- Low Coupling
- High Cohesion

---

# Alternatives Considered

## Traditional Layered Architecture

Simple to implement.

Rejected because business logic typically becomes coupled to infrastructure.

---

## Vertical Slice Architecture

Excellent for feature-oriented development.

Rejected because the solution requires stronger separation between Domain,
Application, Infrastructure, and Presentation.

---

## Onion Architecture

Very similar to Clean Architecture.

Rejected because Clean Architecture provides broader guidance for enterprise
applications and aligns better with project goals.

---

# Consequences

## Positive

- Independent Domain layer
- Easier unit testing
- Better maintainability
- Clear dependency direction
- Infrastructure replaceability
- Improved scalability
- Better modularization

## Negative

- More projects and abstractions
- Higher initial complexity
- Longer learning curve for new contributors

---

# Architecture Impact

The following architectural layers are established:

- Presentation
- Application
- Domain
- Infrastructure
- Shared

Dependencies shall always point inward.

Outer layers may depend on inner layers.

Inner layers shall never depend on outer layers.

---

# Implementation Notes

The solution structure shall reflect architectural boundaries.

Dependency Injection shall be configured within Infrastructure.

Business rules shall remain framework independent.

Repositories shall be accessed only through interfaces.

---

# Compliance Rules

The following rules are mandatory.

1. Domain shall never reference Infrastructure.

2. Domain shall never reference Presentation.

3. Domain shall never reference external frameworks.

4. Infrastructure shall implement Application abstractions.

5. Application shall define interfaces only.

6. Business rules shall exist only inside Domain.

7. DTOs shall never exist inside Domain.

8. Entity Framework Core shall only exist inside Infrastructure.

9. Logging implementations shall only exist inside Infrastructure.

10. UI frameworks shall never be referenced by Application or Domain.

---

# Related Technology Evaluation

Not Applicable

This ADR defines the architectural foundation for all subsequent technology
decisions.

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

- 01-Architecture.md
- 09-CapabilityModel.md
- Development Principles
- Dependency Rules

---

# References

- Clean Architecture — Robert C. Martin
- The Clean Architecture Blog Series
- Microsoft .NET Architecture Guides

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial decision                                      |
| 2.0.0   | 2026-07-18 | Solution Architect | Standardized                                          |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |