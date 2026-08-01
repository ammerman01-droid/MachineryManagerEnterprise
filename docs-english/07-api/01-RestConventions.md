| Property | Value |
|----------|-------|
| **Document ID** | API-001 |
| **Title** | REST Conventions |
| **Version** | 4.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# 1. Purpose

This document defines the REST conventions used throughout MachineryManagerEnterprise.

Every HTTP endpoint shall follow these conventions.

---

# REST Philosophy

REST resources expose business capabilities rather than implementation details.

Resource URIs identify business concepts.

HTTP methods describe the requested operation.

The transport protocol shall never expose internal software architecture.

---

# URI Design Rules

URIs shall:

- identify resources.
- never expose verbs unless representing exceptional business actions.
- remain stable.
- avoid implementation details.
- avoid technology-specific identifiers.

---

# 2. REST Principles

The API shall follow standard REST principles.

Resources represent business entities.

HTTP methods represent operations.

HTTP status codes represent results.

---

# 3. Base URL

Example

```
/api/v1
```

Every public endpoint shall begin with the API version.

---

# 4. Resource Naming

Resources shall:

- use plural nouns;
- use lowercase;
- use hyphen (`-`) as separator when required.

Examples

```
/assets

/engines

/components

/maintenance-orders

/documents

/forecasts
```

Avoid

```
/GetAssets

/AssetList

/AssetManager
```

---

# 5. Resource Identifier

A single resource shall be addressed by its identifier.

Example

```
GET /assets/{assetId}
```

Example

```
GET /engines/{engineId}
```

---

# 6. HTTP Methods

| Method | Purpose |
|----------|---------------------------|
| GET | Read |
| POST | Create |
| PUT | Replace |
| PATCH | Partial Update |
| DELETE | Remove |

---

# 7. GET

GET shall never modify state.

Examples

```
GET /assets

GET /assets/{id}

GET /maintenance-orders
```

GET requests shall be idempotent.

---

# 8. POST

POST creates a new resource.

Example

```
POST /assets
```

POST is not required to be idempotent.

---

# 9. PUT

PUT replaces the entire resource.

Example

```
PUT /assets/{id}
```

PUT shall be idempotent.

---

# 10. PATCH

PATCH updates part of a resource.

Example

```
PATCH /assets/{id}
```

PATCH shall modify only explicitly supplied fields and shall never reset omitted values.

---

# 11. DELETE

DELETE removes or retires a resource.

Example

```
DELETE /documents/{id}
```

Business rules determine whether deletion is physical or logical.

---

# 12. Nested Resources

Nested resources shall represent ownership.

Example

```
GET /assets/{assetId}/engines

GET /assets/{assetId}/documents

GET /assets/{assetId}/maintenance-history
```

Deep nesting should be avoided.

Recommended maximum depth:

```
2
```

---

# 13. Actions

REST favors resources over verbs.

Preferred

```
POST /maintenance-orders

POST /forecasts
```

Avoid

```
POST /create-maintenance

POST /generate-forecast
```

When an explicit business action is required:

```
POST /assets/{id}/retire

POST /engines/{id}/rebuild
```

Business actions shall remain exceptional.

---

# 14. Query Parameters

Filtering and searching shall use query parameters.

Examples

```
GET /assets?status=Active

GET /assets?organizationId=15

GET /engines?serialNumber=ABC123
```

---

# 15. Sorting

Sorting shall use:

```
sort=
```

Examples

```
GET /assets?sort=name

GET /assets?sort=-createdAt
```

Negative indicates descending order.

---

# 16. Pagination

Collections shall support pagination.

Example

```
GET /assets?page=1&pageSize=25
```

Pagination details are defined in a separate document.

---

# 17. Content Types

Requests

```
application/json
```

Responses

```
application/json
```

Future versions may support additional media types.

---

# 18. Idempotency

| Method | Idempotent |
|----------|-----------|
| GET | Yes |
| PUT | Yes |
| PATCH | Usually |
| DELETE | Yes |
| POST | No |

---

# 19. URI Stability

Resource URIs shall remain stable.

Renaming public URIs requires:

- new API version;
- migration strategy;
- documentation update.

---

# 20. Future Extensions

Future API styles may include:

- OData
- GraphQL
- gRPC
- Event Streaming

REST remains the primary integration style.

---

| HTTP   | CQRS    |
| ------ | ------- |
| GET    | Query   |
| POST   | Command |
| PUT    | Command |
| PATCH  | Command |
| DELETE | Command |

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 00-ApiPrinciples.md
- 02-HttpStatusCodes.md
- 03-ErrorResponses.md
- 04-Pagination.md
- 05-Filtering.md
- ADR-0005 — API Strategy

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial REST Conventions                              |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |