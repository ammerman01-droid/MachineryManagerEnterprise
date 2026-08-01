| Property | Value |
|----------|-------|
| **Document ID** | API-004 |
| **Title** | Error Responses |
| **Version** | 4.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# 1. Purpose

This document defines the standard error response model for all HTTP APIs exposed by MachineryManagerEnterprise.

Every failed request shall return a consistent, predictable and machine-readable response.

---

# Error Philosophy

Errors represent business or technical failures in a standardized format.

Error responses shall be:

- Stable
- Predictable
- Machine readable
- Safe

Errors shall never expose internal implementation details.

---

# 2. Principles

Error responses shall be:

- Consistent
- Deterministic
- Machine-readable
- Human-readable
- Traceable

The same type of failure shall always produce the same response format.

---

# 3. Standard Error Structure

Every error response shall follow the structure below.

```json
{
  "errorCode": "BUS-014",
  "title": "Business Rule Violation",
  "message": "The selected engine is already installed.",
  "correlationId": "7f1f25f2-a8db-4e76-ae4d-4f8c16e4c08d",
  "details": []
}
```

---

# 4. Fields

| Field | Required | Description |
|--------|----------|-------------|
| errorCode | Yes | Stable application error code |
| title | Yes | Short error category |
| message | Yes | Human-readable description |
| correlationId | Yes | Request correlation identifier |
| details | No | Validation or additional information |

---

# Error Classification

| Category       | Prefix |
| -------------- | ------ |
| Validation     | VAL    |
| Business       | BUS    |
| Authentication | AUTH   |
| Resource       | RES    |
| Infrastructure | INF    |
| System         | SYS    |

---

# 5. Validation Error

Validation failures return:

HTTP Status

```
400 Bad Request
```

Example

```json
{
  "errorCode": "VAL-001",
  "title": "Validation Failed",
  "message": "Request validation failed.",
  "correlationId": "4af21f0c-37cb-45f3-bc90-0185d0fb5d74",
  "details": [
    {
      "field": "assetNumber",
      "message": "Asset Number is required."
    },
    {
      "field": "engineId",
      "message": "Engine does not exist."
    }
  ]
}
```

---

# 6. Business Rule Error

Business rule violations return:

```
409 Conflict
```

Example

```json
{
  "errorCode": "BUS-014",
  "title": "Business Rule Violation",
  "message": "The selected engine is already installed.",
  "correlationId": "93db2b73-1b0f-4528-9a87-79d1b8c26bb4"
}
```

---

# 7. Authentication Error

Authentication failures return:

```
401 Unauthorized
```

Example

```json
{
  "errorCode": "AUTH-001",
  "title": "Authentication Failed",
  "message": "Authentication is required.",
  "correlationId": "dbe6e9db-cdb7-42b0-a92b-6a7efbb78dd3"
}
```

---

# 8. Authorization Error

Authorization failures return:

```
403 Forbidden
```

Example

```json
{
  "errorCode": "AUTH-003",
  "title": "Access Denied",
  "message": "You do not have permission to perform this operation.",
  "correlationId": "c17d18d9-2930-4df2-a5e4-84fc6d9dc33d"
}
```

---

# 9. Resource Not Found

Missing resources return:

```
404 Not Found
```

Example

```json
{
  "errorCode": "RES-001",
  "title": "Resource Not Found",
  "message": "The requested asset does not exist.",
  "correlationId": "bd2fb65e-f948-4cb8-8f54-5cb1dc8ff6b4"
}
```

---

# 10. Infrastructure Error

Temporary infrastructure failures return:

```
503 Service Unavailable
```

Example

```json
{
  "errorCode": "INF-008",
  "title": "Service Unavailable",
  "message": "The requested service is temporarily unavailable.",
  "correlationId": "6dc8b5d8-7d8d-43c0-b74d-88d4fc50d68f"
}
```

---

# 11. Unexpected Error

Unexpected failures return:

```
500 Internal Server Error
```

Example

```json
{
  "errorCode": "SYS-001",
  "title": "Unexpected Error",
  "message": "An unexpected error occurred.",
  "correlationId": "5d773fc3-84bc-4214-a72c-a19a3b3d92ff"
}
```

Internal implementation details shall never be exposed.

---

# 12. Correlation Identifier

Every error response shall contain a Correlation Id.

The same identifier shall appear in:

- Application logs
- Audit records
- Distributed traces
- Background jobs (when applicable)

This enables complete request tracing.

---

# 13. Localization

The initial version of the API returns messages in a single language.

Future versions may localize:

- title
- message
- validation details

Error codes shall never change due to localization.

---

# 14. Backward Compatibility

Existing error fields shall remain stable.

Future versions may append new optional fields.

Existing fields shall never change semantic meaning.

---

# HTTP Mapping Table

| HTTP | Category                         |
| ---- | -------------------------------- |
| 400  | Validation                       |
| 401  | Authentication                   |
| 403  | Authorization                    |
| 404  | Resource                         |
| 409  | Business                         |
| 422  | Semantic Validation *(Reserved)* |
| 500  | System                           |
| 503  | Infrastructure                   |

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

- 00-ApiPrinciples.md
- 03-RequestResponseModel.md
- docs/05-development/07-ErrorHandling.md
- ADR-0005 — API Strategy

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | Initial | Initial Error Response specification |
| Property | Value |
|----------|-------|
| **Document ID** | API-003 |
| **Title** | Request / Response Model |
| **Version** | 4.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# 1. Purpose

This document defines the standard structure of every API request and response used by MachineryManagerEnterprise.

A consistent request/response model simplifies client development, improves interoperability and reduces implementation ambiguity.

---

# Contract Philosophy

API contracts represent public business interfaces.

Contracts shall remain stable, explicit, implementation independent and
backward compatible.

Internal domain entities shall never be exposed directly.

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

Collection metadata (page, pageSize, totalCount, totalPages) is defined in the Pagination document and shall accompany paginated responses.

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

# 17. Response Envelope Policy

Responses shall return the business resource directly unless an envelope is
required for:

- pagination
- metadata
- asynchronous operations
- standardized error reporting

Avoid unnecessary wrapping of simple resources.

---

| Endpoint Type  | Request Body | Response Body  |
| -------------- | ------------ | -------------- |
| GET Collection | No           | Collection DTO |
| GET Resource   | No           | Resource DTO   |
| POST Create    | Create DTO   | Resource DTO   |
| PUT Replace    | Replace DTO  | Resource DTO   |
| PATCH Update   | Update DTO   | Resource DTO   |
| DELETE         | No           | No Content     |

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
- 02-EndpointDesign.md
- 04-ErrorResponses.md
- 05-Versioning.md
- ADR-0005 — API Strategy

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Request / Response Model                      |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |