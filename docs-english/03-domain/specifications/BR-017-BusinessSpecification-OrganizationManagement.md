| Property | Value |
|----------|-------|
| **Document ID** | BR-015 |
| **Version** | 1.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-08-02 |
| **Last Updated** | 2026-08-02 |

---

# Purpose

## Objective

This specification defines the Organization Management capability of MachineryManagerEnterprise.

The capability exists because every Asset managed by the platform must have a clear business owner. `PROJECT_CHARTER.md` lists **Organization Management** as a Long-Term Objective, and `04-DomainModel.md` defines Organization as the entity that owns Assets (`Organization → Owns → Assets`). `04-modules/07-Authorization.md` additionally relies on Organization as the scope boundary for permissions (`Organization.Manage`, "Resolve Organization").

Without a formally modeled Organization, Asset ownership, and Organization-scoped authorization, cannot be implemented consistently.

Do not describe implementation details.

---

## Scope

### In Scope

This specification defines:

- The business meaning of an Organization within MachineryManagerEnterprise.
- The relationship between an Organization and the Assets it owns.
- The relationship between an Organization and the authorization boundary described in `04-modules/07-Authorization.md`.

### Out of Scope

This specification does not define implementation details, architectural decisions, or technical design. It does not define the Identity/authentication mechanism (see ADR-0030), which is a separate platform concern.

---

# 1. Business Problem

Construction and industrial companies that use MachineryManagerEnterprise own and operate machinery as a business entity, not as individuals. Every Asset, cost, and maintenance record must be traceable to the Organization responsible for it.

Today, the approved documentation (`04-DomainModel.md`, `03-BoundedContexts.md`) references Organization as the owner of Assets and as the source of authorization scope, but no document defines Organization itself as a modeled business capability. This creates ambiguity for anyone implementing Asset ownership or Organization-scoped permissions.

---

# 2. Business Definitions

| Term | Definition |
|------|------------|
| Organization | The business entity (a company or operating unit) that owns Assets and on whose behalf Users act. |
| Asset Ownership | The relationship by which an Organization is the business owner of an Asset, as defined in `04-DomainModel.md`. |
| Organization Boundary | The scope used by the Authorization model to determine which Assets and records a User may access, as referenced in `04-modules/07-Authorization.md`. |

---

# 3. Business Rules

The following rules are derived directly from already-approved documentation. No new business rule is introduced beyond what those documents already imply.

```
BR-015-1 (from 04-DomainModel.md, Section 9 and Section 10)
Every Asset shall have exactly one owning Organization.
```

```
BR-015-2 (from 04-modules/07-Authorization.md)
Every Organization-scoped permission shall be evaluated within the boundary of a single resolved Organization.
```

Additional business rules (e.g., whether an Organization may have sub-organizations, or whether Assets may be transferred between Organizations) are **not yet documented anywhere** and are recorded as open questions in Section 9 rather than assumed here.

---

# 4. Operational Logic

Not yet defined in approved documentation beyond the ownership relationship already stated in `04-DomainModel.md` (Organization → Owns → Assets). Lifecycle states for an Organization (e.g., registration, suspension, deactivation) are not documented and are listed as an open question.

---

# 5. Constraints

- An Asset shall not exist without an owning Organization (derived from `04-DomainModel.md` Ownership Rules).
- Authorization checks that are Organization-scoped shall not evaluate across Organization boundaries (derived from `04-modules/07-Authorization.md`).

---

# 6. Operational Scenarios

## Scenario: Asset Registration

- **Initial State:** An Organization exists in the system.
- **Business Event:** The Organization registers a new Asset.
- **Expected Result:** The Asset is created with exactly one owning Organization, consistent with `04-DomainModel.md`.

## Scenario: Organization-Scoped Access

- **Initial State:** A User is associated with an Organization.
- **Business Event:** The User requests access to an Asset record.
- **Expected Result:** Access is evaluated within the User's resolved Organization boundary, consistent with `04-modules/07-Authorization.md`.

---

# 7. Future Domain Impacts

Every business module that references Assets depends indirectly on Organization as the ownership root, per the Context Map in `03-BoundedContexts.md`:

- Asset Context
- Finance Context (cost/ownership reporting)
- Document Context (ownership documents)
- Maintenance, Forecast, and Reporting modules (all Organization-scoped)

---

# 8. Non-Functional Requirements

- Traceability: every Asset must be traceable to exactly one Organization at all times.
- Auditability: changes to Asset ownership must be preserved as history, consistent with the historical-entity principles in `04-DomainModel.md`.

---

# 9. Open Questions and Future Decisions

The following are explicitly **not** answered by any currently approved document and must not be assumed:

- Can an Organization contain sub-organizations or departments?
- Can Asset ownership be transferred between Organizations, and if so, how is history preserved?
- What is the full lifecycle of an Organization (registration, suspension, deactivation)?
- Does the platform support a single Organization per deployment, or multiple Organizations (multi-tenancy) in the same deployment?

These questions should be resolved through Domain Discovery before the Application layer for this module is implemented.

---

# 10. Conclusion

Organization Management is a foundational capability: it is the business owner of every Asset and the scope boundary for authorization. It should be modeled before, or together with, the Asset Context, since Asset ownership is not meaningful without it.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

| Document | Purpose |
|----------|---------|
| PROJECT_CHARTER.md | States Organization Management as a Long-Term Objective |
| 04-DomainModel.md | Defines Organization as the owner of Assets |
| 03-BoundedContexts.md | Defines the Organization Context (Section 4) |
| 04-modules/07-Authorization.md | Defines Organization-scoped authorization |
| ADR-0030 | Identity as a platform component, distinct from Organization |

---

## Traceability

| Artifact | Reference |
|----------|-----------|
| Capability | Organization Management |
| Bounded Context | Organization Context (`03-BoundedContexts.md`, Section 4) |
| ADR | ADR-0030 |

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial draft, synthesized from PROJECT_CHARTER.md, 04-DomainModel.md, 03-BoundedContexts.md, and 04-modules/07-Authorization.md |
