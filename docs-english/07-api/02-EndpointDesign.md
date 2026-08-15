| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | API-002            |
| **Title**        | Endpoint Design    |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the endpoint design guidelines used throughout MachineryManagerEnterprise.

Every endpoint shall present a consistent, predictable and business-oriented interface.

---

# 2. Endpoint Philosophy

Endpoints expose business capabilities.

Endpoints do not expose database operations.

Every endpoint shall represent a meaningful business action or business resource.

---

# Endpoint Lifecycle

Every endpoint shall:

1. Receive the HTTP request.
2. Validate transport-level constraints.
3. Invoke exactly one Command or Query.
4. Return a stable contract.
5. Never expose internal implementation details.

---

# Endpoint Design Rules

Endpoints shall:

- Represent business capabilities.
- Remain technology independent.
- Use stable URIs.
- Return consistent response models.
- Never expose database concepts.

---

# 3. Resource-Oriented Design

Endpoints shall be organized around resources.

Examples

```
/assets

/engines

/components

/maintenance-orders

/documents

/forecasts
```

Resources represent business entities.

---

# 4. Collection Endpoints

Collection endpoints return multiple resources.

Examples

```
GET /assets

GET /engines

GET /documents
```

Collection endpoints shall support:

- Pagination
- Filtering
- Sorting

---

# 5. Single Resource Endpoints

Single-resource endpoints identify one business object.

Examples

```
GET /assets/{assetId}

GET /engines/{engineId}

GET /documents/{documentId}
```

Identifiers shall be globally unique within their resource.

---

# 6. Resource Creation

Creation endpoints use POST.

Examples

```
POST /assets

POST /maintenance-orders

POST /documents
```

The request body contains the creation model.

The response returns the created resource.

---

# 7. Resource Update

Complete replacement

```
PUT /assets/{assetId}
```

Partial modification

```
PATCH /assets/{assetId}
```

Update requests shall validate business rules before persistence.

---

# 8. Resource Removal

Removal endpoints use DELETE.

Example

```
DELETE /documents/{documentId}
```

Whether deletion is physical or logical depends on business rules.

---

# 9. Business Operations

Some business operations are not CRUD.

Examples

```
POST /assets/{assetId}/retire

POST /engines/{engineId}/install

POST /engines/{engineId}/remove

POST /maintenance-orders/{id}/complete

POST /documents/{id}/renew
```

Business actions that cannot be naturally represented as CRUD operations shall use POST.

---

# 10. Child Resources

Child resources represent ownership.

Examples

```
GET /assets/{assetId}/engines

GET /assets/{assetId}/documents

GET /assets/{assetId}/maintenance-history
```

Nested resources shall describe natural business relationships.

---

# 11. Search Endpoints

Search operations remain resource-oriented.

Preferred

```
GET /assets?serialNumber=...

GET /documents?status=...

GET /engines?manufacturer=...
```

Avoid dedicated `/search` endpoints unless the query is too complex for standard filtering.

---

# 12. Forecast Endpoints

Forecasts are generated business results.

Examples

```
POST /forecasts

GET /forecasts/{forecastId}

GET /assets/{assetId}/forecasts
```

Forecast generation may be synchronous or asynchronous.

---

# 13. Long Running Operations

Operations requiring significant time should return:

```
202 Accepted
```

Example

```
POST /forecasts
```

Response

```
OperationId

Status

Location
```

Clients may later retrieve completion status.

---

# 14. Bulk Operations

Bulk operations shall be explicit.

Examples

```
POST /assets/bulk-import

POST /documents/bulk-update

POST /maintenance-orders/bulk-close
```

Bulk operations shall never overload standard CRUD endpoints.

---

# 15. Endpoint Naming Rules

Endpoint names shall:

- use nouns;
- avoid verbs;
- remain lowercase;
- remain stable.

Correct

```
/assets

/engines

/components
```

Incorrect

```
/CreateAsset

/GetAssets

/DeleteEngine
```

---

# 16. Endpoint Stability

Public endpoints are considered contractual interfaces.

Changing an endpoint requires:

- API version review
- Documentation update
- Backward compatibility assessment

---

# 17. Future Expansion

Future modules shall follow the same endpoint structure.

Examples

```
/inventory

/procurement

/contracts

/fleet

/iot
```

Consistency shall be preserved across all future capabilities.

---

| Endpoint                   | Application            |
| -------------------------- | ---------------------- |
| `GET /assets`              | `GetAssetsQuery`       |
| `GET /assets/{id}`         | `GetAssetByIdQuery`    |
| `POST /assets`             | `RegisterAssetCommand` |
| `PATCH /assets/{id}`       | `UpdateAssetCommand`   |
| `POST /assets/{id}/retire` | `RetireAssetCommand`   |

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
- 01-RestConventions.md
- 03-RequestResponseModel.md
- 04-ErrorResponses.md
- 06-Versioning.md
- ADR-0035 — API Documentation and Client Generation Architecture

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial | Initial Endpoint Design                     |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Corrected reference from non-existent "ADR-0005 — API Strategy" to the actual governing ADR-0035 (API Documentation and Client Generation Architecture) |