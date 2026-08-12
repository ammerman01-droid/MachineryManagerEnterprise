| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | API-004            |
| **Title**        | Error Responses    |
| **Version**      | 4.2.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

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
- ADR-0035 — API Documentation and Client Generation Architecture

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Request / Response Model                      |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.2.0 | 2026-08-02 | Removed ~315 lines of accidentally duplicated content from 03-RequestResponseModel.md (including its own header, "Document ID: API-003", and full body) that had been appended after this document's own Change History; this file now contains only its own content |