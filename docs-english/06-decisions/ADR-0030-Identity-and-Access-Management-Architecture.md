| Property | Value |
|----------|-------|
| **Document ID** | ADR-0030 |
| **Title** | Identity and Access Management Architecture |
| **Status** | Accepted |
| **Version** | 1.0.0 |
| **Decision Date** | 2026-08-02 |
| **Owner** | Solution Architect |
| **Related TE** | TE-0020 – Authentication and Identity Technology Evaluation |

---

# Context

`TE-0020-Authentication-and-Identity-Technology-Evaluation.md` contains a completed, detailed technology evaluation (Final Recommendation, Section 14) for authentication and identity. However, no ADR had formally accepted that recommendation. `000-ADR-INDEX.md` associated TE-0020 with ADR-0026 (Enterprise Security Strategy), but ADR-0026's actual Decision section covers Data Protection, AES-256 encryption, X.509 transport security and key rotation — not authentication technology. ADR-0026 itself states: "Detailed authentication mechanisms are documented separately" (line 226).

Separately, an `Identity` module already exists in the repository (`src/Modules/Identity`) without any ADR or Bounded Context definition explaining its architectural role, and without an explicit statement of whether it is a DDD business Bounded Context or a platform/cross-cutting component.

---

# Problem

Without this decision:

- TE-0020's recommendation remains unratified, and implementation of the Identity module would proceed without an approved ADR, which the AI Engineering Contract requires;
- it remains undocumented whether Identity should be modeled as a DDD Bounded Context (like Asset, Maintenance, Finance) or as a platform component;
- `03-BoundedContexts.md` correctly excludes Identity because that document's declared purpose is limited to business bounded contexts, but no other document fills that gap.

---

# Decision

MachineryManagerEnterprise formally accepts the Final Recommendation of `TE-0020` as the authentication and identity technology standard:

| Responsibility | Selected Technology |
|----------------|---------------------|
| Local Identity Management | ASP.NET Core Identity |
| Authorization Server | OpenIddict |
| Access Token Format | JWT |
| Refresh Tokens | OpenIddict |
| Authorization Model | Policy + Claims + Roles |
| Optional External Providers | Microsoft Entra ID, Google Identity, GitHub Identity (all optional) |

Architecturally, **Identity is a platform/cross-cutting module, not a DDD business Bounded Context**. This is consistent with TE-0020's own architecture principle: "Business modules never authenticate users directly" and "Business modules never manipulate Identity entities directly."

This is the same classification already used elsewhere in this repository's documentation history for platform-level concerns (`000-ADR-INDEX.md` revision 3.4.0 introduced a "Cross-Cutting / Platform Architecture" category for comparable ADRs). Identity belongs in that same category.

Consequently:

- Identity is correctly absent from `03-BoundedContexts.md`, which is scoped to business bounded contexts only.
- The `src/Modules/Identity` module shall be documented and structured as a platform module following the same Clean Architecture layering as business modules (Domain / Application / Infrastructure / Presentation, per `01-SolutionStructure.md`), but it is consumed by business modules through the authorization abstraction rather than being modeled as a business capability itself.

---

# Identity and Access Architecture

```text
                External Identity Providers

        ┌────────────┬────────────┬────────────┐
        │            │            │
        ▼            ▼            ▼

 Microsoft Entra   Google      GitHub

                  │

                  ▼

        ASP.NET Core Identity

                  │

                  ▼

              OpenIddict

                  │

                  ▼

      JWT Access / Refresh Tokens

                  │

                  ▼

          Authorization Middleware

                  │

                  ▼

            Business Modules
```

This diagram is reproduced from TE-0020's Final Recommendation and is now binding.

---

# Relationship to Organization

Identity answers **who the caller is** (authentication).

The Organization Context (`03-BoundedContexts.md`, Section 4; `BR-017-BusinessSpecification-OrganizationManagement.md`) answers **which business boundary the caller is acting within** (authorization scope).

Identity shall never own or duplicate Organization data. Authorization decisions consume both concerns together but each is owned by exactly one component.

---

# Consequences

Positive

- Closes the previously unratified TE-0020 recommendation.
- Removes ambiguity about whether Identity is a business Bounded Context.
- Gives the existing `src/Modules/Identity` skeleton an approved architectural basis to be completed against.

Negative

- The `src/Modules/Identity` module must be restructured to include a Presentation layer, consistent with `01-SolutionStructure.md`.
- `000-ADR-INDEX.md`'s "Overall Technology Comparison" table currently lists ADR-0026 against a "OpenID Connect & Keycloak" standard that does not match ADR-0026's actual Decision section. This pre-existing inconsistency is noted here for visibility; correcting it is outside the scope of this ADR and requires separate review.

---

# Alternatives Considered

## Modeling Identity as a Business Bounded Context

Rejected. TE-0020 explicitly separates Identity from business modules. Forcing Identity into `03-BoundedContexts.md` would contradict that document's own stated purpose (business domain boundaries only).

## Leaving TE-0020 Unratified

Rejected. The AI Engineering Contract requires an approved ADR before implementation proceeds; an evaluation document alone is not a decision record.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Clean Architecture
- ADR-0026 — Enterprise Security Strategy (Data Protection / Encryption — distinct concern)
- TE-0020 — Authentication and Identity Technology Evaluation
- 03-BoundedContexts.md — Section 4, Organization Context
- BR-017-BusinessSpecification-OrganizationManagement.md

---

# Decision Outcome

**Accepted**

MachineryManagerEnterprise adopts ASP.NET Core Identity, OpenIddict, and JWT as its authentication and identity platform, and formally classifies Identity as a platform/cross-cutting module rather than a DDD business Bounded Context.

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial version — ratifies TE-0020 Final Recommendation and classifies Identity as a platform module |
