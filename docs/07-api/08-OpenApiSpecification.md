# OpenAPI Specification

**Document ID:** MME-API-008

**Repository Path:** `docs/07-api/08-OpenApiSpecification.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-ApiPrinciples.md
- 01-RestConventions.md
- 02-EndpointDesign.md
- 03-RequestResponseModel.md
- 04-ErrorResponses.md
- 07-AuthenticationAuthorization.md

---

# 1. Purpose

This document defines how the OpenAPI specification shall be generated, maintained and published for MachineryManagerEnterprise.

The OpenAPI document is the official machine-readable description of the public HTTP API.

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

Swagger UI should support authenticated testing.

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

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial OpenAPI Specification strategy |