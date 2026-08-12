| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | API-000            |
| **Title**        | API Principles     |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the architectural principles governing all external APIs exposed by MachineryManagerEnterprise.

Every HTTP endpoint, request, response and integration contract shall conform to these principles.

These principles take precedence over implementation preferences.

---

# API Design Philosophy

The API exposes business capabilities rather than software implementation.

API Contracts are long-lived public assets.

Every API shall remain stable, explicit, versioned and independently consumable.

---

# 2. Objectives

The API shall be:

- Consistent
- Predictable
- Stable
- Discoverable
- Secure
- Versionable
- Technology Independent

The API represents the public contract of the system.

---

# 3. API Philosophy

The API exposes business capabilities.

The API does not expose internal implementation.

Consumers interact with business concepts rather than database structures.

---

# 4. Architectural Principles

The API shall:

- follow REST principles;
- remain stateless;
- expose resource-oriented endpoints;
- use standard HTTP semantics;
- remain independent of persistence technology.

---

# 5. Business-Oriented Design

Resources shall represent business concepts.

Examples

- Assets
- Engines
- Components
- Maintenance
- Documents
- Forecasts

Resources shall never represent database tables.

---

# 6. Contract Stability

Public contracts are considered stable.

Breaking changes shall be avoided.

When breaking changes are unavoidable:

- a new API version shall be introduced;
- previous versions shall remain supported according to the release policy.

---

# 7. Consistency

The following shall remain consistent across the entire API.

- URI structure
- HTTP methods
- Status codes
- Error responses
- Pagination
- Filtering
- Authentication
- Naming

---

# 8. Stateless Communication

Every request shall contain all information required for execution.

The server shall not rely on client session state.

---

# 9. Explicit Contracts

Every endpoint shall explicitly define:

- Request model
- Response model
- Status codes
- Validation rules
- Authorization requirements

Implicit behavior is prohibited.

---

# 10. Security First

All APIs shall be designed assuming untrusted clients.

Authentication shall always precede authorization.

Sensitive information shall never be exposed.

---

# 11. Versioning

The API shall support explicit versioning.

Version identifiers shall remain stable throughout the supported lifecycle.

---

# 12. Error Handling

Errors shall be:

- deterministic;
- machine-readable;
- human-readable;
- traceable.

Unexpected exceptions shall never be returned directly to clients.

---

# 13. Observability

Every API request shall be traceable through:

- Correlation Identifier
- Request Identifier
- Audit information
- Structured logs

---

# 14. Documentation

Every public endpoint shall be documented.

Documentation shall remain synchronized with implementation.

Generated documentation shall be considered part of the delivered product.

---

# 15. Extensibility

The API shall support future expansion without redesign.

New resources shall integrate consistently with existing conventions.

---

# 16. Technology Independence

Clients shall not be required to know:

- database schema;
- ORM technology;
- internal architecture;
- implementation language.

Only business contracts are exposed.

---

# 17. Decision Hierarchy

When API design decisions conflict:

1. Business Rules
2. Domain Principles
3. Architecture
4. API Principles
5. REST Conventions
6. Implementation Preferences

---

# 18. API Layer Responsibilities

The API Layer shall:

- Translate HTTP requests into Commands or Queries.
- Translate Application Results into HTTP responses.
- Never implement business rules.
- Never access persistence directly.
- Never expose internal domain objects.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- Architecture Overview
- Domain Principles
- Module Overview
- Authorization
- Commands
- Queries
- ADR-0035 — API Documentation and Client Generation Architecture

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial API Principles                                |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Corrected reference from non-existent "ADR-0005 — API Strategy" to the actual governing ADR-0035 (API Documentation and Client Generation Architecture) |