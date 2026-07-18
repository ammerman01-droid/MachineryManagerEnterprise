# Request / Response Model

**Document ID:** MME-API-003

**Repository Path:** `docs/07-api/03-RequestResponseModel.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-ApiPrinciples.md
- 01-RestConventions.md
- 02-EndpointDesign.md

---

# 1. Purpose

This document defines the standard structure of every API request and response used by MachineryManagerEnterprise.

A consistent request/response model simplifies client development, improves interoperability and reduces implementation ambiguity.

---

# 2. Principles

Every request and response shall be:

- Predictable
- Explicit
- Consistent
- Version independent
- Machine readable
- Human understandable

---

# 3. Request Model

A request contains only the information required for execution.

Example

```json
{
  "assetNumber": "AST-000145",
  "organizationId": "ORG-001",
  "engineId": "ENG-0007"
}
```

Requests shall not contain:

- Internal identifiers
- Database metadata
- Audit information
- Server-generated values

---

# 4. Response Model

Successful responses return the requested business resource.

Example

```json
{
  "id": "AST-000145",
  "assetNumber": "AST-000145",
  "status": "Active"
}
```

---

# 5. Collection Response

Collection endpoints return arrays.

Example

```json
{
  "items": [
    {
      "id": "AST-0001",
      "assetNumber": "AST-0001"
    },
    {
      "id": "AST-0002",
      "assetNumber": "AST-0002"
    }
  ]
}
```

Collection metadata is described in the Pagination document.

---

# 6. Resource Identity

Every business resource shall expose a stable identifier.

Example

```json
{
  "id": "AST-000145"
}
```

Identifiers are immutable.

---

# 7. DTO Usage

Public APIs expose DTOs.

Entities shall never be serialized directly.

The API contract remains independent from internal domain implementation.

---

# 8. Null Values

Avoid nullable fields whenever practical.

Preferred

```json
{
  "documents": []
}
```

Avoid

```json
{
  "documents": null
}
```

Collections should return empty arrays instead of null.

---

# 9. Date and Time

Date/time values shall use ISO-8601.

Example

```json
{
  "createdAt": "2026-07-18T09:35:12Z"
}
```

UTC shall be the default representation.

---

# 10. Enumerations

Enumeration values shall be represented as strings.

Preferred

```json
{
  "status": "Active"
}
```

Avoid numeric enum values.

---

# 11. Boolean Values

Boolean properties shall represent clear business meaning.

Examples

```json
{
  "isActive": true,
  "isArchived": false
}
```

Double negatives should be avoided.

---

# 12. Numeric Values

Numbers shall use appropriate JSON numeric types.

Currency values should preserve required precision.

Measurements shall use documented business units.

---

# 13. Optional Properties

Optional properties may be omitted.

Clients shall not assume every property exists.

The meaning of omitted properties shall be documented.

---

# 14. Hypermedia

The initial API does not require HATEOAS.

Future versions may introduce hypermedia links if required.

---

# 15. Contract Stability

Once published, request and response contracts are considered public.

Breaking changes require:

- New API version
- Documentation update
- Compatibility review

---

# 16. Future Extensions

Future response models may include:

- Localization metadata
- ETag values
- Hypermedia links
- Partial projections
- Embedded resources

Such extensions shall remain backward compatible.

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Request / Response Model |