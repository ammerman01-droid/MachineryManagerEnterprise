| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | API-008            |
| **Title**        | OpenAPI Specification |
| **Version**      | 4.2.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines how the OpenAPI specification shall be generated, maintained and published for MachineryManagerEnterprise.

The OpenAPI document is the official machine-readable description of the public HTTP API.

---

# OpenAPI Philosophy

The OpenAPI document is the authoritative machine-readable description of the public HTTP API.

Documentation shall always be generated from source code.

Generated documentation shall accurately represent the deployed API.

---

# Documentation Generation Pipeline

```text
Source Code

↓

XML Comments

↓

OpenAPI Generator

↓

openapi.json

↓

Scalar (primary interactive documentation)

↓

Generated SDK (NSwag)
```

Swagger UI remains available only for backward compatibility with
existing tooling, per ADR-0035 (API Documentation and Client Generation
Architecture); it is not the primary documentation UI.

---

# 2. Objectives

The OpenAPI specification shall provide:

- Accurate API documentation
- Client SDK generation
- API discoverability
- Contract validation
- Long-term maintainability

---

# 3. Specification Version

The project shall generate an OpenAPI 3.x specification.

Example

```
OpenAPI 3.1
```

The exact version depends on framework capabilities.

---

# 4. Documentation Source

The OpenAPI document shall be generated automatically from source code.

Manual editing of generated documents is prohibited.

Source code remains the single source of truth.

---

# 5. API Groups

Each API version shall expose an independent specification.

Example

```
/swagger/v1/swagger.json

/swagger/v2/swagger.json
```

Each version documents only its own endpoints.

---

# 6. Required Endpoint Metadata

Every endpoint shall define:

- Summary
- Description
- Tags
- Request Model
- Response Model
- Status Codes
- Authentication Requirements

Undocumented endpoints are not acceptable.

---

# 7. Schema Generation

Schemas shall be generated from public DTOs.

The following shall never appear as public schemas:

- Domain Entities
- EF Core Entities
- Internal Models
- Infrastructure Types

---

# 8. Example Payloads

Important endpoints should provide example requests and responses.

Example requests improve consumer understanding.

Example responses should represent realistic business data.

---

# 9. Error Documentation

Every endpoint shall document its possible error responses.

Typical documented responses include:

```
400 Bad Request

401 Unauthorized

403 Forbidden

404 Not Found

409 Conflict

500 Internal Server Error
```

---

# 10. Authentication Documentation

Protected endpoints shall declare their authentication scheme.

Scalar (and Swagger UI, where retained for compatibility) should support authenticated testing.

Authentication requirements shall be visible before execution.

---

# 11. Tags

Endpoints shall be grouped using business-oriented tags.

Examples

```
Assets

Engines

Components

Maintenance

Documents

Forecasts

Administration
```

Technical tags shall be avoided.

---

# 12. Deprecation

Deprecated endpoints shall be marked as deprecated within the OpenAPI document.

Documentation shall indicate the preferred replacement endpoint.

---

# 13. Client Generation

The OpenAPI document shall support automatic client generation.

Generated clients should remain compatible with the published contract.

Manual modification of generated clients is discouraged.

---

# 14. Documentation Availability

Interactive documentation shall be available in development environments.

Production availability depends on deployment policy.

Public exposure shall be explicitly controlled.

---

# 15. Validation

The generated OpenAPI document shall remain valid according to the OpenAPI specification.

Validation failures shall fail the build pipeline.

---

# 16. Future Enhancements

Future versions may include:

- Markdown examples
- Multiple languages
- External documentation links
- SDK download pages
- Interactive tutorials

---

# 17. Documentation Quality Rules

Every endpoint shall document:

- Summary
- Description
- Parameters
- Request Body
- Response Model
- Error Responses
- Authentication
- Example Payloads

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
- 03-RequestResponseModel.md
- 04-ErrorResponses.md
- 06-Versioning.md
- ADR-0035 — API Documentation and Client Generation Architecture

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial OpenAPI Specification strategy                |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Corrected reference from non-existent "ADR-0005 — API Strategy" to the actual governing ADR-0035 (API Documentation and Client Generation Architecture) |
| 4.2.0   | 2026-08-08 | Solution Architect | Fixed duplicate "OpenAPI Philosophy" heading (second occurrence was actually the generation pipeline); updated the pipeline and authentication-testing text to name Scalar as the primary documentation UI per ADR-0035, with Swagger UI retained only for compatibility |