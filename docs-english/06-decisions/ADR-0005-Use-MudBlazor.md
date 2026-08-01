| Property | Value |
|----------|-------|
| **Document ID** | ADR-0005 |
| **Title** | Use MudBlazor |
| **Version** | 4.0.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# Context

The MachineryManagerEnterprise solution requires a modern component library for
building enterprise-grade user interfaces with Blazor.

The selected UI component framework should provide:

- Native Blazor integration
- Rich component ecosystem
- Active open-source community
- Material Design implementation
- High maintainability
- Excellent documentation
- Long-term sustainability

The framework must also align with the project's Open Source First Policy.

---

# Decision

The Presentation Layer shall use **MudBlazor** as the primary UI component
library.

MudBlazor shall be the standard component library for all application modules.

---

# Decision Drivers

- Native Blazor integration
- Open Source
- Rich component library
- Active community
- Material Design
- Maintainability
- Developer productivity
- Consistent user experience

---

# Alternatives Considered

## Radzen

Rejected because the open-source version provides fewer enterprise features and
the commercial offering introduces licensing considerations.

---

## Syncfusion

Rejected because the full feature set requires a commercial license.

---

## Telerik UI for Blazor

Rejected because it is a commercial product and conflicts with the project's
Open Source First Policy.

---

## Plain Bootstrap Components

Rejected because it significantly increases UI development effort and results in
inconsistent component implementations.

---

# Consequences

## Positive

- Consistent UI across the application
- Reduced UI development effort
- Excellent integration with Blazor
- Large reusable component set
- Lower maintenance cost
- Modern Material Design experience

## Negative

- Dependency on Material Design principles
- Some advanced scenarios may still require custom components

---

# Architecture Impact

MudBlazor shall only exist inside the **Presentation Layer**.

No other architectural layer shall reference MudBlazor assemblies.

Application and Domain remain completely independent of UI technologies.

---

# Implementation Notes

MudBlazor shall be registered through dependency injection during application
startup.

Custom reusable UI components should wrap MudBlazor components where appropriate
to reduce coupling with the underlying library.

---

# Compliance Rules

1. MudBlazor shall only be used inside the Presentation layer.

2. Application shall never reference MudBlazor.

3. Domain shall never reference MudBlazor.

4. Shared UI components should encapsulate common visual behavior.

5. Business logic shall never be implemented inside MudBlazor components.

---

# Related Technology Evaluation

TE-0003 — MudBlazor

---

# Related Proof of Concept

POC-0001 — Jalali MudDatePicker

---


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
- ADR-0003 — Use .NET 10
- ADR-0004 — Use Blazor
- Dependency Catalog

---

# References

https://mudblazor.com/

https://github.com/MudBlazor/MudBlazor

https://www.nuget.org/packages/MudBlazor

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial decision                                      |
| 2.0.0   | 2026-07-18 | Solution Architect | Standardized                                          |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |