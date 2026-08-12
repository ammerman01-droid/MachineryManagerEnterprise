| Property | Value |
| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | TE-0036            |
| **Title**        | External Integration and Connector Technology Evaluation |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-08-02         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

ADR-0018 — External Integration Architecture establishes the
architectural principles for communication between
MachineryManagerEnterprise and external systems (ERP, CRM, GIS, IoT
Platforms, Identity Providers, Government Services, Email/SMS
Providers, Payment Services, and third-party APIs), but intentionally
deferred concrete technology selection.

No previously approved Technical Evaluation covered this concern —
TE-0012 (Enterprise Messaging) and TE-0018 (Configuration and Secrets
Management) had both been erroneously cross-referenced from ADR-0018 in
earlier documentation revisions, but neither evaluates connector or
external-integration technology. This evaluation closes that gap.

The expected outcome is a recommended connector/integration technology
stack, not a final ADR.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection for
outbound/inbound integration with external systems. Implementation
details are defined by ADR-0018.

---

# Relationship with Previous Technology Evaluations

This evaluation does not supersede or depend on any previously approved
Technology Evaluation, but deliberately considers TE-0012's outcome
(MassTransit / RabbitMQ, approved for internal Enterprise Messaging via
ADR-0016) as an incumbent candidate for reuse, to avoid introducing a
second, redundant messaging technology into the platform.

---

# Architectural References

- ADR-0001 — Adopt Clean Architecture & Modular Monolith
- ADR-0002 — Open Source First Policy
- ADR-0016 — Enterprise Messaging Architecture
- ADR-0018 — External Integration Architecture

---

# Scope

**In scope:** Outbound/inbound connector technology for ERP, CRM, GIS,
IoT, Government Services, Email/SMS, Payment Services, and third-party
REST/SOAP APIs.

**Out of scope:** Internal component-to-component messaging (governed
by ADR-0016), API documentation (governed by ADR-0035), and Identity
Provider protocol selection (governed by ADR-0030).

---

# Functional Requirements

- Support outbound calls to external REST/SOAP/webhook-based systems.
- Support inbound webhook/event reception from external systems.
- Isolate external-system-specific logic from business modules
  (Adapter/Connector pattern).
- Support retry, timeout, and circuit-breaking for unreliable external
  systems.

---

# Non-Functional Requirements

- Open Source First (per ADR-0002), unless no viable open-source
  option exists.
- Cloud neutrality — no mandatory dependency on a single cloud vendor.
- Reuse of already-approved platform technology where reasonable.

---

# 1. Candidate: MassTransit-Based Connector Framework (Reuse of ADR-0016)

MassTransit (already approved for internal messaging) supports courier
routing slips, sagas, and transport-agnostic message adapters that can
be extended to external-system connectors, publishing/consuming through
the same RabbitMQ broker already approved by ADR-0016.

**Pros:** No new technology introduced; reuses existing operational
knowledge, infrastructure, and monitoring; open source; cloud-neutral.

**Cons:** Requires custom adapter code per external system; not a
turnkey low-code integration tool.

---

# 2. Candidate: Azure Logic Apps

A managed, low-code Azure integration service with prebuilt connectors
for many third-party systems.

**Pros:** Fast time-to-integrate for common systems via prebuilt
connectors; minimal custom code.

**Cons:** Azure-proprietary, conflicting with the platform's cloud
neutrality principle; commercial/consumption-based pricing conflicts
with the Open Source First policy (ADR-0002); introduces a second,
disconnected integration mechanism alongside MassTransit.

---

# 3. Candidate: NServiceBus Adapters

A commercial enterprise service bus with built-in external adapters.

**Pros:** Mature adapter ecosystem.

**Cons:** Commercial license, directly conflicting with the Open Source
First policy (ADR-0002); functionally redundant with the
already-approved MassTransit/RabbitMQ stack from ADR-0016.

---

# Overall Technology Comparison

| Candidate | Open Source | Cloud Neutral | Reuses Existing Stack | Recommended |
|---|---|---|---|---|
| MassTransit-based Connector Framework | Yes | Yes | Yes | **Yes** |
| Azure Logic Apps | No | No | No | Approved only as an optional, non-default integration for Azure-hosted deployments |
| NServiceBus Adapters | No | Yes | No | Not recommended |

---

# Final Recommendation

Adopt a **Connector Framework built on the already-approved MassTransit
/ RabbitMQ stack** (ADR-0016) as the default external integration
mechanism, using a dedicated Adapter per external system to isolate
system-specific logic from business modules.

Azure Logic Apps is approved only as an optional, non-default
integration path for Azure-hosted deployments that require rapid
low-code connectivity to a specific prebuilt connector; it shall not be
treated as the platform standard.

NServiceBus Adapters are not adopted.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| Default External Integration Mechanism | MassTransit-based Connector Framework — Approved |
| Optional Azure-specific Path | Azure Logic Apps — Approved (non-default, opt-in) |
| Alternative Enterprise Service Bus | NServiceBus Adapters — Not Adopted |

---

# Related Architecture Decision

- ADR-0018 — External Integration Architecture

---

# Decision Summary

- ✔ Clean Architecture
- ✔ Open Source First Policy
- ✔ Cloud Neutrality
- ✔ Reuses already-approved platform technology (ADR-0016)

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial evaluation, created to close ADR-0018's previously unresolved Technical Evaluation reference |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |